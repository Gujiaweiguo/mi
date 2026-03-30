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

using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;
using Base.DB;
using Base.Biz;
using BaseInfo.Store;
using RentableArea;
using Lease.ConShop;

public partial class ReportM_Shop_ShopUserRpt : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.BindDDL();//绑定下拉列表框
        }
    }
    private void BindDDL()
    {
        BaseBO baseBo = new BaseBO();
        //绑定商业项目
        Resultset rs4 = baseBo.Query(new Store());
        ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs4)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptBase/RptShopInfo.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        BindData();
        Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXMallTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = Session["MallTitle"].ToString();
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ShopInfo");//商铺信息报表
        paraField[1].CurrentValues.Add(discreteValue[1]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = @"select store.storename,floors.floorname,traderelation.tradename,b.shopcode,b.shopname,b.shopid,
contract.contractcode,customer.custname,conshopbrand.brandname,customer.custcode 
from conshop b
inner join store on store.storeid=b.storeid
inner join floors on floors.floorid=b.floorid
inner join contract on contract.contractid=b.contractid
inner join customer on contract.custid=customer.custid
inner join traderelation on traderelation.tradeid=contract.tradeid
left join conshopbrand on b.brandid=conshopbrand.brandid where 1=1";

        

        if (ddlStoreName.Text != "")
        {
            str_sql = str_sql + " AND b.storeID  = '" + ddlStoreName.SelectedValue + "' ";
        }
        if (ddlBuildingName.Text != "")
        {
            str_sql = str_sql + " AND b.BuildingID = '" + ddlBuildingName.SelectedValue + "' ";
        }
        if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND b.FloorId = '" + ddlFloorName.SelectedValue + "' ";
        }
        if (ddlShopCode.Text != "")
        {
            str_sql = str_sql + " AND b.ShopID = '" + ddlShopCode.SelectedValue + "' ";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND b.ShopCode like '%" + txtCustCode.Text + "%'";
        }
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
                strAND += " AND EXISTS (" + arr[i].ToString().Replace("ConShop", "b") + sessionUser.UserID + ")";
            }
            str_sql += strAND;
        }
        //if (txtCustName.Text != "")
        //{
        //    str_sql = str_sql + " AND d.CustName like '%" + txtCustName.Text + "%'";
        //}

        //if (txtBeginWorkDate.Text != "")
        //{
        //    str_sql = str_sql + " AND DateStart >= '" + txtBeginWorkDate.Text + " 00:00:00'";
        //}

        //if (txtBeginWorkDateEnd.Text != "")
        //{
        //    str_sql = str_sql + " AND DateStart M= '" + txtBeginWorkDateEnd.Text + " 00:00:00'";
        //}

        //if (txtBeginWorkDate.Text != "")
        //{
        //    str_sql = str_sql + " AND Dob >= '" + txtBirth.Text + "'";
        //}

        //if (txtBeginWorkDateEnd.Text != "")
        //{
        //    str_sql = str_sql + " AND Dob <= '" + txtBirthEnd.Text + "'";
        //}

        //if (rdoMan.Checked)
        //{
        //    str_sql = str_sql + " AND Sex ='F'";
        //}
        //else if (rdoWoman.Checked)
        //{
        //    str_sql = str_sql + " AND Sex ='M'";
        //}

        //if (rdoWorkNo.Checked)
        //{
        //    str_sql = str_sql + " Order By TPUsrId";
        //}
        //else
        //{
        //    str_sql = str_sql + " Order By TPUsrNm";
        //}

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\RptBase\\RptShopInfo.rpt";
    }
    protected void ddlStoreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        //绑定楼
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + "and storeid='" + ddlStoreName.SelectedValue + "'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Clear();
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));

        ddlFloorName.Items.Clear();
        ddlShopCode.Items.Clear();
    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        ////绑定楼层'
        baseBo.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID + " and Buildingid = '" + this.ddlBuildingName.SelectedValue.ToString() + "'";
        Resultset rs1 = baseBo.Query(new Floors());
        ddlFloorName.Items.Clear();
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
        {
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
        }
        ddlShopCode.Items.Clear();
    }
    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定商铺号
        ddlShopCode.Items.Clear();
        BaseBO baseBo = new BaseBO();
        string sql = "SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + "AND FLOORID='" + ddlFloorName.SelectedValue + "'AND FloorID='" + ddlFloorName.SelectedValue + "' Order By ShopCode";
        DataSet myDS = baseBo.QueryDataSet(sql);
        int count = myDS.Tables[0].Rows.Count;
        ddlShopCode.Items.Clear();
        ddlShopCode.Items.Add("");
        for (int i = 0; i < count; i++)
        {
            ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        }
    }
}
