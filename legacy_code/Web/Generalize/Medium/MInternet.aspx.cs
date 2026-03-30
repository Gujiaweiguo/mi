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
using Base.DB;
using Base.Page;
using Base;
using Generalize.Medium;
using BaseInfo.User;

public partial class Generalize_Medium_MInternet : BasePage
{
    public string baseInfo = "";
    public string errorMes = "";
    public string onlySelected = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies["AnPMaster"].Values["AnPID"] == "")
            {
                ViewState["AnPID"] = -1;
            }
            else
            {

                ViewState["AnPID"] = Request.Cookies["AnPMaster"].Values["AnPID"];
            }

            BindGV();
            BandList();

            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            btnEdit.Attributes.Add("onclick", "return InputValidator(form1)");
            txtEstCosts.Attributes.Add("onkeydown", "textleave()");
            txtDaily.Attributes.Add("onkeydown", "textleave()");
            txtMthlyFr.Attributes.Add("onkeydown", "textleave()");
            txtMthlyTo.Attributes.Add("onkeydown", "textleave()");
            errorMes = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Prints_lblPrints");
            onlySelected = (String)GetGlobalResourceObject("BaseInfo", "PubMessage_OnlySelected");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ClearText();
        btnEdit.Enabled = false;
        ClearGridViewSelected();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        MInternet mInternet = new MInternet();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mInternet.InternetID = Convert.ToInt32(cmbInternet.SelectedValue);
        mInternet.InternetNm = cmbInternet.SelectedItem.Text;
        mInternet.ContentsID = Convert.ToInt32(cmbContentsNm.SelectedValue);
        mInternet.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mInternet.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mInternet.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mInternet.Freq = MInternet.MPRINTS_FREQ_DAY;
            mInternet.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mInternet.Freq = MInternet.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mInternet.FreqMon = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqMon = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mInternet.FreqTue = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqTue = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mInternet.FreqWed = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqWed = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mInternet.FreqThu = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqThu = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mInternet.FreqFri = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqFri = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mInternet.FreqSat = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqSat = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mInternet.FreqSun = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqSun = MInternet.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mInternet.Freq = MInternet.MPRINTS_FREQ_MONTH;
            mInternet.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mInternet.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mInternet.ModifyUserID = sessionUser.UserID;

        baseBO.WhereClause = "MInternetID = " + ViewState["MInternetID"];

        if (baseBO.Update(mInternet) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ClearText();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        BindGV();
        btnEdit.Enabled = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        MInternet mInternet = new MInternet();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mInternet.MInternetID = BaseApp.GetMRadioID();
        mInternet.AnPID = Convert.ToInt32(ViewState["AnPID"]);
        mInternet.InternetID = Convert.ToInt32(cmbInternet.SelectedValue);
        mInternet.InternetNm = cmbInternet.SelectedItem.Text;
        mInternet.ContentsID = Convert.ToInt32(cmbContentsNm.SelectedValue);
        mInternet.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mInternet.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mInternet.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mInternet.Freq = MInternet.MPRINTS_FREQ_DAY;
            mInternet.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mInternet.Freq = MInternet.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mInternet.FreqMon = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqMon = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mInternet.FreqTue = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqTue = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mInternet.FreqWed = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqWed = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mInternet.FreqThu = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqThu = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mInternet.FreqFri = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqFri = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mInternet.FreqSat = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqSat = MInternet.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mInternet.FreqSun = MInternet.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mInternet.FreqSun = MInternet.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mInternet.Freq = MInternet.MPRINTS_FREQ_MONTH;
            mInternet.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mInternet.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mInternet.CreateUserID = sessionUser.UserID;
        mInternet.OprDeptID = sessionUser.DeptID;
        mInternet.OprRoleID = sessionUser.RoleID;

        if (baseBO.Insert(mInternet) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ClearText();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        BindGV();
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBo.WhereClause = "AnPID = " + ViewState["AnPID"];

        DataTable dt = baseBo.QueryDataSet(new MInternet()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvMInternet.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMInternet.DataSource = pds;
            gvMInternet.DataBind();
        }
        else
        {
            gvMInternet.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 11;
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

            this.gvMInternet.DataSource = pds;
            this.gvMInternet.DataBind();
            spareRow = gvMInternet.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMInternet.DataSource = pds;
            gvMInternet.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvMInternet.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[4].Text = "";
            }
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        BindGV();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        BindGV();
    }
    protected void rdoMthly_CheckedChanged(object sender, EventArgs e)
    {
        chkEnabled(false);
        txtDaily.Enabled = false;
        txtMthlyFr.Enabled = true;
        txtMthlyTo.Enabled = true;
        ClearGridViewSelected();
    }
    protected void rdoDaily_CheckedChanged(object sender, EventArgs e)
    {
        chkEnabled(false);
        txtDaily.Enabled = true;
        txtMthlyFr.Enabled = false;
        txtMthlyTo.Enabled = false;
        ClearGridViewSelected();
    }
    protected void rdoWeekly_CheckedChanged(object sender, EventArgs e)
    {

        chkEnabled(true);
        txtDaily.Text = "";
        txtDaily.Enabled = false;
        txtMthlyFr.Text = "";
        txtMthlyFr.Enabled = false;
        txtMthlyTo.Text = "";
        txtMthlyTo.Enabled = false;
        ClearGridViewSelected();
    }

    private void chkEnabled(Boolean boolen)
    {
        if (boolen == false)
        {
            chkFri.Checked = false;
            chkMon.Checked = false;
            chkSat.Checked = false;
            chkSun.Checked = false;
            chkThu.Checked = false;
            chkTue.Checked = false;
            chkWed.Checked = false;
        }
        chkFri.Enabled = boolen;
        chkMon.Enabled = boolen;
        chkSat.Enabled = boolen;
        chkSun.Enabled = boolen;
        chkThu.Enabled = boolen;
        chkTue.Enabled = boolen;
        chkWed.Enabled = boolen;
    }

    private void BandList()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        /*网络名称绑定*/
        baseBO.WhereClause = "InternetStatus = " + Internet.INTERNET_STATUS_YES;
        rs = baseBO.Query(new Internet());
        foreach (Internet internet in rs)
        {
            cmbInternet.Items.Add(new ListItem(internet.InternetNm, internet.InternetID.ToString()));
        }

        /*宣传内容*/
        baseBO.WhereClause = "ContentsStatus = " + Contents.CONTENTS_STATUS_YES;
        rs = baseBO.Query(new Contents());
        foreach (Contents contents in rs)
        {
            cmbContentsNm.Items.Add(new ListItem(contents.ContentsNm, contents.ContentsID.ToString()));
        }
    }

    private void ClearText()
    {
        cmbInternet.SelectedIndex = 0;
        cmbContentsNm.SelectedIndex = 0;
        txtEstCosts.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtDaily.Text = "";
        rdoDaily.Checked = true;
        rdoMthly.Checked = false;
        rdoWeekly.Checked = false;
        chkEnabled(false);
        txtMthlyFr.Text = "";
        txtMthlyFr.Enabled = false;
        txtMthlyTo.Text = "";
        txtMthlyTo.Enabled = false;
    }
    protected void gvMInternet_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        MInternet mInternet = new MInternet();

        ViewState["MInternetID"] = gvMInternet.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "MInternetID = " + gvMInternet.SelectedRow.Cells[0].Text;

        rs = baseBO.Query(mInternet);

        if (rs.Count == 1)
        {
            mInternet = rs.Dequeue() as MInternet;

            cmbInternet.SelectedValue = mInternet.InternetID.ToString();
            cmbContentsNm.SelectedValue = mInternet.ContentsID.ToString();
            txtEstCosts.Text = mInternet.EstCosts.ToString();
            txtStartDate.Text = mInternet.StartDate.ToString("yyy-MM-dd");
            txtEndDate.Text = mInternet.EndDate.ToString("yyyy-MM-dd");

            if (mInternet.Freq == MInternet.MPRINTS_FREQ_DAY)
            {
                rdoDaily.Checked = true;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = false;

                txtDaily.Enabled = true;
                txtDaily.Text = mInternet.FreqDays.ToString();

                chkEnabled(false);

                txtMthlyFr.Text = "";
                txtMthlyFr.Enabled = false;

                txtMthlyTo.Text = "";
                txtMthlyTo.Enabled = false;
            }
            else if (mInternet.Freq == MInternet.MPRINTS_FREQ_WEEK)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = true;
                rdoMthly.Checked = false;

                txtDaily.Text = "";
                txtDaily.Enabled = false;

                if (mInternet.FreqMon == MInternet.MPRINTS_WEEK_STATUS_YES)
                {
                    chkMon.Checked = true;
                }
                else
                {
                    chkMon.Checked = false;
                }

                if (mInternet.FreqTue == MInternet.MPRINTS_WEEK_STATUS_YES)
                {
                    chkTue.Checked = true;
                }
                else
                {
                    chkTue.Checked = false;
                }

                if (mInternet.FreqWed == MInternet.MPRINTS_WEEK_STATUS_YES)
                {
                    chkWed.Checked = true;
                }
                else
                {
                    chkWed.Checked = false;
                }

                if (mInternet.FreqThu == MInternet.MPRINTS_WEEK_STATUS_YES)
                {
                    chkThu.Checked = true;
                }
                else
                {
                    chkThu.Checked = false;
                }

                if (mInternet.FreqFri == MInternet.MPRINTS_WEEK_STATUS_YES)
                {
                    chkFri.Checked = true;
                }
                else
                {
                    chkFri.Checked = false;
                }

                if (mInternet.FreqSat == MInternet.MPRINTS_WEEK_STATUS_YES)
                {
                    chkSat.Checked = true;
                }
                else
                {
                    chkSat.Checked = false;
                }

                if (mInternet.FreqSun == MInternet.MPRINTS_WEEK_STATUS_YES)
                {
                    chkSun.Checked = true;
                }
                else
                {
                    chkSun.Checked = false;
                }

                chkEnabled(true);

                txtMthlyFr.Text = "";
                txtMthlyFr.Enabled = false;

                txtMthlyTo.Text = "";
                txtMthlyTo.Enabled = false;
            }
            else if (mInternet.Freq == MInternet.MPRINTS_FREQ_MONTH)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = true;

                txtDaily.Enabled = false;
                txtDaily.Text = "";

                chkEnabled(false);

                txtMthlyFr.Text = mInternet.BetweenFr.ToString();
                txtMthlyFr.Enabled = true;

                txtMthlyTo.Text = mInternet.BetweenTo.ToString();
                txtMthlyTo.Enabled = true;
            }
        }
        ClearGridViewSelected();

        btnEdit.Enabled = true;
    }
}
