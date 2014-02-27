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
        static readonly Regex SanitizePath;
        static string PhysicalUploadedImagesPath;
        static string PhysicalUploadedThumbnailsPath;

        /// <summary>
        /// Initializes the strings and regex we use
        /// </summary>
        static Gallery()
        {
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            //The approved Extensions are jpg, gif and png
            ApprovedExtensions = new Regex(string.Format("{0}", "^.*\\.(gif|jpg|png)$"));
            PhysicalUploadedImagesPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "Images");
            PhysicalUploadedThumbnailsPath = Path.Combine(PhysicalUploadedImagesPath, "thumbnails");
        }
        /// <summary>
        /// Returns all filenames in the PhysicalUploadedImagesPath
        /// Generally not something that is necessary for the Gallery since it will include files that isn't images
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Returns the relative path to the image given it's filename
        /// </summary>
        /// <param name="name">Name of the image</param>
        /// <param name="thumb">true to return the path to the thumbnail</param>
        /// <returns></returns>
        public static string GetImagePath(string name, bool thumb = false)
        {
            if (name == null)
                throw new ArgumentException();

            if (thumb && ImageExsists(Path.Combine("thumbnails", name)))
                return Path.Combine("images", "thumbnails", name);
            else if (ImageExsists(name))
                return Path.Combine("images", name);
            else
                return "";
        }
        /// <summary>
        /// Used to populate the Repeater
        /// Chooses files from PhysicalUploadedImagesPath which are of extension jpg, gif or png
        /// Also fetches the link for the thumbnail with GetImagePath([mainLink], true)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Galleriet.LinkData> GetImagesPath()
        {
            var di = new DirectoryInfo(PhysicalUploadedImagesPath);
            //Only choose files that are of jpg gif or png
            var regex = new Regex("(.jpg|.gif|.png)", RegexOptions.IgnoreCase);
            //Then we return al the files that matches this criteria
            return (from fi in di.GetFiles()
                    where regex.IsMatch(fi.Name)
                    select new LinkData
                    {
                        FileName = fi.Name,
                        FullLink = fi.FullName,
                        thumbLink = Gallery.GetImagePath(fi.Name, true),
                        //I used a Display variable first, but the current way is much better
                        //Display = regex.IsMatch(fi.Name) ? true : false
                    }).AsEnumerable();
        }
        //Given a imagename, does it exist in the PhysicalUploadedImagesPath
        /// <summary>
        /// Given name, does it exist in PhysicalUploadedImagesPath
        /// You can use this with name that is already a path.combine to search deeper into the PhysicalUploadedImagesPath
        /// Such as to the thumbnails path
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ImageExsists(string name)
        {
            return File.Exists(Path.Combine(PhysicalUploadedImagesPath, name));
        }
        //This validates images given their Guid
        /// <summary>
        /// Validates images based on their guid
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public bool IsValidImage(Image image)
        {
            return image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid 
                || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid 
                || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid;
        }
        /// <summary>
        /// Saves an imagr from a stream with its fileName in PhysicalUploadedImagesPath
        /// If there is already a file with that name it adds a number to the end of the fileName until it finds a filename that isn't taken
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns>Path to the saved image location</returns>
        public string SaveImage(Stream stream, string fileName)
        {
            Stream thumbStream = stream;
            fileName = SanitizePath.Replace(fileName, "");
            string tempFileName = fileName;
            //Convert the stream to a byte array so that we can use GetImageFormat to validate the imageformat
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);

            //If the extension isn't allowed then it's a failure
            if (!ApprovedExtensions.IsMatch(fileName))
                throw new ArgumentException("Bilden måste vara av typen jpeg, gif eller png.");
            //The extension might be correct, but only if the extension and the bytes are correct can we safely use the image
            //This could, of course, be spoofed as well, but there isn't much more we can do
            if (GetImageFormat(bytes) == ImageFormat.Unknown)
                throw new ArgumentException("Bilden måste vara av typen jpeg, gif eller png. Kontrollera filformatet.");

            var image = System.Drawing.Image.FromStream(thumbStream);
            if (!IsValidImage(image))
                throw new ArgumentException("Bilden måste vara av typen jpeg, gif eller png.");

            //A loop to determine if the Filename already exists
            //If it does, adda counter to the filename until we reach a filename that isn't used
            int counter = 0;
            while (true)
            {
                if (File.Exists(Path.Combine(PhysicalUploadedThumbnailsPath, tempFileName)))
                    tempFileName = Path.GetFileNameWithoutExtension(fileName) + counter + Path.GetExtension(fileName);
                else
                    break;
                counter += 1;
            }

            //Walking through a stream moved the Position
            //If we don't reset it we end up with nothing to read since we are at the end of the stream
            stream.Position = 0;
            using (var fs = File.Create(Path.Combine(PhysicalUploadedImagesPath, tempFileName)))
            {
                stream.CopyTo(fs);
            }

            //Create and save the thumbnail image
            //This will decrease the quality quite a lot, but it's an easy way to do it
            var thumbnail = image.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);
            thumbnail.Save(Path.Combine(PhysicalUploadedThumbnailsPath, tempFileName));

            return tempFileName;
        }
        /// <summary>
        /// Represents the result of GetImageFormat
        /// </summary>
        public enum ImageFormat
        {
            Bmp,
            Jpeg,
            Gif,
            Tiff,
            Png,
            Unknown
        }
        /// <summary>
        /// Verifies the imageformat y using the bit signature of the different fileformats
        /// </summary>
        /// <param name="bytes">The image in byte format</param>
        /// <returns>Returns the result in form of the enum ImageFormat</returns>
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
            var jpeg3 = new byte[] { 255, 216, 255, 226 };
            var jpeg4 = new byte[] { 255, 216, 255 };

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.Gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.Png;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.Jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.Jpeg;

            if (jpeg3.SequenceEqual(bytes.Take(jpeg3.Length)))
                return ImageFormat.Jpeg;

            if (jpeg4.SequenceEqual(bytes.Take(jpeg4.Length)))
                return ImageFormat.Jpeg;

            return ImageFormat.Unknown;

            //These commented out functions are things that we do not want to check against in this application
            /*if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.Bmp;*/

            /*if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.Tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.Tiff;*/
        }
    }
}