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
using Lease.ConShop;
using Associator;
using Associator.Perform;
using Base.Page;
using BaseInfo.User;
using Associator.Associator;

public partial class Associator_Perform_ModifyCard : BasePage
{
    public string lblUpdateCardInfo;
    public string enterCardID;
    public string url;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (Request.QueryString["MembCardID"].ToString() != "" && Request.QueryString["MembCardID"].ToString() != null)
            {
                url = "Associator/Perform/ModifyCard.aspx?MembCardID=" + Request.QueryString["MembCardID"].ToString();
            }

                QueryData();

            BindCardCardClass();
            BindCardType();
            lockControl(false);
            lblUpdateCardInfo = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblUpdateCardInfo");
           
            enterCardID = (String)GetGlobalResourceObject("BaseInfo", "Memb_EnterCardID");
        }
    }

    private void BindCardType()
    {
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

    private void QueryData()
    {
        DataSet ds = QueryAssociatorPO.GetLCustByCondition("MembCard.MembCardID = '" + Request.QueryString["MembCardID"].ToString() + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCardID.Text = ds.Tables[0].Rows[0]["MembCardId"].ToString();
            txtDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateIssued"]).ToString("yyyy-MM-dd");
            txtExpiredDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpDate"]).ToString("yyyy-MM-dd");
            dropCardLevel.SelectedValue = ds.Tables[0].Rows[0]["CardClassId"].ToString();
            dropCardType.SelectedValue = ds.Tables[0].Rows[0]["CardTypeId"].ToString();
            string cardStatusId = ds.Tables[0].Rows[0]["CardStatusId"].ToString();

            string cardOwner = ds.Tables[0].Rows[0]["CardOwner"].ToString();

            switch (cardOwner)
            {
                case "N":
                    radCustomer.Checked = true;
                    radEmployee.Checked = false;
                    radOther.Checked = false;
                    break;
                case "E":
                    radCustomer.Checked = false;
                    radEmployee.Checked = true;
                    radOther.Checked = false;
                    break;
                case "O":
                    radCustomer.Checked = false;
                    radEmployee.Checked = false;
                    radOther.Checked = true;
                    break;
            }

            switch (cardStatusId)
            {
                case "N":
                    optNew.Checked = true;

                    optRenew.Checked = false;
                    optLost.Checked = false;
                    optDemage.Checked = false;
                    optInvalidate.Checked = false;
                    optDowngrade.Checked = false;
                    optUpgrade.Checked = false;
                    optReturn.Checked = false;
                    break;

                case "R":
                    optRenew.Checked = true;

                    optNew.Checked = false;
                    optLost.Checked = false;
                    optDemage.Checked = false;
                    optInvalidate.Checked = false;
                    optDowngrade.Checked = false;
                    optUpgrade.Checked = false;
                    optReturn.Checked = false;
                    break;

                case "L":
                    optLost.Checked = true;
                    optNew.Checked = false;
                    optRenew.Checked = false;
                    optDemage.Checked = false;
                    optInvalidate.Checked = false;
                    optDowngrade.Checked = false;
                    optUpgrade.Checked = false;
                    optReturn.Checked = false;
                    break;

                case "D":
                    optDemage.Checked = true;
                    optNew.Checked = false;
                    optRenew.Checked = false;
                    optLost.Checked = false;
                    optInvalidate.Checked = false;
                    optDowngrade.Checked = false;
                    optUpgrade.Checked = false;
                    optReturn.Checked = false;
                    break;

                case "I":
                    optInvalidate.Checked = true;
                    optNew.Checked = false;
                    optRenew.Checked = false;
                    optLost.Checked = false;
                    optDemage.Checked = false;
                    optDowngrade.Checked = false;
                    optUpgrade.Checked = false;
                    optReturn.Checked = false;
                    break;

                case "W":
                    optDowngrade.Checked = true;
                    optNew.Checked = false;
                    optRenew.Checked = false;
                    optLost.Checked = false;
                    optDemage.Checked = false;
                    optInvalidate.Checked = false;
                    optUpgrade.Checked = false;
                    optReturn.Checked = false;
                    break;

                case "U":
                    optUpgrade.Checked = true;
                    optNew.Checked = false;
                    optRenew.Checked = false;
                    optLost.Checked = false;
                    optDemage.Checked = false;
                    optInvalidate.Checked = false;
                    optDowngrade.Checked = false;
                    optReturn.Checked = false;
                    break;

                case "T":
                    optReturn.Checked = true;
                    optNew.Checked = false;
                    optRenew.Checked = false;
                    optLost.Checked = false;
                    optDemage.Checked = false;
                    optInvalidate.Checked = false;
                    optDowngrade.Checked = false;
                    optUpgrade.Checked = false;
                    break;
            }

            txtMembCode.Text = ds.Tables[0].Rows[0]["MembCode"].ToString();
            txtMembName.Text = ds.Tables[0].Rows[0]["MemberName"].ToString();
            txtPassPort.Text = ds.Tables[0].Rows[0]["LOtherId"].ToString();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Memb_NOMemberCard") + "'", true);
        }
    }

    private void lockControl(bool m)
    {
        txtDate.Enabled = m;
        txtMembCode.Enabled = m;
        txtMembName.Enabled = m;
        txtPassPort.Enabled = m;
        txtCardID.Enabled = m;
    }

    private void clearControl()
    {
        txtCardID.Text = "";
        txtDate.Text = "";
        txtExpiredDate.Text = "";
        txtMembCode.Text = "";
        txtMembName.Text = "";
        txtPassPort.Text = "";
        optNew.Checked = true;
        optRenew.Checked = false;
        optLost.Checked = false;
        optDemage.Checked = false;
        optInvalidate.Checked = false;
        optDowngrade.Checked = false;
        optUpgrade.Checked = false;
        optReturn.Checked = false;
        BindCardCardClass();
        BindCardType();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        MembCard membCard = new MembCard();
        membCard.ExpDate = Convert.ToDateTime(txtExpiredDate.Text);
        membCard.CardClassId = Convert.ToInt32(dropCardLevel.SelectedValue);
        membCard.CardTypeId = dropCardType.SelectedValue;
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
        if(optNew.Checked == true)
        {
            membCard.CardStatusId = "N";
        }
        else if(optRenew.Checked == true)
        {
            membCard.CardStatusId = "R";
        }
        else if (optLost.Checked == true)
        {
            membCard.CardStatusId = "L";
        }
        else if (optDemage.Checked == true)
        {
            membCard.CardStatusId = "D";
        }
        else if (optInvalidate.Checked == true)
        {
            membCard.CardStatusId = "I";
        }
        else if (optDowngrade.Checked == true)
        {
            membCard.CardStatusId = "W";
        }
        else if(optUpgrade.Checked == true)
        {
            membCard.CardStatusId = "U";
        }
        else if (optReturn.Checked == true)
        {
            membCard.CardStatusId = "T";
        }

        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        membCard.ModifyTime = DateTime.Now;
        membCard.ModifyUserID = objSessionUser.UserID;
        membCard.OprDeptID = objSessionUser.DeptID;
        membCard.OprRoleID = objSessionUser.RoleID;

        membCard.MembCardId = txtCardID.Text;
        int result = MembCardPO.ModifyMembCardInfo(membCard);
        if (result == 1)
        {
            Response.Redirect("../QueryAssociator.aspx?flag=1");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearControl();
    }
}
