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

public partial class LeaseArea_AddBuilding : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //状态
            int[] status = Building.GetBuildingStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbBuildingStatus.Items.Add(new ListItem(Building.GetBuildingStatusDesc(status[i]), status[i].ToString()));
            }

        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        BaseBO bo = new BaseBO();
        Building buildingInfo = new Building();
        buildingInfo.BuildingID = RentableAreaApp.GetID("Building", "BuildingID");
        buildingInfo.BuildingCode = this.txtBuildingCode.Text.Trim();
        buildingInfo.BuildingName = this.txtBuildingName.Text.Trim();
        buildingInfo.PostCode = this.txtPostCode.Text.Trim();
        buildingInfo.BuildingStatus = Convert.ToInt32(this.cmbBuildingStatus.SelectedItem.Value);
        buildingInfo.BuildingAddr = this.txtBuildingAddr.Text.Trim();
        buildingInfo.Note = this.txtNote.Text;

        //插入
        try
        {
            if (bo.Insert(buildingInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('添加成功!');</script>");
                Response.Redirect("QueryBuilding.aspx");
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
        Response.Redirect("QueryBuilding.aspx");
    }
}
