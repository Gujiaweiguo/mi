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

public partial class Generalize_Medium_MTV : BasePage
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
        MTV mtv = new MTV();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mtv.TVID = Convert.ToInt32(cmbTVNm.SelectedValue);
        mtv.TVNm = cmbTVNm.SelectedItem.Text;
        mtv.Airtime = Convert.ToInt32(txtAirtime.Text);
        mtv.ContentsID = Convert.ToInt32(cmbContentsNm.SelectedValue);
        mtv.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mtv.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mtv.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mtv.Freq = MTV.MPRINTS_FREQ_DAY;
            mtv.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mtv.Freq = MTV.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mtv.FreqMon = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqMon = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mtv.FreqTue = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqTue = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mtv.FreqWed = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqWed = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mtv.FreqThu = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqThu = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mtv.FreqFri = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqFri = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mtv.FreqSat = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqSat = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mtv.FreqSun = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqSun = MTV.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mtv.Freq = MTV.MPRINTS_FREQ_MONTH;
            mtv.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mtv.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mtv.ModifyUserID = sessionUser.UserID;

        baseBO.WhereClause = "MTVID = " + ViewState["MTVID"];

        if (baseBO.Update(mtv) == 1)
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
        MTV mtv = new MTV();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mtv.MTVID = BaseApp.GetMTVID();
        mtv.AnPID = Convert.ToInt32(ViewState["AnPID"]);
        mtv.TVID = Convert.ToInt32(cmbTVNm.SelectedValue);
        mtv.TVNm = cmbTVNm.SelectedItem.Text;
        mtv.Airtime = Convert.ToInt32(txtAirtime.Text);
        mtv.ContentsID = Convert.ToInt32(cmbContentsNm.SelectedValue);
        mtv.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mtv.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mtv.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mtv.Freq = MTV.MPRINTS_FREQ_DAY;
            mtv.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mtv.Freq = MTV.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mtv.FreqMon = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqMon = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mtv.FreqTue = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqTue = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mtv.FreqWed = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqWed = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mtv.FreqThu = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqThu = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mtv.FreqFri = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqFri = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mtv.FreqSat = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqSat = MTV.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mtv.FreqSun = MTV.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mtv.FreqSun = MTV.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mtv.Freq = MTV.MPRINTS_FREQ_MONTH;
            mtv.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mtv.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mtv.CreateUserID = sessionUser.UserID;
        mtv.OprDeptID = sessionUser.DeptID;
        mtv.OprRoleID = sessionUser.RoleID;

        if (baseBO.Insert(mtv) == 1)
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

        DataTable dt = baseBo.QueryDataSet(new MTV()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvMTV.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMTV.DataSource = pds;
            gvMTV.DataBind();
        }
        else
        {
            gvMTV.EmptyDataText = "";
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

            this.gvMTV.DataSource = pds;
            this.gvMTV.DataBind();
            spareRow = gvMTV.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMTV.DataSource = pds;
            gvMTV.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvMTV.Rows)
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

        /*媒体名称绑定*/
        baseBO.WhereClause = "TVStatus = " + TV.TV_STATUS_YES;
        rs = baseBO.Query(new TV());
        foreach (TV tv in rs)
        {
            cmbTVNm.Items.Add(new ListItem(tv.TVNm, tv.TVID.ToString()));
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
        cmbTVNm.SelectedIndex = 0;
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
    protected void gvMTV_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        MTV mtv = new MTV();

        ViewState["MTVID"] = gvMTV.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "MTVID = " + gvMTV.SelectedRow.Cells[0].Text;

        rs = baseBO.Query(mtv);

        if (rs.Count == 1)
        {
            mtv = rs.Dequeue() as MTV;

            cmbTVNm.SelectedValue = mtv.TVID.ToString();
            cmbContentsNm.SelectedValue = mtv.ContentsID.ToString();
            txtAirtime.Text = mtv.Airtime.ToString();
            txtEstCosts.Text = mtv.EstCosts.ToString();
            txtStartDate.Text = mtv.StartDate.ToString("yyy-MM-dd");
            txtEndDate.Text = mtv.EndDate.ToString("yyyy-MM-dd");

            if (mtv.Freq == MTV.MPRINTS_FREQ_DAY)
            {
                rdoDaily.Checked = true;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = false;

                txtDaily.Enabled = true;
                txtDaily.Text = mtv.FreqDays.ToString();

                chkEnabled(false);

                txtMthlyFr.Text = "";
                txtMthlyFr.Enabled = false;

                txtMthlyTo.Text = "";
                txtMthlyTo.Enabled = false;
            }
            else if (mtv.Freq == MTV.MPRINTS_FREQ_WEEK)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = true;
                rdoMthly.Checked = false;

                txtDaily.Text = "";
                txtDaily.Enabled = false;

                if (mtv.FreqMon == MTV.MPRINTS_WEEK_STATUS_YES)
                {
                    chkMon.Checked = true;
                }
                else
                {
                    chkMon.Checked = false;
                }

                if (mtv.FreqTue == MTV.MPRINTS_WEEK_STATUS_YES)
                {
                    chkTue.Checked = true;
                }
                else
                {
                    chkTue.Checked = false;
                }

                if (mtv.FreqWed == MTV.MPRINTS_WEEK_STATUS_YES)
                {
                    chkWed.Checked = true;
                }
                else
                {
                    chkWed.Checked = false;
                }

                if (mtv.FreqThu == MTV.MPRINTS_WEEK_STATUS_YES)
                {
                    chkThu.Checked = true;
                }
                else
                {
                    chkThu.Checked = false;
                }

                if (mtv.FreqFri == MTV.MPRINTS_WEEK_STATUS_YES)
                {
                    chkFri.Checked = true;
                }
                else
                {
                    chkFri.Checked = false;
                }

                if (mtv.FreqSat == MTV.MPRINTS_WEEK_STATUS_YES)
                {
                    chkSat.Checked = true;
                }
                else
                {
                    chkSat.Checked = false;
                }

                if (mtv.FreqSun == MTV.MPRINTS_WEEK_STATUS_YES)
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
            else if (mtv.Freq == MTV.MPRINTS_FREQ_MONTH)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = true;

                txtDaily.Enabled = false;
                txtDaily.Text = "";

                chkEnabled(false);

                txtMthlyFr.Text = mtv.BetweenFr.ToString();
                txtMthlyFr.Enabled = true;

                txtMthlyTo.Text = mtv.BetweenTo.ToString();
                txtMthlyTo.Enabled = true;
            }
        }
        ClearGridViewSelected();

        btnEdit.Enabled = true;
    }
}
