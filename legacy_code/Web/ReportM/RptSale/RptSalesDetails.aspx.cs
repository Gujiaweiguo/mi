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
using BaseInfo.Store;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using Base.Util;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;
using Lease.ConShop;
using BaseInfo.Dept;

public partial class RptBaseMenu_RptSalesDetails : BasePage
{
    public string baseInfo;
    public string sRptName;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtBizEDate.Text = DateTime.Now.ToShortDateString();
            txtBizSDate.Text = DateTime.Now.ToShortDateString();
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblSalesDetails");
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

        //绑定经营方式
        int[] contractType = Contract.GetBizModes();
        int s = contractType.Length + 1;
        ddlBizMode.Items.Add(new ListItem("", ""));
        for (int i = 1; i < s; i++)
        {
            ddlBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(contractType[i - 1])), contractType[i - 1].ToString()));
        }

        //绑定楼



        ////baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //Resultset rs = baseBo.Query(new Building());
        //ddlBuildingName.Items.Add(new ListItem("", ""));
        //foreach (Building bd in rs)
        //    ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));

        ////绑定楼层
        ////baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //Resultset rs1 = baseBo.Query(new Floors());
        //ddlFloorName.Items.Add(new ListItem("", ""));
        //foreach (Floors bd in rs1)
        //    ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));

        ////绑定区域
        ////baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //Resultset rs3 = baseBo.Query(new Area());
        //ddlAreaName.Items.Add(new ListItem("", ""));
        //foreach (Area bd in rs3)
        //    ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));
        
        //绑定商业项目
        //baseBo.WhereClause = "depttype='" + Dept.DEPT_TYPE_MALL + "'";
        //baseBo.OrderBy = "orderid";
        //Resultset rs4 = baseBo.Query(new Dept());
        //ddlStoreName.Items.Add(new ListItem("", ""));
        //foreach (Dept bd in rs4)
            //ddlStoreName.Items.Add(new ListItem(bd.DeptName, bd.DeptID.ToString()));

        baseBo.WhereClause = "StoreStatus=1";
        baseBo.OrderBy = "orderid";
        Resultset rs = baseBo.Query(new Store());
        foreach (Store objStore in rs)
        {
            ddlStoreName.Items.Add(new ListItem(objStore.StoreName, objStore.StoreId.ToString()));
        }

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        sRptName = "SalesDetailsShop.rpt";
        if (this.rdoShopSum.Checked)
            sRptName = "SalesDetailsShopSum.rpt";
        if (this.rdoShopDetail.Checked)
            sRptName = "SalesDetailsShop.rpt";
        if (this.rdoSKUSum.Checked)
            sRptName = "SalesDetailsSKUSum.rpt";
        if (this.rdoSaleDetail.Checked)
            sRptName = "SalesDetailsSKU.rpt";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");

    }

    /* 取得表头资源
     * 
     * 
     */ 
    private String GetRptResx()
    {
        String s = "%23Rpt_lblSalesDetails";
        s += "%23" + "RentableArea_lblBuildingName";
        s += "%23" + "RentableArea_lblFloorName";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "RentableArea_lblAreaName";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "Rpt_SalesDate";
        s += "%23" + "Rpt_TotalReceipt";
        s += "%23" + "Rpt_GrossSales";
        s += "%23" + "Rpt_SkuCode";
        s += "%23" + "Rpt_SkuDesc";
        s += "%23" + "Rpt_SkuPercent";
        s += "%23" + "Rpt_SkuTotalQty";
        s += "%23" + "Rpt_GrossSales";
        s += "%23" + "Rpt_GrossProfit";
        s += "%23" + "Rpt_POSId";
        s += "%23" + "Rpt_TransId";
        s += "%23" + "Rpt_UserID";
        s += "%23" + "Rpt_TransTime";
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
        return sCon;
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        //rdoShopSum
        if (this.rdoShopSum.Checked )
        {
            ParameterField[] paraField = new ParameterField[18];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[18];
            ParameterRangeValue rangeValue = new ParameterRangeValue();
            paraField[0] = new ParameterField();
            paraField[0].ParameterFieldName = "REXTitle";
            discreteValue[0] = new ParameterDiscreteValue();
            discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesDetails_Title") + "(" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_ShopSum") + ")";          
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
            paraField[3].Name = "REXShopCode";
            discreteValue[3] = new ParameterDiscreteValue();
            discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
            paraField[3].CurrentValues.Add(discreteValue[3]);

            paraField[4] = new ParameterField();
            paraField[4].Name = "REXShopName";
            discreteValue[4] = new ParameterDiscreteValue();
            discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
            paraField[4].CurrentValues.Add(discreteValue[4]);

            paraField[5] = new ParameterField();
            paraField[5].Name = "REXPaidAmt";
            discreteValue[5] = new ParameterDiscreteValue();
            discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
            paraField[5].CurrentValues.Add(discreteValue[5]);

            paraField[6] = new ParameterField();
            paraField[6].Name = "REXCostAmt";
            discreteValue[6] = new ParameterDiscreteValue();
            discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CostAmt");
            paraField[6].CurrentValues.Add(discreteValue[6]);

            paraField[7] = new ParameterField();
            paraField[7].Name = "REXPrefAmt";
            discreteValue[7] = new ParameterDiscreteValue();
            discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PrefAmt");
            paraField[7].CurrentValues.Add(discreteValue[7]);

            paraField[8] = new ParameterField();
            paraField[8].Name = "REXPayAmt";
            discreteValue[8] = new ParameterDiscreteValue();
            discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PayAmt");
            paraField[8].CurrentValues.Add(discreteValue[8]);

            paraField[9] = new ParameterField();
            paraField[9].Name = "REXQty";
            discreteValue[9] = new ParameterDiscreteValue();
            discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TrNum");
            paraField[9].CurrentValues.Add(discreteValue[9]);

            paraField[10] = new ParameterField();
            paraField[10].Name = "REXTotalAmt";
            discreteValue[10] = new ParameterDiscreteValue();
            discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
            paraField[10].CurrentValues.Add(discreteValue[10]);

            paraField[11] = new ParameterField();
            paraField[11].Name = "REXProfitAmt";
            discreteValue[11] = new ParameterDiscreteValue();
            discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_ProfitAmt");
            paraField[11].CurrentValues.Add(discreteValue[11]);

            paraField[12] = new ParameterField();
            paraField[12].Name = "REXSdate";
            discreteValue[12] = new ParameterDiscreteValue();
            discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
            paraField[12].CurrentValues.Add(discreteValue[12]);

            paraField[13] = new ParameterField();
            paraField[13].Name = "REXQBDate";
            discreteValue[13] = new ParameterDiscreteValue();
            discreteValue[13].Value = txtBizSDate.Text;
            paraField[13].CurrentValues.Add(discreteValue[13]);

            paraField[14] = new ParameterField();
            paraField[14].Name = "REXQEDate";
            discreteValue[14] = new ParameterDiscreteValue();
            discreteValue[14].Value = txtBizEDate.Text;
            paraField[14].CurrentValues.Add(discreteValue[14]);

            paraField[15] = new ParameterField();
            paraField[15].Name = "REXMallTitle";
            discreteValue[15] = new ParameterDiscreteValue();
            discreteValue[15].Value = Session["MallTitle"].ToString();
            paraField[15].CurrentValues.Add(discreteValue[15]);

            paraField[16] = new ParameterField();
            paraField[16].Name = "REXStoreId";
            discreteValue[16] = new ParameterDiscreteValue();
            discreteValue[16].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_ItemCode");
            paraField[16].CurrentValues.Add(discreteValue[16]);

            paraField[17] = new ParameterField();
            paraField[17].Name = "REXStoreDesc";
            discreteValue[17] = new ParameterDiscreteValue();
            discreteValue[17].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");
            paraField[17].CurrentValues.Add(discreteValue[17]);



            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }

        }
        //rdoShopDetail
        if (this.rdoShopDetail.Checked)
        {
            ParameterField[] paraField = new ParameterField[32];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[32];
            ParameterRangeValue rangeValue = new ParameterRangeValue();
            paraField[0] = new ParameterField();
            paraField[0].ParameterFieldName = "REXTitle";
            discreteValue[0] = new ParameterDiscreteValue();
            discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesDetails_Title") + "(" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_ShopDetail") + ")";
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
            paraField[3].Name = "REXShopCode";
            discreteValue[3] = new ParameterDiscreteValue();
            discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
            paraField[3].CurrentValues.Add(discreteValue[3]);

            paraField[4] = new ParameterField();
            paraField[4].Name = "REXShopName";
            discreteValue[4] = new ParameterDiscreteValue();
            discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
            paraField[4].CurrentValues.Add(discreteValue[4]);

            paraField[5] = new ParameterField();
            paraField[5].Name = "REXPaidAmt";
            discreteValue[5] = new ParameterDiscreteValue();
            discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
            paraField[5].CurrentValues.Add(discreteValue[5]);


            paraField[6] = new ParameterField();
            paraField[6].Name = "REXCostAmt";
            discreteValue[6] = new ParameterDiscreteValue();
            discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CostAmt");
            paraField[6].CurrentValues.Add(discreteValue[6]);

            paraField[7] = new ParameterField();
            paraField[7].Name = "REXPrefAmt";
            discreteValue[7] = new ParameterDiscreteValue();
            discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PrefAmt");
            paraField[7].CurrentValues.Add(discreteValue[7]);

            paraField[8] = new ParameterField();
            paraField[8].Name = "REXPayAmt";
            discreteValue[8] = new ParameterDiscreteValue();
            discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PayAmt");
            paraField[8].CurrentValues.Add(discreteValue[8]);

            paraField[9] = new ParameterField();
            paraField[9].Name = "REXQty";
            discreteValue[9] = new ParameterDiscreteValue();
            discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TrNum");
            paraField[9].CurrentValues.Add(discreteValue[9]);

            paraField[10] = new ParameterField();
            paraField[10].Name = "REXTotalAmt";
            discreteValue[10] = new ParameterDiscreteValue();
            discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
            paraField[10].CurrentValues.Add(discreteValue[10]);

            paraField[11] = new ParameterField();
            paraField[11].Name = "REXProfitAmt";
            discreteValue[11] = new ParameterDiscreteValue();
            discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_ProfitAmt");
            paraField[11].CurrentValues.Add(discreteValue[11]);

            paraField[12] = new ParameterField();
            paraField[12].Name = "REXPosID";
            discreteValue[12] = new ParameterDiscreteValue();
            discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PosID");
            paraField[12].CurrentValues.Add(discreteValue[12]);

            paraField[13] = new ParameterField();
            paraField[13].Name = "REXUserID";
            discreteValue[13] = new ParameterDiscreteValue();
            discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_UserID");
            paraField[13].CurrentValues.Add(discreteValue[13]);

            paraField[14] = new ParameterField();
            paraField[14].Name = "REXTransTime";
            discreteValue[14] = new ParameterDiscreteValue();
            discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TransTime");
            paraField[14].CurrentValues.Add(discreteValue[14]);

            paraField[15] = new ParameterField();
            paraField[15].Name = "REXReceiptID";
            discreteValue[15] = new ParameterDiscreteValue();
            discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_ReceiptID");
            paraField[15].CurrentValues.Add(discreteValue[15]);

            paraField[16] = new ParameterField();
            paraField[16].Name = "REXSkuID";
            discreteValue[16] = new ParameterDiscreteValue();
            discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_SkuID");
            paraField[16].CurrentValues.Add(discreteValue[16]);

            paraField[17] = new ParameterField();
            paraField[17].Name = "REXSkuDesc";
            discreteValue[17] = new ParameterDiscreteValue();
            discreteValue[17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_SkuDesc");
            paraField[17].CurrentValues.Add(discreteValue[17]);

            paraField[18] = new ParameterField();
            paraField[18].Name = "REXOrgPrice";
            discreteValue[18] = new ParameterDiscreteValue();
            discreteValue[18].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_OrgPrice");
            paraField[18].CurrentValues.Add(discreteValue[18]);

            paraField[19] = new ParameterField();
            paraField[19].Name = "REXNewPrice";
            discreteValue[19] = new ParameterDiscreteValue();
            discreteValue[19].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_NewPrice");
            paraField[19].CurrentValues.Add(discreteValue[19]);

            paraField[20] = new ParameterField();
            paraField[20].Name = "REXItemDisc";
            discreteValue[20] = new ParameterDiscreteValue();
            discreteValue[20].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_ItemDisc");
            paraField[20].CurrentValues.Add(discreteValue[20]);

            paraField[21] = new ParameterField();
            paraField[21].Name = "REXAllocDisc";
            discreteValue[21] = new ParameterDiscreteValue();
            discreteValue[21].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_AllocDisc");
            paraField[21].CurrentValues.Add(discreteValue[21]);

            paraField[22] = new ParameterField();
            paraField[22].Name = "REXTax";
            discreteValue[22] = new ParameterDiscreteValue();
            discreteValue[22].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_Tax");
            paraField[22].CurrentValues.Add(discreteValue[22]);

            paraField[23] = new ParameterField();
            paraField[23].Name = "REXDiscRate";
            discreteValue[23] = new ParameterDiscreteValue();
            discreteValue[23].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_DiscRate");
            paraField[23].CurrentValues.Add(discreteValue[23]);

            paraField[24] = new ParameterField();
            paraField[24].Name = "REXTotalAmt";
            discreteValue[24] = new ParameterDiscreteValue();
            discreteValue[24].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
            paraField[24].CurrentValues.Add(discreteValue[24]);


            paraField[25] = new ParameterField();
            paraField[25].Name = "REXSdate";
            discreteValue[25] = new ParameterDiscreteValue();
            discreteValue[25].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
            paraField[25].CurrentValues.Add(discreteValue[25]);

            paraField[26] = new ParameterField();
            paraField[26].Name = "REXQBDate";
            discreteValue[26] = new ParameterDiscreteValue();
            discreteValue[26].Value = txtBizSDate.Text;
            paraField[26].CurrentValues.Add(discreteValue[26]);

            paraField[27] = new ParameterField();
            paraField[27].Name = "REXQEDate";
            discreteValue[27] = new ParameterDiscreteValue();
            discreteValue[27].Value = txtBizEDate.Text;
            paraField[27].CurrentValues.Add(discreteValue[27]);

            paraField[28] = new ParameterField();
            paraField[28].Name = "REXSubtotal";
            discreteValue[28] = new ParameterDiscreteValue();
            discreteValue[28].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSum_Subtotal");
            paraField[28].CurrentValues.Add(discreteValue[28]);

            paraField[29] = new ParameterField();
            paraField[29].Name = "REXMallTitle";
            discreteValue[29] = new ParameterDiscreteValue();
            discreteValue[29].Value = Session["MallTitle"].ToString();
            paraField[29].CurrentValues.Add(discreteValue[29]);

            paraField[30] = new ParameterField();
            paraField[30].Name = "REXStoreId";
            discreteValue[30] = new ParameterDiscreteValue();
            discreteValue[30].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_ItemCode");
            paraField[30].CurrentValues.Add(discreteValue[30]);

            paraField[31] = new ParameterField();
            paraField[31].Name = "REXStoreDesc";
            discreteValue[31] = new ParameterDiscreteValue();
            discreteValue[31].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");
            paraField[31].CurrentValues.Add(discreteValue[31]);

            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }

        }
        //rdoSKUSum
        if (this.rdoSKUSum.Checked )
        {
            ParameterField[] paraField = new ParameterField[19];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[19];
            ParameterRangeValue rangeValue = new ParameterRangeValue();
            paraField[0] = new ParameterField();
            paraField[0].ParameterFieldName = "REXTitle";
            discreteValue[0] = new ParameterDiscreteValue();
            discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesDetails_Title") + "(" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_SKUSum") + ")";
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
            paraField[3].Name = "REXShopCode";
            discreteValue[3] = new ParameterDiscreteValue();
            discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
            paraField[3].CurrentValues.Add(discreteValue[3]);

            paraField[4] = new ParameterField();
            paraField[4].Name = "REXShopName";
            discreteValue[4] = new ParameterDiscreteValue();
            discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
            paraField[4].CurrentValues.Add(discreteValue[4]);

            paraField[5] = new ParameterField();
            paraField[5].Name = "REXPaidAmt";
            discreteValue[5] = new ParameterDiscreteValue();
            discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
            paraField[5].CurrentValues.Add(discreteValue[5]);

            paraField[6] = new ParameterField();
            paraField[6].Name = "REXSkuId";
            discreteValue[6] = new ParameterDiscreteValue();
            discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_SkuId");
            paraField[6].CurrentValues.Add(discreteValue[6]);

            paraField[7] = new ParameterField();
            paraField[7].Name = "REXSkuDesc";
            discreteValue[7] = new ParameterDiscreteValue();
            discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_SkuDesc");
            paraField[7].CurrentValues.Add(discreteValue[7]);

            paraField[8] = new ParameterField();
            paraField[8].Name = "REXNetAmt";
            discreteValue[8] = new ParameterDiscreteValue();
            discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_NetAmt");
            paraField[8].CurrentValues.Add(discreteValue[8]);

            paraField[9] = new ParameterField();
            paraField[9].Name = "REXQty";
            discreteValue[9] = new ParameterDiscreteValue();
            discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TrNum");
            paraField[9].CurrentValues.Add(discreteValue[9]);

            paraField[10] = new ParameterField();
            paraField[10].Name = "REXTotalAmt";
            discreteValue[10] = new ParameterDiscreteValue();
            discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
            paraField[10].CurrentValues.Add(discreteValue[10]);

            paraField[11] = new ParameterField();
            paraField[11].Name = "REXCommChg";
            discreteValue[11] = new ParameterDiscreteValue();
            discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommChg");
            paraField[11].CurrentValues.Add(discreteValue[11]);

            paraField[12] = new ParameterField();
            paraField[12].Name = "REXSdate";
            discreteValue[12] = new ParameterDiscreteValue();
            discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
            paraField[12].CurrentValues.Add(discreteValue[12]);

            paraField[13] = new ParameterField();
            paraField[13].Name = "REXQBDate";
            discreteValue[13] = new ParameterDiscreteValue();
            discreteValue[13].Value = txtBizSDate.Text;
            paraField[13].CurrentValues.Add(discreteValue[13]);

            paraField[14] = new ParameterField();
            paraField[14].Name = "REXQEDate";
            discreteValue[14] = new ParameterDiscreteValue();
            discreteValue[14].Value = txtBizEDate.Text;
            paraField[14].CurrentValues.Add(discreteValue[14]);

            paraField[15] = new ParameterField();
            paraField[15].Name = "REXSubtotal";
            discreteValue[15] = new ParameterDiscreteValue();
            discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSum_Subtotal");
            paraField[15].CurrentValues.Add(discreteValue[15]);

            paraField[16] = new ParameterField();
            paraField[16].Name = "REXMallTitle";
            discreteValue[16] = new ParameterDiscreteValue();
            discreteValue[16].Value = Session["MallTitle"].ToString();
            paraField[16].CurrentValues.Add(discreteValue[16]);

            paraField[17] = new ParameterField();
            paraField[17].Name = "REXStoreId";
            discreteValue[17] = new ParameterDiscreteValue();
            discreteValue[17].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_ItemCode");
            paraField[17].CurrentValues.Add(discreteValue[17]);

            paraField[18] = new ParameterField();
            paraField[18].Name = "REXStoreDesc";
            discreteValue[18] = new ParameterDiscreteValue();
            discreteValue[18].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");
            paraField[18].CurrentValues.Add(discreteValue[18]);

            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }
        }
        //rdoSaleDetail
        if (this.rdoSaleDetail.Checked)
        {
            ParameterField[] paraField = new ParameterField[27];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[27];
            ParameterRangeValue rangeValue = new ParameterRangeValue();
            paraField[0] = new ParameterField();
            paraField[0].ParameterFieldName = "REXTitle";
            discreteValue[0] = new ParameterDiscreteValue();
            discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesDetails_Title") + "(" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_SaleDetail") + ")";
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
            paraField[3].Name = "REXShopCode";
            discreteValue[3] = new ParameterDiscreteValue();
            discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
            paraField[3].CurrentValues.Add(discreteValue[3]);

            paraField[4] = new ParameterField();
            paraField[4].Name = "REXShopName";
            discreteValue[4] = new ParameterDiscreteValue();
            discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
            paraField[4].CurrentValues.Add(discreteValue[4]);

            paraField[5] = new ParameterField();
            paraField[5].Name = "REXPaidAmt";
            discreteValue[5] = new ParameterDiscreteValue();
            discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
            paraField[5].CurrentValues.Add(discreteValue[5]);

            paraField[6] = new ParameterField();
            paraField[6].Name = "REXSkuId";
            discreteValue[6] = new ParameterDiscreteValue();
            discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_SkuId");
            paraField[6].CurrentValues.Add(discreteValue[6]);

            paraField[7] = new ParameterField();
            paraField[7].Name = "REXSkuDesc";
            discreteValue[7] = new ParameterDiscreteValue();
            discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_SkuDesc");
            paraField[7].CurrentValues.Add(discreteValue[7]);

            paraField[8] = new ParameterField();
            paraField[8].Name = "REXCommChg";
            discreteValue[8] = new ParameterDiscreteValue();
            discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommChg");
            paraField[8].CurrentValues.Add(discreteValue[8]);

            paraField[9] = new ParameterField();
            paraField[9].Name = "REXNetAmt";
            discreteValue[9] = new ParameterDiscreteValue();
            discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_NetAmt");
            paraField[9].CurrentValues.Add(discreteValue[9]);

            paraField[10] = new ParameterField();
            paraField[10].Name = "REXTotalAmt";
            discreteValue[10] = new ParameterDiscreteValue();
            discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
            paraField[10].CurrentValues.Add(discreteValue[10]);

            paraField[11] = new ParameterField();
            paraField[11].Name = "REXCommRate";
            discreteValue[11] = new ParameterDiscreteValue();
            discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommRate");
            paraField[11].CurrentValues.Add(discreteValue[11]);

            paraField[12] = new ParameterField();
            paraField[12].Name = "REXPosID";
            discreteValue[12] = new ParameterDiscreteValue();
            discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PosID");
            paraField[12].CurrentValues.Add(discreteValue[12]);

            paraField[13] = new ParameterField();
            paraField[13].Name = "REXUserID";
            discreteValue[13] = new ParameterDiscreteValue();
            discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_UserID");
            paraField[13].CurrentValues.Add(discreteValue[13]);

            paraField[14] = new ParameterField();
            paraField[14].Name = "REXTransTime";
            discreteValue[14] = new ParameterDiscreteValue();
            discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TransTime");
            paraField[14].CurrentValues.Add(discreteValue[14]);

            paraField[15] = new ParameterField();
            paraField[15].Name = "REXReceiptID";
            discreteValue[15] = new ParameterDiscreteValue();
            discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_ReceiptID");
            paraField[15].CurrentValues.Add(discreteValue[15]);

            paraField[16] = new ParameterField();
            paraField[16].Name = "REXCardID";
            discreteValue[16] = new ParameterDiscreteValue();
            discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CardID");
            paraField[16].CurrentValues.Add(discreteValue[16]);

            paraField[17] = new ParameterField();
            paraField[17].Name = "REXEFTID";
            discreteValue[17] = new ParameterDiscreteValue();
            discreteValue[17].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_EFTID");
            paraField[17].CurrentValues.Add(discreteValue[17]);

            paraField[18] = new ParameterField();
            paraField[18].Name = "REXMediaMNo";
            discreteValue[18] = new ParameterDiscreteValue();
            discreteValue[18].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_MediaMNo");
            paraField[18].CurrentValues.Add(discreteValue[18]);

            paraField[19] = new ParameterField();
            paraField[19].Name = "REXMediaMDesc";
            discreteValue[19] = new ParameterDiscreteValue();
            discreteValue[19].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_MediaMDesc");
            paraField[19].CurrentValues.Add(discreteValue[19]);

            paraField[20] = new ParameterField();
            paraField[20].Name = "REXSdate";
            discreteValue[20] = new ParameterDiscreteValue();
            discreteValue[20].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
            paraField[20].CurrentValues.Add(discreteValue[20]);

            paraField[21] = new ParameterField();
            paraField[21].Name = "REXQBDate";
            discreteValue[21] = new ParameterDiscreteValue();
            discreteValue[21].Value = txtBizSDate.Text;
            paraField[21].CurrentValues.Add(discreteValue[21]);

            paraField[22] = new ParameterField();
            paraField[22].Name = "REXQEDate";
            discreteValue[22] = new ParameterDiscreteValue();
            discreteValue[22].Value = txtBizEDate.Text;
            paraField[22].CurrentValues.Add(discreteValue[22]);

            paraField[23] = new ParameterField();
            paraField[23].Name = "REXSubtotal";
            discreteValue[23] = new ParameterDiscreteValue();
            discreteValue[23].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSum_Subtotal");
            paraField[23].CurrentValues.Add(discreteValue[23]);

            paraField[24] = new ParameterField();
            paraField[24].Name = "REXMallTitle";
            discreteValue[24] = new ParameterDiscreteValue();
            discreteValue[24].Value = Session["MallTitle"].ToString();
            paraField[24].CurrentValues.Add(discreteValue[24]);

            paraField[25] = new ParameterField();
            paraField[25].Name = "REXStoreId";
            discreteValue[25] = new ParameterDiscreteValue();
            discreteValue[25].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_ItemCode");
            paraField[25].CurrentValues.Add(discreteValue[25]);

            paraField[26] = new ParameterField();
            paraField[26].Name = "REXStoreDesc";
            discreteValue[26] = new ParameterDiscreteValue();
            discreteValue[26].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");
            paraField[26].CurrentValues.Add(discreteValue[26]);

            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }
        }

        string str_sql = "";
        if (this.rdoShopSum.Checked)
        {
            str_sql = str_sql + "select ConShop.StoreID,(SELECT store.StoreShortName FROM store WHERE conShop.StoreID = Store.StoreID) AS StoreDesc, Customer.CustCode,Customer.CustName,ConShop.ShopCode,ConShop.ShopId,ConShop.ShopName,Count(Distinct TransSku.TransID) as Qty,Sum(PayAmt)  as PayAmt,Sum(PrefAmt)  as PrefAmt,Sum(ProfitAmt)  as ProfitAmt,Sum(CostAmt)  as CostAmt,Sum(PaidAmt)  as PaidAmt" +
                                " from TransSku,ConShop ,Contract ,Customer" +
                                " where TransSku.ShopId=ConShop.ShopId" +
                                " And ConShop.ContractId=Contract.ContractId" +
                                " And Contract.CustId=Customer.CustId";
            if (RB1.Checked)
            {

                str_sql = str_sql + " ";
            }
            if (RB2.Checked)
            {

                str_sql = str_sql + " AND TransSku.datasource=1 ";
            }
            if (RB3.Checked)
            {

                str_sql = str_sql + " AND TransSku.datasource=2 ";
            }
            if (RB4.Checked)
            {

                str_sql = str_sql + " AND TransSku.datasource=3 ";
            }
        }
                if (this.rdoShopDetail.Checked)
                {
                    str_sql = str_sql + "select ConShop.StoreID,(SELECT store.StoreShortName FROM store WHERE conShop.StoreID = Store.StoreID) AS StoreDesc, Customer.CustCode,Customer.CustName,ConShop.ShopCode,ConShop.ShopId,ConShop.ShopName,PosID,UserID,BizDate,TransTime,ReceiptID,SkuID,SkuDesc,Qty,OrgPrice,NewPrice,ItemDisc,AllocDisc,PayAmt,Tax,PrefAmt,PaidAmt,DiscRate,CostAmt,ProfitAmt" +
                                        " from TransSku,ConShop ,Contract ,Customer" +
                                        " where TransSku.ShopId=ConShop.ShopId" +
                                        " And ConShop.ContractId=Contract.ContractId" +
                                        " And Contract.CustId=Customer.CustId";

                    if (RB1.Checked)
                    {

                        str_sql = str_sql + "";
                    }
                    if (RB2.Checked)
                    {

                        str_sql = str_sql + " AND TransSku.datasource=1 ";
                    }
                    if (RB3.Checked)
                    {

                        str_sql = str_sql + " AND TransSku.datasource=2 ";
                    }
                    if (RB4.Checked)
                    {

                        str_sql = str_sql + " AND TransSku.datasource=3 ";
                    }
                }
                if (this.rdoSKUSum.Checked)
                {
                    str_sql = str_sql + "select  ConShop.StoreID,(SELECT store.StoreShortName FROM store WHERE conShop.StoreID = Store.StoreID) AS StoreDesc, Customer.CustCode,Customer.CustName,ConShop.ShopCode,ConShop.ShopId,ConShop.ShopName,TransSkuMedia.SkuId,TransSkuMedia.SkuDesc,Count(Distinct TransId) as Qty,Sum( PaidAmt) as PaidAmt,Sum( CommRate) as CommRate,Sum( CommChg) as CommChg,Sum( NetAmt) as NetAmt " +
                                        " from TransSkuMedia,ConShop ,Contract ,Customer" +
                                        " where TransSkuMedia.ShopId=ConShop.ShopId" +
                                        " And ConShop.ContractId=Contract.ContractId" +
                                        " And Contract.CustId=Customer.CustId";
                    if (RB1.Checked)
                    {
                        str_sql = str_sql + " ";

                    }
                    if (RB2.Checked)
                    {
                        str_sql = str_sql + " AND TransSkuMedia.datasource=1 ";

                    }
                    if (RB3.Checked)
                    {
                        str_sql = str_sql + " AND TransSkuMedia.datasource=2 ";

                    }
                    if (RB4.Checked)
                    {
                        str_sql = str_sql + " AND TransSkuMedia.datasource=3 ";

                    }
                }
                if (this.rdoSaleDetail.Checked)
                {
                    str_sql = str_sql + "select ConShop.StoreID,(SELECT store.StoreShortName FROM store WHERE conShop.StoreID = Store.StoreID) AS StoreDesc, Customer.CustCode,Customer.CustName,ConShop.ShopCode,ConShop.ShopId,ConShop.ShopName,PosID,UserID,BizDate,TransTime,ReceiptID,CardID,EFTID,MediaMNo,MediaMDesc,TransSkuMedia.SkuId,TransSkuMedia.SkuDesc,PaidAmt,CommRate,CommChg,NetAmt " +
                                        " from TransSkuMedia,ConShop ,Contract ,Customer" +
                                        " where TransSkuMedia.ShopId=ConShop.ShopId" +
                                        " And ConShop.ContractId=Contract.ContractId" +
                                        " And Contract.CustId=Customer.CustId ";
                    if (RB1.Checked)
                    {
                        str_sql = str_sql + "";

                    }
                    if (RB2.Checked)
                    {
                        str_sql = str_sql + " AND TransSkuMedia.datasource=1 ";

                    }
                    if (RB3.Checked)
                    {
                        str_sql = str_sql + " AND TransSkuMedia.datasource=2 ";

                    }
                    if (RB4.Checked)
                    {
                        str_sql = str_sql + " AND TransSkuMedia.datasource=3 ";

                    }
                }
        if (ddlStoreName.Text != "")
        {
            str_sql = str_sql + " AND ConShop.storeID  = '" +ddlStoreName.SelectedValue + "' ";
        }
        if (ddlBuildingName.Text != "")
        {
            str_sql = str_sql + " AND ConShop.BuildingID = '" + ddlBuildingName.SelectedValue + "' ";
        }
        if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND ConShop.FloorId = '" + ddlFloorName.SelectedValue + "' ";
        }
        if (ddlBizMode.Text != "")
        {
            str_sql = str_sql + " AND Contract.BizMode = '" + ddlBizMode.Text + "' ";
        }
        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND ConShop.AreaId = '" + ddlAreaName.Text + "' ";
        }
        if (ddlShopCode.Text!="")
        {
            str_sql = str_sql + " AND ConShop.ShopID = '" + ddlShopCode.SelectedValue + "' ";
        }

        if (txtBizSDate.Text != "")
        {
            if (this.rdoSKUSum.Checked || this.rdoSaleDetail.Checked)
                str_sql = str_sql + " AND TransSkuMedia.BizDate >= '" + txtBizSDate.Text + "'  ";
            if (this.rdoShopSum.Checked || this.rdoShopDetail.Checked)
                str_sql = str_sql + " AND TransSku.BizDate >= '" + txtBizSDate.Text + "' ";
        }
        if (txtBizEDate.Text != "")
        {
            if (this.rdoSKUSum.Checked || this.rdoSaleDetail.Checked)
                str_sql = str_sql + " AND TransSkuMedia.BizDate <= '" + txtBizEDate.Text + "' ";
            if (this.rdoShopSum.Checked || this.rdoShopDetail.Checked)
                str_sql = str_sql + " AND TransSku.BizDate<= '" + txtBizEDate.Text + "' ";
        }
       

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if(AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        if (this.rdoShopSum.Checked)
            str_sql = str_sql + " group by ConShop.StoreId,Customer.CustCode,Customer.CustName,ConShop.ShopCode,ConShop.ShopId,ConShop.ShopName Order by ConShop.StoreID,Customer.CustCode,ConShop.ShopId ";
        if (this.rdoShopDetail.Checked)
            str_sql = str_sql + " Order by ConShop.StoreID,Customer.CustCode,ConShop.ShopId ";
        if (this.rdoSKUSum.Checked)
            str_sql = str_sql + " group by  ConShop.StoreId,Customer.CustCode,Customer.CustName,ConShop.ShopCode,ConShop.ShopId,ConShop.ShopName,TransSkuMedia.SkuId,TransSkuMedia.SkuDesc Order by ConShop.StoreID,Customer.CustCode,ConShop.ShopId";
        if (this.rdoSaleDetail.Checked)
            str_sql = str_sql + " Order by ConShop.StoreID,Customer.CustCode,ConShop.ShopId ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\" + sRptName;

    }

    protected void pageclear()
    {
        ddlStoreName.SelectedValue = "";
        ddlBuildingName.SelectedValue = "";
        ddlFloorName.SelectedValue = "";
        ddlBizMode.SelectedValue = "";
        ddlAreaName.SelectedValue = "";
        ddlShopCode.SelectedValue = "";
        txtBizEDate.Text = DateTime.Now.ToShortDateString();
        txtBizSDate.Text = DateTime.Now.ToShortDateString();
        rdoShopSum.Checked = true;
        rdoShopDetail.Checked = false;
        rdoSKUSum.Checked = false;
        rdoSaleDetail.Checked = false;
        RB1.Checked = true;
        RB2.Checked = false;
        RB3.Checked = false;
        RB4.Checked = false;
        

    
    
    
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        pageclear();

    }

    protected void ddlStoreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        //绑定楼
        baseBo.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + "and storeid='" + ddlStoreName.SelectedValue + "'";
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Clear();
        ddlBuildingName.Items.Add(new ListItem("", ""));
        foreach (Building bd in rs)
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));

       //绑定区域
        baseBo.WhereClause = "Area.AreaStatus = " + Area.AREA_STATUS_VALID + "and storeid='" + ddlStoreName.SelectedValue + "'";
        Resultset rs3 = baseBo.Query(new Area());
        ddlAreaName.Items.Clear();
        ddlAreaName.Items.Add(new ListItem("", ""));
        foreach (Area bd in rs3)
        {
            ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));
        }
        ddlFloorName.Items.Clear();
        ddlShopCode.Items.Clear();



    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        ////绑定楼层'
        baseBo.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID  + " and Buildingid = '" + this.ddlBuildingName.SelectedValue.ToString()+"'";
        Resultset rs1 = baseBo.Query(new Floors());
        ddlFloorName.Items.Clear();
        ddlFloorName.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs1)
        {
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
        }
        ddlShopCode.Items.Clear();

    }
    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {

        //绑定商铺号
        ddlShopCode.Items.Clear();
        BaseBO baseBo = new BaseBO();
        string sql = "SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + "AND FLOORID='" + ddlFloorName.SelectedValue + "'AND FloorID='" + ddlFloorName.SelectedValue + "' Order By ShopCode";
        DataSet myDS = baseBo.QueryDataSet(sql);
        int count = myDS.Tables[0].Rows.Count;
        ddlShopCode.Items.Clear();
        ddlShopCode.Items.Add("");
        for (int i = 0; i < count; i++)
        {
            ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        }
    }

}
