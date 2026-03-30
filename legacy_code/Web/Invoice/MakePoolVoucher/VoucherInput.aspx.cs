using System;
using System.Web.UI.WebControls;
using System.IO;

using Invoice.MakePoolVoucher;
using Base.Biz;
using Base.DB;
using Base.Page;
using System.Text;
using Base.Util;
using Lease.Subs;

public partial class Invoice_MakePoolVoucher_VoucherInput : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "MakePoolVoucher_VoucherInput");
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        BindDate();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        WithFile();
    }
    private void BindDate()
    {
        BaseBO baseBO = new BaseBO();
        FinancialType ftype = new FinancialType();
        baseBO.WhereClause = "FinancialTypeStatus=1";
        Resultset rs=new Resultset();
        rs=baseBO.Query(ftype);

        if (rs.Count  >= 1)
        {
            foreach (FinancialType ftypeRs in rs)
            {
                this.cmbFinancialType.Items.Add(new ListItem(ftypeRs.FinancialTypeName,ftypeRs.FinancialTypeID.ToString() ) );
            }
        }

    }
    private void WithFile()
    {
        string vsurl = "";
        AccountPara accountPara = new AccountPara();
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

                Encoding fileEncoding = TxtFileEncoding.GetEncoding(vsurl, Encoding.GetEncoding("GB2312"));
                StreamReader sr = new StreamReader(vsurl, fileEncoding);
                baseTrans.BeginTrans();
                baseTrans.ExecuteUpdate("DELETE FROM AccountPara");
                while ((input = sr.ReadLine()) != null)
                {
                    aryTemp = input.Split('|');
                    //accountPara.AccountParaID = BaseApp.GetActID();
                    accountPara.AccountParaID = Convert.ToInt32(aryTemp[0]);
                    accountPara.ParaType = Convert.ToInt32(aryTemp[1]);
                    accountPara.PAccountParaID = Convert.ToInt32(aryTemp[2]);
                    accountPara.AccountGp = aryTemp[3].ToString();
                    accountPara.AccountNumber = aryTemp[4].ToString();
                    accountPara.AccountName = aryTemp[5].ToString();
                    accountPara.AccountDesc = aryTemp[6].ToString();
                    accountPara.ExNumber = aryTemp[7].ToString();
                    accountPara.IsDept = aryTemp[8].ToString();
                    accountPara.IsCustomer = aryTemp[9].ToString();
                    //accountPara.SQL = aryTemp[10].ToString().Replace("'", "''");
                    accountPara.SQL = aryTemp[10].ToString();
                    try { accountPara.FinancialTypeId = Convert.ToInt32(this.cmbFinancialType.SelectedValue); }
                    catch { accountPara.FinancialTypeId = 0; }

                    if (baseTrans.Insert(accountPara) < 1)
                    {
                        Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value=" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost") + ";</script>");
                        baseTrans.Rollback();
                    }
                }

                sr.Close();
                baseTrans.Commit();
                Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value='" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitSucceed") + "';</script>");


            }
            catch (Exception ex)
            {
                baseTrans.Rollback();
                Logger.Log("凭证导入错误:", ex);
            }
        }
    }
}
