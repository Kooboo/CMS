#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Data
{
    /// <summary>
    /// Use in cms.
    /// </summary>
    public enum DataType
    {
        String,
        Int,
        Decimal,
        DateTime,
        Bool
    }
    /// <summary>
    /// 
    /// </summary>
    public static class DataTypeHelper
    {
        #region Methods
        /// <summary>
        /// Defaults the value.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        public static object DefaultValue(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.String:
                    return "";
                case DataType.Int:
                    return default(int);
                case DataType.Decimal:
                    return default(decimal);
                case DataType.DateTime:
                    return DateTime.UtcNow;
                case DataType.Bool:
                    return default(bool);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Parses the value.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <param name="value">The value.</param>
        /// <param name="throwWhenInvalid">if set to <c>true</c> [throw when invalid].</param>
        /// <returns></returns>
        /// <exception cref="Kooboo.KoobooException"></exception>
        public static object ParseValue(DataType dataType, string value, bool throwWhenInvalid)
        {
            switch (dataType)
            {
                case DataType.String:
                    return value;
                case DataType.Int:
                    int intValue;
                    if (int.TryParse(value, out intValue))
                    {
                        return intValue;
                    }
                    else
                    {
                        if (throwWhenInvalid)
                        {
                            throw new KoobooException("The value is invalid.");
                        }
                        return default(int);
                    }
                case DataType.Decimal:
                    decimal decValue;
                    if (decimal.TryParse(value, out decValue))
                    {
                        return decValue;
                    }
                    else
                    {
                        if (throwWhenInvalid)
                        {
                            throw new KoobooException("The value is invalid.");
                        }
                        return default(decimal);
                    }
                case DataType.DateTime:
                    DateTime dateTime;
                    if (DateTime.TryParse(value, out dateTime))
                    {
                        if (dateTime.Kind != DateTimeKind.Utc)
                        {
                            dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Local).ToUniversalTime();
                        }
                        return dateTime;

                    }
                    else
                    {
                        if (throwWhenInvalid)
                        {
                            throw new KoobooException("The value is invalid.");
                        }
                        return default(DateTime);
                    }
                case DataType.Bool:
                    if (!string.IsNullOrEmpty(value))
                    {
                        bool boolValue;
                        if (bool.TryParse(value, out boolValue))
                        {
                            return boolValue;
                        }
                        else
                        {
                            if (throwWhenInvalid)
                            {
                                throw new KoobooException("The value is invalid.");
                            }
                            return default(bool);
                        }
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return null;
            }

        }
        #endregion
    }
}
