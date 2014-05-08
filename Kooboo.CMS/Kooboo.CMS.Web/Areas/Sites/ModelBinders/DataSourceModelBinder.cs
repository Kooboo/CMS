using Kooboo.CMS.Sites.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.ModelBinders
{
    public class DataSourceModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            string designerName = controllerContext.Controller.ValueProvider.GetValue("Designer").AttemptedValue;

            var designer = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IDataSourceDesigner>(designerName);

            var dataSource = designer.CreateDataSource();

            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => dataSource, dataSource.GetType());

            return dataSource;
        }
    }
}