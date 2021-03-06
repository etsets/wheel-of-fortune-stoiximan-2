﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WheelOfFortune.Models;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using WheelOfFortune.Extensions;
using System.Net.Http.Headers;

namespace WheelOfFortune.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        const string subscriptionKey = "7acca518d1b141db9f629c30c8a424af";
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
                                        isUploaded = await StorageHelper.UploadFileToStorage(formFile, formFile.FileName, storageConfig);

                                        return Ok("Successful Upload");
                                    }
                                    else
                                    {
                                        return BadRequest("Look like the image couldnt upload to the storage");
                                        
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