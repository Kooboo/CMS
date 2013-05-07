


copy "..\..\Kooboo.CMS.Sites.Providers.AzureTable\bin\Release\Kooboo.CMS.Sites.Providers.AzureTable.dll" "AzureTable\Kooboo.CMS.Sites.Providers.AzureTable.dll"
copy "..\..\Kooboo.CMS.Sites.Providers.AzureTable\bin\Release\Microsoft.WindowsAzure.StorageClient.dll" "AzureTable\Microsoft.WindowsAzure.StorageClient.dll"


copy "..\..\Kooboo.CMS.Sites.Providers.SqlServer\bin\Release\Kooboo.CMS.Sites.Providers.SqlServer.dll" "SQLServer\Kooboo.CMS.Sites.Providers.SqlServer.dll"
copy "..\..\Kooboo.CMS.Sites.Providers.SqlServer\bin\Release\EntityFramework.dll" "SQLServer\EntityFramework.dll"

..\7z a ..\Released\Site_Providers.zip AzureTable\*.* SQLServer\*.*