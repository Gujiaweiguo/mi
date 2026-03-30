using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using Lease.ConShop;
using BaseInfo.authUser;
using BaseInfo.User;
using BaseInfo.Dept;
using BaseInfo.Store;

public partial class ReportM_RptSale_RptStoreSaleMth : System.Web.UI.Page
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindBizProject();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreSaleMth");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }

    private void BindBizProject()
    {
        BaseBO baseBo = new BaseBO();
        //baseBo.WhereClause = " depttype='" + Dept.DEPT_TYPE_MALL + "' ";
        //baseBo.OrderBy = " orderid ";
        //Resultset rs = baseBo.Query(new Dept());
        //ddlBizproject.Items.Clear();
        //foreach (Dept dept in rs)
        //{
        //    ddlBizproject.Items.Add(new ListItem(dept.DeptName, dept.DeptID.ToString()));
        //}
        baseBo.WhereClause = "StoreStatus=1";
        baseBo.OrderBy = "orderid";
        Resultset rs = baseBo.Query(new Store());
        ddlBizproject.Items.Clear();
        foreach (Store objStore in rs)
        {
            ddlBizproject.Items.Add(new ListItem(objStore.StoreName, objStore.StoreId.ToString()));
        }
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";

        DdlYear.Items.Clear();
        int y = DateTime.Now.Year;
        for (int m = y - 2; m < y + 3; m++)
        {
            DdlYear.Items.Add(new ListItem(m.ToString(), m.ToString()));
        }
        DdlYear.SelectedValue = y.ToString();

        DdlMonth.Items.Clear();
        for (int i = 1; i < 13; i++)
        {
            DdlMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
        DdlMonth.SelectedValue = DateTime.Now.Month.ToString();
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptSale/RptStoreSaleMth.aspx");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[3];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_StoreSaleMth");//标题
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();//集团名称
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXStore";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = ddlBizproject.SelectedItem.ToString();//项目名称
        paraField[2].CurrentValues.Add(discreteValue[2]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = "";
        str_sql = @"select transshopmth.shopcode,transshopmth.shopname,transshopmth.tradename,conshop.rentarea, 
                    transshopmth.totalreceipt,transshopmth.paidamt,bb.paidamt lypamt,aa.paidamt lmpamt,transshopmth.month
                    from transshopmth 
                    inner join conshop on transshopmth.shopid=conshop.shopid
                    left join (select shopid,shopname,paidamt from transshopmth where storeid='" + ddlBizproject.SelectedValue.Trim()+"' and month='"+DdlYear.Text+"-"+(Convert.ToInt32(DdlMonth.Text)!=1 ? Convert.ToInt32(DdlMonth.Text) -1:1)+@"-01') aa
		                    on aa.shopid=transshopmth.shopid
                    left join (select shopid,shopname,paidamt from transshopmth where storeid='" + ddlBizproject.SelectedValue.Trim() + "' and month='" + (Convert.ToInt32(DdlYear.Text) - 1) + "-" + DdlMonth.Text + @"-01') bb
		                    on bb.shopid=transshopmth.shopid
                    where transshopmth.storeid='" + ddlBizproject.SelectedValue.Trim() + "' and month='" + DdlYear.Text + "-" + DdlMonth.Text + @"-01'
                    order by shopcode";

        if (this.ddlBizproject.Text != "")
        {
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

            for (int i = 0; i < 5; i++)
            {
                strAuth = strAuth.Replace("ConShop", "TransShopMth");//将权限查询Sql中Conshop替换成查询表的名称，该表有ShopID字段
            }
        }

        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Sale\\RptStoreSaleMth.rpt";
    }
}
