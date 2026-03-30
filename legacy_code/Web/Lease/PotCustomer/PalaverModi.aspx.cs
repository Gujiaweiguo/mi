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

using Lease.PotCustLicense;
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

public partial class Lease_PotCustomer_PalaverModi : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int tempCustID = 0;
            if (Request.Cookies["Info1"] != null)
            {
                tempCustID = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
            }
            ViewState["custID"] = tempCustID;
            page();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CustPalaverInfo custPalaver = new CustPalaverInfo();
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "PalaverID = " + ViewState["palaverID"];
            custPalaver.PalaverTime = Convert.ToDateTime(txtPalaverTime.Text);
            custPalaver.PalaverName = txtPalaverUser.Text.Trim();
            custPalaver.PalaverAim = txtPalaverAim.Text.Trim();
            custPalaver.PalaverContent = txtPalaverNode.Text.Trim();

            if (baseBO.Update(custPalaver) != -1)
            {
                txtPalaverTime.Text = "";
                txtPalaverUser.Text = "";
                txtPalaverAim.Text = "";
                txtPalaverNode.Text = "";
                page();
            }
            else
            {
                page();
                return;
            }
        }
        catch { }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtPalaverTime.Text = "";
        txtPalaverTime.CssClass = "ipt160px";
        txtPalaverTime.ReadOnly = false;
        txtPalaverUser.Text = "";
        txtPalaverUser.CssClass = "ipt160px";
        txtPalaverUser.ReadOnly = false;
        txtPalaverAim.Text = "";
        txtPalaverAim.CssClass = "ipt160px";
        txtPalaverAim.ReadOnly = false;
        txtPalaverNode.Text = "";
        txtPalaverNode.CssClass = "OpenColor";
        txtPalaverNode.ReadOnly = false;
        page();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {

    }
    protected void btnNext_Click(object sender, EventArgs e)
    {

    }
    protected void GrdCustPalaverInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells[1].Text != "&nbsp;")
        {
            gIntro = e.Row.Cells[1].Text.ToString();
            e.Row.Cells[1].Text = SubStr(gIntro, 10);
            gIntro = e.Row.Cells[2].Text.ToString();
            e.Row.Cells[2].Text = SubStr(gIntro, 7);
        }
        else
        {
            e.Row.Cells[3].Text = "";
        }
    }
    protected void GrdCustPalaverInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Resultset rs = new Resultset();
        BaseBO baseBO = new BaseBO();
        CustPalaverInfo custPalaver = new CustPalaverInfo();
        baseBO.WhereClause = "PalaverID=" + GrdCustPalaverInfo.SelectedRow.Cells[0].Text;
        ViewState["palaverID"] = GrdCustPalaverInfo.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(custPalaver);
        if (rs.Count == 1)
        {
            CustPalaverInfo cust = rs.Dequeue() as CustPalaverInfo;
            txtPalaverTime.Text = cust.PalaverTime.ToString("yyyy-MM-dd");
            txtPalaverUser.Text = cust.PalaverName;
            txtPalaverAim.Text = cust.PalaverAim;
            txtPalaverNode.Text = cust.PalaverContent;
        }
        page();
        //PalaverTextLock();
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = "CustID=" + ViewState["custID"];
        DataTable dt = baseBO.QueryDataSet(new CustPalaverInfo()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCustPalaverInfo.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustPalaverInfo.DataSource = pds;
            GrdCustPalaverInfo.DataBind();
        }
        else
        {
            GrdCustPalaverInfo.EmptyDataText = "";
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

            this.GrdCustPalaverInfo.DataSource = pds;
            this.GrdCustPalaverInfo.DataBind();
            spareRow = GrdCustPalaverInfo.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustPalaverInfo.DataSource = pds;
            GrdCustPalaverInfo.DataBind();
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
    private void PalaverTextLock()
    {
        txtPalaverTime.ReadOnly = true;
        txtPalaverTime.CssClass = "Enabledipt160px";
        txtPalaverUser.ReadOnly = true;
        txtPalaverUser.CssClass = "Enabledipt160px";
        txtPalaverAim.ReadOnly = true;
        txtPalaverAim.CssClass = "Enabledipt160px";
        txtPalaverNode.ReadOnly = true;
        txtPalaverNode.CssClass = "EnabledColor";
    }
}
