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
            /*foreach(string s in g.GetImageNames())
            {
                QueryStringLabel.Text += s + " ";
                QueryStringLabel.Text += Gallery.GetImagePath(s) + "\n";
                QueryStringLabel.Text += Gallery.GetImagePath(s, true) + "\n";
            }*/
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
                /*QueryStringLabel.Text += String.Join(" ", FileUpload1.FileName.Split('.')) + "\n";
                QueryStringLabel.Text += FileUpload1.FileName.Split('.').Last() + "\n";
                QueryStringLabel.Text += System.IO.Path.GetExtension(FileUpload1.FileName);*/

                Gallery g = Gallery;
                try
                {
                    string savedFileName = g.SaveImage(FileUpload1.PostedFile.InputStream, FileUpload1.PostedFile.FileName);
                    Successmessage = String.Format("Bilden '{0}' laddades upp utan problem!", savedFileName);
                    Response.Redirect(string.Format("?file={0}", savedFileName));
                }
                catch (ArgumentException ax)
                {
                    ModelState.AddModelError("", ax.Message);
                    /*CustomValidator cv = new CustomValidator();
                    cv.ErrorMessage = ax.Message;
                    cv.IsValid = false;
                    Page.Validators.Add(cv);*/
                }
            }
        }
        public IEnumerable<Galleriet.LinkData> repeater_GetData()
        {
            return Gallery.GetImagesPath();
        }
    }
}