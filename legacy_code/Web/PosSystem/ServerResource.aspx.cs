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
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Dept;
using RentableArea;
using BaseInfo.Store;
using BaseInfo.User;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class PosSystem_ServerResource : BasePage
{
    //默认密钥向量
    private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    /**/
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.ShowTree();
            BindDrop();
            ViewState["node"] = "";
        }
        btnSave.Attributes.Add("onclick","return check()");
    }
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO baseBo = new BaseBO();
        string strSql = @"SELECT 
		CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,OrderID
FROM 
		Dept where DeptType<7 and DeptType<>2
 Group  By PDeptID,CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,OrderID
 ORDER BY Pdeptid,isnull(orderid,0) ";
        Dept objDept = new Dept();
        objDept.SetQuerySql(strSql);
        Resultset rs = baseBo.Query(objDept);
        if (rs.Count > 0)
        {
            foreach (Dept dept in rs)
            {
                jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "" + "^";
                baseBo.WhereClause = "DeptID='" + dept.DeptID.ToString() + "'";
                Resultset rsServerRes = baseBo.Query(new ServerResource());
                if (rsServerRes.Count > 0)
                {
                    foreach (ServerResource serverRes in rsServerRes)
                    {
                        if (serverRes.STATUS.ToString() == "1")
                        {
                            jsdept += dept.DeptID.ToString() + serverRes.ResourceID.ToString() + "|" + dept.DeptID + "|" + serverRes.ResourceName + "|" + "" + "^";
                        }
                        else
                        {
                            jsdept += dept.DeptID.ToString() + serverRes.ResourceID.ToString() + "|" + dept.DeptID + "|" + serverRes.ResourceName + "|" + "../App_Themes/nlstree/img/node3.gif" + "^";
                        }
                    }
                }
            }
            depttxt.Value = jsdept;
        }
    }

    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["node"] = deptid.Value;
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        if (ViewState["node"].ToString().Length == 6)
        {
            print();
            btnSave.Enabled = true;
            txtResourceCode.Enabled = false;
        }
        else
        {
            clear();
            ds = baseBo.QueryDataSet("select * from store where storeid=" + Convert.ToInt32(ViewState["node"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    private void BindDrop()
    {
        this.cmbStatus.Items.Clear();
        int[] getStatus = ServerResource.GetStatus();
        for (int i = 0; i < getStatus.Length; i++)
        {
            cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", ServerResource.GetStatusDec(getStatus[i])), getStatus[i].ToString()));
        }
    }

    private void print()
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        baseBo.WhereClause = " ResourceID=" + ViewState["node"].ToString().Substring(3,3);
        ds = baseBo.QueryDataSet(new ServerResource());
        if (ds.Tables.Count > 0)
        {
            txtResourceCode.Text = ds.Tables[0].Rows[0]["ResourceCode"].ToString();
            txtResourceName.Text = ds.Tables[0].Rows[0]["ResourceName"].ToString();
            txtIP.Text = ds.Tables[0].Rows[0]["IP"].ToString();
            txtPort.Text = ds.Tables[0].Rows[0]["PORT"].ToString();
            txtDBName.Text = ds.Tables[0].Rows[0]["DBName"].ToString();
            txtLoginNm.Text = ds.Tables[0].Rows[0]["LoginNm"].ToString();
            txtPwd.Text = DecryptDES(ds.Tables[0].Rows[0]["PWD"].ToString().Trim(), "12345678");
            txtNote.Text = ds.Tables[0].Rows[0]["Node"].ToString();
            cmbStatus.SelectedValue = ds.Tables[0].Rows[0]["STATUS"].ToString();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        ServerResource serverResource = new ServerResource();
        DataSet ds=new DataSet();        
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (ViewState["node"].ToString().Length==3)
        {            
            ds = baseBo.QueryDataSet("select max(resourceid) from serverresource");
            if (ds.Tables[0].Rows[0][0].ToString() == "")
            {
                serverResource.ResourceID = 101;
            }
            else
            {
                serverResource.ResourceID = Convert.ToInt32(ds.Tables[0].Rows[0][0]) + 1;
            }
            ds = baseBo.QueryDataSet("select * from serverresource where resourcecode='" + txtResourceCode.Text.Trim() + "'");
            if (ds.Tables[0].Rows.Count == 0)
            {
                serverResource.ResourceCode = txtResourceCode.Text;
                serverResource.DeptID = Convert.ToInt32(ViewState["node"].ToString());
                serverResource.ResourceName = txtResourceName.Text;
                serverResource.IP = txtIP.Text;
                serverResource.PORT = Convert.ToInt32(txtPort.Text);
                serverResource.DBName = txtDBName.Text;
                serverResource.LoginNm = txtLoginNm.Text;
                serverResource.Pwd = EncryptDES(this.txtPwd.Text.Trim(), "12345678");
                serverResource.STATUS = cmbStatus.SelectedValue.ToString();
                serverResource.Node = txtNote.Text;
                serverResource.CreateUserID = sessionUser.UserID;
                if (baseBo.Insert(serverResource) == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                    txtResourceCode.Enabled = true;
                    btnSave.Enabled = false;
                    clear();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Store_ServerResourceCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            }
        }
        else
        {
            serverResource.ResourceCode = txtResourceCode.Text;
            serverResource.ResourceName = txtResourceName.Text;
            serverResource.DeptID = Convert.ToInt32(ViewState["node"].ToString().Substring(0, 3));
            serverResource.IP = txtIP.Text;
            serverResource.PORT = Convert.ToInt32(txtPort.Text);
            serverResource.DBName = txtDBName.Text;
            serverResource.LoginNm = txtLoginNm.Text;
            serverResource.Pwd = EncryptDES(this.txtPwd.Text.Trim(), "12345678");
            serverResource.STATUS = cmbStatus.SelectedValue.ToString();
            serverResource.Node = txtNote.Text;
            serverResource.ModifyUserID = sessionUser.UserID;
            baseBo.WhereClause = " ResourceID=" + ViewState["node"].ToString().Substring(3, 3);
            if (baseBo.Update(serverResource) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                txtResourceCode.Enabled = true;
                btnSave.Enabled = false;
                clear();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdateLost") + "'", true);
            }
        }
        ShowTree();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clear();
        btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    private void clear()
    {
        txtResourceCode.Text = "";
        txtResourceName.Text = "";
        txtIP.Text = "";
        txtPort.Text = "";
        txtDBName.Text = "";
        txtLoginNm.Text = "";
        txtPwd.Text = "";
        txtNote.Text = "";
        cmbStatus.SelectedValue = "0";
        txtResourceCode.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }

    /**/
    /**/
    /// <summary>
    /// DES加密字符串
    /// </summary>
    /// <param name="encryptString">待加密的字符串</param>
    /// <param name="encryptKey">加密密钥,要求为8位</param>
    /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
    public static string EncryptDES(string encryptString, string encryptKey)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        catch
        {
            return encryptString;
        }
    }

    /**/
    /**/
    /**/
    /// <summary>
    /// DES解密字符串
    /// </summary>
    /// <param name="decryptString">待解密的字符串</param>
    /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
    /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
    public static string DecryptDES(string decryptString, string decryptKey)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        catch
        {
            return decryptString;
        }
    }
}
