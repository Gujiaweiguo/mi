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
using Base;
using Base.Biz;
using Base.DB;
using Associator.Perform;

public partial class Associator_CardLevelView : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblLeveLenactment");
            BindDataCardClass();
        }
    }

    private void BindDataCardClass()
    {
        PagedDataSource pds = new PagedDataSource();
        BaseBO baseBO = new BaseBO();
        DataTable dt = baseBO.QueryDataSet(new CardClass()).Tables[0];
        pds.DataSource = dt.DefaultView;
        this.gvCardClass.DataSource = pds;
        this.gvCardClass.DataBind();
        int spareRow = gvCardClass.Rows.Count;
        for (int i = 0; i < gvCardClass.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        pds.DataSource = dt.DefaultView;
        gvCardClass.DataSource = pds;
        gvCardClass.DataBind();
        ClearGridViewSelect();
    }

    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in gvCardClass.Rows)
        {
            if (gvr.Cells[0].Text == "&nbsp;")
            {
                gvr.Cells[6].Text = "";
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Associator/CardLevelInt.aspx");
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("../Associator/CardLevelInt.aspx?CardClassID=" + gvCardClass.SelectedRow.Cells[0].Text + "&modify=" + 1);
    }
    protected void gvCardClass_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindDataCardClass();
    }
}
