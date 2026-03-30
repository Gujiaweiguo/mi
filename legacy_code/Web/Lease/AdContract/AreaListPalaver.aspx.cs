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
public partial class Lease_AdContract_AreaListPalaver : BasePage
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            beginEndDate = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");
            BindShopType();/*绑定广告位类型*/
            GetAllShopInfo(1);/*获取合同对应的商铺*/
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
        }
    }
    protected void gvShop_SelectedIndexChanged(object sender, EventArgs e)
    {
        int shopId = Convert.ToInt32(gvShop.SelectedRow.Cells[0].Text);
        ViewState["delShopID"] = shopId;
        GetShopBaseInfo(shopId);
        GetAllShopInfo(0);
        ViewState["shopFlag"] = "modify";
    }

    #region 初始化DropDownList
    //绑定广告位类型
    protected void BindShopType()
    {
        rs = baseBo.Query(new AreaType());
        cmbAreaType.Items.Clear();
        foreach (AreaType areaType in rs)
        {
            cmbAreaType.Items.Add(new ListItem(areaType.AreaTypeDesc, areaType.AreaTypeID.ToString()));
        }
    }
    #endregion

    #region 获取合同对应的商铺


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
        string strSql = "select ConAreaID,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,(select AreaCode from AreaManage where AreaID=ConAreaCode) as AreaCode,ConAreaName,ConAreaTypeID,ConAreaDesc,ConAreaStatus,ConAreaStartDate,ConAreaEndDate,RentArea from ConArea";
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
    #endregion

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
            this.txtConAreaCode.Text = BaseInfo.BaseCommon.GetTextValueByID("AreaCode", "AreaID", "AreaManage", conArea.ConAreaCode);
            txtConAreaName.Text = conArea.ConAreaName;
            cmbAreaType.SelectedValue = conArea.ConAreaTypeID.ToString();
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

            conArea.ConAreaCode = txtConAreaCode.Text.Trim();
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


    private void TextClear()
    {
        txtConAreaCode.Text = "";
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
}
