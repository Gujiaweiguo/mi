using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base;
using Lease.PotCustLicense;
using Base.Page;
using Base.DB;
using System.Drawing;

public partial class Lease_PotCustomer_PotCustLicenseUpdate : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            int[] status2 = PotCustLicenseInfo.GetPotCustLicenseType();
            for (int i = 0; i < status2.Length; i++)
            {
                cmbLicenseType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",PotCustLicenseInfo.GetPotCustLicenseTypeDesc(status2[i])), status2[i].ToString()));
            }
            int tempCustID = 0;
            if (Request.Cookies["Info1"] != null)
            {
                tempCustID = Convert.ToInt32(Request.Cookies["Info1"].Values["custid"]);
                BindGridViewPotCustLicense("CustID=" + tempCustID);
            }
            else
            {
                BindGridViewPotCustLicense("CustID=0");
            }
            ViewState["custID"] = tempCustID;
            this.txtLicenseName.Attributes.Add("onblur", "TextIsNotNull(txtLicenseName,ImgCustName)");
            txtLicenseBeginDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtLicenseEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.btnAdd.Attributes.Add("onclick", "return CheckNull()");
            this.btnEdit.Attributes.Add("onclick", "return CheckNull()");
        }
    }
    protected void GrdCustLicense_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            string gIntro = "";
            if (e.Row.Cells[1].Text.Trim() != "&nbsp;")
            {
                gIntro = e.Row.Cells[1].Text.ToString();
                e.Row.Cells[1].Text = SubStr(gIntro, 5);
                gIntro = e.Row.Cells[2].Text.ToString();
                //e.Row.Cells[2].Text = SubStr(gIntro, 10);
                //gIntro = e.Row.Cells[3].Text.ToString();
                //e.Row.Cells[3].Text = SubStr(gIntro, 10);
                if(this.CheckDate(e.Row.Cells[2].Text.Trim()))
                    e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToShortDateString();
                if (this.CheckDate(e.Row.Cells[3].Text.Trim()))
                    e.Row.Cells[3].Text = DateTime.Parse(e.Row.Cells[3].Text).ToShortDateString();
            }
            else
            {
                e.Row.Cells[4].Text = "";
            }
        }
    }
    private bool CheckDate(string strDate)
    {
        try
        {
            DateTime dt = DateTime.Parse(strDate);
            return true;
        }
        catch { return false; }
    }
    protected void GrdCustLicense_SelectedIndexChanged(object sender, EventArgs e)
    {
        //证照
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "CustLicenseID=" + GrdCustLicense.SelectedRow.Cells[0].Text;
        ViewState["custLicenseID"] = GrdCustLicense.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(new PotCustLicenseInfo());

        PotCustLicenseInfo potCustLicenseInfo = rs.Dequeue() as PotCustLicenseInfo;

        txtLicenseID.Text = potCustLicenseInfo.CustLicenseCode;
        txtLicenseName.Text = potCustLicenseInfo.CustLicenseName;
        txtLicenseBeginDate.Text = potCustLicenseInfo.CustLicenseStartDate.ToString("yyyy-MM-dd");
        txtLicenseEndDate.Text = potCustLicenseInfo.CustLicenseEndDate.ToString("yyyy-MM-dd");
        cmbLicenseType.SelectedValue = potCustLicenseInfo.CustLicenseType.ToString();
        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);

        ViewState["UploadLince"] = potCustLicenseInfo.UploadLicense;
        if (potCustLicenseInfo.UploadLicense != ""&& potCustLicenseInfo.UploadLicense!=null)
            this.btnLook.Enabled = true;
        else
            this.btnLook.Enabled = false;
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());

        btnEdit.Enabled = true;
        btnAdd.Enabled = false;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //添加
        if (ViewState["custID"].ToString() != "")
        {
            PotCustLicenseInfo potCustLicense = new PotCustLicenseInfo();
            potCustLicense.CustLicenseID = BaseApp.GetCustLicenseID();
            potCustLicense.CustLicenseCode = txtLicenseID.Text;
            potCustLicense.CustLicenseName = txtLicenseName.Text;
            potCustLicense.CustLicenseType = Convert.ToInt32(cmbLicenseType.SelectedValue);
            try { potCustLicense.CustLicenseStartDate = Convert.ToDateTime(txtLicenseBeginDate.Text); }
            catch { potCustLicense.CustLicenseStartDate = DateTime.Now.Date; }
            try { potCustLicense.CustLicenseEndDate = Convert.ToDateTime(txtLicenseEndDate.Text); }
            catch { potCustLicense.CustLicenseEndDate = DateTime.Now.Date; }
            potCustLicense.CustID = Convert.ToInt32(ViewState["custID"].ToString());

            potCustLicense.UploadLicense = this.GetUploadFileName();//获得上传文件的名称
            if (potCustLicense.UploadLicense != "")
            {
                string vsUrl = Server.MapPath("UploadLicense/") + potCustLicense.UploadLicense;
                FileUpload1.SaveAs(vsUrl);//保存文件
            }

            BaseBO baseBo = new BaseBO();

            int result = baseBo.Insert(potCustLicense);
            if (result != -1)
            {
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
                Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "')</script>");
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "')</script>");
                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "chooseCard(2)", true);
                return;
            }
        }
        else
        {
            Response.Write("<script>alert('" + hidWrite.Value + "')</script>");
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidWrite.Value + "'", true);
        }

        //btnAdd.Enabled = false;
        //btnEdit.Enabled = true;
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        CancelCustLicenseText();
    }
    /// <summary>
    /// 获得上传文件的名称
    /// </summary>
    /// <returns></returns>
    private string GetUploadFileName()
    {
        if (this.FileUpload1.HasFile)
        {
            string strName = this.FileUpload1.FileName;
            int index = this.FileUpload1.FileName.LastIndexOf(".");
            string vstype = this.FileUpload1.FileName.Substring(index).ToLower();//取文件的扩展名（文件类型）
            string strFileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + vstype;
            return strFileName;
        }
        else
            return "";
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //更新
        PotCustLicenseInfo potCustLicense = new PotCustLicenseInfo();
        potCustLicense.CustLicenseCode = txtLicenseID.Text;
        potCustLicense.CustLicenseName = txtLicenseName.Text;
        potCustLicense.CustLicenseType = Convert.ToInt32(cmbLicenseType.SelectedValue);

        try { potCustLicense.CustLicenseStartDate = Convert.ToDateTime(txtLicenseBeginDate.Text); }
        catch { potCustLicense.CustLicenseStartDate = DateTime.Now.Date; }
        try { potCustLicense.CustLicenseEndDate = Convert.ToDateTime(txtLicenseEndDate.Text); }
        catch { potCustLicense.CustLicenseEndDate = DateTime.Now.Date; }

        potCustLicense.UploadLicense = this.GetUploadFileName();//获得上传文件的名称
        if (potCustLicense.UploadLicense != "")
        {
            string vsUrl = Server.MapPath("UploadLicense/") + potCustLicense.UploadLicense;
            FileUpload1.SaveAs(vsUrl);//保存文件
        }

        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "CustLicenseID=" + ViewState["custLicenseID"].ToString();
        int result = baseBo.Update(potCustLicense);
        if (result != -1)
        {
            if (potCustLicense.UploadLicense != "")
                baseBo.ExecuteUpdate("update PotCustLicense set UploadLicense='" + potCustLicense.UploadLicense + "' where CustLicenseID='" + ViewState["custLicenseID"].ToString() + "'");

            Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "')</script>");

            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        }
        else
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "')</script>");
            return;
        }
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
        btnEdit.Enabled = false;
        btnAdd.Enabled = true;
        CancelCustLicenseText();
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/PotCustomer/PotCustLicenseUpdate.aspx");
    }
    public void BindGridViewPotCustLicense(string wherestr)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = wherestr;
        BaseInfo.BaseCommon.BindGridView(baseBO, new PotCustLicenseInfo(), this.GrdCustLicense);
        #region
        //DataTable dt = baseBO.QueryDataSet(new PotCustLicenseInfo()).Tables[0];
        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < GrdCustLicense.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdCustLicense.DataSource = pds;
        //    GrdCustLicense.DataBind();
        //}
        //else
        //{
        //    GrdCustLicense.EmptyDataText = "";
        //    pds.AllowPaging = true;
        //    pds.PageSize = 11;
        //    lblLTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
        //    pds.CurrentPageIndex = int.Parse(lblLCurrent.Text) - 1;
        //    if (pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = true;
        //    }

        //    if (pds.IsLastPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = false;
        //    }

        //    if (pds.IsFirstPage && pds.IsLastPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = false;
        //    }

        //    if (!pds.IsLastPage && !pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = true;
        //    }
        //    this.GrdCustLicense.DataSource = pds;
        //    this.GrdCustLicense.DataBind();
        //    spareRow = GrdCustLicense.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdCustLicense.DataSource = pds;
        //    GrdCustLicense.DataBind();
        //}
        #endregion
    }

    public void CancelCustLicenseText()
    {
        txtLicenseID.Text = "";
        txtLicenseName.Text = "";
        txtLicenseBeginDate.Text = "";
        txtLicenseEndDate.Text = "";
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
    protected void GrdCustLicense_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        foreach (GridViewRow grv in this.GrdCustLicense.Rows)
        {
            grv.BackColor = Color.White;
        }
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
    }
    protected void btnLook_Click(object sender, EventArgs e)
    {
        if (ViewState["UploadLince"]!=null||ViewState["UploadLince"].ToString() != "")
        {
            string strPath = Server.MapPath("UploadLicense\\") + ViewState["UploadLince"].ToString();
            try
            {
                System.Diagnostics.Process.Start(strPath);
            }
            catch { }
        }
        BindGridViewPotCustLicense("CustID=" + ViewState["custID"].ToString());
    }
}
