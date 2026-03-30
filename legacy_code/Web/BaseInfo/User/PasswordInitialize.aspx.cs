using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Lease.Contract;
using BaseInfo.User;
using Base.Page;
using BaseInfo.authUser;
using Base.Sys;

public partial class BaseInfo_User_PasswordInitialize : BasePage
{
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        ViewState["strWhere"] = "and 1=2";
        if (!IsPostBack)
        {
            this.BindData(ViewState["strWhere"].ToString());
        }
    }
   
    protected void BindData(string strWhere)
    {
        string strSql = @"select Users.UserID,users.UserCode,UserName,RoleName,DeptName from users inner join userrole on  userrole.userid=users.userid inner join Role on Role.RoleID=userrole.Roleid inner join Dept on Dept.DeptID = UserRole.Deptid where 1=1 ";
        if (strWhere != "")
            strSql += strWhere;
        BaseInfo.BaseCommon.BindGridView(strSql,this.GrdUser);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strWhere = "";
        if (this.txtCode.Text.Trim() != "")
        {
            strWhere += " and Users.UserCode  like '%"+this.txtCode.Text.Trim()+"%'";
        }
        if (this.txtName.Text.Trim() != "")
        {
            strWhere += " and Users.UserName  like '%" + this.txtName.Text.Trim() + "%'";
        }
        ViewState["strWhere"] = strWhere;
        this.BindData(strWhere);
        this.btnDefault.Enabled = true;
    }
    protected void GrdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindData(ViewState["strWhere"].ToString());
    }
    /// <summary>
    /// 密码初始化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDefault_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        try
        {
            foreach (GridViewRow gr in this.GrdUser.Rows)
            {
                if (gr.Cells[0].Text.Trim() != "" && gr.Cells[0].Text.Trim() != "&nbsp;")
                {
                    PassWord pwd = new PassWord();
                    pwd.EncryptDecrypStr = "1234";
                    pwd.DesEncrypt();
                    objBaseBo.ExecuteUpdate("update Users set Password='" + pwd.MyDesStr + "' where UserID='" + gr.Cells[0].Text.Trim() + "'");
                }
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" +"密码初始化成功。" + "'", true);
        }
        catch {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "密码初始化失败。" + "'", true);
        }
        this.btnDefault.Enabled = false;
        ViewState["strWhere"] = "and 1=2";
        this.BindData(ViewState["strWhere"].ToString());
    }
}
