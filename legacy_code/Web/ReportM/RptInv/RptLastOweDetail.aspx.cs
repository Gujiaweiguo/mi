using System;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using RentableArea;
using BaseInfo.authUser;
using BaseInfo.User;
using Invoice;

public partial class ReportM_RptInv_RptLastOweDetail : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            InitDDL();
            ddlFloorName.Enabled = false;
            ddlFloorName.Items.Add(new ListItem("", "-1"));
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblLastOweDetail");
        }
    }

    private void InitDDL()
    {
        //绑定楼

        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Building());
        ddlBuildingName.Items.Add(new ListItem("-请选择-", "-1"));
        foreach (Building bd in rs)
            ddlBuildingName.Items.Add(new ListItem(bd.BuildingName, bd.BuildingID.ToString()));
    }

    private void BindFloors()
    {
        //绑定楼层
        ddlFloorName.Items.Clear();
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "BuildingID = " + ddlBuildingName.SelectedValue;
        Resultset rs1 = baseBo.Query(new Floors());
        ddlFloorName.Items.Add(new ListItem("", "-1"));
        foreach (Floors bd in rs1)
            ddlFloorName.Items.Add(new ListItem(bd.FloorName, bd.FloorID.ToString()));
    }
    protected void ddlBuildingName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBuildingName.SelectedValue == "-1")
        {
            ddlFloorName.Items.Clear();
            ddlFloorName.Enabled = false;
        }
        else
        {
            BindFloors();
            ddlFloorName.Enabled = true;
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {

    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXContractID";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptContractInfo_ContractID");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXCustName";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CustName");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXShopCode";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopCode");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXInvThisCost";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvThisCost");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXAgoArrear";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_AgoArrear");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXInvInterest";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvInterest");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXPayAmt";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptSale_PayAmt");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXSInvPaidAmt";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvPaidAmt");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXOweAmt";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_OweAmt");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXTitleDetail";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_OweDetailTitle");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXSdate";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXShopName";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ShopName");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXAmount";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[12].CurrentValues.Add(discreteValue[12]);


        paraField[13] = new ParameterField();
        paraField[13].Name = "REXMallTitle";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = Session["MallTitle"].ToString();
        paraField[13].CurrentValues.Add(discreteValue[13]);

        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string whereStr1 = "";
        string whereStr2 = "";
        string whereStr3 = "";
        string whereStr4 = "";
        string whereStr5 = "";

        //string sql = "(SELECT MAX(InvPeriod) FROM InvoiceHeader,Contract WHERE InvoiceHeader.ContractID = Contract.ContractID AND Contract.ContractCode = '" + txtContractID.Text + "')";

        if (txtContractID.Text != "" && txtPeriod.Text == "")
        {
            //whereStr1 = " AND InvoiceHeader.InvPeriod < " + sql;
            whereStr2 = " AND Contract.ContractCode = '" + txtContractID.Text + "'";
        }

        //if (txtPeriod.Text != "" && txtContractID.Text == "")
        //{
        //    whereStr1 = " AND InvoiceHeader.InvPeriod < '" + txtPeriod.Text + "'";
        //}

        if (txtContractID.Text != "" && txtPeriod.Text != "")
        {
            //whereStr1 = " AND InvoiceHeader.InvPeriod < '" + txtPeriod.Text + "'";
            whereStr2 = " AND Contract.ContractCode = '" + txtContractID.Text + "'";
        }

        //if (txtContractID.Text == "" && txtPeriod.Text == "")
        //{
        //    whereStr4 = " INNER JOIN (SELECT MAX(InvPeriod) as InvPeriod,InvoiceHeader.contractid " +
        //                   " FROM InvoiceHeader,Contract" +
        //                    "  WHERE InvoiceHeader.ContractID = Contract.ContractID" +
        //                    " group by InvoiceHeader.contractid) as InvPer " +
        //                    " ON (InvPer.contractid = InvoiceHeader.contractid)";
        //    whereStr5 = " AND InvoiceHeader.InvPeriod < InvPer.InvPeriod";
        //}

        if (ddlFloorName.SelectedValue != "-1")
        {
            whereStr3 = " AND Floors.floorID = '" + ddlFloorName.SelectedValue + "'";
        }

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string authWhere = "";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            authWhere = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID + ")";
        }

        string str_sql = " SELECT ChargeTypeName," +
                               " CASE WHEN InvAmt.ChargeTypeID = 101 THEN '抽成租金'" +
                               " ELSE ''" +
                               " END AS SubTypeName," +
                                " InvAmt.sAmt as Amt," +
                                " Contract.ContractCode," +
                                " ConShop.ShopCode,ConShop.ShopName,InvAmt.InvPeriod" +
                        " FROM InvoiceHeader" +
                        " RIGHT JOIN " +
                        " (" +
                            " SELECT (InvoiceDetail.InvActPayAmt - InvoiceDetail.InvPaidAmt) AS sAmt," +
                                    " ChargeType.ChargeTypeName,InvoiceHeader.InvID," +
                                    " InvoiceHeader.ContractID,ChargeType.ChargeTypeID,InvoiceHeader.InvPeriod,InvoiceDetail.RentType" +
                            " FROM InvoiceDetail" +
                            " INNER JOIN InvoiceHeader ON (InvoiceHeader.InvID = InvoiceDetail.InvID)" +
                            " INNER JOIN ChargeType ON (InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID)" + whereStr4 +
                            " WHERE InvoiceDetail.InvActPayAmt > 0" +
                            " AND InvoiceDetail.InvActPayAmt != InvoiceDetail.InvPaidAmt" +
                            " AND InvoiceDetail.RentType IN (" + InvoiceDetail.RENTTYPE_FIXED_P + "," + InvoiceDetail.RENTTYPE_SINGLE_P + "," + InvoiceDetail.RENTTYPE_MUNCH_P + ")" + whereStr1 + whereStr5 +
                        " ) AS InvAmt ON (InvoiceHeader.InvID = InvAmt.InvID)" +
                        " INNER JOIN Contract ON (InvoiceHeader.ContractID = Contract.ContractID)" +
                        " INNER JOIN ConShop ON (Contract.ContractID = ConShop.ContractID)" +
                        " INNER JOIN Floors ON (ConShop.FloorID = Floors.FloorID)"+ whereStr2 + whereStr3 +
                            
                        " UNION ALL" +

                        " SELECT InvAmt.ChargeTypeName," +
                        " CASE WHEN InvAmt.ChargeTypeID = 101 THEN '固定租金'" +
                       " ELSE ''" +
                       " END AS SubTypeName," +
                        " InvAmt.Amt," +
                        " Contract.ContractCode," +
                        " ConShop.ShopCode,ConShop.ShopName,InvAmt.InvPeriod" +
                        " FROM InvoiceHeader" +
                        " INNER JOIN " +
                        " (" +
                            " SELECT (InvoiceDetail.InvActPayAmt - InvoiceDetail.InvPaidAmt) AS Amt," +
                                    " ChargeType.ChargeTypeName,InvoiceHeader.InvID," +
                                    " InvoiceHeader.ContractID,ChargeType.ChargeTypeID,InvoiceHeader.InvPeriod,InvoiceDetail.RentType" +
                            " FROM InvoiceDetail" +
                            " INNER JOIN InvoiceHeader ON(InvoiceHeader.InvID = InvoiceDetail.InvID)" +
                            " INNER JOIN ChargeType ON (InvoiceDetail.ChargeTypeID = ChargeType.ChargeTypeID)" + whereStr4 +
                            " WHERE InvoiceDetail.InvActPayAmt > 0" +
                            " AND InvoiceDetail.InvActPayAmt != InvoiceDetail.InvPaidAmt" +
                            " AND InvoiceDetail.RentType NOT IN (" + InvoiceDetail.RENTTYPE_FIXED_P + "," + InvoiceDetail.RENTTYPE_SINGLE_P + "," + InvoiceDetail.RENTTYPE_MUNCH_P + ")" + whereStr1 + whereStr5 +
                        " ) AS InvAmt ON (InvoiceHeader.InvID = InvAmt.InvID)" + 
                        " INNER JOIN Contract ON (InvoiceHeader.ContractID = Contract.ContractID)" + 
                        " INNER JOIN ConShop ON (Contract.ContractID = ConShop.ContractID)" + 
                        " INNER JOIN Floors ON (ConShop.FloorID = Floors.FloorID)" + whereStr2 + whereStr3;

                        


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\LastOweDetail.rpt";

    }
}
