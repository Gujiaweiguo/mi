using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;
using Lease.Subs;

public partial class ReportM_RptInv_RptInvChargeDetail : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            this.BindBizProject();//绑定商业项目
            this.BindSubs();//绑定子公司
            this.BindContractStatus();//绑定合同状态
        }
    }
    /// <summary>
    /// 绑定子公司
    /// </summary>
    private void BindSubs()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Subs());
        this.ddlSubs.Items.Add(new ListItem("", ""));
        foreach (Subs subs in rs)
        {
            this.ddlSubs.Items.Add(new ListItem(subs.SubsShortName,subs.SubsID.ToString()));
        }
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }
    }
    /// <summary>
    /// 绑定合同状态
    /// </summary>
    private void BindContractStatus()
    {
        //int[] status = Lease.Contract.Contract.GetContractTypeStatus();
        //foreach (int sta in status)
        //{
        //    this.ddlContractStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Lease.Contract.Contract.GetContractTypeStatusDesc(sta)), sta.ToString()));
        //}
        this.ddlContractStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter","CONTRACTSTATUS_TYPE_INGEAR"),"2"));
        this.ddlContractStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END"), "4"));
    }
    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*绑定大楼*/
        ddlBuildingName.Items.Clear();
        ddlFloorName.Items.Clear();
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + "AND StoreId='" + ddlBizproject.SelectedValue + "'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
        {
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        }
    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFloorName.Items.Clear();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "FloorStatus = " + Floors.FLOOR_STATUS_VALID + " AND BuildingID = '" + ddlBuildingName.SelectedValue + "'";
        Resultset rs1 = baseBO.Query(new Floors());
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
        {
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
        }
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_ContractExpensesDetail");//标题
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();//集团名称
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXStoreName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");//项目名称
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXTotal";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblTotal");//总计
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].ParameterFieldName = "REXsAmt";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ContractsAmt");//应收
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXsActPayAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ContractsActPayAmt");//已收
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXsActnotPayAmt";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ContractsActnotPayAmt");//欠款
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXSubsName";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "MakePoolVoucher_lblSubs");//子公司名称
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXContractCode";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID");//合同号
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXCustName";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustName");//客户名称
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXConStartDate";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopStartDate");//开始日期
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXConEndDate";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_EDate");//结束日期
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXInvPeriod";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "InvAdj_KeepAccountsMth");//记账月
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXShopName";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");//商铺名称
        paraField[13].CurrentValues.Add(discreteValue[13]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = @"SELECT Contract.ContractCode,Customer.CustName,Contract.ContractID,Contract.SubsID,Subsidiary.SubsName,Conshop.StoreID,Store.StoreName,Conshop.BuildingID,Building.BuildingName,Conshop.FloorID,Floors.FloorName,Conshop.ShopID,ConShop.ShopCode,ConShop.ShopName,Contract.ConStartDate,Contract.ConEndDate,Contract.ContractStatus,CurrencyType.CurTypeName,invoiceHeader.InvID,sPay.InvPeriod as InvPeriod,sPay.ChargeTypeName,ISNULL(sPay.InvActPayAmt,0) as sAmt,ISNULL(sPay.InvPaidAmt,0) as sactpayAmt,ISNULL(sPay.InvNotPayAmt,0) as sactnotpayAmt FROM invoiceHeader INNER JOIN ( SELECT SUM(InvoiceDetail.InvActPayAmt) AS InvActPayAmt, SUM(InvoiceDetail.InvPaidAmt) as InvPaidAmt,SUM(InvoiceDetail.InvActPayAmt)-SUM(InvoiceDetail.InvPaidAmt) as InvNotPayAmt,invhe.InvPeriod,ContractID,InvoiceDetail.ChargeTypeID,ct.ChargeTypeName,invhe.InvID FROM invoiceHeader as invhe INNER JOIN InvoiceDetail ON (InvoiceDetail.InvID = invhe.InvID) 
INNER JOIN ChargeType as ct  ON (InvoiceDetail.ChargeTypeID = ct.ChargeTypeID) group by ContractID,InvoiceDetail.ChargeTypeID,ct.ChargeTypeName,invhe.InvID,invhe.InvPeriod) as sPay ON (invoiceHeader.InvID = sPay.InvID) INNER JOIN Contract ON (invoiceHeader.contractID = Contract.ContractID ) INNER JOIN Customer ON (Contract.CustID = Customer.CustID) INNER JOIN CurrencyType ON (CurrencyType.CurTypeId=invoiceheader.InvCurTypeId) INNER JOIN  ConShop ON (Contract.contractID = ConShop.contractID) INNER JOIN Store ON (ConShop.StoreID=Store.StoreID) INNER JOIN Subsidiary ON (Contract.SubsID=Subsidiary.SubsID) INNER JOIN Building ON (Conshop.BuildingID=Building.BuildingID) INNER JOIN Floors ON (Conshop.FloorID=Floors.FloorID) where 1=1 ";

        if (this.ddlSubs.Text != "")//子公司
        {
            str_sql = str_sql + "AND Contract.SubsID='" + this.ddlSubs.SelectedValue + "'";
        }
        if (ddlBizproject.Text != "")//商业项目
        {
            str_sql = str_sql + "AND Conshop.StoreID='" + ddlBizproject.SelectedValue + "'";
        }
        if (this.ddlBuildingName.Text != "")//大楼名称
        {
            str_sql = str_sql + "AND Conshop.BuildingID='" + this.ddlBuildingName.SelectedValue + "'";
        }
        if (this.ddlFloorName.Text != "")//楼层名称
        {
            str_sql = str_sql + "AND Conshop.FloorID='" + this.ddlFloorName.SelectedValue + "'";
        }
        if (this.txtConractCode.Text.Trim() != "")//合同号
        {
            str_sql = str_sql + "AND Contract.ContractCode='" + this.txtConractCode.Text.Trim() + "'";
        }
        if (this.ddlContractStatus.Text != "")//合同状态
        {
            str_sql = str_sql + "AND Contract.ContractStatus='" + this.ddlContractStatus.SelectedValue + "'";
        }
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
            //for (int i = 0; i < 5; i++)
            //{
            //    strAuth = strAuth.Replace("ConShop", strTable);//将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
            //}
        }
        str_sql = str_sql + strAuth;
        
        //str_sql = str_sql + " order by storeid asc";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptInvChargeDetail.rpt";

    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptInv/RptInvChargeDetail.aspx");
    }
}
