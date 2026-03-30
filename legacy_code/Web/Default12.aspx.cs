using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Default12 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Master.PageTitle = "测试内容页";
            Master.StrSql = "Select usercode,username from Users";
            Master.PageFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            Master.PagePath = Request.FilePath;
        }
    }
}
