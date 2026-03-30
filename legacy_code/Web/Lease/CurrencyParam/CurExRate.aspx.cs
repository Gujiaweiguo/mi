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

public partial class Lease_CurrencyParam_CurExRate : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindGV();
            txtDate.Text = DateTime.Now.ToShortDateString();
            btnEdit.Enabled = false;
            BindDrop();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblCurExRate");
        }
        int dfg = 0;
        btnSave.Attributes.Add("onclick","return CheckData()");
    }

    private void BindDrop()
    {
        //绑定本币种
        BaseBO baseBO = new BaseBO();
        dropCurTypeID.Items.Clear();
        baseBO.WhereClause = "IsLocal = " + CurrencyType.ISLOCAL_YES + " and CurTypeStatus = " + CurrencyType.CURTYPESTATUS_YES;
        Resultset rs = baseBO.Query(new CurrencyType());
        foreach (CurrencyType curType in rs)
        {
            dropCurTypeID.Items.Add(new ListItem(curType.CurTypeName.ToString(), curType.CurTypeID.ToString()));
        }
        
        //绑定目标币种
        baseBO.WhereClause = "CurTypeStatus = " + CurrencyType.CURTYPESTATUS_YES;
        DropToCurTypeID.Items.Clear();
        Resultset tempRs = baseBO.Query(new CurrencyType());
        foreach (CurrencyType curType in tempRs)
        {
            DropToCurTypeID.Items.Add(new ListItem(curType.CurTypeName.ToString(), curType.CurTypeID.ToString()));
        }

        //绑定汇率状态
        int[] exRateStatus = CurExRate.GetExRateStatus();
        dropStatus.Items.Clear();        
        for (int i = 0; i < exRateStatus.Length; i++)
        {
            dropStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", CurExRate.GetExRateStatusDesc(exRateStatus[i])), exRateStatus[i].ToString()));
        }

    }
    protected void gvCurExRate_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[7].Text = "";
            }
        }
    }
    protected void gvCurExRate_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        
        int toCurTypeID = 0;

        Resultset rs = new Resultset();
        
        CurExRate curExRate = new CurExRate();
        
        int curExRateID = Convert.ToInt32(gvCurExRate.SelectedRow.Cells[0].Text);

        baseBO.WhereClause = "ExRateID = " + curExRateID;
        
        rs = baseBO.Query(curExRate);

        curExRate = rs.Dequeue() as CurExRate;

        dropCurTypeID.SelectedValue = curExRate.CurTypeID.ToString();
        DropToCurTypeID.SelectedValue = curExRate.ToCurTypeID.ToString();
        dropStatus.SelectedValue = curExRate.Status.ToString();

        if (gvCurExRate.SelectedRow.Cells[4].Text == "&nbsp;")
        {
            txtExRate.Text = "";
        }
        else
        {
            txtExRate.Text = gvCurExRate.SelectedRow.Cells[4].Text;
        }
        if (gvCurExRate.SelectedRow.Cells[5].Text == "&nbsp;")
        {
            txtDate.Text = "";
        }
        else
        {            
            txtDate.Text = gvCurExRate.SelectedRow.Cells[5].Text;
        }
        if (gvCurExRate.SelectedRow.Cells[6].Text == "&nbsp;")
        {
            txtNote.Text = "";
        }
        else
        {            
            txtNote.Text = gvCurExRate.SelectedRow.Cells[6].Text;
        }
        ViewState["exRateID"] = gvCurExRate.SelectedRow.Cells[0].Text;
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        BindGV();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtExRate.Text == "" || txtExRate.Text == "0")
        {
            ClearGridViewSelect();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '汇率不能为零。'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtExRate.select()", true);
            return;
        }
        else
        {
            CurExRate curExRate = new CurExRate();
            //BaseBO baseBo = new BaseBO();
            //Resultset rs = new Resultset();
            //baseBo.WhereClause = "select CurTypeID from CurExRate where CurTypeID=" + Convert.ToInt32(dropCurTypeID.SelectedValue) + " and ToCurTypeID=" + Convert.ToInt32(DropToCurTypeID.SelectedValue) + " and ExRateDate='" + Convert.ToDateTime(txtDate.Text) + "'");
            //rs = baseBo.Query(curExRate);
            //rs=baseBo.Query("select * from CurExRate where CurTypeID=" + Convert.ToInt32(dropCurTypeID.SelectedValue) + " and ToCurTypeID=" + Convert.ToInt32(DropToCurTypeID.SelectedValue) + " and ExRateDate='" + Convert.ToDateTime(txtDate.Text) + "'");
            //DataSet ds = new DataSet();
            ////ds = baseBo.QueryDataSet("select * from CurExRate where CurTypeID=" + Convert.ToInt32(dropCurTypeID.SelectedValue) + " and ToCurTypeID=" + Convert.ToInt32(DropToCurTypeID.SelectedValue) + " and ExRateDate='"+ Convert.ToDateTime(txtDate.Text.ToString()) +"'");
            //if (rs.Count > 0)
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '本汇率已存在。'", true);
            //    BindGV();
            //    return;
            //}
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            curExRate.CreateUserId = sessionUser.UserID;
                curExRate.ExRateID = BaseApp.GetExRateID();
                curExRate.CurTypeID = Convert.ToInt32(dropCurTypeID.SelectedValue);
                curExRate.ToCurTypeID = Convert.ToInt32(DropToCurTypeID.SelectedValue);
                curExRate.Status = Convert.ToInt32(dropStatus.SelectedValue);
                curExRate.ExRate = Convert.ToDecimal(txtExRate.Text);
                curExRate.ExRateDate = Convert.ToDateTime(txtDate.Text);
                curExRate.Note = txtNote.Text;
                BaseBO baseBO = new BaseBO();
                if (baseBO.Insert(curExRate) == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
            
        }
        txtExRate.Text = "";
        txtDate.Text = "";
        txtNote.Text = "";
        BindDrop();
        BindGV();
        ClearGridViewSelect();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (txtExRate.Text == "" || txtExRate.Text == "0")
        {
            ClearGridViewSelect();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '汇率不能为零。'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtExRate.select()", true);
            return;
        }
        else
        {
            CurExRate curExRate = new CurExRate();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            curExRate.ModifyUserId = sessionUser.UserID;
            curExRate.CurTypeID = Convert.ToInt32(dropCurTypeID.SelectedValue);
            curExRate.ToCurTypeID = Convert.ToInt32(DropToCurTypeID.SelectedValue);
            curExRate.Status = Convert.ToInt32(dropStatus.SelectedValue);
            curExRate.ExRate = Convert.ToDecimal(txtExRate.Text);
            curExRate.ExRateDate = Convert.ToDateTime(txtDate.Text);
            curExRate.Note = txtNote.Text;
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ExRateID = " + Convert.ToInt32(ViewState["exRateID"]);
            if (baseBO.Update(curExRate) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            txtExRate.Text = "";
            txtDate.Text = "";
            txtNote.Text = "";
            BindDrop();
        }
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        BindGV();
        ClearGridViewSelect();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtExRate.Text = "";
        //txtDate.Text = "";
        txtNote.Text = "";
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        BindDrop();
        BindGV();
        ClearGridViewSelect();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);

    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        DataTable dt = baseBo.QueryDataSet(new CurExRate()).Tables[0];

        int count = dt.Rows.Count;
        dt.Columns.Add("CurTypeName");
        dt.Columns.Add("ToCurTypeName");
        dt.Columns.Add("StatusName");
        //本币种
        BaseBO basebo = new BaseBO();
        for (int j = 0; j < count; j++)
        {
            basebo.WhereClause = "CurTypeID = " + Convert.ToInt32(dt.Rows[j]["CurTypeID"]);
            Resultset curTypeNameRS = basebo.Query(new CurrencyType());
            CurrencyType curType = curTypeNameRS.Dequeue() as CurrencyType;
            dt.Rows[j]["CurTypeName"] = curType.CurTypeName;
        }

        //目标币种
        for (int j = 0; j < count; j++)
        {
            basebo.WhereClause = "";
            basebo.WhereClause = "CurTypeID = " + Convert.ToInt32(dt.Rows[j]["ToCurTypeID"]);
            Resultset curTypeNameRS = basebo.Query(new CurrencyType());
            CurrencyType curType = curTypeNameRS.Dequeue() as CurrencyType;
            dt.Rows[j]["ToCurTypeName"] = curType.CurTypeName;
        }

        //状态
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["StatusName"] = (String)GetGlobalResourceObject("Parameter", CurExRate.GetExRateStatusDesc(Convert.ToInt32(dt.Rows[j]["Status"])));
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvCurExRate.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvCurExRate.DataSource = pds;
            gvCurExRate.DataBind();
        }
        else
        {
            //gvCurExRate.EmptyDataText = "";
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

            this.gvCurExRate.DataSource = pds;
            this.gvCurExRate.DataBind();
            spareRow = gvCurExRate.Rows.Count;
            for (int i = 0; i < gvCurExRate.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvCurExRate.DataSource = pds;
            gvCurExRate.DataBind();
        }
        ClearGridViewSelect();
    }
    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in gvCurExRate.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[7].Text = "";
            }
        }
    }

    protected void gvCurExRate_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
