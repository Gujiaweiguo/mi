using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Lease.Contract;
using BaseInfo.User;
using Base.Page;
using BaseInfo.authUser;

public partial class Lease_AuditingLease_SelectLease : BasePage
{
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!IsPostBack)
        {
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem"), "0"));
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID"), "1"));
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustShortName"), "2"));
            this.Form.DefaultButton = "btnQuery";
            ViewState["WhereStr"] = "a.ContractID =-1";
            page(ViewState["WhereStr"].ToString());
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "Load();", true);            
        }
    }
    #region
    //protected void page(string strWhere)
    //{
    //    BaseBO baseBO = new BaseBO();
    //    PagedDataSource pds = new PagedDataSource();
    //    int spareRow = 0;

    //    baseBO.WhereClause = strWhere;
    //    baseBO.OrderBy = "ContractCode Asc";

    //    DataTable dt = baseBO.QueryDataSet(new SelectContract()).Tables[0];

    //    for (int j = 0; j < dt.Rows.Count; j++)
    //    {
    //        string contractStatus = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(dt.Rows[j]["ContractStatus"])));
    //        dt.Rows[j]["ContractStatusName"] = contractStatus;
    //    }

    //    pds.DataSource = dt.DefaultView;

    //    GrdCust.EmptyDataText = "";
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

    //    this.GrdCust.DataSource = pds;
    //    this.GrdCust.DataBind();
    //    spareRow = GrdCust.Rows.Count;
    //    for (int i = 0; i < pds.PageSize - spareRow; i++)
    //    {
    //        dt.Rows.Add(dt.NewRow());
    //    }
    //    pds.DataSource = dt.DefaultView;
    //    GrdCust.DataSource = pds;
    //    GrdCust.DataBind();

    //    ClearGridViewSelect();
    //}
    #endregion
    protected void page(string strWhere)
    {
        int spareRow = 0;
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        baseBO.WhereClause = strWhere;
        baseBO.OrderBy = "ContractCode Asc";
        DataTable dt = baseBO.QueryDataSet(new SelectContract()).Tables[0];
        for (int j = 0; j < dt.Rows.Count; j++)
        {
            string contractStatus = (String)GetGlobalResourceObject("Parameter", Contract.GetContractTypeStatusDesc(Convert.ToInt32(dt.Rows[j]["ContractStatus"])));
            dt.Rows[j]["ContractStatusName"] = contractStatus;
        }
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
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text != "&nbsp;")
            {
                if (this.CheckDate(e.Row.Cells[6].Text.Trim().ToString()))
                //if (e.Row.Cells[5].Text.Trim().ToString() != "")
                {
                    string str3 = e.Row.Cells[6].Text.Trim().Substring(0, 10);
                    if (str3.LastIndexOf("0") == 9)
                        e.Row.Cells[6].Text = str3.Substring(0, 9);
                    else
                        e.Row.Cells[6].Text = str3;
                }
            }
            else
            {
                e.Row.Cells[8].Text = "";
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strDate"></param>
    /// <returns></returns>
    private bool CheckDate(string strDate)
    {
        try
        {
            DateTime t = DateTime.Parse(strDate);
           return true;
        }
        catch { return false; }
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
            if (Convert.ToInt32(GrdCust.SelectedRow.Cells[9].Text) == Contract.BIZ_MODE_LEASE)
            {
                Response.Redirect("AuditingLeasePalaver.aspx");
            }
            else if (Convert.ToInt32(GrdCust.SelectedRow.Cells[9].Text) == Contract.BIZ_MODE_UNIT)
            {
                Response.Redirect("AuditingUnion.aspx");
            }
        }
        else
        {
            if (Convert.ToInt32(GrdCust.SelectedRow.Cells[9].Text) == Contract.BIZ_MODE_LEASE)
            {
                Response.Redirect("../Lease.aspx?VoucherID=" + GrdCust.SelectedRow.Cells[0].Text + "&modify=" + 1);
            }
            else if (Convert.ToInt32(GrdCust.SelectedRow.Cells[9].Text) == Contract.BIZ_MODE_UNIT)
            {
                Response.Redirect("../LeaseConUnion/ConUnion.aspx?VoucherID=" + GrdCust.SelectedRow.Cells[0].Text + "&modify=" + 1);
            }
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        page(ViewState["WhereStr"].ToString());
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
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        page(ViewState["WhereStr"].ToString());
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string queryValue = cmbConSelect.SelectedValue;
        string whereStr = "";
        string contractStop = "";
        if (this.chkCheckAll.Checked == false)
        {
            if (chkContractStop.Checked)
            {
                //contractStop = " OR ContractStatus =" + Contract.CONTRACTSTATUS_TYPE_END;
                contractStop = "  and ContractStatus =" + Contract.CONTRACTSTATUS_TYPE_END;
            }
            else
            {
                contractStop = "  and ContractStatus <>" + Contract.CONTRACTSTATUS_TYPE_END;
            }

            if (rdbLease.Checked)
            {
                whereStr = " And a.BizMode = " + Contract.BIZ_MODE_LEASE + contractStop;
            }
            else if (rdbUnion.Checked)
            {
                whereStr = " And a.BizMode = " + Contract.BIZ_MODE_UNIT + contractStop;
            }
            switch (queryValue)
            {
                case "0":
                    ViewState["WhereStr"] = " StoreName like '%" + txtQueryMes.Text + "%' " + whereStr;
                    break;
                /*合同号*/
                case "1":
                    ViewState["WhereStr"] = " ContractCode like '%" + txtQueryMes.Text + "%'" + " " + whereStr;
                    break;
                /*商户简称*/
                case "2":
                    ViewState["WhereStr"] = " CustShortName like '%" + txtQueryMes.Text + "%'" + whereStr;
                    break;
                //case "2":
                //    ViewState["WhereStr"] = " BrandName like '%" + txtQueryMes.Text + "%'"  + whereStr;
                //    break;
                //case "3":
                //    ViewState["WhereStr"] = " RentArea=" + Convert.ToInt32(txtQueryMes.Text)  + whereStr;
                //    break;
                //case "4":
                //    ViewState["WhereStr"] = " ConStartDate='" + Convert.ToDateTime(txtSelectDate.Text) + "' "  + whereStr;
                //    break;
                //case "5":
                //    ViewState["WhereStr"] = " ConEndDate='" + Convert.ToDateTime(txtSelectDate.Text) + "' " + whereStr;
                //    break;
                default:
                    break;
            }
        }
        else//所有合同（终止合同、正常合同）
        {
            ViewState["WhereStr"] = "1=1 and ContractStatus=" + Contract.CONTRACTSTATUS_TYPE_END + " or ContractStatus=" + Contract.CONTRACTSTATUS_TYPE_INGEAR+"";
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            ViewState["WhereStr"] += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }

        //lblTotalNum.Text = "1";
        //lblCurrent.Text = "1";
        page(ViewState["WhereStr"].ToString());
    }
    protected void cmbConSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string queryValue = cmbConSelect.SelectedValue;
        //switch (queryValue)
        //{
        //    case "0":   //商户名称
        //        txtQueryMes.Visible = true;
        //        txtSelectDate.Visible = false;
        //        break;
        //    case "1":   //商户类型
        //        txtQueryMes.Visible = true;
        //        txtSelectDate.Visible = false;
        //        break;
        //    case "2":   //主营品牌
        //        txtQueryMes.Visible = true;
        //        txtSelectDate.Visible = false;
        //        break;
        //    case "3":   //商铺类型
        //        txtQueryMes.Visible = true;
        //        txtSelectDate.Visible = false;
        //        break;
        //    case "4":   //联系人


        //        txtQueryMes.Visible = false;
        //        txtSelectDate.Visible = true;
        //        break;
        //    case "5":  //建档人


        //        txtQueryMes.Visible = false;
        //        txtSelectDate.Visible = true;
        //        break;
        //    default:
        //        break;
        //}
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
                gvr.Cells[7].Text = "";
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
