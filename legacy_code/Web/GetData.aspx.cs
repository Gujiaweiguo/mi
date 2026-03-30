using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Base.Biz;
using System.Data;
using System.Text;
public partial class GetData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strSql = "";
        if (IsPostBack)
            strSql = "";
        else
        {
            if (Session["getdatasql"] != null && !Session["getdatasql"].Equals(""))
            {
                strSql = Session["getdatasql"].ToString();
                Session["getdatasql"] = null;
                BaseBO baseBo = new BaseBO();
                DataSet ds = baseBo.QueryDataSet(strSql);
                string strTemp = BaseInfo.Json.GetJsonForSigma(ds.Tables[0]);
                Response.Clear();
                Response.Write(strTemp);
            }
            else
            {
                Response.Clear();
            }
        }
    }   
}
