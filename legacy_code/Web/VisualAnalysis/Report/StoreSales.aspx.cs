using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StoreSales : System.Web.UI.Page
{
    public string baseInfo;
    public string strPath;
    protected void Page_Load(object sender, EventArgs e)
    {
      baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
        string locaID = Request.QueryString["LocaID"].ToString();
        strPath = "../Disktop.aspx?LocaId=" + locaID ;
    }
}
