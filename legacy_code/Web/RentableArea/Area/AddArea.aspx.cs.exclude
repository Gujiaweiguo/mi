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

public partial class LeaseArea_AddArea : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //状态
            int[] status = Area.GetAreaStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbAreaStatus.Items.Add(new ListItem(Area.GetAreaStatusDesc(status[i]), status[i].ToString()));
            }

        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        BaseBO bo = new BaseBO();
        Area areaInfo = new Area();
        areaInfo.AreaID = RentableAreaApp.GetID("Area", "AreaID");
        areaInfo.AreaCode = this.txtAreaCode.Text.Trim();
        areaInfo.AreaName = this.txtAreaName.Text.Trim();
        areaInfo.AreaStatus =Convert.ToInt32( this.cmbAreaStatus.SelectedItem.Value);
        areaInfo.Note = this.txtNote.Text.Trim();

        //插入数据
        try
        {
            if (bo.Insert(areaInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('添加成功!');</script>");
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
        Response.Redirect("QueryArea.aspx");
    }
}
