using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.ConShop;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;
using Lease.Brand;
public partial class RptBaseMenu_RptBrandSales : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();            
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BrandSales");
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

        rs = baseBo.Query(new BrandLevel());
        ddlBrandLevel.Items.Add(new ListItem("",""));
        foreach (BrandLevel brandLevel in rs)
        {
            ddlBrandLevel.Items.Add(new ListItem(brandLevel.BrandLevelName,brandLevel.BrandLevelID.ToString()));
        }

    }
    private void InitDDL()
    {
        BindBizProject();
        this.txtEndBizTime.Text = DateTime.Now.ToShortDateString();
        this.txtStartBizTime.Text = DateTime.Now.ToShortDateString(); 
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
        ParameterField[] paraField = new ParameterField[11];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[11];
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
            discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptBrandDSales_Title");//标题
        }
        else
        {
            discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptBrandMSales_Title");//标题
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
        paraField[8].Name = "REXDate";
        discreteValue[8] = new ParameterDiscreteValue();
        if (this.RB1.Checked == true)
        {
            discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_Date");//日期
        }
        else
        {
            discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SalesMonth");//记账月
        }
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXBrandName";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblBrandName");//品牌名称
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTotal";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblTotal");//总计
        paraField[10].CurrentValues.Add(discreteValue[10]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = "";
        if (this.RB1.Checked == true)
        {
            str_sql = "select StoreID,StoreName,convert(varchar(10),BizDate,120) as Date,brandlevel.brandlevelname,brandlevel.brandlevelid,transshopday.brandid,transshopday.brandname,sum(paidamt) paidamt," +
                    "sum(TotalReceipt) TotalReceipt,sum(TotalQty) TotalQty,(sum(PaidAmt)/sum(TotalReceipt)) as ReceiptAve,(sum(PaidAmt)/sum(TotalQty)) as QtyAve " +
                    "from transshopday inner join conshopbrand on (transshopday.brandid=conshopbrand.brandid) inner join brandlevel on (conshopbrand.brandlevel=brandlevel.brandlevelid) " + 
                    "where 1=1 ";
        }
        else
        {
            str_sql = "select StoreID,StoreName,convert(varchar(10),month,120) as Date,brandlevel.brandlevelname,brandlevel.brandlevelid,transshopmth.brandid,transshopmth.brandname,sum(paidamt) paidamt," +
                    "sum(TotalReceipt) TotalReceipt,sum(TotalQty) TotalQty,(sum(PaidAmt)/sum(TotalReceipt)) as ReceiptAve,(sum(PaidAmt)/sum(TotalQty)) as QtyAve " +
                    "from transshopmth inner join conshopbrand on (transshopmth.brandid=conshopbrand.brandid) inner join brandlevel on (conshopbrand.brandlevel=brandlevel.brandlevelid) " +
                    "where 1=1 ";
        }

        if (this.ddlBizproject.Text != "")
        {
            str_sql = str_sql + " AND StoreID ='" + ddlBizproject.SelectedValue.ToString() + "'";
        }

        if (this.ddlBrandLevel.Text != "")
        {
            str_sql = str_sql + " AND brandlevel.brandlevelid = '" + this.ddlBrandLevel.SelectedValue + "'";
        }

        if (this.ddlBrandName.Text != "")
        {
            if (this.RB1.Checked)
	    {
		str_sql = str_sql + " AND transshopday.brandid = '" + this.ddlBrandName.SelectedValue + "'";
	    }
	    else
	    {
		str_sql = str_sql + " AND transshopmth.brandid = '" + this.ddlBrandName.SelectedValue + "'";
 	    }
        }
        
        if (this.RB1.Checked)
        {
            if (this.txtStartBizTime.Text != "")
            {
                str_sql = str_sql + " AND BizDate >='" + txtStartBizTime.Text.Trim() + "'";
            }
            if (this.txtEndBizTime.Text != "")
            {
                str_sql = str_sql + " AND BizDate  <='" + txtEndBizTime.Text.Trim() + "'";
            }
        }
        else
        {
            if (this.txtStartBizTime.Text != "")
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
        if (RB1.Checked)
        {
            str_sql = str_sql + " group by StoreID,StoreName,BizDate,TransShopDay.brandid,TransShopDay.brandname,brandlevel.brandlevelname,brandlevel.BrandLevelID order by StoreID,brandlevel.BrandLevelID,TransShopDay.brandid,BizDate";
        }
        else
        {
            str_sql = str_sql + " group by StoreID,StoreName,month,TransShopMth.brandid,TransShopMth.brandname,brandlevel.brandlevelname,brandlevel.BrandLevelID order by StoreID,brandlevel.BrandLevelID,TransShopMth.brandid,Month ";
        }
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptBrandSales.rpt";
    }
   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptBrandSales.aspx");
    }
    protected void RB2_CheckedChanged(object sender, EventArgs e)
    {
        this.Label16.Visible = false;
        this.Label17.Visible = false;
        this.txtEndBizTime.Visible = false;
        this.Label15.Visible = true;
        this.txtEndBizTime.Text = DateTime.Now.ToString("yyyy-MM-01");
        this.txtStartBizTime.Text = DateTime.Now.ToString("yyyy-MM-01");
    }
    protected void RB1_CheckedChanged(object sender, EventArgs e)
    {
        this.Label16.Visible = true;
        this.Label17.Visible = true;
        this.txtEndBizTime.Visible = true;
        this.Label15.Visible = false;
        this.txtEndBizTime.Text = DateTime.Now.ToShortDateString();
        this.txtStartBizTime.Text = DateTime.Now.ToShortDateString();
    }

    protected void ddlBrandLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ddlBrandName.Items.Clear();
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BrandLevel='" + this.ddlBrandLevel.SelectedValue + "'";
        Resultset rs = baseBo.Query(new ConShopBrand());
        ddlBrandName.Items.Add(new ListItem("", ""));
        foreach (ConShopBrand bd in rs)
        {
            ddlBrandName.Items.Add(new ListItem(bd.BrandName, bd.BrandId.ToString()));
        }
    }
    protected void ddlBrandName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

}
