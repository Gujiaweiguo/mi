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

public partial class Lease_PotCust_ClientLevel : BasePage
{
    public string strBaseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SetControlPro();//添加按钮属性
        if (!this.IsPostBack)
        {
            this.BindTypeStatus();//绑定是否有效下拉框
            this.BindData();//绑定GridView
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "PotShop_CustomerLayerVindicate");
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
    private void BindTypeStatus()
    {
        int[] status = CustType.GetCustTypeStatus();
        this.ddlStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            this.ddlStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        ClientLevel objClientlevel = new ClientLevel();
        BaseInfo.BaseCommon.BindGridView(objBaseBo,objClientlevel,this.GrdVewCreditLevel);
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ClientLevel objClientLevel = new ClientLevel();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = objBaseBo.QueryDataSet("select ClientLevelName from ClientLevel where ClientLevelName='" + txtLevelName.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotShop_CustomerLayerName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtLevelName.select()", true);
            this.BindData();
        }
        else
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objClientLevel.ClientLevelId = Base.BaseApp.GetID("ClientLevel", "ClientLevelId");
            objClientLevel.ClientLevelName = this.txtLevelName.Text.Trim();
            objClientLevel.Status = Int32.Parse(this.ddlStatus.SelectedValue.ToString());
            objClientLevel.Note = this.txtNote.Text.Trim();
            objClientLevel.CreateUserId = objSessionUser.UserID;
            //objClientLevel.CreateTime = DateTime.Now.Date;
            objClientLevel.OprDeptID = objSessionUser.OprDeptID;
            objClientLevel.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Insert(objClientLevel) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            this.BindData();
            foreach (GridViewRow grv in GrdVewCreditLevel.Rows)
            {
                grv.BackColor = Color.White;
            }
            
        }
        this.ClearText();
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        this.txtLevelName.Text = "";
        this.txtNote.Text = "";
        this.ddlStatus.SelectedValue = "1";
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["ClientLevelId"] != null && ViewState["ClientLevelId"].ToString() != "")
        {
            ClientLevel objClientLevel = new ClientLevel();
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select ClientLevelName from ClientLevel where ClientLevelName='" + txtLevelName.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objBaseBo.WhereClause = "ClientLevelId=" + ViewState["ClientLevelId"].ToString();
                objClientLevel.ClientLevelName = this.txtLevelName.Text.Trim();
                objClientLevel.Status = Int32.Parse(this.ddlStatus.SelectedValue.ToString());
                objClientLevel.Note = this.txtNote.Text.Trim();
                objClientLevel.ModifyUserId = objSessionUser.UserID;
                //objClientLevel.ModifyTime = DateTime.Now.Date;
                objClientLevel.OprDeptID = objSessionUser.OprDeptID;
                objClientLevel.OprRoleID = objSessionUser.OprRoleID;
                if (objBaseBo.Update(objClientLevel) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                    return;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
                this.ClearText();
                ViewState["ClientLevelId"] = "";
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                this.BindData();
                foreach (GridViewRow grv in GrdVewCreditLevel.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotShop_CustomerLayerName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtLevelName.select()", true);
                this.BindData();
            }
        }
        
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
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/PotCust/ClientLevel.aspx");
    }
    protected void GrdVewCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "ClientLevelId=" + this.GrdVewCreditLevel.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(new ClientLevel());
        if (rs.Count == 1)
        {
            ClientLevel objClientLevel = rs.Dequeue() as ClientLevel;
            ViewState["ClientLevelId"] = objClientLevel.ClientLevelId;
            this.txtLevelName.Text = objClientLevel.ClientLevelName;
            this.txtNote.Text = objClientLevel.Note;
            this.ddlStatus.SelectedValue = objClientLevel.Status.ToString();
        }
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        Session["editLog"] = txtLevelName.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.BindData();
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
