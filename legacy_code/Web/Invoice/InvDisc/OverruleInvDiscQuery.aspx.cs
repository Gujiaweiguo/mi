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

using Base.DB;
using Base.Biz;
using RentableArea;
using Invoice.InvoiceH;
using Base;
using BaseInfo.User;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using Base.Page;
public partial class Invoice_InvDisc_OverruleInvDiscQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["Custumer"].Values["CustumerID"] != "")
            {
                page("a.DiscID=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]));
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "hidden()", true);
        }
    }

    protected void page(string discID)
    {

        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        baseBO.WhereClause = discID;
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        DataTable dt = baseBO.QueryDataSet(new InvDiscAuditing()).Tables[0];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["InvPayAmt"] = Convert.ToDecimal(dt.Rows[i]["InvPayAmt"]) + Convert.ToDecimal(dt.Rows[i]["InvAdjAmt"]) + Convert.ToDecimal(dt.Rows[i]["InvDiscAmt"]);
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdVewInvAdj.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdVewInvAdj.DataSource = pds;
            GrdVewInvAdj.DataBind();
        }
        else
        {
            pds.AllowPaging = true;
            pds.PageSize = 10;
            lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;

            if (pds.IsFirstPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }

            this.GrdVewInvAdj.DataSource = pds;
            this.GrdVewInvAdj.DataBind();
            spareRow = GrdVewInvAdj.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdVewInvAdj.DataSource = pds;
            GrdVewInvAdj.DataBind();
        }   
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {

    }
    protected void btnNext_Click(object sender, EventArgs e)
    {

    }
}
