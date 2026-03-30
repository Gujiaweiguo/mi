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
using System.IO;

public partial class FusionCharts_GetChartRentRateXml : System.Web.UI.Page
{
    public string objid = "";
    public string objtype = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ObjType"] !="")
            objtype=Request.QueryString["ObjType"];
        if (Request.QueryString["ObjID"] !="")
            objid = Request.QueryString["ObjID"];



        StreamWriter writer = new StreamWriter(System.Web.HttpContext.Current.Response.OutputStream, System.Text.Encoding.UTF8);

    }
}
