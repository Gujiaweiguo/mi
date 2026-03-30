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
using Base;
using Lease.CustLicense;
using Base.Page;
using BaseInfo.User;
using Base.DB;
using Lease.PotCust;
using Base.Page;
public partial class Lease_Customer_CustomerQuery : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "CustTypeStatus=" + CustType.CUST_TYPE_STATUS_VALID;
            rs = baseBO.Query(new CustType());
            foreach (CustType custtype in rs)
            {
                cmbCustType.Items.Add(new ListItem(custtype.CustTypeName, custtype.CustTypeID.ToString()));
                cmbCustTypeq.Items.Add(new ListItem(custtype.CustTypeName, custtype.CustTypeID.ToString()));
            }

            int[] status2 = CustLicenseInfo.GetPotCustLicenseType();
            for (int i = 0; i < status2.Length; i++)
            {
                cmbLicenseType.Items.Add(new ListItem(CustLicenseInfo.GetPotCustLicenseTypeDesc(status2[i]), status2[i].ToString()));
            }
            page("a.CustID= 0");
            BindGridViewPotCustLicense("CustID=0");
        }
    }
    public void BindGridView()
    {
        this.GrdCust.DataSource = new BaseBO().Query(new CustomerQuery());
        this.GrdCust.DataBind();
    }

    protected void Button5_Click(object sender, EventArgs e)
    {

    }

    public void BindGridViewPotCustLicense(string wherestr)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = wherestr;
        DataTable dt = baseBO.QueryDataSet(new CustLicenseInfo()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCustLicense.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
        }
        else
        {
            GrdCustLicense.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 11;
            lblLTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblLCurrent.Text) - 1;
            if (pds.IsFirstPage)
            {
                btnLBack.Enabled = false;
                btnLNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnLBack.Enabled = true;
                btnLNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnLBack.Enabled = false;
                btnLNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnLBack.Enabled = true;
                btnLNext.Enabled = true;
            }
            this.GrdCustLicense.DataSource = pds;
            this.GrdCustLicense.DataBind();
            spareRow = GrdCustLicense.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
        }

    }
    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        sNewStr = sNewStr + "...";
        return sNewStr;
    }

    public void BindGridView1()
    {

        BaseBO basebo = new BaseBO();
        basebo.WhereClause = "CustID=" + ViewState["custID"];
        DataSet ds = basebo.QueryDataSet(new CustLicenseInfo());
        this.GrdCustLicense.DataSource = ds.Tables[0];
        this.GrdCustLicense.DataBind();

        txtLicenseID.Text = "";
        txtLicenseName.Text = "";
        txtLicenseBeginDate.Text = "";
        txtLicenseEndDate.Text = "";


    }
    public void CancelCustLicenseText()
    {
        txtLicenseID.Text = "";
        txtLicenseName.Text = "";
        txtLicenseBeginDate.Text = "";
        txtLicenseEndDate.Text = "";
    }

    protected void page(string wherestr)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = wherestr;

        DataTable dt = baseBO.QueryDataSet(new CustomerInfo()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCust.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCust.DataSource = pds;
            GrdCust.DataBind();
        }
        else
        {
            GrdCust.EmptyDataText = "";
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

    }
    protected void ibtnPrev_Click(object sender, ImageClickEventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        QueryData();
    }
    protected void ibtnNext_Click(object sender, ImageClickEventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        QueryData();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //查询
        QueryData();
        BindGridViewPotCustLicense("CustID=0");
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);

    }
    private void QueryData()
    {
        if (cmbCustTypeq.SelectedItem != null)
        {
            if (TextBox2.Text != "")
            {
                page("a.CustID=b.CustID and  a.CustTypeID='" + cmbCustTypeq.SelectedValue + "'  and CustShortName='" + TextBox2.Text + "'");
            }
            else
            {
                page("a.CustID=b.CustID and a.CustTypeID= '" + cmbCustTypeq.SelectedValue + "'");
            }
        }
        else
        {
            page("a.CustID=b.CustID and CustShortName='" + TextBox2.Text + "'");
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblLCurrent.Text) - 1);
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        QueryData();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        QueryData();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(0)", true);
    }
    protected void btnLBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblLCurrent.Text) - 1);
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void btnLNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblLCurrent.Text) + 1);
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
    }
    protected void GrdCustLicense_SelectedIndexChanged(object sender, EventArgs e)
    {
        //证照
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "CustLicenseID=" + GrdCustLicense.SelectedRow.Cells[0].Text;
        ViewState["custLicenseID"] = GrdCustLicense.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(new CustLicenseInfo());

        CustLicenseInfo potCustLicenseInfo = rs.Dequeue() as CustLicenseInfo;

        txtLicenseID.Text = potCustLicenseInfo.CustLicenseCode;
        txtLicenseName.Text = potCustLicenseInfo.CustLicenseName;
        txtLicenseBeginDate.Text = potCustLicenseInfo.CustLicenseStartDate.ToString();
        txtLicenseEndDate.Text = potCustLicenseInfo.CustLicenseEndDate.ToString();
        cmbLicenseType.SelectedValue = potCustLicenseInfo.CustLicenseType.ToString();
        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        QueryData();
    }
    protected void GrdCustLicense_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells[1].Text != "&nbsp;")
        {
            gIntro = e.Row.Cells[1].Text.ToString();
            e.Row.Cells[1].Text = SubStr(gIntro, 5);
            gIntro = e.Row.Cells[2].Text.ToString();
            e.Row.Cells[2].Text = SubStr(gIntro, 10);
            gIntro = e.Row.Cells[3].Text.ToString();
            e.Row.Cells[3].Text = SubStr(gIntro, 10);
        }
        else
        {
            e.Row.Cells[4].Text = "";
        }
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells[1].Text == "&nbsp;")
        {
            e.Row.Cells[7].Text = "";
        }

    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();

        baseBO.WhereClause = "a.CustID=" + GrdCust.SelectedRow.Cells[0].Text;
        ViewState["custID"] = GrdCust.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(new CustomerQuery());
        if (rs != null)
        {
            CustomerQuery potCustomerQuery = rs.Dequeue() as CustomerQuery;
            txtCustCode.Text = potCustomerQuery.CustCode;
            txtCustName.Text = potCustomerQuery.CustName;
            txtCustShortName.Text = potCustomerQuery.CustShortName;
            cmbCustType.SelectedValue = potCustomerQuery.CustTypeID.ToString();
            txtLegalRep.Text = potCustomerQuery.LegalRep;
            txtLegalRepTitle.Text = potCustomerQuery.LegalRepTitle;
            txtRegCap.Text = potCustomerQuery.RegCap.ToString();
            txtRegAddr.Text = potCustomerQuery.RegAddr;
            txtRegCode.Text = potCustomerQuery.RegCode;
            txtTaxCode.Text = potCustomerQuery.TaxCode;
            txtBankName.Text = potCustomerQuery.BankName;
            txtBankAcct.Text = potCustomerQuery.BankAcct;
            txtOfficeAddr.Text = potCustomerQuery.OfficeAddr;
            txtPostAddr.Text = potCustomerQuery.PostAddr;
            txtPostCode.Text = potCustomerQuery.PostCode;
            txtWeb.Text = potCustomerQuery.WebURL;
            txtContactorName.Text = potCustomerQuery.ContactMan;
            txtTitle.Text = potCustomerQuery.Title;
            txtOfficeTel.Text = potCustomerQuery.OfficeTel;
            txtMobileTel.Text = potCustomerQuery.MobileTel;
            txtEMail.Text = potCustomerQuery.EMail;
            txtCommOper.Text = potCustomerQuery.UserName;
            BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
            QueryData();
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(1)", true);
        }
    }
}
