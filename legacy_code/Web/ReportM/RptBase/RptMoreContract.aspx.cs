using System;
using System.Web.UI.WebControls;

using CrystalDecisions.Shared;

using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;
using Base.DB;
using Base.Biz;
using BaseInfo.Store;

public partial class ReportM_RptBase_RptMoreContract : BasePage
{
    public string baseInfo;
    public string fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_MoreContract");
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
        this.Response.Redirect("~/ReportM/RptBase/RptMoreContract.aspx");
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
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_MoreContract");
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
        if (txtCustName.Text != "")
        {
            where += " and customer.custshortname like '%" + txtCustName.Text.Trim() + "%' ";
        }
        string str_sql = @"select customer.custid,customer.custShortname as custname,customer.custname subsname,store.storeid,store.storeShortname as storename,contract.contractid,contract.contractcode,
                            conshop.shopcode,conshop.shopname,conshopbrand.brandname
                            from contract
                            inner join customer on customer.custid=contract.custid
                            left join conshop on (contract.contractid=conshop.contractid)
                            inner join store on (conshop.storeid=store.storeid)
                            left join conshopbrand on (conshopbrand.brandid=conshop.brandid)
                            where 1=1 " + where;
        
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

        str_sql += "order by customer.custname,store.orderid ";
       
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\RptMoreContract.rpt";
    }
}
