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
using Base.Page;

public partial class ReportM_ReportCondition : BasePage
{
    

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        String s = "%26customcode=1010";
        this.Response.Redirect("ShowReport.aspx?ReportName=" + this.txtRptName.Text + s);
        //this.Response.Redirect("ShowReport.aspx?ReportName=" + this.txtRptName);
        //this.Response.Redirect("http://localhost:6164/Web/ReportM/CrystalReport.aspx?rptPathName=/Mi/Base/Customer.rpt");
    }
}
