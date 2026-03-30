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
using RentableArea;
using System.Drawing;

public partial class RentableArea_Unit_UnitType : BasePage
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
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_RegularUnitsTypeVindicate");
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
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        UnitTypes objUnitType = new UnitTypes();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objUnitType, this.GrdVewSourceType);
    }
    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
        //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
    }
    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
        //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        UnitTypes objUnitType = new UnitTypes();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = objBaseBo.QueryDataSet("select UnitTypeCode from UnitType where UnitTypeCode='" + txtCustTypeCode.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtCustTypeCode.select()", true);
            this.BindData();
        }
        else
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objUnitType.UnitTypeID = Base.BaseApp.GetID("UnitType", "UnitTypeID");
            objUnitType.UnitTypeCode = this.txtCustTypeCode.Text;
            objUnitType.UnitTypeName = this.txtSourceTypeName.Text;
            objUnitType.Note = this.txtNote.Text;
            objUnitType.UnitTypeStatus = Int32.Parse(this.cmbSourceTypeStatus.SelectedValue.ToString());


            objUnitType.CreateUserId = objSessionUser.UserID;
            objUnitType.CreateTime = DateTime.Now;
            objUnitType.OprDeptID = objSessionUser.OprDeptID;
            objUnitType.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Insert(objUnitType) <= 0)
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
        this.txtCustTypeCode.Text = "";
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/RentableArea/Unit/UnitType.aspx");
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
            UnitTypes objUnitType = new UnitTypes();
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select UnitTypeCode from UnitType where UnitTypeCode='" + txtCustTypeCode.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objBaseBo.WhereClause = "UnitTypeID=" + Int32.Parse(ViewState["SourceTypeID"].ToString());
                objUnitType.UnitTypeCode = this.txtCustTypeCode.Text;
                objUnitType.UnitTypeName = this.txtSourceTypeName.Text;
                objUnitType.Note = this.txtNote.Text;
                objUnitType.UnitTypeStatus = Int32.Parse(this.cmbSourceTypeStatus.SelectedValue.ToString());

                objUnitType.ModifyUserId = objSessionUser.UserID;
                objUnitType.ModifyTime = DateTime.Now;
                objUnitType.OprDeptID = objSessionUser.OprDeptID;
                objUnitType.OprRoleID = objSessionUser.OprRoleID;
                if (objBaseBo.Update(objUnitType) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                    return;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
                this.ClearText();
                ViewState["SourceTypeID"] = "";
                btnEdit.Enabled = false;
                btnSave.Enabled = true;
                this.BindData();
                foreach (GridViewRow grv in GrdVewSourceType.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtCustTypeCode.select()", true);
                BindData();
            }
        }

    }
    protected void GrdVewCustType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text.Trim() == "&nbsp;")//如果当前行为空的话，选择按钮不显示出来
            {
                e.Row.Cells[5].Text = "";
            }
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
    protected void GrdVewCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        UnitTypes objUnit = new UnitTypes();
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "UnitTypeID=" + Int32.Parse(this.GrdVewSourceType.SelectedRow.Cells[0].Text.Trim());
        Resultset rs = objBaseBo.Query(objUnit);
        if (rs.Count == 1)
        {
            objUnit = rs.Dequeue() as UnitTypes;
            ViewState["SourceTypeID"] = objUnit.UnitTypeID;
            this.txtCustTypeCode.Text = objUnit.UnitTypeCode;
            this.cmbSourceTypeStatus.SelectedValue = objUnit.UnitTypeStatus.ToString();
            this.txtNote.Text = objUnit.Note.ToString();
            this.txtSourceTypeName.Text = objUnit.UnitTypeName;
        }
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        Session["editLog"] = txtCustTypeCode.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
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
        this.BindData();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        foreach (GridViewRow grv in GrdVewSourceType.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
