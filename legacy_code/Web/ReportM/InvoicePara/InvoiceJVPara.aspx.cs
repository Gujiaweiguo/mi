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

public partial class ReportM_InvoicePara_InvoiceJVPara : BasePage
{
    public string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGV();

            int[] status = InvoiceJVPara.GetInvoiceParaStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", InvoiceJVPara.GetInvoiceJVParaStatusDesc(status[i])), status[i].ToString()));
            }

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "InvoicePara_TitleUnion");
            btnEdit.Enabled = false;
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
        DataTable dt = baseBo.QueryDataSet(new InvoiceJVPara()).Tables[0];

        int count = dt.Rows.Count;

        pds.DataSource = dt.DefaultView;


        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["ParaStatusName"] = (String)GetGlobalResourceObject("Parameter", InvoiceJVPara.GetInvoiceJVParaStatusDesc(Convert.ToInt32(dt.Rows[j]["ParaStatus"])));
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
                gvr.Cells[3].Text = "";
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
            InvoiceJVPara invoiceJVPara = new InvoiceJVPara();
            BaseBO baseBO = new BaseBO();
            BaseTrans baseTrans = new BaseTrans();
            Resultset rs = new Resultset();
            Resultset rsinvPara = new Resultset();

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            invoiceJVPara.InvoiceJVParaID = BaseApp.GetInvoiceJVParaID();
            invoiceJVPara.InvHeader = txtInvHeader.Text.Trim();
            invoiceJVPara.InvSubhead = txtInvSubhead.Text.Trim();
            invoiceJVPara.InvH1 = txtInvH1.Text.Trim();
            invoiceJVPara.InvH2 = txtInvH2.Text.Trim();
            invoiceJVPara.InvH3 = txtInvH3.Text.Trim();
            invoiceJVPara.InvH4 = txtInvH4.Text.Trim();
            invoiceJVPara.InvH5 = txtInvH5.Text.Trim();
            invoiceJVPara.InvF1 = txtInvF1.Text.Trim();
            invoiceJVPara.InvF2 = txtInvF2.Text.Trim();
            invoiceJVPara.InvF3 = txtInvF3.Text.Trim();
            invoiceJVPara.InvF4 = txtInvF4.Text.Trim();
            invoiceJVPara.InvF5 = txtInvF5.Text.Trim();
            invoiceJVPara.InvF6 = txtInvF6.Text.Trim();
            invoiceJVPara.InvF7 = txtInvF7.Text.Trim();
            invoiceJVPara.InvoiceParaDesc = txtInvoiceParaDesc.Text.Trim();
            invoiceJVPara.ParaStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            invoiceJVPara.OprDeptID = sessionUser.DeptID;
            invoiceJVPara.OprRoleID = sessionUser.RoleID;
            invoiceJVPara.CreateUserID = sessionUser.UserID;

            baseTrans.BeginTrans();

            if (chkDefault.Checked)
            {
                invoiceJVPara.IsDefault = InvoiceJVPara.INVOICEJVPARA_STATUS_YES;

                rs = baseBO.Query(invoiceJVPara);
                if (rs.Count != 0)
                {
                    if (baseBO.ExecuteUpdate("Update InvoiceJVPara Set IsDefault = " + InvoiceJVPara.INVOICEJVPARA_STATUS_NO) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        ClearGridViewSelected();
                        return;
                    }
                }
            }
            else
            {
                baseBO.WhereClause = "IsDefault = " + InvoiceJVPara.INVOICEJVPARA_STATUS_YES + " And ParaStatus =" + InvoiceJVPara.INVOICEJVPARA_STATUS_YES;
                rsinvPara = baseBO.Query(invoiceJVPara);
                if (rsinvPara.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_OnlyDefault") + "'", true);
                    ClearGridViewSelected();
                    return;
                }
            }

            if (baseBO.Insert(invoiceJVPara) == 1)
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
            InvoiceJVPara invoiceJVPara = new InvoiceJVPara();
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            Resultset rsinvPara = new Resultset();

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

            invoiceJVPara.InvHeader = txtInvHeader.Text.Trim();
            invoiceJVPara.InvSubhead = txtInvSubhead.Text.Trim();
            invoiceJVPara.InvH1 = txtInvH1.Text.Trim();
            invoiceJVPara.InvH2 = txtInvH2.Text.Trim();
            invoiceJVPara.InvH3 = txtInvH3.Text.Trim();
            invoiceJVPara.InvH4 = txtInvH4.Text.Trim();
            invoiceJVPara.InvH5 = txtInvH5.Text.Trim();
            invoiceJVPara.InvF1 = txtInvF1.Text.Trim();
            invoiceJVPara.InvF2 = txtInvF2.Text.Trim();
            invoiceJVPara.InvF3 = txtInvF3.Text.Trim();
            invoiceJVPara.InvF4 = txtInvF4.Text.Trim();
            invoiceJVPara.InvF5 = txtInvF5.Text.Trim();
            invoiceJVPara.InvF6 = txtInvF6.Text.Trim();
            invoiceJVPara.InvF7 = txtInvF7.Text.Trim();
            invoiceJVPara.InvoiceParaDesc = txtInvoiceParaDesc.Text.Trim();
            invoiceJVPara.ParaStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            invoiceJVPara.ModifyUserID = sessionUser.UserID;

            if (chkDefault.Checked)
            {
                invoiceJVPara.IsDefault = InvoiceJVPara.INVOICEJVPARA_STATUS_YES;

                rs = baseBO.Query(invoiceJVPara);
                if (rs.Count != 0)
                {
                    if (baseBO.ExecuteUpdate("Update InvoiceJVPara Set IsDefault = " + InvoiceJVPara.INVOICEJVPARA_STATUS_NO) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        ClearGridViewSelected();
                        return;
                    }
                }
            }
            else
            {
                baseBO.WhereClause = "IsDefault = " + InvoiceJVPara.INVOICEJVPARA_STATUS_YES + " And ParaStatus =" + InvoiceJVPara.INVOICEJVPARA_STATUS_YES;
                rsinvPara = baseBO.Query(invoiceJVPara);
                if (rsinvPara.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_OnlyDefault") + "'", true);
                    ClearGridViewSelected();
                    return;
                }
            }

            baseBO.WhereClause = "InvoiceJVParaID = " + Convert.ToInt32(ViewState["InvoiceParaID"]);

            if (baseBO.Update(invoiceJVPara) == 1)
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
    }
    protected void gvInvoicePara_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();

        InvoiceJVPara invoiceJVPara = new InvoiceJVPara();

        ViewState["InvoiceParaID"] = gvInvoicePara.SelectedRow.Cells[0].Text;

        baseBO.WhereClause = "InvoiceJVParaID =" + Convert.ToInt32(ViewState["InvoiceParaID"]);

        rs = baseBO.Query(invoiceJVPara);

        if (rs.Count == 1)
        {
            invoiceJVPara = rs.Dequeue() as InvoiceJVPara;

            txtInvHeader.Text = invoiceJVPara.InvHeader;
            txtInvSubhead.Text = invoiceJVPara.InvSubhead;
            txtInvH1.Text = invoiceJVPara.InvH1;
            txtInvH2.Text = invoiceJVPara.InvH2;
            txtInvH3.Text = invoiceJVPara.InvH3;
            txtInvH4.Text = invoiceJVPara.InvH4;
            txtInvH5.Text = invoiceJVPara.InvH5;
            txtInvF1.Text = invoiceJVPara.InvF1;
            txtInvF2.Text = invoiceJVPara.InvF2;
            txtInvF3.Text = invoiceJVPara.InvF3;
            txtInvF4.Text = invoiceJVPara.InvF4;
            txtInvF5.Text = invoiceJVPara.InvF5;
            txtInvF6.Text = invoiceJVPara.InvF6;
            txtInvF7.Text = invoiceJVPara.InvF7;
            txtInvoiceParaDesc.Text = invoiceJVPara.InvoiceParaDesc;
            cmbStatus.SelectedValue = invoiceJVPara.ParaStatus.ToString();
            if (invoiceJVPara.IsDefault == InvoiceJVPara.INVOICEJVPARA_STATUS_YES)
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
                e.Row.Cells[1].Text = SubStr(gIntro, 4);
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
