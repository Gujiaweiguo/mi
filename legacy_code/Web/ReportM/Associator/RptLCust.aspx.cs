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
using RentableArea;
using Lease.ConShop;
using Base.Util;
using Invoice.InvoiceH;
using Sell;
using Invoice.BankCard;
using Associator.Perform;

public partial class ReportM_Associator_RptLCust : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Associator_QueryAssociator");
            DropCardType.Items.Add(new ListItem("","-1"));
            DropCardType.Items.Add(new ListItem("顾客卡", "N"));
            DropCardType.Items.Add(new ListItem("员工卡", "E"));
            DropCardType.Items.Add(new ListItem("其它", "O"));
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindDataSum();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void BindDataSum()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[13];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[13];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = "会员信息报表";
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXLCustID";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = "会员编码";
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXLCustName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = "会员名称";
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXDateJoint";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = "入会时间";
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXLOtherIdt";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = "证件号";
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXDob";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = "出生日期";
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXSexNm";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = "性别";
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXNatNm";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = "国籍";
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXMStatusNm";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = "婚姻状况";
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXBizNm";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = "职业";
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "REXMobilPhone";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = "移动电话";
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXEmaile";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = "Email";
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXLcardID";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = "卡号";
        paraField[12].CurrentValues.Add(discreteValue[12]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        string str_sql = "Select Member.MembId,MembCardId,LOtherId,MembCode,MemberName,DateJoint,MobilPhone,email,Dob,SexNm,Biznm,NatNm,MStatusNm From Member Left Join MembCard On Member.MembId = MembCard.MembId Where CardStatusID = 'N'";

        if (txtCustCode.Text.Trim() != "")
        {
            str_sql = str_sql + " And MembCode ='" + txtCustCode.Text.Trim() + "'";
        }

        if (txtStartBizTime.Text != "")
        {
            str_sql = str_sql + " And Convert(varchar(10),DateJoint,120) >='" + txtStartBizTime.Text + "'";
        }

        if (txtEndBizTime.Text != "")
        {
            str_sql = str_sql + " And Convert(varchar(10),DateJoint,120)  <='" + txtEndBizTime.Text + "'";
        }

        if (txtName.Text.Trim() != "")
        {
            str_sql = str_sql + " And MemberName like '%" + txtName.Text + "%'";
        }

        if (txtTel.Text.Trim() != "")
        {
            str_sql = str_sql + " And MobilPhone ='" + txtTel.Text.Trim() + "'";
        }

        if (txtID.Text.Trim() != "")
        {
            str_sql = str_sql + " And LOtherId ='" + txtID.Text.Trim() + "'";
        }

        if (DropCardType.SelectedItem.Value == "N")
        {
            str_sql = str_sql + " And CardOwner ='" + LCard.OPTN_ORMA_LCUST + "'";
        }

        if (DropCardType.SelectedItem.Value == "E")
        {
            str_sql = str_sql + " And CardOwner ='" + LCard.OPT_EMPLOYEE+ "'";
        }

        if (DropCardType.SelectedItem.Value == "O")
        {
            str_sql = str_sql + " And CardOwner ='" + LCard.OPT_OTHERS + "'";
        }


        str_sql = str_sql + " Order By Member.MembId";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Associator\\LCust.rpt";

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {

    } 
}
