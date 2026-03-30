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

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.PotBargain;
using Lease.PotCust;
using RentableArea;
using Base.Util;
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Store;

public partial class ReportM_RptBase_RptPalaver : BasePage
{
    public string baseInfo;
    public string Fresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtPalaverTime.Text = DateTime.Now.Date.ToShortDateString();
            InitDDL();
           // bindBuilding();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_CustPalaver");
            Fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }

    }



    /* 初始化下拉列表 */
    private void InitDDL()
    {
        BaseBO baseBO = new BaseBO();

        //绑定商业项目
        ddlStoreName.Items.Clear();
        Resultset rs = baseBO.Query(new Store());
        ddlStoreName.Items.Add(new ListItem("", ""));
        foreach (Store bd in rs)
            ddlStoreName.Items.Add(new ListItem(bd.StoreName, bd.StoreId.ToString()));

        //招商进程级别
        ddlProcessTypeName.Items.Clear();
        Resultset rs1=baseBO.Query(new ProcessType());
        ddlProcessTypeName.Items.Add(new ListItem("", ""));
        foreach (ProcessType bd in rs1)
            ddlProcessTypeName.Items.Add(new ListItem(bd.ProcessTypeName, bd.ProcessTypeId.ToString()));


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
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXCustName";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidCustName");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXProcessTypeName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustAttractProcessType");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXPalaverRound";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustPalaverRound");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXPalaverPlace";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustPalaverPlace");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXPalaverName";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPalaverUser");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXContactorName";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_PalaverUser");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXPalaverAim";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustMostlyTitle");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXPalaverResult";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustPalaverResult");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXUnSolved";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustUnSolved");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXPalaverTime";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPalaverTime");
        paraField[9].CurrentValues.Add(discreteValue[9]);


        paraField[10] = new ParameterField();
        paraField[10].Name = "REXTitle";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_CustPalaver");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXMallTitle";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = Session["MallTitle"].ToString();
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXBizProject";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem");
        paraField[12].CurrentValues.Add(discreteValue[12]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }



        string str_sql = "select Store.storeshortname, " +
                        "potcustomer.custname, processtype.processtypename,custpalaver.PalaverRound,custpalaver.PalaverPlace, " +
                        "custpalaver.PalaverName,custpalaver.ContactorName,custpalaver.PalaverAim,custpalaver.PalaverResult, " +
                        "custpalaver.UnSolved,custpalaver.PalaverTime from CustPalaver " +
                        "inner join potcustomer on (potcustomer.custid=CustPalaver.custid) " +
                        "inner join processtype on (processtype.processtypeid=custpalaver.processtypeid) " +
                        "inner join potshop on (potcustomer.custid=potshop.custid) "+
                        "inner join Store on (potshop.storeid=Store.StoreID )" +
                       // "inner join potshopunit on (potshop.potshopid=potshopunit.potshopid) "+
                        "where 1=1 ";

        if (ddlStoreName.Text != "")
        {
            str_sql = str_sql + " AND store.storeid='"+ ddlStoreName.SelectedValue+"' ";
        }
        //if (ddlBuildingName.Text != "")
        //{
        //    str_sql = str_sql + " AND potshopunit.buildingid='"+ddlBuildingName.SelectedValue+"'";
        //}
        //if (txtIntentUnits.Text != "")
        //{
        //    str_sql = str_sql + " AND potshopunit.unitCode= '" +txtIntentUnits.Text.Trim().ToString()+"'";
        //}
        if (txtPotCustID.Text != "")
        {
            str_sql = str_sql + " AND potcustomer.custcode like '%" + txtPotCustID.Text.Trim().ToString() + "%' ";
        }
        if (txtPotCustName.Text != "")
        {
            str_sql = str_sql + " AND potcustomer.custname like '%"+txtPotCustName.Text.Trim().ToString()+"%' ";
        }
        if (ddlProcessTypeName.Text != "")
        {
            str_sql = str_sql + " AND processtype.processtypeid='"+ddlProcessTypeName.SelectedValue +"' ";
        }
        if (txtCommOper.Text != "")
        {
            str_sql = str_sql + " AND custpalaver.PalaverName = '" + txtCommOper.Text + "' ";
        }
        if (txtPalaverTime.Text != "")
        {
            str_sql = str_sql + " AND convert(varchar(10),custpalaver.PalaverTime,120) ='" + txtPalaverTime.Text.Trim().ToString() + "' ";
        }



        str_sql = str_sql + "order by CustPalaver. processtypeid ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\RptPalaver.rpt";

    }

    private void ClearPage()
    {
        InitDDL();
        bindBuilding();
        txtPalaverTime.Text = DateTime.Now.Date.ToShortDateString();
        txtPotCustID.Text = "";
        txtPotCustName.Text = "";
        txtIntentUnits.Text = "";
        txtCommOper.Text = "";
    }

    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
    protected void ddlStoreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBuilding();
    }
    protected void bindBuilding()
    {
        //绑定楼
        ddlBuildingName.Items.Clear();
        if (ddlStoreName.Text != "")
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "BuildingStatus = " + Building.BUILDING_STATUS_VALID + " AND StoreID = " + ddlStoreName.SelectedValue.ToString();
            Resultset rs = baseBO.Query(new Building());
            ddlBuildingName.Items.Add(new ListItem("", ""));
            foreach (Building bd in rs)
                ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
        }


    }
}
