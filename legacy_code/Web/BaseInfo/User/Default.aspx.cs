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
using System.Drawing;

using Base;
using BaseInfo;
using Base.Biz;
using Base.DB;
using Base.Page;

namespace MI_Net
{
    public partial class _Default : BasePage
    {

        //获得数据集,显示所有的用户
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { 
                BaseBO bo = new BaseBO();
                DataSet ds = bo.QueryDataSet("select *from [users]");     //获得数据集

                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }


        //修改数据
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = this.GridView1.SelectedRow.Cells[0].Text;     
            Response.Redirect("ChangUser.aspx?id=" + id);
        }
    }
}
