using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

using Invoice;
using BaseInfo.User;
using Base.Biz;
using Base.Page;
using Base;
using Base.DB;
using Lease;
using Lease.Contract;
using Lease.Union;

public partial class Lease_ChargeAccount_InvoiceInterest : BasePage
{
    public string baseInfo;
    public string AccountDate;
    public string AccountEndDate;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtEndDate.Text = DateTime.Now.ToShortDateString();
            setEnable(false);
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_InvoiceInterest");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh"); 
            AccountDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_AccountDate");
            AccountEndDate = (String)GetGlobalResourceObject("BaseInfo", "Prompt_AccountEndDate");
            if (RBtnBatch.Checked == true)
            {
                bt1Save.Enabled = true;
            }
            else
            {
                bt1Save.Enabled = false;
            }
            ViewState["flag"] = 0;
            QueryData("0");
            bt1Save.Attributes.Add("onclick", "return InputValidator(form1)");
        }
        
    }

    protected void btnCount_Click(object sender, EventArgs e)
    {
        Hashtable htb = new Hashtable();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        htb.Add("CreateUserID", objSessionUser.UserID);
        htb.Add("OprDeptID", objSessionUser.DeptID);
        htb.Add("OprRoleID", objSessionUser.RoleID);
        
        if(Convert.ToDateTime(txtEndDate.Text) <= DateTime.Now)
        {
            BaseBO baseBO = new BaseBO();
            string WhereClause = " AND Convert(varchar(10),InvDate,120) >= '" + txtAccDate.Text + "' AND Convert(varchar(10),InvDate,120) < '" + txtAccEndDate.Text + "'";
            if (RBtnSingle.Checked == true)
            {
                WhereClause += " AND Contract.ContractCode = '" + txtInvCode.Text + "'";
            }
            DataSet ds = InvoiceInterestPO.GetAccountInterestInvID(WhereClause);

            int count = ds.Tables[0].Rows.Count;
            int xCount = 0;
            int iFlag = 0;

            for (int i = 0; i < count; i++)
            {
                xCount++;
                ViewState["invID"] = ds.Tables[0].Rows[i]["InvID"].ToString();
                ViewState["contractID"] = ds.Tables[0].Rows[i]["ContractID"].ToString();
                //启征开始时间


                DateTime startDate;
                //获取合同类型
                int bizMode = GetBizModeByID(Convert.ToInt32(ds.Tables[0].Rows[i]["ContractID"]));

                if (txtStartDate.Text == "")
                {
                    startDate = GetIntDay(Convert.ToInt32(ds.Tables[0].Rows[i]["ContractID"]), Convert.ToInt32(ds.Tables[0].Rows[i]["InvID"]), bizMode);
                }
                else
                {
                    startDate = Convert.ToDateTime(txtStartDate.Text);
                }
                //DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
                //根据结算单号获取应结金额
                //decimal invActPayAmt = Invoice.InvoiceH.InvoiceDetailBo.SumInvoiceDetailPayAmtTotal(Convert.ToInt32(ViewState["invID"]));
                ////根据结算单号获取已结金额
                //decimal invPaidAmt = Invoice.InvoiceH.InvoiceDetailBo.SumInvoiceDetailPaidAmtTotal(Convert.ToInt32(ViewState["invID"]));
                ////判断应结金额是否等于已结金额
                //if (invActPayAmt != invPaidAmt)
                //{
                    int result = InvoiceInterestPO.InvInterestCount(Convert.ToInt32(ViewState["invID"]), Convert.ToDateTime(txtAccDate.Text), startDate, Convert.ToDateTime(txtEndDate.Text), Convert.ToInt32(ViewState["contractID"]), htb, bizMode);
                    if (result == InvoiceInterestPO.INTERESTAMT_YES)
                    {
                        iFlag = 1;
                    }
                    if (count == xCount)
                    {
                        if (iFlag == 1)
                        {
                            ViewState["flag"] = 1;
                            //生成成功
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "'", true);
                            //btnPrint.Enabled = true;
                            QueryData(txtInvCode.Text);
                        }
                        else
                        {
                            //未生成滞纳金
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Charge_NoInterest") + "'", true);
                            QueryData("0");
                        }
                    }

                //}
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_DateError") + "'", true);
        }
    }

    /// <summary>
    /// 获取滞纳金启征日期


    /// </summary>
    /// <param name="contractID">合同号</param>
    /// <param name="invID">结算单号</param>
    /// <returns></returns>
    private DateTime GetIntDay(int contractID,int invID,int bizMode)
    {
        BaseBO baseBO = new BaseBO();
        int day = 0;

        if (bizMode == Contract.BIZ_MODE_LEASE)  //租赁
        {
            baseBO.WhereClause = "ContractID = " + contractID;
            Resultset rs = baseBO.Query(new ConLease());
            
            if (rs.Count > 0)
            {
                ConLease conLease = rs.Dequeue() as ConLease;
                //int day = conLease.IntDay;
                day = conLease.IntDay;
            }
        }
        else if (bizMode == Contract.BIZ_MODE_UNIT) //联营
        {
            baseBO.WhereClause = "ContractID = " + contractID;
            Resultset rs = baseBO.Query(new ConUnion());
            if (rs.Count > 0)
            {
                ConUnion conUnion = rs.Dequeue() as ConUnion;
                day = conUnion.IntDay;
            }
        }

        baseBO.WhereClause = "";
        baseBO.WhereClause = "InvID = " + invID;
        Resultset invRs = baseBO.Query(new InvoiceHeader());
        InvoiceHeader invHeader = invRs.Dequeue() as InvoiceHeader;
        DateTime invDate = Convert.ToDateTime(invHeader.InvDate.ToShortDateString());
        //DateTime xDate = invDate.AddDays(-invDate.Day + 1);

        DateTime date = invDate.AddDays(day);

        return date;
    }

    /// <summary>
    /// 根据合同ID获取合同类型
    /// </summary>
    /// <param name="contractID">合同号</param>
    /// <returns></returns>
    private int GetBizModeByID(int contractID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ContractID = " + contractID;
        Resultset rs = baseBO.Query(new Contract());
        int bizMode = 0;
        if (rs.Count > 0)
        {
            Contract contract = rs.Dequeue() as Contract;
            bizMode = contract.BizMode;
        }
        return bizMode;
    }

    private void QueryData(string contractCode)
    {
        string WhereClause = "";
        if (RBtnSingle.Checked == true)
        {
            WhereClause = " AND Contract.ContractCode = '" + contractCode + "' and InvoiceInterest.CreateTime = '" + DateTime.Now.ToShortDateString() + "'";
        }
        else if (RBtnBatch.Checked == true)
        {
            if (Convert.ToInt32(ViewState["flag"]) == 1)
            {
                WhereClause = " AND InvoiceInterest.CreateTime = '" + DateTime.Now.ToShortDateString() + "'";
            }
            else
            {
                WhereClause = " AND Contract.ContractCode = '" + contractCode + "'";
            }
        }
        DataSet ds = InvoiceInterestPO.GetInterestInfo(WhereClause);
        int count = ds.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            //baseBo.WhereClause = "";
            //baseBo.WhereClause = "ChargeTypeID = " + Convert.ToInt32(ds.Tables[0].Rows[i]["Note"]);
            //DataSet tempDS = baseBo.QueryDataSet(new ChargeType());
            ds.Tables[0].Rows[i]["ChargeTypeName"] = ds.Tables[0].Rows[i]["Note"];//tempDS.Tables[0].Rows[0]["ChargeTypeName"].ToString();
        }

        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;


        DataTable dt = ds.Tables[0];

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvChargeType.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvChargeType.DataSource = pds;
            gvChargeType.DataBind();
        }
        //else
        //{
        //    gvChargeType.EmptyDataText = "";
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

        //    this.gvChargeType.DataSource = pds;
        //    this.gvChargeType.DataBind();
        //    spareRow = gvChargeType.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    gvChargeType.DataSource = pds;
        //    gvChargeType.DataBind();
        //}

        gvChargeType.DataSource = pds;
        gvChargeType.DataBind();
        spareRow = gvChargeType.Rows.Count;
        for (int i = 0; i < gvChargeType.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvChargeType.DataSource = pds;
        gvChargeType.DataBind();
    }

    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    QueryData(txtInvCode.Text);
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    QueryData(txtInvCode.Text);
    //}
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        int rentType = InvoiceDetail.RENTTYPE_NO_RENT;   //租金类别：非租金
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "LateInvID = " + Convert.ToInt32(ViewState["invID"]);
        Resultset rs = baseBO.Query(new InvoiceInterest());
        if (rs.Count > 0)
        {
            ArrayList interestAryList = new ArrayList();
            ArrayList interestIDAryList = new ArrayList();

            InvoiceHeader invoiceHer = new InvoiceHeader();

            Hashtable htb = new Hashtable();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

            Resultset conLeaseRS = GetConLease(Convert.ToInt32(ViewState["contractID"]));
            ConLease lease = conLeaseRS.Dequeue() as ConLease;

            //取结算币种对应的汇率
            decimal exRate = ChargeAccount.GetCurExRate(lease.CurTypeID);

            //结算主表
            invoiceHer.CustID = Convert.ToInt32(ViewState["coustID"]);     //客户ID
            invoiceHer.CreateUserID = Convert.ToInt32(objSessionUser.UserID);  //操作员


            invoiceHer.OprDeptID = Convert.ToInt32(objSessionUser.DeptID);  //操作员部门


            invoiceHer.OprRoleID = Convert.ToInt32(objSessionUser.RoleID);  //操作员角色


            invoiceHer.CustName = ViewState["custName"].ToString();   //客户名


            
            invoiceHer.ContractID = Convert.ToInt32(ViewState["contractID"]);  //合同号


            invoiceHer.PrintFlag = 0;    //打印标志
            invoiceHer.InvType = InvoiceHeader.INVTYPE_LEASE;         //结算类型
            invoiceHer.IsFirst = InvoiceHeader.ISFIRST_NO;  //是否首期
            invoiceHer.CurTypeID = lease.CurTypeID;  //币种代码
            invoiceHer.InvCurTypeID = lease.CurTypeID; //结算币种
            invoiceHer.InvExRate = exRate;   //结算汇率
            invoiceHer.InvPeriod = DateTime.Now.AddDays(-DateTime.Now.Day + 1); //结算单记账月
            invoiceHer.BancthID = BaseApp.GetBancthID().ToString();   //批次号


            
            foreach (InvoiceInterest invInterest in rs)
            {
                interestIDAryList.Add(invInterest.InterestID);
                InvoiceDetail invoiceDet = new InvoiceDetail();
                invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                invoiceDet.ChargeTypeID = Convert.ToInt32(invInterest.ChargeTypeID); //费用类别ID
                invoiceDet.Period = Convert.ToDateTime(invInterest.IntStartDate).AddDays(-invInterest.IntStartDate.Day + 1);//balanceMonth.AddDays(-balanceMonth.Day + 1);     //费用记账月


                invoiceDet.InvStartDate = Convert.ToDateTime(invInterest.IntStartDate);  //费用开始日期


                invoiceDet.InvEndDate = Convert.ToDateTime(invInterest.IntEndDate);      //费用结束日期
                invoiceDet.InvCurTypeID = lease.CurTypeID;  //结算币种
                invoiceDet.InvExRate = exRate;  //结算汇率
                invoiceDet.InvPayAmt = Convert.ToDecimal(invInterest.InterestAmt);  //费用应结金额
                invoiceDet.InvPayAmtL = Convert.ToDecimal(invInterest.InterestAmt) / exRate;  //费用应结本币金额 = 费用应结金额 / 结算汇率
                invoiceDet.InvActPayAmt = Convert.ToDecimal(invInterest.InterestAmt); //费用实际应结金额
                invoiceDet.InvActPayAmtL = Convert.ToDecimal(invInterest.InterestAmt) / exRate;  //费用实际应结本币金额
                invoiceDet.RentType = rentType;   //租金类别
                invoiceDet.Note = invInterest.Note;

                interestAryList.Add(invoiceDet);
            }

            BaseTrans trans = new BaseTrans();
            trans.BeginTrans();
            try
            {
                int invID = BaseApp.GetInvID();
                invoiceHer.InvID = invID;
                invoiceHer.InvCode = invID.ToString(); //结算单代码


                //结算单主表


                trans.Insert(invoiceHer);
                int count = interestAryList.Count;
                for (int i = 0; i < count; i++)
                {
                    InvoiceDetail invDet = (InvoiceDetail)interestAryList[i];
                    invDet.InvID = invID;
                    //结算单明细表
                    trans.Insert((BasePO)interestAryList[i]);
                }
                int idCount = interestIDAryList.Count;
                for (int j = 0; j < idCount; j++)
                {
                    string str_sql = "update invoiceInterest set InvCode = '" + invID + "' where InterestID = " + Convert.ToInt32(interestIDAryList[j]);
                    trans.ExecuteUpdate(str_sql);
                }
                this.Response.Redirect("../../ReportM/RptInv/RptInvInterest.aspx?InvCode=" + invID + "&PrtName=" + objSessionUser.UserID, false);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            trans.Commit();
        }
    }

    private static Resultset GetConLease(int contractID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ContractID = " + contractID;
        Resultset rs = baseBO.Query(new ConLease());
        return rs;
    }

    protected void RBtnBatch_CheckedChanged(object sender, EventArgs e)
    {
        setEnable(false);
    }

    private void setEnable(bool m)
    {
        txtInvCode.Enabled = m;
    }
    protected void RBtnSingle_CheckedChanged(object sender, EventArgs e)
    {
        setEnable(true);
    }

    protected void gvChargeType_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        QueryData("0");
    }

}
