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

using Base.DB;
using Base.Biz;
using Invoice.InvoiceH;
using BaseInfo.User;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using WorkFlow;
using Base.Page;
using Base.Util;

public partial class Invoice_InvAdj_InvAdjAuditing : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["InvAdjID"] = Request.QueryString["VoucherID"].ToString();
            this.BindInvAdjDetail(ViewState["InvAdjID"].ToString());
            btnMessage.Attributes.Add("onclick", "ShowMessage()");
            if (Request.QueryString["WrkFlwID"] != null)
            {
                HidenWrkID.Value = Request.QueryString["WrkFlwID"].ToString();
            }
            else
            {
                HidenWrkID.Value = Request.Cookies["Info"].Values["wrkFlwID"].ToString();
            }
            if (Request.QueryString["VoucherID"] != null)
            {
                HidenVouchID.Value = Request.QueryString["VoucherID"].ToString();
            }
            else
            {
                HidenVouchID.Value = Request.Cookies["Info"].Values["conID"].ToString();
            }
        }
    }
    /// <summary>
    /// 绑定结算单信息
    /// </summary>
    private void BindInvAdjDetail(string strInvAdjID)
    {
        BaseBO objBaseBo = new BaseBO();
        DataTable dt = new DataTable();
        string strSql = @"select invAdj.Invid,invadjdet.InvAdjDetID,invadjdet.InvDetailID,invadjdet.InvAdjID,(select custshortname from customer where custid=InvoiceHeader.custid) as custshortname,(select custname from customer where custid=InvoiceHeader.custid) as custname,invadjdet.AdjReason,invadjdet.AdjAmt,(select InvoiceDetail.InvPayAmt+InvoiceDetail.InvAdjAmt+InvoiceDetail.InvDiscAmt+InvoiceDetail.InvChgAmt - InvoiceDetail.InvPaidAmt from InvoiceDetail where InvoiceDetail.invdetailid=invadjdet.invdetailid) as ThisPaid,invadjdet.AdjAmt,'' as ChargeTypeName,invadjdet.ErrorSign,'' as InvCode,Contract.ContractCode,InvoiceHeader.InvType,'' as InvActPayAmt,'' as invPaidAmt,InvPeriod, '' as  AdjBackAmt 
from invadjdet   
inner join invadj on invadj.invadjid=invadjdet.invadjid inner join InvoiceHeader on InvoiceHeader.Invid=invAdj.Invid
inner join InvoiceDetail on InvoiceDetail.Invid=invAdj.Invid inner join Contract  on Contract.ContractID=InvoiceHeader.ContractID
where invadjdet.invadjid = " + strInvAdjID + "";
        strSql += " group by invAdj.Invid,invadjdet.InvAdjDetID,invadjdet.InvDetailID,invadjdet.InvAdjID,InvoiceHeader.CustID,invadjdet.AdjReason,invadjdet.AdjAmt,invadjdet.AdjAmt,invadjdet.ErrorSign,Contract.ContractCode,InvoiceHeader.InvType,InvPeriod";
        DataSet ds = objBaseBo.QueryDataSet(strSql);
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        ViewState["spareRow"] = count;
        string strID = "";
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                objBaseBo.WhereClause = "";
                DataSet dsInv = objBaseBo.QueryDataSet("select ChargeTypeID,invPaidAmt,InvActPayAmt from InvoiceDetail where InvDetailID=" + dt.Rows[i]["InvDetailID"] + "");
                DataSet dsCharge = objBaseBo.QueryDataSet("select ChargeTypeName from ChargeType where ChargeTypeID = " + dsInv.Tables[0].Rows[0]["ChargeTypeID"] + "");
                dt.Rows[i]["ChargeTypeName"] = dsCharge.Tables[0].Rows[0]["ChargeTypeName"].ToString();
                DataSet dsContract = objBaseBo.QueryDataSet("select InvCode from InvoiceHeader where Invid=" + dt.Rows[i]["Invid"] + "");
                dt.Rows[i]["InvCode"] = dsContract.Tables[0].Rows[0]["InvCode"].ToString();
                dt.Rows[i]["invPaidAmt"] = dsInv.Tables[0].Rows[0]["invPaidAmt"].ToString();
                dt.Rows[i]["InvActPayAmt"] = dsInv.Tables[0].Rows[0]["InvActPayAmt"].ToString();
                dt.Rows[i]["AdjBackAmt"] = decimal.Parse(dt.Rows[i]["InvActPayAmt"].ToString()) + decimal.Parse(dt.Rows[i]["AdjAmt"].ToString());//调整后金额
                try { dt.Rows[i]["InvType"] = DateTime.Parse(dt.Rows[i]["InvPeriod"].ToString()).Month.ToString(); }//记账月
                catch (Exception e) { }
                strID += dt.Rows[i]["InvAdjDetID"].ToString() + ",";
            }
            ViewState["CustName"] = dt.Rows[0]["custname"].ToString();
            ViewState["ContractCode"] = dt.Rows[0]["ContractCode"].ToString();
        }
        ViewState["InvAdjDetID"] = strID;
        for (int i = count; i < 15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvInvoice.DataSource = dt;
        gvInvoice.DataBind();
    }

    private void Commit()
    {
        BaseTrans baseTrans = new BaseTrans();
        BaseBO objBasebo = new BaseBO();
        if (ViewState["InvAdjDetID"].ToString().TrimEnd(',').TrimStart(',') != "")
            objBasebo.ExecuteUpdate("update invadjdet set errorsign=0 where InvAdjDetID in (" + ViewState["InvAdjDetID"].ToString().TrimEnd(',').TrimStart(',') + ")");//更新错误标记
        baseTrans.BeginTrans();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        int deptID = sessionUser.DeptID;
        int userID = sessionUser.UserID;
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
        int voucherID = Convert.ToInt32(Request.QueryString["VoucherID"]);
        String voucherHints = ViewState["CustName"].ToString();
        String voucherMemo = ViewState["ContractCode"].ToString();

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
        WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, sequence, vInfo, baseTrans);

        baseTrans.Commit();

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
    }

    protected void btnPass_Click(object sender, EventArgs e)
    {
        int flag = 0;
        int row = Convert.ToInt32(ViewState["spareRow"]);
        for (int i = 0; i < row; i++)
        {
            decimal invActPayAmt = Convert.ToDecimal(gvInvoice.Rows[i].Cells[7].Text);  //实际应结金额
            decimal invPaidAmt = Convert.ToDecimal(gvInvoice.Rows[i].Cells[8].Text);    //付款金额
            decimal invAdjAmt = Convert.ToDecimal(gvInvoice.Rows[i].Cells[9].Text);    //调整金额
            if (invActPayAmt != invPaidAmt || invActPayAmt == 0)
            {
                if (invAdjAmt < 0)
                {
                    if (invActPayAmt - invPaidAmt >= Math.Abs(invAdjAmt))
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_invAdjAmtError") + "'", true);
                        return;
                    }
                }
                else
                {
                    flag = 1;
                }
            }
            else
            {
                flag = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_invOver") + "'", true);
                return;
            }
        }
        if (flag == 1)
        {
            Commit();
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            BaseBO objBaseBo = new BaseBO();
            if (ViewState["InvAdjDetID"].ToString().TrimEnd(',').TrimStart(',') != "")
                objBaseBo.ExecuteUpdate("update invadjdet  set errorsign=0 where InvAdjDetID in (" + ViewState["InvAdjDetID"].ToString().TrimEnd(',').TrimStart(',') + ")");
            FindChecked();//记录选择
            if (ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') != "")
                objBaseBo.ExecuteUpdate("update invadjdet  set errorsign=1 where InvAdjDetID in (" + ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') + ")");//更新错误状态

            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;
            int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
            int sequence = Convert.ToInt32(Request.QueryString["Sequence"]);
            int voucherID = Convert.ToInt32(Request.QueryString["VoucherID"]);
            String voucherHints = ViewState["CustName"].ToString();
            String voucherMemo = this.listBoxRemark.Text.Trim();

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.RejectVoucherTwoNode(wrkFlwID, nodeID, sequence, vInfo);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "WrkFlwEntity_backWrkFlw") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("驳回租金公式错误:", ex);
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
        for (int i = 0; i < this.gvInvoice.Rows.Count; i++)
        {
            TextBox txtInvAdjDetID = (TextBox)this.gvInvoice.Rows[i].FindControl("txtInvAdjDetID");
            //TextBox txtErrorSign = (TextBox)this.gvCharge.Rows[i].FindControl("txtErrorSign");
            string strtemp = txtInvAdjDetID.Text.Trim();
            if (((System.Web.UI.WebControls.CheckBox)this.gvInvoice.Rows[i].Cells[0].FindControl("Checkbox")).Checked)
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
        for (int i = 0; i < this.gvInvoice.Rows.Count; i++)
        {
            TextBox txtInvAdjDetID = (TextBox)this.gvInvoice.Rows[i].FindControl("txtInvAdjDetID");
            string strtemp = txtInvAdjDetID.Text.Trim();
            if (strtemp != "")
            {
                if (strHaveSelects.IndexOf("," + strtemp + ",") >= 0)
                {
                    ((System.Web.UI.WebControls.CheckBox)this.gvInvoice.Rows[i].Cells[0].FindControl("Checkbox")).Checked = true;
                }
            }
        }
    }
    protected void gvInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            if (e.Row.Cells[12].Text == "1")
            {
                foreach (TableCell oCell in e.Row.Cells)
                {
                    oCell.Attributes.Add("Class", "Error");
                }
                ((System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Checkbox")).Checked = true;
            }
        }
    }
    protected void gvInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindInvAdjDetail(ViewState["InvAdjID"].ToString());
        SetDataGridSelectRecords(ViewState["checkeds"].ToString());//设置选择项
    }
    protected void btnMessage_Click(object sender, EventArgs e)
    {
        this.BindInvAdjDetail(ViewState["InvAdjID"].ToString());
    }
}
