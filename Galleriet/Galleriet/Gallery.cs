using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Galleriet
{
    public class Gallery
    {
        Regex ApprovedExtensions;
        string PhysicalUploadedImagesPath;
        Regex SanitizePath;

        Gallery()
        {

        }
        IEnumerable<string> GetImageNames()
        {
            return new List<string>();
        }
        bool ImageExsists(string name)
        {
            return true;
        }
        bool IsValidImage(Image image)
        {
            return true;
        }
        string SaveImage(Stream stream, string fileName)
        {
            return "yes";
        }
    }
}