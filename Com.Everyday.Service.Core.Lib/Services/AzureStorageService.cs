﻿using Com.DanLiris.Service.Core.Lib.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class AzureStorageService
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected CloudStorageAccount StorageAccount { get; private set; }
        protected CloudBlobContainer StorageContainer { get; private set; }

        public AzureStorageService(IServiceProvider serviceProvider)
        {
            string storageAccountName = APIEndpoint.StorageAccountName;
            string storageAccountKey = APIEndpoint.StorageAccountKey;
            string storageContainer = "everyday-product-image";

            this.ServiceProvider = serviceProvider;
            this.StorageAccount = new CloudStorageAccount(new StorageCredentials(storageAccountName, storageAccountKey), useHttps: true);
            this.StorageContainer = this.Configure(storageContainer).GetAwaiter().GetResult();
        }

        private async Task<CloudBlobContainer> Configure(string storageContainer)
        {
            CloudBlobClient cloudBlobClient = this.StorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(storageContainer);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            BlobContainerPermissions permissions = SetContainerPermission(true);
            await cloudBlobContainer.SetPermissionsAsync(permissions);

            return cloudBlobContainer;
        }

        private BlobContainerPermissions SetContainerPermission(Boolean isPublic)
        {
            BlobContainerPermissions permissions = new BlobContainerPermissions();
            if (isPublic)
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            else
                permissions.PublicAccess = BlobContainerPublicAccessType.Off;
            return permissions;
        }
    }

    
}
