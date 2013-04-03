using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.ComponentModel;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Web.Areas.Account.Models;
using Kooboo.CMS.Web.Areas.Sites.ModelBinders;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Sites.Services;
using System.Web.Management;
using Kooboo.CMS.Sites;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public class ControllerBase : AreaControllerBase
    {
        static ControllerBase()
        {

            #region ModelBinder
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(IDataRule), new DataRuleBinder());
			System.Web.Mvc.ModelBinders.Binders.Add(typeof(DataRuleBase), new DataRuleBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(PagePosition), new PagePositionBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(Parameter), new ParameterBinder());
            #endregion

            TypeDescriptorHelper.RegisterMetadataType(typeof(Site), typeof(Site_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(Security), typeof(Security_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(CustomError), typeof(CustomError_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(Layout), typeof(Layout_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(Template), typeof(Template_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(PathResource), typeof(PathResource_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(Kooboo.CMS.Sites.Models.View), typeof(View_Metadata));

            #region Page Metadata
            TypeDescriptorHelper.RegisterMetadataType(typeof(Page), typeof(Page_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(PagePosition), typeof(PagePosition_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(ViewPosition), typeof(ViewPosition_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(HtmlPosition), typeof(HtmlPosition_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(ModulePosition), typeof(ModulePosition_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(Navigation), typeof(Navigation_Metadata));
            #endregion
            TypeDescriptorHelper.RegisterMetadataType(typeof(FileResource), typeof(FileResource_Metadata));
            #region Theme Metadata
            TypeDescriptorHelper.RegisterMetadataType(typeof(Theme), typeof(Theme_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(StyleFile), typeof(Style_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(ThemeImageFile), typeof(ThemeImageFile_Metadata));
            #endregion
            TypeDescriptorHelper.RegisterMetadataType(typeof(CustomFile), typeof(CustomFile_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(ScriptFile), typeof(ScriptFile_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(ScriptFile), typeof(ScriptFile_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(UrlRedirect), typeof(UrlRedirect_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(Kooboo.Globalization.Element), typeof(Element_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(HtmlMeta), typeof(HtmlMeta_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(PageRoute), typeof(Pageroute_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(UrlKeyMap), typeof(UrlKeyMap_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(AssemblyFile), typeof(AssemblyFile_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Type), typeof(Type_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(ModuleInfo), typeof(ModuleInfo_Metadata));

            TypeDescriptorHelper.RegisterMetadataType(typeof(DataRuleSetting), typeof(DataRuleSetting_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(DataRuleBase), typeof(DataRuleBase_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(User), typeof(Kooboo.CMS.Web.Areas.Sites.Models.User_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(Smtp), typeof(Kooboo.CMS.Web.Areas.Sites.Models.Smtp_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(PagePublishingQueueItem), typeof(PagePublishingQueue_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(VersionInfo), typeof(VersionInfo_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(HtmlBlock), typeof(HtmlBlock_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(DiagnosisResult), typeof(DiagnosisResult_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(WebApplicationInformation), typeof(WebApplicationInformation_Metadata));
        }
    }
}