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
using System.Data.SqlClient;
using System.Data;
using Kooboo.CMS.Content.Models;
using System.Data.Common;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common;

namespace Kooboo.CMS.Content.Persistence.SqlServer
{
    public static class SQLServerHelper
    {
        public static bool TestConnection(string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                return conn.State == ConnectionState.Open;
            }
        }
        public static void ExecuteNonQuery(Repository repository, params SqlCommand[] commands)
        {
            var connectionString = repository.GetConnectionString();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (var command in commands)
                {
                    LogCommand(repository, command);
                    try
                    {
                        ResetParameterNullValue(command);
                        command.Connection = conn;
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message + "SQL:" + command.CommandText, e);
                    }
                }

            }
        }
        public static void BatchExecuteNonQuery(Repository repository, params SqlCommand[] commands)
        {
            var connectionString = repository.GetConnectionString();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    foreach (var command in commands)
                    {
                        LogCommand(repository, command);
                        try
                        {
                            ResetParameterNullValue(command);
                            command.Transaction = trans;
                            command.Connection = conn;
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message + "SQL:" + command.CommandText, e);
                        }
                    }

                    trans.Commit();
                }

            }
        }
        public static object ExecuteScalar(Repository repository, SqlCommand command)
        {
            LogCommand(repository, command);
            var connectionString = repository.GetConnectionString();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                ResetParameterNullValue(command);
                command.Connection = conn;
                return command.ExecuteScalar();
            }
        }
        public static IDataReader ExecuteReader(Repository repository, SqlCommand command, out SqlConnection connection)
        {
            LogCommand(repository, command);
            var connectionString = repository.GetConnectionString();

            connection = new SqlConnection(connectionString);

            connection.Open();
            ResetParameterNullValue(command);
            command.Connection = connection;

            return command.ExecuteReader();

        }

        public static void ResetParameterNullValue(SqlCommand command)
        {
            foreach (SqlParameter item in command.Parameters)
            {
                if (item.Value == null)
                {
                    item.Value = DBNull.Value;
                }
            }
        }

        public static T ToContent<T>(this IDataReader dataReader, T content)
            where T : ContentBase
        {
            ITimeZoneHelper timeZoneHelper = EngineContext.Current.Resolve<ITimeZoneHelper>();
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                var fieldName = dataReader.GetName(i);
                var value = dataReader.GetValue(i);
                if (value is DateTime)
                {
                    var dt = new DateTime(((DateTime)value).Ticks, DateTimeKind.Utc);
                    value = timeZoneHelper.ConvertToLocalTime(dt, TimeZoneInfo.Utc);
                }
                if (value == DBNull.Value)
                {
                    value = null;
                }
                content[fieldName] = value;
            }
            return content;
        }
        public static void LogCommand(Repository repository, DbCommand command)
        {
            if (SqlServerSettings.Instance.EnableLogging)
            {
                EngineContext.Current.Resolve<IDBCommandLogger>().Log(repository, ProviderFactory.ProviderName, command);
            }
        }
    }
}
