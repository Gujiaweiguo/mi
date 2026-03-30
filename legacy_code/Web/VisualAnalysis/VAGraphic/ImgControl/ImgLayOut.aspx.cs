using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Base.Page;
using Base.Biz;
using Base.DB;

public partial class VisualAnalysis_VAMenu_ImgControl_ImgLayOut : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string floorid = "";
        if (!IsPostBack)
        {
            if (Request.QueryString["FloorID"] != null && Request.QueryString["FloorID"] != "")
            {
                floorid = Request.QueryString["FloorID"].ToString();
                page(floorid);

            }
            //page("103");
        }

    }
    protected void page(string Floorid)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        string strWhere = "select unit.unitid,unit.unitcode,map,x,y from unit where unit.floorid=" + Floorid;
        DataTable dt = baseBO.QueryDataSet(strWhere).Tables[0];
        gvImages.DataSource = dt.DefaultView;
        gvImages.DataBind();
        for (int i = 0; i < gvImages.Rows.Count; i++)
        {
            Image img = (Image)gvImages.Rows[i].FindControl("img");
            TextBox txtitemx = (TextBox)this.gvImages.Rows[i].FindControl("txtx");
            TextBox txtitemy = (TextBox)this.gvImages.Rows[i].FindControl("txty");
            int x = Convert.ToInt32(txtitemx.Text)+10;
            int y = Convert.ToInt32(txtitemy.Text)+50;

            img.Style.Clear();
            img.Attributes.Add("Style", "left: " + x + "px; position: absolute;top:" + y + "px");
            img.Attributes.Add("src", "../../VAGraphic/Img/floors/" + Floorid + ".png");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("unitid");
        dt.Columns.Add("unitx");
        dt.Columns.Add("unity");
        for (int i = 0; i < gvImages.Rows.Count; i++)
        {
            TextBox txtunitid = (TextBox)this.gvImages.Rows[i].FindControl("unitid");
            TextBox txtitemx = (TextBox)this.gvImages.Rows[i].FindControl("txtx");
            TextBox txtitemy = (TextBox)this.gvImages.Rows[i].FindControl("txty");
            DataRow dr = dt.NewRow();
            dr["unitid"] = txtunitid.Text.ToString();
            dr["unitx"] = txtitemx.Text.ToString();
            dr["unity"] = txtitemy.Text.ToString();
            dt.Rows.Add(dr);
        }
        BaseBO baseBO = new BaseBO();
        BaseTrans basetrans = new BaseTrans();
        basetrans.BeginTrans();
        try
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                
               string insertSQL = "update unit set x='" + dt.Rows[j]["unitx"].ToString() + "',y='" + dt.Rows[j]["unity"].ToString() + "' where unitid='" + dt.Rows[j]["unitid"].ToString() + "'";
               int Q= basetrans.ExecuteUpdate(insertSQL);
            }
        }
        catch
        {
            basetrans.Rollback();
        }
        basetrans.Commit();

        Response.Redirect("~/VisualAnalysis/VAGraphic/ImgControl/ImgLayOut.aspx"); 
    }
}
