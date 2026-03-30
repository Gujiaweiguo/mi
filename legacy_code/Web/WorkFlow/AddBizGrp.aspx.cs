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
public partial class WorkFlow_AddBizGrp : BasePage
{
    private int numCount = 0;
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region
        //btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        #endregion
        btnSave.Attributes.Add("onclick", "return BizGrpValidator(form1)");
        btnEdit.Attributes.Add("onclick", "return BizGrpValidator(form1)");
        if (!IsPostBack)
        {
            int[] status = BizGrp.GetBizGrpStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbBizGrpStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", BizGrp.GetBizGrpStatusDesc(status[i])), status[i].ToString()));
                baseInfo = (String)GetGlobalResourceObject("BaseInfo", "BizGrp_Title");
            }
            page();
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BizGrp objBizGrp = new BizGrp();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "BizGrpCode = '" + txtBizGrpCode.Text.ToString() + "'";
        Resultset rs = baseBO.Query(objBizGrp);
        if (rs.Count == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showlineError()", true);
            page();
            return;
        }
        else
        {
            objBizGrp.BizGrpID = BaseApp.GetBizGrpID();
            objBizGrp.BizGrpCode = txtBizGrpCode.Text.Trim();
            objBizGrp.BizGrpName = txtBizGrpName.Text.Trim();
            objBizGrp.BizGrpStatus = Convert.ToInt32(cmbBizGrpStatus.SelectedValue);
            objBizGrp.Note = txtNote.Text.Trim();

            if (baseBO.Insert(objBizGrp) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mes", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "showlineError()", true);
                page();
                return;
            }
            else
            {
                page();
                textClear();
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "showlineIns()", true);
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mes", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BizGrp objBizGrp = new BizGrp();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "BizGrpID =" + ViewState["BizGrpID"];
        objBizGrp.BizGrpCode = txtBizGrpCode.Text.Trim();
        objBizGrp.BizGrpName = txtBizGrpName.Text.Trim();
        objBizGrp.BizGrpStatus = Convert.ToInt32(cmbBizGrpStatus.SelectedValue);
        objBizGrp.Note = txtNote.Text.Trim();
        if (baseBO.Update(objBizGrp) < 1)
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "showlineError()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mes", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
            page();
            return;
        }
        else
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "showlineIns()", true);
            page();
            textClear();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mes", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        }

    }
    //protected void page()
    //{
    //    BaseBO baseBO = new BaseBO();
    //    Resultset rs = new Resultset();
    //    PagedDataSource pds = new PagedDataSource();
    //    int spareRow = 0;

    //    DataTable dt = baseBO.QueryDataSet(new BizGrp()).Tables[0];

    //    int count = dt.Rows.Count;
    //    for (int j = 0; j < count; j++)
    //    {
    //        string typeName = (String)GetGlobalResourceObject("Parameter", BizGrp.GetBizGrpStatusDesc(Convert.ToInt32(dt.Rows[j]["BizGrpStatus"].ToString())));
    //        dt.Rows[j]["BizGrpStatusName"] = typeName;
    //    }

    //    pds.DataSource = dt.DefaultView;

    //    if (pds.Count < 1)
    //    {
    //        for (int i = 0; i < GrdWrkGrp.PageSize; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;
    //        GrdWrkGrp.DataSource = pds;
    //        GrdWrkGrp.DataBind();
    //    }
    //    else
    //    {
    //        pds.AllowPaging = true;
    //        pds.PageSize = 8;
    //        lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
    //        pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;

    //        if (pds.IsFirstPage)
    //        {
    //            btnBack.Enabled = false;
    //            btnNext.Enabled = true;
    //        }

    //        if (pds.IsLastPage)
    //        {
    //            btnBack.Enabled = true;
    //            btnNext.Enabled = false;
    //        }

    //        if (pds.IsFirstPage && pds.IsLastPage)
    //        {
    //            btnBack.Enabled = false;
    //            btnNext.Enabled = false;
    //        }

    //        if (!pds.IsLastPage && !pds.IsFirstPage)
    //        {
    //            btnBack.Enabled = true;
    //            btnNext.Enabled = true;
    //        }

    //        this.GrdWrkGrp.DataSource = pds;
    //        this.GrdWrkGrp.DataBind();
    //        spareRow = GrdWrkGrp.Rows.Count;
    //        for (int i = 0; i < pds.PageSize - spareRow; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;
    //        GrdWrkGrp.DataSource = pds;
    //        GrdWrkGrp.DataBind();
    //    }

    //}
    protected void page()
    {
        int spareRow = 0;
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        //baseBO.WhereClause = "";
        //baseBO.OrderBy = "BrandName";
        DataSet ds = baseBO.QueryDataSet(new BizGrp());
        dt = ds.Tables[0];
        pds.DataSource = dt.DefaultView;
        GrdWrkGrp.DataSource = pds;
        GrdWrkGrp.DataBind();
        spareRow = GrdWrkGrp.Rows.Count;
        
        for (int i = 0; i < this.GrdWrkGrp.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdWrkGrp.DataSource = pds;
        GrdWrkGrp.DataBind();
    }


    private void textClear()
    {
        txtNote.Text = "";
        txtBizGrpName.Text = "";
        txtBizGrpCode.Text = "";
        cmbBizGrpStatus.SelectedValue = BizGrp.BIZ_GRP_STATUS_VALID.ToString();
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showline()", true);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showline()", true);
    }
    protected void GrdWrkGrp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[5].Text = "";
            }
        }
    }
    protected void GrdWrkGrp_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        QueryBizGrp bizGrp = new QueryBizGrp();
        ViewState["BizGrpID"] = GrdWrkGrp.SelectedRow.Cells[0].Text;
        baseBO.WhereClause = "BizGrpID =" + GrdWrkGrp.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(bizGrp);
        if (rs.Count == 1)
        {
            bizGrp = rs.Dequeue() as QueryBizGrp;
            txtBizGrpCode.Text = bizGrp.BizGrpCode;
            txtBizGrpName.Text = bizGrp.BizGrpName;
            txtNote.Text = bizGrp.Note;
            cmbBizGrpStatus.SelectedValue = bizGrp.BizGrpStatus.ToString();
        }
        page();
        btnSave.Enabled = false;
        btnEdit.Enabled = true;
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showlineIns()", true);
    }
    protected void GrdWrkGrp_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        page();
    }

}
