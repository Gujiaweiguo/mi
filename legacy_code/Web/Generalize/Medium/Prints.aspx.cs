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

public partial class Generalize_Medium_Prints : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGV();

            int[] status = Prints.GetPrintsStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Prints.GetPrintsStatusDesc(status[i])), status[i].ToString()));
            }

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Prints_lblPrints");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtPrintsNm.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            Prints prints = new Prints();
            BaseBO baseBO = new BaseBO();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            prints.PrintsID = BaseApp.GetPrintsID();
            prints.PrintsNm = txtPrintsNm.Text.Trim();
            prints.Addr = txtAddr.Text.Trim();
            prints.ContactNm = txtUserName.Text.Trim();
            prints.Fax = txtFax.Text.Trim();
            prints.OffPhone = txtOffPhone.Text.Trim();
            prints.Phone = txtMobileTel.Text.Trim();
            prints.Title = txtTitle.Text.Trim();
            prints.Web = txtWeb.Text.Trim();
            prints.PrintsStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            prints.OprDeptID = sessionUser.DeptID;
            prints.OprRoleID = sessionUser.RoleID;
            prints.CreateUserID = sessionUser.UserID;

            if (baseBO.Insert(prints) == 1)
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
        if (txtPrintsNm.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            Prints prints = new Prints();
            BaseBO baseBO = new BaseBO();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            prints.PrintsNm = txtPrintsNm.Text.Trim();
            prints.Addr = txtAddr.Text.Trim();
            prints.ContactNm = txtUserName.Text.Trim();
            prints.Fax = txtFax.Text.Trim();
            prints.OffPhone = txtOffPhone.Text.Trim();
            prints.Phone = txtMobileTel.Text.Trim();
            prints.Title = txtTitle.Text.Trim();
            prints.Web = txtWeb.Text.Trim();
            prints.PrintsStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            prints.ModifyUserID = sessionUser.UserID;

            baseBO.WhereClause = "PrintsID = " + ViewState["PrintsID"];

            if (baseBO.Update(prints) == 1)
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

        baseBo.WhereClause = "PrintsStatus = " + Prints.PRINTS_STATUS_YES;

        DataTable dt = baseBo.QueryDataSet(new Prints()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvPrints.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvPrints.DataSource = pds;
            gvPrints.DataBind();
        }
        else
        {
            gvPrints.EmptyDataText = "";
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

            this.gvPrints.DataSource = pds;
            this.gvPrints.DataBind();
            spareRow = gvPrints.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvPrints.DataSource = pds;
            gvPrints.DataBind();
        }
        ClearGridViewSelected();
    }
    protected void gvPrints_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        Prints prints = new Prints();

        baseBO.WhereClause = "PrintsID = " + Convert.ToInt32(gvPrints.SelectedRow.Cells[0].Text);
        rs = baseBO.Query(prints);
        if (rs.Count == 1)
        {
            prints = rs.Dequeue() as Prints;

            ViewState["PrintsID"] = prints.PrintsID;
            txtAddr.Text = prints.Addr;
            txtFax.Text = prints.Fax;
            txtMobileTel.Text = prints.Phone;
            txtOffPhone.Text = prints.OffPhone;
            txtPrintsNm.Text = prints.PrintsNm;
            txtTitle.Text = prints.Title;
            txtUserName.Text = prints.ContactNm;
            txtWeb.Text = prints.Web;
            cmbStatus.SelectedValue = prints.PrintsStatus.ToString();
        }
        btnEdit.Enabled = true;
        BindGV();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvPrints.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
    }

    private void ClearText()
    {
        txtPrintsNm.Text = "";
        txtAddr.Text = "";
        txtUserName.Text = "";
        txtFax.Text = "";
        txtOffPhone.Text = "";
        txtMobileTel.Text = "";
        txtTitle.Text = "";
        txtWeb.Text = "";
    }
}
