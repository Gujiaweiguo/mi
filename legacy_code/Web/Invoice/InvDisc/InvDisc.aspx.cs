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
using System.Text;

using Base.DB;
using Base.Biz;
using RentableArea;
using Invoice.InvoiceH;
using Base;
using BaseInfo.User;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using Base.Page;
using Base.Util;
public partial class Invoice_InvDisc_InvDisc :BasePage
{
    public string billOfDocumentDelete;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            /*大楼内码*/
            baseBO.WhereClause = "BuildingStatus=" + Building.BUILDING_STATUS_VALID;
            rs = baseBO.Query(new Building());
            cmbBuildingID.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Select_Select")));
            foreach (Building buildings in rs)
            {
                cmbBuildingID.Items.Add(new ListItem(buildings.BuildingName, buildings.BuildingID.ToString()));
            }

            /*楼层*/
            cmbFloorID.Items.Clear();
            cmbFloorID.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Select_Select")));


            /*经营类别*/
            baseBO.WhereClause = "TradeLevel = '" + TradeRelation.TRADELEVEL_STATUS_TWO + "'";
            rs = baseBO.Query(new TradeRelation());
            cmbTradeID.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Select_Select")));
            foreach (TradeRelation tradeDef in rs)
            {
                cmbTradeID.Items.Add(new ListItem(tradeDef.TradeName, tradeDef.TradeID.ToString()));
            }

            /*费用类别*/
            baseBO.WhereClause = "";
            rs = baseBO.Query(new ChargeType());
            cmbChargeType.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Select_Select")));
            foreach (ChargeType chargeType in rs)
            {
                cmbChargeType.Items.Add(new ListItem(chargeType.ChargeTypeName, chargeType.ChargeTypeID.ToString()));
            }
            ViewState["Page"] = "b.ChargeTypeID=0";
            page("b.ChargeTypeID=0");
            clearGridViewRow();
            btnBlankOut.Enabled = true;

            btnBlankOut.Attributes.Add("onclick", "return BillOfDocumentDelete()");
            billOfDocumentDelete = (String)GetGlobalResourceObject("BaseInfo", "PublicMes_BillOfDocumentDelete");

