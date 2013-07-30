
copy "..\..\Kooboo.CMS.Account.Persistence.EntityFramework\bin\Release\Kooboo.CMS.Account.Persistence.EntityFramework.dll" "EntityFramework\Kooboo.CMS.Account.Persistence.EntityFramework.dll"

copy "..\..\..\Lib\EntityFramework\*.dll" "EntityFramework\*.*"

..\7z a ..\Released\Account_Providers.zip EntityFramework\*.*