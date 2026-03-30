using System;
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
using Base.Util;
using Associator.Perform;

public partial class Associator_GiftStockList:BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            BindGift();
            BindServer();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Associator_GiftStockList");
        }
    }

    //绑定赠品名称
    private void BindGift() 
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Gift());
        txtGiftName.Items.Add(new ListItem("", ""));
        foreach (Gift gift in rs) 
        {
            txtGiftName.Items.Add(new ListItem(gift.GiftDesc, gift.GiftID + ""));
        }
    }

    //绑定服务台
    private void BindServer()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Counter());
        txtServer.Items.Add(new ListItem("", ""));
        foreach (Counter counter in rs)
        {
            txtServer.Items.Add(new ListItem(counter.CounterDesc, counter.CounterID + ""));
        }
    }

    //绑定数据
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field =new ParameterField[11];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[11];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_GiftStockList");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXGiftName";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_GiftName");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        
        Field[2] = new ParameterField();
        Field[2].Name = "REXServerName";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_ServicePaltform");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        Field[3] = new ParameterField();
        Field[3].Name = "REXGiftCnt";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_GiftNumber");
        Field[3].CurrentValues.Add(DiscreteValue[3]);


        Field[4] = new ParameterField();
        Field[4].Name = "REXRefPrice";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_Price");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        
        Field[5] = new ParameterField();
        Field[5].Name = "REXExByBonus";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_YesOrNoExchangeByBonus");
        Field[5].CurrentValues.Add(DiscreteValue[5]);


        Field[6] = new ParameterField();
        Field[6].Name = "REXExByReceipt";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_YesOrNoExchangeByReceipt");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        

        Field[7] = new ParameterField();
        Field[7].Name = "REXBonusCost";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_ExchangeBonusCost");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        
        Field[8] = new ParameterField();
        Field[8].Name = "REXReceiptMoney";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_ExchangeReceiptNumber");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        Field[9] = new ParameterField();
        Field[9].Name = "REXMainTitle";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = Session["MallTitle"].ToString();
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        Field[10] = new ParameterField();
        Field[10].Name = "REXGiftTypeNum";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_GiftTypeNum");
        Field[10].CurrentValues.Add(DiscreteValue[10]);



        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string str_sql = "select Gift.GiftDesc,(select CounterDesc from Counter where Counter.CounterID=GiftStock.CounterID) AS CounterDesc,GiftStock.StockCnt,GiftStock.RefPrice,Gift.ExByBonus,Gift.BonusCost,Gift.ExByReceipt,Gift.ReceiptMoney from Gift INNER JOIN GiftStock ON(GiftStock.GiftID=Gift.GiftID) INNER JOIN Counter ON(Counter.CounterID=GiftStock.CounterID)";

        if (txtGiftName.SelectedItem.Value != "") {
            if (str_sql.Contains("WHERE"))
            {
                str_sql = str_sql + " AND Gift.GiftID= '" + txtGiftName.SelectedValue + "'";
            }
            else
            {
                str_sql = str_sql + " WHERE Gift.GiftID= '" + txtGiftName.SelectedValue + "'";
            }
            
        }

        if (txtServer.SelectedItem.Value != "") {
            if (str_sql.Contains("WHERE"))
            {
                str_sql = str_sql + " AND Counter.CounterID= '" + txtServer.SelectedValue + "'";
            }
            else
            {
                str_sql = str_sql + " WHERE Counter.CounterID= '" + txtServer.SelectedValue + "'";
            }
            
        }
        str_sql = str_sql + " Order By Gift.GiftID ";

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\GiftStockList.rpt";
    }

    protected void btnOK_Click(object sender, EventArgs e) 
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");

    }


    private void ClearPage() 
    {

        txtGiftName.SelectedIndex = 0;
        txtServer.SelectedIndex = 0;

    }


    protected void BtnCel_Click(object sender, EventArgs e) 
    {
        ClearPage();
    }
}
