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
using Base.Page;
using Base;
using Generalize.Medium;

public partial class Generalize_Medium_SelectAnPMaster : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Master_lblAnPNm"), "0"));
            cmbConSelect.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Master_lblAnPTheme"), "1"));
            this.Form.DefaultButton = "btnQuery";
            BindGV(" And AnpID =-1");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "Load();", true);
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string queryValue = cmbConSelect.SelectedValue;
        string whereStr = "";

        switch (queryValue)
        {
            /*活动名称*/
            case "0":
                whereStr = " And AnPNm like '%" + txtQueryMes.Text + "%'";
                break;
            /*促销主题*/
            case "1":
                whereStr = " And ThemeNm like '%" + txtQueryMes.Text + "%'";
                break;
            default:
                break;
        }
        lblTotalNum.Text = "1";
        lblCurrent.Text = "1";
        BindGV(whereStr);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {

    }
    protected void btnNext_Click(object sender, EventArgs e)
    {

    }
    private void BindGV(string strSql)
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBo.WhereClause = "a.ThemeID= b.ThemeID " + strSql;
        DataTable dt = baseBo.QueryDataSet(new SelectAnPMaster()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvAnPMaster.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvAnPMaster.DataSource = pds;
            gvAnPMaster.DataBind();
        }
        else
        {
            gvAnPMaster.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 11;
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

            this.gvAnPMaster.DataSource = pds;
            this.gvAnPMaster.DataBind();
            spareRow = gvAnPMaster.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvAnPMaster.DataSource = pds;
            gvAnPMaster.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvAnPMaster.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[7].Text = "";
            }
        }
    }
    protected void gvAnPMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("AddAnPMaster.aspx?AnPID=" + gvAnPMaster.SelectedRow.Cells[0].Text);
        //?VoucherID=" + gvAnPMaster.SelectedRow.Cells[0].Text + "&modify=" + 1
    }
}
