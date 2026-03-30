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


using Base.Biz;
using RentableArea;

public partial class LeaseArea_AddLocation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //填充状态

            int[] status = Area.GetAreaStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbLocationStatus.Items.Add(new ListItem(Location.GetLocationStatusDesc(status[i]), status[i].ToString()));
            }

        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        BaseBO bo = new BaseBO();
        Location locationInfo = new Location();
        locationInfo.LocationID = RentableAreaApp.GetID("Location", "LocationID");
        locationInfo.LocationCode = this.txtLocationCode.Text.Trim();
        locationInfo.LocationName = this.txtLocationName.Text.Trim();
        locationInfo.LocationStatus = Convert.ToInt32(this.cmbLocationStatus.SelectedItem.Value);
        locationInfo.Note = this.txtNote.Text.Trim();

        try
        {
            //插入数据
            if (bo.Insert(locationInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('添加成功!');</script>");
                Response.Redirect("QueryLocation.aspx");
            }
            else
            {
                Response.Write("<script language=javascript>alert('添加失败!');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("QueryLocation.aspx");
    }
}
