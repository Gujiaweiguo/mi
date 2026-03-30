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
using BaseInfo.Dept;
using Base.Biz;
using Base.DB;
using Base.Page;
public partial class BaseInfo_Dept_DeptQuery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Button1.Attributes["OnClick"] = "Return Confirm('你确认要删除吗？')";
        if (!IsPostBack)
        {
            int[] status = Dept.GetDeptType();
            for (int i = 0; i < status.Length; i++)
            {
                DropDownList1.Items.Add(new ListItem(Dept.GetDeptTypeDesc(status[i]), status[i].ToString()));
                
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Dept Objdept = new Dept();
        BaseBO deptbo = new BaseBO();
        DataSet querydataset = new DataSet();
        StringBuilder strWhere = new StringBuilder();
        if (TextBox1.Text != null && TextBox1.Text != ""  )
        {
            strWhere.Append(" DeptCode = '" + TextBox1.Text.Trim() + "'");
        }
        if (TextBox2.Text != "" && TextBox2.Text != null)
        {
            if (strWhere.Length > 0)
            {
                strWhere.Append(" and DeptName like '%" + TextBox2.Text.Trim() + "%'");
            }
            else
            {
            strWhere.Append(" DeptName like '%" + TextBox2.Text.Trim() + "%'");
            }
        }
        
        if ( DropDownList1.Text != "" && DropDownList1.Text != null)
        {
            if (strWhere.Length > 0)
            {
                strWhere.Append(" and DeptType = '" + DropDownList1.Text.Trim() + "'");
            }
            else
            { 
                strWhere.Append(" DeptType = '" + DropDownList1.Text.Trim() + "'");
            }

        }
        if (strWhere.Length > 0)
        {
            deptbo.WhereClause = strWhere.ToString();  
        }
        querydataset= deptbo.QueryDataSet(Objdept);
        GridView1.DataSource = querydataset; //deptbo.Query(Objdept);
        GridView1.DataBind();
        
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
            Response.Redirect("DeptUpdate.aspx?name=" + GridView1.SelectedRow.Cells[6].Text.ToString());
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "c=this.style.backgroundColor;this.style.backgroundColor='#6699ff'");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=c");
        }

    }
}
