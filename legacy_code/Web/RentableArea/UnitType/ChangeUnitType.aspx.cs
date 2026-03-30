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

public partial class LeaseArea_UnitType_ChangUnitType : System.Web.UI.Page
{

    BaseBO bo = new BaseBO();
    UnitTypes unitTypesInfo = new UnitTypes();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //填充状态
            int[] status = UnitTypes.GetUnitTypeStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbUnitTypeStatus.Items.Add(new ListItem(UnitTypes.GetUnitTypeStatusDesc(status[i]), status[i].ToString()));
            }

            bo.WhereClause = "UnitTypeID=" + Convert.ToInt32(Request.QueryString["UnitTypeID"].ToString());

            Resultset rs = bo.Query(unitTypesInfo);

            //显示要更新的数据
            if (rs.Count != 0)
            {
                unitTypesInfo = rs.Dequeue() as UnitTypes;
                this.txtUnitTypeCode.Text = unitTypesInfo.UnitTypeCode;
                this.txtUnitTypeName.Text = unitTypesInfo.UnitTypeName;
                this.cmbUnitTypeStatus.SelectedValue = unitTypesInfo.UnitTypeStatus.ToString();
                this.txtNote.Text = unitTypesInfo.Note;
            }
        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        unitTypesInfo.UnitTypeCode = this.txtUnitTypeCode.Text.Trim();
        unitTypesInfo.UnitTypeName = this.txtUnitTypeName.Text.Trim();
        unitTypesInfo.UnitTypeStatus = Convert.ToInt32(this.cmbUnitTypeStatus.SelectedValue);
        unitTypesInfo.Note = this.txtNote.Text.Trim();


        try
        {
            //修改数据
            bo.WhereClause = "UnitTypeID=" + Convert.ToInt32(Request.QueryString["UnitTypeID"].ToString());
            if (bo.Update(unitTypesInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('修改成功!!');</script>");
                Response.Redirect("QueryUnitType.aspx");
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
        Response.Redirect("QueryUnitType.aspx");
    }
}
