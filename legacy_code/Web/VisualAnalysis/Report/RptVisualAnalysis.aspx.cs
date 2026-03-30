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
using CrystalDecisions.CrystalReports.Engine;
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
using BaseInfo.Role;
using BaseInfo.User;
using BaseInfo.authUser;
/// <summary>
/// 编写人:何思键
/// 编写时间:2009年4月10日
/// </summary>
public partial class Report_VisualAnalysis:BasePage
{
    public string baseInfo = "";
    public string floorName = "";
    public string buildingName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
        Session["subReportSql"] = "";
        Session["subRpt"] = "";


        string backhome = "";

        //默认大楼ID ，楼层ID
        int BuildingID = 0;
        int FloorID = 0;

        //得到传入的参数
        string BID = Request.QueryString["BuildingID"]+"";
        string FID = Request.QueryString["FloorID"] + "";

        if (BID.Length == 0)
        {
            BuildingID = 0;
            if (FID.Length > 0 && !FID.Contains("d"))
            {
                FloorID = int.Parse(FID);
            }
            else
            {
                FloorID = 0;
            }

            if (BuildingID == 0)
            {
                BaseBO baseBo = new BaseBO();
                baseBo.WhereClause = " Floors.FloorID=" + FloorID;
                Resultset rs = baseBo.Query(new Floors());
                foreach (Floors floor in rs)
                {
                    floorName = floor.FloorName;
                }
            }
            backhome = (String)GetGlobalResourceObject("BaseInfo", "Login_Backhome");
            txtHidden.Value ="Disktop.aspx?FloorID=" + FloorID+"&FloorName="+floorName;
            BindData(BuildingID, FloorID);
            //this.Response.Redirect("\\ReportM\\ReportShow.aspx");
        }
        else
        {
            FloorID = 0;
            if (BID.Length > 0 && !BID.Contains("d"))
            {
                BuildingID = int.Parse(BID);
            }
            else
            {
                BuildingID = 0;

            }
            backhome = (String)GetGlobalResourceObject("BaseInfo", "Login_Backhome");
            txtHidden.Value =  "Disktop.aspx";
            BindData(BuildingID, FloorID);
            //this.Response.Redirect("\\ReportM\\ReportShow.aspx");
            
        }
        if (!IsPostBack)
        {

        }
    }

    //绑定数据
    private void BindData(int BuildingID, int FloorID)
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[8];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[8];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        //标题
        Field[0] = new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = Session["MallTitle"].ToString();
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        //大楼名称
        Field[1] = new ParameterField();
        Field[1].Name = "REXBuilding";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_BuildingName");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        //楼层
        Field[2] = new ParameterField();
        Field[2].Name = "REXFloors";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_FloorName");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        //出租率
        Field[3] = new ParameterField();
        Field[3].Name = "REXPerRent";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_PerRent");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        //出租率分析报表
        Field[4] = new ParameterField();
        Field[4].Name = "REXVisualAnalysisRent";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_VisualAnalysisRent");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        //打印日期
        Field[5] = new ParameterField();
        Field[5].Name = "REXPrintDate";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtDate");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        //大楼名称
        if (FloorID == 0)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = " Building.BuildingID=" + BuildingID;
            Resultset rs = baseBo.Query(new Building());
            foreach (Building building in rs)
            {
                buildingName = building.BuildingName;
            }
        }
        Field[6] = new ParameterField();
        Field[6].Name = "REXBuildingName";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = buildingName;
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        //楼层名称
        Field[7] = new ParameterField();
        Field[7].Name = "REXFloorName";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = floorName;
        Field[7].CurrentValues.Add(DiscreteValue[7]);


        

        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }


        string  str_sql = "";
        string strAnd = "";




       
        //SQl查询楼层出租率
        if (FloorID == 0)
        {
            strAnd = " And unit.buildingid = " + BuildingID;
            str_sql = " select unit.floorid,unit.buildingid,CASE unit.unitstatus WHEN 1 then '" + (String)GetGlobalResourceObject("ReportInfo", "Rpt_RentOk") + "' else '" + (String)GetGlobalResourceObject("ReportInfo", "Rpt_RentNo") + "' end AS status,unit.unitID from unit where unitstatus <2" + strAnd+"";
        }
        else
        {
            strAnd = "AND unit.floorid=" + FloorID;
            str_sql = " select unit.floorid,unit.buildingid,CASE unit.unitstatus WHEN 1 then '" + (String)GetGlobalResourceObject("ReportInfo", "Rpt_RentOk") + "' else '" + (String)GetGlobalResourceObject("ReportInfo", "Rpt_RentNo") + "' end AS status,unit.unitID from unit  where unitstatus <2"+ strAnd +"";
        }
        
        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\VisualAnalysis\\Report\\RptVisualAnalysis.rpt";

    }
   
}
