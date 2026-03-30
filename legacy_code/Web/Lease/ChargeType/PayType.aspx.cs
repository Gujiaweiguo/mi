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

using Base;
using Base.Biz;
using Base.DB;
using Base.Page;
using Lease.PotBargain;
using Lease.PotCust;
using BaseInfo.authUser;
using BaseInfo.User;

public partial class Lease_ChargeType_PayType : BasePage
{
    public string baseInfo;
    DataTable dt = new DataTable();
    PagedDataSource pds = new PagedDataSource();
    BaseBO basebo = new BaseBO();
    private int numCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDdl();
            ViewState["currentCount"] = 1;
            page();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.btnEdit.Enabled = false;
        }
        btnSave.Attributes.Add("onclick", "return CheckData()");
        btnEdit.Attributes.Add("onclick", "return CheckData()");
    }

    private void BindDdl()
    {
        int[] status = PayType.GetIsPayType();
        ddlPayTypeStatus.Items.Clear();
        for (int i = 0; i < status.Length; i++)
        {
            ddlPayTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", PayType.GetIsPayTypeDesc(status[i])), status[i].ToString()));
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        PayType payType = new PayType();
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "PayTypeCode='" + txtPayTypeCode.Text.Trim() + "'";
        Resultset rs = baseBO.Query(payType);
        if (rs.Count < 1)
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            string sql = "select max(PayTypeID) from PayType";
            BaseBO bo = new BaseBO();
            DataSet ds = bo.QueryDataSet(sql);
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                payType.PayTypeID = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
            }
            else
            {
                payType.PayTypeID = 1;
            }
            payType.PayTypeCode = txtPayTypeCode.Text.Trim();
            payType.PayTypeName = txtPayTypeName.Text.Trim();
            payType.PayTypeStatus = Convert.ToInt32(ddlPayTypeStatus.SelectedValue);
            payType.Node = txtNote.Text.Trim();

            if (baseBO.Insert(payType) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('添加成功')", true);
                //Response.Redirect("CustType.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '添加失败。'", true);
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('添加失败')", true);
            }
            page();
            ViewState["PayTypeID"] = "";
            txtPayTypeCode.Text = "";
            txtPayTypeName.Text = "";
            txtNote.Text = "";

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "document.all.txtPayTypeCode.select()", true);
            this.page();
        }

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["PayTypeID"] == null || ViewState["PayTypeID"].ToString() == "")
        {
            this.page();
            return;
        }
        PayType payType = new PayType();
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBO.QueryDataSet("select PayTypeCode from PayType where PayTypeCode='" + txtPayTypeCode.Text.Trim() + "'");
        if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
        {
            baseBO.WhereClause = "PayTypeID=" + Convert.ToInt32(ViewState["PayTypeID"]);
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            payType.PayTypeCode = txtPayTypeCode.Text.Trim();
            payType.PayTypeName = txtPayTypeName.Text.Trim();
            payType.PayTypeStatus = Convert.ToInt32(ddlPayTypeStatus.SelectedValue);
            payType.Node = txtNote.Text.Trim();

            if (baseBO.Update(payType) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            this.btnEdit.Enabled = false;
            this.btnSave.Enabled = true;
            ViewState["PayTypeID"] = "";
            txtPayTypeCode.Text = "";
            txtPayTypeName.Text = "";
            txtNote.Text = "";
            page();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "document.all.txtPayTypeCode.select()", true);
            page();
        }

    }

    protected void GrdVewCustType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        int payTypeID = 0;
        payTypeID = Convert.ToInt32(GrdVewCustType.SelectedRow.Cells[0].Text);
        ViewState["PayTypeID"] = payTypeID;
        baseBO.WhereClause = "PayTypeID=" + payTypeID;

        rs = baseBO.Query(new PayType());
        if (rs.Count == 1)
        {

            PayType paytype = rs.Dequeue() as PayType;
            txtPayTypeCode.Text = paytype.PayTypeCode;
            txtPayTypeName.Text = paytype.PayTypeName;
            ddlPayTypeStatus.SelectedValue = paytype.PayTypeStatus.ToString();
            txtNote.Text = paytype.Node;

        }
        Session["editLog"] = txtPayTypeCode.Text;
        this.btnEdit.Enabled = true;
        this.btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        page();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ViewState["PayTypeID"] = "";
        txtPayTypeCode.Text = "";
        txtPayTypeName.Text = "";
        txtNote.Text = "";
        this.btnSave.Enabled = true;
        this.btnEdit.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        page();
    }

    protected void page()
    {
        DataSet ds = basebo.QueryDataSet(new PayType());
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        int ss = 0;

        pds.DataSource = dt.DefaultView;
        if (pds.Count < 1)
        {
            for (int i = 0; i < 9; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.PageSize = 9;
            pds.DataSource = dt.DefaultView;
        }
        else
        {
            GrdVewCustType.DataSource = pds;
            GrdVewCustType.DataBind();

            ss = GrdVewCustType.Rows.Count;
            for (int i = 0; i < GrdVewCustType.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
        }
        GrdVewCustType.DataSource = pds;
        GrdVewCustType.DataBind();
    }

    protected void GrdVewCustType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text != "&nbsp;")
            {

                if (e.Row.Cells[4].Text.Length == 1)
                {
                    if (e.Row.Cells[4].Text.Equals(CustType.CUST_TYPE_STATUS_INVALID.ToString()))
                    {
                        e.Row.Cells[4].Text = (String)GetGlobalResourceObject("Parameter", "WrkFlw_Disabled");
                    }
                    else if (e.Row.Cells[4].Text.Equals(CustType.CUST_TYPE_STATUS_VALID.ToString()))
                    {
                        e.Row.Cells[4].Text = (String)GetGlobalResourceObject("Parameter", "WrkFlw_Enabled");
                    }
                }
            }
            else
            {
                e.Row.Cells[5].Text = "";
            }
        }
    }
    protected void GrdVewCustType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.page();
    }
}
