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
using System.IO;


public partial class BaseInfo_User_ShowImage : System.Web.UI.Page
{

    // 初使化 显示图片
    protected void Page_Load(object sender, EventArgs e)
    {

        //System.Web.HttpContext.Current.Response.ContentType = "image/pjpeg";

        //System.Drawing.Image _image = System.Drawing.Image.FromStream(new System.IO.MemoryStream((byte[])Session["image"]));

        //System.Drawing.Image _newimage = _image.GetThumbnailImage(228, 145, null, new System.IntPtr());

        //_newimage.Save(System.Web.HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);


        SqlConnection con = new SqlConnection("server=192.168.0.16;uid=sa;pwd=sa;database=Mi_NET");

        //int id=Convert.ToInt32( Session["UpdateUserId"].ToString());

        int id = 1;

        if(Session["QueryId"]!=null)
        {
           id=Convert.ToInt32(Session["QueryId"].ToString());
        }
        string sql = "SELECT * FROM [Users] WHERE userid ="+id ;

        SqlCommand command = new SqlCommand(sql, con);

        con.Open();
        
        try
        {
            SqlDataReader dr = command.ExecuteReader();

            if (dr.Read())
            {
                Response.BinaryWrite((byte[])dr["Photo"]);   //显示照片
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
        }

        con.Close();
    //    BaseBO bo = new BaseBO();
    //    DataSet ds = bo.QueryDataSet("select *from [user]");

    //    MemoryStream localFile = "jpeg/gif";
    //    byte[] tempFile = new byte[(int)localFile.Length];
    //    localFile.Read(tempFile, 0, (int)localFile.Length);
    //    int Length = (int)localFile.Length;
    //    localFile.Close();
    //    this.Response.OutputStream.Write(tempFile, 0, Length);   

    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
