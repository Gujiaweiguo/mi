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

using Base.DB;
using Base.Biz;
using Shop.ShopType;
using Base;
using Base.Page;
using Lease.PotCust;
using System.Drawing;

public partial class Lease_Shop_ShopType : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int[] status = ShopType.GetShopTypeStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbShopTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter", ShopType.GetShopTypeStatusDesc(status[i])), status[i].ToString()));
            }
            page();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopTypeManagement");
            btnAdd.Attributes.Add("onclick", "return ShopTypeCode(form1)");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "showline();", true);
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ShopType shopType = new ShopType();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ShopTypeID =" + ViewState["ShopTypeID"];
        shopType.ShopTypeCode = txtShopTypeCode.Text.Trim();
        shopType.ShopTypeName = txtShopTypeName.Text.Trim();
        shopType.ShopTypeStatus = Convert.ToInt32(cmbShopTypeStatus.SelectedValue);
        shopType.Note = txtNote.Text.Trim();
        if (baseBO.Update(shopType) < 1)
        {
            page();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mes", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
            return;
        }
        else
        {
            page();
            textClear();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mes", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ShopType shopType = new ShopType();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ShopTypeCode = '" + txtShopTypeCode.Text.ToString() + "'";
        Resultset rs = baseBO.Query(shopType);
        if (rs.Count == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showlineError()", true);
            page();
            return;
        }
        else
        {
            shopType.ShopTypeID = BaseApp.GetShopID();
            shopType.ShopTypeCode = txtShopTypeCode.Text.Trim();
            shopType.ShopTypeName = txtShopTypeName.Text.Trim();
            shopType.ShopTypeStatus = Convert.ToInt32(cmbShopTypeStatus.SelectedValue);
            shopType.Note = txtNote.Text.Trim();
            if (baseBO.Insert(shopType) < 1)
            {
                page();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mes", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }
            else
            {
                page();
                textClear();
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mes", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        }
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
        ShopType shopType = new ShopType();
        ViewState["ShopTypeID"] = GrdShopType.SelectedRow.Cells[0].Text;
        baseBO.WhereClause = "ShopTypeID =" + GrdShopType.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(shopType);
        if (rs.Count == 1)
        {
            shopType = rs.Dequeue() as ShopType;
            txtShopTypeCode.Text = shopType.ShopTypeCode;
            txtShopTypeName.Text = shopType.ShopTypeName;
            txtNote.Text = shopType.Note;
            cmbShopTypeStatus.SelectedValue = shopType.ShopTypeStatus.ToString();
        }
        page();
        btnAdd.Enabled = false;
        btnEdit.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "showlineIns()", true);
    }
    #region
    //protected void page()
    //{
    //    BaseBO baseBO = new BaseBO();
    //    Resultset rs = new Resultset();
    //    PagedDataSource pds = new PagedDataSource();
    //    int spareRow = 0;

    //    baseBO.OrderBy = "ShopTypeCode";
    //    DataTable dt = baseBO.QueryDataSet(new ShopType()).Tables[0];

    //    int count = dt.Rows.Count;
    //    for (int j = 0; j < count; j++)
    //    {
    //        string typeName = (String)GetGlobalResourceObject("Parameter", ShopType.GetShopTypeStatusDesc(Convert.ToInt32(dt.Rows[j]["ShopTypeStatus"].ToString())));
    //        dt.Rows[j]["ShopTypeStatusName"] = typeName;
    //    }

    //    pds.DataSource = dt.DefaultView;

    //    if (pds.Count < 1)
    //    {
    //        for (int i = 0; i < GrdShopType.PageSize; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;
    //        GrdShopType.DataSource = pds;
    //        GrdShopType.DataBind();
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

    //        this.GrdShopType.DataSource = pds;
    //        this.GrdShopType.DataBind();
    //        spareRow = GrdShopType.Rows.Count;
    //        for (int i = 0; i < pds.PageSize - spareRow; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;
    //        GrdShopType.DataSource = pds;
    //        GrdShopType.DataBind();
    //    }
    //    ClearGridSelected();
    //}
    #endregion
    protected void page()
    {
        int spareRow = 0;
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        //baseBO.WhereClause = "";
        //baseBO.OrderBy = "BrandName";
        DataSet ds = baseBO.QueryDataSet(new ShopType());
        dt = ds.Tables[0];
        pds.DataSource = dt.DefaultView;
        GrdShopType.DataSource = pds;
        GrdShopType.DataBind();
        spareRow = GrdShopType.Rows.Count;
        for (int i = 0; i < GrdShopType.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdShopType.DataSource = pds;
        GrdShopType.DataBind();
    }
    private void textClear()
    {
        txtNote.Text = "";
        txtShopTypeName.Text = "";
        txtShopTypeCode.Text = "";
        cmbShopTypeStatus.SelectedValue = ShopType.SHOP_TYPE_STATUS_VALID.ToString();
        btnEdit.Enabled = false;
        btnAdd.Enabled = true;
    }
    private void ClearGridSelected()
    {
        foreach (GridViewRow gvr in GrdShopType.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
    }
    protected void GrdShopType_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        foreach (GridViewRow grv in this.GrdShopType.Rows)
        {
            grv.BackColor = Color.White;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Lease/Shop/ShopType.aspx");
    }
}
