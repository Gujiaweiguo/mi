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

using Lease.Contract;
using Base.Biz;
using Base.DB;
using RentableArea;
using Base;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using BaseInfo.User;
using Base.Page;
using Base.Util;
using Sell;
using Lease.ConShop;
using System.Drawing;

public partial class Sell_Default : BasePage 
{
    #region 定义
    public string baseInfo;
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    BaseTrans baseTrans = new BaseTrans();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ReceiptQuery");
            BindGrdReceiptNull();
            BindGrdDetailNull();
            BindGrdMediaNull();
            //BindDropShopName();

        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "'", true);
        ViewState["currentCount"] = 1;
        page();
    }
    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        string sql = "  SELECT TenantID,ShopName,TransID ,PosID,BatchID,ReceiptID FROM TransHeader inner join ConShop On (TransHeader.TenantId=ConShop.ShopID) " +
                     "  AND Training='N'" +
                     "  AND TenantID='" + allvalue.Value.Trim() + "'";
        if (txtBizdate.Text != "")
        {
            sql = sql + " AND BizDate='" + txtBizdate.Text.Trim() + "'";
        }

        if (txtPosId.Text != "")
        {
            sql = sql + " AND PosId='" + txtPosId.Text.Trim() + "'";
        }

        if (txtBatchId.Text != "")
        {
            sql = sql + " AND BatchId='" + txtBatchId.Text.Trim() + "'";
        }

        if (txtReceiptId.Text != "")
        {
            sql = sql + " AND ReceiptId='" + txtReceiptId.Text.Trim() + "'";
        }

        sql = sql + " order by ReceiptId ";
        DataSet ds = baseBO.QueryDataSet(sql);
        if (ds == null || ds.Tables[0].Rows.Count < 1)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "'", true);
        }
        BaseInfo.BaseCommon.BindGridView(sql, this.GrdReceiptInfo);
        GrdReceiptInfoClear();
        //dt = ds.Tables[0];
        //PagedDataSource pds = new PagedDataSource();
        //int count = dt.Rows.Count;
        //int ss = 0;
        //pds.PageSize = 20;
        //pds.DataSource = dt.DefaultView;
        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < 20; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "'", true);
        //}
        //else
        //{
        //    GrdReceiptInfo.EmptyDataText = "";
        //    pds.AllowPaging = true;
        //    pds.PageSize = 20;
        //    pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCount"]) - 1;
        //    if (pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = true;
        //    }

        //    if (pds.IsLastPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = false;
        //    }

        //    if (pds.IsFirstPage && pds.IsLastPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = false;
        //    }

        //    if (!pds.IsLastPage && !pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = true;
        //    }
        //    ss = dt.Rows.Count - GrdReceiptInfo.Rows.Count * pds.CurrentPageIndex;

        //    for (int i = 0; i < pds.PageSize-ss ; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;

        //}
        //GrdReceiptInfo.DataSource = pds;
        //GrdReceiptInfo.DataBind();
        //GrdReceiptInfoClear();
    }

    //private void BindDropShopName()
    //{
    //    BaseBO baseBO = new BaseBO();
    //    string sql = "  SELECT ShopID,ShopCode,ShopName FROM ConShop Where ShopStatus =  " + ConShop.CONSHOP_TYPE_INGEAR + "  Order By ShopCode";
    //    DataSet myDS = baseBO.QueryDataSet(sql);
    //    int count = myDS.Tables[0].Rows.Count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        //绑定商铺号
    //        ddlShopCode.Items.Add(new ListItem(myDS.Tables[0].Rows[i]["ShopCode"].ToString() + " " + myDS.Tables[0].Rows[i]["ShopName"].ToString(), myDS.Tables[0].Rows[i]["ShopID"].ToString()));
    //    }
    //}
    protected void GVGrdMediaInfo(string sTransId)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = "TransId=" + sTransId;
        DataTable dt = baseBO.QueryDataSet(new Sell.TransMedia()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdMedia.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdMedia.DataSource = pds;
            GrdMedia.DataBind();
        }
        else
        {
            GrdMedia.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 3;

            this.GrdMedia.DataSource = pds;
            this.GrdMedia.DataBind();
            spareRow = GrdMedia.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdMedia.DataSource = pds;
            GrdMedia.DataBind();
        }
    }

    protected void GVGrdDetailInfo(string stransId)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;



        baseBO.WhereClause = "TransId=" + stransId;
        DataTable dt = baseBO.QueryDataSet(new Sell.TransDetail()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdDetail.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdDetail.DataSource = pds;
            GrdDetail.DataBind();
        }
        else
        {
            GrdDetail.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 3;

            this.GrdDetail.DataSource = pds;
            this.GrdDetail.DataBind();
            spareRow = GrdReceiptInfo.Rows.Count;
            for (int i = 0; i < pds.PageSize ; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdDetail.DataSource = pds;
            GrdDetail.DataBind();
        }


    }

    private void BindGrdReceiptNull()
    {
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        dt.Columns.Add("PosId");
        dt.Columns.Add("BatchId");
        dt.Columns.Add("ReceiptId");


        for (int i = 0; i < 20; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdReceiptInfo.DataSource = dt;
        GrdReceiptInfo.DataBind();
        GrdReceiptInfoClear();
    }

    private void BindGrdMediaNull()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("MediaCd");
        dt.Columns.Add("Amountt");
        dt.Columns.Add("Remark1");
        dt.Columns.Add("Remark2");
        dt.Columns.Add("Remark3");

        for (int i = 0; i < 2; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }

        GrdMedia.DataSource = dt;
        GrdMedia.DataBind();

    }

    private void BindGrdDetailNull()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("SkuCd");
        dt.Columns.Add("NewPrice");
        dt.Columns.Add("Qty");
        dt.Columns.Add("ItemDisc");
        dt.Columns.Add("AllocDisc");

        for (int i = 0; i < 2; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }

        GrdDetail.DataSource = dt;
        GrdDetail.DataBind();

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



    protected void GrdReceiptInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindHeaderInfo(GrdReceiptInfo.SelectedRow.Cells[2].Text,GrdReceiptInfo.SelectedRow.Cells[0].Text);
        BindMediaInfo(GrdReceiptInfo.SelectedRow.Cells[2].Text,GrdReceiptInfo.SelectedRow.Cells[0].Text);
        BindDetailInfo(GrdReceiptInfo.SelectedRow.Cells[2].Text,GrdReceiptInfo.SelectedRow.Cells[0].Text);
        BindFooterInfo(GrdReceiptInfo.SelectedRow.Cells[2].Text,GrdReceiptInfo.SelectedRow.Cells[0].Text);
        BindMemberCard(GrdReceiptInfo.SelectedRow.Cells[2].Text,GrdReceiptInfo.SelectedRow.Cells[0].Text);
        GrdReceiptInfoClear();
    }
    private void BindHeaderInfo(string strReceiptId,string posID)
    {
        DataTable temptb = new DataTable();
        baseBo.WhereClause = "";
        baseBo.WhereClause = "ReceiptId = '" + strReceiptId + "' And POSID ='" + posID +"'";
        temptb = baseBo.QueryDataSet(new TransHeader()).Tables[0];
        if (temptb.Rows.Count > 0)
        {
            txtTransTime.Text = temptb.Rows[0]["TransDate"].ToString();
            txtUserId.Text = temptb.Rows[0]["UserId"].ToString();
            txtMReceipt.Text = temptb.Rows[0]["MReceiptId"].ToString();
            txtRReceipt.Text = temptb.Rows[0]["RReceiptId"].ToString();
            txtTransType.Text = temptb.Rows[0]["TranStatus"].ToString();
            txtSalesMan.Text = temptb.Rows[0]["SalesMan"].ToString();
        }
        else
        {
            txtTransTime.Text = "";
            txtUserId.Text = "";
            txtMReceipt.Text = "";
            txtRReceipt.Text = "";
            txtTransType.Text = "";
            txtSalesMan.Text = "";
        }
    }

    private void BindMediaInfo(string strReceiptId,string posID)
    {
        DataTable  tempDs = new DataTable();
        string sql = "SELECT MediaCd,Amountt,Remark1,Remark2,Remark3 FROM TransMedia INNER JOIN TransHeader On (TransMedia.TransId=TransHeader.TransId) " +
                        " WHERE TransHeader.ReceiptId='" + strReceiptId + "' And POSID ='" + posID + "'";
        tempDs = baseBo.QueryDataSet(sql).Tables[0];
        if (tempDs.Rows.Count > 0)
        {
            GrdMedia.DataSource = tempDs;
            GrdMedia.DataBind();
        }
        else
        {
            BindGrdMediaNull();
        }
    }

    private void BindDetailInfo(string strReceiptId,string posID)
    {
        DataTable tempDs = new DataTable();
        string sql = "SELECT SkuCd,NewPrice,Qty,ItemDisc,AllocDisc FROM TransDetail INNER JOIN TransHeader On (TransDetail.TransId=TransHeader.TransId) " +
                        " WHERE TransHeader.ReceiptId='" + strReceiptId + "' And POSID ='" + posID +"'";
        tempDs = baseBo.QueryDataSet(sql).Tables[0];
        if (tempDs.Rows.Count > 0)
        {
            GrdDetail.DataSource = tempDs;
            GrdDetail.DataBind();
        }
        else
        {
            BindGrdDetailNull();
        }
    }
    private void BindFooterInfo(string strReceiptId,string posID)
    {
        DataTable temptb = new DataTable();
        string sql = "SELECT ReceiptDisc,Tax,TaxType,ExemptTax,MiscTax,MiscCharge,SerCharge,TotalAmt,(TotalAmt-ReceiptDisc) as DueAmt FROM Transfooter INNER JOIN TransHeader On (Transfooter.TransId=TransHeader.TransId)" +
                        " AND TransHeader.ReceiptId='" + strReceiptId + "' And POSID ='" + posID +"'";
        temptb = baseBo.QueryDataSet(sql).Tables[0];
        if (temptb.Rows.Count > 0)
        {
            txtReceiptDisc.Text = temptb.Rows[0]["ReceiptDisc"].ToString();
            txtTax.Text = temptb.Rows[0]["Tax"].ToString();
            txtTaxType.Text = temptb.Rows[0]["TaxType"].ToString();
            txtExemptTax.Text = temptb.Rows[0]["ExemptTax"].ToString();
            txtMiscTax.Text = temptb.Rows[0]["MiscTax"].ToString();
            txtMIscCharge.Text = temptb.Rows[0]["MiscCharge"].ToString();
            txtSurCharge.Text = temptb.Rows[0]["SerCharge"].ToString();
            txtTotalAmt.Text = temptb.Rows[0]["TotalAmt"].ToString();
            txtDueAmt.Text = temptb.Rows[0]["DueAmt"].ToString();
        }
        else
        {
            txtReceiptDisc.Text = "";
            txtTax.Text = "";
            txtTaxType.Text = "";
            txtExemptTax.Text = "";
            txtMiscTax.Text = "";
            txtMIscCharge.Text = "";
            txtSurCharge.Text = "";
            txtTotalAmt.Text = "";
            txtDueAmt.Text = "";
        }
    }
    private void BindMemberCard(string strReceiptId, string posID)
    {
         DataTable tdMemberCard = new DataTable();
        string sql = "SELECT membcardid,netamt " +
                     " FROM TransMedia Right JOIN TransHeader On (TransMedia.TransId=TransHeader.TransId) left join purhist on TransHeader.ReceiptId= purhist.ReceiptId " +
                     " WHERE TransHeader.ReceiptId='" + strReceiptId + "' And POSID ='" + posID +"'";
        tdMemberCard = baseBo.QueryDataSet(sql).Tables[0];
        if (tdMemberCard.Rows.Count > 0)
        {
            TextBox9.Text = tdMemberCard.Rows[0]["membcardid"].ToString();
            TextBox10.Text = tdMemberCard.Rows[0]["netamt"].ToString();
        }
        else
        {
            TextBox9.Text = "";
            TextBox10.Text = "";
        }
    }


    private void GrdReceiptInfoClear()
    {
        foreach (GridViewRow gvr in GrdReceiptInfo.Rows)
        {
            if (gvr.Cells[0].Text == "&nbsp;")
            {
                gvr.Cells[3].Text = "";
            }
        }
    }
    protected void ddlShopCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GrdReceiptInfo.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[3].Text = "";
            }
        }
    }
    protected void GrdReceiptInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        foreach (GridViewRow grv in this.GrdDetail.Rows)
        {
            grv.BackColor = Color.White;
        }
        this.page();
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        txtTransTime.Text = "";
        txtUserId.Text = "";
        txtMReceipt.Text = "";
        txtRReceipt.Text = "";
        txtTransType.Text = "";
        txtSalesMan.Text = "";
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
            ddlShopCode.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString() + ds.Tables[0].Rows[0]["ShopName"].ToString();

            //GetShopBrand(ds.Tables[0].Rows[0]["BrandID"].ToString());

            //BaseBO baseBO = new BaseBO();
            //int a = Convert.ToInt32(ViewState["shopID"]);
            //baseBO.WhereClause = "TenantId = " + Convert.ToInt32(ViewState["shopID"]) + " and status='V'";
            //Resultset rs = baseBO.Query(new SkuMaster());
            //if (rs.Count > 0)
            //{
            //    BindDropSkuID(rs);
            //    SkuMaster skuMaster = rs.Dequeue() as SkuMaster;
            //    Query(skuMaster.SkuId);
            //}
            GrdReceiptInfoClear();
        }
    }
}
