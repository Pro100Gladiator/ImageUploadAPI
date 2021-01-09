using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageUploadAPI.Common
{
    public class Base64EncodedJSON
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Base64 { get; set; }

        public Base64EncodedJSON() { }
        public Base64EncodedJSON(string name, string type, string base64)
        {
            this.Name = name;
            this.Type = type;
            this.Base64 = base64;
        }

        //TODO: date format with milliseconds and Unique name
        private string GetUniqueName()
        {
            var rnd = new Random();
            return $"{rnd.Next(999999999)}";
            //return $"picture_{System.DateTime.Now.ToString()}_{rnd.Next(9999)}";
        }

        public string GetName()
        {
            return (!string.IsNullOrWhiteSpace(Name)) ? Name : GetUniqueName();
        }

        public string GetType()
        {
            if (Base64.StartsWith("url", true, null))
               return GetTypeFromBase64(DeleteUrlFromBase64(Base64));
            else if(Base64.StartsWith("data",true,null))
                return GetTypeFromBase64(Base64);
            else
                return (!string.IsNullOrWhiteSpace(Type)) ? Type : null;
        }

        public string GetBase64()
        {
            if (Base64.StartsWith("url", true, null))
                return GetDataFromBase64(DeleteUrlFromBase64(Base64));
            else if (Base64.StartsWith("data", true, null))
                return GetDataFromBase64(Base64);
            else
                return Base64;
        }

        private string DeleteUrlFromBase64(string input)
        {
            var str1 = input.Remove(0, 5);
            var str2 = str1.Remove(str1.Length-2);
            return str2;

        }

        private string GetTypeFromBase64(string input)
        {
            return Regex.Match(input, @"data:image/(?<type>.+?);base64,(?<data>.+)").Groups["type"].Value;            
        }

        private string GetDataFromBase64(string input)
        {
            return Regex.Match(input, @"data:image/(?<type>.+?);base64,(?<data>.+)").Groups["data"].Value;
        }

        //for future - one more variant to check for image type - when you have only base64 string without params
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                //case "AAAAF":
                //    return "mp4";
                //case "JVBER":
                //    return "pdf";
                case "AAABA":
                    return "ico";
                //case "UMFYI":
                //    return "rar";
                //case "E1XYD":
                //    return "rtf";
                //case "U1PKC":
                //    return "txt";
                //case "MQOWM":
                //case "77U/M":
                //    return "srt";
                default:
                    return string.Empty;
            }
        }
    }
}
