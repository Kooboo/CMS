1. Install RemoteDesktop.cer on local computer.

2. Copy the files of kooboo cms into Kooboo_CMS.

3. Run Package.bat

4. Deploy Kooboo_CMS.cspkg to windows azure.

5. Install RemoteDesktop.cer into you system.

6. Add the certificate:RemoteDesktop.pfx on the Azure host.(password: kooboo)

7. The remote desktop account: admin/Kooboocms!@# (NOTE: you can custom the remote desktop account using Azure VS tool,and override the AccountEncryptedPassword setting in ServiceConfiguration.Cloud.cscfg)