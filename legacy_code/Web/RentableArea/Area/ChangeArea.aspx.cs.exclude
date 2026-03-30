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
    Area areaInfo = new Area();

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

            baseBO.WhereClause = "AreaID=" + Convert.ToInt32(Request.QueryString["AreaID"].ToString());

            Resultset rs = baseBO.Query(areaInfo);
             
            //填充修改的记录
            if (rs.Count != 0)
            {
                areaInfo = rs.Dequeue() as Area;
                this.txtAreaCode.Text = areaInfo.AreaCode;
                this.txtAreaName.Text = areaInfo.AreaName;
                this.cmbAreaStatus.SelectedValue=areaInfo.AreaStatus.ToString();
                this.txtNote.Text = areaInfo.Note;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        areaInfo.AreaCode = this.txtAreaCode.Text.Trim();
        areaInfo.AreaName = this.txtAreaName.Text.Trim();
        areaInfo.AreaStatus = Convert.ToInt32(this.cmbAreaStatus.SelectedItem.Value);
        areaInfo.Note = this.txtNote.Text.Trim();

        //修改
        try
        {

            baseBO.WhereClause = "AreaID=" + Convert.ToInt32(Request.QueryString["AreaID"].ToString());
            if (baseBO.Update(areaInfo) != -1)
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
        Response.Redirect("QueryArea.aspx");
    }
}
