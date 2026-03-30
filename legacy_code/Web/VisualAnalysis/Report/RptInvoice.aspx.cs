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
using Base.DB;
using Base.Biz;
using RentableArea;
using BaseInfo.User;
using BaseInfo.authUser;


public partial class VisualAnalysis_Report_RptInvoice:BasePage
{
    public string baseInfo = "";
    private string  buildingID = "";
    private string  floorID = "";
    private string BuildingName = "";
    private string FloorName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome");
        if (Request.QueryString["BuildingID"] != ""&&Request.QueryString["BuildingID"] !=null )
        {
            buildingID = Request.QueryString["BuildingID"];
            txtHidden.Value = baseInfo + ",Disktop.aspx";

        }
        if (Request.QueryString["FloorID"] != "" && Request.QueryString["FloorID"] != null)
        {
            floorID = Request.QueryString["FloorID"];
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = " Floors.FloorID=" + floorID;
            Resultset rs = baseBo.Query(new Floors());
            if (rs.Count == 1)
            {
                Floors floor = rs.Dequeue() as Floors;
                FloorName = floor.FloorName;
            }
            txtHidden.Value = baseInfo + ",Disktop.aspx?FloorID=" + floorID + "&FloorName=" + FloorName;
        }
        //else
        //{
        //    Response.Redirect("../../Disktop.aspx");
        //}

        if (!IsPostBack)
        {

            InitDDL();
            
        }
    }
    
    private void InitDDL()
    {
        txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";

        if (txtStartDate.Text.Trim() != null)
        {
            BindData();
            this.Response.Redirect("../../ReportM/ReportShow.aspx");
        }
        else
        {
            return;
        }

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        InitDDL();
    }

    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[3];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[3];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXMainTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = Session["MallTitle"].ToString();
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_RptInvoiceheader");
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        if(buildingID!="")
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = " Building.BuildingID = " + buildingID;
            Resultset rs = baseBo.Query(new Building());
            if (rs.Count == 1)
            {
                Building building = rs.Dequeue() as Building;
                BuildingName = building.BuildingName;

            }
            Field[2] = new ParameterField();
            Field[2].Name = "REXBuilding&FloorName";
            DiscreteValue[2] = new ParameterDiscreteValue();
            DiscreteValue[2].Value = BuildingName;
            Field[2].CurrentValues.Add(DiscreteValue[2]);
        }


        if (floorID!="")
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = " Floors.FloorID = " + floorID;
            Resultset rs = baseBo.Query(new Floors());
            if (rs.Count == 1)
            {
                Floors floor = rs.Dequeue() as Floors;
                FloorName = floor.FloorName;
            }
            Field[2] = new ParameterField();
            Field[2].Name = "REXBuilding&FloorName";
            DiscreteValue[2] = new ParameterDiscreteValue();
            DiscreteValue[2].Value = FloorName;
            Field[2].CurrentValues.Add(DiscreteValue[2]);

        }
        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string str_sql = "";
        string wheretime = "";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string authWhere = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP.Replace( "TransSku","ConShop") + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD.Replace("TransSku", "ConShop") + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR.Replace("TransSku", "ConShop") + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT.Replace("TransSku", "ConShop") + sessionUser.UserID + ")";
        }
        if (txtStartDate.Text.ToString() != ""&&txtEndDate.Text.ToString()!="")
        {
            string wherestart = txtStartDate.Text.Substring(0, 8) + "01";
            string whereend = txtEndDate.Text.Substring(0, 8) + "01";
            wheretime = "and invoiceDetail.period between '" + wherestart + "'and '"+whereend+"'";
        }
        
        if (floorID != "")
        {
            str_sql = "SELECT invoiceDetail.period," +
                     " sum(invoiceDetail.invActPayAmtL) as invActPayAmtL ," +
                     " sum(invoiceDetail.invPaidAmtL) as invPaidAmtL" +
                     " FROM invoiceHeader " +
                     "INNER JOIN invoiceDetail ON (invoiceHeader.invID = invoiceDetail.invID) inner join conshop on (conshop.contractid=invoiceHeader.contractid) " +
                     "WHERE 1=1 and invoiceHeader.contractID in (SELECT contractID from conShop WHERE Floorid in ('" + floorID + "')) " + wheretime + authWhere + " group by period";
        }
        if (floorID == "")
        {
            str_sql = "select invoiceDetail.period,sum(invoiceDetail.invActPayAmtL) as invActPayAmtL,sum(invoiceDetail.invPaidAmtL) as invPaidAmtL" +
            " from invoiceDetail " +
            " INNER JOIN ChargeType ON (invoiceDetail.chargeTypeID = ChargeType.chargeTypeID ) " +
            " INNER JOIN invoiceHeader ON (invoiceHeader.invID = invoiceDetail.invID) "+
            " inner join conshop on (conshop.contractid=invoiceHeader.contractid) "+
            " where 1=1  " + wheretime + authWhere + " group by invoiceDetail.period";
        
        }

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\VisualAnalysis\\Report\\RptInvoiceHeader.rpt";
    }
     
}
