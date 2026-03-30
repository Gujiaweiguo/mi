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
using Base.Page;
using BaseInfo.User;

public partial class Associator_Perform_IssueNewCard : System.Web.UI.Page
{
    public string url;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (Request.QueryString["MembId"] != null)
            {
                BindMember();

                BindCardType();
                BindCardCardClass();
                txtDate.Text = DateTime.Now.ToShortDateString().ToString();
                txtTermDate.Text = DateTime.Now.AddYears(2).ToShortDateString().ToString();
                lockControl(true);
                btnSubmit.Attributes.Add("onclick", "return InputValidator(form1)");
                url = "Associator/Perform/IssueNewCard.aspx?MembId=" + Request.QueryString["MembId"].ToString() + "&modify=" + 1;
            }
        }
    }
    private void BindMember()
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "MembID = '" + Request.QueryString["MembID"].ToString() + "'";
        Resultset rs = baseBO.Query(new LCust());
        if (rs.Count > 0)
        {
            LCust lCust = rs.Dequeue() as LCust;
            txtMembCode.Text = lCust.MembCode;
            txtMembName.Text = lCust.MemberName;
            txtPassPort.Text = lCust.LOtherId;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Member_NoMemberID") + "'", true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        bool isCardId = ISCardID(txtCardID.Text);
        if (isCardId == false)
        {
            MembCard membCard = new MembCard();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            membCard.MembCardId = txtCardID.Text.Trim();
            membCard.MembId = Convert.ToInt32(Request.QueryString["MembID"]);
            membCard.CardClassId = Convert.ToInt32(dropCardLevel.SelectedValue);
            membCard.CardTypeId = dropCardType.SelectedValue.ToString();
            membCard.DateIssued = Convert.ToDateTime(txtDate.Text);
            membCard.ExpDate = Convert.ToDateTime(txtTermDate.Text);
            membCard.CreateUserID = sessionUser.UserID;
            if (radCustomer.Checked)
            {
                membCard.CardOwner = LCard.OPTN_ORMA_LCUST.ToString();
            }
            else if (radEmployee.Checked)
            {
                membCard.CardOwner = LCard.OPT_EMPLOYEE.ToString();
            }
            else
            {
                membCard.CardOwner = LCard.OPT_OTHERS.ToString();
            } 
            membCard.CardStatusId = "N";
            membCard.NewMembCardID = "";

            int i = baseBO.Insert(membCard);
            if (i == 1)
            {
                Response.Redirect("../Perform/NewCard.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Member_YesCardID") + "'", true);
            return;
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearContral();
    }

    private void BindCardType()
    {
        //dropCardType.Items.Add(new ListItem("A类卡", "D"));
        //dropCardType.Items.Add(new ListItem("B类卡", "P"));
        dropCardType.Items.Add(new ListItem("会员卡", "P"));
    }

    /// <summary>
    /// 绑定卡级别


    /// </summary>
    private void BindCardCardClass()
    {
        dropCardLevel.Items.Clear();
        BaseBO baseBO = new BaseBO();
        Resultset rs = baseBO.Query(new CardClass());
        foreach (CardClass crdCls in rs)
        {
            dropCardLevel.Items.Add(new ListItem(crdCls.CardClassNm, crdCls.CardClassID.ToString()));
        }
    }

    private void clearContral()
    {
        this.txtCardID.Text = "";
        txtDate.Text = DateTime.Now.ToShortDateString().ToString();
        txtPassPort.Text = "";
        txtTermDate.Text = DateTime.Now.AddYears(1).ToShortDateString().ToString();
        BindCardCardClass();
        BindCardType();
    }

    private void lockControl(bool m)
    {
        txtMembName.ReadOnly = m;
        txtPassPort.ReadOnly = m;
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

}
