using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Galleriet
{
    public class LinkData
    {
        /// <summary>
        /// The Filename, My_image.jpg or the likes
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// The link to the large file
        /// </summary>
        public string FullLink
        {
            get;
            set;
        }
        /// <summary>
        /// The link to the thumbnail
        /// </summary>
        public string thumbLink
        {
            get;
            set;
        }
        [Obsolete("This field is unnecessary since we can filter on the fileextension,"
            + " so we do not need a bool for that")]
        public bool Display
        {
            get;
            set;
        }
    }
}