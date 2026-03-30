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

public partial class Generalize_Medium_AnPMaster : BasePage
{
    public string baseInfo = "";
    public string errorMes = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();

            /*活动主题绑定*/
            rs = baseBO.Query(new Theme());
            //cmbThemeName.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Select_Select")));
            foreach (Theme theme in rs)
            {
                cmbThemeName.Items.Add(new ListItem(theme.ThemeNm, theme.ThemeID.ToString()));
            }

            for (int i = 0; i <= 23; i++)
            {
                if (i < 10)
                {
                    cmbStartHour.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));
                    cmbEndHour.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));

                }
                else
                {
                    cmbStartHour.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    cmbEndHour.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }

            for (int i = 0; i <= 59; i++)
            {
                if (i < 10)
                {
                    cmbStartMinute.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));
                    cmbStartSecond.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));
                    cmbEndMinute.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));
                    cmbEndSecond.Items.Add(new ListItem("0" + i.ToString(), i.ToString()));

                }
                else
                {
                    cmbStartMinute.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    cmbStartSecond.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    cmbEndMinute.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    cmbEndSecond.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            

            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            btnEdit.Attributes.Add("onclick", "return InputValidator(form1)");
            txtSales.Attributes.Add("onkeydown", "textleave()");
            txtTargetPeopletime.Attributes.Add("onkeydown", "textleave()");

            errorMes = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Master_lblAnPRecord");

            BindGV();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        AnPMaster anPMaster = new AnPMaster();
        BaseBO baseBO = new BaseBO();

        anPMaster.AnpID = BaseApp.GetAnpID();
        anPMaster.AnPNm = txtAnPNm.Text.Trim();
        anPMaster.ThemeID = Convert.ToInt32(cmbThemeName.SelectedValue);
        anPMaster.StartDate = Convert.ToDateTime(txtStartDay.Text + " " + cmbStartHour.SelectedValue + ":" + cmbStartMinute.SelectedValue + ":" + cmbStartSecond.SelectedValue);
        anPMaster.EndDate = Convert.ToDateTime(txtEndDay.Text + " " + cmbEndHour.SelectedValue + ":" + cmbEndMinute.SelectedValue + ":" + cmbEndSecond.SelectedValue);
        anPMaster.TargetSales = Convert.ToDecimal(txtSales.Text);
        anPMaster.TargetPeopletime = Convert.ToDecimal(txtTargetPeopletime.Text);
        anPMaster.Remark = txtRemark.Text.Trim();

        if (anPMaster.StartDate > anPMaster.EndDate)
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime") + "'", true);
            return;
        }

        if (baseBO.Insert(anPMaster) == 1)
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
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        AnPMaster anPMaster = new AnPMaster();
        BaseBO baseBO = new BaseBO();

        anPMaster.AnPNm = txtAnPNm.Text.Trim();
        anPMaster.ThemeID = Convert.ToInt32(cmbThemeName.SelectedValue);
        anPMaster.StartDate = Convert.ToDateTime(txtStartDay.Text + " " + cmbStartHour.SelectedValue + ":" + cmbStartMinute.SelectedValue + ":" + cmbStartSecond.SelectedValue);
        anPMaster.EndDate = Convert.ToDateTime(txtEndDay.Text + " " + cmbEndHour.SelectedValue + ":" + cmbEndMinute.SelectedValue + ":" + cmbEndSecond.SelectedValue);
        anPMaster.TargetSales = Convert.ToDecimal(txtSales.Text);
        anPMaster.TargetPeopletime = Convert.ToDecimal(txtTargetPeopletime.Text);
        anPMaster.Remark = txtRemark.Text.Trim();

        if (anPMaster.StartDate > anPMaster.EndDate)
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime") + "'", true);
            return;
        }

        baseBO.WhereClause = "AnpID = " + Convert.ToInt32(ViewState["AnPID"]);
        if (baseBO.Update(anPMaster) == 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            btnEdit.Enabled = false;
            ClearText();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
        }
        BindGV();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ClearText();
        btnEdit.Enabled = false;
        BindGV();
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBo.WhereClause = "a.ThemeID= b.ThemeID";
        DataTable dt = baseBo.QueryDataSet(new SelectAnPMaster()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvAnPMaster.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvAnPMaster.DataSource = pds;
            gvAnPMaster.DataBind();
        }
        else
        {
            gvAnPMaster.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 12;
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

            this.gvAnPMaster.DataSource = pds;
            this.gvAnPMaster.DataBind();
            spareRow = gvAnPMaster.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvAnPMaster.DataSource = pds;
            gvAnPMaster.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvAnPMaster.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[3].Text = "";
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

    private void ClearText()
    {
        txtAnPNm.Text = "";
        txtEndDay.Text = "";
        txtRemark.Text = "";
        txtSales.Text = "";
        txtStartDay.Text = "";
        txtTargetPeopletime.Text = "";
        cmbEndHour.SelectedIndex = 0;
        cmbEndMinute.SelectedIndex = 0;
        cmbEndSecond.SelectedIndex = 0;
        cmbStartHour.SelectedIndex = 0;
        cmbStartMinute.SelectedIndex = 0;
        cmbStartSecond.SelectedIndex = 0;
        cmbThemeName.SelectedIndex = 0;
    }
    protected void gvAnPMaster_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        AnPMaster anPMaster = new AnPMaster();

        ViewState["AnPID"] = gvAnPMaster.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "AnPID =" + Convert.ToInt32(ViewState["AnPID"]);

        rs = baseBO.Query(anPMaster);

        if (rs.Count == 1)
        {
            anPMaster = rs.Dequeue() as AnPMaster;

            txtAnPNm.Text = anPMaster.AnPNm;
            cmbThemeName.SelectedValue = anPMaster.ThemeID.ToString();
            txtStartDay.Text = Convert.ToDateTime(anPMaster.StartDate).Date.ToString("yyyy-MM-dd");
            txtEndDay.Text = Convert.ToDateTime(anPMaster.EndDate).Date.ToString("yyyy-MM-dd");
            cmbStartHour.SelectedValue = Convert.ToDateTime(anPMaster.StartDate).Hour.ToString();
            cmbStartMinute.SelectedValue = Convert.ToDateTime(anPMaster.StartDate).Minute.ToString();
            cmbStartSecond.SelectedValue = Convert.ToDateTime(anPMaster.StartDate).Second.ToString();
            cmbEndHour.SelectedValue = Convert.ToDateTime(anPMaster.EndDate).Hour.ToString();
            cmbEndMinute.SelectedValue = Convert.ToDateTime(anPMaster.EndDate).Minute.ToString();
            cmbEndSecond.SelectedValue = Convert.ToDateTime(anPMaster.EndDate).Second.ToString();
            txtSales.Text = anPMaster.TargetSales.ToString();
            txtTargetPeopletime.Text = anPMaster.TargetPeopletime.ToString();
            txtRemark.Text = anPMaster.Remark;

            btnEdit.Enabled = true;
        }
        ClearGridViewSelected();
    }
}
