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

public partial class Generalize_Medium_MActivity : BasePage
{
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

            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            btnEdit.Attributes.Add("onclick", "return InputValidator(form1)");
            errorMes = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            onlySelected = (String)GetGlobalResourceObject("BaseInfo", "PubMessage_OnlySelected");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
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
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ClearText();

        ClearGridViewSelected();

        btnEdit.Enabled = false;
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        MActivity mActivity = new MActivity();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mActivity.ActivityNm = txtAnPNm.Text.Trim();
        mActivity.ContentsNm = txtContentsNm.Text.Trim();
        mActivity.Situs = txtSitus.Text.Trim();
        mActivity.Intention = txtIntention.Text.Trim();
        mActivity.Bound = txtBound.Text.Trim();
        mActivity.Mcompany = txtMcompany.Text.Trim();
        mActivity.Pcompany = txtPcompany.Text.Trim();
        mActivity.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mActivity.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mActivity.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mActivity.Freq = MActivity.MPRINTS_FREQ_DAY;
            mActivity.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mActivity.Freq = MActivity.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mActivity.FreqMon = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqMon = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mActivity.FreqTue = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqTue = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mActivity.FreqWed = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqWed = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mActivity.FreqThu = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqThu = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mActivity.FreqFri = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqFri = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mActivity.FreqSat = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqSat = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mActivity.FreqSun = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqSun = MActivity.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mActivity.Freq = MActivity.MPRINTS_FREQ_MONTH;
            mActivity.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mActivity.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mActivity.ModifyUserID = sessionUser.UserID;

        baseBO.WhereClause = "MActivityID = " + ViewState["MActivityID"];

        if (baseBO.Update(mActivity) == 1)
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        MActivity mActivity = new MActivity();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mActivity.MActivityID = BaseApp.GetMActivityID();
        mActivity.AnPID = Convert.ToInt32(ViewState["AnPID"]);
        mActivity.ActivityNm = txtAnPNm.Text.Trim();
        mActivity.ContentsNm = txtContentsNm.Text.Trim();
        mActivity.Situs = txtSitus.Text.Trim();
        mActivity.Intention = txtIntention.Text.Trim();
        mActivity.Bound = txtBound.Text.Trim();
        mActivity.Mcompany = txtMcompany.Text.Trim();
        mActivity.Pcompany = txtPcompany.Text.Trim();
        mActivity.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mActivity.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mActivity.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mActivity.Freq = MActivity.MPRINTS_FREQ_DAY;
            mActivity.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mActivity.Freq = MActivity.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mActivity.FreqMon = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqMon = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mActivity.FreqTue = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqTue = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mActivity.FreqWed = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqWed = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mActivity.FreqThu = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqThu = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mActivity.FreqFri = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqFri = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mActivity.FreqSat = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqSat = MActivity.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mActivity.FreqSun = MActivity.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mActivity.FreqSun = MActivity.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mActivity.Freq = MActivity.MPRINTS_FREQ_MONTH;
            mActivity.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mActivity.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mActivity.CreateUserID = sessionUser.UserID;
        mActivity.OprDeptID = sessionUser.DeptID;
        mActivity.OprRoleID = sessionUser.RoleID;

