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
using System.Collections.Generic;
using System.Drawing;

using Base.DB;
using Base.Biz;
using Invoice.InvoiceH;
using Lease.Customer;
using Base;
using BaseInfo.User;
using WorkFlow;
using WorkFlow.WrkFlw;
using WorkFlow.Uiltil;
using Base.Page;
using Base.Util;

public partial class Invoice_InvAdj_InvAdj : BasePage
{
    private static int DISPROVE_UP = 1;
    private static int DISPROVE_IN = 2;
    /// <summary>
    /// 用于绑定的表
    /// </summary>
    protected DataTable InvDetailDT
    {
        set
        {
            ViewState["Sour"] = value;
        }
        get
        {
            return (DataTable)ViewState["Sour"];
        }
    }
    protected DataTable SaveDetailDT
    {
        set
        {
            ViewState["SaveSour"] = value;
        }
        get
        {
            return (DataTable)ViewState["SaveSour"];
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            btnMessage.Attributes.Add("onclick", "ShowMessage()");
            ViewState["strWhere"] = "";
            InitInvDetailDT();
            if (Request.QueryString["VoucherID"] != null)
            {
                BaseBO baseBO = new BaseBO();

                DataTable dt = baseBO.QueryDataSet("Select InvID From InvAdj Where InvAdjID=" + Convert.ToInt32(Request.QueryString["VoucherID"])).Tables[0];

                if (dt.Rows.Count == 1)
                {
                    ViewState["InvID"] = Convert.ToInt32(dt.Rows[0]["InvID"]);
                    ViewState["AdjStatus"] = InvAdj.INVADJ_YES_PUT_IN_NO_UPDATE_LEASE_STATUS;

                    ViewState["Flag"] = DISPROVE_UP;
                    ViewState["SequenceID"] = Convert.ToInt32(Request.QueryString["Sequence"]);
                    ViewState["VoucherID"] = Convert.ToInt32(Request.QueryString["VoucherID"]);
                    BindInvAdjDetail(ViewState["VoucherID"].ToString());//绑定结算单明细
                    this.BindCust(" and 1=2 ");//绑定客户信息

                    //DataSet ds = baseBO.QueryDataSet("select InvoiceHeader.InvExRate,InvoiceHeader.CustName,Customer.CustShortName from InvoiceHeader inner join invadj on InvoiceHeader.InvId = invadj.InvId inner join Customer on Customer.custid = InvoiceHeader.custid where invadjid=" + Convert.ToInt32(Request.QueryString["VoucherID"]) + "");
                    DataSet ds = baseBO.QueryDataSet("select InvoiceHeader.InvExRate,InvoiceHeader.CustName,Customer.CustShortName,InvoiceHeader.InvCode,contract.ContractCode,customer.CustCode,month(InvPeriod) as InvPeriod from InvoiceHeader inner join invadj on InvoiceHeader.InvId = invadj.InvId inner join Customer on Customer.custid = InvoiceHeader.custid inner join contract on contract.contractid=InvoiceHeader.contractid  where invadjid=" + Convert.ToInt32(Request.QueryString["VoucherID"]) + "");
                    ViewState["InvExRate"] = ds.Tables[0].Rows[0]["InvExRate"].ToString();
                    ViewState["CustName"] = ds.Tables[0].Rows[0]["CustName"].ToString();
                    ViewState["CustShortName"] = ds.Tables[0].Rows[0]["CustShortName"].ToString();
                    this.txtCustCode.Text = ds.Tables[0].Rows[0]["CustCode"].ToString();
                    this.txtInvCode.Text = ds.Tables[0].Rows[0]["InvCode"].ToString();
                    this.txtInvPeriod.Text = ds.Tables[0].Rows[0]["InvPeriod"].ToString();
                    this.txtContractCode.Text = ds.Tables[0].Rows[0]["ContractCode"].ToString();
                }
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
                this.btnBlankOut.Enabled = true;
                this.btnPutIn.Enabled = true;
                this.btnMessage.Enabled = true;
            }
            else
            {
                BindInvAdjDetail("0");//绑定结算单明细
                this.BindCust(" and 1=2 ");//绑定客户信息
            }
        }
    }
    /// <summary>
    /// 添加表中的列
    /// </summary>
    protected void InitInvDetailDT()
    {
        InvDetailDT = new DataTable();
        InvDetailDT.Columns.Add("InvAdjDetID");
        InvDetailDT.Columns.Add("InvDetailID");
        InvDetailDT.Columns.Add("InvAdjID");
        InvDetailDT.Columns.Add("AdjAmt");
        InvDetailDT.Columns.Add("AdjAmtl");
        InvDetailDT.Columns.Add("AdjReason");
        InvDetailDT.Columns.Add("ChargeTypeID");
        InvDetailDT.Columns.Add("ChargeTypeName");//付款类别名称
        InvDetailDT.Columns.Add("CustName");//客户名称
        InvDetailDT.Columns.Add("CustShortName");//客户简称
        InvDetailDT.Columns.Add("ThisPaid");//费用金额
        InvDetailDT.Columns.Add("InvPaidAmt");//已付金额
        InvDetailDT.Columns.Add("ErrorSign");//错误标记
        InvDetailDT.Columns.Add("AdjBackAmt");//调整后金额
        SaveDetailDT = InvDetailDT.Clone();
    }
    /// <summary>
    /// 绑定结算单信息
    /// </summary>
    private void BindInvAdjDetail(string strInvAdjID)
    {
        #region 屏蔽程序
        //BaseBO objBaseBo = new BaseBO();
        //DataTable dt = new DataTable();
        //objBaseBo.WhereClause = "b.InvID=" + strInvID;
        //string strSql = @"select d.InvDetailID,a.InvAdjID,b.Invid,c.InvAdjDetID,AdjDate,a.AdjAmt as InvAdjAmt,c.AdjAmt,a.AdjAmtL as InvAdjAmtL,c.AdjAmtL,AdjOpr,c.AdjReason,InvCode,ChargeTypeName,ContractCode,Period,CustName,(select custshortname from customer where custid=b.custid) as CustShortName,d.InvPayAmt,d.InvDetailID,'' as UserName,d.InvActPayAmt,d.InvPaidAmt,'' as ThisPaid from InvAdj a left join InvoiceHeader b on a.InvID=b.InvID left join InvAdjDet c on a.InvAdjID=c.InvAdjID left join  InvoiceDetail d on c.InvDetailID=d.InvDetailID left join  ChargeType e on d.ChargeTypeID=e.ChargeTypeID left join Contract f on b.ContractID = f.ContractID where b.InvID=" + strInvID;
        ////DataSet ds = objBaseBo.QueryDataSet(new InvAdjDetIns());
        //DataSet ds = objBaseBo.QueryDataSet(strSql);
        //dt = ds.Tables[0];
        //if (dt.Rows.Count > 0) ViewState["flag"] = DISPROVE_UP;
        //if (strInvID != "0") InvDetailDT = dt;
        //int count = dt.Rows.Count;
        //for (int i = count; i < 15; i++)
        //{
        //    dt.Rows.Add(dt.NewRow());
        //}
        //gvInvoice.DataSource = dt;
        //gvInvoice.DataBind();
        #endregion
        BaseBO objBaseBo = new BaseBO();
        DataTable dt = new DataTable();
        string strSql = @"select invadjdet.InvAdjDetID,invadjdet.InvDetailID,invadjdet.InvAdjID,(select custshortname from customer where custid=InvoiceHeader.custid) as custshortname,(select custname from customer where custid=InvoiceHeader.custid) as custname,invadjdet.AdjReason,invadjdet.AdjAmt,(select InvoiceDetail.InvPayAmt+InvoiceDetail.InvAdjAmt+InvoiceDetail.InvDiscAmt+InvoiceDetail.InvChgAmt - InvoiceDetail.InvPaidAmt from InvoiceDetail where InvoiceDetail.invdetailid=invadjdet.invdetailid) as ThisPaid,invadjdet.AdjAmt,(select InvoiceDetail.InvPaidAmt from InvoiceDetail where InvoiceDetail.invdetailid=invadjdet.invdetailid) as  InvPaidAmt,'' as ChargeTypeName,ErrorSign,(case invadjdet.AdjAmt when null then '' else ((select InvoiceDetail.InvPayAmt+InvoiceDetail.InvAdjAmt+InvoiceDetail.InvDiscAmt+InvoiceDetail.InvChgAmt - InvoiceDetail.InvPaidAmt from InvoiceDetail where InvoiceDetail.invdetailid=invadjdet.invdetailid)+invadjdet.AdjAmt) end ) as AdjBackAmt 
from invadjdet   
inner join invadj on invadj.invadjid=invadjdet.invadjid inner join InvoiceHeader on InvoiceHeader.Invid=invAdj.Invid
inner join InvoiceDetail on InvoiceDetail.Invid=invAdj.Invid 
where invadjdet.invadjid = " + strInvAdjID + "";
        strSql += " group by invAdj.Invid,invadjdet.InvAdjDetID,invadjdet.InvDetailID,invadjdet.InvAdjID,InvoiceHeader.CustID,invadjdet.AdjReason,invadjdet.AdjAmt,invadjdet.AdjAmt,ErrorSign";
        DataSet ds = objBaseBo.QueryDataSet(strSql);
        dt = ds.Tables[0];
        if (dt.Rows.Count > 0) ViewState["flag"] = DISPROVE_UP;
        if (strInvAdjID != "0") InvDetailDT = dt;
        int count = dt.Rows.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                objBaseBo.WhereClause = "";
                //objBaseBo.WhereClause = "InvDetailID=" + Convert.ToInt32(dt.Rows[i]["InvDetailID"]);
                DataSet dsInv = objBaseBo.QueryDataSet("select ChargeTypeID from InvoiceDetail where InvDetailID=" + dt.Rows[i]["InvDetailID"] + "");
                DataSet dsCharge = objBaseBo.QueryDataSet("select ChargeTypeName from ChargeType where ChargeTypeID = " + dsInv.Tables[0].Rows[0]["ChargeTypeID"] + "");
                dt.Rows[i]["ChargeTypeName"] = dsCharge.Tables[0].Rows[0]["ChargeTypeName"].ToString();
            }
        }
        for (int i = count; i < 15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvInvoice.DataSource = dt;
        gvInvoice.DataBind();
    }

    /// <summary>
    /// 绑定客户信息
    /// </summary>
    private void BindCust(string strWhere)
    {
        BaseBO objBaseBo = new BaseBO();
        DataTable dt = new DataTable();
        objBaseBo.WhereClause = " 1=1 "+strWhere;
        DataSet ds = objBaseBo.QueryDataSet(new InvoiceHeader());
        dt = ds.Tables[0];
        int count = dt.Rows.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            { 
                objBaseBo.WhereClause ="";
                objBaseBo.WhereClause = "custid=" + Convert.ToInt32(dt.Rows[i]["custid"]);
                DataSet dsCust = objBaseBo.QueryDataSet(new Customer());
                dt.Rows[i]["CustName"] = dsCust.Tables[0].Rows[0]["CustName"].ToString();
                dt.Rows[i]["CustShortName"] = dsCust.Tables[0].Rows[0]["CustShortName"].ToString();
                try { dt.Rows[i]["InvType"] = DateTime.Parse(dt.Rows[i]["InvPeriod"].ToString()).Month.ToString(); }//记账月
                catch(Exception e) { }
            }
        }
        for (int i = (count%14); i < 14; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvCust.DataSource = dt;
        gvCust.DataBind();
    }
    /// <summary>
    /// 记录每页选中的情况
    /// </summary>
    /// <param name="strHaveSelects"></param>
    public void SetCustSelectRecords(string strHaveSelects)
    {
        strHaveSelects = "," + strHaveSelects.TrimEnd(',').TrimStart(',') + ",";
        for (int i = 0; i < this.gvInvoice.Rows.Count; i++)
        {
            TextBox txtInvDetailID = (TextBox)this.gvInvoice.Rows[i].FindControl("txtInvDetailID");
            string strtemp = txtInvDetailID.Text.Trim();
            if (strtemp != "")
            {
                if (strHaveSelects.IndexOf("," + strtemp + ",") >= 0)
                {
                    ((System.Web.UI.WebControls.CheckBox)this.gvInvoice.Rows[i].Cells[0].FindControl("Checkbox")).Checked = true;
                }
            }
        }
    }
    /// <summary>
    /// 记录表中选中记录的情况
    /// </summary>
    /// <returns></returns>
    private void FindCustChecked()
    {
        string checkeds = "";
        string strShopChecks = "";
        if (ViewState["checkeds"] != null)
            checkeds = "," + ViewState["checkeds"].ToString() + ",";
        for (int i = 0; i < this.gvInvoice.Rows.Count; i++)
        {
            TextBox txtInvDetailID = (TextBox)this.gvInvoice.Rows[i].FindControl("txtInvDetailID");
            TextBox txtAdjReason = (TextBox)this.gvInvoice.Rows[i].FindControl("txtAdjReason");
            TextBox txtAdjAmt = (TextBox)this.gvInvoice.Rows[i].FindControl("txtAdjAmt");
            TextBox txtAdjBackAmt = (TextBox)this.gvInvoice.Rows[i].FindControl("txtAdjBackAmt");//调整后金额

            string strInvDetailID = txtInvDetailID.Text.Trim();
            if (((System.Web.UI.WebControls.CheckBox)this.gvInvoice.Rows[i].Cells[0].FindControl("Checkbox")).Checked)
            {
                if (checkeds.IndexOf("," + strInvDetailID + ",") < 0)
                {
                    checkeds += strInvDetailID + ",";
                    for (int j = 0; j < InvDetailDT.Rows.Count; j++)
                    {
                        if (InvDetailDT.Rows[j]["InvDetailID"].ToString() == strInvDetailID)
                        {
                            InvDetailDT.Rows[j]["AdjReason"] = txtAdjReason.Text.Trim();//调整原因
                            InvDetailDT.Rows[j]["AdjAmt"] = txtAdjAmt.Text.Trim();
                            InvDetailDT.Rows[j]["AdjBackAmt"] = txtAdjBackAmt.Text.Trim();//调整后金额
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < InvDetailDT.Rows.Count; j++)
                    {
                        if (InvDetailDT.Rows[j]["InvDetailID"].ToString() == strInvDetailID)
                        {
                            InvDetailDT.Rows[j]["AdjReason"] = txtAdjReason.Text.Trim();//调整原因
                            InvDetailDT.Rows[j]["AdjAmt"] = txtAdjAmt.Text.Trim();//调整金额
                            InvDetailDT.Rows[j]["AdjBackAmt"] = txtAdjBackAmt.Text.Trim();//调整后金额
                        }
                    }
                }
            }
            else
            {
                //如果没选中则在串中去掉
                checkeds = checkeds.Replace("," + strInvDetailID + ",", ",");
            }
        }
        checkeds = checkeds.TrimEnd(',').TrimStart(',');
        ViewState["checkeds"] = checkeds;
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        ViewState["strWhere"] = "";
        string strWhere = "";
        if (this.txtCustCode.Text.Trim() != "")
        {
            strWhere += " and b.CustCode='"+this.txtCustCode.Text.Trim()+"'";
        }
        if (this.txtInvCode.Text.Trim() != "")
        {
            strWhere += " and a.InvCode='" + this.txtInvCode.Text.Trim() + "'";
        }
        if(this.txtContractCode.Text.Trim()!="")
        {
            strWhere+=" and ContractCode='"+this.txtContractCode.Text.Trim()+"'";
        }
        if(this.txtInvPeriod.Text.Trim()!="")
        {
            strWhere += " and month(InvPeriod)='" + this.txtInvPeriod.Text.Trim() + "'";
        }
        ViewState["strWhere"] = strWhere;
        this.BindCust(strWhere);
    }
    protected void gvCust_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindCust(ViewState["strWhere"].ToString());
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SaveDetailDT.Rows.Clear();
        InvDetailDT.Rows.Clear();
        if (ViewState["InvID"] == null || ViewState["InvID"].ToString() == "")
        {
            return;
        }
        BaseBO objBaseBo = new BaseBO();

        string strSql = @"select customer.CustName,customer.custshortname,Invoiceheader.custid,a.InvDetailID,a.ChargeTypeID,a.InvID,Period,InvStartDate,InvEndDate,a.InvCurTypeID,a.InvExRate,a.InvPayAmt,a.InvPayAmtL,a.InvAdjAmt,a.InvAdjAmtL, a.InvDiscAmt,a.InvDiscAmtL,a.InvChgAmt,a.InvChgAmtL,a.InvPayAmt+a.InvAdjAmt+a.InvDiscAmt+a.InvChgAmt as InvActPayAmt,a.InvPayAmtL+a.InvAdjAmtL+a.InvDiscAmtL+a.InvChgAmtL as InvActPayAmtL,a.InvPaidAmt,a.InvPaidAmtL,a.InvType,a.InvDetStatus,a.Note,ChargeTypeName, a.InvPayAmt+a.InvAdjAmt+a.InvDiscAmt+a.InvChgAmt - a.InvPaidAmt as ThisPaid from InvoiceDetail a left join ChargeType b on a.ChargeTypeID=b.ChargeTypeID left join Invoiceheader  on Invoiceheader.invid=a.invid left join customer on customer.custid=Invoiceheader.custid where a.InvID=" + ViewState["InvID"].ToString() + " And RentType <> " + Invoice.InvoiceDetail.RENTTYPE_BLANK_RECORD_P;

        DataSet ds = objBaseBo.QueryDataSet(strSql);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            DataRow dr = InvDetailDT.NewRow();
            //dr["InvAdjDetID"] ="";//id    
            dr["InvDetailID"] = ds.Tables[0].Rows[i]["InvDetailID"].ToString();//InvoiceDetail表的id
            //dr["InvAdjID"] = "";//InvAdj表的id
            dr["CustShortName"] = ds.Tables[0].Rows[i]["custshortname"].ToString();//客户简称
            dr["CustName"] = ds.Tables[0].Rows[i]["CustName"].ToString();//客户名称
            //dr["ChargeTypeID"] = ds.Tables[0].Rows[i]["ChargeTypeID"].ToString();
            dr["ChargeTypeName"] = ds.Tables[0].Rows[i]["ChargeTypeName"].ToString();
            dr["ThisPaid"] = ds.Tables[0].Rows[i]["ThisPaid"].ToString();
            dr["InvPaidAmt"] = ds.Tables[0].Rows[i]["InvPaidAmt"].ToString();
            //dr["AdjAmt"] = "";
            dr["AdjReason"] = "";

            InvDetailDT.Rows.Add(dr);
        }
        for (int i = (ds.Tables[0].Rows.Count%15); i < 15; i++)
        {
            InvDetailDT.Rows.Add(InvDetailDT.NewRow());
        }
        gvInvoice.DataSource = InvDetailDT;
        gvInvoice.DataBind();
        this.btnPutIn.Enabled = true;
        this.BindCust(ViewState["strWhere"].ToString());
        ViewState["Flag"] = DISPROVE_IN;
    }
    protected void gvInvoice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            TextBox txtInvAdjDetID = (TextBox)e.Row.Cells[1].FindControl("txtInvAdjDetID");
            Label txtCustShortName = (Label)e.Row.Cells[4].FindControl("txtCustShortName");
            Label txtChargeTypeName = (Label)e.Row.Cells[5].FindControl("txtChargeTypeName");
            //Label txtThisPaid = (Label)e.Row.Cells[6].FindControl("txtThisPaid");
            TextBox txtThisPaid = (TextBox)e.Row.Cells[6].FindControl("txtThisPaid");
            Label txtInvPaidAmt = (Label)e.Row.Cells[7].FindControl("txtInvPaidAmt");
            TextBox txtAdjAmt = (TextBox)e.Row.Cells[8].FindControl("txtAdjAmt");
            TextBox txtAdjReason = (TextBox)e.Row.Cells[10].FindControl("txtAdjReason");
            TextBox txtAdjBackAmt = (TextBox)e.Row.Cells[9].FindControl("txtAdjBackAmt");//调整后金额

            txtAdjBackAmt.Enabled = false;
            //txtThisPaid.Enabled = false;
            txtAdjAmt.Attributes.Add("onblur", "SumTotalRmb('" + txtThisPaid.ClientID + "','" + txtAdjAmt.ClientID + "','" + txtAdjBackAmt.ClientID + "')");

            if (txtInvAdjDetID.Text.Trim() != "")
            {
                ((System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Checkbox")).Checked = true;
                ((System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Checkbox")).Enabled = false;
            }
            if (e.Row.Cells[12].Text == "1")
            {
                foreach (TableCell oCell in e.Row.Cells)
                {
                    oCell.Attributes.Add("Class", "Error");
                    txtCustShortName.ForeColor = Color.Red;
                    txtChargeTypeName.ForeColor = Color.Red;
                    txtThisPaid.ForeColor = Color.Red;
                    txtInvPaidAmt.ForeColor = Color.Red;
                    txtAdjAmt.ForeColor = Color.Red;
                    txtAdjReason.ForeColor = Color.Red;
                    txtAdjBackAmt.ForeColor = Color.Red;
                }
            }
        }
    }
    protected void gvCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["checkeds"] = "";/////////////////////////////
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select InvCelID from InvCancel where InvId=" + this.gvCust.SelectedRow.Cells[0].Text.Trim() + "");
        string str = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            str = "结算单已被取消";
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + str + "'", true);
            BindInvAdjDetail("0");//绑定结算单明细
            this.BindCust(ViewState["strWhere"].ToString());
            return;
        }

        DataSet dsPaid = objBaseBo.QueryDataSet("select invid from invoiceheader where InvStatus =2 and  InvId=" + this.gvCust.SelectedRow.Cells[0].Text.Trim() + "");
        if (dsPaid.Tables[0].Rows.Count > 0)
        {
            str = "该结算单已有部分付款";
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + str + "'", true);
            BindInvAdjDetail("0");//绑定结算单明细
            this.BindCust(ViewState["strWhere"].ToString());
            return;
        }

        DataSet dsInv = objBaseBo.QueryDataSet("select invAdjID from invadj where adjstatus in (1,2) and  InvId=" + this.gvCust.SelectedRow.Cells[0].Text.Trim() + "");
        if (dsInv.Tables[0].Rows.Count > 0)
        {
            str = "该结算单正在被受理";
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + str + "'", true);
            BindInvAdjDetail("0");//绑定结算单明细
            this.BindCust(ViewState["strWhere"].ToString());
            return;
        }
        //结算单已经付清
        DataSet dsInvC = objBaseBo.QueryDataSet("select invPaidAmt,InvActPayAmt from InvoiceDetail where InvId=" + this.gvCust.SelectedRow.Cells[0].Text.Trim() + "");
        for (int i = 0; i < dsInvC.Tables[0].Rows.Count; i++)
        {
            decimal invActPayAmt = Convert.ToDecimal(dsInvC.Tables[0].Rows[i]["InvActPayAmt"].ToString());  //实际应结金额
            decimal invPaidAmt = Convert.ToDecimal(dsInvC.Tables[0].Rows[i]["invPaidAmt"].ToString());    //付款金额
            if (invActPayAmt == invPaidAmt && invActPayAmt != 0)
            {
                this.BindCust(ViewState["strWhere"].ToString());
                BindInvAdjDetail("0");//绑定结算单明细
                ScriptManager.RegisterStartupScript(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_invOver") + "'", true);
                return;
            }
        }

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + str + "'", true);
        ViewState["InvID"] = this.gvCust.SelectedRow.Cells[0].Text.Trim();
        ViewState["InvExRate"] = this.gvCust.SelectedRow.Cells[8].Text.Trim();
        ViewState["CustName"] = this.gvCust.SelectedRow.Cells[9].Text.Trim();
        ViewState["CustShortName"] = this.gvCust.SelectedRow.Cells[5].Text.Trim();
        this.txtContractCode.Text = this.gvCust.SelectedRow.Cells[3].Text.Trim();
        this.txtCustCode.Text = this.gvCust.SelectedRow.Cells[10].Text.Trim();
        this.txtInvCode.Text = this.gvCust.SelectedRow.Cells[2].Text.Trim();
        this.txtInvPeriod.Text = this.gvCust.SelectedRow.Cells[4].Text.Trim();
        this.btnAdd_Click(sender, e);
        this.btnAdd.Enabled = true;
        this.BindCust(ViewState["strWhere"].ToString());
    }
    /// <summary>
    /// 作废
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBlankOut_Click(object sender, EventArgs e)
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
            //String voucherHints = txtCustName.Text.Trim();
            //String voucherMemo = txtCustName.Text;
            String voucherHints = ViewState["CustName"].ToString();
            String voucherMemo = ViewState["CustName"].ToString();


            baseTrans.BeginTrans();
            baseTrans.ExecuteUpdate("update InvAdj set AdjStatus = " + InvAdj.INVADJ_BLANK_OUT + " where InvAdjID = " + voucherID);

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, sequence, vInfo, baseTrans);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("作废审批信息错误:", ex);
        }
        baseTrans.Commit();
    }
    protected void gvInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.FindCustChecked();//记录选择           
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
        gvInvoice.DataSource = InvDetailDT;
        gvInvoice.DataBind();
        SetCustSelectRecords(ViewState["checkeds"].ToString());//设置选择项
        this.BindCust(ViewState["strWhere"].ToString());
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select InvCelID from InvCancel where InvId=" + ViewState["InvID"].ToString() + "");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string str = "结算单已被取消";
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + str + "'", true);
            return;
        }

        SaveDetailDT.Rows.Clear();
        this.FindCustChecked();//记录选择
        if (ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') == "")
        {
            return;
        }
        string strArr = "," + ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') + ",";
        string strDateISNull = "没有数据";
        for (int i = 0; i < InvDetailDT.Rows.Count; i++)
        {
            if (InvDetailDT.Rows[i]["InvDetailID"].ToString() != "")
            {
                if (strArr.IndexOf("," + InvDetailDT.Rows[i]["InvDetailID"] + ",") >= 0)
                {
                    if (InvDetailDT.Rows[i]["AdjAmt"].ToString() == "")
                    {
                        this.BindCust(ViewState["strWhere"].ToString());
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISNull + "'", true);
                        return;
                    }
                }
            }
        }
        //DataSet dsInv = objBaseBo.QueryDataSet("select invPaidAmt,InvActPayAmt from InvoiceDetail where InvDetailID in ( " + ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') + ")");
        //for (int i = 0; i < dsInv.Tables[0].Rows.Count; i++)
        //{
        //    decimal invActPayAmt = Convert.ToDecimal(dsInv.Tables[0].Rows[i]["InvActPayAmt"].ToString());  //实际应结金额
        //    decimal invPaidAmt = Convert.ToDecimal(dsInv.Tables[0].Rows[i]["invPaidAmt"].ToString());    //付款金额
        //    if (invActPayAmt == invPaidAmt && invActPayAmt != 0)
        //    {
        //        this.BindCust(ViewState["strWhere"].ToString());
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_invOver") + "'", true);
        //        return;
        //    }
        //}

        BaseBO baseBO = new BaseBO();
        InvAdj invAdj = new InvAdj();
        InvAdjDetIns InvAdjDetIns = new InvAdjDetIns();
        InvAdjDetail invAdjDetail = new InvAdjDetail();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        String voucherHints = ViewState["CustName"].ToString();
        String voucherMemo = "";
        int wrkFlwID = Convert.ToInt32(Request.QueryString["WrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.QueryString["NodeID"]);
        decimal sumAdjAmt = 0;
        BaseTrans baseTrans = new BaseTrans();
        baseTrans.BeginTrans();
        try
        {
            if (Convert.ToInt32(ViewState["Flag"]) == DISPROVE_IN)//新增
            {
                invAdj.InvAdjID = BaseApp.GetInvAdjID();
                ViewState["VoucherID"] = invAdj.InvAdjID;
                //baseTrans.BeginTrans();
                for (int i = 0; i < InvDetailDT.Rows.Count; i++)
                {
                    if (strArr.IndexOf("," + InvDetailDT.Rows[i]["InvDetailID"] + ",") >= 0)//判断是否选择了数据
                    {
                        InvAdjDetIns.InvAdjDetID = BaseApp.GetInvAdjDetID();
                        InvAdjDetIns.InvAdjID = invAdj.InvAdjID;
                        InvAdjDetIns.InvDetailID = Int32.Parse(InvDetailDT.Rows[i]["InvDetailID"].ToString());
                        InvAdjDetIns.AdjAmt = decimal.Parse(InvDetailDT.Rows[i]["AdjAmt"].ToString());//调整金额
                        InvAdjDetIns.AdjAmtL = decimal.Parse(InvDetailDT.Rows[i]["AdjAmt"].ToString()) / Convert.ToDecimal(ViewState["InvExRate"]);//调整本地金额
                        InvAdjDetIns.AdjReason = InvDetailDT.Rows[i]["AdjReason"].ToString();//调整原因
                        sumAdjAmt = sumAdjAmt + InvAdjDetIns.AdjAmt;//调整合计金额
                        if (baseTrans.Insert(InvAdjDetIns) < 1)
                        {
                            SaveDetailDT.ImportRow(InvDetailDT.Rows[0]);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                            baseTrans.Rollback();
                            return;
                        }
                        //调整滞纳金
                        if (objBaseBo.QueryDataSet("select lateInvDetailID from invoiceinterest where lateInvDetailID=" + Int32.Parse(InvDetailDT.Rows[i]["InvDetailID"].ToString()) + "").Tables[0].Rows.Count > 0)
                        {
                            if (baseTrans.ExecuteUpdate("update invoiceinterest set InterestAmt=" + InvAdjDetIns.AdjAmt + " where lateInvDetailID=" + Int32.Parse(InvDetailDT.Rows[i]["InvDetailID"].ToString()) + "") < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                                baseTrans.Rollback();
                                return;
                            }
                        }
                    }
                }
                invAdj.InvID = Convert.ToInt32(ViewState["InvID"]);
                invAdj.CreateUserID = sessionUser.UserID;
                invAdj.CreateTime = DateTime.Now;
                invAdj.ModifyTime = DateTime.Now;
                invAdj.ModifyUserID = sessionUser.UserID;
                invAdj.OprDeptID = sessionUser.DeptID;
                invAdj.OprRoleID = sessionUser.RoleID;
                invAdj.AdjAmt = sumAdjAmt;
                invAdj.AdjAmtL = sumAdjAmt / Convert.ToDecimal(ViewState["InvExRate"]);
                invAdj.AdjDate = DateTime.Now;
                invAdj.AdjOpr = sessionUser.UserID;
                invAdj.AdjReason = "";//调整原因
                invAdj.AdjStatus = InvAdj.INVADJ_YES_PUT_IN_NO_UPDATE_LEASE_STATUS;

                if (baseTrans.Insert(invAdj) < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    baseTrans.Rollback();
                    return;
                }
                VoucherInfo vInfo = new VoucherInfo(Convert.ToInt32(ViewState["VoucherID"]), voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);
                WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo, baseTrans);
                baseTrans.Commit();
            }
            else
            {
                for (int i = 0; i < InvDetailDT.Rows.Count; i++)
                {
                    string txtInvDetailID = InvDetailDT.Rows[i]["InvDetailID"].ToString();
                    if (strArr.IndexOf("," + txtInvDetailID + ",") >= 0)//判断是否选择了数据
                    {
                        //InvAdjDetIns.InvAdjID = invAdj.InvAdjID;
                        InvAdjDetIns.InvDetailID = Int32.Parse(InvDetailDT.Rows[i]["InvDetailID"].ToString());
                        InvAdjDetIns.AdjAmt = decimal.Parse(InvDetailDT.Rows[i]["AdjAmt"].ToString());//调整金额
                        InvAdjDetIns.AdjAmtL = decimal.Parse(InvDetailDT.Rows[i]["AdjAmt"].ToString()) / Convert.ToDecimal(ViewState["InvExRate"]);//调整本地金额
                        InvAdjDetIns.AdjReason = InvDetailDT.Rows[i]["AdjReason"].ToString();//调整原因
                        sumAdjAmt = sumAdjAmt + InvAdjDetIns.AdjAmt;//调整合计金额
                        BaseTrans objBaseTrans = new BaseTrans();
                        objBaseTrans.BeginTrans();
                        string strInvAdjDetID = InvDetailDT.Rows[i]["InvAdjDetID"].ToString();
                        baseTrans.WhereClause = "InvAdjDetID=" + strInvAdjDetID;
                        if (baseTrans.Update(InvAdjDetIns) != -1)
                        {
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                        }
                        //调整滞纳金
                        if (objBaseBo.QueryDataSet("select lateInvDetailID from invoiceinterest where lateInvDetailID=" + Int32.Parse(InvDetailDT.Rows[i]["InvDetailID"].ToString()) + "").Tables[0].Rows.Count > 0)
                        {
                            if (baseTrans.ExecuteUpdate("update invoiceinterest set InterestAmt=" + InvAdjDetIns.AdjAmt + " where lateInvDetailID=" + Int32.Parse(InvDetailDT.Rows[i]["InvDetailID"].ToString()) + "") < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                                baseTrans.Rollback();
                                return;
                            }
                        }
                    }
                }
                invAdj.ModifyTime = DateTime.Now;
                invAdj.ModifyUserID = sessionUser.UserID;
                invAdj.AdjAmt = sumAdjAmt;
                invAdj.AdjAmtL = sumAdjAmt / Convert.ToDecimal(ViewState["InvExRate"]);
                if (baseTrans.ExecuteUpdate("update InvAdj set AdjAmt='" + invAdj.AdjAmt + "',AdjAmtL='" + invAdj.AdjAmtL + "' where InvAdjID=" + ViewState["VoucherID"].ToString() + " ") < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    baseTrans.Rollback();
                    return;
                }

                VoucherInfo vInfo = new VoucherInfo(Convert.ToInt32(ViewState["VoucherID"]), voucherHints, voucherMemo, sessionUser.DeptID, sessionUser.UserID);
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(ViewState["SequenceID"]), vInfo, baseTrans);
                baseTrans.Commit();
            }
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            Response.Write(ex);
        }
        ViewState["checkeds"] = "";
        this.btnPutIn.Enabled = true;
        btnAdd.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
    }
    protected void gvCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >=0)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.Cells[0].Text == "&nbsp;")
                {
                    e.Row.Cells[7].Text = "";
                }
            }
        }
    }
    protected void btnMessage_Click(object sender, EventArgs e)
    {
        this.BindCust(ViewState["strWhere"].ToString());
    }
}
