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

public partial class Generalize_Medium_Radio : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGV();

            int[] status = Radio.GetRadioStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Radio.GetRadioStatusDesc(status[i])), status[i].ToString()));
            }

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Radio_lblRadio");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtRadioNm.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            Radio radio = new Radio();
            BaseBO baseBO = new BaseBO();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            radio.RadioID = BaseApp.GetRadioID();
            radio.RadioNm = txtRadioNm.Text.Trim();
            radio.Addr = txtAddr.Text.Trim();
            radio.ContactNm = txtUserName.Text.Trim();
            radio.Fax = txtFax.Text.Trim();
            radio.OffPhone = txtOffPhone.Text.Trim();
            radio.Phone = txtMobileTel.Text.Trim();
            radio.Title = txtTitle.Text.Trim();
            radio.Web = txtWeb.Text.Trim();
            radio.RadioStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            radio.OprDeptID = sessionUser.DeptID;
            radio.OprRoleID = sessionUser.RoleID;
            radio.CreateUserID = sessionUser.UserID;

            if (baseBO.Insert(radio) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        ClearText();
        BindGV();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (txtRadioNm.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            Radio radio = new Radio();
            BaseBO baseBO = new BaseBO();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            radio.RadioNm = txtRadioNm.Text.Trim();
            radio.Addr = txtAddr.Text.Trim();
            radio.ContactNm = txtUserName.Text.Trim();
            radio.Fax = txtFax.Text.Trim();
            radio.OffPhone = txtOffPhone.Text.Trim();
            radio.Phone = txtMobileTel.Text.Trim();
            radio.Title = txtTitle.Text.Trim();
            radio.Web = txtWeb.Text.Trim();
            radio.RadioStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            radio.ModifyUserID = sessionUser.UserID;

            baseBO.WhereClause = "RadioID = " + ViewState["RadioID"];

            if (baseBO.Update(radio) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                btnEdit.Enabled = false;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        ClearText();
        BindGV();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ClearText();
        ClearGridViewSelected();
        btnEdit.Enabled = false;
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

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBo.WhereClause = "RadioStatus = " + Radio.RADIO_STATUS_YES;

        DataTable dt = baseBo.QueryDataSet(new Radio()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvRadio.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvRadio.DataSource = pds;
            gvRadio.DataBind();
        }
        else
        {
            gvRadio.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 7;
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

            this.gvRadio.DataSource = pds;
            this.gvRadio.DataBind();
            spareRow = gvRadio.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvRadio.DataSource = pds;
            gvRadio.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvRadio.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
    }

    private void ClearText()
    {
        txtRadioNm.Text = "";
        txtAddr.Text = "";
        txtUserName.Text = "";
        txtFax.Text = "";
        txtOffPhone.Text = "";
        txtMobileTel.Text = "";
        txtTitle.Text = "";
        txtWeb.Text = "";
        cmbStatus.SelectedIndex = 0;
    }

    protected void gvRadio_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        Radio radio = new Radio();

        baseBO.WhereClause = "RadioID = " + Convert.ToInt32(gvRadio.SelectedRow.Cells[0].Text);
        rs = baseBO.Query(radio);
        if (rs.Count == 1)
        {
            radio = rs.Dequeue() as Radio;

            ViewState["RadioID"] = radio.RadioID;
            txtAddr.Text = radio.Addr;
            txtFax.Text = radio.Fax;
            txtMobileTel.Text = radio.Phone;
            txtOffPhone.Text = radio.OffPhone;
            txtRadioNm.Text = radio.RadioNm;
            txtTitle.Text = radio.Title;
            txtUserName.Text = radio.ContactNm;
            txtWeb.Text = radio.Web;
            cmbStatus.SelectedValue = radio.RadioStatus.ToString();
        }
        btnEdit.Enabled = true;
        BindGV();
    }
}
