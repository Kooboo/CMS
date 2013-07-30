
copy "..\..\Kooboo.CMS.Member.Persistence.EntityFramework\bin\Release\Kooboo.CMS.Member.Persistence.EntityFramework.dll" "EntityFramework\Kooboo.CMS.Member.Persistence.EntityFramework.dll"

copy "..\..\..\Lib\EntityFramework\*.dll" "EntityFramework\*.*"

..\7z a ..\Released\Membership_Providers.zip EntityFramework\*.*