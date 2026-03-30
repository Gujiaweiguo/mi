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

using BaseInfo;
using Base.Biz;
using Base.DB;
using BaseInfo.User;
using Base.Page;

public partial class BaseInfo_User_CancelUser : BasePage
{
    BaseBO bo = new BaseBO();
    protected void Page_Load(object sender, EventArgs e)
    {
    }


    //取消
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>location='javascript:history.go(-1)'</script>");
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

        string sql = "Update [users] set userStatus=2 where UserCode= '" + this.GridView1.SelectedRow.Cells[0].Text + "' and userStatus =1";


        if (bo.ExecuteUpdate(sql) != -1)
        {
            Response.Write("<script language=javasctip>alert('用户已经作废!');</script>");
        }
        else
        {
            Response.Write("<script language=javasctip>alert('作废失败!');</script>");
        }


    }


    protected void btnQuery_Click(object sender, EventArgs e)
    {
        bo.WhereClause = "UserCode='"+ this.txtUserCode.Text.Trim() +"'";
        Resultset rs=bo.Query(new Users());

        if (rs.Count == 0)
        {
            Response.Write("<script language=javascript>alert('对不起，没有你要的数据!');</script>");
            this.GridView1.Visible = false;
            this.txtUserCode.Text = string.Empty;
            return;
        }
        else
        {
            this.GridView1.Visible = true;
            this.GridView1.DataSource = rs;
            this.GridView1.DataBind();
        }
    }
}
