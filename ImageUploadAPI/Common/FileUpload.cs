using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUploadAPI.Common
{
    public class FileUpload
    {
        public List<IFormFile> Pictures { get; set; }
        public List<String> Urls { get; set; }

        //public string JSON { get; set; }
    }
}
