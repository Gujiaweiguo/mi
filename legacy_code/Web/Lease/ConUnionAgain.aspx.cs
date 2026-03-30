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
using Lease.Union;
using Lease.Contract;
using BaseInfo.User;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using System.Text;
using System.Text.RegularExpressions;
using Base.Page;

public partial class Lease_ConUnionAgain : BasePage
{
    #region 定义
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    BaseTrans baseTrans = new BaseTrans();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    DataSet DeductMoneyDS = new DataSet();
    DataTable DeductMoneyDT = new DataTable();
    private ConUnion conUnion;
    private ConShop conShop;
    //string SaveOrUpdate = "";

    public string emptyStr;
    public string IsFloat;
    public string IsInt;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        #region 设置按钮样式
        btnPutIn.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnPutIn.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");

        btnTempSave.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnTempSave.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");

        IBtnUnitsAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        IBtnUnitsAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");

        btnDispos.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';");
        btnDispos.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';");

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
        #endregion

        if (!this.IsPostBack)
        {

            //初始化DropDownList
            BindDealType();
            BindPenalty();
            BindNotice();
            BindChargeType();
            BindExpressionsType();
            BindBillCycle();
            BindCurrencyTypeType();
            BindTaxType();
            BindBrand();
            BindBuilding();
            BindShopType();
            BindAccount();
            BindArea();

            GetAllShopInfo(1);

            //生成合同ID
            ViewState["contractID"] = BaseApp.GetContractID();

            //设置合同状态



            cmbContractStatus.Text = Contract.GetContractTypeStatusDesc(Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_FIRST));

            DDownListFloors.Enabled = false;
            DDownListLocation.Enabled = false;

            SetTableControlEnable(false);
            SetContentControlEnable(true);
            //绑定GridView
            BindGVConFormulaH(0);
            BindGVDeductMoney();

            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            IsFloat = (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputFloat");
            IsInt = (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputInt");
            btnTempSave.Attributes.Add("onclick","return InputValidator(form1)");
            IBtnSave.Attributes.Add("onclick","return FormulaValidator(form1)");
        }
    }

    #region 绑定DropDownList

    //绑定二级经营类别
    protected void BindDealType()
    {
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        rs = baseBo.Query(new TradeRelation());
        cmbTradeID.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Select_Select")));
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
            cmbPenalty.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Contract.GetPenaltyTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }

