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

public partial class Associator_Largess : BasePage
{
    public string beginEndDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");

            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");


            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();

            rs = baseBO.Query(new Gift());

            foreach (Gift gift in rs)
            {
                cmbGiftID.Items.Add(new ListItem(gift.GiftDesc, gift.GiftID.ToString()));
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        GiftActivity giftActivity = new GiftActivity();

        giftActivity.ActID = BaseApp.GetActID();
        giftActivity.ActDesc = txtActDesc.Text.Trim();
        giftActivity.ShopStartDate = Convert.ToDateTime(txtStartDate.Text);
        giftActivity.ShopEndDate= Convert.ToDateTime(txtEndDate.Text);
        giftActivity.GiftID = Convert.ToInt32(cmbGiftID.SelectedValue);

        if (rdoEveryTime.Checked)
        {
            giftActivity.GiftOption = GiftActivity.GIFTACTIVITY_ONCE;
        }
        else
        {
            giftActivity.GiftOption = GiftActivity.GIFTACTIVITY_DAY;   
        }

        if (baseBO.Insert(giftActivity) == -1)
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
        txtActDesc.Text = "";
        txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        cmbGiftID.SelectedIndex = 0;
        rdoEveryTime.Checked = true;
    }
}
