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

public partial class CryMiddle : System.Web.UI.Page
{
    //超级链接子报表路径
    string urlRptPath = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        urlRptPath = Request.QueryString["transferurl"];
        this.urla.Value = urlRptPath;
        //Response.Redirect(urlRptPath);
    }
}
