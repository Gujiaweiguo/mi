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

using Base.Biz;
using Base.DB;
using BaseInfo.Store;
using Base.Page;

public partial class ReportM_RptGroup_RptCompanyRunCaseAnalyse : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitDDL();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_CompanyRunCaseAnalyse");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }

    private void InitDDL()
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select deptid,deptname from dept where depttype=4");
        DDLArea.Items.Clear();
        DDLArea.Items.Add(new ListItem("", ""));
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            DDLArea.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString().Trim(), ds.Tables[0].Rows[i][0].ToString().Trim()));
        }
        DDlCity.Items.Clear();


        int intMonth = 12;
        ddlMonth.Items.Clear();
        //ddlMonth.Items.Add(new ListItem("", ""));
        for (int iMonth = 1; iMonth <= intMonth; iMonth++)
        {
            ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
        int year = Convert.ToInt16(DateTime.Now.Year);
        //  ddlYear.Items.Add(new ListItem("",""));
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
    }
    protected void DDLArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select deptid,deptname from dept where depttype=5 and pdeptid='" + DDLArea.SelectedValue + "'");
        DDlCity.Items.Clear();
        DDlCity.Items.Add(new ListItem("", ""));
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            DDlCity.Items.Add(new ListItem(ds.Tables[0].Rows[i][1].ToString().Trim(), ds.Tables[0].Rows[i][0].ToString().Trim()));
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
        InitDDL();
        txtCustName.Text = "";
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_CompanyRunCaseAnalyse");
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
        string str_sql = "";
        string wheresql = " where year(transshopmth.month)='" + ddlYear.SelectedValue.Trim() + "' and month(transshopmth.month)='" + ddlMonth.SelectedValue.Trim() + "' ";

        if (DDLArea.Text != "")
        {
            wheresql += " and bb.deptid='" + DDLArea.SelectedValue + "' ";
        }
        if (DDlCity.Text != "")
        {
            wheresql += " and aa.deptid='" + DDlCity.SelectedValue + "' ";
        }
        if(txtCustName.Text.Trim()!="")
        {        
            wheresql += " and customer.custname like '%"+txtCustName.Text.Trim()+"%' ";
        }


        str_sql = "select aaa.custname,aaa.storename,aaa.shopname,aaa.brandname,aaa.month,aaa.rentarea,aaa.saleamt," +
                    "aaa.ppsaleamt,aaa.lysaleamt," +
                    "aaa.InvAmt,aaa.oweAmt,aaa.allinvamt,aaa.alloweamt," +
                    "cast((aaa.saleamt/aaa.rentarea/day(dateadd(mm,1,aaa.month)- day(aaa.month))) as decimal(10,2)) px," +
                    "cast((aaa.ppsaleamt/aaa.rentarea/day(dateadd(mm,1,aaa.month)- day(aaa.month))) as decimal(10,2)) pppx," +
                    "cast((aaa.lysaleamt/aaa.rentarea/day(dateadd(mm,1,aaa.month)- day(aaa.month))) as decimal(10,2)) lypx " +
                    "from (select contract.custid,customer.custname,conshop.storeid,transshopmth.storename,conshop.shopiD,conshop.shopname,transshopmth.brandname," +
                        "conshop.rentarea,transshopmth.month,transshopmth.paidamt saleamt,transshopmth.pppaidamt ppsaleamt," +
                        "transshopmth.lypaidamt lysaleamt,inv.InvAmt,inv.oweAmt,allInv.allInvAmt,allInv.alloweAmt " +
                        "from transshopmth " +
                        "inner join dept on (transshopmth.storeid=dept.deptid) " +
                        "inner join conshop on (transshopmth.shopid=conshop.shopid) " +
                        "inner join contract on (contract.contractid=conshop.contractid) " +
                        "inner join customer on (customer.custid=contract.custid) " +
                        "inner join (select invoiceheader.contractid,invoiceheader.invperiod,sum(invoiceheader.InvActPayAmtl) InvAmt," +
                            "sum(invoiceheader.InvActPayAmtl-invoiceheader.InvPaidAmtL) oweAmt " +
                            "from invoiceheader where invstatus in (1,2) " +
                            "group by invoiceheader.contractid,invoiceheader.invperiod ) Inv " +
                        "on (Inv.contractid=conshop.contractid and inv.invperiod=transshopmth.month) " +
                        "inner join (select invoiceheader.contractid,sum(invoiceheader.InvActPayAmtl) allInvAmt," +
                            "sum(invoiceheader.InvActPayAmtl-invoiceheader.InvPaidAmtL) alloweAmt " +
                            "from invoiceheader where invstatus in (1,2) " +
                            "group by invoiceheader.contractid) AllInv " +
                        "on (AllInv.contractid=conshop.contractid)	" +
                        "left join (select deptid,pdeptid,deptname from dept where depttype=5) aa " +
                            "on (aa.deptid=dept.pdeptid) " +
                        "left join (select deptid,deptname from dept where depttype=4) bb " +
                            "on (bb.deptid=aa.pdeptid)" + wheresql + ") as aaa ";


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Group\\RptCompanyRunCaseAnalyse.rpt";

    }
}
