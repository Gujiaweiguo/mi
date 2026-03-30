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
        this.Response.Redirect("~/ReportM/Shop/ShopUserRpt.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        BindData();
        Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[12];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[12];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTPUsrId";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo","User_lblWorkNo");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTPUsrNm";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "User_lblUserName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXIDNo";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "User_lblIdentity");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXPhone";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "User_lblMobile1");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXSex";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorGender");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXDob";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorBirthday");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXDateStart";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "TpUse_lblBeginWorkDate");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXJobTitleNm";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "TpUsr_REXJobTitleNm");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXTPUsrStatus";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblLocationStatus");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXShopName";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "title";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_TpUsrTitle");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXMallTitle";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = Session["MallTitle"].ToString();
        paraField[11].CurrentValues.Add(discreteValue[11]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = "Select TPUsrId,TPUsrNm,IDNo,szPin,Phone,Case Sex when 'F' Then '" + (String)GetGlobalResourceObject("BaseInfo", "TpUse_lblSexWoman") +
                        "' when 'M' Then '" + (String)GetGlobalResourceObject("BaseInfo", "TpUse_lblSexMan") + "' End as Sex,Dob,DateStart,JobTitleNm,GpId,Case TPUsrStatus when 'N' Then '" +
                        (String)GetGlobalResourceObject("Parameter", "BizGrp_NO") + "' when 'E' Then '" + (String)GetGlobalResourceObject("Parameter", "BizGrp_YES") + "' End as TPUsrStatus,UnitId,ShopName," +
                        "DeleteTime,dept.deptname From TpUsr a Left Join Conshop b On a.UnitId=b.ShopID Left Join Contract c "+
                        "On b.ContractID= c.ContractID Left Join Customer d On c.CustID=d.CustID left join dept on deptid=b.storeid Where 1=1";

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
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\TpUsr.rpt";
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
