


copy "..\..\Kooboo.CMS.SitesProviders\Kooboo.CMS.Sites.Providers.AzureTable\bin\Release\Kooboo.CMS.Sites.Providers.AzureTable.dll" "AzureTable\Kooboo.CMS.Sites.Providers.AzureTable.dll"
copy "..\..\Kooboo.CMS.SitesProviders\Kooboo.CMS.Sites.Providers.AzureTable\bin\Release\Microsoft.WindowsAzure.StorageClient.dll" "AzureTable\Microsoft.WindowsAzure.StorageClient.dll"


copy "..\..\Kooboo.CMS.SitesProviders\Kooboo.CMS.Sites.Providers.SqlServer\bin\Release\Kooboo.CMS.Sites.Providers.SqlServer.dll" "SQLServer\Kooboo.CMS.Sites.Providers.SqlServer.dll"
copy "..\..\..\Lib\EntityFramework\*.dll" "SQLServer\*.*"


copy "..\..\Kooboo.CMS.SitesProviders\Kooboo.CMS.Sites.Persistence.EntityFramework\bin\Release\Kooboo.CMS.Sites.Persistence.EntityFramework.dll" "EntityFramework\Kooboo.CMS.Sites.Persistence.EntityFramework.dll"
copy "..\..\..\Lib\EntityFramework\*.dll" "EntityFramework\*.*"

copy "..\..\Kooboo.CMS.SitesProviders\Kooboo.CMS.Sites.Persistence.Couchbase\bin\Release\Couchbase.dll" "Couchbase\Couchbase.dll"
copy "..\..\Kooboo.CMS.SitesProviders\Kooboo.CMS.Sites.Persistence.Couchbase\bin\Release\Enyim.Caching.dll" "Couchbase\Enyim.Caching.dll"
copy "..\..\Kooboo.CMS.SitesProviders\Kooboo.CMS.Sites.Persistence.Couchbase\bin\Release\Kooboo.CMS.Sites.Persistence.Couchbase.dll" "Couchbase\Kooboo.CMS.Sites.Persistence.Couchbase.dll"




..\7z\7z a ..\Released\Site_Providers.zip AzureTable\*.* SQLServer\*.* EntityFramework\*.* Couchbase\*.*