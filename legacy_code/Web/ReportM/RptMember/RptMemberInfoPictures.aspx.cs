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
using Base;
using Base.Page;
using Base.Biz;
using Base.DB;
using BaseInfo.authUser;
using BaseInfo.User;

/// <summary>
/// 编写人：hesijian
/// 编写时间：2009年7月15日
/// </summary>
public partial class ReportM_RptMember_RptMemberInfoPictures : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        InitDLL();
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MemberInfoPicture");
            
        }
    }

    //初始化
    private void InitDLL()
    {
        //性别
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorGender"), "SexNm"));
        //民族
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorFolk"), "RaceNm"));
        //职业
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorOccupation"), "BizNm"));
        //职务
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorDuty"), "JobTitle"));
        //会员来源
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorOrigin"), "OriginNm"));
        //年龄范围
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AgeArea"), "AgeArea"));
        //距离范围
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorDistance"), "DistanceNm"));
        //收入水平
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorEarning"), "EarningNm"));
        //国家/地区
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_CountryOrArea"), "CountryOrArea"));
        //婚姻状况
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorMarriage"), "Marriage"));
        //教育水平
        txtMemberDesc.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorBringuUp"), "Educated"));
    
    }

    //数据绑定
    private void BindData()
    {
        ParameterFields Fields = new ParameterFields();
        ParameterField[] Field = new ParameterField[8];
        ParameterDiscreteValue[] DiscreteValue = new ParameterDiscreteValue[8];
        ParameterRangeValue RangeValue = new ParameterRangeValue();

        Field[0] = new ParameterField();
        Field[0].Name = "REXTitle";
        DiscreteValue[0] = new ParameterDiscreteValue();
        DiscreteValue[0].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_Member") + "(" + txtMemberDesc.SelectedItem.Text.Trim() + ")" + (String)GetGlobalResourceObject("BaseInfo", "Rpt_AnalysisPicture");
        Field[0].CurrentValues.Add(DiscreteValue[0]);

        Field[1] = new ParameterField();
        Field[1].Name = "REXMallTitle";
        DiscreteValue[1] = new ParameterDiscreteValue();
        DiscreteValue[1].Value = Session["MallTitle"].ToString();
        Field[1].CurrentValues.Add(DiscreteValue[1]);

        //查询日期
        Field[2] = new ParameterField();
        Field[2].Name = "REXCheckDate";
        DiscreteValue[2] = new ParameterDiscreteValue();
        DiscreteValue[2].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_SearcheDate");
        Field[2].CurrentValues.Add(DiscreteValue[2]);

        //开始日期
        Field[3] = new ParameterField();
        Field[3].Name = "REXStartDate";
        DiscreteValue[3] = new ParameterDiscreteValue();
        DiscreteValue[3].Value = txtStartDate.Text.Trim();
        Field[3].CurrentValues.Add(DiscreteValue[3]);

        //结束日期
        Field[4] = new ParameterField();
        Field[4].Name = "REXEndDate";
        DiscreteValue[4] = new ParameterDiscreteValue();
        DiscreteValue[4].Value = txtEndDate.Text.Trim();
        Field[4].CurrentValues.Add(DiscreteValue[4]);

        //横线
        string linevalue = "---";
        if (txtStartDate.Text.Trim() == "" || txtEndDate.Text.Trim() == "")
        {
            linevalue = "";
        }
        Field[5] = new ParameterField();
        Field[5].Name = "REXLine";
        DiscreteValue[5] = new ParameterDiscreteValue();
        DiscreteValue[5].Value = linevalue;
        Field[5].CurrentValues.Add(DiscreteValue[5]);

        //副标题
        string secondTitle = "";
        if (Rdo1.Checked)
        {
            secondTitle = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MembNumbers");
        }
        if (Rdo2.Checked)
        {
            secondTitle = (String)GetGlobalResourceObject("BaseInfo", "Associator_MemberBonus");
        }
        if (Rdo3.Checked)
        {
            secondTitle = (String)GetGlobalResourceObject("BaseInfo","Associator_MemberChangeNum");
        }
        if (Rdo4.Checked)
        {
            secondTitle = (String)GetGlobalResourceObject("BaseInfo","Associator_MemberChangeSum");
        }
        Field[6] = new ParameterField();
        Field[6].Name = "REXSecondTitle";
        DiscreteValue[6] = new ParameterDiscreteValue();
        DiscreteValue[6].Value = secondTitle;
        Field[6].CurrentValues.Add(DiscreteValue[6]);

        Field[7] = new ParameterField();
        Field[7].Name = "REXTitle1";
        DiscreteValue[7] = new ParameterDiscreteValue();
        DiscreteValue[7].Value = (String)GetGlobalResourceObject("BaseInfo", "Rpt_MemberInfoPicture");
        Field[7].CurrentValues.Add(DiscreteValue[7]);



        foreach (ParameterField pf in Field)
        {
            Fields.Add(pf);
        }

        string str_sql = "";
        string strType = "";
        string strWhere = "";

        if (txtStartDate.Text.Trim() != "")
        {
            strWhere = strWhere + " AND Member.DateJoint >= '" + txtStartDate.Text + " 00:00:00'";
        
        }

        if (txtEndDate.Text.Trim() != "")
        {
            strWhere = strWhere + " AND Member.DateJoint <= '" + txtEndDate.Text + " 23:59:59'";
        
        }


        //会员数量
        if (Rdo1.Checked)
        {
            strType = " ,count(Member.MembId) as Number ";
        }

        //会员积分
        if(Rdo2.Checked)
        {
            strType = " ,(select isnull(sum(bonusTotal),0) from bonus where bonus.membid=member.membid) as Number ";
        }

        //会员交易笔次
        if (Rdo3.Checked)
        {
            strType = " ,(select isnull(count(receiptID),0) from purhist where purhist.membid=member.membid) as Number ";
        }

        //会员交易金额
        if (Rdo4.Checked)
        {
            strType = " ,(select isnull(sum(NetAmt),0) from purhist where purhist.membid=member.membid) as Number ";
        }

        //查询条件(会员属性)
        if (txtMemberDesc.SelectedItem.Value == "AgeArea")
        { 
            //说明：ageid = 0 表示为不确定  ageid=null 表示为未记录 
            if(Rdo1.Checked){
                str_sql = " select case member.ageid when '101' then '18岁以下' when '102' then '19-25' when '103' then '26-35' when '104' then '36-45' when '105' then '46-55' when '106' then '56岁以上' when '0' then '不确定' else '未记录' end as ItemDesc " + strType + " from Member where 1=1 " + strWhere + " Group by Member.ageid";
            }
            else
            {
                str_sql = " select case member.ageid when '101' then '18岁以下' when '102' then '19-25' when '103' then '26-35' when '104' then '36-45' when '105' then '46-55' when '106' then '56岁以上' when '0' then '不确定' else '未记录' end as ItemDesc " + strType + " from Member where 1=1 " + strWhere;  
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "OriginNm")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select (case member.ComefromNM when '' then '未记录' else member.ComefromNM end ) as ItemDesc " + strType + " from Member where 1 =1 " + strWhere + " Group by member.ComefromNM";
            }
            else
            {
                str_sql = "select (case member.ComefromNM when '' then '未记录' else member.ComefromNM end ) as ItemDesc " + strType + " from Member where 1 =1 " + strWhere;
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "SexNm")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select (case Member.SexNm when '' then '未记录' else Member.SexNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere + " Group by Member.SexNm";
            }
            else
            {
                str_sql = "select (case Member.SexNm when '' then '未记录' else Member.SexNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere;
            }

        }

        if (txtMemberDesc.SelectedItem.Value == "RaceNm")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select (case Member.RaceNm when '' then '未记录' else Member.RaceNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere + " Group by Member.RaceNm";
            
            }
            else
            {
                str_sql = "select (case Member.RaceNm when '' then '未记录' else Member.RaceNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere;
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "DistanceNm")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select isNUll(distance.DistanceDesc,'未记录') as ItemDesc " + strType + " from Member left join distance on(distance.DistanceId=Member.Incomeid) " + strWhere + " Group by distance.DistanceDesc";
            }
            else
            {
                str_sql = "select isNUll(distance.DistanceDesc,'未记录') as ItemDesc " + strType + " from Member left join distance on(distance.DistanceId=Member.Incomeid) " + strWhere;
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "EarningNm")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select isNUll(Income.IncomeDesc,'未记录') as ItemDesc " + strType + " from Member left join Income on(Income.Incomeid=Member.Incomeid) " + strWhere + " Group by Income.IncomeDesc";
            }
            else
            {
                str_sql = "select isNUll(Income.IncomeDesc,'未记录') as ItemDesc " + strType + " from Member left join Income on(Income.Incomeid=Member.Incomeid) " + strWhere;
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "BizNm")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select (case Member.BizNm when '' then '未记录' else Member.BizNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere + " Group by member.BizNm";
            }
            else
            {
                str_sql = "select (case Member.BizNm when '' then '未记录' else Member.BizNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere;
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "JobTitle")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select (case Member.JobTitleNm when '' then '未记录' else Member.JobTitleNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere + " Group by Member.JobTitleNm";
            }
            else
            {
                str_sql = "select (case Member.JobTitleNm when '' then '未记录' else Member.JobTitleNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere;
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "CountryOrArea")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select (case Member.NatNm when '' then '未记录' else Member.NatNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere + " Group by member.NatNm";
            }
            else
            {
                str_sql = "select (case Member.NatNm when '' then '未记录' else Member.NatNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere;
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "Marriage")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select (case member.MStatusNm when '' then '未记录' else Member.MStatusNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere + " Group by member.MStatusNm";
            }
            else
            {
                str_sql = "select (case member.MStatusNm when '' then '未记录' else Member.MStatusNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere;
            }
        }

        if (txtMemberDesc.SelectedItem.Value == "Educated")
        {
            if (Rdo1.Checked)
            {
                str_sql = "select (case member.EduLevelNm when '' then '未记录' else member.EduLevelNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere + " Group by member.EduLevelNm";
            }
            else
            {
                str_sql = "select (case member.EduLevelNm when '' then '未记录' else member.EduLevelNm end ) as ItemDesc " + strType + " from Member where 1=1 " + strWhere;
            }
        }

        Session["paraFil"] = Fields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\RptMemberInfoPictures.rpt";
    }

    //清除页面
    private void ClearPage()
    {
        txtMemberDesc.SelectedIndex = 0;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        Rdo1.Checked = true;
        Rdo2.Checked = false;
        Rdo3.Checked = false;
        Rdo4.Checked = false;
    }

    //查询操作
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx"); 
    }

    //撤销操作
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
}
