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
using Lease;
using Lease.PotCustLicense;
using Lease.ConShop;
using RentableArea;
using Lease.PotBargain;
using Lease.Formula;
using Lease.Customer;
using Lease.Contract;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using BaseInfo.User;
using System.Text;
using Base.Page;
using Shop.ShopType;
using Base.Util;
using Lease.SMSPara;
using Lease.AdContract;

public partial class Lease_AdContract_AdBoardList : BasePage
{
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    BaseTrans baseTrans = new BaseTrans();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    private ConAdBoard conAdBoard;
    public string beginEndDate;
    private static int CONSHOPID_NULL = 1;
    private static int CONSHOPID_NOTNULL = 2;
    public string enterInfo;
    public string enterMess;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            enterInfo = (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblAdBoardName") + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            enterMess = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            btnShopSave.Attributes.Add("onclick", "return InputValidator(form1)");
            GetAllAdInfo();/*获取合同对应的广告位*/
            BindData();
            this.txtAdBoardCode.Attributes.Add("onclick", "ShowTree()");
            TextEnabled(false);/*锁定控件*/
            if (Request.QueryString["modify"] == null && (Request.Cookies["Info"]["modify"] == null || Request.Cookies["Info"]["modify"] == "")) //非修改合同
            {
                rdoYes.Enabled = false;
                rdoNo.Enabled = false;
            }
            else if (Request.QueryString["modify"] == "1" || Request.Cookies["Info"]["modify"] == "1")
            {
                rdoConfirm.Enabled = false;
                rdoYes.Checked = true;
            }
            this.btnShopDel.Enabled = false;
            btnShopSave.Enabled = false;
        }
    }

    private void BindData()
    {
        ddlDay.Items.Clear();
        for (int i = 0; i < 31; i++)
        {
            ddlDay.Items.Add(new ListItem((i+1).ToString(), (i+1).ToString()));
        }

        cblWeek.Items.Clear();
        cblWeek.Items.Add(new ListItem("星期一", "1"));
        cblWeek.Items.Add(new ListItem("星期二", "2"));
        cblWeek.Items.Add(new ListItem("星期三", "3"));
        cblWeek.Items.Add(new ListItem("星期四", "4"));
        cblWeek.Items.Add(new ListItem("星期五", "5"));
        cblWeek.Items.Add(new ListItem("星期六", "6"));
        cblWeek.Items.Add(new ListItem("星期日", "7"));

        ddlMonthFrom.Items.Clear();
        for (int i = 0; i < 31; i++)
        {
            ddlMonthFrom.Items.Add(new ListItem((i + 1).ToString(), (i + 1).ToString()));
        }

        ddlMonthTo.Items.Clear();
        for (int i = 0; i < 31; i++)
        {
            ddlMonthTo.Items.Add(new ListItem((i + 1).ToString(), (i + 1).ToString()));
        }
    }

    protected void btnShopAdd_Click(object sender, EventArgs e)
    {
        TextEnabled(true);
        GetAllAdInfo();
        ViewState["Flag"] = "add";
        btnShopSave.Enabled = true;
    }
    protected void btnShopDel_Click(object sender, EventArgs e)
    {
        try
        {
            baseTrans.BeginTrans();
            baseTrans.WhereClause = "";
            baseTrans.WhereClause = "ConAdBoardID = " + Convert.ToInt32(ViewState["ConAdBoardID"].ToString());
            if (baseTrans.Delete(new ConAdBoard()) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                baseTrans.Rollback();
                GetAllAdInfo();
                return;
            }
            baseTrans.Commit();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            GetAllAdInfo();
            TextEnabled(false);
            TextClear();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("删除广告位信息错误:", ex);
            baseTrans.Rollback();
        }
        this.btnShopDel.Enabled = false;
        btnShopSave.Enabled = false;
    }
    protected void btnShopSave_Click(object sender, EventArgs e)
    {
        try
        {
            baseTrans.BeginTrans();
            if (InsertOrUpdateBaseInfo() == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                baseTrans.Rollback();
                return;
            }
            this.btnShopDel.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            baseTrans.Commit();
            GetAllAdInfo();
            TextClear();
            TextEnabled(false);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加商铺信息错误:", ex);
            baseTrans.Rollback();
        }
        btnShopSave.Enabled = false;
    }

    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ConAdBoardID = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
        ViewState["ConAdBoardID"] = ConAdBoardID;
        GetBaseInfo(ConAdBoardID);
        GetAllAdInfo();
        ViewState["Flag"] = "modify";
        TextEnabled(true);
        this.btnShopDel.Enabled = true;
        btnShopSave.Enabled = true;
    }
   

    #region 获取对应广告
    private void GetAllAdInfo()
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";
        int contractID = 0;
        if (Request.Cookies["Info"].Values["conID"] != "")
            contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        //baseBo.WhereClause = "conadboard.contractid = " + contractID;
        string strSql = @"select conadboard.conadboardid,adboardmanage.adboardcode,adboardmanage.adboardname
                            from conadboard
                            left join adboardmanage on adboardmanage.adboardid=conadboard.adboardid 
                            where conadboard.contractid = " + contractID;
        //ConAdBoard objConAdBoard = new ConAdBoard();
        //objConAdBoard.SetQuerySql(strSql);
        DataSet shopDs = baseBo.QueryDataSet(strSql);
        DataTable shopDt = shopDs.Tables[0];
        int shopCount = shopDt.Rows.Count;

        //decimal ss = 0;
        //for (int j = 0; j < shopCount; j++)
        //{
        //    ss += Convert.ToDecimal(shopDt.Rows[j]["RentArea"]);
        //}
        //ViewState["shopArea"] = ss;  //所有商铺面积之和

        baseBo.OrderBy = "";
        baseBo.WhereClause = "ContractID = " + contractID;
        DataSet contractDate = baseBo.QueryDataSet(new Contract());
        DataTable contractDt = contractDate.Tables[0];
        if (contractDt.Rows.Count > 0)
        {
            txtStartDate.Text = Convert.ToDateTime(contractDt.Rows[0]["conStartDate"]).ToString("yyyy-MM-dd");
            txtEndDate.Text = Convert.ToDateTime(contractDt.Rows[0]["conEndDate"]).ToString("yyyy-MM-dd");
        }
        int countNull = 13 - shopCount;
        for (int i = 0; i < countNull; i++)
        {
            shopDt.Rows.Add(shopDt.NewRow());
        }
        gvShop.DataSource = shopDt;
        gvShop.DataBind();
        //int gvCount = gvShop.Rows.Count;
        //for (int j = shopCount; j < gvCount; j++)
        //    gvShop.Rows[j].Cells[3].Text = "";
        //if (FirstStatus == 1)
        //{
        //    if (shopCount > 0)
        //    {
        //        GetBaseInfo(Convert.ToInt32(shopDt.Rows[0]["ConAdBoardID"].ToString()));
        //    }
        //}
    }
    #endregion
    #region 获取商铺基本信息
    protected void GetBaseInfo(int conAdBoardID)
    {
        baseBo.WhereClause = " ConAdBoardID = " + conAdBoardID;
        //baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBo.Query(new ConAdBoard());
        if (rs.Count == 1)
        {
            ConAdBoard conAdBoard = rs.Dequeue() as ConAdBoard;
            allvalue.Value = conAdBoard.AdBoardID.ToString().Trim();
            baseBo.WhereClause = " AdBoardID = " + conAdBoard.AdBoardID;
            DataSet ds = new DataSet();
            ds = baseBo.QueryDataSet(new AdBoardManage());
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAdBoardCode.Text = ds.Tables[0].Rows[0]["AdBoardCode"].ToString();
                txtConAdBoardName.Text = ds.Tables[0].Rows[0]["AdBoardName"].ToString();
            }
            else
            {
                txtAdBoardCode.Text = "";
                txtConAdBoardName.Text = "";            
            }
            txtStartDate.Text = conAdBoard.ConAdBoardStartDate.ToString("yyyy-MM-dd");
            txtEndDate.Text = conAdBoard.ConAdBoardEndDate.ToString("yyyy-MM-dd");
            txtTime.Text = conAdBoard.Airtime.ToString();
            txtArea.Text = conAdBoard.RentArea.ToString();
            if (conAdBoard.Freq == ConAdBoard.Freq_Day)
            {
                rbtDay.Checked = true;
                rbtMonth.Checked = false;
                rbtWeek.Checked = false;
                ddlDay.Visible = true;
                Label3.Visible = true;
                cblWeek.Visible = false;
                ddlMonthFrom.Visible = false;
                ddlMonthTo.Visible = false;
                Label2.Visible = false;
                ddlDay.SelectedValue = conAdBoard.FreqDays.ToString();
            }
            else if (conAdBoard.Freq == ConAdBoard.Freq_Week)
            {
                rbtWeek.Checked = true;
                rbtDay.Checked = false;
                rbtMonth.Checked = false;
                ddlDay.Visible = false;
                Label3.Visible = false;
                cblWeek.Visible = true;
                ddlMonthFrom.Visible = false;
                ddlMonthTo.Visible = false;
                Label2.Visible = false;
                if (conAdBoard.FreqMon == ConAdBoard.FreqWeek_Yes)
                {
                    cblWeek.Items[0].Selected = true;
                }
                if (conAdBoard.FreqTue == ConAdBoard.FreqWeek_Yes)
                {
                    cblWeek.Items[1].Selected = true;
                }
                if (conAdBoard.FreqWed == ConAdBoard.FreqWeek_Yes)
                {
                    cblWeek.Items[2].Selected = true;
                }
                if (conAdBoard.FreqThu == ConAdBoard.FreqWeek_Yes)
                {
                    cblWeek.Items[3].Selected = true;
                }
                if (conAdBoard.FreqFri == ConAdBoard.FreqWeek_Yes)
                {
                    cblWeek.Items[4].Selected = true;
                }
                if (conAdBoard.FreqSat == ConAdBoard.FreqWeek_Yes)
                {
                    cblWeek.Items[5].Selected = true;
                }
                if (conAdBoard.FreqSun == ConAdBoard.FreqWeek_Yes)
                {
                    cblWeek.Items[6].Selected = true;
                }
            }
            else if (conAdBoard.Freq == ConAdBoard.Freq_Month)
            {
                rbtMonth.Checked = true;
                rbtDay.Checked = false;
                rbtWeek.Checked = false;
                ddlDay.Visible = false;
                Label3.Visible = false;
                cblWeek.Visible = false;
                ddlMonthFrom.Visible = true;
                ddlMonthTo.Visible = true;
                Label2.Visible = true;
                ddlMonthFrom.SelectedValue = conAdBoard.BetweenFr.ToString();
                ddlMonthTo.SelectedValue = conAdBoard.BetweenTo.ToString();
            }
            if (Request.QueryString["modify"] == null && (Request.Cookies["Info"]["modify"] == null || Request.Cookies["Info"]["modify"] == "")) //非修改合同
            {
                rdoConfirm.Checked = true;
                rdoYes.Checked = false;
                rdoNo.Checked = false;
            }
            else if (Request.QueryString["modify"] == "1" || Request.Cookies["Info"]["modify"] == "1")
            {
                rdoConfirm.Checked = false;
                if (conAdBoard.ConAdBoardStatus == ConAdBoard.BLANKOUT_STATUS_INVALID)
                {
                    rdoYes.Checked = false;
                    rdoNo.Checked = true;
                }
                else if (conAdBoard.ConAdBoardStatus == ConAdBoard.BLANKOUT_STATUS_LEASEOUT)
                {
                    rdoNo.Checked = false;
                    rdoYes.Checked = true;
                }
            }
            ViewState["ConAdBoardID"] = conAdBoard.ConAdBoardID;
        }
    }
    #endregion
    #region 添加或修改基本信息

    protected int InsertOrUpdateBaseInfo()
    {
        int result = 0;
        try
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            if (ViewState["Flag"].ToString() == "add")
            {
                FillBaseInfo();
                conAdBoard.ConAdBoardID = BaseApp.GetConAdBoardID();
                conAdBoard.CreateTime = DateTime.Now;
                conAdBoard.CreateUserID = objSessionUser.UserID;
                ViewState["ConAdBoardID"] = conAdBoard.ConAdBoardID;
                result = baseTrans.Insert(conAdBoard);
                ViewState["Flag"] = "modify";
            }
            else if (ViewState["Flag"].ToString() == "modify")
            {
                FillBaseInfo();
                conAdBoard.ConAdBoardID = Convert.ToInt32(ViewState["ConAdBoardID"].ToString());
                conAdBoard.ModifyTime = DateTime.Now;
                conAdBoard.ModifyUserID = objSessionUser.UserID;
                baseTrans.WhereClause = "ConAdBoardID = " + Convert.ToInt32(ViewState["ConAdBoardID"].ToString());
                result = baseTrans.Update(conAdBoard);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
        }
        return result;
    }
    #endregion

    #region 填充广告位基本信息

    protected void FillBaseInfo()
    {
        try
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            conAdBoard = new ConAdBoard();
            conAdBoard.ContractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            conAdBoard.AdBoardID = int.Parse(allvalue.Value);
            BaseBO basebo = new BaseBO();
            DataSet ds = new DataSet();
            AdBoardManage adManage = new AdBoardManage();
            basebo.WhereClause = " adboardid='" + int.Parse(allvalue.Value) + "' ";
            ds = basebo.QueryDataSet(adManage);
            if (ds.Tables[0].Rows.Count > 0)
            {
                conAdBoard.StoreID = int.Parse(ds.Tables[0].Rows[0]["StoreID"].ToString());
                conAdBoard.BuildingID = int.Parse(ds.Tables[0].Rows[0]["BuildingID"].ToString());
            }
            conAdBoard.RentArea = decimal.Parse(txtArea.Text.Trim());
            conAdBoard.ConAdBoardStatus = ConAdBoard.BLANKOUT_STATUS_PAUSE;
            conAdBoard.ConAdBoardStartDate = Convert.ToDateTime(txtStartDate.Text);
            conAdBoard.ConAdBoardEndDate = Convert.ToDateTime(txtEndDate.Text);
            if (txtTime.Text.Trim() != "")
            {
                conAdBoard.Airtime = int.Parse(txtTime.Text.Trim());
            }
            conAdBoard.OprDeptID = objSessionUser.DeptID;
            conAdBoard.OprRoleID = objSessionUser.RoleID;
            if (rbtDay.Checked)
            {
                conAdBoard.Freq = ConAdBoard.Freq_Day;
                conAdBoard.FreqDays = int.Parse(ddlDay.SelectedValue.Trim());
            }
            else if (rbtWeek.Checked)
            {
                conAdBoard.Freq = ConAdBoard.Freq_Week;
                if (cblWeek.Items[0].Selected)
                {
                    conAdBoard.FreqMon = ConAdBoard.FreqWeek_Yes;
                }
                if (cblWeek.Items[1].Selected)
                {
                    conAdBoard.FreqTue = ConAdBoard.FreqWeek_Yes;
                }
                if (cblWeek.Items[2].Selected)
                {
                    conAdBoard.FreqWed = ConAdBoard.FreqWeek_Yes;
                }
                if (cblWeek.Items[3].Selected)
                {
                    conAdBoard.FreqThu = ConAdBoard.FreqWeek_Yes;
                }
                if (cblWeek.Items[4].Selected)
                {
                    conAdBoard.FreqFri = ConAdBoard.FreqWeek_Yes;
                }
                if (cblWeek.Items[5].Selected)
                {
                    conAdBoard.FreqSat = ConAdBoard.FreqWeek_Yes;
                }
                if (cblWeek.Items[6].Selected)
                {
                    conAdBoard.FreqSun = ConAdBoard.FreqWeek_Yes;
                }
            }
            else if (rbtMonth.Checked)
            {
                conAdBoard.Freq = ConAdBoard.Freq_Month;
                conAdBoard.BetweenFr = int.Parse(ddlMonthFrom.SelectedValue.Trim());
                conAdBoard.BetweenTo = int.Parse(ddlMonthTo.SelectedValue.Trim());
            }
            if (Request.QueryString["modify"] == null && (Request.Cookies["Info"]["modify"] == null || Request.Cookies["Info"]["modify"] == "")) //非修改合同
            {
                conAdBoard.ConAdBoardStatus = ConAdBoard.BLANKOUT_STATUS_PAUSE;
            }
            else if (Request.QueryString["modify"] == "1" || Request.Cookies["Info"]["modify"] == "1")
            {
                if (rdoYes.Checked)
                    conAdBoard.ConAdBoardStatus = ConAdBoard.BLANKOUT_STATUS_LEASEOUT;
                else if (rdoNo.Checked)
                    conAdBoard.ConAdBoardStatus = ConAdBoard.BLANKOUT_STATUS_INVALID;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加合同再签信息错误:", ex);
        }
    }
    #endregion


    private void TextEnabled(Boolean boolean)
    {
        txtAdBoardCode.Enabled = boolean;
        txtConAdBoardName.Enabled = boolean;
        txtEndDate.Enabled = boolean;
        txtStartDate.Enabled = boolean;
        txtTime.Enabled = boolean;
        rbtDay.Enabled = boolean;
        rbtMonth.Enabled = boolean;
        rbtWeek.Enabled = boolean;
        ddlDay.Enabled = boolean;
        cblWeek.Enabled = boolean;
        ddlMonthFrom.Enabled = boolean;
        ddlMonthTo.Enabled = boolean;
    }

    private void TextClear()
    {
        txtAdBoardCode.Text = "";
        txtConAdBoardName.Text = "";
        txtEndDate.Text = "";
        txtStartDate.Text = "";
        txtTime.Text = "";
        txtArea.Text = "";
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string strID = allvalue.Value;
        BaseBO objBase = new BaseBO();
        DataSet ds = objBase.QueryDataSet("select * from AdBoardManage where AdBoardID='" + strID + "'");
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            txtAdBoardCode.Text = strID.Trim();
            this.txtConAdBoardName.Text = ds.Tables[0].Rows[0]["AdBoardName"].ToString();
            txtArea.Text = ds.Tables[0].Rows[0]["UseArea"].ToString();
        }
        GetAllAdInfo();
    }
    protected void gvShop_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.RowIndex >= 0)
            {
                if (e.Row.Cells[0].Text == "&nbsp;")
                {
                    e.Row.Cells[3].Text = "";
                }
            }
        }
    }
    protected void rbtDay_CheckedChanged(object sender, EventArgs e)
    {
        ddlDay.Visible = true;
        Label3.Visible = true;
        cblWeek.Visible = false;
        ddlMonthFrom.Visible = false;
        ddlMonthTo.Visible = false;
        Label2.Visible = false;
        GetAllAdInfo();
    }
    protected void rbtWeek_CheckedChanged(object sender, EventArgs e)
    {
        ddlDay.Visible = false;
        Label3.Visible = false;
        cblWeek.Visible = true;
        ddlMonthFrom.Visible = false;
        ddlMonthTo.Visible = false;
        Label2.Visible = false;
        GetAllAdInfo();
    }
    protected void rbtMonth_CheckedChanged(object sender, EventArgs e)
    {
        ddlDay.Visible = false;
        Label3.Visible = false;
        cblWeek.Visible = false;
        ddlMonthFrom.Visible = true;
        ddlMonthTo.Visible = true;
        Label2.Visible = true;
        GetAllAdInfo();
    }
}
