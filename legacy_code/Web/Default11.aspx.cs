using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Base;
using Base.Biz;
using BaseInfo.Dept;

public partial class Default11 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSql = "Select deptcode,deptname from dept";
        DataTable dt = new DataTable();
        BaseBO basebo = new BaseBO();
        dt = basebo.QueryDataSet(strSql).Tables[0];
        txtBox.Text= BaseInfo.Json.GetJson(dt);
    }
}