        if (baseBO.Insert(mActivity) == 1)
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
    protected void rdoMthly_CheckedChanged(object sender, EventArgs e)
    {
        chkEnabled(false);
        txtDaily.Enabled = false;
        txtMthlyFr.Enabled = true;
        txtMthlyTo.Enabled = true;
        ClearGridViewSelected();
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBo.WhereClause = "AnPID = " + ViewState["AnPID"];

        DataTable dt = baseBo.QueryDataSet(new MActivity()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvMActivity.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMActivity.DataSource = pds;
            gvMActivity.DataBind();
        }
        else
        {
            gvMActivity.EmptyDataText = "";
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

            this.gvMActivity.DataSource = pds;
            this.gvMActivity.DataBind();
            spareRow = gvMActivity.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMActivity.DataSource = pds;
            gvMActivity.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvMActivity.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[4].Text = "";
            }
        }
    }
    protected void gvMActivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        MActivity mActivity = new MActivity();

        ViewState["MActivityID"] = gvMActivity.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "MActivityID = " + gvMActivity.SelectedRow.Cells[0].Text;

        rs = baseBO.Query(mActivity);

        if (rs.Count == 1)
        {
            mActivity = rs.Dequeue() as MActivity;

            txtAnPNm.Text = mActivity.ActivityNm;
            txtContentsNm.Text = mActivity.ContentsNm;
            txtSitus.Text = mActivity.Situs;
            txtIntention.Text = mActivity.Intention;
            txtBound.Text = mActivity.Bound;
            txtMcompany.Text = mActivity.Mcompany;
            txtPcompany.Text = mActivity.Pcompany;
            txtEstCosts.Text = mActivity.EstCosts.ToString();
            txtStartDate.Text = mActivity.StartDate.ToString("yyy-MM-dd");
            txtEndDate.Text = mActivity.EndDate.ToString("yyyy-MM-dd");

            if (mActivity.Freq == MActivity.MPRINTS_FREQ_DAY)
            {
                rdoDaily.Checked = true;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = false;

                txtDaily.Enabled = true;
                txtDaily.Text = mActivity.FreqDays.ToString();

                chkEnabled(false);

                txtMthlyFr.Text = "";
                txtMthlyFr.Enabled = false;

                txtMthlyTo.Text = "";
                txtMthlyTo.Enabled = false;
            }
            else if (mActivity.Freq == MActivity.MPRINTS_FREQ_WEEK)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = true;
                rdoMthly.Checked = false;

                txtDaily.Text = "";
                txtDaily.Enabled = false;

                if (mActivity.FreqMon == MActivity.MPRINTS_WEEK_STATUS_YES)
                {
                    chkMon.Checked = true;
                }
                else
                {
                    chkMon.Checked = false;
                }

                if (mActivity.FreqTue == MActivity.MPRINTS_WEEK_STATUS_YES)
                {
                    chkTue.Checked = true;
                }
                else
                {
                    chkTue.Checked = false;
                }

                if (mActivity.FreqWed == MActivity.MPRINTS_WEEK_STATUS_YES)
                {
                    chkWed.Checked = true;
                }
                else
                {
                    chkWed.Checked = false;
                }

                if (mActivity.FreqThu == MActivity.MPRINTS_WEEK_STATUS_YES)
                {
                    chkThu.Checked = true;
                }
                else
                {
                    chkThu.Checked = false;
                }

                if (mActivity.FreqFri == MActivity.MPRINTS_WEEK_STATUS_YES)
                {
                    chkFri.Checked = true;
                }
                else
                {
                    chkFri.Checked = false;
                }

                if (mActivity.FreqSat == MActivity.MPRINTS_WEEK_STATUS_YES)
                {
                    chkSat.Checked = true;
                }
                else
                {
                    chkSat.Checked = false;
                }

                if (mActivity.FreqSun == MActivity.MPRINTS_WEEK_STATUS_YES)
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
            else if (mActivity.Freq == MActivity.MPRINTS_FREQ_MONTH)
            { 
                rdoDaily.Checked = false;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = true;

                txtDaily.Enabled = false;
                txtDaily.Text = "";

                chkEnabled(false);

                txtMthlyFr.Text = mActivity.BetweenFr.ToString();
                txtMthlyFr.Enabled = true;

                txtMthlyTo.Text = mActivity.BetweenTo.ToString();
                txtMthlyTo.Enabled = true;
            }
        }
        ClearGridViewSelected();

        btnEdit.Enabled = true;
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

    private void ClearText()
    {
        txtAnPNm.Text = "";
        txtContentsNm.Text = "";
        txtSitus.Text = "";
        txtIntention.Text = "";
        txtBound.Text = "";
        txtMcompany.Text = "";
        txtPcompany.Text = "";
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
}
