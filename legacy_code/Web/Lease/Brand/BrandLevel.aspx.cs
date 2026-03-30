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
using BaseInfo.Dept;
using BaseInfo.Store;
using Base.Biz;
using Base.DB;
using Lease.PotCust;
using Lease.CustLicense;
using Lease.Brand;
using Base;
using Base.Page;
using BaseInfo.User;
using System.Drawing;

/// <summary>
/// Add By TJM at 2009.03.17
/// </summary>

public partial class Lease_Brand_BrandLevel : BasePage
{


  public   string baseInfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        SetControlPro();
        if (!this.IsPostBack)
        {
            txtBrandLevelCode.Attributes.Add("onkeydown", "textleave()");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            BindData();
            ClearControlValue();
        }
        btnSave.Attributes.Add("onclick","return CheckData()");
    }

    private void SetControlPro()
    {
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");

    }


    private void BindData()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        DataTable dt = baseBO.QueryDataSet(new BrandLevel()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdBrandLevel.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdBrandLevel.DataSource = pds;
            GrdBrandLevel.DataBind();
        }
        else
        {
            //pds.AllowPaging = true;
            //pds.PageSize = 10;
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
            //this.GrdBrandLevel.DataSource = pds;
            //this.GrdBrandLevel.DataBind();
            //spareRow = GrdBrandLevel.Rows.Count;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //GrdBrandLevel.DataSource = pds;
            //GrdBrandLevel.DataBind();
            GrdBrandLevel.DataSource = pds;
            GrdBrandLevel.DataBind();
            spareRow = GrdBrandLevel.Rows.Count;
            for (int i = 0; i < GrdBrandLevel.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            GrdBrandLevel.DataSource = pds;
            GrdBrandLevel.DataBind();

        }
        ClearGridViewSelect();
    }

    protected void GrdBrandLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        BrandLevel brandLevel = new BrandLevel();
        ViewState["BrandLevelID"] = this.GrdBrandLevel.SelectedRow.Cells[0].Text.Trim();
        baseBo.WhereClause = "BrandLevelID=" + this.GrdBrandLevel.SelectedRow.Cells[0].Text.Trim();
        Resultset rs = baseBo.Query(new BrandLevel());
        if (rs.Count !=0)
        {
            brandLevel = rs.Dequeue() as BrandLevel;
            txtBrandLevelCode.Text = brandLevel.BrandLevelCode.ToString();
            txtBrandLevelName.Text = brandLevel.BrandLevelName.ToString();
            txtNode.Text = brandLevel.Note.ToString();
         }
         btnEdit.Enabled = true;
         btnSave.Enabled = false;
         Session["editLog"] = txtBrandLevelName.Text;
         ClearGridViewSelect();
         BindData();
         ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  \"\"", true);


       

    }
 

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (txtBrandLevelCode.Text.Trim() != "" && txtBrandLevelName.Text.Trim() != "")
        {
            BaseBO baseBo = new BaseBO();           
            BrandLevel brandLevel = new BrandLevel();
            DataSet ds = new DataSet();
            ds = baseBo.QueryDataSet("select BrandLevelName from BrandLevel where BrandLevelName='" + txtBrandLevelName.Text+ "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "BrandLevel_BrandLevelName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtBrandLevelName.select()", true);
                BindData();
            }
            else
            {
                brandLevel.BrandLevelID = BaseApp.GetID("BrandLevel", "BrandLevelID");
                brandLevel.CreateUserID = sessionUser.UserID;

                GetBrandLevel(brandLevel);

                int result = baseBo.Insert(brandLevel);
                if (result == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                    ClearControlValue();
                    btnEdit.Enabled = false;
                    BindData();

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                }
                BindData();
                foreach (GridViewRow grv in GrdBrandLevel.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }

        }
        
        ClearGridViewSelect();

    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (txtBrandLevelCode.Text.Trim() != "" && txtBrandLevelName.Text.Trim() != "")
        {
            if (ViewState["BrandLevelID"] != null)
            {
                BaseBO baseBo = new BaseBO();
                BrandLevel brandLevel = new BrandLevel();
                DataSet ds = new DataSet();
                ds = baseBo.QueryDataSet("select BrandLevelName from BrandLevel where BrandLevelName='" + txtBrandLevelName.Text + "'");
                if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
                {
                    brandLevel.BrandLevelID = Convert.ToInt32(ViewState["BrandLevelID"]);
                    baseBo.WhereClause = "BrandLevelID=" + ViewState["BrandLevelID"];
                    brandLevel.ModifyUserID = sessionUser.UserID;
                    GetBrandLevel(brandLevel);
                    int result = baseBo.Update(brandLevel);
                    if (result == 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                        ClearControlValue();
                        btnEdit.Enabled = false;
                        btnSave.Enabled = true;
                        BindData();
                    }
                    else
                    {
                        BindData();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    }
                    BindData();
                    foreach (GridViewRow grv in GrdBrandLevel.Rows)
                    {
                        grv.BackColor = Color.White;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "BrandLevel_BrandLevelName") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtBrandLevelName.select()", true);
                    BindData();
                }
            }
        }
        
        ClearGridViewSelect();



    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ClearControlValue();
        ClearGridViewSelect();
        btnSave.Enabled = true;
        ViewState["BrandLevelID"] = "";
        foreach (GridViewRow grv in GrdBrandLevel.Rows)
        {
            grv.BackColor = Color.White;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  \"\"", true);


    }
    private void GetBrandLevel(BrandLevel brandLevel)
    {
        brandLevel.BrandLevelCode = txtBrandLevelCode.Text.Trim();
        brandLevel.BrandLevelName = txtBrandLevelName.Text.Trim();
        brandLevel.Note = txtNode.Text.Trim();
    }

    private void ClearControlValue()
    {
        txtBrandLevelCode.Text = "";
        txtBrandLevelName.Text = "";
        txtNode.Text = "";
        btnEdit.Enabled = false;

    }

    private void ClearGridViewSelect()
    {
        foreach (GridViewRow gvr in GrdBrandLevel.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[3].Text = "";
            }
        }
    }
    protected void GrdBrandLevel_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindData();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        foreach (GridViewRow grv in GrdBrandLevel.Rows)
        {
            grv.BackColor = Color.White;
        }
    }

}
