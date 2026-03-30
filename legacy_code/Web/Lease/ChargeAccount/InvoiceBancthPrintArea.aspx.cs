using System;
using System.Data;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Lease.InvoicePara;
using Invoice.InvoiceH;
using Base.Page;
using Lease.Contract;

public partial class Lease_ChargeAccount_InvoiceBancthPrintArea : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtStartDate.Text = DateTime.Now.ToShortDateString();
            txtEndDate.Text = DateTime.Now.ToShortDateString();
            BindDrop();
            BindNullGV();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Invoice_BacthPrintArea");
        }
    }

    private void BindDrop()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = baseBO.Query(new InvoiceAreaPara());
        foreach (InvoiceAreaPara invPara in rs)
        {
            dropRptType.Items.Add(new ListItem(invPara.InvHeader, invPara.InvoiceAreaParaID.ToString()));
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DateTime startDate = Convert.ToDateTime(txtStartDate.Text + " 00:00:00.000");
        DateTime endDate = Convert.ToDateTime(txtEndDate.Text + " 23:59:59.999");
        DataSet ds = InvoicePrintPO.GetInvoiceBacthPrint(startDate, endDate, Contract.BIZ_MODE_AdBoard);
        ViewState["resultDS"] = ds;
        BindGV(ds);
    }

    protected void gvChargeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells[0].Text == "&nbsp;")
        {
            e.Row.Cells[2].Text = "";
        }
    }

    protected void gvChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Response.Redirect("../../ReportM/RptAeraInv.aspx?paraID=" + Convert.ToInt32(dropRptType.SelectedValue) + "&flag=" + 1 + " &bacthID=" + gvChargeType.SelectedRow.Cells[1].Text);
    }

    private void BindNullGV()
    {
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        DataTable dt = new DataTable();
        dt.Columns.Add("CreateTime");
        dt.Columns.Add("BancthID");

        int count = dt.Rows.Count;
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvChargeType.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvChargeType.DataSource = pds;
            gvChargeType.DataBind();
        }
    }

    private void BindGV(DataSet ds)
    {
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        DataTable dt = ds.Tables[0];

        int count = dt.Rows.Count;
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvChargeType.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvChargeType.DataSource = pds;
            gvChargeType.DataBind();
        }
        gvChargeType.DataSource = pds;
        gvChargeType.DataBind();

        spareRow = gvChargeType.Rows.Count;
        for (int i = 0; i < gvChargeType.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvChargeType.DataSource = pds;
        gvChargeType.DataBind();
        //else
        //{
        //    gvChargeType.EmptyDataText = "";
        //    pds.AllowPaging = true;
        //    pds.PageSize = 10;
        //    lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
        //    pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
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

        //    this.gvChargeType.DataSource = pds;
        //    this.gvChargeType.DataBind();
        //    spareRow = gvChargeType.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    gvChargeType.DataSource = pds;
        //    gvChargeType.DataBind();
        //}

    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    BindGV((DataSet)ViewState["resultDS"]);
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    BindGV((DataSet)ViewState["resultDS"]);
    //}
    protected void GrdCust_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        if (ViewState["resultDS"] != null)
        {
            DataSet ds = (DataSet)ViewState["resultDS"];
            BindGV(ds);
        }
    }

}
