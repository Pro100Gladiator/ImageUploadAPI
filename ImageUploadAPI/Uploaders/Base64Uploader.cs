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

namespace ImageUploadAPI.Uploaders
{
    public class Base64Uploader
    {
        private static IWebHostEnvironment _environment;
        public Base64Uploader(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string Process(JsonElement input)
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

            string result = "";
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
                        Previewer.uploadPreview(picPath, name + "." + type);
                    }
                    return result + $"Total pictures uploaded: {successCounter} from {curCounter}";
                }
                catch (Exception ex)
                {
                    //TODO: log error: ex.Message.ToString()
                    return "Error. Please, try again.";
                }
            }
            else
                return "Unsucsessful uploading. Please, try again.";
        }

    }
}
