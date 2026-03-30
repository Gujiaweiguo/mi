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

public partial class Associator_LargessQuery : System.Web.UI.Page
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = "会员赠品兑换查询";
            string strTemp = " AND 1 = 0";
            BindDate(strTemp, strTemp, strTemp);
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        string strWhere1 = "";
        string strWhere2 = "";
        string strWhere3 = "";
        string strTmep = " AND 1 = 0";
        
        if (txtCard.Text.Trim() != "")
        {
            strWhere1 += " AND MembCard.MembCardID = '" + txtCard.Text + "'";
            strWhere2 += " AND MembCard.MembCardID = '" + txtCard.Text + "'";
            strWhere3 += " AND MembCard.MembCardID = '" + txtCard.Text + "'";
        }
        if (txtModil.Text.Trim() != "")
        {
            strWhere1 += " AND Member.MobilPhone = '" + txtModil.Text + "'";
            strWhere2 += " AND Member.MobilPhone = '" + txtModil.Text + "'";
            strWhere3 += " AND Member.MobilPhone = '" + txtModil.Text + "'";
        }
        if (txtStartDate.Text.Trim() != "")
        {
            strWhere1 += " AND Convert(varchar(10),FreeGiftTrans.ActDate,120) >= '" + txtStartDate.Text + "'";
            strWhere2 += " AND Convert(varchar(10),RedeemH.RedeemDate,120) >= '" + txtStartDate.Text + "'";
            strWhere3 += " AND Convert(varchar(10),ExTrans.ExDate,120) >= '" + txtStartDate.Text + "'";
        }
        if (txtEndDate.Text.Trim() != "")
        {
            strWhere1 += " AND Convert(varchar(10),FreeGiftTrans.ActDate,120) <= '" + txtEndDate.Text + "'";
            strWhere2 += " AND Convert(varchar(10),RedeemH.RedeemDate,120) <= '" + txtEndDate.Text + "'";
            strWhere3 += " AND Convert(varchar(10),ExTrans.ExDate,120) <= '" + txtEndDate.Text + "'";
        }
        if (strWhere1 == "")
        {
            strWhere1 = strTmep;
        }
        if (strWhere2 == "")
        {
            strWhere2 = strTmep;
        }
        if (strWhere3 == "")
        {
            strWhere3 = strTmep;
        }
        ViewState["where1"] = strWhere1;
        ViewState["where2"] = strWhere2;
        ViewState["where3"] = strWhere3;
        BindDate(strWhere1, strWhere2, strWhere3);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        BindDate(ViewState["where1"].ToString(), ViewState["where2"].ToString(), ViewState["where3"].ToString());
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        BindDate(ViewState["where1"].ToString(), ViewState["where2"].ToString(), ViewState["where3"].ToString());
    }

    private void BindDate(string strWhere1,string strWhere2,string strWhere3)
    {
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        BaseBO baseBO = new BaseBO();
        DataSet ds = GiftPO.GetAllGift(strWhere1, strWhere2, strWhere3);
        DataTable dt = ds.Tables[0];

        int count = dt.Rows.Count;
        for (int i = 0; i < count; i++)
        {
            if(dt.Rows[i]["giftType"].ToString() == "0")
            {
                dt.Rows[i]["giftTypeName"] = "免费兑换";
            }
            else if(dt.Rows[i]["giftType"].ToString() == "1")
            {
                dt.Rows[i]["giftTypeName"] = "积分兑换";
            }
            else if(dt.Rows[i]["giftType"].ToString() == "2")
            {
                dt.Rows[i]["giftTypeName"] = "小票兑换";
            }
        }

        if (dt.Rows.Count <= 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '无兑换记录!'", true);
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
        BindDate(ViewState["where1"].ToString(), ViewState["where2"].ToString(), ViewState["where3"].ToString());

    }
}
