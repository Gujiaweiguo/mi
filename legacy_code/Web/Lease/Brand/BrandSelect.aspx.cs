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

using RentableArea;
using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using BaseInfo.Store;
using Lease.ConShop;
using Lease.Brand;
using System.Drawing;

public partial class Lease_TradeRelation_TradeRelationSelect : BasePage
{
    public string selectTradeLevel;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowTree();
            selectTradeLevel = (String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblBrandName");
            this.BindBrandData();//绑定品牌列表
        }
    }
    /// <summary>
    /// 绑定查询品牌
    /// </summary>
    private void BindBrandData()
    {
        ConShopBrand objBrand = new ConShopBrand();
        BaseBO objBaseBo = new BaseBO();
        if(this.txtSearch.Text.Trim()!="")
            objBaseBo.WhereClause = "BrandName like '%"+this.txtSearch.Text.Trim()+"%'";
        else
            objBaseBo.WhereClause = "BrandName =''";
        BaseInfo.BaseCommon.BindGridView(objBaseBo, objBrand, this.GridView1);
    }
    /// <summary>
    /// 显示品牌列表
    /// </summary>
    private void ShowTree()
    {
        BaseBO objBaseBo = new BaseBO();
        string jsdept = "";
        jsdept = "100" + "|" + "10" + "|" + "品牌名称" + "^";
        //jsdept += "1" + "|" + "100" + "|" + "国际知名" + "^";
        //jsdept += "2" + "|" + "100" + "|" + "国际二流" + "^";
        //jsdept += "3" + "|" + "100" + "|" + "国内知名" + "^";

        BaseBO objBrandLevel = new BaseBO();
        Resultset rsBrandLevel = objBrandLevel.Query(new BrandLevel());
        if (rsBrandLevel.Count > 0)
        {
            foreach (BrandLevel brandLevel in rsBrandLevel)
            {
                jsdept += "0" + brandLevel.BrandLevelCode + "|" + "100" + "|" + brandLevel.BrandLevelName + "^";
            }
        }
        Resultset rs = objBaseBo.Query(new ConShopBrand());
        if (rs.Count > 0)
        {
            foreach (ConShopBrand objConShop in rs)
            {
                jsdept += objConShop.BrandId + "|" + "0" + objConShop.BrandLevel + "|" + objConShop.BrandName + "^";
            }
        }
        depttxt.Value = jsdept;
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindBrandData();
        foreach (GridViewRow grv in this.GridView1.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        this.BindBrandData();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowIndex>=0)
        {
            if (e.Row.Cells[0].Text.Trim() != "" && e.Row.Cells[0].Text.Trim() != "&nbsp;")
            {
                e.Row.Attributes.Add("onclick", "SelectBrand(" + e.Row.Cells[0].Text.Trim() + ")");
            }
        }
    }
}
