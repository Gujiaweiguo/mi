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


using Base.Biz;
using BaseInfo.User;


public partial class BaseInfo_User_ChangPassword : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
      //  string sql = "Update [users] set password = '"+this.txtPassword.Text.Trim()+"' Where UserId='"+ Convert.ToInt32( Session["Password"].ToString()) +"'";
        string sql = "Update [users] set password = '" + this.txtPassword.Text.Trim() + "' Where UserId= '"+ Convert.ToInt32( this.Session["Password"].ToString())+"'";

        if (bo.ExecuteUpdate(sql) != -1)
        {
            Response.Write("<script language=javascript>alert('密码修改成功!!');</script>");
        }
        else
        {
            Response.Write("<script language=javascript>alert('密码修改失败!!');</script>");
        }
    }
}
