#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.EventBus;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using Kooboo.CMS.eCommerce.Services.Catalog.Templates;
using Kooboo.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Catalog
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProductTypeService))]
    public class ProductTypeService : Non_RelationalServiceBase<ProductType, IProductTypeProvider>, IProductTypeService
    {
        #region Properties
        public IProductTypeTemplateGenerator TemplateGenerator { get; private set; }
        #endregion

        #region .ctor
        public ProductTypeService(IProductTypeProvider productTypeProvider, IEventPublisher eventPublisher,
            IProductTypeTemplateGenerator templateGenerator)
            : base(productTypeProvider, eventPublisher)
        {
            this.TemplateGenerator = templateGenerator;
        }
        #endregion

        #region Methods
        public override void Add(ProductType productType)
        {
            base.Add(productType);
            ResetTemplates(productType);
        }

        public override void Update(ProductType newProductType, Models.Catalog.ProductType oldProductType)
        {
            base.Update(newProductType, oldProductType);

            ResetTemplates(newProductType);
        }

        public override void Delete(ProductType productType)
        {
            base.Delete(productType);

            DeleteTemplates(productType);
        }

        public virtual void ResetTemplates(ProductType productType)
        {
            var templatePath = productType.GetTemplatePath();
            IOUtility.SaveStringToFile(templatePath.CustomFieldsTemplatePath.PhysicalPath, TemplateGenerator.GenerateCustomeFieldsEditForm(productType));
            IOUtility.SaveStringToFile(templatePath.VariantCreateTemplatePath.PhysicalPath, TemplateGenerator.GenerateVariantCreateForm(productType));
            IOUtility.SaveStringToFile(templatePath.VariantEditTemplatePath.PhysicalPath, TemplateGenerator.GenerateVariantEditForm(productType));
            IOUtility.SaveStringToFile(templatePath.VariantGridTemplatePath.PhysicalPath, TemplateGenerator.GenerateVariantGridForm(productType));

        }

        protected virtual void DeleteTemplates(ProductType productType)
        {
            var templatePath = productType.GetTemplatePath();
            IOUtility.DeleteDirectory(templatePath.BaseDirPath.PhysicalPath, true);
        }
        #endregion
    }
}
