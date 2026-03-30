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
using Base.Page;
using BaseInfo.Dept;
using RentableArea;
using Lease.ConShop;
using Lease.PotBargain;
using Invoice;
using WorkFlow.WrkFlw;
using WorkFlow;
using Base;
using BaseInfo.User;
using WorkFlow.Uiltil;

public partial class Lease_ChargeAccount_OtherChargePalaver : BasePage
{
    public string emptyStr;
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int chgID = Convert.ToInt32(Request["VoucherID"]);
            ViewState["chgID"] = chgID;
            //WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"].ToString()));
            //string ss = objWrkFlwEntity.VoucherMemo;
            //this.listBoxRemark.Text = ss;
            page(chgID);
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Account_lblOtherChargePalaver");
            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
        }
    }
    protected void page(int chgId)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        baseBO.WhereClause = "rangecode = " + chgId;
        OtherChargeD objOtherCharge = new OtherChargeD();
        objOtherCharge.SetQuerySql("select dept.deptname,OtherChargeD.otherchargedid,OtherChargeD.otherchargehid,OtherChargeD.ChargeTypeID,OtherChargeD.ChgName,Convert(char(10),StartDate,120) as StartDate,Convert(char(10),EndDate,120) as EndDate,OtherChargeD.RefID,OtherChargeD.ChgAmt,'' as ChargeTypeName,OtherChargeD.ChgPeriod,OtherChargeH.ShopID,(select shopname from conshop where conshop.shopid=OtherChargeH.shopid) as ShopName from  OtherChargeD left join OtherChargeH on OtherChargeD.otherchargehid=OtherChargeH.otherchargehid left join conshop on conshop.shopid=OtherChargeH.shopid left join dept on dept.deptid=conshop.storeid");
        DataSet ds = baseBO.QueryDataSet(objOtherCharge);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        ViewState["count"] = count;

        for (int i = count; i <15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }

        gvCharge.DataSource = dt;
        gvCharge.DataBind();
    }
    /// <summary>
    /// 驳回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        BaseTrans baseTrans = new BaseTrans();
        baseTrans.BeginTrans();
        try
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int voucherID = Convert.ToInt32(Request["voucherID"]);
            int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);

            String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
            String voucherMemo = "";
            //修改其他费用单状态为草稿状态
            String sql = "Update OtherChargeH set ChgStatus = " + OtherChargeH.CHGSTATUS_TYPE_TEMP + " where RangeCode =" + voucherID;
            baseTrans.ExecuteUpdate(sql);
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
            WrkFlwApp.RejectVoucherTwoNode1(wrkFlwID, nodeID, sequence, vInfo, baseTrans);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "WrkFlwEntity_backWrkFlw") + "'", true);
            btnBlankOut.Enabled = false;
            //btnCancel.Enabled = false;
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            throw ex;
        }
        baseTrans.Commit();
        page(0);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
    }
    /// <summary>
    /// 同意
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPass_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(Request["voucherID"]);
        //String voucherHints = txtShopName.Text.Trim();
        int sequence = Convert.ToInt32(Request["sequence"]);
        String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
        String voucherMemo = "";

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
        WrkFlwApp.ConfirmVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]), vInfo);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_PalaverYes") + "'", true);

        page(0);
        btnBlankOut.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
    }
    protected void gvCharge_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.page(Convert.ToInt32(Request["voucherID"]));
    }
}
