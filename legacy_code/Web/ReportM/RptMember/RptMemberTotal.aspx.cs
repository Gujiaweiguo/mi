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

public partial class ReportM_RptMember_RptMemberTotal : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDropDownList();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_MembTotal");
        }
    }

    private void BindDropDownList()
    {
        dropDL1.Items.Add(new ListItem("",""));
        dropDL1.Items.Add(new ListItem("性别", "sex"));
        dropDL1.Items.Add(new ListItem("职业", "BizNm"));
        dropDL1.Items.Add(new ListItem("民族", "RaceNm"));
        dropDL1.Items.Add(new ListItem("职务", "JobTitleNm"));
        //dropDL1.Items.Add(new ListItem("消费兴趣", "InterestItem"));
        //dropDL1.Items.Add(new ListItem("个人爱好", "FavorItem"));
        //dropDL1.Items.Add(new ListItem("活动讯息", "ActivityItem"));
        dropDL1.Items.Add(new ListItem("入会日期", "DateJoint"));
        dropDL1.Items.Add(new ListItem("会员来源", "ComeFromNm"));
        dropDL1.Items.Add(new ListItem("会员生日", "Dob"));
        dropDL1.Items.Add(new ListItem("年龄范围", "ageID"));
        dropDL1.Items.Add(new ListItem("国家/地区", "NatNm"));
        dropDL1.Items.Add(new ListItem("婚姻状况", "MstatusNm"));
        dropDL1.Items.Add(new ListItem("结婚纪念日", "MAnnDate"));
        dropDL1.Items.Add(new ListItem("收入水平", "IncomeID"));
        dropDL1.Items.Add(new ListItem("距离范围", "distanceID"));
        dropDL1.Items.Add(new ListItem("教育水平", "EdulevelNm"));

        dropDL2.Items.Add(new ListItem("", ""));
        dropDL2.Items.Add(new ListItem("性别", "sex"));
        dropDL2.Items.Add(new ListItem("职业", "BizNm"));
        dropDL2.Items.Add(new ListItem("民族", "RaceNm"));
        dropDL2.Items.Add(new ListItem("职务", "JobTitleNm"));
        //dropDL2.Items.Add(new ListItem("消费兴趣", "InterestItem"));
        //dropDL2.Items.Add(new ListItem("个人爱好", "FavorItem"));
        //dropDL2.Items.Add(new ListItem("活动讯息", "ActivityItem"));
        dropDL2.Items.Add(new ListItem("入会日期", "DateJoint"));
        dropDL2.Items.Add(new ListItem("会员来源", "ComeFromNm"));
        dropDL2.Items.Add(new ListItem("会员生日", "Dob"));
        dropDL2.Items.Add(new ListItem("年龄范围", "ageID"));
        dropDL2.Items.Add(new ListItem("国家/地区", "NatNm"));
        dropDL2.Items.Add(new ListItem("婚姻状况", "MstatusNm"));
        dropDL2.Items.Add(new ListItem("结婚纪念日", "MAnnDate"));
        dropDL2.Items.Add(new ListItem("收入水平", "IncomeID"));
        dropDL2.Items.Add(new ListItem("距离范围", "distanceID"));
        dropDL2.Items.Add(new ListItem("教育水平", "EdulevelNm"));

        dropDL3.Items.Add(new ListItem("", ""));
        dropDL3.Items.Add(new ListItem("性别", "sex"));
        dropDL3.Items.Add(new ListItem("职业", "BizNm"));
        dropDL3.Items.Add(new ListItem("民族", "RaceNm"));
        dropDL3.Items.Add(new ListItem("职务", "JobTitleNm"));
        //dropDL3.Items.Add(new ListItem("消费兴趣", "InterestItem"));
        //dropDL3.Items.Add(new ListItem("个人爱好", "FavorItem"));
        //dropDL3.Items.Add(new ListItem("活动讯息", "ActivityItem"));
        dropDL3.Items.Add(new ListItem("入会日期", "DateJoint"));
        dropDL3.Items.Add(new ListItem("会员来源", "ComeFromNm"));
        dropDL3.Items.Add(new ListItem("会员生日", "Dob"));
        dropDL3.Items.Add(new ListItem("年龄范围", "ageID"));
        dropDL3.Items.Add(new ListItem("国家/地区", "NatNm"));
        dropDL3.Items.Add(new ListItem("婚姻状况", "MstatusNm"));
        dropDL3.Items.Add(new ListItem("结婚纪念日", "MAnnDate"));
        dropDL3.Items.Add(new ListItem("收入水平", "IncomeID"));
        dropDL3.Items.Add(new ListItem("距离范围", "distanceID"));
        dropDL3.Items.Add(new ListItem("教育水平", "EdulevelNm"));

        dropDL4.Items.Add(new ListItem("", ""));
        dropDL4.Items.Add(new ListItem("性别", "sex"));
        dropDL4.Items.Add(new ListItem("职业", "BizNm"));
        dropDL4.Items.Add(new ListItem("民族", "RaceNm"));
        dropDL4.Items.Add(new ListItem("职务", "JobTitleNm"));
        //dropDL4.Items.Add(new ListItem("消费兴趣", "InterestItem"));
        //dropDL4.Items.Add(new ListItem("个人爱好", "FavorItem"));
        //dropDL4.Items.Add(new ListItem("活动讯息", "ActivityItem"));
        dropDL4.Items.Add(new ListItem("入会日期", "DateJoint"));
        dropDL4.Items.Add(new ListItem("会员来源", "ComeFromNm"));
        dropDL4.Items.Add(new ListItem("会员生日", "Dob"));
        dropDL4.Items.Add(new ListItem("年龄范围", "ageID"));
        dropDL4.Items.Add(new ListItem("国家/地区", "NatNm"));
        dropDL4.Items.Add(new ListItem("婚姻状况", "MstatusNm"));
        dropDL4.Items.Add(new ListItem("结婚纪念日", "MAnnDate"));
        dropDL4.Items.Add(new ListItem("收入水平", "IncomeID"));
        dropDL4.Items.Add(new ListItem("距离范围", "distanceID"));
        dropDL4.Items.Add(new ListItem("教育水平", "EdulevelNm"));

        dropDL5.Items.Add(new ListItem("", ""));
        dropDL5.Items.Add(new ListItem("性别", "sex"));
        dropDL5.Items.Add(new ListItem("职业", "BizNm"));
        dropDL5.Items.Add(new ListItem("民族", "RaceNm"));
        dropDL5.Items.Add(new ListItem("职务", "JobTitleNm"));
        //dropDL5.Items.Add(new ListItem("消费兴趣", "InterestItem"));
        //dropDL5.Items.Add(new ListItem("个人爱好", "FavorItem"));
        //dropDL5.Items.Add(new ListItem("活动讯息", "ActivityItem"));
        dropDL5.Items.Add(new ListItem("入会日期", "DateJoint"));
        dropDL5.Items.Add(new ListItem("会员来源", "ComeFromNm"));
        dropDL5.Items.Add(new ListItem("会员生日", "Dob"));
        dropDL5.Items.Add(new ListItem("年龄范围", "ageID"));
        dropDL5.Items.Add(new ListItem("国家/地区", "NatNm"));
        dropDL5.Items.Add(new ListItem("婚姻状况", "MstatusNm"));
        dropDL5.Items.Add(new ListItem("结婚纪念日", "MAnnDate"));
        dropDL5.Items.Add(new ListItem("收入水平", "IncomeID"));
        dropDL5.Items.Add(new ListItem("距离范围", "distanceID"));
        dropDL5.Items.Add(new ListItem("教育水平", "EdulevelNm"));
        
    }

    private string GetSqlByKey(DropDownList drop)
    {
        string str_sql = "";
        if (drop.SelectedValue == "sex")
        {
            str_sql += " CASE WHEN SEXNm = '' THEN '未标记' ELSE SEXNm END ";
        }
        if (drop.SelectedValue == "BizNm")
        {
            str_sql += " CASE WHEN BizNm = '' THEN '未标记' ELSE BizNm END ";
        }
        if (drop.SelectedValue == "RaceNm")
        {
            str_sql += " CASE WHEN RaceNm = '' THEN '未标记' ELSE RaceNm END ";
        }
        if (drop.SelectedValue == "JobTitleNm")
        {
            str_sql += " CASE WHEN JobTitleNm = '' THEN '未标记' ELSE JobTitleNm END ";
        }
        //if (drop.SelectedValue == "InterestItem")
        //{
        //    str_sql += " (SELECT InterestItem.IItemName FROM ConsumeInterest,InterestItem WHERE ConsumeInterest.IItemID = InterestItem.IItemID)";
        //}
        //if (drop.SelectedValue == "FavorItem")
        //{
        //    str_sql += " (SELECT FavorItem.FItemName FROM FavorItem,Favor WHERE Favor.FItemID = FavorItem.FItemID)";
        //}
        //if (drop.SelectedValue == "ActivityItem")
        //{
        //    str_sql += " (SELECT Activity.AItemName FROM Activity,ActivityItem WHERE ActivityItem.AItemID = Activity.AItemID)";
        //}
        if (drop.SelectedValue == "DateJoint")
        {
            str_sql += " CASE WHEN DateJoint = '' THEN '未标记' ELSE DateJoint END ";
        }
        if (drop.SelectedValue == "ComeFromNm")
        {
            str_sql += " CASE WHEN ComeFromNm = '' THEN '未标记' ELSE ComeFromNm END ";
        }
        if (drop.SelectedValue == "Dob")
        {
            str_sql += " CASE WHEN Dob = '' THEN '未标记' ELSE Dob END ";
        }
        if (drop.SelectedValue == "ageID")
        {
            str_sql += " ISNULL((SELECT ageStr FROM Age WHERE Age.AgeID = MEMBER.AgeID),'未标记')";
        }
        if (drop.SelectedValue == "NatNm")
        {
            str_sql += " CASE WHEN NatNm = '' THEN '未标记' ELSE NatNm END ";
        }
        if (drop.SelectedValue == "MstatusNm")
        {
            str_sql += " CASE WHEN MstatusNm = '' THEN '未标记' ELSE MstatusNm END ";
        }
        if (drop.SelectedValue == "MAnnDate")
        {
            str_sql += " CASE WHEN MAnnDate = '' THEN '未标记' ELSE MAnnDate END ";
        }
        if (drop.SelectedValue == "IncomeID")
        {
            str_sql += " ISNULL((SELECT IncomeDesc FROM Income WHERE Income.IncomeID = MEMBER.IncomeID),'未标记')";
        }
        if (drop.SelectedValue == "distanceID")
        {
            str_sql += " ISNULL((SELECT distanceDesc FROM distance WHERE distance.distanceID = MEMBER.distanceID),'未标记')";
        }
        if (drop.SelectedValue == "EdulevelNm")
        {
            str_sql += " CASE WHEN EdulevelNm = '' THEN '未标记' ELSE EdulevelNm END ";
        }
        return str_sql;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string sql = " SELECT ";
        int flag = 0;
        if (dropDL1.SelectedValue != "")
        {
            flag++;
            sql += GetSqlByKey(dropDL1) + " AS File" + flag + ",";
        }
        if (dropDL2.SelectedValue != "")
        {
            flag++;
            sql += GetSqlByKey(dropDL2) + " AS File" + flag + ",";             
        }
        if (dropDL3.SelectedValue != "")
        {
            flag++;
            sql += GetSqlByKey(dropDL3) + " AS File" + flag + ","; 
        }
        if (dropDL4.SelectedValue != "")
        {
            flag++;
            sql += GetSqlByKey(dropDL4) + " AS File" + flag + ","; 
        }
        if (dropDL5.SelectedValue != "")
        {
            flag++;
            sql += GetSqlByKey(dropDL5) + " AS File" + flag + ","; 
        }
        sql += " 0 AS File0 FROM Member WHERE 1=1";

        if (txtStartDate.Text != "")
        {
            sql += " AND Member.DateJoint >= '" + txtStartDate.Text + " 00:00:00'";
        }
        if (txtEndDate.Text != "")
        {
            sql += " AND Member.DateJoint <= '" + txtEndDate.Text + " 23:59:59'";
        }

        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();

        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXRptTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Menu_MembTotal");
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

        if (flag == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '至少选择一条查询条件。'", true);
        }
        else
        {
            Session["paraFil"] = paraFields;
            Session["sql"] = sql;
            if (flag == 5)
            {
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\member\\RptMemberTotal5.rpt";
            }
            if (flag == 4)
            {
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\member\\RptMemberTotal4.rpt";
            }
            if (flag == 3)
            {
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\member\\RptMemberTotal3.rpt";
            }
            if (flag == 2)
            {
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\member\\RptMemberTotal2.rpt";
            }
            if (flag == 1)
            {
                Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\member\\RptMemberTotal1.rpt";
            }
            this.Response.Redirect("../ReportShow.aspx");
        }
    }
}
