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
using MySql.Data.MySqlClient;
using Kooboo.Common;

namespace Kooboo.CMS.Content.Persistence.Mysql.QueryProcessor
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
            MysqlVisitor<T> visitor = new MysqlVisitor<T>();
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
        private T First(MysqlVisitor<T> visitor)
        {
            var content = FirstOrDefault(visitor);
            if (content == null)
            {
                throw new InvalidOperationException(SR.GetString("NoElements"));
            }
            return content;
        }
        private T Last(MysqlVisitor<T> visitor)
        {
            var content = LastOrDefault(visitor);
            if (content == null)
            {
                throw new InvalidOperationException(SR.GetString("NoElements"));
            }
            return content;
        }
        private T FirstOrDefault(MysqlVisitor<T> visitor)
        {
            IEnumerable<Parameter> parameters;
            var sql = BuildQuerySQL(visitor, out parameters);

            string orderBy = "Id ASC";
            if (visitor.OrderClauses != null && visitor.OrderClauses.Count > 0)
            {
                orderBy = ToOrderString(visitor);
            }

            sql = string.Format("SELECT * FROM ({0})T ORDER BY {1} limit {2},1", sql, orderBy, visitor.Skip);
            var command = BuildCommand(sql, parameters);

            T content = default(T);
            MySqlConnection connection;
            using (var dataReader = MysqlHelper.ExecuteReader(ContentQuery.Repository, command, out connection))
            {
                if (dataReader.Read())
                {
                    content = new T();
                    dataReader.ToContent(content);
                }
                dataReader.Close();
                connection.Close();
            }
            return content;
        }
        private T LastOrDefault(MysqlVisitor<T> visitor)
        {
            IEnumerable<Parameter> parameters;
            var sql = BuildQuerySQL(visitor, out parameters);

            string orderBy = "Id ASC";
            if (visitor.OrderClauses != null && visitor.OrderClauses.Count > 0)
            {
                orderBy = ToReverseOrderString(visitor);
            }

            sql = string.Format("SELECT * FROM ({0})T ORDER BY {1} limit {2},1", sql, orderBy, visitor.Skip);

            var command = BuildCommand(sql, parameters);
            MySqlConnection connection;
            using (var dataReader = MysqlHelper.ExecuteReader(ContentQuery.Repository, command, out connection))
            {
                while (dataReader.Read())
                {
                    T content = new T();
                    dataReader.ToContent(content);
                    return content;
                }
                dataReader.Close();
                connection.Close();
            }
            return null;
        }

        private IEnumerable<T> List(MysqlVisitor<T> visitor)
        {
            var command = BuildCommand(visitor);

            List<T> list = new List<T>();

            MySqlConnection connection;
            using (var dataReader = MysqlHelper.ExecuteReader(ContentQuery.Repository, command, out connection))
            {
                while (dataReader.Read())
                {
                    T content = new T();
                    list.Add(dataReader.ToContent(content));
                }
                dataReader.Close();
                connection.Close();
            }
            return list;
        }

        private int Count(MysqlVisitor<T> visitor)
        {
            IEnumerable<Parameter> parameters;
            var sql = BuildQuerySQL(visitor, out parameters);

            sql = string.Format("SELECT COUNT(Id) FROM ({0})T", sql);

            var command = BuildCommand(sql, parameters);

            return (int)(long)MysqlHelper.ExecuteScalar(ContentQuery.Repository, command);
        }

        protected static string ToOrderString(MysqlVisitor<T> visitor)
        {
            return string.Join(",", visitor.OrderClauses.Distinct(new OrderClauseComparer()).Select(it => "`" + it.FieldName + "` " + (it.Descending ? "DESC" : "ASC")).ToArray());
        }
        protected static string ToReverseOrderString(MysqlVisitor<T> visitor)
        {
            return string.Join(",", visitor.OrderClauses.Distinct(new OrderClauseComparer()).Select(it => "`" + it.FieldName + "` " + (it.Descending ? "ASC" : "DESC")).ToArray());
        }
        #endregion


        protected virtual MySqlCommand BuildCommand(MysqlVisitor<T> visitor)
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


            sql = sql + " ORDER BY " + orderBy;

            if (visitor.Take != 0)
            {
                sql = string.Format(@"
                        SELECT *
                            FROM ({0})content                         
                        limit {1},{2} "
                    , sql
                    , visitor.Skip
                    , visitor.Take);
            }
            return BuildCommand(sql, parameters);
        }

        protected virtual MySqlCommand BuildCommand(string commentText, IEnumerable<Parameter> parameters)
        {
            var command = new MySqlCommand();

            command.CommandText = commentText;

            foreach (var item in parameters)
            {
                command.Parameters.Add(new MySqlParameter(item.Name, item.Value));
            }

            return command;
        }
        public abstract string BuildQuerySQL(MysqlVisitor<T> visitor, out IEnumerable<Parameter> parameters);
    }
}
