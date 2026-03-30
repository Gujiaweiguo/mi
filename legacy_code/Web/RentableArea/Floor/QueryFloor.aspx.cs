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

using System.Text;

using RentableArea;
using Base.Biz;
using Base.DB;

public partial class LeaseArea_Floor_QueryFloor : System.Web.UI.Page
{
    BaseBO baseBO = new BaseBO();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        Floors floorInfo = new Floors();

        baseBO.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID;
        GridView1.DataSource = baseBO.Query(floorInfo);
        GridView1.DataBind();
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddFloor.aspx");

    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = "Update Floors Set FloorStatus='" + Floors.FLOOR_STATUS_INVALID + "' where FloorID=" + Convert.ToInt32(GridView1.SelectedRow.Cells[0].Text);

        if (baseBO.ExecuteUpdate(sql) != -1)
        {
            Response.Write("<script language=javascript>alert(\"作废成功!\");</script>");
            Response.Redirect("QueryFloor.aspx");
        }
        else
        {
            Response.Write("<script language=javascript>alert(\"作废失败!\");</script>");
        }
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Response.Redirect("ChangeFloor.aspx?FloorID=" + GridView1.Rows[e.NewEditIndex].Cells[0].Text);
    }
}
