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
using BaseInfo.User;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using Lease.Contract;
using Invoice;
using Base.Page;

public partial class Lease_ConLeasePalaver : BasePage
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

        //btnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");

        //btnQuit.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnQuit.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");

        //btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");

        btnDispose.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnDispose.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");

        IBtnUnitsDel.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        IBtnUnitsDel.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");

        #endregion

        if (!this.IsPostBack)
        {
            #region 初始化DropDownList
            BindDealType();
            BindPenalty();
            BindNotice();
            BindChargeType();
            BindShopType();
            BindBrand();
            BindBuilding();
            BindFollrs();
            BindLocation();
            BindUnits();
            BindArea();
            BindBillCycle();
            BindCurrencyTypeType();
            BindSettleMode();
            BindIfPrepay();
            BindPayTypeId();
            BindTaxType();
            BindFormulaType();
            #endregion

            int contractID = Convert.ToInt32(Request["VoucherID"]);
            GetContractInfo(contractID);
            GetLeaseItemInfo();
            GetAllShopInfo(1);
            //GetShopBaseInfo();
            GetShopUnits();
            GetConFormulaInfo();

            //bool canSmtToMgr = WrkFlwApp.CanSmtToMgr(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]));
            //if (canSmtToMgr)
            //{
            //    this.btnPutIn.Visible = true;
            //}
            BindGVType();
            BindGVDeductMoney();
            BindGVKeepMin();

            btnDispose.Attributes.Add("onclick", "return ListBoxValidator(form1)");

        }

    }
    #endregion

    #region GridView绑定
    protected void BindGVType()
    {
        ds.Clear();
        baseBo.WhereClause = "";
        baseBo.OrderBy = "FEndDate Desc";
        baseBo.WhereClause = "ContractID = '" + ViewState["contractID"] + "'";
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

    #region 初始化DropDownList

    //绑定二级经营类别
    protected void BindDealType()
    {
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        rs = baseBo.Query(new TradeRelation());
        cmbTradeID.Items.Add(new ListItem("--请选择--"));
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
            DDownListPenalty.Items.Add(new ListItem(Contract.GetPenaltyTypeStatusDesc(status[i]), status[i].ToString()));
        }
    }

    //绑定终约通知期限
    protected void BindNotice()
    {
        int[] status = Contract.GetNotices();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            DDownListTerm.Items.Add(new ListItem(Contract.GetNoticeDesc(status[i]), status[i].ToString()));
        }
    }

    //绑定费用类别
    protected void BindChargeType()
    {
        rs = baseBo.Query(new ChargeType());
        cmbChargeTypeID.Items.Add(new ListItem("--请选择--"));
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
            cmbFormulaType.Items.Add(new ListItem(ConFormulaH.GetFormulaTypeStatusDesc(status[i]), status[i].ToString()));
    }

    //绑定商铺类型
    protected void BindShopType()
    {
        //int[] status = PotShop.GetShopTypeStatus();
        //int s = status.Length;
        //for (int i = 0; i < s; i++)
        //    DDownListShopType.Items.Add(new ListItem(PotShop.GetShopTypeStatusDesc(status[i]), status[i].ToString()));
    }

    //绑定主营品牌
    protected void BindBrand()
    {
        rs = baseBo.Query(new ConShopBrand());
        DDownListBrand.Items.Add(new ListItem("--请选择--"));
        foreach (ConShopBrand shopBrand in rs)
            DDownListBrand.Items.Add(new ListItem(shopBrand.BrandName, shopBrand.BrandId.ToString()));
    }

    //绑定大楼
    protected void BindBuilding()
    {
        rs = baseBo.Query(new Building());
        DDownListBuilding.Items.Add(new ListItem("--请选择--"));
        foreach (Building building in rs)
            DDownListBuilding.Items.Add(new ListItem(building.BuildingName, building.BuildingID.ToString()));
    }

    //绑定楼层名称
    protected void BindFollrs()
    {
        rs = baseBo.Query(new Floors());
        DDownListFloors.Items.Add(new ListItem("--请选择--"));
        foreach (Floors floors in rs)
            DDownListFloors.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
    }

    //绑定方位名称
    protected void BindLocation()
    {
        rs = baseBo.Query(new Location());
        DDownListLocation.Items.Add(new ListItem("--请选择--"));
        foreach (Location loca in rs)
            DDownListLocation.Items.Add(new ListItem(loca.LocationName, loca.LocationID.ToString()));

    }

    //绑定单元
    protected void BindUnits()
    {
        baseBo.WhereClause = "UnitStatus = " + Units.UNIT_STATUS_VALID;
        rs = baseBo.Query(new Units());
        DDownListUnit.Items.Add(new ListItem("--请选择--"));
        foreach (Units units in rs)
            DDownListUnit.Items.Add(new ListItem(units.UnitCode, units.UnitID.ToString()));

    }

    //绑定经营区名称


    protected void BindArea()
    {
        baseBo.WhereClause = "";
        rs = baseBo.Query(new Area());
        DDownListAreaName.Items.Add(new ListItem("--请选择--"));
        foreach (Area area in rs)
            DDownListAreaName.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
    }

    //绑定结算周期
    protected void BindBillCycle()
    {
        int[] status = ConLease.GetFirstSetAcountMonStatus();
        DDownListBillCycle.Items.Add(new ListItem("--请选择--"));
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
        int[] status = ConLease.GetSettleModeTypeStatus();
        DDownListSettleMode.Items.Add(new ListItem("--请选择--"));
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListSettleMode.Items.Add(new ListItem(ConLease.GetSettleModeTypeStatusDesc(status[i]), status[i].ToString()));
    }

    //是否预收保底
    protected void BindIfPrepay()
    {
        int[] status = ConLease.GetIfPrepayStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListIfPrepay.Items.Add(new ListItem(ConLease.GetIfPrepayStatusDesc(status[i]), status[i].ToString()));
    }

    //首次结算计帐月


    //protected void BindBalanceMonth()
    //{
    //    int[] status = ConLease.GetFirstSetAcountMonStatus();
    //    int s = status.Length;
    //    for (int i = 0; i < s; i++)
    //        DDownListBalanceMonth.Items.Add(new ListItem(ConLease.GetFirstSetAcountMonStatusDesc(status[i]), status[i].ToString()));
    //}

    //押金方式
    protected void BindPayTypeId()
    {
        //int[] status = ConLease.GetPayTypeIdTypeStatus();
        //int s = status.Length;
        //for (int i = 0; i < s; i++)
        //    DDownListPayTypeId.Items.Add(new ListItem(ConLease.GetPayTypeIdTypeStatusDesc(status[i]), status[i].ToString()));
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
            DDownListTaxType.Items.Add(new ListItem(ConLease.GetTaxTypeStatusDesc(status[i]), status[i].ToString()));
    }

    #endregion

  
    protected void GVType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");
            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEvent('" + e.Row.Cells[0].Text + "')");
        }

    }
    protected void GVType_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    //获取合同信息
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
            int custId = Convert.ToInt32(contractDs.Tables[0].Rows[0]["CustID"]);
            baseBo.WhereClause = "";
            baseBo.WhereClause = "CustID = " + custId;
            DataSet custDS = baseBo.QueryDataSet(new Customer());
            if (custDS.Tables[0].Rows.Count > 0)
            {
                txtCustCode.Text = custDS.Tables[0].Rows[0]["CustCode"].ToString();
                txtCustName.Text = custDS.Tables[0].Rows[0]["CustName"].ToString();
                txtCustShortName.Text = custDS.Tables[0].Rows[0]["CustName"].ToString();
            }
            cmbContractStatus.Text = Contract.GetContractTypeStatusDesc(Convert.ToInt32(contractDs.Tables[0].Rows[0]["ContractStatus"].ToString()));
            txtConStartDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConStartDate"]).ToString("yyyy-MM-dd");
            txtConEndDate.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ConEndDate"]).ToString("yyyy-MM-dd");
            txtChargeStart.Text = Convert.ToDateTime(contractDs.Tables[0].Rows[0]["ChargeStartDate"]).ToString("yyyy-MM-dd");
            //listBoxAddItem.Text = contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString();

            cmbTradeID.Text = (contractDs.Tables[0].Rows[0]["TradeID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["TradeID"].ToString());
            txtContractCode.Text = (contractDs.Tables[0].Rows[0]["ContractCode"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["ContractCode"].ToString());
            txtRefID.Text = (contractDs.Tables[0].Rows[0]["RefID"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["RefID"].ToString());
            DDownListPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            DDownListTerm.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtBargain.Text = (contractDs.Tables[0].Rows[0]["EConURL"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["EConURL"].ToString());
            //listBoxAddItem.Text = (contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString());
            //listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Note"].ToString());

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

            ViewState["contractID"] = Convert.ToInt32(Request["VoucherID"]);
            ViewState["custId"] = contractDs.Tables[0].Rows[0]["CustID"];
        }
    }

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
            txtLatePayInt.Text = (Convert.ToDecimal(lease.LatePayInt)*1000).ToString();
            txtIntDay.Text = lease.IntDay.ToString();
            txtTaxRate.Text = (Convert.ToDecimal(lease.TaxRate)*100).ToString();
            DDownListTaxType.SelectedValue = lease.TaxType.ToString();
        }
    }

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
            txtShopCode.Text = shop.ShopCode.ToString();
            txtShopName.Text = shop.ShopName.ToString();
            DDownListShopType.SelectedValue = shop.ShopTypeID.ToString();
            DDownListBrand.SelectedValue = shop.BrandID.ToString();
            txtRentArea.Text = shop.RentArea.ToString();
            DDownListAreaName.SelectedValue = shop.AreaId.ToString();
            txtStartDate.Text = shop.ShopStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = shop.ShopEndDate.ToString("yyyy-MM-dd");
            txtContactName.Text = (shop.ContactorName == null ? "" : shop.ContactorName.ToString());
            txtContactTel.Text = (shop.Tel == null ? "" : shop.Tel.ToString());
            DDownListBuilding.SelectedValue = shop.BuildingID.ToString();
            DDownListFloors.SelectedValue = shop.FloorID.ToString();
            DDownListLocation.SelectedValue = shop.LocationID.ToString();

            ViewState["ShopId"] = shop.ShopId;
        }

    }

    //获取商铺信息中的单元信息
    protected void GetShopUnits()
    {
        //移除原有的商铺信息


        int Index = 0;
        for (int i = 0; i < ListBoxUnits.Items.Count; i++)
        {
            ListItem item = ListBoxUnits.Items[Index];
            ListBoxUnits.Items.Remove(item);
            Index++;
        }

        baseBo.WhereClause = "";
        baseBo.WhereClause = "ShopID = " + Convert.ToInt32(ViewState["ShopId"]);
        DataSet shopDS = baseBo.QueryDataSet(new ConShopUnit());
        int count = shopDS.Tables[0].Rows.Count;
        if (count > 0)
        {
            Resultset myrs = new Resultset();
            for (int i = 0; i < count; i++)
            {
                baseBo.WhereClause = "";
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

    }

    //获取公式列表中的信息
    protected void GetConFormulaInfo()
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBo.Query(new ConFormulaH());
        if (rs.Count > 0)
        {
            ConFormulaH formulaH = rs.Dequeue() as ConFormulaH;
            cmbChargeTypeID.SelectedValue = formulaH.ChargeTypeID.ToString();
            cmbFormulaType.SelectedValue = formulaH.FormulaType.ToString();
            txtBeginDate.Text = formulaH.FStartDate.ToString("yyyy-MM-dd");
            txtOverDate.Text = formulaH.FEndDate.ToString("yyyy-MM-dd");
            txtBaseAmt.Text = formulaH.BaseAmt.ToString();
            if (formulaH.RateType == ConFormulaH.RATETYPE_TYPE_MONTH)
                rabMonthHire.Checked = true;
            if (formulaH.RateType == ConFormulaH.RATETYPE_TYPE_DAY)
                rabDayHire.Checked = true;
            txtArea.Text = formulaH.TotalArea.ToString();
            txtUnitHire.Text = formulaH.UnitPrice.ToString();
            txtFixedRental.Text = formulaH.FixedRental.ToString();
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
        {
            GVType.Enabled = false;
            BindGVType();
        }
    }

    /// <summary>
    /// 驳回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDispose_Click(object sender, EventArgs e)
    {
        if (listBoxRemark.Text != "")
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int voucherID = Convert.ToInt32(ViewState["contractID"]);
            int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);


            WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(wrkFlwID, nodeID, sequence);

            if ((WrkFlwEntity.NODE_STATUS_REJECT_PENDING == objWrkFlwEntity.NodeStatus) || (WrkFlwEntity.NODE_STATUS_NORMAL_PENDING == objWrkFlwEntity.NodeStatus))
            {
                String str = "window.open('../Test/Default3.aspx?" + "WrkFlwID=" + wrkFlwID + "&VoucherID=" + voucherID + "&Sequence=" + sequence + "&NodeID=" + nodeID + "&VoucherMemo=" + listBoxRemark.Text.Trim() + "','正常驳回操作',height=200,width=400,status=1,toolbar=0,menubar=0);";
                //Response.Write(str);
                //Page.RegisterClientScriptBlock("", str);
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", str, true);
            }
            else if (WrkFlwEntity.NODE_STATUS_MGR_PENDING == objWrkFlwEntity.NodeStatus)
            {
                String voucherHints = txtCustName.Text.Trim();
                String voucherMemo = listBoxRemark.Text.Trim();
                voucherID = Convert.ToInt32(ViewState["contractID"]);
                int operatorID = objSessionUser.UserID;
                int deptID = objSessionUser.DeptID;
                VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
                WrkFlwApp.MgrRejectVoucher(wrkFlwID, nodeID, sequence, vInfo);
                Response.Write("<script language=javascript>alert('操作成功!!');</script>");
            }
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }

    /// <summary>
    /// 同意
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(ViewState["contractID"]);
        String voucherHints = txtCustName.Text.Trim();
        String voucherMemo = "";

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

        WrkFlwApp.ConfirmVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]), vInfo);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_PalaverYes") + "'", true);
    
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);

    }

    /// <summary>
    /// 提交领导审批
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPutIn_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        int deptID = objSessionUser.DeptID;
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);

        String voucherHints = txtCustName.Text.Trim();
        String voucherMemo = listBoxRemark.Text.Trim();
        int voucherID = Convert.ToInt32(ViewState["contractID"]);
        int operatorID = objSessionUser.UserID;
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
        WrkFlwApp.SmtToMgr(wrkFlwID, nodeID, sequence, vInfo);
        Response.Write("<script language=javascript>alert('操作成功!!');</script>");
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }

    protected void IBtnUnitsDel_Click(object sender, EventArgs e)
    {
        int count = ListBoxUnits.Items.Count;
        int Index = 0;
        for (int i = 0; i < count; i++)
        {
            ListItem item = ListBoxUnits.Items[Index];
            if (ListBoxUnits.Items[Index].Selected == true)
            {
                ListBoxUnits.Items.Remove(item);
            }
            Index++;
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }

    protected void IBtnUnitsAdd_Click(object sender, EventArgs e)
    {
        ListBoxUnits.Items.Add(new ListItem(DDownListUnit.SelectedItem.ToString(), DDownListUnit.SelectedValue.ToString()));
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }

    #region GridView单击事件
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        DataSet tempDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + this.Hidden2.Value + "'";
        tempDs = baseBo.QueryDataSet(new ConFormulaH());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            cmbChargeTypeID.SelectedValue = tempDs.Tables[0].Rows[0]["ChargeTypeID"].ToString();
            cmbFormulaType.SelectedValue = tempDs.Tables[0].Rows[0]["FormulaType"].ToString();
            txtBeginDate.Text = tempDs.Tables[0].Rows[0]["FStartDate"].ToString();
            txtOverDate.Text = tempDs.Tables[0].Rows[0]["FEndDate"].ToString();
            txtBaseAmt.Text = tempDs.Tables[0].Rows[0]["BaseAmt"].ToString();
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
            baseBo.WhereClause = "FormulaID = '" + this.Hidden2.Value + "'";
            tempDs.Clear();
            tempDs = baseBo.QueryDataSet(new ConFormulaP());
            this.GVDeductMoney.DataSource = tempDs;
            this.GVDeductMoney.DataBind();

            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = '" + this.Hidden2.Value + "'";
            tempDs.Clear();
            tempDs = baseBo.QueryDataSet(new ConFormulaM());
            this.GVKeepMin.DataSource = tempDs;
            this.GVKeepMin.DataBind();
            ViewState["formulaHID"] = this.Hidden2.Value;
        }
       
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    #endregion

    }
    protected void DDownListBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DDownListFloors_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DDownListLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
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
            txtBeginDate.Text = tempDs.Tables[0].Rows[0]["FStartDate"].ToString();
            txtOverDate.Text = tempDs.Tables[0].Rows[0]["FEndDate"].ToString();
            txtBaseAmt.Text = tempDs.Tables[0].Rows[0]["BaseAmt"].ToString();
            txtArea.Text = tempDs.Tables[0].Rows[0]["TotalArea"].ToString();
            txtUnitHire.Text = tempDs.Tables[0].Rows[0]["UnitPrice"].ToString();
            txtFixedRental.Text = tempDs.Tables[0].Rows[0]["FixedRental"].ToString();
            rabMonthHire.Checked = false;
            rabDayHire.Checked = false;
            rabFastness.Checked = false;
            rabMultilevel.Checked = false;
            rabMonopole.Checked = false;
            rabFastness2.Checked = false;
            rabLevel.Checked = false;
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

            GVDeductMoney.Enabled = true;
            GVKeepMin.Enabled = true;

            ViewState["formulaHID"] = this.Hidden1.Value;
        }
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void GVDeductMoney_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GVKeepMin_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void btnDeduct_Click(object sender, EventArgs e)
    {

    }
    protected void btnKeepMin_Click(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        
    }

    #region 获取合同对应的商铺


    private void GetAllShopInfo(int FirstStatus)
    {
        baseBo.WhereClause = "";
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


        int countNull = 11 - shopCount;
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
            GetShopBaseInfo(Convert.ToInt32(shopDt.Rows[0]["ShopID"]));
        }

    }
    #endregion

    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
        GetShopBaseInfo(shopId);
        GetShopUnits();
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
}
