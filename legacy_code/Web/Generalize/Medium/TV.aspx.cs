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

public partial class Generalize_Medium_TV : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGV();

            int[] status = TV.GetTVStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",TV.GetTVStatusDesc(status[i])), status[i].ToString()));
            }

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "TV_lblTV");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtTVNm.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            TV tv = new TV();
            BaseBO baseBO = new BaseBO();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            tv.TVID = BaseApp.GetTVID();
            tv.TVNm = txtTVNm.Text.Trim();
            tv.Addr = txtAddr.Text.Trim();
            tv.ContactNm = txtUserName.Text.Trim();
            tv.Fax = txtFax.Text.Trim();
            tv.OffPhone = txtOffPhone.Text.Trim();
            tv.Phone = txtMobileTel.Text.Trim();
            tv.Title = txtTitle.Text.Trim();
            tv.Web = txtWeb.Text.Trim();
            tv.TVStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            tv.OprDeptID = sessionUser.DeptID;
            tv.OprRoleID = sessionUser.RoleID;
            tv.CreateUserID = sessionUser.UserID;

            if (baseBO.Insert(tv) == 1)
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
        if (txtTVNm.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            TV tv = new TV();
            BaseBO baseBO = new BaseBO();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            tv.TVNm = txtTVNm.Text.Trim();
            tv.Addr = txtAddr.Text.Trim();
            tv.ContactNm = txtUserName.Text.Trim();
            tv.Fax = txtFax.Text.Trim();
            tv.OffPhone = txtOffPhone.Text.Trim();
            tv.Phone = txtMobileTel.Text.Trim();
            tv.Title = txtTitle.Text.Trim();
            tv.Web = txtWeb.Text.Trim();
            tv.TVStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            tv.ModifyUserID = sessionUser.UserID;

            baseBO.WhereClause = "TVID = " + ViewState["TVID"];

            if (baseBO.Update(tv) == 1)
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

        baseBo.WhereClause = "TVStatus = " + TV.TV_STATUS_YES;

        DataTable dt = baseBo.QueryDataSet(new TV()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvTV.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTV.DataSource = pds;
            gvTV.DataBind();
        }
        else
        {
            gvTV.EmptyDataText = "";
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

            this.gvTV.DataSource = pds;
            this.gvTV.DataBind();
            spareRow = gvTV.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvTV.DataSource = pds;
            gvTV.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvTV.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
    }

    private void ClearText()
    {
        txtTVNm.Text = "";
        txtAddr.Text = "";
        txtUserName.Text = "";
        txtFax.Text = "";
        txtOffPhone.Text = "";
        txtMobileTel.Text = "";
        txtTitle.Text = "";
        txtWeb.Text = "";
        cmbStatus.SelectedIndex = 0;
    }

    protected void gvTV_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        TV tv = new TV();

        baseBO.WhereClause = "TVID = " + Convert.ToInt32(gvTV.SelectedRow.Cells[0].Text);
        rs = baseBO.Query(tv);
        if (rs.Count == 1)
        {
            tv = rs.Dequeue() as TV;

            ViewState["TVID"] = tv.TVID;
            txtAddr.Text = tv.Addr;
            txtFax.Text = tv.Fax;
            txtMobileTel.Text = tv.Phone;
            txtOffPhone.Text = tv.OffPhone;
            txtTVNm.Text = tv.TVNm;
            txtTitle.Text = tv.Title;
            txtUserName.Text = tv.ContactNm;
            txtWeb.Text = tv.Web;
            cmbStatus.SelectedValue = tv.TVStatus.ToString();
        }
        btnEdit.Enabled = true;
        BindGV();
    }
}
