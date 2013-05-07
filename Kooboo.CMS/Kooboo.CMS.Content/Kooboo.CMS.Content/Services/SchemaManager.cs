#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Form;
using Kooboo.Extensions;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Content.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(SchemaManager))]
    public class SchemaManager : ManagerBase<Schema, ISchemaProvider>, IManager<Schema, ISchemaProvider>
    {
        public SchemaManager(ISchemaProvider provider)
            : base(provider)
        {
        }
        public virtual Schema Create(Repository repository, string schemaName, string templateName)
        {
            var template = ServiceFactory.RepositoryTemplateManager.GetItemTemplate(templateName);
            if (template == null)
            {
                throw new KoobooException("The template file does not exists.".Localize());
            }
            using (FileStream fs = new FileStream(template.TemplateFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Create(repository, schemaName, fs);
            }
        }

        public virtual Schema Create(Repository repository, string schemaName, Stream templateStream)
        {
            return Provider.Create(repository, schemaName, templateStream);
        }

        public override Schema Get(Repository repository, string name)
        {
            var dummy = new Schema(repository, name);
            return Provider.Get(dummy);
        }

        public new void Remove(Repository repository, Schema schema)
        {
            if (GetRelationFolders(schema).Count() > 0)
            {
                throw new Exception(string.Format("'{0}' is being used by some folders!".Localize(), schema.Name));
            }
            base.Remove(repository, schema);
        }

        #region Import & Export Schema
        public virtual void Export(Repository repository, Stream stream, IEnumerable<Schema> exports)
        {
            foreach (var item in exports)
            {
                item.Repository = repository;
            }
            Provider.Export(repository, exports, stream);
        }

        public virtual void Import(Repository repository, Stream stream, bool @override)
        {
            Provider.Import(repository, stream, @override);
        }
        #endregion


        public virtual Column GetColumn(Repository repository, string schemaName, string name)
        {
            var schema = Get(repository, schemaName);
            return schema.Columns.Where(o => o.Name == name).FirstOrDefault();
        }

        public virtual void AddColumn(Repository repository, string schemaName, Column column)
        {
            var schema = Get(repository, schemaName);
            var old = (Schema)schema.DeepClone();
            if (schema.Columns.Where(o => o.Name == column.Name).Count() > 0)
            {
                throw new ItemAlreadyExistsException();
            }
            schema.AddColumn(column);
            this.Update(repository, schema, old);
        }

        public virtual void UpdateColumn(Repository repository, string schemaName, Column @new, string oldName)
        {
            var schema = Get(repository, schemaName);
            var old = (Schema)schema.DeepClone();
            if (@new.Name != oldName &&
                schema.Columns.Where(o => o.Name == @new.Name).Count() > 0)
            {
                throw new ItemAlreadyExistsException();
            }
            schema.UpdateColumn(new Column() { Name = oldName }, @new);
            this.Update(repository, schema, old);
        }

        public virtual void DeleteColumn(Repository repository, string schemaName, string name)
        {
            var schema = Get(repository, schemaName);
            var old = (Schema)schema.DeepClone();
            schema.RemoveColumn(new Column() { Name = name });
            this.Update(repository, schema, old);
        }



        #region Build Forms
        public virtual void ResetForm(Repository repository, string schemaName, FormType formType)
        {
            Schema schema = new Schema(repository, schemaName).AsActual();
            SchemaPath path = new SchemaPath(schema);
            if ((formType & FormType.Grid) == FormType.Grid)
            {
                ResetForm(schema, path, FormType.Grid);
            }
            if ((formType & FormType.Create) == FormType.Create)
            {
                ResetForm(schema, path, FormType.Create);
            }
            if ((formType & FormType.Detail) == FormType.Detail)
            {
                ResetForm(schema, path, FormType.Detail);
            }
            if ((formType & FormType.List) == FormType.List)
            {
                ResetForm(schema, path, FormType.List);
            }
            if ((formType & FormType.Selectable) == FormType.Selectable)
            {
                ResetForm(schema, path, FormType.Selectable);
            }
            if ((formType & FormType.Update) == FormType.Update)
            {
                ResetForm(schema, path, FormType.Update);
            }

            schema.TemplateBuildByMachine = true;

            Update(repository, schema, schema);
        }

        private static void ResetForm(Schema schema, SchemaPath path, FormType formType)
        {
            var formFilePhysicalPath = schema.GetFormFilePhysicalPath(formType);
            string form = schema.GenerateForm(formType);
            IO.IOUtility.SaveStringToFile(formFilePhysicalPath, form);
        }
        public virtual string GetForm(Repository repository, string schemaName, FormType formType)
        {
            Schema schema = new Schema(repository, schemaName);
            var formFilePhysicalPath = schema.GetFormFilePhysicalPath(formType);
            if (!File.Exists(formFilePhysicalPath))
            {
                ResetForm(repository, schemaName, formType);
            }
            return IO.IOUtility.ReadAsString(formFilePhysicalPath);
        }
        public virtual void SaveForm(Repository repository, string schemaName, string body, FormType formType, bool maunallyChanged)
        {
            Schema schema = new Schema(repository, schemaName);

            var formFilePhysicalPath = schema.GetFormFilePhysicalPath(formType);

            IO.IOUtility.SaveStringToFile(formFilePhysicalPath, body);

            schema = schema.AsActual();

            schema.TemplateBuildByMachine = maunallyChanged;

            Update(repository, schema, schema);

        }
        #endregion


        #region relation
        public virtual IEnumerable<TextFolder> GetRelationFolders(Schema schema)
        {
            var folderProvider = Providers.DefaultProviderFactory.GetProvider<ITextFolderProvider>();

            return folderProvider.BySchema(schema);

        }
        #endregion

        #region Copy

        public virtual void Copy(Repository repository, string sourceName, string destName)
        {
            var provider = Provider;
            provider.Copy(repository, sourceName, destName);
        }

        #endregion
    }
}
