using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ClearSession : System.Web.UI.Page
{
    string strLink = string.Empty;
    string newURL = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        strLink = Request.QueryString["link"];
        newURL = Request.QueryString["newURL"];

        if (strLink != null && strLink == "true")
        {
            Session.RemoveAll();
            Response.Redirect(newURL);
        }

    }
}
