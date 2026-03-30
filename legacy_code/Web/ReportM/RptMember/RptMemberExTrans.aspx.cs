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
using Base.Util;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class ReportM_RptMember_RptMemberExTrans : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 8) + "01";
            txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtLCardID.Attributes.Add("onkeydown", "textleave()");
            txtMobileTel.Attributes.Add("onkeydown", "textleave()");

        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string mex = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");


        if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
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
        ParameterField[] paraField = new ParameterField[9];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[9];
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
        paraField[3].Name = "REXGiftDesc";//赠品名称
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("ReportInfo", "RptMem_GiftDesc");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXComeFromNm";//会员来源 Associator_AssociatorOrigin
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorOrigin");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXMemberExTrans";//小票换礼品报表
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_RptMemberExTrans");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXGiftQty";//兑换数量Associator_lblExchangeNumber
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblExchangeNumber");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXMallTitle";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = Session["MallTitle"].ToString();
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXExchangePrice";//兑换金额
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_lblExchangePrice");
        paraField[8].CurrentValues.Add(discreteValue[8]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }



        string str_sql = "SELECT " +
                         "member.membCode,(SELECT TOP 1 MembCardID FROM membCard WHERE membCard.membID=member.membID AND membCard.cardstatusID='N') AS membCard " +
                         ",member.MemberName,member.MobilPhone,member.ComefromNm,ExTrans.TransAmt,ExTrans.GiftQty,gift.giftDesc FROM MEMBER" +
                         " INNER JOIN ExTrans ON (member.membID=ExTrans.membID )" +
                         " INNER JOIN GIFT ON (GIFT.giftid = ExTrans.giftid)" +
                         "WHERE 1=1 ";
        if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
        {
            str_sql = str_sql + "AND ExTrans.ExDate BETWEEN '" + txtStartDate.Text.Trim() + "' AND '" + txtEndDate.Text.Trim() + "' ";
        }
        if (txtLCardID.Text.Trim() != "")
        {
            str_sql = str_sql + "AND Member.membID in (SELECT membID FROM MembCard WHERE MembCard.MembcardID = '" + txtLCardID.Text.Trim() + "') ";
        }
        if (txtMemberName.Text.Trim() != "")
        {
            str_sql = str_sql + "AND Member.MemberName like '%" + txtMemberName.Text.Trim() + "%' ";
        }
        if (txtMobileTel.Text.Trim() != "")
        {
            str_sql = str_sql + "   AND Member.MobilPhone like '%" + txtMobileTel.Text.Trim() + "%' ";
        }


        str_sql = str_sql + " Order By ExTrans.ExDate DESC ,Member.membID ASC ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptMemberExTrans.rpt";




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
