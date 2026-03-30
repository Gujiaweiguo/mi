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
public partial class LeaseArea_Location_CancelLocation : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    Location locationInfo = new Location();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bo.WhereClause = "LocationID=" + Convert.ToInt32(Request.QueryString["locationId"].ToString());

            Resultset rs = bo.Query(locationInfo);

            //显示 Code
            if (rs.Count != 0)
            {
                locationInfo = rs.Dequeue() as Location;
                this.txtLocationCode.Text = locationInfo.LocationCode;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        string sql = "Update [Location] Set locationStatus='" + Location.LOCATION_STATUS_INVALID + "' where LocationID=" + Convert.ToInt32(Request.QueryString["locationId"].ToString());

        //修改数据
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
