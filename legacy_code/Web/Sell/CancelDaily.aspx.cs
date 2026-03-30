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

using Sell;
using Base.Biz;
using Base.Page;

public partial class Sell_CancelDaily : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Page();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Sale_lblCancelDaily");
        }
    }
    protected void BtnQry_Click(object sender, EventArgs e)
    {
        Page();
    }

    private void Page()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        DateTime date;
        if (txtDate.Text == "")
        {
            date = Convert.ToDateTime("9999-12-31");
        }
        else
        {
            date = Convert.ToDateTime(txtDate.Text);
        }

        DataSet ds = CancelTransPO.GetTransDailyByDate(date);
        DataTable dt = ds.Tables[0];

        int count = dt.Rows.Count;
        dt.Columns.Add("ShopCode");
        //获取商铺号
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["ShopCode"] = CancelTransPO.GetShopCodeByShopID(Convert.ToInt32(dt.Rows[j]["ShopID"]));
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GVDaily.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVDaily.DataSource = pds;
            GVDaily.DataBind();
        }
        else
        {
            GVDaily.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 10;
            lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
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

            this.GVDaily.DataSource = pds;
            this.GVDaily.DataBind();
            spareRow = GVDaily.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GVDaily.DataSource = pds;
            GVDaily.DataBind();
        }

        //int countNull = 10 - count;
        //for (int i = 0; i < countNull; i++)
        //{
        //    dt.Rows.Add(dt.NewRow());
        //}
        //GVDaily.DataSource = dt;
        //GVDaily.DataBind();
        int gvCount = GVDaily.Rows.Count;
        for (int j = count; j < gvCount; j++)
            GVDaily.Rows[j].Cells[7].Text = "";
    }
    protected void GVDaily_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CancelDaily")
        {
            string bacthID = GVDaily.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].Text.ToString();
            //string bacthID = GVDaily.SelectedRow.Cells[2].Text.ToString();
            BaseTrans baseTrans = new BaseTrans();
            baseTrans.BeginTrans();
            try
            {
                baseTrans.WhereClause = "BatchID = '" + bacthID + "'";
                baseTrans.Delete(new TransSku());
                baseTrans.Delete(new TransSkuMedia());
            }
            catch (Exception ex)
            {
                baseTrans.Rollback();
                Response.Write(ex.ToString());
            }
            baseTrans.Commit();
        }
        Page();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        Page();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        Page();
    }
}
