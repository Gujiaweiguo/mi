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

using RentableArea;
using Base.Biz;
using Base.DB;
using System.Text;

public partial class LeaseArea_Unit_QueryUnit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       BaseBO bo = new BaseBO();
        UnitInfo unitInfo = new UnitInfo();
        GridView1.DataSource = bo.Query(unitInfo);
        GridView1.DataBind();
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //Response.Redirect("AddUnit.aspx");
        String str = "<script>window.open('AddUnit.aspx','',height=200,width=400,status=1,toolbar=0,menubar=0);</script>";
        Response.Write(str);
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Response.Redirect("ChangeUnit.aspx?UnitID=" + GridView1.Rows[e.NewEditIndex].Cells[0].Text);
    }
}
