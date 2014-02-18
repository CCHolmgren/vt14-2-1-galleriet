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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Page.Session["gallery"] = new Gallery();

            Gallery g = (Gallery)Page.Session["gallery"];
            Largeimage.ImageUrl = Gallery.GetImagePath(Request.QueryString["file"]);
            Largeimage.Visible = true;
            foreach(string s in g.GetImageNames())
            {
                QueryStringLabel.Text += s + " ";
                QueryStringLabel.Text += Gallery.GetImagePath(s) + "\n";
                QueryStringLabel.Text += Gallery.GetImagePath(s, true) + "\n";
            }
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
            
            g.SaveImage(FileUpload1.PostedFile.InputStream, FileUpload1.PostedFile.FileName);
            Response.Redirect("?file=" + FileUpload1.FileName);
        }

        public IEnumerable<Galleriet.LinkData> repeater_GetData()
        {
            var di = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), @"Images"));
            var regex = new Regex("(.jpg|.gif|.png)", RegexOptions.IgnoreCase);
            return (from fi in di.GetFiles()
                    where regex.IsMatch(fi.Name)
                    select new LinkData
                    {
                        Name = fi.Name,
                        Link = fi.FullName,
                        thumbLink = Gallery.GetImagePath(fi.Name, true),
                        //Display = regex.IsMatch(fi.Name) ? true : false
                    }).AsEnumerable();
        }
    }
}