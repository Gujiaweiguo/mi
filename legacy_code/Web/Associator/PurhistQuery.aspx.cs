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
using Lease.ConShop;

public partial class Associator_PurhistQuery : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = "会员消费记录查询";
            txtShopCode.Attributes.Add("onclick", "ShowTree()");
            BindDate("AND 1=0");
        }

    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        string strWhere = "";
        if (txtCard.Text.Trim() != "")
        {
            strWhere += " AND Purhist.MembCardID = '" + txtCard.Text + "'";
        }
        if (txtShopCode.Text.Trim() != "")
        {
            strWhere += " AND ConShop.ShopID = " + Convert.ToInt32(ViewState["shopID"]);
        }
        if (txtStartDate.Text.Trim() != "")
        {
            strWhere += " AND Convert(varchar(10),Purhist.TransDT,120) >= '" + txtStartDate.Text + "'";
        }
        if (txtEndDate.Text.Trim() != "")
        {
            strWhere += " AND Convert(varchar(10),Purhist.TransDT,120) <= '" + txtEndDate.Text + "'";
        }
        if (strWhere == "")
        {
            strWhere = " AND 1=0";
        }
        ViewState["where"] = strWhere;
        BindDate(strWhere);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        BindDate(ViewState["where"].ToString());
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        BindDate(ViewState["where"].ToString());
    }

    private void BindDate(string strWhere)
    {
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        BaseBO baseBO = new BaseBO();
        DataSet ds = PurhistPO.GetPurhistByWhere(strWhere);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count <= 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '无此会员消费记录!'", true);
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
        for (int i = 0; i < pds.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        pds.DataSource = dt.DefaultView;
        GrdCust.DataSource = pds;
        GrdCust.DataBind();
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            txtShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + "(" + ds.Tables[0].Rows[0]["ShopName"].ToString() + ")";
        }
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
        BindDate(ViewState["where"].ToString());

    }
}
