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
        public string FileName
        {
            get;
            set;
        }
        public string FullLink
        {
            get;
            set;
        }
        public string thumbLink
        {
            get;
            set;
        }
        public bool Display
        {
            get;
            set;
        }
    }
}