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
using RentableArea;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using Lease.Customer;
using Lease.Contract;
using Lease.ConShop;
public partial class Lease_PotCustomer_CustPalaverAutiding : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            page();
        }
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        if (Convert.ToString(Request.Cookies["Custumer"]) == "")
        {
            baseBO.WhereClause = "CustID=" + 0;
        }
        else
        {
            baseBO.WhereClause = "CustID=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
        }


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
            pds.AllowPaging = true;
            pds.PageSize = 6;
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

    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        page();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        page();
    }

    protected void GrdCustPalaverInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells[1].Text != "&nbsp;")
        {
            gIntro = e.Row.Cells[3].Text.ToString();
            e.Row.Cells[3].Text = SubStr(gIntro, 7);
        }
        else
        {
            e.Row.Cells[4].Text = "";
        }
    }
    protected void GrdCustPalaverInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        CustPalaverInfo tempCustPalaver = new CustPalaverInfo();
        Resultset rs = new Resultset();
        baseBO.WhereClause = "PalaverID=" + GrdCustPalaverInfo.SelectedRow.Cells[0].Text;

        rs = baseBO.Query(tempCustPalaver);
        if (rs.Count == 1)
        {

            CustPalaverInfo custPalaver = rs.Dequeue() as CustPalaverInfo;
            txtPalaverContent.Text = custPalaver.PalaverContent;
            page();
        }
        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "chooseCard(1);", true);
    }

}
