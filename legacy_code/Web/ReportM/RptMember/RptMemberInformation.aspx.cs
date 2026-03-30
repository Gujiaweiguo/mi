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

public partial class ReportM_RptMember_RptMemberInformation : System.Web.UI.Page
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //InitDDL();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_Title");
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {

    }
}
