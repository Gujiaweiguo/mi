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
using BaseInfo.Dept;
using BaseInfo.Store;
using Base.Biz;
using Base.DB;
using Lease.PotCust;
using Lease.CustLicense;
using BaseInfo.User;
using System.Drawing;
using BaseInfo.authUser;
public partial class Store_StoreHistory : System.Web.UI.Page
{
    public string PotCustomer_Basic;
    public string Store_CardInfo;
    public string Store_ItemBackdrop;
    public string isbro;
    public string baseinfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        //显示树形列表
        this.ShowTree();
        this.SetControlPro();//为按钮添加属性
        if (!this.IsPostBack)
        {
            if (Request["look"] != null)
            {
                isbro = Request["look"].ToString();   
                if (isbro.ToLower() == "yes")
                {
                    this.btnEdit.Visible = false;
                    this.btnCancel.Visible = false;
                    this.btnSave.Visible = false;
                    this.LockControl();
                    baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Store_BusinessItemBasicInfobrowse");
                }
                else
                    baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Store_BusinessItemBasicInfoMaintenance");
            }
            this.SetControlLock();
            BindData("");
            PotCustomer_Basic = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic");
            Store_CardInfo = (String)GetGlobalResourceObject("BaseInfo", "Store_CardInfo");
            Store_ItemBackdrop = (String)GetGlobalResourceObject("BaseInfo", "Store_ItemBackdrop");
            this.btnSave.Enabled = false;
            this.btnEdit.Enabled = false;
        }
    }
    private void LockControl()
    {
        this.txtHistoryDate.Enabled = false;
        this.txtHistoryDesc.Enabled = false;
    }
    /// <summary>
    /// 显示树形列表
    /// </summary>
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "depttype = 6";
        objBaseBo.OrderBy = "OrderID";
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select DeptName from Dept where DeptType=1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            jsdept = "100" + "|" + "10" + "|" + ds.Tables[0].Rows[0][0] + "^";
        }
        else
        {
            jsdept = "100" + "|" + "10" + "|" + "商业项目" + "^";
        }
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        Resultset rs = objBaseBo.Query(new Dept());
        if (rs.Count > 0)
        {
            foreach (Dept objDept in rs)
            {
                jsdept += objDept.DeptID + "|" + "100" + "|" + objDept.DeptName + "^";
            }
        }
        depttxt.Value = jsdept;
    }
    /// <summary>
    /// 为按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        //this.txtHistoryDate.Attributes.Add("onblur", "TextIsNotNull(txtHistoryDate,ImgCustName)");
        this.btnSave.Attributes.Add("onclick", "return CheckIsNull()");
        this.txtHistoryDesc.Attributes.Add("onKeyDown", "CheckLength(this,8,10);");
        this.txtHistoryDesc.Attributes.Add("onKeyUp", "CheckLength(this,8,10);");

    }
    /// <summary>
    /// 设置控件不可写
    /// </summary>
    private void SetControlLock()
    {
        this.txtHistoryDate.Text = "";
        this.txtHistoryDesc.Text = "";
    }
    /// <summary>
    /// 设置控件为可写
    /// </summary>
    private void SetControlUNLock()
    {
        this.txtHistoryDate.Text = "";
        this.txtHistoryDesc.Text = "";
    }
    /// <summary>
    /// 绑定GridView数据
    /// </summary>
    private void BindData(string strDeptID)
    {
        BaseBO objBaseBo = new BaseBO();
        if(strDeptID=="")
            objBaseBo.WhereClause = "StoreId=100";
        else
            objBaseBo.WhereClause = "StoreId=" + strDeptID;
        StoreHistory objHistory = new StoreHistory();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objHistory, this.GrdUser);
        #region
        //DataTable dt = baseBO.QueryDataSet(new StoreHistory()).Tables[0];
        //pds.DataSource = dt.DefaultView;
        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < GrdUser.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdUser.DataSource = pds;
        //    GrdUser.DataBind();
        //}
        //else
        //{
        //    pds.AllowPaging = true;
        //    pds.PageSize = 10;
        //    lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
        //    pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
        //    if (pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = true;
        //    }

        //    if (pds.IsLastPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = false;
        //    }

        //    if (pds.IsFirstPage && pds.IsLastPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = false;
        //    }
        //    if (!pds.IsLastPage && !pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = true;
        //    }
        //    this.GrdUser.DataSource = pds;
        //    this.GrdUser.DataBind();
        //    spareRow = GrdUser.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdUser.DataSource = pds;
        //    GrdUser.DataBind();
        //}
        #endregion
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["DeptID"] = deptid.Value;
        if (ViewState["DeptID"].ToString() != "100")
        {
            this.BindData(ViewState["DeptID"].ToString());
            this.SetControlUNLock();
        }
        else
        {
            this.BindData("");
            this.SetControlLock();
        }
        foreach (GridViewRow grv in this.GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
        this.btnEdit.Enabled = false;
        this.btnSave.Enabled = true ;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void GrdUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        StoreHistory objStoreHis = new StoreHistory();
        ViewState["HistoryId"] = this.GrdUser.SelectedRow.Cells[0].Text.Trim();
        objBaseBo.WhereClause = "historyId="+this.GrdUser.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(new StoreHistory());
        if (rs.Count != 0)
        {
            objStoreHis = rs.Dequeue() as StoreHistory;
            if (objStoreHis.HistoryDate.ToString() != "")
                this.txtHistoryDate.Text = objStoreHis.HistoryDate.ToShortDateString();
            this.txtHistoryDesc.Text = objStoreHis.HistoryDesc.ToString();
            this.BindData(objStoreHis.StoreId.ToString());
        }
        this.btnEdit.Enabled = true;
        this.btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["DeptID"] != null && ViewState["DeptID"].ToString() != "100")
        {
            BaseBO objBaseBo = new BaseBO();
            StoreHistory objStoreHis = new StoreHistory();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objStoreHis.HistoryId = Base.BaseApp.GetCustumerID("StoreHistory", "historyId");
            objStoreHis.StoreId = Int32.Parse(ViewState["DeptID"].ToString());
            objStoreHis.HistoryDate = DateTime.Parse(this.txtHistoryDate.Text.Trim());
            objStoreHis.HistoryDesc = this.txtHistoryDesc.Text.Trim();

            objStoreHis.CreateUserId = objSessionUser.CreateUserID;
            objStoreHis.CreateTime = DateTime.Now.Date;
            objStoreHis.ModifyUserId = objSessionUser.ModifyUserID;
            objStoreHis.ModifyTime = DateTime.Now.Date;
            objStoreHis.OprRoleID = objSessionUser.OprRoleID;
            objStoreHis.OprDeptID = objSessionUser.OprDeptID;

            if (objBaseBo.Insert(objStoreHis) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            this.BindData(ViewState["DeptID"].ToString());
            this.SetControlLock();
        }
        else
        {
            this.BindData("");
        }
        this.btnEdit.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);

    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["HistoryId"] != null && ViewState["HistoryId"].ToString() != "")
        {
            BaseBO objBaseBo = new BaseBO();
            StoreHistory objStoreHis = new StoreHistory();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objBaseBo.WhereClause = "HistoryId=" + ViewState["HistoryId"].ToString();
            //objStoreHis.HistoryId = Int32.Parse(ViewState["HistoryId"].ToString());
            if (this.txtHistoryDate.Text.Trim() != "")
                objStoreHis.HistoryDate = DateTime.Parse(this.txtHistoryDate.Text.Trim());
            objStoreHis.HistoryDesc = this.txtHistoryDesc.Text.Trim();

            objStoreHis.CreateUserId = objSessionUser.CreateUserID;
            objStoreHis.CreateTime = DateTime.Now.Date;
            objStoreHis.ModifyUserId = objSessionUser.ModifyUserID;
            objStoreHis.ModifyTime = DateTime.Now.Date;
            objStoreHis.OprRoleID = objSessionUser.OprRoleID;
            objStoreHis.OprDeptID = objSessionUser.OprDeptID;

            if (objBaseBo.Update(objStoreHis) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
            this.BindData(ViewState["DeptID"].ToString());
            this.SetControlLock();
            foreach (GridViewRow grv in this.GrdUser.Rows)
            {
                grv.BackColor = Color.White;
            }
            ViewState["HistoryId"] = "";
            this.btnEdit.Enabled = false;
        }
        else
        {
            this.BindData("");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/BaseInfo/Store/StoreHistory.aspx?look=no");
    }
    protected void GrdUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[2].Text == "&nbsp;" || e.Row.Cells[2].Text == "")
                e.Row.Cells[4].Text = "";
            if (e.Row.Cells[0].Text != "&nbsp;" && e.Row.Cells[2].Text != "")
            {
                string strDate = e.Row.Cells[2].Text.ToString().Substring(0, 10);
                if (strDate.LastIndexOf('0') != -1)
                    strDate = e.Row.Cells[2].Text.ToString().Substring(0, 9);
                e.Row.Cells[2].Text = strDate;

            }
        }
    }
    protected void GrdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindData(ViewState["DeptID"].ToString());
        foreach (GridViewRow grv in this.GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
        this.SetControlLock();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
}
