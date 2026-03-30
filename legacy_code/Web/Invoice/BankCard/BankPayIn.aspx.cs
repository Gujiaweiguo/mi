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
using Base.Sys;
using Invoice.BankCard;
using Invoice.InvoiceH;
using Base;
using BaseInfo.User;
using Base.Page;
public partial class Invoice_BankCard_BankPayIn : BasePage
{
    public string baseInfo;  //基本信息
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            for (int i = 1; i <= 12; i++)
            {
                cmbMonth.Items.Add(i.ToString());
            }
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_GenerateReceivables");
        }
        btnSave.Attributes.Add("onclick","return CheckData()");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        BankTransDetPayInID bankTransDetPayInID = new BankTransDetPayInID();
        PayIn payIn = new PayIn();
        BaseTrans baseTrans = new BaseTrans();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        //baseBO.WhereClause = "BankTransTime>='" + txtBingeDate.Text + "' and BankTransTime <='" + txtEndDate.Text + "' and PayInID=0" + " and ReconcType = " + BankTransDet.BANKTRANSDET_RECONCTYPE_SUCCEED_ANTITHESES;
        //rs = baseBO.Query(new BankTransDet());
        string startDate = txtBingeDate.Text;
        string endDate = txtEndDate.Text;
        DataSet ds = PayInPO.TotalBankAmtToPayInByDate(Convert.ToDateTime(startDate).ToString("yyyy-MM-dd"), Convert.ToDateTime(endDate).ToString("yyyy-MM-dd"));
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            //baseTrans.BeginTrans();
            for (int i = 0; i < count; i++ )
            {
                payIn.PayInID = BaseApp.GetPayInID();
                payIn.ShopID = Convert.ToInt32(ds.Tables[0].Rows[i]["ShopID"]);
                payIn.CreateUserID = sessionUser.UserID;
                payIn.CreateTime = Convert.ToDateTime(DateTime.Now);//DateTime.Now;
                payIn.OprDeptID = sessionUser.DeptID;
                payIn.OprRoleID = sessionUser.RoleID;
                payIn.PayInCode = payIn.PayInID.ToString();
                payIn.PayInPeriod = Convert.ToDateTime(DateTime.Now.Year + "-" + cmbMonth.Text + "-" + "1");
                payIn.PayInType = Convert.ToInt32(ds.Tables[0].Rows[i]["CardType"]);
                payIn.PayInStartDate = Convert.ToDateTime(txtBingeDate.Text);
                payIn.PayInEndDate = Convert.ToDateTime(txtEndDate.Text);
                payIn.PayInDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["BizDate"]);
                payIn.PayInAmt = Convert.ToDecimal(ds.Tables[0].Rows[i]["BankAmt"]);
                payIn.PayOutAmtSum = 0;
                payIn.PayInStatus = PayIn.PAYIN_NO_BALANCE_IN_HAND;
                payIn.PayInDataSource = PayIn.PAYINDATASOURCE_STSTEM;
                payIn.PaidAmt = Convert.ToDecimal(ds.Tables[0].Rows[i]["PaidAmt"]);
                payIn.CommRate = Convert.ToDecimal(ds.Tables[0].Rows[i]["CommRate"]);

                if (baseBO.Insert(payIn) < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                    //baseTrans.Rollback();
                    return;
                }

                //bankTransDetPayInID.PayInID = payIn.PayInID;
                //baseTrans.WhereClause = " ShopID = " + ds.Tables[0].Rows[i]["ShopID"] + " AND Convert(varchar(10),BankTransTime,120) = '" + ds.Tables[0].Rows[i]["BankTransTime"] + "'";
                //if (baseTrans.Update(bankTransDetPayInID) < 1)
                //{
                //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                //    baseTrans.Rollback();
                //    return;
                //}
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "'", true);
            //baseTrans.Commit();
        }
    }
}
