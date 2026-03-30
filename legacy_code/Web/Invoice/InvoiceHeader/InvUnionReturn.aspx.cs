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

using Base;
using Base.DB;
using Base.Biz;
using Invoice;
using Lease.Customer;
using BaseInfo.User;
using Base.Page;
using Lease.Contract;
using BaseInfo.authUser;

public partial class Invoice_InvUnionReturn : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
            dropInvCode.Items.Add(selected);
            dropInvCode.Enabled = false;
            BindInvJVDetail(0);
            BindInvDetail(0);
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_InvUnionReturn");
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        string str_sql = "select A.InvCode from InvoiceHeader A ,Contract B ,ConShop " +
                         "where A.ContractID = B.ContractID AND B.ContractID=ConShop.ContractID" +
                         " and B.ContractCode = '" + txtContractID.Text +
                         "' and A.InvStatus = " + InvoiceHeader.INVSTATUS_VALID +
                         " and B.BizMode = " + Contract.BIZ_MODE_UNIT +
                         " and A.InvType = " + InvoiceHeader.INVTYPE_UNION +
                         " and ( B.ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_INGEAR +
                         " or B.ContractStatus = " + Contract.CONTRACTSTATUS_TYPE_END + ")";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        ds = baseBO.QueryDataSet(str_sql);
        int count = ds.Tables[0].Rows.Count;
        for (int i = 0; i < count; i++)
        {
            dropInvCode.Enabled = true;
            dropInvCode.Items.Add(ds.Tables[0].Rows[i]["InvCode"].ToString());
        }
    }


    protected void dropInvCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        int invCode = Convert.ToInt32(dropInvCode.SelectedItem.Text);
        ViewState["invcode"] = invCode;
        BindInvJVDetail(invCode);
        BindInvDetail(invCode);
        BindTextBox();
    }

    private void BindTextBox()
    {
        DataSet ds = InvUnionReturnPO.GetCustomer(txtContractID.Text);
        if (ds.Tables[0].Rows.Count == 1)
        {
            txtBank.Text = ds.Tables[0].Rows[0]["BankName"].ToString();
            txtBankAcct.Text = ds.Tables[0].Rows[0]["BankAcct"].ToString();
            txtCustCode.Text = ds.Tables[0].Rows[0]["CustCode"].ToString();
            txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
            txtTaxCode.Text = ds.Tables[0].Rows[0]["TaxCode"].ToString();
            txtTotalMoney.Text = Convert.ToString(InvUnionReturnPO.SumInvJVCostAmtByInvID(Convert.ToInt32(ViewState["invcode"])) - InvUnionReturnPO.SumInvActPayAmt(Convert.ToInt32(ViewState["invcode"])));
        }
    }

    private void BindInvJVDetail(int invCode)
    {
        DataSet ds = InvUnionReturnPO.GetInvJVDetail(invCode);
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            ViewState["taxRate"] = dt.Rows[0]["JVTaxRate"];
        }
        else
        {
            ViewState["taxRate"] = 0;
        }
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        pds.DataSource = dt.DefaultView;
        pds.AllowPaging = true;
        //gridInvJVDetail.PageSize = 5;
        pds.PageSize = 5;
        if (pds.Count < 1)
        {
            for (int i = 0; i < gridInvJVDetail.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gridInvJVDetail.DataSource = pds;
            gridInvJVDetail.DataBind();
        }
        else
        {
            //pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    bt1Back.Enabled = false;
            //    bt1Next.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    bt1Back.Enabled = true;
            //    bt1Next.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    bt1Back.Enabled = false;
            //    bt1Next.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    bt1Back.Enabled = true;
            //    bt1Next.Enabled = true;
            //}
            //this.gridInvJVDetail.DataSource = pds;
            //this.gridInvJVDetail.DataBind();
            //spareRow = gridInvJVDetail.Rows.Count;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //gridInvJVDetail.DataSource = pds;
            //gridInvJVDetail.DataBind();
            gridInvJVDetail.DataBind();
            spareRow = gridInvJVDetail.Rows.Count;
            for (int i = 0; i < gridInvJVDetail.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            gridInvJVDetail.DataSource = pds;
            gridInvJVDetail.DataBind();

        }
    }

    private void BindInvDetail(int invCode)
    {
        DataSet ds = InvUnionReturnPO.GetInvDetailByInvID(invCode);
        DataTable dt = ds.Tables[0];
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        pds.DataSource = dt.DefaultView;
        //gridInvDetail.PageSize = 5;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gridInvDetail.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gridInvDetail.DataSource = pds;
            gridInvDetail.DataBind();
        }
        else
        {
            //pds.AllowPaging = true;
            //pds.PageSize = 5;
            //pds.CurrentPageIndex = int.Parse(lblDetailCurrent.Text) - 1;
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
            //this.gridInvDetail.DataSource = pds;
            //this.gridInvDetail.DataBind();
            //spareRow = gridInvDetail.Rows.Count;
            //ViewState["spareRow"] = spareRow;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //gridInvDetail.DataSource = pds;
            //gridInvDetail.DataBind();
            gridInvDetail.DataBind();
            spareRow = gridInvDetail.Rows.Count;
            for (int i = 0; i < gridInvDetail.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            gridInvDetail.DataSource = pds;
            gridInvDetail.DataBind();

        }
    }
    protected void GrdVewInvoiceHeader_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = InvUnionReturnPO.SumInvJVCostAmtByInvID(Convert.ToInt32(ViewState["invcode"])).ToString();
            e.Row.Cells[4].Text = Convert.ToString(Convert.ToDecimal(e.Row.Cells[2].Text) - (Convert.ToDecimal(e.Row.Cells[2].Text) * Convert.ToDecimal(ViewState["taxRate"])));
            e.Row.Cells[5].Text = (String)GetGlobalResourceObject("BaseInfo", "ConLease_lblTaxFrank") + (Convert.ToDecimal(ViewState["taxRate"]) * 100).ToString();
            e.Row.Cells[7].Text = Convert.ToString(Convert.ToDecimal(e.Row.Cells[2].Text) * Convert.ToDecimal(ViewState["taxRate"]));
        } 
    }
    protected void GrdVewInvoiceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[3].Text = InvUnionReturnPO.SumInvActPayAmt(Convert.ToInt32(ViewState["invcode"])).ToString();
        } 
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {

    }
    //protected void btnBackType_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    BindInvJVDetail(Convert.ToInt32(ViewState["invcode"]));
    //}
    //protected void btnNextType_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    BindInvJVDetail(Convert.ToInt32(ViewState["invcode"]));
    //}
    //protected void btnLast_Click(object sender, EventArgs e)
    //{
    //    lblDetailCurrent.Text = Convert.ToString(int.Parse(lblDetailCurrent.Text) - 1);
    //    BindInvDetail(Convert.ToInt32(ViewState["invcode"]));
    //}
    //protected void BtnNext_Click(object sender, EventArgs e)
    //{
    //    lblDetailCurrent.Text = Convert.ToString(int.Parse(lblDetailCurrent.Text) + 1);
    //    BindInvDetail(Convert.ToInt32(ViewState["invcode"]));
    //}
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        InvUnionReturnPO.ReturnMoney(Convert.ToInt32(ViewState["invcode"]));
        BindInvJVDetail(0);
        BindInvDetail(0);
        txtBank.Text = "";
        txtBankAcct.Text = "";
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtTaxCode.Text = "";
        txtTotalMoney.Text = "";
        txtContractID.Text = "";
        dropInvCode.Items.Clear();
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        dropInvCode.Items.Add(selected);
        dropInvCode.Enabled = false;

    }
    protected void gridInvJVDetail_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindInvJVDetail( Convert.ToInt32(ViewState["invcode"]));
    }
    protected void gridInvDetail_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindInvDetail(Convert.ToInt32(ViewState["invcode"]));
    }
}
