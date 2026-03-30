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
using Base;
using Lease.Formula;
using Base.Page;
using BaseInfo.User;

public partial class Lease_CurrencyParam_CurrencyType : BasePage
{
    public string baseInfo;
    public string enterInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Session["editLog"]="";
            BindDrop();
            BindGV();
            btnEdit.Enabled = false;
            btnSave.Attributes.Add("onclick","return InputValidator(form1)");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblCurrencyType");
            enterInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblEnterCurTypeName");
        }
    }

    private void BindDrop()
    {
        //绑定币种状态
        dropCurTypeStatus.Items.Clear();
        int[] curTypeStatus = CurrencyType.GetCurTypeStatus();
        for (int i = 0; i < curTypeStatus.Length; i++)
        {
            dropCurTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CurrencyType.GetCurTypeStatusDesc(curTypeStatus[i])), curTypeStatus[i].ToString()));
        }

        //绑定是否本币
        dropIsLocal.Items.Clear();
        int[] isLocal = CurrencyType.GetIsLocal();
        for (int i = 0; i < isLocal.Length; i++)
        {
            dropIsLocal.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CurrencyType.GetIsLocalDesc(isLocal[i])), isLocal[i].ToString()));
        }
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        baseBo.OrderBy = "CurTypeName";
        DataTable dt = baseBo.QueryDataSet(new CurrencyType()).Tables[0];

        int count = dt.Rows.Count;
        dt.Columns.Add("curTypeStatusName");
        dt.Columns.Add("isLocalName");
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["curTypeStatusName"] = (String)GetGlobalResourceObject("Parameter", CurrencyType.GetCurTypeStatusDesc(Convert.ToInt32(dt.Rows[j]["CurTypeStatus"])));
            dt.Rows[j]["isLocalName"] = (String)GetGlobalResourceObject("Parameter", CurrencyType.GetIsLocalDesc(Convert.ToInt32(dt.Rows[j]["IsLocal"])));
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvCurrencyType.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvCurrencyType.DataSource = pds;
            gvCurrencyType.DataBind();
        }
        else
        {
            //gvCurrencyType.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 11;
            //lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            //pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
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

            this.gvCurrencyType.DataSource = pds;
            this.gvCurrencyType.DataBind();
            spareRow = gvCurrencyType.Rows.Count;
            for (int i = 0; i < gvCurrencyType.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvCurrencyType.DataSource = pds;
            gvCurrencyType.DataBind();
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtCurTypeName.Text.ToString() != "")
        {
            CurrencyType curType = new CurrencyType();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];            
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            baseBO.WhereClause = "curtypename='" + txtCurTypeName.Text.Trim() + "'";
            rs = baseBO.Query(curType);
            if (rs.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_lblCurTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "。'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtCurTypeName.select()", true);
            }
            else
            {
                curType.CurTypeID = BaseApp.GetCurTypeID();
                curType.CurTypeName = txtCurTypeName.Text;
                curType.CurTypeStatus = Convert.ToInt32(dropCurTypeStatus.SelectedValue);
                curType.IsLocal = Convert.ToInt32(dropIsLocal.SelectedValue);
                curType.Note = txtNote.Text;
                curType.CreateUserId = sessionUser.UserID;
                if (baseBO.Insert(curType) == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                BindGV();
                txtCurTypeName.Text = "";
                txtNote.Text = "";
            }
        }
        ClearGridViewSelect();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (txtCurTypeName.Text.ToString() != "")
        {
            CurrencyType curType = new CurrencyType();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            BaseBO baseBO = new BaseBO();
            DataSet ds = new DataSet();
            ds = baseBO.QueryDataSet("select CurTypeName from CurrencyType where CurTypeName='" + txtCurTypeName.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                curType.CurTypeName = txtCurTypeName.Text;
                curType.CurTypeStatus = Convert.ToInt32(dropCurTypeStatus.SelectedValue);
                curType.IsLocal = Convert.ToInt32(dropIsLocal.SelectedValue);
                curType.Note = txtNote.Text;
                curType.ModifyUserId = sessionUser.UserID;
                baseBO.WhereClause = "CurTypeID = " + Convert.ToInt32(ViewState["curTypeID"]);
                if (baseBO.Update(curType) == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                BindGV();
                txtCurTypeName.Text = "";
                txtNote.Text = "";
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                ClearGridViewSelect();
            }
            else 
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_lblCurTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "。'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtCurTypeName.select()", true);
                BindGV();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Lease_lblCurTypeName") + "不能为空。'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtCurTypeName.select()", true);
            BindGV();
        }
            
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtCurTypeName.Text = "";
        txtNote.Text = "";
        BindDrop();
        BindGV();
        ViewState["curTypeID"] = "";
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        ClearGridViewSelect();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
    }

    protected void gvCurrencyType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[5].Text = "";
            }
        }
    }
    protected void gvCurrencyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        int curTypeID = 0;
        curTypeID = Convert.ToInt32(gvCurrencyType.SelectedRow.Cells[0].Text);
        ViewState["curTypeID"] = curTypeID;
        baseBO.WhereClause = "curTypeID=" + ViewState["curTypeID"];
        rs=baseBO.Query(new CurrencyType());
        if(rs.Count==1)
        {
            CurrencyType currencyType=rs.Dequeue()as CurrencyType;
        // txtCurTypeName.Text = gvCurrencyType.SelectedRow.Cells[1].Text;
                    txtCurTypeName.Text = currencyType.CurTypeName;
        }
        //BaseBO baseBO = new BaseBO();
        //int curTypeID = 0;
        //curTypeID = Convert.ToInt32(gvCurrencyType.SelectedRow.Cells[0].Text);
        //ViewState["curTypeID"] = curTypeID;
        //txtCurTypeName.Text = gvCurrencyType.SelectedRow.Cells[1].Text;
        dropCurTypeStatus.SelectedValue = gvCurrencyType.SelectedRow.Cells[6].Text;
        dropIsLocal.SelectedValue = gvCurrencyType.SelectedRow.Cells[7].Text;
        if (gvCurrencyType.SelectedRow.Cells[4].Text == "&nbsp;")
        {
            txtNote.Text = "";
        }
        else
        {
            txtNote.Text = gvCurrencyType.SelectedRow.Cells[4].Text;
        }
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        Session["editLog"] = txtCurTypeName.Text;
        BindGV();
    }


 private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in gvCurrencyType.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
    }
    protected void gvCurrencyType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
}
