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
using Base;
using Lease.PotBargain;
using Base.Page;
using BaseInfo.User;

public partial class Lease_ChargeType_ChargeType : BasePage
{
    public string baseInfo;
    public string emptyStr;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            //绑定费用分类
            int[] chargeClass = ChargeType.GetChargeClass();
            for (int i = 0; i < chargeClass.Length; i++)
            {
                this.DownListChargeClass.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ChargeType.GetChargeClassDesc(chargeClass[i])), chargeClass[i].ToString()));
            }

            //绑定收费时间交叉标志
            int[] isChargeCross = ChargeType.GetIsChargeCross();
            for (int i = 0; i < isChargeCross.Length; i++)
            {
                this.DownListIsChargeCross.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", ChargeType.GetIsChargeCrossDesc(isChargeCross[i])), isChargeCross[i].ToString()));
            }

            BindGV();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "ChargeType_lblChargeTypeDefine");
            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            btnSave.Attributes.Add("onclick", "return FormulaValidator()");
            this.btnEdit.Attributes.Add("onclick", "return FormulaValidator()");
            btnEdit.Enabled = false;
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Publicmessage", "Load();", true);
        }
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();
        //DataSet ds = baseBo.QueryDataSet(new ChargeType());
        //DataTable dt = ds.Tables[0];




        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;


        DataTable dt = baseBo.QueryDataSet(new ChargeType()).Tables[0];

        int count = dt.Rows.Count;
        dt.Columns.Add("ChargeClassName");
        dt.Columns.Add("IsChargeCrossName");
        //获取费用分类
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["ChargeClassName"] = (String)GetGlobalResourceObject("Parameter", ChargeType.GetChargeClassDesc(Convert.ToInt32(dt.Rows[j]["ChargeClass"])));
        }
        //获取收费时间交叉标志
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["IsChargeCrossName"] = (String)GetGlobalResourceObject("Parameter", ChargeType.GetIsChargeCrossDesc(Convert.ToInt32(dt.Rows[j]["IsChargeCross"])));
        }

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < gvChargeType.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvChargeType.DataSource = pds;
            gvChargeType.DataBind();
        }
        else
        {
            //gvChargeType.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 11;
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

            this.gvChargeType.DataSource = pds;
            this.gvChargeType.DataBind();
            spareRow = gvChargeType.Rows.Count;
            for (int i = 0; i < gvChargeType.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            gvChargeType.DataSource = pds;
            gvChargeType.DataBind();
        }

        //int countNull = 7-count;
        //for (int i = 0; i < countNull; i++)
        //{
        //    dt.Rows.Add(dt.NewRow());
        //}
        //gvChargeType.DataSource = dt;
        //gvChargeType.DataBind();
        //int gvCount = gvChargeType.Rows.Count;
        //for (int j = count; j < gvCount; j++)
        //    gvChargeType.Rows[j].Cells[6].Text = "";
        //this.btnEdit.Enabled = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ChargeType chargeType = new ChargeType();
        BaseBO baseBO = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBO.QueryDataSet("select ChargeTypeCode from ChargeType where ChargeTypeCode='" + txtChargeTypeCode.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "select", "document.all.txtChargeTypeName.select()", true);
            this.BindGV();
        }
        else
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            chargeType.CreateUserId = sessionUser.UserID;
            chargeType.ChargeTypeID = BaseApp.GetChargeTypeID();
            chargeType.ChargeTypeCode = txtChargeTypeCode.Text;
            chargeType.ChargeTypeName = txtChargeTypeName.Text;
            chargeType.ChargeClass = Convert.ToInt32(DownListChargeClass.SelectedValue);
            chargeType.IsChargeCross = Convert.ToInt32(DownListIsChargeCross.SelectedValue);
            chargeType.Note = txtNote.Text;
            chargeType.AccountNumber = txtFinal.Text;
            txtChargeTypeCode.Text = "";
            txtChargeTypeName.Text = "";
            txtFinal.Text = "";
            ViewState["chargeTypeID"] = "";
            if (baseBO.Insert(chargeType) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '添加失败。'", true);
            }
            BindGV();
        }      
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["chargeTypeID"]!=null&&ViewState["chargeTypeID"].ToString() != "")
        {
            ChargeType chargeType = new ChargeType();
            BaseBO baseBO = new BaseBO();
            DataSet ds = new DataSet();
            ds = baseBO.QueryDataSet("select ChargeTypeCode from ChargeType where ChargeTypeCode='" + txtChargeTypeCode.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                chargeType.ModifyUserId = sessionUser.UserID;
                chargeType.ChargeTypeCode = txtChargeTypeCode.Text;
                chargeType.ChargeTypeName = txtChargeTypeName.Text;
                chargeType.ChargeClass = Convert.ToInt32(DownListChargeClass.SelectedValue);
                chargeType.IsChargeCross = Convert.ToInt32(DownListIsChargeCross.SelectedValue);
                chargeType.Note = txtNote.Text;
                chargeType.AccountNumber = txtFinal.Text;

                baseBO.WhereClause = "";
                baseBO.WhereClause = "ChargeTypeID = " + Convert.ToInt32(ViewState["chargeTypeID"]);
                if (baseBO.Update(chargeType) != -1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                ViewState["chargeTypeID"] = "";
                txtChargeTypeCode.Text = "";
                txtChargeTypeName.Text = "";
                this.btnSave.Enabled = true;
                btnEdit.Enabled = false;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "select", "document.all.txtChargeTypeName.select()", true);
            }
        }
        BindGV();
        
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        txtChargeTypeCode.Text = "";
        txtChargeTypeName.Text = "";
        txtFinal.Text = "";
        ViewState["chargeTypeID"] = "";
        this.btnSave.Enabled = true;
        btnEdit.Enabled = false;
        BindGV();
    }
    protected void gvChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        int chargeID = 0;
        chargeID = Convert.ToInt32(gvChargeType.SelectedRow.Cells[0].Text);
        ViewState["chargeTypeID"] = chargeID;
        baseBO.WhereClause = "ChargeTypeID=" + chargeID;

        Resultset rs = baseBO.Query(new ChargeType());
        if (rs.Count == 1)
        {
            ChargeType chargeType = rs.Dequeue() as ChargeType;
            txtChargeTypeCode.Text = chargeType.ChargeTypeCode;
            txtChargeTypeName.Text = chargeType.ChargeTypeName;
            DownListChargeClass.SelectedValue = chargeType.ChargeClass.ToString();
            DownListIsChargeCross.SelectedValue = chargeType.IsChargeCross.ToString();
            txtFinal.Text = chargeType.AccountNumber;
        }
        BindGV();
        Session["editLog"] = txtChargeTypeCode.Text;
        this.btnSave.Enabled = false;
        this.btnEdit.Enabled = true;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        BindGV();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        BindGV();
    }
    protected void gvChargeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[6].Text = "";
            }
        }
    }
    protected void gvChargeType_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindGV();
    }
}
