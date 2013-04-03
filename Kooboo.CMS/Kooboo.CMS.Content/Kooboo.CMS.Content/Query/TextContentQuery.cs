using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Content.Query
{
    public static class TextContentQueryExtensions
    {
        public static IContentQuery<TextContent> CreateQuery(this TextFolder textFolder)
        {
            return new TextContentQuery(textFolder.Repository, new Schema(textFolder.Repository, textFolder.AsActual().SchemaName)
                , textFolder);
        }
        public static IContentQuery<TextContent> CreateQuery(this Schema schema)
        {
            return new TextContentQuery(schema.Repository, schema);
        }

        public static IContentQuery<TextContent> Categories(this IContentQuery<TextContent> source, string categoryFolder)
        {
            return Categories(source, new TextFolder(source.Repository, categoryFolder));
        }
        public static IContentQuery<TextContent> Categories(this IContentQuery<TextContent> source, TextFolder categoryFolder)
        {
            var categoriesQuery = new CategoriesQuery(source.Repository, categoryFolder, source);
            return categoriesQuery;
        }
        public static IContentQuery<TextContent> Categories(this TextContent content, TextFolder categoryFolder)
        {
            var repository = Providers.RepositoryProvider.Get(new Repository(content.Repository));
            var folder = new TextFolder(repository, content.FolderName) { SchemaName = content.SchemaName };
            return folder.CreateQuery().WhereEquals("UUID", content.UUID).Categories(categoryFolder);
        }
        public static IContentQuery<TextContent> Categories(this TextContent content, string categoryFolder)
        {
            var repository = content.GetRepository();
            return Categories(content, new TextFolder(repository, categoryFolder));
        }

        public static IContentQuery<TextContent> WhereCategory(this IContentQuery<TextContent> source, IContentQuery<TextContent> categoryQuery)
        {
            var expression = new WhereCategoryExpression(source.Expression, categoryQuery);
            return source.Create(expression);
        }


        public static IContentQuery<TextContent> Children(this IContentQuery<TextContent> source, string embeddedFolder)
        {
            return Children(source, new TextFolder(source.Repository, embeddedFolder));
        }
        public static IContentQuery<TextContent> Children(this IContentQuery<TextContent> source, TextFolder embeddedFolder)
        {
            return new ChildrenQuery(source.Repository, embeddedFolder.GetSchema(), embeddedFolder, source);
        }
        public static IContentQuery<TextContent> Children(this IContentQuery<TextContent> source, Schema childSchema)
        {
            return new ChildrenQuery(source.Repository, childSchema, null, source);
        }
        public static IContentQuery<TextContent> Children(this TextContent parent, string embeddedFolder)
        {
            var parentSchema = new Schema(new Repository(parent.Repository), parent.SchemaName);
            return parentSchema.CreateQuery().WhereEquals("UUID", parent.UUID).Children(embeddedFolder);
        }
        public static IContentQuery<TextContent> Children(this TextContent parent, Schema childSchema)
        {
            var parentSchema = new Schema(new Repository(parent.Repository), parent.SchemaName);
            return parentSchema.CreateQuery().WhereEquals("UUID", parent.UUID).Children(childSchema);
        }
        public static IContentQuery<TextContent> Children(this TextContent parent, TextFolder embeddedFolder)
        {
            var parentSchema = new Schema(new Repository(parent.Repository), parent.SchemaName);
            return parentSchema.CreateQuery().WhereEquals("UUID", parent.UUID).Children(embeddedFolder);
        }

        public static IContentQuery<TextContent> Parent(this IContentQuery<TextContent> source, string parentFolder)
        {
            return Parent(source, new TextFolder(source.Repository, parentFolder));
        }
        public static IContentQuery<TextContent> Parent(this IContentQuery<TextContent> source, TextFolder parentFolder)
        {
            return new ParentQuery(source.Repository, parentFolder.GetSchema(), parentFolder, source);
        }
        public static IContentQuery<TextContent> Parent(this IContentQuery<TextContent> source, Schema parentSchema)
        {
            return new ParentQuery(source.Repository, parentSchema, null, source);
        }
        public static IContentQuery<TextContent> Parent(this TextContent childContent, string parentFolder)
        {
            var childSchema = new Schema(new Repository(childContent.Repository), childContent.SchemaName);
            return childSchema.CreateQuery().WhereEquals("UUID", childContent.UUID).Parent(parentFolder);
        }
        public static IContentQuery<TextContent> Parent(this TextContent childContent, TextFolder parentFolder)
        {
            var childSchema = new Schema(new Repository(childContent.Repository), childContent.SchemaName);
            return childSchema.CreateQuery().WhereEquals("UUID", childContent.UUID).Parent(parentFolder);
        }
        public static IContentQuery<TextContent> Parent(this TextContent childContent, Schema parentSchema)
        {
            var childSchema = new Schema(new Repository(childContent.Repository), childContent.SchemaName);
            return childSchema.CreateQuery().WhereEquals("UUID", childContent.UUID).Parent(parentSchema);
        }
        public static TextContent Next(this TextContent textContent)
        {
            var textFolder = textContent.GetFolder().AsActual();
            var orderField = "UtcCreationDate";
            var direction = OrderDirection.Descending;
            if (textFolder.OrderSetting != null)
            {
                orderField = textFolder.OrderSetting.FieldName;
                direction = textFolder.OrderSetting.Direction;
            }
            return Next(textContent, orderField, direction);
        }
        public static TextContent Next(this TextContent textContent, string orderField, OrderDirection direction)
        {
            var textFolder = textContent.GetFolder().AsActual();
            if (direction == OrderDirection.Ascending)
            {
                return textFolder.CreateQuery()
                    .WhereEquals("Published", true)
                    .WhereEquals("ParentUUID", textContent.ParentUUID)
                    .WhereEquals("ParentFolder", textContent.ParentFolder)
                    .WhereGreaterThan(orderField, textContent[orderField]).OrderBy(orderField).FirstOrDefault();
            }
            else
            {
                return textFolder.CreateQuery()
                    .WhereEquals("Published", true)
                    .WhereEquals("ParentUUID", textContent.ParentUUID)
                    .WhereEquals("ParentFolder", textContent.ParentFolder)
                    .WhereLessThan(orderField, textContent[orderField]).OrderByDescending(orderField).FirstOrDefault();
            }
        }

        public static TextContent Previous(this TextContent textContent)
        {
            var textFolder = textContent.GetFolder().AsActual();
            var orderField = "UtcCreationDate";
            var direction = OrderDirection.Descending;
            if (textFolder.OrderSetting != null)
            {
                orderField = textFolder.OrderSetting.FieldName;
                direction = textFolder.OrderSetting.Direction;
            }
            return Previous(textContent, orderField, direction);
        }
        public static TextContent Previous(this TextContent textContent, string orderField, OrderDirection direction)
        {
            var textFolder = textContent.GetFolder().AsActual();
            if (direction == OrderDirection.Ascending)
            {
                return textFolder.CreateQuery()
                    .WhereEquals("ParentUUID", textContent.ParentUUID)
                    .WhereEquals("ParentFolder", textContent.ParentFolder)
                    .WhereLessThan(orderField, textContent[orderField]).OrderByDescending(orderField).FirstOrDefault();
            }
            else
            {
                return textFolder.CreateQuery()
                    .WhereEquals("ParentUUID", textContent.ParentUUID)
                    .WhereEquals("ParentFolder", textContent.ParentFolder)
                    .WhereGreaterThan(orderField, textContent[orderField]).OrderBy(orderField).FirstOrDefault();
            }
        }

        public static IContentQuery<TextContent> DefaultOrder(this IContentQuery<TextContent> contentQuery)
        {
            if (contentQuery is TextContentQueryBase)
            {
                var textFolder = ((TextContentQueryBase)contentQuery).Folder.AsActual();
                var orderField = "UtcCreationDate";
                var direction = OrderDirection.Descending;

                if (textFolder.OrderSetting != null && !string.IsNullOrEmpty(textFolder.OrderSetting.FieldName))
                {
                    orderField = textFolder.OrderSetting.FieldName;
                    direction = textFolder.OrderSetting.Direction;
                }
                if (direction == OrderDirection.Ascending)
                {
                    return contentQuery.OrderBy(orderField);
                }
                else
                {
                    return contentQuery.OrderByDescending(orderField);
                }
            }
            return contentQuery;
        }
    }
    public abstract class TextContentQueryBase : ContentQuery<TextContent>
    {
        public TextContentQueryBase(Repository repository)
            : base(repository, null)
        {

        }
        public TextContentQueryBase(Repository repository, IExpression expression)
            : base(repository, expression)
        {
        }

        public abstract TextFolder Folder { get; }
    }
    public class TextContentQuery : TextContentQueryBase
    {
        public TextContentQuery(Repository repository, Schema schema)
            : this(repository, schema, null)
        {

        }
        public TextContentQuery(Repository repository, Schema schema, TextFolder textFolder)
            : this(repository, schema, textFolder, null)
        {
        }
        public TextContentQuery(Repository repository, Schema schema, TextFolder textFolder, IExpression expression)
            : base(repository, expression)
        {
            this.Schema = schema;

            this.textFolder = textFolder;
        }
        public Schema Schema { get; private set; }

        private TextFolder textFolder;
        public override TextFolder Folder { get { return textFolder; } }

        public override IContentQuery<TextContent> Create(IExpression expression)
        {
            return new TextContentQuery(this.Repository, this.Schema, this.Folder, expression);
        }
    }

    public class CategoriesQuery : TextContentQueryBase
    {
        public CategoriesQuery(Repository repository, TextFolder categoryFolder, IContentQuery<TextContent> innerQuery)
            : this(repository, categoryFolder, innerQuery, null)
        {
        }
        public CategoriesQuery(Repository repository, TextFolder categoryFolder, IContentQuery<TextContent> innerQuery, IExpression expression)
            : base(repository, expression)
        {
            this.CategoryFolder = categoryFolder;
            this.InnerQuery = innerQuery;
        }
        public TextFolder CategoryFolder { get; set; }

        public IContentQuery<TextContent> InnerQuery { get; set; }

        public override IContentQuery<TextContent> Create(IExpression expression)
        {
            return new CategoriesQuery(this.Repository, this.CategoryFolder, this.InnerQuery, expression);
        }

        public override TextFolder Folder
        {
            get { return CategoryFolder; }
        }
    }

    //public class CategorizablesQuery : ContentQuery<TextContent>
    //{
    //    public CategorizablesQuery(Repository repository, TextFolder categorizableFolder, IContentQuery<TextContent> categoryQuery)
    //        : this(repository, categorizableFolder, categoryQuery, null)
    //    {

    //    }
    //    public CategorizablesQuery(Repository repository, TextFolder categorizableFolder, IContentQuery<TextContent> categoryQuery, IExpression expression)
    //        : base(repository, expression)
    //    {
    //        this.CategorizableFolder = categorizableFolder;
    //        this.CategoryQuery = categoryQuery;
    //    }
    //    public TextFolder CategorizableFolder { get; set; }

    //    public IContentQuery<TextContent> CategoryQuery { get; set; }

    //    public override IContentQuery<TextContent> Create(IExpression expression)
    //    {
    //        return new CategorizablesQuery(this.Repository, this.CategorizableFolder, this.CategoryQuery, expression);
    //    }
    //}

    public class ChildrenQuery : TextContentQueryBase
    {
        public ChildrenQuery(Repository repository, Schema childSchema, TextFolder embeddedFolder, IContentQuery<TextContent> parentQuery)
            : this(repository, childSchema, embeddedFolder, parentQuery, null)
        { }
        public ChildrenQuery(Repository repository, Schema childSchema, TextFolder embeddedFolder, IContentQuery<TextContent> parentQuery, IExpression expression)
            : base(repository, expression)
        {
            this.ChildSchema = childSchema;
            this.ParentQuery = parentQuery;
            this.EmbeddedFolder = embeddedFolder;
        }

        public Schema ChildSchema { get; set; }
        public TextFolder EmbeddedFolder { get; set; }

        public IContentQuery<TextContent> ParentQuery { get; set; }

        public override IContentQuery<TextContent> Create(IExpression expression)
        {
            return new ChildrenQuery(this.Repository, this.ChildSchema, EmbeddedFolder, this.ParentQuery, expression);
        }

        public override TextFolder Folder
        {
            get { return EmbeddedFolder; }
        }
    }

    public class ParentQuery : TextContentQueryBase
    {
        public ParentQuery(Repository repository, Schema parentSchema, TextFolder parentFolder, IContentQuery<TextContent> childrenQuery)
            : this(repository, parentSchema, parentFolder, childrenQuery, null)
        {

        }
        public ParentQuery(Repository repository, Schema parentSchema, TextFolder parentFolder, IContentQuery<TextContent> childrenQuery, IExpression expression)
            : base(repository, expression)
        {
            this.ParentSchema = parentSchema;
            this.ChildrenQuery = childrenQuery;
            this.ParentFolder = parentFolder;
        }

        public Schema ParentSchema { get; set; }
        public TextFolder ParentFolder { get; set; }

        public IContentQuery<TextContent> ChildrenQuery { get; set; }

        public override IContentQuery<TextContent> Create(IExpression expression)
        {
            return new ParentQuery(this.Repository, this.ParentSchema, this.ParentFolder, this.ChildrenQuery, expression);
        }

        public override TextFolder Folder
        {
            get { return ParentFolder; }
        }
    }
}