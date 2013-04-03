using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.Content.Models;
using Kooboo.Data;

namespace Kooboo.CMS.Content.Persistence.Sqlce.Tests
{
    /// <summary>
    /// Summary description for SchemaManagerTests
    /// </summary>
    [TestClass]
    public class SchemaManagerTests
    {
        Repository repository = new Repository("SchemaManagerTests");

        [TestMethod]
        public void TestAdd()
        {
            Schema schema = new Schema(repository, "news") { IsDummy = false };

            schema.AddColumn(new Column()
            {
                Name = "Title",
                DataType = DataType.String,
                Length = 100
            });
            
            //add
            SchemaManager.Add(schema);

            Schema newSchema = new Schema(repository, "news") { IsDummy = false };

            newSchema.AddColumn(new Column()
            {
                Name = "Title",
                DataType = DataType.String,
                Length = 100
            });

            newSchema.AddColumn(new Column()
            {
                Name = "Body",
                DataType = DataType.String,
                Length = 256
            });
            newSchema.AddColumn(new Column()
            {
                Name = "Comments",
                DataType = DataType.Int
            });
            //add column
            SchemaManager.Update(newSchema, schema);

            //remove column
            Schema lastSchema = new Schema(repository, "news") { IsDummy = false };

            lastSchema.AddColumn(new Column()
            {
                Name = "Title",
                DataType = DataType.String,
                Length = 100
            });

            lastSchema.AddColumn(new Column()
            {
                Name = "Body",
                DataType = DataType.String,
                Length = 256
            });

            //add column
            SchemaManager.Update(lastSchema, newSchema);

            SchemaManager.Delete(lastSchema);

        }
    }
}
