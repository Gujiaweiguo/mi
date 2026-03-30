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
using Lease.ConShop;

public partial class ReportM_RptBase_RptStoreRentalHeader : BasePage
{
    public string strBaseInfo;
    string sRptName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Init();
            ddlstatus();
            txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreRentalInfo");
        }
    }



    protected void Init()
    {
        //绑定商业项目
        BaseBO baseBo = new BaseBO();
        Resultset rs3 = baseBo.Query(new Store());
        this.ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs3)
        {
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));
        }
        //绑定商铺类型
        Resultset rs2 = baseBo.Query(new ShopType());
        ddlShopType.Items.Add(new ListItem("", ""));
        foreach (ShopType shopType in rs2)
        {
            ddlShopType.Items.Add(new ListItem(shopType.ShopTypeName, shopType.ShopTypeID.ToString()));
        }
    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ddlStoreName.Text = "";
        ddlShopType.Text = "";
        ddlFloorName.Text = "";
        ddlBuildingName.Text = "";
        txtPeriod.Text = "";
       // txtPeriod.Text = DateTime.Now.ToShortDateString();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        sRptName = "";
            Session["subReportSql"] = "";
            Session["subRpt"] = "";
            if (rdo1.Checked)
            {
                sRptName = "RptStoreRentalHeader.rpt";
            }
            if (rdo2.Checked)
            {
                sRptName = "RptStoreRentalDetail.rpt";
            }

            BindData();
            this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        string str_sql = "";

        ParameterFields paraFields = new ParameterFields();
        if (rdo1.Checked)
        {

            ParameterField[] paraField = new ParameterField[13];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
            ParameterRangeValue rangeValue = new ParameterRangeValue();
            paraField[0] = new ParameterField();
            paraField[0].ParameterFieldName = "REXPeriod"; //记账月
            discreteValue[0] = new ParameterDiscreteValue();
            discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "InvAdj_KeepAccountsMth");
            paraField[0].CurrentValues.Add(discreteValue[0]);

            paraField[1] = new ParameterField();
            paraField[1].Name = "REXShopType";//商铺类型
            discreteValue[1] = new ParameterDiscreteValue();
            discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");
            paraField[1].CurrentValues.Add(discreteValue[1]);

            paraField[2] = new ParameterField();
            paraField[2].Name = "REXRentArea";//签约面积
            discreteValue[2] = new ParameterDiscreteValue();
            discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblRentArea");
            paraField[2].CurrentValues.Add(discreteValue[2]);

            paraField[3] = new ParameterField();
            paraField[3].Name = "REXAvgRentalAmt";//平均日租金
            discreteValue[3] = new ParameterDiscreteValue();
            discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_AvgRentalAmt");
            paraField[3].CurrentValues.Add(discreteValue[3]);

            paraField[4] = new ParameterField();
            paraField[4].Name = "REXThisMonth";//本月
            discreteValue[4] = new ParameterDiscreteValue();
            discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ThisMonth");
            paraField[4].CurrentValues.Add(discreteValue[4]);

            paraField[5] = new ParameterField();
            paraField[5].Name = "REXLaseYearPeriod";//同比
            discreteValue[5] = new ParameterDiscreteValue();
            discreteValue[5].Value =  (String)GetGlobalResourceObject("BaseInfo", "Rpt_LaseYearPeriod");
            paraField[5].CurrentValues.Add(discreteValue[5]);

            paraField[6] = new ParameterField();
            paraField[6].Name = "REXRingThan";//环比
            discreteValue[6] = new ParameterDiscreteValue();
            discreteValue[6].Value =  (String)GetGlobalResourceObject("BaseInfo", "Rpt_RingThan");
            paraField[6].CurrentValues.Add(discreteValue[6]);

            paraField[7] = new ParameterField();
            paraField[7].Name = "REXoldMRate";
            discreteValue[7] = new ParameterDiscreteValue();
            discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMRate");//环比差异
            paraField[7].CurrentValues.Add(discreteValue[7]);

            paraField[8] = new ParameterField();
            paraField[8].Name = "REXyearRate";
            discreteValue[8] = new ParameterDiscreteValue();
            discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearRate");//同比差异
            paraField[8].CurrentValues.Add(discreteValue[8]);

            paraField[9] = new ParameterField();
            paraField[9].Name = "REXTitle";//汇总
            discreteValue[9] = new ParameterDiscreteValue();
            discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreRentalHeader");
            paraField[9].CurrentValues.Add(discreteValue[9]);

            paraField[10] = new ParameterField();
            paraField[10].Name = "REXMallTitle";
            discreteValue[10] = new ParameterDiscreteValue();
            discreteValue[10].Value = Session["MallTitle"].ToString();
            paraField[10].CurrentValues.Add(discreteValue[10]);

            paraField[11] = new ParameterField();
            paraField[11].Name = "REXBusinessItem";
            discreteValue[11] = new ParameterDiscreteValue();
            discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");//商业项目
            paraField[11].CurrentValues.Add(discreteValue[11]);

            paraField[12] = new ParameterField();
            paraField[12].Name = "REXMonthAmt";//基础月租金
            discreteValue[12] = new ParameterDiscreteValue();
            discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MonthAmt");
            paraField[12].CurrentValues.Add(discreteValue[12]);




            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }


            string wherestr = " ";


            if (ddlStoreName.Text != "")
            {
                wherestr = " AND conshop.storeid='" + ddlStoreName.SelectedValue + "' ";
            }
            if (txtPeriod.Text != "")
            {
                wherestr =wherestr+" AND invoicedetail.period='"+txtPeriod.Text+"' ";
            }
          
            str_sql =  "select aa.storename,aa.rentarea,aa.period,aa.invpayamt,aa.lyinvpayamt,aa.ppinvpayamt, "+
	                        "(case when aa.lyinvpayamt<>0 then (aa.invpayamt-aa.lyinvpayamt)/aa.lyinvpayamt else 0 end) *100  as lyinvrate, "+
	                        "(case when aa.ppinvpayamt<>0 then (aa.invpayamt-aa.ppinvpayamt)/aa.ppinvpayamt else 0 end) *100 as ppinvrate, "+
                            "aa.invpayamt / (cast(datediff(day,aa.period,dateadd(mm,1,aa.period) )as int) * aa.rentarea ) as  avgrentamt "+
                        "from "+
	                        "(select store.storename,sum(conshop.rentarea) as rentarea, invoicedetail.period,sum(invoicedetail.invpayamtl) as invpayamt, "+
		                        "(select isnull(sum(a.invpayamtl),0) from invoicedetail a inner join invoiceheader b on b.invid=a.invid inner join conshop c on (c.contractid=b.contractid) where c.storeid=conshop.storeid and a.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) and a.period=dateadd(mm,-1,invoicedetail.period)) as lyinvpayamt, "+  //上月
		                        "(select isnull(sum(a.invpayamtl),0) from invoicedetail a inner join invoiceheader b on b.invid=a.invid inner join conshop c on (c.contractid=b.contractid) where c.storeid=conshop.storeid and a.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) and a.period=dateadd(year,-1,invoicedetail.period)) as ppinvpayamt "+ //同比
	                        "from invoicedetail	inner join invoiceheader on (invoicedetail.invid=invoiceheader.invid) "+
	                        "inner join conshop on (conshop.contractid=invoiceheader.contractid) "+
	                        "inner join store on (conshop.storeid=store.storeid) "+
	                        "where invoicedetail.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) "+wherestr+
	                        "group by store.storename, invoicedetail.period,conshop.storeid ) as aa ";

        }
        if (rdo2.Checked)
        {
            ParameterField[] paraField = new ParameterField[14];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
            ParameterRangeValue rangeValue = new ParameterRangeValue();
            paraField[0] = new ParameterField();
            paraField[0].ParameterFieldName = "REXPeriod"; //记账月
            discreteValue[0] = new ParameterDiscreteValue();
            discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "InvAdj_KeepAccountsMth");
            paraField[0].CurrentValues.Add(discreteValue[0]);

            paraField[1] = new ParameterField();
            paraField[1].Name = "REXShopType";//商铺类型
            discreteValue[1] = new ParameterDiscreteValue();
            discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");
            paraField[1].CurrentValues.Add(discreteValue[1]);

            paraField[2] = new ParameterField();
            paraField[2].Name = "REXRentArea";//签约面积
            discreteValue[2] = new ParameterDiscreteValue();
            discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblRentArea");
            paraField[2].CurrentValues.Add(discreteValue[2]);

            paraField[3] = new ParameterField();
            paraField[3].Name = "REXAvgRentalAmt";//平均日租金
            discreteValue[3] = new ParameterDiscreteValue();
            discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_AvgRentalAmt");
            paraField[3].CurrentValues.Add(discreteValue[3]);

            paraField[4] = new ParameterField();
            paraField[4].Name = "REXThisMonth";//本月
            discreteValue[4] = new ParameterDiscreteValue();
            discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ThisMonth");
            paraField[4].CurrentValues.Add(discreteValue[4]);

            paraField[5] = new ParameterField();
            paraField[5].Name = "REXLaseYearPeriod";//同比
            discreteValue[5] = new ParameterDiscreteValue();
            discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_LaseYearPeriod");
            paraField[5].CurrentValues.Add(discreteValue[5]);

            paraField[6] = new ParameterField();
            paraField[6].Name = "REXRingThan";//环比
            discreteValue[6] = new ParameterDiscreteValue();
            discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_RingThan");
            paraField[6].CurrentValues.Add(discreteValue[6]);

            paraField[7] = new ParameterField();
            paraField[7].Name = "REXoldMRate";
            discreteValue[7] = new ParameterDiscreteValue();
            discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMRate");//环比差异
            paraField[7].CurrentValues.Add(discreteValue[7]);

            paraField[8] = new ParameterField();
            paraField[8].Name = "REXyearRate";
            discreteValue[8] = new ParameterDiscreteValue();
            discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearRate");//同比差异
            paraField[8].CurrentValues.Add(discreteValue[8]);

            paraField[9] = new ParameterField();
            paraField[9].Name = "REXTitle";//明细
            discreteValue[9] = new ParameterDiscreteValue();
            discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreRentalDetail");
            paraField[9].CurrentValues.Add(discreteValue[9]);

            paraField[10] = new ParameterField();
            paraField[10].Name = "REXMallTitle";
            discreteValue[10] = new ParameterDiscreteValue();
            discreteValue[10].Value = Session["MallTitle"].ToString();
            paraField[10].CurrentValues.Add(discreteValue[10]);

            paraField[11] = new ParameterField();
            paraField[11].Name = "REXBusinessItem";
            discreteValue[11] = new ParameterDiscreteValue();
            discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");//商业项目
            paraField[11].CurrentValues.Add(discreteValue[11]);

            paraField[12] = new ParameterField();
            paraField[12].Name = "REXMonthAmt";//基础月租金
            discreteValue[12] = new ParameterDiscreteValue();
            discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MonthAmt");
            paraField[12].CurrentValues.Add(discreteValue[12]);

            paraField[13] = new ParameterField();
            paraField[13].Name = "REXFloorName";//楼层
            discreteValue[13] = new ParameterDiscreteValue();
            discreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblFloorName");
            paraField[13].CurrentValues.Add(discreteValue[13]);


            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }



            string wherestr = " ";

            if (ddlStoreName.Text != "")
            {
                wherestr = "AND store.storeid='" + ddlStoreName.SelectedValue + "'";
            }
            if (ddlBuildingName.Text != "")
            {
                wherestr = wherestr+"AND conshop.buildingid='" + ddlBuildingName.SelectedValue + "'";
            }
            if (ddlFloorName.Text != "")
            {
                wherestr = wherestr + "AND conshop.floorid= '" + ddlFloorName.SelectedValue + "'";
            }
            if (ddlShopType.Text != "")
            {
                wherestr = wherestr + "AND shoptype.shoptypeid= '" + ddlShopType.SelectedValue + "'";
            }
            if (txtPeriod.Text != "")
            {
                wherestr = wherestr + " AND invoicedetail.period='" + txtPeriod.Text + "' ";
            }

            str_sql = "select aa.storename,aa.shoptypename,aa.floorname,aa.rentarea,aa.period,aa.invpayamt,aa.lyinvpayamt,aa.ppinvpayamt," +
                            "(case when aa.lyinvpayamt<>0 then (aa.invpayamt-aa.lyinvpayamt)/aa.lyinvpayamt else 0 end) *100  as lyinvrate, " +
                            "(case when aa.ppinvpayamt<>0 then (aa.invpayamt-aa.ppinvpayamt)/aa.ppinvpayamt else 0 end) *100 as ppinvrate, " +
                            "aa.invpayamt / (cast(datediff(day,aa.period,dateadd(mm,1,aa.period) )as int) * aa.rentarea ) as  avgrentamt " +
                        "from " +
                            "(select store.storename,shoptype.shoptypename,floors.floorname,sum(conshop.rentarea) as rentarea, invoicedetail.period,sum(invoicedetail.invpayamtl) as invpayamt, " +
                                "(select isnull(sum(a.invpayamtl),0) from invoicedetail a inner join invoiceheader b on b.invid=a.invid inner join conshop c on (c.contractid=b.contractid) where c.storeid=conshop.storeid and a.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) and a.period=dateadd(mm,-1,invoicedetail.period)) as lyinvpayamt, " +  //--上月
                                "(select isnull(sum(a.invpayamtl),0) from invoicedetail a inner join invoiceheader b on b.invid=a.invid inner join conshop c on (c.contractid=b.contractid) where c.storeid=conshop.storeid and a.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) and a.period=dateadd(year,-1,invoicedetail.period)) as ppinvpayamt " + //--同比
                            "from invoicedetail	inner join invoiceheader on (invoicedetail.invid=invoiceheader.invid) " +
                            "inner join conshop on (conshop.contractid=invoiceheader.contractid) " +
                            "inner join shoptype on (conshop.shoptypeid=shoptype.shoptypeid) " +
                            "inner join floors on (conshop.floorid=floors.floorid) " +
                            "inner join store on (conshop.storeid=store.storeid) " +
                            "where invoicedetail.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) " +wherestr+
                            "group by store.storename, invoicedetail.period,conshop.storeid,shoptype.shoptypename,floors.floorname ) as aa ";


        }


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\" + sRptName;

    }



    protected void rdo1_CheckedChanged(object sender, EventArgs e)
    {
        ddlstatus();
    }
    protected void rdo2_CheckedChanged(object sender, EventArgs e)
    {
        ddlBuildingName.Visible = true;
        ddlFloorName.Visible = true;
        ddlShopType.Visible = true;
        labBuildingName.Visible = true;
        labFloorName.Visible = true;
        labShopType.Visible = true;
    }
    protected void ddlstatus()
    {
        ddlBuildingName.Visible = false;
        ddlFloorName.Visible = false;
        ddlShopType.Visible = false;
        labBuildingName.Visible = false;
        labFloorName.Visible = false;
        labShopType.Visible = false;
        ddlBuildingName.Text = "";
        ddlFloorName.Text = "";
        ddlShopType.Text = "";
    }



    protected void ddlStoreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBuildingName.Items.Clear();
        ddlFloorName.Items.Clear();

        /*绑定大楼*/
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + "and storeid='" + ddlStoreName.SelectedValue + "'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
        {
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        }
    }
    protected void ddlBuildingName_SelectedIndexChanged1(object sender, EventArgs e)
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
}
