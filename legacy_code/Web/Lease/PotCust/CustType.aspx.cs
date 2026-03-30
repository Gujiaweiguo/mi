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
using WorkFlow.WrkFlw;
using Base.Biz;
using Base.Page;
using Base.DB;
using Base.Page;
using Base;
using System.Text;
using Lease.PotCust;
using BaseInfo.User;

public partial class Lease_PotCust_CustType : BasePage
{
    public string baseInfo;
    DataTable dt = new DataTable();
    PagedDataSource pds = new PagedDataSource();
    BaseBO basebo = new BaseBO();
    private int numCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int[] status = CustType.GetCustTypeStatus();
            cmbCustTypeStatus.Items.Clear();
            for (int i = 0; i < status.Length; i++)
            {
                cmbCustTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",CustType.GetCustTypeStatusDesc(status[i])), status[i].ToString()));
            }
            ViewState["currentCount"] = 1;
           page();
           baseInfo = (String)GetGlobalResourceObject("BaseInfo", "User_UserType");
            //GridViewData();
           this.btnEdit.Enabled = false;
        }
        btnSave.Attributes.Add("onclick", "return CheckData()");
        
    }

    private void GridViewData()
    {
        GrdVewCustType.DataSource = new BaseBO().Query(new CustType());
        GrdVewCustType.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        CustType custType = new CustType();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "CustTypeCode='" + txtCustTypeCode.Text.Trim() + "'";
        Resultset rs = baseBO.Query(custType);
        if (rs.Count < 1)
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            custType.CreateUserId = sessionUser.UserID;
            custType.CustTypeID = BaseApp.GetCustTypeID();
            custType.CustTypeCode = txtCustTypeCode.Text.Trim();
            custType.CustTypeName = txtCustTypeName.Text.Trim();
            custType.CustTypeStatus = Convert.ToInt32(cmbCustTypeStatus.SelectedValue);
            custType.Note = txtNote.Text.Trim();

            if (baseBO.Insert(custType) != -1)
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
            page();
            ViewState["CustTypeID"] = "";
            txtCustTypeCode.Text = "";
            txtCustTypeName.Text = "";
            txtNote.Text = "";

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "document.all.txtCustTypeCode.select()", true);
            this.page();
        }
        
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["CustTypeID"] == null || ViewState["CustTypeID"].ToString() == "")
        {
            this.page();
            return;
        }
        CustType custType = new CustType();
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBO.QueryDataSet("select CustTypeCode from CustType where CustTypeCode='" + txtCustTypeCode.Text.Trim()+"'");
        if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
        {
            baseBO.WhereClause = "CustTypeID=" + Convert.ToInt32(ViewState["CustTypeID"]);
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            custType.ModifyUserId = sessionUser.UserID;
            custType.CustTypeCode = txtCustTypeCode.Text.Trim();
            custType.CustTypeName = txtCustTypeName.Text.Trim();
            custType.CustTypeStatus = Convert.ToInt32(cmbCustTypeStatus.SelectedValue);
            custType.Note = txtNote.Text.Trim();

            if (baseBO.Update(custType) != -1)
            {
                //Response.Redirect("CustType.aspx");
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            this.btnEdit.Enabled = false;
            this.btnSave.Enabled = true;
            ViewState["CustTypeID"] = "";
            txtCustTypeCode.Text = "";
            txtCustTypeName.Text = "";
            txtNote.Text = "";
            page();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "document.all.txtCustTypeCode.select()", true);
            page();
        }
        
    }

    protected void GrdVewCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        int custTypeID = 0;
        custTypeID = Convert.ToInt32(GrdVewCustType.SelectedRow.Cells[0].Text);
        ViewState["CustTypeID"] = custTypeID;
        baseBO.WhereClause = "CustTypeID=" + custTypeID;

        rs = baseBO.Query(new CustType());
        if (rs.Count == 1)
        {

            CustType custtype = rs.Dequeue() as CustType;
            txtCustTypeCode.Text = custtype.CustTypeCode;
            txtCustTypeName.Text = custtype.CustTypeName;
            cmbCustTypeStatus.SelectedValue = custtype.CustTypeStatus.ToString();
            txtNote.Text = custtype.Note;

        }
        Session["editLog"] = txtCustTypeCode.Text;
        this.btnEdit.Enabled = true;
        this.btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        page();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ViewState["CustTypeID"] = "";
        txtCustTypeCode.Text = "";
        txtCustTypeName.Text = "";
        txtNote.Text = "";
        this.btnSave.Enabled = true;
        this.btnEdit.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        page();
    }

    protected void page()
    {
        DataSet ds = basebo.QueryDataSet(new CustType());
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        int ss = 0;
        //获取状态




        //for (int j = 0; j < count; j++)
        //{
        //    string custTypeStatusName = CustType.GetCustTypeStatusDesc(Convert.ToInt32(dt.Rows[j]["CustTypeStatus"].ToString()));
        //    dt.Rows[j]["CustTypeStatusName"] = custTypeStatusName;
        //}

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
            //GrdVewCustType.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 11;
            //pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCount"]) - 1;
            //if (pds.IsFirstPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = true;
            //}
            GrdVewCustType.DataSource = pds;
            GrdVewCustType.DataBind();

            ss = GrdVewCustType.Rows.Count;
            for (int i = 0; i < GrdVewCustType.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
           
        }
        GrdVewCustType.DataSource = pds;
        GrdVewCustType.DataBind();
      
        //for (int j = 0; j < pds.PageSize - ss; j++)
        //    GrdVewCustType.Rows[(pds.PageSize - 1) - j].Cells[5].Text = "";

    }

    protected void GrdVewCustType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //GridViewRow row = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal); 
        ////row.Style.Add(HtmlTextWriterStyle.FontSize
        ////GridView1.Rows[i].Attributes.Add("bgcolor", "red"); 
        
        //GrdVewCustType.Rows[i].Cells[2].HorizontalAlign = "Center";
        //GrdVewCustType.Rows[i].Cells[2].Font = 
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text != "&nbsp;")
            {

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
            else
            {
                //e.Row.Cells[4].Text = "";
                e.Row.Cells[5].Text = "";
            }
        }
    }
    protected void GrdVewCustType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.page();
    }
}
