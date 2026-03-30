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
using System.Drawing;

public partial class Lease_Customer_CustLicenseModify : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            int[] status2 = CustLicenseInfo.GetPotCustLicenseType();
            for (int i = 0; i < status2.Length; i++)
            {
                cmbLicenseType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",CustLicenseInfo.GetPotCustLicenseTypeDesc(status2[i])), status2[i].ToString()));
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
            btnCancel.Attributes.Add("onclick", "return LicenseTextClear()");
            this.txtLicenseName.Attributes.Add("onblur", "TextIsNotNull(txtLicenseName,ImgCustName)");
            this.btnAdd.Attributes.Add("onclick", "return CheckNull()");
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //添加
        if (ViewState["custID"].ToString() != "")
        {
            CustLicenseInfo potCustLicense = new CustLicenseInfo();
            potCustLicense.CustLicenseID = BaseApp.GetCustLicenseIDD();
            potCustLicense.CustLicenseCode = txtLicenseID.Text;
            potCustLicense.CustLicenseName = txtLicenseName.Text;
            potCustLicense.CustLicenseType = Convert.ToInt32(cmbLicenseType.SelectedValue);
            try{potCustLicense.CustLicenseStartDate = Convert.ToDateTime(txtLicenseBeginDate.Text.Trim());}
            catch { potCustLicense.CustLicenseStartDate = DateTime.Now.Date; }
            try { potCustLicense.CustLicenseEndDate = Convert.ToDateTime(txtLicenseEndDate.Text); }
            catch { potCustLicense.CustLicenseEndDate = DateTime.Now.Date; }
            potCustLicense.CustID = Convert.ToInt32(ViewState["custID"].ToString());
            BaseBO baseBo = new BaseBO();

            int result = baseBo.Insert(potCustLicense);
            if (result != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidWrite.Value + "'", true);
        }

        btnAdd.Enabled = false;
        btnEdit.Enabled = true;
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        foreach (GridViewRow grv in this.GrdCustLicense.Rows)
        {
            grv.BackColor = Color.White;
        }
        CancelCustLicenseText();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //更新
        CustLicenseInfo potCustLicense = new CustLicenseInfo();
        potCustLicense.CustLicenseCode = txtLicenseID.Text;
        potCustLicense.CustLicenseName = txtLicenseName.Text;
        potCustLicense.CustLicenseType = Convert.ToInt32(cmbLicenseType.SelectedValue);
        potCustLicense.CustLicenseStartDate = Convert.ToDateTime(txtLicenseBeginDate.Text);
        potCustLicense.CustLicenseEndDate = Convert.ToDateTime(txtLicenseEndDate.Text);

        BaseBO baseBo = new BaseBO();

        baseBo.WhereClause = "CustLicenseID=" + ViewState["custLicenseID"].ToString();

        int result = baseBo.Update(potCustLicense);
        if (result != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
            return;
        }
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        btnEdit.Enabled = false;
        btnAdd.Enabled = true;
        CancelCustLicenseText();
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/Customer/CustLicenseModify.aspx");
    }
    protected void GrdCustLicense_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
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
        txtLicenseBeginDate.Text = Convert.ToDateTime(potCustLicenseInfo.CustLicenseStartDate).ToString("yyyy-MM-dd");
        txtLicenseEndDate.Text = Convert.ToDateTime(potCustLicenseInfo.CustLicenseEndDate).ToString("yyyy-MM-dd");
        cmbLicenseType.SelectedValue = potCustLicenseInfo.CustLicenseType.ToString();
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        //QueryData();
        btnEdit.Enabled = true;
        btnAdd.Enabled = false;
    }

    public void BindGridViewPotCustLicense(string wherestr)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = wherestr;
        BaseInfo.BaseCommon.BindGridView(baseBO, new CustLicenseInfo(), this.GrdCustLicense);
        //DataTable dt = baseBO.QueryDataSet(new CustLicenseInfo()).Tables[0];
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
        cmbLicenseType.SelectedValue = CustLicenseInfo.CUST_DEAL_IN_LICENCE.ToString();
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
        this.btnEdit.Enabled = false;
        this.btnAdd.Enabled = true;
        CancelCustLicenseText();//清空输入框
    }
}
