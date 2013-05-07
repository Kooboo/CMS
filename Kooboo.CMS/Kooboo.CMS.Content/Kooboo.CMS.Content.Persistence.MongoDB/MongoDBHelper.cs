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
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using QueryBuilder = MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using MongoDB.Driver;
namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    public static class MongoDBHelper
    {
        public static IMongoQuery EQIgnoreCase(string fieldName, object value)
        {
            value = ModelExtensions.ToCaseInsensitiveValue(value);

            return QueryBuilder.Query.EQ(ModelExtensions.GetCaseInsensitiveFieldName(fieldName), BsonHelper.Create(value));

        }
        public static IMongoQuery NEIgnoreCase(string fieldName, object value)
        {
            value = ModelExtensions.ToCaseInsensitiveValue(value);

            return QueryBuilder.Query.NE(ModelExtensions.GetCaseInsensitiveFieldName(fieldName), BsonHelper.Create(value));

        }
    }
}
