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

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease;
using Lease.Customer;
using Lease.Contract;
using Lease.CustLicense;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
using Lease.PotCust;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class ReportM_RptMember_RptMemberRedeemHList : BasePage
{
    public string baseInfo;  //基本信息

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

           
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_MemberRedeemHList");
            txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 8) + "01";
            txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtLCardID.Attributes.Add("onkeydown", "textleave()");
            txtMobileTel.Attributes.Add("onkeydown", "textleave()");
            
        }

    }







    protected void btnOK_Click(object sender, EventArgs e)
    {
        string mex = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");


        if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "" )
        {

            Session["subReportSql"] = "";
            Session["subRpt"] = "";
            BindData();
            this.Response.Redirect("../ReportShow.aspx"); 
        }
        else 
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + mex + "')", true);

        }


    }


    private void BindData()
    {

        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[18];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[18];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXMembID";//会员号Associator_AssociatorCode
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorCode");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMembCardID";//会员卡号Associator_lblAssociatorCard
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblAssociatorCard");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXMemberName";//会员姓名Associator_MemberName
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_MemberName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXMobilPhone";//移动电话 Associator_AssociatorMobileTel
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorMobileTel");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXComeFromNm";//会员来源 Associator_AssociatorOrigin
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorOrigin");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXGiftID";//赠品编号 Associator_lblLargessNum
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblLargessNum");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXGiftDesc";//赠品描述 Associator_lblLargessBewrite
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblLargessBewrite");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXRedeemDate";//兑换日期Associator_lblExchangeDate
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblExchangeDate");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXGiftQty";//兑换数量Associator_lblExchangeNumber
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblExchangeNumber");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXBonusPrev";//兑换前积分Associator_BonusPrev
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_BonusPrev");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXRedeemAmt";//兑换积分Associator_lblExchangeIntegral
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblExchangeIntegral");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXBonusCurr";//兑换后积分Associator_BonusCurr
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_BonusCurr");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXTitle";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_MemberRedeemHList");
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXMallTitle";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = Session["MallTitle"].ToString();
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "REXAmount";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_Amount");
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXGiftQtyAmount";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_GiftQtyAmount");
        paraField[15].CurrentValues.Add(discreteValue[15]);

        paraField[16] = new ParameterField();
        paraField[16].Name = "REXRedeemAmtAmount";
        discreteValue[16] = new ParameterDiscreteValue();
        discreteValue[16].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_RedeemAmtAmount");
        paraField[16].CurrentValues.Add(discreteValue[16]);

        paraField[17] = new ParameterField();
        paraField[17].Name = "REXLogAmount";
        discreteValue[17] = new ParameterDiscreteValue();
        discreteValue[17].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_LogAmount");
        paraField[17].CurrentValues.Add(discreteValue[17]);




        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }



        string str_sql = "SELECT " +
                         "member.membCode,(SELECT  MembCardID FROM membCard WHERE membCard.membID=member.membID AND membCard.cardstatusID='N') AS membCard " +
                         ",member.MemberName,member.MobilPhone,member.ComefromNm,gift.giftID,gift.giftDesc,redeemH.RedeemDate " +
                         ",redeemH.GiftQty,RedeemH.BonusPrev,RedeemH.RedeemAmt,RedeemH.BonusCurr FROM member " +
                         "INNER JOIN redeemH ON (member.membID=redeemH.membID ) " +
                         "LEFT JOIN gift ON (redeemH.giftID = gift.giftID) " +
                         "WHERE 1=1 ";
        if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
        {
            str_sql = str_sql + "AND RedeemH.RedeemDate BETWEEN '" + txtStartDate.Text.Trim() + "' AND '" + txtEndDate.Text.Trim() + "' ";
        }
        if (txtLCardID.Text.Trim() != "")
        {
            str_sql = str_sql + "AND Member.membID in (SELECT membID FROM MembCard WHERE MembCard.MembcardID = '"+txtLCardID.Text.Trim()+"') ";
        }
        if (txtMemberName.Text.Trim() != "")
        {
            str_sql = str_sql + "AND Member.MemberName like '%"+txtMemberName.Text.Trim()+"%' ";
        }
        if (txtMobileTel.Text.Trim() != "")
        {
            str_sql = str_sql + "   AND Member.MobilPhone like '%"+txtMobileTel.Text.Trim()+"%' ";
        }


        str_sql = str_sql + " Order By RedeemH.RedeemDate DESC ,Member.membID ASC ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptMemberRedeemHList.rpt";




    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtLCardID.Text = "";
        txtMemberName.Text = "";
        txtMobileTel.Text = "";
        txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 8) + "01";
        txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
    }

}