using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Galleriet
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Page.Session["gallery"] = new Gallery();
            Gallery g = (Gallery)Page.Session["gallery"];
            Largeimage.ImageUrl = g.GetImagePath(Request.QueryString["file"]);
            /*QueryStringLabel.Text = Request.Url.PathAndQuery + "\n";
            QueryStringLabel.Text += Request.Url.Query + "\n";
            QueryStringLabel.Text += Request.QueryString["file"] + "\n";
            QueryStringLabel.Text += Request.Path + "\n";
            QueryStringLabel.Text += Request.PathInfo + "\n";
            QueryStringLabel.Text += Request.RawUrl + "\n";
            QueryStringLabel.Text += Request.Url.OriginalString + "\n";
            QueryStringLabel.Text += Request.Url.LocalPath + "\n";
            QueryStringLabel.Text += Request.Url.Fragment + "\n";
            QueryStringLabel.Text += Request.Url.AbsoluteUri + "\n";
            QueryStringLabel.Text += Request.Url.AbsolutePath + "\n";
            QueryStringLabel.Text += Request.RawUrl + "\n";
            QueryStringLabel.Text += Request.PathInfo + "\n";
            QueryStringLabel.Text += Request.Path + "\n";
            QueryStringLabel.Text += Request.FilePath + "\n";
            QueryStringLabel.Text += AppDomain.CurrentDomain.GetData("APPBASE").ToString();*/
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            QueryStringLabel.Text += String.Join(" ",FileUpload1.FileName.Split('.'));
            QueryStringLabel.Text += FileUpload1.FileName.Split('.').Last()+"\n";
            QueryStringLabel.Text += System.IO.Path.GetExtension(FileUpload1.FileName);

            
            Gallery g = (Gallery)Page.Session["gallery"];
            FileUpload1.
            g.SaveImage(FileUpload1.PostedFile.InputStream, FileUpload1.PostedFile.FileName);
            Response.Redirect("?file=" + FileUpload1.FileName);
        }
    }
}