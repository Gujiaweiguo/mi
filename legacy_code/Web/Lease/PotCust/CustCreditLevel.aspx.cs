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

public partial class Lease_PotCust_CreditLevel : BasePage
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
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CreditLevel");
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
        this.cmbCustTypeStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            cmbCustTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定DataGrid
    /// </summary>
    //private void BindData(int iCurrentpage)
    //{
    //    BaseBO objBaseBo = new BaseBO();
    //    CreditLevel objCredit = new CreditLevel();
    //    BaseInfo.BaseCommon.BindGridView(objBaseBo, objCredit, 10, iCurrentpage, this.btnBack, this.btnNext, this.GrdVewCreditLevel);
    //}
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        CreditLevel objCredit = new CreditLevel();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objCredit, this.GrdVewCreditLevel);
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        CreditLevel objLevel = new CreditLevel();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = objBaseBo.QueryDataSet("select CreditLevelName from CreditLevel where CreditLevelName='" + txtLevelName.Text + "'");
        if (ds.Tables[0].Rows.Count == 0)
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objLevel.CreditLevelId = Base.BaseApp.GetID("CreditLevel", "CreditLevelId");
            objLevel.CreditLevelName = this.txtLevelName.Text.Trim();
            objLevel.Status = Int32.Parse(this.cmbCustTypeStatus.SelectedValue.ToString());
            objLevel.Note = this.txtNote.Text.Trim();
            objLevel.CreateUserId = objSessionUser.UserID;
            //objLevel.CreateTime = DateTime.Now.Date;
            //objLevel.ModifyUserId = objSessionUser.ModifyUserID;
            //objLevel.ModifyTime = DateTime.Now.Date;
            objLevel.OprDeptID = objSessionUser.OprDeptID;
            objLevel.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Insert(objLevel) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            this.BindData();
            this.ClearText();
            foreach (GridViewRow grv in GrdVewCreditLevel.Rows)
            {
                grv.BackColor = Color.White;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_LevelName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtLevelName.select()", true);
            BindData();
        }
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        this.txtLevelName.Text = "";
        this.txtNote.Text = "";
        this.cmbCustTypeStatus.SelectedValue = "1";
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["CreditLevelId"] != null && ViewState["CreditLevelId"].ToString() != "")
        {
            CreditLevel objLevel = new CreditLevel();
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select CreditLevelName from CreditLevel where CreditLevelName='" + txtLevelName.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objBaseBo.WhereClause = "CreditLevelId=" + ViewState["CreditLevelId"].ToString();
                objLevel.CreditLevelName = this.txtLevelName.Text.Trim();
                objLevel.Status = Int32.Parse(this.cmbCustTypeStatus.SelectedValue.ToString());
                objLevel.Note = this.txtNote.Text.Trim();
                //objLevel.CreateUserId = objSessionUser.CreateUserID;
                //objLevel.CreateTime = DateTime.Now.Date;
                objLevel.ModifyUserId = objSessionUser.UserID;
                //objLevel.ModifyTime = DateTime.Now.Date;
                objLevel.OprDeptID = objSessionUser.OprDeptID;
                objLevel.OprRoleID = objSessionUser.OprRoleID;
                if (objBaseBo.Update(objLevel) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                    return;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
                this.ClearText();

                ViewState["CreditLevelId"] = "";
                //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
                btnEdit.Enabled = false;
                btnSave.Enabled = true;
                this.BindData();
                foreach (GridViewRow grv in GrdVewCreditLevel.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }

            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_LevelName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtLevelName.select()", true);
                BindData();
            }
        }
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/PotCust/CustCreditLevel.aspx");
    }
    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
        //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));//绑定
    }
    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
        //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));// 绑定
    }
    protected void GrdVewCustType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text.Trim() == "&nbsp;")//如果当前行没有数据，选择按钮不显示出来
                e.Row.Cells[4].Text = "";
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
        CreditLevel objCreate = new CreditLevel();
        objBaseBo.WhereClause = "CreditLevelId=" + this.GrdVewCreditLevel.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(objCreate);
        if (rs.Count == 1)
        {
            CreditLevel objCreateLevel = rs.Dequeue() as CreditLevel;
            ViewState["CreditLevelId"] = objCreateLevel.CreditLevelId;
            this.txtLevelName.Text =  objCreateLevel.CreditLevelName;
            this.txtNote.Text = objCreateLevel.Note;
            this.cmbCustTypeStatus.SelectedValue = objCreateLevel.Status.ToString();
        }
        //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        Session["editLog"] = txtLevelName.Text;
        this.BindData();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
    }
    protected void GrdVewCreditLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        foreach (GridViewRow grv in GrdVewCreditLevel.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
