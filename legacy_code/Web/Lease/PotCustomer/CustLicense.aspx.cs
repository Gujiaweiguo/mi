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

using Base.Page;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;

using Base.Biz;
using Base;
using Lease.PotCustLicense;
using BaseInfo.User;
using Base.DB;
using BaseInfo.Dept;
using Lease.PotCust;
using Base.Util;
using System.Drawing;

public partial class Lease_PotCustomer_CustLicense : BasePage
{
    private int numCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnAdd.Attributes.Add("onclick", "return LicenseBoxValidator(txtLicenseBeginDate,txtLicenseEndDate,'" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime") + "')");
            this.btnAdd.Attributes.Add("onclick", "return CheckIsNull()");
            txtLicenseID.Attributes.Add("onblur", "TextIsNotNull(txtLicenseID,ImgLicenseID)");
            txtLicenseName.Attributes.Add("onblur", "TextIsNotNull(txtLicenseName,ImgLicenseName)");
            /*证照类型*/
            int[] status2 = PotCustLicenseInfo.GetPotCustLicenseType();
            for (int i = 0; i < status2.Length; i++)
            {
                cmbLicenseType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",PotCustLicenseInfo.GetPotCustLicenseTypeDesc(status2[i])), status2[i].ToString()));
            }
            if (Request.Cookies["Custumer"].Values["CustumerID"] != "")
            {
                ViewState["CustumerID"] = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
                pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
            }
            else
            { 
                pageDisprove(Convert.ToInt32(0));
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Publicmessage", "Load();", true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.Cookies["Custumer"] != null)
            {

                PotCustLicenseInfo potCustLicense = new PotCustLicenseInfo();
                BaseBO baseBo = new BaseBO();
                DataSet ds = new DataSet();
                ds = baseBo.QueryDataSet("select CustLicenseCode from PotCustLicense where CustLicenseCode='" + txtLicenseID.Text + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustLicenseCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtLicenseID.select()", true);
                    pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
                }
                else
                {
                    potCustLicense.CustLicenseID = BaseApp.GetCustLicenseID();
                    potCustLicense.CustLicenseCode = txtLicenseID.Text;
                    potCustLicense.CustLicenseName = txtLicenseName.Text;

                    try { potCustLicense.CustLicenseType = Convert.ToInt32(cmbLicenseType.SelectedValue); }
                    catch { potCustLicense.CustLicenseType = 0; }
                    try { potCustLicense.CustLicenseStartDate = Convert.ToDateTime(txtLicenseBeginDate.Text); }
                    catch { potCustLicense.CustLicenseStartDate = DateTime.Now; }
                    try { potCustLicense.CustLicenseEndDate = Convert.ToDateTime(txtLicenseEndDate.Text); }
                    catch { potCustLicense.CustLicenseEndDate = DateTime.Now; }
                    potCustLicense.CustID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);

                    potCustLicense.UploadLicense = this.GetUploadFileName();//获得上传文件的名称
                    if (potCustLicense.UploadLicense != "")
                    {
                        string vsUrl = Server.MapPath("UploadLicense/") + potCustLicense.UploadLicense;
                        FileUpload1.SaveAs(vsUrl);//保存文件
                    }

                    int result = baseBo.Insert(potCustLicense);
                    if (result != -1)
                    {

                        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                        Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "')</script>");
                        pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
                    }
                    else
                    {
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                        Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "')</script>");
                        pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
                        return;
                    }
                    CancelCustLicenseText();
                    numCount = 0;
                    
                }
            }
            else
            {
                pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
                return;
            }          
            
        }
        catch (Exception ex)
        {
            string str = (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            Response.Write("<script>alert('" + str + "')</script>");
            Logger.Log("添加证照信息错误:", ex);
            pageDisprove(Convert.ToInt32(0));
        }
        foreach (GridViewRow grv in GrdCustLicense.Rows)
        {
            grv.BackColor = Color.White;
        }
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

    public void CancelCustLicenseText()
    {
        txtLicenseID.Text = "";
        txtLicenseName.Text = "";
        txtLicenseBeginDate.Text = "";
        txtLicenseEndDate.Text = "";
        btnAdd.Enabled = true;
        btnEdit.Enabled = false;

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        PotCustLicenseInfo potCustLicense = new PotCustLicenseInfo();
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select CustLicenseCode from PotCustLicense where CustLicenseCode='" + txtLicenseID.Text + "'");
        if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
        {
            potCustLicense.CustLicenseCode = txtLicenseID.Text;
            potCustLicense.CustLicenseName = txtLicenseName.Text;
            potCustLicense.CustLicenseType = Convert.ToInt32(cmbLicenseType.SelectedValue);
            potCustLicense.CustLicenseStartDate = Convert.ToDateTime(txtLicenseBeginDate.Text);
            potCustLicense.CustLicenseEndDate = Convert.ToDateTime(txtLicenseEndDate.Text);

            potCustLicense.UploadLicense = this.GetUploadFileName();//获得上传文件的名称
            if (potCustLicense.UploadLicense != "")
            {
                string vsUrl = Server.MapPath("UploadLicense/") + potCustLicense.UploadLicense;
                FileUpload1.SaveAs(vsUrl);//保存文件
            }

            try
            {
                baseBo.WhereClause = "CustLicenseID=" + ViewState["custLicenseID"].ToString();

                int result = baseBo.Update(potCustLicense);
                if (result != -1)
                {
                    if (potCustLicense.UploadLicense!="")
                        baseBo.ExecuteUpdate("update PotCustLicense set UploadLicense='" + potCustLicense.UploadLicense + "' where CustLicenseID='" + ViewState["custLicenseID"].ToString() + "'");

                    pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
                    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                    Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "')</script>");
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "')</script>");
                    CancelCustLicenseText();
                    return;
                }

                btnEdit.Enabled = false;
                btnAdd.Enabled = true;
                CancelCustLicenseText();
                foreach (GridViewRow grv in GrdCustLicense.Rows)
                {
                    grv.BackColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                Response.Write("<script>alert('" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "')</script>");
                Logger.Log("编辑证照信息错误:", ex);
            }
        }
        else
        {
            string str = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustLicenseCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist");
            Response.Write("<script>alert('" + str + "')</script>");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustLicenseCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtLicenseID.select()", true);
            pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
        }


    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        CancelCustLicenseText();
        pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
        foreach (GridViewRow grv in GrdCustLicense.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
    //}
    protected void pageDisprove(int custumerID)
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        baseBO.WhereClause = "CustID=" + custumerID;

        DataTable dt = baseBO.QueryDataSet(new PotCustLicenseInfo()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCustLicense.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
        }
        else
        {
            //GrdCustLicense.EmptyDataText = "";
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
            //this.GrdCustLicense.DataSource = pds;
            //this.GrdCustLicense.DataBind();
            //spareRow = GrdCustLicense.Rows.Count;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
            spareRow = GrdCustLicense.Rows.Count;
            for (int i = 0; i < GrdCustLicense.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }

            pds.DataSource = dt.DefaultView;
            GrdCustLicense.DataSource = pds;
            GrdCustLicense.DataBind();
        }
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

    protected void GrdCustLicense_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text != "&nbsp;")
            {
                gIntro = e.Row.Cells[1].Text.ToString();
                e.Row.Cells[1].Text = SubStr(gIntro, 5);
                gIntro = e.Row.Cells[2].Text.ToString();
                e.Row.Cells[2].Text = SubStr(gIntro, 10);
                gIntro = e.Row.Cells[3].Text.ToString();
                e.Row.Cells[3].Text = SubStr(gIntro, 10);
            }
            else
            {
                e.Row.Cells[4].Text = "";
            }
        }
    }

    protected void GrdCustLicense_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["custLicenseID"] = GrdCustLicense.SelectedRow.Cells[0].Text;
        BaseBO bo = new BaseBO();
        bo.WhereClause = "CustLicenseID=" + ViewState["custLicenseID"].ToString();
        Resultset rs = bo.Query(new PotCustLicenseInfo());
        if (rs.Count != 0)
        {
            PotCustLicenseInfo potCustLicense = rs.Dequeue() as PotCustLicenseInfo;
            txtLicenseID.Text = potCustLicense.CustLicenseCode;
            txtLicenseName.Text = potCustLicense.CustLicenseName;
            cmbLicenseType.SelectedValue = potCustLicense.CustLicenseType.ToString();
            txtLicenseBeginDate.Text = potCustLicense.CustLicenseStartDate.ToShortDateString();
            txtLicenseEndDate.Text = potCustLicense.CustLicenseEndDate.ToShortDateString();

            btnAdd.Enabled = false;
            btnEdit.Enabled = true;
            //BtnDel.Enabled = true;

        }
        Session["editLog"] = txtLicenseID.Text;
        pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
    }
    protected void BtnDel_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();

        try
        {
            baseBo.WhereClause = "CustLicenseID=" + ViewState["custLicenseID"].ToString();

            int result = baseBo.Delete(new PotCustLicenseInfo());
            if (result != -1)
            {
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
            }
            else
            {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                CancelCustLicenseText();
                return;
            }
            btnEdit.Enabled = false;
            btnAdd.Enabled = true;
            //BtnDel.Enabled = false;
            CancelCustLicenseText();
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            Logger.Log("删除证照信息错误:", ex);
        }
    }
    protected void GrdCustLicense_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        pageDisprove(Convert.ToInt32(ViewState["CustumerID"]));
        foreach (GridViewRow grv in GrdCustLicense.Rows)
        {
            grv.BackColor = Color.White;
        }
    }

}
