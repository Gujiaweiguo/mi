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
using Base;
using Associator.Perform;
using Associator.Associator;
using BaseInfo.User;

public partial class Associator_Perform_TicketChargeGift : BasePage
{
    public string ticketChargeGift;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            txtUser.Text = sessionUser.UserName;
            BindDrop();
            dropActID.Enabled = false;
            btnSave.Enabled = false;
            ticketChargeGift = (String)GetGlobalResourceObject("BaseInfo", "Associator_TicketChargeGift");
            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
        }
    }

    private void BindDrop()
    {
        //服务台

        BaseBO baseBO = new BaseBO();
        baseBO.OrderBy = "CounterID";
        Resultset rs = baseBO.Query(new Counter());
        dropServiceDesk.Items.Add(new ListItem("--请选择--", "0"));
        foreach (Counter counter in rs)
        {
            dropServiceDesk.Items.Add(new ListItem(counter.CounterDesc, counter.CounterID.ToString()));
        }
    }
    protected void dropServiceDesk_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dropServiceDesk.SelectedValue == "0")
        {
            txtTransAmt.Text = "";
            txtCardID.Enabled = false;
            btnQuery.Enabled = false;
            dropActID.Enabled = false;
        }
        else
        {
            dropActID.Enabled = true;

            //赠品活动
            //baseBO.OrderBy = "";
            //baseBO.OrderBy = "ActID ExID";
            dropActID.Items.Clear();
            DataSet giftRS = GiftPO.GetGift(1, Convert.ToInt32(dropServiceDesk.SelectedValue));
            dropActID.Items.Add(new ListItem("--请选择--", "0"));
            int count = giftRS.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                dropActID.Items.Add(new ListItem(giftRS.Tables[0].Rows[i]["GiftDesc"].ToString(), giftRS.Tables[0].Rows[i]["GiftID"].ToString()));
            }
        }
    }
    protected void dropActID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dropActID.SelectedValue != "0")
        {
            Resultset rs = GiftPO.GetGiftByID(Convert.ToInt32(dropActID.SelectedValue));
            if (rs.Count > 0)
            {
                Gift gift = rs.Dequeue() as Gift;
                ViewState["limitOne"] = gift.LimitOne;

                txtTransAmt.Text = gift.ReceiptMoney.ToString();
                txtCardID.Enabled = true;
                btnQuery.Enabled = true;
            }
        }
        else
        {
            txtTransAmt.Text = "";
            txtCardID.Enabled = false;
            btnQuery.Enabled = false;
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (txtCardID.Text.Trim() != "")
        {
            DataSet ds = QueryAssociatorPO.GetLCustByCondition("MembCard.MembCardID = '" + txtCardID.Text.Trim() + "' AND MembCard.CardStatusID IN ('N')" +
                                " AND MembCard.ExpDate >= '" + DateTime.Now.ToShortDateString() + "'");

            if (ds.Tables[0].Rows.Count > 0)
            {
               //int flag = ExTransPO.GetExTransPOByID(Convert.ToInt32(ds.Tables[0].Rows[0]["MembId"]),Convert.ToInt32(dropActID.SelectedValue),Convert.ToInt32(ViewState["limitOne"]),
                //int flag = FreeGiftTransPO.GetFreeGiftTransByID(Convert.ToInt32(ds.Tables[0].Rows[0]["MembId"]), Convert.ToInt32(ViewState["giftID"]), Convert.ToInt32(dropActID.SelectedValue), giftOption);
                
                ViewState["membID"] = ds.Tables[0].Rows[0]["MembId"].ToString();
                txtMembName.Text = ds.Tables[0].Rows[0]["MemberName"].ToString();
                txtCertNum.Text = ds.Tables[0].Rows[0]["LOtherID"].ToString();
                txtNumber.Text = "1";
                txtActDate.Text = DateTime.Now.ToShortDateString();
                btnSave.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '会员卡号不对或卡无效,请检查!'", true);
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
         DataSet ds = GiftStockPO.GetGiftStockCnt(Convert.ToInt32(dropActID.SelectedValue), Convert.ToInt32(dropServiceDesk.SelectedValue));
         if (Convert.ToInt32(ds.Tables[0].Rows[0]["StockCnt"]) > 0)
         {
             int flag = ExTransPO.GetExTransPOByID(Convert.ToInt32(ViewState["membID"]), Convert.ToInt32(dropActID.SelectedValue), Convert.ToInt32(ViewState["limitOne"]), txtRecepitID.Text);
             if (flag == 0)
             {
                 if (Convert.ToInt32(ds.Tables[0].Rows[0]["StockCnt"]) >= Convert.ToInt32(txtNumber.Text))
                 {
                     if (Convert.ToDecimal(txtAmt.Text) >= Convert.ToDecimal(txtTransAmt.Text))
                     {
                         ExTrans exTrans = new ExTrans();
                         exTrans.ExID = BaseApp.GetExID();
                         exTrans.ExDate = Convert.ToDateTime(txtActDate.Text);
                         exTrans.GiftQty = Convert.ToInt32(txtNumber.Text);
                         exTrans.MembID = Convert.ToInt32(ViewState["membID"]);
                         exTrans.ReceiptNum = txtRecepitID.Text;
                         exTrans.TransAmt = Convert.ToDecimal(txtAmt.Text);
                         exTrans.GiftID = Convert.ToInt32(dropActID.SelectedValue);

                         BaseTrans baseTrans = new BaseTrans();
                         baseTrans.BeginTrans();
                         try
                         {
                             baseTrans.Insert(exTrans);
                             GiftStockPO.updateGiftStock(baseTrans, Convert.ToInt32(dropServiceDesk.SelectedValue), Convert.ToInt32(dropActID.SelectedValue), Convert.ToInt32(txtNumber.Text));
                             ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '礼品换购成功!'", true);
                         }
                         catch (Exception ex)
                         {
                             baseTrans.Rollback();
                             throw ex;
                             // ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + ex +" '", true);
                                                         
                         }
                         baseTrans.Commit();
                         btnSave.Enabled = false;
                         txtActDate.Text = "";
                         txtAmt.Text = "";
                         txtCardID.Text = "";
                         txtCertNum.Text = "";
                         txtMembName.Text = "";
                         txtNumber.Text = "";
                         txtRecepitID.Text = "";
                         txtTransAmt.Text = "";
                     }
                     else
                     {
                         ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '小票金额不够!'", true);
                     }
                 }
                 else
                 {
                     ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "alert('赠品数量不够！')", true);
                 }
             }
             else
             {
                 ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '该小票已兑换过礼品!'", true);
             }
         }
         else
         {
             ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "alert('赠品已兑换完！')", true);
         }
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
        txtActDate.Text = "";
        txtAmt.Text = "";
        txtCardID.Text = "";
        txtCertNum.Text = "";
        txtMembName.Text = "";
        txtNumber.Text = "";
        txtRecepitID.Text = "";
        txtTransAmt.Text = "";
    }
}
