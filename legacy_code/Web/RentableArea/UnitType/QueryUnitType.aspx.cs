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

public partial class LeaseArea_UnitType_QueryUnitType : System.Web.UI.Page
{
    BaseBO bo = new BaseBO();
    protected void btnOk_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddUnitType.aspx");

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        UnitTypes unitTypeInfo = new UnitTypes();
        bo.WhereClause = "UnitTypeStatus=" + UnitTypes.UNITYEPE_STATUS_VALID;
        GridView1.DataSource = bo.Query(unitTypeInfo);
        GridView1.DataBind();
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sql = "Update UnitType Set UnitTypeStatus='" + UnitTypes.UNITYEPE_STATUS_INVALID + "' where UnitTypeID=" + Convert.ToInt32(GridView1.SelectedRow.Cells[0].Text);

        if (bo.ExecuteUpdate(sql) != -1)
        {
            Response.Write("<script language=javascript>alert(\"作废成功!\");</script>");
            Response.Redirect("QeryLocation.aspx");
        }
        else
        {
            Response.Write("<script language=javascript>alert(\"作废失败!\");</script>");
        }
    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Response.Redirect("ChangeUnitType.aspx?UnitTypeID=" + GridView1.Rows[e.NewEditIndex].Cells[0].Text);
    }
}
