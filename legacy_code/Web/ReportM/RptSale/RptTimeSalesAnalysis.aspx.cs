using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.Contract;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;


/// <summary>
/// 修改人：hesijian
/// 修改时间：2009年6月16日
/// </summary>
public partial class RptBaseMenu_RptTimeSalesAnalysis : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
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



        //绑定区域
        baseBo.WhereClause = "AreaStatus = " + Area.AREA_STATUS_VALID;
        Resultset rs3 = baseBo.Query(new Area());
        ddlAreaName.Items.Add(new ListItem("", ""));
        foreach (Area bd in rs3)
            ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));

        //绑定一级经营类别



        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        Resultset rs4 = baseBo.Query(new TradeRelation());
        ddlTradeID.Items.Add(new ListItem("", ""));
        foreach (TradeRelation tradeDef in rs4)
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
        String s = "%23Rpt_lblTimeSalesAnalysis";
        s += "%23" + "Rpt_SalesTime";
        s += "%23" + "Rpt_TotalReceipt";
        s += "%23" + "Rpt_GrossSales";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        sCon += "%26BizSDate=" + GetdateNull(this.txtBizSDate.Text);
        sCon += "%26BizEDate=" + GetdateNull(this.txtBizEDate.Text);
        //sCon += "%26SBizSTime=" + GetdateNull(this.txtStartBizTime.Text);
        //sCon += "%26EBizETime=" + GetdateNull(this.txtEndBizTime.Text);
        return sCon;
    }

    //水晶报表数据绑定
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[10];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[10];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTransTime";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TransTime");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXtrNum";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_trNum");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXPaidAmt";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXTitle";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptTimeSalesAnalysis_Title");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXTotalAmt";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXQBDate";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = txtBizSDate.Text;
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXQEDate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = txtBizEDate.Text;
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXSdate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_BizDate");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXMallTitle";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = Session["MallTitle"].ToString();
        paraField[8].CurrentValues.Add(discreteValue[8]);


        paraField[9] = new ParameterField();
        paraField[9].Name = "REXBizProject";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[9].CurrentValues.Add(discreteValue[9]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string strWhere1 = "";
        string str_case="";

        //数据来源
        if (RB5.Checked)
        {
            strWhere1 = "";
        }
        if (RB6.Checked)
        {
            strWhere1 = "AND TransSkuMedia.datasource=1";
        }
        if (RB7.Checked)
        {
            strWhere1 = "AND TransSkuMedia.datasource=2";
        }
        if (RB8.Checked)
        {
            strWhere1 = "AND TransSkuMedia.datasource=3";
        }
        if (rdo61.Checked)   //15分
        {
            str_case = "(CASE WHEN datepart(mi,transtime) BETWEEN  0 AND 15 THEN RIGHT(convert(char(13),transtime,120),2)+':00 - '+RIGHT(convert(char(13),transtime,120),2)+':15' " +
                       " WHEN datepart(mi,transtime) BETWEEN 16 AND 30 THEN RIGHT(convert(char(13),transtime,120),2)+':16 - '+RIGHT(convert(char(13),transtime,120),2)+':30'" +
                       " WHEN datepart(mi,transtime) BETWEEN 31 AND 45 THEN RIGHT(convert(char(13),transtime,120),2)+':31 - '+RIGHT(convert(char(13),transtime,120),2)+':45'" +
                       " WHEN datepart(mi,transtime) BETWEEN 46 AND 59 THEN RIGHT(convert(char(13),transtime,120),2)+':46 - '+RIGHT(convert(char(13),transtime,120),2)+':59'" +
                       " END)";
        }
        else if (rdo62.Checked)  //30分
        {
            str_case = "(CASE WHEN datepart(mi,transtime) BETWEEN  0 AND 30 THEN RIGHT(convert(char(13),transtime,120),2)+':00 - '+RIGHT(convert(char(13),transtime,120),2)+':30'" +
                       " WHEN datepart(mi,transtime) BETWEEN 31 AND 59 THEN RIGHT(convert(char(13),transtime,120),2)+':31 - '+RIGHT(convert(char(13),transtime,120),2)+':59'" +
                       " END)";
        }
        else
        {
            str_case = "(RIGHT(convert(char(13),transtime,120),2)+':00 - '+RIGHT(convert(char(13),transtime,120),2)+':59')";
        }
        string str_where = "";

        //条件查询
        if (txtBizSDate.Text != "")
        {
            str_where = str_where + " AND TransSkuMedia.BizDate >='" + txtBizSDate.Text + "'";
        }
        if (txtBizEDate.Text != "")
        {
            str_where = str_where + " AND TransSkuMedia.BizDate  <='" + txtBizEDate.Text + "'";
        }
        if (txtStartBizTime.Text != "" && txtEndBizTime.Text != "")
        {
            str_where = str_where + " AND datepart(hh,TransSkuMedia.TransTime) BETWEEN '" + txtStartBizTime.Text + "' and '" + txtEndBizTime.Text + "' ";
        }
        if (ddlBizproject.Text != "")
        {
            str_where = str_where + " AND TransSkuMedia.Storeid ='" + ddlBizproject.SelectedValue.ToString().Trim() + "'";
        }
        string str_sql = " select store.storeid,store.storeshortname,TransM.transTime,Sum(PaidAmt) as PaidAmt,Count(Transid) as TrNum from " +
                            " ( SELECT transSkuMedia.ShopID," + str_case + " AS transTime," +
                            " Sum(PaidAmt) PaidAmt,Count(transid) Transid from TransSkuMedia where 1=1 " + str_where + strWhere1 + " group by transSkuMedia.ShopID,transTime,Transid )  TransM" +
                            " inner join ConShop on (TransM.ShopId=ConShop.ShopId)" +
                            " inner join store on (conshop.storeid=store.storeid) " +
                            " inner join Contract On (ConShop.ContractId=Contract.ContractId)";
        //if (txtShopCode.Text != "")
        //{
        //    str_sql = str_sql + " AND ConShop.ShopCode ='" + txtShopCode.Text + "'";
        //}
        if (ddlBizMode.Text != "")
        {
            str_sql = str_sql + " AND Contract.BizMode ='" + ddlBizMode.SelectedValue + "'";
        }
        if (ddlBuildingName.Text != "")
        {
            str_sql = str_sql + " AND ConShop.BuildingID = '" + ddlBuildingName.SelectedValue + "'";
        }
        if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND ConShop.FloorId ='" + ddlFloorName.SelectedValue + "'";
        }
        if (ddlLocationName.Text != "")
        {
            str_sql = str_sql + " AND ConShop.LocationId ='" + ddlLocationName.SelectedValue + "'";
        }
        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND ConShop.AreaId ='" + ddlAreaName.SelectedValue + "'";
        }
        if (ddlTradeID.Text != "")
        {
            str_sql = str_sql + " AND Contract.TradeID ='" + ddlTradeID.SelectedValue + "'";
        }


        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        str_sql = str_sql + " group by TransM.transTime,store.storeid,store.storeshortname order by TransM.transTime";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\TimeSalesAnalysis.rpt";
    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定楼层
        ddlFloorName.Items.Clear();
        ddlLocationName.Items.Clear();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "FloorStatus = " + Floors.FLOOR_STATUS_VALID + " AND BuildingID = " + ddlBuildingName.SelectedValue.ToString();
        Resultset rs1 = baseBO.Query(new Floors());
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
    }
    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定方位
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "LocationStatus = " + Location.LOCATION_STATUS_VALID + " AND FloorID = " + ddlFloorName.SelectedValue;
        Resultset rs2 = baseBO.Query(new Location());
        ddlLocationName.Items.Add(new ListItem("", ""));
        foreach (Location bd in rs2)
        {
            ddlLocationName.Items.Add(new ListItem(bd.LocationName, bd.LocationID.ToString()));
        }
    }
    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBuildingName.Items.Clear();
        ddlFloorName.Items.Clear();
        ddlLocationName.Items.Clear();
        /*绑定大楼*/
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID+"and storeid='"+ddlBizproject.SelectedValue+"'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
        {
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptTimeSalesAnalysis.aspx");
    }
}
