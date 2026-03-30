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
using Lease.Contract;
using BaseInfo.User;
using System.Drawing;

public partial class Lease_AdContract_ContractType : BasePage
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
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Contract_ContractTypeVindicate");
            btnEdit.Enabled = false;
        }
    }
    /// <summary>
    /// 为按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        //this.btnCel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnCel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        this.btnSave.Attributes.Add("onclick", "return allTextBoxValidator()");
        this.btnEdit.Attributes.Add("onclick", "return allTextBoxValidator()");
    }
    /// <summary>
    /// 绑定是否有效下拉框
    /// </summary>
    private void BindSourceTypeStatus()
    {
        int[] status = CustType.GetCustTypeStatus();
        this.ddlStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            this.ddlStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定DataGrid
    /// </summary>
    //private void BindData(int iCurrentpage)
    //{
    //    BaseBO objBaseBo = new BaseBO();
    //    ContractType objContractType = new ContractType();
    //    BaseInfo.BaseCommon.BindGridView(objBaseBo, objContractType, 10, iCurrentpage, this.btnBack, this.btnNext, this.GrdVewType);
    //}
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        ContractType objContractType = new ContractType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objContractType, this.GrdVewType);
    }
    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    //ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
    //    //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
    //}
    ///// <summary>
    ///// 下一页
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    //ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
    //    //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
    //}
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ContractType objContractType = new ContractType();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = objBaseBo.QueryDataSet("select ContractTypeCode from ContractType where ContractTypeCode='" + txtCode.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtCode.select()", true);
            this.BindData();
        }
        else
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objContractType.ContractTypeID = Base.BaseApp.GetID("ContractType", "ContractTypeID");
            objContractType.ContractTypeCode = this.txtCode.Text.Trim();
            objContractType.ContractTypeName = this.txtName.Text.Trim();
            try { objContractType.ContractTypeStatus = Convert.ToInt32(this.ddlStatus.SelectedValue); }
            catch { objContractType.ContractTypeStatus = 0; }
            objContractType.Note = this.txtNote.Text.Trim();
            objContractType.CreateUserId = objSessionUser.UserID;
            //objContractType.CreateTime = DateTime.Now.Date;
            objContractType.OprDeptID = objSessionUser.OprDeptID;
            objContractType.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Insert(objContractType) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            this.ClearText();
            this.BindData();
            foreach (GridViewRow grv in GrdVewType.Rows)
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
        this.txtName.Text = "";
        this.txtNote.Text = "";
        this.txtCode.Text = "";
        this.ddlStatus.SelectedValue = "1";
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["TypeID"] != null && ViewState["TypeID"].ToString() != "")
        {
            ContractType objContractType = new ContractType();
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select ContractTypeCode from ContractType where ContractTypeCode='" + txtCode.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objBaseBo.WhereClause = "ContractTypeID=" + Int32.Parse(ViewState["TypeID"].ToString());
                objContractType.ContractTypeCode = this.txtCode.Text.Trim();
                objContractType.ContractTypeName = this.txtName.Text.Trim();
                objContractType.ContractTypeStatus = Int32.Parse(ddlStatus.SelectedValue);
                objContractType.Note = this.txtNote.Text.Trim();

                objContractType.ModifyUserId = objSessionUser.UserID;
                //objContractType.ModifyTime = DateTime.Now.Date;
                objContractType.OprDeptID = objSessionUser.OprDeptID;
                objContractType.OprRoleID = objSessionUser.OprRoleID;
                if (objBaseBo.Update(objContractType) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                    return;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
                this.ClearText();
                ViewState["TypeID"] = "";
                this.BindData();
                btnEdit.Enabled = false;
                btnSave.Enabled = true;
                foreach (GridViewRow grv in GrdVewType.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtCode.select()", true);
                this.BindData();
            }
        }
       
    }
    protected void GrdVewType_RowDataBound(object sender, GridViewRowEventArgs e)
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
    protected void GrdVewType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        ContractType objContractType = new ContractType();
        objBaseBo.WhereClause = "ContractTypeID="+this.GrdVewType.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(objContractType);
        if (rs.Count == 1)
        {
            ContractType objContract = rs.Dequeue() as ContractType;
            ViewState["TypeID"] = objContract.ContractTypeID;
            this.txtCode.Text = objContract.ContractTypeCode;
            this.txtName.Text = objContract.ContractTypeName;
            this.txtNote.Text = objContract.Note;
            this.ddlStatus.SelectedValue = objContract.ContractTypeStatus.ToString();
        }
        this.BindData();
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        Session["editLog"] = txtCode.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/AdContract/ContractType.aspx");

    }
    protected void GrdVewType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        foreach (GridViewRow grv in GrdVewType.Rows)
        {
            grv.BackColor = Color.White;
        }

    }
}
