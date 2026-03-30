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
using Base.Page;
using WorkFlow.WrkFlw;
using Base.DB;
using Base.Page;
public partial class WorkFlow_WrkFlwList : BasePage
{
    public string baseInfo;
    int numCount = 0;
    int numCount_two = 0;
    BaseBO baseBO = new BaseBO();
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridView1.DataSource = new BaseBO().Query(new WrkFlw());
        //GridView1.DataBind();
        page();
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "WrkFlw_QueryList");
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("WrkFlwUpdate.aspx?WrkFlwID=" + GridView1.SelectedRow.Cells[0].Text);
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[9].Text = "";
            }
        }
    }

    private void FillRows()
    {

        //int toLeft = GridView1.PageSize - numCount;
        //int numCols = GridView1.Rows[0].Cells.Count - 1;

        //for (int i = 0; i < toLeft; i++)
        //{
        //    GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
        //    for (int j = 0; j < numCols; j++)
        //    {
        //        TableCell cell = new TableCell();
        //        cell.Text = "&nbsp;";
        //        row.Cells.Add(cell);
        //    }
        //    GridView1.Controls[0].Controls.AddAt(numCount + 1 + i, row);
        //}
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    #region
    //protected void page()
    //{
    //    BaseBO baseBO = new BaseBO();
    //    Resultset rs = new Resultset();
    //    PagedDataSource pds = new PagedDataSource();
    //    int spareRow = 0;

    //    DataTable dt = baseBO.QueryDataSet(new WrkFlw()).Tables[0];
    //    int count = dt.Rows.Count;

    //    for (int j = 0; j < count; j++)
    //    {
    //        dt.Rows[j]["InitVoucherName"] = WrkFlw.GetInitVoucherDesc(Convert.ToInt32(dt.Rows[j]["InitVoucher"]));
    //    }

    //    for (int j = 0; j < count; j++)
    //    {
    //        dt.Rows[j]["IfTransitName"] = WrkFlw.GetIfTransitDesc(Convert.ToInt32(dt.Rows[j]["IfTransit"]));
    //    }

    //    for (int j = 0; j < count; j++)
    //    {
    //        dt.Rows[j]["WrkFlwStatusName"] = WrkFlw.GetWrkFlwStatusDesc(Convert.ToInt32(dt.Rows[j]["WrkFlwStatus"]));
    //    }
    //    pds.DataSource = dt.DefaultView;

    //    for (int i = 0; i < GridView1.PageSize; i++)
    //    {
    //        dt.Rows.Add(dt.NewRow());
    //    }
    //    pds.DataSource = dt.DefaultView;
    //    GridView1.DataSource = pds;
    //    GridView1.DataBind();
    //}
    //protected void page()
    //{
    //    BaseBO baseBO = new BaseBO();
    //    Resultset rs = new Resultset();
    //    PagedDataSource pds = new PagedDataSource();
    //    int spareRow = 0;
    //    baseBO.OrderBy = " wrkflwStatus DESC,wrkflwID ";
    //    DataTable dt = baseBO.QueryDataSet(new WrkFlw()).Tables[0];

    //    pds.DataSource = dt.DefaultView;

    //    if (pds.Count < 1)
    //    {
    //        for (int i = 0; i < GridView1.PageSize; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;
    //        GridView1.DataSource = pds;
    //        GridView1.DataBind();
    //    }
    //    else
    //    {
    //        pds.AllowPaging = true;
    //        pds.PageSize = 8;
    //        lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
    //        pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;

    //        if (pds.IsFirstPage)
    //        {
    //            btnBack.Enabled = false;
    //            btnNext.Enabled = true;
    //        }

    //        if (pds.IsLastPage)
    //        {
    //            btnBack.Enabled = true;
    //            btnNext.Enabled = false;
    //        }

    //        if (pds.IsFirstPage && pds.IsLastPage)
    //        {
    //            btnBack.Enabled = false;
    //            btnNext.Enabled = false;
    //        }

    //        if (!pds.IsLastPage && !pds.IsFirstPage)
    //        {
    //            btnBack.Enabled = true;
    //            btnNext.Enabled = true;
    //        }

    //        this.GridView1.DataSource = pds;
    //        this.GridView1.DataBind();
    //        spareRow = GridView1.Rows.Count;
    //        for (int i = 0; i < pds.PageSize - spareRow; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;
    //        GridView1.DataSource = pds;
    //        GridView1.DataBind();
    //    }

    //}
    #endregion
    protected void page()
    {
        int spareRow = 0;
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        //baseBO.WhereClause = "";
        //baseBO.OrderBy = "BrandName";
        DataSet ds = baseBO.QueryDataSet(new WrkFlw());
        dt = ds.Tables[0];
        pds.DataSource = dt.DefaultView;
        GridView1.DataSource = pds;
        GridView1.DataBind();
        spareRow = GridView1.Rows.Count;
        for (int i = 0; i < GridView1.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GridView1.DataSource = pds;
        GridView1.DataBind();
    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        page();
    }

}
