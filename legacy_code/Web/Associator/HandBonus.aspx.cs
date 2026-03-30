using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Base.Page;
using Associator.Associator;
using Associator.Perform;
using Base.Biz;
using BaseInfo.User;
using Lease.ConShop;
using Base.DB;
using System.Drawing;
using Sell;
using System.Text.RegularExpressions;

/// <summary>
/// 编写人：hesijian
/// 编写时间：2009年6月25日
/// </summary>

public partial class Associator_HandBonus : BasePage
{
    public string baseInfo;  //基本信息
    private decimal bonusPer; // 积分率
    private decimal transAmt; //消费金额
    private string transNewid;//新交易号
  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Init();
            txtShopCode.Attributes.Add("onclick", "ShowTree()");
            txtShopCode1.Attributes.Add("onclick", "ShowTree()");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_lblHandBonus");
            txtAmt.Attributes.Add("onkeydown", "textleave()");
            txtBonus.Attributes.Add("onkeydown", "textleave()");
            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            btnSave1.Attributes.Add("onclick", "return InputValidator1(form1)");
            btnQuery.Attributes.Add("onclick", "return InputValidator2(form1)");

        }
        
    }

    //初始化
    private void Init()
    {
        StopNoPOSReceipt();
        StartPOSReceipt();
    }

    #region POS小票和非POS小票的启动与禁用
    //禁用POS小票查询保存
    private void StopPOSReceipt()
    {
        
        txtAmt.Text = "";
        txtAmt.BackColor = Color.FromArgb(180, 193, 209);
        txtBonus.Text = "";
        txtBonus.BackColor = Color.FromArgb(180, 193, 209);
        txtCardID.Text = "";
        txtCardID.BackColor = Color.FromArgb(180, 193, 209);
        txtDate.Text = "";
        txtDate.BackColor = Color.FromArgb(180, 193, 209);
        txtRecepid.Text = "";
        txtRecepid.BackColor = Color.FromArgb(180, 193, 209); 
        txtShopCode.Text = "";
        txtShopCode.BackColor = Color.FromArgb(180, 193, 209);
        txtTrans.Text = "";
        txtTrans.BackColor = Color.FromArgb(180, 193, 209);
        txtDate.Enabled = false;
        txtRecepid.Enabled = false;
        txtShopCode.Enabled = false;
        txtCardID.Enabled = false;
        txtBonus.Enabled = false;
        txtDate.Enabled = false;
        btnQuit.Enabled = false;
        btnSave.Enabled = false;
        btnQuery.Enabled = false;
        txtInputText.Visible = false;
        Rdo1.Checked = false;
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);
    
    }

    //启用POS小票查询保存
    private void StartPOSReceipt()
    {
        txtAmt.Text = "";
        txtAmt.BackColor = Color.FromArgb(255, 255, 255);
        txtBonus.Text = "";
        txtBonus.BackColor = Color.FromArgb(255, 255, 255);
        txtCardID.Text = "";
        txtCardID.BackColor = Color.FromArgb(255, 255, 255);
        txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtDate.BackColor = Color.FromArgb(255, 255, 255);
        txtRecepid.Text = "";
        txtRecepid.BackColor = Color.FromArgb(255, 255, 255);
        txtShopCode.Text = "";
        txtShopCode.BackColor = Color.FromArgb(255, 255, 255);
        txtTrans.Text = "";
        txtTrans.BackColor = Color.FromArgb(255, 255, 255);
        txtDate.Enabled = true;
        txtRecepid.Enabled = true;
        txtShopCode.Enabled = true;
        txtCardID.Enabled = true;
        txtBonus.Enabled = true;
        txtDate.Enabled = true;
        btnQuit.Enabled = true;
        btnSave.Enabled = true;
        btnQuery.Enabled = true;
        txtInputText.Visible = false;
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);
        
    
    }
   
    //禁用非POS小票输入保存
    private void StopNoPOSReceipt()
    {
        txtShopCode1.Text = "";
        txtShopCode1.BackColor = Color.FromArgb(180, 193, 209); 
        txtReceiptid1.Text = "";
        txtReceiptid1.BackColor = Color.FromArgb(180, 193, 209);
        txtDate1.Text = "";
        txtDate1.BackColor = Color.FromArgb(180, 193, 209);
        txtBonus1.Text = "";
        txtBonus1.BackColor = Color.FromArgb(180, 193, 209);
        txtAmt1.Text = "";
        txtAmt1.BackColor = Color.FromArgb(180, 193, 209);
        txtCardId1.Text = "";
        txtCardId1.BackColor = Color.FromArgb(180, 193, 209); 
        txtCardId1.Enabled = false;
        txtDate1.Enabled = false;
        txtReceiptid1.Enabled = false;
        txtShopCode1.Enabled = false;
        txtBonus1.Enabled = false;
        txtAmt1.Enabled = false;
        btnSave1.Enabled = false;
        btnCancel1.Enabled = false;
        Rdo2.Checked = false;
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);
    
    }

    //启动非POS小票输入保存
    private void StartNoPOSReceipt()
    {
        txtShopCode1.Text = "";
        txtShopCode1.BackColor = Color.FromArgb(255, 255, 255);
        txtReceiptid1.Text = "";
        txtReceiptid1.BackColor = Color.FromArgb(255, 255, 255);
        txtDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtDate1.BackColor = Color.FromArgb(255, 255, 255); 
        txtBonus1.Text = "";
        txtBonus1.BackColor = Color.FromArgb(255, 255, 255);
        txtAmt1.Text = "";
        txtAmt1.BackColor = Color.FromArgb(255, 255, 255);
        txtCardId1.Text = "";
        txtCardId1.BackColor = Color.FromArgb(255, 255, 255);
        txtCardId1.Enabled = true;
        txtDate1.Enabled = true;
        txtReceiptid1.Enabled = true;
        txtShopCode1.Enabled = true;
        txtBonus1.Enabled = true;
        txtAmt1.Enabled = true;
        btnSave1.Enabled = true;
        btnCancel1.Enabled = true;

        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);

    }
    #endregion


    #region POS小票与非POS小票撤消操作
    //POS小票撤消操作
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Associator/HandBonus.aspx");
       // ClearPOSAllPage();
    }

   

    //非POS小票撤消操作
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Associator/HandBonus.aspx");
       //ClearNoPOSAllPage();
    }
    #endregion

    #region 清除POS小票与非POS小票页面
    //清除POS小票页面
    private void ClearPOSAllPage()
    {
        txtAmt.Text = "";
        txtBonus.Text = "";
        txtCardID.Text = "";
        txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtRecepid.Text = "";
        txtShopCode.Text = "";
        txtTrans.Text = "";
        txtInputText.Visible = false;
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);
        
    }

    //清除非POS小票页面
    private void ClearNoPOSAllPage()
    {
        txtCardId1.Text = "";
        txtAmt1.Text = "";
        txtBonus1.Text = "";
        txtDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtReceiptid1.Text = "";
        txtShopCode1.Text = "";    
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);

       


    }
    #endregion

    #region 树状菜单操作
    //POS小票树状菜单
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            txtShopCode.Text= ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
           
        }
    }

    //非POS小票树状菜单
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            txtShopCode1.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
           
        }

    }
    #endregion

    #region POS小票查询与保存
    //查询POS小票交易号TransID
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        txtInputText.Visible = false;
        int shopid = Convert.ToInt32(ViewState["shopID"]);
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        ////获得会员卡的相关信息
        //DataSet ds1 = QueryAssociatorPO.GetLCustByCondition("MembCard.MembCardID = '" + txtCardID.Text.Trim() + "'");

        //在TransSku表里通过小票号与商铺号获得交易号Transid
        BaseBO baseBo = new BaseBO();
        string WhereClause = " TransSku.ReceiptId = '" + txtRecepid.Text + "' AND TransSku.ShopID = '" + shopid + "'";
        DataSet ds2 = TransSkuPO.GetTransSkuPaidAmt(WhereClause);//baseBo.QueryDataSet(new TransSku());
        string transid = null;
        try
        {
            transid = ds2.Tables[0].Rows[0]["TransID"].ToString();

            DataSet ds3 = PurhistPO.GetPurhistByWhere(" AND Purhist.TransID = '" + transid+"'");
            ////判断会员卡是否存在
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    //检验在Purhist表里是否存在相应的交易号Transid
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        txtTrans.Text = transid;
                        txtAmt.Text = ds3.Tables[0].Rows[0]["NetAmt"].ToString();
                        txtBonus.Text = ds3.Tables[0].Rows[0]["BonusAmt"].ToString();
                    }
                    else
                    {
                        txtTrans.Text = transid;
                        txtAmt.Text = ds2.Tables[0].Rows[0]["PaidAmt"].ToString();
                        GetBonusPer();
                        txtBonus.Text = decimal.Truncate(Convert.ToDecimal(txtAmt.Text.Trim()) * bonusPer) + "";
                    }

                }
                else
                {
                    txtAmt.Text = "";
                    txtBonus.Text = "";
                    txtDate.Text = "";
                    txtTrans.Text = "";
                    txtAmt.Enabled = true;
                    txtBonus.Enabled = true;
                    txtDate.Enabled = true;
                    txtInputText.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '小票号或商铺号错误!'", true);
                
                }

            //}
            //else
            //{
            //    txtAmt.Text = "";
            //    txtBonus.Text = "";
            //    txtDate.Text = "";
            //    txtTrans.Text = "";
            //    txtAmt.Enabled = true;
            //    txtBonus.Enabled = true;
            //    txtDate.Enabled = true;
            //    txtInputText.Visible = false;
            //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '会员卡号不对或信息不正确,请检查!'", true);
            //}

        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "myMessage", "alert('查询信息不正确,请重试!')", true);
        }
        #region
        ////=============================================================================
        //if (txtCardID.Text.Trim() != "" && txtRecepid.Text.Trim() !="" && txtShopCode.Text.Trim() != "" )
        //{
        //    string whrStr = " AND purhist.MembCardID = '" + txtCardID.Text + "' AND purhist.Receiptid = '"+txtRecepid.Text+"' AND purhist.ShopID = '"+shopid+"'";
        //    DataSet ds = PurhistPO.GetPurhistByWhere(whrStr);
           
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        if (Rdo1.Checked)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);
        //            txtTrans.Text = ds.Tables[0].Rows[0]["TransId"].ToString();
        //            txtDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["TransDT"].ToString()).ToString("yyyy-MM-dd");
        //            GetTransAmt();
        //            GetBonusPer();
        //            txtAmt.Text = transAmt + "";
        //            //截取积分整数部分
        //            txtBonus.Text = decimal.Truncate(transAmt * bonusPer) + "";

        //        }
        //        else
        //        {
        //            txtAmt.Text = "";
        //            txtBonus.Text = "";
        //            txtDate.Text = "";
        //            txtTrans.Text = "";
        //            txtAmt.Enabled = true;
        //            txtBonus.Enabled = true;
        //            txtDate.Enabled = true;
        //            txtInputText.Visible = false;
        //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请选择正确的POS小票选项!'", true);
                
        //        }
                
        //    }
        //    else
        //    {
        //        if (Rdo2.Checked)
        //        {
        //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);
        //            transNewid = "9" + sessionUser.UserID + DateTime.Now.ToString("yyMMddhhmmss");
        //            txtTrans.Text = transNewid;
        //            GetBonusPer();
                    
        //            //截取积分整数部分
        //            try
        //            {
        //                txtBonus.Text = decimal.Truncate(Convert.ToDecimal(txtAmt.Text) * bonusPer) + "";
        //            }
        //            catch
        //            {

        //                txtBonus.Text = "";
        //            }
        //            return;
                
        //        }
        //        txtAmt.Text = "";
        //        txtBonus.Text = "";
        //        txtDate.Text = "";
        //        txtTrans.Text = "";
        //        txtAmt.Enabled = true;
        //        txtBonus.Enabled = true;
        //        txtDate.Enabled = true;
        //        txtInputText.Visible = false;
        //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '会员卡号不对或信息不正确,请检查!'", true);
        //    }
        //}
        //else
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请输入完整的查询信息!'", true);
        //}
        #endregion
    }

    //POS小票保存信息
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int shopID = Convert.ToInt32(ViewState["shopID"]);
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        DataSet ds = QueryAssociatorPO.GetLCustByCondition("MembCard.MembCardID = '" + txtCardID.Text.Trim() + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataSet myDs = HandBonusPO.GetBonusByMembID(Convert.ToInt32(ds.Tables[0].Rows[0]["MembID"]));
            if (myDs.Tables[0].Rows.Count > 0)
            {
                DataSet tempDS = PurhistPO.GetPurhistByWhere(" AND TransID = '" + txtTrans.Text + "'");
                if (tempDS.Tables[0].Rows.Count > 0)
                {
                    decimal amt = 0;
                    if (Convert.ToDecimal(txtAmt.Text) >= 0)
                    {
                        amt = 0;
                    }
                    else
                    {
                        amt = Convert.ToDecimal(txtAmt.Text);
                    }
                    flagHidn.Value = Convert.ToInt32(ds.Tables[0].Rows[0]["MembID"]) + "," + ds.Tables[0].Rows[0]["MembCardID"].ToString().Trim() + "," + Convert.ToDecimal(txtBonus.Text) + "," + txtTrans.Text + "," + Convert.ToDateTime(txtDate.Text) + "," + amt + "," + Convert.ToInt32(sessionUser.UserID) + "," + shopID + "," + txtRecepid.Text + "," + Convert.ToDateTime(datetime);
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "me", "ConfirmYesOrNo1()", true);
                }
                else
                {
                    HandBonusPO.UpdateOrInsertBouns(1, Convert.ToInt32(ds.Tables[0].Rows[0]["MembID"]), ds.Tables[0].Rows[0]["MembCardID"].ToString().Trim(), Convert.ToDecimal(txtBonus.Text), txtTrans.Text, Convert.ToDateTime(txtDate.Text), Convert.ToDecimal(txtAmt.Text), Convert.ToInt32(sessionUser.UserID), shopID, txtRecepid.Text, Convert.ToDateTime(datetime));
                    txtAmt.Text = "";
                    txtBonus.Text = "";
                    txtCardID.Text = "";
                    txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtRecepid.Text = "";
                    txtShopCode.Text = "";
                    txtTrans.Text = "";
                    txtBonus.Enabled = true;
                    txtDate.Enabled = true;
                    txtInputText.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '积分补登成功!'", true);
                }
            }
            else
            {
                HandBonusPO.UpdateOrInsertBouns(0, Convert.ToInt32(ds.Tables[0].Rows[0]["MembID"]), ds.Tables[0].Rows[0]["MembCardID"].ToString().Trim(), Convert.ToDecimal(txtBonus.Text), txtTrans.Text, Convert.ToDateTime(txtDate.Text), Convert.ToDecimal(txtAmt.Text), Convert.ToInt32(sessionUser.UserID), shopID, txtRecepid.Text, Convert.ToDateTime(datetime));
                txtAmt.Text = "";
                txtBonus.Text = "";
                txtCardID.Text = "";
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtRecepid.Text = "";
                txtShopCode.Text = "";
                txtTrans.Text = "";
                txtBonus.Enabled = true;
                txtDate.Enabled = true;
                txtInputText.Visible = false;
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '积分补登成功!'", true);
            }
            
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '会员卡号不正确，请检查!'", true);
        }
    }
    #endregion

    //非POS小票保存信息
    protected void btnSave1_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int shopID = Convert.ToInt32(ViewState["shopID"]);
        DataSet ds = QueryAssociatorPO.GetLCustByCondition("MembCard.MembCardID = '" + txtCardId1.Text.Trim() + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ValidatorNoPOSPage())
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "me", "ConfirmYesOrNo()", true);
                
            }
            else
            {
                int membid = Convert.ToInt32(ds.Tables[0].Rows[0]["MembID"]);
                transNewid = "9" + sessionUser.UserID + DateTime.Now.ToString("yyMMddHHmmss");
                
                DataSet ds1 = BonusPO.GetBonusByMembID(membid);
                //判断该会员号是否存在积分
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    HandBonusPO.UpdateOrInsertBouns(1, membid, ds.Tables[0].Rows[0]["MembCardID"].ToString().Trim(), Convert.ToDecimal(txtBonus1.Text), transNewid, Convert.ToDateTime(txtDate1.Text), Convert.ToDecimal(txtAmt1.Text), Convert.ToInt32(sessionUser.UserID), shopID, txtReceiptid1.Text, Convert.ToDateTime(datetime));
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '更新成功! 交易号为:" + transNewid + "'", true);
                    txtAmt1.Text = "";
                    txtBonus1.Text = "";
                    txtCardId1.Text = "";
                    txtDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtReceiptid1.Text = "";
                    txtShopCode1.Text = "";

                }
                else
                {

                    HandBonusPO.UpdateOrInsertBouns(0, membid, ds.Tables[0].Rows[0]["MembCardID"].ToString().Trim(), Convert.ToDecimal(txtBonus1.Text), transNewid, Convert.ToDateTime(txtDate1.Text), Convert.ToDecimal(txtAmt1.Text), Convert.ToInt32(sessionUser.UserID), shopID, txtReceiptid1.Text, Convert.ToDateTime(datetime));
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '补登成功! 交易号为:" + transNewid + "'", true);
                    txtAmt1.Text = "";
                    txtBonus1.Text = "";
                    txtCardId1.Text = "";
                    txtDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    txtReceiptid1.Text = "";
                    txtShopCode1.Text = "";
                }
            }
        }
        else
        {
            
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '无此会员卡，请核对!'", true);
        
        }

    }

    //获取积分率
    private void GetBonusPer()
    {
        int shopid = Convert.ToInt32(ViewState["shopID"]);
        DataSet ds = BonusGPPO.GetBonusGpPerByShopID(shopid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            bonusPer = Convert.ToDecimal(ds.Tables[0].Rows[0]["BonusGpPer"].ToString());
        }
        else
        {
            return;
        }
        
    }

    //验证非POS小票保存的信息 
    private Boolean ValidatorNoPOSPage()
    {
        //DataSet ds = QueryAssociatorPO.GetLCustByCondition("MembCard.MembCardID = '" +txtCardId1.Text.Trim()+"'");
        //int membid = Convert.ToInt32(ds.Tables[0].Rows[0]["MembID"]);
        int shopid = Convert.ToInt32(ViewState["shopID"]);
        decimal amt = Convert.ToDecimal(txtAmt1.Text);
        string receiptid = txtReceiptid1.Text;
        string whrStr = " AND Purhist.NetAmt ='" + amt + "' AND Purhist.Receiptid = '" + receiptid + "' AND Purhist.ShopID = '"+shopid+"'";
        DataSet ds1 = PurhistPO.GetPurhistByWhere(whrStr);

        if (ds1.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    #region POS小票与非POS小票的选择
    //选择POS小票
    protected void POSReceiptChecked(object sender, EventArgs e)
    {
        StartPOSReceipt();
        StopNoPOSReceipt();
    }

    //选择非POS小票
    protected void NoPOSReceiptChecked(object sender, EventArgs e)
    {
        StartNoPOSReceipt();
        StopPOSReceipt();
    }
    #endregion

    //隐藏的按钮
    protected void hiddenBtn_Click(object sender, EventArgs e)
    {
         SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
         int shopid = Convert.ToInt32(ViewState["shopID"]);
        DataSet ds2 = PurhistPO.GetPurhistByWhere(" AND Purhist.ReceiptId = '" + txtReceiptid1.Text + "' AND Purhist.ShopID = '" + shopid + "'");
        string transid = ds2.Tables[0].Rows[0]["TransID"].ToString();


        string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        int shopID = Convert.ToInt32(ViewState["shopID"]);
        DataSet ds = QueryAssociatorPO.GetLCustByCondition("MembCard.MembCardID = '" + txtCardId1.Text.Trim() + "'");

        DataSet ds1 = BonusPO.GetBonusByMembID(Convert.ToInt32(ds.Tables[0].Rows[0]["MembId"]));

        if (ds1.Tables[0].Rows.Count > 0)
        {
            HandBonusPO.UpdateOrInsertBouns(1, Convert.ToInt32(ds.Tables[0].Rows[0]["MembID"]), ds.Tables[0].Rows[0]["MembCardID"].ToString().Trim(), Convert.ToDecimal(txtBonus1.Text), transid, Convert.ToDateTime(txtDate1.Text), Convert.ToDecimal("0"), Convert.ToInt32(sessionUser.UserID), shopID, txtReceiptid1.Text, Convert.ToDateTime(datetime));
        }
        else
        {
            HandBonusPO.UpdateOrInsertBouns(0, Convert.ToInt32(ds.Tables[0].Rows[0]["MembID"]), ds.Tables[0].Rows[0]["MembCardID"].ToString().Trim(), Convert.ToDecimal(txtBonus1.Text), transid, Convert.ToDateTime(txtDate1.Text), Convert.ToDecimal("0"), Convert.ToInt32(sessionUser.UserID), shopID, txtReceiptid1.Text, Convert.ToDateTime(datetime));
        }

        txtAmt1.Text = "";
        txtBonus1.Text = "";
        txtCardId1.Text = "";
        txtDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtReceiptid1.Text = "";
        txtShopCode1.Text = "";
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '补登成功!'", true);
        
    }

    //非POS小票自动计算积分
    protected void BlurAmt1Change(object sender, EventArgs e)
    {
        if (txtAmt1.Text.Trim() != "")
        {
            GetBonusPer();
            txtBonus1.Text = decimal.Truncate(Convert.ToDecimal(txtAmt1.Text.Trim()) * bonusPer) + "";

        }
        else
        {
            txtBonus1.Text = "0";
        
        }
    }
    protected void hidnBtn_Click(object sender, EventArgs e)
    {
        string[] values = Regex.Split(flagHidn.Value, ",");
        int count = values.Length;
        HandBonusPO.UpdateOrInsertBouns(1, Convert.ToInt32(values[0]),values[1].ToString(),Convert.ToDecimal(values[2]),values[3].ToString(),Convert.ToDateTime(values[4]),Convert.ToDecimal(values[5]),Convert.ToInt32(values[6]),Convert.ToInt32(values[7]),values[8].ToString(),Convert.ToDateTime(values[9]));
        txtAmt.Text = "";
        txtBonus.Text = "";
        txtCardID.Text = "";
        txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        txtRecepid.Text = "";
        txtShopCode.Text = "";
        txtTrans.Text = "";
        txtBonus.Enabled = true;
        txtDate.Enabled = true;
        txtInputText.Visible = false;
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '积分补登成功!'", true);
    }
}
