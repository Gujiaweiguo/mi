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

using Base;
using Base.DB;
using Base.Biz;
using Lease.SMSPara;
using Base.Page;
using BaseInfo.User;
using Lease.ConShop;
using System.IO;

public partial class Lease_SMSPara_SMSPara : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SMSPara sMSPara = new SMSPara();
            Resultset rs = new Resultset();
            BaseBO baseBO = new BaseBO();

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_SMSPara");

            rs = baseBO.Query(new SMSPara());

            if (rs.Count == 1)
            {
                sMSPara = rs.Dequeue() as SMSPara;

                ViewState["SMSParaID"] = sMSPara.SMSparaID;

                if (sMSPara.AutoContractCode == SMSPara.AUTO_YES)
                {
                    chkContract.Checked = true;
                    txtContractCode.Text = sMSPara.NextContractCode;
                }
                else
                {
                    txtContractCode.ReadOnly  = true;
                }

                if (sMSPara.AutoCustCode == SMSPara.AUTO_YES)
                {
                    chkCustCode.Checked = true;
                    txtCustCode.Text = sMSPara.NextCustCode;
                }
                else
                {
                    txtCustCode.ReadOnly = true;
                }

                if (sMSPara.AutoSkuID == SMSPara.AUTO_YES)
                {
                    chkSkuCode.Checked = true;
                    txtSkuCode.Text = sMSPara.NextSkuID;
                }
                else
                {
                    txtSkuCode.ReadOnly = true;
                }

                if (sMSPara.AutoTPUserID == SMSPara.AUTO_YES)
                {
                    chkUserCode.Checked = true;
                    txtUserCode.Text = sMSPara.NextTPUserID;
                }
                else
                {
                    txtUserCode.ReadOnly = true;
                }

                if (sMSPara.AutoShopCode == SMSPara.AUTO_YES)
                {
                    chkShopCode.Checked = true;
                }

                ViewState["Type"] = "0";
                txtMailSMTP.Text = sMSPara.MailSMTP.ToString();
                txtMailSMTPUser.Text = sMSPara.MailSMTPUser.ToString();
                txtMailSMTPPassword.Text = sMSPara.MailSMTPPassword.ToString();
            }
            else if (rs.Count < 1)
            {
                ViewState["Type"] = "1";
                txtContractCode.ReadOnly = true;
                txtCustCode.ReadOnly = true;
                txtSkuCode.ReadOnly = true;
                txtUserCode.ReadOnly = true;
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        SMSPara sMSPara = new SMSPara();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        if (chkContract.Checked)
        {
            sMSPara.AutoContractCode = SMSPara.AUTO_YES;
        }
        else
        {
            sMSPara.AutoContractCode = SMSPara.AUTO_NO;
        }

        if (chkCustCode.Checked)
        {
            sMSPara.AutoCustCode = SMSPara.AUTO_YES;
        }
        else
        {
            sMSPara.AutoCustCode = SMSPara.AUTO_NO;
        }

        if (chkSkuCode.Checked)
        {
            sMSPara.AutoSkuID = SMSPara.AUTO_YES;
        }
        else
        {
             sMSPara.AutoSkuID = SMSPara.AUTO_NO;
        }

        if (chkUserCode.Checked)
        {
            sMSPara.AutoTPUserID = SMSPara.AUTO_YES;
        }
        else
        {
             sMSPara.AutoTPUserID = SMSPara.AUTO_NO;
        }

        if (chkShopCode.Checked)
        {
            sMSPara.AutoShopCode = SMSPara.AUTO_YES;
        }
        else
        {
            sMSPara.AutoShopCode = SMSPara.AUTO_NO;
        }


        sMSPara.NextContractCode = txtContractCode.Text;
        sMSPara.NextCustCode = txtCustCode.Text;
        sMSPara.NextSkuID = txtSkuCode.Text;
        sMSPara.NextTPUserID = txtUserCode.Text;
        sMSPara.MI_IN = txtIn.Text.Trim();
        sMSPara.MI_OUT = txtOut.Text.Trim();
        sMSPara.MailSMTP = txtMailSMTP.Text.Trim();
        sMSPara.MailSMTPUser = txtMailSMTPUser.Text.Trim();
        sMSPara.MailSMTPPassword = txtMailSMTPPassword.Text.Trim();

        if (ViewState["Type"].Equals("1"))
        {
            sMSPara.SMSparaID = BaseApp.GetSMSParaID();
            sMSPara.OprDeptID = sessionUser.DeptID;
            sMSPara.OprRoleID = sessionUser.RoleID;
            sMSPara.CreateUserID = sessionUser.UserID;

            if (baseBO.Insert(sMSPara) == -1)
            {
                return;
            }
            ClearPage();
        }
        else if (ViewState["Type"].Equals("0"))
        {
            sMSPara.ModifyUserID = sessionUser.UserID;
            baseBO.WhereClause = "SMSparaID = " + ViewState["SMSParaID"];

            if (baseBO.Update(sMSPara) != 1)
            {
                return;
            }
            ClearPage();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }

    }
    protected void chkContract_CheckedChanged(object sender, EventArgs e)
    {
        if (chkContract.Checked)
        {
            txtContractCode.ReadOnly = false;
        }
        else
        {
            txtContractCode.ReadOnly = true;
            txtContractCode.Text = "";
        }
    }
    protected void chkCustCode_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCustCode.Checked)
        {
            txtCustCode.ReadOnly = false;
        }
        else
        {
            txtCustCode.ReadOnly = true;
            txtCustCode.Text = "";
        }
    }
    protected void chkSkuCode_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSkuCode.Checked)
        {
            txtSkuCode.ReadOnly = false;
        }
        else
        {
            txtSkuCode.ReadOnly = true;
            txtSkuCode.Text = "";
        }
    }
    protected void chkUserCode_CheckedChanged(object sender, EventArgs e)
    {
        if (chkUserCode.Checked)
        {
            txtUserCode.ReadOnly = false;
        }
        else
        {
            txtUserCode.ReadOnly = true;
            txtUserCode.Text = "";
        }
    }

    private void ClearPage()
    {
        chkContract.Checked = false;
        chkCustCode.Checked = false;
        chkSkuCode.Checked = false;
        chkUserCode.Checked = false;
        chkShopCode.Checked = false;
        txtContractCode.Text = "";
        txtCustCode.Text = "";
        txtSkuCode.Text = "";
        txtUserCode.Text = "";
        txtIn.Text = "";
        txtOut.Text = "";
    }
    protected void btnIn_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        ConShop conShop = new ConShop();

        baseBO.WhereClause = "ShopStatus=" + ConShop.CONSHOP_TYPE_INGEAR;

        rs = baseBO.Query(conShop);

        foreach (ConShop shop in rs)
        {

            string[] xmlFiles = Directory.GetDirectories(txtIn.Text.Replace(@"\", @"\\"));

            if (xmlFiles.Length == 0)
            {
                Directory.CreateDirectory(txtIn.Text.Replace(@"\", @"\\") + @"\\" + shop.ShopCode.Trim());
            }
            else
            {
                for (int i = 0; i < xmlFiles.Length; i++)
                {
                    if (!xmlFiles[i].Contains(txtIn.Text.Replace(@"\", @"\\") + @"\" + shop.ShopCode))
                    {
                        Directory.CreateDirectory(txtIn.Text.Replace(@"\", @"\\") + @"\\" + shop.ShopCode.Trim());
                    }
                }
            }
        }
    }
}
