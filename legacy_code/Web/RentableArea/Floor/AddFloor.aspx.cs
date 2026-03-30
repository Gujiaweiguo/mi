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

public partial class LeaseArea_AddFloor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //状态
            int[] status = Floors.GetFloorStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbFloorStatus.Items.Add(new ListItem(Floors.GetFloorStatusDesc(status[i]), status[i].ToString()));
            }

        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        BaseBO bo = new BaseBO();
        Floors floorInfo = new Floors();
        floorInfo.FloorID = RentableAreaApp.GetID("Floors", "FloorID");
        floorInfo.FloorCode = this.txtFloorCode.Text.Trim();
        floorInfo.FloorName = this.txtFloorName.Text.Trim();
        floorInfo.FloorStatus = Convert.ToInt32(this.cmbFloorStatus.SelectedItem.Value);
        floorInfo.Note = this.txtNote.Text.Trim();

        //插入数据
        try
        {
            if (bo.Insert(floorInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('添加成功!');</script>");
                Response.Redirect("QueryFloor.aspx");
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
        Response.Redirect("QueryFloor.aspx");
    }
}
