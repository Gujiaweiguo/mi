using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using Lease.InvoicePara;
using Lease.Contract;
using Invoice;
using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class Lease_ChargeAccount_InvoiceAgainPrint : BasePage
{
    public string baseInfo;  //基本信息
    public string UserName;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            page("-1");
            BindData();
            ViewState["currentCount"] = 1;
        }
        txtLocked();
        controlStatus(true);
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_Billreprint");
    }
    private void txtLocked()
    {
        txtBuildDate.Enabled = false;
        txtCustName.Enabled = false;
        txtInvoiceID.Enabled = false;
        txtPrintStatus.Enabled = false;
        //txtTotalMoney.Enabled = false;
        txtContractCode.Enabled = false;
    }
    private void controlStatus(bool blStatus)
    {
        btnQuery.Enabled = blStatus;
        btnSave.Enabled = !blStatus;

    }

    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "ParaStatus = " + InvoicePara.INVOICEPARA_STATUS_YES;
        Resultset rs = baseBO.Query(new InvoicePara());
        foreach (InvoicePara invPara in rs)
        {
            dropRptType.Items.Add(new ListItem(invPara.InvHeader,invPara.InvoiceParaID.ToString()));
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (txtContractID.Text != "")
        {
            page(txtContractID.Text);
            ViewState["txtContractID"] = txtContractID.Text;

        }
        else
        {
            page("-1");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }

    }

    protected void page(string txtContractID)
    {
        int spareRow = 0;
        BaseBO baseBo = new BaseBO();
        string WhereSQL = "";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            WhereSQL = " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        string sql = "select A.CustCode,A.CustName,A.TaxCode,A.BankName,A.BankAcct,B.InvID,B.InvCode,B.InvDate,C.ContractCode,C.ContractID,C.ContractTypeID,B.InvStatus,B.InvPayAmtL" +
                     " from Customer A,InvoiceHeader B,Contract C ,ConShop" +
                     " where A.CustID = B.CustID and B.ContractID = C.ContractID and C.ContractID=ConShop.ContractID and C.BizMode = " + Contract.BIZ_MODE_LEASE + " and C.ContractCode = '" + txtContractID + "' and B.InvStatus != '" + InvoiceHeader.INVSTATUS_CANCEL +"'"+WhereSQL+ " order by B.InvID Desc";

        DataSet ds = baseBo.QueryDataSet(sql);
        DataTable dt = ds.Tables[0];
        PagedDataSource pds = new PagedDataSource();
        pds.DataSource = dt.DefaultView;
        gvCharge.DataSource = pds;
        gvCharge.DataBind();
        spareRow = gvCharge.Rows.Count;
        for (int i = 0; i < gvCharge.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }

        gvCharge.DataSource = pds;
        gvCharge.DataBind();
        for (int j = 0; j < gvCharge.PageSize - spareRow; j++)
            gvCharge.Rows[(gvCharge.PageSize - 1) - j].Cells[8].Text = "";
    }

    protected void gvCharge_SelectedIndexChanged(object sender, EventArgs e)
    {
        int invID = Convert.ToInt32(gvCharge.SelectedRow.Cells[0].Text);

        txtCustName.Text = gvCharge.SelectedRow.Cells[6].Text;
        txtContractCode.Text = gvCharge.SelectedRow.Cells[9].Text;
        txtInvoiceID.Text = gvCharge.SelectedRow.Cells[1].Text;
        txtBuildDate.Text = gvCharge.SelectedRow.Cells[3].Text;
        //txtTotalMoney.Text = gvCharge.SelectedRow.Cells[4].Text;
        txtPrintStatus.Text = (String)GetGlobalResourceObject("Parameter", InvoiceHeader.GetInvStatusDesc(Convert.ToInt32(gvCharge.SelectedRow.Cells[5].Text)));

        controlStatus(false);
        ClearGridViewSelect();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        int billFlag = 0;
        if (cbType.Checked == true)
        {
            billFlag = 1;
        }
        this.Response.Redirect("../../ReportM/RptLeaseInv.aspx?InvCode=" + txtInvoiceID.Text + "&paraID=" + Convert.ToInt32(dropRptType.SelectedValue) + "&flag=" + 1 + "&billFlag=" + billFlag);
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtCustName.Text = "";
        txtInvoiceID.Text = "";
        txtBuildDate.Text = "";
        //txtTotalMoney.Text = "";
        txtPrintStatus.Text = "";
        txtContractCode.Text = "";
        page("-1");

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
        page(ViewState["txtContractID"].ToString());
    }
    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in gvCharge.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[8].Text = "";
            }
        }
    }

}
