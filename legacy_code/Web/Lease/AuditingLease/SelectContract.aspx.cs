using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

using Base.Biz;
using Lease.Contract;
using BaseInfo.User;
using Base.Page;
using Invoice;
using BaseInfo.authUser;


public partial class Lease_AuditingLease_SelectContract : BasePage
{
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
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

        string str_sql = "select A.ContractID,A.CustID,A.ContractCode,A.RefID,A.BizMode,convert(char(10),A.ConStartDate,120) as ConStartDate,A.ConEndDate,A.PenaltyItem,A.ChargeStartDate," +
                          " A.TradeID,A.ContractStatus,A.Penalty,A.Notice,A.AdditionalItem,A.EConURL,A.Note,A.CommOper,A.NorentDays,A.ContractTypeID,StoreName,custshortname from Contract A " +
                          "Inner join conshop on (conshop.contractid=A.contractid) Inner join store on (conshop.storeid=store.storeid) Inner join customer on (customer.custid=A.custid)" +
                             " where A.ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR + " and A.BizMode = " + Contract.BIZ_MODE_LEASE + " and A.ContractID not in (select ContractID from InvoiceHeader where IsFirst = " + InvoiceHeader.ISFIRST_YES + ")";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }

        str_sql +=" order by A.ContractCode";

        DataTable dt = baseBO.QueryDataSet(str_sql).Tables[0];
        pds.DataSource = dt.DefaultView;
        GrdCust.DataSource = pds;
        GrdCust.DataBind();
        spareRow = GrdCust.Rows.Count;
        for (int i = 0; i < GrdCust.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdCust.DataSource = pds;
        GrdCust.DataBind();
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
        page();
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[7].Text = "";
            }
        }
    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*把点击的合同号存入Cookies*/
        HttpCookie cookies = new HttpCookie("Info");
        cookies.Expires = System.DateTime.Now.AddHours(1);
        cookies.Values.Add("conID", GrdCust.SelectedRow.Cells[0].Text);
        Response.AppendCookie(cookies);
        Response.Redirect("AuditingLease.aspx");
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        page();
    }
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
    protected void btnBack_Click(object sender, EventArgs e)
    {
        page();
    }
}
