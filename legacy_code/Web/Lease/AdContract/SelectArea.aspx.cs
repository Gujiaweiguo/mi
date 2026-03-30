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
using Base;
using Lease;
using Lease.PotCustLicense;
using Lease.ConShop;
using RentableArea;
using Lease.PotBargain;
using Lease.Formula;
using Lease.Customer;
using Lease.Contract;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using BaseInfo.User;
using System.Text;
using Base.Page;
using Invoice;
using BaseInfo.authUser;

public partial class Lease_AdContract_SelectArea : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            page("ContractID=-1");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "Load();", true);
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string sql = "ContractStatus =" + Contract.CONTRACTSTATUS_TYPE_INGEAR + "And BizMode = " + Contract.BIZ_MODE_Area;
        if (txtContractCode.Text != "")
        {
            sql += " And ContractCode ='" + txtContractCode.Text.Trim() + "'";
        }
        if (txtRefID.Text != "")
        {
            sql += " And RefID ='" + txtRefID.Text.Trim() + "'";
        }
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        //if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        //{
        //    sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
        //                ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        //}
        page(sql);
    }

    protected void page(string strWhere)
    {
        ViewState["WhereStr"] = strWhere;
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = strWhere;

        DataTable dt = baseBO.QueryDataSet(new Contract()).Tables[0];

        pds.DataSource = dt.DefaultView;

        GrdCust.EmptyDataText = "";
        pds.AllowPaging = true;
        pds.PageSize = 10;
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
        for (int i = 0; i < pds.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        pds.DataSource = dt.DefaultView;
        GrdCust.DataSource = pds;
        GrdCust.DataBind();

        ClearGridViewSelect();
    }

    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    page(ViewState["WhereStr"].ToString());
    //}
    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        sNewStr = sNewStr + "...";
        return sNewStr;
    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    page(ViewState["WhereStr"].ToString());
    //}
    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in GrdCust.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[4].Text = "";
            }
        }
    }

    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("../AdContract/UpdateAreaContract.aspx?VoucherID=" + GrdCust.SelectedRow.Cells[0].Text + "&modify=" + 1);
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
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
        page(ViewState["WhereStr"].ToString());
    }
}
