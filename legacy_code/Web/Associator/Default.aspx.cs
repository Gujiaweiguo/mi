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
using System.Data.SqlClient;

using System.Drawing;

using System.Data;

using System.IO;

using System.Drawing.Imaging;

using Base.Page;
using Base.DB;
using Base.Biz;
using Associator.Perform;

public partial class Associator_Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //MemoryStream stream = new MemoryStream();
        //SqlConnection connection = new
        //  SqlConnection(@"server=Flag;database=Ymi_net;uid=sa;pwd=sa");

        //BaseBO baseBO = new BaseBO();
        //Resultset rs = new Resultset();
        //LCust lCust = new LCust();

        //baseBO.WhereClause = "MemberId =" + Request.QueryString["Pic"];

        //rs = baseBO.Query(lCust);

        //if (rs.Count == 1)
        //{
        //    lCust = rs.Dequeue() as LCust;
        //    byte[] image = (byte[])lCust.MemberPic;
        //    stream.Write(image, 0, image.Length);
        //    Bitmap bitmap = new Bitmap(stream);
        //    Response.ContentType = "image/gif";
        //    bitmap.Save(Response.OutputStream, ImageFormat.Gif);

        //}

        //try
        //{
        //    connection.Open();
        //    SqlCommand command = new
        //      SqlCommand("select lcustpic from lcust", connection);

        //}
        //finally
        //{
        //    connection.Close();
        //    stream.Close();
        //}
    }
    //private void Page_Load(object sender, System.EventArgs e)
    //{
    //    // 在此处放置用户代码以初始化页面
    //    if (!this.IsPostBack)
    //    {
    //        BindGrid();
    //    }

    //}
    //private void BindGrid()
    //{
    //    string strCnn = "Data Source=.;Initial Catalog=ymi_net;User Id=sa;Password=sa;";
    //    SqlConnection myConnection = new SqlConnection(strCnn);
    //    SqlCommand myCommand = new SqlCommand("SELECT * FROM Person", myConnection);
    //    myCommand.CommandType = CommandType.Text;
    //    try
    //    {
    //        myConnection.Open();
    //        DG_Persons.DataSource = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
    //        DG_Persons.DataBind();
    //    }
    //    catch (SqlException SQLexc)
    //    {
    //        Response.Write("提取数据时出现错误：" + SQLexc.ToString());
    //    }
    //}
    //protected string FormatURL(object strArgument)
    //{
    //    return "ReadImage.aspx?id=" + strArgument.ToString();
    //}

    //#region Web Form Designer generated code
    //override protected void OnInit(EventArgs e)
    //{
    //    //
    //    // CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
    //    //
    //    InitializeComponent();
    //    base.OnInit(e);
    //}
    ///// <summary>
    ///// 设计器支持所需的方法 - 不要使用代码编辑器修改
    ///// 此方法的内容。
    ///// </summary>
    //private void InitializeComponent()
    //{
    //    this.Load += new System.EventHandler(this.Page_Load);
    //}
    //#endregion

}
