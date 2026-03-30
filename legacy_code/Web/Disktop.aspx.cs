using System;
using Base.Page;
using BaseInfo.User;
using Base.XML;
using Base.DB;
using System.Data;
using Base.Biz;
using System.IO;
using RentableArea;
public partial class Disktop : BasePage
{
    string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {        
        baseInfo = (string)GetGlobalResourceObject("BaseInfo", "Menu_VAtongzhou");        
        //生成VA菜单
        //BaseInfo.User.VAMenu.GetVAMenu(101, sessionUser);
        //生成kpi的xml
        UpdateFlash();
        UpdateCityFlash();
        UpdateStoreFlash();
        UpdateBuildFlash();
        UpdateFloorFlash();
        UpdateGpFlash();
    }

    private void InitPage()
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string User = "";
        User = sessionUser.UserID.ToString();
        Hiduser.Value = User;
        //if (Request.QueryString["FloorID"] != null && Request.QueryString["FloorID"] != "")
        //{
        //    if (Request.QueryString["FloorName"] != null && Request.QueryString["FloorName"] != "")
        //    {
        //        hidFloorID.Value = Request.QueryString["FloorID"].ToString() + "^" + Request.QueryString["FloorName"].ToString();
        //    }
        //}
        //else
        //{
        //    hidFloorID.Value = "";
        //}
        if (Directory.Exists("E:\\work\\mi_net\\code\\web\\VisualAnalysis\\VAMenu\\" + sessionUser.UserID.ToString()) == false)
        {
            Directory.CreateDirectory("E:\\work\\mi_net\\code\\web\\VisualAnalysis\\VAMenu\\" + sessionUser.UserID.ToString());
        }
    }

    /// <summary>
    /// 生成集团KPI指标
    /// </summary>
    /// <returns></returns>
    private void UpdateGpFlash()
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//Gp";

        BaseBO baseBo = new BaseBO();      
        AngularXml angular = new AngularXml();
        DataSet dsTemp = new DataSet();
        string strsql ="select sum(yearamt)/10000 budget," +
                       "(select sum(invactpayamtl) from invoiceheader where year(invperiod)=budget.budgetyear)/10000 invpay " +
                       "from budget where budgetyear=" + DateTime.Now.AddYears(-1).Year.ToString() +
                       " group by budgetyear";
        dsTemp=baseBo.QueryDataSet(strsql);
        if (dsTemp.Tables[0].Rows.Count==1)
        {
            int lower=Convert.ToInt32(Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["budget"].ToString())/2);
            int upper = Convert.ToInt32(Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["budget"].ToString()) * 3 / 2);
            angular.LowerLimit = lower;
            angular.UpperLimit = upper;
            angular.LowValue = Convert.ToInt32(Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["budget"].ToString()) * 4 / 5);
            angular.MaxValue1 = Convert.ToInt32(Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["budget"].ToString()) * 4 / 5);
            angular.MaxValue2 = Convert.ToInt32(Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["budget"].ToString()) * 6 / 5);
            angular.LowValueTxt = "最低完成";
            angular.SmlTital = "单位:万元";
            angular.BigTital = "集团" + DateTime.Now.Year.ToString() + "年预算完成情况";
            angular.TotalTxt = "1-" + DateTime.Now.AddMonths(-1).Month.ToString() + "月";
            angular.TotalValue = Convert.ToInt32(Convert.ToDecimal(dsTemp.Tables[0].Rows[0]["invpay"].ToString()));

            angular.GetXml(strFile + "kpi1.xml");
        }


        DataTable dt = new DataTable();
        dsTemp = new DataSet();
        Pie3DXML pie3d = new Pie3DXML();
        strsql = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                        "FROM (select unitrent.storeid AS storeID,unitrent.useArea AS useArea," +
                        "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                        "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                        "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                        "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("2009-12-01") + "' " +
                        ") AS a GROUP BY a.RentStatus";

        dsTemp = baseBo.QueryDataSet(strsql);
        dt = dsTemp.Tables[0];    
        pie3d.Caption = "集团" + DateTime.Now.ToString("MM") + "月商铺租赁状况";
        pie3d.GetXml(strFile + "kpi2.xml" , dt);

        //生成集团kpi3
        pie3d = new Pie3DXML();
        dt = new DataTable();
        strsql = "select aa.invstatus, case aa.invstatus when '已付' then sum(aa.invpaidamt) " +
                 "when '欠款' then sum(aa.oweamt) end amt from ( " +
                 "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                 "else '已付' end invstatus from invoiceheader where invoiceheader.invperiod='" + DateTime.Now.ToString("2009-12-01") + "') as aa " +
                 "group by aa.invstatus";
        dsTemp = baseBo.QueryDataSet(strsql);
        dt = dsTemp.Tables[0];  
        pie3d.Caption = "集团" + DateTime.Now.AddMonths(-1).ToString("MM") + "月份租赁收入汇总";
        pie3d.GetXml(strFile + "kpi3.xml", dt);

        this.xmlFiles.InnerText = "../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/gpKpi1.xml";
        this.xmlFiles2.InnerText = "../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/gpKpi2.xml";
        this.xmlFiles3.InnerText = "../VisualAnalysis/VAMenu/" + sessionUser.UserID.ToString() + "/gpKpi3.xml";
    }
    /// <summary>
    /// 生成所有大区kpi
    /// </summary>
    /// <returns></returns>
    private void UpdateFlash()
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//loca";

        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "DeptType=" + BaseInfo.Dept.Dept.DEPT_TYPE_REGION;
        DataSet ds = baseBo.QueryDataSet(new BaseInfo.Dept.Dept());
        
        int intRow = ds.Tables[0].Rows.Count;
        if (intRow > 1)
        {
            AngularXml angular = new AngularXml();
            Pie3DXML pie3d = new Pie3DXML();
            DataTable dt = new DataTable();
            for(int i = 0 ; i<intRow ; i++)
            {
                angular = new AngularXml();
                angular.LowerLimit = 100;
                angular.LowValue = 160;
                angular.MaxValue1 = 70;
                angular.MaxValue2 = 150;
                angular.LowValueTxt = "最低完成";
                angular.SmlTital = "单位:百万元";
                angular.BigTital = ds.Tables[0].Rows[i]["DeptName"].ToString() + DateTime.Now.Year.ToString() + "年度预算完成情况";
                angular.TotalTxt = "1-" + DateTime.Now.AddMonths(-1).Month.ToString() + "月";
                angular.TotalValue = 180;
                angular.UpperLimit = 200;
                angular.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi1.xml");

                dt = new DataTable();
                DataSet dsTemp = new DataSet();
                pie3d = new Pie3DXML();
                string strsql = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.storeid AS storeID,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" +DateTime.Now.ToString("2009-12-01") + "' " +
                                ") AS a inner join dept on (dept.deptid=a.storeid) " +
                                "inner join (select deptid,deptname,pdeptid from dept where depttype=5) aa on (aa.deptid=dept.pdeptid) " +
                                "inner join (select deptid,deptname,pdeptid from dept where depttype=4) bb on (aa.pdeptid=bb.deptid) " +
                                "where bb.deptid=" + ds.Tables[0].Rows[i]["DeptID"].ToString() +
                                "GROUP BY bb.deptid,a.RentStatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];    
                pie3d.Caption = ds.Tables[0].Rows[i]["DeptName"].ToString() + DateTime.Now.ToString("MM") + "月份商铺租赁状况";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi2.xml", dt);

                dt = new DataTable();
                pie3d = new Pie3DXML();
                strsql = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                         "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                         "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                         "else '已付' end invstatus from invoiceheader " +
                         "inner join conshop on (conshop.contractid=invoiceheader.contractid) inner join dept on (dept.deptid=conshop.storeid) " +
                         "left join (select deptid,deptname,pdeptid from dept where depttype=5) aa " +
                            "on (aa.deptid=dept.pdeptid)" +
                         "left join (select deptid,deptname from dept where depttype=4) bb on (aa.pdeptid=bb.deptid) " +
                         "where invoiceheader.invperiod='" + DateTime.Now.ToString("2009-12-01") + "' and bb.deptid=" + ds.Tables[0].Rows[i]["DeptID"].ToString() + ") as aaa " +
                         "group by aaa.invstatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];
                pie3d.Caption = ds.Tables[0].Rows[i]["DeptName"].ToString() + DateTime.Now.AddMonths(-1).ToString("MM") + "月份租赁收入汇总";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi3.xml", dt);
            }            
        }
    }
    /// <summary>
    /// 生成所有城市Kpi
    /// </summary>
    /// <returns></returns>
    private void UpdateCityFlash()
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//city";
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "DeptType=" + BaseInfo.Dept.Dept.DEPT_TYPE_CITY;
        DataSet ds = baseBo.QueryDataSet(new BaseInfo.Dept.Dept());
        int intRow = ds.Tables[0].Rows.Count;

        if (intRow > 0)
        {
            AngularXml angular = new AngularXml();
            Pie3DXML pie3d = new Pie3DXML();
            DataTable dt = new DataTable();
            for (int i = 0; i < intRow; i++)
            {
                angular.LowerLimit = 100;
                angular.LowValue = 160;
                angular.MaxValue1 = 150;
                angular.MaxValue2 = 250;
                angular.LowValueTxt = "最低完成";
                angular.SmlTital = "单位:元";
                angular.BigTital =DateTime.Now.Year.ToString() +"年度预算完成情况";
                angular.TotalTxt = "1-" + DateTime.Now.AddMonths(-1).ToString("MM") +"月";
                angular.TotalValue = 250;
                angular.UpperLimit = 300;
                angular.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi1.xml");

                dt = new DataTable();
                DataSet dsTemp = new DataSet();
                pie3d = new Pie3DXML();
                string strsql = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.storeid AS storeID,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("2009-12-01") + "' " +
                                ") AS a inner join dept on (dept.deptid=a.storeid) " +
                                "inner join (select deptid,deptname,pdeptid from dept where depttype=5) aa on (aa.deptid=dept.pdeptid) " +
                                "inner join (select deptid,deptname,pdeptid from dept where depttype=4) bb on (aa.pdeptid=bb.deptid) " +
                                "where aa.deptid=" + ds.Tables[0].Rows[i]["DeptID"].ToString() +
                                "GROUP BY aa.deptid,a.RentStatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];    
                pie3d.Caption =  DateTime.Now.ToString("MM") +"月份商铺租赁状况";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi2.xml", dt);


                pie3d = new Pie3DXML();
                dt = new DataTable();
                strsql = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                         "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                         "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                         "else '已付' end invstatus from invoiceheader " +
                         "inner join conshop on (conshop.contractid=invoiceheader.contractid) inner join dept on (dept.deptid=conshop.storeid) " +
                         "left join (select deptid,deptname,pdeptid from dept where depttype=5) aa " +
                            "on (aa.deptid=dept.pdeptid)" +
                         "left join (select deptid,deptname from dept where depttype=4) bb on (aa.pdeptid=bb.deptid) " +
                         "where invoiceheader.invperiod='" + DateTime.Now.ToString("2009-12-01") + "' and aa.deptid=" + ds.Tables[0].Rows[i]["DeptID"].ToString() + ") as aaa " +
                         "group by aaa.invstatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];
                pie3d.Caption = DateTime.Now.AddMonths(-1).ToString("MM") + "月份租赁收入汇总";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi3.xml", dt);
            }
        }
    }

    /// <summary>
    /// 生成所有购物中心Kpi
    /// </summary>
    /// <returns></returns>
    private void UpdateStoreFlash()
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//mall";
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "DeptType=" + BaseInfo.Dept.Dept.DEPT_TYPE_MALL;
        DataSet ds = baseBo.QueryDataSet(new BaseInfo.Dept.Dept());
        int intRow = ds.Tables[0].Rows.Count;

        if (intRow > 0)
        {
            AngularXml angular = new AngularXml();
            Pie3DXML pie3d = new Pie3DXML();
            DataTable dt = new DataTable();
            for (int i = 0; i < intRow; i++)
            {
                angular.LowerLimit = 100;
                angular.LowValue = 160;
                angular.MaxValue1 = 150;
                angular.MaxValue2 = 250;
                angular.LowValueTxt = "最低完成";
                angular.SmlTital = "单位:元";
                angular.BigTital = DateTime.Now.ToString("yyyy") + "年度预算完成情况";
                angular.TotalTxt = "1-" + DateTime.Now.AddMonths(-1).Month.ToString() + "月";
                angular.TotalValue = 250;
                angular.UpperLimit = 300;
                angular.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi1.xml");

                dt = new DataTable();
                DataSet dsTemp = new DataSet();
                pie3d = new Pie3DXML();
                string strsql = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.storeid AS storeID,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("2009-12-01") + "' " +
                                ") AS a inner join dept on (dept.deptid=a.storeid) " +
                                "where dept.deptid=" + ds.Tables[0].Rows[i]["DeptID"].ToString() +
                                "GROUP BY dept.deptid,a.RentStatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];
                pie3d.Caption = DateTime.Now.ToString("MM") + "月份商铺租赁状况";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi2.xml", dt);


                pie3d = new Pie3DXML();
                dt = new DataTable();
                strsql = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                         "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                         "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                         "else '已付' end invstatus from invoiceheader " +
                         "inner join conshop on (conshop.contractid=invoiceheader.contractid) inner join dept on (dept.deptid=conshop.storeid) " +
                         "where invoiceheader.invperiod='" + DateTime.Now.ToString("2009-12-01") + "' and dept.deptid=" + ds.Tables[0].Rows[i]["DeptID"].ToString() + ") as aaa " +
                         "group by aaa.invstatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];
                pie3d.Caption = DateTime.Now.AddMonths(-1).ToString("MM") + "月份租赁收入汇总";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["DeptID"].ToString() + "kpi3.xml", dt);
            }
        }
    }
    /// <summary>
    /// 生成所有大楼的kpi
    /// </summary>
    private void UpdateBuildFlash()
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//build";
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BuildingStatus=" + Building.BUILDING_STATUS_VALID ;
        DataSet ds = baseBo.QueryDataSet(new Building());
        int intRow = ds.Tables[0].Rows.Count;

        if (intRow > 0)
        {
            AngularXml angular = new AngularXml();
            Pie3DXML pie3d = new Pie3DXML();
            DataTable dt = new DataTable();
            for (int i = 0; i < intRow; i++)
            {
                angular.LowerLimit = 100;
                angular.LowValue = 160;
                angular.MaxValue1 = 150;
                angular.MaxValue2 = 250;
                angular.LowValueTxt = "最低完成";
                angular.SmlTital = "单位:元";
                angular.BigTital = DateTime.Now.Year.ToString() + "年度预算完成情况";
                angular.TotalTxt = "1-" + DateTime.Now.AddMonths(-1).Month.ToString() + "月";
                angular.TotalValue = 250;
                angular.UpperLimit = 300;
                angular.GetXml(strFile + ds.Tables[0].Rows[i]["BuildingID"].ToString() + "kpi1.xml");

                dt = new DataTable();
                DataSet dsTemp = new DataSet();
                pie3d = new Pie3DXML();
                string strsql = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.Buildingid AS Buildingid,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("2009-12-01") + "' " +
                                ") AS a " +
                                "where a.Buildingid=" + ds.Tables[0].Rows[i]["BuildingID"].ToString() +
                                "GROUP BY a.Buildingid,a.RentStatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];
                pie3d.Caption = DateTime.Now.ToString("MM") + "月份商铺租赁状况";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["BuildingID"].ToString() + "kpi2.xml", dt);


                pie3d = new Pie3DXML();
                dt = new DataTable();
                strsql = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                         "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                         "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                         "else '已付' end invstatus from invoiceheader " +
                         "inner join conshop on (conshop.contractid=invoiceheader.contractid) " +
                         "where invoiceheader.invperiod='" + DateTime.Now.ToString("2009-12-01") + "' and conshop.BuildingID=" + ds.Tables[0].Rows[i]["BuildingID"].ToString() + ") as aaa " +
                         "group by aaa.invstatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];
                pie3d.Caption =  DateTime.Now.AddMonths(-1).ToString("MM") + "月份租赁收入汇总";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["BuildingID"].ToString() + "kpi3.xml", dt);
            }
        }

    }
    /// <summary>
    /// 生成所有楼层的kpi
    /// </summary>
    private void UpdateFloorFlash()
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strFile = "E://work//mi_net//code//web//VisualAnalysis//VAMenu//" + sessionUser.UserID.ToString() + "//floor";
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID ;
        DataSet ds = baseBo.QueryDataSet(new Floors());
        int intRow = ds.Tables[0].Rows.Count;

        if (intRow > 0)
        {
            AngularXml angular = new AngularXml();
            Pie3DXML pie3d = new Pie3DXML();
            DataTable dt = new DataTable();
            for (int i = 0; i < intRow; i++)
            {
                angular.LowerLimit = 100;
                angular.LowValue = 160;
                angular.MaxValue1 = 150;
                angular.MaxValue2 = 250;
                angular.LowValueTxt = "最低完成";
                angular.SmlTital = "单位:元";
                angular.BigTital =DateTime.Now.Year.ToString() + "年度预算完成情况";
                angular.TotalTxt = "1-" + DateTime.Now.AddMonths(-1).Month.ToString() + "月";
                angular.TotalValue = 250;
                angular.UpperLimit = 300;
                angular.GetXml(strFile + ds.Tables[0].Rows[i]["FloorID"].ToString() + "kpi1.xml");

                dt = new DataTable();
                DataSet dsTemp = new DataSet();
                pie3d = new Pie3DXML();
                string strsql = "SELECT a.RentStatus,sum(a.useArea) useArea " +
                                "FROM (select unitrent.Floorid AS Floorid,unitrent.useArea AS useArea," +
                                "(CASE unitRent.shopid WHEN 0 THEN '空置' " +
                                "ELSE (CASE WHEN datediff(month,getdate(),conShop.shopEndDate) <3 then '即将到期' ELSE '正常' END ) " +
                                "END) AS RentStatus  FROM unitrent LEFT JOIN " +
                                "conShop ON (unitRent.shopid = conShop.shopID) WHERE period = '" + DateTime.Now.ToString("2009-12-01") + "' " +
                                ") AS a " +
                                "where a.Floorid=" + ds.Tables[0].Rows[i]["FloorID"].ToString() +
                                "GROUP BY a.Floorid,a.RentStatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];
                pie3d.Caption = DateTime.Now.ToString("MM") + "月份商铺租赁状况";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["FloorID"].ToString() + "kpi2.xml", dt);


                pie3d = new Pie3DXML();
                dt = new DataTable();
                strsql = "select aaa.invstatus, case aaa.invstatus when '已付' then sum(aaa.invpaidamt) " +
                         "when '欠款' then sum(aaa.oweamt) end amt from ( " +
                         "select (invactpayamt-invpaidamtl) oweamt, invpaidamt,case when (invactpayamt-invpaidamtl)>0 then '欠款' " +
                         "else '已付' end invstatus from invoiceheader " +
                         "inner join conshop on (conshop.contractid=invoiceheader.contractid) " +
                         "where invoiceheader.invperiod='" + DateTime.Now.ToString("2009-12-01") + "' and conshop.FloorID=" + ds.Tables[0].Rows[i]["FloorID"].ToString() + ") as aaa " +
                         "group by aaa.invstatus";
                dsTemp = baseBo.QueryDataSet(strsql);
                dt = dsTemp.Tables[0];
                pie3d.Caption =  DateTime.Now.AddMonths(-1).ToString("MM") + "月租赁收入汇总";
                pie3d.GetXml(strFile + ds.Tables[0].Rows[i]["FloorID"].ToString() + "kpi3.xml", dt);
            }

        }
    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        InitializeComponent();
        base.OnInit(e);
    }

    private void InitializeComponent()
    {
        this.InitPage();
    }
    #endregion


}
