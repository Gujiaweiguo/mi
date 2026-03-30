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
using Base.Page;

public partial class Lease_ConUnionPalaver : BasePage
{
    #region 定义
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    BaseTrans baseTrans = new BaseTrans();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    DataSet DeductMoneyDS = new DataSet();
    DataTable DeductMoneyDT = new DataTable();
    //string SaveOrUpdate = "";
    #endregion

    #region Page_Load事件
    protected void Page_Load(object sender, EventArgs e)
    {
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

            int contractID = Convert.ToInt32(Request["VoucherID"]);
            GetContractInfo(contractID);
            GetLeaseItemInfo();
            GetAllShopInfo(1);
            GetShopUnits();
            GetConFormulaInfo();

            //bool canSmtToMgr = WrkFlwApp.CanSmtToMgr(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]));
            //if (canSmtToMgr)
            //{
            //    this.btnPutIn.Visible = true;
            //}


            SetTableControlEnable(true);
            SetContentControlEnable(true);
            //绑定GridView
            BindGVConFormulaH();
            BindGVDeductMoney();
        }

    }

    #endregion

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
            cmbContractStatus.Text = Contract.GetContractTypeStatusDesc(Convert.ToInt32((contractDs.Tables[0].Rows[0]["ContractStatus"].ToString())));
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
            cmbPenalty.SelectedValue = (contractDs.Tables[0].Rows[0]["Penalty"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Penalty"].ToString());
            cmbNotice.SelectedValue = (contractDs.Tables[0].Rows[0]["Notice"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Notice"].ToString());
            txtEBargain.Text = (contractDs.Tables[0].Rows[0]["EConURL"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["EConURL"].ToString());
            listBoxAddItem.Text = (contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["AdditionalItem"].ToString());
            //listBoxRemark.Text = (contractDs.Tables[0].Rows[0]["Note"].ToString() == null ? "" : contractDs.Tables[0].Rows[0]["Note"].ToString());

            ViewState["contractID"] = Convert.ToInt32(Request["VoucherID"]);
            ViewState["custId"] = contractDs.Tables[0].Rows[0]["CustID"];
        }
    }

    //获取赁相关条款信息




    protected void GetLeaseItemInfo()
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBo.Query(new ConUnion());
        if (rs.Count == 1)
        {
            ConUnion union = rs.Dequeue() as ConUnion;
            DDownListBillCycle.SelectedValue = union.BillCycle.ToString();
            DDownListCurrencyType.SelectedValue = union.CurTypeID.ToString();
            DDownListAccountCycle.SelectedValue = union.AccountCycle.ToString();
            txtRentInc.Text = union.RentInc;
            txtTaxRate.Text = (Convert.ToDecimal(union.TaxRate)*100).ToString();
            DDownListTaxType.SelectedValue = union.TaxType.ToString();
        }
    }

    //获取商铺基本信息
    protected void GetAllShopInfo(int FirstStatus)
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

    //获取商铺信息中的单元信息
    protected void GetShopUnits()
    {
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
                baseBo.WhereClause = "UnitID = " + shopDS.Tables[0].Rows[i]["UnitID"];
                DataSet unitDS = baseBo.QueryDataSet(new Units());
                ListItem mylistItem = new ListItem();
                mylistItem.Text = unitDS.Tables[0].Rows[0]["UnitCode"].ToString();
                mylistItem.Value = unitDS.Tables[0].Rows[0]["UnitID"].ToString();
                DDownListUnit.Items.Add(mylistItem);
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
            txtStartDate.Text = formulaH.FStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = formulaH.FEndDate.ToString("yyyy-MM-dd");
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (formulaH.PcentOpt == ConFormulaH.PCENTOPT_TYPE_M)
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
            txtBalanceDeduct.Text = (Convert.ToDecimal(myDs.Tables[0].Rows[0]["MinSum"])*100).ToString();
        }

        myDs.Clear();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + Convert.ToInt32(ViewState["formulaHID"]);
        myDs = baseBo.QueryDataSet(new ConFormulaP());
        GVDeductMoney.DataSource = myDs;
        GVDeductMoney.DataBind();
    }


    #region 绑定DropDownList

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

    //绑定合同状态




    //protected void BindLeaseType()
    //{
    //    int[] status = Contract.GetContractTypeStatus();
    //    int s = status.Length;
    //    for (int i = 0; i < s; i++)
    //    {
    //        cmbContractStatus.Items.Add(new ListItem(Contract.GetContractTypeStatusDesc(status[i]), status[i].ToString()));
    //    }
    //}

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
            cmbFormulaType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",UnioFormulaH.GetFormulaTypeStatusDesc(status[i])), status[i].ToString()));
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
    protected void BindGVConFormulaH()
    {
        ds.Clear();
        baseBo.WhereClause = "";
        baseBo.OrderBy = "FEndDate Desc";
        baseBo.WhereClause = "ContractID = " + ViewState["contractID"];
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
        //txtSellCount.Enabled = !t;
        //txtDistill.Enabled = !t;
        //IBtnTakeAdd.Enabled = !t;
        //IBtnTakeDel.Enabled = !t;
    }

    #endregion

    #region 保存草稿
    //联营合同项目保存草稿
    protected void SaveBaseBargain()
    {
       /* ds.Clear();
        baseBo.WhereClause = "TradeID = '" + Convert.ToInt32(cmbTradeID.SelectedValue) + "'";
        ds = baseBo.QueryDataSet(new TradeRelation());

        Contract contact = new Contract();
        contact.ContractID = BaseApp.GetContractID();
        contact.CustID = Convert.ToInt32(Request["CustID"]);
        contact.ContractCode = txtContractCode.Text;
        contact.RefID = Convert.ToInt32(txtRefID.Text);
        contact.ConStartDate = Convert.ToDateTime(txtConStartDate.Text);
        contact.ConEndDate = Convert.ToDateTime(txtConEndDate.Text);
        contact.PenaltyItem = cmbPenalty.SelectedValue;
        contact.ChargeStartDate = Convert.ToDateTime(txtChargeStart.Text);
        contact.TradeID = Convert.ToInt32(cmbTradeID.SelectedValue);
        contact.ContractStatus = Convert.ToInt32(cmbContractStatus.SelectedValue);
        //contact.Penalty = Convert.ToInt32(txtOverItem.Text);
        contact.Notice = Convert.ToInt32(cmbNotice.SelectedValue);
        contact.AdditionalItem = listBoxAddItem.Text;
        contact.EConURL = txtEBargain.Text;
        contact.Note = listBoxRemark.Text;
        contact.RootTradeID = Convert.ToInt32(ds.Tables[0].Rows[0]["TradeID"]);

        ViewState["ConStartDate"] = contact.ConStartDate;
        ViewState["ConEndDate"] = contact.ConEndDate;
        ViewState["BargainId"] = contact.ContractID;

        if (baseTrans.Insert(contact) != -1)
        {
            //提示添加成功
        }
        else
        {
            //提示操作失败
        }*/
    }

    //保存联营合同相关条款
    protected void SaveUnioItem()
    {
        ConUnion union = new ConUnion();
        union.ContractID = Convert.ToInt32(ViewState["BargainId"]);
        union.BillCycle = Convert.ToInt32(DDownListBillCycle.SelectedValue);
        union.RentInc = txtRentInc.Text;
        union.AccountCycle = Convert.ToInt32(DDownListAccountCycle.SelectedValue);
        union.TaxRate = Convert.ToDecimal(txtTaxRate.Text);
        union.TaxType = Convert.ToInt32(DDownListTaxType.SelectedValue);
        union.CurTypeID = Convert.ToInt32(DDownListCurrencyType.SelectedValue);

        if (baseTrans.Insert(union) != 1)
        {
        }
    }

    //商铺基本内容保存草稿
    protected void SaveShopBaseInfo()
    {
        ConShop conShop = new ConShop();
        conShop.ShopId = BaseApp.GetShopID();
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

        ViewState["ShopId"] = conShop.ShopId;
        ViewState["RentArea"] = conShop.RentArea;
        if (baseTrans.Insert(conShop) != -1)
        {
            //提示添加成功
        }
        else
        {
            //提示操作失败
        }

    }


    //商铺信息基本内容中的单元信息保存草稿
    protected void SaveShopUnits()
    {
        ConShopUnit conShopUnit = new ConShopUnit();
        int count = ListBoxUnits.Items.Count;
        int index = 0;
        for (int i = 0; i < count; i++)
        {
            conShopUnit.ShopID = Convert.ToInt32(ViewState["ShopId"]);
            conShopUnit.UnitID = Convert.ToInt32(ListBoxUnits.Items[index].Value);
            //conShopUnit.RentArea = Convert.ToDecimal(ViewState["RentArea"]);
            conShopUnit.RentStatus = ConShopUnit.RENTSTATUS_TYPE_YES;
            if (baseTrans.Insert(conShopUnit) != -1)
            {
            }
            index++;
        }

    }

    #endregion

    #region 保存和修改结算公式





    //保存和修改公式列表




    protected void SaveOrUpdateFormulaH()
    {
        UnioFormulaH formula = new UnioFormulaH();
        formula.FormulaID = BaseApp.GetFormulaID();
        formula.ContractID = Convert.ToInt32(ViewState["BargainId"]);
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
            baseTrans.WhereClause = "FormulaID = " + Hidden1.Value;
            baseTrans.Update(formula);
        }

        BindGVConFormulaH();
    }

    //保存和修改保底表
    protected void SaveOrUpdateFormulaM()
    {
        ConFormulaM formulaM = new ConFormulaM();
        formulaM.FormulaID = Convert.ToInt32(ViewState["FormulaID"]);
        formulaM.SalesTo = Convert.ToDecimal(txtMinSumSell.Text);
        formulaM.MinSum = Convert.ToDecimal(txtBalanceDeduct.Text);

        if (ViewState["SaveOrUpdate"].ToString() == "Save")
        {
            if (baseTrans.Insert(formulaM) != -1)
            {
            }
            else
            {
            }
        }
        if (ViewState["SaveOrUpdate"].ToString() == "Update")
        {
            baseTrans.WhereClause = "";
            baseTrans.WhereClause = "FormulaID = " + Hidden1.Value;
            baseTrans.Update(formulaM);
        }
    }

    #endregion

    #region 页面控件事件

    protected void butSaveTemp_Click(object sender, EventArgs e)
    {
        baseTrans.BeginTrans();
        SaveBaseBargain();
        SaveUnioItem();
        SaveShopBaseInfo();
        SaveShopUnits();
        baseTrans.Commit();
    }
    #endregion

    protected void butAdd_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["SaveOrUpdate"] = "Save";
        SetTableControlEnable(true);
        SetContentControlEnable(false);

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
        txtEndDate.Text = ViewState["ConEndDate"].ToString();
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        DataSet tempDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
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
        }

        DataSet myDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
        myDs = baseBo.QueryDataSet(new ConFormulaM());
        if (myDs.Tables[0].Rows.Count > 0)
        {
            txtMinSumSell.Text = myDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtBalanceDeduct.Text = myDs.Tables[0].Rows[0]["MinSum"].ToString();
        }

        myDs.Clear();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + this.Hidden1.Value + "'";
        myDs = baseBo.QueryDataSet(new ConFormulaP());
        GVDeductMoney.DataSource = myDs;
        GVDeductMoney.DataBind();
        ViewState["formulaHID"] = this.Hidden1.Value;
    }

    protected void GVConFormulaH_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void butSave_Click(object sender, ImageClickEventArgs e)
    {
        baseTrans.BeginTrans();
        SaveOrUpdateFormulaH();
        SaveOrUpdateFormulaM();
        baseTrans.Commit();
    }
    protected void IBtnTakeAdd_Click(object sender, ImageClickEventArgs e)
    {
        DataSet tDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + ViewState["formulaHID"];
        tDs = baseBo.QueryDataSet(new ConFormulaP());
        if (tDs.Tables[0].Rows.Count > 0)
        {
            //txtDistill.Enabled = false;
            //txtSellCount.Enabled = false;
            return;
        }
        else
        {
            ConFormulaP formulaP = new ConFormulaP();
            formulaP.FormulaID = Convert.ToInt32(ViewState["formulaHID"]);
            //formulaP.Pcent = Convert.ToDecimal(txtDistill.Text);
            //formulaP.SalesTo = Convert.ToDecimal(txtSellCount.Text);
            if (baseBo.Insert(formulaP) != -1)
            {
                //添加成功
            }
            //txtDistill.Enabled = false;
            //txtSellCount.Enabled = false;
            BindGVDeductMoney();
        }
    }


    protected void IBtnTakeDel_Click(object sender, ImageClickEventArgs e)
    {
        ConFormulaP formulaP = new ConFormulaP();
        formulaP.FormulaID = Convert.ToInt32(Hidden1.Value);
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = " + formulaP.FormulaID;
        if (baseBo.Delete(formulaP) != -1)
        {
            //提示删除成功
        }
    }
    protected void butUpdate_Click(object sender, ImageClickEventArgs e)
    {
        ViewState["SaveOrUpdate"] = "Update";
        SetTableControlEnable(true);
        SetContentControlEnable(false);
    }
    protected void GVConFormulaH_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void butDelete_Click(object sender, ImageClickEventArgs e)
    {

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
    }
    protected void IBtnUnitsAdd_Click(object sender, EventArgs e)
    {
        ListBoxUnits.Items.Add(new ListItem(DDownListUnit.SelectedItem.ToString(), DDownListUnit.SelectedValue.ToString()));
    }
    protected void btnDispose_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int voucherID = Convert.ToInt32(ViewState["contractID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);


        WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(wrkFlwID, nodeID, sequence);

        if ((WrkFlwEntity.NODE_STATUS_REJECT_PENDING == objWrkFlwEntity.NodeStatus) || (WrkFlwEntity.NODE_STATUS_NORMAL_PENDING == objWrkFlwEntity.NodeStatus))
        {
           
           //Response.Write("alert('aaaa')");
            String str = "window.open('../Test/Default3.aspx?" + "WrkFlwID=" + wrkFlwID + "&VoucherID=" + voucherID + "&Sequence=" + sequence + "&NodeID=" + nodeID + "&VoucherMemo=" + listBoxRemark.Text.Trim() + "','正常驳回操作',height=200,width=400,status=1,toolbar=0,menubar=0);";
            //Response.Write("<script language = 'javascript'>window.open('../Test/Default3.aspx?" + "WrkFlwID=" + wrkFlwID + "&VoucherID=" + voucherID + "&Sequence=" + sequence + "&NodeID=" + nodeID + "&VoucherMemo=" + listBoxRemark.Text.Trim() + "','正常驳回操作',height=200,width=400,status=1,toolbar=0,menubar=0);</script>");
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
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void btnTempSave_Click(object sender, EventArgs e)
    {
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
    protected void butDel_Click(object sender, EventArgs e)
    {

    }
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
            txtStartDate.Text = Convert.ToDateTime(tempDs.Tables[0].Rows[0]["FStartDate"]).ToString("yyyy-MM-dd");
            txtEndDate.Text = Convert.ToDateTime(tempDs.Tables[0].Rows[0]["FEndDate"]).ToString("yyyy-MM-dd");

            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_FAST)
                rabFastness.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_S)
                rabMonopole.Checked = true;
            if (tempDs.Tables[0].Rows[0]["PcentOpt"].ToString() == UnioFormulaH.PCENTOPT_TYPE_M)
                rabMultilevel.Checked = true;

            //BtnDeductAdd.Enabled = true;
            //BtnDeductDel.Enabled = true;
        }
        else
        {
            //BtnDeductAdd.Enabled = false;
            //BtnDeductDel.Enabled = false;
        }

        DataSet myDs = new DataSet();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "FormulaID = '" + Hidden1.Value + "'";
        myDs = baseBo.QueryDataSet(new ConFormulaM());
        if (myDs.Tables[0].Rows.Count > 0)
        {
            txtMinSumSell.Text = myDs.Tables[0].Rows[0]["SalesTo"].ToString();
            txtBalanceDeduct.Text = (Convert.ToDecimal(myDs.Tables[0].Rows[0]["MinSum"]) * 100).ToString();
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
            DDownListBrand.SelectedValue = shop.BrandID.ToString();
            txtRentArea.Text = shop.RentArea.ToString();
            DDownListAreaName.SelectedValue = shop.AreaId.ToString();
            txtShopStartDate.Text = shop.ShopStartDate.ToString("yyyy-MM-dd");
            txtShopEndDate.Text = shop.ShopEndDate.ToString("yyyy-MM-dd");

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
        GetAllShopInfo(0);
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {

    }
    protected void GVDeductMoney_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
        GetShopBaseInfo(shopId);
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(3)", true);
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
    protected void btnShopSave_Click(object sender, EventArgs e)
    {

    }
}
