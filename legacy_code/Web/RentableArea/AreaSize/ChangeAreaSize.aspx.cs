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
using Base.DB;
public partial class RentableArea_AreaSize_ChangeAreaSize : System.Web.UI.Page
{
    AreaSize areaSize = new AreaSize();
    BaseBO baseBO = new BaseBO();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int[] status = AreaSize.GetAreaSizeStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbAreaSizeStatus.Items.Add(new ListItem(AreaSize.GetAreaSizeStatusDesc(status[i]), status[i].ToString()));
            }

            baseBO.WhereClause = "AreaSizeID=" + Convert.ToInt32(Request.QueryString["AreaSizeID"].ToString());

            Resultset rs = baseBO.Query(areaSize);

            //填充修改的记录

            if (rs.Count != 0)
            {
                areaSize = rs.Dequeue() as AreaSize;
                this.txtAreaSizeCode.Text = areaSize.AreaSizeCode;
                this.txtAreaSizeName.Text = areaSize.AreaSizeName;
                this.cmbAreaSizeStatus.SelectedValue = areaSize.AreaSizeStatus.ToString();
                this.txtNote.Text = areaSize.Note;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {

        areaSize.AreaSizeCode = this.txtAreaSizeCode.Text;
        areaSize.AreaSizeName = this.txtAreaSizeName.Text;
        areaSize.AreaSizeStatus = Convert.ToInt32(this.cmbAreaSizeStatus.SelectedValue);
        areaSize.Note = this.txtNote.Text;

        try
        {
            baseBO.WhereClause = "AreaSizeID=" + Convert.ToInt32(Request.QueryString["AreaSizeID"].ToString());
            if (baseBO.Update(areaSize) != -1)
            {
                Response.Write("<script language=javascript>alert('修改成功!!');</script>");
                Response.Redirect("QueryAreaSize.aspx");
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
        Response.Redirect("QueryAreaSize.aspx");
    }
}
