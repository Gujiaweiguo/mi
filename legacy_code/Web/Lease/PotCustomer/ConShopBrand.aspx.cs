using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Base;
using Lease.ConShop;
using Base.Page;
using Lease.Brand;
using System.Drawing;
public partial class Lease_PotCustomer_ConShopBrand : BasePage
{
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
             BaseBO basebo = new BaseBO();
            BrandLevel brandlevel = new BrandLevel();
            DataSet ds = basebo.QueryDataSet(brandlevel);
            BaseInfo.BaseCommon.BindDropDownList(basebo, brandlevel, "BrandLevelCode", "BrandLevelName", DownListBrandLevel);
 
            ViewState["currentCount"] = 1;
            ViewState["WhereSql"] = "";
            page(ViewState["WhereSql"].ToString());
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblConShopBrandDefine");
            this.btnAdd.Attributes.Add("onclick", "return CheckIsNull()");
            this.btnEdit.Attributes.Add("onclick", "return CheckIsNull()");
            btnEdit.Enabled = false;
        }
    }


    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = baseBo.QueryDataSet(new ConShopBrand());
        DataTable dt = ds.Tables[0];
        int count = dt.Rows.Count;
        //获取品牌等级
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["BrandLevelName"] = GetGlobalResourceObject("Parameter", ConShopBrand.GetBrandLevelDesc(Convert.ToInt32(dt.Rows[j]["BrandLevel"])));
        }
        int countNull = 7 - count;
        for (int i = 0; i < countNull; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvShopBrand.DataSource = dt;
        gvShopBrand.DataBind();
        int gvCount = gvShopBrand.Rows.Count;
        for (int j = count; j < gvCount; j++)
            gvShopBrand.Rows[j].Cells[6].Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ConShopBrand shopBrand = new ConShopBrand();
        BaseBO baseBO = new BaseBO();

        baseBO.WhereClause = "BrandName = '" + txtBrandName.Text + "'";
        Resultset rs = baseBO.Query(shopBrand);
        if (rs.Count == 0)
        {

            shopBrand.BrandId = BaseApp.GetBrandID();
            shopBrand.BrandName = txtBrandName.Text;
            shopBrand.BrandLevel = Convert.ToInt32(DownListBrandLevel.SelectedValue);
            shopBrand.BrandRegDesc = txtBrandRegDesc.Text;
            shopBrand.BrandProduce = txtBrandProduce.Text;
            shopBrand.BrandTargetCust = txtBrandTargetCust.Text;
            if (baseBO.Insert(shopBrand) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                return;
            }
            txtBrandName.Text = "";
            txtBrandRegDesc.Text = "";
            txtBrandProduce.Text = "";
            txtBrandTargetCust.Text = "";
            page(ViewState["WhereSql"].ToString());
        }
        else 
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ConShopBrand_BrandNameIsIn") + "'", true);
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["brandID"] != null && ViewState["brandID"].ToString() != "")
        {
            ConShopBrand shopBrand = new ConShopBrand();
            BaseBO baseBO = new BaseBO();
            shopBrand.BrandName = txtBrandName.Text;
            shopBrand.BrandLevel = Convert.ToInt32(DownListBrandLevel.SelectedValue);
            shopBrand.BrandRegDesc = txtBrandRegDesc.Text;
            shopBrand.BrandProduce = txtBrandProduce.Text;
            shopBrand.BrandTargetCust = txtBrandTargetCust.Text;

            baseBO.WhereClause = "";
            baseBO.WhereClause = "BrandId = " + Convert.ToInt32(ViewState["brandID"]);
            if (baseBO.Update(shopBrand) != -1)
            {

            }

            txtBrandName.Text = "";
            txtBrandRegDesc.Text = "";
            txtBrandProduce.Text = "";
            txtBrandTargetCust.Text = "";
        }
        ViewState["brandID"] = "";
        btnAdd.Enabled = true;
        btnEdit.Enabled = false;
        page(ViewState["WhereSql"].ToString());
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtBrandName.Text = "";
        txtBrandRegDesc.Text = "";
        txtBrandProduce.Text = "";
        txtBrandTargetCust.Text = "";
        ViewState["brandID"] = "";
        ViewState["WhereSql"] = "";
        btnAdd.Enabled = true;
        btnEdit.Enabled = false;
        page(ViewState["WhereSql"].ToString());
    }
    protected void gvShopBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        int brandId = 0;
        brandId = Convert.ToInt32(gvShopBrand.SelectedRow.Cells[0].Text);
        ViewState["brandID"] = brandId;
        baseBO.WhereClause = "BrandId=" + brandId;

        Resultset rs = baseBO.Query(new ConShopBrand());
        if (rs.Count == 1)
        {
            ConShopBrand shopBrand = rs.Dequeue() as ConShopBrand;
            txtBrandName.Text = shopBrand.BrandName;
            DownListBrandLevel.SelectedValue = shopBrand.BrandLevel.ToString();
            txtBrandRegDesc.Text = shopBrand.BrandRegDesc;
            txtBrandProduce.Text = shopBrand.BrandProduce;
            txtBrandTargetCust.Text = shopBrand.BrandTargetCust;
        }
        btnEdit.Enabled = true;
        btnAdd.Enabled = false;
        page(ViewState["WhereSql"].ToString());
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
        page(ViewState["WhereSql"].ToString());
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
        page(ViewState["WhereSql"].ToString());
    }

    protected void page(string where)
    {
        int spareRow = 0;
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        baseBO.WhereClause = where;
        baseBO.OrderBy = "BrandName";
        DataSet ds = baseBO.QueryDataSet(new ConShopBrand());
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        int ss = 0;
        //获取状态
        for (int j = 0; j < count; j++)
        {
            string sqlStr="select BrandLevelName from BrandLevel where BrandLevelCode='"+Convert.ToString(dt.Rows[j]["BrandLevel"])+"'";
            DataSet ds1=new DataSet();
            ds1=baseBO.QueryDataSet(sqlStr);
            //dt.Rows[j]["BrandLevelName"] = GetGlobalResourceObject("Parameter", ConShopBrand.GetBrandLevelDesc(Convert.ToInt32(dt.Rows[j]["BrandLevel"])));
            dt.Rows[j]["BrandLevelName"] = ds1.Tables[0].Rows[0][0];
        }

        pds.DataSource = dt.DefaultView;
        #region
        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //}
    //    else
    //    {

    //        gvShopBrand.EmptyDataText = "";
    //        pds.AllowPaging = true;
    //        pds.PageSize = 7;
    //        pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCount"]) - 1;
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
    //        gvShopBrand.DataSource = pds;
    //        gvShopBrand.DataBind();

    //        ss = gvShopBrand.Rows.Count;
    //        for (int i = 0; i < pds.PageSize - ss; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;

    //    }
    //    gvShopBrand.DataSource = pds;
    //    gvShopBrand.DataBind();
    //    for (int j = 0; j < pds.PageSize - ss; j++)
    //        gvShopBrand.Rows[(pds.PageSize - 1) - j].Cells[6].Text = "";

        //}
        #endregion
        gvShopBrand.DataSource = pds;
        gvShopBrand.DataBind();

        spareRow = gvShopBrand.Rows.Count;
        for (int i = 0; i < gvShopBrand.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvShopBrand.DataSource = pds;
        gvShopBrand.DataBind();
    }
    protected void gvShopBrand_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        page(ViewState["WhereSql"].ToString());
        foreach (GridViewRow grv in this.gvShopBrand.Rows)
        {
            grv.BackColor = Color.White;
        }
    }

    protected void gvShopBrand_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text.Trim() == "&nbsp;")//如果当前行没有数据，选择按钮不显示出来
                e.Row.Cells[6].Text = "";
        }
    }
    protected void txtBrandName_TextChanged(object sender, EventArgs e)
    {
        ViewState["WhereSql"]="brandname like '%" + txtBrandName.Text + "%'";
        page(ViewState["WhereSql"].ToString());        
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (txtBrandName.Text != "")
        {
            BaseBO objBaseBo = new BaseBO();
            ConShopBrand brand = new ConShopBrand();
            string UpdateSql = "update conshop set brandid='" + allvalue.Value + "' where brandid='" + gvShopBrand.SelectedRow.Cells[0].Text.Trim() + "'";
            string DeleteSql = "brandid='" + gvShopBrand.SelectedRow.Cells[0].Text.Trim() + "'";
            DataSet ds = new DataSet();
            ds = objBaseBo.QueryDataSet("select * from conshop where brandid='" + gvShopBrand.SelectedRow.Cells[0].Text.Trim() + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (objBaseBo.ExecuteUpdate(UpdateSql) > 0)
                {
                    objBaseBo.WhereClause = DeleteSql;
                    if (objBaseBo.Delete(brand) > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "together", "parent.document.all.txtWroMessage.value =  '合并成功'", true);
                        ViewState["WhereSql"] = "";
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "together", "parent.document.all.txtWroMessage.value =  '删除失败'", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "together", "parent.document.all.txtWroMessage.value =  '更新失败'", true);
                }
            }
            else
            {
                objBaseBo.WhereClause = DeleteSql;
                if (objBaseBo.Delete(brand) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "together", "parent.document.all.txtWroMessage.value =  '合并成功'", true);
                    ViewState["WhereSql"] = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "together", "parent.document.all.txtWroMessage.value =  '删除失败'", true);
                }
            }
            txtBrandName.Text = "";
            txtBrandRegDesc.Text = "";
            txtBrandProduce.Text = "";
            txtBrandTargetCust.Text = "";
            page(ViewState["WhereSql"].ToString());
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "together", "parent.document.all.txtWroMessage.value =  '请先选择品牌'", true);        
        }
    }
}
