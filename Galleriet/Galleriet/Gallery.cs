using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public IEnumerable<string> GetImageNames()
        {
            DirectoryInfo df = new DirectoryInfo(PhysicalUploadedImagesPath);
            List<string> files = new List<string>();
            foreach(FileInfo fi in df.GetFiles())
            {
                files.Add(fi.ToString());
            }
            return files;
        }
        public static string GetImagePath(string name, bool thumb = false)
        {
            if (name == null)
                return "";
            if (thumb)
                return Path.Combine("images/thumbnails", name);
            else if (ImageExsists(name))
                return Path.Combine("images", name);
            return "";
        }
        public static bool ImageExsists(string name)
        {
            return File.Exists(Path.Combine(PhysicalUploadedImagesPath, name));
        }
        public bool IsValidImage(Image image)
        {
            return true;
            //return image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid;
        }
        public string SaveImage(Stream stream, string fileName)
        {
            Stream thumbStream = stream;
            if (!ApprovedExtensions.IsMatch(fileName))
                throw new ArgumentException();

            fileName = SanitizePath.Replace(fileName, "");
            string tempFileName = fileName;
            string thumbnailPath = Path.Combine(PhysicalUploadedImagesPath, "thumbnails");
            var image = System.Drawing.Image.FromStream(thumbStream);
            int counter = 0;
            /*while (File.Exists(Path.Combine(thumbnailPath, fileName)))
            {
                tempFileName = Path.GetFileNameWithoutExtension(fileName) + counter + Path.GetExtension(fileName);
            }*/
            stream.Position = 0;
            using(var fs = File.Create(Path.Combine(PhysicalUploadedImagesPath, fileName)))
            {
                stream.CopyTo(fs);
            }
            /*if (!IsValidImage(image))
                throw new ArgumentException();
            */
            var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
            /*Bitmap thumbnail = new Bitmap(60, 45);
            using (Graphics gr = Graphics.FromImage(image))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(thumbnail, new Rectangle(0, 0, 60, 45));
            }
            */
            thumbnail.Save(Path.Combine(thumbnailPath, tempFileName));

            /*using(var fs = File.Create(Path.Combine(PhysicalUploadedImagesPath, tempFileName)))
            {
                stream.CopyTo(fs);
            }*/
            return Path.Combine(PhysicalUploadedImagesPath, tempFileName);
        }
        
    }
}