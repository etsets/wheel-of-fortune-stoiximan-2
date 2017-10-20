using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelOfFortune.Models
{
    public class AzureStorageConfig
    {
        //public string AccountName { get; set; }
        public string AccountName = "storage_account_name";
        //public string AccountKey { get; set; }
        public string AccountKey = "storage_account_key";
        public string QueueName { get; set; }
        //public string ImageContainer { get; set; }
        public string ImageContainer = "faceapi";
        //public string ThumbnailContainer { get; set; }
        public string ThumbnailContainer = "faceapi-thumb";

    }
}
