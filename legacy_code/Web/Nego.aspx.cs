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

public partial class Nego : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BandGridView();
        }
    }

    private void BandGridView()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("temp_id");
        dt.Columns.Add("temp_name");
        dt.Columns.Add("temp_url");

        if (dt.Rows.Count == 0)
        {
            dt.Rows.Add(dt.NewRow());
        }

        this.GridView1.DataSource = dt;
        this.GridView1.DataBind();

        int columnCount = dt.Columns.Count;
        GridView1.Rows[0].Cells.Clear();
        GridView1.Rows[0].Cells.Add(new TableCell());
        GridView1.Rows[0].Cells[0].ColumnSpan = columnCount;
        GridView1.Rows[0].Cells[0].Text = "没有记录";
        GridView1.Rows[0].Cells[0].Style.Add("text-align", "center");
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
     
    }
}
