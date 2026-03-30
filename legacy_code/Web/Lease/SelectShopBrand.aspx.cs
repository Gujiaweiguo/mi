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
using Lease.ConShop;
using Base.Page;



public partial class Lease_SelectShopBrand : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ViewState["currentCount"] = 1;
            ViewState["sqlWhere"] = "";
            page();
        }
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        baseBO.OrderBy = "BrandName Asc";
        baseBO.WhereClause = ViewState["sqlWhere"].ToString();
        DataSet ds = baseBO.QueryDataSet(new ConShopBrand());
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        int ss = 0;
        pds.PageSize = 7;
        pds.DataSource = dt.DefaultView;
        if (pds.Count < 1)
        {
            for (int i = 0; i < 7; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
        }
        else
        {

            gvShopBrand.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 7;
            pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCount"]) - 1;
            if (pds.IsFirstPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }
            gvShopBrand.DataSource = pds;
            gvShopBrand.DataBind();

            ss = gvShopBrand.Rows.Count;
            for (int i = 0; i < pds.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;

        }
        gvShopBrand.DataSource = pds;
        gvShopBrand.DataBind();
        for (int j = 0; j < pds.PageSize - ss; j++)
            gvShopBrand.Rows[(pds.PageSize - 1) - j].Cells[2].Text = "";

    }

    protected void gvShopBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        brandID.Value = gvShopBrand.SelectedRow.Cells[0].Text;
        brandName.Value = gvShopBrand.SelectedRow.Cells[1].Text.Replace("&amp;","&");
        Page.RegisterStartupScript("", "<script>clickok();</script>");
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
        page();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
        page();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        ViewState["sqlWhere"] = "BrandName like '%" + txtShopBrand.Text + "%'";
        page();
    }
}
