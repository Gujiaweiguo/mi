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

using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;
using Base.DB;
using Base.Biz;
using BaseInfo.Store;
using RentableArea;
using Lease.ConShop;
using BaseInfo.Store;

public partial class ReportM_RptBase_RptEmphasisContract : BasePage
{
    public string baseInfo;
    public string fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_EmphasisContract");
            fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            BindStore();
        }
    }

    private void BindStore()
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "storestatus=1";
        baseBo.OrderBy = "orderid";
        ddlProjuct.Items.Clear();
        ddlProjuct.Items.Add(new ListItem("", ""));
        Resultset rs = baseBo.Query(new Store());
        foreach (Store store in rs)
        {
            ddlProjuct.Items.Add(new ListItem(store.StoreShortName, store.StoreId.ToString()));
        }
    }
   
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptBase/RptEmphasisContract.aspx");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        BindData();
        Response.Redirect("../ReportShow.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXMallTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = Session["MallTitle"].ToString();
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_EmphasisContract");//重点关注合同列表
        paraField[1].CurrentValues.Add(discreteValue[1]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string where = "";
        if (ddlProjuct.Text != "")
        {
            where += " and store.storeid='" + ddlProjuct.SelectedValue.Trim() + "' ";
        }
        string str_sql = @"select ROW_NUMBER() OVER(ORDER BY Store.orderID) AS rownum,Store.StoreShortName as StoreName,Conshop.ShopCode,ConShop.ShopName,TradeRelation.TradeName,Conshop.RentArea,ConShopBrand.BrandName,Contract.ContractCode,convert(char(10),Contract.ConStartDate,120) as ConStartDate,convert(char(10),Contract.ConEndDate,120) as ConEndDate from Contract
                            inner join conshop on conshop.contractid=contract.Contractid
                            inner join TradeRelation on TradeRelation.TradeID=Contract.TradeID
                            inner join ConShopBrand on ConShopBrand.BrandID = Conshop.BrandID
                            inner join Store on Store.StoreID=ConShop.StoreID
                            where  Conshop.RentArea>300 and datediff(year,Contract.ConStartDate,Contract.ConEndDate)>2 " + where;
        
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            string[] arr = new string[5];
            arr[0] = AuthBase.AUTH_SQL_SHOP;
            arr[1] = AuthBase.AUTH_SQL_BUILD;
            arr[2] = AuthBase.AUTH_SQL_FLOOR;
            arr[3] = AuthBase.AUTH_SQL_CONTRACT;
            arr[4] = AuthBase.AUTH_SQL_STORE;
            string strAND = "";
            for (int i = 0; i < arr.Length; i++)
            {
                strAND += " AND EXISTS (" + arr[i].ToString().Replace("ConShop", "b") + sessionUser.UserID + ")";
            }
            str_sql += strAND;
        }

        str_sql+=" order by Store.orderID";
       
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\RptEmphasisContract.rpt";
    }
}
