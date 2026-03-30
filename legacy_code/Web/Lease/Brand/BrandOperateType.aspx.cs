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
using Lease.Brand;
using Base.Biz;
using Base.DB;
using Lease.PotCust;
using BaseInfo.User;
using Base.Page;
using System.Drawing;

public partial class Lease_Brand_BrandOperateType : BasePage
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
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BrandOperateType");
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
        BrandOperateType objBrandOperate = new BrandOperateType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objBrandOperate, 10, iCurrentpage, this.btnBack, this.btnNext, this.GrdBrandOperateType);
    }
    private void BindData()
    {
        BaseBO objBaseBo = new BaseBO();
        BrandOperateType objBrandOperate = new BrandOperateType();
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objBrandOperate, this.GrdBrandOperateType);
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BrandOperateType objBrandOperate = new BrandOperateType();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = objBaseBo.QueryDataSet("select OperateName from BrandOperateType where OperateName='" + txtOperateName.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "CustType_lblCustTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtOperateName.select()", true);
            BindData();
        }
        else
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            objBrandOperate.OperateTypeId = Base.BaseApp.GetID("BrandOperateType", "OperateTypeId");
            objBrandOperate.OperateName = this.txtOperateName.Text.Trim();
            objBrandOperate.Status = Int32.Parse(this.cmbTypeStatus.SelectedValue.ToString());
            objBrandOperate.Note = this.txtNote.Text.Trim();
            objBrandOperate.CreateUserId = objSessionUser.UserID;
            //objBrandOperate.CreateTime = DateTime.Now.Date;
            //objBrandOperate.ModifyUserId = objSessionUser.ModifyUserID;
            //objBrandOperate.ModifyTime = DateTime.Now.Date;
            objBrandOperate.OprDeptID = objSessionUser.OprDeptID;
            objBrandOperate.OprRoleID = objSessionUser.OprRoleID;
            if (objBaseBo.Insert(objBrandOperate) <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            this.BindData();
            this.ClearText();
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
        this.txtOperateName.Text = "";
        this.txtNote.Text = "";
        this.cmbTypeStatus.SelectedValue = "1";
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["OperateTypeId"] != null && ViewState["OperateTypeId"].ToString() != "")
        {
            BrandOperateType objBrandOperate = new BrandOperateType();
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select OperateName from BrandOperateType where OperateName='" + txtOperateName.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                objBaseBo.WhereClause = "OperateTypeId=" + ViewState["OperateTypeId"].ToString();

                objBrandOperate.OperateName = this.txtOperateName.Text.Trim();
                objBrandOperate.Status = Int32.Parse(this.cmbTypeStatus.SelectedValue.ToString());
                objBrandOperate.Note = this.txtNote.Text.Trim();
                //objBrandOperate.CreateUserId = objSessionUser.CreateUserID;
                //objBrandOperate.CreateTime = DateTime.Now.Date;
                objBrandOperate.ModifyUserId = objSessionUser.UserID;
                //objBrandOperate.ModifyTime = DateTime.Now.Date;
                objBrandOperate.OprDeptID = objSessionUser.OprDeptID;
                objBrandOperate.OprRoleID = objSessionUser.OprRoleID;
                if (objBaseBo.Update(objBrandOperate) <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                    return;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
                this.ClearText();
                ViewState["OperateTypeId"] = "";
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtOperateName.select()", true);
                BindData();
            }
        }

    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/Brand/BrandOperateType.aspx");
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
        BrandOperateType objBrand = new BrandOperateType();
        objBaseBo.WhereClause = "OperateTypeId=" + this.GrdBrandOperateType.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = objBaseBo.Query(objBrand);
        if (rs.Count == 1)
        {
            BrandOperateType objBrandOperate = rs.Dequeue() as BrandOperateType;
            ViewState["OperateTypeId"] = objBrandOperate.OperateTypeId;
            this.txtOperateName.Text = objBrandOperate.OperateName;
            this.txtNote.Text = objBrandOperate.Note;
            this.cmbTypeStatus.SelectedValue = objBrandOperate.Status.ToString();
        }
        //this.BindData(Convert.ToInt32(ViewState["currentCount"].ToString()));
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        Session["editLog"] = txtOperateName.Text;
        this.BindData();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
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
