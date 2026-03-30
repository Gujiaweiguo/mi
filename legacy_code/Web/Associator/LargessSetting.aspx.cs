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

using Associator.Perform;
using Base.Biz;
using Base.Page;
using Base.Util;
using Base.DB;
using Base;

public partial class Associator_LargessSetting : BasePage
{
    public string beginEndDate;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtAnStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            chkExByBonus.Attributes.Add("onclick", "CheckedExByBonus()");
            chkExByReceipt.Attributes.Add("onclick", "CheckedExByReceipt()");

            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");

            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Gift gift = new Gift();

        gift.GiftID = BaseApp.GetGiftID();
        gift.GiftDesc = txtGiftDesc.Text.Trim();
        gift.ShopStartDate = Convert.ToDateTime(txtAnStartDate.Text); 
        gift.ShopEndDate = Convert.ToDateTime(txtEndDate.Text);

        if (chkExByBonus.Checked)
        {
            gift.ExByBonus = Gift.EXBYBONUS_YES;
            gift.BonusCost = Convert.ToInt32(txtExByBonus.Text);
        }
        else
        {
            gift.ExByBonus = Gift.EXBYBONUS_NO;
        }

        if (chkExByReceipt.Checked)
        {
            gift.ExByReceipt = Gift.EXBYRECEIPT_YES;
            gift.ReceiptMoney = Convert.ToDecimal(txtExByReceipt.Text);
            if (rdoYes.Checked)
            {
                gift.LimitOne = Gift.LIMITONE_YES;
            }
            else
            {
                gift.LimitOne = Gift.LIMITONE_NO;
            }
        }
        else
        {
            gift.ExByReceipt = Gift.EXBYRECEIPT_NO;
        }

        if (chkFreeGift.Checked)
        {
            gift.FreeGift = Gift.FREEGIFT_YES;
        }
        else
        {
            gift.FreeGift = Gift.FREEGIFT_NO;
        }

        if (baseBO.Insert(gift) == -1)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
            return;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ClearText();
        }
    }

    private void ClearText()
    {
        txtExByReceipt.Text="";
        txtGiftDesc.Text="";
        txtAnStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        chkExByBonus.Checked = false;
        txtExByBonus.Enabled = false;
        txtExByBonus.Text = "";
        chkExByReceipt.Checked = false;
        txtExByReceipt.Enabled = false;
        txtExByReceipt.Text = "";
        rdoYes.Enabled = false;
        rdoNo.Enabled = false;
        chkFreeGift.Checked = false;
    }

}
