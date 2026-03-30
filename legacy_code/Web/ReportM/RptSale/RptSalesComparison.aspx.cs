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
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
using Lease.ConShop;

/// <summary>
/// 修改人：hesijian
/// 修改时间：2009年6月16日
/// </summary>

public partial class RptBaseMenu_RptSalesComparison : BasePage
{
    public string baseInfo;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptSalesComparison_Title");
            Fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
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
        return s.Trim() == "" ? "2007-12-25" : s;
    }
    /* 初始化下拉列表



     * 
     * 
     */
    private void InitDDL()
    {
        //绑定店



        BaseBO baseBo = new BaseBO();
        //ddlShopCode.Items.Add(new ListItem("", ""));
        //Resultset rs0 = baseBo.Query(new ConShop());
        //foreach (ConShop conshop in rs0)
        //    ddlShopCode.Items.Add(new ListItem(conshop.ShopName, conshop.ShopCode));
        string sql = "  SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop order by ShopId";
        DataSet myDS = baseBo.QueryDataSet(sql);
        int count = myDS.Tables[0].Rows.Count;
        ddlShopCode.Items.Clear();
        ddlShopCode.Items.Add("");
        for (int i = 0; i < count; i++)
        {
            //绑定商铺号


            ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopID"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        }
        //绑定经营方式
        int[] contractType = Contract.GetBizModes();
        int s = contractType.Length + 1;
        ddlBizMode.Items.Add(new ListItem("", ""));
        for (int i = 1; i < s; i++)
        {
            ddlBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(contractType[i - 1])), contractType[i - 1].ToString()));
        }

        //绑定大楼
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));

        //绑定方位
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        Resultset rs2 = baseBo.Query(new Location());
        ddlLocationName.Items.Add(new ListItem("", ""));
        foreach (Location bd in rs2)
            ddlLocationName.Items.Add(new ListItem(bd.LocationName, bd.LocationID.ToString()));

        

        //绑定一级经营类别



        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        Resultset rs4 = baseBo.Query(new TradeRelation());
        ddlTrade1Name.Items.Add(new ListItem("", ""));
        foreach (TradeRelation tradeDef in rs4)
            ddlTrade1Name.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));

        //绑定二级经营类别
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        ddlTrade2Name.Items.Add(new ListItem("", ""));
        Resultset rs5 = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs5)
            ddlTrade2Name.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    /* 取得表头资源
     * 
     * 
     */ 
    private String GetRptResx()
    {
        String s = "%23Rpt_lblSalesComparison";
        s += "%23" + "RentableArea_lblBuildingName";
        s += "%23" + "RentableArea_lblFloorName";
        s += "%23" + "RentableArea_lblTradeRelation";
        s += "%23" + "LeaseholdContract_labTradeID";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "Rpt_SearcheDate";
        s += "%23" + "Rpt_TotalReceipt";
        s += "%23" + "Rpt_GrossSales";
        s += "%23" + "Rpt_CompareDate";
        s += "%23" + "Rpt_TotalReceipt";
        s += "%23" + "Rpt_GrossSales";
        s += "%23" + "Rpt_SalesBalance";
        s += "%23" + "Rpt_ReceiptUp";
        s += "%23" + "Rpt_SalesBalanceRate";
        s += "%23" + "Rpt_ReceiptUpRate";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        sCon += "%26BizSDate=" + GetdateNull(this.txtBizSDate.Text);
        sCon += "%26BizEDate=" + GetdateNull(this.txtBizEDate.Text);
        sCon += "%26CompDate=" + GetdateNull(this.txtCompDate.Text);
        sCon += "%26" + "CustCode=" + GetStrNull(this.txtCustCode.Text);
        sCon += "%26" + "ShopCode=" + GetStrNull(this.ddlShopCode.Text);
        sCon += "%26" + "BizMode=" + GetStrNull(this.ddlBizMode.Text);
        sCon += "%26" + "BuildingName=" + GetStrNull(this.ddlBuildingName.Text);
        sCon += "%26" + "FloorName=" + GetStrNull(this.ddlFloorName.Text);
        sCon += "%26" + "LocationName=" + GetStrNull(this.ddlLocationName.Text);
        sCon += "%26" + "AreaName=" + GetStrNull(this.ddlAreaName.Text);
        sCon += "%26" + "Trade1Name=" + GetStrNull(this.ddlTrade1Name.Text);
        sCon += "%26" + "Trade2Name=" + GetStrNull(this.ddlTrade2Name.Text);
        return sCon;
    }

    //水晶报表数据绑定
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[19];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[19];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesComparison_Title");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXBuildingName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_BuildingName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXFloorName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_FloorName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXTradeCode";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TradeCode");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXTradeName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_Trade2Name");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXShopCode";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXShopName";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXBizDate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_BizDate");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXaPaidAmt";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXtrNum";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TrNum");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXmaxBizDate";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_BizDate");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXdPaidAmt";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_dPaidAmt");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXTransCnt";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TransCnt");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXDiffAmt";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_DiffAmt");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXDiffCnt";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_DiffCnt");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXpRate";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_pRate");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXCntRate";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CntRate");
        paraField[16].CurrentValues.Add(discreteValue[16]);

        paraField[17] = new ParameterField();
        paraField[17].Name = "REXTotalAmt";
        discreteValue[17] = new ParameterDiscreteValue();
        discreteValue[17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[17].CurrentValues.Add(discreteValue[17]);

        paraField[18] = new ParameterField();
        paraField[18].Name = "REXMallTitle";
        discreteValue[18] = new ParameterDiscreteValue();
        discreteValue[18].Value = Session["MallTitle"].ToString();
        paraField[18].CurrentValues.Add(discreteValue[18]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_where = "";
        if (txtCompDate.Text != "")
        {
            str_where = str_where + " AND TransSku.BizDate ='" + txtCompDate.Text + "'";
        }
        string str_sql = "select BuildingName,FloorName,b.TradeCode,b.TradeName,c.ShopCode,c.ShopName," +
                         "a.BizDate ,sum(a.PaidAmt) aPaidAmt,count(distinct(a.TransID)) trNum, max(d.BizDate) maxBizDate, sum(d.PaidAmt) dPaidAmt, sum(TransCnt) TransCnt," +
                         "sum(a.PaidAmt)-sum(d.PaidAmt) DiffAmt,count(distinct(a.TransID))-sum(TransCnt) DiffCnt," +
                         "case when sum(d.PaidAmt)=0 then 0 else (sum(a.PaidAmt)-sum(d.PaidAmt))/sum(d.PaidAmt) end pRate, " +
                         "case when sum(TransCnt)=0 then 0 else count(distinct(a.TransID))-sum(TransCnt) end CntRate " +
                         "from   TransSku a,TradeRelation b, ConShop c, " +
                         "(select TransSku.BizDate, sum(PaidAmt) PaidAmt ,count(distinct(TransID)) TransCnt from TransSku where 1=1 " + str_where + "group by TransSku.BizDate ) d,Contract e,Customer f" +
                         " where  a.Trade2ID=b.TradeID and a.ShopID=c.ShopID"+
                         " and c.ContractID= e.ContractID and e.CustID = f.CustID" ;

        //条件查询
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND f.CustCode ='" + txtCustCode.Text + "'";
        }
        if (ddlShopCode.Text != "")
        {
            str_sql = str_sql + " AND c.ShopID ='" + ddlShopCode.SelectedItem.Text.Substring(0, ddlShopCode.SelectedItem.Text.IndexOf(" ")) + "'";
        }
        if (ddlBizMode.Text != "")
        {
            str_sql = str_sql + " AND e.BizMode ='" + ddlBizMode.Text + "'";
        }
        if (ddlBuildingName.Text != "")
        {
            str_sql = str_sql + " AND a.BuildingID = '" + ddlBuildingName.Text + "'";
        }
        if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND a.FloorId ='" + ddlFloorName.Text + "'";
        }
        if (ddlLocationName.Text != "")
        {
            str_sql = str_sql + " AND a.LocationId ='" + ddlLocationName.Text + "'";
        }
        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND a.AreaId ='" + ddlAreaName.Text + "'";
        }
        if (ddlTrade1Name.Text != "")
        {
            str_sql = str_sql + " AND e.TradeID ='" + ddlTrade1Name.Text + "'";
        }
        //if (ddlTrade2Name.Text != "")
        //{
        //    str_sql = str_sql + " AND a.Trade2ID ='" + ddlTrade2Name.Text + "'";
        //}
        if (txtBizSDate.Text != "")
        {
            str_sql = str_sql + " AND Convert(Char(10),a.BizDate ,120)>='" + txtBizSDate.Text + "'";
        }
        if (txtBizEDate.Text != "")
        {
            str_sql = str_sql + " AND Convert(Char(10),a.BizDate ,120) <='" + txtBizEDate.Text + "'";
        }

        str_sql = str_sql + "  group  by BuildingName,FloorName,b.TradeCode,b.TradeName,c.ShopCode,c.ShopName,a.BizDate";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\SalesComparison.rpt";

    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptSalesComparison.aspx");
    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //绑定楼层
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = " BuildingID = '" + ddlBuildingName.SelectedValue + "' ";
        Resultset rs1 = baseBo.Query(new Floors());
        ddlFloorName.Items.Clear();
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));

        //绑定区域
        baseBo.WhereClause = " StoreID in (select Storeid from building where buildingid='" + ddlBuildingName.SelectedValue + "') ";
        Resultset rs3 = baseBo.Query(new Area());
        ddlAreaName.Items.Clear();
        ddlAreaName.Items.Add(new ListItem("", ""));
        foreach (Area bd in rs3)
            ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));
    }
}
