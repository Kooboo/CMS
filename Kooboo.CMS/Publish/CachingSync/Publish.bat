


copy "..\..\Kooboo.CMS.Caching.AzureSync\bin\Release\Kooboo.CMS.Caching.AzureSync.dll" "Azure\Kooboo.CMS.Caching.AzureSync.dll"
copy "..\..\Kooboo.CMS.Caching.AzureSync\bin\Release\Microsoft.WindowsAzure.ServiceRuntime.dll" "Azure\Microsoft.WindowsAzure.ServiceRuntime.dll"

md Remote
copy "..\..\Kooboo.CMS.Caching.NotifyRemote\bin\Release\Kooboo.CMS.Caching.NotifyRemote.dll" "Remote\Kooboo.CMS.Caching.NotifyRemote.dll"


..\7z\7z a ..\Released\Caching_Sync.zip Azure\*.* Remote\*.*