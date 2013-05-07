cd ContentProviders
call publish.bat
cd..
cd TemplateEngines
call publish.bat
cd..
cd UserProviders
call Publish.bat
cd..
cd SiteProviders
call Publish.bat
cd..

cd CachingSync
call Publish.bat
cd..

copy "TemplateEngines\Razor\*.*" "Web\Bin\*.*"
copy "..\Kooboo.CMS.Web\bin\Ninject.dll" "Web\Bin\*.*"
copy "..\Kooboo.CMS.Web\bin\Kooboo.CMS.Common.Runtime.Dependency.Ninject.dll" "Web\Bin\*.*"


xcopy Default Web /S /E /Y /H

del Web\Web_mono.config /Q /S
del web\Web.Debug.config /Q /S
del web\Web.Release.config /Q /S
del Web\bin\*.pdb /Q /S
del web\bin\*.xml /Q /S