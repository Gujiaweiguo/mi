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

public partial class LeaseArea_ChangBuilding : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    Building buildingInfo = new Building();

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
            bo.WhereClause = "buildingID=" + Convert.ToInt32(Request.QueryString["BuildingID"].ToString());

            Resultset rs = bo.Query(buildingInfo);

            //填充修改的记录
            if (rs.Count != 0)
            {
                buildingInfo = rs.Dequeue() as Building;
                this.txtBuildingCode.Text = buildingInfo.BuildingCode;
                this.txtBuildingName.Text = buildingInfo.BuildingName;
                this.cmbBuildingStatus.SelectedValue = buildingInfo.BuildingStatus.ToString();
                this.txtBuildingAddr.Text=buildingInfo.BuildingAddr;
                this.txtPostCode.Text=buildingInfo.PostCode;
                this.txtNote.Text = buildingInfo.Note;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        buildingInfo.BuildingCode = this.txtBuildingCode.Text.Trim();
        buildingInfo.BuildingName = this.txtBuildingName.Text.Trim();
        buildingInfo.BuildingStatus = Convert.ToInt32(this.cmbBuildingStatus.SelectedItem.Value);
        buildingInfo.BuildingAddr=this.txtBuildingAddr.Text.Trim();
        buildingInfo.PostCode=this.txtPostCode.Text.Trim();
        buildingInfo.Note = this.txtNote.Text.Trim();

        //修改
        try
        {

            bo.WhereClause = "buildingID=" + Convert.ToInt32(Request.QueryString["buildingId"].ToString());
            if (bo.Update(buildingInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('修改成功!!');</script>");
                Response.Redirect("QueryBuilding.aspx");
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("QueryBuilding.aspx");
    }
}
