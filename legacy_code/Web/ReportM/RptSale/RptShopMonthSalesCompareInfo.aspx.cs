using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;


public partial class ReportM_RptSale_RptShopMonthSalesCompareInfo : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ShopMonthSalesCompareInfo");
            InitDLL();
        }
    }

    private void InitDLL()
    {

        int intMonth = 12;
        ddlMonth.Items.Clear();
        for (int iMonth = 1; iMonth <= intMonth; iMonth++)
        {
            ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        this.ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

        int year = Convert.ToInt16(DateTime.Now.Year);
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        this.ddlYear.SelectedValue = DateTime.Now.Year.ToString();

        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "DeptType=" + BaseInfo.Dept.Dept.DEPT_TYPE_MALL;
        baseBo.OrderBy = "OrderID";
        Resultset rs = baseBo.Query(new BaseInfo.Dept.Dept());
        this.ddlStoreName.Items.Clear();
        this.ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (BaseInfo.Dept.Dept store in rs)
        {
            this.ddlStoreName.Items.Add(new ListItem(store.DeptName, store.DeptID.ToString()));
        }
               
    }



    //绑定数据
    private void BindData()
    {

        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[25];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[25];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].ParameterFieldName = "REXMainTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = Session["MallTitle"].ToString();
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].ParameterFieldName = "REXTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_ShopMonthSalesCompareInfo");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        Field[2] = new ParameterField();
        Field[2].ParameterFieldName = "REXShopName";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        Field[3] = new ParameterField();
        Field[3].ParameterFieldName = "REXTradeType";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblTradeType");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] = new ParameterField();
        Field[4].ParameterFieldName = "REXBrandName";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_Brand");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].ParameterFieldName = "REXRentArea";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_RentArea");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].ParameterFieldName = "REXShopTypeName";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].ParameterFieldName = "REXShopCode";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labShopCode");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        //本月销售
        Field[8] = new ParameterField();
        Field[8].ParameterFieldName = "REXCurrentMonthSales";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MonthSellCount");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        //同比销售额
        Field[9] = new ParameterField();
        Field[9].ParameterFieldName = "REXYearMPaidAmt";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearMPaidAmt");
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        //同比差异
        Field[10] = new ParameterField();
        Field[10].ParameterFieldName = "REXYearRate";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_yearRate");
        Field[10].CurrentValues.Add(DiscreteValue[10]);

        //环比销售额
        Field[11] = new ParameterField();
        Field[11].ParameterFieldName = "REXOldMPaidAmt";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMPaidAmt");
        Field[11].CurrentValues.Add(DiscreteValue[11]);

        //环比差异
        Field[12] = new ParameterField();
        Field[12].ParameterFieldName = "REXOldMRate";
        DiscreteValue[12] = new ParameterDiscreteValue();
        DiscreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_oldMRate");
        Field[12].CurrentValues.Add(DiscreteValue[12]);


        //本年
        Field[13] = new ParameterField();
        Field[13].ParameterFieldName = "REXCurrentYear";
        DiscreteValue[13] = new ParameterDiscreteValue();
        DiscreteValue[13].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_CurrentYear");
        Field[13].CurrentValues.Add(DiscreteValue[13]);

        //上年
        Field[14] = new ParameterField();
        Field[14].ParameterFieldName = "REXDownYear";
        DiscreteValue[14] = new ParameterDiscreteValue();
        DiscreteValue[14].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_DownYear");
        Field[14].CurrentValues.Add(DiscreteValue[14]);

        //年度差异
        Field[15] = new ParameterField();
        Field[15].ParameterFieldName = "REXYearDiff";
        DiscreteValue[15] = new ParameterDiscreteValue();
        DiscreteValue[15].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_YearDiff");
        Field[15].CurrentValues.Add(DiscreteValue[15]);

        //打印日期
        Field[16] = new ParameterField();
        Field[16].ParameterFieldName = "REXSearchDate";
        DiscreteValue[16] = new ParameterDiscreteValue();
        DiscreteValue[16].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_PrintDate");
        Field[16].CurrentValues.Add(DiscreteValue[16]);

        
        Field[17] = new ParameterField();
        Field[17].ParameterFieldName = "REXCustName";
        DiscreteValue[17] = new ParameterDiscreteValue();
        DiscreteValue[17].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustName");
        Field[17].CurrentValues.Add(DiscreteValue[17]);

        //环比
        Field[18] = new ParameterField();
        Field[18].ParameterFieldName = "REXCycleCompare";
        DiscreteValue[18] = new ParameterDiscreteValue();
        DiscreteValue[18].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_CycleCompare");
        Field[18].CurrentValues.Add(DiscreteValue[18]);

        
        //年度
        Field[19] = new ParameterField();
        Field[19].ParameterFieldName = "REXYear";
        DiscreteValue[19] = new ParameterDiscreteValue();
        DiscreteValue[19].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblYearSum");
        Field[19].CurrentValues.Add(DiscreteValue[19]);

        //商业项目
        Field[20] = new ParameterField();
        Field[20].ParameterFieldName = "REXBizProject";
        DiscreteValue[20] = new ParameterDiscreteValue();
        DiscreteValue[20].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        Field[20].CurrentValues.Add(DiscreteValue[20]);
        //可均价
        Field[21] = new ParameterField();
        Field[21].ParameterFieldName = "RexPrice";
        DiscreteValue[21] = new ParameterDiscreteValue();
        DiscreteValue[21].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_SalesPrice");
        Field[21].CurrentValues.Add(DiscreteValue[21]);
        //交易笔数
        Field[22] = new ParameterField();
        Field[22].ParameterFieldName = "RexTotalReceipt";
        DiscreteValue[22] = new ParameterDiscreteValue();
        DiscreteValue[22].Value = (String)GetGlobalResourceObject("BaseInfo", "Sale_lblTransNumber");
        Field[22].CurrentValues.Add(DiscreteValue[22]);
        //坪效
        Field[23] = new ParameterField();
        Field[23].ParameterFieldName = "RexPx";
        DiscreteValue[23] = new ParameterDiscreteValue();
        DiscreteValue[23].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_AreaSalesRate");
        Field[23].CurrentValues.Add(DiscreteValue[23]);

        //月份：查询条件；
        Field[24] = new ParameterField();
        Field[24].ParameterFieldName = "RexMonth";
        DiscreteValue[24] = new ParameterDiscreteValue();
        DiscreteValue[24].Value = this.ddlYear.SelectedValue.ToString() + "年" +this.ddlMonth.SelectedValue.ToString() +"月" ;
        Field[24].CurrentValues.Add(DiscreteValue[24]);

        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string strAnd = "";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAnd  = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
            for (int i = 0; i < 5; i++)
            {
                strAnd = strAnd.Replace("ConShop", "transshopmth");
            }
        }

        //注意：在有销售额相除时，要转换成decimal(16,2)，不然容易溢出
        string str_sql = "select aa.shopcode,aa.storename, aa.month,aa.shopname,aa.shoptypename,aa.floorname,aa.tradename,aa.brandname,aa.rentarea," +
                 " aa.paidamt,aa.lypaidamt,aa.lyrate,aa.pppaidamt,aa.pprate,aa.yearpaidamt,aa.lyyearpaidamt,aa.custshortname custname," +
                 //" case aa.totalreceipt when 0 then 0 else cast((aa.paidamt / aa.totalreceipt) as decimal(16,2)) end as price," +  //客均价：月销售总额/月小票数
                 //" case aa.usearea when 0 then 0 else cast((aa.paidamt/aa.usearea)/aa.mDays as decimal(16,2)) end as px," +  //坪效：月销售总额/当月天数/可出租面积，非签约面积
                 " (case when aa.lyyearpaidamt<>0 then ((aa.yearpaidamt-aa.lyyearpaidamt)/aa.lyyearpaidamt)*100 else 0 end) as lyyearrate" +
                 " from (" +  //嵌套查询
                        " select transshopmth.storename, transshopmth.month,transshopmth.shopname,transshopmth.shopcode,transshopmth.shoptypename," +
                        " transshopmth.floorname,transshopmth.tradename,transshopmth.brandname,conshop.rentarea,customer.custshortname," +
                        " transshopmth.paidamt,transshopmth.totalreceipt,isnull(transshopmth.lypaidamt,0) as lypaidamt," +
                        " (case when transshopmth.lypaidamt<>0 then ((transshopmth.paidamt-transshopmth.lypaidamt) / transshopmth.lypaidamt) *100 else 0 end) as lyrate," +
                        " isnull(transshopmth.pppaidamt,0) as pppaidamt," + 
                        " (case when transshopmth.pppaidamt<>0 then ((transshopmth.paidamt-transshopmth.pppaidamt) / transshopmth.pppaidamt) *100 else 0 end) as pprate," +
                        " (select isnull(sum(a.paidamt),0) from transshopmth a where a.shopid=transshopmth.shopid and a.month between cast(year(transshopmth.month) as char(4)) and transshopmth.month) as yearpaidamt," +  //年度累计销售额
                        " (select isnull(sum(a.paidamt),0)  from transshopmth a where a.shopid=transshopmth.shopid and a.month between  cast(year(datediff(year,-1,transshopmth.month)) as char(4)) and datediff(year,-1,transshopmth.month)) as lyyearpaidamt," + //同比年度累计销售额
                        " (select sum(unit.usearea) from unit inner join conshopunit on (conshopunit.unitid=unit.unitid) where conshopunit.shopid=conshop.shopid) as usearea" +  //商铺可出租面积，不同于签约面积
                        " from transshopmth inner join conshop on (conshop.shopid=transshopmth.shopid) inner join contract on (conshop.contractid=contract.contractid)" +
                        " inner join customer on (contract.custid=customer.custid)" +
                        " where conshop.shopstatus=1";


        //条件查询
        if (ddlStoreName.Text != "")
        {
            str_sql += " AND transshopmth.storeID  = '" + ddlStoreName.SelectedValue.Trim() + "' ";
        }

        if (ddlMonth.SelectedValue.ToString() != "" || ddlYear.SelectedValue.ToString() != "")
        {
            str_sql += " And transshopmth.month = '" + this.ddlYear.SelectedValue.ToString() + "-" + this.ddlMonth.SelectedValue.ToString() + "-01'";
        }

   
        str_sql += strAnd + ") as aa order by aa.shoptypename,aa.floorname,aa.storename,aa.month ";

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptShopMonthSalesCompareInfo.rpt"; 

    }


    //清除页面
    private void ClearPage()
    {
        ddlStoreName.SelectedIndex = 0;
    
    }


    //查询操作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }


    //撤消操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();

    }



}
