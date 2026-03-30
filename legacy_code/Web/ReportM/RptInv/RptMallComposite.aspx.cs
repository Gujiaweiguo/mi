using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using System.Text;

using Base.Page;
using Base.Biz;
using Base.DB;
using BaseInfo.Store;

public partial class ReportM_RptInv_RptMallComposite : BasePage
{
    public string pageTitle = "";
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDdl();
            pageTitle = (String)GetGlobalResourceObject("ReportInfo", "RptInv_MallComposite");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
        }
    }

    private void BindDdl()
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "StoreStatus=1";
        baseBo.OrderBy = "orderid";
        Resultset rs = baseBo.Query(new Store());
        ddlStoreName.Items.Add("");
        foreach (Store objStore in rs)
        {
            ddlStoreName.Items.Add(new ListItem(objStore.StoreName, objStore.StoreId.ToString()));
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (this.txtPeriod.Text.Trim().Length == 0)
            return;
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ddlStoreName.SelectedIndex = 0;
        txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
    }

    private void BindData()
    {
        if (this.txtPeriod.Text.Trim() == "")
            this.txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");

        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_MallComposite");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);

        //paraField[2] = new ParameterField();
        //paraField[2].Name = "REXStoreName";
        //discreteValue[2] = new ParameterDiscreteValue();
        //discreteValue[2].Value = this.ddlStoreName.SelectedItem.Text.Trim();//项目名称
        //paraField[2].CurrentValues.Add(discreteValue[2]);

        DateTime dt = DateTime.Parse(this.txtPeriod.Text.Trim());
        paraField[2] = new ParameterField();
        paraField[2].Name = "REXMonth";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = dt.Year.ToString() + "年"+dt.Month.ToString()+"月";//查询月份
        paraField[2].CurrentValues.Add(discreteValue[2]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        
        StringBuilder sb = new StringBuilder();
        sb.Append(@"select store.storename,ConShop.ShopCode,ConShop.ShopName,isnull(TradeRelation.TradeName,'') as tradename,
	                ConShop.RentArea,aa.baseRental,aa.Pcent,case ISNULL(dd.salesamt,0) when 0 then 0 else cc.rentamt/dd.salesamt *100 end                   pcrate ,ISNULL(bb.months,0) months,Contract.ConEndDate
                from ConShop 
                inner join Contract on (Contract.ContractID=ConShop.ContractID)
                inner join store on (conshop.storeid=store.storeid)
                left join TradeRelation on (Contract.TradeID=TradeRelation.TradeID)");
        sb.Append(@"inner join (
	            select contractid,case when FixedRental=0 then isnull(conformulam.minsum,0) else fixedrental end baseRental,
	            ISNULL(ConFormulaP.Pcent,0)*100 Pcent from ConFormulaH 
	            inner join ChargeType on (ChargeType.ChargeTypeID=ConFormulaH.ChargeTypeID and ChargeType.ChargeClass=1)
	            left join conformulam on (conformulam.FormulaID=ConFormulaH.FormulaID)
	            left join ConFormulaP on (ConFormulaP.FormulaID=ConFormulaH.FormulaID)
                where '" + this.txtPeriod.Text.Trim() + "' between FStartDate and FendDate" +
            ") aa on aa.ContractID=Contract.ContractID ");
        sb.Append(@"left join (
                select ContractID,DATEDIFF(M,InvPeriod,GETDATE()) months from InvoiceHeader
                where InvoiceHeader.InvPeriod='"+this.txtPeriod.Text.Trim()+"') bb on bb.ContractID=Contract.ContractID ");
        sb.Append(@"left join (
                select InvoiceHeader.ContractID,SUM(InvoiceDetail.invpayamtl) rentamt from InvoiceDetail 
                inner join InvoiceHeader on (InvoiceHeader.InvID=InvoiceDetail.InvID)
                where InvoiceDetail.ChargeTypeID in (select ChargeTypeID from ChargeType where ChargeClass=1)
                and InvoiceDetail.Period='"+this.txtPeriod.Text.Trim()+"'group by InvoiceHeader.ContractID,InvoiceDetail.Period ) cc on cc.                 ContractID=Contract.ContractID ");
        sb.Append(@"left join (
                select Contract.ContractID,SUM(transshopmth.PayAmt) salesamt from transshopmth
                inner join ConShop on (ConShop.ShopID=TransShopMth.ShopID)
                inner join Contract on (Contract.Contractid=ConShop.Contractid)
                where TransShopMth.Month='"+this.txtPeriod.Text.Trim()+"' group by Contract.ContractID ) dd on dd.ContractID=Contract.                      ContractID  where 1=1");

        if (ddlStoreName.Text != "")
        {
            sb.Append(" and ConShop.StoreID='" + ddlStoreName.SelectedValue + "'");
        }
        sb.Append(" order by ConShop.ShopCode");
        
        Session["paraFil"] = paraFields;
        Session["sql"] = sb.ToString();
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptMallComposite.rpt";
    }
}
