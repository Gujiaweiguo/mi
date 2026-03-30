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

/// <summary>
/// Author:hesijian
/// Date:2009-11-19
/// Content:Created
/// </summary>

public partial class Lease_AuditingAdAndGround_SelectAdAndGroundContract : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID"), "0"));
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustShortName"), "1"));
           
           
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labConStartDate"), "4"));
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labConEndDate"), "5"));
            this.Form.DefaultButton = "btnQuery";
            ViewState["WhereStr"] = "a.ContractID =-1";
            page(ViewState["WhereStr"].ToString());

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "Load();", true);
        }
    }

    protected void page(string strWhere)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        string Strsql = @"Select a.ContractID,a.CustID,ContractCode,a.RefID,a.BizMode,ConStartDate,ConEndDate,PenaltyItem,ChargeStartDate,
                        TradeID,ContractStatus,Penalty,Notice,AdditionalItem,EConURL,a.Note,a.CommOper,NorentDays,ContractTypeID,
                        '' as ContractStatusName, 
                        d.CustShortName
                        From contract a 
                        Left Join Customer d on a.CustID=d.CustID where " + strWhere + " order by ContractCode Asc ";
        //baseBO.WhereClause = strWhere;
        //baseBO.OrderBy = "ContractCode Asc";

        DataTable dt = baseBO.QueryDataSet(Strsql).Tables[0];

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            string contractStatus = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(dt.Rows[j]["ContractStatus"])));
            dt.Rows[j]["ContractStatusName"] = contractStatus;
        }

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

    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["modify"] == null)
        {
            /*把点击的合同号存入Cookies*/
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddHours(1);
            cookies.Values.Add("conID", GrdCust.SelectedRow.Cells[0].Text);
            Response.AppendCookie(cookies);

            //广告位合同
            if (Convert.ToInt32(GrdCust.SelectedRow.Cells[6].Text) == Contract.BIZ_MODE_AdBoard)
            {
                Response.Redirect("AuditingAdContract.aspx");
            }

            //场地合同
            else if (Convert.ToInt32(GrdCust.SelectedRow.Cells[6].Text) == Contract.BIZ_MODE_Area)
            {
                Response.Redirect("AuditingGroundContract.aspx");
            }
        }
        //else
        //{
        //    if (Convert.ToInt32(GrdCust.SelectedRow.Cells[6].Text) == Contract.BIZ_MODE_LEASE)
        //    {
        //        Response.Redirect("../Lease.aspx?VoucherID=" + GrdCust.SelectedRow.Cells[0].Text + "&modify=" + 1);
        //    }
        //    else if (Convert.ToInt32(GrdCust.SelectedRow.Cells[6].Text) == Contract.BIZ_MODE_UNIT)
        //    {
        //        Response.Redirect("../LeaseConUnion/ConUnion.aspx?VoucherID=" + GrdCust.SelectedRow.Cells[0].Text + "&modify=" + 1);
        //    }
        //    else if (Convert.ToInt32(GrdCust.SelectedRow.Cells[6].Text) == Contract.BIZ_MODE_AdBoard)
        //    {
        //        Response.Redirect("../AdContract/AdContract.aspx?VoucherID=" + GrdCust.SelectedRow.Cells[0].Text + "&modify=" + 1);
        //    }
        //}
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


    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string queryValue = cmbConSelect.SelectedValue;
        string whereStr = "";
        string contractStop = "";
        if (chkContractStop.Checked)
        {
            contractStop = " AND ContractStatus =" + Contract.CONTRACTSTATUS_TYPE_END;
        }
        if (!chkContractStop.Checked)
        {
            contractStop = " AND ContractStatus =" + Contract.CONTRACTSTATUS_TYPE_INGEAR;

        }

        if (rdbLease.Checked)
        {
            whereStr = "And a.BizMode = " + Contract.BIZ_MODE_Area + contractStop;
        }
        else if (rdbUnion.Checked)
        {
            whereStr = "And a.BizMode = " + Contract.BIZ_MODE_AdBoard + contractStop;
        }
        switch (queryValue)
        {
            /*合同号*/
            case "0":
                ViewState["WhereStr"] = "ContractCode like '%" + txtQueryMes.Text + "%'" + whereStr;
                break;
            /*客户简称*/
            case "1":
                ViewState["WhereStr"] = "CustShortName like '%" + txtQueryMes.Text + "%'" + whereStr;
                break;
           
            case "4":
                ViewState["WhereStr"] = "ConStartDate='" + Convert.ToDateTime(txtSelectDate.Text)+"' " + whereStr;
                break;
            case "5":
                ViewState["WhereStr"] = "ConEndDate='" + Convert.ToDateTime(txtSelectDate.Text)+"' " + whereStr;
                break;
            default:
                break;
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            ViewState["WhereStr"] += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        //lblTotalNum.Text = "1";
        //lblCurrent.Text = "1";
        page(ViewState["WhereStr"].ToString());
    }

    protected void cmbConSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        string queryValue = cmbConSelect.SelectedValue;
        switch (queryValue)
        {
            case "0":   //客户名称
                txtQueryMes.Visible = true;
                txtSelectDate.Visible = false;
                break;
            case "1":   //客户类型
                txtQueryMes.Visible = true;
                txtSelectDate.Visible = false;
                break;
           
            case "4":   //联系人
                txtQueryMes.Visible = false;
                txtSelectDate.Visible = true;
                break;
            case "5":  //建档人
                txtQueryMes.Visible = false;
                txtSelectDate.Visible = true;
                break;
            default:
                break;
        }
        //lblTotalNum.Text = "1";
        //lblCurrent.Text = "1";
        page("a.ContractID =-1");
    }

    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in GrdCust.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
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
