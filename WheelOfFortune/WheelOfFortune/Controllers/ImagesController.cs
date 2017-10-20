using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelOfFortune.Extensions;
using WheelOfFortune.Models;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;

namespace WheelOfFortune.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        const string subscriptionKey = "face_api_key";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";
        // make sure that appsettings.json is filled with the necessary details of the azure storage
        private readonly AzureStorageConfig storageConfig = null;

        public ImagesController(IOptions<AzureStorageConfig> config)
        {
            storageConfig = config.Value;
        }

        // POST /api/images/upload
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            bool isUploaded = false;

            try
            {

                if (files.Count == 0)

                    return BadRequest("No files received from the upload");

                if (storageConfig.AccountKey == string.Empty || storageConfig.AccountName == string.Empty)

                    return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");

                if (storageConfig.ImageContainer == string.Empty)

                    return BadRequest("Please provide a name for your image container in the azure blob storage");

                foreach (var formFile in files)
                {
                    if (StorageHelper.IsImage(formFile))
                    {
                        if (formFile.Length > 0)
                        {
                            using (Stream streamfile = formFile.OpenReadStream())
                            {
                                //formFile.FileName = formFile.FileName + DateTime.Now.ToString();
                                HttpClient client = new HttpClient();

                                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                                string requestParameters = "visualFeatures=Categories,Description,Color&language=en";
                                string uri = uriBase + "?" + requestParameters;
                                HttpResponseMessage response;
                                Guid guid = Guid.NewGuid();
                                BinaryReader binaryReader = new BinaryReader(streamfile);
                                byte[] byteData = binaryReader.ReadBytes((int)streamfile.Length);
                                using (ByteArrayContent content = new ByteArrayContent(byteData))
                                {
                                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                    response = await client.PostAsync(uri, content);
                                    string contentString = await response.Content.ReadAsStringAsync();

                                    if (contentString.Contains("faceId"))
                                    {
                                        isUploaded = await StorageHelper.UploadFileToStorage(streamfile, formFile.FileName, storageConfig);

                                        //return Ok("Successful Upload");
                                    }
                                    else
                                    {
                                        return BadRequest("Look like the image contains no Face");
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        return new UnsupportedMediaTypeResult();
                    }
                }

                if (isUploaded)
                {
                    if (storageConfig.ThumbnailContainer != string.Empty)

                        return new AcceptedAtActionResult("GetThumbNails", "Images", null, null);

                    else

                        return new AcceptedResult();
                }
                else

                    return BadRequest("Look like the image couldnt upload to the storage");


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> upload2(IFormFile files)
        {
            bool isUploaded = false;

            try
            {

                // if (photo.Count == 0)

                // return BadRequest("No files received from the upload");

                if (storageConfig.AccountKey == string.Empty || storageConfig.AccountName == string.Empty)

                    return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");

                if (storageConfig.ImageContainer == string.Empty)

                    return BadRequest("Please provide a name for your image container in the azure blob storage");

                //foreach (var formFile in files)
                // {
                if (StorageHelper.IsImage(files))
                {
                    if (files.Length > 0)
                    {
                        using (Stream stream = files.OpenReadStream())
                        {
                            //formFile.FileName = formFile.FileName + DateTime.Now.ToString();
                            isUploaded = await StorageHelper.UploadFileToStorage(stream, files.FileName, storageConfig);
                        }
                    }
                }
                else
                {
                    return new UnsupportedMediaTypeResult();
                }
                //  }

                if (isUploaded)
                {
                    if (storageConfig.ThumbnailContainer != string.Empty)

                        return new AcceptedAtActionResult("GetThumbNails", "Images", null, null);

                    else

                        return new AcceptedResult();
                }
                else

                    return BadRequest("Look like the image couldnt upload to the storage");


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // GET /api/images/thumbnails
        [HttpGet("thumbnails")]
        public async Task<IActionResult> GetThumbNails()
        {

            try
            {
                if (storageConfig.AccountKey == string.Empty || storageConfig.AccountName == string.Empty)

                    return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");

                if (storageConfig.ImageContainer == string.Empty)

                    return BadRequest("Please provide a name for your image container in the azure blob storage");

                List<string> thumbnailUrls = await StorageHelper.GetThumbNailUrls(storageConfig);

                return new ObjectResult(thumbnailUrls);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}