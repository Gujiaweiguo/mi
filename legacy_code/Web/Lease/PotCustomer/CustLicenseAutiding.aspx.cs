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
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;

using Base.Biz;
using Base;
using Lease.PotCustLicense;
using BaseInfo.User;
using Base.DB;
using BaseInfo.Dept;
using Lease.PotCust;
using System.Drawing;

public partial class Lease_PotCustomer_CustLicenseAutiding : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            /*证照类型*/
            int[] status2 = PotCustLicenseInfo.GetPotCustLicenseType();
            for (int i = 0; i < status2.Length; i++)
            {
                cmbLicenseType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",PotCustLicenseInfo.GetPotCustLicenseTypeDesc(status2[i])), status2[i].ToString()));
            }

            if (Request.Cookies["Custumer"].Values["CustumerID"] != null)
            {
                pageDisprove("CustID=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]));
            }
            else
            {
                page();
            }
        }
    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    page();
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    page();
    //}
    protected void pageDisprove(string wherestr)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = wherestr;

        DataTable dt = baseBO.QueryDataSet(new PotCustLicenseInfo()).Tables[0];
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
            //GrdCustLicense.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 11;
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
            //this.GrdCustLicense.DataSource = pds;
            //this.GrdCustLicense.DataBind();
            //spareRow = GrdCustLicense.Rows.Count;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //GrdCustLicense.DataSource = pds;
            //GrdCustLicense.DataBind();
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
            spareRow = GrdCustLicense.Rows.Count;
            for (int i = 0; i < GrdCustLicense.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            pds.DataSource = dt.DefaultView;
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
        }
    }
    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        if (Convert.ToString(ViewState["CustID"]) == "")
        {
            baseBO.WhereClause = "CustID=" + 0;
        }
        else
        {
            baseBO.WhereClause = "CustID=" + ViewState["CustID"];
        }
        DataTable dt = baseBO.QueryDataSet(new PotCustLicenseInfo()).Tables[0];
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
            //GrdCustLicense.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 11;
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
            //this.GrdCustLicense.DataSource = pds;
            //this.GrdCustLicense.DataBind();
            //spareRow = GrdCustLicense.Rows.Count;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //GrdCustLicense.DataSource = pds;
            //GrdCustLicense.DataBind();
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
            spareRow = GrdCustLicense.Rows.Count;
            for (int i = 0; i < GrdCustLicense.PageSize - spareRow; i++)
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

    protected void GrdCustLicense_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
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
    }

    protected void GrdCustLicense_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["custLicenseID"] = GrdCustLicense.SelectedRow.Cells[0].Text;
        BaseBO bo = new BaseBO();
        bo.WhereClause = "CustLicenseID=" + ViewState["custLicenseID"].ToString();
        Resultset rs = bo.Query(new PotCustLicenseInfo());
        if (rs.Count != 0)
        {
            PotCustLicenseInfo potCustLicense = rs.Dequeue() as PotCustLicenseInfo;
            txtLicenseID.Text = potCustLicense.CustLicenseCode;
            txtLicenseName.Text = potCustLicense.CustLicenseName;
            cmbLicenseType.SelectedValue = potCustLicense.CustLicenseType.ToString();
            txtLicenseBeginDate.Text = potCustLicense.CustLicenseStartDate.ToShortDateString();
            txtLicenseEndDate.Text = potCustLicense.CustLicenseEndDate.ToShortDateString();
        }
        pageDisprove("CustID=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]));
    }
    protected void GrdCustLicense_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        pageDisprove("CustID=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]));
        foreach (GridViewRow grv in GrdCustLicense.Rows)
        {
            grv.BackColor = Color.White;
        }
        txtLicenseID.Text = "";
        txtLicenseName.Text = "";
        txtLicenseBeginDate.Text = "";
        txtLicenseEndDate.Text = "";
        cmbLicenseType.SelectedValue = null;
    }
}
