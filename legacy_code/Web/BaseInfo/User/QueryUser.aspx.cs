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
using System.Text;

using Base.Biz;
using Base.DB;
using BaseInfo;
using System.Data.Common;
using BaseInfo.User;
using Base.Page;


namespace MI_Net
{
    public partial class QueryUser : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        //按条件查询用户
        protected void btnOk_Click(object sender, EventArgs e)
        {

            BaseBO bo = new BaseBO();

            Users objUser = new Users();

            objUser.UserName = this.txtUserName.Text.Trim();
            objUser.WorkNo = this.txtWorkNo.Text.Trim();

            DateTime dt = DateTime.Now;
            if (string.IsNullOrEmpty(txtValidDate.Text.Trim()))   //如果日期没有输入刚默认为当前的日期
                objUser.ValidDate = dt;
            else
                objUser.ValidDate = Convert.ToDateTime(txtValidDate.Text.Trim());

            string userName = objUser.UserName;
            string workNo = objUser.WorkNo;
            DateTime validDate = objUser.ValidDate;

            StringBuilder builder = new StringBuilder();     //  builder获得要查询的SQL语句

            if (!string.IsNullOrEmpty(userName))
            {
                builder.Append("userName='"+this.txtUserName.Text+"'");
            }

            if (!string.IsNullOrEmpty(workNo))
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append("And workNo='" + this.txtWorkNo.Text + "'");
                }
                else
                {
                    builder.Append("workNo='"+this.txtWorkNo.Text+"'");
                }
            }

            if (validDate != dt)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append("And validDate='" + this.txtValidDate.Text + "'");
                }
                else
                {
                    builder.Append("validDate='" + this.txtValidDate.Text + "'");
                }
            }

            bo.WhereClause = builder.ToString();

            try
            {

                //得到数据集

                Resultset rs = bo.Query(new Users());

                if (rs.Count == 0)   //没有查询的数据没有符合要求的
                {
                    Response.Write("<script languge=javascript>alert('没有你要查找的数据!');</script>");
                    this.GridView1.Visible = false;
                    this.txtUserName.Text = string.Empty;
                    this.txtWorkNo.Text = string.Empty;
                    this.txtValidDate.Text = string.Empty;

                    return;
                }
                this.GridView1.Visible = true;
                this.GridView1.DataSource = rs;
                this.GridView1.DataBind();

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }


        //取消
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Write("<script>location='javascript:history.go(-1)'</script>");
        }


        //修改个人资料
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)     
        {
            string id = this.GridView1.SelectedRow.Cells[0].Text;
            Session["QueryId"] = id;
            Response.Redirect("ChangUser.aspx?id=" + id);
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;

            this.GridView1.DataBind();
        }

}
}
