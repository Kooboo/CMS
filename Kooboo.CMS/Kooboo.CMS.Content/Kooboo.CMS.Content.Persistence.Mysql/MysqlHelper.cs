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
using MySql.Data.MySqlClient;
using System.Data;
using Kooboo.CMS.Content.Models;
using System.Data.Common;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public static class MysqlHelper
    {
        public static bool TestConnection(string connectionString)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                return conn.State == ConnectionState.Open;
            }
        }
        public static void ExecuteNonQuery(Repository repository, params MySqlCommand[] commands)
        {
            var connectionString = repository.GetConnectionString();
            using (var conn = new MySqlConnection(connectionString))
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
                        throw new KoobooException(e.Message + "SQL:" + command.CommandText, e);
                    }

                }
                conn.Close();
            }
        }
        public static void BatchExecuteNonQuery(Repository repository, params MySqlCommand[] commands)
        {
            var connectionString = repository.GetConnectionString();
            using (var conn = new MySqlConnection(connectionString))
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
                            throw new KoobooException(e.Message + "SQL:" + command.CommandText, e);
                        }
                    }

                    trans.Commit();
                }
                conn.Close();
            }
        }
        public static object ExecuteScalar(Repository repository, MySqlCommand command)
        {
            LogCommand(repository, command);

            var connectionString = repository.GetConnectionString();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                ResetParameterNullValue(command);
                command.Connection = conn;
                var value = command.ExecuteScalar();

                conn.Close();

                return value;
            }
        }
        public static IDataReader ExecuteReader(Repository repository, MySqlCommand command, out MySqlConnection connection)
        {
            LogCommand(repository, command);

            var connectionString = repository.GetConnectionString();
            connection = new MySqlConnection(connectionString);

            connection.Open();

            ResetParameterNullValue(command);
            command.Connection = connection;
            return command.ExecuteReader();

        }

        public static void ResetParameterNullValue(MySqlCommand command)
        {
            foreach (MySqlParameter item in command.Parameters)
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
                if (dataReader.GetFieldType(i) == typeof(ulong))
                {
                    value = dataReader.GetValue(i);
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    else
                    {
                        value = Convert.ToBoolean(value);
                    }
                }
                content[fieldName] = value;
            }
            return content;
        }


        public static void LogCommand(Repository repository, DbCommand command)
        {
            if (MysqlSettings.Instance.EnableLogging)
            {
                EngineContext.Current.Resolve<IDBCommandLogger>().Log(repository, ProviderFactory.ProviderName, command);
            }
        }
    }
}
