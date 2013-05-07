copy "..\..\..\Kooboo.Connect.Providers.SqlServer\bin\Release\Kooboo.Connect.Providers.SqlServer.dll" "SQLServer\Kooboo.Connect.Providers.SqlServer.dll"

copy "..\..\Kooboo.CMS.Account.Persistence.SqlSever\bin\Release\Kooboo.CMS.Account.Persistence.SqlSever.dll" "SQLServer\Kooboo.CMS.Account.Persistence.SqlSever.dll"

..\7z a ..\Released\User_Providers.zip SQLServer\*.*