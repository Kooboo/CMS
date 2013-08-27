copy "..\..\Kooboo.CMS.Sites.TemplateEngines.NVelocity\bin\Release\Kooboo.CMS.Sites.TemplateEngines.NVelocity.dll" "NVelocity\Kooboo.CMS.Sites.TemplateEngines.NVelocity.dll"

md Razor

copy "..\..\Kooboo.CMS.Sites.TemplateEngines.Razor\bin\Release\Kooboo.CMS.Sites.TemplateEngines.Razor.dll" "Razor\Kooboo.CMS.Sites.TemplateEngines.Razor.dll"

md WebForm
copy "..\..\Kooboo.CMS.Sites.TemplateEngines.WebForm\bin\Release\Kooboo.CMS.Sites.TemplateEngines.WebForm.dll" "WebForm\Kooboo.CMS.Sites.TemplateEngines.WebForm.dll"

..\7z\7z a ..\Released\Template_Engines.zip NVelocity\*.* Razor\*.* WebForm\*.*