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

using Base.Page;
using Base.Biz;
using Invoice;
using Base.Sys;
using Base;
using BaseInfo.User;
using System.Text;
using Lease.PayIn;

public partial class Invoice_Surplus_Builder_Auditing : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_Builder_Auditing");

            cmbYear.Items.Clear();
            for (int i = Convert.ToInt32(DateTime.Now.Year) - 10; i <= Convert.ToInt32(DateTime.Now.Year) + 20; i++)
            {
                cmbYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            cmbYear.SelectedIndex = 10;
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        DataSet dsLastYear = new DataSet();
        dsLastYear = baseBO.QueryDataSet("Select ContractCode,Balance,BalanceL From BuilderAuditing Order By ContractCode,Status");

        if (dsLastYear.Tables[0].Rows.Count <= 0)
        {
            InsertBuilderAuditing(0);
        }
        else
        {
            InsertBuilderAuditing(1);
        }
    }

    private void InsertBuilderAuditing(int _status)
    {
        BuilderAuditing builderAuditing = new BuilderAuditing();
        DataSet dscontract = new DataSet();
        DataSet ds = new DataSet();
        BaseTrans baseTrans = new BaseTrans();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseBO baseBO = new BaseBO();
        DataSet dsLastYear = new DataSet();
        StringBuilder strSql = new StringBuilder();
        
        string contractCode = "";
        decimal _balanceSum = 0;
        decimal _balanceSumL = 0;

        int sYear = Convert.ToInt32(cmbYear.SelectedItem.Text) -1 ;

        if (baseBO.ExecuteUpdate("Delete BuilderAuditing Where InvPeriod >= '" + cmbYear.SelectedItem.Text + "-01-01' And InvPeriod <= '" + cmbYear.SelectedItem.Text + "-12-31'") == -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
            return;
        }

        dscontract = baseBO.QueryDataSet("Select ContractCode From Contract Where ContractStatus <> 0 And  ContractStatus <> 1 Order By ContractCode");

        if (dscontract.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dscontract.Tables[0].Rows.Count; i++)
            {

                if (_status == 1)
                {
                    dsLastYear = baseBO.QueryDataSet("Select Top 1 Balance,BalanceL,InvPeriod From BuilderAuditing  Where ContractCode = '" + dscontract.Tables[0].Rows[i]["ContractCode"].ToString().Trim() + "' And InvPeriod >= '" + sYear + "-01-01' And InvPeriod <= '" + sYear + "-12-31' Order By Status Desc");

                    if (dsLastYear.Tables[0].Rows.Count > 0)
                    {
                        _balanceSum = Convert.ToDecimal(dsLastYear.Tables[0].Rows[0]["Balance"]);
                        _balanceSumL = Convert.ToDecimal(dsLastYear.Tables[0].Rows[0]["BalanceL"]);
                    }
                }
                strSql.Remove(0, strSql.Length);
                strSql.Append(" Select CustID,ContractID,ChargeTypeName,CustName,ContractCode,ShopCode,ShopName,CustCode,InvPeriod,IsFirst,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,Status,ChargeTypeID,InvDetailID,InvPayID,PayInID,PayOutID,InvID,InvAdjID ");
                strSql.Append(" From(     ");
                strSql.Append(" Select InvoiceHeader.CustID,InvoiceHeader.ContractID,ChargeTypeName,Customer.CustName,ContractCode,ShopCode,ShopName,CustCode,InvoiceHeader.InvPeriod,InvoiceHeader.IsFirst,  " );
                        strSql.Append(" (InvoiceDetail.InvActPayAmt) As InvActPayAmt,  " );
                        strSql.Append(" (InvoiceDetail.InvActPayAmtL) As InvActPayAmtL,     " );
                        strSql.Append(" 0 As InvPaidAmt,0 As InvPaidAmtL,0 As Status,InvoiceDetail.ChargeTypeID,InvoiceDetail.InvDetailID,'' As InvPayID, " );
                        strSql.Append(" '' As PayInID,'' As PayOutID,InvoiceHeader.InvID,'' As InvAdjID      " );
                        strSql.Append(" From  InvoiceHeader      " );
                        strSql.Append(" Inner Join  Contract On InvoiceHeader.ContractID= Contract.ContractID     " );
                        strSql.Append(" Inner Join InvoiceDetail  On InvoiceHeader.InvID= InvoiceDetail.InvID     " );
                        strSql.Append(" Inner Join ChargeType On InvoiceDetail.ChargeTypeID= ChargeType.ChargeTypeID     " );
                        strSql.Append(" Inner Join ConShop On ConShop.ContractID= Contract.ContractID     " );
                        strSql.Append(" Inner Join Customer On InvoiceHeader.CustID= Customer.CustID     " );
                        strSql.Append(" Where InvoiceHeader.InvStatus <> 4 And InvoiceHeader.InvPeriod >= '" + cmbYear.SelectedItem.Text + "-01-01' And InvoiceHeader.InvPeriod <= '" + cmbYear.SelectedItem.Text + "-12-31' And ContractCode = '" + dscontract.Tables[0].Rows[i]["ContractCode"].ToString().Trim() + "'" );
                        strSql.Append(" Union All  " );
                        strSql.Append(" Select Customer.CustID,Contract.ContractID,Case PayOutType When 1 Then '返还代收款' End As ChargeTypeName,CustName,ContractCode,ShopCode,ShopName,CustCode, " );
                        strSql.Append(" PayInDate As InvPeriod,0 As IsFirst,PayInAmt As InvActPayAmt,PayInAmt As InvActPayAmtL,0 As InvPaidAmt,0 As InvPaidAmtL, " );
                        strSql.Append(" 0 As Status,'' As ChargeTypeID,'' As InvDetailID,'' As InvPayID,'' As PayInID,PayOutID,'' As InvID,'' As InvAdjID ");
                        strSql.Append(" From PayOut " );
                        strSql.Append(" Inner Join PayIn On PayOut.PayInID = PayIn.PayInID " );
                        strSql.Append(" Inner Join Conshop On PayIn.ShopID = Conshop.ShopID " );
                        strSql.Append(" Inner Join Contract On Conshop.ContractID = Contract.ContractID " );
                        strSql.Append(" Inner Join Customer On Contract.CustID = Customer.CustID " );
                        strSql.Append(" Where PayOutStatus = 1 And PayOutType = 1 And PayInDate >= '" + cmbYear.SelectedItem.Text + "-01-01' And PayInDate <= '" + cmbYear.SelectedItem.Text + "-12-31' And ContractCode = '" + dscontract.Tables[0].Rows[i]["ContractCode"].ToString().Trim() + "'");
                        strSql.Append(" Union All " );
                        strSql.Append(" Select Customer.CustID,Contract.ContractID,'结算单调整' As ChargeTypeName,Customer.CustName,ContractCode,ShopCode,ShopName,CustCode, " );
                        strSql.Append(" AdjDate As InvPeriod,0 As IsFirst,AdjAmt As InvActPayAmt,AdjAmtL As InvActPayAmtL,0 As InvPaidAmt,0 As InvPaidAmtL, " );
                        strSql.Append(" 0 As Status,'' As ChargeTypeID,'' As InvDetailID,'' As InvPayID,'' As PayInID,'' As PayOutID,InvAdj.InvID,InvAdjID	 ");
                        strSql.Append(" From InvAdj " );
                        strSql.Append(" Inner Join InvoiceHeader On InvAdj.InvID = InvoiceHeader.InvID " );
                        strSql.Append(" Inner Join Contract On InvoiceHeader.ContractID = Contract.ContractID " );
                        strSql.Append(" Inner Join Customer On InvoiceHeader.CustID = Customer.CustID " );
                        strSql.Append(" Inner Join Conshop On Contract.ContractID = Conshop.ContractID " );
                        strSql.Append(" Where AdjStatus = 3 And AdjDate >= '" + cmbYear.SelectedItem.Text + "-01-01 00:00:00' And AdjDate <= '" + cmbYear.SelectedItem.Text + "-12-31 23:59:59' And ContractCode = '" + dscontract.Tables[0].Rows[i]["ContractCode"].ToString().Trim() + "'");
                        strSql.Append(" Union All " );
                        strSql.Append(" Select InvoicePay.CustID,InvoicePay.ContractID,Case InvPayType When 1 Then '现金' When 2 Then '汇票' When 3 Then '支票' When 6 Then '其他' End As   " );
                        strSql.Append(" ChargeTypeName,CustName,ContractCode,ShopCode,ShopName,CustCode,InvPayDate As InvPeriod,0 As IsFirst,     " );
                        strSql.Append(" 0 As InvActPayAmt,0 As InvActPayAmtL,InvoicePay.InvPaidAmt,InvoicePay.InvPaidAmtL,1 As Status,'' As ChargeTypeID,'' As InvDetailID,InvoicePay.InvPayID,'' As PayInID, " );
                        strSql.Append(" '' As PayOutID,InvoicePayDetail.InvID,'' As InvAdjID     " );
                        strSql.Append(" From InvoicePay  " );
                        strSql.Append(" Inner Join InvoicePayDetail On InvoicePay.InvPayID= InvoicePayDetail.InvPayID   " );
                        strSql.Append(" Inner Join Contract On InvoicePay.ContractID= Contract.ContractID     " );
                        strSql.Append(" Inner Join Customer On InvoicePay.CustID = Customer.CustID     ");
                        strSql.Append(" Inner Join ConShop On ConShop.ContractID= Contract.ContractID     " );
                        strSql.Append(" Where InvPayStatus = 1 And InvPayType<>5  And InvPayDate >= '" + cmbYear.SelectedItem.Text + "-01-01' And InvPayDate <= '" + cmbYear.SelectedItem.Text + "-12-31' And ContractCode = '" + dscontract.Tables[0].Rows[i]["ContractCode"].ToString().Trim() + "'");
                        strSql.Append(" Union All	 " );
                        strSql.Append(" Select Customer.CustID,Contract.ContractID,'代收款' As ChargeTypeName,CustName,ContractCode,ShopCode,ShopName,CustCode, " );
                        strSql.Append(" PayInDate As InvPeriod,0 As IsFirst,0 As InvActPayAmt,0 As InvActPayAmtL,PayInAmt As InvPaidAmt,PayInAmt As InvPaidAmtL, ");
                        strSql.Append(" 1 As Status,'' As ChargeTypeID,'' As InvDetailID,'' As InvPayID,PayInID,'' As PayOutID,'' As InvID,'' As InvAdjID	 " );
                        strSql.Append(" From PayIn  " );
                        strSql.Append(" Inner Join Conshop On PayIn.ShopID = Conshop.ShopID " );
                        strSql.Append(" Inner Join Contract On Conshop.ContractID = Contract.ContractID " );
                        strSql.Append(" Inner Join Customer On Contract.CustID = Customer.CustID " );
                        strSql.Append(" Where PayInStatus <> 4 And PayInDate >= '" + cmbYear.SelectedItem.Text + "-01-01' And PayInDate <= '" + cmbYear.SelectedItem.Text + "-12-31' And ContractCode = '" + dscontract.Tables[0].Rows[i]["ContractCode"].ToString().Trim() + "' And PayInDataSource = " + PayIn.PAYINDATASOURCE_HAND);
                        strSql.Append(" Union All	 ");
                        strSql.Append(" Select Customer.CustID,Contract.ContractID,'代收款' As ChargeTypeName,CustName,ContractCode,ShopCode,ShopName,CustCode, ");
                        strSql.Append(" dateadd(d,-1,dateadd(m,1,Cast('" + cmbYear.SelectedItem.Text + "-' + Rtrim(cast(Month(payindate) As char)) + '-01' As smalldatetime))) As InvPeriod,0 As IsFirst,0 As InvActPayAmt,0 As InvActPayAmtL,Sum(PayInAmt) As InvPaidAmt,Sum(PayInAmt) As InvPaidAmtL, ");
                        strSql.Append(" 1 As Status,'' As ChargeTypeID,'' As InvDetailID,'' As InvPayID,'' As PayInID,'' As PayOutID,'' As InvID,'' As InvAdjID	 ");
                        strSql.Append(" From PayIn  ");
                        strSql.Append(" Inner Join Conshop On PayIn.ShopID = Conshop.ShopID ");
                        strSql.Append(" Inner Join Contract On Conshop.ContractID = Contract.ContractID ");
                        strSql.Append(" Inner Join Customer On Contract.CustID = Customer.CustID ");
                        strSql.Append(" Where PayInStatus <> 4 And Year(PayInDate) = '" + cmbYear.SelectedItem.Text + "' And ContractCode = '" + dscontract.Tables[0].Rows[i]["ContractCode"].ToString().Trim() + "'And PayInDataSource = " + PayIn.PAYINDATASOURCE_SYS + " Group by Month(PayIn.PayInDate),Customer.CustID,Contract.ContractID,Customer.CustName,Contract.ContractCode,Conshop.ShopCode,Conshop.ShopName,Customer.CustCode");        
                
                strSql.Append(" ) As a  Order By ContractCode,InvPeriod,InvDetailID,ChargeTypeID,InvPayID,PayInID,PayOutID,InvAdjID  ");

                        ds = baseBO.QueryDataSet(strSql.ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    baseTrans.BeginTrans();
                    decimal balanceSum = _balanceSum;
                    decimal balanceSumL = _balanceSumL;
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        builderAuditing.CustID = Convert.ToInt32(ds.Tables[0].Rows[j]["CustID"]);
                        builderAuditing.ContractID = Convert.ToInt32(ds.Tables[0].Rows[j]["CustID"]);
                        builderAuditing.ChargeTypeName = ds.Tables[0].Rows[j]["ChargeTypeName"].ToString();
                        builderAuditing.ContractCode = ds.Tables[0].Rows[j]["ContractCode"].ToString();
                        builderAuditing.CustName = ds.Tables[0].Rows[j]["CustName"].ToString();
                        builderAuditing.ShopCode = ds.Tables[0].Rows[j]["ShopCode"].ToString();
                        builderAuditing.ShopName = ds.Tables[0].Rows[j]["ShopName"].ToString();
                        builderAuditing.CustCode = ds.Tables[0].Rows[j]["CustCode"].ToString();
                        builderAuditing.InvActPayAmt = Convert.ToDecimal(ds.Tables[0].Rows[j]["InvActPayAmt"]);
                        builderAuditing.InvActPayAmtL = Convert.ToDecimal(ds.Tables[0].Rows[j]["InvActPayAmtL"]);
                        builderAuditing.InvPaidAmt = Convert.ToDecimal(ds.Tables[0].Rows[j]["InvPaidAmt"]);
                        builderAuditing.InvPaidAmtL = Convert.ToDecimal(ds.Tables[0].Rows[j]["InvPaidAmtL"]);
                        builderAuditing.InvPeriod = Convert.ToDateTime(ds.Tables[0].Rows[j]["InvPeriod"]);
                        builderAuditing.InvID = Convert.ToInt32(ds.Tables[0].Rows[j]["InvID"]);
                        if (Convert.ToInt32(ds.Tables[0].Rows[j]["Status"]) == 0)
                        {
                            balanceSum = balanceSum + Convert.ToDecimal(ds.Tables[0].Rows[j]["InvActPayAmt"]);
                            balanceSumL = balanceSumL + Convert.ToDecimal(ds.Tables[0].Rows[j]["InvActPayAmtL"]);
                            builderAuditing.Balance = balanceSum;
                            builderAuditing.BalanceL = balanceSumL;
                        }
                        else if (Convert.ToInt32(ds.Tables[0].Rows[j]["Status"]) == 1)
                        {
                            balanceSum = balanceSum - Convert.ToDecimal(ds.Tables[0].Rows[j]["InvPaidAmt"]);
                            balanceSumL = balanceSumL - Convert.ToDecimal(ds.Tables[0].Rows[j]["InvPaidAmtL"]);

                            builderAuditing.Balance = balanceSum;
                            builderAuditing.BalanceL = balanceSumL;
                        }

                        builderAuditing.Status = BaseApp.GetBuilderAuditingStatus();
                        builderAuditing.CreateUserID = sessionUser.UserID;
                        builderAuditing.OprDeptID = sessionUser.DeptID;
                        builderAuditing.OprRoleID = sessionUser.RoleID;

                        if (baseTrans.Insert(builderAuditing) < 0)
                        {
                            baseTrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                            return;
                        }
                    }

                    baseTrans.Commit();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "    " + cmbYear.SelectedItem.Text + (String)GetGlobalResourceObject("BaseInfo", "MakePoolVoucher_FYear") + "'", true);
                }
            }
        }
    }



    protected void BtnCel_Click(object sender, EventArgs e)
    {

    }
}
