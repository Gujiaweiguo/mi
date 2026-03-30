<%@ WebHandler Language="C#" Class="checkCustCodeHandler" %>

using System;
using System.Web;
using Base.Biz;
using Base.DB;
using BaseInfo.Store;
using System.Data;

public class checkCustCodeHandler : IHttpHandler {
    public string strCode = string.Empty;
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "text/plain";
        strCode = context.Request.QueryString["code"].ToString();
        string flag = string.Empty;
        if (this.CheckCode(strCode))
        {
            flag = "1";
        }
        else
            flag = "0";  
        context.Response.Write(flag);
    }
    public bool CheckCode(string strCode)
    {
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select CustCode from potcustomer where CustCode='" + strCode + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
            return false;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}