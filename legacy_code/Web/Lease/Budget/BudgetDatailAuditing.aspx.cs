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


public partial class Lease_Budget_BudgetDatailAuditing : BasePage
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
            baseInfo = "预算审批";
            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
        }
    }
    protected void page(int batchID)
    {


        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        baseBO.WhereClause = "BudgetDetail.BatchID = " + batchID;
        //DataSet ds = baseBO.QueryDataSet(new OtherChargeD());
        BudgetDetail objBudgetDetail = new BudgetDetail();
        string strSql = " SELECT  BudgetDetail.BudgetID,BudgetDetail.BatchID,BudgetDetail.UnitID,BudgetDetail.FloorArea,BudgetDetail.UseArea," +
                        "         BudgetDetail.ShopTypeID,BudgetDetail.TradeID,BudgetDetail.UnitTypeID,BudgetDetail.BudgetYear," +
                        "         Convert(char(10),BudgetDetail.StartDate,120) as StartDate," +
                        "         Convert(char(10),BudgetDetail.EndDate,120) as EndDate," +
                        "         BudgetDetail.ChargeTypeID,BudgetDetail.RentType,BudgetDetail.RentAmt,BudgetDetail.BudgetStatus," +
                        "         BudgetDetail.ModifyUserId,BudgetDetail.ModifyTime,BudgetDetail.OprRoleID,BudgetDetail.OprDeptID," +
                        "         ShopType.ShopTypeName,TradeRelation.TradeName,UnitType.UnitTYpeName,Unit.UnitCode" +
                        " FROM    BudgetDetail INNER JOIN ShopType ON (BudgetDetail.ShopTypeID=ShopType.ShopTypeID) " +
                        "         INNER JOIN TradeRelation ON (BudgetDetail.TradeID=TradeRelation.TradeID) " +
                        "         INNER JOIN UnitType ON (BudgetDetail.UnitTypeID=UnitType.UnitTypeID)" +
                        "         INNER JOIN Unit ON(BudgetDetail.UnitID=Unit.UnitID)";

        objBudgetDetail.SetQuerySql(strSql);
        //BudgetID,BatchID,UnitID,FloorArea,UseArea,ShopTypeID,TradeID,UnitTypeID,BudgetYear,Convert(char(10),StartDate,120) as StartDate,Convert(char(10),EndDate,120) as EndDate,ChargeTypeID,RentType,RentAmt,BudgetStatus,ModifyUserId,ModifyTime,OprRoleID,OprDeptID
        DataSet ds = baseBO.QueryDataSet(objBudgetDetail);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        ViewState["count"] = count;
        for (int i = count; i < 15; i++)
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
            String sql = "Update BudgetDetail set BudgetStatus = " + BudgetDetail.BDGSTATUS_TYPE_REJECT + " where BatchID =" + voucherID;
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
    protected void gvCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowIndex >= 0)
        {
            TextBox txtBudgetID = (TextBox)e.Row.FindControl("txtBudgetID");
            TextBox txtUnitID = (TextBox)e.Row.FindControl("txtUnitID");
            //if (txtBudgetID.Text.Trim() != "")
            //{
            //    ((System.Web.UI.WebControls.CheckBox)e.Row.FindControl("Checkbox")).Checked = true;
            //    ((System.Web.UI.WebControls.CheckBox)e.Row.FindControl("Checkbox")).Enabled = false;
            //    ((System.Web.UI.WebControls.DropDownList)e.Row.FindControl("ddlRentType")).Enabled = true;
            //}
            if (txtUnitID.Text.Trim() == "")
            {

                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUnitCode")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtFloorArea")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUseArea")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtShopTypeName")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtTradeName")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtUnitTypeName")).Visible = false;
                ((System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtRentAmt")).Visible = false;
                ((System.Web.UI.WebControls.DropDownList)e.Row.FindControl("ddlRentType")).Visible = false;
            }

        }
        if (((DropDownList)e.Row.FindControl("ddlRentType")) != null)
        {
            // TextBox txtBudgetID = (TextBox)e.Row.FindControl("txtBudgetID");
            TextBox txtUnitID = (TextBox)e.Row.FindControl("txtUnitID");
            if (txtUnitID.Text.ToString() != "" && txtUnitID.Text.ToString() != null)
            {

                DropDownList ddlRentType = (DropDownList)e.Row.FindControl("ddlRentType");
                ddlRentType.Items.Clear();
                ddlRentType.Items.Add(new ListItem("日", "D"));
                ddlRentType.Items.Add(new ListItem("月", "M"));
                ddlRentType.Items.Add(new ListItem("年", "Y"));
                //DropDownList初始被选择的项
                ddlRentType.SelectedValue = ((HiddenField)e.Row.FindControl("hidRentType")).Value;
            }
        }


    }
}
