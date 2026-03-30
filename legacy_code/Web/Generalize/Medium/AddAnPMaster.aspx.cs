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

public partial class Generalize_Medium_AddAnPMaster : BasePage
{
    public string baseInfo = "";
    public string prints = "";
    public string mtv = "";
    public string radio = "";
    public string internet = "";
    public string display = "";
    public string activity = "";
    public string errorMes = "";
    public string onlySelected = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["AnPID"] == null)
            {
                if (Request.Cookies["AnPMaster"].Values["AnPID"] != "")
                {
                    ViewState["AnPID"] = Request.Cookies["AnPMaster"].Values["AnPID"];
                }
            }
            else
            {
                /*把推广活动编号存入Cookies*/
                HttpCookie cookiesAnPMaster = new HttpCookie("AnPMaster");

                cookiesAnPMaster.Expires = System.DateTime.Now.AddHours(1);
                cookiesAnPMaster.Values.Add("AnPID", Request.QueryString["AnPID"]);

                Response.AppendCookie(cookiesAnPMaster);

                ViewState["AnPID"] = Request.QueryString["AnPID"];
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
            prints = (String)GetGlobalResourceObject("BaseInfo", "Master_cmdPrints");
            mtv = (String)GetGlobalResourceObject("BaseInfo", "Master_cmdTV");
            radio = (String)GetGlobalResourceObject("BaseInfo", "Master_cmdRadio");
            internet = (String)GetGlobalResourceObject("BaseInfo", "Master_cmdInternat");
            display = (String)GetGlobalResourceObject("BaseInfo", "Master_cmdDisplay");
            activity = (String)GetGlobalResourceObject("BaseInfo", "Master_cmdActivity");
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
        MPrints mPrints = new MPrints();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mPrints.PrintsID = Convert.ToInt32(cmbPrintsNm.SelectedValue);
        mPrints.PrintsNm = cmbPrintsNm.SelectedItem.Text;
        mPrints.PrintSizeID = Convert.ToInt32(cmbPrintSizeNm.SelectedValue);

        if (rdoClour.Checked)
        {
            mPrints.Colour = MPrints.MPRINTS_COLOUR_YES;
        }
        else
        {
            mPrints.Colour = MPrints.MPRINTS_COLOUR_NO;
        }

        mPrints.ContentsID = Convert.ToInt32(cmbContentsNm.SelectedValue);
        mPrints.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mPrints.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mPrints.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mPrints.Freq = MPrints.MPRINTS_FREQ_DAY;
            mPrints.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mPrints.Freq = MPrints.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mPrints.FreqMon = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqMon = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mPrints.FreqTue = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqTue = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mPrints.FreqWed = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqWed = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mPrints.FreqThu = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqThu = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mPrints.FreqFri = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqFri = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mPrints.FreqSat = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqSat = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mPrints.FreqSun = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqSun = MPrints.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mPrints.Freq = MPrints.MPRINTS_FREQ_MONTH;
            mPrints.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mPrints.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mPrints.ModifyUserID = sessionUser.UserID;

        baseBO.WhereClause = "MPrintsID = " + ViewState["MPrintsID"];

        if (baseBO.Update(mPrints) == 1)
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
        MPrints mPrints = new MPrints();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        mPrints.MPrintsID = BaseApp.GetMPrintsID();
        mPrints.AnPID = Convert.ToInt32(ViewState["AnPID"]);
        mPrints.PrintsID = Convert.ToInt32(cmbPrintsNm.SelectedValue);
        mPrints.PrintsNm = cmbPrintsNm.SelectedItem.Text;
        mPrints.PrintSizeID = Convert.ToInt32(cmbPrintSizeNm.SelectedValue);

        if (rdoClour.Checked)
        {
            mPrints.Colour = MPrints.MPRINTS_COLOUR_YES;
        }
        else
        {
            mPrints.Colour = MPrints.MPRINTS_COLOUR_NO;
        }

        mPrints.ContentsID = Convert.ToInt32(cmbContentsNm.SelectedValue);
        mPrints.EstCosts = Convert.ToDecimal(txtEstCosts.Text);
        mPrints.StartDate = Convert.ToDateTime(txtStartDate.Text);
        mPrints.EndDate = Convert.ToDateTime(txtEndDate.Text);

        if (rdoDaily.Checked)
        {
            mPrints.Freq = MPrints.MPRINTS_FREQ_DAY;
            mPrints.FreqDays = Convert.ToInt32(txtDaily.Text);
        }
        else if (rdoWeekly.Checked)
        {
            mPrints.Freq = MPrints.MPRINTS_FREQ_WEEK;
            if (chkMon.Checked)
            {
                mPrints.FreqMon = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqMon = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkTue.Checked)
            {
                mPrints.FreqTue = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqTue = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkWed.Checked)
            {
                mPrints.FreqWed = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqWed = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkThu.Checked)
            {
                mPrints.FreqThu = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqThu = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkFri.Checked)
            {
                mPrints.FreqFri = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqFri = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSat.Checked)
            {
                mPrints.FreqSat = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqSat = MPrints.MPRINTS_WEEK_STATUS_NO;
            }

            if (chkSun.Checked)
            {
                mPrints.FreqSun = MPrints.MPRINTS_WEEK_STATUS_YES;
            }
            else
            {
                mPrints.FreqSun = MPrints.MPRINTS_WEEK_STATUS_NO;
            }
        }
        else if (rdoMthly.Checked)
        {
            mPrints.Freq = MPrints.MPRINTS_FREQ_MONTH;
            mPrints.BetweenFr = Convert.ToInt32(txtMthlyFr.Text);
            mPrints.BetweenTo = Convert.ToInt32(txtMthlyTo.Text);
        }

        mPrints.CreateUserID = sessionUser.UserID;
        mPrints.OprDeptID = sessionUser.DeptID;
        mPrints.OprRoleID = sessionUser.RoleID;

        if (baseBO.Insert(mPrints) == 1)
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

        DataTable dt = baseBo.QueryDataSet(new MPrints()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvMPrints.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMPrints.DataSource = pds;
            gvMPrints.DataBind();
        }
        else
        {
            gvMPrints.EmptyDataText = "";
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

            this.gvMPrints.DataSource = pds;
            this.gvMPrints.DataBind();
            spareRow = gvMPrints.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvMPrints.DataSource = pds;
            gvMPrints.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvMPrints.Rows)
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
        baseBO.WhereClause = "PrintsStatus = " + Prints.PRINTS_STATUS_YES;
        rs = baseBO.Query(new Prints());
        foreach (Prints prints in rs)
        {
            cmbPrintsNm.Items.Add(new ListItem(prints.PrintsNm, prints.PrintsID.ToString()));
        }

        /*版面大小*/
        baseBO.WhereClause = "PrintSizeStatus = " + PrintSize.PRINTSIZE_STATUS_YES;
        rs = baseBO.Query(new PrintSize());
        foreach (PrintSize printSize in rs)
        {
            cmbPrintSizeNm.Items.Add(new ListItem(printSize.PrintSizeNm, printSize.PrintSizeID.ToString()));
        }

        /*宣传内容*/
        baseBO.WhereClause = "ContentsStatus = " + Contents.CONTENTS_STATUS_YES;
        rs = baseBO.Query(new Contents());
        foreach (Contents contents in rs)
        {
            cmbContentsNm.Items.Add(new ListItem(contents.ContentsNm, contents.ContentsID.ToString()));
        }
    }
    protected void gvMPrints_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        MPrints mPrints = new MPrints();

        ViewState["MPrintsID"] = gvMPrints.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "MPrintsID = " + gvMPrints.SelectedRow.Cells[0].Text;

        rs = baseBO.Query(mPrints);

        if (rs.Count == 1)
        {
            mPrints = rs.Dequeue() as MPrints;

            cmbPrintsNm.SelectedValue = mPrints.PrintsID.ToString();
            cmbContentsNm.SelectedValue = mPrints.ContentsID.ToString();
            cmbPrintSizeNm.SelectedValue = mPrints.PrintSizeID.ToString();

            if (mPrints.Colour == MPrints.MPRINTS_COLOUR_YES)
            {
                rdoClour.Checked = true;
                rdoBnW.Checked = false;
            }
            else if (mPrints.Colour == MPrints.MPRINTS_COLOUR_NO)
            {
                rdoClour.Checked = false;
                rdoBnW.Checked = true;
            }

            txtEstCosts.Text = mPrints.EstCosts.ToString();
            txtStartDate.Text = mPrints.StartDate.ToString("yyy-MM-dd");
            txtEndDate.Text = mPrints.EndDate.ToString("yyyy-MM-dd");

            if (mPrints.Freq == MPrints.MPRINTS_FREQ_DAY)
            {
                rdoDaily.Checked = true;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = false;

                txtDaily.Enabled = true;
                txtDaily.Text = mPrints.FreqDays.ToString();

                chkEnabled(false);

                txtMthlyFr.Text = "";
                txtMthlyFr.Enabled = false;

                txtMthlyTo.Text = "";
                txtMthlyTo.Enabled = false;
            }
            else if (mPrints.Freq == MPrints.MPRINTS_FREQ_WEEK)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = true;
                rdoMthly.Checked = false;

                txtDaily.Text = "";
                txtDaily.Enabled = false;

                if (mPrints.FreqMon == MPrints.MPRINTS_WEEK_STATUS_YES)
                {
                    chkMon.Checked = true;
                }
                else
                {
                    chkMon.Checked = false;
                }

                if (mPrints.FreqTue == MPrints.MPRINTS_WEEK_STATUS_YES)
                {
                    chkTue.Checked = true;
                }
                else
                {
                    chkTue.Checked = false;
                }

                if (mPrints.FreqWed == MPrints.MPRINTS_WEEK_STATUS_YES)
                {
                    chkWed.Checked = true;
                }
                else
                {
                    chkWed.Checked = false;
                }

                if (mPrints.FreqThu == MPrints.MPRINTS_WEEK_STATUS_YES)
                {
                    chkThu.Checked = true;
                }
                else
                {
                    chkThu.Checked = false;
                }

                if (mPrints.FreqFri == MPrints.MPRINTS_WEEK_STATUS_YES)
                {
                    chkFri.Checked = true;
                }
                else
                {
                    chkFri.Checked = false;
                }

                if (mPrints.FreqSat == MPrints.MPRINTS_WEEK_STATUS_YES)
                {
                    chkSat.Checked = true;
                }
                else
                {
                    chkSat.Checked = false;
                }

                if (mPrints.FreqSun == MPrints.MPRINTS_WEEK_STATUS_YES)
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
            else if (mPrints.Freq == MPrints.MPRINTS_FREQ_MONTH)
            {
                rdoDaily.Checked = false;
                rdoWeekly.Checked = false;
                rdoMthly.Checked = true;

                txtDaily.Enabled = false;
                txtDaily.Text = "";

                chkEnabled(false);

                txtMthlyFr.Text = mPrints.BetweenFr.ToString();
                txtMthlyFr.Enabled = true;

                txtMthlyTo.Text = mPrints.BetweenTo.ToString();
                txtMthlyTo.Enabled = true;
            }
        }
        ClearGridViewSelected();

        btnEdit.Enabled = true;
    }

    private void ClearText()
    {
        cmbPrintsNm.SelectedIndex = 0;
        cmbPrintSizeNm.SelectedIndex = 0;
        rdoClour.Checked = true;
        rdoBnW.Checked = false;
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
}
