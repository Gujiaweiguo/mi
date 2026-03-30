using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using Lease.AdContract;
using BaseInfo.User;
using BaseInfo.Dept;
using BaseInfo.authUser;
using RentableArea;

public partial class Lease_AdContract_AreaManagement : BasePage
{
    public string baseInfo;
    public string enterInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_AreaManagement");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            this.btnEdit.Attributes.Add("onclick", "return InputValidator(form1)");
            enterInfo = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            this.BindDDL();//绑定下拉列表框
            Session["Sql"] = "select AreaID,AreaCode,AreaName,AreaTypeID,'' as AreaTypeName,AreaStatus,'' as AreaStatusName,Note  from AreaManage where 1=2";
            this.BindGV(Session["Sql"].ToString());
            btnSave.Enabled = false;
            btnEdit.Enabled = false;
            showtree();
        }
    }
    /// <summary>
    /// 绑定下拉列表框

    /// </summary>
    private void BindDDL()
    {
        this.ddlStatus.Items.Clear();
        int[] Status = AdBoardManage.GetAdStatus();
        for (int i = 0; i < Status.Length; i++)
        {
            ddlStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", AdBoardManage.GetAdStatusDesc(Status[i])), Status[i].ToString()));
        }

        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "AreaTypeStatus=1";
        Resultset rs = objBaseBo.Query(new AreaType());
        foreach (AreaType objAreaType in rs)
        {
            this.ddlAdType.Items.Add(new ListItem(objAreaType.AreaTypeDesc.ToString(), objAreaType.AreaTypeID.ToString()));
        }
    }

    private void showtree()
    {
        BaseBO objBase = new BaseBO();
        Resultset rs = new Resultset();
        Dept objDept = new Dept();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        string jsdept = "";
        rs = objBase.Query(objDept);
        if (rs.Count == 1)
        {
            objDept = rs.Dequeue() as Dept;
            jsdept = objDept.DeptID + "|" + "0" + "|" + objDept.DeptName + "^";
        }
        else
        {
            return;
        }
        objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;//商业项目
        objBase.OrderBy = " OrderID ";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBase.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = objBase.Query(objDept);
        objBase.OrderBy = "";
        if (rs.Count > 0)
        {
            foreach (Dept store in rs)
            {
                jsdept += store.DeptID + "|" + objDept.DeptID + "|" + store.DeptName + "^";
                objBase.WhereClause = "StoreId=" + store.DeptID;
                rs = objBase.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                    }
                }
            }
        }
        depttxt.Value = jsdept;
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);

    }

    private void BindGV(string strSql)
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        //string strSql = "select AreaID,AreaCode,AreaName,AreaTypeID,'' as AreaTypeName,AreaStatus,'' as AreaStatusName,Note  from AreaManage";
        DataTable dt = baseBo.QueryDataSet(strSql).Tables[0];
        int count = dt.Rows.Count;
        //状态

        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["AreaStatusName"] = (String)GetGlobalResourceObject("Parameter", AdBoardManage.GetAdStatusDesc(Convert.ToInt32(dt.Rows[j]["AreaStatus"])));
        }

        //广告位类型

        for (int j = 0; j < count; j++)
        {
            baseBo.WhereClause = "";
            baseBo.WhereClause = "AreaTypeID = " + Convert.ToInt32(dt.Rows[j]["AreaTypeID"]);
            Resultset rsType = baseBo.Query(new AreaType());
            if (rsType.Count == 1)
            {
                AreaType objType = rsType.Dequeue() as AreaType;
                dt.Rows[j]["AreaTypeName"] = objType.AreaTypeDesc;
            }
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < this.gvAdBoard.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvAdBoard.DataSource = pds;
            gvAdBoard.DataBind();
        }
        else
        {
            gvAdBoard.EmptyDataText = "";
            

            this.gvAdBoard.DataSource = pds;
            this.gvAdBoard.DataBind();
            spareRow = gvAdBoard.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvAdBoard.DataSource = pds;
            gvAdBoard.DataBind();
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        AreaManage objArea = new AreaManage();
        objArea.AreaID = BaseApp.GetID("AreaManage","AreaID");
        objArea.AreaCode = this.txtAdBordCode.Text.Trim();
        objArea.AreaName = this.txtAdBordName.Text.Trim();
        try { objArea.AreaStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { }
        try { objArea.AreaTypeID = Int32.Parse(this.ddlAdType.SelectedValue); }
        catch { }
        objArea.Note = this.txtNote.Text.Trim();
        objArea.StoreID = int.Parse(deptid.Value.Substring(0, 3));
        objArea.BuildingID = int.Parse(deptid.Value.Substring(3));
        if (objBaseBo.Insert(objArea) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        BindGV(Session["Sql"].ToString());
        this.txtAdBordCode.Text = "";
        this.txtAdBordName.Text = "";
        this.txtNote.Text = "";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        AreaManage objArea = new AreaManage();
        objArea.AreaCode = this.txtAdBordCode.Text.Trim();
        objArea.AreaName = this.txtAdBordName.Text.Trim();
        try { objArea.AreaStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { }
        try { objArea.AreaTypeID = Int32.Parse(this.ddlAdType.SelectedValue); }
        catch { }
        objArea.Note = this.txtNote.Text.Trim();
        objBaseBo.WhereClause = "AreaID='" + ViewState["AreaID"].ToString() + "'";
        if (objBaseBo.Update(objArea) == 1)
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        BindGV(Session["Sql"].ToString());
        this.txtAdBordCode.Text = "";
        this.txtAdBordName.Text = "";
        this.txtNote.Text = "";
        btnEdit.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.txtAdBordCode.Text = "";
        this.txtAdBordName.Text = "";
        this.txtNote.Text = "";
        Session["Sql"] = "select AreaID,AreaCode,AreaName,AreaTypeID,'' as AreaTypeName,AreaStatus,'' as AreaStatusName,Note  from AreaManage where 1=2";
        BindGV(Session["Sql"].ToString());
        btnSave.Enabled = false;
        btnEdit.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void gvAdBoard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            if (e.Row.Cells[0].Text == "&nbsp;")
            {
                e.Row.Cells[8].Text = "";
            }
        }
    }
    protected void gvAdBoard_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["AreaID"] = this.gvAdBoard.SelectedRow.Cells[0].Text.Trim();
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "AreaID='" + this.gvAdBoard.SelectedRow.Cells[0].Text.Trim() + "'";
        Resultset rs = objBaseBo.Query(new AreaManage());
        if (rs.Count == 1)
        {
            AreaManage objManage = rs.Dequeue() as AreaManage;
            this.txtAdBordCode.Text = objManage.AreaCode;// this.gvAdBoard.SelectedRow.Cells[1].Text.Trim();
            this.txtAdBordName.Text = objManage.AreaName;// this.gvAdBoard.SelectedRow.Cells[2].Text.Trim();
            this.txtNote.Text = objManage.Note;// this.gvAdBoard.SelectedRow.Cells[7].Text.Trim();
            try { this.ddlStatus.SelectedValue = objManage.AreaStatus.ToString();}// this.gvAdBoard.SelectedRow.Cells[5].Text.Trim(); }
            catch { }
            try { this.ddlAdType.SelectedValue = objManage.AreaTypeID.ToString(); }// this.gvAdBoard.SelectedRow.Cells[3].Text.Trim(); }
            catch { }
        }
        this.BindGV(Session["Sql"].ToString());
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        Dept dept = new Dept();
        DeptBO deptBO = new DeptBO();
        DeptInfo objDeptInfo = new DeptInfo();
        string deptId = "";
        TreeNode Node = new TreeNode();

        deptId = deptid.Value;
        if (deptId.Length == 6)
        {
            btnSave.Enabled = true;
            Session["Sql"] = "select AreaID,AreaCode,AreaName,AreaTypeID,'' as AreaTypeName,AreaStatus,'' as AreaStatusName,Note  from AreaManage  where buildingid='" + deptId.Substring(3) + "' ";
            BindGV(Session["Sql"].ToString());
        }
        else
        {
            Session["Sql"] = "select AreaID,AreaCode,AreaName,AreaTypeID,'' as AreaTypeName,AreaStatus,'' as AreaStatusName,Note  from AreaManage  where 1=2";
            btnSave.Enabled = false;
            BindGV(Session["Sql"].ToString());
        }



        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "parent.document.all.txtWroMessage.value = ''", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    protected void gvAdBoard_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindGV(Session["Sql"].ToString());
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
}
