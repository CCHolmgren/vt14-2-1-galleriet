using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
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
            foreach (FileInfo fi in df.GetFiles())
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
            return image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid;
        }
        public string SaveImage(Stream stream, string fileName)
        {
            Stream thumbStream = stream;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);

            if (GetImageFormat(bytes) == ImageFormat.Unknown)
                throw new ArgumentException();
            if (!ApprovedExtensions.IsMatch(fileName))
                throw new ArgumentException();

            var image = System.Drawing.Image.FromStream(thumbStream);

            fileName = SanitizePath.Replace(fileName, "");
            string tempFileName = fileName;
            string thumbnailPath = Path.Combine(PhysicalUploadedImagesPath, "thumbnails");

            int counter = 0;
            while (true)
            {
                if (File.Exists(Path.Combine(thumbnailPath, tempFileName)))
                    tempFileName = Path.GetFileNameWithoutExtension(fileName) + counter + Path.GetExtension(fileName);
                else
                    break;
                counter += 1;
            }
            stream.Position = 0;
            using (var fs = File.Create(Path.Combine(PhysicalUploadedImagesPath, tempFileName)))
            {
                stream.CopyTo(fs);
            }

            var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
            /*Bitmap thumbnail = new Bitmap(60, 45);
            using (Graphics gr = Graphics.FromImage(image))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(thumbnail, new Rectangle(0, 0, 60, 45));
            }*/

            thumbnail.Save(Path.Combine(thumbnailPath, tempFileName));

            /*using(var fs = File.Create(Path.Combine(PhysicalUploadedImagesPath, tempFileName)))
            {
                stream.CopyTo(fs);
            }*/
            return Path.Combine(PhysicalUploadedImagesPath, tempFileName);
        }
        /// <summary>
        /// The document says to validate the files and that the MIME-type cannot be trusted.
        /// That's why I have these functions. These are copied from 
        /// http://stackoverflow.com/questions/210650/validate-image-from-file-in-c-sharp
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /*static bool IsValidImage(string filePath)
        {
            return File.Exists(filePath) && IsValidImage(new FileStream(filePath, FileMode.Open, FileAccess.Read));
        }*/
        /// <summary>
        /// The document says to validate the files and that the MIME-type cannot be trusted.
        /// That's why I have these functions. These are copied from 
        /// http://stackoverflow.com/questions/210650/validate-image-from-file-in-c-sharp
        /// 
        /// Checks the Stream to see if the first bytes of the the images are what we would expect them to be
        /// This is the best way to do it, and there isn't really any other way to really validate images
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public enum ImageFormat
        {
            Bmp,
            Jpeg,
            Gif,
            Tiff,
            Png,
            Unknown
        }
        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.Bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.Gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.Png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.Tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.Tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.Jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.Jpeg;

            return ImageFormat.Unknown;
        }
    }
}