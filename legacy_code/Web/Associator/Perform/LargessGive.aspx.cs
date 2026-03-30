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
using Associator.Perform;
using Base.Biz;
using Base.DB;
using BaseInfo.User;
using Associator.Associator;
using Base;

/// <summary>
/// 修改人：hesijian
/// 修改时间:2009年5月15日
/// </summary>
public partial class Associator_Perform_LargessGive : BasePage 
{
    public string chkExtend;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            txtUser.Text = sessionUser.UserName;
            BindDrop();
            dropActID.Enabled = false;
            txtCardID.Enabled = false;
            btnQuery.Enabled = false;
            btnSave.Enabled = false;
            chkExtend = (String)GetGlobalResourceObject("BaseInfo", "Associator_lblLargessCharge");
            
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
                int giftOption = 0;
                if (radGiftOption.Checked == true)
                {
                    giftOption = GiftActivity.GIFTACTIVITY_ONCE;
                }
                else if(radGiftOptionOneDay.Checked == true)
                {
                    giftOption = GiftActivity.GIFTACTIVITY_DAY;
                }
                int flag = FreeGiftTransPO.GetFreeGiftTransByID(Convert.ToInt32(ds.Tables[0].Rows[0]["MembId"]), Convert.ToInt32(ViewState["giftID"]), Convert.ToInt32(dropActID.SelectedValue), giftOption);
                if (flag == 0)
                {
                    ViewState["membID"] = ds.Tables[0].Rows[0]["MembId"].ToString();
                    txtMembName.Text = ds.Tables[0].Rows[0]["MemberName"].ToString();
                    txtCertNum.Text = ds.Tables[0].Rows[0]["LOtherID"].ToString();
                    txtTel.Text = ds.Tables[0].Rows[0]["MobilPhone"].ToString();
                    txtNumber.Text = "1";
                    txtActDate.Text = DateTime.Now.ToShortDateString();
                    btnSave.Enabled = true;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '该会员已领赠品!'", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '会员卡号不对或卡无效,请检查!'", true);
            }
        }
        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        DataSet ds = GiftStockPO.GetGiftStockCnt1(Convert.ToInt32(dropActID.SelectedValue), Convert.ToInt32(dropServiceDesk.SelectedValue));
         if (Convert.ToInt32(ds.Tables[0].Rows[0]["StockCnt"]) > 0)
         {
             if (Convert.ToInt32(ds.Tables[0].Rows[0]["StockCnt"]) >= Convert.ToInt32(txtNumber.Text))
             {
                 FreeGiftTrans fGiftTrans = new FreeGiftTrans();
                 fGiftTrans.GiftTransID = BaseApp.GetGiftTransID();
                 fGiftTrans.GiftID = Convert.ToInt32(ViewState["giftID"]);
                 fGiftTrans.ActID = Convert.ToInt32(dropActID.SelectedValue);
                 fGiftTrans.ActDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                 fGiftTrans.GiftQty = Convert.ToInt32(txtNumber.Text);
                 fGiftTrans.MembID = Convert.ToInt32(ViewState["membID"]);
                 //fGiftTrans.CreateUserID = sessionUser.UserID;
                 //fGiftTrans.CreateTime = DateTime.Now;
                 //fGiftTrans.ModifyUserID = 0;
                 //fGiftTrans.ModifyTime = DateTime.Now;
                 //fGiftTrans.OprRoleID = sessionUser.RoleID;
                 //fGiftTrans.OprDeptID = sessionUser.DeptID;
                 //fGiftTrans.CounterID = Convert.ToInt32(dropServiceDesk.SelectedValue.Trim());
                 BaseTrans baseTrans = new BaseTrans();
                 baseTrans.BeginTrans();
                 try
                 {
                     baseTrans.Insert(fGiftTrans);
                     GiftStockPO.updateGiftStock(baseTrans, Convert.ToInt32(dropServiceDesk.SelectedValue), Convert.ToInt32(ViewState["giftID"]), Convert.ToInt32(txtNumber.Text));
                 }
                 catch (Exception ex)
                 {
                     baseTrans.Rollback();
                     throw ex;
                     // ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + ex +" '", true);

                     return;
                 }
                 baseTrans.Commit();
                 btnSave.Enabled = false;
                 txtCardID.Text = "";
                 txtMembName.Text = "";
                 txtCertNum.Text = "";
                 txtTel.Text = "";
                 txtNumber.Text = "";
                 txtActDate.Text = "";
                 ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '赠品赠送成功!'", true);
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
        btnSave.Enabled = false;
        txtCardID.Text = "";
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }

    private void BindDrop()
    {
        //服务台

        BaseBO baseBO = new BaseBO();
        baseBO.OrderBy = "CounterID";
        Resultset rs = baseBO.Query(new Counter());
        dropServiceDesk.Items.Add(new ListItem("--请选择--","0"));
        foreach (Counter counter in rs)
        {
            dropServiceDesk.Items.Add(new ListItem(counter.CounterDesc,counter.CounterID.ToString()));
        }       
    }
    protected void dropActID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dropActID.SelectedValue != "0")
        {
            txtCardID.Text = "";
            txtMembName.Text = "";
            txtCertNum.Text = "";
            txtTel.Text = "";
            txtNumber.Text = "";
            txtActDate.Text = "";

            DataSet ds = GiftActivityPO.GetGiftActivityByID(Convert.ToInt32(dropActID.SelectedValue), Gift.FREEGIFT_YES);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtStartDate.Text = ds.Tables[0].Rows[0]["ShopStartDate"].ToString();
                txtEndDate.Text = ds.Tables[0].Rows[0]["ShopEndDate"].ToString();
                txtGiftDesc.Text = ds.Tables[0].Rows[0]["GiftDesc"].ToString();
                ViewState["giftID"] = ds.Tables[0].Rows[0]["GiftID"].ToString();
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["GiftOption"]) == GiftActivity.GIFTACTIVITY_ONCE)
                {
                    radGiftOption.Checked = true;
                    radGiftOptionOneDay.Checked = false;
                }
                else if (Convert.ToInt32(ds.Tables[0].Rows[0]["GiftOption"]) == GiftActivity.GIFTACTIVITY_DAY)
                {
                    radGiftOption.Checked = false;
                    radGiftOptionOneDay.Checked = true;
                }
                txtCardID.Enabled = true;
                btnQuery.Enabled = true;
            }
        }
        else
        {
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtGiftDesc.Text = "";
            radGiftOption.Checked = false;
            radGiftOptionOneDay.Checked = false;
            txtCardID.Enabled = false;
            btnQuery.Enabled = false;
        }
       
    }
    protected void dropServiceDesk_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dropServiceDesk.SelectedValue == "0")
        {
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            txtGiftDesc.Text = "";
            radGiftOption.Checked = false;
            radGiftOptionOneDay.Checked = false;
            txtCardID.Enabled = false;
            btnQuery.Enabled = false;
            dropActID.Enabled = false;
        }
        else
        {
            dropActID.Enabled = true;

            //赠品活动
            //baseBO.OrderBy = "";
            //baseBO.OrderBy = "ActID DESC";
            dropActID.Items.Clear();
            DataSet ActRS = FreeGiftTransPO.GiftActivityByID(Convert.ToInt32(dropServiceDesk.SelectedValue));
            int count = ActRS.Tables[0].Rows.Count;
            dropActID.Items.Add(new ListItem("--请选择--", "0"));
            for (int i = 0; i < count; i++)
            {
                dropActID.Items.Add(new ListItem(ActRS.Tables[0].Rows[i]["ActDesc"].ToString(), ActRS.Tables[0].Rows[i]["ActID"].ToString()));
                //dropActID.Items.Add(new ListItem(giftAct.ActDesc, giftAct.ActID.ToString()));
            }
        }
    }
}
