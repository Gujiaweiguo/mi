using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Store;
using Shop.ShopType;

public partial class ReportM_RptSale_RptStoreShopTypeRentalThan : BasePage
{
    public string strBaseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Init();
            txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }



    protected void Init()
    {
        //绑定商业项目
        BaseBO baseBo = new BaseBO();
        Resultset rs3 = baseBo.Query(new Store());
        this.ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs3)
        {
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));
        }
        //绑定商铺类型
        Resultset rs2 = baseBo.Query(new ShopType());
        ddlShopType.Items.Add(new ListItem("", ""));
        foreach (ShopType shopType in rs2)
        {
            ddlShopType.Items.Add(new ListItem(shopType.ShopTypeName, shopType.ShopTypeName));
        }
    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ddlStoreName.Text = "";
        ddlShopType.Text = "";
        txtPeriod.Text = DateTime.Now.ToString("yyyy-MM-01");
       // txtPeriod.Text = DateTime.Now.ToShortDateString();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {

            ParameterFields paraFields = new ParameterFields();
            ParameterField[] paraField = new ParameterField[9];
            ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[9];
            ParameterRangeValue rangeValue = new ParameterRangeValue();
            paraField[0] = new ParameterField();
            paraField[0].ParameterFieldName = "REXPeriod"; //记账月
            discreteValue[0] = new ParameterDiscreteValue();
            discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "InvAdj_KeepAccountsMth");
            paraField[0].CurrentValues.Add(discreteValue[0]);

            paraField[1] = new ParameterField();
            paraField[1].Name = "REXShopType";//商铺类型
            discreteValue[1] = new ParameterDiscreteValue();
            discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType");
            paraField[1].CurrentValues.Add(discreteValue[1]);

            paraField[2] = new ParameterField();
            paraField[2].Name = "REXTitle";//标题
            discreteValue[2] = new ParameterDiscreteValue();
            discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreShopTypeRentalThan");
            paraField[2].CurrentValues.Add(discreteValue[2]);

            paraField[3] = new ParameterField();
            paraField[3].Name = "REXMallTitle";
            discreteValue[3] = new ParameterDiscreteValue();
            discreteValue[3].Value = Session["MallTitle"].ToString();
            paraField[3].CurrentValues.Add(discreteValue[3]);

            paraField[4] = new ParameterField();
            paraField[4].Name = "REXBusinessItem";
            discreteValue[4] = new ParameterDiscreteValue();
            discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");//商业项目
            paraField[4].CurrentValues.Add(discreteValue[4]);

            paraField[5] = new ParameterField();
            paraField[5].Name = "REXSellCount";
            discreteValue[5] = new ParameterDiscreteValue();
            discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "ConLease_labSellCount");//销售额
            paraField[5].CurrentValues.Add(discreteValue[5]);

            paraField[6] = new ParameterField();
            paraField[6].Name = "REXFixedRental";
            discreteValue[6] = new ParameterDiscreteValue();
            discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "ConLease_labFixedRental");//租金额
            paraField[6].CurrentValues.Add(discreteValue[6]);

            paraField[7] = new ParameterField();
            paraField[7].Name = "REXRentalThan";
            discreteValue[7] = new ParameterDiscreteValue();
            discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "ConLease_labRentalThan");//租售比
            paraField[7].CurrentValues.Add(discreteValue[7]);

            paraField[8] = new ParameterField();
            paraField[8].Name = "REXCount";
            discreteValue[8] = new ParameterDiscreteValue();
            discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");//合计
            paraField[8].CurrentValues.Add(discreteValue[8]);
        
            foreach (ParameterField pf in paraField)
            {
                paraFields.Add(pf);
            }


            string wherestr = " ";
            string str_sql = "";
            //string whereShop = "";
            if (ddlStoreName.Text!="")
            {
                wherestr = " AND aa.storeid='" + ddlStoreName.SelectedValue + "'";
            }
            if (ddlShopType.Text != "")
            {
                wherestr = wherestr + " AND transshopmth.shoptypename='" + ddlShopType.SelectedValue + "'";
            }
            if (txtPeriod.Text != "")
            {
                wherestr =wherestr+" AND aa.period='"+txtPeriod.Text+"' ";
            }

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            string func_sql = "";


            if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
            {
                func_sql = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                            ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                            ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                            ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID +
                            ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
                for (int i = 0; i < 5; i++)
                {
                    func_sql = func_sql.Replace("ConShop", "aa");
                }
            }

            str_sql =   "select transshopmth.storename,transshopmth.shoptypename,aa.period,sum(transshopmth.paidamt) as paidamt, " +	 //销售额
                                "sum(aa.invamt) as invamt, " +//租金额
                                "(case when sum(aa.invamt)<>0 then sum(aa.invamt)/sum(transshopmth.paidamt) else 0 end) *100 as Rate " + //租售比
                                "from transshopmth " +
                                "inner join (select conshop.storeid, conshop.shopid,invoicedetail.period,sum(invoicedetail.invactpayamtl) invamt from invoicedetail " +
                                    "inner join invoiceheader on (invoicedetail.invid=invoiceheader.invid) " +
                                    "inner join conshop on (conshop.contractid=invoiceheader.contractid) " +
                                    "where conshop.shopstatus=1 and invoicedetail.chargetypeid in (select chargetypeid from chargetype where chargeclass=1) " +
                                    " group by invoicedetail.period,conshop.shopid,conshop.storeid" +
                                ") aa on (aa.shopid=transshopmth.shopid and aa.period=transshopmth.month) " +
                        "where 1=1 " +wherestr+func_sql+
                        " group by transshopmth.storename,transshopmth.shoptypename,aa.period ";

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\Rpt_StoreShopTypeRentalThan.rpt";

    }


}
