#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Services.Catalog.Templates;
using System.Collections.Generic;

namespace Kooboo.CMS.eCommerce.Tests.Services.Catalog.Templates
{
    [TestClass]
    public class ProductTypeTemplateGeneratorTests
    {
        #region Fields
        private ProductType productType;

        private ProductTypeTemplateGenerator templateGenerator = new ProductTypeTemplateGenerator();
        #endregion

        #region Data Initialization
        [TestInitialize]
        public void TestInitialize()
        {
            productType = new ProductType()
            {
                Name = "Notebook",
                CustomFields = new List<ProductFieldType>() {
                    new ProductFieldType(){
                        Name="Color",
                        ControlType ="TextBox",
                        DataType = Common.DataType.String                        
                    },
                    new ProductFieldType(){
                        Name="Size",
                        ControlType ="TextBox",
                        DataType = Common.DataType.String       
                    },
                    new ProductFieldType(){
                        Name="Style",
                        ControlType = "RadioList",
                        DataType = Common.DataType.String,
                        SelectionItems= new Kooboo.CMS.Form.SelectListItem[]{
                            new Kooboo.CMS.Form.SelectListItem(){ Text ="极致轻薄",Value="极致轻薄"},
                            new Kooboo.CMS.Form.SelectListItem(){ Text ="多彩时尚",Value="多彩时尚"},
                            new Kooboo.CMS.Form.SelectListItem(){ Text ="游戏狂人",Value="游戏狂人"},
                            new Kooboo.CMS.Form.SelectListItem(){ Text ="全能商务",Value="全能商务"}
                        }

                    }
                },
                Variants = new List<ProductFieldType>()
                {
                    new ProductFieldType(){
                        Name="Memory",
                        ControlType = "RadioList",
                        DataType = Common.DataType.String,
                        SelectionItems= new Kooboo.CMS.Form.SelectListItem[]{
                            new Kooboo.CMS.Form.SelectListItem(){ Text ="4G",Value="4G"},
                            new Kooboo.CMS.Form.SelectListItem(){ Text ="8G",Value="8G"}                        
                        }
                    }
                }
            };
        }
        #endregion

        #region GenerateCustomeFieldsEditForm

        [TestMethod]
        public void Test_GenerateCustomeFieldsEditForm()
        {
            var template = templateGenerator.GenerateCustomeFieldsEditForm(productType);

            Assert.IsNotNull(template);
            Assert.AreNotEqual("", template);
        }
        #endregion

        #region GenerateVariantGridForm
        [TestMethod]
        public void Test_GenerateVariantGridForm()
        {
            var template = templateGenerator.GenerateVariantGridForm(productType);

            Assert.IsNotNull(template);
            Assert.AreNotEqual("", template);
        }
        #endregion

        #region GenerateVariantCreateForm
        [TestMethod]
        public void Test_GenerateVariantCreateForm()
        {
            var template = templateGenerator.GenerateVariantCreateForm(productType);

            Assert.IsNotNull(template);
            Assert.AreNotEqual("", template);
        }
        #endregion

        #region GenerateVariantEditForm
        [TestMethod]
        public void Test_GenerateVariantEditForm()
        {
            var template = templateGenerator.GenerateVariantEditForm(productType);

            Assert.IsNotNull(template);
            Assert.AreNotEqual("", template);
        }
        #endregion
    }
}
