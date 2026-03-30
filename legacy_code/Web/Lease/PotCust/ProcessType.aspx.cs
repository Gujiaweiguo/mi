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
using Lease.PotCust;
using BaseInfo.User;
using Base.DB;
using Base.Biz;
using Base.Page;
using System.Drawing;

public partial class Lease_PotCust_ProcessType : BasePage
{
    public string strBaseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetControlPro();//添加按钮属性
        if (!this.IsPostBack)
        {
            ViewState["currentCount"] = "1";
            this.BindSourceTypeStatus();//绑定是否有效下拉框
            this.BindData();//绑定GridView
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ProcessType");
            btnEdit.Enabled = false;
        }
    }
    /// <summary>
    /// 为按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        this.btnSave.Attributes.Add("onclick", "return allTextBoxValidator()");
        this.btnEdit.Attributes.Add("onclick", "return allTextBoxValidator()");
    }
    /// <summary>
    /// 绑定是否有效下拉框
    /// </summary>
    private void BindSourceTypeStatus()
    {
        int[] status = CustType.GetCustTypeStatus();
        this.cmbTypeStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            this.cmbTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定DataGrid
    /// </summary>
    private void BindData(int iCurrentpage)
    {
        BaseBO objBaseBo = new BaseBO();
        ProcessType objPressType = new ProcessType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objPressType, 10, iCurrentpage, this.btnBack, this.btnNext, this.GrdBrandOperateType);
    }
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        ProcessType objPressType = new ProcessType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objPressType, this.GrdBrandOperateType);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ProcessType objPressType = new ProcessType();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = objBaseBo.QueryDataSet("select ProcessTypeCode,ProcessTypeName from ProcessType where ProcessTypeName='" + txtProcessName.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtProcessName.select()", true);
            this.BindData();
        }
        else
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objPressType.ProcessTypeId = Base.BaseApp.GetID("ProcessType", "ProcessTypeId");
            objPressType.ProcessTypeCode = this.txtProcessCode.Text.Trim();
            objPressType.ProcessTypeName = this.txtProcessName.Text.Trim();
            objPressType.Status = Int32.Parse(this.cmbTypeStatus.SelectedValue.ToString());
            objPressType.Note = this.txtNote.Text.Trim();
            objPressType.CreateUserId = objSessionUser.UserID;
            //objPressType.CreateTime = DateTime.Now.Date;
            //objPressType.ModifyUserId = objSessionUser.ModifyUserID;
            //objPressType.ModifyTime = DateTime.Now.Date;
            objPressType.OprDeptID = objSessionUser.OprDeptID;
            objPressType.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Insert(objPressType) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            this.ClearText();
            this.BindData();
            foreach (GridViewRow grv in GrdBrandOperateType.Rows)
            {
                grv.BackColor = Color.White;
            }
        }
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        this.txtProcessName.Text = "";
        this.txtNote.Text = "";
        this.cmbTypeStatus.SelectedValue = "1";
        this.txtProcessCode.Text = "";
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["ProcessTypeId"] != null && ViewState["ProcessTypeId"].ToString() != "")
        {
            ProcessType objPressType = new ProcessType();
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select ProcessTypeName from ProcessType where ProcessTypeName='" + txtProcessName.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objBaseBo.WhereClause = "ProcessTypeId=" + ViewState["ProcessTypeId"].ToString();

                objPressType.ProcessTypeName = this.txtProcessName.Text.Trim();
                objPressType.ProcessTypeCode = this.txtProcessCode.Text.Trim();
                objPressType.Status = Int32.Parse(this.cmbTypeStatus.SelectedValue.ToString());
                objPressType.Note = this.txtNote.Text.Trim();
                //objPressType.CreateUserId = objSessionUser.CreateUserID;
                //objPressType.CreateTime = DateTime.Now.Date;
                objPressType.ModifyUserId = objSessionUser.UserID;
                //objPressType.ModifyTime = DateTime.Now.Date;
                objPressType.OprDeptID = objSessionUser.OprDeptID;
                objPressType.OprRoleID = objSessionUser.OprRoleID;
                if (objBaseBo.Update(objPressType) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                    return;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
                this.ClearText();
                ViewState["ProcessTypeId"] = "";
                //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                this.BindData();
                foreach (GridViewRow grv in GrdBrandOperateType.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtProcessName.select()", true);
                this.BindData();
            }
        }
        
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/PotCust/ProcessType.aspx");
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
        this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));//绑定
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
        this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));// 绑定
    }
    protected void GrdBrandOperateType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[2].Text.Trim() == "&nbsp;")//如果当前行没有数据，选择按钮不显示出来
                e.Row.Cells[5].Text = "";
            if (e.Row.Cells[4].Text.Length == 1)
            {
                if (e.Row.Cells[4].Text.Equals(CustType.CUST_TYPE_STATUS_INVALID.ToString()))
                {
                    e.Row.Cells[4].Text = (String)GetGlobalResourceObject("Parameter", "WrkFlw_Disabled");
                }
                else if (e.Row.Cells[4].Text.Equals(CustType.CUST_TYPE_STATUS_VALID.ToString()))
                {
                    e.Row.Cells[4].Text = (String)GetGlobalResourceObject("Parameter", "WrkFlw_Enabled");
                }
            }
        }
    }
    protected void GrdBrandOperateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        ProcessType objPressType = new ProcessType();
        objBaseBo.WhereClause = "ProcessTypeId=" + this.GrdBrandOperateType.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(objPressType);
        if (rs.Count == 1)
        {
            ProcessType objPress = rs.Dequeue() as ProcessType;
            ViewState["ProcessTypeId"] = objPress.ProcessTypeId;
            this.txtProcessName.Text = objPress.ProcessTypeName;
            this.txtProcessCode.Text = objPress.ProcessTypeCode;
            this.txtNote.Text = objPress.Note;
            this.cmbTypeStatus.SelectedValue = objPress.Status.ToString();
        }
        //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
        Session["editLog"] = txtProcessName.Text;
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.BindData();
    }
    protected void GrdBrandOperateType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        foreach (GridViewRow grv in GrdBrandOperateType.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
