using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSql = "";
        if (!IsPostBack)
        {
            Session["getdatasql"] = strSql;
        }
        Session["getdatasql"] = "select deptcode,deptname from dept";
    }
}
