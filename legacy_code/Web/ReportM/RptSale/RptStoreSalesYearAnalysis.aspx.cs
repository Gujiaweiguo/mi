using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using BaseInfo.Store;
using Base.Page;
using BaseInfo.Dept;
public partial class ReportM_RptSale_RptStoreSalesYearAnalysis : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitDDL();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Menu_StoreSalesYearAnalysis");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.RadioButton8.Checked = true;
        }
    }


    private void InitDDL()
    {
        BaseBO baseBo = new BaseBO();
        //绑定商业项目
        baseBo.WhereClause ="depttype=" + Dept.DEPT_TYPE_MALL + " and DeptStatus=" + Dept.DEPTSTATUS_VALID ;
        baseBo.OrderBy = "OrderID";
        Resultset rs4 = baseBo.Query(new Dept());

        ddlStoreName.Items.Clear();
        //ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Dept bd in rs4)
        {
            ddlStoreName.Items.Add(new ListItem(bd.DeptName , bd.DeptID.ToString()));
        }

        //int intMonth = 12;
        //ddlMonth.Items.Clear();
        //ddlMonth.Items.Add(new ListItem("", ""));
        //for (int iMonth = 1; iMonth <= intMonth; iMonth++) 
        //{
        //    ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        //}
        //ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
        int year = Convert.ToInt16(DateTime.Now.Year);
      //  ddlYear.Items.Add(new ListItem("",""));
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        ddlYear.SelectedValue = DateTime.Now.Year.ToString();

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
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_StoreSalesYearAnalysis");
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
        string str_sql="";
        string wheresql="";
        if (ddlStoreName.Text != "")
        {
            wheresql = " AND transshopmth.storeid=" + Convert.ToInt32(ddlStoreName.SelectedValue).ToString();
        }
        if (ddlYear.Text != "" )
        {
            wheresql += " AND (year(transshopmth.month)='" + ddlYear.Text + "' or year(dateadd(year,1,transshopmth.month))='" + ddlYear.Text + "') ";
        }
        else
        {
            wheresql += " AND (year(transshopmth.month)='" + DateTime.Now.Year.ToString() + "' or year(dateadd(year,1,transshopmth.month))='" + DateTime.Now.Year.ToString() + "') ";
        }

        if (this.RadioButton8.Checked == true)
        {
            str_sql = "select aa.storename,aa.shoptypename,year(aa.month) year," +
                   "sum(case when month(aa.month)=1 then saleamt end) one," +
                   "sum(case when month(aa.month)=2 then saleamt end) two," +
                   "sum(case when month(aa.month)=3 then saleamt end) three," +
                   "sum(case when month(aa.month)=4 then saleamt end) four," +
                   "sum(case when month(aa.month)=5 then saleamt end) five," +
                   "sum(case when month(aa.month)=6 then saleamt end) six," +
                   "sum(case when month(aa.month)=7 then saleamt end) seven," +
                   "sum(case when month(aa.month)=8 then saleamt end) eight," +
                   "sum(case when month(aa.month)=9 then saleamt end) nine," +
                   "sum(case when month(aa.month)=10 then saleamt end) ten," +
                   "sum(case when month(aa.month)=11 then saleamt end) ele," +
                   "sum(case when month(aa.month)=12 then saleamt end) twn,sum(saleamt) totalamt " +
                   "from " +
                       "(select transshopmth.storename,transshopmth.shoptypename,transshopmth.month,sum(transshopmth.paidamt) SaleAmt " +
                       "from transshopmth where 1=1" + wheresql + //查询条件
                       "group by transshopmth.storename,transshopmth.shoptypename,transshopmth.month" +
                   ") aa group by aa.storename,aa.shoptypename,year(aa.month) " +
                   "order by aa.storename,aa.shoptypename,year(aa.month)";

            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptStoreSalesYearAnalysisNew.rpt";

        }
        else
        {
            str_sql = "select aa.storename,0 as shoptypename,year(aa.month) year," +
                   "sum(case when month(aa.month)=1 then saleamt end) one," +
                   "sum(case when month(aa.month)=2 then saleamt end) two," +
                   "sum(case when month(aa.month)=3 then saleamt end) three," +
                   "sum(case when month(aa.month)=4 then saleamt end) four," +
                   "sum(case when month(aa.month)=5  then saleamt end) five," +
                   "sum(case when month(aa.month)=6 then saleamt end) six," +
                   "sum(case when month(aa.month)=7 then saleamt end) seven," +
                   "sum(case when month(aa.month)=8 then saleamt end) eight," +
                   "sum(case when month(aa.month)=9 then saleamt end) nine," +
                   "sum(case when month(aa.month)=10 then saleamt end) ten," +
                   "sum(case when month(aa.month)=11 then saleamt end) ele," +
                   "sum(case when month(aa.month)=12 then saleamt end) twn,sum(saleamt) totalamt " +
                   "from " +
                       "(select transshopmth.storename,transshopmth.month,sum(transshopmth.paidamt) SaleAmt " +
                       "from transshopmth where 1=1" + wheresql + //查询条件
                       "group by transshopmth.storename,transshopmth.month" +
                   ") aa group by aa.storename,year(aa.month) " +
                   "order by aa.storename,year(aa.month)";

            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptStoreSalesYearAnalysisNew1.rpt";
        }
        
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        

    }

}
