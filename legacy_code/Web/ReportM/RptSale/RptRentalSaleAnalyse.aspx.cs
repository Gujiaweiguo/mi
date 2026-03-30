using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using Shop.ShopType;
using BaseInfo.Dept;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class ReportM_RptSale_RptRentalSaleAnalyse : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreShopTypeRentalSaleAnalyse");
            this.BindArea();//绑定区域
            this.BindCity();//绑定城市
            this.BindShopType();//绑定商铺类型
            this.BindYear();//绑定年
            this.ddlYear.Text = DateTime.Now.Year.ToString();
            this.BindMonth();//绑定月
        }
    }
    /// <summary>
    /// 绑定年
    /// </summary>
    private void BindYear()
    { 
        int iYear = DateTime.Now.Year;
        for (int i = iYear - 5; i <= iYear + 5; i++)
        {
            this.ddlYear.Items.Add(new ListItem(i.ToString(),i.ToString()));
        }
    }
    /// <summary>
    /// 绑定月
    /// </summary>
    private void BindMonth()
    {
        this.ddlMonth.Items.Add(new ListItem("", ""));
        for (int i = 1; i <= 12; i++)
        {
            this.ddlMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }
    /// <summary>
    /// 绑定区域
    /// </summary>
    private void BindArea()
    {
        this.ddlArea.Items.Add(new ListItem("", ""));
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "DeptType=4";
        objBaseBo.OrderBy = "deptid";
        Resultset rs = objBaseBo.Query(new Dept());
        foreach (Dept objDept in rs)
        {
            ddlArea.Items.Add(new ListItem(objDept.DeptName, objDept.DeptID.ToString()));
        }
    }
    /// <summary>
    /// 绑定城市
    /// </summary>
    private void BindCity()
    {
        this.ddlCity.Items.Add(new ListItem("", ""));
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "DeptType=5";
        objBaseBo.OrderBy = "deptid";
        Resultset rs = objBaseBo.Query(new Dept());
        foreach (Dept objDept in rs)
        {
            ddlCity.Items.Add(new ListItem(objDept.DeptName, objDept.DeptID.ToString()));
        }
    }
    /// <summary>
    /// 绑定商铺类型
    /// </summary>
    private void BindShopType()
    {
        BaseBO objBase = new BaseBO();
        objBase.WhereClause = "ShopTypeStatus = " + ShopType.SHOP_TYPE_STATUS_VALID;
        Resultset rs3 = objBase.Query(new ShopType());
        this.ddlShopType.Items.Add(new ListItem("", ""));
        foreach (ShopType st in rs3)
            ddlShopType.Items.Add(new ListItem(st.ShopTypeName, st.ShopTypeID.ToString()));
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptRentalSaleAnalyse.aspx");
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[9];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[9];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXStoreName";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");//项目名称
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXSalesMonth";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblSalesMonth");//月份
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXShopTypeName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");//商铺类型
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXRentSale";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_RentSaleCompare");//租售比
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXMallTitle";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = Session["MallTitle"].ToString();//Mall名称
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXUnitTypeSalesSum";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreShopTypeRentalSaleAnalyse");//标题
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXTotle";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblTotal");//总计
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXRental";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FixedRental");//租金额
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXSale";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_CostAmt");//销售额
        paraField[8].CurrentValues.Add(discreteValue[8]);

       
        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = @"select transshopmth.storeid,transshopmth.StoreName,transshopmth.shoptypeid,convert(char(7),transshopmth.month,120) as month,sum(transshopmth.paidamt) SalesAmt,
dept.deptname,a.deptname cityname,b.deptname ddname,shoptype.shoptypename,aa.InvPayAmt
from transshopmth
inner join dept on (transshopmth.storeid=dept.deptid)
inner join shoptype on (transshopmth.shoptypeid=shoptype.shoptypeid)
--城市
left join (select deptid,deptname,pdeptid from dept where depttype=5) a on (dept.pdeptid=a.deptid)
--区域
left join (select deptid,deptname from dept where depttype=4) b on (a.pdeptid=b.deptid)
--费用
inner join (select conshop.storeid,conshop.shoptypeid,invoicedetail.period,sum(invoicedetail.invpayamtl) InvPayAmt 
	from invoicedetail inner join invoiceheader on (invoiceheader.invid=invoicedetail.invid)
	inner join conshop on (invoiceheader.contractid=conshop.contractid)
	inner join chargetype on (chargetype.chargetypeid=invoicedetail.chargetypeid)
	where chargetype.chargeclass in (1,3)
	group by conshop.storeid,conshop.shoptypeid,invoicedetail.period) aa
	on (aa.storeid=transshopmth.storeid and aa.period=transshopmth.month and
	aa.shoptypeid=transshopmth.shoptypeid)
--查询条件
where 1=1 ";

        if (this.ddlArea.Text != "")
        {
            str_sql = str_sql + " and b.Deptid='" + ddlArea.SelectedValue + "'";
        }
        if (this.ddlCity.Text != "")
        {
            str_sql = str_sql + " and a.Deptid='" + this.ddlCity.SelectedValue + "'";
        }
        if (this.ddlShopType.Text != "")
        {
            str_sql = str_sql + " and transshopmth.shoptypeid='" + this.ddlShopType.SelectedValue + "'";
        }
        if (this.ddlYear.Text != "")
        {
            str_sql = str_sql + " and year(transshopmth.month)='" + this.ddlYear.SelectedValue + "'";
        }
        if (this.ddlMonth.Text != "")
        {
            str_sql = str_sql + " and month(transshopmth.month)='" + this.ddlMonth.SelectedValue + "'";
        }
        

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {

            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
            for (int i = 0; i < 5; i++)
            {
                //将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
                strAuth = strAuth.Replace("ConShop", "transshopmth");
            }
        }

        str_sql = str_sql + strAuth + " group by transshopmth.storeid,transshopmth.StoreName,transshopmth.shoptypeid,transshopmth.month,dept.deptname,a.deptname,b.deptname,shoptype.shoptypename,aa.InvPayAmt";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptRentalSaleAnalyse.rpt";
    }
}
