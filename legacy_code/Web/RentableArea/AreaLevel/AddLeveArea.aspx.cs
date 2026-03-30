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

public partial class LeaseArea_AddArea : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //状态


            int[] status = AreaLevel.GetAreaLevelStatus();
            for(int i = 0; i < status.Length; i++)
            {
                this.cmbAreaStatus.Items.Add(new ListItem(AreaLevel.GetAreaLevelStatusDesc(status[i]), status[i].ToString()));
            }

        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        AreaLevel areaLevel = new AreaLevel();
        areaLevel.AreaLevelID = RentableAreaApp.GetID("AreaLevel", "AreaLevelID");
        areaLevel.AreaLevelCode= this.txtAreaCode.Text.Trim();
        areaLevel.AreaLevelName = this.txtAreaName.Text.Trim();
        areaLevel.AreaLevelStatus = Convert.ToInt32(this.cmbAreaStatus.SelectedItem.Value);
        areaLevel.Note = this.txtNote.Text.Trim();

        //插入数据
        try
        {
            if (baseBO.Insert(areaLevel) != -1)
            {
                Response.Write("<script language=javascript>alert('添加成功!');</script>");
                Response.Redirect("QueryLeveArea.aspx");
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
        Response.Redirect("QueryLeveArea.aspx");
    }
}
