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

using Base.Biz;
using Base.DB;
using Base.Page;
using Base.Sys;
using Base.Util;
using Lease.Brand;
using Lease.ConShop;

public partial class Lease_PotCustomer_PotCustBrandSelect : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.txtBrandName.Attributes.Add("onclick", "ShowBrandTree()");
        if (!this.IsPostBack)
        {
            ViewState["strWhere"] = "";
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Customer_labBusinessManQuery");
            this.BindData("0");
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void BindData(string strBrandID)
    {
        BaseBO objBaseBo = new BaseBO();
        string strSql = @"select PotCustomer.CustID,CustCode,CustName,BrandName,TradeName,OperateName,AvgAmt from PotCustBrand 
 inner join PotCustomer on PotCustomer.CustID=PotCustBrand.CustID inner join traderelation on traderelation.tradeid=PotCustBrand.tradeid
inner join BrandOperateType on BrandOperateType.OperateTypeID=PotCustBrand.OperateTypeID
inner join ConShopBrand on  ConShopBrand.brandid=PotCustBrand.brandid";
        if (strBrandID != "")
            strSql += " where ConShopBrand.BrandID='" + strBrandID + "'";

        BaseInfo.BaseCommon.BindGridView(strSql, this.GrdWrk);
    }
    protected void btnDellBrand_Click(object sender, EventArgs e)
    {
        this.SelectBrandID();
        //this.BindData(ViewState["strWhere"].ToString());
        foreach (GridViewRow gr in this.GrdWrk.Rows)
        {
            if (gr.Cells[1].Text == "&nbsp;")
                gr.Cells[7].Text = "";
        }
    }
    /// <summary>
    /// 选择品牌名称
    /// </summary>
    private void SelectBrandID()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "ConShopBrand.BrandId = " + Convert.ToInt32(hidBrandID.Text);
        Resultset rs = objBaseBo.Query(new ConShopBrand());
        if (rs.Count == 1)
        {
            ConShopBrand objConShopBrand = rs.Dequeue() as ConShopBrand;
            this.txtBrandName.Text = objConShopBrand.BrandName;
        }
        objBaseBo.WhereClause = "";
        ViewState["strWhere"] = hidBrandID.Text.Trim();
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.BindData(ViewState["strWhere"].ToString());
    }
    protected void GrdWrk_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //string str = this.GrdWrk.SelectedRow.Cells[0].Text.ToString();
        //string str2 = this.GrdWrk.SelectedRow.Cells[1].Text.ToString();
        //Response.Redirect("../CustomerInfo.aspx?custID=" + this.GrdWrk.SelectedRow.Cells[0].Text);
        //Response.Redirect("PotCustomerBaseInfo.aspx?custID=" + GrdWrk.SelectedRow.Cells[0].Text);
    }
    protected void GrdWrk_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.Cells[1].Text == "&nbsp;")
                {
                    e.Row.Cells[7].Text = "";
                }
                else
                    e.Row.Cells[7].Text = "<a href=../PotCustomer/PotCustomerBaseInfo.aspx?custID=" + e.Row.Cells[0].Text + ">" + (String)GetGlobalResourceObject("BaseInfo", "PotShop_Selected") + "</a>";
            }
        }
    }
    protected void GrdWrk_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindData(ViewState["strWhere"].ToString());
    }
}
