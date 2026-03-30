using System;
using System.Data;
using System.Security;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Configuration;
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
using Base.Util;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;

/// <summary>
/// 编写人：何思键 English Name : Bruce
/// 修改时间：2009年6月17日
/// </summary>

public partial class Report_MemberShopSalesList : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MemberShopSalesList");
            BindBuilding();
            BindFloor();
            BindBizType();
            BindArea();
            txtFirstDate.Text = DateTime.Now.ToShortDateString();
            txtSecondDate.Text = DateTime.Now.ToShortDateString();
        }
    }

    //绑定楼
    private void BindBuilding()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Building());
        txtBuilding.Items.Add(new ListItem("", ""));
        foreach (Building building in rs)
        {
            txtBuilding.Items.Add(new ListItem(building.BuildingName.Trim(), building.BuildingID.ToString()));
        }
    }


    //绑定楼层
    private void BindFloor()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Floors());
        txtFloor.Items.Add(new ListItem("", ""));
        foreach (Floors floor in rs)
        {
            txtFloor.Items.Add(new ListItem(floor.FloorName.Trim(), floor.FloorID.ToString()));
        }
    }


    //绑定经营类型
    private void BindBizType()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new TradeRelation());
        txtBizStyle.Items.Add(new ListItem("", ""));
        foreach (TradeRelation tr in rs)
        {
            txtBizStyle.Items.Add(new ListItem(tr.TradeName.Trim(), tr.TradeID.ToString()));
        }
    }

    //绑定区域
    private void BindArea()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Area());
        txtArea.Items.Add(new ListItem("", ""));
        foreach (Area area in rs)
        {
            txtArea.Items.Add(new ListItem(area.AreaName.Trim(), area.AreaID.ToString()));
        }
    }

    //绑定数据
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[12];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[12];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MemberShopSalesList");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXTrade2Name";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_Trade2Name");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        Field[2] = new ParameterField();
        Field[2].Name = "REXFloorName";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_FloorName");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        Field[3] = new ParameterField();
        Field[3].Name = "REXShopCode";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblShopCode");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] = new ParameterField();
        Field[4].Name = "REXShopName";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].Name = "REXChangeNumber";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TrNum");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].Name = "REXBizBonus";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].Name = "REXMemberChangeNum";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_MemberChangeNum2");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        Field[8] = new ParameterField();
        Field[8].Name = "REXMemberBizBonus";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "MemberChangeSum");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        Field[9] = new ParameterField();
        Field[9].Name = "REXMemberPerBizBonus";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "MemberChangePerSum");
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        Field[10] = new ParameterField();
        Field[10].Name = "REXMemberPerChangeNum";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "MemberChangePerNum");
        Field[10].CurrentValues.Add(DiscreteValue[10]);

        Field[11] = new ParameterField();
        Field[11].Name = "REXMainTitle";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = Session["MallTitle"].ToString();
        Field[11].CurrentValues.Add(DiscreteValue[11]);

        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string str_sql = "";
        string strAnd="";
        string strOrder=" Order By transSku.Trade2ID ,transSku.FloorID,transSku.shopID,conShop.shopCode";

        //条件查询
        if (txtBuilding.SelectedValue != "") {
            strAnd = strAnd + " AND transSku.BuildingID=" + int.Parse(txtBuilding.SelectedValue) + "";
        }

        if (txtFloor.SelectedValue != "") {
            strAnd = strAnd + " AND transSku.FloorID=" + int.Parse(txtFloor.SelectedValue) + "";
        }

        if (txtBizStyle.SelectedValue != "") {
            strAnd = strAnd + " AND transSku.Trade2ID=" + int.Parse(txtBizStyle.SelectedValue) + "";
        }

        if (txtArea.SelectedValue != "") {
            strAnd = strAnd + " AND transSku.AreaID=" + int.Parse(txtArea.SelectedValue) + "";
        }

        if (txtRdoChangeNumber.Checked) {
            strOrder = " Order By receipt1 ";
        }

        if (txtRdoBizBonus.Checked) {
            strOrder =" Order By paidAmt ";
        }

        if (txtRdoMemberChangeNum.Checked) {
            strOrder =" Order By receipt2 ";
        }

        if (txtRdoMemberBizBonus.Checked) {
            strOrder =" Order By custAmt ";
        }

        if (txtRdoGetAll.Checked) {
            strAnd = strAnd + " ";
        }

        if (txtRdoSalePOS.Checked) {
            strAnd = strAnd + " AND transSku.DataSource = 1 ";
        }

        if (txtRdoImportSale.Checked) {
            strAnd = strAnd + " AND transSku.DataSource = 2 ";
        }

        if (txtRdoInput.Checked) {
            strAnd = strAnd + " AND transSku.DataSource = 3 ";
        }

        str_sql = @"SELECT Trans.Trade2ID,Trans.Trade2Name,Trans.FloorID,Trans.FloorName,Trans.                             shopID,conShop.shopCode,Trans.shopName,
                            count(Trans.receipt1) as receipt1,
                            ISNULL(SUM(Trans.paidAmt),0)as paidAmt,
                            count(Pur.receipt2)as receipt2,
                            ISNULL(SUM(Pur.custAmt),0) as custAmt
                    FROM
                        (
	                    SELECT transSku.transID,transSku.Trade2ID,transSku.Trade2Name,
		                       transSku.FloorID,transSku.FloorName,transSku.shopID,transSku.shopName,
		                       ISNULL(SUM(transSku.paidAmt),0) AS paidAmt,
		                       count(distinct transSku.transID) AS receipt1
	                    FROM TransSku 
                        WHERE bizDate BETWEEN '" + txtFirstDate.Text.Trim() + "' AND '" + txtSecondDate.Text.Trim() + "'" + strAnd +
                        @" GROUP BY transSku.transID,transSku.Trade2ID,transSku.Trade2Name,transSku.FloorID,transSku.FloorName,transSku.shopID,transSku.shopName ) AS Trans 
                        INNER JOIN conShop ON (Trans.shopID = conShop.shopID)
                        LEFT JOIN 
	                    (
		                SELECT distinct purhist.transID,
                               ISNULL(SUM(purhist.netAmt),0) AS custAmt,
			                   count(distinct purhist.transID) AS receipt2
		                FROM Purhist
		                WHERE exists
				        (
				        SELECT transid 
                        FROM transsku 
                        WHERE bizDate BETWEEN '" + txtFirstDate.Text.Trim() + "' AND '" + txtSecondDate.Text.Trim() + "' and transsku.TransID = Purhist.TransID " + strAnd +@"
                        ) GROUP BY purhist.transID
                        ) AS Pur 
                        ON (Trans.transID = Pur.transID)
                        group by Trans.Trade2ID,Trans.Trade2Name,Trans.FloorID,Trans.FloorName,Trans.shopID,conShop.shopCode,Trans.shopName"+ strOrder;

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptMemberShopSalesList.rpt";

    }

    //撤消操作
    private void ClearPage()
    {
        txtBuilding.SelectedIndex = 0;
        txtFloor.SelectedIndex = 0;
        txtBizStyle.SelectedIndex = 0;
        txtArea.SelectedIndex = 0;
        txtFirstDate.Text = DateTime.Now.ToShortDateString();
        txtSecondDate.Text = DateTime.Now.ToShortDateString();
        txtRdoChangeNumber.Checked = true;
        txtRdoBizBonus.Checked = false;
        txtRdoMemberChangeNum.Checked = false;
        txtRdoMemberBizBonus.Checked = false;
        txtRdoGetAll.Checked = true;
        txtRdoSalePOS.Checked = false;
        txtRdoImportSale.Checked = false;
        txtRdoInput.Checked = false;
    }


    //查询按钮
    protected void BtnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    //撤消按钮
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
