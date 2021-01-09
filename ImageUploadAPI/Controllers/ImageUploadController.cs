using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MimeTypeExtension;
using ImageUploadAPI.Common;
using System.Drawing;

namespace ImageUploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public ImageUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<string> ProcessMultipart([FromForm] FileUpload input)
        {

            if (input?.Pictures?.Count > 0)
            {
                try
                {
                    string picPath = _environment.WebRootPath + "\\Upload\\";
                    if (!Directory.Exists(picPath))
                    {
                        Directory.CreateDirectory(picPath);
                    }

                    string result="";
                    int successCounter = 0;
                    int curCounter = 0;

                    foreach (var pic in input.Pictures)
                    {
                        using (FileStream fileStream = System.IO.File.Create(picPath + pic.FileName))
                        {
                            pic.CopyTo(fileStream);
                            fileStream.Flush();

                            successCounter++;
                            curCounter++;
                            result += $"Picture {curCounter} successfully downloaded.\n";
                        }
                        uploadPreview(picPath, pic.FileName);
                    }
                    return result + $"Total pictures uploaded: {successCounter} from {curCounter}";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
            else
                return "Unsucsessful uploading. Please, try again.";
            
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<string> ProcessJSON([FromBody] JsonElement input)
        {
            var json = input.ToString();

            if (String.IsNullOrWhiteSpace(json))
                return "JSON is empty.";

            List<Base64EncodedJSON> images;

            try
            {
                images = JsonConvert.DeserializeObject<List<Base64EncodedJSON>>(json);
            }
            catch (Exception ex)
            {
                return "Error in convetsation: " + ex.Message;
            }

            string result="";
            int successCounter = 0;
            int curCounter = 0;

            if (images?.Count > 0)
            {
                try
                {
                    string picPath = _environment.WebRootPath + "\\Upload\\";
                    if (!Directory.Exists(picPath))
                    {
                        Directory.CreateDirectory(picPath);
                    }

                    foreach (var pic in images)
                    {
                        string type = pic.GetType();
                        if (string.IsNullOrWhiteSpace(type))
                        {
                            result += $"Picture {curCounter} can not be loaded - because it have invalid image type.\r\n";
                            curCounter++;
                            continue;
                        }                           

                        string name = pic.GetName();


                        //TODO: problem with same name files
                        var base64 = pic.GetBase64();
                        using (FileStream fileStream = System.IO.File.Create(picPath + name + "." + type))
                        {
                            fileStream.Write(Convert.FromBase64String(base64));
                            fileStream.Flush();

                            successCounter++;
                            curCounter++;
                            result += $"Picture {curCounter} successfully downloaded.\n";
                        }
                        uploadPreview(picPath, name + "." + type);
                    }
                    return result + $"Total pictures uploaded: {successCounter} from {curCounter}";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
            else
                return "Unsucsessful uploading. Please, try again.";               
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        public async Task<string> ProcessURLs([FromForm] FileUpload input)
        {
            if (input?.Urls?.Count > 0)
            {
                try
                {
                    string picPath = _environment.WebRootPath + "\\Upload\\";
                    if (!Directory.Exists(picPath))
                    {
                        Directory.CreateDirectory(picPath);
                    }

                    string result = "";
                    int successCounter = 0;
                    int curCounter = 0;

                    foreach (var url in input.Urls)
                    {
                        string nameNExt = "";
                        if (url.Split('/').Length > 0)
                            nameNExt = url.Split('/')[url.Split('/').Length - 1];
                        else
                            return "URL is empty";

                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(new Uri(url), picPath + nameNExt);

                            successCounter++;
                            curCounter++;
                            result += $"Picture {curCounter} successfully downloaded.\n";
                        }

                        uploadPreview(picPath, nameNExt);
                    }

                    return result + $"Total pictures uploaded: {successCounter} from {curCounter}";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
            else
                return "Failed.";
        }

        private void uploadPreview(string path, string picNameAndExt)
        {
            var image = Image.FromFile(path + picNameAndExt);
            var preview = resizeImage(image, new Size(100, 100));
            preview.Save($"{path}preview_{picNameAndExt}");
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));            
        }

        #region Some Help

        //If You don't know the Format(.png, .jpeg etc) of Image
        //public void SaveImage(string filename, ImageFormat format)
        //{
        //    WebClient client = new WebClient();
        //    Stream stream = client.OpenRead(imageUrl);
        //    Bitmap bitmap; bitmap = new Bitmap(stream);

        //    if (bitmap != null)
        //    {
        //        bitmap.Save(filename, format);
        //    }

        //    stream.Flush();
        //    stream.Close();
        //    client.Dispose();
        //}

        ////Using it
        ////In method
        //private void Test()
        //{
        //    try
        //    {
        //        SaveImage("--- Any Image Path ---", ImageFormat.Png)
        //    }
        //    catch (ExternalException)
        //    {
        //        // Something is wrong with Format -- Maybe required Format is not 
        //        // applicable here
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        // Something wrong with Stream
        //    }

        //}
        #endregion



    }
}
