using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
namespace Kooboo.CMS.Sites.Providers.AzureTable
{
    public static class CloudStorageAccountHelper
    {
        public static CloudStorageAccount GetStorageAccount()
        {
            StorageCredentials crendentials = new StorageCredentialsAccountAndKey(SiteOnAzureTableSettings.Instance.AccountName
                , SiteOnAzureTableSettings.Instance.AccountKey);
            Uri tableEndpoint = new Uri(SiteOnAzureTableSettings.Instance.Endpoint);
            CloudStorageAccount storageAccount = new CloudStorageAccount(crendentials, null, null, tableEndpoint);
            return storageAccount;
        }        
    }
}
