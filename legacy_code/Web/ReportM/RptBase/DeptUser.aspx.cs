using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;
using BaseInfo.Dept;

public partial class ReportM_RptBase_DeptUser : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_DeptUser");
            strFresh = (String)GetGlobalResourceObject("ReportInfo", "Page_Refresh");
            bindStatus();
            bindStore();
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        Session["subReportSql"] = "";
        Session["subRpt"] = "";
        BindData();
        this.Response.Redirect("../ReportShow.aspx");
    }

    private void bindStore()
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "deptstatus=1";
        baseBo.OrderBy = "orderid";
        ddlProjuct.Items.Clear();
        ddlProjuct.Items.Add(new ListItem("", ""));
        Resultset rs = baseBo.Query(new Dept());
        foreach (Dept dept in rs)
        {
            ddlProjuct.Items.Add(new ListItem(dept.DeptName, dept.DeptID.ToString()));
        }
    }

    private void bindStatus()
    {
        cmbUserState.Items.Add(new ListItem("", ""));
        int[] status = Users.GetUserStatus();
        foreach (int sta in status)
        {
            this.cmbUserState.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Users.GetUserStautsDesc(sta)), sta.ToString()));
        }
    }

    private void BindData()
    {

        ParameterFields paraFields = new ParameterFields();
        ParameterField[] paraField = new ParameterField[2];
        ParameterDiscreteValue[] discreteValue = new ParameterDiscreteValue[2];
        ParameterRangeValue rangeValue = new ParameterRangeValue();
        

        paraField[0] = new ParameterField();
        paraField[0].Name = "REXTitle";
        discreteValue[0] = new ParameterDiscreteValue();
        discreteValue[0].Value = (String)GetGlobalResourceObject("ReportInfo", "Rpt_DeptUser");
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

        string where = "";
        if (ddlProjuct.Text != "")
        {
            where += " and dept.deptid='" + ddlProjuct.SelectedValue.ToString().Trim() + "' ";
        }
        if (cmbUserState.Text != "")
        {
            where += " and users.userstatus='" + cmbUserState.SelectedValue.Trim() + "' ";
        }
        if (txtName.Text != "")
        {
            where += " and users.username like '%" + txtName.Text.Trim() + "%' ";
        }

        string str_sql = @"select userrole.userid,users.usercode,users.username,userrole.deptid,dept.deptcode,dept.deptname,dept.orderid,
                            users.userstatus,case users.userstatus when 1 then '有效' when 2 then '离职' when 3 then '禁用' end status
                            from userrole
                            inner join users on users.userid=userrole.userid
                            inner join dept on dept.deptid=userrole.deptid
                            where 1=1 " + where;
        str_sql+=" order by dept.orderid,userrole.deptid,users.userstatus ";


        Session["paraFil"] = paraFields;
        Session["sql"] = str_sql;
        Session["rpUrl"] = "..\\ReportM\\Report\\Mi\\Base\\RptDeptUser.rpt";

    }
    protected void BtnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/ReportM/RptBase/DeptUser.aspx");

    }
}
