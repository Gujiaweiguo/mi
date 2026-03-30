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
using Associator.Associator;
using Associator.Perform;
using BaseInfo.User;
using Base.Page;

/// <summary>
/// 修改人：zhangguangrui
/// 修改时间：2009年8月5日

/// </summary>

public partial class Associator_Perform_LargessCharge : BasePage
{
    public string lbllargessExcharg;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            txtUser.Text = sessionUser.UserName;
            BindDrop();
            txtCardID.Enabled = false;
            btnQuery.Enabled = false;
            btnSave.Enabled = false;
            dropActID.Enabled = false;
            txtNum.Attributes.Add("onblur", "AccountBonus()");
            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            lbllargessExcharg = (String)GetGlobalResourceObject("BaseInfo", "Associator_lbllargessExcharg");
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
        if (dropServiceDesk.SelectedValue != "0")
        {
            txtCardID.Text = "";
            txtCardID.Enabled = true;
            btnQuery.Enabled = true;
        }
        else
        {
            txtCardID.Enabled = false;
            btnQuery.Enabled = false;
        }
    }
    protected void dropActID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dropActID.SelectedValue != "0")
        {
            btnSave.Enabled = true;
            Resultset rs = GiftPO.GetGiftByID(Convert.ToInt32(dropActID.SelectedValue));
            Gift gift = rs.Dequeue() as Gift;
            txtMustBonus.Text = gift.BonusCost.ToString();
            txtDate.Text = DateTime.Now.ToShortDateString();
        }
        else
        {
            txtMustBonus.Text = "";
            txtNum.Text = "";
            txtOverTotal.Text = "";
            txtDate.Text = "";
            txtTotalBonus.Text = "";
            btnSave.Enabled = false;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataSet ds = GiftStockPO.GetGiftStockCnt(Convert.ToInt32(dropActID.SelectedValue), Convert.ToInt32(dropServiceDesk.SelectedValue));
        if (Convert.ToInt32(ds.Tables[0].Rows[0]["StockCnt"]) > 0)
        {
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["StockCnt"]) >= Convert.ToInt32(txtNum.Text))
            {
                if (Convert.ToDecimal(txtOverTotal.Text) >= 0)
                {
                    BaseTrans baseTrans = new BaseTrans();
                    RedeemH redH = new RedeemH();
                    if (Convert.ToDecimal(txtMustBonus.Text) * Convert.ToDecimal(txtNum.Text) == Convert.ToDecimal(txtTotalBonus.Text))
                    {
                        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                        redH.RedeemID = BaseApp.GetRedeemID();
                        redH.RedeemDate = Convert.ToDateTime(txtDate.Text);
                        redH.MembID = Convert.ToInt32(ViewState["membID"]);
                        redH.GiftID = Convert.ToInt32(dropActID.SelectedValue);
                        redH.BonusPrev = Convert.ToDecimal(txtBonus.Text);
                        redH.RedeemAmt = Convert.ToDecimal(txtTotalBonus.Text);
                        redH.BonusCurr = redH.BonusPrev - redH.RedeemAmt;
                        redH.GiftQty = Convert.ToInt32(txtNum.Text);
                        redH.CreateUserID = objSessionUser.UserID;
                        redH.CreateTime = DateTime.Now;
                        redH.ModifyUserID = 0;
                        redH.ModifyTime = DateTime.Now;
                        redH.OprRoleID = objSessionUser.RoleID;
                        redH.OprDeptID = objSessionUser.DeptID;
                        redH.CounterID = Convert.ToInt32(dropServiceDesk.SelectedValue.Trim());
                        baseTrans.BeginTrans();
                        try
                        {
                            baseTrans.Insert(redH);
                            GiftStockPO.updateGiftStock(baseTrans, Convert.ToInt32(dropServiceDesk.SelectedValue), Convert.ToInt32(dropActID.SelectedValue), Convert.ToInt32(txtNum.Text));
                            BonusPO.AccountBonus(redH.MembID, redH.BonusCurr, baseTrans);
                            txtNum.Text = "";
                            txtBonus.Text = txtOverTotal.Text;
                        }
                        catch (Exception ex)
                        {
                            baseTrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '积分兑换失败！'", true);
                            return;
                        }
                        baseTrans.Commit();
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '积分兑换成功！'", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '积分兑换输入有误！'", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '积分不够兑换！'", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "alert('赠品数量不够！')", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "alert('赠品已兑换完！')", true);
        }
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        txtMustBonus.Text = "";
        txtNum.Text = "";
        txtOverTotal.Text = "";
        txtDate.Text = "";
        txtTotalBonus.Text = "";
        dropActID.SelectedValue = "0";
        btnSave.Enabled = false;
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //赠品活动

        BaseBO baseBO = new BaseBO();

        dropActID.Items.Clear();
        DataSet giftRS = GiftPO.GetGift(2, Convert.ToInt32(dropServiceDesk.SelectedValue));
        dropActID.Items.Add(new ListItem("--请选择--", "0"));
        int count = giftRS.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            dropActID.Items.Add(new ListItem(giftRS.Tables[0].Rows[i]["GiftDesc"].ToString(), giftRS.Tables[0].Rows[i]["GiftID"].ToString()));
        }

        DataSet ds = QueryAssociatorPO.GetLCustByCondition("MembCard.MembCardID = '" + txtCardID.Text.Trim() + "'  AND MembCard.CardStatusID IN ('N')" +
                                " AND MembCard.ExpDate >= '" + DateTime.Now.ToShortDateString() + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            dropActID.Enabled = true;
            ViewState["membID"] = ds.Tables[0].Rows[0]["MembId"].ToString();
            txtMembName.Text = ds.Tables[0].Rows[0]["MemberName"].ToString();
            txtCertNum.Text = ds.Tables[0].Rows[0]["LOtherId"].ToString();
            txtActDate.Text = ds.Tables[0].Rows[0]["ExpDate"].ToString();
            DataSet bonusDS = BonusPO.GetBonusByMembID(Convert.ToInt32(ViewState["membID"]));
            if (bonusDS.Tables[0].Rows.Count > 0)
            {
                txtBonus.Text = bonusDS.Tables[0].Rows[0]["BonusTotal"].ToString();
            }
            else
            {
                string sqlStr ="Insert into Bonus (membid,BonusTotal,BonusPrev,BonusCurr,BonusBook,BonusReset,TotBonusReset,ResetDate,TransID,Updated) " +
                                " Values('" + ds.Tables[0].Rows[0]["MembId"].ToString().Trim() + "',0,0,0,0,0,0,'" + DateTime.Now +"','','" + DateTime.Now +"')";

                if (baseBO.ExecuteUpdate(sqlStr) == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '会员卡积分信息错误！'", true);
                    return;
                }

                txtBonus.Text = "0";
            }
            dropActID.Enabled = true;
        }
        else
        {
            txtMustBonus.Text = "";
            txtNum.Text = "";
            txtOverTotal.Text = "";
            txtDate.Text = "";
            txtTotalBonus.Text = "";
            dropActID.SelectedValue = "0";
            txtMembName.Text = "";
            txtActDate.Text = "";
            txtBonus.Text = "";
            txtCertNum.Text = "";
            btnSave.Enabled = false;
            dropActID.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '会员卡号不对或卡无效,请检查!'", true);
        }
    }
}
