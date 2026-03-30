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
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using Invoice;
using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Lease.InvoicePara;

public partial class Lease_ChargeAccount_UnionAgainPrint : BasePage
{
    public string baseInfo;  //基本信息
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindGridViewNull();
            BindData();
            ViewState["currentCount"] = 1;
            this.Form.DefaultButton = "btnQuery";
            string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_UnionBillreprint");
            DataSet ds = InvUnionReturnPO.UnionAgainPrintInfo(txtContractID.Text);
            ViewState["ds"] = ds;
            page(ds);
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
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string WhereSQL = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            WhereSQL = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        string WhereBO = "select A.CustCode,A.CustName,A.TaxCode,A.BankName,A.BankAcct,B.InvID,B.InvCode,B.InvDate,C.ContractCode" +
                             " from Customer A,InvoiceHeader B,Contract C ,ConShop" +
                             " where A.CustID = B.CustID and B.ContractID = C.ContractID and C.ContractID=ConShop.ContractID and C.BizMode = " + Contract.BIZ_MODE_UNIT + " and C.ContractCode = '" + txtContractID.Text + "' and B.InvStatus != '" + InvoiceHeader.INVSTATUS_CANCEL + "'" + WhereSQL + " order by InvCode Desc";

        ds =baseBo.QueryDataSet(WhereBO);
        ViewState["ds"] = ds;
        page(ds);
    }

    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ParaStatus = " + InvoiceJVPara.INVOICEJVPARA_STATUS_YES;
        Resultset rs = baseBO.Query(new InvoiceJVPara());
        foreach (InvoiceJVPara invPara in rs)
        {
            dropRptType.Items.Add(new ListItem(invPara.InvHeader, invPara.InvoiceJVParaID.ToString()));
        }
    }

    protected void page(DataSet tmepDS)
    {
        int spareRow = 0;
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();

        //BaseBO baseBO = new BaseBO();
        DataTable dt = tmepDS.Tables[0];
        //PagedDataSource pds = new PagedDataSource();
        //int ss = 0;
        pds.DataSource = dt.DefaultView;
        gvCharge.DataSource = pds;
        gvCharge.DataBind();
        spareRow = gvCharge.Rows.Count;
        for (int i = 0; i < gvCharge.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvCharge.DataSource = pds;
        gvCharge.DataBind();

        //gvCharge.EmptyDataText = "";
        //pds.AllowPaging = true;
        ////pds.PageSize = 10;
        //pds.DataSource = dt.DefaultView;

        //if (dt.Rows.Count < 1)
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    controlStatus(true);
        //}
        //else
        //{
            //pds.PageSize = 10;
            //pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCount"]) - 1;
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
            //gvCharge.DataSource = pds;
            //gvCharge.DataBind();

            //ss = gvCharge.Rows.Count;
            //for (int i = 0; i < pds.PageSize - ss; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //controlStatus(false);

        //}
        //gvCharge.DataSource = pds;
        //gvCharge.DataBind();

        //ViewState["count"] = pds.PageSize - ss;
        //ViewState["pdsPageSize"] = pds.PageSize;

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
    protected void gvCharge_SelectedIndexChanged(object sender, EventArgs e)
    {
        int billFlag = 0;
        if (cbType.Checked == true)
        {
            billFlag = 1;
        }
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        this.Response.Redirect("../../ReportM/RptLeaseInvJV.aspx?InvCode=" + Convert.ToInt32(gvCharge.SelectedRow.Cells[0].Text) + "&paraID=" + Convert.ToInt32(dropRptType.SelectedValue) + "&flag=" + 1 + "&billFlag=" + billFlag);
    }
    protected void gvCharge_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //GridView theGrid = sender as GridView;
        //int newPageIndex = 0;
        //if (-2 == e.NewPageIndex)
        //{
        //    TextBox txtNewPageIndex = null;
        //    GridViewRow pagerRow = theGrid.BottomPagerRow;
        //    if (null != pagerRow)
        //    {
        //        txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
        //    }
        //    if (null != txtNewPageIndex)
        //    {
        //        newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
        //    }
        //}
        //else
        //{ newPageIndex = e.NewPageIndex; }
        //newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        //newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        //theGrid.PageIndex = newPageIndex;

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
        //page(ViewState["WhereStr"].ToString());
        if (ViewState["ds"] != null)
        {
            page((DataSet)ViewState["ds"]);
        }

    }
}
