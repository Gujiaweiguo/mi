using System;
using System.IO;
using System.Data;
using Base.Biz;
using Base.XML;
public partial class FusionCharts_GetChartXmlData : System.Web.UI.Page
{
    private string strkpi1 = "";
    private string strkpi2 = "";
    private string strkpi3 = "";
    private string strcaption = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string strRet="";
        string charttype = "Group";
        string chartid="100";
        string kpiid="1";
        charttype = Request.QueryString["charttype"];
        chartid = Request.QueryString["chartid"];
        kpiid = Request.QueryString["kpiid"];        
        GetSql(charttype,chartid);
 
        BaseBO baseBo = new BaseBO();
        DataTable dt = new DataTable();
        StreamWriter writer =new StreamWriter(System.Web.HttpContext.Current.Response.OutputStream, System.Text.Encoding.UTF8);

        if (kpiid == "1")
        {
            AngularXml angular = new AngularXml();
            if (strkpi1 != "")
            {
                dt = baseBo.QueryDataSet(strkpi1).Tables[0];
                if (dt.Rows.Count == 1)
                {
                    int lower = Convert.ToInt32(Convert.ToDecimal(dt.Rows[0]["budget"].ToString()) / 2);
                    int upper = Convert.ToInt32(Convert.ToDecimal(dt.Rows[0]["budget"].ToString()) * 3 / 2);
                    angular.LowerLimit = lower;
                    angular.UpperLimit = upper;
                    angular.LowValue = Convert.ToInt32(Convert.ToDecimal(dt.Rows[0]["budget"].ToString()) * 4 / 5);
                    angular.MaxValue1 = Convert.ToInt32(Convert.ToDecimal(dt.Rows[0]["budget"].ToString()) * 4 / 5);
                    angular.MaxValue2 = Convert.ToInt32(Convert.ToDecimal(dt.Rows[0]["budget"].ToString()) * 6 / 5);
                    angular.LowValueTxt = "最低完成";
                    angular.SmlTital = "单位:万元";
                    angular.BigTital = strcaption;
                    angular.TotalTxt = "1-" + DateTime.Now.Month.ToString() + "月";
                    angular.TotalValue = Convert.ToInt32(Convert.ToDecimal(dt.Rows[0]["invpay"].ToString()));
                    strRet = angular.GetXml();
                }
                else
                    strRet = "<chart></chart>";
            }
            else
                strRet = "<chart></chart>";
        }
        else if (kpiid == "2")
        {
            Pie3DXML pie3d = new Pie3DXML();
            dt = baseBo.QueryDataSet(strkpi2).Tables[0];
            if (dt.Rows.Count >= 1)
            {
                pie3d.Caption = strcaption;
                strRet = "<chart></chart>";
            }
            else
                strRet = "<chart></chart>";
        }
        else if (kpiid == "3")
        {
            strRet = "<chart></chart>";
        }

