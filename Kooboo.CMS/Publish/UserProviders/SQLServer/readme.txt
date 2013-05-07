1.Create a database called: Kooboo_CMS, run "Install_Script.sql" to install the database.

2. Add two connectionstring in web.config for example:

    <add name="Kooboo.Connect.Providers.SqlServer.Properties.Settings.KoobooConnect" connectionString="Server=.\SQLExpress;Database=Kooboo_CMS; Trusted_Connection=Yes;"/>
    <add name="Kooboo_CMS" connectionString="Server=.\SQLExpress;Database=Kooboo_CMS; Trusted_Connection=Yes;" providerName="System.Data.SqlClient"/>

3. Copy the two assemblies to bin folder.


