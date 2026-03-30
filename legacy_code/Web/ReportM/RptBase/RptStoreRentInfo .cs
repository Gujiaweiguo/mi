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
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Store;
using Shop.ShopType;
/// <summary>
/// ADD by TJM at 20090325
/// </summary>
public partial class ReportM_RptBase_RptStoreRentInfo : BasePage
{
    public string baseInfo;
    public string sRptName; 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //ddlBuildingName.Enabled = false;
            //ddlFloorName.Enabled = false;
            //ddlShopType.Enabled = false;
            //this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            InitDDL();
            //bindBuilding();
            //bindFloor();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_StoreRentInfo");
        }

    }




    /* 初始化下拉列表  */
    private void InitDDL()
    {
        BaseBO baseBO = new BaseBO();

        //绑定商业项目
        Resultset rs = baseBO.Query(new Store());
        ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));


    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
            sRptName = "";
            Session["subReportSql"] = "";
            Session["subRpt"] = "";
            //if (rdo1.Checked)
            //{
            //    sRptName = "RptStoreRentInfoDetail.rpt";
            //}
            //if (rdo2.Checked)
            //{
                sRptName = "RptStoreRentInfo.rpt";
            //}
            BindData();
            this.Response.Redirect("../ReportShow.aspx");

    }



    private void BindData()
    {
        string str_sql = "";

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        ParameterFields paraFields = new ParameterFields();

        ParameterField[] paraField = new ParameterField[12];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[12];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXPeriod";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "InvAdj_KeepAccountsMth");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXShopTypeFloors";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXAreas";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblFloorArea");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXUseAreas";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblUseArea");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXRentareas";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_Rentareas");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXAreaRate";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_AreaRate");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXUseRate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_UseRate");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXRentalAmt";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "RptStoreRentInfo_lblRentAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXAvgRentalAmt";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = "平均日租金";
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTitle";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_StoreRentInfo");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXMallTitle";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = Session["MallTitle"].ToString();
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXBizProject";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string wherestr = " ";


        if (ddlStoreName.Text != "")
        {
            wherestr = "AND store.storeid=" + ddlStoreName.SelectedValue;
        }

        str_sql = "select a.storename storeshortname ,a.period,a.areas,a.useareas,a.rentareas," +
                " (CASE WHEN a.areas <> 0 THEN Round(a.useareas/a.areas*100,2) ELSE 0 END ) AS arearate," +    //得房率
                " (CASE WHEN a.useareas <> 0 THEN Round(a.rentareas/a.useareas*100,2) ELSE 0 END ) AS userate," +   //出租率
                " 0 as RentalAmt,	0 as avgRentalAmt" +
                " from" +
                " (select store.storename,'' as period,(SELECT sum(unit.floorarea) from unit where unitstatus<>2  and unit.storeid=store.storeid ) as areas  ," +   //建筑面积
                " (SELECT sum(unit.usearea) from unit where unitstatus<>2  and unit.storeid=store.storeid ) as useareas," +   //使用面积
                " (select sum(unit.usearea) from unit where unitstatus = 1  and unit.storeid=store.storeid ) AS rentareas" +  //已出租面积、非签约面积
                " from store where 1=1 " + wherestr + 
                " ) as a";


        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\"+sRptName;

    }

    private void ClearPage()
    {
        ddlStoreName.Items.Clear();
        InitDDL();
    }


    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.ClearPage();
    }
    protected void ddlStoreName_SelectedIndexChanged(object sender, EventArgs e)
    {
       // bindBuilding();
    }
    //protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    bindFloor();
    //}
    //protected void rdo2_CheckedChanged(object sender, EventArgs e)
    //{
    //    ddlBuildingName.Enabled = true;
    //    ddlFloorName.Enabled = true;
    //    ddlShopType.Enabled = true;
    //}
    //protected void rdo1_CheckedChanged(object sender, EventArgs e)
    //{
    //    ddlBuildingName.Text = "";
    //    ddlFloorName.Text = "";
    //    ddlShopType.Text = "";
    //    ddlBuildingName.Enabled = false;
    //    ddlFloorName.Enabled = false;
    //    ddlShopType.Enabled = false;
    //}
}