        writer.Write(strRet);
        writer.Flush();

    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.Load += new System.EventHandler(this.Page_Load);
    }
    #endregion

    /// <summary>
    /// 根据KPI类型生成kpi查询相关的sql
    /// </summary>
    /// <param name="charttype">kpi类型</param>
    private void GetSql(string charttype, string chartid)
    {
        BaseBO baseBo = new BaseBO();
        string strSql = "";
        DataTable dt = new DataTable();
        switch (charttype)
        {
            case "Group":
                strSql = "select deptid,deptname from dept where deptid=" + chartid;
                dt = new DataTable();
                dt = baseBo.QueryDataSet(strSql).Tables[0];
                if (dt.Rows.Count == 1)
                    strcaption = dt.Rows[0]["deptname"].ToString() + DateTime.Now.Year.ToString() + "年预算完成情况";
                else 
                    strcaption = "";
                strkpi1 = "select sum(yearamt)/10000 budget," +
                               "(select sum(invactpayamtl) from invoiceheader where year(invperiod)=budget.budgetyear)/10000 invpay " +
                               "from budget where budgetyear=" + DateTime.Now.AddYears(-1).Year.ToString() +
                               " group by budgetyear";
                strkpi2 = "";
                strkpi3 = "";
                break;
            case "Loca":
                strSql = "select deptid,deptname from dept where deptid=" + chartid;
                dt = new DataTable();
                dt = baseBo.QueryDataSet(strSql).Tables[0];
                if (dt.Rows.Count == 1)
                    strcaption = dt.Rows[0]["deptname"].ToString() + DateTime.Now.Year.ToString() + "年预算完成情况";
                else
                    strcaption = "";
                strkpi2 = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.storeid AS storeID,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("yyyy-MM-01") + "' " +
                                ") AS a inner join dept on (dept.deptid=a.storeid) " +
                                "inner join (select deptid,deptname,pdeptid from dept where depttype=5) aa on (aa.deptid=dept.pdeptid) " +
                                "inner join (select deptid,deptname,pdeptid from dept where depttype=4) bb on (aa.pdeptid=bb.deptid) " +
                                "where bb.deptid=" + chartid +
                                "GROUP BY bb.deptid,a.RentStatus";
                strkpi1 = "";
                strkpi3 = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                                 "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                                 "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                                 "else '已付' end invstatus from invoiceheader " +
                                 "inner join conshop on (conshop.contractid=invoiceheader.contractid) inner join dept on (dept.deptid=conshop.storeid) " +
                                 "left join (select deptid,deptname,pdeptid from dept where depttype=5) aa " +
                                    "on (aa.deptid=dept.pdeptid)" +
                                 "left join (select deptid,deptname from dept where depttype=4) bb on (aa.pdeptid=bb.deptid) " +
                                 "where invoiceheader.invperiod='" + DateTime.Now.ToString("yyyy-MM-01") + "' and bb.deptid=" + chartid + ") as aaa " +
                                 "group by aaa.invstatus";
                break;
            case "City":
                strSql = "select deptid,deptname from dept where deptid=" + chartid;
                dt = new DataTable();
                dt = baseBo.QueryDataSet(strSql).Tables[0];
                if (dt.Rows.Count == 1)
                    strcaption = dt.Rows[0]["deptname"].ToString() + DateTime.Now.Year.ToString() + "年预算完成情况";
                else
                    strcaption = "";
                strkpi2 = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.storeid AS storeID,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("yyyy-MM-01") + "' " +
                                ") AS a inner join dept on (dept.deptid=a.storeid) " +
                                "inner join (select deptid,deptname,pdeptid from dept where depttype=5) aa on (aa.deptid=dept.pdeptid) " +
                                "inner join (select deptid,deptname,pdeptid from dept where depttype=4) bb on (aa.pdeptid=bb.deptid) " +
                                "where aa.deptid=" + chartid +
                                "GROUP BY aa.deptid,a.RentStatus";
                strkpi1 = "";
                strkpi3 = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                                 "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                                 "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                                 "else '已付' end invstatus from invoiceheader " +
                                 "inner join conshop on (conshop.contractid=invoiceheader.contractid) inner join dept on (dept.deptid=conshop.storeid) " +
                                 "left join (select deptid,deptname,pdeptid from dept where depttype=5) aa " +
                                    "on (aa.deptid=dept.pdeptid)" +
                                 "left join (select deptid,deptname from dept where depttype=4) bb on (aa.pdeptid=bb.deptid) " +
                                 "where invoiceheader.invperiod='" + DateTime.Now.ToString("yyyy-MM-01") + "' and aa.deptid=" + chartid + ") as aaa " +
                                 "group by aaa.invstatus";
                break;
            case "Mall":
                strSql = "select deptid,deptname from dept where deptid=" + chartid;
                dt = new DataTable();
                dt = baseBo.QueryDataSet(strSql).Tables[0];
                if (dt.Rows.Count == 1)
                    strcaption = dt.Rows[0]["deptname"].ToString() + DateTime.Now.Year.ToString() + "年预算完成情况";
                else
                    strcaption = "";
                strkpi2 = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.storeid AS storeID,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("yyyy-MM-01") + "' " +
                                ") AS a inner join dept on (dept.deptid=a.storeid) " +
                                "where dept.deptid=" + chartid +
                                "GROUP BY dept.deptid,a.RentStatus";
                strkpi1 = "";
                strkpi3 = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                                 "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                                 "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                                 "else '已付' end invstatus from invoiceheader " +
                                 "inner join conshop on (conshop.contractid=invoiceheader.contractid) inner join dept on (dept.deptid=conshop.storeid) " +
                                 "where invoiceheader.invperiod='" + DateTime.Now.ToString("yyyy-MM-01") + "' and dept.deptid=" + chartid + ") as aaa " +
                                 "group by aaa.invstatus";
                break;
            case "Floor":
                strSql = "select floorid,floorcode,floorname from floors where floorid=" + chartid;
                dt = new DataTable();
                dt = baseBo.QueryDataSet(strSql).Tables[0];
                if (dt.Rows.Count == 1)
                    strcaption = dt.Rows[0]["floorname"].ToString() + DateTime.Now.Year.ToString() + "年预算完成情况";
                else
                    strcaption = "";
                strkpi1 = "";
                strkpi2 = "";
                strkpi3 = "";
                break;
            case "Build":
                strSql = "select buildingid,buildingcode,buildingname from building where buildingid=" + chartid;
                dt = new DataTable();
                dt = baseBo.QueryDataSet(strSql).Tables[0];
                if (dt.Rows.Count == 1)
                    strcaption = dt.Rows[0]["buildingname"].ToString() + DateTime.Now.Year.ToString() + "年预算完成情况";
                else
                    strcaption = "";
                strkpi2 = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.Buildingid AS Buildingid,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("yyyy-MM-01") + "' " +
                                ") AS a inner join building on (building.buildingid=a.buildingid) " +
                                "where building.buildingid=" + chartid +
                                "GROUP BY building.buildingid,a.RentStatus";
                strkpi1 = "";
                strkpi3 = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                                 "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                                 "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                                 "else '已付' end invstatus from invoiceheader " +
                                 "inner join conshop on (conshop.contractid=invoiceheader.contractid) inner join Building on (Building.Buildingid=conshop.Buildingid) " +
                                 "where invoiceheader.invperiod='" + DateTime.Now.ToString("yyyy-MM-01") + "' and Building.Buildingid=" + chartid + ") as aaa " +
                                 "group by aaa.invstatus";
                break;
            default:
                strSql = "";
                strkpi1 = "";
                strkpi2 = "";
                strkpi3 = "";
                break;
        }

    }
}
