#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using System.Data.SqlClient;

namespace Kooboo.CMS.Content.Persistence.SqlServer.QueryProcessor
{
    public abstract class ContentQueryExecutorBase<T> : IQueryExecutor<T>
        where T : ContentBase, new()
    {
        public ContentQueryExecutorBase(ContentQuery<T> contentQuery)
        {
            this.ContentQuery = contentQuery;
        }
        public ContentQuery<T> ContentQuery { get; private set; }
        #region IQueryExecutor Members

        public virtual object Execute()
        {
            SQLServerVisitor<T> visitor = new SQLServerVisitor<T>();
            visitor.Visite(ContentQuery.Expression);


            switch (visitor.CallType)
            {
                case Kooboo.CMS.Content.Query.Expressions.CallType.Count:
                    return Count(visitor);
                case Kooboo.CMS.Content.Query.Expressions.CallType.First:
                    return First(visitor);
                case Kooboo.CMS.Content.Query.Expressions.CallType.LastOrDefault:
                    return LastOrDefault(visitor);
                case Kooboo.CMS.Content.Query.Expressions.CallType.FirstOrDefault:
                    return FirstOrDefault(visitor);
                case Kooboo.CMS.Content.Query.Expressions.CallType.Last:
                    return Last(visitor);
                case Kooboo.CMS.Content.Query.Expressions.CallType.Unspecified:
                default:
                    return List(visitor);
            }
        }
        private T First(SQLServerVisitor<T> visitor)
        {
            var content = FirstOrDefault(visitor);
            if (content == null)
            {
                throw new InvalidOperationException(SR.GetString("NoElements"));
            }
            return content;
        }
        private T Last(SQLServerVisitor<T> visitor)
        {
            var content = LastOrDefault(visitor);
            if (content == null)
            {
                throw new InvalidOperationException(SR.GetString("NoElements"));
            }
            return content;
        }
        private T FirstOrDefault(SQLServerVisitor<T> visitor)
        {
            IEnumerable<Parameter> parameters;
            var sql = BuildQuerySQL(visitor, out parameters);

            string orderBy = "Id ASC";
            if (visitor.OrderClauses != null && visitor.OrderClauses.Count > 0)
            {
                orderBy = ToOrderString(visitor);
            }
            if (visitor.Skip != 0)
            {
                sql = string.Format(@"SELECT * 
                    FROM (
                        SELECT *,ROW_NUMBER() OVER(ORDER BY {0}) AS RowIndex
                            FROM ({1})content
                         )paging
                    WHERE (RowIndex > {2})", orderBy, sql, visitor.Skip);
            }

            sql = string.Format("SELECT TOP 1 * FROM ({0})T ORDER BY {1}", sql, orderBy);
            var command = BuildCommand(sql, parameters);

            T content = null;
            SqlConnection connection;
            using (var dataReader = SQLServerHelper.ExecuteReader(ContentQuery.Repository, command, out connection))
            {
                try
                {
                    if (dataReader.Read())
                    {
                        content = new T();
                        dataReader.ToContent(content);
                    }

                }
                finally
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return content;
        }
        private T LastOrDefault(SQLServerVisitor<T> visitor)
        {
            IEnumerable<Parameter> parameters;
            var sql = BuildQuerySQL(visitor, out parameters);

            string orderBy = "Id ASC";
            if (visitor.OrderClauses != null && visitor.OrderClauses.Count > 0)
            {
                orderBy = ChangeOrderDirectionOrderString(visitor);
            }
            if (visitor.Skip != 0)
            {                
                sql = string.Format(@"SELECT * 
                    FROM (
                        SELECT *,ROW_NUMBER() OVER(ORDER BY {0}) AS RowIndex
                            FROM ({1})content
                         )paging
                    WHERE (RowIndex > {2})", orderBy, sql, visitor.Skip);
            }
            sql = string.Format("SELECT TOP 1 * FROM ({0})T ORDER BY {1}", sql, orderBy);

            var command = BuildCommand(sql, parameters);
            T content = null;
            SqlConnection connection;
            using (var dataReader = SQLServerHelper.ExecuteReader(ContentQuery.Repository, command, out connection))
            {
                try
                {
                    if (dataReader.Read())
                    {
                        content = new T();
                        dataReader.ToContent(content);
                    }
                }
                finally
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return content;
        }

        private IEnumerable<T> List(SQLServerVisitor<T> visitor)
        {
            var command = BuildCommand(visitor);
            SqlConnection connection;
            List<T> list = new List<T>();
            using (var dataReader = SQLServerHelper.ExecuteReader(ContentQuery.Repository, command, out connection))
            {
                try
                {
                    while (dataReader.Read())
                    {
                        T content = new T();
                        list.Add(dataReader.ToContent(content));
                    }
                }
                finally
                {
                    dataReader.Close();
                    connection.Close();
                }
            }
            return list;
        }

        private int Count(SQLServerVisitor<T> visitor)
        {
            IEnumerable<Parameter> parameters;
            var sql = BuildQuerySQL(visitor, out parameters);

            if (visitor.Skip != 0)
            {
                string orderBy = "Id DESC";
                if (visitor.OrderClauses != null && visitor.OrderClauses.Count() > 0)
                {
                    orderBy = ToOrderString(visitor);
                }

                sql = string.Format(@"SELECT * 
                    FROM (
                        SELECT *,ROW_NUMBER() OVER(ORDER BY {0}) AS RowIndex
                            FROM ({1})content
                         )paging
                    WHERE (RowIndex > {2})", orderBy, sql, visitor.Skip);
            }
            sql = string.Format("SELECT COUNT(Id) FROM ({0})T", sql);

            var command = BuildCommand(sql, parameters);

            return (int)SQLServerHelper.ExecuteScalar(ContentQuery.Repository, command);
        }

        protected static string ToOrderString(SQLServerVisitor<T> visitor)
        {
            return string.Join(",", visitor.OrderClauses.Distinct(new OrderClauseComparer()).Select(it => "[" + it.FieldName + "] " + (it.Descending ? "DESC" : "ASC")).ToArray());
        }
        protected static string ChangeOrderDirectionOrderString(SQLServerVisitor<T> visitor)
        {
            return string.Join(",", visitor.OrderClauses.Distinct(new OrderClauseComparer()).Select(it => "[" + it.FieldName + "] " + (it.Descending ? "ASC" : "DESC")).ToArray());
        }
        #endregion


        protected virtual SqlCommand BuildCommand(SQLServerVisitor<T> visitor)
        {
            string whereClause = "1=1";
            if (!string.IsNullOrEmpty(visitor.WhereClause))
            {
                whereClause = visitor.WhereClause;
            }
            string orderBy = "Id DESC";
            if (visitor.OrderClauses != null && visitor.OrderClauses.Count() > 0)
            {
                orderBy = ToOrderString(visitor);
            }
            IEnumerable<Parameter> parameters;
            string sql = BuildQuerySQL(visitor, out parameters);

            if (visitor.Take != 0)
            {
                sql = string.Format(@"
                SELECT * 
                    FROM (
                        SELECT *,ROW_NUMBER() OVER(ORDER BY {0}) AS RowIndex
                            FROM ({1})content
                         )paging
                    WHERE (RowIndex > {2}) AND (RowIndex <= {3}) "
                    , orderBy
                    , sql
                    , visitor.Skip
                    , visitor.Skip + visitor.Take);
            }
            else
            {
                sql = sql + " ORDER BY " + orderBy;
            }
            return BuildCommand(sql, parameters);
        }

        protected virtual SqlCommand BuildCommand(string commentText, IEnumerable<Parameter> parameters)
        {
            var command = new SqlCommand();

            command.CommandText = commentText;

            foreach (var item in parameters)
            {
                command.Parameters.Add(new SqlParameter(item.Name, item.Value));
            }

            return command;
        }
        public abstract string BuildQuerySQL(SQLServerVisitor<T> visitor, out IEnumerable<Parameter> parameters);
    }
}
