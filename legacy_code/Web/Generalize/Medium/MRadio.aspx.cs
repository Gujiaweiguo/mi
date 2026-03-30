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

public partial class Generalize_Medium_MRadio : BasePage
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
            txtAirtime.Attributes.Add("onkeydown", "textleave()");
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
        MRadio mRadio = new MRadio();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mRadio.RadioID = Convert.ToInt32(cmbRadio.SelectedValue);
        mRadio.RadioNm = cmbRadio.SelectedItem.Text;
        mRadio.Airtime = Convert.ToInt32(txtAirtime.Text);
        mRadio.ContentsID = Convert.ToInt32(cmbContentsNm.SelectedValue);
        mRadio.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mRadio.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mRadio.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mRadio.Freq = MRadio.MPRINTS_FREQ_DAY;
            mRadio.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mRadio.Freq = MRadio.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mRadio.FreqMon = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqMon = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mRadio.FreqTue = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqTue = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mRadio.FreqWed = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqWed = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mRadio.FreqThu = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqThu = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mRadio.FreqFri = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqFri = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mRadio.FreqSat = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqSat = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mRadio.FreqSun = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqSun = MRadio.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mRadio.Freq = MRadio.MPRINTS_FREQ_MONTH;
            mRadio.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mRadio.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mRadio.ModifyUserID = sessionUser.UserID;

        baseBO.WhereClause = "MRadioID = " + ViewState["MRadioID"];

        if (baseBO.Update(mRadio) == 1)
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
        MRadio mRadio = new MRadio();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mRadio.MRadioID = BaseApp.GetMRadioID();
        mRadio.AnPID = Convert.ToInt32(ViewState["AnPID"]);
        mRadio.RadioID = Convert.ToInt32(cmbRadio.SelectedValue);
        mRadio.RadioNm = cmbRadio.SelectedItem.Text;
        mRadio.Airtime = Convert.ToInt32(txtAirtime.Text);
        mRadio.ContentsID = Convert.ToInt32(cmbContentsNm.SelectedValue);
        mRadio.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mRadio.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mRadio.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mRadio.Freq = MRadio.MPRINTS_FREQ_DAY;
            mRadio.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mRadio.Freq = MRadio.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mRadio.FreqMon = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqMon = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mRadio.FreqTue = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqTue = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mRadio.FreqWed = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqWed = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mRadio.FreqThu = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqThu = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mRadio.FreqFri = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqFri = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mRadio.FreqSat = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqSat = MRadio.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mRadio.FreqSun = MRadio.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mRadio.FreqSun = MRadio.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mRadio.Freq = MRadio.MPRINTS_FREQ_MONTH;
            mRadio.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mRadio.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mRadio.CreateUserID = sessionUser.UserID;
        mRadio.OprDeptID = sessionUser.DeptID;
        mRadio.OprRoleID = sessionUser.RoleID;

        if (baseBO.Insert(mRadio) == 1)
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

        DataTable dt = baseBo.QueryDataSet(new MRadio()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvMRadio.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMRadio.DataSource = pds;
            gvMRadio.DataBind();
        }
        else
        {
            gvMRadio.EmptyDataText = "";
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

            this.gvMRadio.DataSource = pds;
            this.gvMRadio.DataBind();
            spareRow = gvMRadio.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMRadio.DataSource = pds;
            gvMRadio.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvMRadio.Rows)
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

        /*电台名称绑定*/
        baseBO.WhereClause = "RadioStatus = " + Radio.RADIO_STATUS_YES;
        rs = baseBO.Query(new Radio());
        foreach (Radio radio in rs)
        {
            cmbRadio.Items.Add(new ListItem(radio.RadioNm, radio.RadioID.ToString()));
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
        cmbRadio.SelectedIndex = 0;
        txtAirtime.Text = "";
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
    protected void gvMRadio_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        MRadio mRadio = new MRadio();

        ViewState["MRadioID"] = gvMRadio.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "MRadioID = " + gvMRadio.SelectedRow.Cells[0].Text;

        rs = baseBO.Query(mRadio);

        if (rs.Count == 1)
        {
            mRadio = rs.Dequeue() as MRadio;

            cmbRadio.SelectedValue = mRadio.RadioID.ToString();
            cmbContentsNm.SelectedValue = mRadio.ContentsID.ToString();
            txtAirtime.Text = mRadio.Airtime.ToString();
            txtEstCosts.Text = mRadio.EstCosts.ToString();
            txtStartDate.Text = mRadio.StartDate.ToString("yyy-MM-dd");
            txtEndDate.Text = mRadio.EndDate.ToString("yyyy-MM-dd");

            if (mRadio.Freq == MRadio.MPRINTS_FREQ_DAY)
            {
                rdoDaily.Checked = true;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = false;

                txtDaily.Enabled = true;
                txtDaily.Text = mRadio.FreqDays.ToString();

                chkEnabled(false);

                txtMthlyFr.Text = "";
                txtMthlyFr.Enabled = false;

                txtMthlyTo.Text = "";
                txtMthlyTo.Enabled = false;
            }
            else if (mRadio.Freq == MRadio.MPRINTS_FREQ_WEEK)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = true;
                rdoMthly.Checked = false;

                txtDaily.Text = "";
                txtDaily.Enabled = false;

                if (mRadio.FreqMon == MRadio.MPRINTS_WEEK_STATUS_YES)
                {
                    chkMon.Checked = true;
                }
                else
                {
                    chkMon.Checked = false;
                }

                if (mRadio.FreqTue == MRadio.MPRINTS_WEEK_STATUS_YES)
                {
                    chkTue.Checked = true;
                }
                else
                {
                    chkTue.Checked = false;
                }

                if (mRadio.FreqWed == MRadio.MPRINTS_WEEK_STATUS_YES)
                {
                    chkWed.Checked = true;
                }
                else
                {
                    chkWed.Checked = false;
                }

                if (mRadio.FreqThu == MRadio.MPRINTS_WEEK_STATUS_YES)
                {
                    chkThu.Checked = true;
                }
                else
                {
                    chkThu.Checked = false;
                }

                if (mRadio.FreqFri == MRadio.MPRINTS_WEEK_STATUS_YES)
                {
                    chkFri.Checked = true;
                }
                else
                {
                    chkFri.Checked = false;
                }

                if (mRadio.FreqSat == MRadio.MPRINTS_WEEK_STATUS_YES)
                {
                    chkSat.Checked = true;
                }
                else
                {
                    chkSat.Checked = false;
                }

                if (mRadio.FreqSun == MRadio.MPRINTS_WEEK_STATUS_YES)
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
            else if (mRadio.Freq == MRadio.MPRINTS_FREQ_MONTH)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = true;

                txtDaily.Enabled = false;
                txtDaily.Text = "";

                chkEnabled(false);

                txtMthlyFr.Text = mRadio.BetweenFr.ToString();
                txtMthlyFr.Enabled = true;

                txtMthlyTo.Text = mRadio.BetweenTo.ToString();
                txtMthlyTo.Enabled = true;
            }
        }
        ClearGridViewSelected();

        btnEdit.Enabled = true;
    }
}
