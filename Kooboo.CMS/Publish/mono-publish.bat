rd mono /Q /S
md mono

cd..

call update_version.vbs

cd Publish

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" ..\Kooboo.CMS.Mono-Release.sln /t:rebuild /l:FileLogger,Microsoft.Build.Engine;logfile=MONO-Publish.log;

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" ..\Kooboo.CMS.Web\Kooboo.CMS.Web.MONO.csproj /t:ResolveReferences;Compile /t:_CopyWebApplication /p:Configuration=Release /p:WebProjectOutputDir=..\Publish\mono /p:OutputPath=..\Publish\mono\Bin

call mono-copy.bat

cd MONO
..\7z a ..\Released\Kooboo_CMS_MONO.zip 
cd..
