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

using Base.Page;
using Base.Biz;
using BaseInfo.Dept;
using Base.DB;
using BaseInfo.Store;

public partial class ReportM_RptInv_RptInvMonth : BasePage
{
    public string pageTitle = "";
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDdl();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_InvMonth");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01"); 
        }
    }

    private void BindDdl()
    {
        //BaseBO basebo = new BaseBO();
        //Dept dept = new Dept();
        //DataSet ds = new DataSet();
        //basebo.WhereClause = "depttype=" + Dept.DEPT_TYPE_MALL;
        //basebo.OrderBy = "orderid";
        //ds = basebo.QueryDataSet(dept);
        //ddlStoreName.Items.Clear();
        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //{
        //    ddlStoreName.Items.Add(new ListItem(ds.Tables[0].Rows[i]["deptname"].ToString(), ds.Tables[0].Rows[i]["deptid"].ToString()));
        //}
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "StoreStatus=1";
        baseBo.OrderBy = "orderid";
        Resultset rs = baseBo.Query(new Store());
        foreach (Store objStore in rs)
        {
            ddlStoreName.Items.Add(new ListItem(objStore.StoreName, objStore.StoreId.ToString()));
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
        txtContract.Text = "";
        ddlStoreName.SelectedIndex = 0;
        txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
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
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_InvMonth");
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

        if (this.txtPeriod.Text.Trim() == "")
        { this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01"); }

//        string str_sql = @"select dept.deptid,dept.deptname,subsidiary.subsid,subsidiary.subsname,contract.contractcode,
//	                            customer.custcode,customer.custname,conshop.shopcode,conshop.shopname,
//	                            conformulah.fstartdate,conformulah.fenddate,convert(varchar(7),transheader.bizdate,120) salemonth,
//	                            case formulatype when 'o' then (year(conformulah.fenddate)-year(conformulah.fstartdate))*12+month(conformulah.fenddate)-month(conformulah.fstartdate) 
//	                            when 'f' then conformulah.fixedrental when 'v' then conformulam.minsum end rental,
//	                            case formulatype when 'v' then conformulap.pcent*100 else 0 end rate,
//	                            case formulatype when 'v' then conformulap.salesto else 0 end salesto,
//	                            sum(transdetail.netamt) amt,cast(sum(transdetail.netamt)*(case formulatype when 'v' then conformulap.pcent*100 else 0 end)/100 as decimal(15,2)) amtp,
//	                            cast(sum(transdetail.netamt)*(case formulatype when 'v' then conformulap.pcent*100 else 0 end)/100 as decimal(15,2))-
//	                            (case formulatype when 'o' then (year(conformulah.fenddate)-year(conformulah.fstartdate))*12+month(conformulah.fenddate)-month(conformulah.fstartdate) 
//	                            when 'f' then conformulah.fixedrental when 'v' then conformulam.minsum end) percentamt,chargetype.chargetypeid,chargetype.chargetypename
//                            from contract 
//	                            inner join customer on customer.custid=contract.custid
//	                            inner join conshop on conshop.contractid=contract.contractid
//	                            inner join conformulah on conformulah.contractid=contract.contractid
//	                            left join conformulap on conformulap.formulaid=conformulah.formulaid
//	                            left join conformulam on conformulam.formulaid=conformulah.formulaid
//	                            left join transheader on transheader.tenantid=conshop.shopid
//	                            left join transdetail on transdetail.transid=transheader.transid
//	                            inner join dept on dept.deptid=conshop.storeid
//	                            inner join subsidiary on contract.subsid=subsidiary.subsid
//                                inner join chargetype on conformulah.chargetypeid=chargetype.chargetypeid
//                            where conformulah.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) 
//	                            and convert(varchar(7),transheader.bizdate,120)='" + txtPeriod.Text.Substring(0, 7) + @"' 
//	                            and transheader.bizdate between conformulah.fstartdate and conformulah.fenddate ";

        string str_sql = @"select dept.deptid,dept.deptname,subsidiary.subsid,subsidiary.subsname,contract.contractcode,customer.custcode,conshop.shopcode,conshop.shopname," +
        "conformulah.fstartdate,conformulah.fenddate,'" + this.txtPeriod.Text + "' salemonth," +
        "case formulatype when 'o' then (year(conformulah.fenddate)-year(conformulah.fstartdate))*12+month(conformulah.fenddate)-month(conformulah.fstartdate) " +
            "when 'f' then conformulah.fixedrental when 'v' then conformulam.minsum end rental," +
            "case formulatype when 'v' then conformulap.pcent*100 else 0 end rate," +
            "case formulatype when 'v' then conformulap.salesto else 0 end salesto," +
            "aa.paidamt amt,cast(aa.paidamt*(case formulatype when 'v' then conformulap.pcent*100 else 0 end)/100 as decimal(15,2)) amtp," +
            "cast(aa.paidamt*(case formulatype when 'v' then conformulap.pcent*100 else 0 end)/100 as decimal(15,2))-(case formulatype when 'o' then (year(conformulah.fenddate)-year(conformulah.fstartdate))*12+month(conformulah.fenddate)-month(conformulah.fstartdate) " +
            "when 'f' then conformulah.fixedrental when 'v' then conformulam.minsum end) percentamt,chargetype.chargetypeid,chargetype.chargetypename " +
            "from contract inner join customer on (customer.custid=contract.custid) " +
            "inner join conshop on (contract.contractid=conshop.contractid) " +
            "inner join conformulah on conformulah.contractid=contract.contractid " +
            "left join conformulap on conformulap.formulaid=conformulah.formulaid " +
            "left join conformulam on conformulam.formulaid=conformulah.formulaid " +
            "inner join dept on dept.deptid=conshop.storeid inner join subsidiary on contract.subsid=subsidiary.subsid " +
            "inner join chargetype on conformulah.chargetypeid=chargetype.chargetypeid " +
            "inner join (select sum(paidamt) paidamt,shopid from transsku where bizdate between '" + this.txtPeriod.Text + "' and '" + Convert.ToDateTime(this.txtPeriod.Text).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + "' group by shopid) aa on (aa.shopid=conshop.shopid) " +
            "where chargetype.chargetypeid in(101,102) and '2010-3-1' between conformulah.fstartdate and conformulah.fenddate ";

        if (ddlStoreName.Text != "")
        {
            str_sql += "and dept.deptid='" + ddlStoreName.SelectedValue + "' ";
        }
        if (txtContract.Text != "")
        {
            str_sql += "AND contract.contractcode like '%" + txtContract.Text.Trim() + "%' ";    
        }
        //str_sql += @"group by dept.deptid,dept.deptname,subsidiary.subsid,subsidiary.subsname,contract.contractcode,
                    //customer.custcode,customer.custname,conshop.shopcode,conshop.shopname,
                    //conformulah.fstartdate,conformulah.fenddate,convert(varchar(7),transheader.bizdate,120),
                    //case formulatype when 'o' then (year(conformulah.fenddate)-year(conformulah.fstartdate))*12+month(conformulah.fenddate)-month(conformulah.fstartdate) 
                    //when 'f' then conformulah.fixedrental when 'v' then conformulam.minsum end,
                    //case formulatype when 'v' then conformulap.pcent*100 else 0 end,
                    //case formulatype when 'v' then conformulap.salesto else 0 end,chargetype.chargetypeid,chargetype.chargetypename 
                    //order by contract.contractcode,conshop.shopcode,conformulah.fenddate,chargetype.chargetypeid,salesto ";
        
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptInvMonth.rpt";

    }
}
