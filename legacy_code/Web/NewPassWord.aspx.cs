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
using Base.DB;
using Base;
using Base.Page;
using Base.Util;
using Sell;
using BaseInfo.User;
using Base.Sys;
public partial class NewPassWord :BasePage 
{
    private string strPwd="";
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_NewPassword");
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        txtUsrID.Text = objSessionUser.UserID.ToString(); ;
        txtUsrName.Text = objSessionUser.UserName;
        strPwd = objSessionUser.Password;
        
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        PassWord pwd = new PassWord();
        pwd.EncryptDecrypStr = txtoldUsrPwd.Text.Trim();
        pwd.DesEncrypt();
        if (!strPwd.Equals(pwd.MyDesStr))
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "User_ErrorPassword") + "'", true);
        }
        else
        {
            if (txtNewPwd.Text.Trim() != txtNewPwdT.Text.Trim())
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + Label1.Text + "<>" + Label2.Text + "'", true);
            }
            else
            {
                try
                {
                    pwd.EncryptDecrypStr = "";
                    pwd.EncryptDecrypStr = txtNewPwd.Text.Trim();
                    pwd.DesEncrypt();
                    string sql = "Update Users set Password='" + pwd.MyDesStr + "' where UserId='" + txtUsrID.Text.Trim() + "'";
                    BaseBO baseBO = new BaseBO();
                    baseBO.ExecuteUpdate(sql);
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
        }

    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        txtNewPwd.Text = "";
        txtoldUsrPwd.Text = "";
        txtNewPwdT.Text = "";
    }


}
