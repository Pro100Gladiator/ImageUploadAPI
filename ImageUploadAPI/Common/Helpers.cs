using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUploadAPI.Common
{
    public class Helpers
    {
        public static string GetUniqueName(string input, string path)
        {
            string inputName = input.Split('.')[0];
            string inputExt = input.Split('.')[1];

            string name = inputName;
            int copyCounter = 1;

            while (System.IO.File.Exists(path+name+"."+inputExt))
            {
                name = $"{inputName}_({copyCounter})";
                copyCounter++;
            }

            return name;
        }
    }
}
