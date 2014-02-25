using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Galleriet
{
    public partial class Default : System.Web.UI.Page
    {
        public Gallery Gallery
        {
            get { return Session["gallery"] as Gallery; }
            set { Session["gallery"] = value; }
        }
        private string Successmessage
        {
            get { return Session["successmessage"] as string; }
            set { Session["successmessage"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Gallery = new Gallery();

            Gallery g = Gallery;
            //We don't want to send in filename if ?file= is null
            if (Request.QueryString["file"] != null)
            {
                Largeimage.ImageUrl = Gallery.GetImagePath(Request.QueryString["file"]);
                Largeimage.Visible = true;
            }
            if (Successmessage != null)
            {
                UploadPanel.Visible = true;
                UploadLabel.Text = Successmessage;
                Successmessage = null;
            }
        }
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Gallery g = Gallery;
                try
                {
                    if (FileUpload1.PostedFile.ContentLength < 5242880) //5 mb
                    {
                        string savedFileName = g.SaveImage(FileUpload1.FileContent, FileUpload1.FileName);
                        Successmessage = String.Format("Bilden '{0}' laddades upp utan problem!", savedFileName);
                        Response.Redirect(string.Format("?file={0}", savedFileName));
                    }
                    else
                        throw new ArgumentException("Filen är för stor. Maximal storlek är 5 mb.");
                }
                catch (ArgumentException ax)
                {
                    ModelState.AddModelError("", ax.Message);
                }
            }
        }
        public IEnumerable<Galleriet.LinkData> repeater_GetData()
        {
            return Gallery.GetImagesPath();
        }
    }
}