using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Store;
using Lease.Subs;
using BaseInfo.Dept;
using Lease.ConShop;

public partial class ReportM_RptInv_RptMoreContractQuery : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.txtBrand.Attributes.Add("onclick", "selectShopBrand()");
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_MoreContractQuery");//标题
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        
        string str_sql = @"select contract.contractcode,customer.custcode,customer.custname,traderelation.tradename,
                            case contract.bizmode when 1 then '租赁' when 2 then '联营' end bizmode,
                            contract.constartdate,contract.conenddate,
                            conshop.shopcode,conshop.shopname,dbo.getcontractrentarea(contract.contractid) rentArea,
                            conshopbrand.brandname,shoptype.shoptypename,Subsidiary.subsname,store.storename
                            from contract
                            inner join customer on (contract.custid=customer.custid)
                            inner join traderelation on (traderelation.tradeid=contract.tradeid)
                            inner join Subsidiary on (contract.subsid=Subsidiary.subsid)
                            left join conshop on (conshop.contractid=contract.contractid)
                            left join conshopbrand on (conshopbrand.brandid=conshop.brandid)
                            left join shoptype on (shoptype.shoptypeid=conshop.shoptypeid)
                            left join store on (conshop.storeid=store.storeid)
                            where contract.contractstatus=2 ";
        if(this.txtCustName.Text.Trim().Length>0)
        {
            str_sql += " And customer.custname like '%" + this.txtCustName.Text.Trim() + "%'";
        }

        if (this.allvalue.Value != "")//品牌
        {
            str_sql += " AND conshopbrand.brandid='" + this.allvalue.Value + "'";
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
        }
        str_sql += strAuth;

        str_sql += "order by store.storename,contract.contractcode ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        if(this.rbBrand.Checked==true)
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptMoreContractQuery.rpt";
        else
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptMoreContractQueryCustName.rpt";
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptInv/RptMoreContractQuery.aspx");
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "BrandId = " + Convert.ToInt32(allvalue.Value);
        Resultset rs = objBaseBo.Query(new ConShopBrand());
        if (rs.Count == 1)
        {
            ConShopBrand objConShopBrand = rs.Dequeue() as ConShopBrand;
            this.txtBrand.Text = objConShopBrand.BrandName;
            ViewState["brandID"] = allvalue.Value.ToString();
        }
        objBaseBo.WhereClause = "";
    }
}
