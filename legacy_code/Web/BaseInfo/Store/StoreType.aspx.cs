using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Base;
using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.Store;
using BaseInfo.User;

public partial class BaseInfo_Store_StoreType : BasePage
{
    public string baseInfo;    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (string)GetGlobalResourceObject("BaseInfo", "Store_StoreType");
            BindGV();
        }
        this.btnSave.Attributes.Add("onclick", "return CheckData()");
    }

    private void BindGV()
    {
        BaseBO basebo = new BaseBO();
        DataSet ds = basebo.QueryDataSet(new StoreType());
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        int ss = 0;
        pds.DataSource = dt.DefaultView;
        if (pds.Count < 1)
        {
            for (int i = 0; i < 9; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.PageSize = 9;
            pds.DataSource = dt.DefaultView;
        }
        else
        {
            GrdVewStoreType.DataSource = pds;
            GrdVewStoreType.DataBind();

            ss = GrdVewStoreType.Rows.Count;
            for (int i = 0; i < GrdVewStoreType.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;

        }
        GrdVewStoreType.DataSource = pds;
        GrdVewStoreType.DataBind();
    }
    protected void GrdVewStoreType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        int storeTypeID = 0;
        storeTypeID = Convert.ToInt32(GrdVewStoreType.SelectedRow.Cells[0].Text);
        ViewState["StoreTypeID"] = storeTypeID;
        baseBO.WhereClause = "StoreTypeID=" + storeTypeID;

        rs = baseBO.Query(new StoreType());
        if (rs.Count == 1)
        {

            StoreType storetype = rs.Dequeue() as StoreType;
            txtStoreTypeCode.Text = storetype.StoreTypeCode;
            txtStoreTypeName.Text = storetype.StoreTypeName;
        }
        ViewState["editLog"] = txtStoreTypeCode.Text;
        this.btnEdit.Enabled = true;
        this.btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        BindGV();
    }
    protected void GrdVewStoreType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindGV();
    }
    protected void GrdVewStoreType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;" || e.Row.Cells[1].Text == "")
            {
                e.Row.Cells[3].Text = "";
            }
        }
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtStoreTypeCode.Text = "";
        txtStoreTypeName.Text = "";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.BindGV();
        btnCancel.Enabled = true;
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        StoreType storeType = new StoreType();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "StoreTypeCode='" + txtStoreTypeCode.Text.Trim() + "'";
        Resultset rs = baseBO.Query(storeType);
        if (rs.Count < 1)
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            storeType.CreateUserId = sessionUser.UserID;
            int aa = baseBO.QueryDataSet("select max(StoreTypeID) from StoreType").Tables[0].Rows.Count;
            if (baseBO.QueryDataSet("select max(StoreTypeID) from StoreType").Tables[0].Rows[0][0].ToString() != "")
            {
                storeType.StoreTypeId = Convert.ToInt32(baseBO.QueryDataSet("select max(StoreTypeID) from StoreType").Tables[0].Rows[0][0]) + 1;
            }
            else
            {
                storeType.StoreTypeId = 1;
            }
            storeType.StoreTypeCode = txtStoreTypeCode.Text.Trim();
            storeType.StoreTypeName = txtStoreTypeName.Text.Trim();

            if (baseBO.Insert(storeType) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('添加成功')", true);
                //Response.Redirect("CustType.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '添加失败。'", true);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('添加失败')", true);
            }
            BindGV();
            ViewState["StoreTypeID"] = "";
            txtStoreTypeCode.Text = "";
            txtStoreTypeName.Text = "";
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "document.all.txtStoreTypeCode.select()", true);
            this.BindGV();
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["StoreTypeID"] == null || ViewState["StoreTypeID"].ToString() == "")
        {
            this.BindGV();
            return;
        }
        StoreType storeType = new StoreType();
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBO.QueryDataSet("select StoreTypeCode from StoreType where StoreTypeCode='" + txtStoreTypeCode.Text.Trim() + "'");
        if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == ViewState["editLog"].ToString())
        {
            baseBO.WhereClause = "StoreTypeID=" + Convert.ToInt32(ViewState["StoreTypeID"]);
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            storeType.ModifyUserId = sessionUser.UserID;
            storeType.StoreTypeCode = txtStoreTypeCode.Text.Trim();
            storeType.StoreTypeName = txtStoreTypeName.Text.Trim();

            if (baseBO.Update(storeType) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            this.btnEdit.Enabled = false;
            this.btnSave.Enabled = true;
            ViewState["StoreTypeID"] = "";
            txtStoreTypeCode.Text = "";
            txtStoreTypeName.Text = "";
            BindGV();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "document.all.txtStoreTypeCode.select()", true);
            BindGV();
        }
    }
}
