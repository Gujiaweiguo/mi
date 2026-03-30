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

using Base.DB;
using Base.Biz;
using Base;
using Lease.InvoicePara;
using BaseInfo.User;
using Base.Page;
using Lease.Subs;

public partial class ReportM_InvoicePara_InvoicePara : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGV();
            this.BindSubS();//绑定子公司
            int[] status = InvoicePara.GetInvoiceParaStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", InvoicePara.GetInvoiceParaStatusDesc(status[i])), status[i].ToString()));
            }

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "InvoicePara_Title");
            btnEdit.Enabled = false;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }
    /// <summary>
    /// 绑定子公司
    /// </summary>
    private void BindSubS()
    {
        BaseBO objBaseBo = new BaseBO();
        this.ddlSubs.Items.Add(new ListItem("","0"));
        Resultset rs = objBaseBo.Query(new Subs());
        foreach(Subs objSubs in rs)
        {
            this.ddlSubs.Items.Add(new ListItem(objSubs.SubsName,objSubs.SubsID.ToString()));
        }
    }
    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        //baseBo.WhereClause = "a.ThemeID= b.ThemeID";
        DataTable dt = baseBo.QueryDataSet(new InvoicePara()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;


        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["ParaStatusName"] = (String)GetGlobalResourceObject("Parameter", InvoicePara.GetInvoiceParaStatusDesc(Convert.ToInt32(dt.Rows[j]["ParaStatus"])));
        }

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvInvoicePara.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvInvoicePara.DataSource = pds;
            gvInvoicePara.DataBind();
        }
        else
        {
            #region
            //gvInvoicePara.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 15;
            //lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            //pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = true;
            //}

            //this.gvInvoicePara.DataSource = pds;
            //this.gvInvoicePara.DataBind();
            //spareRow = gvInvoicePara.Rows.Count;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //gvInvoicePara.DataSource = pds;
            //gvInvoicePara.DataBind();
            #endregion
            gvInvoicePara.DataSource = pds;
            gvInvoicePara.DataBind();
            spareRow = gvInvoicePara.Rows.Count;
            for (int i = 0; i < gvInvoicePara.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            gvInvoicePara.DataSource = pds;
            gvInvoicePara.DataBind();

        }
        ClearGridViewSelected();
    }

    private void ClearGridViewSelected()
    {
        foreach (GridViewRow gvr in gvInvoicePara.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[4].Text = "";
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtInvHeader.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "", "document.all.txtInvHeader.select()", true);
            return;
        }
        else
        {
            InvoicePara invoicePara = new InvoicePara();
            BaseBO baseBO = new BaseBO();
            BaseTrans baseTrans = new BaseTrans();
            Resultset rs = new Resultset();
            Resultset rsinvPara = new Resultset();

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            invoicePara.InvoiceParaID = BaseApp.GetInvoiceParaID();
            invoicePara.InvHeader = txtInvHeader.Text.Trim();
            invoicePara.InvSubhead = txtInvSubhead.Text.Trim();
            invoicePara.InvH1 = txtInvH1.Text.Trim();
            invoicePara.InvH2 = txtInvH2.Text.Trim();
            invoicePara.InvH3 = txtInvH3.Text.Trim();
            invoicePara.InvH4 = txtInvH4.Text.Trim();
            invoicePara.InvH5 = txtInvH5.Text.Trim();
            invoicePara.InvF1 = txtInvF1.Text.Trim();
            invoicePara.InvF2 = txtInvF2.Text.Trim();
            invoicePara.InvF3 = txtInvF3.Text.Trim();
            invoicePara.InvF4 = txtInvF4.Text.Trim();
            invoicePara.InvF5 = txtInvF5.Text.Trim();
            invoicePara.InvF6 = txtInvF6.Text.Trim();
            invoicePara.InvF7 = txtInvF7.Text.Trim();
            invoicePara.SubsID = Int32.Parse(this.ddlSubs.SelectedValue.ToString());
            invoicePara.InvoiceParaDesc = txtInvoiceParaDesc.Text.Trim();
            invoicePara.ParaStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            invoicePara.OprDeptID = sessionUser.DeptID;
            invoicePara.OprRoleID = sessionUser.RoleID;
            invoicePara.CreateUserID = sessionUser.UserID;

            baseTrans.BeginTrans();

            if (chkDefault.Checked)
            {
                invoicePara.IsDefault = InvoicePara.INVOICEPARA_STATUS_YES;

                rs = baseBO.Query(invoicePara);
                if (rs.Count != 0)
                {
                    if (baseBO.ExecuteUpdate("Update InvoicePara Set IsDefault = " + InvoicePara.INVOICEPARA_STATUS_NO) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        ClearGridViewSelected();
                        return;
                    }
                }
            }
            else
            {
                baseBO.WhereClause = "IsDefault = " + InvoicePara.INVOICEPARA_STATUS_YES + " And ParaStatus =" + InvoicePara.INVOICEPARA_STATUS_YES;
                rsinvPara = baseBO.Query(invoicePara);
                if (rsinvPara.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_OnlyDefault") + "'", true);
                    ClearGridViewSelected();
                    return;
                }
            }

            if (baseBO.Insert(invoicePara) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
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
        if (txtInvHeader.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            InvoicePara invoicePara = new InvoicePara();
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            Resultset rsinvPara = new Resultset();

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            invoicePara.InvHeader = txtInvHeader.Text.Trim();
            invoicePara.InvSubhead = txtInvSubhead.Text.Trim();
            invoicePara.InvH1 = txtInvH1.Text.Trim();
            invoicePara.InvH2 = txtInvH2.Text.Trim();
            invoicePara.InvH3 = txtInvH3.Text.Trim();
            invoicePara.InvH4 = txtInvH4.Text.Trim();
            invoicePara.InvH5 = txtInvH5.Text.Trim();
            invoicePara.InvF1 = txtInvF1.Text.Trim();
            invoicePara.InvF2 = txtInvF2.Text.Trim();
            invoicePara.InvF3 = txtInvF3.Text.Trim();
            invoicePara.InvF4 = txtInvF4.Text.Trim();
            invoicePara.InvF5 = txtInvF5.Text.Trim();
            invoicePara.InvF6 = txtInvF6.Text.Trim();
            invoicePara.InvF7 = txtInvF7.Text.Trim();
            invoicePara.SubsID = Int32.Parse(this.ddlSubs.SelectedValue.ToString());
            invoicePara.InvoiceParaDesc = txtInvoiceParaDesc.Text.Trim();
            invoicePara.ParaStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            invoicePara.ModifyUserID = sessionUser.UserID;

            if (chkDefault.Checked)
            {
                invoicePara.IsDefault = InvoicePara.INVOICEPARA_STATUS_YES;

                rs = baseBO.Query(invoicePara);
                if (rs.Count != 0)
                {
                    if (baseBO.ExecuteUpdate("Update InvoicePara Set IsDefault = " + InvoicePara.INVOICEPARA_STATUS_NO) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        ClearGridViewSelected();
                        return;
                    }
                }
            }
            else
            {
                baseBO.WhereClause = "IsDefault = " + InvoicePara.INVOICEPARA_STATUS_YES + " And ParaStatus =" + InvoicePara.INVOICEPARA_STATUS_YES;
                rsinvPara = baseBO.Query(invoicePara);
                if (rsinvPara.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_OnlyDefault") + "'", true);
                    ClearGridViewSelected();
                    return;
                }
            }

            baseBO.WhereClause = "InvoiceParaID = " + Convert.ToInt32(ViewState["InvoiceParaID"]);

            if (baseBO.Update(invoicePara) == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
        }
        btnSave.Enabled = true;
        ClearText();
        BindGV();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
        ClearText();
        BindGV();
    }
    private void ClearText()
    {
        txtInvHeader.Text = "";
        txtInvSubhead.Text = "";
        txtInvH1.Text = "";
        txtInvH2.Text = "";
        txtInvH3.Text = "";
        txtInvH4.Text = "";
        txtInvH5.Text = "";
        txtInvF1.Text = "";
        txtInvF2.Text = "";
        txtInvF3.Text = "";
        txtInvF4.Text = "";
        txtInvF5.Text = "";
        txtInvF6.Text = "";
        txtInvF7.Text = "";
        txtInvoiceParaDesc.Text = "";
        cmbStatus.SelectedIndex = 0;
        btnEdit.Enabled = false;
        chkDefault.Checked = false;
        this.ddlSubs.SelectedIndex = 0;
    }
    protected void gvInvoicePara_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        InvoicePara invoicePara = new InvoicePara();

        ViewState["InvoiceParaID"] = gvInvoicePara.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "InvoiceParaID =" + Convert.ToInt32(ViewState["InvoiceParaID"]);

        rs = baseBO.Query(invoicePara);

        if (rs.Count == 1)
        {
            invoicePara = rs.Dequeue() as InvoicePara;

            txtInvHeader.Text = invoicePara.InvHeader;
            txtInvSubhead.Text = invoicePara.InvSubhead;
            txtInvH1.Text = invoicePara.InvH1;
            txtInvH2.Text = invoicePara.InvH2;
            txtInvH3.Text = invoicePara.InvH3;
            txtInvH4.Text = invoicePara.InvH4;
            txtInvH5.Text = invoicePara.InvH5;
            txtInvF1.Text = invoicePara.InvF1;
            txtInvF2.Text = invoicePara.InvF2;
            txtInvF3.Text = invoicePara.InvF3;
            txtInvF4.Text = invoicePara.InvF4;
            txtInvF5.Text = invoicePara.InvF5;
            txtInvF6.Text = invoicePara.InvF6;
            txtInvF7.Text = invoicePara.InvF7;
            txtInvoiceParaDesc.Text = invoicePara.InvoiceParaDesc;
            cmbStatus.SelectedValue = invoicePara.ParaStatus.ToString();
            this.ddlSubs.SelectedValue = invoicePara.SubsID.ToString();
            if (invoicePara.IsDefault == InvoicePara.INVOICEPARA_STATUS_YES)
            {
                chkDefault.Checked = true;
            }
            else
            {
                chkDefault.Checked = false;
            }

            btnEdit.Enabled = true;
            btnSave.Enabled = false;
        }
        ClearGridViewSelected();
    }

    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        sNewStr = sNewStr + "...";
        return sNewStr;
    }
    protected void gvInvoicePara_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text != "&nbsp;")
            {
                gIntro = e.Row.Cells[1].Text.ToString();
                e.Row.Cells[1].Text = SubStr(gIntro, 5);
            }
            if (e.Row.Cells[2].Text != "&nbsp;")
            {
                e.Row.Cells[2].Text = SubStr(e.Row.Cells[2].Text.ToString(), 4);
            }
        }
    }
    protected void gvInvoicePara_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (-2 == e.NewPageIndex)
        {
            TextBox txtNewPageIndex = null;
            GridViewRow pagerRow = theGrid.BottomPagerRow;
            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (null != txtNewPageIndex)
            {
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        { newPageIndex = e.NewPageIndex; }
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        BindGV();
    }

}
