using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class ReportM_RptSale_RptShopSKUInfo : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            InitDdl();
        }
    }

    private void InitDdl()
    {
        BaseBO baseBo = new BaseBO();
        ddlStoreName.Items.Clear();
        ddlSkuClass.Items.Clear();
        //绑定商业项目
        baseBo.WhereClause = "";
        Resultset rs = baseBo.Query(new Store());
        ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));


        //榜定商品分类
        baseBo.WhereClause = "classstatus=1 and classlevel=2";
        Resultset rs1 = baseBo.Query(new SkuClass());
        ddlSkuClass.Items.Add(new ListItem("", ""));
        foreach (SkuClass bd in rs1)
            ddlSkuClass.Items.Add(new ListItem(bd.ClassName, bd.ClassID.ToString()));
    }

    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBuildingName.Items.Clear();
        ddlFloorName.Items.Clear();
        ddlCustCode.Items.Clear();

        BaseBO baseBo = new BaseBO();
        /*绑定大楼*/
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + "and storeid='" + ddlStoreName.SelectedValue + "'";
        Resultset rs = baseBo.Query(new Building());
        this.ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
        {
            this.ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        }
    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFloorName.Items.Clear();
        ddlCustCode.Items.Clear();
        //绑定楼层
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "FloorStatus = " + Floors.FLOOR_STATUS_VALID + " AND BuildingID = '" + this.ddlBuildingName.SelectedValue.ToString() + "'";
        Resultset rs1 = baseBO.Query(new Floors());
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
        {
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
        }
    }
    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCustCode.Items.Clear();
        //绑定商铺
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        ds=baseBO.QueryDataSet("select * from conshop where shopstatus=1 and floorid=" + Convert.ToInt32(ddlFloorName.SelectedValue));
        this.ddlCustCode.Items.Add(new ListItem("", ""));
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
            {
                this.ddlCustCode.Items.Add(new ListItem(ds.Tables[0].Rows[i]["ShopName"].ToString(), ds.Tables[0].Rows[i]["ShopId"].ToString()));
            }
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        
    }

    private void BindData()
    {

        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[14];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].ParameterFieldName = "REXMainTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = Session["MallTitle"].ToString();
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].ParameterFieldName = "REXTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ShopSKUInfo");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        Field[2] = new ParameterField();
        Field[2].ParameterFieldName = "REXShopCode";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblShopCode");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        Field[3] = new ParameterField();
        Field[3].ParameterFieldName = "REXShopName";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] = new ParameterField();
        Field[4].ParameterFieldName = "REXSkuId";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SkuId");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].ParameterFieldName = "REXSkuDesc";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SkuDesc");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].ParameterFieldName = "REXSkuDeptID";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SkuDeptID");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].ParameterFieldName = "REXSkuClassName";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SkuClassName");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        Field[8] = new ParameterField();
        Field[8].ParameterFieldName = "REXBrandID";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_BrandID");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        Field[9] = new ParameterField();
        Field[9].ParameterFieldName = "REXBrandName";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_BrandName");
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        Field[10] = new ParameterField();
        Field[10].ParameterFieldName = "REXDiscountPcentRate";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_DiscountPcentRate");
        Field[10].CurrentValues.Add(DiscreteValue[10]);

        Field[11] = new ParameterField();
        Field[11].ParameterFieldName = "REXBonusGpPer";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_BonusGpPer");
        Field[11].CurrentValues.Add(DiscreteValue[11]);

        Field[12] = new ParameterField();
        Field[12].ParameterFieldName = "REXSequence";
        DiscreteValue[12] = new ParameterDiscreteValue();
        DiscreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "WrkFlw_Sequence");
        Field[12].CurrentValues.Add(DiscreteValue[12]);


        Field[13] = new ParameterField();
        Field[13].ParameterFieldName = "RexSKUPrice";
        DiscreteValue[13] = new ParameterDiscreteValue();
        DiscreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_Price");
        Field[13].CurrentValues.Add(DiscreteValue[13]);

        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string str_sql = @"SELECT  ConShop.ShopID,
                            ConShop.ShopCode,
		                    ConShop.ShopName,
                            ConShop.FloorID,
		                    SkuMaster.SkuID,
		                    SkuMaster.SkuDesc,
		                    isnull(SkuMaster.DeptID,0) as DeptID, --商品分类ID
		                    isnull(SkuClass.ClassName,'') as ClassName,
		                    SkuMaster.Brand, --品牌ID
		                    ConShopBrand.BrandName,
		                    SkuMaster.DiscountPcentRate, --抽成率
		                    SkuMaster.BonusGpPer, --积分率
                            SkuMaster.UnitPrice as SkuPrice,
                            dept.deptname
                            FROM    SkuMaster
		                    INNER JOIN  ConShop ON (ConShop.ShopId=SkuMaster.TenantId)
                            inner join dept on conshop.storeid=dept.deptid
                            inner join contract on (conshop.contractid=contract.contractid)
		                    left JOIN SkuClass ON (convert(int,SkuMaster.DeptID)=SkuClass.ClassID)
		                    left JOIN ConShopBrand ON(SkuMaster.Brand=ConShopBrand.BrandID)
                            WHERE 1=1 and contract.contractstatus=2";

        //条件查询
        if (ddlStoreName.Text != "")
        {
            str_sql += " AND ConShop.StoreID = '" + ddlStoreName.SelectedValue.Trim() + "' ";
        }

        if (ddlBuildingName.Text != "")
        {
            str_sql += " AND ConShop.BuildingID = '" + ddlBuildingName.SelectedValue.Trim() + "' ";
        }

        if (ddlFloorName.Text != "")
        {
            str_sql += " AND ConShop.FloorID = '" + ddlFloorName.SelectedValue.Trim() + "' ";
        }

        if (ddlCustCode.Text != "")
        {
            str_sql += " AND ConShop.ShopID = '" + ddlCustCode.SelectedValue.Trim() + "' ";
        }

        if (ddlSkuClass.Text != "")
        {
            str_sql += " AND SkuMaster.DeptID = '" + ddlSkuClass.SelectedValue.Trim() + "'";
        }

        //if (txtShopName.Text != "")
        //{
        //    str_sql += " AND transshopmth.shopName like '%" + txtShopName.Text.Trim() + "%'";
        //}

        //if (txtBizSDate.Text != "")
        //{
        //    str_sql += " AND transshopmth.month = '" + txtBizSDate.Text + "' ";
        //}
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        str_sql += " order by ConShop.FloorID,ConShop.ShopID ";

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptShopSKUInfo.rpt";
    }

}
