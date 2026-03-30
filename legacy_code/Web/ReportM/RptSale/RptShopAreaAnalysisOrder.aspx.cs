using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.Contract;
using RentableArea;
using Lease.ConShop;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class ReportM_RptSale_RptShopAreaAnalysisOrder : BasePage

{
    public string baseInfo;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopAreaAnalysisOrder");
            Fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.txtStartBizTime.Text  = DateTime.Now.Date.ToString("yyyy-MM-01");
            this.txtEndBizTime.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            this.txtCount.Text = "30";
        }
    }

    private void BindShop()
    {
        if (this.ddlBizproject.Text.Trim().Length > 0)
        {
            BaseBO baseBo = new BaseBO();
            string sql = "select shopid,shopname from conshop inner join store on (store.storeid=conshop.storeid) where shopstatus=1 " +
                    "and store.storeid=" + ddlBizproject.SelectedValue + " order by shopname" ;
            DataSet myDs = baseBo.QueryDataSet(sql);
            if (myDs.Tables[0].Rows.Count > 0)
            {
                this.ddlShopCode.Items.Clear();
                ddlShopCode.Items.Add("");
                for (int i = 0; i < myDs.Tables[0].Rows.Count; i++)
                {
                    ddlShopCode.Items.Add(new ListItem(myDs.Tables[0].Rows[i]["shopname"].ToString(),myDs.Tables[0].Rows[i]["shopid"].ToString()));
                }
            }
        }
    }

    private void bindArea()
    { 
        if (this.ddlBizproject.Text.Trim().Length > 0)
        {
            BaseBO baseBo = new BaseBO();
            string sql = "select areaid,areaname from area where areastatus=1 and storeid=" + ddlBizproject.SelectedValue + " order by areaname" ;
            DataSet myDs = baseBo.QueryDataSet(sql);
            if (myDs.Tables[0].Rows.Count > 0)
            {
                this.ddlAreaName.Items.Clear();
                ddlAreaName.Items.Add("");
                for (int i = 0; i < myDs.Tables[0].Rows.Count; i++)
                {
                    ddlAreaName.Items.Add(new ListItem(myDs.Tables[0].Rows[i]["areaname"].ToString(),myDs.Tables[0].Rows[i]["areaid"].ToString()));
                }
            }
        }
    }

     #region 绑定下拉列表
    private void InitDDL()
    {

        clear();
        BaseBO baseBo = new BaseBO();
        string sql = "select storeid,storename from store where storestatus=1";
        DataSet myDs = baseBo.QueryDataSet(sql);
        if (myDs.Tables[0].Rows.Count > 0)
        {
            ddlBizproject.Items.Clear();
            ddlBizproject.Items.Add("");
            for (int i = 0; i < myDs.Tables[0].Rows.Count; i++)
            {
                ddlBizproject.Items.Add(new ListItem(myDs.Tables[0].Rows[i]["storename"].ToString(),myDs.Tables[0].Rows[i]["storeid"].ToString()));
            }
        }
        //string sql = "SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + " Order By ShopCode";
        //DataSet myDS = baseBo.QueryDataSet(sql);
        //int count = myDS.Tables[0].Rows.Count;
        //ddlShopCode.Items.Clear();
        //ddlShopCode.Items.Add("");
        //for (int i = 0; i < count; i++)
        //{
        //    //绑定商铺号
        //    ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        //}

   
        ////绑定区域
        //baseBo.WhereClause = "AreaStatus = " + Area.AREA_STATUS_VALID;
        //Resultset rs3 = baseBo.Query(new Area());
        //ddlAreaName.Items.Add(new ListItem("", ""));
        //foreach (Area bd in rs3)
        //{
        //    ddlAreaName.Items.Add(new ListItem(bd.AreaName, bd.AreaID.ToString()));
        //}

        //绑定经营类别
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        ddlTrade2Name.Items.Add(new ListItem("", ""));
        Resultset rs5 = baseBo.Query(new TradeRelation());
        foreach (TradeRelation tradeDef in rs5)
        {
            ddlTrade2Name.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
        }
    }
     #endregion

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        clear();
    }
    #region 绑定数据
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[16];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[16];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";    //标题
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopAreaAnalysisOrder");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXShopCode";　　　　//商铺代码
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXShopName";     //商铺名称
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXAvgPaidAmt";   //单位面积销售

        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_AvgArea");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXTrNum";　　　　//交易笔数
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TrNum");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXPaidAmt";       //交易金额
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXTotalAmt";      //总计
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXQBDate";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = txtStartBizTime.Text.ToString();
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXQEDate";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = txtEndBizTime.Text.ToString();
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXSdate";       //查询日期
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[9].CurrentValues.Add(discreteValue[9]);


        paraField[10] = new ParameterField();
        paraField[10].Name = "REXRptNum";     //序号
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "WrkFlw_Sequence");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXMallTitle";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = Session["MallTitle"].ToString();
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXRentArea";  //签约面积
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "RptUnitInfo_RentArea");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXTradeName"; // 经营类别
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_Trade2Name");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXTradeCode";  //经营种类编码
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TradeCode");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXAreaName";  //区域名称
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_AreaName");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = "";
        if (txtCount.Text != "")
        {
            str_sql = " select Top " + txtCount.Text.Trim() + " store.storename,Conshop.ShopCode as ShopId ,Conshop.ShopName ," +
                    "(select Area.AreaName from Area where Area.AreaID=ConShop.AreaID) as AreaName ," +
                    "TransSku.Trade2ID,TransSku.Trade2Name,conshop.RentArea,Sum(PaidAmt) as PaidAmt," +
                    "Sum(PaidAmt)/conshop.rentarea as AvgPaidAmt ,Count(ReceiptId) as TotalReceipt";
        }
        else
        {

          str_sql = " select store.storename,Conshop.ShopCode as ShopId ,Conshop.ShopName ," +
                    "(select Area.AreaName from Area where Area.AreaID=ConShop.AreaID) as AreaName ," +
                    "TransSku.Trade2ID,TransSku.Trade2Name,conshop.RentArea,Sum(PaidAmt) as PaidAmt," +
                    "Sum(PaidAmt)/conshop.rentarea as AvgPaidAmt ,Count(ReceiptId) as TotalReceipt";
        }
        str_sql = str_sql + " from TransSku inner join ConShop on (conshop.shopid=transsku.shopid) " +
                            "inner join Contract on (conshop.Contractid=Contract.Contractid) " +
                            "inner join store on (conshop.storeid=store.storeid) ";
        if (this.ddlBizproject.Text.Trim() != "")
        {
            str_sql =str_sql + " and Store.storeid=" + ddlBizproject.SelectedValue;
        }

        if (ddlShopCode.Text != "")
        {
            str_sql = str_sql + " AND ConShop.ShopCode ='" + ddlShopCode.SelectedItem.Text.Substring(0, ddlShopCode.SelectedItem.Text.IndexOf(" ")) + "'";
        }
       if (ddlFloorName.Text != "")
        {
            str_sql = str_sql + " AND TransSku.FloorId ='" + ddlFloorName.SelectedValue + "'";
        }
        if (ddlAreaName.Text != "")
        {
            str_sql = str_sql + " AND TransSku.AreaId ='" + ddlAreaName.SelectedValue + "'";
        }

        if (ddlTrade2Name.Text != "")
        {
            str_sql = str_sql + " AND TransSku.Trade2ID ='" + ddlTrade2Name.SelectedValue + "'";
        }
        if (txtStartBizTime.Text != "")
        {
            str_sql = str_sql + " AND TransSku.BizDate>='" + txtStartBizTime.Text + "'";
        }
        if (txtEndBizTime.Text != "")
        {
            str_sql = str_sql + " AND TransSku.BizDate <='" + txtEndBizTime.Text + "'";
        }
        if (txtAreaB.Text != "")
        {
            str_sql = str_sql + "AND conshop.RentArea>='" + txtAreaB.Text.Trim().ToString() + "'";
        }
        if (txtAreaE.Text != "")
        {
            str_sql = str_sql + "AND conshop.RentArea<='" + txtAreaE.Text.Trim().ToString() + "'";
        }
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

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            AuthBase.AUTH_SQL_SHOP = AuthBase.AUTH_SQL_SHOP.Replace("ConShop", "TransSku");
            AuthBase.AUTH_SQL_BUILD = AuthBase.AUTH_SQL_BUILD.Replace("ConShop", "TransSku");
            AuthBase.AUTH_SQL_FLOOR = AuthBase.AUTH_SQL_FLOOR.Replace("ConShop", "TransSku");
            AuthBase.AUTH_SQL_CONTRACT = AuthBase.AUTH_SQL_CONTRACT.Replace("ConShop", "TransSku");

            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        str_sql = str_sql + " group by Store.StoreName,Conshop.ShopCode,Conshop.ShopName,Conshop.ShopId,"+
                            "conshop.rentarea,ConShop.AreaID,TransSku.Trade2ID,TransSku.Trade2Name ";
        if (RB5.Checked)
        {
            str_sql = str_sql + " ORDER BY ConShop.ShopCode ";
        }
        if (RB6.Checked)
        {
            str_sql = str_sql + " ORDER BY AvgPaidAmt desc ";
        }
        if (RB7.Checked)
        {
            str_sql = str_sql + " ORDER BY TransSku.Trade2ID ";
        }

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptShopAreaAnalysisOrder.rpt";

    }
    #endregion

    protected void clear()
    {
        ddlShopCode.Text = "";
        ddlFloorName.Text = "";
        ddlAreaName.Text = "";
        ddlTrade2Name.Text = "";
        txtStartBizTime.Text = "";
        txtEndBizTime.Text = "";
        txtAreaB.Text = "";
        txtAreaE.Text = "";
        txtCount.Text = "";
        RB1.Checked = true;
        RB2.Checked = false;
        RB3.Checked = false;
        RB4.Checked = false;
        RB5.Checked = false ;
        RB6.Checked = true ;
        RB7.Checked = false;
    }
    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddlBizproject.Text.Trim().Length > 0)
        {
            bindArea();
            BindShop();
        }
    }
}

