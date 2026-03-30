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

public partial class ReportM_RptBase_RptPosSvrinfoQuery : BasePage
{
    public string baseInfo;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_PosSvrinfoQuery");
            Fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
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
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_PosSvrinfoQuery");//RptPosSvrinfoQuery
        paraField[1].CurrentValues.Add(discreteValue[1]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = @"select ROW_NUMBER() OVER(ORDER BY Store.orderID) AS rownum,Possvrinfo.PosSvrID,Store.StoreName,Possvrinfo.PosSvrName,Possvrinfo.IP from possvrinfo
                            inner join store on store.storeid=possvrinfo.storeid ";
        
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
       
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] =  "..\\ReportM\\Report\\Mi\\Base\\RptPosSvrinfoQuery.rpt";
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        Response.Redirect("../ReportShow.aspx");
    }
}
