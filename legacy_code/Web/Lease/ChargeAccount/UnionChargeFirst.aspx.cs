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

public partial class Lease_ChargeAccount_UnionChargeFirst : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);

        if (Session["invid"] != null)
        {
            this.btnOK.Enabled = true;
            this.BtnCel.Enabled = true;
            this.btnFirstCharge.Enabled = false;

            QueryData(Convert.ToInt32(Session["invid"]));
        }
        else
        {
            this.btnOK.Enabled = false;
            this.BtnCel.Enabled = false;
            this.btnFirstCharge.Enabled = true;
            QueryData(0);
        }
    }
    protected void btnFirstCharge_Click(object sender, EventArgs e)
    {
        int invCode;
        Hashtable htb = new Hashtable();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        htb.Add("CreateUserID", objSessionUser.UserID);
        htb.Add("OprDeptID", objSessionUser.DeptID);
        htb.Add("OprRoleID", objSessionUser.RoleID);

        string bancthID = BaseApp.GetInvoiceHeaderBancthID().ToString();

        //费用类型
        string chargeType = "";
        BaseBO basebo = new BaseBO();
        Resultset chargeTypeRs = basebo.Query(new ChargeType());
        foreach (ChargeType chgeType in chargeTypeRs)
        {
            chargeType += chgeType.ChargeTypeID + ",";
        }
        ArrayList bfoChgNoAryList = new ArrayList();

        //合同信息
        basebo.WhereClause = "";
        basebo.WhereClause = "ContractID = " + Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        Resultset conRS = basebo.Query(new Contract());
        Contract contract = conRS.Dequeue() as Contract;

        int result = Invoice.UnionChargeCount.AccountCharge(Convert.ToInt32(Request.Cookies["Info"].Values["conID"]), contract.ChargeStartDate,contract.ConEndDate, 1, chargeType, htb, bancthID, out invCode, out bfoChgNoAryList);
        if (result == Invoice.ChargeAccount.PROMT_SUCCED) //成功
        {
            QueryData(invCode);
            Session["invid"] = invCode;
            ViewState["id"] = invCode;
            this.btnOK.Enabled = true;
            this.BtnCel.Enabled = true;
            this.btnFirstCharge.Enabled = false;
        }
        if (result == Invoice.ChargeAccount.PROMT_FIRST_CHARGE_YES)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Promt_First_Charge_Yes") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_FIRST_CHARGE_NO) //首期费用未生成
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_First_Charge_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_NO) //合同无效
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_INFO_NO) //未有合同信息
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_Info_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_DATE_NO) //结算时间段完全不在合同时间范围内
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contact_Date_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_BEFORE_CHARGE_NO) //前期费用未生成
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Before_Charge_No") + "'", true);
        }
        if (result == Invoice.ChargeAccount.PROMT_EXRATE_NO) //汇率有误
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_lblExRateError") + "'", true);
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int invCode = Convert.ToInt32(ViewState["id"]);
        this.Response.Redirect("../../ReportM/RptLeaseInvJV.aspx?InvCode=" + invCode + "&flag =" + 0);
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        string celReason = (String)GetGlobalResourceObject("BaseInfo", "Promt_First_Charge_Cancel");

        //结算单取消主表
        string str_sql = "insert into InvCancel values (" + Convert.ToInt32(ViewState["id"]) + "," + Convert.ToInt32(ViewState["id"]) + ",'" + celReason + "'," + Invoice.InvoiceH.InvCancel.INVCANCEL_UPDATE_LEASE_STATUS + ",'" + celReason + "')";

        //修改结算主表算单状态和是否首期
        string str_sql_invoiceH = "update InvoiceHeader set IsFirst = " + Invoice.InvoiceHeader.ISFIRST_NO +
                                    " , InvStatus = " + Invoice.InvoiceHeader.INVSTATUS_CEL +
                                    " where InvID = " + Convert.ToInt32(ViewState["id"]);
        //将对应的结算明细导入结算取消表中               
        string str_sql_invoiceC = "insert into InvoiceCancel select InvDetailID,ChargeTypeID,InvID,Period,InvStartDate,InvEndDate,InvCurTypeID,InvExRate,InvPayAmt,InvPayAmtL,InvAdjAmt,InvAdjAmtL," +
                                    "InvDiscAmt,InvDiscAmtL,InvChgAmt,InvChgAmtL,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvType,InvDetStatus,Note,RentType " +
                                    " from InvoiceDetail where InvID = " + Convert.ToInt32(ViewState["id"]);

        //将对应的费用表中的结算单号至为初始状态
        string str_sql_Charge = "update Charge set InvCode = '" + 0 + "' where InvCode = " + Convert.ToInt32(ViewState["id"]);

        //将对应的其它费用表中的结算单号至为初始状态
        string str_sql_OtherChargeH = "update OtherChargeH set InvCode = '" + 0 + "' where InvCode = " + Convert.ToInt32(ViewState["id"]);

        //将对应的结算单明细表数据删除
        string str_sql_invoiceD = "delete from InvoiceDetail where InvID = " + Convert.ToInt32(ViewState["id"]);

        BaseTrans baseTrans = new BaseTrans();
        baseTrans.BeginTrans();
        try
        {
            baseTrans.ExecuteUpdate(str_sql);
            baseTrans.ExecuteUpdate(str_sql_invoiceH);
            baseTrans.ExecuteUpdate(str_sql_invoiceC);
            baseTrans.ExecuteUpdate(str_sql_invoiceD);
            baseTrans.ExecuteUpdate(str_sql_Charge);
            baseTrans.ExecuteUpdate(str_sql_OtherChargeH);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            throw ex;
        }
        baseTrans.Commit();
        QueryData(0);
    }

    private void QueryData(int invCode)
    {
        string sql = "select A.ChargeTypeID,A.InvStartDate,A.InvEndDate,A.InvActPayAmtL,'' as ChargeTypeName " +
                        " from invoicedetail A , ChargeType B " +
                        " where A.InvID = " + invCode + " and A.ChargeTypeID = B.ChargeTypeID and InvPayAmt > " + 0;
        BaseBO baseBo = new BaseBO();
        DataSet ds = baseBo.QueryDataSet(sql);
        int count = ds.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "ChargeTypeID = " + ds.Tables[0].Rows[i]["ChargeTypeID"];
            DataSet tempDS = baseBo.QueryDataSet(new ChargeType());
            ds.Tables[0].Rows[i]["ChargeTypeName"] = tempDS.Tables[0].Rows[0]["ChargeTypeName"].ToString();
        }

        //补空行


        int ss = 15 - count;
        for (int i = 0; i < ss; i++)
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
        }

        GVCust.DataSource = ds;
        GVCust.DataBind();
    }
}
