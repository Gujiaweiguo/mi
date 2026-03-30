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
using Lease.CustLicense;
using Lease.PotBargain;
using RentableArea;
using Base.Util;
using Lease.PotCust;
using BaseInfo.User;
using Associator.Perform;

/// <summary>
/// 编写人：hesijian
/// 编写时间：2009年4月30日
/// </summary>

public partial class ReportM_RptMember_RptMemberFreeGiftTrans : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtMobilephone.Attributes.Add("onkeypress", "TestNum()");
            txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-01");
            txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MemberFreeGiftTrans");
            InitDDL();
        }

    }

    private void InitDDL()
    {

        BaseBO baseBo = new BaseBO();

        baseBo.WhereClause = "FreeGift = '" + Gift.FREEGIFT_YES + "'";
        Resultset rs = baseBo.Query(new Gift());
        ddlGiftDesc.Items.Add(new ListItem("", ""));
        foreach (Gift bd in rs)
        {
            ddlGiftDesc.Items.Add(new ListItem(bd.GiftDesc, bd.GiftID.ToString()));
        }





    }
    //绑定数据
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields(); 
        ParameterField[] Field = new ParameterField[12];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[12];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MemberFreeGiftTrans");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXMainTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = Session["MallTitle"].ToString();
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        Field[2] = new ParameterField();
        Field[2].Name = "REXMembCode";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorCode");
        Field[2].CurrentValues.Add(DiscreteValue[2]);


        Field[3] = new ParameterField();
        Field[3].Name = "REXMembCard";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblAssociatorCard");
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        Field[4] = new ParameterField();
        Field[4].Name = "REXMembName";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Member_MemberName");
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        Field[5] = new ParameterField();
        Field[5].Name = "REXMobilePhone";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorMobileTel");
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        Field[6] = new ParameterField();
        Field[6].Name = "REXMembFrom";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorOrigin");
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].Name = "REXActDesc";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblBewrite");
        Field[7].CurrentValues.Add(DiscreteValue[7]);

        Field[8] = new ParameterField();
        Field[8].Name = "REXGiftID";
        DiscreteValue[8] = new ParameterDiscreteValue();
        DiscreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Member_GiftID");
        Field[8].CurrentValues.Add(DiscreteValue[8]);

        Field[9] = new ParameterField();
        Field[9].Name = "REXGiftDesc";
        DiscreteValue[9] = new ParameterDiscreteValue();
        DiscreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblLargessBewrite");
        Field[9].CurrentValues.Add(DiscreteValue[9]);

        Field[10] = new ParameterField();
        Field[10].Name = "REXGiftNum";
        DiscreteValue[10] = new ParameterDiscreteValue();
        DiscreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Member_GiftQty");
        Field[10].CurrentValues.Add(DiscreteValue[10]);

        Field[11] = new ParameterField();
        Field[11].Name = "REXActDate";
        DiscreteValue[11] = new ParameterDiscreteValue();
        DiscreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Member_FreeGiftTransDate");
        Field[11].CurrentValues.Add(DiscreteValue[11]);


        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string str_sql = "";

        str_sql = @"SELECT member.membCode,(SELECT MembCardID FROM membCard WHERE membCard.membID=member.membID AND cardstatusID='N') AS membCard,member.MemberName
       ,member.MobilPhone
       ,member.ComefromNm
       ,giftactivity.ActDesc
       ,gift.giftID
       ,gift.giftDesc
       ,freegifttrans.GiftQty
       ,freegifttrans.ActDate
  FROM member
       INNER JOIN freegifttrans ON (member.membID=freegifttrans.membID )
       INNER JOIN giftactivity ON (freegifttrans.actID=giftactivity.actID)
       LEFT JOIN gift ON (giftactivity.giftID = gift.giftID)
 WHERE freegifttrans.ActDate BETWEEN '" + txtStartDate.Text.Trim() + "' AND '" + txtEndDate.Text.Trim() + "'";

        //条件查询
        if (txtCustCode.Text.Trim() != "")
        {
            str_sql = str_sql + " And member.membID in (SELECT membID FROM MembCard WHERE MembCard.MembcardID = '" + txtCustCode.Text.Trim()+"')";
        }

        if (txtCustName.Text.Trim() != "")
        {
            str_sql = str_sql + " And Member.MemberName like '%" + txtCustName.Text.Trim() + "%'";
        }

        if (txtMobilephone.Text.Trim() != "")
        {
            str_sql = str_sql + " And Member.MobilePhone like '%" + txtMobilephone.Text.Trim() + "%'";
        }
        if (ddlGiftDesc.Text != "")
        {
            str_sql = str_sql + "And gift.GiftID='" + ddlGiftDesc.SelectedValue.ToString() + "'";
        }

        str_sql = str_sql + " Order by freegifttrans.ActDate desc , member.membCode asc";
        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptMemberFreeGiftTrans.rpt";

    }

    //时间处理器
    private string GetFirstDayOnMonth(string date)
    {
        string str1 = date.Substring(0, 5);
        string str2 = date.Substring(5, 1);
        string union = str1 +  str2 + "-01";
        return union;
    }


    private void ClearPage()
    {
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtStartDate.Text = GetFirstDayOnMonth(DateTime.Now.ToShortDateString());
        txtEndDate.Text = DateTime.Now.ToShortDateString();
        txtMobilephone.Text = "";
    }

    //查询操作
    protected void BtnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
        {
            BindData();
            this.Response.Redirect("../ReportShow.aspx");
        }
        else
        {
            return;
        }
    }

    //撤消操作
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
