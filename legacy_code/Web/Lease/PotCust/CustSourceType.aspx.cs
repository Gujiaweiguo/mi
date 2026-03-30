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
using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.PotCust;
using BaseInfo.User;
using System.Drawing;

public partial class Lease_PotCust_CustSourceType :BasePage
{
    public string strBaseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetControlPro();//为控件添加属性
        if (!this.IsPostBack)
        {
            ViewState["currentCount"] = "1";
            this.BindData();//绑定GridView
            this.BindSourceTypeStatus();//绑定是否有效下拉框
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_SourceType");
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
        this.cmbSourceTypeStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            cmbSourceTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定DataGrid
    /// </summary>
    private void BindData(int iCurrentpage)
    {
        BaseBO objBaseBo = new BaseBO();
        SourceType objSourceType = new SourceType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objSourceType, 11, iCurrentpage, this.btnBack, this.btnNext, this.GrdVewSourceType);
    }
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        SourceType objSourceType = new SourceType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objSourceType,this.GrdVewSourceType);
    }
    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
        this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
    }
    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
        this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SourceType objSourceType = new SourceType();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = objBaseBo.QueryDataSet("select SourceTypeName from SourceType where SourceTypeName='" + txtSourceTypeName.Text +"'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtSourceTypeName.select()", true);
            this.BindData();
        }
        else
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objSourceType.SourceTypeId = Base.BaseApp.GetID("SourceType", "SourceTypeId");
            objSourceType.SourceTypeName = this.txtSourceTypeName.Text.Trim();
            objSourceType.SourceTypeStatus = Int32.Parse(this.cmbSourceTypeStatus.SelectedValue.ToString());
            objSourceType.Note = this.txtNote.Text.Trim();
            objSourceType.CreateUserId = objSessionUser.UserID;
            //objSourceType.CreateTime = DateTime.Now.Date;
            objSourceType.OprDeptID = objSessionUser.OprDeptID;
            objSourceType.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Insert(objSourceType) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            this.ClearText();
            this.BindData();
            foreach (GridViewRow grv in GrdVewSourceType.Rows)
            {
                grv.BackColor = Color.White;
            }
        }
        
    }
    /// <summary>
    /// 清空控件中的值
    /// </summary>
    private void ClearText()
    {
        this.txtSourceTypeName.Text = "";
        this.txtNote.Text = "";
        this.cmbSourceTypeStatus.SelectedValue = "1";
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/PotCust/CustSourceType.aspx");
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["SourceTypeID"] != null && ViewState["SourceTypeID"].ToString() != "")
        {

            SourceType objSourceType = new SourceType();
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select SourceTypeName from SourceType where SourceTypeName='" + txtSourceTypeName.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objBaseBo.WhereClause = "SourceTypeId=" + Int32.Parse(ViewState["SourceTypeID"].ToString());
                objSourceType.SourceTypeName = this.txtSourceTypeName.Text.Trim();
                objSourceType.SourceTypeStatus = Int32.Parse(this.cmbSourceTypeStatus.SelectedValue.ToString());
                objSourceType.Note = this.txtNote.Text.Trim();
                objSourceType.ModifyUserId = objSessionUser.UserID;
                //objSourceType.ModifyTime = DateTime.Now.Date;
                objSourceType.OprDeptID = objSessionUser.OprDeptID;
                objSourceType.OprRoleID = objSessionUser.OprRoleID;
                if (objBaseBo.Update(objSourceType) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                    return;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
                this.ClearText();
                ViewState["SourceTypeID"] = "";
                //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                this.BindData();
                foreach (GridViewRow grv in GrdVewSourceType.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtSourceTypeName.select()", true);
                this.BindData();
            }
        }

        
    }
    protected void GrdVewCustType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text.Trim() == "&nbsp;")//如果当前行为空的话，选择按钮不显示出来
            {
                e.Row.Cells[4].Text = "";
            }
            if (e.Row.Cells[3].Text.Length == 1)
            {
                if (e.Row.Cells[3].Text.Equals(CustType.CUST_TYPE_STATUS_INVALID.ToString()))
                {
                    e.Row.Cells[3].Text = (String)GetGlobalResourceObject("Parameter", "WrkFlw_Disabled");
                }
                else if (e.Row.Cells[3].Text.Equals(CustType.CUST_TYPE_STATUS_VALID.ToString()))
                {
                    e.Row.Cells[3].Text = (String)GetGlobalResourceObject("Parameter", "WrkFlw_Enabled");
                }
            }
        }
    }
    protected void GrdVewCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        SourceType objSource = new SourceType();
        objBaseBo.WhereClause ="SourceTypeId="+ Int32.Parse(this.GrdVewSourceType.SelectedRow.Cells[0].Text.Trim());
        Resultset rs = objBaseBo.Query(objSource);
        if (rs.Count == 1)
        { 
            SourceType objSourceType = rs.Dequeue() as SourceType;
            ViewState["SourceTypeID"] = objSourceType.SourceTypeId;
            this.txtSourceTypeName.Text = objSourceType.SourceTypeName;
            this.cmbSourceTypeStatus.SelectedValue = objSourceType.SourceTypeStatus.ToString();
            this.txtNote.Text = objSourceType.Note.ToString();
        }
        //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        Session["editLog"] = txtSourceTypeName.Text;
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        this.BindData();
    }
    protected void GrdVewSourceType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        foreach (GridViewRow grv in GrdVewSourceType.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
