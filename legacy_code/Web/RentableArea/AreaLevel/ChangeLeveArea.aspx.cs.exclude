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

public partial class LeaseArea_Area_ChangArea : System.Web.UI.Page
{

    BaseBO baseBO = new BaseBO();
    AreaLevel areaLevel = new AreaLevel();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //状态            
            int[] status = AreaLevel.GetAreaLevelStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbAreaStatus.Items.Add(new ListItem(AreaLevel.GetAreaLevelStatusDesc(status[i]), status[i].ToString()));
            }

            baseBO.WhereClause = "AreaLevelID=" + Convert.ToInt32(Request.QueryString["AreaLevelID"].ToString());

            Resultset rs = baseBO.Query(areaLevel);
             
            //填充修改的记录
            if (rs.Count != 0)
            {
                areaLevel = rs.Dequeue() as AreaLevel;
                this.txtAreaCode.Text = areaLevel.AreaLevelCode;
                this.txtAreaName.Text = areaLevel.AreaLevelName;
                this.cmbAreaStatus.SelectedValue = areaLevel.AreaLevelStatus.ToString();
                this.txtNote.Text = areaLevel.Note;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        areaLevel.AreaLevelCode = this.txtAreaCode.Text.Trim();
        areaLevel.AreaLevelName = this.txtAreaName.Text.Trim();
        areaLevel.AreaLevelStatus = Convert.ToInt32(this.cmbAreaStatus.SelectedItem.Value);
        areaLevel.Note = this.txtNote.Text.Trim();

        //修改
        try
        {

            baseBO.WhereClause = "AreaLevelID=" + Convert.ToInt32(Request.QueryString["AreaLevelID"].ToString());
            if (baseBO.Update(areaLevel) != -1)
            {
                Response.Write("<script language=javascript>alert('修改成功!!');</script>");
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
        Response.Redirect("QueryLeveArea.aspx");
    }
}
