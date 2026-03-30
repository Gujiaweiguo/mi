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
using Base.Biz;
using Base.DB;
using BaseInfo.User;
using BaseInfo.Role;
using Base.Page;
public partial class BaseInfo_Role_Default2 : Page
{
    private int numCount = 0;
    private Resultset rs = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int[] status = BaseInfo.Role.Role.GetLeader();
            foreach (int sta in status)
            {
                this.cmbLeader.Items.Add(new ListItem(BaseInfo.Role.Role.GetLeaderDesc(sta), sta.ToString()));
            }

            int[] rolestatus = BaseInfo.Role.Role.GetRoleStatus();
            foreach (int sta in rolestatus)
            {
                this.cmbRoleStatus.Items.Add(new ListItem(BaseInfo.Role.Role.GetRoleStatusDesc(sta),sta.ToString()));
            }

            rs = new BaseBO().Query(new Role());
            GridView1.DataSource = rs;
            GridView1.DataBind();
        }
    }
    
    protected void btnSearch_Click1(object sender, EventArgs e)
    {
        
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    Role objRole = new Role();

        //    SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            
        //    objRole.CreateUserID = sessionUser.UserID;
        //    objRole.CreateTime = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
        //    objRole.ModifyUserID = sessionUser.UserID;
        //    objRole.ModifyTime = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
        //    objRole.OprRoleID = sessionUser.RoleID;
        //    objRole.OprDeptID = sessionUser.DeptID;
        //    objRole.RoleCode = this.txtRoleCode.Text.Trim();
        //    objRole.RoleName = this.txtRoleName.Text.Trim();
        //    objRole.RoleStatus = Convert.ToInt32(this.cmbRoleStatus.SelectedValue.ToString());
        //    objRole.IsLeader = Convert.ToInt16(this.cmbLeader.SelectedValue.ToString());
            
        //    int result = 0;
        //    string str = null;
        //    BaseBO objbo = new BaseBO();

        //    objbo.WhereClause = " RoleId=" ;
        //    result = objbo.Update(objRole);

        //    if (result != -1)
        //    {
        //        str = "修改成功";
        //    }
        //    else
        //    {
        //        str = "修改失败";
        //    }
        //    Response.Write("<script language=javascript>alert('" + str + "');</script>");
        //}
        //catch(Exception ex)
        //{
        //    Response.Write(ex.ToString());
        //}
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (rs != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 计算自动填充的行数
                numCount++;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                // 计算完毕，在此添加缺少的行

                int toLeft = GridView1.PageSize - numCount;
                int numCols = GridView1.Rows[0].Cells.Count - 1;

                for (int i = 0; i < toLeft; i++)
                {
                    GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                    for (int j = 0; j < numCols; j++)
                    {
                        TableCell cell = new TableCell();
                        cell.Text = "&nbsp;";
                        row.Cells.Add(cell);
                    }
                    GridView1.Controls[0].Controls.AddAt(numCount + 1 + i, row);
                }
            }
        }
        else
        {
           // Table table = new Table();
            int toLeft = GridView1.PageSize - numCount;
            int numCols = GridView1.Rows[0].Cells.Count - 1;

            for (int i = 0; i < toLeft; i++)
            {
                GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
                for (int j = 0; j < numCols; j++)
                {
                    TableCell cell = new TableCell();
                    cell.Text = "&nbsp;";
                    row.Cells.Add(cell);
                }
                GridView1.Controls[0].Controls.AddAt(numCount + 1 + i, row);
            }

        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        baseBO.WhereClause = "RoleID=" + GridView1.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(new Role());
        if (rs.Count == 1)
        {

            Role role = rs.Dequeue() as Role;
            txtRoleCode.Text = role.RoleCode;
            txtRoleName.Text = role.RoleName;
            cmbRoleStatus.SelectedValue = role.RoleStatus.ToString();
            cmbLeader.SelectedValue = role.IsLeader.ToString();
            
        }
    }
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Role objRole = new Role();

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            objRole.CreateUserID = sessionUser.UserID;
            objRole.CreateTime = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
            objRole.ModifyUserID = sessionUser.UserID;
            objRole.ModifyTime = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
            objRole.OprRoleID = sessionUser.RoleID;
            objRole.OprDeptID = sessionUser.DeptID;
            objRole.RoleCode = this.txtRoleCode.Text.Trim();
            objRole.RoleName = this.txtRoleName.Text.Trim();
            objRole.RoleStatus = Convert.ToInt32(this.cmbRoleStatus.SelectedValue.ToString());
            objRole.IsLeader = Convert.ToInt16(this.cmbLeader.SelectedValue.ToString());

            int result = 0;
            string str = null;
            BaseBO objbo = new BaseBO();

            objbo.WhereClause = " RoleId=";
            result = objbo.Update(objRole);

            if (result != -1)
            {
                str = "修改成功";
            }
            else
            {
                str = "修改失败";
            }
            Response.Write("<script language=javascript>alert('" + str + "');</script>");
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }
    }
}
