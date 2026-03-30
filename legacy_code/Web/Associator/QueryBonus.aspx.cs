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

using Base.Page;
using Base;
using Base.Biz;
using Base.DB;
using Associator.Perform;
using Associator.Associator;

public partial class Associator_QueryBonus : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindQueryCondition();
            BindDate("1=0");
            this.Form.DefaultButton = "btnQuery";
            baseInfo = "会员积分查询";
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        strWhere();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        strWhere();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (txtQuery.Text.Trim() != "")
        {
            strWhere();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '请输入查询信息!'", true);
        }
    }
    protected void dropQuery_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }

    private void BindQueryCondition()
    {
        dropQuery.Items.Add(new ListItem("会员卡号", "1"));
        dropQuery.Items.Add(new ListItem("会员名", "3"));
        dropQuery.Items.Add(new ListItem("家庭电话", "4"));
        dropQuery.Items.Add(new ListItem("移动电话", "5"));
    }

    private void BindDate(string strWhere)
    {
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        DataSet ds;

        BaseBO baseBO = new BaseBO();
        ds = BonusPO.GetBonusByWhereStr(strWhere);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count <= 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '无此会员积分!'", true);
        }

        pds.DataSource = dt.DefaultView;

        GrdCust.EmptyDataText = "";
        //pds.AllowPaging = true;
        //pds.PageSize = 10;
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

        this.GrdCust.DataSource = pds;
        this.GrdCust.DataBind();
        spareRow = GrdCust.Rows.Count;
        for (int i = 0; i < GrdCust.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        pds.DataSource = dt.DefaultView;
        GrdCust.DataSource = pds;
        GrdCust.DataBind();
    }

    private void strWhere()
    {
        string WhereClause = "";
        if (dropQuery.SelectedValue == "1")
        {
            WhereClause = "MembCard.MembCardId like '%" + txtQuery.Text.Trim() + "%'";
        }
        else if (dropQuery.SelectedValue == "3")
        {
            WhereClause = "MemberName like '%" + txtQuery.Text.Trim() + "%'";
        }
        else if (dropQuery.SelectedValue == "4")
        {
            WhereClause = "HomePhone like '%" + txtQuery.Text.Trim() + "%'";
        }
        else if (dropQuery.SelectedValue == "5")
        {
            WhereClause = "MobilPhone like '%" + txtQuery.Text.Trim() + "%'";
        }
        BindDate(WhereClause);
    }
    protected void GrdCust_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        strWhere();
    }
}
