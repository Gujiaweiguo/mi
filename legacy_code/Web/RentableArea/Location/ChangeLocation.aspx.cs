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


public partial class LeaseArea_Location_ChangLocation : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    Location locationInfo = new Location();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //填充状态
            int[] status = Location.GetLocationStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbLocationStatus.Items.Add(new ListItem(Location.GetLocationStatusDesc(status[i]), status[i].ToString()));
            }

            bo.WhereClause = "LocationID=" + Convert.ToInt32(Request.QueryString["LocationID"].ToString());

            Resultset rs = bo.Query(locationInfo);

            //显示要更新的数据
            if (rs.Count != 0)
            {
                locationInfo = rs.Dequeue() as Location;
                this.txtLocationCode.Text = locationInfo.LocationCode;
                this.txtLocationName.Text = locationInfo.LocationName;
                this.cmbLocationStatus.SelectedValue= locationInfo.LocationStatus.ToString();
                this.txtNote.Text = locationInfo.Note;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        locationInfo.LocationCode = this.txtLocationCode.Text.Trim();
        locationInfo.LocationName = this.txtLocationName.Text.Trim();
        locationInfo.LocationStatus = Convert.ToInt32(this.cmbLocationStatus.SelectedItem.Value);
        locationInfo.Note = this.txtNote.Text.Trim();


        try
        {
            //修改数据
            bo.WhereClause = "LocationID=" + Convert.ToInt32(Request.QueryString["locationId"].ToString());
            if (bo.Update(locationInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('修改成功!!');</script>");
                Response.Redirect("QueryLocation.aspx");
            }
            else
            {
                Response.Write("<script language=javascript>alert('修改失败!!');</script>");
            }

        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
}
