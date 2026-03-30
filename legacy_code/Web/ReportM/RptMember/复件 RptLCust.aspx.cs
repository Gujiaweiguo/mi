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
using System.Drawing.Imaging;

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
using Associator.Associator;



/*
 * English Name : Bruce ; Chinese Name : 何思键  
 * 
 * 修改时间：2009年4月17日 
 * 
 * 编码类型：Modify，Add(修改与增加)
 * 
 * 
 */


public partial class ReportM_Associator_RptLCust : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitMonAndDay();
            InitSexNm();
            InitNationOrArea();
            InitYearOld();
            InitMarryStatus();
            InitIncome();
            InitCreer();
            InitPersonFavor();
            InitInterest();
            InitActivity();
            if (Request.QueryString["MemberId"] != null) 
            {
                BaseBO baseBO = new BaseBO();
                baseBO.WhereClause = "MembId = '" + Request.QueryString["MemberId"].ToString() + "'";
                Resultset rs = baseBO.Query(new LCust());
                GetInterestByID(Request.QueryString["MemberId"].ToString());
                GetActivityByID(Request.QueryString["MemberId"].ToString());
                GetFavorByID(Request.QueryString["MemberId"].ToString());
                ViewState["flag"] = 1; //修改
                ViewState["MemberId"] = Request.QueryString["MemberId"].ToString();

            }

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Associator_QueryAssociator");
            txtMemberFrom.Items.Add(new ListItem("", "-1"));
            txtMemberFrom.Items.Add(new ListItem("普通入会", "普通入会"));
            txtMemberFrom.Items.Add(new ListItem("公司嘉宾", "公司嘉宾"));
            txtMemberFrom.Items.Add(new ListItem("内部员工", "内部员工"));
            txtMemberFrom.Items.Add(new ListItem("资源互换", "资源互换"));
            txtMemberFrom.Items.Add(new ListItem("联名卡", "联名卡"));
            txtMemberFrom.Items.Add(new ListItem("其他", "其他"));
        }
    }


    /**
     * 
     *绑定性别 
     *
     */
    private void InitSexNm() {

        txtGender.Items.Add(new ListItem("","sex"));
        txtGender.Items.Add(new ListItem("男","男"));
        txtGender.Items.Add(new ListItem("女","女"));
    
    }

    /**
     * 
     * 绑定国家/地区
     * 
     */
    private void InitNationOrArea(){
        BaseBO baseBo = new BaseBO();
        baseBo.OrderBy = "EntryAt";
        Resultset rs = baseBo.Query(new Nationality());
        txtNationalOrArea.Items.Add(new ListItem("",""));
        foreach (Nationality nationality in rs) {
            txtNationalOrArea.Items.Add(new ListItem(nationality.NatNm.Trim()));
        }

    }
    /**
     * 
     * 绑定年龄范围
     * 
     */
    private void InitYearOld() {
        //txtYearArea.Items.Add(new ListItem("", "-1"));
        //txtYearArea.Items.Add(new ListItem("18以下","101"));
        //txtYearArea.Items.Add(new ListItem("19-25", "102"));
        //txtYearArea.Items.Add(new ListItem("26-35", "103"));
        //txtYearArea.Items.Add(new ListItem("36-45", "104"));
        //年龄段
        DataSet ds = AgePO.GetAge();
        int count = ds.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            txtYearArea.Items.Add(new ListItem(ds.Tables[0].Rows[i]["Agestr"].ToString(), ds.Tables[0].Rows[i]["AgeID"].ToString()));
        }
    }

    /**
     * 
     *婚姻状况 
     * 
     */
    private void InitMarryStatus() {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Mstatus());
        txtIsMarry.Items.Add(new ListItem("", ""));
        foreach (Mstatus mt in rs) {
            txtIsMarry.Items.Add(new ListItem(mt.MstatusNm.Trim()));
        }
    
    }

    /**
     * 
     *绑定收入水平 
     * 
     */
    private void InitIncome() {
        //BaseBO baseBo = new BaseBO();
        //Resultset rs = baseBo.Query(new Income());
        //txtIncome.Items.Add(new ListItem("",""));
        //foreach (Income income in rs) {
        //    if (income.IncomeUpper == 0)
        //    {
        //        txtIncome.Items.Add(new ListItem(income.IncomeLower + "以上", income.IncomeId.ToString()));
        //    }
        //    else {
        //        txtIncome.Items.Add(new ListItem(income.IncomeLower + "-" + income.IncomeUpper, income.IncomeId.ToString()));
        //    }
        //}

        /*收入水平*/
        Resultset rs = baseBO.Query(new Income());
        //cmbIncomeId.Items.Add(new ListItem(""));
        foreach (Income income in rs)
        {
            if (income.IncomeUpper == 0)
            {
                txtIncome.Items.Add(new ListItem(income.IncomeLower + "以上", income.IncomeId.ToString()));
            }
            else if (income.IncomeUpper == 1)
            {
                txtIncome.Items.Add(new ListItem("其它", income.IncomeId.ToString()));
            }
            else
            {
                txtIncome.Items.Add(new ListItem(income.IncomeLower + "-" + income.IncomeUpper, income.IncomeId.ToString()));
            }
        }
    
    }
    
    /**
     * 
     *绑定职业 
     * 
     */
    private void InitCreer() {
        BaseBO baseBo = new BaseBO();
        Resultset rs = baseBo.Query(new Biz());
        txtCreer.Items.Add(new ListItem(""));
        foreach (Biz biz in rs) {
            txtCreer.Items.Add(new ListItem(biz.BizNm.Trim(),biz.BizNm.ToString()));
        }
    }

    /**
     * 
     * 个人爱好
     * 
     * 
     */
    private void InitPersonFavor() {
        DataSet ds = FacorPO.GetFavorItem();
        txtPersonFunny.DataSource = ds;
        txtPersonFunny.DataBind();
    
    }

    /**
     * 
     * 消费兴趣
     * 
     */
    private void InitInterest()
    {
        DataSet ds = ConsumeInterestPO.GetInterestItem();
        Buy.DataSource = ds;
        Buy.DataBind();

    }

    /**
     * 
     * 
     * 活动讯息
     * 
     * 
     * 
     **/
    private void InitActivity() {
        DataSet ds = ActivityPO.GetActivityItem();
        txtAction.DataSource = ds;
        txtAction.DataBind();
    
    }

    //得到兴趣
    private void GetInterestByID(string MembID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "MembID = '" + MembID + "'";
        Resultset rs = baseBO.Query(new ConsumeInterest());
        int count = Buy.Items.Count;
        for (int i = 0; i < count; i++)
        {
            foreach (ConsumeInterest conInt in rs)
            {
                if (Convert.ToInt32(Buy.Items[i].Value) == conInt.IItemID)
                {
                    Buy.Items[i].Selected = true;
                }
            }
        }
    }

    //得到爱好
    private void GetFavorByID(string MembID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "MembID = '" + MembID + "'";
        Resultset rs = baseBO.Query(new Favor());
        int count = txtPersonFunny.Items.Count;
        for (int i = 0; i < count; i++)
        {
            foreach (Favor favor in rs)
            {
                if (Convert.ToInt32(txtPersonFunny.Items[i].Value) == favor.FItemID)
                {
                    txtPersonFunny.Items[i].Selected = true;
                }
            }
        }
    }

    //得到活动
    private void GetActivityByID(string MembID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "MembID = '" + MembID + "'";
        Resultset rs = baseBO.Query(new Activity());
        int count = txtAction.Items.Count;
        for (int i = 0; i < count; i++)
        {
            foreach (Activity activity in rs)
            {
                if (Convert.ToInt32(txtAction.Items[i].Value) == activity.AItemID)
                {
                    txtAction.Items[i].Selected = true;
                }
            }
        }
    }


    /**
     * 
     * 初始化月 日
     * 
     */
    private void InitMonAndDay() {

        for (int i = 1; i <= 12; i++) 
        {
            if (i - 1 == 0) {
                txtStartMonth.Items.Add(new ListItem("",""));
            }
            if (i - 10 < 0)
            {
                txtStartMonth.Items.Add(new ListItem(i + "月", "0" + i));

            }
            else
            {
                txtStartMonth.Items.Add(new ListItem(i + "月", "" + i));

            }
        
        }

        for (int i = 1; i <= 31; i++)
        {
            if (i - 1 == 0)
            {
                txtStartDay.Items.Add(new ListItem("", ""));
            }
            if (i - 10 < 0)
            {
                txtStartDay.Items.Add(new ListItem(i + "日", "0" + i));
            }
            else
            {
                txtStartDay.Items.Add(new ListItem(i + "日", i + ""));
            }
        }

        for (int i = 1; i <= 12; i++)
        {
            if (i - 1 == 0)
            {
                txtEndMonth.Items.Add(new ListItem("", ""));
            }
            if (i - 10 < 0)
            {
                txtEndMonth.Items.Add(new ListItem(i + "月", "0" + i));

            }
            else
            {
                txtEndMonth.Items.Add(new ListItem(i + "月", "" + i));

            }

        }

        for (int i = 1; i <= 31; i++)
        {
            if (i - 1 == 0)
            {
                txtEndDay.Items.Add(new ListItem("", ""));
            }
            if (i - 10 < 0)
            {
                txtEndDay.Items.Add(new ListItem(i + "日", "0" + i));
            }
            else
            {
                txtEndDay.Items.Add(new ListItem(i + "日", i + ""));
            }
        }

        for (int i = 1; i <= 12; i++)
        {
            if (i - 1 == 0)
            {
                txtStartMarryMonth.Items.Add(new ListItem("", ""));
            }
            if (i - 10 < 0)
            {
                txtStartMarryMonth.Items.Add(new ListItem(i + "月", "0" + i));

            }
            else
            {
                txtStartMarryMonth.Items.Add(new ListItem(i + "月", "" + i));

            }

        }
       

        for (int i = 1; i <= 31; i++)
        {
            if (i - 1 == 0)
            {
                txtStartMarryDay.Items.Add(new ListItem("", ""));
            }
            if (i - 10 < 0)
            {
                txtStartMarryDay.Items.Add(new ListItem(i + "日", "0" + i));
            }
            else {
                txtStartMarryDay.Items.Add(new ListItem(i + "日", i + ""));
            }
            
        }


        for (int i = 1; i <= 12; i++)
        {
            if (i - 1 == 0)
            {
                txtEndMarryMonth.Items.Add(new ListItem("", ""));
            }
            if (i - 10 < 0)
            {
                txtEndMarryMonth.Items.Add(new ListItem(i + "月", "0" + i));

            }
            else {
                txtEndMarryMonth.Items.Add(new ListItem(i + "月", "" + i));
            
            }
            

        }
       
        for (int i = 1; i <= 31; i++)
        {
            if (i - 1 == 0)
            {
                txtEndMarryDay.Items.Add(new ListItem("", ""));
            }
            if (i - 10 < 0)
            {
                txtEndMarryDay.Items.Add(new ListItem(i + "日", "0" + i));
            }
            else {
                txtEndMarryDay.Items.Add(new ListItem(i + "日", "" + i));
            }
        }
    }

    /**
     * 
     * 判断数据空值,返回默认值
     * 
     */
    private String GetStrNull(String s)
    {
        return s.Trim() == "" ? "-32766" : s;
    }

    /**
     *
     * 判断日期空值,返回默认值
     * 
     */
    private String GetdateNull(String s)
    {
        return s.Trim() == "" ? "2007-12-25" : s;
    }

    /**
     * 
     *查询按钮 
     * 
     */
    protected void btnOK_Click(object sender, EventArgs e)
    {
        

        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindDataSum();
        this.Response.Redirect("../ReportShow.aspx");
    }

    /**
     * 
     * 绑定数据字典
     * 
     */
    private void BindDataSum()
    {
        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[14];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[14];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        paraField[0] = new ParameterField();
        paraField[0].ParameterFieldName = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "RptLCust");
        paraField[0].CurrentValues.Add(discreteValue[0]);

        paraField[1] = new ParameterField();
        paraField[1].Name = "REXMemberCode";
        discreteValue[1] = new ParameterDiscreteValue();
        discreteValue[1].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblAssociatorNum");
        paraField[1].CurrentValues.Add(discreteValue[1]);

        paraField[2] = new ParameterField();
        paraField[2].Name = "REXMemberName";
        discreteValue[2] = new ParameterDiscreteValue();
        discreteValue[2].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_MemberName");
        paraField[2].CurrentValues.Add(discreteValue[2]);

        paraField[3] = new ParameterField();
        paraField[3].Name = "REXMemberCardId";
        discreteValue[3] = new ParameterDiscreteValue();
        discreteValue[3].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblAssociatorCard");
        paraField[3].CurrentValues.Add(discreteValue[3]);

        paraField[4] = new ParameterField();
        paraField[4].Name = "REXDob";
        discreteValue[4] = new ParameterDiscreteValue();
        discreteValue[4].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorBirthday");
        paraField[4].CurrentValues.Add(discreteValue[4]);

        paraField[5] = new ParameterField();
        paraField[5].Name = "REXLOtherId";
        discreteValue[5] = new ParameterDiscreteValue();
        discreteValue[5].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorIdentity");
        paraField[5].CurrentValues.Add(discreteValue[5]);

        paraField[6] = new ParameterField();
        paraField[6].Name = "REXDateJoint";
        discreteValue[6] = new ParameterDiscreteValue();
        discreteValue[6].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorEnrollment");
        paraField[6].CurrentValues.Add(discreteValue[6]);

        paraField[7] = new ParameterField();
        paraField[7].Name = "REXSexNm";
        discreteValue[7] = new ParameterDiscreteValue();
        discreteValue[7].Value =(String) GetGlobalResourceObject("BaseInfo", "Associator_AssociatorGender");
        paraField[7].CurrentValues.Add(discreteValue[7]);

        paraField[8] = new ParameterField();
        paraField[8].Name = "REXMembAgeDis";
        discreteValue[8] = new ParameterDiscreteValue();
        discreteValue[8].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_MemberAgeDistrict");
        paraField[8].CurrentValues.Add(discreteValue[8]);

        paraField[9] = new ParameterField();
        paraField[9].Name = "REXNatNm";
        discreteValue[9] = new ParameterDiscreteValue();
        discreteValue[9].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorNationality");
        paraField[9].CurrentValues.Add(discreteValue[9]);


        paraField[10] = new ParameterField();
        paraField[10].Name = "REXMembAddress";
        discreteValue[10] = new ParameterDiscreteValue();
        discreteValue[10].Value = (String)GetGlobalResourceObject("BaseInfo", "Dept_lblOfficeAddr");
        paraField[10].CurrentValues.Add(discreteValue[10]);
       

        paraField[11] = new ParameterField();
        paraField[11].Name = "REXBizNm";
        discreteValue[11] = new ParameterDiscreteValue();
        discreteValue[11].Value = (String)GetGlobalResourceObject("BaseInfo", "Associator_AssociatorOccupation");
        paraField[11].CurrentValues.Add(discreteValue[11]);

        paraField[12] = new ParameterField();
        paraField[12].Name = "REXMainTitle";
        discreteValue[12] = new ParameterDiscreteValue();
        discreteValue[12].Value = Session["MallTitle"].ToString();
        paraField[12].CurrentValues.Add(discreteValue[12]);

        paraField[13] = new ParameterField();
        paraField[13].Name = "REXMemberTotal";
        discreteValue[13] = new ParameterDiscreteValue();
        discreteValue[13].Value = (String)GetGlobalResourceObject("ReportInfo", "Associator_MemberTotal");
        paraField[13].CurrentValues.Add(discreteValue[13]);



        foreach (ParameterField pf in paraField)
        {
            paraFields.Add(pf);
        }
        string str_sql = "";
        string strAnd = "";
       


        //根据条件查询各项
        if (txtCustCode.Text.Trim() != "")
        {
            strAnd = strAnd + " And Member.MembCode ='" + txtCustCode.Text.Trim() + "'";
        }

        if (txtStartBizTime.Text != "")
        {
            strAnd = strAnd + " And Convert(varchar(10),DateJoint,120) >='" + txtStartBizTime.Text + "'";
        }

        if (txtEndBizTime.Text != "")
        {
            strAnd = strAnd + " And Convert(varchar(10),DateJoint,120)  <='" + txtEndBizTime.Text + "'";
        }

        if (txtName.Text.Trim() != "")
        {
            strAnd = strAnd + " And Member.MemberName like '%" + txtName.Text + "%'";
        }

        if (txtTel.Text.Trim() != "")
        {
            strAnd = strAnd + " And Member.MobilPhone ='" + txtTel.Text.Trim() + "'";
        }

        if (txtLAddr.Text.Trim() != "")
        {
            strAnd = strAnd + " And Member.Addr1 like '%" + txtLAddr.Text.Trim() + "%'";
        }

        if (txtID.Text.Trim() != "")
        {
            strAnd = strAnd + " And Member.LOtherId ='" + txtID.Text.Trim() + "'";
        }

        //年龄范围
        if (txtYearArea.SelectedItem.Value == "101")
        {
            strAnd = strAnd + " And Member.AgeID = " + txtYearArea.SelectedValue.Trim();
        }
        if (txtYearArea.SelectedItem.Value == "102")
        {
            strAnd = strAnd + " And Member.AgeID = " + txtYearArea.SelectedValue.Trim();
        }
        if (txtYearArea.SelectedItem.Value == "103")
        {
            strAnd = strAnd + " And Member.AgeID = " + txtYearArea.SelectedValue.Trim();
        }
        if (txtYearArea.SelectedItem.Value == "104")
        {
            strAnd = strAnd + " And Member.AgeID = " + txtYearArea.SelectedValue.Trim();
        }

        //会员来源
        if (txtMemberFrom.SelectedItem.Value == "普通入会")
        {
            strAnd = strAnd + " And Member.ComefromNM = '" + txtMemberFrom.SelectedValue.Trim() +"'";
        }

        if (txtMemberFrom.SelectedItem.Value == "公司嘉宾")
        {
            strAnd = strAnd + " And Member.ComefromNM = '" + txtMemberFrom.SelectedValue.Trim() +"'";
        }
        if (txtMemberFrom.SelectedItem.Value == "内部员工")
        {
            strAnd = strAnd + " And Member.ComefromNM = '" + txtMemberFrom.SelectedValue.Trim() +"'";
        }
        if (txtMemberFrom.SelectedItem.Value == "资源互换")
        {
            strAnd = strAnd + " And Member.ComefromNM = '" + txtMemberFrom.SelectedValue.Trim() +"'";
        }
        if (txtMemberFrom.SelectedItem.Value == "联名卡")
        {
            strAnd = strAnd + " And Member.ComefromNM = '" + txtMemberFrom.SelectedValue.Trim() +"'";
        }
        if (txtMemberFrom.SelectedItem.Value == "其他")
        {
            strAnd = strAnd + " And Member.ComefromNM = '" + txtMemberFrom.SelectedValue.Trim() +"'";
        }
       
        //性别
        if (txtGender.SelectedItem.Value == "男") 
        {
            strAnd = strAnd + " And Member.SexNm ='" + txtGender.SelectedValue + "'";
        }

        if (txtGender.SelectedItem.Value == "女") 
        {
            strAnd = strAnd + "And Member.SexNm ='" + txtGender.SelectedValue + "'";
        }
        

        //出生日期查询
        if (txtStartMonth.SelectedItem.Value != "" && txtStartDay.SelectedItem.Value != "")
        {
            strAnd = strAnd + " And convert(char(5),Member.Dob,10) >= '" + txtStartMonth.SelectedValue.Trim() + "-" + txtStartDay.SelectedValue.Trim() + "'";
        }

        if (txtEndMonth.SelectedItem.Value != "" && txtEndDay.SelectedItem.Value != "")
        {
            strAnd = strAnd + " And convert(char(5),Member.Dob,10) <= '" + txtEndMonth.SelectedValue.Trim() + "-" + txtEndDay.SelectedValue.Trim() + "'";
        }
        
        //结婚纪念日查询
         if (txtStartMarryMonth.SelectedItem.Value !="" && txtStartMarryDay.SelectedItem.Value !="")
        {
            strAnd = strAnd + " And Member.Manndate >= '" + txtStartMarryMonth.SelectedValue.Trim() + "-" + txtStartMarryDay.SelectedValue.Trim() + "'";
        }

        if (txtEndMarryMonth.SelectedItem.Value != "" && txtEndMarryDay.SelectedItem.Value != "")
        {
            strAnd = strAnd + " And Member.Manndate <= '" + txtEndMarryMonth.SelectedValue.Trim() + "-" + txtEndMarryDay.SelectedValue.Trim() + "'";
        }

        //国家地区查询
        if (txtNationalOrArea.SelectedItem.Value != "")
        {
            strAnd = strAnd + " And Member.NatNm ='" + txtNationalOrArea.SelectedValue + "'";
        }

        //是否已经结婚查询
        if (txtIsMarry.SelectedValue.Trim() != "")
        {
            strAnd = strAnd + " And Member.MStatusNm ='" + txtIsMarry.SelectedValue.Trim() + "'";
        }

        //收入查询
        if (txtIncome.SelectedValue.Trim() == "1")
        {
            strAnd = strAnd + " And Income.IncomeId =" + int.Parse(txtIncome.SelectedValue.Trim());
        }
        if (txtIncome.SelectedValue.Trim() == "2")
        {
            strAnd = strAnd + " And Income.IncomeId =" + int.Parse(txtIncome.SelectedValue.Trim());
        }
        if (txtIncome.SelectedValue.Trim() == "3")
        {
            strAnd = strAnd + " And Income.IncomeId =" + int.Parse(txtIncome.SelectedValue.Trim());
        }
        if (txtIncome.SelectedValue.Trim() == "4")
        {
            strAnd = strAnd + " And Income.IncomeId =" + int.Parse(txtIncome.SelectedValue.Trim());
        }

        //职业查询
        if (txtCreer.SelectedItem.Value.Trim() == "其它") 
        {
            strAnd = strAnd + " And Biz.BizNm ='" + txtCreer.SelectedValue.Trim() + "'";
        }
        if (txtCreer.SelectedItem.Value.Trim() == "企业单位")
        {
            strAnd = strAnd + " And Biz.BizNm ='" + txtCreer.SelectedValue.Trim() + "'";
        }
        if (txtCreer.SelectedItem.Value.Trim() == "政府行业")
        {
            strAnd = strAnd + " And Biz.BizNm ='" + txtCreer.SelectedValue.Trim() + "'";
        }


        //积分查询
        if (txtFirstNumber.Text.Trim() != "" && txtSecondNumber.Text.Trim() != "" && int.Parse(txtFirstNumber.Text.Trim()) <= int.Parse(txtSecondNumber.Text.Trim())) {
            str_sql = str_sql + " And EXISTS (SELECT 1 FROM Bonus WHERE bonus.MembID = Member.MembID AND BonusCurr BETWEEN "+int.Parse(txtFirstNumber.Text.Trim())+ " AND "+ int.Parse(txtSecondNumber.Text.Trim())+")";
        }

        //消费兴趣
        int count = Buy.Items.Count;
        string s1 = "";
        for (int i = 0; i < count; i++)
        {
            if (Buy.Items[i].Selected == true)
            {
                s1 = Buy.Items[i].Value +",";
            }

        }
        
        if (s1.Length>0)
        {
            strAnd = strAnd + " And EXISTS (SELECT 1 FROM ConsumeInterest WHERE ConsumeInterest.MembID = Member.MembID AND ConsumeID in (" + s1.TrimEnd(',') + "))";
        }
        //个人爱好
        int favorCount = txtPersonFunny.Items.Count;
        string s2 = "";
        for (int i = 0; i < favorCount; i++)
        {
            if (txtPersonFunny.Items[i].Selected == true)
            {
                
                s2 = txtPersonFunny.Items[i].Value+",";
            }
        }
        if(s2.Length > 0)
        {
            strAnd = strAnd + " And EXISTS (SELECT 1 FROM Favor WHERE Favor.MembID = Member.MembID AND FavorID in (" + s2.TrimEnd(',') + "))";
        }


        //活动讯息
        int activityCount = txtAction.Items.Count;
        string s3 = "";
        for (int i = 0; i < activityCount; i++)
        {
            if (txtAction.Items[i].Selected == true)
            {
                s3 = txtAction.Items[i].Value + ",";
            }
        }

        if (s3.Length > 0)
        {
            strAnd = strAnd + "  And EXISTS (SELECT 1 FROM Activity WHERE Activity.MembID = Member.MembID AND AItemID in (" + s3.TrimEnd(',') + "))";
        }


        str_sql = "SELECT Member.MembId,Member.MembCode,Member.MemberName,MembCard.MembCardId,Member.Dob,Member.LOtherId,Member.DateJoint,Member.SexNm,(SELECT ageStr FROM age WHERE age.ageID = member.ageID) AS ageStr,Member.NatNm,Member.Addr1,Member.Biznm,(SELECT convert(nvarchar(8),incomeLower) + '-'+ convert(nvarchar(8),incomeupper) FROM income WHERE income.incomeID = Member.incomeID   ) AS incomeNm,Member.email,Member.MobilPhone,ISNULL((SELECT BonusCurr FROM bonus WHERE bonus.membID = Member.membID),0) AS BonusCurr FROM Member INNER JOIN MembCard On (Member.MembId = MembCard.MembId) WHERE MembCard.CardStatusID = 'N' "+strAnd;


        str_sql = str_sql + " Order By Member.MembId ";
        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Member\\MemberInfo.rpt";
    }

    /**
     * 
     * 撤消操作
     * 
     */
    private void ClearPage() {
        txtComment.Text = "";
        txtCustCode.Text = "";
        txtName.Text = "";
        txtTel.Text = "";
        txtLAddr.Text = "";
        txtID.Text = "";
        txtFirstNumber.Text = "";
        txtSecondNumber.Text = "";
        txtStartBizTime.Text = "";
        txtEndBizTime.Text = "";
        txtIncome.SelectedIndex = 0;
        txtIsMarry.SelectedIndex = 0;
        txtNationalOrArea.SelectedIndex = 0;
        txtYearArea.SelectedIndex = 0;
        txtStartMonth.SelectedIndex = 0;
        txtEndMonth.SelectedIndex = 0;
        txtStartDay.SelectedIndex = 0;
        txtEndDay.SelectedIndex = 0;
        txtGender.SelectedIndex = 0;
        txtCreer.SelectedIndex = 0;
        txtStartMarryMonth.SelectedIndex = 0;
        txtStartMarryDay.SelectedIndex = 0;
        txtEndMarryMonth.SelectedIndex = 0;
        txtEndMarryDay.SelectedIndex = 0;
        txtMemberFrom.SelectedIndex = 0;
        txtAction.ClearSelection();
        Buy.ClearSelection();
        txtPersonFunny.ClearSelection();
    }

    //取消按钮
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        ClearPage();
    }
  
}
