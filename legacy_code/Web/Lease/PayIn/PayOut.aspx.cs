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
using Lease.PayIn;
using BaseInfo.User;
using Base.Page;
using Invoice;
using Lease.ConShop;

public partial class Lease_PayIn_PayOut : BasePage
{
    public string baseInfo;
    public string msg1;
    public string msg2;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //绑定处理方式
            int[] payOutType = PayOut.GetPayOutType();
            int s = payOutType.Length;
            for (int i = 0; i < s; i++)
            {
                dropPayOutType.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter",PayOut.GetPayOutTypeDesc(payOutType[i])), payOutType[i].ToString()));
            }

            ViewState["currentCount"] = 1;
            ViewState["currentCountIH"] = 1;
            page();
            pageIH();
            txtPayOutAmt.Attributes.Add("onkeydown", "textleave()");
            //txtPayOutAmt.Attributes.Add("onFocus", "GetPayOutAmt()");
            msg1 = (String)GetGlobalResourceObject("BaseInfo", "PayOut_PayOutAmtZero");
            msg2 = (String)GetGlobalResourceObject("BaseInfo", "PayOut_PayOutAmtBig");
            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Inv_lblPayOut");
            txtPayOutAmt.Attributes.Add("onFocus", "getPayOutAmt()");
            txtShopCode.Attributes.Add("onclick", "ShowTree()");
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (txtPayInCode.Text.Trim() == "" && txtShopCode.Text.Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputQuery") + "'", true);
        }
        page();
        pageIH();
        SetControlsEmpty();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtPayInAmt.Text.Trim() == "")
            return;
        if (Convert.ToDecimal(txtPayInAmt.Text) >= Convert.ToDecimal(txtPayOutAmt.Text))
        {
            BaseTrans trans = new BaseTrans();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            try
            {
                trans.BeginTrans();
                int i = Invoice.InvoiceH.PayInPO.InvPayOutAndPayIn(Convert.ToInt32(ViewState["shopID"]), Convert.ToDecimal(txtPayOutAmt.Text), trans, sessionUser, 0);

                trans.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
                trans.Rollback();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PayOut_PayOutAmtBig") + "'", true);
        }

        ////处理金额 <= 代收款金额 - 累计返还金额
        //if (Convert.ToDecimal(txtPayOutAmt.Text) <= (Convert.ToDecimal(txtPayInAmt.Text) - Convert.ToDecimal(txtPayOutAmtSum.Text)))
        //{
        //    BaseTrans baseTrans = new BaseTrans();
        //    PayOut payOut = new PayOut();
        //    payOut.PayOutID = BaseApp.GetPayOutID();
        //    payOut.PayInID = Convert.ToInt32(ViewState["payInID"]);
        //    SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        //    payOut.CreateUserID = objSessionUser.UserID;
        //    payOut.OprDeptID = objSessionUser.DeptID;
        //    payOut.OprRoleID = objSessionUser.RoleID;
        //    payOut.PayOutAmt = Convert.ToDecimal(txtPayOutAmt.Text);
        //    payOut.PayOutDate = DateTime.Now;
        //    payOut.PayOutType = Convert.ToInt32(dropPayOutType.SelectedValue);
        //    payOut.PayOutStatus = PayOut.PAYOUTSTATUS_YES;

        //    //累计返还金额
        //    UpdatePayOutAmtSum payIn = new UpdatePayOutAmtSum();
        //    baseTrans.WhereClause = "PayInID = " + Convert.ToInt32(ViewState["payInID"]);
        //    payIn.PayOutAmtSum = Convert.ToDecimal(txtPayOutAmtSum.Text) + Convert.ToDecimal(txtPayOutAmt.Text);
        //    if (payIn.PayOutAmtSum == Convert.ToDecimal(txtPayInAmt.Text))  //累计还款 = 代收款金额


        //    {
        //        payIn.PayInStatus = PayIn.PAYINSTATRS_YESINV;
        //    }
        //    if ((payIn.PayOutAmtSum < Convert.ToDecimal(txtPayInAmt.Text)) && (payIn.PayOutAmtSum > 0))  //0<累计还款<代收款金额


        //    {
        //        payIn.PayInStatus = PayIn.PAYINSTATRS_HALFINV;
        //    }

        //    baseTrans.BeginTrans();
        //    try
        //    {
        //        int l = baseTrans.Insert(payOut);  //插入代收款返还信息表
        //        int i = baseTrans.Update(payIn);   //修改代收款信息中的累计返还金额
        //        baseTrans.Commit();
        //        //this.Response.Redirect("../../ReportM/RptInv/RptPayOut.aspx?PayOutID=" + payOut.PayOutID);
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        baseTrans.Rollback();
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        //    }
        //    baseTrans.Commit();
        //}
        //else if (Convert.ToDecimal(txtPayInAmt.Text) == (Convert.ToDecimal(txtPayOutAmtSum.Text)))  //代收款金额 = 累计返还金额
        //{
        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_PayInPayOut") + "'", true);
        //}
        //else
        //{
        //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PayOut_PayOutAmtBig") + "'", true);
        //}
        SetControlsEmpty();
        page();
        pageIH();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtPayInCode.Text = "";
        SetControlsEmpty();
        page();
        pageIH();
    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
    //    page();
    //    pageIH();
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
    //    page();
    //    pageIH();
    //}

    protected void page()
    {
        //BaseBO baseBO = new BaseBO();
        //DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        //if ((ViewState["shopID"] != "") && (ViewState["shopID"] != null))
        //{
        //    baseBO.WhereClause = " ShopID = '" + ViewState["shopID"] + "' and ";
        //}
        //baseBO.WhereClause += " 1 = 1 ";
        //if ((ViewState["payInCode"] != "") && (ViewState["payInCode"] != null))
        //{
        //    baseBO.WhereClause += " and PayInCode = '" + ViewState["payInCode"] + "'";
        //}
        //baseBO.WhereClause += " and PayInStatus in (" + PayIn.PAYINSTATRS_NOINV + "," + PayIn.PAYINSTATRS_HALFINV + ")";
        //if (((ViewState["shopID"] == "") || (ViewState["shopID"] == null)) && ((ViewState["payInCode"] == "") || (ViewState["payInCode"] == null)))
        //{
        //    baseBO.WhereClause = " 1 = 0";
        //}
        DataSet ds = Invoice.InvoiceH.PayOutPO.GetPayInByID(txtPayInCode.Text, allvalue.Value);
        
        DataTable dt = ds.Tables[0];
        int count = dt.Rows.Count;
        if (count > 0)
        {
            decimal payInAmtSum = Invoice.InvoiceH.PayInPO.GetPayInAmtSum(Convert.ToInt32(ds.Tables[0].Rows[0]["ShopID"]));
            txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
            txtContractCode.Text = ds.Tables[0].Rows[0]["ContractCode"].ToString();
            txtPayInAmt.Text = payInAmtSum.ToString();
            txtShopCodeF.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString();
            ViewState["shopID"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ShopID"]);
        }
        int ss = 0;
        pds.PageSize = 3;
        pds.DataSource = dt.DefaultView;
        if (pds.Count < 1)
        {
            for (int i = 0; i < 3; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            //btnBack.Enabled = false;
            //btnNext.Enabled = false;
            pds.DataSource = dt.DefaultView;
        }
        else
        {
            //gdvwPayIn.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 3;
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
            //gdvwPayIn.DataSource = pds;
            //gdvwPayIn.DataBind();

            //ss = gdvwPayIn.Rows.Count;
            //for (int i = 0; i < pds.PageSize - ss; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            gdvwPayIn.DataSource = pds;
            gdvwPayIn.DataBind();
            ss = gdvwPayIn.Rows.Count;
            for (int i = 0; i < gdvwPayIn.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            gdvwPayIn.DataSource = pds;
            gdvwPayIn.DataBind();


        }
        
        gdvwPayIn.DataSource = pds;
        gdvwPayIn.DataBind();
        for (int j = 0; j < gdvwPayIn.PageSize - ss; j++)
            gdvwPayIn.Rows[(gdvwPayIn.PageSize - 1) - j].Cells[4].Text = "";
    }

    protected void pageIH()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        string sql = "select InvID,InvCode,InvDate,InvActPayAmt-InvPaidAmt as NoInvPayAmtL from InvoiceHeader,Contract,ConShop WHERE InvoiceHeader.ContractID = Contract.ContractID AND ConShop.ContractID = Contract.ContractID"+
                        " and ( InvStatus = " + InvoiceHeader.INVSTATUS_NOINV + " or InvStatus = " + InvoiceHeader.INVSTATUS_HALFINV + ")";
        if (txtPayInCode.Text != "")
        {
            sql = sql + " AND Contract.ContractCode = '" + txtPayInCode.Text + "'"; 
        }
        //if (txtShopCode.Text != "")
        //{
        //    sql = sql + " AND ConShop.ShopCode = '" + txtShopCode.Text + "'"; 
        //}
        if (allvalue.Value != "")
        {
            sql = sql + " AND ConShop.ShopID = '" + allvalue.Value + "'";
        }
        if (txtPayInCode.Text == "" && txtShopCode.Text == "")
        {
            sql = sql + " AND Contract.ContractID = " + 0;
        }
        DataSet ds = baseBO.QueryDataSet(sql);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        int ss = 0;
        pds.PageSize = 4;
        pds.DataSource = dt.DefaultView;
        if (pds.Count < 1)
        {
            for (int i = 0; i < 4; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            //bt1Back.Enabled = false;
            //bt1Next.Enabled = false;
            pds.DataSource = dt.DefaultView;
        }
        else
        {
            //gdvwInvHeader.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 4;
            //pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCountIH"]) - 1;
            //if (pds.IsFirstPage)
            //{
            //    bt1Back.Enabled = false;
            //    bt1Next.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    bt1Back.Enabled = true;
            //    bt1Next.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    bt1Back.Enabled = false;
            //    bt1Next.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    bt1Back.Enabled = true;
            //    bt1Next.Enabled = true;
            //}
            //gdvwInvHeader.DataSource = pds;
            //gdvwInvHeader.DataBind();

            //ss = gdvwInvHeader.Rows.Count;
            //for (int i = 0; i < pds.PageSize - ss; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            gdvwInvHeader.DataSource = pds;
            gdvwInvHeader.DataBind();
            ss = gdvwInvHeader.Rows.Count;
            for (int i = 0; i < gdvwInvHeader.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            
        }
        gdvwInvHeader.DataSource = pds;
        gdvwInvHeader.DataBind();
    }
    //protected void btnBackT_Click1(object sender, EventArgs e)
    //{
    //    ViewState["currentCountTH"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCountTH"]) - 1);
    //    page();
    //    pageIH();
    //}
    //protected void btnNextT_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCountTH"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCountTH"]) + 1);
    //    page();
    //    pageIH();
    //}
    protected void gdvwPayIn_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["shopID"] = null;
        ViewState["payInCode"] = null;
        BaseBO baseBo = new BaseBO();
        int payInID = Convert.ToInt32(gdvwPayIn.SelectedRow.Cells[0].Text);
        string sql = "select A.CustName,B.ContractID,B.ContractCode,C.ShopID,C.ShopCode,D.PayInID,D.PayInCode,D.PayInAmt,D.PayInDate,D.PayOutAmtSum,E.InvCode,E.InvDate,E.InvPayAmtL from Customer A,Contract B,ConShop C,PayIn D,InvoiceHeader E" +
                           " where A.CustID = B.CustID and B.ContractID = C.ContractID and D.ShopID = C.ShopID and E.ContractID = B.ContractID and D.PayInID = " + payInID;
        DataSet ds = baseBo.QueryDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
            txtContractCode.Text = ds.Tables[0].Rows[0]["ContractCode"].ToString();
            txtPayInAmt.Text = ds.Tables[0].Rows[0]["PayInAmt"].ToString();
            txtShopCodeF.Text  = ds.Tables[0].Rows[0]["ShopCode"].ToString();
            ViewState["payInID"] = ds.Tables[0].Rows[0]["PayInID"].ToString();
        }
        if (txtPayInCode.Text != "")
        {
            ViewState["payInCode"] = gdvwPayIn.SelectedRow.Cells[1].Text;
        }
        if (txtShopCode.Text != "")
        {
            ViewState["shopID"] = gdvwPayIn.SelectedRow.Cells[5].Text;
        }
        page();
        pageIH();
    }

    private void SetControlsEmpty()
    {
        txtContractCode.Text = "";
        txtCustName.Text = "";
        txtPayInAmt.Text = "";
        txtPayOutAmt.Text = "";
        //txtShopCode.Text = "";
        txtShopCodeF.Text = "";
    }
    protected void gvShopBrand_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        pageIH();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            this.txtShopCode.Text = ds.Tables[0].Rows[0]["ShopName"].ToString();
        }
    }
}
