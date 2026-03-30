using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.Page;
using Sell;
using Lease.ConShop;
using BaseInfo.User;

public partial class Sell_ManualReceipt : BasePage 
{
    public string strFresh;
    #region 定义
    public string baseInfo;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        this.txtTransId.Attributes.Add("onkeydown", "textleave()");
        this.txtPosId.Attributes.Add("onkeydown", "textleave()");
        this.txtBatchId.Attributes.Add ("onkeydown", "textleave()");
        this.txtSalesAmt.Attributes.Add("onkeydown", "textleave()");

        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ManualReceipt");
            txtShopCode.Attributes.Add("onclick", "ShowTree()");
            BindDropList();
        }
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        btnSave.Attributes.Add("onclick", "return CheckData()");
    }

    private void BindDropList()
    {
        BaseBO baseB1 = new BaseBO();
        string sql1 = "  SELECT MediaMNo,MediaMDesc FROM MediaM";
        DataSet myDS1 = baseB1.QueryDataSet(sql1);
        int count1 = myDS1.Tables[0].Rows.Count;
        ddlMediaID.Items.Clear();
        for (int i = 0; i < count1; i++)
        {
            //绑定商铺号

            ddlMediaID.Items.Add(new ListItem(myDS1.Tables[0].Rows[i]["MediaMNo"].ToString() + " " + myDS1.Tables[0].Rows[i]["MediaMDesc"].ToString(), myDS1.Tables[0].Rows[i]["MediaMDesc"].ToString()));
        }
        ddlSkuId.Items.Add("");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //首先验证：１．ＲｅｃｅｉｐｔＩＤ该ｐｏｓ机当天是否存在　
        //POSID　４位数字
        if (!ExistColumn("ReceiptID", "TransSku", txtTransId.Text.Trim()))
        {
            TransSku transSku = new TransSku();
            TransSkuMedia transSkuMedia = new TransSkuMedia();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            BaseTrans baseTrans = new BaseTrans();
            baseTrans.BeginTrans();
            BaseBO baseBO = new BaseBO();
            DataTable tempTb = new DataTable();
            string sql = "  SELECT " +
                            " ISNULL(ConShop.ShopID,0) as ShopID,ConShop.ShopName," +
                            " ISNULL(ConShop.BuildingId,0) as BuildingId, BuildingName," +
                            " ISNULL(ConShop.FloorId,0) as FloorId,FloorName," +
                            " ISNULL(ConShop.LocationId,0) as LocationId,LocationName," +
                            " ISNULL(ConShop.AreaId,0) as AreaId,AreaName," +
                            " ISNULL(ConShop.BrandId,0) as BrandId,BrandName," +
                            " ISNULL(TradeRelation.TradeId,0) as TradeId,TradeName,ConShop.StoreID,ConShop.MainLocationID" +
                            " FROM ConShop LEFT JOIN Building On (ConShop.BuildingId=Building.BuildingId)" +
                            " LEFT JOIN Floors On (ConShop.FloorId=Floors.FloorId)" +
                            " LEFT JOIN Location On (ConShop.LocationId=Location.LocationId)" +
                            " LEFT JOIN Area On (ConShop.AreaId=Area.AreaId)" +
                            " LEFT JOIN ConShopBrand On (ConShop.BrandId=ConShopBrand.BrandId)" +
                            " LEFT JOIN Contract On(ConShop.ContractId=Contract.ContractId) " +
                            " LEFT JOIN TradeRelation On(TradeRelation.TradeId=Contract.TradeId) " +
                            " WHERE ShopId=" + Convert.ToInt32(ViewState["shopID"]);
            tempTb = baseBO.QueryDataSet(sql).Tables[0];

            if (txtShopCode.Text != "" && ddlSkuId.Text!="" && ddlMediaID.Text !="")
            {
                transSku.ShopID = Convert.ToInt32(ViewState["shopID"]);
                transSku.PosID = txtPosId.Text.Trim();
                //transSku.ReceiptID = txtTransId.Text.Trim(); //不能大于9999 4位数字
                transSku.TransID = tempTb.Rows[0]["StoreID"].ToString().Trim() + txtPosId.Text.Trim() + GetSixDate(txtBizDate.Text.Trim()) + txtTransId.Text.Trim();
                // transSku.TransID = StoreID + POS号 + 日期（100304） + ReceiptID
                transSku.SkuID =  ddlSkuId.SelectedItem.Text.Substring(0, ddlSkuId.SelectedItem.Text.IndexOf(" "));
                transSku.SkuDesc = ddlSkuId.SelectedItem.Value;
                transSku.PaidAmt = Convert.ToDecimal(txtSalesAmt.Text.Trim());
                transSku.PayAmt = Convert.ToDecimal(txtSalesAmt.Text.Trim());
                transSku.NewPrice = Convert.ToDecimal(txtSalesAmt.Text.Trim());
                transSku.BizDate = Convert.ToDateTime(txtBizDate.Text.Trim());
                transSku.TransTime = Convert.ToDateTime(txtBizDate.Text.Trim());
                transSku.BatchID = txtBatchId.Text.Trim();
                transSku.ShopName = txtShopName.Text.Trim() ;
                transSku.DataSource = TransSku.DATASOURCE_WORK ;
                transSku.BuildingID = Convert.ToInt32(tempTb.Rows[0]["BuildingID"]);
                transSku.BuildingName = tempTb.Rows[0]["BuildingName"].ToString();
                transSku.FloorID = Convert.ToInt32(tempTb.Rows[0]["FloorID"]);
                transSku.FloorName = tempTb.Rows[0]["FloorName"].ToString();
                transSku.LocationID = Convert.ToInt32(tempTb.Rows[0]["LocationID"]);
                transSku.LocationName = tempTb.Rows[0]["LocationName"].ToString();
                transSku.AreaID = Convert.ToInt32(tempTb.Rows[0]["AreaID"]);
                transSku.AreaName = tempTb.Rows[0]["AreaName"].ToString();
                transSku.BrandID = Convert.ToInt32(tempTb.Rows[0]["BrandID"]);
                transSku.BrandName = tempTb.Rows[0]["BrandName"].ToString();              
                transSku.Trade2ID = Convert.ToInt32(tempTb.Rows[0]["TradeId"]);
                transSku.Trade2Name = tempTb.Rows[0]["TradeName"].ToString();
                transSku.Qty = 1;
                if (this.txtTransId.Text.Trim() == "")
                {
                    transSku.ReceiptID = "0000";
                }
                else
                {
                    transSku.ReceiptID = this.txtTransId.Text.Trim();
                }
                transSku.UserID = objSessionUser.UserID;
                transSku.StoreID = Convert.ToInt32(tempTb.Rows[0]["StoreID"]);
                transSku.MainLocationID = Convert.ToInt32( tempTb.Rows[0]["MainLocationID"]);
                //交易金额流水表

                transSkuMedia.ShopID = Convert.ToInt32(ViewState["shopID"]);
                transSkuMedia.PosID = txtPosId.Text.Trim();
                //同ＴｒａｎｓＳＫＵ相同的逻辑　
                transSku.TransID = tempTb.Rows[0]["StoreID"].ToString().Trim() + txtPosId.Text.Trim() + GetSixDate(txtBizDate.Text.Trim()) + txtTransId.Text.Trim();
                // transSku.TransID = StoreID + POS号 + 日期（100304） + ReceiptID
                transSkuMedia.SkuID = ddlSkuId.SelectedItem.Text.Substring(0, ddlSkuId.SelectedItem.Text.IndexOf(" "));
                transSkuMedia.SkuDesc = ddlSkuId.SelectedItem.Value;
                transSkuMedia.PaidAmt = Convert.ToDecimal(txtSalesAmt.Text.Trim());
                transSkuMedia.BizDate = Convert.ToDateTime(txtBizDate.Text.Trim());
                transSkuMedia.TransTime = Convert.ToDateTime(txtBizDate.Text.Trim());
                transSkuMedia.BatchID = txtBatchId.Text.Trim();
                transSkuMedia.ShopName = txtShopName.Text.Trim();
                transSkuMedia.DataSource = TransSku.DATASOURCE_WORK ;
                transSkuMedia.MediaMNo = Convert.ToInt32(ddlMediaID.SelectedItem.Text.Substring(0, ddlMediaID.SelectedItem.Text.IndexOf(" ")));
                transSkuMedia.MediaMDesc = ddlMediaID.SelectedItem.Value;
                if (this.txtTransId.Text.Trim() == "")
                {
                    transSkuMedia.ReceiptID = "0000"; 
                }
                else
                {
                    transSkuMedia.ReceiptID = this.txtTransId.Text.Trim();
                }
                transSkuMedia.UserID = objSessionUser.UserID;
                transSkuMedia.StoreID = Convert.ToInt32(tempTb.Rows[0]["StoreID"]);
                transSkuMedia.MainLocationID = Convert.ToInt32(tempTb.Rows[0]["MainLocationID"]);
                try
                {
                    baseTrans.Insert(transSku);
                    baseTrans.Insert(transSkuMedia);
                    //执行日报\月报生成
                    string strUpdateDaySql = "Exec SPMI_ComputerShopDaySales " + tempTb.Rows[0]["StoreID"].ToString() + ",'" + Convert.ToDateTime(txtBizDate.Text.Trim()).ToString("yyyy-MM-dd") + "'"; 
                    baseTrans.ExecuteUpdate(strUpdateDaySql);
                    string strMthSql = "Exec SPMI_ComputerShopMonthSales " + tempTb.Rows[0]["StoreID"].ToString() + ",'" + Convert.ToDateTime(txtBizDate.Text.Trim()).ToString("yyyy-MM-01") + "'";
                    baseTrans.ExecuteUpdate(strMthSql);
                    baseTrans.Commit();                    
                    txtNULL();

                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                }
                catch (Exception ex)
                {
                    baseTrans.Rollback();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
                BindDropList();
            }
        }
        else
        {
            txtTransId.Text = "";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
        }

        
    }

    private string GetSixDate(string Date)
    {
        string[] parts = Date.Split('-');
        string SixStr = parts[0].Substring(2) + parts[1] + parts[2];
        return SixStr;
    }

    protected void BtnDel_Click(object sender, EventArgs e)
    {
            BaseTrans baseTrans = new BaseTrans();
            baseTrans.BeginTrans();

            if (txtShopCode.Text != "")
            {

                string strwhere = "ShopID = '" + ViewState["shopID"].ToString() + "'";
                if (txtPosId.Text != "")
                    strwhere = strwhere + " AND PosId = '" + txtPosId.Text + "'";
                if (txtTransId.Text != "")
                    strwhere = strwhere + " AND TransID = '" + txtTransId.Text + "'";
                if (txtBatchId.Text != "")
                    strwhere = strwhere + " AND BatchID = '" + txtBatchId.Text + "'";

                baseTrans.WhereClause = strwhere;
                try
                {

                    baseTrans.Delete(new TransSku());
                    baseTrans.Delete(new TransSkuMedia());
                    baseTrans.Commit();
                    txtNULL();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                }
                catch (Exception ex)
                {
                    baseTrans.Rollback();
                    Response.Write(ex.ToString());
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
            }
            btnSave.Enabled = true;
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        txtNULL();
        btnSave.Enabled = true;
    }
    private void txtNULL()
    {
        txtBizDate.Text = "";
        txtPosId.Text = "";
        txtBatchId.Text = "";
        txtSalesAmt.Text = "";
        txtTransId.Text = "";
        txtShopName.Text = "";
        txtShopCode.Text = "";
        ddlSkuId.SelectedItem.Text = "";
        ddlMediaID.SelectedItem.Text = "";
        BindDropList();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        DataTable tempTb = new DataTable();
        if (txtTransId.Text != "" )
        {
            string sql = "SELECT ShopId,(Select ShopCode from ConShop where ShopId=TransSkuMedia.ShopId) as ShopCode,ShopName,BizDate,PosId,BatchId,TransId,Skuid,SkuDesc,MediaMNo,MediaMDesc,PaidAmt FROM TransSkuMedia where DataSource='3'" +
                            " AND TransId='" +txtTransId.Text + "'";

            tempTb = baseBO.QueryDataSet(sql).Tables[0];

            if (!tempTb.Rows.Count.Equals(0))
            {
                txtBizDate.Text =Convert.ToDateTime(tempTb.Rows[0]["BizDate"]).ToString("yyyy-MM-dd");
                txtPosId.Text = tempTb.Rows[0]["PosId"].ToString();
                txtBatchId.Text = tempTb.Rows[0]["BatchId"].ToString();
                ddlSkuId.SelectedItem.Text = tempTb.Rows[0]["Skuid"].ToString() + " " + tempTb.Rows[0]["SkuDesc"].ToString();
                ddlSkuId.SelectedItem.Value = tempTb.Rows[0]["Skuid"].ToString();
                txtSalesAmt.Text = tempTb.Rows[0]["PaidAmt"].ToString();
                txtTransId.Text = tempTb.Rows[0]["TransId"].ToString();
                txtShopName.Text = tempTb.Rows[0]["ShopName"].ToString();
                ddlMediaID.SelectedItem.Text = tempTb.Rows[0]["MediaMNo"].ToString()+" "+tempTb.Rows[0]["MediaMDesc"].ToString();
                ddlMediaID.SelectedItem.Value = tempTb.Rows[0]["MediaMNo"].ToString();
                txtShopCode.Text = tempTb.Rows[0]["ShopCode"].ToString() + " " + tempTb.Rows[0]["ShopName"].ToString();
                ViewState["shopID"] = tempTb.Rows[0]["ShopId"].ToString();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ' '", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "'", true);
                txtNULL();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + Label7.Text+" " + (String)GetGlobalResourceObject("BaseInfo", "BankCard_NotSelect") + "'", true);
            txtNULL();
        }
    }

    private bool ExistColumn(string Column, string tableName, string ColumnValue)
    {
        BaseBO baseBO = new BaseBO();
        DataTable tempTb = new DataTable();
        string sql = "Select " + Column + " FROM " + tableName + " WHERE " + Column + " ='" + ColumnValue + "' and bizdate='" + txtBizDate.Text.Trim() + "' and posid='" + txtPosId.Text.Trim() + "'";
        tempTb = baseBO.QueryDataSet(sql).Tables[0];

        if (tempTb.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            txtShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
            txtShopName.Text = ds.Tables[0].Rows[0]["ShopName"].ToString();
            BaseBO baseB1 = new BaseBO();
            string sql1 = "  SELECT SkuId,SkuDesc FROM SkuMaster Where Status='V' And TenantId='" + ViewState["shopID"].ToString().Trim() + "'" +
                           " Order by SkuId";
            DataSet myDS1 = baseB1.QueryDataSet(sql1);
            int count1 = myDS1.Tables[0].Rows.Count;
            ddlSkuId.Items.Clear();
            ddlSkuId.Items.Add("");
            for (int i = 0; i < count1; i++)
            {
                //绑定商铺号

                ddlSkuId.Items.Add(new ListItem(myDS1.Tables[0].Rows[i]["SkuId"].ToString() + " " + myDS1.Tables[0].Rows[i]["SkuDesc"].ToString(), myDS1.Tables[0].Rows[i]["SkuDesc"].ToString()));
            }
        }
    }
}
