using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Invoice.InvoiceH;
using Base.Biz;
using Base.DB;
using Base;
using BaseInfo.User;
using Base.Page;
using Lease.Contract;

public partial class Invoice_DepBal_DepBal : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //绑定处理方式
            int[] depBalType = DepBal.GetDepBalType();
            int s = depBalType.Length;
            for (int i = 0; i < s; i++)
            {
                dropDepBalType.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter",DepBal.GetDepBalTypeDesc(depBalType[i])), depBalType[i].ToString()));
            }

            ViewState["currentCount"] = 1;
            ViewState["currentCountIH"] = 1;
            BindGridViewNull();
            pageIH();
            txtDepBalAmt.Attributes.Add("onkeydown", "textleave()");
            txtDepBalAmt.Attributes.Add("onFocus", "GetDepAmt()");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Inv_lblDepBal");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.SetControlsEmpty();
        if ((txtCustCode.Text != "") || (txtShopCode.Text != ""))
        {
            page();
            GetBizModeByContractID();
        }
        else
        {
            BindGridViewNull();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
        pageIH();
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        //处理金额 <= 押金金额 - 累计处理金额
        if (Convert.ToDecimal(txtDepBalAmt.Text) <= Convert.ToDecimal(txtDepAmt.Text) - Convert.ToDecimal(txtPayOutAmtSum.Text))
        {
            BaseTrans baseTrans = new BaseTrans();
            //押金返还处理
            DepBal depBal = new DepBal();
            depBal.CreateTime = DateTime.Now;
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            depBal.CreateUserID = objSessionUser.UserID;
            depBal.CustID = Convert.ToInt32(ViewState["custid"]);
            depBal.DepBalAmt = Convert.ToDecimal(txtDepBalAmt.Text);
            depBal.DepBalAmtL = Convert.ToDecimal(txtDepBalAmt.Text) * Convert.ToInt32(ViewState["exRate"]);
            depBal.DepBalCurID = Convert.ToInt32(ViewState["curTypeID"]);
            depBal.DepBalExRate = Convert.ToInt32(ViewState["exRate"]);
            depBal.DepBalID = BaseApp.GetDepBalID();
            depBal.DepBalType = Convert.ToInt32(dropDepBalType.SelectedValue);
            depBal.OprDeptID = objSessionUser.OprDeptID;
            depBal.OprRoleID = objSessionUser.OprRoleID;
            depBal.InvPayID = Convert.ToInt32(txtInvPayID.Text);

            //押金返还明细
            DepBalDel depBalDel = new DepBalDel();
            depBalDel.DepBalDetID = BaseApp.GetDepBalDetID();
            depBalDel.DepBalID = depBal.DepBalID;
            depBalDel.DepBalExRate = depBal.DepBalExRate;
            depBalDel.DepBalCurID = depBal.DepBalCurID;
            depBalDel.DepBalAmt = depBal.DepBalAmt;
            depBalDel.DepBalAmtL = depBal.DepBalAmt * depBal.DepBalExRate;
            depBalDel.InvPayDetID = Convert.ToInt32(ViewState["invPayDetID"]);

            //修改结算单付款明细中的累计返还金额和付款单明细状态
            InvoicePayDetail ivoPayDetail = new InvoicePayDetail();
            baseTrans.WhereClause = "InvPayDetID = " + Convert.ToInt32(ViewState["invPayDetID"]);
            ivoPayDetail.PayOutAmtSum = Convert.ToDecimal(txtPayOutAmtSum.Text) + Convert.ToDecimal(txtDepBalAmt.Text);
            if(ivoPayDetail.PayOutAmtSum == Convert.ToDecimal(txtDepAmt.Text))
            {
                ivoPayDetail.InvPayDetStatus = InvoicePayDetail.INVOICEPAYDETAIL_FULL_BACKING_OUT;
            }
            if ((ivoPayDetail.PayOutAmtSum < Convert.ToDecimal(txtDepAmt.Text)) && (ivoPayDetail.PayOutAmtSum > 0))
            {
                ivoPayDetail.InvPayDetStatus = InvoicePayDetail.INVOICEPAYDETAIL_PART_BACKING_OUT;
            }

            baseTrans.BeginTrans();
            try
            {
                int k = baseTrans.Insert(depBal);   //插入押金返还
                int l = baseTrans.Insert(depBalDel); //插入押金返还明细
                int m = baseTrans.Update(ivoPayDetail);   //修改结算单付款明细
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            catch (Exception ex)
            {
                baseTrans.Rollback();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            baseTrans.Commit();
        }
        else if (Convert.ToDecimal(txtDepAmt.Text) == (Convert.ToDecimal(txtPayOutAmtSum.Text)))  //押金金额 = 累计返还金额
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_PayInPayOut") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PayOut_PayOutAmtBig") + "'", true);
        }
        SetControlsEmpty();
        page();
        pageIH();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        //SetControlsEmpty();
        //page();
        Response.Redirect("~/Invoice/DepBal/DepBal.aspx");
    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
    //    page();
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
    //    page();
    //}
    //protected void btnBackT_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCountTH"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCountTH"]) - 1);
    //    page();
    //}
    //protected void btnNextT_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCountTH"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCountTH"]) + 1);
    //    page();
    //}
    protected void gdvwInvPayDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        int invPayDetID = Convert.ToInt32(gdvwInvPayDetail.SelectedRow.Cells[0].Text);
        txtInvPayID.Text = gdvwInvPayDetail.SelectedRow.Cells[1].Text;
        txtDepAmt.Text = gdvwInvPayDetail.SelectedRow.Cells[2].Text;
        txtPayOutAmtSum.Text = gdvwInvPayDetail.SelectedRow.Cells[3].Text;
        txtCustName.Text = gdvwInvPayDetail.SelectedRow.Cells[5].Text;
        txtContractCode.Text = gdvwInvPayDetail.SelectedRow.Cells[6].Text;
        dropShopName.SelectedValue = gdvwInvPayDetail.SelectedRow.Cells[7].Text;
        txtChargeTypeName.Text = Lease.PotBargain.ChargeType.GetChargeClassDesc(Convert.ToInt32(gdvwInvPayDetail.SelectedRow.Cells[8].Text));
        ViewState["custid"] = gdvwInvPayDetail.SelectedRow.Cells[9].Text;
        ViewState["contractID"] = gdvwInvPayDetail.SelectedRow.Cells[10].Text;
        ViewState["invPayDetID"] = invPayDetID;
        page();
        pageIH();
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        string sql = "select A.CustID, A.CustName,B.ContractID,B.ContractCode,C.ShopID,C.ShopCode,C.ShopName,D.InvPayDetID,D.InvPayID,D.ChargeTypeID,D.InvPaidAmt,D.PayOutAmtSum from Customer A,Contract B,ConShop C,InvoicePayDetail D,InvoicePay E" +
                        " where A.CustID = B.CustID and B.ContractID = C.ContractID and D.InvPayID = E.InvPayID and B.ContractID = E.ContractID and D.ChargeTypeID in (select F.ChargeTypeID from ChargeType F where F.ChargeClass = " +
                        Lease.PotBargain.ChargeType.CHARGECLASS_DEPOSIT + ") and D.InvPayDetStatus != " + Invoice.InvoiceH.InvoicePayDetail.INVOICEPAYDETAIL_CANCEL;
        if (txtCustCode.Text.Trim() != "")
        {
            //sql = sql + " and A.CustID = '" + txtCustCode.Text + "'";
            sql = sql + " and A.CustCode = '" + txtCustCode.Text + "'";
        }
        if (txtShopCode.Text.Trim() != "")
        {
            sql = sql + " and C.ShopCode = '" + txtShopCode.Text + "'";
        }
        DataSet ds = baseBO.QueryDataSet(sql);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
            //txtContractCode.Text = ds.Tables[0].Rows[0]["ContractCode"].ToString();
            //txtDepAmt.Text = ds.Tables[0].Rows[0]["InvPaidAmt"].ToString();
            //txtPayOutAmtSum.Text = ds.Tables[0].Rows[0]["PayOutAmtSum"].ToString();
            //txtInvPayID.Text = ds.Tables[0].Rows[0]["InvPayID"].ToString();
            //txtChargeTypeName.Text = Lease.PotBargain.ChargeType.GetChargeClassDesc(Convert.ToInt32(ds.Tables[0].Rows[0]["ChargeTypeID"].ToString()));

            ViewState["contractID"] = ds.Tables[0].Rows[0]["ContractID"].ToString();
            BindDropShopName(ds);
            dropShopName.SelectedValue = ds.Tables[0].Rows[0]["ShopID"].ToString();
        }
        else
        {
            dropShopName.Items.Add(new ListItem("",""));
            dropShopName.SelectedValue = "";
        }
        dt = ds.Tables[0];
        PagedDataSource pds = new PagedDataSource();
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
            //gdvwInvPayDetail.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 4;
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
            //gdvwInvPayDetail.DataSource = pds;
            //gdvwInvPayDetail.DataBind();

            //ss = gdvwInvPayDetail.Rows.Count;
            //for (int i = 0; i < pds.PageSize - ss; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            gdvwInvPayDetail.DataSource = pds;
            gdvwInvPayDetail.DataBind();
            ss = gdvwInvPayDetail.Rows.Count;
            for (int i = 0; i < gdvwInvPayDetail.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

        }
        gdvwInvPayDetail.DataSource = pds;
        gdvwInvPayDetail.DataBind();
        for (int j = 0; j < gdvwInvPayDetail.PageSize - ss; j++)
            gdvwInvPayDetail.Rows[(gdvwInvPayDetail.PageSize - 1) - j].Cells[4].Text = "";
    }

    protected void pageIH()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        string sql = "select A.InvID,A.InvCode,A.InvDate,B.InvActPayAmt-B.InvPaidAmt as NoInvPayAmtL from InvoiceHeader A,InvoiceDetail B";
        if ((ViewState["contractID"] != "") && (ViewState["contractID"] != null))
        {
            sql = sql + " where A.ContractID = " + ViewState["contractID"] + "and A.InvID = B.InvID and B.ChargeTypeID in (select F.ChargeTypeID from ChargeType F where F.ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_DEPOSIT + ") and ( A.InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_NOINV + " or A.InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_HALFINV + ")";
        }
        else
        {
            sql = sql + " where A.ContractID = " + 0 + "and A.InvID = B.InvID and B.ChargeTypeID in (select F.ChargeTypeID from ChargeType F where F.ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_DEPOSIT + ") and ( A.InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_NOINV + " or A.InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_HALFINV + ")";
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

    private void BindGridViewNull()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("InvPayDetID");
        dt.Columns.Add("InvPayID");
        dt.Columns.Add("InvPaidAmt");
        dt.Columns.Add("PayOutAmtSum");
        dt.Columns.Add("CustName");
        dt.Columns.Add("ContractCode");
        dt.Columns.Add("ShopID");
        dt.Columns.Add("ChargeTypeID");
        dt.Columns.Add("CustID");
        dt.Columns.Add("ContractID");

        for (int i = 0; i < 4; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }

        gdvwInvPayDetail.DataSource = dt;
        gdvwInvPayDetail.DataBind();

        for (int j = 0; j < 4; j++)
            gdvwInvPayDetail.Rows[j].Cells[4].Text = "";
    }

    private void SetControlsEmpty()
    {
        txtChargeTypeName.Text = "";
        txtContractCode.Text = "";
        txtCustName.Text = "";
        txtDepAmt.Text = "";
        txtDepBalAmt.Text = "";
        txtInvPayID.Text = "";
        txtPayOutAmtSum.Text = "";
        //try { this.dropShopName.SelectedItem.Text = "----"; }
        //catch {}
    }

    /// <summary>
    /// 根据经营方式获取汇率
    /// </summary>
    private void GetExRateByContractID()
    {
        string sql = "";
        int bizMode = GetBizModeByContractID();
        if (bizMode == Contract.BIZ_MODE_LEASE) //租赁
        {
            sql = "select A.ExRate,A.CurTypeID from CurExRate A,CurrencyType B,ConLease C where C.ContractID = B.ContractID and B.CurTypeID = C.CurTypeID" +
                            " and C.ContractID = '" + Convert.ToInt32(ViewState["contractID"]) + "'";
        }
        else if (bizMode == Contract.BIZ_MODE_UNIT)  //联营
        {
            sql = "select A.ExRate,A.CurTypeID from CurExRate A,CurrencyType B,ConUnion C where C.ContractID = B.ContractID and B.CurTypeID = C.CurTypeID" +
                           " and C.ContractID = '" + Convert.ToInt32(ViewState["contractID"]) + "'";
        }
        BaseBO baseBo = new BaseBO();
        DataSet ds = baseBo.QueryDataSet(sql);
        ViewState["exRate"] = ds.Tables[0].Rows[0]["ExRate"].ToString();
        ViewState["curTypeID"] = ds.Tables[0].Rows[0]["curTypeID"].ToString();
    }

    /// <summary>
    /// 根据合同号获取经营方式
    /// </summary>
    /// <returns></returns>
    private int GetBizModeByContractID()
    {
        int bizMode = 0;
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "ContractID = '" + Convert.ToInt32(ViewState["contractID"]) + "'";
        Resultset rs = baseBo.Query(new Contract());
        if (rs.Count == 1)
        {
            Contract contract = rs.Dequeue() as Contract;
            return bizMode = contract.BizMode;
        }
        return bizMode;
    }
    protected void gdvwInvPayDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[0].Text.Trim() == "&nbsp;")//如果当前行没有数据，选择按钮不显示出来
                e.Row.Cells[4].Text = "";
        }
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
