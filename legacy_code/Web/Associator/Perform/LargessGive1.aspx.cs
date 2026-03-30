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

public partial class Associator_Perform_LargessGive : System.Web.UI.Page
{
    public string chkExtend;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            chkExtend = (String)GetGlobalResourceObject("BaseInfo", "Associator_chkExtend");
        }
    }
}
