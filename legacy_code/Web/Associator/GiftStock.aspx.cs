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

using Base.DB;
using Base.Biz;
using Base.Sys;
using Associator.Perform;
using Base.Page;
using Base;

public partial class Associator_GiftStock :BasePage
{
    public string tMunu_GiftStock="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Resultset rs = new Resultset();
            BaseBO baseBO = new BaseBO();
            GiftStock giftStock = new GiftStock();

            /*赠品*/
            rs = baseBO.Query(new Gift());
            foreach (Gift gift in rs)
            {
                cmbGiftID.Items.Add(new ListItem(gift.GiftDesc,gift.GiftID.ToString()));
            }

            /*服务台*/
            rs = baseBO.Query(new Counter());
            foreach (Counter counter in rs)
            {
                cmbStockID.Items.Add(new ListItem(counter.CounterDesc, counter.CounterID.ToString()));
            }


            if (!Request.QueryString["GiftStock"].ToString().Equals("0"))
            {
                baseBO.WhereClause = "StockID=" + Request.QueryString["GiftStock"].ToString();
                rs = baseBO.Query(giftStock);

                giftStock = rs.Dequeue() as GiftStock;

                cmbGiftID.SelectedValue = giftStock.GiftID.ToString();
                cmbStockID.SelectedValue = giftStock.CounterID.ToString();
                txtStockCnt.Text = giftStock.StockCnt.ToString();
                txtRefPrice.Text = giftStock.RefPrice.ToString();

                ViewState["GiftStockFlag"] = "Update";
            }
            else
            {
                ViewState["GiftStockFlag"] = "";
            }


            tMunu_GiftStock = (String)GetGlobalResourceObject("BaseInfo", "TMunu_GiftStock");

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtRefPrice.Text.Trim() != "" && txtStockCnt.Text.Trim() != "")
        {
            BaseBO baseBO = new BaseBO();
            GiftStock giftStock = new GiftStock();

            giftStock.StockID = BaseApp.GetGiftStockID();
            giftStock.GiftID = Convert.ToInt32(cmbGiftID.SelectedValue);
            giftStock.CounterID = Convert.ToInt32(cmbStockID.SelectedValue);
            giftStock.StockCnt = Convert.ToInt32(txtStockCnt.Text);
            giftStock.RefPrice = Convert.ToDecimal(txtRefPrice.Text);


            if (ViewState["GiftStockFlag"].Equals("Update"))
            {
                baseBO.WhereClause = "StockID=" + Request.QueryString["GiftStock"].ToString();
                if (baseBO.Update(giftStock) == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    return;
                }
                else
                {
                    txtRefPrice.Text = "";
                    txtStockCnt.Text = "";
                    ViewState["GiftStockFlag"] = "";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                }
            }
            else
            {
                if (baseBO.Insert(giftStock) == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    return;
                }
                else
                {
                    txtRefPrice.Text = "";
                    txtStockCnt.Text = "";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                }
            }
        }
    }
}
