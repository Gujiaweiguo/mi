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

public partial class Lease_AuditingLease_SelectArea : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["invid"] = null;
            page();

        }
    }
    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        string str_sql = "select A.ContractID,A.CustID,A.ContractCode,A.RefID,A.BizMode,A.ConStartDate,A.ConEndDate,A.PenaltyItem,A.ChargeStartDate," +
                          " A.TradeID,A.ContractStatus,A.Penalty,A.Notice,A.AdditionalItem,A.EConURL,A.Note,A.CommOper,A.NorentDays,A.ContractTypeID from Contract A " +
                             " where A.ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR + " and A.BizMode = " + Contract.BIZ_MODE_Area + " and A.ContractID not in (select ContractID from InvoiceHeader where IsFirst = " + InvoiceHeader.ISFIRST_YES + ") order by A.ContractCode";

        DataTable dt = baseBO.QueryDataSet(str_sql).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCust.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCust.DataSource = pds;
            GrdCust.DataBind();
        }
        else
        {
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
        }

    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[5].Text = "";
            }
        }
    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*把点击的合同号存入Cookies*/
        HttpCookie cookies = new HttpCookie("Info");
        cookies.Expires = System.DateTime.Now.AddHours(1);
        cookies.Values.Add("conID", GrdCust.SelectedRow.Cells[0].Text);
        cookies.Values.Add("modify", "1");
        Response.AppendCookie(cookies);
        Response.Redirect("AuditingArea.aspx");
    }
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    page();
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
    //    page();
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
        page();
    }
}
