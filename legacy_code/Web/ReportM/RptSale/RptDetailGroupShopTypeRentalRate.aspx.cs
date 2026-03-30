using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Dept;
using Shop.ShopType;

/// <summary>
/// 集团项目整体出租率报表
/// </summary>
public partial class ReportM_RptSale_RptDetailGroupShopTypeRentalRate : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            this.BindShopType();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_DetailGroupShopTypeRentalRate");
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
        //this.ddlShopType.Items.Add(new ListItem("", ""));
        foreach (ShopType st in rs3)
            ddlShopType.Items.Add(new ListItem(st.ShopTypeName, st.ShopTypeID.ToString()));
    }
    private void InitDDL()
    {
        //区域
        BaseBO baseBo=new BaseBO();
        DataSet ds=new DataSet();
        baseBo.WhereClause = "depttype=4";
        ds = baseBo.QueryDataSet(new Dept());
        ddlArea.Items.Add(new ListItem("", ""));
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            ddlArea.Items.Add(new ListItem(ds.Tables[0].Rows[i]["deptname"].ToString(), ds.Tables[0].Rows[i]["deptid"].ToString()));
        }
        //城市
        DataSet ds1 = new DataSet();
        baseBo.WhereClause = "depttype=5";
        ds1 = baseBo.QueryDataSet(new Dept());
        ddlCity.Items.Add(new ListItem("", ""));
        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
        {
            ddlCity.Items.Add(new ListItem(ds1.Tables[0].Rows[i][1].ToString(), ds1.Tables[0].Rows[i][0].ToString()));
        }
        ////商铺类型
        //DataSet ds2 = new DataSet();
        //ds2 = baseBo.QueryDataSet(new ShopType());
        //this.ddlShopType.Items.Add(new ListItem("", ""));
        //for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
        //{
        //    ddlShopType.Items.Add(new ListItem(ds2.Tables[0].Rows[i]["ShopTypeName"].ToString(), ds2.Tables[0].Rows[i]["ShopTypeID"].ToString()));
        //}
        //月份
        int intMonth = 12;
        ddlMonth.Items.Clear();
        ddlMonth.Items.Add(new ListItem("",""));
        for (int iMonth = 1; iMonth <= intMonth; iMonth++)
        {
            ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        //年份
        int year = DateTime.Now.Year;
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        this.ddlYear.SelectedValue = DateTime.Now.Year.ToString();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptDetailGroupShopTypeRentalRate.aspx");
    }
    //水晶报表数据绑定
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
        paraField[1].Name = "REXArea";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "AreaVindicate_labAreaTitle");//区域
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCity";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_City");//城市
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXMallTitle";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = Session["MallTitle"].ToString();//Mall名称
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXUnitTypeSalesSum";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_DetailGroupShopTypeRentalRate");//标题
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXTotle";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblTotal");//总计
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXMonth";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblSalesMonth");//月份
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXRate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_UseRate");//出租率
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXShopTypeName";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopTypeName");//出租率
        paraField[8].CurrentValues.Add(discreteValue[8]);



        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string wheresql = "";
        

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        string str_sql = @"select dept.deptname,aa.leasearea,bb.totalarea,convert(char(7),aa.period,120) as period,
                        (select shoptypename from shoptype where shoptypeid=aa.shoptypeid) shoptypename,
                        a.deptid,a.deptname cityname, --城市
                                b.deptid,b.deptname ddname --区域
                        from dept 
                        left join (select deptid,deptname,pdeptid from dept where depttype=5) a on (dept.pdeptid=a.deptid)
                        left join (select deptid,deptname from dept where depttype=4) b on (a.pdeptid=b.deptid)

                        inner join (
	                        select storeid,period,shoptypeid,sum(floorarea) leasearea from unitrent
	                        where shopid<>0
	                        group by storeid,period,shoptypeid) aa
                        on (aa.storeid=dept.deptid)
                        inner join (
	                        select storeid,period,shoptypeid,sum(floorarea) totalarea from unitrent 
	                        group by storeid,period,shoptypeid) bb
                        on (bb.storeid=dept.deptid)
                        --查询条件
                        where  aa.shoptypeid=bb.shoptypeid 
                         ";

        if (ddlArea.Text != "")
        {
            str_sql = str_sql + " AND b.deptid ='" + ddlArea.SelectedValue + "'";
        }
        if (ddlCity.Text != "")
        {
            str_sql = str_sql + " AND a.deptid ='" + ddlCity.SelectedValue + "'";
        }
        if (this.ddlShopType.Text != "")
        {
            str_sql = str_sql + " AND aa.shoptypeid ='" + this.ddlShopType.SelectedValue + "'";
        }

        if (this.ddlYear.Text != "")
        {
            str_sql = str_sql + " and year(aa.period)='" + this.ddlYear.SelectedValue + "' and year(bb.period)='" + this.ddlYear.SelectedValue + "'";
        }
        if (this.ddlMonth.Text != "")
        {
            str_sql = str_sql + " and month(aa.period)='" + this.ddlMonth.SelectedValue + "' and month(bb.period)='" + this.ddlMonth.SelectedValue + "'";
        }

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
        //str_sql = str_sql + strAuth ;
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        if (RB2.Checked)
        { 
            if(this.ckbArea.Checked==false&&this.ckbCity.Checked==false&&this.ckbShopType.Checked==false)
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate.rpt";
            if (this.ckbArea.Checked == true && this.ckbCity.Checked == false && this.ckbShopType.Checked == false)
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate1.rpt";
            if (this.ckbArea.Checked == false && this.ckbCity.Checked == true && this.ckbShopType.Checked == false)
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate2.rpt";
            if (this.ckbArea.Checked == false && this.ckbCity.Checked == false && this.ckbShopType.Checked == true)
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate3.rpt";
            if (this.ckbArea.Checked == true && this.ckbCity.Checked == false && this.ckbShopType.Checked == true)
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate4.rpt";
            if (this.ckbArea.Checked == false && this.ckbCity.Checked == true && this.ckbShopType.Checked == true)
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate5.rpt";
            if (this.ckbArea.Checked == true && this.ckbCity.Checked == true && this.ckbShopType.Checked == false)
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate6.rpt";
            if (this.ckbArea.Checked == true && this.ckbCity.Checked == true && this.ckbShopType.Checked == true)
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate7.rpt";
        }
        else
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptDetailGroupShopTypeRentalRate8.rpt";
    }
}

