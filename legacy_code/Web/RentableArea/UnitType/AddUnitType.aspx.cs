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
public partial class LeaseArea_AddUnitType : System.Web.UI.Page
{
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

        }
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        BaseBO bo = new BaseBO();
        UnitTypes unitTypeInfo = new UnitTypes();
        unitTypeInfo.UnitTypeID = RentableAreaApp.GetID("UnitType", "UnitTypeID");
        unitTypeInfo.UnitTypeCode = this.txtUnitTypeCode.Text.Trim();
        unitTypeInfo.UnitTypeName = this.txtUnitTypeName.Text.Trim();
        unitTypeInfo.UnitTypeStatus = Convert.ToInt32(this.cmbUnitTypeStatus.SelectedItem.Value);
        unitTypeInfo.Note = this.txtNote.Text.Trim();

        //插入数据
        try
        {
            if (bo.Insert(unitTypeInfo) != -1)
            {
                Response.Write("<script language=javascript>alert('添加成功!');</script>");
                Response.Redirect("QueryUnitType.aspx");
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
        Response.Redirect("QueryUnitType.aspx");
    }
}
