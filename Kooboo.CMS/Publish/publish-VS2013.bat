call clear.bat

cd..

call update_version.vbs

cd Publish

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" ..\Kooboo.CMS-Release.sln /t:rebuild /l:FileLogger,Microsoft.Build.Engine;logfile=Publish.log; /p:VisualStudioVersion=12.0

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" ..\Kooboo.CMS.Web\Kooboo.CMS.Web.csproj /t:ResolveReferences;Compile /t:_CopyWebApplication /p:Configuration=Release /p:WebProjectOutputDir=..\Publish\Web /p:OutputPath=..\Publish\Web\Bin /p:VisualStudioVersion=12.0

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" ..\Kooboo.CMS.Content\Kooboo.CMS.Content.FileServer.sln /t:rebuild /l:FileLogger,Microsoft.Build.Engine;logfile=Publish_FileServer.log; /p:VisualStudioVersion=12.0

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" ..\Kooboo.CMS.Content\Kooboo.CMS.Content.FileServer.Web\Kooboo.CMS.Content.FileServer.Web.csproj /t:ResolveReferences;Compile /t:_CopyWebApplication /p:Configuration=Release /p:WebProjectOutputDir=..\Publish\FileServer.Web /p:OutputPath=..\Publish\FileServer.Web\Bin /p:VisualStudioVersion=12.0

call copy.bat

call zip.bat