using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

public partial class RptBaseMenu_RptTradeSalesAnalysis : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblTradeSalesAnalysis");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
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
    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Add(new ListItem("", ""));
        foreach (Store store in rs)
        {
            ddlBizproject.Items.Add(new ListItem(store.StoreName, store.StoreId.ToString()));
        }

    }
    private void InitDDL()
    {
        int intMonth = 12;
        ddlMonth.Items.Clear();
        for (int iMonth = 1; iMonth <= intMonth; iMonth++)
        {
            ddlMonth.Items.Add(new ListItem(iMonth.ToString(), iMonth.ToString()));
        }
        this.ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

        int year = Convert.ToInt16(DateTime.Now.Year);
        ddlYear.Items.Clear();
        for (int time = year - 5; time <= year + 5; time++)
        {
            ddlYear.Items.Add(new ListItem(time.ToString(), time.ToString()));
        }
        this.ddlYear.SelectedValue = DateTime.Now.Year.ToString();
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
        String s = "%23Rpt_lblTradeSalesAnalysis";
        s += "%23" + "RentableArea_lblTradeRelation";
        s += "%23" + "LeaseholdContract_labTradeID";
        s += "%23" + "Rpt_TotalReceipt";
        s += "%23" + "Rpt_GrossSales";
        return s;
    }

    /* 组合查询条件
     * 
     * 
     */
    private String GetRptCond()
    {
        String sCon = "%26sPara=''";
        //sCon += "%26BizSDate=" + GetdateNull(this.txtBizSDate.Text);
        //sCon += "%26BizEDate=" + GetdateNull(this.txtBizEDate.Text);
        //sCon += "%26" + "BizMode=" + GetStrNull(this.ddlBizMode.Text);
        //sCon += "%26" + "BuildingName=" + GetStrNull(this.ddlBuildingName.Text);
        //sCon += "%26" + "FloorName=" + GetStrNull(this.ddlFloorName.Text);
        //sCon += "%26" + "LocationName=" + GetStrNull(this.ddlLocationName.Text);
        //sCon += "%26" + "AreaName=" + GetStrNull(this.ddlAreaName.Text);
        //sCon += "%26" + "Trade1Name=" + GetStrNull(this.ddlTradeID.Text);
        //sCon += "%26" + "Trade2Name=" + GetStrNull(this.ddlTrade2Name.Text);
        return sCon;
    }
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[12];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[12];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblTradeSalesAnalysis");
        paraField[0].CurrentValues.Add(discreteValue[0]);
        //租金
        paraField[1] = new ParameterField();
        paraField[1].Name = "REXRentAmt";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_FixedRental");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXTradeName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_Trade2Name");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXArea";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = "签约面积";
        paraField[3].CurrentValues.Add(discreteValue[3]); 
        //销售额
        paraField[4] = new ParameterField();
        paraField[4].Name = "REXPaidAmt";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_CostAmt");
        paraField[4].CurrentValues.Add(discreteValue[4]);
        //总计
        paraField[5] = new ParameterField();
        paraField[5].Name = "REXTotalAmt";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);
        // "查询月份"
        paraField[6] = new ParameterField();
        paraField[6].Name = "REXSdate";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_SeachMonth");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXQBDate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = this.ddlYear.SelectedValue.ToString() + "年" + this.ddlMonth.SelectedValue.ToString() +"月";
        paraField[7].CurrentValues.Add(discreteValue[7]);
        //租售比
        paraField[8] = new ParameterField();
        paraField[8].Name = "REXRate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_SalesRentRate");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXMallTitle";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = Session["MallTitle"].ToString();
        paraField[9].CurrentValues.Add(discreteValue[9]);

        //商户数量
        paraField[10] = new ParameterField();
        paraField[10].Name = "REXContractCount";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustCount");
        paraField[10].CurrentValues.Add(discreteValue[10]);
        //商铺数量
        paraField[11] = new ParameterField();
        paraField[11].Name = "REXShopCount";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCount");
        paraField[11].CurrentValues.Add(discreteValue[11]);
       

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        } 

        //租售比=rate*100  月租金类收入/销售额×100(百分比)
        //坪效= 销售/合同签约面积/月天数
        //string str_sql = "select store.storeid,store.storeshortname,(rtrim(shoptype.shoptypename)+ floors.floorname) as shoptype,count(conshop.shopid) as shopCount,sum(invoiceDetail.invActPayAmtL) AS RentalAmt,(sum(invoiceDetail.invActPayAmtL)/sum(PaidAmt) * 100) as rat," +
        //                  " count(contract.contractid) as contractCount,sum(conshop.rentarea) as totalArea, " +
        //                   "(Sum(PaidAmt)/sum(conshop.rentarea)/ datediff(day,'" + dtSeach + "',dateadd(month,1, '" + dtSeach + "'))) as srRate," +
        //                  "TransShopMth.TradeId as TradeCode,TransShopMth.TradeName as TradeName,Sum(PaidAmt) as PaidAmt,Count(TransShopMth.TotalReceipt ) as TrNum" +
        //                   " from TransShopMth inner join ConShop On (TransShopMth.ShopId=ConShop.ShopId) inner join store ON(conshop.storeid=store.storeid) " +
        //                   " inner join Contract On (ConShop.Contractid=Contract.ContractId)" +
        //                   " inner join shoptype on (shoptype.shoptypeid=conshop.shoptypeid)" +
        //                    " inner join floors on (floors.floorid=conshop.floorid) inner join invoiceheader on (invoiceheader.contractid=conshop.contractid) " +
        //                    " inner join invoicedetail ON (invoiceHeader.invid = invoiceDetail.invid)" +
        //                   " Where 1=1 ";


        // 费用记账月 销售查该月的销售 如果为空则为当前月
        string dtSeach = "";
        //年月均不可为空
        dtSeach = Convert.ToDateTime(this.ddlYear.SelectedValue.ToString() + "-" + this.ddlMonth.SelectedValue.ToString() + "-01").ToString("yyyy-MM-dd");

        string strWhere = "";
        if (this.ddlYear.SelectedValue.ToString() != "" || this.ddlMonth.SelectedValue.ToString() != "")
        {
            //确定费用取收付实现制月份
            strWhere = "invoicedetail.period ='" + dtSeach + "'";
        }
        else
        {
            strWhere = "invoicedetail.period ='" + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) + "'";
        }
        

        string str_sql = "select TransShopMth.storeId,TransShopMth.storename storeshortname," +
                        "(rtrim(TransShopMth.shoptypename)+ TransShopMth.floorname) as shoptype," +
                        "count(TransShopMth.shopid) as shopCount,sum(aa.invActPayAmtL) AS RentalAmt," +
                        "(sum(aa.invActPayAmtL)/sum(PaidAmt) * 100) as rat, count(aa.contractid) as contractCount," +
                        "sum(conshop.rentarea) as totalArea,0 as srRate,TransShopMth.TradeId as TradeCode," +
                        "TransShopMth.TradeName as TradeName,Sum(PaidAmt) as PaidAmt,Count(TransShopMth.TotalReceipt ) as TrNum " +
                        "from TransShopMth inner join conshop on (TransShopMth.shopid = conshop.shopid)" +
                        " inner join " +
                            "(select invoiceheader.contractid,sum(invoiceDetail.invActPayAmtL) invActPayAmtL  from invoiceheader " +
                            "inner join invoicedetail on (invoicedetail.invid=invoiceheader.invid) " +
                            "inner join chargetype on (invoicedetail.chargetypeid=chargetype.chargetypeid) " + //确定租金额来自租金和每月固定费用
                            "where chargetype.chargeclass in (1,3) and " + strWhere +
                            " group by invoiceheader.contractid ) aa on (aa.contractid=conshop.contractid) " +
                        " where TransShopMth.month='" + dtSeach + "' ";

        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + "AND TransShopMth.STOREID='" + ddlBizproject.SelectedValue + "'";
        }            


        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE  + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        str_sql = str_sql + strAuth + " group by TransShopMth.storeId,TransShopMth.storename,TransShopMth.shoptypename,TransShopMth.floorname," +
                  "TransShopMth.TradeId,TransShopMth.TradeName order by TransShopMth.StoreID,TransShopMth.floorname,TransShopMth.shoptypename";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\TradeSalesAnalysis.rpt";

    }

    
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptTradeSalesAnalysis.aspx");
    }
}