            if (Request.QueryString["Type"] == "Old")
            {
                HttpCookie cookiesCustumer = new HttpCookie("Custumer");

                cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
                cookiesCustumer.Values.Add("CustumerID", Request.QueryString["VoucherID"]);

                Response.AppendCookie(cookiesCustumer);
            }

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "PublicMessage", "hidden()", true);
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        StringBuilder stringBuilder = new StringBuilder();
        /*大楼*/
        if (cmbBuildingID.SelectedIndex != 0)
        {
            stringBuilder.Append(" e.BuildingID=" + Convert.ToInt32(cmbBuildingID.SelectedValue));
        }

        /*楼层*/
        if (cmbFloorID.SelectedIndex != 0)
        {
            if (stringBuilder.ToString() != null && stringBuilder.ToString().Length > 0)
            {
                stringBuilder.Append(" and e.FloorID=" + Convert.ToInt32(cmbFloorID.SelectedValue));
            }
            else
            {
                stringBuilder.Append(" e.FloorID=" + Convert.ToInt32(cmbFloorID.SelectedValue));
            }
        }

        /*经营类别*/
        if (cmbTradeID.SelectedIndex != 0)
        {
            if (stringBuilder.ToString() != null && stringBuilder.ToString().Length > 0)
            {
                stringBuilder.Append(" and d.TradeID=" + Convert.ToInt32(cmbTradeID.SelectedValue));
            }
            else
            {
                stringBuilder.Append(" d.TradeID=" + Convert.ToInt32(cmbTradeID.SelectedValue));
            }
        }

        /*账单记账月*/
        if (txtInvPeriod.Text.Trim().Length > 0)
        {
            if (stringBuilder.ToString() != null && stringBuilder.ToString().Length > 0)
            {
                stringBuilder.Append(" and InvPeriod= '" + Convert.ToDateTime(txtInvPeriod.Text) + "'");
            }
            else
            {
                stringBuilder.Append(" InvPeriod='" + Convert.ToDateTime(txtInvPeriod.Text) + "'");
            }
        }

        /*费用类别*/
        if (cmbChargeType.SelectedIndex != 0)
        {
            if (stringBuilder.ToString() != null && stringBuilder.ToString().Length > 0)
            {
                stringBuilder.Append(" and b.ChargeTypeID=" + Convert.ToInt32(cmbChargeType.SelectedValue));
            }
            else
            {
                stringBuilder.Append(" b.ChargeTypeID=" + Convert.ToInt32(cmbChargeType.SelectedValue));
            }
        }

        if (stringBuilder.Length > 0)
        {
            stringBuilder.Append(" And InvStatus= " + InvoiceHeader.INVOICEHEADER_AVAILABILITY);
        }
        else
        {
            stringBuilder.Append("InvStatus= " + InvoiceHeader.INVOICEHEADER_AVAILABILITY);
        }

        ViewState["Page"] = stringBuilder.ToString();
        page(stringBuilder.ToString());
        clearGridViewRow();
        //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "hidden();", true);
    }
    protected void butAuditing_Click(object sender, EventArgs e)
    {
        if (txtDiscRate.Text.Trim().Length != 0)
        {
            foreach (GridViewRow gvr in GrdVewInvoiceDetail.Rows)
            {
                if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                {
                    decimal invActPayAmt = Convert.ToDecimal(gvr.Cells[4].Text);
                    decimal invPaidAmt = Convert.ToDecimal(gvr.Cells[5].Text);
                    decimal invDiscAmt = Convert.ToDecimal(txtDiscRate.Text) / 100 * invActPayAmt;
                    if (invActPayAmt - invPaidAmt < invDiscAmt)
                    {
                        clearGridViewRow();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_invAdjAmtError") + "'", true);
                        return;
                    }
                }
            }

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            BaseTrans baseTrans = new BaseTrans();
            InvDiscDet invDiscDet = new InvDiscDet();
            InvDisc invDisc = new InvDisc();
            int j = 0;
            baseTrans.BeginTrans();
            invDisc.DiscID = BaseApp.GetDiscID();

            if (txtDiscRate.Text.Trim().Length == 0)
            {
                clearGridViewRow();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "InvAdjDisc_DiscRateNotNull") + "'", true);
                return;
            }

            decimal TotalDiscAmt = 0;
            decimal TotalDiscAmtL = 0;

            foreach (GridViewRow gvr in GrdVewInvoiceDetail.Rows)
            {
                if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                {
                    invDiscDet.DiscDetID = BaseApp.GetDiscDetID();
                    invDiscDet.DiscID = invDisc.DiscID;
                    invDiscDet.InvDetailID = Convert.ToInt32(gvr.Cells[0].Text);
                    invDiscDet.DiscReason = txtDiscReason.Text.Trim();

                    if (txtDiscRate.Text.Trim().Length > 0)
                    {
                        invDiscDet.DiscType = InvDiscDet.INVDISCDET_DISCRATE;
                        invDiscDet.DiscRate = Convert.ToDecimal(txtDiscRate.Text) / 100;
                        invDiscDet.DiscAmt = 0 - Convert.ToDecimal(gvr.Cells[4].Text) * invDiscDet.DiscRate;
                        invDiscDet.DiscAmtL = invDiscDet.DiscAmt * Convert.ToDecimal(gvr.Cells[7].Text);

                        TotalDiscAmt += invDiscDet.DiscAmt;
                        TotalDiscAmtL += invDiscDet.DiscAmtL;
                    }


                    //if (txtDiscAmt.Text.Trim().Length > 0)
                    //{
                    //    invDiscDet.DiscType = InvDiscDet.INVDISCDET_DISCAMT;
                    //    invDiscDet.DiscAmt = Convert.ToDecimal(txtDiscAmt.Text);
                    //    invDiscDet.DiscAmtL = Convert.ToDecimal(txtDiscAmt.Text);
                    //}
                    if (baseTrans.Insert(invDiscDet) < 1)
                    {
                        baseTrans.Rollback();
                        return;
                    } j++;
                }
            }

            if (j < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "InvAdjDise_SelInvfullInfo") + "'", true);
                return;
            }

            invDisc.CreateUserID = sessionUser.UserID;
            invDisc.CreateTime = DateTime.Now;
            invDisc.ModifyUserID = sessionUser.UserID;
            invDisc.ModifyTime = DateTime.Now;
            invDisc.OprDeptID = sessionUser.DeptID;
            invDisc.OprRoleID = sessionUser.RoleID;
            invDisc.DiscDate = DateTime.Now;
            invDisc.DiscOpr = sessionUser.DeptID;
            invDisc.DiscReason = txtDiscReason.Text.Trim();
            invDisc.DiscStatus = InvDisc.INVDISC_YES_PUT_IN_NO_UPDATE_LEASE_STATUS;
            invDisc.DiscAmt = TotalDiscAmt;
            invDisc.DiscAmtL = TotalDiscAmtL;

            if (baseTrans.Insert(invDisc) < 1)
            {
                baseTrans.Rollback();
                return;
            }


            /*提交审批*/
            int voucherID = 0;
            voucherID = Convert.ToInt32(invDisc.DiscID);
            String voucherHints = DateTime.Now.ToString();
            String voucherMemo = txtDiscReason.Text.Trim();
            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);
            WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo, baseTrans);
            baseTrans.Commit();

            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        clearPage();
    }
    protected void cmbBuildingID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        /*楼层*/
        if (cmbBuildingID.SelectedIndex != 0)
        {
            cmbFloorID.Items.Clear();
            cmbFloorID.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Select_Select")));
            baseBO.WhereClause = "FloorStatus=" + Floors.FLOOR_STATUS_VALID + "and BuildingID = " + Convert.ToInt32(cmbBuildingID.SelectedValue);
            rs = baseBO.Query(new Floors());
            foreach (Floors floors in rs)
            {
                cmbFloorID.Items.Add(new ListItem(floors.FloorName, floors.FloorID.ToString()));
            }
            cmbFloorID.Enabled = true;
        }
        else
        {
            cmbFloorID.Enabled = false;
            cmbFloorID.Items.Clear();
            cmbFloorID.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "Select_Select")));
        }
        clearGridViewRow();
    }

    protected void page(string stringBuilder)
    {

        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        baseBO.WhereClause = stringBuilder;
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;

        DataTable dt = baseBO.QueryDataSet(new InvDiscSel()).Tables[0];

        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    dt.Rows[i]["InvPayAmt"] = Convert.ToDecimal(dt.Rows[i]["InvPayAmt"]) + Convert.ToDecimal(dt.Rows[i]["invAdjAmt"]) + Convert.ToDecimal(dt.Rows[i]["InvDiscAmt"]);
        //}

        pds.DataSource = dt.DefaultView;

        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdVewInvoiceDetail.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdVewInvoiceDetail.DataSource = pds;
            GrdVewInvoiceDetail.DataBind();
            btnBack.Enabled = false;
            btnNext.Enabled = false;
            //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "divshow", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidNoData") + "';", true);
        }
        else
        {
            pds.AllowPaging = true;
            pds.PageSize = 9;
            lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
            pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;

            if (pds.IsFirstPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = true;
            }

            if (pds.IsLastPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = false;
            }

            if (pds.IsFirstPage && pds.IsLastPage)
            {
                btnBack.Enabled = false;
                btnNext.Enabled = false;
            }

            if (!pds.IsLastPage && !pds.IsFirstPage)
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
            }

            this.GrdVewInvoiceDetail.DataSource = pds;
            this.GrdVewInvoiceDetail.DataBind();
            spareRow = GrdVewInvoiceDetail.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GrdVewInvoiceDetail.DataSource = pds;
            GrdVewInvoiceDetail.DataBind();
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        page(ViewState["Page"].ToString());
        clearGridViewRow();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        page(ViewState["Page"].ToString());
        clearGridViewRow();
    }
    protected void GrdVewInvoiceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GrdVewInvoiceDetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        InvDiscSel invDiscSel = new InvDiscSel();

        baseBO.WhereClause = "InvDetailID=" + Convert.ToInt32(GrdVewInvoiceDetail.SelectedRow.Cells[0].Text);
        rs = baseBO.Query(invDiscSel);
        if (rs.Count == 1)
        {
            invDiscSel = rs.Dequeue() as InvDiscSel;

            txtCustCode.Text = invDiscSel.CustCode;
            txtCustName.Text = invDiscSel.CustName;
            txtContractID.Text = invDiscSel.ContractCode;
            txtShopName.Text = invDiscSel.ShopName;
        }
        clearGridViewRow();
    }

    protected void clearGridViewRow()
    {
        foreach (GridViewRow gvr in GrdVewInvoiceDetail.Rows)
        {
            if (gvr.Cells[2].Text == "&nbsp;")
            {
                gvr.Cells[1].Text = "";
                gvr.Cells[6].Text = "";
            }
        }
    }

    protected void clearPage()
    {
        page("b.ChargeTypeID=0");
        txtCustCode.Text = "";
        txtCustName.Text = "";
        txtContractID.Text = "";
        txtShopName.Text = "";
        txtDiscRate.Text = "";
        txtDiscReason.Text = "";
        cmbBuildingID.SelectedIndex = 0;
        cmbFloorID.SelectedIndex = 0;
        cmbFloorID.Enabled = false;
        cmbTradeID.SelectedIndex = 0;
        txtInvPeriod.Text = "";
        cmbChargeType.SelectedIndex = 0;
        clearGridViewRow();
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < this.GrdVewInvoiceDetail.Rows.Count; i++)
        {
            ((CheckBox)GrdVewInvoiceDetail.Rows[i].FindControl("chkSelect")).Checked = CheckBox1.Checked;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    protected void btnBalnkOut_Click(object sender, EventArgs e)
    {
        BaseTrans baseTrans = new BaseTrans();
        try
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;
            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
            int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            int voucherID = Convert.ToInt32(Request.QueryString["VoucherID"]);
            String voucherHints = txtCustName.Text.Trim();
            String voucherMemo = txtCustName.Text;

            baseTrans.BeginTrans();
            baseTrans.ExecuteUpdate("update InvAdj set AdjStatus = " + InvDisc.INVDISC_STATUS_OUT + " where InvAdjID = " + voucherID);

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, sequence, vInfo);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            
            clearPage();
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("租赁合同作废审批信息错误:", ex);
        }
        baseTrans.Commit();

    }
}
