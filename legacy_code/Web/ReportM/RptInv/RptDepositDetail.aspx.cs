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
using BaseInfo.Dept;

public partial class ReportM_RptInv_RptDepositDetail : BasePage
{
    public string fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    private void InitDDL()
    {
        BaseBO baseBo = new BaseBO();
        Dept dept = new Dept();
        Resultset rs = new Resultset();
        baseBo.WhereClause = "depttype='" + Dept.DEPT_TYPE_REGION + "'";
        rs = baseBo.Query(dept);
        ddlArea.Items.Clear();
        ddlArea.Items.Add(new ListItem("", ""));
        foreach (Dept dep in rs)
        {
            ddlArea.Items.Add(new ListItem(dep.DeptName, dep.DeptID.ToString()));
        }

        baseBo.WhereClause = "depttype='" + Dept.DEPT_TYPE_CHILD_SONLD + "'";
        rs = baseBo.Query(dept);
        ddlSubs.Items.Clear();
        ddlSubs.Items.Add(new ListItem("", ""));
        foreach (Dept dep in rs)
        {
            ddlSubs.Items.Add(new ListItem(dep.DeptName, dep.DeptID.ToString()));
        }
    }
    protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        Dept dept = new Dept();
        Resultset rs = new Resultset();
        baseBo.WhereClause = "pdeptid='" + ddlArea.SelectedValue + "'";
        rs = baseBo.Query(dept);
        ddlCity.Items.Clear();
        ddlCity.Items.Add(new ListItem("", ""));
        foreach (Dept dep in rs)
        {
            ddlCity.Items.Add(new ListItem(dep.DeptName, dep.DeptID.ToString()));
        }
    }
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        Dept dept = new Dept();
        Resultset rs = new Resultset();
        baseBo.WhereClause = "pdeptid='" + ddlCity.SelectedValue + "'";
        rs = baseBo.Query(dept);
        DDlStore.Items.Clear();
        DDlStore.Items.Add(new ListItem("", ""));
        foreach (Dept dep in rs)
        {
            DDlStore.Items.Add(new ListItem(dep.DeptName, dep.DeptID.ToString()));
        }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptInv/RptDepositDetail.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitleDetail";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptDepositDetail");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXTitleSum";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptDepositSum");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string where_sql = "";
        if (ddlArea.Text != "")
        {
            where_sql = where_sql + " AND c.deptid='" + ddlArea.SelectedValue + "' ";
        }
        if (ddlCity.Text != "")
        {
            where_sql = where_sql + " AND b.deptid='" + ddlCity.SelectedValue + "' ";
        }
        if (DDlStore.Text != "")
        {
            where_sql = where_sql + " AND a.deptid='" + DDlStore.SelectedValue + "' ";
        }
        if (ddlSubs.Text != "")
        {
            where_sql = where_sql + " AND subsidiary.subsid='" + ddlSubs.SelectedValue + "' ";
        }
        if (rbtSum.Checked)
        {
            string str_sql = @"select contract.contractcode,contract.constartdate,contract.conenddate,
                                customer.custshortname,sum(conformulah.baseamt) baseamt,subsidiary.subsid,subsidiary.subsname,
                                a.deptid storeid,a.deptname storename,b.deptid cityid,b.deptname cityname,c.deptid areaid,c.deptname areaname 
                                from conformulah
                                inner join contract on conformulah.contractid=contract.contractid
                                inner join customer on contract.custid=customer.custid
                                left join conshop on conshop.contractid=contract.contractid
                                left join dept a on a.deptid=conshop.storeid 
                                left join dept b on a.pdeptid=b.deptid
                                left join dept c on b.pdeptid=c.deptid
                                left join subsidiary on contract.subsid=subsidiary.subsid
                                where conformulah.chargetypeid in (select chargetypeid from chargetype where chargeclass=2)"
                                + where_sql +
                                @"group by contract.contractcode,contract.constartdate,contract.conenddate,
                                customer.custshortname,subsidiary.subsid,subsidiary.subsname,a.deptid,a.deptname,
                                b.deptid,b.deptname,c.deptid,c.deptname ";

            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptDepositSum.rpt";
        }
        else
        {
            string str_sql = @" select contract.contractcode,contract.constartdate,contract.conenddate,
                                customer.custshortname,chargetype.chargetypename,sum(conformulah.baseamt) baseamt,subsidiary.subsid,subsidiary.subsname,
                                a.deptid storeid,a.deptname storename,b.deptid cityid,b.deptname cityname,c.deptid areaid,c.deptname areaname
                                from conformulah
                                inner join contract on conformulah.contractid=contract.contractid
                                inner join customer on contract.custid=customer.custid
                                left join conshop on conshop.contractid=contract.contractid
                                left join dept a on a.deptid=conshop.storeid 
                                left join dept b on a.pdeptid=b.deptid
                                left join dept c on b.pdeptid=c.deptid
                                left join subsidiary on contract.subsid=subsidiary.subsid
                                left join chargetype on chargetype.chargetypeid=conformulah.chargetypeid
                                where conformulah.chargetypeid in (select chargetypeid from chargetype where chargeclass=2)"
                                + where_sql +
                                @"group by contract.contractcode,contract.constartdate,contract.conenddate,
                                customer.custshortname,chargetype.chargetypename,subsidiary.subsid,subsidiary.subsname,a.deptid,a.deptname,
                                b.deptid,b.deptname,c.deptid,c.deptname ";

            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptDepositDetail.rpt";
        }
        


        

    }
}
