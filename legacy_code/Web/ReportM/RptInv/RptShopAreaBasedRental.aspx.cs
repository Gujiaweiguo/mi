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

public partial class RptBaseMenu_RptShopAreaBasedRental : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblShopAreaBasedRental");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }

    }


    /* 判断数据空值,返回默认值

     * 
     * 
     */
    private String GetStrNull(String s)
    {
        return s.Trim() == "" ? "-32766" : s;
    }
    /* 初始化下拉列表

     * 
     * 
     */
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
    private void InitDDL()
    {

        ////绑定楼

        BaseBO baseBo = new BaseBO();
        ////baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //Resultset rs = baseBo.Query(new Building());
        //ddlBuildingName.Items.Add(new ListItem("", ""));
        //foreach (Building bd in rs)
        //    ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingCode));

        ////绑定楼层
        ////baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //Resultset rs1 = baseBo.Query(new Floors());
        //ddlFloorName.Items.Add(new ListItem("", ""));
        //foreach (Floors bd in rs1)
        //    ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorCode));

        //绑定区域
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        Resultset rs2 = baseBo.Query(new Area());
        ddlAreaName.Items.Add(new ListItem("", ""));
        foreach (Area bd in rs2)
            ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaCode));

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    /* 取得表头资源
     * 
     * 
     */ 
    private String GetRptResx()
    {
        String s = "%23Rpt_lblShopAreaBasedRental";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "RentableArea_lblBuildingName";
        s += "%23" + "RentableArea_lblFloorName";
        s += "%23" + "RentableArea_lblAreaName";
        s += "%23" + "RentableArea_lblRentArea";
        s += "%23" + "Rpt_lblHireType";
        s += "%23" + "ConLease_labFastnessHire";
        s += "%23" + "Rpt_UnitPrice";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        sCon += "%26ShopCode=" + GetStrNull(this.txtShopCode.Text);
        sCon += "%26CustCode=" + GetStrNull(this.txtCustCode.Text);
        sCon += "%26" + "BuildingName=" + GetStrNull(this.ddlBuildingName.Text);
        sCon += "%26" + "FloorName=" + GetStrNull(this.ddlFloorName.Text);
        sCon += "%26" + "AreaName=" + GetStrNull(this.ddlAreaName.Text);
        return sCon;
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[15];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[15];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptShopAreaBasedRental_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustCode";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXShopName";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXBuildingName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_BuildingName");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXFloorName";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_FloorName");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXAreaName";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_AreaName");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXRentArea";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_RentArea");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXRateType";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_RateType");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXFixedRental";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FixedRental");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXUnitRen";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_UnitRen");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXShopCode";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXTotalAmt";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[12].CurrentValues.Add(discreteValue[12]);


        paraField[13] = new ParameterField();
        paraField[13].Name = "REXBizProject";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXMallTitle";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = Session["MallTitle"].ToString();
        paraField[14].CurrentValues.Add(discreteValue[14]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        //比值面积使用的是签约面积
        //注意：一份合同签约多个商铺则单位面积租金相同
        //如果是保底租金，则去保底金额进行计算
        string str_sql = "select store.storeid,store.storeshortname, " +
                           " Customer.CustCode,Customer.CustName CustName," +
                            " ConShop.ShopCode, ConShop.ShopName,Building.BuildingName,Floors.FloorName,Area.AreaName," +
                            " round(FormulaRent.FixedRental,2) as UnitRen,Round(FormulaRent.FixedRental * Conshop.Rentarea,2) FixedRental,Conshop.Rentarea,FormulaRent.FormulaType RateType" +
                             " from ConShop " +
                             " inner join store on (ConShop.storeid=store.storeid)" +
                             " inner join Building on (ConShop.BuildingID = Building.BuildingID)" +
                             " inner join Floors on (ConShop.FloorID = Floors.FloorID)" +
                             " inner join Area on  (ConShop.AreaID = Area.AreaID)" +
                             " inner join Contract on (Contract.ContractID = ConShop.ContractID)" +
                             " inner join ShopType on (conshop.shoptypeid=shoptype.shoptypeid)" +
                             " inner join Customer on (Contract.custid=customer.custid)" +
                             " inner join (" +
                                    " select ConFormulaH.contractid,ConFormulaH.TotalArea,ConFormulaH.FormulaType," +
                                    " case ConFormulaH.FormulaType when 'F'" +  //固定租金
                                    " THEN ConFormulaH.FixedRental/ConFormulaH.TotalArea" +
                                    " WHEN 'V' THEN " +  //抽成保底租金,则用保底租金同签约面积比
                                    " (Select top 1 ConFormulaM.MinSum from ConFormulaM where ConFormulaM.FormulaID=ConFormulaH.FormulaID )/ConFormulaH.TotalArea" +
                                    " END as FixedRental" +
                                    " from ConFormulaH where ConFormulaH.fstartdate <= getdate() and ConFormulaH.fEnddate >= getdate()" +
                                    " and ConFormulaH.chargetypeid in (select chargetypeid  from chargetype where chargetype.chargeclass=1)" +  //租金大类
                             ") as FormulaRent on (FormulaRent.ContractID=Contract.ContractId)" + 
                             " " +
                            " where Contract.ContractStatus=2";  //有效合同

        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + "AND store.storeid='"+ddlBizproject.SelectedValue+"'";
        }
        if (txtShopCode.Text != "")
        {
            str_sql = str_sql + " AND ConShop.ShopCode ='" + txtShopCode.Text + "'";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode ='" + txtCustCode.Text + "'";
        }
        if (ddlBuildingName.Text != "")
        {
            str_sql = str_sql + " AND Building.BuildingCode = '" + ddlBuildingName.SelectedValue + "'";
        }
        if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND Floors.FloorCode ='" + ddlFloorName.SelectedValue + "'";
        }
        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND Area.AreaCode ='" + ddlAreaName.SelectedValue + "'";
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                      ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                      ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                      ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE  + sessionUser.UserID +
                      ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
            //for (int i = 0; i < 5; i++)
            //{
            //    //将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
            //    strAuth = strAuth.Replace("ConShop", "transshopday");
            //}
        }

        //str_sql = str_sql + " group by store.storeid,store.storeshortname," +
        //                    " Customer.CustCode,Customer.CustShortName,FormulaRent.FixedRental,FormulaRent.TotalArea RentArea,FormulaRent.FormulaType RateType," +
        //                    " ConShop.ShopCode,ConShop.ShopName,Building.BuildingName,Floors.FloorName,Area.AreaName,Contract.Contractid";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\ShopAreaBasedRental.rpt";

    }

    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
                /*绑定大楼*/
        ddlBuildingName.Items.Clear();
        ddlFloorName.Items.Clear();
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID+"AND StoreId='"+ddlBizproject.SelectedValue+"'";
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptInv/RptShopAreaBasedRental.aspx");
    }
}
