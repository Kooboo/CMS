
xcopy "Web\*.*" "WPI\Kooboo_CMS\*.*" /S /E /Y /H

cd WPI

call Publish

cd..

xcopy "Web\*.*" "Azure\Kooboo_CMS\*.*" /S /E /Y /H

cd Azure
copy Startup.bat Kooboo_CMS\Startup.bat

call Pack.bat


..\7z\7z a ..\Released\Kooboo_CMS_Azure.zip readme.txt Kooboo_CMS.cspkg ServiceConfiguration.Cloud.cscfg

cd.. 

copy SDK\Kooboo_CMS.chm Released\Kooboo_CMS.chm

cd FileServer.Web
..\7z\7z a ..\Released\FileServer_Web.zip 
cd..


