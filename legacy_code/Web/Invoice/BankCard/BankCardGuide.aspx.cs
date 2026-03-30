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

using System.IO;
using Invoice.BankCard;
using Base.DB;
using Base.Biz;
using Base;
using Base.Page;
public partial class Invoice_BankCard_BankCardGuide : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "BankCard_Transmit");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        WithFile();
    }

    private void WithFile()
    {
        string vsurl = "";
        BankTransDet bankTransDet = new BankTransDet();
        BaseTrans baseTrans = new BaseTrans();

        string[] aryTemp;
        String input = "";

        if (FileUpload1.HasFile)//判断文件是否为空
        {
            try
            {
                //string vsfullname = FileUpload1.PostedFile.FileName;//获取文件的名称包含路径

                string vsfilename = FileUpload1.FileName;//获取文件的名称

                int index = vsfilename.LastIndexOf(".");
                string vstype = vsfilename.Substring(index).ToLower();//取文件的扩展名

                string vsnewname = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");//声称文件名，防止重复
                vsnewname = vsnewname + vstype;//完整的上传文件名
                string fullpath = Server.MapPath("upfileReceipt/");//文件的上传路径

                if (!Directory.Exists(fullpath))//判断上传文件夹是否存在，若不存在，则创建
                {
                    //创建文件夹

                    Directory.CreateDirectory(fullpath);//创建文件夹 
                }
                vsurl = Server.MapPath("upfileReceipt/") + vsnewname;
                FileUpload1.SaveAs(vsurl);

                /*存入数据库*/
                StreamReader sr = File.OpenText(vsurl);
                baseTrans.BeginTrans();
                while ((input = sr.ReadLine()) != null)
                {
                    aryTemp = input.Split(',');
                    bankTransDet.BankTransID = BaseApp.GetBankTransID();
                    bankTransDet.BankEFTID = aryTemp[0].ToString();
                    bankTransDet.BankCardID = aryTemp[1].ToString();
                    bankTransDet.BankTransTime = Convert.ToDateTime(aryTemp[2] + " " + aryTemp[3]);
                    bankTransDet.BankAmt = Convert.ToDecimal(aryTemp[4]);
                    bankTransDet.BankChgAmt = Convert.ToDecimal(aryTemp[5]);
                    bankTransDet.BankNetAmt = Convert.ToDecimal(aryTemp[6]);
                    bankTransDet.ReconcType = BankTransDet.BANKTRANSDET_RECONCTYPE_NOT_ANTITHESES;
                    bankTransDet.DataSource = BankTransDet.BANKTRANSDET_DATASOURCE_SYSTEM;
                    bankTransDet.ReconcID = 0;
                    bankTransDet.PayInID = 0;
                    if (baseTrans.Insert(bankTransDet) < 1)
                    {
                        Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value=" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost") + ";</script>");
                        baseTrans.Rollback();
                    }
                }

                sr.Close();
                baseTrans.Commit();
                Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value='" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitSucceed") + "';</script>");


            }
            catch (Exception error)
            {
                Response.Write(error.ToString());
            }
        }
    }
}
