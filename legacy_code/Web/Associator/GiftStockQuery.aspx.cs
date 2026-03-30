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

using Base.Biz;
using Base.DB;
using Base.Sys;
using Associator.Associator;
using Associator.Perform;
using Base.Page;

public partial class Associator_GiftStockQuery : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = "赠品库存信息";
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            BindGridView();
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        BindGridView();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) -1);
        BindGridView();
    }

    private void BindGridView()
    {
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        GiftStockQuery giftStockQuery = new GiftStockQuery();

        DataSet ds;

        BaseBO baseBO = new BaseBO();

        ds = baseBO.QueryDataSet(giftStockQuery);

        DataTable dt = ds.Tables[0];

        pds.DataSource = dt.DefaultView;

        GrdCust.EmptyDataText = "";
        //pds.AllowPaging = true;
        //pds.PageSize = 10;
        //lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
        //pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
        //if (pds.IsFirstPage)
        //{
        //    btnBack.Enabled = false;
        //    btnNext.Enabled = true;
        //}

        //if (pds.IsLastPage)
        //{
        //    btnBack.Enabled = true;
        //    btnNext.Enabled = false;
        //}

        //if (pds.IsFirstPage && pds.IsLastPage)
        //{
        //    btnBack.Enabled = false;
        //    btnNext.Enabled = false;
        //}

        //if (!pds.IsLastPage && !pds.IsFirstPage)
        //{
        //    btnBack.Enabled = true;
        //    btnNext.Enabled = true;
        //}

        this.GrdCust.DataSource = pds;
        this.GrdCust.DataBind();
        spareRow = GrdCust.Rows.Count;
        for (int i = 0; i < GrdCust.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        pds.DataSource = dt.DefaultView;
        GrdCust.DataSource = pds;
        GrdCust.DataBind();

        ClearGridViewSelect();
    }

    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in GrdCust.Rows)
        {
            if (gvr.Cells[0].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("GiftStock.aspx?GiftStock=" + GrdCust.SelectedRow.Cells[0].Text);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Response.Redirect("GiftStock.aspx?GiftStock=" + "0");
    }
    protected void GrdCust_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindGridView();
    }
}
