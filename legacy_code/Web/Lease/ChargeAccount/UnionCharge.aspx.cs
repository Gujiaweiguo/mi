using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using Base.Biz;
using Base.DB;
using Base;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using Invoice;
using Base.Page;
using BaseInfo.User;
using Lease.Union;
using BaseInfo.authUser;
public partial class Lease_ChargeAccount_UnionCharge : BasePage
{
    public string emptyStr;
    public string IsInt;
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SetControl(true);
            RBtnNoPrint.Checked = true;
            RBtnPrint.Checked = false;
            RBtnNoPrint.Enabled = false;
            RBtnPrint.Enabled = false;

            //绑定合同类型
            int[] contractType = Contract.GetBizModes();
            int s = contractType.Length;
            for (int i = 0; i < s; i++)
            {
                cmbContractType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(contractType[i])), contractType[i].ToString()));
            }

            cmbContractType.SelectedValue = Contract.BIZ_MODE_UNIT.ToString();

            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            IsInt = (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputInt");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_UnionCount");
            this.btnQuery.Attributes.Add("onclick", "return InputValidator(form1)");

            BaseBO baseBo = new BaseBO();
            Resultset rs = baseBo.Query(new ChargeType());
            chklChargeType.DataSource = rs;
            chklChargeType.DataBind();

            //初始化CheckBoxList
            SetChecked();
            //this.chklChargeType.Attributes.Add("onclick", "CheckAll()");
        }
    }

    private void SetControl(bool s)
    {
        txtContractID.ReadOnly = s;
        RBtnNoPrint.Checked = s;
        RBtnPrint.Checked = !s;
        RBtnNoPrint.Enabled = !s;
        RBtnPrint.Enabled = !s;
    }

    protected void RBtnOneContract_CheckedChanged(object sender, EventArgs e)
    {
        if (RBtnOneContract.Checked == true)
        {
            SetControl(false);

        }
    }
    protected void RBtnAllContract_CheckedChanged(object sender, EventArgs e)
    {
        if (RBtnAllContract.Checked == true)
        {
            SetControl(true);
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (this.txtContractID.Text != "")
        {
            string contractCode = this.txtContractID.Text;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ContractCode = '" + contractCode + "' and (ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR + " or ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_END + ")";
            Resultset rs = baseBo.Query(new Contract());
            if (rs.Count == 1)
            {
                Contract contct = rs.Dequeue() as Contract;
                int custId = contct.CustID;
                ViewState["contractid"] = contct.ContractID;
                int contractTypeId = contct.BizMode;
                this.cmbContractType.SelectedItem.Text = (String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(contractTypeId));
                if (cmbContractType.SelectedValue == Contract.BIZ_MODE_LEASE.ToString())
                {
                    btnSave.Enabled = false;
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseChargeAccount") + "'", true);
                }

                baseBo.WhereClause = "";
                baseBo.WhereClause = "CustID = " + custId;
                Resultset custRs = baseBo.Query(new Customer());
                if (custRs.Count == 1)
                {
                    Customer cutmr = custRs.Dequeue() as Customer;
                    this.txtCustName.Text = cutmr.CustName;
                }
            }
        }
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        SetControl(true);
        btnSave.Enabled = true;
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (txtChargeDate.Text == "" || txtEndDate.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputChargeDate") + "'", true);
            return;
        }
        else
        {

            string begainYM  = Convert.ToDateTime(txtChargeDate.Text).Year.ToString() + Convert.ToDateTime(txtChargeDate.Text).Month.ToString();
            string endYM = Convert.ToDateTime(txtEndDate.Text).Year.ToString() + Convert.ToDateTime(txtEndDate.Text).Month.ToString();
            if (begainYM == endYM)
            {
                //得到CheckBoxList中选中了的值
                string chargeTypeValue = GetChecked();

                int invCode;
                Hashtable htb = new Hashtable();
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                htb.Add("CreateUserID", objSessionUser.UserID);
                htb.Add("OprDeptID", objSessionUser.DeptID);
                htb.Add("OprRoleID", objSessionUser.RoleID);

                ArrayList bfoChgNoAryList = new ArrayList();

                string bancthID = BaseApp.GetInvoiceHeaderBancthID().ToString();

                BaseBO basebo = new BaseBO();
                //查询条件:合同状态-正常; 经营方式:租赁
                //basebo.WhereClause = "ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR + " and BizMode = " + Contract.BIZ_MODE_LEASE;

                //查询条件：合同状态-正常或终止（并且终止状态的合同：终止日期>算费日期）；经营方式:联营
                basebo.WhereClause = " BizMode = " + Contract.BIZ_MODE_UNIT + " and (ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR + " or (ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_END + " and StopDate > '" + Convert.ToDateTime(txtChargeDate.Text) +"'))";
                if (RBtnOneContract.Checked == true)
                {
                    basebo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractid"]) + " and " + basebo.WhereClause;
                }
                string str_sql = "";
                SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                {
                    str_sql = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                }
                basebo.WhereClause = basebo.WhereClause + str_sql;
                Resultset rs = basebo.Query(new ConLeaseCharge());
                if (rs.Count > 0)
                {
                    string bchID = BaseApp.GetBancthID().ToString();
                    string message = "";
                    foreach (ConLeaseCharge conLeaseCharge in rs)
                    {
                        this.txtAccountContractID.Text = conLeaseCharge.ContractCode;
                        int result = UnionChargeCount.AccountCharge(conLeaseCharge.ContractID, Convert.ToDateTime(txtChargeDate.Text), Convert.ToDateTime(txtEndDate.Text), 0, chargeTypeValue, htb, bancthID, out invCode, out bfoChgNoAryList);
                        if (result == Invoice.ChargeAccount.PROMT_SUCCED) //成功
                        {
                            if (invCode > 0)
                            {
                                message = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed");
                                InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_YES, message, 0, bchID);
                                if (rs.Count > 1)
                                {
                                    if (invCode > 0)
                                    {
                                        //this.Response.Redirect("../../ReportM/RptInv/RptChargeCountLog.aspx?bancthID=" + );
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No_Data") + "'", true);
                                        //Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No_Data") + "');</script>");
                                    }
                                }
                                if (RBtnPrint.Checked == true)
                                {
                                    if (bfoChgNoAryList.Count > 0) //前期费用未生成
                                    {
                                        InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_NO, 1, bfoChgNoAryList, bchID);
                                        if (rs.Count == 1)
                                        {
                                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Before_Charge_No") + "'", true);
                                        }
                                    }
                                    if (invCode > 0)
                                    {
                                        this.Response.Redirect("../../ReportM/RptLeaseInvJV.aspx?InvCode=" + invCode + "&flag =" + 0);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No_Data") + "'", true);
                                        //Response.Write("<script language=javascript>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No_Data") + "');</script>");
                                    }
                                }
                            }

                            //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Succed") + "'", true);
                        }
                        if (result == Invoice.ChargeAccount.PROMT_MONTH_CHARGE_YES) //该月费用已生成
                        {
                            message = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Month_Charge_Yes");
                            InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_NO, message, 0, bchID);
                            if (rs.Count == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Month_Charge_Yes") + "'", true);
                            }
                        }
                        if (result == Invoice.ChargeAccount.PROMT_FIRST_CHARGE_NO) //首期费用未生成
                        {
                            message = (String)GetGlobalResourceObject("BaseInfo", "Prompt_First_Charge_No");
                            InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_NO, message, 0, bchID);
                            if (rs.Count == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_First_Charge_No") + "'", true);
                            }
                        }
                        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_NO) //合同无效
                        {
                            message = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No");
                            InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_NO, message, 0, bchID);
                            if (rs.Count == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No") + "'", true);
                            }
                        }
                        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_INFO_NO) //未有合同信息
                        {
                            message = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_Info_No");
                            InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_NO, message, 0, bchID);
                            if (rs.Count == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_Info_No") + "'", true);
                            }
                        }
                        if (result == Invoice.ChargeAccount.PROMT_CONTRACT_DATE_NO) //结算时间段完全不在合同时间范围内
                        {
                            message = (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contact_Date_No");
                            InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_NO, message, 0, bchID);
                            if (rs.Count == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contact_Date_No") + "'", true);
                            }
                        }
                        if (bfoChgNoAryList.Count > 0) //前期费用未生成
                        {
                            //int count = bfoChgNoAryList.Count;
                            //for (int i = 0; i < count; i++)
                            //{
                            //    string chgTypeName = ChargeTypePO.GetChargeTypeNameByID(Convert.ToInt32(bfoChgNoAryList[i]));
                            //    message = chgTypeName + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Before_Charge_No");
                            InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_NO, 1, bfoChgNoAryList, bchID);
                            //}
                            if (rs.Count == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Before_Charge_No") + "'", true);
                            }
                        }
                        if (result == Invoice.ChargeAccount.PROMT_EXRATE_NO)  //汇率有误
                        {
                            message = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblExRateError");
                            InsertLog(conLeaseCharge.ContractID, ChargeCountLog.PRODUCTSTATUS_NO, message, 0, bchID);
                            if (rs.Count == 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_lblExRateError") + "'", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No_Data") + "'", true);
                        }
                    }
                    if (rs.Count > 1)
                    {
                        this.Response.Redirect("../../ReportM/RptInv/RptChargeCountLog.aspx?bancthID=" + bchID);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Contract_No") + "'", true);
            }
        }
    }

    /// <summary>
    /// 初始化CheckBoxList,使其默认为选中状态
    /// </summary>
    private void SetChecked()
    {
        for (int i = 0; i < chklChargeType.Items.Count; i++)
        {
            chklChargeType.Items[i].Selected = true;
        }
    }

    /// <summary>
    /// 得到CheckBoxList中选中了的值
    /// </summary>
    /// <returns></returns>
    private string GetChecked()
    {
        string separator = ",";
        string selval = "";
        for (int i = 0; i < chklChargeType.Items.Count; i++)
        {
            if (chklChargeType.Items[i].Selected)
            {
                selval += chklChargeType.Items[i].Value + separator;
            }
        }
        return selval;
    }

    /// <summary>
    /// 写日志表
    /// </summary>
    /// <param name="conID">合同号</param>
    /// <param name="flag">是否生成成功；0：成功；1：失败</param>
    /// <param name="note">错误信息</param>
    /// <param name="befFlag">前期费用未生成标志</param>
    private void InsertLog(int conID, int flag, string note, int befFlag, string bancthID)
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "ContractID = " + conID;
        Resultset rs = baseBo.Query(new Contract());
        Contract contct = rs.Dequeue() as Contract;

        //取费用类型;
        string chgTypeID = GetChecked();
        string[] ss = Regex.Split(chgTypeID, ",");
        int chgeTypeCount = ss.Length;

        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + conID;
        Resultset leaseRS = baseBo.Query(new ConUnion());
        ConUnion conlse = leaseRS.Dequeue() as ConUnion;
        int billCyle = conlse.BillCycle;
        DateTime period = Convert.ToDateTime(txtChargeDate.Text);
        for (int l = 0; l < billCyle; l++)
        {
            period = period.AddMonths(l);
            for (int i = 0; i < chgeTypeCount - 1; i++)
            {
                ChargeCountLog chgCutLog = new ChargeCountLog();
                chgCutLog.ContractID = contct.ContractID;
                chgCutLog.CustID = contct.CustID;
                chgCutLog.ChargePeriod = Convert.ToDateTime(txtChargeDate.Text);
                chgCutLog.ProductDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                chgCutLog.ProductStatus = flag;
                chgCutLog.ProductNote = note;
                chgCutLog.ChargeTypeID = Convert.ToInt32(ss[i]);
                chgCutLog.BancthID = bancthID;
                if (flag == 0)
                {
                    DataSet chgcntDS = ChargeCountLogPO.GetPayAmt(chgCutLog.ContractID, period, Convert.ToInt32(ss[i]));
                    int dsCount = chgcntDS.Tables[0].Rows.Count;
                    if (dsCount > 0)
                    {
                        for (int m = 0; m < dsCount; m++)
                        {
                            chgCutLog.InvPayAmt = Convert.ToDecimal(chgcntDS.Tables[0].Rows[m]["InvPayAmt"]);
                            chgCutLog.ChargeCountID = BaseApp.GetChargeCountID();
                            ChargeCountLogPO.InsertChargeCountLog(chgCutLog);
                        }
                    }
                }
                else
                {
                    chgCutLog.InvPayAmt = 0;
                    chgCutLog.ChargeCountID = BaseApp.GetChargeCountID();
                    ChargeCountLogPO.InsertChargeCountLog(chgCutLog);
                    //if (befFlag == 1)
                    //{
                    //    break;
                    //}
                }
            }
        }
    }

    /// <summary>
    /// 写日志表
    /// </summary>
    /// <param name="conID">合同号</param>
    /// <param name="flag">是否生成成功；0：成功；1：失败</param>
    /// <param name="note">错误信息</param>
    /// <param name="befFlag">前期费用未生成标志</param>
    private void InsertLog(int conID, int flag, int befFlag, ArrayList aryList, string bancthID)
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "ContractID = " + conID;
        Resultset rs = baseBo.Query(new Contract());
        Contract contct = rs.Dequeue() as Contract;

        ////取费用类型;
        //string chgTypeID = GetChecked();
        //string[] ss = Regex.Split(chgTypeID, ",");
        //int chgeTypeCount = ss.Length;

        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + conID;
        Resultset leaseRS = baseBo.Query(new ConUnion());
        ConUnion conlse = leaseRS.Dequeue() as ConUnion;
        int billCyle = conlse.BillCycle;
        DateTime period = Convert.ToDateTime(txtChargeDate.Text);
        for (int l = 0; l < billCyle; l++)
        {
            period = period.AddMonths(l);
            for (int i = 0; i < aryList.Count; i++)
            {
                ChargeCountLog chgCutLog = new ChargeCountLog();
                string chgTypeName = ChargeTypePO.GetChargeTypeNameByID(Convert.ToInt32(aryList[i]));
                string message = chgTypeName + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Before_Charge_No");
                chgCutLog.ContractID = contct.ContractID;
                chgCutLog.CustID = contct.CustID;
                chgCutLog.ChargePeriod = Convert.ToDateTime(txtChargeDate.Text);
                chgCutLog.ProductDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                chgCutLog.ProductStatus = flag;
                chgCutLog.ProductNote = message;
                chgCutLog.ChargeTypeID = Convert.ToInt32(aryList[i]);
                chgCutLog.BancthID = bancthID;
                if (flag == 0)
                {
                    DataSet chgcntDS = ChargeCountLogPO.GetPayAmt(chgCutLog.ContractID, period, Convert.ToInt32(aryList[i]));
                    int dsCount = chgcntDS.Tables[0].Rows.Count;
                    if (dsCount > 0)
                    {
                        for (int m = 0; m < dsCount; m++)
                        {
                            chgCutLog.InvPayAmt = Convert.ToDecimal(chgcntDS.Tables[0].Rows[m]["InvPayAmt"]);
                            chgCutLog.ChargeCountID = BaseApp.GetChargeCountID();
                            ChargeCountLogPO.InsertChargeCountLog(chgCutLog);
                        }
                    }
                }
                else
                {
                    chgCutLog.InvPayAmt = 0;
                    chgCutLog.ChargeCountID = BaseApp.GetChargeCountID();
                    ChargeCountLogPO.InsertChargeCountLog(chgCutLog);
                    //if (befFlag == 1)
                    //{
                    //    break;
                    //}
                }
            }
        }
    }
}
