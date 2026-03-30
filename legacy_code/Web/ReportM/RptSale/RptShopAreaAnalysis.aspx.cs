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
using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
using Lease.ConShop;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

/// <summary>
/// 修改人：hesijian
/// 修改时间：2009年6月16日
/// </summary>
public partial class RptBaseMenu_RptShopAreaAnalysis : BasePage
{
    public string baseInfo;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptShopAreaAnalysis_Title");
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


        BaseBO baseBo = new BaseBO();


        //绑定经营方式
        int[] contractType = Contract.GetBizModes();
        int s = contractType.Length + 1;
        ddlBizMode.Items.Add(new ListItem("", ""));
        for (int i = 1; i < s; i++)
        {
            ddlBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Contract.GetBizModeDesc(contractType[i - 1])), contractType[i - 1].ToString()));
        }


        //绑定区域
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //Resultset rs3 = baseBo.Query(new Area());
        //ddlAreaName.Items.Add(new ListItem("", ""));
        //foreach (Area bd in rs3)
        //    ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));


        //绑定二级经营类别
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        ddlTrade2Name.Items.Add(new ListItem("", ""));
        Resultset rs5 = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs5)
            ddlTrade2Name.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
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
        String s = "%23Rpt_lblShopAreaAnalysis";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "RentableArea_lblAreaName";
        s += "%23" + "RentableArea_lblTradeRelation";
        s += "%23" + "LeaseholdContract_labTradeID";
        s += "%23" + "RentableArea_lblRentArea";
        s += "%23" + "Rpt_GrossSales";
        s += "%23" + "Rpt_AreaSales";
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
        //sCon += "%26" + "ShopCode=" + GetStrNull(this.ddlShopCode.Text);
        //sCon += "%26" + "BizMode=" + GetStrNull(this.ddlBizMode.Text);
        //sCon += "%26" + "BuildingName=" + GetStrNull(this.ddlBuildingName.Text);
        //sCon += "%26" + "FloorName=" + GetStrNull(this.ddlFloorName.Text);
        //sCon += "%26" + "LocationName=" + GetStrNull(this.ddlLocationName.Text);
        //sCon += "%26" + "AreaName=" + GetStrNull(this.ddlAreaName.Text);
        //sCon += "%26" + "Trade1Name=" + GetStrNull(this.ddlTrade1Name.Text);
        //sCon += "%26" + "Trade2Name=" + GetStrNull(this.ddlTrade2Name.Text);
        return sCon;
    }

    //水晶报表数据绑定
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[15];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[15];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptShopAreaAnalysis_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();                      //商铺类型
        paraField[1].Name = "REXShopType";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXShopName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXAreaName";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_AreaName");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXTradeCode";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TradeCode");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXTradeName";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_Trade2Name");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXRentArea";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_RentArea");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXPaidAmt";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXAvgArea";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_AvgArea");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTotalAmt";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXSdate";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[10].CurrentValues.Add(discreteValue[6]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXQBDate";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = txtStartBizTime.Text;
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXQEDate";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = txtEndBizTime.Text;
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
        string str_sql = "select (transshopday.floorname+transshopday.shoptypename) shopcode,transshopday.storeid,transshopday.storename storeshortname,transshopday.shopcode,transshopday.shopname," +
                         " transshopday.areaname,transshopday.tradename,conshop.rentarea,sum(paidamt) paidamt," +
                         " sum(paidamt)/conshop.rentarea AvgArea from transshopday" +
                         " inner join conshop on (conshop.shopid=transshopday.shopid) where 1=1";

        //条件查询
        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + " AND transshopday.storeid='" + ddlBizproject.SelectedValue + "'";
        }
        if (this.ddlTrade2Name.Text != "")
        {
            str_sql = str_sql + " And transshopday.tradeid='" + this.ddlTrade2Name.SelectedValue + "'";
        }
        if (ddlShopCode.Text != "")
        {
            str_sql = str_sql + " AND transshopday.ShopCode ='" + ddlShopCode.SelectedItem.Text.Substring(0, ddlShopCode.SelectedItem.Text.IndexOf(" ")) + "'";
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

        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND transshopday.AreaId ='" + ddlAreaName.SelectedValue + "'";
        }

        if (txtStartBizTime.Text != "")
        {
            str_sql = str_sql + " AND transshopday.BizDate >='" + txtStartBizTime.Text + "'";
        }
        if (txtEndBizTime.Text != "")
        {
            str_sql = str_sql + " AND transshopday.BizDate  <='" + txtEndBizTime.Text + "'";
        }

        if (txtAreaB.Text != "")
        {
            str_sql = str_sql + " AND conshop.RentArea  >='" + txtAreaB.Text + "'";
        }

        if (txtAreaE.Text != "")
        {
            str_sql = str_sql + " AND conshop.RentArea  <='" + txtAreaE.Text + "'";
        }
        //if (RB1.Checked)
        //{

        //    str_sql = str_sql + "";
        //}
        //if (RB2.Checked)
        //{

        //    str_sql = str_sql + " AND a.datasource=1 ";
        //}
        //if (RB3.Checked)
        //{

        //    str_sql = str_sql + " AND a.datasource=2 ";
        //}
        //if (RB4.Checked)
        //{

        //    str_sql = str_sql + " AND a.datasource=3 ";
        //}

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            string[] arr = new string[5];
            arr[0] = AuthBase.AUTH_SQL_SHOP;
            arr[1] = AuthBase.AUTH_SQL_BUILD;
            arr[2] = AuthBase.AUTH_SQL_FLOOR;
            arr[3] = AuthBase.AUTH_SQL_CONTRACT;
            arr[4] = AuthBase.AUTH_SQL_STORE;
            string strAND = "";
            for (int i = 0; i < arr.Length; i++)
            {
                strAND += " AND EXISTS (" + arr[i].ToString().Replace("ConShop", "transshopday") + sessionUser.UserID + ")";
            }
            str_sql += strAND; 

        }

        str_sql = str_sql + " group by transshopday.storeid,transshopday.storename,transshopday.shopcode,transshopday.shopname,transshopday.areaname," +
                            "transshopday.tradename,conshop.rentarea,transshopday.shoptypename,transshopday.floorname";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\ShopAreaAnalysis.rpt";

    }

    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBuildingName.Items.Clear();
        ddlFloorName.Items.Clear();

        /*绑定大楼*/
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + "and storeid='" + ddlBizproject.SelectedValue + "'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
        {
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        }

        baseBo.WhereClause = "StoreID=" + this.ddlBizproject.SelectedValue + " and AreaStatus=" + Area.AREA_STATUS_VALID ;
        Resultset rs3 = baseBo.Query(new Area());
        ddlAreaName.Items.Add(new ListItem("", ""));
        foreach (Area bd in rs3)
            ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));

    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定楼层
        ddlFloorName.Items.Clear();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "FloorStatus = " + Floors.FLOOR_STATUS_VALID + " AND BuildingID = " + ddlBuildingName.SelectedValue.ToString();
        Resultset rs1 = baseBO.Query(new Floors());
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
    }
    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        string sql = "SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + "AND FLOORID='"+ddlFloorName.SelectedValue+"' Order By ShopCode";
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
        this.Response.Redirect("~/ReportM/RptSale/RptShopAreaAnalysis.aspx");
    }
}
