using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class ReportM_RptInv_RptPOweDetail : BasePage
{
    public string baseInfo;
    public string pageTitle;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!this.IsPostBack)
        {
            BindFloor();
            pageTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblLastOweDetail");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtContractID.Text = "";
        txtPeriod.Text = "";
    }

    protected void BindFloor()
    {

        //绑定楼层
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "FloorStatus = " + Floors.FLOOR_STATUS_VALID + " AND BuildingID = '101'";
        Resultset rs = baseBO.Query(new Floors());
        cmbFloorID.Items.Add(new ListItem("", ""));
        foreach (Floors bd in rs)
        {
            cmbFloorID.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
        }
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

     

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblLastOweDetail");
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
       
        string whereStr1 = "";
        string whereStr2 = "";
     
        if (txtContractID.Text != "" && txtPeriod.Text == "")
        {
            whereStr1 = " AND Contract.ContractCode = '" + txtContractID.Text + "'";

        }

        if (txtPeriod.Text != "" && txtContractID.Text == "")
        {
            whereStr1 = " AND invoiceDetail.Period = '" + txtPeriod.Text + "'";
        }
        if (txtContractID.Text != "" && txtPeriod.Text != "")
        {
                whereStr1 = " AND Contract.ContractCode = '" + txtContractID.Text +
                            "' AND invoiceDetail.Period = '" + txtPeriod.Text + "'";
         }
         if (cmbFloorID.SelectedValue != "")
         {
             whereStr2 = " AND invoiceHeader.contractID in (SELECT contractID from conShop WHERE floorID='" + cmbFloorID.SelectedValue + "') ";
         }
        

       

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            string authWhere = "";
            if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
            {
                authWhere = "AND invoiceHeader.contractID in (SELECT contractID from conShop WHERE floorID in(select floorid from authshop where userid='" + sessionUser.UserID + "')) ";
            }

            string str_sql = "select 1 as ID,contract.Contractcode, " +
                             "invoiceHeader.CustName, " +
                             "(SELECT MIN(shopCode) FROM conShop WHERE conShop.contractID = invoiceHeader.contractID) AS ShopCode, " +
                             "(SELECT MIN(shopName) FROM conShop WHERE conShop.contractID = invoiceHeader.contractID) AS ShopName, " +
                             "invoiceDetail.Period, " +
                             "(SELECT ChargeTypeName FROM ChargeType WHERE ChargeType.ChargeTypeID = invoiceDetail.ChargeTypeID) AS ChargeTypeName, " +
                             "SUM(invoiceDetail.invActPayAmtL - invoiceDetail.invPaidAmtL) AS InvAmtL "  +
                             "from invoiceHeader INNER JOIN " +
                             "invoiceDetail ON (invoiceHeader.invID = invoiceDetail.invID) INNER JOIN CONTRACT ON " +
                             "(invoiceHeader.contractID=contract.contractid) " +
                             "where invoiceDetail.invDetStatus in (1,2)  " + authWhere + whereStr1 + whereStr2 +
                              "group by  contract.contractid,invoiceDetail.ChargeTypeID,invoiceHeader.contractID,contract.contractcode, " +
                             "invoiceHeader.custID,invoiceHeader.CustName,invoiceDetail.Period ";


            Session["paraFil"] = paraFields;
            Session["sql"] = str_sql;
            Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\POweDetail.rpt";


    }
}
