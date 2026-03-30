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
using BaseInfo.Dept;

public partial class BaseInfo_Dept_DeptType : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            baseInfo = (string)GetGlobalResourceObject("BaseInfo", "Dept_lblDeptType");
            BindGV();
        }
        this.btnSave.Attributes.Add("onclick", "return CheckData()");
    }

    private void BindGV()
    {
        BaseBO basebo = new BaseBO();
        DataSet ds = basebo.QueryDataSet(new DeptTypePO());
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
            GrdVewDeptType.DataSource = pds;
            GrdVewDeptType.DataBind();

            ss = GrdVewDeptType.Rows.Count;
            for (int i = 0; i < GrdVewDeptType.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;

        }
        GrdVewDeptType.DataSource = pds;
        GrdVewDeptType.DataBind();
    }
    protected void GrdVewStoreType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        int deptTypeID = 0;
        deptTypeID = Convert.ToInt32(GrdVewDeptType.SelectedRow.Cells[0].Text);
        ViewState["DeptTypeID"] = deptTypeID;
        baseBO.WhereClause = "DeptType=" + deptTypeID;

        rs = baseBO.Query(new DeptTypePO());
        if (rs.Count == 1)
        {
            DeptTypePO deptType = rs.Dequeue() as DeptTypePO;
            txtDeptTypeName.Text = deptType.DeptTypeName;
        }
        ViewState["editLog"] = txtDeptTypeName.Text;
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
                e.Row.Cells[2].Text = "";
            }
        }
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtDeptTypeName.Text = "";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.BindGV();
        btnCancel.Enabled = true;
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DeptTypePO deptType = new DeptTypePO();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "DeptTypeName='" + txtDeptTypeName.Text.Trim() + "'";
        Resultset rs = baseBO.Query(deptType);
        if (rs.Count < 1)
        {
            if (baseBO.QueryDataSet("select max(DeptType) from DeptType").Tables[0].Rows[0][0].ToString() != "")
            {
                deptType.DeptType = Convert.ToInt32(baseBO.QueryDataSet("select max(DeptType) from DeptType").Tables[0].Rows[0][0]) + 1;
            }
            else
            {
                deptType.DeptType = 1;
            }
            deptType.DeptTypeName = txtDeptTypeName.Text.Trim();

            if (baseBO.Insert(deptType) != -1)
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
            ViewState["DeptTypeID"] = "";
            txtDeptTypeName.Text = "";
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Dept_lblDeptType") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "document.all.txtDeptTypeName.select()", true);
            this.BindGV();
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["DeptTypeID"] == null || ViewState["DeptTypeID"].ToString() == "")
        {
            this.BindGV();
            return;
        }
        DeptTypePO deptType = new DeptTypePO();
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBO.QueryDataSet("select DeptTypeName from DeptType where DeptTypeName='" + txtDeptTypeName.Text.Trim() + "'");
        if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == ViewState["editLog"].ToString())
        {
            baseBO.WhereClause = "DeptType=" + Convert.ToInt32(ViewState["DeptTypeID"]);
            deptType.DeptTypeName = txtDeptTypeName.Text.Trim();

            if (baseBO.Update(deptType) != -1)
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
            txtDeptTypeName.Text = "";
            BindGV();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Dept_lblDeptType") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "document.all.txtDeptTypeName.select()", true);
            BindGV();
        }
    }
}
