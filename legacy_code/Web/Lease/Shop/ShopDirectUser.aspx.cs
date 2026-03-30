using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.DB;
using Base.Biz;
using RentableArea;
using BaseInfo.Dept;
using Base;
using Lease.ConShop;
using Lease.SMSPara;
using BaseInfo.authUser;
using BaseInfo.User;
using System.Drawing;
using Base.Page;

public partial class Lease_Shop_ShopDirectUser : BasePage
{
    public string title = "";
    private static int OPR_ADD = 1;/*添加*/
    private static int OPR_EDIT = 2;/*更新*/
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!IsPostBack)
        {
            ViewState["BuildingID"] = "";
            ShowTreeNode();
            //ViewState["BuildingID"] = deptid.Value;
            this.BindData();
            title = (String)GetGlobalResourceObject("BaseInfo", "TpUse_lblAddDirectUserTitle");
            ViewState["Flag"] = OPR_ADD;
            BindDrop();//绑定下拉框
            BindStation();//绑定岗位
        }
        else
        {
            txtPassword.Attributes.Add("value", "");
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        }
    }
    /// <summary>
    /// 绑定岗位
    /// </summary>
    private void BindStation()
    {
        cmbStation.Items.Add(new ListItem("收款主管"));
        cmbStation.Items.Add(new ListItem("收款员"));
    }
    /// <summary>
    /// 绑定下拉框
    /// </summary>
    private void BindDrop()
    {
        /*是否有效*/
        int[] status = TpUsr.GetTpUsrStatus();
        cmbStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", TpUsr.GetTpUsrStatusDesc(status[i])), status[i].ToString()));
        }

        /*收款权限*/
        cmbGathering.Items.Clear();
        for (int i = 1; i < 10; i++)
        {
            cmbGathering.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
    }
    /// <summary>
    /// 树形列表
    /// </summary>
    private void ShowTreeNode()
    {
        string jsdept = "";
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        Dept dept = new Dept();

        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        rs = baseBO.Query(dept);
        if (rs.Count == 1)
        {
            dept = rs.Dequeue() as Dept;
            jsdept = dept.DeptID + "|" + "0" + "|" + dept.DeptName + "^";
        }
        else
            return;
        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            baseBO.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = baseBO.Query(dept);
        if (rs.Count > 0)
        {
            foreach (Dept store in rs)
            {
                jsdept += store.DeptID + "|" + dept.DeptID + "|" + store.DeptName + "^";
                baseBO.WhereClause = "StoreId=" + store.DeptID;
                rs = baseBO.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)//大楼
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    protected void GrdUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex>=0)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[4].Text = "";
            }
        }
    }
    protected void GrdUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        TpUsr tpUsr = new TpUsr();
        baseBO.WhereClause = "TPUsrId = '" + GrdUser.SelectedRow.Cells[0].Text.Trim() + "'";
        rs = baseBO.Query(tpUsr);

        if (rs.Count == 1)
        {
            txtPassword.Attributes.Add("value", "****");
            tpUsr = rs.Dequeue() as TpUsr;
            ViewState["TPUSERID"] = tpUsr.TPUsrId.Trim();
            txtWorkNo.Text = tpUsr.TPUsrId.Trim().Substring(tpUsr.TPUsrId.Trim().Length-2);
            txtID.Text = tpUsr.IDNo.Trim();
            txtUserName.Text = tpUsr.TPUsrNm.Trim();
            txtMobile.Text = tpUsr.Phone.Trim();
            hidnPassword.Value = tpUsr.szPin.Trim();
            if (tpUsr.Sex.Trim() == "M")
            {
                rdoMan.Checked = true;
                rdoWoman.Checked = false;
            }
            else if (tpUsr.Sex.Trim() == "F")
            {
                rdoMan.Checked = false;
                rdoWoman.Checked = true;
            }

            txtBirth.Text = Convert.ToDateTime(tpUsr.Dob).ToString("yyyy-MM-dd");
            txtBeginWorkDate.Text = Convert.ToDateTime(tpUsr.DateStart).ToString("yyyy-MM-dd");
            cmbStation.SelectedItem.Text = tpUsr.JobTitleNm.Trim();
            cmbGathering.SelectedItem.Text = tpUsr.GpId;

            if (tpUsr.TPUsrStatus.Trim() == "E")
                chkConcerned.Checked = true;
            else if (tpUsr.TPUsrStatus.Trim() == "N")
                chkConcerned.Checked = false;

            if (tpUsr.DeleteTime == null)
                cmbStatus.SelectedValue = TpUsr.TPUSR_STATUS_YES.ToString();
            else
                cmbStatus.SelectedValue = TpUsr.TPUSR_STATUS_NO.ToString();

            ViewState["Flag"] = OPR_EDIT;
            BindData();
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.txtUserName.Text.Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "User_lblUserName") + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            ClearGridViewSelected();
            return;
        }
        if (this.txtWorkNo.Text.Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "User_lblWorkNo") + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        if (this.txtWorkNo.Text.Trim().Length != 2)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "User_lblWorkNo") + "为2位数字'", true);
            ClearGridViewSelected();
            return;
        }
        BaseBO baseBO = new BaseBO();
        TpUsr tpUsr = new TpUsr();
        
        tpUsr.TPUsrId = "99" + ViewState["BuildingID"].ToString().Trim() + txtWorkNo.Text.Trim();
        tpUsr.IDNo = txtID.Text.Trim();
        tpUsr.TPUsrNm = txtUserName.Text.Trim();

        tpUsr.Phone = txtMobile.Text.Trim();
        if (rdoMan.Checked)
        {
            tpUsr.Sex = "M";
        }
        else if (rdoWoman.Checked)
        {
            tpUsr.Sex = "F";
        }
        try { tpUsr.Dob = Convert.ToDateTime(txtBirth.Text); }
        catch { tpUsr.Dob = DateTime.Now.Date; }
        try { tpUsr.DateStart = Convert.ToDateTime(txtBeginWorkDate.Text); }
        catch { tpUsr.DateStart = DateTime.Now.Date; }
        tpUsr.JobTitleNm = cmbStation.SelectedItem.Text;
        tpUsr.GpId = cmbGathering.SelectedItem.Text;

        if (chkConcerned.Checked)
            tpUsr.TPUsrStatus = "E";
        else
            tpUsr.TPUsrStatus = "N";
        tpUsr.UnitId = "0";
        tpUsr.BuildingID = ViewState["BuildingID"].ToString();
        tpUsr.CustID = "0";
        if (Convert.ToInt32(cmbStatus.SelectedValue) == TpUsr.TPUSR_STATUS_NO)
            tpUsr.DeleteTime = DateTime.Now;

        if (Convert.ToInt32(ViewState["Flag"]) == OPR_ADD)
        {
            tpUsr.szPin = txtPassword.Text.Trim();
            string str_sql = "select TPUsrID from TpUsr where TPUsrID = '" + tpUsr.TPUsrId + "'";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                if (baseBO.Insert(tpUsr) == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "    " + (String)GetGlobalResourceObject("BaseInfo", "Associator_lblStaffID") + ":" + tpUsr.TPUsrId + "'", true);
                    ClearText();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            }
        }
        else if (Convert.ToInt32(ViewState["Flag"]) == OPR_EDIT)
        {
            if (Convert.ToInt32(cmbStatus.SelectedValue) == TpUsr.TPUSR_STATUS_NO)
            {
                if (baseBO.ExecuteUpdate("Update TpUsr Set DeleteTime = '" + DateTime.Now + "' Where TPUsrId = '" + ViewState["TPUSERID"].ToString() + "'") == -1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                    ClearText();
                }
            }
            else
            {
                tpUsr.szPin = txtPassword.Text.Trim();
                BaseTrans trans = new BaseTrans();
                trans.BeginTrans();
                try
                {
                    tpUsr.DeleteTime = DateTime.Now;
                    trans.WhereClause = "TPUsrId = '" + ViewState["TPUSERID"] + "'";
                    if (trans.Update(tpUsr) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    }
                    else
                    {
                        trans.ExecuteUpdate("update TpUsr set DeleteTime = null where TPUsrId = '" + ViewState["TPUSERID"] + "'");
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                        ClearText();
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                trans.Commit();
            }
        }
        BindDrop();
        ViewState["Flag"] = OPR_ADD;
        BindData();
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ViewState["BuildingID"] = "";
        ClearText();
        BindData();
        foreach (GridViewRow grv in GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    /// <summary>
    /// 树形列表点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeClick_Click(object sender, EventArgs e)
    {
        string deptId = "";
        deptId = deptid.Value;
        selectdeptid.Value = deptid.Value;
        if (deptId.Length > 3)
        {
            ViewState["BuildingID"] = deptId.Substring(deptId.Length - 3, 3);
            BindData();
            this.btnSave.Enabled = true;
            ViewState["TPUSERID"] = "";
        }
        else
        {
            ViewState["BuildingID"] = "";
            BindData();
            this.btnSave.Enabled = false;
        }
        this.ClearText();
        foreach (GridViewRow grv in GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    protected void BindData()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        if (ViewState["BuildingID"].ToString() == "")
            baseBO.WhereClause = "BuildingID =" + 0;
        else
            baseBO.WhereClause = "len(TpUsrID)>5 and BuildingID =" + ViewState["BuildingID"];
        baseBO.OrderBy = "TPUsrId";
        DataTable dt = baseBO.QueryDataSet(new TpUsr()).Tables[0];
        int count = dt.Rows.Count;
        if (count==0 || count % 13 != 0)
        {
            for (int i = (count % 13); i < 13; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
        }
        GrdUser.DataSource = dt;
        GrdUser.DataBind();
        foreach (GridViewRow grv in GrdUser.Rows)
        {
            if (grv.Cells[1].Text != "&nbsp;")
            {
                grv.Cells[3].Text = grv.Cells[3].Text.Trim().Substring(0, 10);
            }
        }
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in GrdUser.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[4].Text = "";
            }
            else
                gvr.Cells[3].Text = gvr.Cells[3].Text.Substring(0,10);
        }
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        txtBeginWorkDate.Text = "";
        txtBirth.Text = "";
        txtID.Text = "";
        txtMobile.Text = "";
        txtPassword.Text = "";
        txtUserName.Text = "";
        txtWorkNo.Text = "";
        cmbGathering.SelectedIndex=0;
        cmbStation.SelectedIndex=0;
        this.chkConcerned.Checked = false;
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
        
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.BindData();
        foreach (GridViewRow grv in GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
