using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WheelOfFortune.Models
{
    public class AzureStorageConfig
    {

        public string AccountName { get; set; }
        public string AccountKey { get; set; }


        public string QueueName { get; set; }
        public string ImageContainer = "faceapi";
        public string ThumbnailContainer = "faceapi-thumb";

    }
}
