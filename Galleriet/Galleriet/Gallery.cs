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
        static readonly Regex ApprovedExtensions;
        static string PhysicalUploadedImagesPath;
        static readonly Regex SanitizePath;

        static Gallery()
        {
            ApprovedExtensions = new Regex(string.Format("{0}", "^.*\\.(gif|jpg|png)$"));
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            PhysicalUploadedImagesPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "Images");
        }
        IEnumerable<string> GetImageNames()
        {
            DirectoryInfo df = new DirectoryInfo(PhysicalUploadedImagesPath);
            List<string> files = new List<string>();
            foreach(FileInfo fi in df.GetFiles())
            {
                files.Add(fi.ToString());
            }
            return files;
        }
        static bool ImageExsists(string name)
        {
            return File.Exists(Path.Combine(PhysicalUploadedImagesPath, name));
        }
        bool IsValidImage(Image image)
        {
            return image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid;
        }
        string SaveImage(Stream stream, string fileName)
        {
            string thumbnailPath = Path.Combine(PhysicalUploadedImagesPath, "thumbnails");
            var image = System.Drawing.Image.FromStream(stream);
            if (!IsValidImage(image))
                throw new ArgumentException();

            var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
            int counter = 0;
            string tempFileName = fileName;

            while (File.Exists(Path.Combine(thumbnailPath, fileName)))
            {
                tempFileName = fileName + counter;
            }
            thumbnail.Save(Path.Combine(thumbnailPath, tempFileName));

            using(var fs = File.Create(Path.Combine(PhysicalUploadedImagesPath, tempFileName)))
            {
                stream.CopyTo(fs);
            }
            return Path.Combine(PhysicalUploadedImagesPath, tempFileName);
        }
    }
}