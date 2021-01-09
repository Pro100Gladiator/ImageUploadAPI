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

namespace ImageUploadAPI.Common
{
    public class Previewer
    {
        public static void uploadPreview(string pathFrom, string pathTo, string picNameAndExt)
        {
            var image = Image.FromFile(pathFrom + picNameAndExt);
            var preview = resizeImage(image, new Size(100, 100));
            preview.Save($"{pathTo}preview_{picNameAndExt}");
            image.Dispose();
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
    }
}
