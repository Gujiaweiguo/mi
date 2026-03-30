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

public partial class Associator_QueryAssociator : BasePage
{
    public string baseInfo;
    public string url;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindQueryCondition();
            BindDate("1=0");
            this.Form.DefaultButton = "btnQuery";
            if (Request.QueryString["flag"] == "0") //修改会员信息
            {
                this.Label2.Text = "会员信息修改";
                baseInfo = "会员信息修改";
                url = "Associator/QueryAssociator.aspx?flag=0";
            }
            else if (Request.QueryString["flag"] == "1")  //修改卡信息
            {
                this.Label2.Text = "会员卡修改";
                baseInfo = "会员卡修改";
                url = "Associator/QueryAssociator.aspx?flag=1";
            }
            
        }
        else
        {
             ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hidden", "document.getElementById('lblTotalNum').style.display='none'", true);
             ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "hiddennum", "document.getElementById('lblCurrent').style.display='none'", true);
        }
    }

    private void BindQueryCondition()
    {
        dropQuery.Items.Add(new ListItem("会员卡号", "1"));
        dropQuery.Items.Add(new ListItem("会员号", "2"));
        dropQuery.Items.Add(new ListItem("会员名", "3"));
        dropQuery.Items.Add(new ListItem("家庭电话", "4"));
        dropQuery.Items.Add(new ListItem("移动电话", "5"));
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        strWhere();

    }

    private void BindDate(string strWhere)
    {
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        DataSet ds;

        BaseBO baseBO = new BaseBO();
        //if (dropQuery.SelectedValue == "1")
        //{
            ds = QueryAssociatorPO.GetLCustByCondition(strWhere);
            GrdCust.Columns[3].Visible = true;
        //}
        //else
        //{
        //    baseBO.WhereClause = strWhere;
        //    ds = baseBO.QueryDataSet(new LCust());
        //    //ds.Tables[1].Columns.Add("MemberCode");
        //    GrdCust.Columns[3].Visible = false;

        //}
        DataTable dt = ds.Tables[0];

        if (strWhere != "1=0")
        {
            if (dt.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "msg_NotFindData") + "'", true);
            }
            else
            {
                string str = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + str + "'", true);
            }
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

        ClearGridViewSelect();
    }

    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["flag"] == "0") //修改会员信息
        {
            Response.Redirect("../Associator/AddAssociator.aspx?MemberId=" + GrdCust.SelectedRow.Cells[0].Text + "&modify=" + 1);
        }
        else if (Request.QueryString["flag"] == "1")  //修改卡信息
        {
            Response.Redirect("../Associator/Perform/ModifyCard.aspx?MembCardID=" + GrdCust.SelectedRow.Cells[3].Text);
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        strWhere();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        strWhere();
    }

    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in GrdCust.Rows)
        {
            if (gvr.Cells[0].Text == "&nbsp;")
            {
                gvr.Cells[7].Text = "";
            }
        }
    }

    private void ClearControls()
    {
        txtQuery.Text = "";
    }

    private void strWhere()
    {
        string WhereClause = "";
        if (dropQuery.SelectedValue == "1")
        {
            WhereClause = "MembCard.MembCardId = '" + txtQuery.Text.Trim() + "'";
        }
        else if (dropQuery.SelectedValue == "2")
        {
            WhereClause = "MembCode = '" + txtQuery.Text.Trim() + "'";
        }
        else if (dropQuery.SelectedValue == "3")
        {
            WhereClause = "MemberName like '%" + txtQuery.Text.Trim() + "%'";
        }
        else if (dropQuery.SelectedValue == "4")
        {
            WhereClause = "HomePhone = '" + txtQuery.Text.Trim() + "'";
        }
        else if (dropQuery.SelectedValue == "5")
        {
            WhereClause = "MobilPhone = '" + txtQuery.Text.Trim() + "'";
        }
        if (txtQuery.Text == "")
        {
            WhereClause = "1=1";
        }
        BindDate(WhereClause);
        ClearGridViewSelect();
    }
    protected void dropQuery_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearControls();
        ClearGridViewSelect();
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
