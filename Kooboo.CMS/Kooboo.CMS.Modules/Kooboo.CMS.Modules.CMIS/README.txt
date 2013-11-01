For publishing page reason, the repositoryId in CMIS is the siteId(FullName of site) actually.

How to deploy CMIS:

1. Copy the Kooboo.CMS.Modules.CMIS.dll into Bin folder of Kooboo CMS.

2. Copy the following configuration into web.config:

3. Visit http://{hostname}/api/cmis.svc to test if the CMIS service works.
<system.serviceModel>
    <bindings>
      <basicHttpBinding>
        <binding name="unsecureHttpBinding" messageEncoding="Mtom" closeTimeout="00:01:00" openTimeout="00:01:00" receiveTimeout="00:10:00" sendTimeout="00:01:00" transferMode="Streamed" maxBufferSize="65536" maxReceivedMessageSize="67108864">
        </binding>
        <binding name="secureHttpBinding" messageEncoding="Mtom" closeTimeout="00:01:00" openTimeout="00:01:00" receiveTimeout="00:10:00" sendTimeout="00:01:00" transferMode="Streamed" maxBufferSize="65536" maxReceivedMessageSize="67108864">
          <security mode="TransportWithMessageCredential">
            <message clientCredentialType="UserName"/>
            <transport clientCredentialType="None"></transport>
          </security>
        </binding>
      </basicHttpBinding>
    </bindings>
    <behaviors>
      <serviceBehaviors>
        <behavior name="enableMetadata">
          <serviceMetadata httpGetEnabled="true" />
          <serviceDebug includeExceptionDetailInFaults="true" />
          <serviceCredentials>
            <userNameAuthentication userNamePasswordValidationMode="Custom" customUserNamePasswordValidatorType="Kooboo.CMS.Modules.CMIS.WcfExtensions.UserValidator,Kooboo.CMS.Modules.CMIS"/>
          </serviceCredentials>
        </behavior>
      </serviceBehaviors>
    </behaviors>

    <services>
      <service name="Kooboo.CMS.Modules.CMIS.Services.Implementation.Service" behaviorConfiguration="enableMetadata">
        <!--<endpoint address="" contract="Kooboo.CMS.Modules.CMIS.Services.IService" binding="basicHttpBinding" bindingConfiguration="secureHttpBinding" />
        <endpoint address="mex" contract="IMetadataExchange" binding="mexHttpsBinding" />-->
        <endpoint address="" binding="basicHttpBinding"  contract="Kooboo.CMS.Modules.CMIS.Services.IService" bindingConfiguration="unsecureHttpBinding"  />
        <endpoint address="mex" contract="IMetadataExchange" binding="mexHttpBinding"/>
      </service>
    </services>
    <serviceHostingEnvironment>
      <serviceActivations>
        <add relativeAddress="api/cmis.svc" service="Kooboo.CMS.Modules.CMIS.Services.Implementation.Service" factory="Kooboo.CMS.Modules.CMIS.WcfExtensions.DIServiceHostFactory,Kooboo.CMS.Modules.CMIS" />
      </serviceActivations>
    </serviceHostingEnvironment>
  </system.serviceModel>


Security tips:

1. The default Cmis service is published by HTTP, the service setting in the web.config is:

 <service name="Kooboo.CMS.Modules.CMIS.Services.Implementation.Service" behaviorConfiguration="enableMetadata">       
        <endpoint address="" binding="basicHttpBinding"  contract="Kooboo.CMS.Modules.CMIS.Services.IService" bindingConfiguration="unsecureHttpBinding"  />
        <endpoint address="mex" contract="IMetadataExchange" binding="mexHttpBinding"/>
 </service>

2. The Username/Passowrd validation only works with HTTPS, because the .NET framework does not allow calls with UsernameTokens over plain HTTP.

3. How to publish the Cmis service by HTTPS.

 - Bind a SSL certificate to your site.
 - Modify the "services" section as following.(Just comment the default HTTP binding, uncomment the HTTPS binding.)

  <service name="Kooboo.CMS.Modules.CMIS.Services.Implementation.Service" behaviorConfiguration="enableMetadata">
       <endpoint address="" contract="Kooboo.CMS.Modules.CMIS.Services.IService" binding="basicHttpBinding" bindingConfiguration="secureHttpBinding" />
       <endpoint address="mex" contract="IMetadataExchange" binding="mexHttpsBinding" />
  </service>