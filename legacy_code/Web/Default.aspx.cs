using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using Base.Sys;
using System.Globalization;
using System.Text;
public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        PassWord pwd = new PassWord();
        pwd.EncryptDecrypStr = "我们的歌";
        pwd.DesEncrypt();
        TextBox1.Text = pwd.MyDesStr;

        //byte[] MyStr_E = Encoding.UTF8.GetBytes("我们的歌");
        //for (int i = 0; i < MyStr_E.Length; i++)
        //{
        //    TextBox1.Text = TextBox1.Text + MyStr_E[i].ToString();
        //}


    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
        //PassWord pwd = new PassWord();
        //pwd.DecryptStr = TextBox1.Text;
        //pwd.MyDesKey = TextBox2.Text;
        //pwd.DesDecrypt();
        //TextBox2.Text = pwd.MyDesStr;
    }
}
