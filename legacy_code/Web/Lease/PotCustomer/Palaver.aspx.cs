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

using Lease.PotCustLicense;
using Lease.Contract;
using Base.Biz;
using Base.DB;
using RentableArea;
using Base;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using BaseInfo.User;
using Base.Page;
using Base.Util;
using System.Drawing;

public partial class Lease_PotCustomer_Palaver : BasePage
{
    public string strError;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnSave.Attributes.Add("onclick", "return InputValidator()");
        if (!IsPostBack)
        {
            if (Request["look"] != null)
            {
                if (Request["look"] == "yes")
                {
                    this.btnSave.Visible = false;
                    this.btnCancel.Visible = false;
                    this.btnEdit.Visible = false;
                    this.ddlProcessTypeId.Enabled = false;
                }
            }
            strError = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");//信息不能为空
            txtPalaverAim.Attributes.Add("onblur", "TextIsNotNull(txtPalaverAim,ImgPalaverAim)");
            this.BindProcessType();//绑定招商进程级别
            page();
            this.txtPalaverRound.Text = this.GetMaxPalaverRound().ToString();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Publicmessage", "Load();", true);
        }
    }

    /// <summary>
    /// 绑定招商进程级别
    /// </summary>
    private void BindProcessType()
    {
        BaseInfo.BaseCommon.BindDropDownList("select ProcessTypeID,ProcessTypeName from ProcessType where status=1", "ProcessTypeID", "ProcessTypeName", this.ddlProcessTypeId);
    }
    /// <summary>
    /// 得到谈判轮次最大次数
    /// </summary>
    /// <returns></returns>
    private int GetMaxPalaverRound()
    {
        BaseBO objBaseBo = new BaseBO();
        CustPalaverInfo custPalaver = new CustPalaverInfo();
        string strSql = "select max(PalaverRound) as PalaverRound from CustPalaver where CustID='" + Request.Cookies["Custumer"].Values["CustumerID"].ToString() + "' and sequence='" + Request.Cookies["Sequence"].Values["SequenceID"].ToString() + "'";
        string strMaxRound = objBaseBo.QueryDataSet(strSql).Tables[0].Rows[0]["PalaverRound"].ToString();
        if (strMaxRound == "")
            return 1;
        else
            return Int32.Parse(strMaxRound)+1;
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        this.txtNode.Text = "";
        //this.txtPalaverRound.Text = "";
        this.txtPalaverPlace.Text = "";
        this.txtContactorName.Text = "";
        this.txtPalaverAim.Text = "";
        this.txtPalaverContent.Text = "";
        this.txtPalaverResult.Text = "";
        this.txtUnSolved.Text = "";
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CustPalaverInfo custPalaver = new CustPalaverInfo();
            BaseBO baseBO = new BaseBO();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            custPalaver.CustID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            custPalaver.PalaverID = BaseApp.GetPalaverID();
            try { custPalaver.ProcessTypeId = Int32.Parse(this.ddlProcessTypeId.SelectedValue.ToString()); }
            catch { custPalaver.ProcessTypeId = 0; }
            try { custPalaver.PalaverRound = Int32.Parse(this.txtPalaverRound.Text.Trim()); }
            catch { custPalaver.PalaverRound = 0; }
            custPalaver.PalaverPlace = this.txtPalaverPlace.Text.Trim();
            custPalaver.ContactorName = this.txtContactorName.Text.Trim();
            custPalaver.PalaverAim = this.txtPalaverAim.Text.Trim();
            custPalaver.PalaverContent = this.txtPalaverContent.Text.Trim();
            custPalaver.PalaverResult = this.txtPalaverResult.Text.Trim();
            custPalaver.UnSolved = this.txtUnSolved.Text.Trim();
            custPalaver.Node = this.txtNode.Text.Trim();
            custPalaver.PalaverTime = DateTime.Now.Date;
            custPalaver.CreateUserId = objSessionUser.CreateUserID;
            custPalaver.CreateTime = DateTime.Now.Date;
            custPalaver.ModifyUserId = objSessionUser.ModifyUserID;
            custPalaver.ModifyTime = DateTime.Now.Date;
            custPalaver.OprRoleID = objSessionUser.OprRoleID;
            custPalaver.OprDeptID = objSessionUser.OprDeptID;
            custPalaver.Sequence = Convert.ToInt32(Request.Cookies["Sequence"].Values["SequenceID"]);
            custPalaver.PalaverStatus = 1;
            int strSort = 1;
            if(baseBO.QueryDataSet("select shopsort from potshop where custid="+Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"])+" and shopstatus=1").Tables[0].Rows.Count>0)
            {
                 strSort = Int32.Parse(baseBO.QueryDataSet("select shopsort from potshop where custid="+Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"])+" and shopstatus=1").Tables[0].Rows[0]["shopsort"].ToString());
            }
            else
            {
                strSort=Int32.Parse(baseBO.QueryDataSet("select count(shopsort) as shopsort from potshop where custid="+Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"])+" and shopstatus=0").Tables[0].Rows[0]["shopsort"].ToString())+1;
            }
            custPalaver.PalaverSort = strSort;

            if (baseBO.Insert(custPalaver) != -1)
            {
                this.txtPalaverRound.Text = this.GetMaxPalaverRound().ToString();//谈判轮次增加1
                ClearText();//清空输入框
                page();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                page();
                return;
            }
        }
        catch(Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加谈判信息错误:", ex);
        }
        foreach (GridViewRow grv in GrdCustPalaverInfo.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/PotCustomer/Palaver.aspx");
    }
    protected void butAuditing_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        PotCustomerStatus potCustomerStatus = new PotCustomerStatus();
        baseBO.WhereClause = "custid=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
        potCustomerStatus.CustomerStatus = PotCustomer.POTCUSTOMER_YES_PUT_IN_NO_UPDATE_LEASE_STATUS;
        if (baseBO.Update(potCustomerStatus) < 1)
        {
            return;
        }
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        int voucherID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);

        String voucherHints = "";
        String voucherMemo = "";
        int operatorID = objSessionUser.UserID;
        int deptID = objSessionUser.DeptID;
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, operatorID);
        WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo);
    }
    protected void GrdCustPalaverInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Resultset rs = new Resultset();
        BaseBO baseBO = new BaseBO();
        CustPalaverInfo custPalaver = new CustPalaverInfo();
        ViewState["PalaverID"] = GrdCustPalaverInfo.SelectedRow.Cells[0].Text;
        baseBO.WhereClause = "PalaverID=" + GrdCustPalaverInfo.SelectedRow.Cells[0].Text;
        rs = baseBO.Query(custPalaver);
        if (rs.Count == 1)
        {
            CustPalaverInfo cust = rs.Dequeue() as CustPalaverInfo;
            this.ddlProcessTypeId.SelectedValue = cust.ProcessTypeId.ToString();
            this.txtPalaverRound.Text = cust.PalaverRound.ToString();
            this.txtPalaverPlace.Text = cust.PalaverPlace;
            this.txtContactorName.Text = cust.ContactorName;
            this.txtPalaverAim.Text = cust.PalaverAim;
            this.txtPalaverContent.Text = cust.PalaverContent;
            this.txtPalaverResult.Text = cust.PalaverResult;
            this.txtUnSolved.Text = cust.UnSolved;
            this.txtNode.Text = cust.Node;
        }
        this.btnEdit.Enabled = true;
        this.btnSave.Enabled = false;
        page();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        
    }
    protected void GrdCustPalaverInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text != "&nbsp;")
            {
                gIntro = e.Row.Cells[1].Text.ToString();
                e.Row.Cells[1].Text = SubStr(gIntro, 10);
                gIntro = e.Row.Cells[2].Text.ToString();
                e.Row.Cells[2].Text = SubStr(gIntro, 7);
            }
            else
            {
                e.Row.Cells[3].Text = "";
            }
        }
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        try
        {
            if (Request.Cookies["Custumer"].Values["CustumerID"] == "")
            {
                baseBO.WhereClause = "CustID=" + 0;
            }
            else
            {
                baseBO.WhereClause = "custid=" + Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]) + " and PalaverStatus=1";
            }
        }
        catch 
        {
            baseBO.WhereClause = "CustID=" + 0;
        }

        DataTable dt = baseBO.QueryDataSet(new CustPalaverInfo()).Tables[0];
        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdCustPalaverInfo.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustPalaverInfo.DataSource = pds;
            GrdCustPalaverInfo.DataBind();
        }
        else
        {
            this.GrdCustPalaverInfo.DataSource = pds;
            this.GrdCustPalaverInfo.DataBind();
            spareRow = GrdCustPalaverInfo.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdCustPalaverInfo.DataSource = pds;
            GrdCustPalaverInfo.DataBind();
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
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (ViewState["PalaverID"] == null || ViewState["PalaverID"].ToString() == "")
        {
            return;
        }
        try
        {
            CustPalaverInfo custPalaver = new CustPalaverInfo();
            BaseBO baseBO = new BaseBO();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            custPalaver.CustID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
            //custPalaver.PalaverID = BaseApp.GetPalaverID();
            try { custPalaver.ProcessTypeId = Int32.Parse(this.ddlProcessTypeId.SelectedValue.ToString()); }
            catch { custPalaver.ProcessTypeId = 0; }
            try { custPalaver.PalaverRound = Int32.Parse(this.txtPalaverRound.Text.Trim()); }
            catch { custPalaver.PalaverRound = 0; }
            custPalaver.PalaverPlace = this.txtPalaverPlace.Text.Trim();
            custPalaver.ContactorName = this.txtContactorName.Text.Trim();
            custPalaver.PalaverAim = this.txtPalaverAim.Text.Trim();
            custPalaver.PalaverContent = this.txtPalaverContent.Text.Trim();
            custPalaver.PalaverResult = this.txtPalaverResult.Text.Trim();
            custPalaver.UnSolved = this.txtUnSolved.Text.Trim();
            custPalaver.Node = this.txtNode.Text.Trim();

            custPalaver.CreateUserId = objSessionUser.CreateUserID;
            custPalaver.CreateTime = DateTime.Now.Date;
            custPalaver.ModifyUserId = objSessionUser.ModifyUserID;
            custPalaver.ModifyTime = DateTime.Now.Date;
            custPalaver.OprRoleID = objSessionUser.OprRoleID;
            custPalaver.OprDeptID = objSessionUser.OprDeptID;
            baseBO.WhereClause = "PalaverID=" + ViewState["PalaverID"].ToString();
            if (baseBO.Update(custPalaver) != -1)
            {
                this.txtPalaverRound.Text = this.GetMaxPalaverRound().ToString();//谈判轮次增加1
                ClearText();//清空输入框
                page();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_Error") + "'", true);
                page();
                return;
            }
            this.btnEdit.Enabled = false;
            this.btnSave.Enabled = true;
            foreach (GridViewRow grv in GrdCustPalaverInfo.Rows)
            {
                grv.BackColor = Color.White;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("修改谈判信息错误:", ex);
        }
        ViewState["PalaverID"] = "";

    }

    protected void gvShopBrand_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        page();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        foreach (GridViewRow grv in GrdCustPalaverInfo.Rows)
        {
            grv.BackColor = Color.White;
        }
    }

}
