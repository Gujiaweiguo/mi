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

using Invoice.InvoiceH;
using Base.Biz;
using Base.DB;
using Base;
using BaseInfo.User;
using Base.Page;
using Invoice;

public partial class Invoice_Surplus_Surplus : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //绑定处理方式
            int[] surBalType = SurBal.GetSurBalType();
            int s = surBalType.Length;
            for (int i = 0; i < s; i++)
            {
                dropSurBalType.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter",SurBal.GetSurBalTypeDesc(surBalType[i])), surBalType[i].ToString()));
            }

            ViewState["currentCount"] = 1;
            ViewState["currentCountIH"] = 1;
            page();
            pageIH();
            txtSurBalAmtL.Attributes.Add("onkeydown", "textleave()");
            txtSurBalAmtL.Attributes.Add("onFocus", "GetSurBalAmt()");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Invoice_lblSurBal");
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if ((txtCustCode.Text != "") || (txtShopCode.Text != ""))
        {
            BaseBO baseBo = new BaseBO();
            string sql = "select A.CustID, A.CustName,B.ContractID,B.ContractCode,C.ShopID,C.ShopCode,C.ShopName,D.SurID,D.BillID,D.SurAmt,D.PayOutAmtSum,D.SurDate,D.SurExRate,D.SurCurTypeID from Customer A,Contract B,ConShop C,Surplus D" + 
                            " where A.CustID = B.CustID and B.ContractID = C.ContractID and D.CustID = A.CustID";
            if (txtCustCode.Text.Trim() != "")
            {
                sql = sql + " and A.CustCode = '" + txtCustCode.Text + "'";
            }
            if (txtShopCode.Text.Trim() != "")
            {
                sql = sql + " and C.ShopCode = '" + txtShopCode.Text + "'";
            }
            DataSet ds = baseBo.QueryDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
                txtBillID.Text = ds.Tables[0].Rows[0]["BillID"].ToString();
                txtPayOutAmtSum.Text = ds.Tables[0].Rows[0]["PayOutAmtSum"].ToString();
                txtSurAmtL.Text = ds.Tables[0].Rows[0]["SurAmt"].ToString();
                txtSurDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["SurDate"]).ToString("yyyy-MM-dd");
                ViewState["custID"] = ds.Tables[0].Rows[0]["CustID"].ToString();
                ViewState["contractID"] = ds.Tables[0].Rows[0]["ContractID"].ToString();
                ViewState["surID"] = ds.Tables[0].Rows[0]["SurID"].ToString();
                ViewState["surExRate"] = ds.Tables[0].Rows[0]["SurExRate"].ToString();
                ViewState["surCurTypeID"] = ds.Tables[0].Rows[0]["SurCurTypeID"].ToString();
                BindDropShopName(ds);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
        page();
        pageIH();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //处理金额 <= 余款金额 - 累计处理
        if (Convert.ToDecimal(txtSurBalAmtL.Text) <= (Convert.ToDecimal(txtSurAmtL.Text) - Convert.ToDecimal(txtPayOutAmtSum.Text)))
        {
            BaseTrans baseTrans = new BaseTrans();
            Surplus surplus = new Surplus();  //余款信息
            SurBal surBal = new SurBal();    //余款处理
            SurBalDel surBalDel = new SurBalDel();  //余款处理明细
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

            //余款处理
            surBal.SurBalID = BaseApp.GetSurBalID();
            surBal.CustID = Convert.ToInt32(ViewState["custID"]);
            surBal.CreateUserID = objSessionUser.UserID;
            surBal.CreateTime = DateTime.Now;
            surBal.OprDeptID = objSessionUser.DeptID;
            surBal.OprRoleID = objSessionUser.RoleID;
            surBal.SurBalType = Convert.ToInt32(dropSurBalType.SelectedValue);
            surBal.SurBalInvID = surBal.SurBalID;
            surBal.SurBalAmt = Convert.ToDecimal(txtSurBalAmtL.Text);
            surBal.SurBalAmtL = surBal.SurBalAmt * Convert.ToDecimal(ViewState["surExRate"]);
            surBal.SurBalDate = DateTime.Now;
            surBal.SurBalStatus = SurBal.SURBAL_UP_TO_SNUFF;

            //余款处理明细
            surBalDel.SurBalDelID = BaseApp.GetSurBalDelID();
            surBalDel.SurID = Convert.ToInt32(ViewState["surID"]);
            surBalDel.SurCurID = Convert.ToInt32(ViewState["surCurTypeID"]);
            surBalDel.SurExRate = Convert.ToDecimal(ViewState["surExRate"]);
            surBalDel.SurBalID = surBal.SurBalID;
            surBalDel.SurBalAmt = surBal.SurBalAmt;
            surBalDel.SurBalAmtL = surBal.SurBalAmtL;

            //修改余款信息中的累计余款金额
            baseTrans.WhereClause = "SurID = " + Convert.ToInt32(ViewState["surID"]);
            surplus.PayOutAmtSum = Convert.ToDecimal(txtPayOutAmtSum.Text) + surBal.SurBalAmt;

            baseTrans.BeginTrans();
            try
            {
                int l = baseTrans.Insert(surBal);  //插入余款处理
                int m = baseTrans.Insert(surBalDel); //插入余款处理明细
                int i = baseTrans.Update(surplus);   //修改余款信息中的累计余款金额
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            catch (Exception ex)
            {
                baseTrans.Rollback();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            baseTrans.Commit();
            SetControlsEmpty();
        }
        else if (Convert.ToDecimal(txtSurAmtL.Text) == (Convert.ToDecimal(txtPayOutAmtSum.Text)))
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_PayInPayOut") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PayOut_PayOutAmtBig") + "'", true);
        }
        page();
        pageIH();


    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
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
    //protected void btnBackT_Click(object sender, EventArgs e)
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
    protected void gdvwSurplus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        int surID = Convert.ToInt32(gdvwSurplus.SelectedRow.Cells[0].Text);
        string sql = "select A.CustID, A.CustName,B.ContractID,B.ContractCode,C.ShopID,C.ShopCode,C.ShopName,D.SurID,D.BillID,D.SurAmt,D.PayOutAmtSum,D.SurDate,D.SurExRate,D.SurCurTypeID from Customer A,Contract B,ConShop C,Surplus D" +
                           " where A.CustID = B.CustID and B.ContractID = C.ContractID and D.CustID = A.CustID and SurID = " + surID;
        DataSet ds = baseBo.QueryDataSet(sql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
            txtBillID.Text = ds.Tables[0].Rows[0]["BillID"].ToString();
            txtPayOutAmtSum.Text = ds.Tables[0].Rows[0]["PayOutAmtSum"].ToString();
            txtSurAmtL.Text = ds.Tables[0].Rows[0]["SurAmt"].ToString();
            txtSurDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["SurDate"]).ToString("yyyy-MM-dd");
            dropShopName.SelectedValue = ds.Tables[0].Rows[0]["ShopID"].ToString();
            ViewState["surID"] = ds.Tables[0].Rows[0]["SurID"].ToString();
            ViewState["custID"] = ds.Tables[0].Rows[0]["CustID"].ToString();
            ViewState["surExRate"] = ds.Tables[0].Rows[0]["SurExRate"].ToString();
            ViewState["surCurTypeID"] = ds.Tables[0].Rows[0]["SurCurTypeID"].ToString();
        }
        page();
        pageIH();
    }

    private void SetControlsEmpty()
    {
        txtBillID.Text = "";
        txtCustName.Text = "";
        txtPayOutAmtSum.Text = "";
        txtSurAmtL.Text = "";
        txtSurBalAmtL.Text = "";
        txtSurDate.Text = "";
    }

    private void BindDropShopName(DataSet myDS)
    {
        int count = myDS.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            //绑定商铺号
            dropShopName.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        }
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        baseBO.WhereClause = "CustID = '" + ViewState["custID"]+"'";
        DataSet ds = baseBO.QueryDataSet(new Surplus());
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
            pds.DataSource = dt.DefaultView;
        }
        else
        {
            //gdvwSurplus.EmptyDataText = "";
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
            //gdvwSurplus.DataSource = pds;
            //gdvwSurplus.DataBind();

            //ss = gdvwSurplus.Rows.Count;
            //for (int i = 0; i < pds.PageSize - ss; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            gdvwSurplus.DataSource = pds;
            gdvwSurplus.DataBind();
            ss = gdvwSurplus.Rows.Count;
            for (int i = 0; i < gdvwSurplus.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }


        }

        gdvwSurplus.DataSource = pds;
        gdvwSurplus.DataBind();
        for (int j = 0; j < gdvwSurplus.PageSize - ss; j++)
            gdvwSurplus.Rows[(gdvwSurplus.PageSize - 1) - j].Cells[4].Text = "";
    }

    protected void pageIH()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        string sql = "select InvID,InvCode,InvDate,InvActPayAmt-InvPaidAmt as NoInvPayAmtL from InvoiceHeader";
        if ((ViewState["contractID"] != "") && ViewState["contractID"] != null)
        {
            sql = sql + " where ContractID = " + ViewState["contractID"] + " and ( InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_NOINV + " or InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_HALFINV + ")";
        }
        else
        {
            sql = sql + " where ContractID = " + 0 + "and ( InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_NOINV + " or InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_HALFINV + ")";
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

}
