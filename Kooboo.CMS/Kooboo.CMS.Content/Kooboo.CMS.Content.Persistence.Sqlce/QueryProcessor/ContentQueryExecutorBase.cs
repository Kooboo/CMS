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
using System.Data.SqlServerCe;
using Kooboo.Common;

namespace Kooboo.CMS.Content.Persistence.Sqlce.QueryProcessor
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
            SQLCeVisitor<T> visitor = new SQLCeVisitor<T>();
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
        private T First(SQLCeVisitor<T> visitor)
        {
            var content = FirstOrDefault(visitor);
            if (content == null)
            {
                throw new InvalidOperationException(SR.GetString("NoElements"));
            }
            return content;
        }
        private T Last(SQLCeVisitor<T> visitor)
        {
            var content = LastOrDefault(visitor);
            if (content == null)
            {
                throw new InvalidOperationException(SR.GetString("NoElements"));
            }
            return content;
        }
        private T FirstOrDefault(SQLCeVisitor<T> visitor)
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
                sql = string.Format("SELECT * FROM ({0})T ORDER BY {1} OFFSET {2} ROWS FETCH NEXT 1 ROWS ONLY",
                sql, orderBy, visitor.Skip);
            }
            else
            {
                sql = string.Format("SELECT TOP 1 * FROM ({0})T ORDER BY {1}", sql, orderBy);
            }
            var command = BuildCommand(sql, parameters);

            var connectionString = ContentQuery.Repository.GetConnectionString();
            T content = null;
            SqlCeConnection connection;
            using (var dataReader = SQLCeHelper.ExecuteReader(connectionString, command, out connection))
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
        private T LastOrDefault(SQLCeVisitor<T> visitor)
        {
            IEnumerable<Parameter> parameters;
            var sql = BuildQuerySQL(visitor, out parameters);

            var connectionString = ContentQuery.Repository.GetConnectionString();
            string orderBy = "Id ASC";
            if (visitor.OrderClauses != null && visitor.OrderClauses.Count > 0)
            {
                orderBy = ToReverseOrderString(visitor);
            }
            if (visitor.Skip != 0)
            {
                sql = string.Format("SELECT * FROM ({0})T ORDER BY {1}  OFFSET {2} ROWS FETCH NEXT 1 ROWS ONLY",
                sql, orderBy, visitor.Skip);
            }
            else
            {
                sql = string.Format("SELECT TOP 1 * FROM ({0})T ORDER BY {1}", sql, orderBy);
            }

            var command = BuildCommand(sql, parameters);

            T content = null;
            SqlCeConnection connection;
            using (var dataReader = SQLCeHelper.ExecuteReader(connectionString, command, out connection))
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

        private IEnumerable<T> List(SQLCeVisitor<T> visitor)
        {
            var command = BuildCommand(visitor);
            var connectionString = ContentQuery.Repository.GetConnectionString();
            List<T> list = new List<T>();
            SqlCeConnection connection;
            using (var dataReader = SQLCeHelper.ExecuteReader(connectionString, command, out connection))
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

        private int Count(SQLCeVisitor<T> visitor)
        {
            IEnumerable<Parameter> parameters;
            var sql = BuildQuerySQL(visitor, out parameters);

            var connectionString = ContentQuery.Repository.GetConnectionString();

            sql = string.Format("SELECT COUNT(Id) FROM ({0})T", sql);

            var command = BuildCommand(sql, parameters);

            return (int)SQLCeHelper.ExecuteScalar(connectionString, command);
        }

        protected static string ToOrderString(SQLCeVisitor<T> visitor)
        {
            return string.Join(",", visitor.OrderClauses.Distinct(new OrderClauseComparer()).Select(it => "[" + it.FieldName + "] " + (it.Descending ? "DESC" : "ASC")).ToArray());
        }
        protected static string ToReverseOrderString(SQLCeVisitor<T> visitor)
        {
            return string.Join(",", visitor.OrderClauses.Distinct(new OrderClauseComparer()).Select(it => "[" + it.FieldName + "] " + (it.Descending ? "ASC" : "DESC")).ToArray());
        }
        #endregion


        protected virtual SqlCeCommand BuildCommand(SQLCeVisitor<T> visitor)
        {
            string selectClause = "*";
            if (visitor.SelectFields != null && visitor.SelectFields.Length > 0)
            {
                selectClause = string.Join(",", visitor.SelectFields);
            }

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
            string sql = string.Format("{0} ORDER BY {1}  OFFSET {2} ROWS",
                BuildQuerySQL(visitor, out parameters), orderBy, visitor.Skip);

            if (visitor.Take != 0)
            {
                sql = sql + string.Format(" FETCH NEXT {0} ROWS ONLY", visitor.Take);
            }

            var command = new SqlCeCommand();

            command.CommandText = sql;

            foreach (var item in parameters)
            {
                command.Parameters.Add(new SqlCeParameter(item.Name, item.Value));
            }

            return BuildCommand(sql, parameters);
        }
        protected virtual SqlCeCommand BuildCommand(string commentText, IEnumerable<Parameter> parameters)
        {
            var command = new SqlCeCommand();

            command.CommandText = commentText;

            foreach (var item in parameters)
            {
                command.Parameters.Add(new SqlCeParameter(item.Name, item.Value));
            }

            return command;
        }
        public abstract string BuildQuerySQL(SQLCeVisitor<T> visitor, out IEnumerable<Parameter> parameters);
    }
}
