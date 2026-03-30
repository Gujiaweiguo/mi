using System;
using System.Security;
using System.Web.Configuration;
using System.Web.Security;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Text;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

using Base.DB;
using BaseInfo.User;
using Base.Biz;
using Base.Page;
using Base.Util;
using Base;
using Lease;
using Lease.PotBargain;
using RentableArea;
using Lease.Contract;
using Lease.Customer;
using BaseInfo.Store;





public partial class Report_ContractAlert:BasePage
{
    public string baseInfo;
    public string fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindStore();
           // BindBizMode();
            //BindAllBiz();
            //BindBizStyle();
            //BindLease();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ContractAlert");
            fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            BtnQuery.Attributes.Add("OnClick", "return CheckAll(form1)");
            txtLeaseStart.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtLeaseEnd.Text = DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd");
        }
    }

    //
    private void BindStore()
    {

        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = " storestatus=1";
        baseBo.OrderBy = " orderid";
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }
    }


    //绑定经营方式
    private void BindBizMode()
    {
        int[] contractType = Contract.GetBizModes();
        int s = contractType.Length + 1;
        txtBizType.Items.Add(new ListItem("", ""));
        for (int i = 1; i < s; i++)
        {
            txtBizType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(contractType[i - 1])), contractType[i - 1].ToString()));
        }
    }

    //绑定所有业态
    //private void BindAllBiz()
    //{
    //    BaseBO baseBo = new BaseBO();
    //    txtAllBiz.Items.Add(new ListItem("", ""));
    //    baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
    //    Resultset rs = baseBo.Query(new TradeRelation());
    //    foreach (TradeRelation tr in rs)
    //    {
    //        txtAllBiz.Items.Add(new ListItem(tr.TradeName, tr.TradeID.ToString()));
    //    }
    
    //}

    //绑定经营类别
    private void BindBizStyle()
    {
        BaseBO baseBo = new BaseBO();
        txtBizStyle.Items.Add(new ListItem("",""));
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        Resultset rs = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tr in rs)
        {
            txtBizStyle.Items.Add(new ListItem(tr.TradeName, tr.TradeID.ToString()));
        }
    }

    //绑定租约期限
    private void BindLease()
    {
        int[] contractStutas = Contract.GetContractTypeStatus();
        int s = contractStutas.Length + 1;
        txtLease.Items.Add(new ListItem("", ""));
        for (int i = 1; i < s; i++)
        {
            txtLease.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Contract.GetContractTypeStatusDesc(contractStutas[i-1])),contractStutas[i-1].ToString()));
        }
    }

    //绑定数据
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field =new ParameterField[13];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue prv = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_ContractAlert");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXID";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = Session["MallTitle"].ToString();
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        Field[2] = new ParameterField();
        Field[2].Name = "REXStoreDesc";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        Field[3] = new ParameterField();
        Field[3].Name = "REXContractCode";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] = new ParameterField();
        Field[4].Name = "REXCustCode";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = "商户编码";//(String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].Name = "REXCustName";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = "商户名称";//(String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].Name = "RexBizMode";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = "经营方式"; //(String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].Name = "REXContractStratDate";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_ConStartDate");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        Field[8] = new ParameterField();
        Field[8].Name = "REXContractEndDate";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractSumInfo_ConEndDate");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        Field[9] = new ParameterField();
        Field[9].Name = "REXGetArea";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_RentArea");
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        Field[10] = new ParameterField();  //业态
        Field[10].Name = "RexTradeName";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = "业态";//(String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_RentArea");
        Field[10].CurrentValues.Add(DiscreteValue[10]);

        Field[11] = new ParameterField();  //日租金
        Field[11].Name = "RexRentDay";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvDayRent");
        Field[11].CurrentValues.Add(DiscreteValue[11]);


        Field[12] = new ParameterField();
        Field[12].Name = "RexmonthRent";   //月租金
        DiscreteValue[12] = new ParameterDiscreteValue();
        DiscreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_invMonthRent");
        Field[12].CurrentValues.Add(DiscreteValue[12]);

        foreach (ParameterField pt in Field)
        {
            Fields.Add(pt);
        }

        string str_sql="";

          str_sql = "SELECT conshop.shopid," +
                 " store.storename AS StoreDesc," +
                " Contract.ContractCode as ContractCode," +
                " Customer.CustCode as CustCode," +
                " Customer.CustName as CustName,shoptype.shoptypename,floors.floorname," +
                " Case Contract.BizMode when 1 then '租赁' when '2' then '联营' End as BizMode," +
                " traderelation.tradename,round(b.rentamt/conshop.rentarea,2) as rentamt,round((b.rentamt/conshop.rentarea) * 365 /12,2) as monthrent," +
                " Case Contract.ContractStatus when '0' then '初始状态' when '1' then '草稿状态' when '2' then '正常' when '3' then '到期' when '4' then '终止' when '5' then '暂停执行' End as ContractStatus ," +
                " conshop.shopname,Contract.ConStartDate as ConStartDate,Contract.ConEndDate as ConEndDate ,conshop.rentarea as TotalArea " +
                " FROM Contract left Join Conshop On(Contract.Contractid=conshop.Contractid) inner join shoptype on shoptype.shoptypeid=conshop.shoptypeid"+
                " inner join floors on floors.floorid=conshop.floorid"+
                " inner Join Store On(conshop.storeid=store.storeid) inner join traderelation on (traderelation.tradeid=contract.tradeid)" +
                " inner Join  Customer On (Contract.CustId=Customer.CustId)" +
                " inner join (select Contractid, case A.ratetype when 'D' THEN A.rentamt WHEN 'M' THEN A.rentamt * 12 /365 ELSE 0 END as rentamt" +  //根据租金类型生成日至今
                        " from (" +
                        " select  ConFormulaH.formulaid,CONTRACTID,ratetype,(case ConFormulaH.formulatype when 'F' THEN ConFormulaH.fixedrental" + //固定租金
                        " WHEN 'V' THEN (SELECT top 1 ConFormulaM.minsum from ConFormulaM where ConFormulaM.formulaid=ConFormulaH.formulaid ORDER BY ConFormulaM.ConFormulaMID DESC)" +  //抽成租金:会有多条,考虑最后一条
                        " WHEN 'O' THEN	ConFormulaH.fixedrental end) as rentamt" +  //一次性
                        " from ConFormulaH" +
                        " where ConFormulaH.Formulaid in (" +
                            " select max(a.Formulaid) as Formulaid from ConFormulaH a " +
                            " where a.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) group by a.contractid)" +
                        " ) AS A" +
                        ") as b on (b.Contractid=Contract.Contractid)" +
                " WHERE Contract.contractStatus = 2 ";

        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + "AND STORE.STOREID='"+ddlBizproject.SelectedValue+"'";
        }
        //if (txtContractID.Text != "")
        //{
        //    str_sql = str_sql + " AND Contract.ContractCode like '%" + txtContractID.Text.Trim() + "%'";
        //}

        //if (txtCustomerID.Text != "")
        //{
        //    str_sql = str_sql + " AND Customer.CustCode like '%" + txtCustomerID.Text.Trim() + "%'";
        //}

        //if (txtCustName.Text != "")
        //{
        //    str_sql = str_sql + " AND Customer.CustName like '%" + txtCustName.Text.Trim() + "%'";
        //}

        //if (txtBizType.SelectedValue != "")
        //{
        //    str_sql = str_sql + " AND Contract.ContractTypeID= " + int.Parse(txtBizType.SelectedValue.Trim()) + "";
        //}

        //if (txtAllBiz.SelectedValue != "")
        //{
        //    str_sql = str_sql + " AND Contract.TradeID= " + int.Parse(txtAllBiz.SelectedValue.Trim()) + "";
        //}

        //if (txtBizStyle.SelectedValue != "")
        //{
        //    str_sql = str_sql + " AND Contract.TradeID= " + int.Parse(txtBizStyle.SelectedValue.Trim()) + "";
        //}

        //if (txtLease.SelectedValue != "")
        //{
        //    str_sql = str_sql + " AND Contract.ContractStatus= " + int.Parse(txtLease.SelectedValue.Trim()) + "";
        //}

        if (txtLeaseStart.Text != "" && txtLeaseEnd.Text != "")
        {
            str_sql = str_sql + " AND Contract.ConEndDate between '" + txtLeaseStart.Text + "' and '" + txtLeaseEnd.Text + "'";
        }


        str_sql = str_sql + " Order By Contract.ContractCode,StoreDesc";

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\RptContractAlert.rpt";
    }

    protected void BtnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }


    private void ClearPage()
    {
        txtContractID.Text = "";
        txtCustomerID.Text = "";
        txtCustName.Text = "";
        //txtAllBiz.SelectedIndex = 0;
        txtBizStyle.SelectedIndex = 0;
        txtBizType.SelectedIndex = 0;
        txtLease.SelectedIndex = 0;
        txtLeaseStart.Text = "";
        txtLeaseEnd.Text = "";
    }


}
