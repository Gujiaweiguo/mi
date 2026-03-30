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


public partial class LeaseArea_Floor_ChangFloor : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    Floors floorInfo = new Floors();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //填充状态
            int[] status = Floors.GetFloorStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbFloorStatus.Items.Add(new ListItem(Floors.GetFloorStatusDesc(status[i]), status[i].ToString()));
            }

            bo.WhereClause = "FloorID=" + Convert.ToInt32(Request.QueryString["FloorID"].ToString());

            Resultset rs = bo.Query(floorInfo);

            //显示要更新的数据
            if (rs.Count != 0)
            {
                floorInfo = rs.Dequeue() as Floors;
                this.txtFloorCode.Text = floorInfo.FloorCode;
                this.txtFloorName.Text = floorInfo.FloorName;
                this.cmbFloorStatus.SelectedValue= floorInfo.FloorStatus.ToString();
                this.txtNote.Text = floorInfo.Note;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {

        floorInfo.FloorCode = this.txtFloorCode.Text.Trim();
        floorInfo.FloorName = this.txtFloorName.Text.Trim();
        floorInfo.FloorStatus = Convert.ToInt32(this.cmbFloorStatus.SelectedItem.Value);
        floorInfo.Note = this.txtNote.Text.Trim();

        //修改数据
        try
        {

            bo.WhereClause = "FloorID=" + Convert.ToInt32(Request.QueryString["floorId"].ToString());
            if (bo.Update(floorInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('修改成功!!');</script>");
                Response.Redirect("QueryFloor.aspx");
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
        Response.Redirect("QueryFloor.aspx");
    }
}
