using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using Base.Biz;
using BaseInfo.User;
using BaseInfo.Dept;
using Base.Page;
using Base.DB;
using Lease.ConShop;
using BaseInfo.Store;
using Lease.Brand;
using BaseInfo.authUser;
public partial class BaseInfo_Role_AddRole : BasePage
{
    public string baseInfo;
    public string strrefresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ShowTree();
        this.SetControlPro();
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            this.BindData();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Store_ItemAttentionVariety");
            strrefresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    /// <summary>
    /// 按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        #region
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        #endregion
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }
    #region
    //private void ShowCheckedTree(string strDeptID)
    //{
    //    BaseBO objBaseBo = new BaseBO();

    //    string jsdept = "";
    //    jsdept = "100" + "|" + "10" + "|" + "阳光新业" + "^";
    //    jsdept += "1" + "|" + "100" + "|" + "国际知名" + "^";
    //    jsdept += "2" + "|" + "100" + "|" + "国际二流" + "^";
    //    jsdept += "3" + "|" + "100" + "|" + "国内知名" + "^";

    //    DeptBrand objBrand = new DeptBrand();
    //    objBrand.strDeptID = strDeptID;

    //    Resultset rs = objBaseBo.Query(objBrand);
    //    if (rs.Count > 0)
    //    {
    //        foreach (DeptBrand objDeptBrand in rs)
    //        {
    //            jsdept += objDeptBrand.BrandId + "|" + objDeptBrand.BrandLevel + "|" + objDeptBrand.BrandName + "|" + objDeptBrand.Status + "^";
    //        }
    //    }
    //    depttxt.Value = jsdept;

    //}
    #endregion
    /// <summary>
    /// 显示树形列表
    /// </summary>
    private void ShowCheckTree()
    {
        BaseBO objBaseBo = new BaseBO();
        Resultset ds = objBaseBo.Query(new BrandLevel());
        string jsdept = "";
        jsdept = "100" + "|" + "10" + "|" + "阳光新业" + "^";
        if (ds.Count > 0)
        {
            foreach (BrandLevel brandlevel in ds)
            {
                BaseBO basebo = new BaseBO();
                basebo.WhereClause = "BrandLevel='" + brandlevel.BrandLevelCode + "'";
                DataTable dt = basebo.QueryDataSet(new ConShopBrand()).Tables[0];
                PagedDataSource pds = new PagedDataSource();
                pds.DataSource = dt.DefaultView;
                if (pds.Count < 1) { }
                else
                {
                    jsdept += brandlevel.BrandLevelCode + "|" + "100" + "|" + brandlevel.BrandLevelName + "|" + "^";
                }
            }
        }

        Resultset rs = objBaseBo.Query(new ConShopBrand());
        if (rs.Count > 0)
        {
            foreach (ConShopBrand objConShop in rs)
            {
                jsdept += objConShop.BrandLevel.ToString().Trim() + objConShop.BrandId.ToString().Trim() + "|" + objConShop.BrandLevel + "|" + objConShop.BrandName + "|" + this.GetCount(ViewState["DeptID"].ToString(), objConShop.BrandId.ToString()) + "^";
            }
        }
        depttxt.Value = jsdept;
    }
    /// <summary>
    /// 查询DeptBrand表中是否存在品牌的数据
    /// </summary>
    /// <param name="strDeptID"></param>
    /// <param name="strbrandID"></param>
    /// <returns></returns>
    private int GetCount(string strDeptID, string strbrandID)
    {
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select brandid from DeptBrand where deptid='" + strDeptID + "' and brandid='" + strbrandID + "'");
        if (ds.Tables[0].Rows.Count > 0)
            return 1;
        else
            return 0;
    }
    /// <summary>
    /// 显示属性列表
    /// </summary>
    private void ShowTree()
    {
        BaseBO objBaseBo = new BaseBO();
        Resultset ds = objBaseBo.Query(new BrandLevel());
        string jsdept = "";
        jsdept = "100" + "|" + "10" + "|" + "阳光新业" + "^";
        if (ds.Count > 0)
        {
            foreach (BrandLevel brandlevel in ds)
            {
                BaseBO basebo = new BaseBO();
                basebo.WhereClause = "BrandLevel='" + brandlevel.BrandLevelCode + "'";
                DataTable dt = basebo.QueryDataSet(new ConShopBrand()).Tables[0];
                PagedDataSource pds = new PagedDataSource();
                pds.DataSource = dt.DefaultView;
                if (pds.Count < 1) { }
                else
                {
                    jsdept += brandlevel.BrandLevelCode + "|" + "100" + "|" + brandlevel.BrandLevelName + "|" + "^";
                }

            }
        }
        Resultset rs = objBaseBo.Query(new ConShopBrand());
        if (rs.Count > 0)
        {
            foreach (ConShopBrand objConShop in rs)
            {
                jsdept += objConShop.BrandLevel + objConShop.BrandId + "|" + objConShop.BrandLevel + "|" + objConShop.BrandName + "|" + 0 + "^";
            }
        }
        depttxt.Value = jsdept;
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["DeptID"] != null && ViewState["DeptID"].ToString() != "")
        {
            BaseBO objBaseBo = new BaseBO();
            BaseTrans objTrans = new BaseTrans();
            DeptBrand objBrand = new DeptBrand();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objBrand.CreateUserId = objSessionUser.UserID;
            objBrand.CreateTime = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            objBrand.ModifyUserId = objSessionUser.UserID;
            objBrand.ModifyTime = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
            objBrand.OprRoleID = objSessionUser.RoleID;
            objBrand.OprDeptID = objSessionUser.DeptID;
            DataSet ds = objBaseBo.QueryDataSet("select deptid from DeptBrand where deptid='" + ViewState["DeptID"].ToString() + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                DeptBrand objDeptBrand = new DeptBrand();
                objBaseBo.WhereClause = "deptid=" + ViewState["DeptID"].ToString();
                if (objBaseBo.Delete(objDeptBrand) == -1)
                    return;
            }
            char[] treenodeid = new char[] { ',' };
            string treestr = deptid.Value;
            objTrans.BeginTrans();
            foreach (string substr in treestr.Split(treenodeid))
            {
                if (substr != "")
                {
                    objBrand.BrandId = Convert.ToInt32(substr.Substring(substr.Length - 3));
                    //objBrand.BrandId = Convert.ToInt32(substr.ToString().Substring(3));
                    objBrand.DeptId = Int32.Parse(ViewState["DeptID"].ToString());
                    if (objTrans.Insert(objBrand) == -1)
                    {
                        objTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                        return;
                    }
                }
            }
            objTrans.Commit();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showTitle", "ShowTitle()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        }
        this.BindData();
        ViewState["DeptID"] = null;
        foreach (GridViewRow grv in GrdRole.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    protected void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        objBaseBo.WhereClause = "Depttype=6";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        Dept objDept = new Dept();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objDept, this.GrdRole);
        #region
        //DataTable dt = objBaseBo.QueryDataSet(new Dept()).Tables[0];
        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < GrdRole.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdRole.DataSource = pds;
        //    GrdRole.DataBind();
        //}
        //else
        //{
        //    pds.AllowPaging = true;
        //    pds.PageSize = 14;
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

        //    this.GrdRole.DataSource = pds;
        //    this.GrdRole.DataBind();
        //    spareRow = GrdRole.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdRole.DataSource = pds;
        //    GrdRole.DataBind();
        //}
        #endregion
    }
    protected void GrdRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["DeptID"] = this.GrdRole.SelectedRow.Cells[0].Text.Trim();
        this.BindData();
        this.ShowCheckTree();
        this.btnSave.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showTitle", "ShowTitle()", true);
    }
    protected void GrdRole_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[0].Text == "&nbsp;")
            {
                e.Row.Cells[2].Text = "";
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ViewState["DeptID"] = null;
        this.Response.Redirect("~/BaseInfo/Store/StoreBrand.aspx");
    }
    protected void GrdRole_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindData();
        foreach (GridViewRow grv in GrdRole.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}

