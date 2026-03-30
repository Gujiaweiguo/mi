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
using System.Text;

using Base.Biz;
using Base.DB;
using Lease.Customer;

public partial class Lease_AdContract_Customer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!this.IsPostBack)
        {
            string shortCustName1 = Request.QueryString["shortName"].ToString();
            BindGV(shortCustName1);
        }
    }

    private void BindGV(string custName)
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "CustShortName like '%" + custName + "%'";
        Resultset rs = baseBo.Query(new Customer());
        GVCust.DataSource = rs;
        GVCust.DataBind();
    }
    protected void GVCust_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void GVCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        CustID.Value = GVCust.SelectedRow.Cells[0].Text;
        CustCode.Value = GVCust.SelectedRow.Cells[1].Text;
        CustName.Value = GVCust.SelectedRow.Cells[2].Text;
        CustShortName.Value = GVCust.SelectedRow.Cells[3].Text;
        Page.RegisterStartupScript("", "<script>clickok();</script>");
    }
}
