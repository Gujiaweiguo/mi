using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.Contract;
using RentableArea;
using Lease.ConShop;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

/// <summary>
///修改人:hesijian
///修改时间：2009年6月16日
/// </summary>
public partial class RptBaseMenu_RptShopSalesRank : BasePage
{
    public string baseInfo;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptShopSalesRank_Title");
            Fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
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
    /* 判断日期空值,返回默认值



     * 
     * 
     */
    private String GetdateNull(String s)
    {
        return s.Trim() == "" ? "2007-12-25" : s;
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
        this.txtStartBizTime.Text = DateTime.Now.ToShortDateString();
        this.txtEndBizTime.Text = DateTime.Now.ToShortDateString();
        //绑定经营方式
        int[] contractType = Contract.GetBizModes();
        int s = contractType.Length + 1;
        ddlBizMode.Items.Add(new ListItem("", ""));
        for (int i = 1; i < s; i++)
        {
            ddlBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(contractType[i - 1])), contractType[i - 1].ToString()));
        }

        //绑定楼



        BaseBO baseBo = new BaseBO();

        //string sql = "SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + " Order By ShopCode";
        //DataSet myDS = baseBo.QueryDataSet(sql);
        //int count = myDS.Tables[0].Rows.Count;
        //ddlShopCode.Items.Clear();
        //ddlShopCode.Items.Add("");
        //for (int i = 0; i < count; i++)
        //{
        //    //绑定商铺号


        //    ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        //}

        ///*绑定大楼*/
        //baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID;
        //Resultset rs = baseBo.Query(new Building());
        //ddlBuildingName.Items.Add(new ListItem("", ""));
        //foreach (Building bd in rs)
        //{
        //    ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        //}



        //绑定二级经营类别
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        ddlTradeID.Items.Clear();
        ddlTradeID.Items.Add(new ListItem("", ""));
        Resultset rs5 = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs5)
            ddlTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
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
        String s = "%23Rpt_lblShopSalesRank";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "Rpt_TotalReceipt";
        s += "%23" + "Rpt_GrossSales";
        s += "%23" + "Rpt_SalesPer";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        //sCon += "%26BizSDate=" + GetdateNull(this.txtStartBizTime.Text);
        //sCon += "%26BizEDate=" + GetdateNull(this.txtEndBizTime.Text);
        //sCon += "%26" + "ShopCode=" + GetStrNull(this.txtShopCode.Text);
        //sCon += "%26" + "BizMode=" + GetStrNull(this.ddlBizMode.Text);
        //sCon += "%26" + "BuildingName=" + GetStrNull(this.ddlBuildingName.Text);
        //sCon += "%26" + "FloorName=" + GetStrNull(this.ddlFloorName.Text);
        //sCon += "%26" + "LocationName=" + GetStrNull(this.ddlLocationName.Text);
        //sCon += "%26" + "AreaName=" + GetStrNull(this.ddlAreaName.Text);
        //sCon += "%26" + "Trade1Name=" + GetStrNull(this.ddlTradeID.Text);
        //sCon += "%26" + "Trade2Name=" + GetStrNull(this.ddlTrade2Name.Text);
        return sCon;
    }

    //水晶报表数据绑定
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptShopSalesRank_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXShopCode";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXShopName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField(); // /客均价
        paraField[3].Name = "REXAvgPaidAmt";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_SalesPrice");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();//交易笔数
        paraField[4].Name = "REXTrNum";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TrNum");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXPaidAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXTotalAmt";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXQBDate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = txtStartBizTime.Text;
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXQEDate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = txtEndBizTime.Text;
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXSdate";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[9].CurrentValues.Add(discreteValue[9]);


        paraField[10] = new ParameterField();
        paraField[10].Name = "REXRptNum";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "WrkFlw_Sequence");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXMallTitle";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = Session["MallTitle"].ToString();
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXBizProject";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[12].CurrentValues.Add(discreteValue[12]);



        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = "";
        if (txtCount.Text != "")
        {
            str_sql = "select Top " + txtCount.Text.Trim() + " transshopday.storeid,transshopday.storename storeshortname,transshopday.shopcode shopid,transshopday.shopname," +
                        " sum(transshopday.paidamt) as PaidAmt,sum(transshopday.totalreceipt) as TotalReceipt,sum(transshopday.paidamt)/sum(transshopday.totalreceipt) as AvgPaidAmt" +
                        " from transshopday where 1=1 ";
        }
        else
        {
            str_sql = "select transshopday.storeid,transshopday.storename storeshortname,transshopday.shopcode shopid,transshopday.shopname," +
                      " sum(transshopday.paidamt) as PaidAmt,sum(transshopday.totalreceipt) as TotalReceipt,sum(transshopday.paidamt)/sum(transshopday.totalreceipt) as AvgPaidAmt" +
                      " from transshopday where 1=1 ";
        }

        //条件查询
        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + " AND transshopday.storeid='" + ddlBizproject.SelectedValue + "'";
        }
        if (ddlShopCode.Text != "")
        {
            str_sql = str_sql + " AND transshopday.Shopcode ='" + ddlShopCode.SelectedItem.Text.Substring(0, ddlShopCode.SelectedItem.Text.IndexOf(" ")) + "'";
        }
        if (ddlBizMode.Text != "")
        {
            str_sql = str_sql + " AND transshopday.BizMode ='" + ddlBizMode.SelectedValue + "'";
        }
        if (ddlBuildingName.Text != "")
        {
            str_sql = str_sql + " AND transshopday.BuildingID = '" + ddlBuildingName.SelectedValue + "'";
        }
        if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND transshopday.FloorId ='" + ddlFloorName.SelectedValue + "'";
        }
        if (ddlLocationName.Text != "")
        {
            str_sql = str_sql + " AND transshopday.LocationId ='" + ddlLocationName.SelectedValue + "'";
        }
        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND transshopday.AreaId ='" + ddlAreaName.SelectedValue + "'";
        }
        //if (ddlTradeID.Text != "")
        //{
        //    str_sql = str_sql + " AND Contract.RootTradeID ='" + ddlTradeID.SelectedValue + "'";
        //}
        if (this.ddlTradeID.Text != "")
        {
            str_sql = str_sql + " AND transshopday.TradeID ='" + ddlTradeID.SelectedValue + "'";
        }
        if (txtStartBizTime.Text != "")
        {
            str_sql = str_sql + " AND transshopday.BizDate >='" + txtStartBizTime.Text + "'";
        }
        if (txtEndBizTime.Text != "")
        {
            str_sql = str_sql + " AND transshopday.BizDate <='" + txtEndBizTime.Text + "'";
        }
        //if (RB1.Checked)
        //{

        //    str_sql = str_sql + " ";
        //}
        //if (RB2.Checked)
        //{

        //    str_sql = str_sql + " AND TransSku.datasource=1 ";
        //}
        //if (RB3.Checked)
        //{

        //    str_sql = str_sql + " AND TransSku.datasource=2 ";
        //}
        //if (RB4.Checked)
        //{

        //    str_sql = str_sql + " AND TransSku.datasource=3 ";
        //}

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {

            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE  + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";

            for (int i = 0; i < 5; i++)
            {
                strAuth = strAuth.Replace("ConShop", "transshopday");
            }
        }

        str_sql = str_sql + strAuth  + " group by transshopday.storeid,transshopday.storename,transshopday.shopcode,transshopday.shopname" +
                    " ORDER BY PaidAmt desc";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\ShopSalesRank.rpt";

    }

    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFloorName.Items.Clear();
        ddlLocationName.Items.Clear();
        ddlShopCode.Items.Clear();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "FloorStatus = " + Floors.FLOOR_STATUS_VALID + " AND BuildingID = '" + ddlBuildingName.SelectedValue + "'";
        Resultset rs1 = baseBO.Query(new Floors());
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
        {
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
        }
    }
    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定方位
        ddlLocationName.Items.Clear();
        ddlShopCode.Items.Clear();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "LocationStatus = " + Location.LOCATION_STATUS_VALID + " AND FloorID = '" + ddlFloorName.SelectedValue + "'";
        Resultset rs2 = baseBO.Query(new Location());
        ddlLocationName.Items.Add(new ListItem("", ""));
        foreach (Location bd in rs2)
        {
            ddlLocationName.Items.Add(new ListItem(bd.LocationName, bd.LocationID.ToString()));
        }
    }
    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*绑定大楼*/
        ddlBuildingName.Items.Clear();
        ddlFloorName.Items.Clear();
        ddlLocationName.Items.Clear();
        ddlShopCode.Items.Clear();
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + "AND StoreId='" + ddlBizproject.SelectedValue + "'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
        {
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        }


        //绑定区域
        baseBo.WhereClause = "StoreID= " + ddlBizproject.SelectedValue + "And AreaStatus = " + Area.AREA_STATUS_VALID;
        Resultset rs3 = baseBo.Query(new Area());
        ddlAreaName.Items.Add(new ListItem("", ""));
        foreach (Area bd in rs3)
            ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));
    }
    protected void ddlLocationName_SelectedIndexChanged(object sender, EventArgs e)
    {

        //绑定商铺号
        ddlShopCode.Items.Clear();
        BaseBO baseBo = new BaseBO();
        string sql = "SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + "AND FLOORID='" + ddlFloorName.SelectedValue + "'AND LocationID='" + ddlLocationName.SelectedValue + "' Order By ShopCode";
        DataSet myDS = baseBo.QueryDataSet(sql);
        int count = myDS.Tables[0].Rows.Count;
        ddlShopCode.Items.Clear();
        ddlShopCode.Items.Add("");
        for (int i = 0; i < count; i++)
        {
            ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptShopSalesRank.aspx");
    }
}
