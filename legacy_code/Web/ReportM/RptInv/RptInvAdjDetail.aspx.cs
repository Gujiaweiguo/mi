using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.Shared;

using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;
using Invoice.InvoiceH;
using Lease.ConShop;

/// <summary>
/// Author:hesijian
/// Date:2009-11-04
/// Content:Created AND Modify
/// </summary>
public partial class ReportM_RptInv_RptInvAdjDetail : BasePage
{
    public string baseInfo;  //基本信息
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        txtShopCode.Attributes.Add("onclick", "ShowTree(LinkButton1)");
    }

    private void BindData()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[21];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[21];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_InvAdjDetail");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMallTitle";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = Session["MallTitle"].ToString();
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXPrintDate";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_PrtDate");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXStartDate";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = txtStartDate.Text.Trim();
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXEndDate";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = txtEndDate.Text.Trim();
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXQueryDate";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("ReportInfo", "RptFloatSaleQuery_Sdate");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXInvoiceID";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_InvCode");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXChargeName";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_ChargeName");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXInvPayAmt";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvPayAmt");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXInvAdjAmt";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvAdjAmt");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXInvActPayAmt";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_InvActPayAmt");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXAdjUserName";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_AdjUserName");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXCheckUserName";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CheckUserName");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXCreateDate";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CreateDate");
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXCheckDate";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_CheckDate");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXAdjReason";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_AdjReason");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXTotal";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[16].CurrentValues.Add(discreteValue[16]);

        paraField[17] = new ParameterField();
        paraField[17].Name = "REXSign";
        discreteValue[17] = new ParameterDiscreteValue();
        discreteValue[17].Value = GetSign();
        paraField[17].CurrentValues.Add(discreteValue[17]);

        paraField[18] = new ParameterField();
        paraField[18].Name = "REXContractCode";
        discreteValue[18] = new ParameterDiscreteValue();
        discreteValue[18].Value = (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID");
        paraField[18].CurrentValues.Add(discreteValue[18]);

        paraField[19] = new ParameterField();
        paraField[19].Name = "REXShopCode";
        discreteValue[19] = new ParameterDiscreteValue();
        discreteValue[19].Value = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblShopCode");
        paraField[19].CurrentValues.Add(discreteValue[19]);

        paraField[20] = new ParameterField();
        paraField[20].Name = "REXShopName";
        discreteValue[20] = new ParameterDiscreteValue();
        discreteValue[20].Value = (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName");
        paraField[20].CurrentValues.Add(discreteValue[20]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_orderby = "";
        string str_Authsql = "";
        string str_andsql1 = "";
        string str_andsql2 = "";

       

        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(objSessionUser.UserID) > 0)
        {
            str_Authsql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + objSessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + objSessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + objSessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + objSessionUser.UserID + ")";
        }

       

        if (txtStartDate.Text.Trim() != "")
        {
            str_andsql2 += " AND InvAdj.CreateTime >= '" + txtStartDate.Text.Trim() + "' ";
        }

        if (txtEndDate.Text.Trim() != "")
        {
            str_andsql2 += " AND InvAdj.CreateTime <= '" + txtEndDate.Text.Trim() + "' ";
        }

        if (txtContractID.Text.Trim() != "")
        {
            str_andsql2 += " AND Contract.ContractCode = '" + txtContractID.Text + "' ";
        }

        if (txtAccountMon.Text.Trim() != "")
        {
            str_andsql2 += " AND InvoiceHeader.InvPeriod = '" + txtAccountMon.Text.Trim() + "'";
        }

        if (txtShopCode.Text.Trim() != "")
        {
            str_andsql2 += " AND ConShop.ShopCode = '" + ViewState["shopcode"].ToString() + "'";
        }


        if (txtInvID.Text == "" && txtStartDate.Text == "" && txtEndDate.Text == "" && txtContractID.Text == "" && txtShopCode.Text == "" && txtAccountMon.Text == "")
        {
            str_andsql1 += " AND InvAdj.InvID IN (SELECT InvID FROM InvoiceHeader WHERE InvStatus != '" + Invoice.InvoiceHeader.INVSTATUS_CANCEL + @"' AND InvAdjAmt != 0)";
        
        }

        if (txtInvID.Text != "")
        {
            str_andsql1 += " AND InvAdj.InvID IN (" + txtInvID.Text + ")";
        
        }

        str_orderby = " ORDER BY ConShop.ShopCode,Contract.ContractCode,InvoiceDetail.InvID";

        string str_sql = @"SELECT InvoiceDetail.InvID,InvoiceDetail.ChargeTypeID,InvoiceHeader.InvPeriod,ConShop.ShopCode,                               ConShop.ShopName,Contract.ContractCode,
                            ChargeType.ChargeTypeName,
                            InvAdjDet.InvPayAmt,InvAdjDet.AdjAmt AS InvAdjAmt,InvAdjDet.InvActPayAmt,
                            InvAdj.CreateUserID,InvAdj.CreateTime,Users.UserName,WrkFlw.completedtime,
                            WrkFlw.UserName as CheckUserName,InvAdjDet.AdjReason,WrkFlw.invadjid
                            FROM InvAdjDet
                            LEFT JOIN 
                            (
	                            SELECT WrkFlwEntity.UserID,InvAdj.InvAdjID,Users.UserName,WrkFlwEntity.completedtime FROM WrkFlwEntity,InvAdj,Users
	                            WHERE WrkFlwEntity.VoucherID = InvAdj.InvAdjID
	                            AND Users.UserID = WrkFlwEntity.UserID
                                AND WrkFlwEntity.VoucherID = InvAdj.InvAdjID
                                AND WrkFlwEntity.wrkflwid IN(172,144,145,146) 
                                AND WrkFlwEntity.nodeid in
                                                        (SELECT max(nodeid) FROM WrkFlwEntity,InvAdj
								                        WHERE InvAdj.InvAdjID = WrkFlwEntity.VoucherID 
								                        AND WrkFlwEntity.wrkflwid IN(172,144,145,146) 
								                        " + str_andsql1 + @"
								                        AND adjstatus = " + InvAdj.INVADJ_UPDATE_LEASE_STATUS + @" GROUP BY VoucherID)
                                AND adjstatus = " + InvAdj.INVADJ_UPDATE_LEASE_STATUS + @")                                                                  AS WrkFlw ON (WrkFlw.InvAdjID = InvAdjDet.InvAdjID)
                            INNER JOIN InvoiceDetail ON (InvAdjDet.InvDetailID = InvoiceDetail.InvDetailID)
                            INNER JOIN InvoiceHeader ON (InvoiceHeader.InvID = InvoiceDetail.InvID)
                            INNER JOIN Contract ON (InvoiceHeader.ContractID = Contract.ContractID)
                            INNER JOIN ConShop ON (ConShop.ContractID = Contract.ContractID)
                            INNER JOIN InvAdj ON (InvAdjDet.InvAdjID = InvAdj.InvAdjID)
                            INNER JOIN ChargeType ON (InvoiceDetail.ChargeTypeid = ChargeType.ChargeTypeID)
                            INNER JOIN Users ON (Users.UserID = InvAdj.CreateUserID)
                            WHERE  InvoiceDetail.InvAdjAmt != 0
                            AND InvAdj.AdjStatus = " + InvAdj.INVADJ_UPDATE_LEASE_STATUS +str_andsql1+ str_andsql2;


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql + str_Authsql + str_orderby;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Inv\\RptInvAdjDetail.rpt";

    }

    //清除页面
    private void ClearPage()
    {
        txtContractID.Text = "";
        txtInvID.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtAccountMon.Text = "";
        txtShopCode.Text = "";
    }

    //得到Sign
    private String GetSign()
    {
        string s = "";
        if (txtStartDate.Text != "" && txtEndDate.Text != "")
        {
            s = "---";
            return s;

        }
        else
        {
            s = "";
        
        }
        return s;
    }

    //查询操作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (txtStartDate.Text != "" && txtEndDate.Text != "")
        {
            DateTime dt1 = Convert.ToDateTime(txtStartDate.Text.Trim());

            DateTime dt2 = Convert.ToDateTime(txtEndDate.Text.Trim());

            if (dt1.CompareTo(dt2) > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Message", "alert('" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime") + "')", true);
                return;
            }
        }
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx"); ;
    }

    //撤销操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopcode"] = ds.Tables[0].Rows[0]["ShopCode"].ToString();
            txtShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
        }
    }
}
