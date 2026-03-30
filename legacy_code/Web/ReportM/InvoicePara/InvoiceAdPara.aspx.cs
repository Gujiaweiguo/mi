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


public partial class ReportM_InvoicePara_InvoiceAdPara : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGV();

            int[] status = InvoiceAdPara.GetInvoiceParaStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", InvoiceAdPara.GetInvoiceParaStatusDesc(status[i])), status[i].ToString()));
            }

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "InvoiceAdPara_Title");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "Load()", true);
        }
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        //baseBo.WhereClause = "a.ThemeID= b.ThemeID";
        DataTable dt = baseBo.QueryDataSet(new InvoiceAdPara()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;


        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["ParaStatusName"] = (String)GetGlobalResourceObject("Parameter", InvoiceAdPara.GetInvoiceParaStatusDesc(Convert.ToInt32(dt.Rows[j]["ParaStatus"])));
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
            gvInvoicePara.EmptyDataText = "";
            pds.AllowPaging = true;
            pds.PageSize = 15;
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

            this.gvInvoicePara.DataSource = pds;
            this.gvInvoicePara.DataBind();
            spareRow = gvInvoicePara.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtInvHeader.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            InvoiceAdPara invoiceAdPara = new InvoiceAdPara();
            BaseBO baseBO = new BaseBO();
            BaseTrans baseTrans = new BaseTrans();
            Resultset rs = new Resultset();
            Resultset rsinvPara = new Resultset();

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            invoiceAdPara.InvoiceAdParaID = BaseApp.GetInvoiceParaID();
            invoiceAdPara.InvHeader = txtInvHeader.Text.Trim();
            invoiceAdPara.InvSubhead = txtInvSubhead.Text.Trim();
            invoiceAdPara.InvH1 = txtInvH1.Text.Trim();
            invoiceAdPara.InvH2 = txtInvH2.Text.Trim();
            invoiceAdPara.InvH3 = txtInvH3.Text.Trim();
            invoiceAdPara.InvH4 = txtInvH4.Text.Trim();
            invoiceAdPara.InvH5 = txtInvH5.Text.Trim();
            invoiceAdPara.InvF1 = txtInvF1.Text.Trim();
            invoiceAdPara.InvF2 = txtInvF2.Text.Trim();
            invoiceAdPara.InvF3 = txtInvF3.Text.Trim();
            invoiceAdPara.InvF4 = txtInvF4.Text.Trim();
            invoiceAdPara.InvF5 = txtInvF5.Text.Trim();
            invoiceAdPara.InvF6 = txtInvF6.Text.Trim();
            invoiceAdPara.InvF7 = txtInvF7.Text.Trim();
            invoiceAdPara.InvoiceParaDesc = txtInvoiceParaDesc.Text.Trim();
            invoiceAdPara.ParaStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            invoiceAdPara.OprDeptID = sessionUser.DeptID;
            invoiceAdPara.OprRoleID = sessionUser.RoleID;
            invoiceAdPara.CreateUserID = sessionUser.UserID;

            baseTrans.BeginTrans();

            if (chkDefault.Checked)
            {
                invoiceAdPara.IsDefault = InvoiceAdPara.INVOICEPARA_STATUS_YES;

                rs = baseBO.Query(invoiceAdPara);
                if (rs.Count != 0)
                {
                    if (baseBO.ExecuteUpdate("Update InvoiceAdPara Set IsDefault = " + InvoiceAdPara.INVOICEPARA_STATUS_NO) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        ClearGridViewSelected();
                        return;
                    }
                }
            }
            else
            {
                baseBO.WhereClause = "IsDefault = " + InvoiceAdPara.INVOICEPARA_STATUS_YES + " And ParaStatus =" + InvoiceAdPara.INVOICEPARA_STATUS_YES;
                rsinvPara = baseBO.Query(invoiceAdPara);
                if (rsinvPara.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_OnlyDefault") + "'", true);
                    ClearGridViewSelected();
                    return;
                }
            }

            if (baseBO.Insert(invoiceAdPara) == 1)
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
        if (txtInvHeader.Text == "")
        {
            ClearGridViewSelected();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
            return;
        }
        else
        {
            InvoiceAdPara invoiceAdPara = new InvoiceAdPara();
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            Resultset rsinvPara = new Resultset();

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            invoiceAdPara.InvHeader = txtInvHeader.Text.Trim();
            invoiceAdPara.InvSubhead = txtInvSubhead.Text.Trim();
            invoiceAdPara.InvH1 = txtInvH1.Text.Trim();
            invoiceAdPara.InvH2 = txtInvH2.Text.Trim();
            invoiceAdPara.InvH3 = txtInvH3.Text.Trim();
            invoiceAdPara.InvH4 = txtInvH4.Text.Trim();
            invoiceAdPara.InvH5 = txtInvH5.Text.Trim();
            invoiceAdPara.InvF1 = txtInvF1.Text.Trim();
            invoiceAdPara.InvF2 = txtInvF2.Text.Trim();
            invoiceAdPara.InvF3 = txtInvF3.Text.Trim();
            invoiceAdPara.InvF4 = txtInvF4.Text.Trim();
            invoiceAdPara.InvF5 = txtInvF5.Text.Trim();
            invoiceAdPara.InvF6 = txtInvF6.Text.Trim();
            invoiceAdPara.InvF7 = txtInvF7.Text.Trim();
            invoiceAdPara.InvoiceParaDesc = txtInvoiceParaDesc.Text.Trim();
            invoiceAdPara.ParaStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            invoiceAdPara.ModifyUserID = sessionUser.UserID;

            if (chkDefault.Checked)
            {
                invoiceAdPara.IsDefault = InvoiceAdPara.INVOICEPARA_STATUS_YES;

                rs = baseBO.Query(invoiceAdPara);
                if (rs.Count != 0)
                {
                    if (baseBO.ExecuteUpdate("Update InvoiceAdPara Set IsDefault = " + InvoiceAdPara.INVOICEPARA_STATUS_NO) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        ClearGridViewSelected();
                        return;
                    }
                }
            }
            else
            {
                baseBO.WhereClause = "IsDefault = " + InvoiceAdPara.INVOICEPARA_STATUS_YES + " And ParaStatus =" + InvoiceAdPara.INVOICEPARA_STATUS_YES;
                rsinvPara = baseBO.Query(invoiceAdPara);
                if (rsinvPara.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_OnlyDefault") + "'", true);
                    ClearGridViewSelected();
                    return;
                }
            }

            baseBO.WhereClause = "InvoiceAdParaID = " + Convert.ToInt32(ViewState["InvoiceAdParaID"]);

            if (baseBO.Update(invoiceAdPara) == 1)
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
    protected void btnCel_Click(object sender, EventArgs e)
    {
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
    }
    protected void gvInvoicePara_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        InvoiceAdPara invoiceAdPara = new InvoiceAdPara();

        ViewState["InvoiceAdParaID"] = gvInvoicePara.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "InvoiceAdParaID =" + Convert.ToInt32(ViewState["InvoiceAdParaID"]);

        rs = baseBO.Query(invoiceAdPara);

        if (rs.Count == 1)
        {
            invoiceAdPara = rs.Dequeue() as InvoiceAdPara;

            txtInvHeader.Text = invoiceAdPara.InvHeader;
            txtInvSubhead.Text = invoiceAdPara.InvSubhead;
            txtInvH1.Text = invoiceAdPara.InvH1;
            txtInvH2.Text = invoiceAdPara.InvH2;
            txtInvH3.Text = invoiceAdPara.InvH3;
            txtInvH4.Text = invoiceAdPara.InvH4;
            txtInvH5.Text = invoiceAdPara.InvH5;
            txtInvF1.Text = invoiceAdPara.InvF1;
            txtInvF2.Text = invoiceAdPara.InvF2;
            txtInvF3.Text = invoiceAdPara.InvF3;
            txtInvF4.Text = invoiceAdPara.InvF4;
            txtInvF5.Text = invoiceAdPara.InvF5;
            txtInvF6.Text = invoiceAdPara.InvF6;
            txtInvF7.Text = invoiceAdPara.InvF7;
            txtInvoiceParaDesc.Text = invoiceAdPara.InvoiceParaDesc;
            cmbStatus.SelectedValue = invoiceAdPara.ParaStatus.ToString();
            if (invoiceAdPara.IsDefault == InvoiceAdPara.INVOICEPARA_STATUS_YES)
            {
                chkDefault.Checked = true;
            }
            else
            {
                chkDefault.Checked = false;
            }

            btnEdit.Enabled = true;
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
        if (e.Row.Cells[1].Text != "&nbsp;")
        {
            gIntro = e.Row.Cells[1].Text.ToString();
            e.Row.Cells[1].Text = SubStr(gIntro, 4);
        }
    }
}
