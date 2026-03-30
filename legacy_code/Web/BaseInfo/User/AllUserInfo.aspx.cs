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

using BaseInfo.User;
using Base.Page;
using Base.Biz;
using Base.DB;

public partial class BaseInfo_User_AllUserInfo : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseBO bo = new BaseBO();

        bo.AppendWhere = false;
        bo.WhereClause = null;


        //得到数据集

        Resultset rs = bo.Query(new Users());

        if (rs.Count == 0)   //没有查询的数据没有符合要求的
        {
            Response.Write("<script languge=javascript>alert('密码不正确!');</script>");
            return;
        }

        this.GridView1.DataSource = rs;
        this.GridView1.DataBind();
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string id = this.GridView1.SelectedRow.Cells[0].Text;
        Session["QueryId"] = id;
        Response.Redirect("ChangUser.aspx?id=" + id);
    }

}
