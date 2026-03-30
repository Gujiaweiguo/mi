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
using Base.Util;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;

/**
 * 
 * 编写人：何思键 ; English Name : bruce
 * 
 * 编写时间:2009年6月17日
 * 
 * 更新类型：Add,Modify(增加及修改)
 * 
 * 
 * **/

public partial class Rpt_MemberShop_BonusList:BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender,EventArgs e) {
        if (!this.IsPostBack) {
            InitData();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MemberShopBonusList");
            txtFirstDate.Text = DateTime.Now.ToShortDateString();
            txtSecondDate.Text = DateTime.Now.ToShortDateString();
            txtFirstNm.Attributes.Add("OnBlur", "return InputValidator(form3)");
            txtSecondNm.Attributes.Add("OnBlur", "return  InputValidator(form3)");
        }
    }


    private void InitData() {

        BaseBO baseBo = new BaseBO();

        //绑定楼
        Resultset rs1 = baseBo.Query(new Building());
        BuildingName.Items.Add(new ListItem("", ""));
        foreach (Building building in rs1) {
            BuildingName.Items.Add(new ListItem(building.BuildingName.Trim(),building.BuildingID.ToString()));
        }

        //绑定楼层
        Resultset rs2 = baseBo.Query(new Floors());
        FloorName.Items.Add(new ListItem("", ""));
        foreach(Floors floor in rs2){
            FloorName.Items.Add(new ListItem(floor.FloorName.Trim(),floor.FloorID.ToString()));
        }

        //绑定经营类型
        Resultset rs3 = baseBo.Query(new TradeRelation());
        BizStyle.Items.Add(new ListItem("", ""));
        foreach (TradeRelation tr in rs3) {
            BizStyle.Items.Add(new ListItem(tr.TradeName.Trim(), tr.TradeID.ToString()));
        }

        //绑定区域
        Resultset rs4 = baseBo.Query(new Area());
        AreaName.Items.Add(new ListItem("", ""));
        foreach (Area area in rs4) {
            AreaName.Items.Add(new ListItem(area.AreaName.Trim(),area.AreaID.ToString()));
        }
    
    
    }

    //绑定资源文件数据
    private void BindData() {

        ParameterFields Fields = new ParameterFields();

            ParameterField[] Field = new ParameterField[15];
            ParameterDiscreteValue[] DisValue = new ParameterDiscreteValue[15];
            ParameterRangeValue pv = new ParameterRangeValue();

            Field[0] = new ParameterField();
            Field[0].Name = "REXTitle";
            DisValue[0] = new ParameterDiscreteValue();
            DisValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MemberShopBonusList");
            Field[0].CurrentValues.Add(DisValue[0]);

            Field[1] = new ParameterField();
            Field[1].Name = "REXShopCode";
            DisValue[1] = new ParameterDiscreteValue();
            DisValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rdo_ShopCode");
            Field[1].CurrentValues.Add(DisValue[1]);

            Field[2] = new ParameterField();
            Field[2].Name = "REXShopName";
            DisValue[2] = new ParameterDiscreteValue();
            DisValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
            Field[2].CurrentValues.Add(DisValue[2]);

        //交易笔数
            Field[3] = new ParameterField();
            Field[3].Name = "REXBizNm";
            DisValue[3] = new ParameterDiscreteValue();
            DisValue[3].Value = (String)GetGlobalResourceObject("BaseInfo","Sale_lblTransNumber");
            Field[3].CurrentValues.Add(DisValue[3]);

        //销售额
            Field[4] = new ParameterField();
            Field[4].Name = "REXSaleNm";
            DisValue[4] = new ParameterDiscreteValue();
            DisValue[4].Value = (String)GetGlobalResourceObject("BaseInfo","ConLease_labSellCount");
            Field[4].CurrentValues.Add(DisValue[4]);


            Field[5] = new ParameterField();
            Field[5].Name = "REXChangeNm";
            DisValue[5] = new ParameterDiscreteValue();
            DisValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_MemberChangeNum");
            Field[5].CurrentValues.Add(DisValue[5]);


            Field[6] = new ParameterField();
            Field[6].Name = "REXPerSale";
            DisValue[6] = new ParameterDiscreteValue();
            DisValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "MemberChangePerNum");
            Field[6].CurrentValues.Add(DisValue[6]);


            Field[7] = new ParameterField();
            Field[7].Name = "REXBiz";
            DisValue[7] = new ParameterDiscreteValue();
            DisValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_MemberChangeSum");
            Field[7].CurrentValues.Add(DisValue[7]);


            Field[8] = new ParameterField();
            Field[8].Name = "REXPerBizNm";
            DisValue[8] = new ParameterDiscreteValue();
            DisValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "MemberChangePerSum");
            Field[8].CurrentValues.Add(DisValue[8]);


            Field[9] = new ParameterField();
            Field[9].Name = "REXGetNm";
            DisValue[9] = new ParameterDiscreteValue();
            DisValue[9].Value = (String)GetGlobalResourceObject("BaseInfo","Tab_integral");
            Field[9].CurrentValues.Add(DisValue[9]);


            Field[10] = new ParameterField();
            Field[10].Name = "REXPerGetNm";
            DisValue[10] = new ParameterDiscreteValue();
            DisValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "PerTotalBonus");
            Field[10].CurrentValues.Add(DisValue[10]);


            Field[11] = new ParameterField();
            Field[11].Name = "REXPerChangeNm";
            DisValue[11] = new ParameterDiscreteValue();
            DisValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "PerTotalChangeNum");
            Field[11].CurrentValues.Add(DisValue[11]);


            Field[12] = new ParameterField();
            Field[12].Name = "REXPerSaleNm";
            DisValue[12] = new ParameterDiscreteValue();
            DisValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "PerTotalCHangeSum");
            Field[12].CurrentValues.Add(DisValue[12]);

            Field[13] = new ParameterField();
            Field[13].Name = "REXMainTitle";
            DisValue[13] = new ParameterDiscreteValue();
            DisValue[13].Value = Session["MallTitle"].ToString();
            Field[13].CurrentValues.Add(DisValue[13]);

            Field[14] = new ParameterField();
            Field[14].Name = "REXTotal";
            DisValue[14] = new ParameterDiscreteValue();
            DisValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
            Field[14].CurrentValues.Add(DisValue[14]);
            

            

        foreach (ParameterField pt in Field) {
                Fields.Add(pt);
            }

            string str_sql = "";
            string strAnd = "";
            string orderBy = " Order By conShop.ShopCode ";
            string strAnd2 = "";
            

            //大楼查询
            if (BuildingName.SelectedItem.Value != "") {
                strAnd = strAnd + " AND transSku.BuildingID = " + int.Parse(BuildingName.SelectedValue.Trim());
            }

            //楼层查询
            if (FloorName.SelectedItem.Value != "") {
                strAnd = strAnd + " AND transSku.FloorID = " + int.Parse(FloorName.SelectedValue.Trim());
            }

            //经营类型查询
            if (BizStyle.SelectedItem.Value != "") {
                strAnd = strAnd + " AND transSku.Trade2ID = " + int.Parse(BizStyle.SelectedValue.Trim());
            }

            //区域名称查询
            if (AreaName.SelectedItem.Value != "") {
                strAnd = strAnd + " AND transSku.AreaID =" + int.Parse(AreaName.SelectedValue.Trim());
            }

            //积分查询
            if (txtFirstNm.Text != "" && txtSecondNm.Text != "") {
                strAnd2 = strAnd2 + " having ISNULL(SUM(purhist.BonusAmt),0) BETWEEN " + int.Parse(txtFirstNm.Text.Trim()) + " AND " + int.Parse(txtSecondNm.Text.Trim()) + "";
            }

            if (txtRdoShopCode.Checked)
            {
                orderBy = " Order By conShop.ShopCode";
            }

            if (txtRdoChangeNm.Checked) {
                orderBy = " Order By receiptNm desc";
            }

            if (txtRdoSaleNm.Checked) {
                orderBy = " Order By custAmt desc";
            }

            if (txtRdoGetNm.Checked) {
                orderBy = " Order By bonusAmt desc";
            }

            if (txtRdoGetAll.Checked)
            {
                strAnd = strAnd + " ";
            }

            if (txtRdoSalePOS.Checked)
            {
                strAnd = strAnd + " AND transSku.DataSource = 1 ";
            }

            if (txtRdoImportSale.Checked)
            {
                strAnd = strAnd + " AND transSku.DataSource = 2 ";
            }

            if (txtRdoInput.Checked)
            {
                strAnd = strAnd + " AND transSku.DataSource = 3 ";
            }
            //AND transSku.shopid=purhist.shopid
            str_sql = @"SELECT transSku.ShopID,conShop.ShopCode,transSku.ShopName,
                            count(distinct transSku.TransID) AS receipt,
                            SUM(transSku.PaidAmt) AS paidAmt,
                            count(distinct purhist.TransID) AS receiptNm,
                            ISNULL(SUM(purhist.NetAmt),0) AS custAmt,
                            ISNULL(SUM(purhist.BonusAmt),0) AS bonusAmt  
                       FROM transSku  
                       INNER JOIN conShop ON (transSku.ShopID = conShop.ShopID) 
                       left JOIN purhist ON (transSku.TransID = purhist.TransID)
                       where transSku.BizDate BETWEEN '" + txtFirstDate.Text.Trim() + "'  And '" + txtSecondDate.Text.Trim() + "'" + strAnd + 
                       @" GROUP BY transSku.shopID,transSku.shopName,conShop.ShopCode"+strAnd2 + orderBy + "";

            Session["paraFil"] = Fields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptMemberShopBonusList.rpt";
    }

    private void ClearPage()
    {
        BuildingName.SelectedIndex = 0;
        FloorName.SelectedIndex = 0;
        BizStyle.SelectedIndex = 0;
        AreaName.SelectedIndex = 0;
        txtFirstNm.Text = "";
        txtSecondNm.Text = "";
        txtFirstDate.Text = DateTime.Now.ToShortDateString();
        txtSecondDate.Text = DateTime.Now.ToShortDateString();
        txtRdoShopCode.Checked = true;
        txtRdoChangeNm.Checked = false;
        txtRdoSaleNm.Checked = false;
        txtRdoGetNm.Checked = false;
        txtRdoGetAll.Checked = true;
        txtRdoSalePOS.Checked = false;
        txtRdoImportSale.Checked = false;
        txtRdoInput.Checked = false;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");

    }

    protected void btnCel_Click(object sender, EventArgs e) 
    {
        ClearPage();

    }




    
}
