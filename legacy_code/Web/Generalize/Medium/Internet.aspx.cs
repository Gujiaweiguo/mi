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

public partial class Generalize_Medium_Internet : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGV();

            int[] status = Internet.GetInternetStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Internet.GetInternetStatusDesc(status[i])), status[i].ToString()));
            }

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Internet_lblInternet");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtInternetNm.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            Internet intrtnet = new Internet();
            BaseBO baseBO = new BaseBO();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            intrtnet.InternetID = BaseApp.GetInternetID();
            intrtnet.InternetNm = txtInternetNm.Text.Trim();
            intrtnet.Addr = txtAddr.Text.Trim();
            intrtnet.ContactNm = txtUserName.Text.Trim();
            intrtnet.Fax = txtFax.Text.Trim();
            intrtnet.OffPhone = txtOffPhone.Text.Trim();
            intrtnet.Phone = txtMobileTel.Text.Trim();
            intrtnet.Title = txtTitle.Text.Trim();
            intrtnet.Web = txtWeb.Text.Trim();
            intrtnet.InternetStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            intrtnet.OprDeptID = sessionUser.DeptID;
            intrtnet.OprRoleID = sessionUser.RoleID;
            intrtnet.CreateUserID = sessionUser.UserID;

            if (baseBO.Insert(intrtnet) == 1)
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
        if (txtInternetNm.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            Internet intrtnet = new Internet();
            BaseBO baseBO = new BaseBO();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            intrtnet.InternetNm = txtInternetNm.Text.Trim();
            intrtnet.Addr = txtAddr.Text.Trim();
            intrtnet.ContactNm = txtUserName.Text.Trim();
            intrtnet.Fax = txtFax.Text.Trim();
            intrtnet.OffPhone = txtOffPhone.Text.Trim();
            intrtnet.Phone = txtMobileTel.Text.Trim();
            intrtnet.Title = txtTitle.Text.Trim();
            intrtnet.Web = txtWeb.Text.Trim();
            intrtnet.InternetStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            intrtnet.ModifyUserID = sessionUser.UserID;

            baseBO.WhereClause = "InternetID = " + ViewState["InternetID"];

            if (baseBO.Update(intrtnet) == 1)
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

        baseBo.WhereClause = "InternetStatus = " + Internet.INTERNET_STATUS_YES;

        DataTable dt = baseBo.QueryDataSet(new Internet()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvInternet.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvInternet.DataSource = pds;
            gvInternet.DataBind();
        }
        else
        {
            gvInternet.EmptyDataText = "";
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

            this.gvInternet.DataSource = pds;
            this.gvInternet.DataBind();
            spareRow = gvInternet.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvInternet.DataSource = pds;
            gvInternet.DataBind();
        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvInternet.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
    }

    private void ClearText()
    {
        txtInternetNm.Text = "";
        txtAddr.Text = "";
        txtUserName.Text = "";
        txtFax.Text = "";
        txtOffPhone.Text = "";
        txtMobileTel.Text = "";
        txtTitle.Text = "";
        txtWeb.Text = "";
        cmbStatus.SelectedIndex = 0;
    }

    protected void gvInternet_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        Internet intrtnet = new Internet();

        baseBO.WhereClause = "InternetID = " + Convert.ToInt32(gvInternet.SelectedRow.Cells[0].Text);
        rs = baseBO.Query(intrtnet);
        if (rs.Count == 1)
        {
            intrtnet = rs.Dequeue() as Internet;

            ViewState["InternetID"] = intrtnet.InternetID;
            txtAddr.Text = intrtnet.Addr;
            txtFax.Text = intrtnet.Fax;
            txtMobileTel.Text = intrtnet.Phone;
            txtOffPhone.Text = intrtnet.OffPhone;
            txtInternetNm.Text = intrtnet.InternetNm; ;
            txtTitle.Text = intrtnet.Title;
            txtUserName.Text = intrtnet.ContactNm;
            txtWeb.Text = intrtnet.Web;
            cmbStatus.SelectedValue = intrtnet.InternetStatus.ToString();
        }
        btnEdit.Enabled = true;
        BindGV();
    }
}
