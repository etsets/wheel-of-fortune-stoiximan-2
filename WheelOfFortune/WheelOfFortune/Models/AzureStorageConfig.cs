using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelOfFortune.Models
{
    public class AzureStorageConfig
    {
        public string AccountName = "azure_storage_account_name";
        public string AccountKey = "azure_storage_account_key";
        public string QueueName { get; set; }
        public string ImageContainer = "faceapi";
        public string ThumbnailContainer = "faceapi-thumb";
    }
}
