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
using ImageUploadAPI.Uploaders;

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

            formUploader = new MultipartUploader(_environment);
            urlUploader = new UrlUploader(_environment);
            jsonUploader = new Base64Uploader(_environment);
        }

        MultipartUploader formUploader;
        UrlUploader urlUploader;
        Base64Uploader jsonUploader;

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<string> ProcessMultipart([FromForm] FileUpload input)
        {
            return formUploader.Process(input);
            
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<string> ProcessJSON([FromBody] JsonElement input)
        {
            return jsonUploader.Process(input);
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Produces("application/json")]
        public async Task<string> ProcessURLs([FromForm] FileUpload input)
        {
            return urlUploader.Process(input);
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
