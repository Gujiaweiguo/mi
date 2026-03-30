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

using Base.Page;
using Base.Biz;
using Base.DB;
using Associator.Perform;

public partial class Associator_CardLevelInt : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadBand();
        }
    }
    protected void rdoMoney_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void chkCardInvalidation_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCardInvalidation.Checked)
        {
            txtInvVal.ReadOnly = false;
            txtInvMth.ReadOnly = false;
            txtInvWarnVal.ReadOnly = false;
            txtInvWarnMth.ReadOnly = false;
        }
        else
        {
            txtDnVal.ReadOnly = true;
            txtDnVal.Text = "";
            txtDnMth.ReadOnly = true;
            txtDnMth.Text = "";
            cmdDnId.Enabled = false;
            txtDnWarnVal.ReadOnly = true;
            txtDnWarnVal.Text = "";
            txtDnWarnMth.ReadOnly = true;
            txtDnWarnMth.Text = "";

            txtUpVal.ReadOnly = true;
            txtUpVal.Text = "";
            txtUpMth.ReadOnly = true;
            txtUpMth.Text = "";
            cmdUpId.Enabled = false;
        }
    }
    protected void chkCardDegrade_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCardDegrade.Checked)
        {
            txtDnVal.ReadOnly = false;
            txtDnMth.ReadOnly = false;
            cmdDnId.Enabled = true;
            txtDnWarnVal.ReadOnly = false;
            txtDnWarnMth.ReadOnly = false;
        }
        else
        {
            txtInvVal.ReadOnly = true;
            txtInvVal.Text = "";
            txtInvMth.ReadOnly = true;
            txtInvMth.Text = "";
            txtInvWarnVal.ReadOnly = true;
            txtInvWarnVal.Text = "";
            txtInvWarnMth.ReadOnly = true;
            txtInvWarnMth.Text = "";

            txtUpVal.ReadOnly = true;
            txtUpVal.Text = "";
            txtUpMth.ReadOnly = true;
            txtUpMth.Text = "";
            cmdUpId.Enabled = false;
        }
    }
    protected void chkCardUpgrade_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCardUpgrade.Checked)
        {
            txtUpVal.ReadOnly = false;
            txtUpMth.ReadOnly = false;
            cmdUpId.Enabled = true;
        }
        else
        {
            txtInvVal.ReadOnly = true;
            txtInvVal.Text = "";
            txtInvMth.ReadOnly = true;
            txtInvMth.Text = "";
            txtInvWarnVal.ReadOnly = true;
            txtInvWarnVal.Text = "";
            txtInvWarnMth.ReadOnly = true;
            txtInvWarnMth.Text = "";

            txtDnVal.ReadOnly = true;
            txtDnVal.Text = "";
            txtDnMth.ReadOnly = true;
            txtDnMth.Text = "";
            cmdDnId.Enabled = false;
            txtDnWarnVal.ReadOnly = true;
            txtDnWarnVal.Text = "";
            txtDnWarnMth.ReadOnly = true;
            txtDnWarnMth.Text = "";

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        CardClass cardClass = new CardClass();

        cardClass.CardClassID = Convert.ToInt32(txtCardClassId.Text);
        cardClass.CardClassNm = txtCardClassNm.Text.Trim();
        cardClass.BonusPer = Convert.ToDecimal(txtBonusPer.Text.Trim());
        cardClass.NewCharge = Convert.ToDecimal(txtNewCharge.Text.Trim());
        cardClass.LostCharge = Convert.ToDecimal(txtLostCharge.Text.Trim());
        cardClass.DemageCharge = Convert.ToDecimal(txtDemageCharge.Text.Trim());

        if (chkCardInvalidation.Checked)
        {
            cardClass.Invalidate = CardClass.STATUS_YES;

            cardClass.InvVal = Convert.ToDecimal(txtInvVal.Text.Trim());
            cardClass.InvMth = Convert.ToInt32(txtInvMth.Text.Trim());
            cardClass.InvWarnVal = Convert.ToDecimal(txtInvWarnVal.Text.Trim());
            cardClass.InvWarnMth = Convert.ToInt32(txtInvWarnMth.Text.Trim());
        }
        else
        {
            cardClass.Invalidate = CardClass.STATUS_NO;
        }

        if (chkCardDegrade.Checked)
        {
            cardClass.DownGrade = CardClass.STATUS_YES;
        }
        else
        {
            cardClass.DownGrade = CardClass.STATUS_NO;
        }

        if (chkCardUpgrade.Checked)
        {
            cardClass.Upgrade = CardClass.STATUS_YES;

            cardClass.DnVal = Convert.ToDecimal(txtDnVal.Text.Trim());
            cardClass.DnMth = Convert.ToInt32(txtDnMth.Text.Trim());
            cardClass.DnId = cmdDnId.SelectedValue.ToString();
            cardClass.DnWarnVal = Convert.ToDecimal(txtDnWarnVal.Text.Trim());
            cardClass.DnWarnMth = Convert.ToInt32(txtDnWarnMth.Text.Trim());
        }
        else
        {
            cardClass.Upgrade = CardClass.STATUS_NO;
        }

        if (rdoConsumption.Checked)
        {
            cardClass.BasedOn = CardClass.STATUS_BONUS_B;
        }
        else
        {
            cardClass.BasedOn = CardClass.STATUS_BONUS_T;
        }

        if (chkInvalidationYear.Checked)
        {
            cardClass.Expire = CardClass.STATUS_YES;

            cardClass.UpVal = Convert.ToDecimal(txtUpVal.Text.Trim());
            cardClass.UpMth = Convert.ToInt32(txtUpMth.Text.Trim());
            cardClass.UpId = cmdUpId.SelectedValue.ToString();
        }
        else
        {
            cardClass.Expire = CardClass.STATUS_NO;
        }


        if (chkInvalidationYear.Checked)
        {
            cardClass.ExpireYear = Convert.ToInt32(cmbExpireYear.SelectedValue.ToString());
        }

        if (baseBO.Insert(cardClass) == 1)
        {
            ClearPage();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
    }

    private void ClearPage()
    {
        
    }

    private void LoadBand()
    {
        for (int i = 1; i <= 10; i++)
        {
            cmbExpireYear.Items.Add(new ListItem(i.ToString(),i.ToString()));
        }
    }
    protected void chkInvalidationYear_CheckedChanged(object sender, EventArgs e)
    {
        cmbExpireYear.Enabled = true;
        cmbExpireYear.SelectedIndex = 0;
    }
}
