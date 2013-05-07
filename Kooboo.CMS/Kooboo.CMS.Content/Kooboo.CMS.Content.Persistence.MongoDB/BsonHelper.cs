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
using MongoDB.Bson;

namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    public static class BsonHelper
    {
        public static BsonValue Create(object value)
        {
            if (value == null)
            {
                return BsonNull.Value;
            }
            //Mongodb does not support decimal
            if (value is decimal)
            {
                value = (double)((decimal)value);
            }            
            return BsonValue.Create(value);
        }
        public static object GetValue(BsonValue bsonValue)
        {
            if (bsonValue is BsonDateTime)
            {
                return DateTime.SpecifyKind(bsonValue.AsDateTime, DateTimeKind.Utc);
            }

            object value = bsonValue.RawValue;

            if (value is double)
            {
                value = (decimal)(double)value;
            }

            return value;
        }
    }
}
