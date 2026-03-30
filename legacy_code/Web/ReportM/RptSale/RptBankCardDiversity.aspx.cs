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
using Lease.ConShop;
using Base.Util;
using Invoice.InvoiceH;
using Sell;
using Invoice.BankCard;

public partial class ReportM_RptSale_RptBankCardDiversity : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptBankCardDiversitylblTrans");
        }
    }

    /* 判断数据空值,返回默认值

    * 
    * 
    */
    private String GetStrNull(String s)
    {
        return s.Trim() == "" ? "-32766" : s;
    }
    /* 判断日期空值,返回默认值

     * 
     * 
     */
    private String GetdateNull(String s)
    {
        return s.Trim() == "" ? "2007-12-15" : s;
    }
    /* 初始化下拉列表

     * 
     * 
     */
    private void InitDDL()
    {
        //绑定店

        //BaseBO baseBo = new BaseBO();
        //ddlShopCode.Items.Add(new ListItem("", ""));
        //Resultset rs = baseBo.Query(new ConShop());
        //foreach (ConShop conshop in rs)
        //    ddlShopCode.Items.Add(new ListItem(conshop.ShopName, conshop.ShopId.ToString()));

        BaseBO baseBO = new BaseBO();
        string sql = "  SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop order by ShopId";
        DataSet myDS = baseBO.QueryDataSet(sql);
        int count = myDS.Tables[0].Rows.Count;
        ddlShopCode.Items.Clear();
        ddlShopCode.Items.Add("");
        for (int i = 0; i < count; i++)
        {
            //绑定商铺号
            ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        }

        /*绑定经营类型*/
        int[] bizModes = Contract.GetBizModes();
        cmbBizMode.Items.Add(new ListItem("", ""));
        for (int i = 0; i < bizModes.Length; i++)
        {
            cmbBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(bizModes[i])), bizModes[i].ToString()));
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
   
        BindDataSum();

        this.Response.Redirect("../ReportShow.aspx");
    }

    /* 取得表头资源
     * 
     * 
     */
    private String GetRptResx()
    {
        String s = "%23Rpt_lblSalesMediaSum";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "Rpt_MediaMDesc";
        s += "%23" + "Rpt_MediaDesc";
        s += "%23" + "Rpt_GrossSales";
        s += "%23" + "Rpt_ComAmt";
        s += "%23" + "Rpt_NetAmt";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        //sCon += "%26BizSDate=" + GetdateNull(this.txtStartBizTime.Text);
        //sCon += "%26BizEDate=" + GetdateNull(this.txtEndBizTime.Text);
        //sCon += "%26" + "CustCode=" + GetStrNull(this.txtCustCode.Text);
        //sCon += "%26" + "ShopCode=" + GetStrNull(this.ddlShopCode.SelectedValue);
        //sCon += "%26" + "MediaMDesc=" + GetStrNull(this.ddlMediaMDesc.SelectedValue);
        return sCon;
    }

    private void BindDataSum()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[16];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[16];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptBankCardDiversitylblTrans");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustCode";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCode");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXCustName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXBizDate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_BizDate");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXEFTID";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_EFTID");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXCardID";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CardID");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXSdate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXQBDate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = txtStartBizTime.Text;
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXQEDate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = txtEndBizTime.Text;
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXStatus";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXGrossAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXCommChg";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommChg");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXAmount";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXNetAmt";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_NetAmt");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXSubtotal";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSum_Subtotal");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXPrtDate";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtDate");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string trandPOS = "";
        string bank = "";
        string strSql = "";
        string strTableNm = "CardTemp";

        trandPOS="SELECT EFTID,CardId,TransHeader.Bizdate," +
                    " netAmt AS GrossAmt,  CommChg AS CommChg,  netAmt - CommChg AS netAmt,'" + chkTradePos.Text + "' AS status, " +
                    " ABS(netAmt) AS Amt,Customer.CustName,Customer.CustCode  FROM TransHeader" +  
                    " INNER JOIN TransSkuMedia ON (transHeader.transID = transSkuMedia.transID)" + 
                    " INNER JOIN ConShop ON (transSkuMedia.ShopID = Conshop.ShopID)" +    
                    " INNER JOIN Contract ON (Contract.ContractID = ConShop.ContractID)" +
                    " LEFT JOIN Customer ON (Contract.CustID = Customer.CustID)  " +
                    " WHERE TransHeader.BizDate BETWEEN '" + txtStartBizTime.Text +"' AND '" + txtEndBizTime.Text +"' " +
                    " AND EXISTS (SELECT 1 FROM transCard " +
                    " WHERE transHeader.transID = transCard.transID AND transCard.ReconcId = 0 )" +
                    " AND TransSkuMedia.MediaMNo in('401','601','801','901')";


        bank = "SELECT BankTransDet.BankEFTId as EFTId,BankCardId as CardId,BankTransTime as BizDate, BankAmt AS GrossAmt,BankChgAmt AS CommChg " +
                ",BankNetAmt as NetAmt,'" + chkbank.Text + "'  AS status,ABS(BankNetAmt) AS Amt,'' as CustCode,'' as CustName  FROM BankTransDet WHERE ReconcId = 0  " +
                "AND BankTransTime BETWEEN '" + txtStartBizTime.Text + " 00:00:00' AND '" + txtEndBizTime.Text + " 23:59:59'";


        if (cmbBizMode.SelectedValue != "")
        {
            if (Convert.ToInt32(cmbBizMode.SelectedValue) == Contract.BIZ_MODE_LEASE)
            {
                trandPOS = trandPOS + " AND Contract.BizMode =" + Contract.BIZ_MODE_LEASE;
            }
            else if (Convert.ToInt32(cmbBizMode.SelectedValue) == Contract.BIZ_MODE_UNIT)
            {
                trandPOS = trandPOS + " AND Contract.BizMode =" + Contract.BIZ_MODE_UNIT;
            }
        }


        if (chkTradePos.Checked && chkbank.Checked)
        {
            strSql = trandPOS + " UNION ALL " + bank;
        }
        else if (chkTradePos.Checked)
        {
            strSql = trandPOS;
        }
        else if (chkbank.Checked)
        {
            strSql = bank;
        }
        else
        {
            return;
        }


        strSql = "SELECT * FROM (SELECT BizDate,CustCode,CustName," +
                " EFTId,CardID,Status,  SUM(GrossAmt) AS GrossAmt ,Sum(CommChg) AS CommChg,"+
                " SUM(NetAmt) AS NetAmt  FROM (" + strSql + " ) AS " + strTableNm;

        strSql = strSql + " GROUP BY bizDate,eftID,cardID,status,amt,CustName,CustCode) AS a  WHERE GrossAmt <> 0 ";




        //string str_sql = "SELECT CustCode,CustName,SUM(BankAmt) AS BankAmt,SUM(BankChgAmt) AS BankChgAmt," +
        //                    "SUM(BankNetAmt) AS BankNetAmt,Contract.ContractCode,ConShop.ShopCode,ConShop.ShopName FROM BankTransDet LEFT jOIN ConShop ON BankTransDet.ShopID=ConShop.ShopID LEFT JOIN Contract ON " +
        //                    "ConShop.ContractID=Contract.ContractID LEFT JOIN Customer ON Contract.CustID = Customer.CustID WHERE ReconcType=" + BankTransDet.BANKTRANSDET_RECONCTYPE_SUCCEED_ANTITHESES;

        //if (txtCustCode.Text != "")
        //{
        //    str_sql = str_sql + " AND Customer.CustCode ='" + txtCustCode.Text + "'";
        //}

        //if (ddlShopCode.Text != "")
        //{
        //    str_sql = str_sql + " AND ConShop.ShopCode ='" + ddlShopCode.SelectedItem.Text.Substring(0, ddlShopCode.SelectedItem.Text.IndexOf(" ")) + "'";
        //}

        //if (txtStartBizTime.Text != "")
        //{
        //    str_sql = str_sql + " AND BankTransDet.BankTransTime >='" + txtStartBizTime.Text + " 00:00:00'";
        //}

        //if (txtEndBizTime.Text != "")
        //{
        //    str_sql = str_sql + " AND BankTransDet.BankTransTime  <='" + txtEndBizTime.Text + " 23:59:59'";
        //}

        //if (Convert.ToInt32(cmbBizMode.SelectedIndex) != 0)
        //{
        //    str_sql = str_sql + " AND Contract.BizMode =" + cmbBizMode.SelectedValue;
        //}

        //str_sql = str_sql + " GROUP BY CustCode,CustName,Contract.ContractCode,ConShop.ShopCode,ConShop.ShopName";
        Session["paraFil"] = paraFields;
        Session["sql"] = strSql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptBankCardDiversity.rpt";

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {

    }
}
