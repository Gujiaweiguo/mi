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
public partial class LeaseArea_Floor_CancelFloor : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    Floors floorInfo = new Floors();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //填充 Code 
            bo.WhereClause = "FloorID=" + Convert.ToInt32(Request.QueryString["floorId"].ToString());

            Resultset rs = bo.Query(floorInfo);

            if (rs.Count != 0)
            {
                floorInfo = rs.Dequeue() as Floors;
                this.txtFloorCode.Text = floorInfo.FloorCode;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        //修改数据
        string sql = "Update [floors] Set FloorStatus='" + Floors.FLOOR_STATUS_INVALID + "' where Floorid=" + Convert.ToInt32(Request.QueryString["floorId"].ToString());

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
