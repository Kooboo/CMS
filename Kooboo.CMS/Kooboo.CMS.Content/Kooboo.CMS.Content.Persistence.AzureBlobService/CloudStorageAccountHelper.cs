#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    public static class CloudStorageAccountHelper
    {
        public static CloudStorageAccount GetStorageAccount()
        {
            StorageCredentials crendentials = new StorageCredentialsAccountAndKey(AzureBlobServiceSettings.Instance.AccountName
                , AzureBlobServiceSettings.Instance.AccountKey);
            Uri blobEndpoint = new Uri(AzureBlobServiceSettings.Instance.Endpoint);
            CloudStorageAccount storageAccount = new CloudStorageAccount(crendentials, blobEndpoint, null, null);
            return storageAccount;
        }        
    }
}
