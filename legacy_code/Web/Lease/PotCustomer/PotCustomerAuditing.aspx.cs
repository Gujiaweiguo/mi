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
using System.Text.RegularExpressions;

using Base.Page;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;

using Base.Biz;
using Base;
using Lease.PotCustLicense;
using BaseInfo.User;
using Base.DB;
using BaseInfo.Dept;
using Lease.PotCust;
using Lease.Customer;
using Lease.Contract;
using Lease.ConShop;
using Lease.Formula;
using Base.Util;


public partial class Lease_PotCustomer_PotCustomerAuditing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //绑定信用等级
            BaseInfo.BaseCommon.BindDropDownList("select CreditLevelId,CreditLevelName from CreditLevel where status=1", "CreditLevelId", "CreditLevelName", this.ddlCreditLevel);
            //绑定客户来源
            BaseInfo.BaseCommon.BindDropDownList("select SourceTypeId,SourceTypeName from SourceType where SourceTypeStatus=1", "SourceTypeId", "SourceTypeName", this.ddlSourceType);

            //butConsent.Attributes.Add("onclick", "refurbishTree()");

            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();


            /*币种*/
            baseBO.WhereClause = "";
            rs = baseBO.Query(new CurrencyType());
            foreach (CurrencyType curType in rs)
            {
                DDownListCurrencyType.Items.Add(new ListItem(curType.CurTypeName.ToString(), curType.CurTypeID.ToString()));
            }

            /*商户类型*/
            baseBO.WhereClause = "CustTypeStatus=" + CustType.CUST_TYPE_STATUS_VALID;
            rs = baseBO.Query(new CustType());
            foreach (CustType custtype in rs)
            {
                cmbCustType.Items.Add(new ListItem(custtype.CustTypeName, custtype.CustTypeID.ToString()));
            }

            /*招商员选择列表*/
            baseBO.WhereClause = "UserStatus=" + UserInfo.USER_STATUS_VALID;
            baseBO.GroupBy = "a.userid,UserName,a.UserCode,WorkNo,OfficeTel,UserStatus";
            rs = baseBO.Query(new UserInfo());
            foreach (UserInfo user in rs)
            {
                cmbCommOper.Items.Add(new ListItem(user.UserName, user.UserID.ToString()));
            }

            #region 
            if (Request.QueryString["VoucherID"] != null)
            {
                showCustomerInfo(Convert.ToInt32(Request.QueryString["VoucherID"]));

                /*把商户ID存入Cookies*/
                HttpCookie cookies = new HttpCookie("Custumer");

                cookies.Expires = System.DateTime.Now.AddHours(1);
                cookies.Values.Add("CustumerID", Request.QueryString["VoucherID"]);
                Response.AppendCookie(cookies);

                ViewState["custID"] = Request.QueryString["VoucherID"].ToString();

                SaveWrkCookies();

                
                
            }
            else if (Request.Cookies["Custumer"].Values["CustumerID"] != null)
            {
                //SaveWrkCookies();
                ViewState["custID"] = Request.Cookies["Custumer"].Values["CustumerID"].ToString();
                showCustomerInfo(Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]));
            }
            #endregion  
         
            if (Request.QueryString["WrkFlwID"] != null)
            {
                HidenWrkID.Value = Request.QueryString["WrkFlwID"].ToString();
            }
            else
            {
                HidenWrkID.Value = Request.Cookies["WorkFlow"].Values["WorkFlowID"].ToString();
            }
            if (Request.QueryString["VoucherID"] != null)
            {
                HidenVouchID.Value = Request.QueryString["VoucherID"].ToString();
            }
            else
            {
                HidenVouchID.Value = Request.Cookies["WorkFlow"].Values["VoucherID"].ToString();
            }


            btnMessage.Attributes.Add("onclick", "ShowMessage()");
        }
    }

    #region 显示商户信息
    private void showCustomerInfo(int custID)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        baseBO.WhereClause = "a.CustID=" + custID;
        baseBO.GroupBy = "";
        rs = baseBO.Query(new PotCustomerQuery());
        if (rs != null)
        {
            PotCustomerQuery potCustomerQuery = rs.Dequeue() as PotCustomerQuery;
            txtCustCode.Text = potCustomerQuery.CustCode.ToString();
            txtCustName.Text = potCustomerQuery.CustName;
            txtCustShortName.Text = potCustomerQuery.CustShortName;
            cmbCustType.SelectedValue = potCustomerQuery.CustTypeID.ToString();
            txtLegalRep.Text = potCustomerQuery.LegalRep;
            txtLegalRepTitle.Text = potCustomerQuery.LegalRepTitle;
            txtRegCap.Text = potCustomerQuery.RegCap.ToString();
            txtRegAddr.Text = potCustomerQuery.RegAddr;
            txtRegCode.Text = potCustomerQuery.RegCode;
            txtTaxCode.Text = potCustomerQuery.TaxCode;
            txtBankName.Text = potCustomerQuery.BankName;
            txtBankAcct.Text = potCustomerQuery.BankAcct;
            txtOfficeAddr.Text = potCustomerQuery.OfficeAddr;
            txtOfficeAddr2.Text = potCustomerQuery.OfficeAddr2;
            txtOfficeAddr3.Text = potCustomerQuery.OfficeAddr3;
            txtPostAddr.Text = potCustomerQuery.PostAddr;
            txtPostAddr2.Text = potCustomerQuery.PostAddr2;
            txtPostAddr3.Text = potCustomerQuery.PostAddr3;
            txtPostCode.Text = potCustomerQuery.PostCode;
            txtWeb.Text = potCustomerQuery.WebURL;
            //txtContactorName.Text = potCustomerQuery.ContactorName;
            //txtTitle.Text = potCustomerQuery.Title;
            //txtOfficeTel.Text = potCustomerQuery.OfficeTel;
            //txtMobileTel.Text = potCustomerQuery.MobileTel;
            //txtEMail.Text = potCustomerQuery.EMail;
            //txtFax.Text = potCustomerQuery.Fax;
            this.ddlCreditLevel.SelectedValue = potCustomerQuery.CreditLevelId.ToString();
            this.ddlSourceType.SelectedValue = potCustomerQuery.SourceTypeId.ToString();
            cmbCommOper.SelectedValue = potCustomerQuery.CommOper.ToString();
            DDownListCurrencyType.SelectedValue = potCustomerQuery.CurTypeID.ToString();

        }
    }
    #endregion
    protected void butConsent_Click(object sender, EventArgs e)
    {
        Consent();
        this.UpdateStatus();//改变招商状态
    }
    /// <summary>
    /// 更改招商状态
    /// </summary>
    private void UpdateStatus()
    {
        int custID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
        BaseBO objBaseBo = new BaseBO();
        DataSet dsShop = objBaseBo.QueryDataSet("select potshopid,shopStatus from potshop where custid=" + custID + " and shopStatus=1");
        if (dsShop != null && dsShop.Tables[0].Rows.Count > 0)
        {
            objBaseBo.ExecuteUpdate("update potshop set shopStatus=0 where potshopid=" + dsShop.Tables[0].Rows[0]["potshopid"].ToString() + "");
        }
        DataSet dsPalaver = objBaseBo.QueryDataSet("select palaverid,palaverstatus from custpalaver where custid=" + custID + " and palaverstatus=1");
        if (dsPalaver != null && dsPalaver.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsPalaver.Tables[0].Rows.Count; i++)
            {
                objBaseBo.ExecuteUpdate("update CustPalaver set PalaverStatus=0 where palaverid = " + dsPalaver.Tables[0].Rows[i]["palaverid"].ToString() + "");
            }
        }
    }
    private void Consent()
    {
        /*工作流结转和生成正式商户资料*/
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
        int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);
        int sequence = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["Sequence"]);
        int voucherID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
        int shopType = -1;
       
        String voucherHints = txtCustShortName.Text;
        String voucherMemo = "";//txtVoucherMemo.Text.Trim();
        int operatorID = objSessionUser.UserID;
        int deptID = objSessionUser.DeptID;
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        Resultset rsWrkFlw = new Resultset();
        PotCustomer potcustomer = new PotCustomer();
        Customer customer = new Customer();
        SignUpContract signUpContract = new SignUpContract();
        PotShop potShop = new PotShop();
        TransmitConShop transmitConShop = new TransmitConShop();
        BaseTrans basetrans = new BaseTrans();
        int i = 0;

        try
        {
            int custID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            baseBO.WhereClause = "Custid=" + custID + " and shopstatus=1";
            rs = baseBO.Query(potShop);
            if (rs.Count == 1)
            {
                potShop = rs.Dequeue() as PotShop;
                shopType = potShop.BizMode;
            }

            WrkFlwNode[] nextNodes = WrkFlwApp.GetNextWrkFlwNodes(wrkFlwID, nodeID);


            basetrans.BeginTrans();

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
            basetrans.ExecuteUpdate("delete from CustLicense where custid=" + custID + "");
            basetrans.ExecuteUpdate("delete from CustContact where custid=" + custID + "");
            basetrans.ExecuteUpdate("delete from custbrand where custid=" + custID + "");
            basetrans.ExecuteUpdate("delete from CustOprInfo where custid=" + custID + "");
            basetrans.ExecuteUpdate("delete from Customer where custid=" + custID + "");
            if (shopType == Contract.BIZ_MODE_LEASE)
            {
                DataTable dt = baseBO.QueryDataSet("Select ProcessClassOne From WrkFlw Where WrkFlwID=" + wrkFlwID).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string[] wrkflwID = Regex.Split(dt.Rows[0]["ProcessClassOne"].ToString(), ",");

                    WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo, basetrans);

                    if (nextNodes == null)
                    {

                        if (custID.ToString() == "")
                        {
                            return;
                        }//导入权证
                        string sqlstr = "Insert Into  CustLicense Select CustLicenseID,CustID,CustLicenseName,CustLicenseCode,CustLicenseType,CustLicenseStartDate,CustLicenseEndDate,Note From  PotCustLicense Where custid=" + custID;
                        i = basetrans.ExecuteUpdate(sqlstr);
                        if (i == -1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustLicense") + "'", true);
                            return;
                        }//导入联系人
                        i = basetrans.ExecuteUpdate("Insert Into  CustContact Select CustContactID,CustID,ContactorName,Title,OfficeTel,MobileTel,EMail,OfficeAddr,Note,ManageArea,Fax,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID From  PotCustContact Where custid=" + custID);//联系人
                        if (i == -1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustContact") + "'", true);
                            return;
                        }
                        //导入经营品牌 add by lcp at 2009-3-26
                        i = basetrans.ExecuteUpdate("Insert Into  custbrand Select CustBrandId,CustId,TradeId,BrandId,OperateTypeId,ConsumerSex,ConsumerAge,AvgAmt,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID,ClientLevelId,PriceRange From  potcustbrand Where custid=" + custID);
                        if (i == -1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustContact") + "'", true);
                            return;
                        }
                        //导入经营概况
                        i = basetrans.ExecuteUpdate("Insert Into  CustOprInfo Select CustOprInfoId,CustID,CustTypeID,OperateAreas,ShopNumber,AreaSalesRate,BaseDiscount,PromoteArea,PromoteCost,planShopNumber,planArea,planDate,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID From  PotCustOprInfo Where custid=" + custID);
                        if (i == -1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustContact") + "'", true);
                            return;
                        }
                        //
                        baseBO.WhereClause = "Custid=" + custID;
                        rs = baseBO.Query(potcustomer);
                        if (rs.Count == 1)
                        {
                            potcustomer = rs.Dequeue() as PotCustomer;
                            customer.CommOper = potcustomer.CommOper;
                        }

                        i = basetrans.ExecuteUpdate("Insert Into Customer Select CustID,CustCode,CustName,CustShortName,CustTypeID,LegalRep,LegalRepTitle,RegCode,LegalCode,TaxCode,CreditLevel," +
                            "IsBlacklist,RegCap,BankName,BankAcct,RegAddr,OfficeAddr,PostAddr,PostCode,CustomerStatus,OfficeTel,Fax,WebURL," +
                            "BizMode,FirstSigningDate,CommOper,Note,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,CurTypeID,OfficeAddr2,OfficeAddr3,PostAddr2,PostAddr3,SourceTypeId,CreditLevelId From PotCustomer Where Custid=" + custID);
                        if (i < 1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                            return;
                        }

                        baseBO.WhereClause = "Custid=" + custID + "  and shopstatus=1";
                        rs = baseBO.Query(potShop);
                        if (rs.Count == 1)
                        {
                            potShop = rs.Dequeue() as PotShop;
                            signUpContract.ContractID = BaseApp.GetContractID();
                            voucherID = signUpContract.ContractID;
                            signUpContract.CustID = custID;
                            signUpContract.ContractStatus = Contract.CONTRACTSTATUS_TYPE_FIRST;
                            signUpContract.SigningMode = Contract.CONTRACTSTATUS_TYPE_N;
                            signUpContract.ConStartDate = Convert.ToDateTime(potShop.ShopStartDate);
                            signUpContract.ConEndDate = Convert.ToDateTime(potShop.ShopEndDate);
                            signUpContract.ChargeStartDate = Convert.ToDateTime(potShop.ShopStartDate);
                            signUpContract.NorentDays = 0;
                            signUpContract.BizMode = potShop.BizMode;
                            signUpContract.CommOper = customer.CommOper;
                            shopType = potShop.BizMode;
                            if (shopType == Contract.BIZ_MODE_LEASE)
                            {
                                signUpContract.ContractTypeID = Contract.BIZ_MODE_LEASE;
                            }
                            else if (shopType == Contract.BIZ_MODE_UNIT)
                            {
                                signUpContract.ContractTypeID = Contract.BIZ_MODE_UNIT;
                            }
                            transmitConShop.ShopID = BaseApp.GetShopID();
                            transmitConShop.ContractID = signUpContract.ContractID;
                            transmitConShop.ShopName = potShop.PotShopName;
                            transmitConShop.ShopTypeID = potShop.ShopTypeID;
                            transmitConShop.ShopStartDate = potShop.ShopStartDate;
                            transmitConShop.RentArea = potShop.RentArea;
                            transmitConShop.AreaID = potShop.AreaID;
                            transmitConShop.ShopEndDate = potShop.ShopEndDate;
                        }
                        if (basetrans.Insert(signUpContract) < 1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                            return;
                        }
                        if (basetrans.Insert(transmitConShop) < 1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                            return;
                        }

                        VoucherInfo voucherInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);

                        WrkFlwApp.CommitVoucher(Convert.ToInt32(wrkflwID[0]), Convert.ToInt32(wrkflwID[1]), WrkFlwEntity.NODE_STATUS_NORMAL_PENDING, voucherInfo, basetrans);
                    }
                }
                else
                {
                    basetrans.Rollback();
                    return;
                }
                
            }
            else if (shopType == Contract.BIZ_MODE_UNIT)//联营客户
            {

                DataTable dt = baseBO.QueryDataSet("Select ProcessClassTwo From WrkFlw Where WrkFlwID=" + wrkFlwID).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string[] wrkflwID = Regex.Split(dt.Rows[0]["ProcessClassTwo"].ToString(), ",");

                    WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo, basetrans);
                    if (nextNodes == null)
                    {
                        if (custID.ToString() == "")
                        {
                            return;
                        }//导入权证
                        string sqlstr = "Insert Into  CustLicense Select CustLicenseID,CustID,CustLicenseName,CustLicenseCode,CustLicenseType,CustLicenseStartDate,CustLicenseEndDate,Note From  PotCustLicense Where custid=" + custID;
                        i = basetrans.ExecuteUpdate(sqlstr);
                        if (i == -1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustLicense") + "'", true);
                            return;
                        }//导入联系人
                        i = basetrans.ExecuteUpdate("Insert Into  CustContact Select CustContactID,CustID,ContactorName,Title,OfficeTel,MobileTel,EMail,OfficeAddr,Note,ManageArea,Fax,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID From  PotCustContact Where custid=" + custID);
                        if (i == -1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustContact") + "'", true);
                            return;
                        }
                        //导入经营品牌 add by lcp at 2009-3-26
                        i = basetrans.ExecuteUpdate("Insert Into  custbrand Select CustBrandId,CustId,TradeId,BrandId,OperateTypeId,ConsumerSex,ConsumerAge,AvgAmt,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID,ClientLevelId,PriceRange From  potcustbrand Where custid=" + custID);
                        if (i == -1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustContact") + "'", true);
                            return;
                        }
                        //导入经营概况
                        i = basetrans.ExecuteUpdate("Insert Into  CustOprInfo Select CustOprInfoId,CustID,CustTypeID,OperateAreas,ShopNumber,AreaSalesRate,BaseDiscount,PromoteArea,PromoteCost,planShopNumber,planArea,planDate,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID From  PotCustOprInfo Where custid=" + custID);
                        if (i == -1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustContact") + "'", true);
                            return;
                        }
                        //
                        baseBO.WhereClause = "Custid=" + custID;
                        rs = baseBO.Query(potcustomer);
                        if (rs.Count == 1)
                        {
                            potcustomer = rs.Dequeue() as PotCustomer;
                            customer.CommOper = potcustomer.CommOper;
                        }

                        i = basetrans.ExecuteUpdate("Insert Into Customer Select CustID,CustCode,CustName,CustShortName,CustTypeID,LegalRep,LegalRepTitle,RegCode,LegalCode,TaxCode,CreditLevel," +
                            "IsBlacklist,RegCap,BankName,BankAcct,RegAddr,OfficeAddr,PostAddr,PostCode,CustomerStatus,OfficeTel,Fax,WebURL," +
                            "BizMode,FirstSigningDate,CommOper,Note,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,CurTypeID,OfficeAddr2,OfficeAddr3,PostAddr2,PostAddr3,SourceTypeId,CreditLevelId From PotCustomer Where Custid=" + custID);
                        if (i < 1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                            return;
                        }

                        baseBO.WhereClause = "Custid=" + custID + " and  shopstatus=1";
                        rs = baseBO.Query(potShop);
                        if (rs.Count == 1)
                        {
                            potShop = rs.Dequeue() as PotShop;
                            signUpContract.ContractID = BaseApp.GetContractID();
                            voucherID = signUpContract.ContractID;
                            signUpContract.CustID = custID;
                            signUpContract.ContractStatus = Contract.CONTRACTSTATUS_TYPE_FIRST;
                            signUpContract.SigningMode = Contract.CONTRACTSTATUS_TYPE_N;
                            signUpContract.ConStartDate = Convert.ToDateTime(potShop.ShopStartDate);
                            signUpContract.ConEndDate = Convert.ToDateTime(potShop.ShopEndDate);
                            signUpContract.ChargeStartDate = Convert.ToDateTime(potShop.ShopStartDate);
                            signUpContract.NorentDays = 0;
                            signUpContract.BizMode = potShop.BizMode;
                            signUpContract.CommOper = customer.CommOper;
                            shopType = potShop.BizMode;
                            if (shopType == Contract.BIZ_MODE_LEASE)
                            {
                                signUpContract.ContractTypeID = Contract.BIZ_MODE_LEASE;
                            }
                            else if (shopType == Contract.BIZ_MODE_UNIT)
                            {
                                signUpContract.ContractTypeID = Contract.BIZ_MODE_UNIT;
                            }
                            transmitConShop.ShopID = BaseApp.GetShopID();
                            transmitConShop.ContractID = signUpContract.ContractID;
                            transmitConShop.ShopName = potShop.PotShopName;
                            transmitConShop.ShopTypeID = potShop.ShopTypeID;
                            transmitConShop.ShopStartDate = potShop.ShopStartDate;
                            transmitConShop.RentArea = potShop.RentArea;
                            transmitConShop.AreaID = potShop.AreaID;
                            transmitConShop.ShopEndDate = potShop.ShopEndDate;
                        }
                        if (basetrans.Insert(signUpContract) < 1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                            return;
                        }
                        if (basetrans.Insert(transmitConShop) < 1)
                        {
                            basetrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                            return;
                        }
                        VoucherInfo voucherInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);

                        WrkFlwApp.CommitVoucher(Convert.ToInt32(wrkflwID[0]), Convert.ToInt32(wrkflwID[1]), WrkFlwEntity.NODE_STATUS_NORMAL_PENDING, voucherInfo, basetrans);

                    }
                }
                else
                {
                    basetrans.Rollback();
                    return;
                }
            }
            basetrans.Commit();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);

            this.ClearCookies();//清空cookies
            clearText();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        catch (Exception ex)
        {
            basetrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("提交审批信息错误:", ex);
        }

    }
    /// <summary>
    /// 清空Cookies
    /// </summary>
    private void ClearCookies()
    {
        /*删除Cookies商户ID*/
        HttpCookie cookiesCustumer = new HttpCookie("Custumer");

        cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
        cookiesCustumer.Values.Add("CustumerID", "");
        Response.AppendCookie(cookiesCustumer);

        /*删除Cookies驳回状态*/
        HttpCookie cookiesDisprove = new HttpCookie("Disprove");

        cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
        cookiesDisprove.Values.Add("DisproveID", "");
        Response.AppendCookie(cookiesDisprove);


        /*删除Cookies工作流ID和节点ID*/
        HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

        cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
        cookiesWorkFlow.Values.Add("WorkFlowID", "");
        cookiesWorkFlow.Values.Add("NodeID", "");
        cookiesWorkFlow.Values.Add("Sequence", "");
        cookiesWorkFlow.Values.Add("VoucherID", "");
        Response.AppendCookie(cookiesWorkFlow);
    }
    protected void clearText()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtCustShortName.Text = "";
        //potCustomer.CustType = cmbCustType.SelectedIndex;
        txtLegalRep.Text = "";
        txtLegalRepTitle.Text = "";
        txtRegCap.Text = "0";
        txtRegAddr.Text = "";
        txtRegCode.Text = "";
        txtTaxCode.Text = "";
        txtBankName.Text = "";
        txtBankAcct.Text = "";
        txtOfficeAddr.Text = "";
        txtPostAddr.Text = "";
        txtPostCode.Text = "";
        txtWeb.Text = "";
        //txtContactorName.Text = "";
        //txtTitle.Text = "";
        //txtOfficeTel.Text = "";
        //txtMobileTel.Text = "";
        //txtEMail.Text = "";
        //txtFax.Text = "";
        txtNote.Text = "";
        txtOfficeAddr2.Text = "";
        txtOfficeAddr3.Text = "";
        txtPostAddr2.Text = "";
        txtPostAddr3.Text = "";
        this.ddlCreditLevel.SelectedIndex = 0;
        this.ddlSourceType.SelectedIndex = 0;
    }
    protected void butOverrule_Click(object sender, EventArgs e)
    {
        try
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;

            int wrkFlwID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["WorkFlowID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["NodeID"]);
            int sequence = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["Sequence"]);
            int voucherID = Convert.ToInt32(Request.Cookies["WorkFlow"].Values["VoucherID"]);
            String voucherHints = txtCustShortName.Text.Trim();
            String voucherMemo = txtNote.Text;

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.RejectVoucherTwoNode(wrkFlwID, nodeID, sequence, vInfo);
            clearText();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            this.ClearCookies();//清空cookies
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("驳回审批信息错误:", ex);
        }
    }

    private void SaveWrkCookies()
    {
        /*保存Cookies工作流ID和节点ID*/
        HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

        cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
        cookiesWorkFlow.Values.Add("WorkFlowID", Request.QueryString["WrkFlwID"]);
        cookiesWorkFlow.Values.Add("NodeID", Request.QueryString["NodeID"]);
        cookiesWorkFlow.Values.Add("Sequence", Request.QueryString["Sequence"]);
        cookiesWorkFlow.Values.Add("VoucherID", Request.QueryString["VoucherID"]);
        Response.AppendCookie(cookiesWorkFlow);
    }
    protected void btnPeople_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "window.showModalDialog('PotCustContact.aspx?look=yes&custID=" + ViewState["custID"] + "','window','dialogWidth=700px;dialogHeight=460px')", true);
    }
}
