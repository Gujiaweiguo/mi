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
using Base;
using Associator;
using Associator.Perform;
using Associator.Associator;
using Base.Page;
using BaseInfo.User;

public partial class Associator_Perform_OldMembChargeCard : BasePage
{
    /// <summary>
    /// 用于绑定的表
    /// </summary>
    protected DataTable MembDT
    {
        set
        {
            ViewState["Sour"] = value;
        }
        get
        {
            return (DataTable)ViewState["Sour"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToShortDateString();
            txtCardID.Enabled = false;
            txtDate.Enabled = false;
            BtnSave.Enabled = false;
            BtnSave.Attributes.Add("onclick", "return InputValidator(form1)");
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (this.txtOldCard.Text.Trim() != "")
        {
            BaseBO baseBO = new BaseBO();
            string WhereClause = "MembCard.MembCardId = '" + this.txtOldCard.Text.Trim() + "'"; 
            MembDT = QueryAssociatorPO.GetLCustByCondition(WhereClause).Tables[0];
            if (MembDT.Rows.Count > 0)
            {
                BaseBO baseBo = new BaseBO();
                baseBo.WhereClause = "CardClassID = " + Convert.ToInt32(MembDT.Rows[0]["CardClassID"]);
                Resultset rs = baseBo.Query(new CardClass());
                if (rs.Count == 1)
                {
                    CardClass cardClass = rs.Dequeue() as CardClass;
                    txtCardClass.Text = cardClass.CardClassNm;
                }

                string cardStatus = "";
                if (MembDT.Rows[0]["CardStatusID"].ToString() == "N")
                {
                    cardStatus = "新卡";
                }
                else if (MembDT.Rows[0]["CardStatusID"].ToString() == "R")
                {
                    cardStatus = "更新卡";
                }
                else if (MembDT.Rows[0]["CardStatusID"].ToString() == "L")
                {
                    cardStatus = "丢失";
                }
                else if (MembDT.Rows[0]["CardStatusID"].ToString() == "D")
                {
                    cardStatus = "损坏";
                }
                else if (MembDT.Rows[0]["CardStatusID"].ToString() == "I")
                {
                    cardStatus = "无效";
                }
                else if (MembDT.Rows[0]["CardStatusID"].ToString() == "W")
                {
                    cardStatus = "降级";
                }
                else if (MembDT.Rows[0]["CardStatusID"].ToString() == "U")
                {
                    cardStatus = "升级";
                }
                else if (MembDT.Rows[0]["CardStatusID"].ToString() == "T")
                {
                    cardStatus = "退卡";
                }

                txtCardStatus.Text = cardStatus;

                if (MembDT.Rows[0]["CardTypeID"].ToString() == "P")
                {
                    txtCardType.Text = "会员卡";
                }
                else
                {
                    txtCardType.Text = "其它";
                }

                txtMembID.Text = MembDT.Rows[0]["MembId"].ToString();
                txtMembName.Text = MembDT.Rows[0]["MemberName"].ToString();
                txtPassDate.Text = Convert.ToDateTime(MembDT.Rows[0]["ExpDate"]).ToShortDateString();
                txtStartDate.Text = Convert.ToDateTime(MembDT.Rows[0]["DateIssued"]).ToShortDateString();

                txtCardID.Enabled = true;
                txtDate.Enabled = true;
                BtnSave.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请输入旧会员卡卡号！'", true);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        bool isCardId = ISCardID(txtCardID.Text);
        if (isCardId == false)
        {
            if (rBtnStatus.SelectedValue != "")
            {
                BaseTrans baseTrans = new BaseTrans();
                baseTrans.BeginTrans();
                try
                {
                    MembCard membCard = new MembCard();
                    SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                    membCard.MembCardId = txtCardID.Text.Trim();
                    membCard.MembId = Convert.ToInt32(txtMembID.Text);
                    membCard.CardClassId = Convert.ToInt32(MembDT.Rows[0]["CardClassID"]);
                    membCard.CardTypeId = MembDT.Rows[0]["CardTypeId"].ToString();
                    membCard.DateIssued = Convert.ToDateTime(txtDate.Text);
                    membCard.ExpDate = Convert.ToDateTime(MembDT.Rows[0]["ExpDate"]);
                    membCard.CreateUserID = sessionUser.UserID;
                    membCard.CardOwner = MembDT.Rows[0]["CardOwner"].ToString();
                    membCard.CardStatusId = "N";
                    membCard.NewMembCardID = "";

                    int i = baseTrans.Insert(membCard);

                    int j = MembCardPO.ModifyOldMembCardByID(txtOldCard.Text.Trim(), txtCardID.Text.Trim(), DateTime.Now, sessionUser.UserID, rBtnStatus.SelectedValue, baseTrans);
                    SetControlValue();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '发卡成功！'", true);
                }
                catch (Exception ex)
                {
                    baseTrans.Rollback();
                    throw ex;
                }
                baseTrans.Commit();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请选择卡状态！'", true);
            }
        }
    }

    /// <summary>
    /// 判断卡号是否以存在
    /// </summary>
    /// <param name="cardID">卡号</param>
    /// <returns></returns>
    private bool ISCardID(string cardID)
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "MembCardID = '" + cardID + "'";
        Resultset rs = baseBO.Query(new MembCard());
        if (rs.Count > 0)
            return true;
        else
            return false;
    }

    private void SetControlValue()
    {
        txtCardClass.Text = "";
        txtCardID.Text = "";
        txtCardStatus.Text = "";
        txtCardType.Text = "";
        txtMembID.Text = "";
        txtMembName.Text = "";
        txtOldCard.Text = "";
        txtPassDate.Text = "";
        txtStartDate.Text = "";
        txtTel.Text = "";

    }
}
