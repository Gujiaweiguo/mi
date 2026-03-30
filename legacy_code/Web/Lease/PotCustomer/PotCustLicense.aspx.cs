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
using Lease.PotCustLicense;
using Base.Page;
using BaseInfo.User;
using Base.DB;
using Lease.PotCust;
using Base.Page;
using System.Drawing;

public partial class Lease_PotCustomer_PotCustLicense : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;
    public string baseInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            int[] status2 = PotCustLicenseInfo.GetPotCustLicenseType();
            for (int i = 0; i < status2.Length; i++)
            {
                cmbLicenseType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",PotCustLicenseInfo.GetPotCustLicenseTypeDesc(status2[i])), status2[i].ToString()));
            }
            int tempCustID = 0;
            if (Request.Cookies["Info1"] != null)
            {
                tempCustID = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
                BindGridViewPotCustLicense("CustID=" + tempCustID);
            }
            else
            {
                BindGridViewPotCustLicense("CustID=0");
            }
            ViewState["custID"] = tempCustID;
        }
    }

    public void BindGridViewPotCustLicense(string wherestr)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = wherestr;
        //DataTable dt = baseBO.QueryDataSet(new PotCustLicenseInfo()).Tables[0];
        BaseInfo.BaseCommon.BindGridView(baseBO, new PotCustLicenseInfo(), this.GrdCustLicense);
        #region
        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < GrdCustLicense.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdCustLicense.DataSource = pds;
        //    GrdCustLicense.DataBind();
        //}
        //else
        //{
        //    GrdCustLicense.EmptyDataText = "";
        //    pds.AllowPaging = true;
        //    pds.PageSize = 11;
        //    lblLTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
        //    pds.CurrentPageIndex = int.Parse(lblLCurrent.Text) - 1;
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
        //    this.GrdCustLicense.DataSource = pds;
        //    this.GrdCustLicense.DataBind();
        //    spareRow = GrdCustLicense.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdCustLicense.DataSource = pds;
        //    GrdCustLicense.DataBind();
        //}
#endregion
    }
    public void CancelCustLicenseText()
    {
        txtLicenseID.Text = "";
        txtLicenseName.Text = "";
        txtLicenseBeginDate.Text = "";
        txtLicenseEndDate.Text = "";
    }
    protected void GrdCustLicense_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >=0)
        {
            string gIntro = "";
            if (e.Row.Cells[1].Text != "&nbsp;")
            {
                gIntro = e.Row.Cells[1].Text.ToString();
                e.Row.Cells[1].Text = SubStr(gIntro, 5);
                gIntro = e.Row.Cells[2].Text.ToString();
                //e.Row.Cells[2].Text = SubStr(gIntro, 10);
                e.Row.Cells[2].Text = DateTime.Parse(gIntro).ToShortDateString();
                gIntro = e.Row.Cells[3].Text.ToString();
                //e.Row.Cells[3].Text = SubStr(gIntro, 10);
                e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToShortDateString();
            }
            else
            {
                e.Row.Cells[4].Text = "";
            }
        }
    }
    protected void GrdCustLicense_SelectedIndexChanged(object sender, EventArgs e)
    {
        //证照
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "CustLicenseID=" + GrdCustLicense.SelectedRow.Cells[0].Text;
        ViewState["custLicenseID"] = GrdCustLicense.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(new PotCustLicenseInfo());

        PotCustLicenseInfo potCustLicenseInfo = rs.Dequeue() as PotCustLicenseInfo;

        txtLicenseID.Text = potCustLicenseInfo.CustLicenseCode;
        txtLicenseName.Text = potCustLicenseInfo.CustLicenseName;
        txtLicenseBeginDate.Text = DateTime.Parse(potCustLicenseInfo.CustLicenseStartDate.ToString()).ToShortDateString();
        txtLicenseEndDate.Text = DateTime.Parse(potCustLicenseInfo.CustLicenseEndDate.ToString()).ToShortDateString();
        cmbLicenseType.SelectedValue = potCustLicenseInfo.CustLicenseType.ToString();

        ViewState["UploadLince"] = potCustLicenseInfo.UploadLicense;
        if (potCustLicenseInfo.UploadLicense != "" && potCustLicenseInfo.UploadLicense != null)
            this.btnLook.Enabled = true;
        else
            this.btnLook.Enabled = false;

        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {

    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {

    }
    protected void GrdCustLicense_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        foreach (GridViewRow grv in this.GrdCustLicense.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    protected void btnLook_Click(object sender, EventArgs e)
    {
        if (ViewState["UploadLince"] != null || ViewState["UploadLince"].ToString() != "")
        {
            string strPath = Server.MapPath("UploadLicense\\") + ViewState["UploadLince"].ToString();
            try
            {
                System.Diagnostics.Process.Start(strPath);
            }
            catch { }
        }
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        foreach (GridViewRow grv in this.GrdCustLicense.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}
