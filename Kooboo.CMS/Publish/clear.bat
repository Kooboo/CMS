rd Azure\Kooboo_CMS /Q /S
md Azure\Kooboo_CMS
rd FileServer.Web /Q /S
md FileServer.Web
rd Released /Q /S
del Azure\Kooboo_CMS.cspkg  /Q /S
del CachingSync\Azure\Kooboo.CMS.Caching.AzureSync.dll /Q /S
del CachingSync\Remote\Kooboo.CMS.Caching.NotifyRemote.dll /Q /S
del ContentProviders\AzureBlob\Kooboo.CMS.Content.Persistence.AzureBlobService.dll /Q /S
del ContentProviders\FileServerProvider\*.dll /Q /S
del ContentProviders\MongoDB\Kooboo.CMS.Content.Persistence.MongoDB.dll  /Q /S
del ContentProviders\Mysql\Kooboo.CMS.Content.Persistence.Mysql.dll  /Q /S
del ContentProviders\SQLCe\Kooboo.CMS.Content.Persistence.SQLCe.dll /Q /S
del ContentProviders\SQLServer\Kooboo.CMS.Content.Persistence.SQLServer.dll /Q /S
del SiteProviders\AzureTable\Kooboo.CMS.Sites.Providers.AzureTable.dll /Q /S
del SiteProviders\SQLServer\Kooboo.CMS.Sites.Providers.SqlServer.dll /Q /S
del SiteProviders\EntityFramework\Kooboo.CMS.Sites.Persistence.EntityFramework.dll /Q /S
del MembershipProviders\EntityFramework\Kooboo.CMS.Account.Persistence.EntityFramework.dll /Q /S
rd mono /Q /S
md mono
del Default\bin\Kooboo.CMS.Sites.TemplateEngines.Razor.dll /Q /S
del Default\bin\Kooboo.CMS.Sites.TemplateEngines.WebForm.dll /Q /S
del TemplateEngines\NVelocity\Kooboo.CMS.Sites.TemplateEngines.NVelocity.dll /Q /S
del TemplateEngines\Razor\Kooboo.CMS.Sites.TemplateEngines.Razor.dll /Q /S
del TemplateEngines\WebForm\Kooboo.CMS.Sites.TemplateEngines.WebForm.dll /Q /S
del AccountProviders\EntityFramework\Kooboo.CMS.Account.Persistence.EntityFramework.dll /Q /S

rd Web /Q /S
md Web
rd WPI\Kooboo_CMS /Q /S
md WPI\Kooboo_CMS
del WPI\Kooboo_CMS.zip /Q /S
del *.log /Q /S
rd ..\Kooboo.CMS.Content\Publish /Q /S
rd ..\..\Publish /Q /S