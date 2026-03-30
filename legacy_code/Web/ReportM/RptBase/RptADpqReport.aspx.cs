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
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
using Lease.AdContract;
using BaseInfo.Dept;

public partial class ReportM_RptBase_RptADpqReport : BasePage
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
        baseBo.WhereClause = " depttype='" + Dept.DEPT_TYPE_MALL + "' ";
        rs = baseBo.Query(dept);
        foreach (Dept dep in rs)
        {
            ddlStore.Items.Add(new ListItem(dep.DeptName, dep.DeptID.ToString()));
        }

        AdBoardType adType = new AdBoardType();
        baseBo.WhereClause = "";
        rs = baseBo.Query(adType);
        ddlAdType.Items.Add(new ListItem("", ""));
        foreach (AdBoardType type in rs)
        {
            ddlAdType.Items.Add(new ListItem(type.AdBoardTypeName, type.AdBoardTypeID.ToString()));
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptBase/RptADpqReport.aspx");
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
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_ADpqReport");
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
        string str_sql = @" select contract.contractcode,customer.custcode,customer.custshortname,Case Contract.ContractStatus when '0' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_FIRST") + "' when '1' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_TEMP") + "' when '2' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_INGEAR") + "' when '3' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_ATTREM") + "' when '4' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_END") + "' when '5' then '" + (String)GetGlobalResourceObject("Parameter", "CONTRACTSTATUS_TYPE_PAUSE") + @"' End as ContractStatus,
                            contract.constartdate,contract.conenddate,adboardmanage.adboardcode,adboardmanage.adboardname,adboardtype.adboardtypename,
                            conadboard.airtime,case conadboard.freq when 'D' then '" + (String)GetGlobalResourceObject("BaseInfo", "Internet_optDaily") + "' when 'W' then '" + (String)GetGlobalResourceObject("BaseInfo", "Internet_optWeekly") + "' when 'M' then '" + (String)GetGlobalResourceObject("BaseInfo", "Internet_optMthly") + @"' end freq,
                            case freq when 'D' then cast(conadboard.freqdays as varchar(5)) when 'W' then ((case conadboard.freqmon when 'Y' then '周一,' when 'N' then '' end) + (case conadboard.freqtue when 'Y' then '周二,' when 'N' then '' end)+(case conadboard.freqwed when 'Y' then '周三,' when 'N' then '' end)+(case conadboard.freqthu when 'Y' then '周四,' when 'N' then '' end)+(case conadboard.freqfri when 'Y' then '周五,' when 'N' then '' end)+(case conadboard.freqsat when 'Y' then '周六,' when 'N' then '' end)+(case conadboard.freqsun when 'Y' then '周日' when 'N' then '' end))
                            when 'M' then (cast(conadboard.betweenfr as varchar(5)) +' - '+ cast(conadboard.betweento as varchar(5))) end betwe  
                            from contract
                            left join customer on contract.custid=customer.custid
                            left join conadboard on conadboard.contractid=contract.contractid
                            left join adboardmanage on adboardmanage.adboardid=conadboard.adboardid
                            left join adboardtype on adboardtype.adboardtypeid=adboardmanage.adboardtypeid
                            where contract.bizmode=3 ";

        if (ddlStore.Text != "")
        {
            str_sql = str_sql + " AND conadboard.storeid = '" + ddlStore.SelectedValue.Trim() + "' ";
        }
        if (ddlAdBoard.Text != "")
        {
            str_sql = str_sql + " AND adboardmanage.adboardid = '" + ddlAdBoard.SelectedValue.Trim() + "' ";
        }
        if (ddlAdType.Text != "")
        {
            str_sql = str_sql + " AND adboardtype.adboardtypeid = '" + ddlAdType.SelectedValue.Trim() + "' ";
        }


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\RptADpqReport.rpt";

    }
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlAdBoard.Items.Clear();
        BaseBO basebo = new BaseBO();
        AdBoardManage adManage = new AdBoardManage();
        Resultset rs = new Resultset();
        basebo.WhereClause = " storeid='" + ddlStore.SelectedValue.Trim() + "' ";
        rs = basebo.Query(adManage);
        ddlAdBoard.Items.Add(new ListItem("", ""));
        foreach (AdBoardManage AD in rs)
        {
            ddlAdBoard.Items.Add(new ListItem(AD.AdBoardName, AD.AdBoardID.ToString()));
        }
    }
}
