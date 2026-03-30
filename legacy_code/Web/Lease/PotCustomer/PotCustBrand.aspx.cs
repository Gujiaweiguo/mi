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
using Base.Util;
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
            txtTrade.Attributes.Add("onblur", "TextIsNotNull(txtTrade,ImgTrade)");
            this.BindData();//绑定GridView 数据
            this.BindOperateType();//绑定品牌经营方式
            this.BindSex();//绑定性别
            this.BindClientLevel();//绑定客层
            if (Request["look"] != null)
            {
                if (Request["look"] == "yes")
                {
                    this.btnCancel.Visible = false;
                    this.btnAdd.Visible = false;
                    this.btnEdit.Visible = false;
                    this.ddlClientLevel.Enabled = false;
                    this.ddlSex.Enabled = false;
                    this.ddlOperateTypeId.Enabled = false;
                }
            }
        }
    }
    /// <summary>
    /// 从cookie中得到客户编号
    /// </summary>
    private void GetCustIDFromCookie()
    {
        ViewState["CustumerID"] = "0";
        try
        {
            if (Request.Cookies["Custumer"].Values["CustumerID"] != "")//从cookie中得到客户的编号
            {
                ViewState["CustumerID"] = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            }
        }
        catch
        {
            ViewState["CustumerID"] = "0";
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
    private void BindData()
    {
        //BaseBO objBaseBo = new BaseBO();
        //objBaseBo.WhereClause = "CustID=" + ViewState["CustumerID"].ToString();
        //BaseInfo.BaseCommon.BindGridView(objBaseBo, new PotCustBrand(), 11, iCurrentPage, this.btnBack, this.btnNext, this.GrdCustBrand);
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = "CustID=" + ViewState["CustumerID"].ToString();

        DataTable dt = baseBO.QueryDataSet(new PotCustBrand()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCustBrand.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustBrand.DataSource = pds;
            GrdCustBrand.DataBind();
        }
        else
        {
            GrdCustBrand.DataSource = pds;
            GrdCustBrand.DataBind();
            spareRow = GrdCustBrand.Rows.Count;
            for (int i = 0; i < GrdCustBrand.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            pds.DataSource = dt.DefaultView;
            GrdCustBrand.DataSource = pds;
            GrdCustBrand.DataBind();
        }
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private bool SaveAdd()
    {
        try
        {
            //if (Request.Cookies["Custumer"] != null)
            if (ViewState["CustumerID"].ToString()!="0")
            {
                BaseBO objBaseBo = new BaseBO();
                PotCustBrand objPotCustBrand = new PotCustBrand();
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objPotCustBrand.CustBrandId = Base.BaseApp.GetID("PotCustBrand", "CustBrandId");
                objPotCustBrand.CustId = Int32.Parse(ViewState["CustumerID"].ToString());
                try { objPotCustBrand.TradeId = Int32.Parse(this.hidTradeID.Text.Trim()); }//经营类别
                catch { objPotCustBrand.TradeId = 0; }
                try { objPotCustBrand.BrandId = Int32.Parse(this.hidBrandID.Text.Trim()); }//品牌名称
                catch { objPotCustBrand.BrandId = 0; }
                try { objPotCustBrand.OperateTypeId = Int32.Parse(this.ddlOperateTypeId.SelectedValue.ToString()); }//品牌经营方式
                catch { objPotCustBrand.OperateTypeId = 0; }
                objPotCustBrand.ConsumerSex = this.ddlSex.SelectedItem.Text.Trim();
                objPotCustBrand.ConsumerAge = this.txtConsumerAge.Text.Trim();
                try { objPotCustBrand.AvgAmt = decimal.Parse(this.txtAvgAmt.Text.Trim()); }
                catch { objPotCustBrand.AvgAmt = 0; }
                objPotCustBrand.Note = this.txtNote.Text.Trim();
                try { objPotCustBrand.ClientLevelId =Int32.Parse(this.ddlClientLevel.SelectedValue); }
                catch { objPotCustBrand.ClientLevelId = 0; }
                objPotCustBrand.PriceRange = this.txtPriceRange.Text.Trim();
                objPotCustBrand.CreateUserId = objSessionUser.CreateUserID;
                objPotCustBrand.CreateTime = DateTime.Now.Date;
                objPotCustBrand.ModifyUserId = objSessionUser.ModifyUserID;
                objPotCustBrand.ModifyTime = DateTime.Now.Date;
                objPotCustBrand.OprRoleID = objSessionUser.OprRoleID;
                objPotCustBrand.OprDeptID = objSessionUser.OprDeptID;
                if (objBaseBo.Insert(objPotCustBrand) <= 0)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.Log("添加经营品牌错误:", ex);
            return false;
        }
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
       this.BindData();//从新绑定
       this.ClearTextValue();//将输入框清空
       foreach (GridViewRow grv in GrdCustBrand.Rows)
       {
           grv.BackColor = Color.White;
       }
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
        this.BindData();//绑定
        this.ClearTextValue();
        this.btnAdd.Enabled = true;
        this.btnEdit.Enabled = false;
        ViewState["CustBrandID"] = "";
        foreach (GridViewRow grv in GrdCustBrand.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    /// <summary>
    /// 更新方法
    /// </summary>
    /// <param name="strBrandID"></param>
    /// <returns></returns>
    private bool SaveUpdate(string strBrandID)
    {
        BaseBO objBaseBo = new BaseBO();
        PotCustBrand objPotCustBrand = new PotCustBrand();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objBaseBo.WhereClause = "CustBrandId=" + strBrandID;
        objPotCustBrand.CustId = Int32.Parse(ViewState["CustumerID"].ToString());
        try { objPotCustBrand.TradeId = Int32.Parse(this.hidTradeID.Text.Trim()); }
        catch { objPotCustBrand.TradeId = 0; }
        try { objPotCustBrand.BrandId = Int32.Parse(this.hidBrandID.Text.Trim()); }
        catch { objPotCustBrand.BrandId = 0; }
        try { objPotCustBrand.OperateTypeId = Int32.Parse(this.ddlOperateTypeId.SelectedValue.ToString()); }
        catch { objPotCustBrand.OperateTypeId = 0; }
        objPotCustBrand.ConsumerSex = this.ddlSex.SelectedItem.Text.Trim();
        objPotCustBrand.ConsumerAge = this.txtConsumerAge.Text.Trim();
        try { objPotCustBrand.AvgAmt = decimal.Parse(this.txtAvgAmt.Text.Trim()); }
        catch { objPotCustBrand.AvgAmt = 0; }
        objPotCustBrand.Note = this.txtNote.Text.Trim();
        try { objPotCustBrand.ClientLevelId = Int32.Parse(this.ddlClientLevel.SelectedValue); }//客层
        catch { objPotCustBrand.ClientLevelId = 0; }
        objPotCustBrand.PriceRange = this.txtPriceRange.Text.Trim();//单品价格范围
        objPotCustBrand.CreateUserId = objSessionUser.CreateUserID;
        objPotCustBrand.CreateTime = DateTime.Now.Date;
        objPotCustBrand.ModifyUserId = objSessionUser.ModifyUserID;
        objPotCustBrand.ModifyTime = DateTime.Now.Date;
        objPotCustBrand.OprRoleID = objSessionUser.OprRoleID;
        objPotCustBrand.OprDeptID = objSessionUser.OprDeptID;
        try
        {
            if (objBaseBo.Update(objPotCustBrand) <= 0)
                return false;
            else
                return true;
        }
        catch (Exception ex)
        {
            Logger.Log("编辑经营品牌错误:", ex);
            return false;
        }
    }
    /// <summary>
    /// 清空TextBox与DropDownList
    /// </summary>
    private void ClearTextValue()
    {
        this.txtNote.Text = "";
        this.txtConsumerAge.Text = "";
        this.txtAvgAmt.Text = "";
        this.txtNote.Text = "";
        this.hidTradeID.Text = "";
        this.hidBrandID.Text = "";
        this.txtTrade.Text = "";
        this.txtBrandName.Text = "";
        this.txtPriceRange.Text = "";
        this.ImgTrade.Src = "../../App_Themes/Main/Images/must.gif";
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/PotCustomer/PotCustBrand.aspx");
    }
    ///// <summary>
    ///// 上一页
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
    //    this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));//绑定
    //}
    ///// <summary>
    ///// 下一页
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
    //    this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));// 绑定
    //}
    /// <summary>
    /// 点击GridView一行绑定数据到text
    /// </summary>
    /// <param name="strCustBrandID"></param>
    private void BindDataFrmGrdCust(string strCustBrandID)
    {
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select CustBrandId,CustId,TradeId,BrandId,(select tradename from traderelation where tradeid=PotCustBrand.tradeid) as TradeName,(select brandname from ConShopBrand where brandid=PotCustBrand.brandid) as BrandName,OperateTypeId,ConsumerSex,ConsumerAge,AvgAmt,Note,ClientLevelId,PriceRange from PotCustBrand where CustBrandId=" + strCustBrandID);
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
            try{this.ddlClientLevel.SelectedValue = ds.Tables[0].Rows[0]["ClientLevelId"].ToString();}
            catch{this.ddlClientLevel.SelectedIndex=0;}
        }
    }
    protected void GrdCustBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["CustBrandID"] = this.GrdCustBrand.SelectedRow.Cells[0].Text.Trim();
        this.btnAdd.Enabled = false;
        this.btnEdit.Enabled = true;
        this.BindDataFrmGrdCust(this.GrdCustBrand.SelectedRow.Cells[0].Text.Trim());
        this.BindData();// 绑定
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
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
        this.BindData();
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
            this.ImgTrade.Src = "../../App_Themes/Main/Images/pic_right.gif";
        }
        else
            this.ImgTrade.Src = "../../App_Themes/Main/Images/must.gif";
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
        this.BindData();
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
    protected void GrdCustBrand_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindData();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        foreach (GridViewRow grv in GrdCustBrand.Rows)
        {
            grv.BackColor = Color.White;
        }
    }

}
