using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Globalization;
using System.Threading;
using System.Net;
using Base.Biz;

namespace Base.Page
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            //String currentculture = Session["Currentculture"].ToString();
            //if (!String.IsNullOrEmpty(currentculture))
            if (Session["Currentculture"] != null)
            {
                //UICulture - 决定了采用哪一种本地化资源，也就是使用哪种语言
                //Culture - 决定各种数据类型是如何组织，如数字与日期
                String currentculture = Session["Currentculture"].ToString();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentculture);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentculture);
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/SessionIsNull.aspx");
            }
            //不保存页面缓存
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now;
            Response.Expires = 0;
            Response.CacheControl = "no-cache";

            //清除子报表Session
            Session["subReportSql"] = null;
            Session["subReportSql1"] = null;

            if (Request.QueryString["funcid"] != null && Request.QueryString["funcid"].ToString().Trim() != "")
            {
                Session["funcID"] = Convert.ToInt32(Request.QueryString["funcid"]);
            }
            else
            {
                if (Session["funcID"] == null || Session["funcID"].ToString() == "")
                    Session["funcID"] = "65537";
                else
                    Session["funcID"] = Session["funcID"];
            }
        }

        //protected void Page_PreRenderComplete()
        //{
        //    BaseBO baseBO = new BaseBO();

        //    SessionUserLog objSessionUser = (SessionUserLog)Session["SessionUserLog"];
            
        //    OpenPageLog openPageLog = new OpenPageLog();

        //    openPageLog.CreateUserID = objSessionUser.UserID;
        //    openPageLog.CreateUserName = objSessionUser.UserName;
        //    openPageLog.OprDeptID = objSessionUser.DeptID;
        //    openPageLog.OprRoleID = objSessionUser.RoleID;
        //    openPageLog.PagePath = Request.FilePath;
        //    //客户端ip
        //    openPageLog.IpAddress = Page.Request.UserHostAddress.ToString();

        //    baseBO.ExecuteUpdate("Insert Into OpenPageLog(CreateUserID,CreateUserName,OprDeptID,OprRoleID,PagePath,CreateTime,IPAddress) Values ('" +
        //                        objSessionUser.UserID + "','" + objSessionUser.UserName + "','" + objSessionUser.DeptID + "','" + objSessionUser.RoleID + "','" +
        //                         Request.Url.LocalPath + "','" + DateTime.Now + "','" + Page.Request.UserHostAddress.ToString() + "')");
        //}
    }
}
