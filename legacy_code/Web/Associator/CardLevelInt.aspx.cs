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
using BaseInfo.User;

public partial class Associator_CardLevelInt : BasePage
{
    public string url;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadBand();
            BindDate();
            if (Request.QueryString["CardClassID"] != null)
            {
                BaseBO baseBO = new BaseBO();
                baseBO.WhereClause = "CardClassID = '" + Request.QueryString["CardClassID"].ToString() + "'";
                Resultset rs = baseBO.Query(new CardClass());
                SetContorlsValues(rs);
                ViewState["flag"] = 1; //修改
                url = "Associator/CardLevelInt.aspx?CardClassID=" + Request.QueryString["CardClassID"].ToString();
            }
            else
            {
                ViewState["flag"] = 0; //添加
                url = "Associator/CardLevelInt.aspx";
            }
        }
    }

    protected void rdoMoney_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void chkCardInvalidation_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCardInvalidation.Checked)
        {
            txtInvVal.Enabled = true;
            txtInvMth.Enabled = true;
            txtInvWarnVal.Enabled = true;
            txtInvWarnMth.Enabled = true;
        }
        else
        {
            txtInvVal.Enabled = false;
            txtInvMth.Enabled = false;
            txtInvWarnVal.Enabled = false;
            txtInvWarnMth.Enabled = false;

            txtInvVal.Text = "";
            txtInvMth.Text = "";
            txtInvWarnVal.Text = "";
            txtInvWarnMth.Text = "";

        }
    }
    protected void chkCardDegrade_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCardDegrade.Checked)
        {
            txtDnVal.Enabled = true;
            txtDnMth.Enabled = true;
            cmdDnId.Enabled = true;
            txtDnWarnVal.Enabled = true;
            txtDnWarnMth.Enabled = true;
        }
        else
        {
            txtDnVal.Enabled = false;
            txtDnMth.Enabled = false;
            cmdDnId.Enabled = false;
            txtDnWarnVal.Enabled = false;
            txtDnWarnMth.Enabled = false;

            txtDnVal.Text = "";
            txtDnMth.Text = "";
            txtDnWarnVal.Text = "";
            txtDnWarnMth.Text = "";
        }
    }
    protected void chkCardUpgrade_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCardUpgrade.Checked)
        {
            txtUpVal.Enabled = true;
            txtUpMth.Enabled = true;
            cmdUpId.Enabled = true;
        }
        else
        {
            txtUpVal.Enabled = false;
            txtUpMth.Enabled = false;
            cmdUpId.Enabled = false;

            txtUpVal.Text = "";
            txtUpMth.Text = "";

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        CardClass cardClass = new CardClass();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        if (ViewState["flag"].ToString() == "0")
        {
            
            cardClass.CreateTime = DateTime.Now;
            cardClass.CreateUserID = objSessionUser.UserID;
            cardClass.OprDeptID = objSessionUser.DeptID;
            cardClass.OprRoleID = objSessionUser.RoleID;

            cardClass = GetControlsVales(cardClass);

            if (baseBO.Insert(cardClass) == 1)
            {
                Response.Redirect("../Associator/CardLevelView.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else if (ViewState["flag"].ToString() == "1")
        {
            baseBO.WhereClause = "CardClassID = '" + txtCardClassId.Text.Trim() + "'";
            cardClass.ModifyTime = DateTime.Now;
            cardClass.ModifyUserID = objSessionUser.UserID;
            cardClass.OprDeptID = objSessionUser.DeptID;
            cardClass.OprRoleID = objSessionUser.RoleID;
            cardClass = GetControlsVales(cardClass);
            if (baseBO.Update(cardClass) == 1)
            {
                Response.Redirect("../Associator/CardLevelView.aspx");
                //ClearPage();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
    }

    private CardClass GetControlsVales(CardClass cardClass)
    {
        cardClass.CardClassID = Convert.ToInt32(txtCardClassId.Text);
        cardClass.CardClassNm = txtCardClassNm.Text.Trim();
        cardClass.BonusPer = Convert.ToDecimal(txtBonusPer.Text.Trim() == "" ? "0" : txtBonusPer.Text.Trim())/100;
        cardClass.NewCharge = Convert.ToDecimal(txtNewCharge.Text.Trim() == "" ? "0" : txtNewCharge.Text.Trim());
        cardClass.LostCharge = Convert.ToDecimal(txtLostCharge.Text.Trim() == "" ? "0" : txtLostCharge.Text.Trim());
        cardClass.DemageCharge = Convert.ToDecimal(txtDemageCharge.Text.Trim() == "" ? "0" : txtDemageCharge.Text.Trim());

        if (chkCardInvalidation.Checked)
        {
            cardClass.Invalidate = CardClass.STATUS_YES;

            cardClass.InvVal = Convert.ToDecimal(txtInvVal.Text.Trim() == "" ? "0" : txtInvVal.Text.Trim());
            cardClass.InvMth = Convert.ToInt32(txtInvMth.Text.Trim() == "" ? "0" : txtInvMth.Text.Trim());
            cardClass.InvWarnVal = Convert.ToDecimal(txtInvWarnVal.Text.Trim() == "" ? "0" : txtInvWarnVal.Text.Trim());
            cardClass.InvWarnMth = Convert.ToInt32(txtInvWarnMth.Text.Trim() == "" ? "0" : txtInvWarnMth.Text.Trim());
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

            cardClass.DnVal = Convert.ToDecimal(txtDnVal.Text.Trim() == "" ? "0" : txtDnVal.Text.Trim());
            cardClass.DnMth = Convert.ToInt32(txtDnMth.Text.Trim() == "" ? "0" : txtDnMth.Text.Trim());
            cardClass.DnId = cmdDnId.SelectedValue.ToString();
            cardClass.DnWarnVal = Convert.ToDecimal(txtDnWarnVal.Text.Trim() == "" ? "0" : txtDnWarnVal.Text.Trim());
            cardClass.DnWarnMth = Convert.ToInt32(txtDnWarnMth.Text.Trim() == "" ? "0" : txtDnWarnMth.Text.Trim());
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

            cardClass.UpVal = Convert.ToDecimal(txtUpVal.Text.Trim() == "" ? "0" : txtUpVal.Text.Trim());
            cardClass.UpMth = Convert.ToInt32(txtUpMth.Text.Trim() == "" ? "0" : txtUpMth.Text.Trim());
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
        return cardClass;
    }

    private void ClearPage()
    {
        txtCardClassId.Text = "";
        txtCardClassNm.Text = "";
        txtBonusPer.Text = "";
        txtDemageCharge.Text = "";
        txtDnMth.Text = "";
        txtDnVal.Text = "";
        txtDnWarnMth.Text = "";
        txtDnWarnVal.Text = "";
        txtInvMth.Text = "";
        txtInvVal.Text = "";
        txtInvWarnMth.Text = "";
        txtInvWarnVal.Text = "";
        txtLostCharge.Text = "";
        txtNewCharge.Text = "";
        txtUpMth.Text = "";
        txtUpVal.Text = "";
        chkCardUpgrade.Checked = false;
        chkCardDegrade.Checked = false;
        chkCardInvalidation.Checked = false;
        chkInvalidationYear.Checked = false;
        LoadBand();
    }

    private void LoadBand()
    {
        for (int i = 1; i <= 10; i++)
        {
            cmbExpireYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }
    protected void chkInvalidationYear_CheckedChanged(object sender, EventArgs e)
    {
        if (chkInvalidationYear.Checked)
        {
            cmbExpireYear.Enabled = true;
            cmbExpireYear.SelectedIndex = 0;
        }
        else
        {
            cmbExpireYear.Enabled = false;
            cmbExpireYear.SelectedIndex = 0;
        }
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        ClearPage();
    }

    private void SetContorlsValues(Resultset rs)
    {
        if (rs.Count == 1)
        {
            CardClass cardClass = rs.Dequeue() as CardClass;
            txtCardClassId.Text = cardClass.CardClassID.ToString();
            txtCardClassNm.Text = cardClass.CardClassNm;
            txtBonusPer.Text = Convert.ToString(cardClass.BonusPer*100);
            txtDemageCharge.Text = cardClass.DemageCharge.ToString();
            txtDnMth.Text = cardClass.DnMth.ToString();
            txtDnVal.Text = cardClass.DnVal.ToString();
            txtDnWarnMth.Text = cardClass.DnWarnMth.ToString();
            txtDnWarnVal.Text = cardClass.DnWarnVal.ToString();
            txtInvMth.Text = cardClass.InvMth.ToString();
            txtInvVal.Text = cardClass.InvVal.ToString();
            txtInvWarnMth.Text = cardClass.InvWarnMth.ToString();
            txtInvWarnVal.Text = cardClass.InvWarnVal.ToString();
            txtLostCharge.Text = cardClass.LostCharge.ToString();
            txtNewCharge.Text = cardClass.NewCharge.ToString();
            txtUpMth.Text = cardClass.UpMth.ToString();
            txtUpVal.Text = cardClass.UpVal.ToString();
            if (cardClass.BasedOn == CardClass.STATUS_BONUS_B)
            {
                rdoConsumption.Checked = true;
                rdoMoney.Checked = false;
            }
            else
            {
                rdoConsumption.Checked = false;
                rdoMoney.Checked = true;
            }

            if (cardClass.Invalidate == CardClass.STATUS_YES)
            {
                chkCardInvalidation.Checked = true;
            }
            else
            {
                chkCardInvalidation.Checked = false;
            }

            if (cardClass.DownGrade == CardClass.STATUS_YES)
            {
                chkCardDegrade.Checked = true;
            }
            else
            {
                chkCardDegrade.Checked = false;
            }

            if (cardClass.Upgrade == CardClass.STATUS_YES)
            {
                chkCardUpgrade.Checked = true;
            }
            else
            {
                chkCardUpgrade.Checked = false;
            }

            txtUpVal.Enabled = true;
            txtUpMth.Enabled = true;
            cmdUpId.Enabled = true;

            txtDnVal.Enabled = true;
            txtDnMth.Enabled = true;
            cmdDnId.Enabled = true;
            txtDnWarnVal.Enabled = true;
            txtDnWarnMth.Enabled = true;

            txtInvVal.Enabled = true;
            txtInvMth.Enabled = true;
            txtInvWarnVal.Enabled = true;
            txtInvWarnMth.Enabled = true;

        }
    }

    private void BindDate()
    {
        cmdUpId.Items.Clear();
        cmdDnId.Items.Clear();
        cmdUpId.Items.Add(new ListItem(""));
        cmdDnId.Items.Add(new ListItem(""));
        BaseBO baseBO = new BaseBO();
        Resultset rs = baseBO.Query(new CardClass());
        foreach (CardClass crdCls in rs)
        {
            cmdUpId.Items.Add(new ListItem(crdCls.CardClassNm, crdCls.CardClassID.ToString()));
            cmdDnId.Items.Add(new ListItem(crdCls.CardClassNm, crdCls.CardClassID.ToString()));
        }
    }
}
