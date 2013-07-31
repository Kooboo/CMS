cd ContentProviders
call publish.bat
cd..
cd TemplateEngines
call publish.bat
cd..
cd AccountProviders
call Publish.bat
cd..
cd MembershipProviders
call Publish.bat
cd..
cd SiteProviders
call Publish.bat
cd..

cd CachingSync
call Publish.bat
cd..

copy "TemplateEngines\Razor\*.*" "mono\Bin\*.*"
copy "..\Kooboo.CMS.Web\bin\Ninject.dll" "mono\Bin\*.*"
copy "..\Kooboo.CMS.Web\bin\Kooboo.CMS.Common.Runtime.Dependency.Ninject.dll" "mono\Bin\*.*"

xcopy Default Mono /S /E /Y /H

del "mono\Web.config" /Q /S
del "mono\bin\Microsoft.Web.Infrastructure.dll" /Q /S
del "mono\bin\Microsoft.Web.Infrastructure.xml" /Q /S

copy mono\Web_mono.config mono\Web.config
del mono\Web_mono.config /Q /S
del mono\bin\*.xml /Q /S
del mono\Web.Debug.config /Q /S
del mono\Web.Release.config /Q /S
del mono\bin\*.pdb /Q /S
