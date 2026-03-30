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
using Lease.ConShop;

/// <summary>
/// 修改人：hesijian
/// 修改日期：2009年7月6日
/// </summary>


public partial class ReportM_RptMember_RptShopMemberSales : BasePage 
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtShopCode.Attributes.Add("onclick", "ShowShopTree(LinkButton1)");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopMemberSales");
            btnOK.Attributes.Add("onclick", "return InputValidator(form1)");
        }
    }

    //数据绑定
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[12];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[12];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXStartDate";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = txtStartDate.Text;
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXEndDate";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = txtEndDate.Text;
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXMallTitle";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = Session["MallTitle"].ToString();
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXTitle";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopMemberSales");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXShopCode";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblShopCode");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXShopName";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXTimeDiff";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_Date");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXSales";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "ConLease_labSellCount");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXMemberSales";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptMem_ShopMemberSales");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXPerSales";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_PerSales");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXBonus";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Tab_integral");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXTotal";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string strOrder = "";
        string strWhere = "";
        string strWhere1 = "";

        //排序条件
        if (Rdo1.Checked)
        {
            strOrder = " Order by perAmt";
        
        }
        if (Rdo2.Checked)
        {
            strOrder = " Order by ConShop.ShopCode";
        }

        //查询条件
        if (txtShopCode.Text != "")
        {
            strWhere += " AND Purhist.ShopID = " + Convert.ToInt32(ViewState["shopID"]);
            strWhere1 += " AND TransSku.ShopID = " + Convert.ToInt32(ViewState["shopID"]);
        }
        if (txtStartDate.Text != "")
        {
            strWhere += " AND Convert(varchar(10),TransDT,120) >= '" + txtStartDate.Text + "'";
            strWhere1 += " AND BizDate >= '" + txtStartDate.Text + "'";
        }
        if (txtEndDate.Text != "")
        {
            strWhere += " AND Convert(varchar(10),TransDT,120) <= '" + txtEndDate.Text + "'";
            strWhere1 += " AND BizDate <= '" + txtEndDate.Text + "'";
        }
        string str_sql = "";
        if (rbtnTotal.Checked == true)
        {
            str_sql = "SELECT Purhist.ShopID,PaidAmt," +
                               " SUM(Purhist.NetAmt) AS NetAmt,SUM(Purhist.BonusAmt) AS BonusAmt," +
                               " SUM(Purhist.NetAmt)/TransSku.PaidAmt*100 as perAmt," +
                               " ConShop.ShopCode,ConShop.ShopName" +
                               " FROM Purhist" +
                               " INNER JOIN ConShop ON (Purhist.ShopID = ConShop.ShopID)" +
                               " INNER JOIN " +
                               " (SELECT SUM(PaidAmt) AS PaidAmt,ShopID FROM TransSku WHERE 1=1 " + strWhere1 + "GROUP BY ShopID) AS TransSku" +
                               " ON (TransSku.shopid = Purhist.shopid)" +
                               " WHERE Purhist.ShopID = ConShop.ShopID" +
                               " AND Purhist.ShopID = TransSku.ShopID " + strWhere +
                               " GROUP BY Purhist.ShopID,ConShop.ShopCode,ConShop.ShopName,PaidAmt" + strOrder;
        }
        else
        {
            str_sql = "SELECT Purhist.PurhistID,Purhist.MembCardID,Purhist.MembID,Purhist.ShopID,Purhist.TransID,Purhist.TransDT," +
                               " Purhist.szArtNmbr,SUM(Purhist.NetAmt) AS NetAmt,SUM(Purhist.BonusAmt) AS BonusAmt,Purhist.ReceiptID,Purhist.EntryAt,Purhist.EntryBy," +
                               " ConShop.ShopCode,ConShop.ShopName " +
                               " Member.MembID,Member.MembCode" +
                               " MembCard.MembCardID" +
                               " FROM Purhist,ConShop,Member,MembCard" +
                               " WHERE Purhist.MembCardID = MembCard.MembCardID" +
                               " AND Purhist.ShopID = ConShop.ShopID" +
                               " AND Purhist.MembID = Member.MembID" + strWhere + strOrder;
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptShopMemberSales.rpt";
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            txtShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
        }
    }

    //清除页面
    private void ClearPage()
    {
        txtShopCode.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
    }

    //撤消操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();

    }

    //提交操作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

}
