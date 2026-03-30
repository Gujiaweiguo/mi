using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Invoice;
using BaseInfo.User;
using Base.Biz;
using Base.Page;
using Base.DB;
using Lease.InvoicePara;

public partial class Lease_ChargeAccount_InterestAgainPrint : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ViewState["currentCount"] = 1;
            BindGridViewNull();
            BindData();
            this.Form.DefaultButton = "btnQuery";
            txtAccEndDate.Text = DateTime.Now.ToShortDateString();
            txtAccDate.Text = DateTime.Now.ToShortDateString();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_InterestPrint");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (txtContractCode.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value='请输入合同编码。'", true);
            BindGridViewNull();
        }
        else
        {
            DataSet ds = InvoiceInterestPO.GetAgainInterestInfo(txtContractCode.Text, txtAccDate.Text, txtAccEndDate.Text);
            ViewState["ds"] = ds;
            page(ds);
        }
        
    }

    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ParaStatus = " + InvoicePara.INVOICEPARA_STATUS_YES;
        Resultset rs = baseBO.Query(new InvoicePara());
        foreach (InvoicePara invPara in rs)
        {
            dropRptType.Items.Add(new ListItem(invPara.InvHeader, invPara.InvoiceParaID.ToString()));
        }
    }


    private void BindGridViewNull()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("InvID");
        dt.Columns.Add("InvCode");
        dt.Columns.Add("ContractCode");
        dt.Columns.Add("InvDate");
        dt.Columns.Add("CustCode");
        dt.Columns.Add("CustName");
        dt.Columns.Add("TaxCode");
        dt.Columns.Add("BankName");
        dt.Columns.Add("BankAcct");


        for (int i = 0; i < 10; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }

        gvCharge.DataSource = dt;
        gvCharge.DataBind();
    }

    protected void page(DataSet tmepDS)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = tmepDS.Tables[0];
        PagedDataSource pds = new PagedDataSource();
        int ss = 0;

        gvCharge.EmptyDataText = "";
        pds.AllowPaging = true;
        //pds.PageSize = 10;
        pds.DataSource = dt.DefaultView;

        if (dt.Rows.Count < 1)
        {
            for (int i = 0; i < 10; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            //controlStatus(true);
        }
        //else
        //{
        //    pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCount"]) - 1;
        //    if (pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = true;
        //    }

        //    if (pds.IsLastPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = false;
        //    }

        //    if (pds.IsFirstPage && pds.IsLastPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = false;
        //    }

        //    if (!pds.IsLastPage && !pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = true;
        //    }
        //    gvCharge.DataSource = pds;
        //    gvCharge.DataBind();

        //ss = gvCharge.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - ss; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    //controlStatus(false);

        //}
        //gvCharge.DataSource = pds;
        //gvCharge.DataBind();

        ////ViewState["count"] = pds.PageSize - ss;
        ////ViewState["pdsPageSize"] = pds.PageSize;

        gvCharge.DataSource = pds;
        gvCharge.DataBind();
        ss = gvCharge.Rows.Count;
        for (int i = 0; i < gvCharge.PageSize - ss; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvCharge.DataSource = pds;
        gvCharge.DataBind();


    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
    //    page((DataSet)ViewState["ds"]);
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
    //    page((DataSet)ViewState["ds"]);
    //}
    protected void gvCharge_SelectedIndexChanged(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        this.Response.Redirect("../../ReportM/RptInv/RptInvInterest.aspx?contractCode=" + txtContractCode.Text + "&PrtName=" + objSessionUser.UserID + "&paraID=" + Convert.ToInt32(dropRptType.SelectedValue));
    }
    protected void gvCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[9].Text = "";
            }
        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("../../ReportM/RptInv/RptInvInterest.aspx?paraID=" + Convert.ToInt32(dropRptType.SelectedValue));
    }

    protected void gvCharge_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        page((DataSet)ViewState["ds"]);
    }

}
