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

using Base.Page;

/// <summary>
/// 修改人：hesijian
/// 修改时间：2009年6月17日
/// </summary>
public partial class ReportM_RptMember_RptMemberPurhist : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Associator_Purhist");
        }
    }

    //数据绑定
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[12];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[12];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name ="REXMallTitle" ;
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = Session["MallTitle"].ToString();
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_Purhist");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        Field[2] = new ParameterField();
        Field[2].Name = "REXMembCardID";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblAssociatorCard");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        Field[3] = new ParameterField();
        Field[3].Name = "REXMembName";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_MemberName");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] = new ParameterField();
        Field[4].Name = "REXChargeDate";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblConsumeDate");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].Name = "REXShopCode";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblShopCode");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].Name = "REXShopName";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].Name = "REXChargeMoney";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PayInAmt");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        Field[8] = new ParameterField();
        Field[8].Name = "REXBonus";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Tab_integral");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        Field[9] = new ParameterField();
        Field[9].Name = "REXSmallCounter";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSum_Subtotal");
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        Field[10] = new ParameterField();
        Field[10].Name = "REXTotal";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        Field[10].CurrentValues.Add(DiscreteValue[10]);

        Field[11] = new ParameterField();
        Field[11].Name = "REXTransAmount";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_TransAmount");
        Field[11].CurrentValues.Add(DiscreteValue[11]);

        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string strWhere = "";
        string strOrder = "";

        if (txtLCardID.Text != "")
        {
            strWhere += " AND Purhist.MembCardID = '" + txtLCardID.Text.Trim() + "'";
        }
        if (txtLCustID.Text != "")
        {
            strWhere += " AND Purhist.MembID = " + txtLCustID.Text.Trim();
        }
        if (txtShopCode.Text != "")
        {
            strWhere += " AND ConShop.ShopCode = '" + txtShopCode.Text.Trim() + "'";
        }
        if (txtStartDate.Text != "")
        {
            strWhere += " AND Purhist.TransDT >= '" + txtStartDate.Text + " 00:00:00'";
        }
        if (txtEndDate.Text != "")
        {
            strWhere += " AND Purhist.TransDT <= '" + txtEndDate.Text + " 23:59:59'";
        }

        if (Rdo1.Checked)
        {
            strOrder = " Order by Purhist.TransDT";
        }

        if (Rdo2.Checked)
        {
            strOrder = " Order by ConShop.ShopCode";
        }

        if (Rdo3.Checked)
        {
            strOrder = " Order by Purhist.NetAmt";
        }

        if (Rdo4.Checked)
        {
            strOrder = " Order by Purhist.BonusAmt";
        }

        string str_sql = "SELECT Purhist.PurhistID,Purhist.MembCardID,Purhist.MembID,Purhist.ShopID,Purhist.TransID,Purhist.TransDT," +
                           " Purhist.szArtNmbr,Purhist.NetAmt,Purhist.BonusAmt,Purhist.ReceiptID,Purhist.EntryAt,Purhist.EntryBy," +
                           " ConShop.ShopCode,ConShop.ShopName,Member.MemberName" +
                           " FROM Purhist,ConShop,Member" +
                           " WHERE Purhist.ShopID = ConShop.ShopID" +
                           " AND Member.MembID = Purhist.MembID" +
                           " AND 1=1 " + strWhere+strOrder;

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptPurhist.rpt";

    }


    //提交操作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    //清理页面
    private void ClearPage()
    {
        txtLCardID.Text = "";
        txtShopCode.Text = "";
        txtLCustID.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
    
    }

    //撤消操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
