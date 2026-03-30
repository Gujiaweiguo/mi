using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.Contract;
using Lease.ConShop;
using Sell;
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Store;

/// <summary>
/// 修改人：hesijian
/// 修改时间：2009年6月16日
/// </summary>
public partial class RptBaseMenu_RptSalesMediaSum : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSumCust_Title");
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
        return s.Trim() == "" ? "2007-12-15" : s;
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
        //绑定店

        this.txtEndBizTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        this.txtStartBizTime.Text = DateTime.Now.ToString("yyyy-MM-dd");

        //BaseBO baseBo = new BaseBO();
        //ddlShopCode.Items.Add(new ListItem("", ""));
        //Resultset rs = baseBo.Query(new ConShop());
        //foreach (ConShop conshop in rs)
        //    ddlShopCode.Items.Add(new ListItem(conshop.ShopName, conshop.ShopId.ToString()));

        BaseBO baseBO = new BaseBO();
        //string sql = "  SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + " Order By ShopCode";
        //DataSet myDS = baseBO.QueryDataSet(sql);
        //int count = myDS.Tables[0].Rows.Count;
        //ddlShopCode.Items.Clear();
        //ddlShopCode.Items.Add("");
        //for (int i = 0; i < count; i++)
        //{
        //    //绑定商铺号


        //    ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        //}

        baseBO.WhereClause = "";
        Resultset rs = baseBO.Query(new MediaM());
        ddlMediaMDesc.Items.Add(new ListItem("", ""));
        foreach (MediaM mediaM in rs)
        {
            ddlMediaMDesc.Items.Add(new ListItem( mediaM.MediaMDesc,mediaM.MediaMNo.ToString()));
        }

        /*绑定经营类型*/
        int[] bizModes = Contract.GetBizModes();
        cmbBizMode.Items.Add(new ListItem("", ""));
        for (int i = 0; i < bizModes.Length; i++)
        {
            cmbBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(bizModes[i])), bizModes[i].ToString()));
        }
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
        String s = "%23Rpt_lblSalesMediaSum";
        s += "%23" + "PotCustomer_lblCustCode";
        s += "%23" + "PotCustomer_lblCustName";
        s += "%23" + "LeaseholdContract_labShopCode";
        s += "%23" + "PotShop_lblPotShopName";
        s += "%23" + "Rpt_MediaMDesc";
        s += "%23" + "Rpt_MediaDesc";
        s += "%23" + "Rpt_GrossSales";
        s += "%23" + "Rpt_ComAmt";
        s += "%23" + "Rpt_NetAmt";
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
        //sCon += "%26" + "CustCode=" + GetStrNull(this.txtCustCode.Text);
        //sCon += "%26" + "ShopCode=" + GetStrNull(this.ddlShopCode.SelectedValue);
        //sCon += "%26" + "MediaMDesc=" + GetStrNull(this.ddlMediaMDesc.SelectedValue);
        return sCon;
    }

    //水晶报表数据绑定
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[31];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[31];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXNetAmt";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_NetAmt");
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
        paraField[5].Name = "REXMediaMDesc";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_MediaMDesc");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXMediaDesc";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_MediaDesc");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXPaidAmt";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PaidAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXCommChg";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_CommChg");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTitle";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSumCust_Title");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTotalAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_TotalAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXSdate";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = "销售日期";//(String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXQBDate";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = txtStartBizTime.Text;
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXQEDate";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = txtEndBizTime.Text;
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXContractCode";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXAmount";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXSubtotal";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSalesMediaSum_Subtotal");
        paraField[16].CurrentValues.Add(discreteValue[16]);

        paraField[17] = new ParameterField();
        paraField[17].Name = "REXCASH";
        discreteValue[17] = new ParameterDiscreteValue();
        discreteValue[17].Value = (String)GetGlobalResourceObject("BaseInfo", "InvoicePay_INVPAYTYPE_CASH");
        paraField[17].CurrentValues.Add(discreteValue[17]);

        paraField[18] = new ParameterField();
        paraField[18].Name = "REXCHECK";
        discreteValue[18] = new ParameterDiscreteValue();
        discreteValue[18].Value = (String)GetGlobalResourceObject("BaseInfo", "InvoicePay_INVPAYTYPE_CHECK");
        paraField[18].CurrentValues.Add(discreteValue[18]);

        paraField[19] = new ParameterField();
        paraField[19].Name = "REXXinYeLianJiNeiKa";
        discreteValue[19] = new ParameterDiscreteValue();
        discreteValue[19].Value = (String)GetGlobalResourceObject("ReportInfo", "RptXinYeLianJiNeiKa");
        paraField[19].CurrentValues.Add(discreteValue[19]);

        paraField[20] = new ParameterField();
        paraField[20].Name = "REXXinYeLianJiWaiKa";
        discreteValue[20] = new ParameterDiscreteValue();
        discreteValue[20].Value = (String)GetGlobalResourceObject("ReportInfo", "RptXinYeLianJiWaiKa");
        paraField[20].CurrentValues.Add(discreteValue[20]);

        paraField[21] = new ParameterField();
        paraField[21].Name = "REXXinYeTuoJiWaiKa";
        discreteValue[21] = new ParameterDiscreteValue();
        discreteValue[21].Value = (String)GetGlobalResourceObject("ReportInfo", "RptXinYeTuoJiWaiKa");
        paraField[21].CurrentValues.Add(discreteValue[21]);

        paraField[22] = new ParameterField();
        paraField[22].Name = "REXXinYeTuoJiNeiKa";
        discreteValue[22] = new ParameterDiscreteValue();
        discreteValue[22].Value = (String)GetGlobalResourceObject("ReportInfo", "RptXinYeTuoJiNeiKa");
        paraField[22].CurrentValues.Add(discreteValue[22]);

        paraField[23] = new ParameterField();
        paraField[23].Name = "REXJianSheLianJiNeiKa";
        discreteValue[23] = new ParameterDiscreteValue();
        discreteValue[23].Value = (String)GetGlobalResourceObject("ReportInfo", "RptJianSheLianJiNeiKa");
        paraField[23].CurrentValues.Add(discreteValue[23]);

        paraField[24] = new ParameterField();
        paraField[24].Name = "REXJianSheLianJiWaiKa";
        discreteValue[24] = new ParameterDiscreteValue();
        discreteValue[24].Value = (String)GetGlobalResourceObject("ReportInfo", "RptJianSheLianJiWaiKa");
        paraField[24].CurrentValues.Add(discreteValue[24]);

        paraField[25] = new ParameterField();
        paraField[25].Name = "REXJianSheTuoJiNeiKa";
        discreteValue[25] = new ParameterDiscreteValue();
        discreteValue[25].Value = (String)GetGlobalResourceObject("ReportInfo", "RptJianSheTuoJiNeiKa");
        paraField[25].CurrentValues.Add(discreteValue[25]);

        paraField[26] = new ParameterField();
        paraField[26].Name = "REXJianSheTuoJiWaiKa";
        discreteValue[26] = new ParameterDiscreteValue();
        discreteValue[26].Value = (String)GetGlobalResourceObject("ReportInfo", "RptJianSheTuoJiWaiKa");
        paraField[26].CurrentValues.Add(discreteValue[26]);

        paraField[27] = new ParameterField();
        paraField[27].Name = "REXXingYeLianMing";
        discreteValue[27] = new ParameterDiscreteValue();
        discreteValue[27].Value = (String)GetGlobalResourceObject("ReportInfo", "RptXingYeLianMing");
        paraField[27].CurrentValues.Add(discreteValue[27]);

        paraField[28] = new ParameterField();
        paraField[28].Name = "REXJianSheLianMing";
        discreteValue[28] = new ParameterDiscreteValue();
        discreteValue[28].Value = (String)GetGlobalResourceObject("ReportInfo", "RptJianSheLianMing");
        paraField[28].CurrentValues.Add(discreteValue[28]);

        paraField[29] = new ParameterField();
        paraField[29].Name = "REXMallTitle";
        discreteValue[29] = new ParameterDiscreteValue();
        discreteValue[29].Value = Session["MallTitle"].ToString();
        paraField[29].CurrentValues.Add(discreteValue[29]);

        paraField[30] = new ParameterField();
        paraField[30].Name = "REXBizProject";
        discreteValue[30] = new ParameterDiscreteValue();
        discreteValue[30].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[30].CurrentValues.Add(discreteValue[30]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = " select store.storeid, Store.storeshortname, Customer.CustCode,Customer.CustName,ConShop.ShopCode,ConShop.ShopName,MediaM.MediaMDesc,Media.MediaDesc,sum(PaidAmt) PaidAmt,sum(CommRate) CommChg,sum(NetAmt) as NetAmt,Contract.ContractCode,MediaM.MediaMNo " +
                         " from   TransSkuMedia , ConShop ,STORE, Contract ,Customer , MediaM , Media " +
                         " where  TransSkuMedia.ShopID=ConShop.ShopID AND CONSHOP.StoreID=Store.Storeid and ConShop.ContractID=Contract.ContractID and Contract.CustID=Customer.CustID" +
                         " and TransSkuMedia.MediaMNo=MediaM.MediaMNo and MediaM.MediaNo=Media.MediaNo" ;


        //条件查询
        if (ddlBizproject.Text != "")
        {
            str_sql = str_sql + " AND store.storeid='"+ddlBizproject.SelectedValue+"'";
        }
        if (txtCustCode.Text != "")
        {
            str_sql = str_sql + " AND Customer.CustCode ='" + txtCustCode.Text + "'";
        }

        if (ddlShopCode.Text != "")
        {
            str_sql = str_sql + " AND ConShop.ShopCode ='" + ddlShopCode.SelectedItem.Text.Substring(0, ddlShopCode.SelectedItem.Text.IndexOf(" ")) + "'";
        }
        if (ddlMediaMDesc.Text != "")
        {
            str_sql = str_sql + " AND MediaM.MediaMNo ='" + ddlMediaMDesc.SelectedValue + "'";
        }
        if (txtStartBizTime.Text != "")
        {
            str_sql = str_sql + " AND TransSkuMedia.BizDate >='" + txtStartBizTime.Text + "'";
        }
        if (txtEndBizTime.Text != "")
        {
            str_sql = str_sql + " AND TransSkuMedia.BizDate  <='" + txtEndBizTime.Text  + "'";
        }
        if (RB1.Checked)
        {
            str_sql = str_sql + " ";

        }
        if (RB2.Checked)
        {
            str_sql = str_sql + " AND TransSkuMedia.datasource=1 ";

        }
        if (this.RB5.Checked)
        {
            str_sql = str_sql + " AND TransSkuMedia.datasource=2 ";

        }
        if (RB6.Checked)
        {
            str_sql = str_sql + " AND TransSkuMedia.datasource=3 ";

        }

        if (Convert.ToInt32(cmbBizMode.SelectedIndex) !=0)
        {
            str_sql = str_sql + " AND Contract.BizMode =" + cmbBizMode.SelectedValue;
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }

        str_sql = str_sql + " group  by store.storeid, Store.storeshortname,Customer.CustCode,Customer.CustName,ConShop.ShopCode,ConShop.ShopName,MediaM.MediaMDesc,Media.MediaDesc,Contract.ContractCode,MediaM.MediaMNo ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\SalesMediaSum.rpt";

    }

    protected void ddlBizproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindShop();
    }

    protected void BindShop()
    {
        BaseBO baseBO = new BaseBO();
        string sql = "  SELECT ConShop.ShopID,ConShop.ShopCode,ConShop.ShopName FROM ConShop Where ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + "AND StoreID='"+ddlBizproject.SelectedValue+"' Order By ShopCode";
        DataSet myDS = baseBO.QueryDataSet(sql);
        int count = myDS.Tables[0].Rows.Count;
        ddlShopCode.Items.Clear();
        ddlShopCode.Items.Add("");
        for (int i = 0; i < count; i++)
        {
            //绑定商铺号

            ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
        }
    
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptSalesMediaSum.aspx");
    }
}
