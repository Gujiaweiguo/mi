using System;
using System.Data;

using System.IO;
using Base.DB;
using Base.Biz;
using Base.Page;
using System.Text;
using Associator.Perform;
using BaseInfo.User;

public partial class Associator_CardInput : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label3.Text = "";
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_CardInformationInput");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        WithFile();
    }

    private void WithFile()
    {
        DataTable tempDT = new DataTable();
        tempDT.Columns.Add("ID");
        
        string vsurl = "";
        LCust lCust = new LCust();
        MembCard membCard = new MembCard();
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        BaseTrans baseTrans = new BaseTrans();

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string[] aryTemp;
        String input = "";

        rs = baseBO.Query(new CardClass());
        CardClass crdCls = new CardClass();
        crdCls = rs.Dequeue() as CardClass;
       
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
                StreamReader sr = new StreamReader(vsurl,Encoding.GetEncoding("GB2312")); 
                int i = 0;
                    
                baseTrans.BeginTrans();
                while ((input = sr.ReadLine()) != null)
                {
                    baseBO.WhereClause = "MembID = " + input.Substring(0, 16).Trim();
                    int count = tempDT.Rows.Count;
                    for (int j = 0; j < count; j++)
                    {
                        if (tempDT.Rows[j]["ID"].ToString() == input.Substring(0, 16).Trim())
                        {
                            baseTrans.Rollback();
                            sr.Close();
                            baseTrans.Commit();
                            Label3.Text = (String)GetGlobalResourceObject("BaseInfo", "Prompt_InputMembBankCardError");
                            return;
                        }
                    }
                    DataRow dr = tempDT.NewRow();
                    dr["ID"] = input.Substring(0, 16).Trim();
                    
                    tempDT.Rows.Add(dr);
                    rs = baseBO.Query(lCust);
               
                    lCust.MembId = Convert.ToInt32(Substring(input,0, 16).Trim());
                    lCust.LOtherId = Substring(input,16, 18).Trim();
                    lCust.MembCode = Substring(input,0, 16).Trim();
                    lCust.MemberName = Substring(input, 34, 20).Trim();
                    lCust.Salutation = Substring(input, 54, 6).Trim();
                    lCust.SexNm = Substring(input,60, 6).Trim();
                    lCust.Dob = Convert.ToDateTime(Substring(input,66, 10).Trim());
                    lCust.MobilPhone = Substring(input,76, 18).Trim();
                    lCust.Addr1 = Substring(input,94, 90).Trim();
                    lCust.PostalCode1 = Substring(input,184, 6).Trim();
                    lCust.Email = Substring(input, 190, 40).Trim();
                    lCust.CreateUserID = sessionUser.UserID;
                    lCust.OprDeptID = sessionUser.DeptID;
                    lCust.OprRoleID = sessionUser.RoleID;
                    lCust.ComefromNM = "联名卡";
                    lCust.MAnnDate = "1-1";

                    membCard.MembCardId = input.Substring(0, 16).Trim();
                    membCard.MembId = Convert.ToInt32(input.Substring(0, 16).Trim());
                    membCard.CreateUserID = sessionUser.UserID;
                    membCard.OprDeptID = sessionUser.DeptID;
                    membCard.OprRoleID = sessionUser.RoleID;
                    membCard.DateIssued = Convert.ToDateTime(Substring(input,230, 10).Trim());
                    membCard.ExpDate = Convert.ToDateTime(Substring(input, 240, 10).Trim());
                    membCard.CardStatusId = "N";
                    membCard.CardTypeId = "J";
                    membCard.CardOwner = "N";
                    membCard.CardClassId = crdCls.CardClassID;


                    if (rs.Count < 1)
                    {
                        lCust.DateJoint = DateTime.Now;
                        if (baseTrans.Insert(lCust) < 1)
                        {
                            Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value=" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost") + ";</script>");
                            baseTrans.Rollback();
                        }
                        if (baseTrans.Insert(membCard) < 1)
                        {
                            Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value=" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost") + ";</script>");
                            baseTrans.Rollback();
                        }
                    }
                    else
                    {
                        baseTrans.WhereClause = "MembId = " + input.Substring(0, 16).Trim();
                        membCard.ModifyTime = DateTime.Now;
                        membCard.ModifyUserID = sessionUser.UserID;
                        lCust.ModifyTime = DateTime.Now;
                        lCust.ModifyUserID = sessionUser.UserID;

                        LCust xlCust = rs.Dequeue() as LCust;
                        lCust.DateJoint = xlCust.DateJoint;

                        if (baseTrans.Update(lCust) < 1)
                        {
                            Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value=" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost") + ";</script>");
                            baseTrans.Rollback();
                        }
                        if (baseTrans.Update(membCard) < 1)
                        {
                            Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value=" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost") + ";</script>");
                            baseTrans.Rollback();
                        }
                    }
                    i++;
                }

                sr.Close();
                baseTrans.Commit();
                Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value='" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitSucceed") + "';</script>");
            }
            catch (Exception error)
            {
                baseTrans.Rollback();
                Response.Write(error.ToString());
            }
        }
    }
    protected void btnInput_Click(object sender, EventArgs e)
    {
        string vsurl = "";
        MembCard membCard = new MembCard();
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        BaseTrans baseTrans = new BaseTrans();

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string[] aryTemp;
        String input = "";

        if (FileUpload2.HasFile)//判断文件是否为空
        {
            try
            {
                //string vsfullname = FileUpload1.PostedFile.FileName;//获取文件的名称包含路径



                string vsfilename = FileUpload2.FileName;//获取文件的名称



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
                FileUpload2.SaveAs(vsurl);

                /*存入数据库*/
                StreamReader sr = File.OpenText(vsurl);
                baseTrans.BeginTrans();
                while ((input = sr.ReadLine()) != null)
                {
                    if (baseTrans.ExecuteUpdate("Update MembCard Set CardStatusid='" + Substring(input, 16, 1).Trim() + "',ModifyTime = '" + DateTime.Now + "',ModifyUserID ='" + sessionUser.UserID + "' Where MembCardId = '" + Substring(input, 0, 16).Trim() + "'") < 1)
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

    public string Substring(string strValue, int startIndex, int length)
    {
        int iStartTemp = 0;
        int iTemp = 0;
        string returnString = "";
        if (Length(strValue) > startIndex)
        {
            for (int i = 0; i < strValue.Length; i++)
            {
                int c = (int)strValue[i];
                if (c < 0)
                    c += 65536;
                if (c > 255)
                    iTemp += 2;
                else
                    iTemp += 1;
                if (iTemp > startIndex)
                {
                    iStartTemp = i;
                    break;
                }
            }
        }
        else
            return returnString;

        iTemp = 0;
        if (Length(strValue) > (startIndex + length))
        {
            for (int i = iStartTemp; i < strValue.Length; i++)
            {
                int c = (int)strValue[i];
                if (c < 0)
                    c += 65536;
                if (c > 255)
                    iTemp += 2;
                else
                    iTemp += 1;
                if (iTemp > length)
                    break;
                else
                    returnString += strValue[i].ToString();
            }
        }
        else
        {
            returnString = strValue.Substring(iStartTemp);
        }
        return returnString;
    }

    public int Length(string strLen)
    {
        int l, t, c;
        int i;
        l = strLen.Length;
        t = l;
        for (i = 0; i < l; i++)
        {
            c = (int)strLen[i];
            if (c < 0)
            {
                c = c + 65536;
            }
            if (c > 255)
            {
                t = t + 1;
            }
        }
        return t;
    }
}
