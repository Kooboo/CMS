using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using QueryBuilder = MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
namespace Kooboo.CMS.Content.Persistence.MongoDB
{
    public static class MongoDBHelper
    {
        public static QueryComplete EQIgnoreCase(string fieldName, object value)
        {
            if (value is string)
            {
                return QueryBuilder.Query.Matches(fieldName, new BsonRegularExpression("^" + Regex.Escape(value.ToString()) + "$", "i"));

            }
            else
            {
                return QueryBuilder.Query.EQ(fieldName, BsonHelper.Create(value));
            }
        }
        public static QueryComplete NEIgnoreCase(string fieldName, object value)
        {
            if (value is string)
            {
                return QueryBuilder.Query.Not(fieldName).Matches(new BsonRegularExpression("^" + Regex.Escape(value.ToString()) + "$", "i"));
            }
            {
                return QueryBuilder.Query.NE(fieldName, BsonHelper.Create(value));
            }
        }
    }
}
