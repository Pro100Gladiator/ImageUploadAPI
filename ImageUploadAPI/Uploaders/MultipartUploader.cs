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
    public class MultipartUploader
    {
        private static IWebHostEnvironment _environment;
        public MultipartUploader(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string Process(FileUpload input)
        {
            if (input?.Pictures?.Count > 0)
            {
                try
                {
                    string picPath = _environment.WebRootPath + "\\Upload\\";
                    string previewPath = _environment.WebRootPath + "\\Preview\\";
                    if (!Directory.Exists(picPath))
                    {
                        Directory.CreateDirectory(picPath);
                    }
                    if (!Directory.Exists(previewPath))
                    {
                        Directory.CreateDirectory(previewPath);
                    }

                    string result = "";
                    int successCounter = 0;
                    int curCounter = 0;

                    foreach (var pic in input.Pictures)
                    {
                        string fileName = pic.FileName;
                        fileName = Helpers.GetUniqueName(fileName, picPath);

                        using (FileStream fileStream = System.IO.File.Create(picPath + fileName))
                        {
                            pic.CopyTo(fileStream);
                            fileStream.Flush();

                            successCounter++;
                            curCounter++;
                            result += $"Picture {curCounter} successfully downloaded.\n";
                        }
                        Previewer.uploadPreview(picPath, previewPath, fileName);
                    }
                    return result + $"Total pictures uploaded: {successCounter} from {curCounter}";
                }
                catch (Exception ex)
                {
                    //TODO: log error: ex.Message.ToString()
                    //return "Error. Please, try again.";
                    return ex.Message.ToString();
                }
            }
            else
                return "Unsucsessful uploading. Please, try again.";
        }
    }
}
