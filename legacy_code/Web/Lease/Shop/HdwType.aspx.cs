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
using Lease.ConShop;
using Lease.PotBargain;
using Base.Page;
using BaseInfo.User;

public partial class Lease_Shop_HdwType : BasePage
{
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDate();
            BindGV();
            btnEdit.Enabled = false;
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "LeaseHdw_lblHdwTypeModi");
        }
        this.btnSave.Attributes.Add("onclick","return CheckData()");
    }

    private void BindDate()
    {
        //绑定硬件类别
        dropHdwClass.Items.Clear();
        int[] hdwClass = HdwType.GetHdwClass();
        int s = hdwClass.Length;
        for (int i = 0; i < s; i++)
        {
            dropHdwClass.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", HdwType.GetHdwClassDesc(hdwClass[i])), hdwClass[i].ToString()));
        }

        //绑定硬件类型状态
        dropHdwTypeStatus.Items.Clear();
        int[] hdwTypeStatus = HdwType.GetHdwTypeStatus();
        int k = hdwTypeStatus.Length;
        for (int l = 0; l < k; l++)
        {
            dropHdwTypeStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", HdwType.GetHdwTypeStatusDesc(hdwTypeStatus[l])), hdwTypeStatus[l].ToString()));
        }
    }

    private void BindGV()
    {
        BaseBO baseBo = new BaseBO();

        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;


        DataTable dt = baseBo.QueryDataSet(new HdwType()).Tables[0];

        int count = dt.Rows.Count;

        dt.Columns.Add("HdwClassName");
        dt.Columns.Add("HdwTypeStatusName");
        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["HdwClassName"] = (String)GetGlobalResourceObject("Parameter", HdwType.GetHdwClassDesc(Convert.ToInt32(dt.Rows[j]["HdwClass"])));
            dt.Rows[j]["HdwTypeStatusName"] = (String)GetGlobalResourceObject("Parameter", HdwType.GetHdwTypeStatusDesc(Convert.ToInt32(dt.Rows[j]["HdwTypeStatus"])));
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
        //else
        //{
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
        //}

    }

    protected void gvChargeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[5].Text = "";
            }
        }
    }
    protected void gvChargeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtHdwTypeName.Text = gvChargeType.SelectedRow.Cells[1].Text;
        dropHdwClass.SelectedValue = gvChargeType.SelectedRow.Cells[6].Text;
        dropHdwTypeStatus.SelectedValue = gvChargeType.SelectedRow.Cells[7].Text;
        if (gvChargeType.SelectedRow.Cells[4].Text == "&nbsp;")
        {
            txtNote.Text = "";
        }
        else
        {
            txtNote.Text = gvChargeType.SelectedRow.Cells[4].Text;
        }
        ViewState["hdwTypeID"] = gvChargeType.SelectedRow.Cells[0].Text;
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        Session["editLog"] = txtHdwTypeName.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        BindGV();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        HdwType hdwType = new HdwType();
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select HdwTypeName from HdwType where HdwTypeName='" + txtHdwTypeName.Text + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "LeaseShopHdw_lblHdwTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "。'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtHdwTypeName.select()", true);
        }
        else
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            hdwType.HdwTypeID = BaseApp.GetHdwTypeID();
            hdwType.CreateUserId = sessionUser.UserID;
            GetHdwType(hdwType);
            int result = HdwTypePO.InsertHdwType(hdwType);
            if (result == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                ClearControlValue();

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            
        }
        BindGV();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["hdwTypeID"] != null)
        {
            HdwType hdwType = new HdwType();
            BaseBO baseBo = new BaseBO();
            DataSet ds = new DataSet();
            ds = baseBo.QueryDataSet("select HdwTypeName from HdwType where HdwTypeName='" + txtHdwTypeName.Text + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
            {
                hdwType.HdwTypeID = Convert.ToInt32(ViewState["hdwTypeID"]);
                SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                hdwType.ModifyUserId = sessionUser.UserID;
                GetHdwType(hdwType);
                int result = HdwTypePO.UpdateHdwTypeByID(hdwType);
                if (result == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                    ClearControlValue();
                    btnEdit.Enabled = false;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                btnSave.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "LeaseShopHdw_lblHdwTypeName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "。'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtHdwTypeName.select()", true);
            }
        }
                
        BindGV();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        ClearControlValue();
        btnSave.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        BindGV();

    }

    private void GetHdwType(HdwType hdwtype)
    {
        hdwtype.HdwTypeName = txtHdwTypeName.Text;
        hdwtype.HdwTypeStatus = Convert.ToInt32(dropHdwTypeStatus.SelectedValue);
        hdwtype.HdwClass = Convert.ToInt32(dropHdwClass.SelectedValue);
        hdwtype.Note = txtNote.Text;
    }

    private void ClearControlValue()
    {
        txtNote.Text = "";
        txtHdwTypeName.Text = "";
        BindDate();
        btnEdit.Enabled = false;
        BindGV();
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
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        this.BindGV();
    }
}
