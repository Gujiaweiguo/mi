using System;
using System.Collections.Generic;
using System.Text;

using System.Web;

namespace Base.Sys
{
    public class ClearCookies:System.Web.UI.Page
    {
        public void clearCookies()
        {
            ///*删除Cookies客户ID*/
            //HttpCookie cookiesCustumer = new HttpCookie("Custumer");

            //cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
            //cookiesCustumer.Values.Add("CustumerID", "");
            //Response.AppendCookie(cookiesCustumer);

            ///*删除Cookies驳回状态*/
            //HttpCookie cookiesDisprove = new HttpCookie("Disprove");

            //cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
            //cookiesDisprove.Values.Add("DisproveID", "");
            //Response.AppendCookie(cookiesDisprove);


            ///*删除Cookies工作流ID和节点ID*/
            //HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

            //cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
            //cookiesWorkFlow.Values.Add("WorkFlowID", "");
            //cookiesWorkFlow.Values.Add("NodeID", "");
            //Response.AppendCookie(cookiesWorkFlow);


            ///*清除合同Cookies 合同ID,工作流ID,节点ID,单据ID*/
            //HttpCookie cookies = new HttpCookie("Info");
            //cookies.Expires = System.DateTime.Now.AddDays(1);
            //cookies.Values.Add("conID", "");
            //cookies.Values.Add("wrkFlwID", "");
            //cookies.Values.Add("sequence", "");
            //cookies.Values.Add("nodeID", "");

        }
    }
}
