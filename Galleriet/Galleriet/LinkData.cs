using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Galleriet
{
    public class LinkData
    {
        public string Name
        {
            get;
            set;
        }
        public string Link
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