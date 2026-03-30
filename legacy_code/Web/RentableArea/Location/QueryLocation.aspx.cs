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

public partial class LeaseArea_Location_QeryLocation : System.Web.UI.Page
{ 
    BaseBO baseBO = new BaseBO();
    protected void Page_Load(object sender, EventArgs e)
    {
        Location locationInfo = new Location();
        baseBO.WhereClause = "LocationStatus=" + Location.LOCATION_STATUS_VALID;
        GridView1.DataSource = baseBO.Query(locationInfo);
        GridView1.DataBind();
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddLocation.aspx");
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string sql = "Update Location Set LocationStatus='" + Location.LOCATION_STATUS_INVALID + "' where LocationID=" + Convert.ToInt32(GridView1.SelectedRow.Cells[0].Text);

        //if (baseBO.ExecuteUpdate(sql) != -1)
        //{
        //    Response.Write("<script language=javascript>alert(\"作废成功!\");</script>");
        //    Response.Redirect("QeryLocation.aspx");
        //}
        //else
        //{
        //    Response.Write("<script language=javascript>alert(\"作废失败!\");</script>");
        //}
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Response.Redirect("ChangeLocation.aspx?LocationID=" + GridView1.Rows[e.NewEditIndex].Cells[0].Text);
    }
}
