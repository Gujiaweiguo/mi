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
using Base.DB;
using Lease.Brand;
using Base.Biz;
using RentableArea;
using Lease.ConShop;
using BaseInfo;
using BaseInfo.User;
using Lease.PotCust;
using System.Drawing;
public partial class Lease_PotCustomer_CustBrand :BasePage
{
    /// <summary>
    /// 经营品牌 Add by lcp at 2009-3-19
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetControlPro();//为控件添加属性
        if (!this.IsPostBack)
        {
            this.GetCustIDFromCookie();//从cookie中得到客户编号
            ViewState["currentCount"] = "1";
            this.BindData(1);//绑定GridView 数据
            this.BindOperateType();//绑定品牌经营方式
            this.BindSex();//绑定性别客层
            this.BindClientLevel();//绑定
            if (Request["browse"] != null && Request["browse"].ToString() == "yes")//如果进行查询，则保存按钮、修改按钮、取消按钮不可见
            {
                this.SetControlLock();
            }
        }
    }
    /// <summary>
    /// 设置控件不可写
    /// </summary>
    private void SetControlLock()
    {
        this.txtNote.Enabled = false;
        this.txtConsumerAge.Enabled = false;
        this.txtAvgAmt.Enabled = false;
        this.txtNote.Enabled = false;
        this.hidTradeID.Enabled = false;
        this.hidBrandID.Enabled = false;
        this.txtTrade.Enabled = false;
        this.txtBrandName.Enabled = false;
        this.txtPriceRange.Enabled = false;
        this.btnAdd.Visible = false;
        this.btnEdit.Visible = false;
        this.btnCancel.Visible = false;
        this.ddlSex.Enabled = false;
        this.ddlOperateTypeId.Enabled = false;
        this.ddlClientLevel.Enabled = false;
    }
    /// <summary>
    /// 从cookie中得到客户编号
    /// </summary>
    private void GetCustIDFromCookie()
    {
        ViewState["custID"] = "0";
        try
        {
            if (Request.Cookies["Info1"] != null)//从cookie中得到客户的编号
            {
                ViewState["custID"] = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
            }
        }
        catch
        {
            ViewState["custID"] = "0";
        }
    }
    /// <summary>
    /// 为按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        txtTrade.Attributes.Add("onclick", "ShowTradeTree()");
        this.txtBrandName.Attributes.Add("onclick", "ShowBrandTree()");
        this.btnAdd.Attributes.Add("onclick", "return allTextBoxValidator()");
        this.btnEdit.Attributes.Add("onclick", "return allTextBoxValidator()");
        this.txtTrade.Attributes.Add("onblur", "TextIsNotNull(txtTrade,ImgTrade)");
        this.txtBrandName.Attributes.Add("onblur", "TextIsNotNull(txtBrandName,ImgBrandName)");
    }
    /// <summary>
    /// 绑定品牌经营方式
    /// </summary>
    private void BindOperateType()
    {
        BaseInfo.BaseCommon.BindDropDownList("select OperateTypeID,OperateName from BrandOperateType where status=1", "OperateTypeID", "OperateName", this.ddlOperateTypeId);
    }
    /// <summary>
    /// 绑定性别
    /// </summary>
    private void BindSex()
    {
        this.ddlSex.Items.Add(new ListItem("----", "----"));
        this.ddlSex.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "TpUse_lblSexMan")));
        this.ddlSex.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "TpUse_lblSexWoman")));
    }
    /// <summary>
    /// 绑定客层
    /// </summary>
    private void BindClientLevel()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "status=1";
        ClientLevel objClientlevle = new ClientLevel();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, objClientlevle, "ClientLevelId", "ClientLevelName", this.ddlClientLevel);
    }
    /// <summary>
    /// 绑定GridView列表
    /// </summary>
    private void BindData(int iCurrentPage)
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "CustID=" + ViewState["custID"].ToString();
        //BaseInfo.BaseCommon.BindGridView(objBaseBo, new CustBrand(), 11, iCurrentPage, this.btnBack, this.btnNext, this.GrdCustBrand);
        BaseInfo.BaseCommon.BindGridView(objBaseBo, new CustBrand(),this.GrdCustBrand);
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private bool SaveAdd()
    {
        BaseBO objBaseBo = new BaseBO();
        CustBrand objCustBrand = new CustBrand();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objCustBrand.CustBrandId = Base.BaseApp.GetCustumerID("CustBrand", "CustBrandId");
        objCustBrand.CustId = Int32.Parse(ViewState["custID"].ToString());
        try { objCustBrand.TradeId = Int32.Parse(this.hidTradeID.Text.Trim()); }//经营类别
        catch { objCustBrand.TradeId = 0; }
        try { objCustBrand.BrandId = Int32.Parse(this.hidBrandID.Text.Trim()); }//品牌名称
        catch { objCustBrand.BrandId = 0; }
        try { objCustBrand.OperateTypeId = Int32.Parse(this.ddlOperateTypeId.SelectedValue.ToString()); }//品牌经营方式
        catch { objCustBrand.OperateTypeId = 0; }
        objCustBrand.ConsumerSex = this.ddlSex.SelectedItem.Text.Trim();
        objCustBrand.ConsumerAge = this.txtConsumerAge.Text.Trim();
        try { objCustBrand.AvgAmt = decimal.Parse(this.txtAvgAmt.Text.Trim()); }
        catch { objCustBrand.AvgAmt = 0; }
        try { objCustBrand.ClientLevelId = Int32.Parse(this.ddlClientLevel.SelectedValue); }//
        catch { objCustBrand.ClientLevelId = 0; }
        objCustBrand.PriceRange = this.txtPriceRange.Text.Trim();//
        objCustBrand.Note = this.txtNote.Text.Trim();
        objCustBrand.CreateUserId = objSessionUser.CreateUserID;
        objCustBrand.CreateTime = DateTime.Now.Date;
        objCustBrand.OprRoleID = objSessionUser.OprRoleID;
        objCustBrand.OprDeptID = objSessionUser.OprDeptID;
        if (objBaseBo.Insert(objCustBrand) <= 0)
            return false;
        else
            return true;
    }
    /// <summary>
    /// 新增保存事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
       if(this.SaveAdd())
           ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        else
           ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
       this.BindData(1);//从新绑定
       this.ClearTextValue();//将输入框清空
    }
    /// <summary>
    /// 更新事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["CustBrandID"] == null || ViewState["CustBrandID"].ToString() == "")
            return;
        if(this.SaveUpdate(ViewState["CustBrandID"].ToString()))
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
        this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));//绑定
        this.ClearTextValue();
        this.btnAdd.Enabled = true;
        this.btnEdit.Enabled = false;
        ViewState["CustBrandID"] = "";
    }
    /// <summary>
    /// 更新方法
    /// </summary>
    /// <param name="strBrandID"></param>
    /// <returns></returns>
    private bool SaveUpdate(string strBrandID)
    {
        BaseBO objBaseBo = new BaseBO();
        CustBrand objCustBrand = new CustBrand();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objBaseBo.WhereClause = "CustBrandId=" + strBrandID;
        objCustBrand.CustId = Int32.Parse(ViewState["custID"].ToString());
        try { objCustBrand.TradeId = Int32.Parse(this.hidTradeID.Text.Trim()); }
        catch { objCustBrand.TradeId = 0; }
        try { objCustBrand.BrandId = Int32.Parse(this.hidBrandID.Text.Trim()); }
        catch { objCustBrand.BrandId = 0; }
        try { objCustBrand.OperateTypeId = Int32.Parse(this.ddlOperateTypeId.SelectedValue.ToString()); }
        catch { objCustBrand.OperateTypeId = 0; }
        objCustBrand.ConsumerSex = this.ddlSex.SelectedItem.Text.Trim();
        objCustBrand.ConsumerAge = this.txtConsumerAge.Text.Trim();
        try { objCustBrand.AvgAmt = decimal.Parse(this.txtAvgAmt.Text.Trim()); }
        catch { objCustBrand.AvgAmt = 0; }
        objCustBrand.Note = this.txtNote.Text.Trim();
        try { objCustBrand.ClientLevelId = Int32.Parse(this.ddlClientLevel.SelectedValue); }//
        catch { objCustBrand.ClientLevelId = 0; }
        objCustBrand.PriceRange = this.txtPriceRange.Text.Trim();//
        objCustBrand.CreateUserId = objSessionUser.CreateUserID;
        objCustBrand.CreateTime = DateTime.Now.Date;
        objCustBrand.ModifyUserId = objSessionUser.ModifyUserID;
        objCustBrand.ModifyTime = DateTime.Now.Date;
        objCustBrand.OprRoleID = objSessionUser.OprRoleID;
        objCustBrand.OprDeptID = objSessionUser.OprDeptID;
        if (objBaseBo.Update(objCustBrand) <= 0)
            return false;
        else
            return true;
    }
    /// <summary>
    /// 清空TextBox与DropDownList
    /// </summary>
    private void ClearTextValue()
    {
        //this.BindOperateType();//绑定品牌经营方式
        //this.BindSex();//绑定性别
        this.txtNote.Text = "";
        this.txtConsumerAge.Text = "";
        this.txtAvgAmt.Text = "";
        this.txtNote.Text = "";
        this.hidTradeID.Text = "";
        this.hidBrandID.Text = "";
        this.txtTrade.Text = "";
        this.txtBrandName.Text = "";
        this.txtPriceRange.Text = "";
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/Customer/CustBrand.aspx");
    }
    
    /// <summary>
    /// 点击GridView一行绑定数据到text
    /// </summary>
    /// <param name="strCustBrandID"></param>
    private void BindDataFrmGrdCust(string strCustBrandID)
    {
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select CustBrandId,CustId,TradeId,BrandId,(select tradename from traderelation where tradeid=CustBrand.tradeid) as TradeName,(select brandname from ConShopBrand where brandid=CustBrand.brandid) as BrandName,OperateTypeId,ConsumerSex,ConsumerAge,AvgAmt,Note,ClientLevelId,PriceRange from CustBrand where CustBrandId=" + strCustBrandID);
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            this.hidTradeID.Text = ds.Tables[0].Rows[0]["TradeId"].ToString();
            this.hidBrandID.Text = ds.Tables[0].Rows[0]["BrandId"].ToString();
            this.txtTrade.Text = ds.Tables[0].Rows[0]["TradeName"].ToString();
            this.txtBrandName.Text = ds.Tables[0].Rows[0]["BrandName"].ToString();
            this.ddlOperateTypeId.SelectedValue = ds.Tables[0].Rows[0]["OperateTypeId"].ToString();
            this.ddlSex.SelectedValue = ds.Tables[0].Rows[0]["ConsumerSex"].ToString();
            this.txtConsumerAge.Text = ds.Tables[0].Rows[0]["ConsumerAge"].ToString();
            this.txtAvgAmt.Text = ds.Tables[0].Rows[0]["AvgAmt"].ToString();
            this.txtNote.Text = ds.Tables[0].Rows[0]["Note"].ToString();
            this.txtPriceRange.Text = ds.Tables[0].Rows[0]["PriceRange"].ToString();
            //this.ddlClientLevel.SelectedValue = ds.Tables[0].Rows[0]["ClientLevelId"].ToString();
            try { this.ddlClientLevel.SelectedValue = ds.Tables[0].Rows[0]["ClientLevelId"].ToString(); }
            catch { this.ddlClientLevel.SelectedIndex = 0; }
        }
    }
    protected void GrdCustBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CustBrandID"] = this.GrdCustBrand.SelectedRow.Cells[0].Text.Trim();
        this.btnAdd.Enabled = false;
        this.btnEdit.Enabled = true;
        this.BindDataFrmGrdCust(this.GrdCustBrand.SelectedRow.Cells[0].Text.Trim());
        this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));// 绑定
    }
    protected void GrdCustBrand_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[3].Text = "";
            }
        }
    }
    /// <summary>
    /// 点击经营类别输入框，弹出页面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBindDealType_Click(object sender, EventArgs e)
    {
        SelectTradeID();
        this.BindData(1);
    }
    /// <summary>
    /// 选择经营类别
    /// </summary>
    private void SelectTradeID()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "TradeID = " + Convert.ToInt32(hidTradeID.Text);
        Resultset rs = objBaseBo.Query(new TradeRelation());
        if (rs.Count == 1)
        {
            TradeRelation tradeRelation = rs.Dequeue() as TradeRelation;
            this.txtTrade.Text = tradeRelation.TradeName;
        }
        objBaseBo.WhereClause = "";
    }
    /// <summary>
    /// 点击品牌名称，弹出页面
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDellBrand_Click(object sender, EventArgs e)
    {
        this.SelectBrandID();
        this.BindData(1);
    }
    /// <summary>
    /// 选择品牌名称
    /// </summary>
    private void SelectBrandID()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "BrandId = " + Convert.ToInt32(hidBrandID.Text);
        Resultset rs = objBaseBo.Query(new ConShopBrand());
        if (rs.Count == 1)
        {
            ConShopBrand objConShopBrand = rs.Dequeue() as ConShopBrand;
            this.txtBrandName.Text = objConShopBrand.BrandName;
        }
        objBaseBo.WhereClause = "";
    }
    protected void GrdCustBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        
        this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));//绑定
        foreach (GridViewRow grv in this.GrdCustBrand.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
