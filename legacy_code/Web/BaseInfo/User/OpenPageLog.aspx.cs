using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Page;

public partial class BaseInfo_User_OpenPageLog : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            page("Users.UserCode ='-1'");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "messagea", "Load();", true);
        }
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string sql = "1=1 and openpagelog.pagepath = '/Web/Login.aspx'";

        if (txtUserCode.Text != "")
        {
            sql = sql + " And users.UserCode ='" + txtUserCode.Text.Trim() + "'";

        }
        if (txtStartTime.Text != "")
        {
            sql = sql + " And OpenPageLog.CreateTime >='" + txtStartTime.Text.Trim() + " 00:00:00'";
        }
        if (txtEndTime.Text != "")
        {
            sql = sql + " And OpenPageLog.CreateTime <='" + txtEndTime.Text.Trim() + " 23:59:59'";
        }
            
        page(sql);
        //else
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '请输入用户登录名'", true);
        //}
    }

    protected void page(string strWhere)
    {
        ViewState["WhereStr"] = strWhere;
        string sql = "Select CreateUserName as UserName,oprRoleName as RoleName,oprDeptName as DeptName,ipaddress,OpenPagelog.CreateTime, PageName " +
            " From OpenPagelog inner join Users on Openpagelog.createuserid=users.userid" +
            " where " + strWhere +
            " order by OpenPagelog.CreateTime desc";

        BaseInfo.BaseCommon.BindGridView(sql, this.GrdCust);//绑定GridView
        ClearGridViewSelect();
    }
    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in GrdCust.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[4].Text = "";
            }
        }
    }
    protected void GrdCust_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        page(ViewState["WhereStr"].ToString());
    }
}