    //绑定终约通知期限
    protected void BindNotice()
    {
        int[] status = Contract.GetNotices();
        int s = status.Length;
        for (int i = 0; i < s; i++)
        {
            cmbNotice.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Contract.GetNoticeDesc(status[i])), status[i].ToString()));
        }
    }

    //绑定费用类别
    protected void BindChargeType()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        rs = baseBo.Query(new ChargeType());
        cmbChargeTypeID.Items.Add(new ListItem(selected));
        foreach (ChargeType chargeType in rs)
        {
            cmbChargeTypeID.Items.Add(new ListItem(chargeType.ChargeTypeName, chargeType.ChargeTypeID.ToString()));
        }
    }

    //绑定公式类别
    protected void BindExpressionsType()
    {
        string[] status = UnioFormulaH.GetFormulaTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            cmbFormulaType.Items.Add(new ListItem(UnioFormulaH.GetFormulaTypeStatusDesc(status[i]), status[i].ToString()));
    }

    //绑定结算周期
    protected void BindBillCycle()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        int[] status = ConUnion.GetFirstSetAcountMonStatus();
        DDownListBillCycle.Items.Add(new ListItem(selected));
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListBillCycle.Items.Add(new ListItem(ConLease.GetFirstSetAcountMonStatusDesc(status[i]), status[i].ToString()));
    }

    //绑定结算币种
    protected void BindCurrencyTypeType()
    {
        //int[] status = ConUnion.GetCurrencyTypeTypeStatus();
        //int s = status.Length;
        //for (int i = 0; i < s; i++)
        //    DDownListCurrencyType.Items.Add(new ListItem(ConLease.GetCurrencyTypeTypeStatusDesc(status[i]), status[i].ToString()));

        baseBo.WhereClause = "";
        rs = baseBo.Query(new CurrencyType());
        foreach (CurrencyType curType in rs)
            DDownListCurrencyType.Items.Add(new ListItem(curType.CurTypeName.ToString(), curType.CurTypeID.ToString()));
    }

    //发票类型
    protected void BindTaxType()
    {
        int[] status = ConUnion.GetTaxTypeStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListTaxType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",ConLease.GetTaxTypeStatusDesc(status[i])), status[i].ToString()));
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
        baseBo.WhereClause = "UnitStatus = " + Units.BLANKOUT_STATUS_INVALID + " and LocationID = " + tempUnits;
        rs = baseBo.Query(new Units());
        DDownListUnit.Items.Add(new ListItem(selected, "-1"));
        foreach (Units units in rs)
            DDownListUnit.Items.Add(new ListItem(units.UnitCode, units.UnitID.ToString()));

    }

    //绑定商铺类型
    protected void BindShopType()
    {
        //int[] status = PotShop.GetShopTypeStatus();
        //int s = status.Length;
        //for (int i = 0; i < s; i++)
        //    DDownListShopType.Items.Add(new ListItem(PotShop.GetShopTypeStatusDesc(status[i]), status[i].ToString()));
    }


    //帐期
    protected void BindAccount()
    {
        int[] status = ConUnion.GetAccountCycleStatus();
        int s = status.Length;
        for (int i = 0; i < s; i++)
            DDownListAccountCycle.Items.Add(new ListItem(ConUnion.GetAccountCycleStatusDesc(status[i]), status[i].ToString()));
    }

    protected void BindArea()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        baseBo.WhereClause = "";
        rs = baseBo.Query(new Area());
        DDownListAreaName.Items.Add(new ListItem(selected));
        foreach (Area area in rs)
            DDownListAreaName.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
    }


    #endregion

    #region 绑定GridView
    //绑定公式列表
    protected void BindGVConFormulaH(int condition)
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
            baseBo.WhereClause = "ContractID = " + ViewState["contractID"];
        }
        ds = baseBo.QueryDataSet(new ConFormulaH());

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
            if (tempDs.Tables[0].Rows.Count == 1)
            {
                dt.Rows[j]["ChargeTypeName"] = tempDs.Tables[0].Rows[0]["ChargeTypeName"].ToString();
            }
        }

        //获取公式类别名称
        for (int j = 0; j < count; j++)
        {
            string formulaTypeName = ConFormulaH.GetFormulaTypeStatusDesc(dt.Rows[j]["FormulaType"].ToString());
            dt.Rows[j]["FormulaTypeName"] = formulaTypeName;
        }

        //补空行



        int ss = 8 - count;
        for (int i = 0; i < ss; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GVConFormulaH.DataSource = dt;
        GVConFormulaH.DataBind();

        for (int l = 0; l < count; l++)
        {
            string gIntro;
            if (GVConFormulaH.PageIndex == 0)
            {
                gIntro = GVConFormulaH.Rows[l].Cells[1].Text.ToString();
                GVConFormulaH.Rows[l].Cells[1].Text = SubStr(gIntro, 2);
                gIntro = GVConFormulaH.Rows[l].Cells[4].Text.ToString();
                GVConFormulaH.Rows[l].Cells[4].Text = SubStr(gIntro, 4);
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


    //绑定抽成列表
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
    #endregion

    #region 设置公式定义控件是否可编辑



    private void SetTableControlEnable(bool s)
    {
        cmbChargeTypeID.Enabled = s;
        cmbFormulaType.Enabled = s;
        txtStartDate.Enabled = s;
        txtEndDate.Enabled = s;
        //butSave.Enabled = s;
        //butAdd.Enabled = s;
        //butUpdate.Enabled = s;
        //butDelete.Enabled = s;       
    }

    private void SetContentControlEnable(bool t)
    {
        txtMinSumSell.Enabled = !t;
        txtBalanceDeduct.Enabled = !t;
        rabFastness.Enabled = !t;
        rabMonopole.Enabled = !t;
        rabMultilevel.Enabled = !t;
        txtSellCount.Enabled = !t;
        txtDistill.Enabled = !t;
        BtnDeductAdd.Enabled = !t;
        BtnDeductDel.Enabled = !t;
        //IBtnTakeAdd.Enabled = !t;
        //IBtnTakeDel.Enabled = !t;
    }

    #endregion

    #region 保存和修改结算公式




    //保存和修改公式列表



    protected void SaveOrUpdateFormulaH()
    {
        UnioFormulaH formula = new UnioFormulaH();
        formula.FormulaID = BaseApp.GetFormulaID();
        formula.ContractID = Convert.ToInt32(ViewState["contractID"]);
        formula.ChargeTypeID = Convert.ToInt32(cmbChargeTypeID.SelectedValue);
        formula.FormulaType = cmbFormulaType.SelectedValue;
        formula.FStartDate = Convert.ToDateTime(txtStartDate.Text);
        formula.FEndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rabFastness.Checked == true)
            formula.PcentOpt = UnioFormulaH.PCENTOPT_TYPE_FAST;
        if (rabMonopole.Checked == true)
            formula.PcentOpt = UnioFormulaH.PCENTOPT_TYPE_S;
        if (rabMultilevel.Checked == true)
            formula.PcentOpt = UnioFormulaH.PCENTOPT_TYPE_M;

        ViewState["FormulaID"] = formula.FormulaID;

        if (ViewState["SaveOrUpdate"].ToString() == "Save")
        {
            if (baseTrans.Insert(formula) != -1)
            {
                //提示保存成功
            }
            else
            {
                //提示保存失败
            }
        }
        if (ViewState["SaveOrUpdate"] == "Update")
        {
            baseTrans.WhereClause = "";
            baseTrans.WhereClause = "FormulaID = " + ViewState["formulaHID"];
            baseTrans.Update(formula);
        }


    }

    //保存和修改保底表
    protected void SaveOrUpdateFormulaM()
    {
        ConFormulaM formulaM = new ConFormulaM();
        formulaM.FormulaID = Convert.ToInt32(ViewState["FormulaID"]);
        formulaM.SalesTo = Convert.ToDecimal(txtMinSumSell.Text);
        formulaM.MinSum = Convert.ToDecimal(txtBalanceDeduct.Text);
        formulaM.ConFormulaMID = BaseApp.GetConFormulaMID();
        if (baseTrans.Insert(formulaM) != -1)
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
        }
        else
        {
        }
    }

    #endregion

    #region 页面控件事件

    protected void butSaveTemp_Click(object sender, EventArgs e)
    {
        /*baseTrans.BeginTrans();
        SaveBaseBargain();
        SaveUnioItem();
        SaveShopBaseInfo();
        SaveShopUnits();
        baseTrans.Commit();*/
    }
    #endregion

    protected void GVConFormulaH_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");

            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEvent('" + e.Row.Cells[0].Text + "')");
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
        }

    }


    protected void GVConFormulaH_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

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
        contact.RefID = txtRefID.Text;
        contact.ConStartDate = Convert.ToDateTime(txtConStartDate.Text);
        contact.ConEndDate = Convert.ToDateTime(txtConEndDate.Text);
        contact.ChargeStartDate = Convert.ToDateTime(txtChargeStart.Text);
        contact.TradeID = Convert.ToInt32(cmbTradeID.SelectedValue);
        contact.ContractStatus = Convert.ToInt32(Contract.CONTRACTSTATUS_TYPE_TEMP);
        contact.Penalty = Convert.ToInt32(cmbPenalty.SelectedValue);
        //contact.Penalty = Convert.ToInt32(txtOverItem.Text);
        contact.Notice = Convert.ToInt32(cmbNotice.SelectedValue);
        contact.AdditionalItem = listBoxAddItem.Text;
        contact.EConURL = txtEBargain.Text;
        contact.Note = listBoxRemark.Text;
        contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["PTradeID"]);
        contact.NorentDays = Convert.ToInt32(Hidden_txtNorentDays.Value);

        ViewState["ConStartDate"] = contact.ConStartDate;
        ViewState["ConEndDate"] = contact.ConEndDate;

        result = baseTrans.Insert(contact);
        return result;
    }

    //判断是添加还是修改联营相关信息



    protected int InsertOrUpdateLeaseItemInfo()
    {
        FillLeaseItemInfo();
        int result = baseTrans.Insert(conUnion);
        return result;
    }

    //填充租赁相关条款信息
    protected void FillLeaseItemInfo()
    {
        conUnion = new ConUnion();
        conUnion.ContractID = Convert.ToInt32(ViewState["contractID"]);
        conUnion.BillCycle = Convert.ToInt32(DDownListBillCycle.SelectedValue);
        conUnion.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);
        conUnion.AccountCycle = Convert.ToInt32(DDownListAccountCycle.SelectedValue);
        conUnion.RentInc = txtRentInc.Text;
        conUnion.TaxRate = Convert.ToDecimal(txtTaxRate.Text);
        conUnion.TaxType = Convert.ToInt32(DDownListTaxType.SelectedValue);

    }

    //添加或修改商铺基本信息



    protected int InsertOrUpdateShopBaseInfo()
    {
        FillShopBaseInfo();
        int result = baseTrans.Insert(conShop);
        return result;
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
            txtShopName.Text = shop.ShopName.ToString();
            DDownListShopType.SelectedValue = shop.ShopTypeID.ToString();
            txtRentArea.Text = shop.RentArea.ToString();
            DDownListAreaName.SelectedValue = shop.AreaId.ToString();
            txtStartDate.Text = shop.ShopStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = shop.ShopEndDate.ToString("yyyy-MM-dd");

            txtShopCode.Text = (shop.ShopCode == null ? "" : shop.ShopCode.ToString());
            txtContactName.Text = (shop.ContactorName == null ? "" : shop.ContactorName.ToString());
            txtContactTel.Text = (shop.Tel == null ? "" : shop.Tel.ToString());
            DDownListBuilding.SelectedValue = (shop.BuildingID == null ? "" : shop.BuildingID.ToString());
            DDownListBrand.SelectedValue = (shop.BrandID == null ? "" : shop.BrandID.ToString());

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
        GetAllShopInfo(0);
    }

    //填充商铺基本信息
    protected void FillShopBaseInfo()
    {
        conShop = new ConShop();
        ViewState["ShopId"] = BaseApp.GetShopID();
        conShop.ShopId = Convert.ToInt32(ViewState["ShopId"]);
        conShop.ShopCode = txtShopCode.Text;
        conShop.ShopName = txtShopName.Text;
        conShop.ShopTypeID = Convert.ToInt32(DDownListShopType.SelectedValue);
        conShop.BrandID = Convert.ToInt32(DDownListBrand.SelectedValue);
        conShop.RentArea = Convert.ToDecimal(txtRentArea.Text);
        conShop.AreaId = Convert.ToInt32(DDownListAreaName.SelectedValue);
        conShop.ShopStartDate = Convert.ToDateTime(txtShopStartDate.Text);
        conShop.ShopEndDate = Convert.ToDateTime(txtShopEndDate.Text);
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

    protected void btnPutIn_Click(object sender, EventArgs e)
    {
        //保存合同
        baseTrans.BeginTrans();
        try
        {
            if (SaveBaseBargain() == -1)
            {
                //提示合同基本信息出错
            }
            if (InsertOrUpdateLeaseItemInfo() == -1)
            {
                //提示租赁合同相关条款出错
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
        String voucherHints = txtCustName.Text.Trim();
        String voucherMemo = "";
        //int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

        //WrkFlwApp.CommitVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), vInfo);
        WrkFlwApp.ConfirmVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]), vInfo);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
        baseTrans.BeginTrans();
        try
        {
            if (SaveBaseBargain() == -1)
            {
                //提示合同基本信息出错
            }
            if (InsertOrUpdateLeaseItemInfo() == -1)
            {
                //提示租赁合同相关条款出错
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
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
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
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    protected void IBtnSave_Click(object sender, EventArgs e)
    {
        baseTrans.BeginTrans();
        try
        {
            SaveOrUpdateFormulaH();
            SaveOrUpdateFormulaM();
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            Response.Write(ex.ToString());
        }
        baseTrans.Commit();
        BindGVConFormulaH(1);
        SetTableControlEnable(false);
        SetContentControlEnable(true);
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void IBtnAdd_Click(object sender, EventArgs e)
    {
        ViewState["SaveOrUpdate"] = "Save";
        SetTableControlEnable(true);
        SetContentControlEnable(false);
        BtnDeductAdd.Enabled = false;
        BtnDeductDel.Enabled = false;

        if (dt.Rows.Count > 0)
        {
            DateTime dateTime = Convert.ToDateTime(dt.Rows[0]["FEndDate"]);
            dateTime.AddDays(1);
            txtStartDate.Text = dateTime.ToString();
            int formulaID = Convert.ToInt32(dt.Rows[0]["FormulaID"]);

            DataSet myDs = new DataSet();
            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = " + formulaID;
            myDs = baseBo.QueryDataSet(new ConFormulaM());
            if (myDs.Tables[0].Rows.Count > 0)
            {
                txtMinSumSell.Text = myDs.Tables[0].Rows[0]["SalesTo"].ToString();
                txtBalanceDeduct.Text = myDs.Tables[0].Rows[0]["MinSum"].ToString();
            }

            myDs.Clear();
            baseBo.WhereClause = "";
            baseBo.WhereClause = "FormulaID = " + formulaID;
            myDs = baseBo.QueryDataSet(new ConFormulaP());
            GVDeductMoney.DataSource = myDs;
            GVDeductMoney.DataBind();

            if (dt.Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (dt.Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_S)
                rabMonopole.Checked = true;
            if (dt.Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_M)
                rabMultilevel.Checked = true;
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void IBtnModify_Click(object sender, EventArgs e)
    {
        ViewState["SaveOrUpdate"] = "Update";
        SetTableControlEnable(true);
        SetContentControlEnable(false);
        BtnDeductAdd.Enabled = false;
        BtnDeductDel.Enabled = false;
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void IBtnDel_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        rs = baseBo.Query(new ConFormulaP());
        if (rs.Count > 0)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('该记录存在抽成关系，不能删除！')", true);
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
        BindGVConFormulaH(1);
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void BtnDeductAdd_Click(object sender, EventArgs e)
    {
        DataSet tDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + ViewState["formulaHID"];
        tDs = baseBo.QueryDataSet(new ConFormulaP());
        if (tDs.Tables[0].Rows.Count > 0)
        {
            txtDistill.Enabled = false;
            txtSellCount.Enabled = false;
            return;
        }
        else
        {
            ConFormulaP formulaP = new ConFormulaP();
            formulaP.FormulaID = Convert.ToInt32(ViewState["formulaHID"]);
            formulaP.Pcent = Convert.ToDecimal(txtDistill.Text);
            formulaP.SalesTo = Convert.ToDecimal(txtSellCount.Text);
            formulaP.ConFormulaPID = BaseApp.GetConFormulaPID();
            if (baseBo.Insert(formulaP) != -1)
            {
                //添加成功
            }
            txtDistill.Enabled = false;
            txtSellCount.Enabled = false;
            txtDistill.Text = "";
            txtSellCount.Text = "";
            BindGVDeductMoney();
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void BtnDeductDel_Click(object sender, EventArgs e)
    {
        int ss = Convert.ToInt32(this.Hidden3.Value);
        ConFormulaP formulaP = new ConFormulaP();
        formulaP.FormulaID = Convert.ToInt32(ss);
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + formulaP.FormulaID;
        if (baseBo.Delete(formulaP) != -1)
        {
            //提示删除成功
        }
        txtSellCount.Text = "";
        txtDistill.Text = "";
        BindGVDeductMoney();
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
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
            txtStartDate.Text = formulaH.FStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = formulaH.FEndDate.ToString("yyyy-MM-dd");
            if (formulaH.PcentOpt == UnioFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (formulaH.PcentOpt == UnioFormulaH.PCENTOPT_TYPE_M)
                rabMultilevel.Checked = true;
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_S)
                rabMonopole.Checked = true;
            GVConFormulaH.DataSource = rs;
            GVConFormulaH.DataBind();
            GVConFormulaH.Enabled = true;
        }
        else
            GVConFormulaH.Enabled = false;


        DataSet myDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        myDs = baseBo.QueryDataSet(new ConFormulaM());
        if (myDs.Tables[0].Rows.Count > 0)
        {
            txtMinSumSell.Text = myDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtBalanceDeduct.Text = myDs.Tables[0].Rows[0]["MinSum"].ToString();
        }

        myDs.Clear();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        myDs = baseBo.QueryDataSet(new ConFormulaP());
        GVDeductMoney.DataSource = myDs;
        GVDeductMoney.DataBind();
    }

    protected void GVDeductMoney_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //鼠标移动到每项时颜色交替效果
            e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor='White';this.style.color='#003399'");
            e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor='#FFFFCD';this.style.color='#8C4510'");

            //单击事件
            e.Row.Attributes.Add("OnClick", "ClickEvent1('" + e.Row.Cells[0].Text + "')");
        }
    }

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
            BindFollrs(-1);
            BindLocation(-1);
            BindUnits(-1);
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
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
            BindLocation(-1);
            BindUnits(-1);
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
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
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    #endregion

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet tempDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + Hidden1.Value + "'";
        tempDs = baseBo.QueryDataSet(new ConFormulaH());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            cmbChargeTypeID.SelectedValue = tempDs.Tables[0].Rows[0]["ChargeTypeID"].ToString();
            cmbFormulaType.SelectedValue = tempDs.Tables[0].Rows[0]["FormulaType"].ToString();
            txtStartDate.Text = tempDs.Tables[0].Rows[0]["FStartDate"].ToString();
            txtEndDate.Text = tempDs.Tables[0].Rows[0]["FEndDate"].ToString();

            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_S)
                rabMonopole.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_M)
                rabMultilevel.Checked = true;

            txtSellCount.Enabled = true;
            txtDistill.Enabled = true;
            BtnDeductAdd.Enabled = true;
            BtnDeductDel.Enabled = true;
        }
        else
        {
            txtSellCount.Enabled = false;
            txtDistill.Enabled = false;
            BtnDeductAdd.Enabled = false;
            BtnDeductDel.Enabled = false;
        }

        DataSet myDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + Hidden1.Value + "'";
        myDs = baseBo.QueryDataSet(new ConFormulaM());
        if (myDs.Tables[0].Rows.Count > 0)
        {
            txtMinSumSell.Text = myDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtBalanceDeduct.Text = myDs.Tables[0].Rows[0]["MinSum"].ToString();
        }

        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + Hidden1.Value + "'";
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

        ViewState["formulaHID"] = Hidden1.Value;
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + Hidden3.Value;
        DataSet tempDs = baseBo.QueryDataSet(new ConFormulaP());
        if (tempDs.Tables[0].Rows.Count > 0)
        {
            txtSellCount.Text = tempDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtDistill.Text = tempDs.Tables[0].Rows[0]["Pcent"].ToString();
            BtnDeductDel.Enabled = true;
            ViewState["deduct"] = Hidden3.Value;
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }

    #region 获取合同对应的商铺



    private void GetAllShopInfo(int FirstStatus)
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        DataSet shopDs = baseBo.QueryDataSet(new ConShop());
        DataTable shopDt = shopDs.Tables[0];
        int shopCount = shopDt.Rows.Count;
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
            if (shopCount > 0)
            {
                GetShopBaseInfo(Convert.ToInt32(shopDt.Rows[0]["ShopID"]));
            }
        }
    }
    #endregion

    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
        GetShopBaseInfo(shopId);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }
    protected void btnShopSave_Click(object sender, EventArgs e)
    {
        baseTrans.BeginTrans();
        try
        {
            if (InsertOrUpdateShopBaseInfo() == -1)
            {
                //提示基本商铺信息出错
            }
            if (SaveShopUnits() == -1)
            {
                //提示基本商铺信息出错
            }
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            Response.Write(ex.ToString());
        }
        baseTrans.Commit();
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
    }

    protected void btnDispos_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void imgCustCodeQ_Click(object sender, EventArgs e)
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
            GetAllShopInfo(0);
            return;
        }
        GetAllShopInfo(0);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void imgCustNameQ_Click(object sender, EventArgs e)
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
            return;
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
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
}
