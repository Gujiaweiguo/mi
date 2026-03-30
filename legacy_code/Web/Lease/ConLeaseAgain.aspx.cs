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
using System.Text.RegularExpressions;
using Base.Page;

public partial class Lease_ConLeaseAgain : BasePage
{
    #region 定义
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    BaseTrans baseTrans = new BaseTrans();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    DataSet DeductMoneyDS = new DataSet();
    DataTable DeductMoneyDT = new DataTable();
    DataSet KeepMinDS = new DataSet();
    DataTable KeepMinDT = new DataTable();
    private ConLease conLease;
    private ConShop conShop;

    public string emptyStr;
    public string IsFloat;
    public string IsInt;
    #endregion

    #region Page_Load事件
    protected void Page_Load(object sender, EventArgs e)
    {
        #region 设置按钮样式
        btnPutIn.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnPutIn.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");

        btnTempSave.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnTempSave.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");

        IBtnUnitsAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        IBtnUnitsAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");

        btnDispose.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnDispose.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");

        IBtnUnitsDel.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        IBtnUnitsDel.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");


        //公式类表中的按钮
        IBtnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        IBtnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");

        IBtnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        IBtnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");

        IBtnModify.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        IBtnModify.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");

        IBtnDel.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        IBtnDel.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");

        BtnDeductAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        BtnDeductAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        BtnDeductDel.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        BtnDeductDel.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");

        BtnKeepAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        BtnKeepAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        BtnKeepDel.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        BtnKeepDel.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");


        #endregion

        if (!this.IsPostBack)
        {
            #region 初始化DropDownList
            BindDealType();
            BindPenalty();
            BindNotice();
            BindChargeType();
            BindFormulaType();
            BindShopType();
            BindBrand();
            BindBuilding();
            BindArea();
            BindBillCycle();
            BindCurrencyTypeType();
            BindSettleMode();
            BindIfPrepay();
            BindPayTypeId();
            BindTaxType();
            #endregion

            if ((Request["VoucherID"] == "") || (Request["VoucherID"] == null))
            {
                //生成合同ID
                ViewState["contractID"] = BaseApp.GetContractID();
                //设置合同状态



                cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter",Contract.GetContractTypeStatusDesc(Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_FIRST)));
                ViewState["shopFlag"] = "add";
            }
            else
            {
                int contractID = Convert.ToInt32(Request["VoucherID"]);
                ViewState["contractID"] = contractID;
                GetContractInfo(contractID);
                GetLeaseItemInfo();
                GetShopUnits();
                GetConFormulaInfo();
            }
            GetAllShopInfo(1);
            DDownListFloors.Enabled = false;
            DDownListLocation.Enabled = false;

            InitaExpressionsControls(false);
            BindGVType(0);
            BindGVDeductMoney();
            BindGVKeepMin();

            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            IsFloat = (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputFloat");
            IsInt = (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputInt");
            btnTempSave.Attributes.Add("onclick", "return InputValidator(form1)");
            IBtnSave.Attributes.Add("onclick", "return FormulaValidator(form1)");
        }
    }
    #endregion

    #region 获取合同信息
    protected void GetContractInfo(int contractID)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + contractID;
        DataSet contractDs = baseBo.QueryDataSet(new Contract());
        if (contractDs.Tables[0].Rows.Count > 0)
        {
            int flag = Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"]);
            if (flag == Contract.CONTRACTSTATUS_TYPE_FIRST)
            {
                ViewState["myFlag"] = "Inserted";
            }
            else if (flag == Contract.CONTRACTSTATUS_TYPE_TEMP)
            {
                ViewState["myFlag"] = "Updated";
            }
            ViewState["shopFlag"] = "modify";

            //判断首期费用是否可生成





            if (flag == Contract.CONTRACTSTATUS_TYPE_INGEAR)
            {
                //btnFirstCharge.Enabled = true;
                btnTempSave.Enabled = false;
                btnPutIn.Enabled = false;
            }
            else
            {
                //btnFirstCharge.Enabled = false;
            }
            int custId = Convert.ToInt32(contractDs.Tables[0].Rows[0]["CustID"]);
            baseBo.WhereClause = "";
            baseBo.WhereClause = "CustID = " + custId;
            DataSet custDS = baseBo.QueryDataSet(new Customer());
            if (custDS.Tables[0].Rows.Count > 0)
            {
                txtCustCode.Text = custDS.Tables[0].Rows[0]["CustCode"].ToString();
                txtCustName.Text = custDS.Tables[0].Rows[0]["CustName"].ToString();
                txtCustShortName.Text = custDS.Tables[0].Rows[0]["CustShortName"].ToString();
            }
            cmbContractStatus.Text = (String)GetGlobalResourceObject("Parameter",Contract.GetContractTypeStatusDesc(Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"])));
            txtConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            txtChargeStart.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();
            txtNorentDays.Text = contractDs.Tables[0].Rows[0]["NorentDays"].ToString();

            //获取招商员姓名





            baseBo.WhereClause = "";
            baseBo.WhereClause = "UserID = " + contractDs.Tables[0].Rows[0]["CommOper"];
            Resultset rs = baseBo.Query(new Users());
            if (rs.Count > 0)
            {
                Users user = rs.Dequeue() as Users;
                txtCommOper.Text = user.UserName;
            }


            cmbTradeID.SelectedValue = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtBargain.Text = (contractDs.Tables[0].Rows[0]["EConURL"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["EConURL"].ToString());
            listBoxAddItem.Text = (contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString());
            //listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Note"].ToString());
             
            StringBuilder sb = new StringBuilder();
            if (contractDs.Tables[0].Rows[0]["Note"].ToString() != "")
            {
                sb.Append(contractDs.Tables[0].Rows[0]["Note"].ToString());
            }
            //如果有驳回意见，则取驳回意见
            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);

            WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(wrkFlwID, nodeID, sequence);
            string ss = objWrkFlwEntity.VoucherMemo;
            if (ss != "")
            {
                sb.Append("［");
                sb.Append(ss);
                sb.Append("］");
            }
            listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : sb.ToString());

            ViewState["contractID"] = Convert.ToInt32(Request["VoucherID"]);
            ViewState["custId"] = contractDs.Tables[0].Rows[0]["CustID"];
            ViewState["ConStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            ViewState["chargeStartDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            ViewState["ConEndDate"] = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
        }
    }
    #endregion

    #region 获取赁相关条款信息




    //获取赁相关条款信息




    protected void GetLeaseItemInfo()
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBo.Query(new ConLease());
        if (rs.Count == 1)
        {
            ConLease lease = rs.Dequeue() as ConLease;
            DDownListBillCycle.SelectedValue = lease.BillCycle.ToString();
            DDownListCurrencyType.SelectedValue = lease.CurTypeID.ToString();
            DDownListSettleMode.SelectedValue = lease.SettleMode.ToString();
            txtMonthSettleDays.Text = lease.MonthSettleDays.ToString();
            DDownListIfPrepay.SelectedValue = lease.IfPrepay.ToString();
            txtBalanceMonth.Text = lease.BalanceMonth.ToString("yyyy-MM-dd");
            DDownListPayTypeId.SelectedValue = lease.PayTypeID.ToString();
            txtLatePayInt.Text = Convert.ToString(Convert.ToDecimal(lease.LatePayInt) * 1000);
            txtIntDay.Text = lease.IntDay.ToString();
            txtTaxRate.Text = Convert.ToString(Convert.ToDecimal(lease.TaxRate) * 100);
            DDownListTaxType.SelectedValue = lease.TaxType.ToString();
        }
    }
    #endregion

    #region 获取公式列表中的信息

    //获取公式列表中的信息
    protected void GetConFormulaInfo()
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        baseBo.OrderBy = "ChargeTypeID,FStartDate";
        rs = baseBo.Query(new ConFormulaH());
        if (rs.Count > 0)
        {
            ConFormulaH formulaH = rs.Dequeue() as ConFormulaH;
            cmbChargeTypeID.SelectedValue = formulaH.ChargeTypeID.ToString();
            cmbFormulaType.SelectedValue = formulaH.FormulaType.ToString();
            txtBeginDate.Text = formulaH.FStartDate.ToString("yyyy-MM-dd");
            txtOverDate.Text = formulaH.FEndDate.ToString("yyyy-MM-dd");
            txtBaseAmt.Text = formulaH.BaseAmt.ToString();
            rabMonthHire.Checked = false;
            rabDayHire.Checked = false;
            if (formulaH.RateType == ConFormulaH.RATETYPE_TYPE_MONTH)
                rabMonthHire.Checked = true;
            if (formulaH.RateType == ConFormulaH.RATETYPE_TYPE_DAY)
                rabDayHire.Checked = true;
            //txtArea.Text = formulaH.TotalArea.ToString();
            txtUnitHire.Text = formulaH.UnitPrice.ToString();
            txtFixedRental.Text = formulaH.FixedRental.ToString();
            rabFastness.Checked = false;
            rabMultilevel.Checked = false;
            rabMonopole.Checked = false;
            rabFastness2.Checked = false;
            rabLevel.Checked = false;
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_M)
                rabMultilevel.Checked = true;
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_S)
                rabMonopole.Checked = true;
            if (formulaH.MinSumOpt == ConFormulaH.MINSUMOPT_TYPE_FAST)
                rabFastness2.Checked = true;
            if (formulaH.MinSumOpt == ConFormulaH.MINSUMOPT_TYPE_T)
                rabLevel.Checked = true;
            GVType.DataSource = rs;
            GVType.DataBind();
            GVType.Enabled = true;
        }
        else
            GVType.Enabled = false;
        txtArea.Text = ViewState["shopArea"].ToString();
    }
    #endregion

    #region 初始化DropDownList

    //绑定二级经营类别
    protected void BindDealType()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        rs = baseBo.Query(new TradeRelation());
        cmbTradeID.Items.Add(new ListItem(selected));
        foreach (TradeRelation tradeDef in rs)
            cmbTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));

        baseBo.WhereClause = "";

    }

    //绑定提前终约处罚否




    protected void BindPenalty()
    {
        int[] status = Contract.GetPenaltyTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListPenalty.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetPenaltyTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }

    //绑定终约通知期限
    protected void BindNotice()
    {
        int[] status = Contract.GetNotices();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListTerm.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetNoticeDesc(status[i])), status[i].ToString()));
        }
    }

    //绑定费用类别
    protected void BindChargeType()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        rs = baseBo.Query(new ChargeType());
        cmbChargeTypeID.Items.Add(new ListItem(selected, "-1"));
        foreach (ChargeType chargeType in rs)
        {
            cmbChargeTypeID.Items.Add(new ListItem(chargeType.ChargeTypeName, chargeType.ChargeTypeID.ToString()));
        }
    }

    //绑定公式类别
    protected void BindFormulaType()
    {
        string[] status = ConFormulaH.GetFormulaTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));
    }

    //绑定商铺类型
    protected void BindShopType()
    {
        //int[] status = PotShop.GetShopTypeStatus();
        //int s = status.Length;
        //for (int i = 0; i < s; i++)
        //    DDownListShopType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",PotShop.GetShopTypeStatusDesc(status[i])), status[i].ToString()));
    }

    //绑定主营品牌
    protected void BindBrand()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        rs = baseBo.Query(new ConShopBrand());
        DDownListBrand.Items.Add(new ListItem(selected, "-1"));
        foreach (ConShopBrand shopBrand in rs)
            DDownListBrand.Items.Add(new ListItem(shopBrand.BrandName, shopBrand.BrandId.ToString()));
    }

    //绑定大楼
    protected void BindBuilding()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        rs = baseBo.Query(new Building());
        DDownListBuilding.Items.Add(new ListItem(selected, "-1"));
        foreach (Building building in rs)
            DDownListBuilding.Items.Add(new ListItem(building.BuildingName, building.BuildingID.ToString()));
    }

    //绑定楼层名称
    protected void BindFollrs(int bulid)
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        DDownListFloors.Items.Clear();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "BuildingID = " + bulid;
        rs = baseBo.Query(new Floors());
        DDownListFloors.Items.Add(new ListItem(selected, "-1"));
        foreach (Floors floors in rs)
            DDownListFloors.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
    }

    //绑定方位名称
    protected void BindLocation(int floor)
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        DDownListLocation.Items.Clear();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FloorID = " + floor;
        rs = baseBo.Query(new Location());
        DDownListLocation.Items.Add(new ListItem(selected, "-1"));
        foreach (Location loca in rs)
            DDownListLocation.Items.Add(new ListItem(loca.LocationName, loca.LocationID.ToString()));

    }

    //绑定单元
    protected void BindUnits(int tempUnits)
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        DDownListUnit.Items.Clear();
        baseBo.OrderBy = "";
        baseBo.WhereClause = "UnitStatus = " + Units.BLANKOUT_STATUS_INVALID + " and LocationID = " + tempUnits;
        rs = baseBo.Query(new Units());
        DDownListUnit.Items.Add(new ListItem(selected, "-1"));
        foreach (Units units in rs)
            DDownListUnit.Items.Add(new ListItem(units.UnitCode, units.UnitID.ToString()));

    }

    //绑定经营区名称




    protected void BindArea()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBo.WhereClause = "";
        rs = baseBo.Query(new Area());
        DDownListAreaName.Items.Add(new ListItem(selected, "-1"));
        foreach (Area area in rs)
            DDownListAreaName.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
    }

    //绑定结算周期
    protected void BindBillCycle()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        int[] status = ConLease.GetFirstSetAcountMonStatus();
        DDownListBillCycle.Items.Add(new ListItem(selected));
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListBillCycle.Items.Add(new ListItem(ConLease.GetFirstSetAcountMonStatusDesc(status[i]), status[i].ToString()));
    }

    //绑定结算币种
    protected void BindCurrencyTypeType()
    {
        int[] status = ConLease.GetCurrencyTypeTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListCurrencyType.Items.Add(new ListItem(ConLease.GetCurrencyTypeTypeStatusDesc(status[i]), status[i].ToString()));
    }

    //结算处理方式
    protected void BindSettleMode()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        int[] status = ConLease.GetSettleModeTypeStatus();
        DDownListSettleMode.Items.Add(new ListItem(selected));
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListSettleMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetSettleModeTypeStatusDesc(status[i])), status[i].ToString()));
    }

    //是否预收保底
    protected void BindIfPrepay()
    {
        int[] status = ConLease.GetIfPrepayStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListIfPrepay.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetIfPrepayStatusDesc(status[i])), status[i].ToString()));
    }

    //押金方式
    protected void BindPayTypeId()
    {
        //int[] status = ConLease.GetPayTypeIdTypeStatus();
        //int s = status.Length;
        //for (int i = 0; i < s; i++)
        //    DDownListPayTypeId.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetPayTypeIdTypeStatusDesc(status[i])), status[i].ToString()));
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        PayType payType = new PayType();
        baseBo.WhereClause = " PayTypeStatus='" + PayType.ISPAYTYPESTATUS_YES + "' ";
        rs = baseBo.Query(payType);
        foreach (PayType pt in rs)
        {
            DDownListPayTypeId.Items.Add(new ListItem(pt.PayTypeName, pt.PayTypeID.ToString()));
        }
    }

    //发票类型
    protected void BindTaxType()
    {
        int[] status = ConLease.GetTaxTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListTaxType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ConLease.GetTaxTypeStatusDesc(status[i])), status[i].ToString()));
    }

    #endregion

    #region 初始化结算公式页面控件







    protected void InitaExpressionsControls(bool s)
    {
        //定义列表中的控件
        cmbChargeTypeID.Enabled = s;
        cmbFormulaType.Enabled = s;
        txtBeginDate.Enabled = s;
        txtOverDate.Enabled = s;
        txtBaseAmt.Enabled = s;
        IBtnAdd.Enabled = !s;
        IBtnDel.Enabled = !s;
        IBtnModify.Enabled = !s;
        IBtnSave.Enabled = !s;

        //定义内容中的控件
        rabMonthHire.Enabled = s;
        rabDayHire.Enabled = s;
        txtUnitHire.Enabled = s;
        txtFixedRental.Enabled = s;
        rabFastness.Enabled = s;
        rabMonopole.Enabled = s;
        rabMultilevel.Enabled = s;
        rabFastness2.Enabled = s;
        rabLevel.Enabled = s;
        txtForePer.Enabled = s;
        txtFore.Enabled = s;
        txtForeKeep.Enabled = s;
        txtForeKeepMin.Enabled = s;
        GVDeductMoney.Enabled = s;
        GVKeepMin.Enabled = s;
        BtnDeductAdd.Enabled = s;
        BtnDeductDel.Enabled = s;
        BtnKeepDel.Enabled = s;
        BtnKeepAdd.Enabled = s;
    }
    #endregion

    #region GridView绑定
    protected void BindGVType(int condition)
    {
        ds.Clear();
        baseBo.WhereClause = "";
        baseBo.OrderBy = "FEndDate Desc";
        if (condition == 0)
        {
            baseBo.WhereClause = "ContractID = '" + "'";
        }
        else
        {
            baseBo.WhereClause = "ContractID = '" + ViewState["contractID"] + "'";
        }
        ds = baseBo.QueryDataSet(new ConFormulaH());
        int dsCount = ds.Tables[0].Rows.Count;

        dt = ds.Tables[0];
        int count = dt.Rows.Count;

        //获取费用类别名字
        for (int j = 0; j < count; j++)
        {
            baseBo.WhereClause = "";
            baseBo.OrderBy = "";
            baseBo.WhereClause = "ChargeTypeID = " + dt.Rows[j]["ChargeTypeID"];
            DataSet tempDs = new DataSet();
            tempDs = baseBo.QueryDataSet(new ChargeType());
            dt.Rows[j]["ChargeTypeName"] = tempDs.Tables[0].Rows[0]["ChargeTypeName"].ToString();
        }

        //获取公式类别名称
        for (int j = 0; j < count; j++)
        {
            string formulaTypeName = ConFormulaH.GetFormulaTypeStatusDesc(dt.Rows[j]["FormulaType"].ToString());
            dt.Rows[j]["FormulaTypeName"] = formulaTypeName;
        }

        //补空行




        int ss = 10 - count;
        for (int i = 0; i < ss; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GVType.DataSource = dt;
        GVType.DataBind();

        for (int l = 0; l < count; l++)
        {
            string gIntro;
            if (GVType.PageIndex == 0)
            {
                gIntro = GVType.Rows[l].Cells[1].Text.ToString();
                GVType.Rows[l].Cells[1].Text = SubStr(gIntro, 2);
                gIntro = GVType.Rows[l].Cells[4].Text.ToString();
                GVType.Rows[l].Cells[4].Text = SubStr(gIntro, 4);
            }
        }
    }

    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        sNewStr = sNewStr;
        return sNewStr;
    }

    protected void BindGVDeductMoney()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        DeductMoneyDS = baseBo.QueryDataSet(new ConFormulaP());
        DeductMoneyDT = DeductMoneyDS.Tables[0];
        int count = DeductMoneyDT.Rows.Count;

        int ss = 5 - count;
        for (int i = 0; i < ss; i++)
        {
            DeductMoneyDT.Rows.Add(DeductMoneyDT.NewRow());
        }
        GVDeductMoney.DataSource = DeductMoneyDT;
        GVDeductMoney.DataBind();

    }

    protected void BindGVKeepMin()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        KeepMinDS = baseBo.QueryDataSet(new ConFormulaM());
        KeepMinDT = KeepMinDS.Tables[0];
        int count = KeepMinDT.Rows.Count;

        int ss = 5 - count;
        for (int i = 0; i < ss; i++)
        {
            KeepMinDT.Rows.Add(KeepMinDT.NewRow());
        }
        GVKeepMin.DataSource = KeepMinDT;
        GVKeepMin.DataBind();
    }
    #endregion

    #region 保存结算公式

    /// <summary>
    /// 保存结算公式
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void IBtnSave_Click(object sender, EventArgs e)
    {
        ExpressionAddAndModify();
        BindGVType(1);
        InitaExpressionsControls(false);
        GVType.Enabled = false;
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    #endregion

    #region 公式添加按钮事件
    protected void IBtnAdd_Click(object sender, EventArgs e)
    {
        ViewState["ExpressionFlag"] = 0;
        InitaExpressionsControls(true);
        GVType.Enabled = true;
        IBtnAdd.Enabled = true;
        IBtnDel.Enabled = true;
        IBtnModify.Enabled = true;
        IBtnSave.Enabled = true;
        BtnDeductAdd.Enabled = false;
        BtnDeductDel.Enabled = false;
        BtnKeepDel.Enabled = false;
        BtnKeepAdd.Enabled = false;
        txtFore.Enabled = false;
        txtForePer.Enabled = false;
        txtForeKeepMin.Enabled = false;
        txtForeKeep.Enabled = false;

        if (dt.Rows.Count > 0)
        {
            DateTime dateTime = Convert.ToDateTime(dt.Rows[0]["FEndDate"]);
            dateTime.AddDays(1);
            txtBeginDate.Text = dateTime.ToString();
            txtBaseAmt.Text = dt.Rows[0]["BaseAmt"].ToString();
            txtArea.Text = dt.Rows[0]["TotalArea"].ToString();
            txtUnitHire.Text = dt.Rows[0]["UnitPrice"].ToString();
            txtFixedRental.Text = dt.Rows[0]["FixedRental"].ToString();
            if (rabFastness.Checked == true)
            {
                txtForePer.Text = DeductMoneyDT.Rows[0]["Pcent"].ToString();
                txtFore.Text = DeductMoneyDT.Rows[0]["SalesTo"].ToString();
            }
            else
            {
                txtForePer.Text = "";
                txtFore.Text = "";
            }
            if (rabFastness2.Checked == true)
            {
                txtForeKeepMin.Text = KeepMinDT.Rows[0]["SalesTo"].ToString();
                txtForeKeep.Text = KeepMinDT.Rows[0]["MinSum"].ToString();
            }
            else
            {
                txtForeKeepMin.Text = "";
                txtForeKeep.Text = "";
            }
        }
        else
        {
            //txtBeginDate.Text = Convert.ToDateTime(ViewState["chargeStartDate"]).ToString("yyyy-MM-dd");
            //txtOverDate.Text = Convert.ToDateTime(ViewState["ConEndDate"]).ToString("yyyy-MM-dd");
            txtBaseAmt.Text = "0";
            txtUnitHire.Text = "0";
            txtFixedRental.Text = "0";
        }
        rabMonthHire.Checked = true;
        rabFastness.Checked = true;
        rabFastness2.Checked = true;
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3);", true);

    }
    #endregion

    #region 公式编辑按钮事件
    protected void IBtnModify_Click(object sender, EventArgs e)
    {

        ViewState["ExpressionFlag"] = 1;
        InitaExpressionsControls(true);
        GVType.Enabled = true;
        IBtnAdd.Enabled = true;
        IBtnDel.Enabled = true;
        IBtnModify.Enabled = true;
        IBtnSave.Enabled = true;

        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);

    }
    #endregion

    #region 公式删除按钮事件
    protected void IBtnDel_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        rs = baseBo.Query(new ConFormulaP());
        if (rs.Count > 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('该记录存在抽成关系，不能删除！')", true);
        }
        rs = baseBo.Query(new ConFormulaM());
        if (rs.Count > 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('该记录存在保底关系，不能删除！')", true);
        }
        else if (Convert.ToInt32(ViewState["formulaHID"]) != 0)
        {
            if (baseBo.Delete(new ConFormulaH()) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDelete") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
            }
        }

        BindGVType(1);
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    #endregion

    #region 抽成添加
    protected void BtnDeductAdd_Click(object sender, EventArgs e)
    {
        ConFormulaP formulaP = new ConFormulaP();
        formulaP.ConFormulaPID = BaseApp.GetConFormulaPID();
        formulaP.FormulaID = Convert.ToInt32(ViewState["formulaHID"]);
        formulaP.SalesTo = Convert.ToDecimal(txtFore.Text);
        formulaP.Pcent = Convert.ToDecimal(txtForePer.Text) / 100;
        if (baseBo.Insert(formulaP) != -1)
        {
            //提示
        }
        BindGVDeductMoney();
        txtFore.Text = "";
        txtForePer.Text = "";
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    #endregion

    #region 删除抽成
    protected void BtnDeductDel_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ConFormulaPID = " + Convert.ToInt32(ViewState["deduct"]);
        if (baseBo.Delete(new ConFormulaP()) != -1)
        {
            //删除成功！




        }
        txtFore.Text = "";
        txtForePer.Text = "";
        BindGVDeductMoney();
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    #endregion

    #region 保底添加
    protected void BtnKeepAdd_Click(object sender, EventArgs e)
    {
        ConFormulaM formulaM = new ConFormulaM();
        formulaM.ConFormulaMID = BaseApp.GetConFormulaMID();
        formulaM.FormulaID = Convert.ToInt32(ViewState["formulaHID"]);
        formulaM.SalesTo = Convert.ToDecimal(txtForeKeepMin.Text);
        formulaM.MinSum = Convert.ToDecimal(txtForeKeep.Text);
        if (baseBo.Insert(formulaM) != -1)
        {
            //提示
        }

        BindGVKeepMin();
        txtForeKeep.Text = "";
        txtForeKeepMin.Text = "";
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    #endregion

    #region 删除保底
    protected void BtnKeepDel_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ConFormulaMID = " + Convert.ToInt32(ViewState["KeepMin"]);
        if (baseBo.Delete(new ConFormulaM()) != -1)
        {
            //删除成功！




        }
        txtForeKeepMin.Text = "";
        txtForeKeep.Text = "";
        BindGVKeepMin();
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    #endregion

    #region 获取选定的大楼







    protected void DDownListBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDownListBuilding.SelectedValue.ToString() != "-1")
        {
            DDownListFloors.Enabled = true;
            int builder = Convert.ToInt32(DDownListBuilding.SelectedValue);
            BindFollrs(builder);
        }
        else
        {
            DDownListFloors.Enabled = false;
            DDownListLocation.Enabled = false;
            DDownListUnit.Enabled = false;
            BindFollrs(-1);
            BindLocation(-1);
            BindUnits(-1);
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    #endregion

    #region 获取选定的楼层







    protected void DDownListFloors_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDownListFloors.SelectedValue.ToString() != "-1")
        {
            DDownListLocation.Enabled = true;
            int floorID = Convert.ToInt32(DDownListFloors.SelectedValue);
            BindLocation(floorID);
        }
        else
        {
            DDownListLocation.Enabled = false;
            DDownListUnit.Enabled = false;
            BindLocation(-1);
            BindUnits(-1);
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    #endregion

    #region 获取单元信息
    protected void DDownListLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDownListLocation.SelectedValue.ToString() != "-1")
        {
            DDownListUnit.Enabled = true;
            int units = Convert.ToInt32(DDownListLocation.SelectedValue);
            BindUnits(units);
        }
        else
        {
            DDownListUnit.Enabled = false;
            BindUnits(-1);
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    #endregion

    #region js中调用按钮事件








    //公式列表
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet tempDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
        tempDs = baseBo.QueryDataSet(new ConFormulaH());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            cmbChargeTypeID.SelectedValue = tempDs.Tables[0].Rows[0]["ChargeTypeID"].ToString();
            cmbFormulaType.SelectedValue = tempDs.Tables[0].Rows[0]["FormulaType"].ToString();
            txtBeginDate.Text = Convert.ToDateTime(tempDs.Tables[0].Rows[0]["FStartDate"]).ToString("yyyy-MM-dd").ToString();
            txtOverDate.Text = Convert.ToDateTime(tempDs.Tables[0].Rows[0]["FEndDate"]).ToString("yyyy-MM-dd").ToString();
            txtBaseAmt.Text = tempDs.Tables[0].Rows[0]["BaseAmt"].ToString();
            txtArea.Text = tempDs.Tables[0].Rows[0]["TotalArea"].ToString();
            txtUnitHire.Text = tempDs.Tables[0].Rows[0]["UnitPrice"].ToString();
            txtFixedRental.Text = tempDs.Tables[0].Rows[0]["FixedRental"].ToString();

            if (tempDs.Tables[0].Rows[0]["RateType"].ToString() == ConFormulaH.RATETYPE_TYPE_MONTH)
                rabMonthHire.Checked = true;
            if (tempDs.Tables[0].Rows[0]["RateType"].ToString() == ConFormulaH.RATETYPE_TYPE_DAY)
                rabDayHire.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == ConFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == ConFormulaH.PCENTOPT_TYPE_S)
                rabMonopole.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == ConFormulaH.PCENTOPT_TYPE_M)
                rabMultilevel.Checked = true;
            if (tempDs.Tables[0].Rows[0]["MinSumOpt"].ToString() == ConFormulaH.MINSUMOPT_TYPE_FAST)
                rabFastness2.Checked = true;
            if (tempDs.Tables[0].Rows[0]["MinSumOpt"].ToString() == ConFormulaH.MINSUMOPT_TYPE_T)
                rabLevel.Checked = true;

            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
            tempDs.Clear();
            tempDs = baseBo.QueryDataSet(new ConFormulaP());
            DeductMoneyDT = tempDs.Tables[0];
            int count1 = DeductMoneyDT.Rows.Count;
            int ss1 = 5 - count1;
            for (int i = 0; i < ss1; i++)
            {
                DeductMoneyDT.Rows.Add(DeductMoneyDT.NewRow());
            }
            GVDeductMoney.DataSource = DeductMoneyDT;
            GVDeductMoney.DataBind();

            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
            tempDs.Clear();
            tempDs = baseBo.QueryDataSet(new ConFormulaM());
            KeepMinDT = tempDs.Tables[0];
            int count2 = KeepMinDT.Rows.Count;
            int ss2 = 5 - count2;
            for (int j = 0; j < ss2; j++)
            {
                KeepMinDT.Rows.Add(KeepMinDT.NewRow());
            }
            GVKeepMin.DataSource = KeepMinDT;
            GVKeepMin.DataBind();

            txtFore.Enabled = true;
            txtForePer.Enabled = true;
            txtForeKeepMin.Enabled = true;
            txtForeKeep.Enabled = true;
            BtnDeductAdd.Enabled = true;
            BtnDeductDel.Enabled = true;
            BtnKeepDel.Enabled = true;
            BtnKeepAdd.Enabled = true;
            GVDeductMoney.Enabled = true;
            GVKeepMin.Enabled = true;

            ViewState["formulaHID"] = this.Hidden1.Value;
        }
        else
        {
            txtFore.Enabled = false;
            txtForePer.Enabled = false;
            txtForeKeepMin.Enabled = false;
            txtForeKeep.Enabled = false;
            BtnDeductAdd.Enabled = false;
            BtnDeductDel.Enabled = false;
            BtnKeepDel.Enabled = false;
            BtnKeepAdd.Enabled = false;
            GVDeductMoney.Enabled = false;
            GVKeepMin.Enabled = false;
        }

        txtFore.Text = "";
        txtForePer.Text = "";
        txtForeKeepMin.Text = "";
        txtForeKeep.Text = "";
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }

    //抽成列表
    protected void lBtnP_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ConFormulaPID = " + HiddenDeduct1.Value;
        DataSet tempDs = baseBo.QueryDataSet(new ConFormulaP());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            txtFore.Text = tempDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtForePer.Text = tempDs.Tables[0].Rows[0]["Pcent"].ToString();
            BtnDeductDel.Enabled = true;
            ViewState["deduct"] = HiddenDeduct1.Value;
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }

    //保底列表
    protected void lBtnM_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + HiddenKeepMin1.Value;
        DataSet tempDs = baseBo.QueryDataSet(new ConFormulaM());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            txtForeKeepMin.Text = tempDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtForeKeep.Text = tempDs.Tables[0].Rows[0]["MinSum"].ToString();
            BtnKeepDel.Enabled = true;
            ViewState["KeepMin"] = HiddenKeepMin1.Value;
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);

    }
    #endregion

    #region GridView的RowDataBound
    protected void GVType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");
            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEvent('" + e.Row.Cells[0].Text + "')");
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
        }

    }
    protected void GVType_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    #endregion

    #region 作废
    //作废
    protected void btnDispose_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    #endregion

    #region 执行事物保存草稿
    //保存草稿
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        baseTrans.BeginTrans();

        try
        {
            if (SaveBaseBargain() == -1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('合同基本信息保存草稿失败！')", true);
            }
            if (InsertOrUpdateLeaseItemInfo() == -1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('合同相关条款保存草稿失败！')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('保存草稿失败！')", true);
        }


        baseTrans.Commit();
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);

    }


    #endregion

    #region 提交审批
    //提交审批
    protected void btnPutIn_Click(object sender, EventArgs e)
    {
        //保存合同
        baseTrans.BeginTrans();
        try
        {
            if (SaveBaseBargain() == -1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('合同基本信息保存草稿失败！')", true);
            }
            if (InsertOrUpdateLeaseItemInfo() == -1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('合同相关条款保存草稿失败！')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDelete") + "'", true);
            }
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('保存草稿失败！')", true);
        }
        baseTrans.Commit();
        GetAllShopInfo(0);
        //提交审批
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(ViewState["contractID"]);
        String voucherHints = txtCustShortName.Text.Trim();
        String voucherMemo = "";
        //int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

        //WrkFlwApp.CommitVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), vInfo);
        WrkFlwApp.ConfirmVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]), vInfo);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    #endregion

    #region 抽成列表RowDataBound事件
    protected void GVDeductMoney_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");
            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEventDeduct('" + e.Row.Cells[0].Text + "')");
        }
    }
    #endregion

    #region 保底列表RowDataBound事件
    protected void GVKeepMin_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");
            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEventKeepMin('" + e.Row.Cells[0].Text + "')");
        }
    }
    #endregion

    #region 公式的添加和修改
    protected void ExpressionAddAndModify()
    {
        ConFormulaH formula = new ConFormulaH();

        formula.ContractID = Convert.ToInt32(ViewState["contractID"]);
        formula.ChargeTypeID = Convert.ToInt32(cmbChargeTypeID.SelectedValue);
        formula.FormulaType = cmbFormulaType.SelectedValue;
        formula.FStartDate = Convert.ToDateTime(txtBeginDate.Text);
        formula.FEndDate = Convert.ToDateTime(txtOverDate.Text);
        formula.BaseAmt = Convert.ToDecimal(txtBaseAmt.Text);
        if (rabMonthHire.Checked == true)
            formula.RateType = ConFormulaH.RATETYPE_TYPE_MONTH;
        if (rabDayHire.Checked == true)
            formula.RateType = ConFormulaH.RATETYPE_TYPE_DAY;
        if (rabFastness.Checked == true)
            formula.PcentOpt = ConFormulaH.PCENTOPT_TYPE_FAST;
        if (rabMonopole.Checked == true)
            formula.PcentOpt = ConFormulaH.PCENTOPT_TYPE_S;
        if (rabMultilevel.Checked == true)
            formula.PcentOpt = ConFormulaH.PCENTOPT_TYPE_M;
        if (rabFastness2.Checked == true)
            formula.MinSumOpt = ConFormulaH.MINSUMOPT_TYPE_FAST;
        if (rabLevel.Checked == true)
            formula.MinSumOpt = ConFormulaH.MINSUMOPT_TYPE_T;
        string ss = Hidden_txtArea.Value;
        formula.TotalArea = Convert.ToDecimal(ViewState["shopArea"]);
        formula.UnitPrice = Convert.ToDecimal(txtUnitHire.Text);
        formula.FixedRental = Convert.ToDecimal(txtFixedRental.Text);


        ViewState["FormulaID"] = formula.FormulaID;

        if (Convert.ToInt32(ViewState["ExpressionFlag"]) == 0)
        {
            formula.FormulaID = BaseApp.GetFormulaID();
            if (baseBo.Insert(formula) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            else
            {
                //提示保存失败
            }
        }
        if (Convert.ToInt32(ViewState["ExpressionFlag"]) == 1)
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);

            if (baseBo.Update(formula) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }

        }
        BindGVType(1);
    }
    #endregion

    #region 获取合同对应的商铺




    private void GetAllShopInfo(int FirstStatus)
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        DataSet shopDs = baseBo.QueryDataSet(new ConShop());
        DataTable shopDt = shopDs.Tables[0];
        int shopCount = shopDt.Rows.Count;
        decimal ss = 0;
        for (int j = 0; j < shopCount; j++)
        {
            ss += Convert.ToDecimal(shopDt.Rows[j]["RentArea"]);
        }
        ViewState["shopArea"] = ss;  //所有商铺面积之和




        int countNull = 13 - shopCount;
        for (int i = 0; i < countNull; i++)
        {
            shopDt.Rows.Add(shopDt.NewRow());
        }
        gvShop.DataSource = shopDt;
        gvShop.DataBind();
        int gvCount = gvShop.Rows.Count;
        for (int j = shopCount; j < gvCount; j++)
            gvShop.Rows[j].Cells[3].Text = "";

        if (FirstStatus == 1)
        {
            if (shopCount > 0)
            {
                GetShopBaseInfo(Convert.ToInt32(shopDt.Rows[0]["ShopID"]));
            }
        }
    }
    #endregion

    #region 获取商铺基本信息
    //获取商铺基本信息
    protected void GetShopBaseInfo(int shopId)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ShopId = " + shopId;
        //baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBo.Query(new ConShop());
        if (rs.Count == 1)
        {
            ConShop shop = rs.Dequeue() as ConShop;
            txtShopName.Text = shop.ShopName.ToString();
            DDownListShopType.SelectedValue = shop.ShopTypeID.ToString();
            txtRentArea.Text = shop.RentArea.ToString();
            DDownListAreaName.SelectedValue = shop.AreaId.ToString();
            txtStartDate.Text = shop.ShopStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = shop.ShopEndDate.ToString("yyyy-MM-dd");

            txtShopCode.Text = (shop.ShopCode == null ? "" : shop.ShopCode.ToString());
            txtContactName.Text = (shop.ContactorName == null ? "" : shop.ContactorName.ToString());
            txtContactTel.Text = (shop.Tel == null ? "" : shop.Tel.ToString());
            DDownListBuilding.SelectedValue = (shop.BuildingID == 0 ? "-1" : shop.BuildingID.ToString());
            DDownListBrand.SelectedValue = (shop.BrandID == 0 ? "-1" : shop.BrandID.ToString());

            if (DDownListBuilding.SelectedValue.ToString() != "-1")
            {
                int bulid = Convert.ToInt32(DDownListBuilding.SelectedValue);
                BindFollrs(bulid);
                DDownListFloors.SelectedValue = (shop.FloorID == null ? "" : shop.FloorID.ToString());
            }
            else
                BindFollrs(-1);
            if (DDownListFloors.SelectedValue.ToString() != "-1")
            {
                int floor = Convert.ToInt32(DDownListFloors.SelectedValue);
                BindLocation(floor);
                DDownListLocation.SelectedValue = (shop.LocationID == null ? "" : shop.LocationID.ToString());
            }
            else
                BindLocation(-1);
            ViewState["ShopId"] = shop.ShopId;
        }
    }
    #endregion

    #region 保存草稿

    //基本合同项目保存草稿
    protected int SaveBaseBargain()
    {
        int result;
        ds.Clear();
        baseBo.WhereClause = "TradeID = '" + Convert.ToInt32(cmbTradeID.SelectedValue) + "'";
        ds = baseBo.QueryDataSet(new TradeRelation());

        Contract contact = new Contract();
        contact.ContractID = Convert.ToInt32(ViewState["contractID"]);
        contact.CustID = Convert.ToInt32(ViewState["custId"]);
        contact.ContractCode = txtContractCode.Text;
        contact.RefID = txtRefID.Text.Trim();
        contact.ConStartDate = Convert.ToDateTime(txtConStartDate.Text);
        contact.ConEndDate = Convert.ToDateTime(txtConEndDate.Text);
        contact.ChargeStartDate = Convert.ToDateTime(txtChargeStart.Text);
        contact.TradeID = Convert.ToInt32(cmbTradeID.SelectedValue);
        contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_TEMP);
        contact.Penalty = Convert.ToInt32(DDownListPenalty.SelectedValue);
        //contact.Penalty = Convert.ToInt32(txtOverItem.Text);
        contact.Notice = Convert.ToInt32(DDownListTerm.SelectedValue);
        contact.AdditionalItem = listBoxAddItem.Text;
        contact.EConURL = txtBargain.Text;
        contact.Note = listBoxRemark.Text;
        contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
        contact.NorentDays = Convert.ToInt32(Hidden_txtNorentDays.Value);

        result = baseTrans.Insert(contact);
        return result;
    }
    #endregion

    #region 判断是添加还是修改租赁相关信息





    protected int InsertOrUpdateLeaseItemInfo()
    {
        FillLeaseItemInfo();
        int result = baseTrans.Insert(conLease);
        return result;
    }
    #endregion

    #region 填充租赁相关条款信息

    //填充租赁相关条款信息
    protected void FillLeaseItemInfo()
    {
        conLease = new ConLease();
        conLease.ContractID = Convert.ToInt32(ViewState["contractID"]);
        conLease.BillCycle = Convert.ToInt32(DDownListBillCycle.SelectedValue);
        conLease.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);
        conLease.SettleMode = Convert.ToInt32(DDownListSettleMode.SelectedValue);
        conLease.MonthSettleDays = float.Parse(txtMonthSettleDays.Text.Trim());
        conLease.IfPrepay = Convert.ToInt32(DDownListIfPrepay.SelectedValue);
        conLease.BalanceMonth = Convert.ToDateTime(txtBalanceMonth.Text);
        conLease.PayTypeID = Convert.ToInt32(DDownListPayTypeId.SelectedValue);
        conLease.LatePayInt = Convert.ToDecimal(txtLatePayInt.Text) / 1000;
        conLease.IntDay = Convert.ToInt32(txtIntDay.Text);
        conLease.TaxRate = Convert.ToDecimal(txtTaxRate.Text) / 100;
        conLease.TaxType = Convert.ToInt32(DDownListTaxType.SelectedValue);
        conLease.RentInc = "";
    }
    #endregion

    //获取商铺信息中的单元信息
    protected void GetShopUnits()
    {
        //移除原有的商铺信息



        int Index = 0;
        int unitCount = ListBoxUnits.Items.Count;
        for (int i = 0; i < unitCount; i++)
        {
            ListItem item = ListBoxUnits.Items[Index];
            ListBoxUnits.Items.Remove(item);
            Index = 0;
        }

        baseBo.WhereClause = "";
        baseBo.WhereClause = "ShopID = " + Convert.ToInt32(ViewState["ShopId"]);
        DataSet shopDS = baseBo.QueryDataSet(new ConShopUnit());
        int count = shopDS.Tables[0].Rows.Count;
        if (count > 0)
        {
            DDownListUnit.Enabled = true;
            Resultset myrs = new Resultset();
            for (int i = 0; i < count; i++)
            {
                baseBo.WhereClause = "";
                //baseBo.WhereClause = "UnitID = " + shopDS.Tables[0].Rows[i]["UnitID"];
                string sql = "select UnitID,UnitCode + ' : ' + cast(UseArea as varchar(50)) as UnitCodeArea from Unit where UnitID = " + Convert.ToInt32(shopDS.Tables[0].Rows[i]["UnitID"]);
                DataSet unitDS = baseBo.QueryDataSet(sql);
                ListItem mylistItem = new ListItem();
                mylistItem.Text = unitDS.Tables[0].Rows[0]["UnitCodeArea"].ToString();
                mylistItem.Value = unitDS.Tables[0].Rows[0]["UnitID"].ToString();
                //DDownListUnit.Items.Add(mylistItem);
                ListBoxUnits.Items.Add(mylistItem);

            }
            ViewState["UnitID"] = shopDS.Tables[0].Rows[0]["UnitID"];
        }
        else
            DDownListUnit.Enabled = false;
    }

    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
        ViewState["delShopID"] = shopId;
        GetShopBaseInfo(shopId);
        GetShopUnits();
        GetAllShopInfo(0);
        ViewState["shopFlag"] = "modify";
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }

    #region 商铺信息中的单元信息从单元集合中删除
    protected void IBtnUnitsDel_Click(object sender, EventArgs e)
    {
        int Index = 0;
        for (int i = 0; i < ListBoxUnits.Items.Count; i++)
        {
            ListItem item = ListBoxUnits.Items[Index];
            if (ListBoxUnits.Items[Index].Selected == true)
            {
                ListBoxUnits.Items.Remove(item);
            }
            Index++;
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    #endregion

    #region 商铺信息单元集信息添加




    protected void IBtnUnitsAdd_Click(object sender, EventArgs e)
    {
        if (DDownListUnit.SelectedValue.ToString() != "-1")
        {
            BaseBO basebo = new BaseBO();
            basebo.WhereClause = "UnitID = " + Convert.ToInt32(DDownListUnit.SelectedItem.ToString());
            string sql = "select UnitID,UnitCode + ' : ' + cast(UseArea as varchar(50)) as UnitCodeArea from Unit where UnitID = " + Convert.ToInt32(DDownListUnit.SelectedValue.ToString());
            DataSet UnitDS = basebo.QueryDataSet(sql);
            //ListBoxUnits.Items.Add(new ListItem(DDownListUnit.SelectedItem.ToString(), DDownListUnit.SelectedValue.ToString()));
            ListBoxUnits.Items.Add(new ListItem(UnitDS.Tables[0].Rows[0]["UnitCodeArea"].ToString(), UnitDS.Tables[0].Rows[0]["UnitID"].ToString()));
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_SelUserfullInfo") + "'", true);
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    #endregion

    protected void btnShopSave_Click(object sender, EventArgs e)
    {
        baseTrans.BeginTrans();
        try
        {
            if (InsertOrUpdateShopBaseInfo() == -1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('商铺基本信息保存草稿失败！')", true);
            }
            if (SaveShopUnits() == -1)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('商铺基本信息保存草稿失败！')", true);
            }
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
        }
        baseTrans.Commit();
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }

    #region 添加或修改商铺基本信息




    protected int InsertOrUpdateShopBaseInfo()
    {
        int result = 0;
        if (ViewState["shopFlag"].ToString() == "add")
        {
            FillShopBaseInfo();
            conShop.ShopId = BaseApp.GetShopID();
            result = baseTrans.Insert(conShop);
        }
        else if (ViewState["shopFlag"].ToString() == "modify")
        {
            //GetShopBaseInfo();
            FillShopBaseInfo();
            conShop.ShopId = Convert.ToInt32(ViewState["ShopId"]);
            baseTrans.WhereClause = "";
            baseTrans.WhereClause = "ShopID = " + Convert.ToInt32(ViewState["ShopId"]);
            result = baseTrans.Update(conShop);
        }
        return result;
    }
    #endregion

    #region 商铺信息基本内容中的单元信息保存草稿

    //商铺信息基本内容中的单元信息保存草稿
    protected int SaveShopUnits()
    {
        int result = 0;
        Units units = new Units();
        UnitsStutas unitsStutas = new UnitsStutas();
        ConShopUnit conShopUnit = new ConShopUnit();

        //将删除的单元改成未出租状态




        baseBo.WhereClause = "";
        baseBo.WhereClause = "ShopID = " + ViewState["ShopId"];
        Resultset tempRs = baseBo.Query(conShopUnit);
        if (tempRs.Count > 0)
        {
            foreach (ConShopUnit shopUnit in tempRs)
            {
                baseTrans.WhereClause = "";
                baseTrans.WhereClause = "UnitID = " + shopUnit.UnitID;
                unitsStutas.UnitStatus = Units.BLANKOUT_STATUS_INVALID;
                int l = baseTrans.Update(unitsStutas);
            }
        }

        //先将商铺对应的单元删除,再插入已选好的单元




        baseTrans.WhereClause = "";
        baseTrans.WhereClause = "ShopID = " + ViewState["ShopId"];

        if (baseTrans.Delete(conShopUnit) != -1)
        {
            int count = ListBoxUnits.Items.Count;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                conShopUnit.ShopID = Convert.ToInt32(ViewState["ShopId"]);
                conShopUnit.UnitID = Convert.ToInt32(ListBoxUnits.Items[index].Value);
                //conShopUnit.RentArea = Convert.ToDecimal(ViewState["RentArea"]);
                conShopUnit.RentStatus = ConShopUnit.RENTSTATUS_TYPE_YES;
                result = baseTrans.Insert(conShopUnit);

                //修改单元状态




                baseTrans.WhereClause = "";
                baseTrans.WhereClause = "UnitID = " + Convert.ToInt32(ListBoxUnits.Items[index].Value);
                unitsStutas.UnitStatus = Units.BLANKOUT_STATUS_LEASEOUT;
                int x = baseTrans.Update(unitsStutas);
                index++;
            }
        }
        return result;
    }
    #endregion

    #region 填充商铺基本信息

    //填充商铺基本信息
    protected void FillShopBaseInfo()
    {


        conShop = new ConShop();
        ViewState["ShopId"] = BaseApp.GetShopID();
        conShop.ShopCode = txtShopCode.Text;
        conShop.ShopName = txtShopName.Text;
        conShop.ShopTypeID = Convert.ToInt32(DDownListShopType.SelectedValue);
        conShop.BrandID = Convert.ToInt32(DDownListBrand.SelectedValue);
        conShop.RentArea = Convert.ToDecimal(txtRentArea.Text);
        conShop.AreaId = Convert.ToInt32(DDownListAreaName.SelectedValue);
        conShop.ShopStartDate = Convert.ToDateTime(txtStartDate.Text);
        conShop.ShopEndDate = Convert.ToDateTime(txtEndDate.Text);
        conShop.ContactorName = txtContactName.Text;
        conShop.Tel = txtContactTel.Text;
        conShop.BuildingID = Convert.ToInt32(DDownListBuilding.SelectedValue);
        conShop.FloorID = Convert.ToInt32(DDownListFloors.SelectedValue);
        conShop.LocationID = Convert.ToInt32(DDownListLocation.SelectedValue);
        conShop.CreateTime = DateTime.Now;
        conShop.ModifyTime = DateTime.Now;
        conShop.ContractID = Convert.ToInt32(ViewState["contractID"]);


        ViewState["RentArea"] = conShop.RentArea;

    }
    #endregion

    protected void imgCustCodeQ_Click(object sender, EventArgs e)
    {
        if (txtCustCode.Text != "")
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "CustCode= '" + txtCustCode.Text.Trim() + "'";
            Resultset rs = baseBo.Query(new Customer());
            if (rs.Count == 1)
            {
                Customer customer = rs.Dequeue() as Customer;
                txtCustName.Text = customer.CustName;
                txtCustShortName.Text = customer.CustShortName;
                ViewState["custId"] = customer.CustID;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_NoCustCode") + "'", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void imgCustNameQ_Click(object sender, EventArgs e)
    {
        if (txtCustShortName.Text != "")
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "CustShortName like '%" + txtCustShortName.Text.Trim() + "%'";
            Resultset rs = baseBo.Query(new Customer());
            if (rs.Count == 1)
            {
                Customer customer = rs.Dequeue() as Customer;
                ViewState["custId"] = customer.CustID;
                txtCustCode.Text = customer.CustCode;
                txtCustName.Text = customer.CustName;
                txtCustShortName.Text = customer.CustShortName;
            }
            else if (rs.Count > 1)
            {
                string str = "selectCust()";
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_NoCustShortName") + "'", true);
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", str, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void ImaBtnOK_Click(object sender, EventArgs e)
    {
        string s = allvalue.Value.ToString();
        string[] ss = Regex.Split(s, ",");

        string custID = ss[0].ToString();
        string custCode = ss[1].ToString();
        string custName = ss[2].ToString();
        string custShortName = ss[3].ToString();

        if (custID == "")
        {
            return;
        }
        ViewState["custId"] = custID;
        txtCustCode.Text = custCode;
        txtCustName.Text = custName;
        txtCustShortName.Text = custShortName;
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void btnShopAdd_Click(object sender, EventArgs e)
    {
        txtShopCode.Text = "";
        txtShopName.Text = "";
        BindShopType();
        DDownListBrand.SelectedValue = "-1";
        txtRentArea.Text = "";
        DDownListAreaName.SelectedValue = "-1";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtContactName.Text = "";
        txtContactTel.Text = "";
        DDownListBuilding.SelectedValue = "-1";
        DDownListFloors.Items.Clear();
        DDownListLocation.Items.Clear();
        DDownListUnit.Items.Clear();
        ListBoxUnits.Items.Clear();

        ViewState["shopFlag"] = "add";
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
    protected void btnShopDel_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "ShopID = " + Convert.ToInt32(ViewState["delShopID"]);
        if (baseBo.Delete(new ConShop()) != -1)
        {
        }
        GetAllShopInfo(1);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
    }
}
