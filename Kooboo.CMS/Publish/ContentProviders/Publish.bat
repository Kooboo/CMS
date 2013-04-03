
copy "..\..\Kooboo.CMS.Content\Kooboo.CMS.Content.Persistence.SQLCe\bin\Release\Kooboo.CMS.Content.Persistence.SQLCe.dll" "SQLCe\Kooboo.CMS.Content.Persistence.SQLCe.dll"

copy "..\..\Kooboo.CMS.Content\Kooboo.CMS.Content.Persistence.Mysql\bin\Release\Kooboo.CMS.Content.Persistence.Mysql.dll" "Mysql\Kooboo.CMS.Content.Persistence.Mysql.dll"

copy "..\..\Kooboo.CMS.Content\Kooboo.CMS.Content.Persistence.SQLServer\bin\Release\Kooboo.CMS.Content.Persistence.SQLServer.dll" "SQLServer\Kooboo.CMS.Content.Persistence.SQLServer.dll"

copy "..\..\Kooboo.CMS.Content\Kooboo.CMS.Content.Persistence.MongoDB\bin\Release\Kooboo.CMS.Content.Persistence.MongoDB.dll" "MongoDB\Kooboo.CMS.Content.Persistence.MongoDB.dll"


copy "..\..\Kooboo.CMS.Content\Kooboo.CMS.Content.Persistence.AzureBlobService\bin\Release\Kooboo.CMS.Content.Persistence.AzureBlobService.dll" "AzureBlob\Kooboo.CMS.Content.Persistence.AzureBlobService.dll"
copy "..\..\Kooboo.CMS.Content\Kooboo.CMS.Content.Persistence.AzureBlobService\bin\Release\Microsoft.WindowsAzure.StorageClient.dll" "AzureBlob\Microsoft.WindowsAzure.StorageClient.dll"


copy "..\..\Kooboo.CMS.Content\Kooboo.CMS.Content.Persistence.FileServerProvider\bin\Release\Kooboo.CMS.Content.Persistence.FileServerProvider.dll" "FileServerProvider\Kooboo.CMS.Content.Persistence.FileServerProvider.dll"
copy "..\..\Kooboo.CMS.Content\Kooboo.CMS.Content.FileServer.Interfaces\bin\Release\Kooboo.CMS.Content.FileServer.Interfaces.dll" "FileServerProvider\Kooboo.CMS.Content.FileServer.Interfaces.dll"


..\7z a ..\Released\Content_Providers.zip AzureBlob\*.* FileServerProvider\*.* MongoDB\*.* Mysql\*.* SQLCe\*.* SQLServer\*.*
