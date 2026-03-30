using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;

public partial class ReportM_RptGroup_RptUnitLeaseAnalyse : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitDDL();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_UnitLeaseAnalyse");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }

    private void InitDDL()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = new Resultset();
        Dept dept = new Dept();
        baseBo.OrderBy  = "OrderID";
        baseBo.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        rs = baseBo.Query(dept);
        DDLStore.Items.Clear();
        //DDLStore.Items.Add(new ListItem("", ""));
        foreach (Dept dep in rs)
        {
            DDLStore.Items.Add(new ListItem(dep.DeptName, dep.DeptID.ToString()));
        }
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
        DDLStore.SelectedValue = "";
        txtUnitCode.Text = "";
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_UnitLeaseAnalyse");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }



        string where = "";

        if (DDLStore.Text.Trim() != "")
        {
            where += " and unit.storeid='" + DDLStore.SelectedValue.Trim() + "' ";
        }
        if (txtUnitCode.Text.Trim() != "")
        {
            where += " and unit.unitcode like '%" + txtUnitCode.Text.Trim() + "%' ";
        }


        string str_sql = "select unit.unitcode,case when unit.unitstatus=0 then '未出租' when unit.unitstatus=1 then '已出租' end unitstatus,unit.floorarea,tr1.tradename,aa.unitprice budget,bb.unitprice leaseprice," +
                                 "potUnit.custname,potunit.rentarea,tr2.tradename tradename2,conshopbrand.brandname,potunit.avgamt," +
                                 "potunit.rentalprice,potunit.rentinc,potunit.rentmonth " +
                            "from unit " +
                            "inner join traderelation tr1 on (unit.trade2ID=tr1.tradeid) " +
                            "left join (select unitid,budgetyear,unitprice from budget where budgetyear=year(getdate())) aa " +
                                "on (unit.unitid=aa.unitid) " +
                            "left join (select conshopunit.unitid,ConFormulaH.UnitPrice," +
                                "ConFormulaH.fstartdate,ConFormulaH.Fenddate from conshopunit " +
                                "inner join conshop on (conshop.shopid=conshopunit.shopid) " +
                                "inner join ConFormulaH on (ConFormulaH.contractid=conshop.contractid) " +
                                "where ConFormulaH.chargetypeid =101 and ConFormulaH.fstartdate <=getdate()  " +
                                "and ConFormulaH.Fenddate>=getdate() " +
                            ") bb on (unit.unitid=bb.unitid) " +
                            "inner join (select potshopunit.unitid,potcustomer.custcode,potcustomer.custname,potshop.rentarea,potshop.rentalprice,potshop.rentinc," +
                                "datediff(month,shopstartdate,shopenddate) rentmonth,potbrand.tradeid,potbrand.brandid,potbrand.avgamt " +
                                "from potshopunit inner join potshop on (potshop.potshopid=potshopunit.potshopid) " +
                                 "inner join potcustomer on (potshop.custid=potcustomer.custid) " +
                                "left join (select custid,tradeid,brandid,avgamt from PotCustBrand) potbrand " +
                                "on (potbrand.custid=potcustomer.custid)) potUnit on (potunit.unitid=unit.unitid) " +
                                "left join traderelation tr2 on (potunit.tradeid=tr2.tradeid) " +
                                "left join conshopbrand on (potunit.brandid=conshopbrand.brandid)  where unit.unitstatus<>2 ";



        str_sql += where;

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Group\\RptUnitLeaseAnalyse.rpt";

    }
}
