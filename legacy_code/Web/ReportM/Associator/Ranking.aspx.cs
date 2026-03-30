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
public partial class ReportM_Associator_Ranking : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtSum.Attributes.Add("onkeydown", "textleave()");
            TextBox1.Attributes.Add("onkeydown", "textleave()");
            TextBox2.Attributes.Add("onkeydown", "textleave()");
            TextBox3.Attributes.Add("onkeydown", "textleave()");
            TextBox4.Attributes.Add("onkeydown", "textleave()");
            TextBox5.Attributes.Add("onkeydown", "textleave()");
            TextBox6.Attributes.Add("onkeydown", "textleave()");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Associator_Ranking");
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
        ParameterField[] paraField = new ParameterField[16];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[16];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        string topPur = "";
        string orderby = "";
        string top = "";

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "Head";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_Ranking");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "Period";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("ReportInfo", "RptInv_Date");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "From";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = txtStartBizTime.Text;
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "To";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = "到";
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "ToDate";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = txtEndBizTime.Text;
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "RankBy";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = "排序方式";
        paraField[5].CurrentValues.Add(discreteValue[5]);

        if (RadioButton1.Checked)
        {
            topPur = RadioButton1.Text + " " + Label5.Text + "--" + txtSum.Text.Trim();
        }
        else if (RadioButton2.Checked)
        {
            topPur = RadioButton2.Text + " " + Label6.Text + "--" + TextBox1.Text.Trim();
        }
        else if (RadioButton3.Checked)
        {
            topPur = RadioButton3.Text + " " + Label7.Text + "--" + TextBox2.Text.Trim();
        }
        else if (RadioButton4.Checked)
        {
            topPur = RadioButton4.Text + " " + Label8.Text + "--" + TextBox3.Text.Trim();
        }
        else if (RadioButton5.Checked)
        {
            topPur = RadioButton5.Text + " " + Label9.Text + "--" + TextBox4.Text.Trim();
        }
        else if (RadioButton6.Checked)
        {
            topPur = RadioButton6.Text + " " + Label10.Text + "--" + TextBox5.Text.Trim();
        }

        paraField[6] = new ParameterField();
        paraField[6].Name = "TopPur";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = topPur;
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "Rank";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Dept_lblOrderID");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "MemId";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorCode");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "MemNm";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_MemberName");
        paraField[9].CurrentValues.Add(discreteValue[9]);

        paraField[10] = new ParameterField();
        paraField[10].Name = "Addr";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Prints_lblAddr");
        paraField[10].CurrentValues.Add(discreteValue[10]);

        paraField[11] = new ParameterField();
        paraField[11].Name = "Phone";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = "办公电话/手机";
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "BonPoint";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = "积分点";
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "PurAmt";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = "购买总额";
        paraField[13].CurrentValues.Add(discreteValue[13]);

        paraField[14] = new ParameterField();
        paraField[14].Name = "NoPur";
        discreteValue[14] = new ParameterDiscreteValue();
        discreteValue[14].Value = "购买数量";
        paraField[14].CurrentValues.Add(discreteValue[14]);

        paraField[15] = new ParameterField();
        paraField[15].Name = "REXMainTitle";
        discreteValue[15] = new ParameterDiscreteValue();
        discreteValue[15].Value = Session["MallTitle"].ToString();
        paraField[15].CurrentValues.Add(discreteValue[15]);


        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }

        if (TextBox6.Text != "")
        {
            top = "Top " + TextBox6.Text.Trim();
        }

        string str_sql = "Select " + top +" max(m.memberName) as CustNm,isnull(max(m.lOtherID),'--') as mOtherID,max(m.salutation) as salutation," +
                        "max(p.membcardid) as CardID,max(m.membID) as CustID," +
                        "max(m.addr1) as Addr1,max(m.addr2) as Addr2,isnull(max(m.offphone),'') as OfficePhone, isnull(max(m.homephone),'') as homephone," +
                        "isnull(max(m.mobilphone),'') as Mphone," +
                        "isnull(sum(bonusAmt),0) as BonusPoint, isnull(sum(netAmt),0) as PurAmt," +
                        "isnull(count(p.purhistID),0) as NoOFPur " +
                        "From  member m inner join  purhist p on m.membID=p.membID Where 1=1";

        if (txtStartBizTime.Text.Trim() != "")
        {
            str_sql = str_sql + " And p.transDT >='" + txtStartBizTime.Text.Trim() + "'";
        }

        if (txtEndBizTime.Text != "")
        {
            str_sql = str_sql + " And p.transDT <='" + txtEndBizTime.Text + "'";
        }

        if (RadioButton1.Checked)
        {
            str_sql = str_sql + " group by p.membID having sum(bonusAmt) > " + (txtSum.Text == "" ? "0" : txtSum.Text);
            orderby = " Order By sum(netAmt)";
        }
        else if (RadioButton2.Checked)
        {
            str_sql = str_sql + " group by p.membID having sum(netAmt) > " + (TextBox1.Text == "" ? "0" : TextBox1.Text);
            orderby = " Order By sum(netAmt)";
        }
        else if (RadioButton3.Checked)
        {
            str_sql = str_sql + " group by p.membID having count(p.purhistID) > " + (TextBox2.Text == "" ? "0" : TextBox2.Text);
            orderby = " Order By count(p.purhistID)";
        }
        else if (RadioButton4.Checked)
        {
            str_sql = str_sql + " group by p.membID having sum(bonusAmt) < " + (TextBox3.Text == "" ? "0" : TextBox3.Text);
            orderby = " Order By sum(netAmt)";
        }
        else if (RadioButton5.Checked)
        {
            str_sql = str_sql + " group by p.membID having sum(netAmt) < " + (TextBox4.Text == "" ? "0" : TextBox4.Text);
            orderby = " Order By sum(netAmt)";
        }
        else if (RadioButton6.Checked)
        {
            str_sql = str_sql + " group by p.membID having count(p.purhistID) < " + (TextBox5.Text == "" ? "0" : TextBox5.Text);
            orderby = " Order By count(p.purhistID)";
        }

        if (RadioButton7.Checked)
        {
            orderby = orderby + " " + "Asc";
        }
        else
        {
            orderby = orderby + " " + "Desc";
        }

        str_sql = str_sql + orderby;
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Associator\\Ranking.rpt";

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        txtStartBizTime.Text = "";
        txtEndBizTime.Text = "";
        txtSum.Text = "";
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox5.Text = "";
        TextBox6.Text = "";
    }
}
