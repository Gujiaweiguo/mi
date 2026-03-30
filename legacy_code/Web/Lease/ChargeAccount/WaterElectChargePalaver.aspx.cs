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

public partial class Lease_ChargeAccount_WaterElectChargePalaver : BasePage
{
    public string emptyStr;
    public string baseInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            emptyStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Account_lblWaterElectChargePalaver");
            int chgeId = Convert.ToInt32(Request["voucherID"]);
            page(chgeId);

        }
    }
    protected void page(int chgId)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        //baseBO.WhereClause = "ChgID = " + chgId;
        //DataSet ds = baseBO.QueryDataSet(new ChargeDetail());
        baseBO.WhereClause = "rangecode = " + chgId;
        ChargeDetail objChargeDetail = new ChargeDetail();
        //objChargeDetail.SetQuerySql("select dept.deptname,ChgDetID,HdwID,ChargeDetail.ChgID,ChargeTypeID,HdwID,ChgName,Convert(char(10),StartDate,120) as StartDate,Convert(char(10),EndDate,120) as EndDate,ChargeDetail.RefID,ChgAmt,LastQty,CurQty,CostQty,Times,FreeQty,Price,'' as HdwCode,'' as ChargeTypeName,ChgPeriod,charge.ShopID,(select shopname from conshop where conshop.shopid=charge.shopid) as ShopName from  ChargeDetail left join charge on charge.ChgID=ChargeDetail.ChgID left join conshop on conshop.shopid=charge.shopid left join dept on dept.deptid=conshop.storeid");
        objChargeDetail.SetQuerySql("select chargedetail.ChgDetID,chargedetail.HdwID,ChargeDetail.ChgID,ChargeTypeID,HdwID,ChgName,Convert(char(10),StartDate,120) as StartDate,Convert(char(10),EndDate,120) as EndDate,ChargeDetail.RefID,ChgAmt,LastQty,CurQty,CostQty,Times,FreeQty,Price,'' as HdwCode,'' as ChargeTypeName,ChgPeriod,charge.ShopID,(select shopcode+'  '+shopname from conshop where conshop.shopid=charge.shopid) as ShopName,ChargeDetail.ErrorSign from  ChargeDetail left join charge on charge.ChgID=ChargeDetail.ChgID left join conshop on conshop.shopid=charge.shopid ");
        DataSet ds = baseBO.QueryDataSet(objChargeDetail);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        ViewState["count"] = count;
        string strID = "";
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                baseBO.WhereClause = "";
                baseBO.WhereClause = "ChargeTypeID = " + Convert.ToInt32(dt.Rows[i]["ChargeTypeID"]);
                DataSet tempDs = baseBO.QueryDataSet(new ChargeType());
                dt.Rows[i]["ChargeTypeName"] = tempDs.Tables[0].Rows[0]["ChargeTypeName"].ToString();

                baseBO.WhereClause = "";
                baseBO.WhereClause = "ShopHdw.ShopID = " + Convert.ToInt32(dt.Rows[i]["ShopID"]);
                DataSet tempHDs = baseBO.QueryDataSet(new ShopHdw());
                dt.Rows[i]["HdwCode"] = tempHDs.Tables[0].Rows[0]["HdwName"].ToString();
                strID += dt.Rows[i]["ChgDetID"].ToString() + ",";
            }
        }
        ViewState["ChgDetID"] = strID;
        for (int i = count; i < 15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvCharge.DataSource = dt;
        gvCharge.DataBind();
    }
    /// <summary>
    /// 审批
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPass_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        if (ViewState["ChgDetID"].ToString().TrimEnd(',').TrimStart(',') != "")
            objBaseBo.ExecuteUpdate("update chargedetail set errorsign=0 where chgdetid in (" + ViewState["ChgDetID"].ToString().TrimEnd(',').TrimStart(',') + ")");//更新错误标记
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(Request["voucherID"]);
        int sequence = Convert.ToInt32(Request["sequence"]);
        String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
        String voucherMemo = this.listBoxRemark.Text.Trim();

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
        WrkFlwApp.ConfirmVoucher(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"]), vInfo);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_PalaverYes") + "'", true);
        page(0);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        this.listBoxRemark.Text = "";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
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
            if (ViewState["ChgDetID"].ToString().TrimEnd(',').TrimStart(',') != "")
                baseTrans.ExecuteUpdate("update chargedetail set errorsign=0 where chgdetid in (" + ViewState["ChgDetID"].ToString().TrimEnd(',').TrimStart(',') + ")");
            FindChecked();//记录选择
            if (ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') != "")
                baseTrans.ExecuteUpdate("update chargedetail set errorsign=1 where chgdetid in (" + ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') + ")");//更新错误状态

            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int voucherID = Convert.ToInt32(Request["voucherID"]);
            int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);

            String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
            String voucherMemo = listBoxRemark.Text;

            //修改费用单状态为草稿状态
            String sql = "Update Charge set ChgStatus = " + Charge.CHGSTATUS_TYPE_TEMP + " where RangeCode =" + voucherID;
            baseTrans.ExecuteUpdate(sql);

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
            WrkFlwApp.RejectVoucherTwoNode(wrkFlwID, nodeID, sequence, vInfo);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "WrkFlwEntity_backWrkFlw") + "'", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            throw ex;
        }
        baseTrans.Commit();
        page(0);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
    }
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvCharge_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        FindChecked();//记录选择
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
        SetDataGridSelectRecords(ViewState["checkeds"].ToString());//设置选择项
    }
    protected void gvCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            if (e.Row.Cells[18].Text == "1")
            {
                foreach (TableCell oCell in e.Row.Cells)
                {
                    oCell.Attributes.Add("Class", "Error");
                }
                ((System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Checkbox")).Checked = true;
            }
            //if (e.Row.Cells[18].Text.Trim() == "&nbsp;")
            //{
            //    e.Row.Cells[0].Text = " ";
            //}
        }
    }
    /// <summary>
    /// 记录表中选中记录的情况
    /// </summary>
    /// <returns></returns>
    private void FindChecked()
    {
        string checkeds = "";
        if (ViewState["checkeds"] != null)
            checkeds = "," + ViewState["checkeds"].ToString() + ",";
        for (int i = 0; i < this.gvCharge.Rows.Count; i++)
        {
            TextBox txtChgDetID = (TextBox)this.gvCharge.Rows[i].FindControl("txtChgDetID");
            TextBox txtErrorSign = (TextBox)this.gvCharge.Rows[i].FindControl("txtErrorSign");
            string strtemp = txtChgDetID.Text.Trim();
            if (((System.Web.UI.WebControls.CheckBox)this.gvCharge.Rows[i].Cells[0].FindControl("Checkbox")).Checked)
            {
                if (checkeds.IndexOf("," + strtemp + ",") < 0)
                {
                    checkeds += strtemp + ",";
                }
            }
            else
            {
                //如果没选中则在串中去掉
                checkeds = checkeds.Replace("," + strtemp + ",", ",");
            }
        }
        checkeds = checkeds.TrimEnd(',').TrimStart(',');
        ViewState["checkeds"] = checkeds;
    }
    /// <summary>
    /// 记录每页选中的情况
    /// </summary>
    /// <param name="strHaveSelects"></param>
    public void SetDataGridSelectRecords(string strHaveSelects)
    {
        strHaveSelects = "," + strHaveSelects.TrimEnd(',').TrimStart(',') + ",";
        for (int i = 0; i < this.gvCharge.Rows.Count; i++)
        {
            TextBox txtChgDetID = (TextBox)this.gvCharge.Rows[i].FindControl("txtChgDetID");
            string strtemp = txtChgDetID.Text.Trim();
            if (strtemp != "")
            {
                if (strHaveSelects.IndexOf("," + strtemp + ",") >= 0)
                {
                    ((System.Web.UI.WebControls.CheckBox)this.gvCharge.Rows[i].Cells[0].FindControl("Checkbox")).Checked = true;
                }
            }
        }
    }
}
