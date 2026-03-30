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

using Base.DB;
using Base.Biz;
using RentableArea;

public partial class LeaseArea_Building_CancelBuilding : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    Building buildingInfo = new Building();

    protected void Page_Load(object sender, EventArgs e)
    {
        //填充代码
        if (!IsPostBack)
        {
            bo.WhereClause = "buildingID=" + Convert.ToInt32(Request.QueryString["buildingId"].ToString());

            Resultset rs = bo.Query(buildingInfo);

            if (rs.Count != 0)
            {
                buildingInfo = rs.Dequeue() as Building;
                this.txtBuildingCode.Text = buildingInfo.BuildingCode;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //修改
        string sql = "Update [building] Set buildingStatus='" + Building.BUILDING_STATUS_INVALID + "' where buildingID=" + Convert.ToInt32(Request.QueryString["buildingId"].ToString());

        if (bo.ExecuteUpdate(sql) != -1)
        {
            Response.Write("<script language=javascript>alert(\"作废成功!\");</script>");
        }
        else
        {
            Response.Write("<script language=javascript>alert(\"作废失败!\");</script>");
        }
    }
}
