using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelOfFortune.Models
{
    public class AzureStorageConfig
    {
        public string AccountName = "gypweufs01";
        public string AccountKey = "BVcWDWEvOZedWGNV0AZd0UpdJ8HkQFEaE3JBvC8C2DCC3byn4HIrCuQ9ZNDlB3WzCMha4OQuKLwHkQOoN5Ui1A==";
        public string QueueName { get; set; }
        public string ImageContainer = "faceapi";
        public string ThumbnailContainer = "faceapi-thumb";
    }
}
