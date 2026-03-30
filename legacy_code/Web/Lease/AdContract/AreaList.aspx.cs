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

public partial class Lease_AdContract_AreaList : BasePage
{
    BaseBO baseBo = new BaseBO();
    Resultset rs = new Resultset();
    BaseTrans baseTrans = new BaseTrans();
    DataSet ds = new DataSet();
    DataTable dt = new DataTable();
    private ConArea conArea;
    public string beginEndDate;
    private static int CONSHOPID_NULL = 1;
    private static int CONSHOPID_NOTNULL = 2;
    public string enterInfo;
    public string enterMess;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            enterInfo = (String)GetGlobalResourceObject("BaseInfo", "Rpt_AreaName") + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            enterMess = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            btnShopSave.Attributes.Add("onclick", "return InputValidator(form1)");
            BindShopType();/*绑定广告位类型*/
            BindArea();//绑定场地
            GetAllShopInfo(1);/*获取合同对应的商铺*/
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
        }
    }
    protected void btnShopAdd_Click(object sender, EventArgs e)
    {
        BindShopType();
        TextEnabled(true);
        TextClear();
        ViewState["shopFlag"] = "add";
        GetAllShopInfo(0);
    }
    protected void btnShopDel_Click(object sender, EventArgs e)
    {
        try
        {
            baseTrans.BeginTrans();
            baseTrans.WhereClause = "";
            baseTrans.WhereClause = "ConAreaID = " + Convert.ToInt32(ViewState["delShopID"]);
            if (baseTrans.Delete(new ConArea()) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                baseTrans.Rollback();
                GetAllShopInfo(1);
                return;
            }
            baseTrans.Commit();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            GetAllShopInfo(1);
            TextEnabled(false);
            TextClear();
            this.btnShopDel.Enabled = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("删除场地信息错误:", ex);
            baseTrans.Rollback();
        }
    }
    protected void btnShopSave_Click(object sender, EventArgs e)
    {
        try
        {
            baseTrans.BeginTrans();
            if (InsertOrUpdateShopBaseInfo() == -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
            }
            this.btnShopDel.Enabled = false;
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            baseTrans.Commit();
            GetAllShopInfo(0);
            TextClear();
            TextEnabled(false);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加场地信息错误:", ex);
            baseTrans.Rollback();
        }
    }

    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
        ViewState["delShopID"] = shopId;
        GetShopBaseInfo(shopId);
        GetAllShopInfo(0);
        ViewState["shopFlag"] = "modify";
        TextEnabled(true);
        this.btnShopDel.Enabled = true;
    }

    
    /// <summary>
    /// 绑定广告位类型
    /// </summary>
    protected void BindShopType()
    {
        rs = baseBo.Query(new AreaType());
        cmbAreaType.Items.Clear();
        foreach (AreaType areaType in rs)
        {
            cmbAreaType.Items.Add(new ListItem(areaType.AreaTypeDesc, areaType.AreaTypeID.ToString()));
        }
    }
    /// <summary>
    /// 绑定场地编码
    /// </summary>
    private void BindArea()
    {
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        BaseBO objbaseBo = new BaseBO();
        objbaseBo.WhereClause = "AreaStatus=1";
        Resultset rs = objbaseBo.Query(new AreaManage());
        ddlAreaCode.Items.Add(new ListItem(selected));
        foreach (AreaManage objArea in rs)
            this.ddlAreaCode.Items.Add(new ListItem(objArea.AreaCode.ToString(), objArea.AreaID.ToString()));
    }

    private void GetAllShopInfo(int FirstStatus)
    {
        baseBo.WhereClause = "";
        baseBo.OrderBy = "";

        int contractID = 0;
        if (Request.Cookies["Info"].Values["conID"] != "")
        {
            contractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
        }
        baseBo.WhereClause = "ContractID = " + contractID;
        //DataSet shopDs = baseBo.QueryDataSet(new ConArea());
        //string strSql = "select ConAdBoardID,ContractID,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,(select AdBoardCode from AdBoardManage where AdBoardID=ConAdBoardCode) as AdBoardCode," +
        //            "ConAdBoardName,ConAdBoardTypeID,ConAdBoardDesc,ConAdBoardStatus,ConAdBoardStartDate,ConAdBoardEndDate,RentArea from ConAdBoard";
        string strSql = "select ConAreaID,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,(select AreaCode from AreaManage where AreaID=ConAreaCode) as AreaCode,(select AreaName from AreaManage where AreaID=ConAreaCode) as ConAreaName,ConAreaTypeID,ConAreaDesc,ConAreaStatus,ConAreaStartDate,ConAreaEndDate,RentArea from ConArea";
        //ConAdBoard objConAdBoard = new ConAdBoard();
        ConArea objArea = new ConArea();
        objArea.SetQuerySql(strSql);
        DataSet shopDs = baseBo.QueryDataSet(objArea);
        DataTable shopDt = shopDs.Tables[0];
        int shopCount = shopDt.Rows.Count;

        decimal ss = 0;
        for (int j = 0; j < shopCount; j++)
        {
            ss += Convert.ToDecimal(shopDt.Rows[j]["RentArea"]);
        }
        ViewState["shopArea"] = ss;  //所有商铺面积之和

        /*取合同日期*/
        baseBo.WhereClause = "";
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
        int gvCount = gvShop.Rows.Count;
        for (int j = shopCount; j < gvCount; j++)
            gvShop.Rows[j].Cells[3].Text = "";

        if (FirstStatus == 1)
        {
            if (shopCount > 0)
            {
                GetShopBaseInfo(Convert.ToInt32(shopDt.Rows[0]["ConAreaID"]));
            }
        }
    }

    #region 获取场地基本信息
    protected void GetShopBaseInfo(int conAreaID)
    {
        baseBo.WhereClause = "";
        baseBo.WhereClause = " ConAreaID = " + conAreaID;
        //baseBo.WhereClause = "ContractID = " + Convert.ToInt32(ViewState["contractID"]);
        rs = baseBo.Query(new ConArea());
        if (rs.Count == 1)
        {
            ConArea conArea = rs.Dequeue() as ConArea;
            //txtConAreaCode.Text = conArea.ConAreaCode;
            try { this.ddlAreaCode.SelectedValue = conArea.ConAreaCode; }//存的是ID
            catch { }
            //txtConAreaName.Text = conArea.ConAreaName;
            txtConAreaName.Text = BaseInfo.BaseCommon.GetTextValueByID("AreaName", "AreaID", "AreaManage", conArea.ConAreaCode.ToString());
            try { cmbAreaType.SelectedValue = conArea.ConAreaTypeID.ToString(); }
            catch { }
            txtEndDate.Text = conArea.ConAreaEndDate.ToString("yyyy-MM-dd");
            txtStartDate.Text = conArea.ConAreaStartDate.ToString("yyyy-MM-dd");
            txtNote.Text = conArea.ConAreaDesc;

            if (Request.QueryString["modify"] == null && (Request.Cookies["Info"]["modify"] == null || Request.Cookies["Info"]["modify"] == "")) //非修改合同
            {
                rdoConfirm.Checked = true;
                rdoYes.Checked = false;
                rdoNo.Checked = false;
            }
            else if (Request.QueryString["modify"] == "1" || Request.Cookies["Info"]["modify"] == "1")
            {
                rdoConfirm.Checked = false;
                if (conArea.ConAreaStatus == ConAdBoard.BLANKOUT_STATUS_INVALID)
                {
                    rdoYes.Checked = false;
                    rdoNo.Checked = true;
                }
                else if (conArea.ConAreaStatus == ConAdBoard.BLANKOUT_STATUS_LEASEOUT)
                {
                    rdoNo.Checked = false;
                    rdoYes.Checked = true;
                }
            }

            ViewState["ShopId"] = conArea.ConAreaID;
            ViewState["delShopID"] = conAreaID;
        }
    }
    #endregion

    #region 添加或修改商铺基本信息


    protected int InsertOrUpdateShopBaseInfo()
    {
        int result = 0;
        string aa = ViewState["shopFlag"].ToString();
        try
        {
            if (ViewState["shopFlag"].ToString() == "add")
            {
                /*添加商铺信息*/
                FillShopBaseInfo();
                conArea.ConAreaID = BaseApp.GetConAreaID();
                ViewState["ShopId"] = conArea.ConAreaID;
                result = baseTrans.Insert(conArea);
                ViewState["shopFlag"] = "modify";
            }
            else if (ViewState["shopFlag"].ToString() == "modify")
            {
                FillShopBaseInfo();
                conArea.ConAreaID = Convert.ToInt32(ViewState["ShopId"]);
                baseTrans.WhereClause = "";
                baseTrans.WhereClause = "ConAreaID = " + Convert.ToInt32(ViewState["ShopId"]);
                result = baseTrans.Update(conArea);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加场地合同信息错误:", ex);
        }
        return result;
    }
    #endregion

    #region 填充广告位基本信息
    protected void FillShopBaseInfo()
    {
        try
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            conArea = new ConArea();

            //conArea.ConAreaCode = txtConAreaCode.Text.Trim();
            conArea.ConAreaCode = this.ddlAreaCode.SelectedValue;//存的是ID
            conArea.ConAreaName = txtConAreaName.Text.Trim();
            conArea.ConAreaTypeID = Convert.ToInt32(cmbAreaType.SelectedValue);
            conArea.ConAreaStatus = ConAdBoard.BLANKOUT_STATUS_PAUSE;
            conArea.ConAreaStartDate = Convert.ToDateTime(txtStartDate.Text);
            conArea.ConAreaEndDate = Convert.ToDateTime(txtEndDate.Text);
            conArea.ConAreaDesc = txtNote.Text.Trim();
            conArea.ContractID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            conArea.CreateTime = DateTime.Now;
            conArea.ModifyTime = DateTime.Now;
            conArea.CreateUserID = objSessionUser.UserID;
            conArea.OprDeptID = objSessionUser.DeptID;
            conArea.OprRoleID = objSessionUser.RoleID;

            if (Request.QueryString["modify"] == null && (Request.Cookies["Info"]["modify"] == null || Request.Cookies["Info"]["modify"] == "")) //非修改合同
            {
                conArea.ConAreaStatus = ConAdBoard.BLANKOUT_STATUS_PAUSE;
            }
            else if (Request.QueryString["modify"] == "1" || Request.Cookies["Info"]["modify"] == "1")
            {
                if (rdoYes.Checked)
                {
                    conArea.ConAreaStatus = ConAdBoard.BLANKOUT_STATUS_LEASEOUT;
                }
                else if (rdoNo.Checked)
                {
                    conArea.ConAreaStatus = ConAdBoard.BLANKOUT_STATUS_INVALID;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("添加场地合同信息错误:", ex);
        }
    }
    #endregion


    private void TextEnabled(Boolean boolean)
    {
        //txtConAreaCode.Enabled = boolean;
        this.ddlAreaCode.Enabled = boolean;
        txtConAreaName.Enabled = boolean;
        //cmbAreaType.Enabled = boolean;
        txtEndDate.Enabled = boolean;
        txtStartDate.Enabled = boolean;
        txtNote.Enabled = boolean;
    }

    private void TextClear()
    {
       //txtConAreaCode.Text = "";
       txtConAreaName.Text = "";
        //cmbAdBoardType.Items.Clear();
        txtEndDate.Text = "";
        txtStartDate.Text = "";
        txtNote.Text = "";
        //BindShopType();
       
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {

    }
    protected void ddlAreaCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO objbaseBo = new BaseBO();
        if (this.ddlAreaCode.SelectedValue.ToString().Trim() != "---请选择---")
        {
            objbaseBo.WhereClause = "AreaID='" + this.ddlAreaCode.SelectedValue + "'";
            Resultset rs = objbaseBo.Query(new AreaManage());
            if (rs.Count == 1)
            {
                AreaManage objArea = rs.Dequeue() as AreaManage;
                this.txtConAreaName.Text = objArea.AreaName;
                try { this.cmbAreaType.SelectedValue = objArea.AreaTypeID.ToString(); }
                catch { }
            }
        }
        else
        {
            this.txtConAreaName.Text = "";
            this.BindShopType();
        }
        GetAllShopInfo(0);
    }
}
