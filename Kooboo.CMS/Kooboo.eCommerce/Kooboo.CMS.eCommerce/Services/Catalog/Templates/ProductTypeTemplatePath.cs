#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Services.Catalog.Templates
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductTypeTemplatePath
    {
        #region Static fields
        public static string DirName = "ProductTypes";
        #endregion

        #region .ctor
        public ProductTypeTemplatePath(ICommerceDataDir commerceDataDir, ProductType productType)
        {
            var basePhysicalPath = Path.Combine(commerceDataDir.DataPhysicalPath, DirName, productType.Name);
            var baseVirtualPath = UrlUtility.Combine(commerceDataDir.DataVirutalPath, DirName, productType.Name);

            BaseDirPath = new HttpFilePath()
            {
                PhysicalPath = basePhysicalPath,
                VirtualPath = baseVirtualPath
            };

            CustomFieldsTemplatePath = new HttpFilePath()
            {
                PhysicalPath = Path.Combine(basePhysicalPath, "CustomFields.cshtml"),
                VirtualPath = UrlUtility.Combine(baseVirtualPath, "CustomFields.cshtml")
            };
            VariantGridTemplatePath = new HttpFilePath()
            {
                PhysicalPath = Path.Combine(basePhysicalPath, "VariantGrid.cshtml"),
                VirtualPath = UrlUtility.Combine(baseVirtualPath, "VariantGrid.cshtml")
            };

            VariantCreateTemplatePath = new HttpFilePath()
            {
                PhysicalPath = Path.Combine(basePhysicalPath, "VariantCreate.cshtml"),
                VirtualPath = UrlUtility.Combine(baseVirtualPath, "VariantCreate.cshtml")
            };

            VariantEditTemplatePath = new HttpFilePath()
            {
                PhysicalPath = Path.Combine(basePhysicalPath, "VariantEdit.cshtml"),
                VirtualPath = UrlUtility.Combine(baseVirtualPath, "VariantEdit.cshtml")
            };
        }
        #endregion

        #region Properties
        public HttpFilePath BaseDirPath { get; set; }
        public HttpFilePath CustomFieldsTemplatePath { get; set; }
        public HttpFilePath VariantGridTemplatePath { get; set; }
        public HttpFilePath VariantCreateTemplatePath { get; set; }
        public HttpFilePath VariantEditTemplatePath { get; set; }
        #endregion
    }
}
