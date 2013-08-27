
copy "..\..\Kooboo.CMS.Membership.Persistence.EntityFramework\bin\Release\Kooboo.CMS.Membership.Persistence.EntityFramework.dll" "EntityFramework\"

copy "..\..\..\Lib\EntityFramework\*.dll" "EntityFramework\*.*"

..\7z\7z a ..\Released\Membership_Providers.zip EntityFramework\*.*
