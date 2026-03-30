using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
//using CrystalDecisions.CrystalReports.Engine;

using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;

public partial class RptBaseMenu_RptTradeSalesSum : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_TradeSaleStatistics");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
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
        this.txtStartBizTime.Text = DateTime.Now.ToShortDateString();
        this.txtEndBizTime.Text = DateTime.Now.ToShortDateString();
        BaseBO baseBo = new BaseBO();
        #region
        //绑定经营方式
        //int[] contractType = Contract.GetBizModes();
        //int s = contractType.Length + 1;
        //ddlBizMode.Items.Add(new ListItem("", ""));
        //for (int i = 1; i < s; i++)
        //{
        //    ddlBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Contract.GetBizModeDesc(contractType[i - 1])), contractType[i - 1].ToString()));
        //}

        ////绑定商铺号

        //string sql = "SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + " Order By ShopCode";
        //DataSet myDS = baseBo.QueryDataSet(sql);
        //int count = myDS.Tables[0].Rows.Count;
        //ddlShopCode.Items.Clear();
        //ddlShopCode.Items.Add("");
        //for (int i = 0; i < count; i++)
        //{
        //    ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        //}
        ////绑定二级经营类别
        //baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
        //ddlTrade2Name.Items.Add(new ListItem("", ""));
        //Resultset rs5 = baseBo.Query(new TradeRelation());
        //foreach (TradeRelation tradeDef in rs5)
        //    ddlTrade2Name.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));

        /*绑定销售数据来源*/
        //cmbDataSource.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Rpt_rdoAll"), ""));
        //cmbDataSource.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "DataSource_POS"), "1"));
        //cmbDataSource.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "DataSource_Put"), "2"));
        //cmbDataSource.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "DataSource_Manual"), "3"));
        #endregion

        //绑定一级经营类别
        baseBo.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_ONE + "'";
        Resultset rs4 = baseBo.Query(new TradeRelation());
        ddlTradeID.Items.Add(new ListItem("", ""));
        foreach (TradeRelation tradeDef in rs4)
            ddlTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
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
        String s = "%23Rpt_lblShopSalesSum";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
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
        //sCon += "%26BizSDate=" + GetdateNull(this.txtStartBizTime.Text);
        //sCon += "%26BizEDate=" + GetdateNull(this.txtEndBizTime.Text);
        //sCon += "%26" + "ShopCode=" + GetStrNull(this.txtShopCode.Text);
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
        paraField[0].ParameterFieldName = "REXPaidAmt";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TransTotalAmt");//交易总额
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTotalReceipt";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_TrNum");//交易笔数
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXTotalQty";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_ShopNum");//商品总数
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXReceiptAve";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptReceiptAve");//客均价
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXQtyAve";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_GoodsPrice");//商品均价
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].ParameterFieldName = "REXTitle";
        discreteValue[5] = new ParameterDiscreteValue();
        if (RB1.Checked)
        {
            discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptTradeSaleSumday");//标题
        }
        else
        {
            discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptTradeSaleSummth");//标题
        }
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXMallTitle";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = Session["MallTitle"].ToString();//集团名称
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXStoreName";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Store_StoreName");//项目名称
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXFloorName";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblFloorName");//楼层名称
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTotal";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblTotal");//总计
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTradeName";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labTradeID");//业态
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXDate";
        discreteValue[11] = new ParameterDiscreteValue();
        paraField[11].CurrentValues.Add(discreteValue[11]);


        if (this.RB1.Checked)
        {
            discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_Date");//日期
        }
        else
        {
            discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SalesMonth");//记账月
        }

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
                
        string str_sql = "";
        if (RB1.Checked)
        {
            str_sql = "Select StoreID,StoreName,FloorID,FloorName,TradeID,TradeName,convert(varchar(10),BizDate,120) as Date,sum(PaidAmt)as PaidAmt,sum(TotalReceipt)as TotalReceipt,sum(TotalQty) as TotalQty,Convert(Decimal(10,2),(sum(PaidAmt)/sum(TotalReceipt))) As ReceiptAve,Convert(Decimal(10,2),(sum(PaidAmt)/sum(TotalQty))) As QtyAve From TransShopDay Where 1=1 ";
        }
        else
        {
            str_sql = "Select StoreID,StoreName,FloorID,FloorName,TradeID,TradeName,convert(varchar,year(Month))+'-'+convert(varchar,month(Month)) as Date,sum(PaidAmt)as PaidAmt,sum(TotalReceipt)as TotalReceipt,sum(TotalQty) as TotalQty,Convert(Decimal(10,2),(sum(PaidAmt)/sum(TotalReceipt))) As ReceiptAve,Convert(Decimal(10,2),(sum(PaidAmt)/sum(TotalQty))) As QtyAve From TransShopmth Where 1=1 ";
        }

        if (this.ddlBizproject.Text != "")
        {
            str_sql = str_sql + " AND StoreID ='" + ddlBizproject.SelectedValue.ToString() + "'";
        }
        
        if (ddlTradeID.Text != "")
        {
            str_sql = str_sql + " AND TradeID ='" + ddlTradeID.SelectedValue + "'";
        }

        if (this.RB1.Checked)
        {
            if (txtStartBizTime.Text != "")
            {
                str_sql = str_sql + " AND BizDate >='" + txtStartBizTime.Text.Trim() + "'";
            }
            if (txtEndBizTime.Text != "")
            {
                str_sql = str_sql + " AND BizDate  <='" + txtEndBizTime.Text.Trim() + "'";
            }
        }
        else
        {
            if (txtEndBizTime.Text != "")
            {
                str_sql = str_sql + " AND year(Month)  =year('" + DateTime.Parse(this.txtStartBizTime.Text.Trim()) + "') AND month(Month)  =month('" + DateTime.Parse(this.txtStartBizTime.Text.Trim()) + "')";
            }
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strAuth = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strAuth = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
            string strTable = "";
            if (this.RB1.Checked)
                strTable = "TransShopDay";
            else
                strTable = "TransShopMth";
            for (int i = 0; i < 5; i++)
            {
                strAuth = strAuth.Replace("ConShop", strTable);//将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
            }
        }
        str_sql = str_sql + strAuth;
        if (this.RB1.Checked)
        {
            str_sql = str_sql + " group by StoreID,StoreName,FloorID,FloorName,tradeID,TradeName,BizDate ";
        }
        else
        {
            str_sql = str_sql + " group by StoreID,StoreName,FloorID,FloorName,tradeID,TradeName,Month ";
        }
        if (RB1.Checked)
        {
            str_sql = str_sql + " order by storeid,FloorID,TradeID,BizDate";
        }
        else
        {
            str_sql = str_sql + " order by storeid,FloorID,TradeID,Month ";
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptTradeSalesSum.rpt";

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptShopSalesSum.aspx");
    }
    protected void RB1_CheckedChanged(object sender, EventArgs e)
    {
        this.Label14.Visible = false;
        this.Label6.Visible = true;
        this.Label12.Visible = true;
        this.txtEndBizTime.Visible = true;
        this.txtStartBizTime.Text = DateTime.Now.ToShortDateString();
        this.txtEndBizTime.Text = DateTime.Now.ToShortDateString();
    }
    protected void RB2_CheckedChanged(object sender, EventArgs e)
    {
        this.Label14.Visible = true;
        this.Label6.Visible = false;
        this.Label12.Visible = false;
        this.txtEndBizTime.Visible = false;
        this.txtStartBizTime.Text = DateTime.Now.ToShortDateString();
        this.txtEndBizTime.Text = DateTime.Now.ToShortDateString();
    }
}
