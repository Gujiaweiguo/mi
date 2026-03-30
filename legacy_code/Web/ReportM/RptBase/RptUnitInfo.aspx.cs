using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Store;

public partial class RptBaseMenu_RptUnitInfo : BasePage
{
    public string baseInfo;
    private String unitStatus = "-32766";
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_Title");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
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

    private void InitDDL()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }

        Resultset rs2 = baseBo.Query(new Location());
        ddlLocationName.Items.Add(new ListItem("", ""));
        foreach (Location bd in rs2)
        {
            ddlLocationName.Items.Add(new ListItem(bd.LocationName, bd.LocationCode));
        }
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
        String s = "%23Rpt_Title_lblUnitInfo";
        s += "%23" + "RentableArea_lblUnitCode";
        s += "%23" + "RentableArea_lblBuildingName";
        s += "%23" + "RentableArea_lblFloorName";
        s += "%23" + "RentableArea_lblLocationName";
        s += "%23" + "RentableArea_lblFloorArea";
        s += "%23" + "RentableArea_lblRentArea";
        s += "%23" + "RentableArea_lblUnitStatus";
        s += "%23" + "RentableArea_lblArea";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
                sCon = "%26UnitStatus=" + unitStatus;
        sCon += "%26UnitCode=" + GetStrNull(this.txtUnitCode.Text);
        sCon += "%26BuildingName=" + GetStrNull(this.ddlBuildingName.SelectedValue);
        sCon += "%26FloorName=" + GetStrNull(this.ddlFloorName.SelectedValue);
        sCon += "%26LocationName=" + GetStrNull(this.ddlLocationName.SelectedValue);
        sCon += "%26AreaName=" + GetStrNull(this.ddlAreaName.SelectedValue);

        return sCon;
    }

    private void rdoSel()
    {
        if (this.rdoNoLeaseOut.Checked)
            unitStatus = "1";
        else if (this.rdoLeaseOut.Checked)
            unitStatus = "0";
        else if (this.rdoBlankOut.Checked)
            unitStatus = "2";
        else if (this.rdoall.Checked)
            unitStatus = "3";
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
 
        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTotalAmt";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXMallTitle";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = Session["MallTitle"].ToString();
        paraField[2].CurrentValues.Add(discreteValue[2]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        string func_sql = "";
        if (AuthBase.GetAuthUser(objSessionUser.UserID) > 0)
        {
            func_sql = " And  Unit.UnitID in (" + AuthBase.AUTH_SQL_UnitID + objSessionUser.UserID + ")";
        }


        string str_sql = " select conshop.shopname,traderelation.tradename,shoptype.shoptypename,store.storeshortname,Unit.UnitCode,Building.BuildingName,Floors.FloorName,Location.LocationName,Unit.FloorArea,Unit.usearea as RentArea," +
                         " Case  when Unit.UnitStatus=0 then '" + (String)GetGlobalResourceObject("Parameter", "Unit_NoLeaseOut") + "' when Unit.UnitStatus=1 then '" + (String)GetGlobalResourceObject("Parameter", "Unit_LeaseOut") + "' when Unit.UnitStatus=2 then '" + (String)GetGlobalResourceObject("Parameter", "Unit_Nonrentable") + "' End as UnitStatus," +
                         " Area.AreaName" +
                         " from Unit inner join store on store.storeid=unit.storeid" +
                        " inner join Area on (area.areaid=unit.areaid)" +
                        " inner join Building on (building.buildingid=unit.buildingid)" +
                        " inner join Floors on (Floors.floorid=unit.floorid)" +
                        " inner join Location on (Location.Locationid=unit.Locationid)" +
                        " inner join shoptype on (shoptype.shoptypeid=unit.shoptypeid)" +
                        " inner join traderelation on (traderelation.tradeid=unit.trade2id)" +
                        " left join conshopunit on (conshopunit.unitid=unit.unitid)" +
                        " left join conshop on (conshop.shopid=conshopunit.shopid)" +
                         " where 1=1 " + 
                         func_sql;

        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + "and store.storeid = '"+ddlBizproject.SelectedValue+" '";
        }
        if (txtUnitCode.Text != "")
        {
            str_sql = str_sql + " AND Unit.UnitCode like '%" + txtUnitCode.Text + "%'";
        }
        if (ddlBuildingName.Text != "")
        {
            str_sql = str_sql + " AND Building.BuildingID = '" + ddlBuildingName.SelectedValue + "'";
        }
        if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND Floors.FloorID = '" + ddlFloorName.SelectedValue + "'";
        }
        //if (ddlLocationName.Text != "")
        //{
        //    str_sql = str_sql + " AND Location.LocationCode = '" + ddlLocationName.SelectedValue + "'";
        //}
        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND Area.AreaCode = '" + ddlAreaName.SelectedValue + "'";
        }
        if (this.rdoNoLeaseOut.Checked)
            str_sql = str_sql + " AND unitStatus = '0'";
        else if (this.rdoLeaseOut.Checked)
            str_sql = str_sql + " AND unitStatus = '1'";
        else if (this.rdoBlankOut.Checked)
            str_sql = str_sql + " AND unitStatus = '2'";
        else if (this.rdoall.Checked)
            str_sql += " And unitStatus in (0,1)";


        //string strTemp = BaseInfo.Dept.DeptSaleAuth.GetStore(objSessionUser.DeptID);
        //if (strTemp.Trim() != "")
        //{
        //    str_sql += " And Unit.StoreID in (" + strTemp + ")";
        //}
        

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\UnitInfo.rpt";

    }
    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBuilding();
    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindFloor();
    }
    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void bindBuilding()
    {
        //绑定楼
        BaseBO baseBO = new BaseBO();
        ddlBuildingName.Items.Clear();
        ddlFloorName.Items.Clear();
        if (ddlBizproject.Text != "")
        {
            baseBO.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + " AND StoreID = " +ddlBizproject.SelectedValue.ToString();
            Resultset rs = baseBO.Query(new Building());
            ddlBuildingName.Items.Clear();
            ddlBuildingName.Items.Add(new ListItem("", ""));
            foreach (Building bd in rs)
                ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        }

        //绑定区域
        // baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        ddlAreaName.Items.Clear();
        if (ddlBizproject.Text != "")
        {
            baseBO.WhereClause = "StoreID =" + ddlBizproject.SelectedValue.ToString();
            Resultset rs3 = baseBO.Query(new Area());

            ddlAreaName.Items.Add(new ListItem("", ""));
            foreach (Area bd in rs3)
            {
                ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaCode));
            }
        }
    }
    protected void bindFloor()
    {

        //绑定楼层
        ddlFloorName.Items.Clear();
        if (ddlBuildingName.Text != "")
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "FloorStatus = " + Floors.FLOOR_STATUS_VALID + " AND BuildingID = " + ddlBuildingName.SelectedValue.ToString();
            Resultset rs1 = baseBO.Query(new Floors());
            ddlFloorName.Items.Add(new ListItem("", ""));
            foreach (Floors bd in rs1)
                ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptBase/RptUnitInfo.aspx");
    }
}
