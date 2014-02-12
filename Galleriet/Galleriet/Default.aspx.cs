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
            QueryStringLabel.Text = Request.Url.PathAndQuery + " ";
            QueryStringLabel.Text += Request.Url.Query + " ";
            QueryStringLabel.Text += Request.Url.OriginalString + " ";
            QueryStringLabel.Text += Request.Url.LocalPath + " ";
            QueryStringLabel.Text += Request.Url.Fragment + " ";
            QueryStringLabel.Text += Request.Url.AbsoluteUri + " ";
            QueryStringLabel.Text += Request.Url.AbsolutePath + " ";
            QueryStringLabel.Text += Request.RawUrl + " ";
            QueryStringLabel.Text += Request.PathInfo + " ";
            QueryStringLabel.Text += Request.Path + " ";
            QueryStringLabel.Text += Request.FilePath + " ";
            
        }
    }
}