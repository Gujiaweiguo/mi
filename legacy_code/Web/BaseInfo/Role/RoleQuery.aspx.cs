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
using BaseInfo;
using Base.DB;
using Base.Biz;
using BaseInfo.Role;
using Base.Page;
public partial class BaseInfo_Role_Default : Page
{
    //static int intRoleId;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack) { }

        //ShowData();
    }

    protected void ShowData()
    {
        BaseBO objBo = new BaseBO();
        Role objRole = new Role();

        DataSet ds = new DataSet();
        ds = objBo.QueryDataSet(objRole);

        this.grdInfo.DataSource = ds.Tables[0].DefaultView;
        if (!IsPostBack)
        {
            this.grdInfo.DataBind();
        }
        
    }

    //protected void grdInfo_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    //{
        
    //}

    //protected void grdInfo_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    this.grdInfo.EditIndex = (int)e.NewEditIndex;
        
    //        this.grdInfo.DataBind();
     
    //}
    //protected void grdInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    this.grdInfo.EditIndex = -1;
    //    this.grdInfo.DataBind();
    //}
    //protected void grdInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        BaseBO objBo = new BaseBO();

    //        Role objRole = new Role();
       

    //        GridViewRow gvr = this.grdInfo.Rows[e.RowIndex];

    //        intRoleId = Convert.ToInt32(((TextBox)gvr.Cells[0].Controls[0]).Text);
    //        //objRole.CreateUserID = Convert.ToInt32(((TextBox)gvr.Cells[1].Controls[0]).Text);
    //        //objRole.CreateTime = Convert.ToDateTime(((TextBox)gvr.Cells[2].Controls[0]).Text);
    //        objRole.ModifyUserID =1;
    //        objRole.ModifyTime = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
    //        objRole.OprRoleID = Convert.ToInt16(((TextBox)gvr.Cells[5].Controls[0]).Text);
    //        objRole.OprDeptID = Convert.ToInt16(((TextBox)gvr.Cells[6].Controls[0]).Text);
    //        objRole.RoleCode = ((TextBox)gvr.Cells[7].Controls[0]).Text;
    //        objRole.RoleName = ((TextBox)gvr.Cells[8].Controls[0]).Text;
    //        objRole.RoleStatus = Convert.ToInt16(((TextBox)gvr.Cells[9].Controls[0]).Text);
    //        objRole.IsLeader = Convert.ToInt16(((TextBox)gvr.Cells[10].Controls[0]).Text);

            
    //        int result = 0;
    //        string str = null;
    //        BaseBO objbo = new BaseBO();

    //        objbo.WhereClause = " RoleId=" + intRoleId;
            
    //        result = objbo.Update(objRole);

    //        if (result != -1)
    //        {
    //            str = "修改成功";
    //        }
    //        else
    //        {
    //            str = "修改失败";
    //        }
    //        Response.Write("<script language=javascript>alert('" + str + "');</script>");
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write(ex.ToString());
    //    }
    //    ShowData();
    //    //finally
    //    //{
    //    //    this.grdInfo.EditIndex = -1;
    //    //    this.grdInfo.DataBind();
    //    //}
    //}
    //protected void grdInfo_SelectedIndexChanged(object sender, EventArgs e)
    //{
        
    //}

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BaseBO objBo = new BaseBO();
        Role objRole = new Role();
        DataSet ds = new DataSet();
        string strWhere="";

        if (txtRoleCode.Text != "")
        {
            strWhere = " RoleCode='" + txtRoleCode.Text.ToString().Trim() + "'";
        }
        if (txtRoleName.Text != "")
        {
            if (strWhere == "")
            {
                strWhere = " RoleName='" + txtRoleName.Text.ToString().Trim() + "'";
            }
            else
            {
                strWhere = strWhere + " and RoleName='" + txtRoleName.Text.ToString().Trim() + "'";
            }
        }

        if (strWhere != "")
        {
            objBo.WhereClause = strWhere;
        }
        ds = objBo.QueryDataSet(objRole);
        if (ds.Tables[0].Rows.Count == 0)
        {
            Response.Write("<script language=javascript>alert('无数据');</script>");
            return;
        }
        grdInfo.DataSource = ds.Tables[0].DefaultView;

        this.grdInfo.DataBind();
    }
}
