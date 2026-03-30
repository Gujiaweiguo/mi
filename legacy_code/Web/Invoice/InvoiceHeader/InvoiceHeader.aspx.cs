using System;
using System.Data;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base;
using Base.DB;
using Base.Biz;
using Invoice.InvoiceH;
using BaseInfo.User;
using Base.Page;
using System.Drawing;
using BaseInfo.authUser;

public partial class Lease_InvoiceHeader_InvoiceHeader : BasePage
{
    public string notInsertStr;
    public string notUpdateStr;
    public string AddStr;
    public string invPayTypeStr;
    public string baseInfo;  //基本信息
    public string strFresh;
    public string invPayType;
    public string invPaidAmtSum;

    private decimal invHeadPayAmt;  //结算主表应结总额
    private decimal invHeadPaidAmt; //结算主表已结金额

    protected void Page_Load(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        txtInvPaidAmtSum.Attributes.Add("onblur", "txtSurAmtSum()");
        if (!IsPostBack)
        {
            ViewState["flag"] = 0;
            /*付款方式*/
            BindInvPayType();

            /*绑定币种*/
            baseBO.WhereClause = "CurTypeStatus = " + CurrencyType.CURTYPESTATUS_VALID;
            rs = baseBO.Query(new CurrencyType());
            foreach (CurrencyType currencyType in rs)
            {
                cmbCurrencyType.Items.Add(new ListItem(currencyType.CurTypeName, currencyType.CurTypeID.ToString()));
            }
            //BindInvoiceDetailNull();
            BindInvoiceDetail();
            notInsertStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidInsert");
            notUpdateStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdateLost");
            AddStr = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Invoice_InvBalan");
            invPayType = (String)GetGlobalResourceObject("BaseInfo", "Prompt_selectPayType");
            invPaidAmtSum = (String)GetGlobalResourceObject("BaseInfo", "Prompt_paidAmtSum");
            btnSave.Attributes.Add("onclick", "return InputValidator(form1)");
            SetTextEnable(false);
            txtInvPaidAmtSum.Attributes.Add("onkeydown", "textleave()");
        }
        BindInvoiceHeader();
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }

    /*付款方式*/
    private void BindInvPayType()
    {
        cmbInvPayType.Items.Clear();
        string selected = (String)GetGlobalResourceObject("BaseInfo", "Select_Select");
        int[] status = InvoicePay.GetInvPayType();
        cmbInvPayType.Items.Add(new ListItem(selected));
        for (int i = 0; i < status.Length; i++)
        {
            cmbInvPayType.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", InvoicePay.GetInvPayTypeDesc(status[i])), status[i].ToString()));
        }
    }

    protected void BindInvoiceHeader()
    {
        /*绑定结算单主表*/
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        baseBO.WhereClause = "InvStatus in (" + InvoiceHeader.INVOICEHEADER_AVAILABILITY + "," + InvoiceHeader.INVOICEHEADER_PART_BACKING_OUT + ")";
       

       /* if (ViewState["InvID"] != null)
        {
            baseBO.WhereClause += " and InvID = " + ViewState["InvID"];
        }
        if (ViewState["CustID"] != null)
        {
            baseBO.WhereClause += " and a.CustID = " + ViewState["CustID"];
        }
        if (ViewState["CustID"] == null && ViewState["InvID"] == null)
        {
            baseBO.WhereClause += " and 1=0";
        }*/

        if (this.txtCustCode.Text.Trim() != "")
        {
            baseBO.WhereClause += " AND b.CustCode = '" + txtCustCode.Text + "'";
        }
        if (this.txtContractCode.Text.Trim() != "")
        {
            baseBO.WhereClause += " AND c.ContractCode = '" + txtContractCode.Text + "'";
        }
        if (this.txtInvCode.Text.Trim() != "")
        {
            baseBO.WhereClause += "  AND  a.InvCode = '" + txtInvCode.Text + "'";
        }
        if (Convert.ToInt32(ViewState["flag"]) == 0)
        {
            baseBO.WhereClause += " AND 1=0";
        }
        //baseBO.WhereClause += " ORDER BY a.InvID DESC";

        string str_sql = "select ConShop.ShopID, a.InvID,a.CustID,CustCode,a.ContractID,ContractCode,a.CurTypeID,a.CreateUserID,a.CreateTime,a.ModifyUserID,a.ModifyTime,a.OprRoleID,a.OprDeptID,InvCode,b.CustName,InvDate,InvPeriod,InvStatus," +
                        " a.InvType,a.IsFirst,a.InvCurTypeID,a.InvExRate,a.InvPayAmt,a.InvPayAmtL,a.InvAdjAmt,a.InvAdjAmtL,a.InvDiscAmt,a.InvDiscAmtL,a.InvChngAmt,a.InvChngAmtL,sum(e.InvPayAmt+e.InvAdjAmt+e.InvDiscAmt+e.InvChgAmt) as InvActPayAmt," +
                        " sum(e.InvPayAmtL+e.InvAdjAmtL+e.InvDiscAmtL+e.InvChgAmtL) as InvActPayAmtL,a.InvPaidAmt,a.InvPaidAmtL,PrintFlag,a.Note,a.InvChngAmtL,sum(e.InvPayAmt+e.InvAdjAmt+e.InvDiscAmt+e.InvChgAmt - e.InvPaidAmt) as ThisPaid " +
                        " from InvoiceHeader a" + 
                        " left join Customer b on a.CustID=b.CustID " +
                        " left join Contract c on a.ContractID=c.ContractID" + 
                        " left join CurrencyType d on a.InvCurTypeID=d.CurTypeID" +
                        " left join invoicedetail e on a.invid = e.invid " +
                        " INNER JOIN ConShop ON c.ContractID = ConShop.ContractID";
        str_sql += " WHERE " + baseBO.WhereClause + " AND a.InvType = " + Invoice.InvoiceHeader.INVTYPE_LEASE + " AND a.InvPayAmt >= 1";

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            str_sql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        str_sql += " group by ConShop.ShopID,a.InvID,a.CustID,CustCode,a.ContractID,ContractCode,a.CurTypeID,a.CreateUserID,a.CreateTime,a.ModifyUserID,a.ModifyTime,a.OprRoleID,a.OprDeptID,InvCode,b.CustName,InvDate,InvPeriod,InvStatus," +
                   " a.InvType,a.IsFirst,a.InvCurTypeID,a.InvExRate,a.InvPayAmt,a.InvPayAmtL,a.InvAdjAmt,a.InvAdjAmtL,a.InvDiscAmt,a.InvDiscAmtL,a.InvChngAmt,a.InvChngAmtL,"+
                   " a.InvPaidAmt,a.InvPaidAmtL,PrintFlag,a.Note,a.InvChngAmtL ORDER BY a.InvID DESC";

        //DataTable dt = baseBO.QueryDataSet(new InvoiceHeader()).Tables[0];
        DataTable dt = baseBO.QueryDataSet(str_sql).Tables[0];

        pds.DataSource = dt.DefaultView;
        pds.AllowPaging = true;
        //GrdVewInvoiceHeader.PageSize = 5;
        //pds.PageSize = 5;
        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdVewInvoiceHeader.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            //bt1Back.Enabled = false;
            //bt1Next.Enabled = false;
            pds.DataSource = dt.DefaultView;
            GrdVewInvoiceHeader.DataSource = pds;
            GrdVewInvoiceHeader.DataBind();
        }
        else
        {
            //ViewState["ShopID"] = dt.Rows[0]["ShopID"];
            ////change by lcp
            //if (dt.Rows.Count < GrdVewInvoiceHeader.PageSize)
            //    pds.CurrentPageIndex = 0;
            //else
            //    pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            ////end
            //if (pds.IsFirstPage)
            //{
            //    bt1Back.Enabled = false;
            //    bt1Next.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    bt1Back.Enabled = true;
            //    bt1Next.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    bt1Back.Enabled = false;
            //    bt1Next.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    bt1Back.Enabled = true;
            //    bt1Next.Enabled = true;
            //}
            //this.GrdVewInvoiceHeader.DataSource = pds;
            //this.GrdVewInvoiceHeader.DataBind();
            //spareRow = GrdVewInvoiceHeader.Rows.Count;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //GrdVewInvoiceHeader.DataSource = pds;
            //GrdVewInvoiceHeader.DataBind();
            GrdVewInvoiceHeader.DataSource = pds;
            GrdVewInvoiceHeader.DataBind();
            spareRow = GrdVewInvoiceHeader.Rows.Count;
            for (int i = 0; i < GrdVewInvoiceHeader.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            GrdVewInvoiceHeader.DataSource = pds;
            GrdVewInvoiceHeader.DataBind();

        }
        ViewState["flag"] = 1;
    }
    protected void BindInvoiceDetail()
    {
        /*绑定结算单明细*/
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        string BoSql = "select a.InvDetailID,a.ChargeTypeID,a.InvID,a.Period,a.InvStartDate,a.InvEndDate,a.InvCurTypeID,a.InvExRate,a.InvPayAmt,a.InvPayAmtL,a.InvAdjAmt,a.InvAdjAmtL, "+
                       " a.InvDiscAmt,a.InvDiscAmtL,a.InvChgAmt,a.InvChgAmtL,a.InvPayAmt+a.InvAdjAmt+a.InvDiscAmt+a.InvChgAmt as InvActPayAmt,a.InvPayAmtL+a.InvAdjAmtL+a.InvDiscAmtL+a.InvChgAmtL as "+
                       " InvActPayAmtL,a.InvPaidAmt,a.InvPaidAmtL,a.InvType,a.InvDetStatus,a.Note,b.ChargeTypeName, a.InvPayAmt+a.InvAdjAmt+a.InvDiscAmt+a.InvChgAmt - a.InvPaidAmt as ThisPaid "+
                       " from InvoiceDetail a left join ChargeType b on a.ChargeTypeID=b.ChargeTypeID "+
                       " left join invoiceheader on a.InvID=invoiceheader.InvID "+
                       " left join ConShop on Invoiceheader.ContractID=ConShop.ContractID Where 1=1 ";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            BoSql += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                        ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
        }
        BoSql += " AND a.InvID= '" + Convert.ToInt32(ViewState["DetailInvID"]) + "' and a.InvDetStatus in ( " + InvoiceDetail.INVOICEDETAIL_AVAILABILITY + "," + InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT + "," + InvoiceDetail.INVOICEDETAIL_FULL_BACKING_OUT + ") and a.InvActPayAmt > 0";

        DataTable dt = baseBO.QueryDataSet(BoSql).Tables[0];

        pds.DataSource = dt.DefaultView;
        //GrdVewInvoiceDetail.PageSize = 10;
        
        if (pds.Count < 1)
        {
            for (int i = 0; i < GrdVewInvoiceDetail.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            //btnBack.Enabled = false;
            //btnNext.Enabled = false;
            pds.DataSource = dt.DefaultView;
            GrdVewInvoiceDetail.DataSource = pds;
            GrdVewInvoiceDetail.DataBind();
        }
        else
        {
            //pds.AllowPaging = true;
            //pds.PageSize = 10;
            //pds.CurrentPageIndex = int.Parse(lblDetailCurrent.Text) - 1;
            //if (pds.IsFirstPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = true;
            //}

            //if (pds.IsLastPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = false;
            //}

            //if (pds.IsFirstPage && pds.IsLastPage)
            //{
            //    btnBack.Enabled = false;
            //    btnNext.Enabled = false;
            //}

            //if (!pds.IsLastPage && !pds.IsFirstPage)
            //{
            //    btnBack.Enabled = true;
            //    btnNext.Enabled = true;
            //}
            //this.GrdVewInvoiceDetail.DataSource = pds;
            //this.GrdVewInvoiceDetail.DataBind();
            //spareRow = GrdVewInvoiceDetail.Rows.Count;
            //ViewState["spareRow"] = spareRow;
            //for (int i = 0; i < pds.PageSize - spareRow; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            //GrdVewInvoiceDetail.DataSource = pds;
            //GrdVewInvoiceDetail.DataBind();
            GrdVewInvoiceDetail.DataSource = pds;
            GrdVewInvoiceDetail.DataBind();
            spareRow = GrdVewInvoiceDetail.Rows.Count;
            for (int i = 0; i < GrdVewInvoiceDetail.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            GrdVewInvoiceDetail.DataSource = pds;
            GrdVewInvoiceDetail.DataBind();
        }
    }

    /*
    protected void BindInvoiceDetailNull()
    {
       
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        baseBO.WhereClause = "InvID=" + Convert.ToInt32(ViewState["DetailInvID"]);
        DataTable dt = baseBO.QueryDataSet(new InvoiceDetail()).Tables[0];

        pds.DataSource = dt.DefaultView;
        GridView1.PageSize = 5;
        pds.PageSize = 5;
        if (pds.Count < 1)
        {
            for (int i = 0; i < GridView1.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GridView1.DataSource = pds;
            GridView1.DataBind();
        }
        else
        {

            this.GridView1.DataSource = pds;
            this.GridView1.DataBind();
            spareRow = GridView1.Rows.Count;
            for (int i = 0; i < pds.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            GridView1.DataSource = pds;
            GridView1.DataBind();
        }
    }*/
    protected void GrdVewInvoiceDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[6].Text = "";
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[3].Text = InvoiceDetailBo.SumInvoiceDetailPayAmtTotal(Convert.ToInt32(ViewState["DetailInvID"])).ToString();
            e.Row.Cells[4].Text = InvoiceDetailBo.SumInvoiceDetailPaidAmtTotal(Convert.ToInt32(ViewState["DetailInvID"])).ToString();
            e.Row.Cells[5].Text = Convert.ToString(Convert.ToDecimal(e.Row.Cells[3].Text) - Convert.ToDecimal(e.Row.Cells[4].Text));
        } 

    }
    protected void GrdVewInvoiceHeader_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[0].Text == "&nbsp;")
            {
                e.Row.Cells[5].Text = "";
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[3].Text != null)
            {
                string type = DataBinder.Eval(e.Row.DataItem, "InvID").GetType().ToString();
                if (type != "System.DBNull")
                {
                    invHeadPayAmt += InvoiceHeaderBo.SumInvoiceHeaderPayAmtTotal(Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "InvID")));
                    invHeadPaidAmt += InvoiceHeaderBo.SumInvoiceHeaderPaidAmtTotal(Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "InvID")));
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = invHeadPayAmt.ToString();
            e.Row.Cells[3].Text = invHeadPaidAmt.ToString();
            e.Row.Cells[4].Text = Convert.ToString(Convert.ToDecimal(e.Row.Cells[2].Text) - Convert.ToDecimal(e.Row.Cells[3].Text));
            invHeadPayAmt = 0;
            invHeadPaidAmt = 0;
        } 
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        clear();
        /*BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        InvoiceHeader invoiceHeader = new InvoiceHeader();
        //CurExRate curExRate = new CurExRate();
        //按客户号查找信息
        if (txtCustCode.Text.Trim().Length > 0 )
        {
            Customer customer = new Customer();
            baseBO.WhereClause = "CustCode ='" + txtCustCode.Text.Trim() + "'";
            rs = baseBO.Query(customer);
            txtCustName.Text = customer.CustName;
            if (rs.Count == 1)
            {
                customer = rs.Dequeue() as Customer;
                ViewState["CustID"] = customer.CustID;
                baseBO.WhereClause = "a.CustID=" + customer.CustID;
                rs = baseBO.Query(invoiceHeader);
                if (rs.Count == 1)
                {
                    invoiceHeader = rs.Dequeue() as InvoiceHeader;
                    txtContractID.Text = invoiceHeader.ContractCode.ToString();
                    ViewState["ContractID"] = invoiceHeader.ContractID;
                    //txtInvPayDate.Text = DateTime.Now.ToString();
                    txtInvPayExRate.Text = invoiceHeader.InvExRate.ToString();
                    ViewState["HeaderInvPaidAmt"] = invoiceHeader.InvPaidAmt;
                    //baseBO.WhereClause = "ToCurTypeID=" + Convert.ToInt32(cmbCurrencyType.SelectedValue);
                    //rs = baseBO.Query(curExRate);
                    //if (rs.Count == 1)
                    //{
                    //    curExRate = rs.Dequeue() as CurExRate;
                    //    txtInvPayExRate.Text = curExRate.ExRate.ToString();
                    //}
                    ViewState["InvID"] = invoiceHeader.InvID.ToString();
                    ViewState["InvType"] = invoiceHeader.InvType;
                }
                txtInvPayDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                BindInvoiceHeader();
            }
        }

        //按结算单号查找信息


        if (txtInvCode.Text.Trim().Length > 0)
        {
            baseBO.WhereClause = "InvCode = " + txtInvCode.Text.Trim();
            rs = baseBO.Query(invoiceHeader);
            if (rs.Count == 1)
            {
                invoiceHeader = rs.Dequeue() as InvoiceHeader;
                ViewState["CustID"] = invoiceHeader.CustID;
                txtContractID.Text = invoiceHeader.ContractCode.ToString();
                
                txtCustName.Text = invoiceHeader.CustName;
                txtInvPayDate.Text = DateTime.Now.ToString();
                ViewState["HeaderInvPaidAmt"] = invoiceHeader.InvPaidAmt;
                //baseBO.WhereClause = "ToCurTypeID=" + Convert.ToInt32(cmbCurrencyType.SelectedValue);
                //rs = baseBO.Query(curExRate);
                //if (rs.Count == 1)
                //{
                //    curExRate = rs.Dequeue() as CurExRate;
                //    txtInvPayExRate.Text = curExRate.ExRate.ToString();
                //}
                ViewState["ContractID"] = invoiceHeader.ContractID;
                ViewState["InvID"] = invoiceHeader.InvID.ToString();
                ViewState["InvType"] = invoiceHeader.InvType;
                txtInvPayDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                BindInvoiceHeader();
            }
        }
        

        string whereStr = " WHERE ";
        if (this.txtCustCode.Text.Trim() != "")
        {
            whereStr += " b.CustCode = '" + txtCustCode.Text + "' AND ";
        }
        if (this.txtContractCode.Text.Trim())
        {
            whereStr += " c.ContractCode = '" + txtContractCode.Text + "' AND ";
        }
        if (this.txtInvCode.Text.Trim())
        {
            whereStr += " a.InvCode = '" + txtInvCode.Text + "' AND ";
        }
        whereStr += "1 = 1";
        DataSet ds = InvoiceHeaderBo.QueryInvHeader(whereStr);*/
        if (this.txtCustCode.Text.Trim() != "" || txtContractCode.Text != "" || txtInvCode.Text != "")
        {
            BindInvoiceHeader();
            BindInvoiceDetail();
            txtInvPayDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
        foreach (GridViewRow grv in GrdVewInvoiceHeader.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    protected void GrdVewInvoiceHeader_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lblDetailCurrent.Text = "1";
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        InvoiceDetailSum invoiceDetailSum = new InvoiceDetailSum();
        txtContractID.Text = GrdVewInvoiceHeader.SelectedRow.Cells[9].Text.ToString();
        txtCustName.Text = GrdVewInvoiceHeader.SelectedRow.Cells[8].Text.ToString();
        ViewState["DetailInvID"] = GrdVewInvoiceHeader.SelectedRow.Cells[0].Text.ToString();
        cmbCurrencyType.SelectedValue = GrdVewInvoiceHeader.SelectedRow.Cells[7].Text.ToString();
        this.txtInvPayExRate.Text = GrdVewInvoiceHeader.SelectedRow.Cells[6].Text.ToString();
        //txtInvPaidAmt.Text = GrdVewInvoiceHeader.SelectedRow.Cells[2].Text.ToString();
        txtInvPaidAmt.Text = ((Label)GrdVewInvoiceHeader.SelectedRow.FindControl("Label3")).Text.ToString();
        HidnAmt.Value = ((Label)GrdVewInvoiceHeader.SelectedRow.FindControl("Label3")).Text.ToString();
        baseBO.WhereClause = "InvID=" + Convert.ToInt32(ViewState["DetailInvID"]);
        rs = baseBO.Query(invoiceDetailSum);
        invoiceDetailSum = rs.Dequeue() as InvoiceDetailSum;
        ViewState["invPaidAmt"] = ((Label)GrdVewInvoiceHeader.SelectedRow.FindControl("Label2")).Text.ToString();
        ViewState["invActPayAmt"] = ((Label)GrdVewInvoiceHeader.SelectedRow.FindControl("Label1")).Text.ToString();
        txtSurAmt.Text = txtInvPaidAmtSum.Text;
        ViewState["CustID"] = GrdVewInvoiceHeader.SelectedRow.Cells[10].Text.ToString();
        ViewState["ContractID"] = GrdVewInvoiceHeader.SelectedRow.Cells[11].Text.ToString();
        ViewState["InvID"] = GrdVewInvoiceHeader.SelectedRow.Cells[0].Text.ToString();
        //GrdVewInvoiceDetail.DataSource = null;
        //GrdVewInvoiceDetail.DataBind();
        BindInvoiceDetail();
        //GridView1.DataSource = null;
        //GridView1.DataBind();
        SetTextEnable(true);
        
    }
    protected void cmbInvPayType_SelectedIndexChanged(object sender, EventArgs e)
    {
        SurplusSum surplusSum = new SurplusSum();
        PayInSum payInSum = new PayInSum();
        InvoicePaySum invoicePaySum = new InvoicePaySum();
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        
        ///*余款诋付*/
        //if (Convert.ToInt32(cmbInvPayType.SelectedValue) == InvoicePay.INVPAYTYPE_RESIDUAL_MORTAGAGE)
        //{
        //    baseBO.WhereClause = "CustID =" + Convert.ToInt32(ViewState["CustID"]);
        //    rs = baseBO.Query(surplusSum);
        //    surplusSum = rs.Dequeue() as SurplusSum;
        //    txtSurAmt.Text = Convert.ToString(Convert.ToDecimal(surplusSum.SurAmt) - Convert.ToDecimal(surplusSum.PayOutAmtSum));
        //    txtInvPaidAmtSum.Text = txtSurAmt.Text;
        //    txtInvPaidAmtSum.CssClass = "Enabledipt160px";
        //    txtInvPaidAmtSum.ReadOnly = true;
        //}

        ///*代收款诋扣*/
        //else if (Convert.ToInt32(cmbInvPayType.SelectedValue) == InvoicePay.INVPAYTYPE_REPLACE_FUND)
        //{
        //    baseBO.WhereClause = "a.ContractID=b.ContractID and b.ShopID=c.ShopID and a.CustID=" + Convert.ToInt32(ViewState["CustID"]);
        //    rs = baseBO.Query(payInSum);
        //    payInSum = rs.Dequeue() as PayInSum;
        //    txtSurAmt.Text = Convert.ToString(Convert.ToDecimal(payInSum.PayInAmt) - Convert.ToDecimal(payInSum.PayOutAmtSum));
        //    txtInvPaidAmtSum.Text = txtSurAmt.Text;
        //    txtInvPaidAmtSum.CssClass = "Enabledipt160px";
        //    txtInvPaidAmtSum.ReadOnly = true;
        //}

        ///*押金诋付*/
        //else if (Convert.ToInt32(cmbInvPayType.SelectedValue) == InvoicePay.INVPAYTYPE_DEPOSIT_MORTAGAGE)
        //{
        //    baseBO.WhereClause = "CustID=" + Convert.ToInt32(ViewState["CustID"]) + " and InvPayDetStatus =" + InvoicePayDetail.INVOICEPAYDETAIL_AVAILABILITY +
        //    " or InvPayDetStatus=" + InvoicePayDetail.INVOICEPAYDETAIL_PART_BACKING_OUT;
        //    rs = baseBO.Query(invoicePaySum);
        //    invoicePaySum = rs.Dequeue() as InvoicePaySum;
        //    txtSurAmt.Text = Convert.ToString(Convert.ToDecimal(invoicePaySum.InvPaidAmt) - Convert.ToDecimal(invoicePaySum.PayOutAmtSum));
        //    txtInvPaidAmtSum.Text = txtSurAmt.Text;
        //    txtInvPaidAmtSum.CssClass = "Enabledipt160px";
        //    txtInvPaidAmtSum.ReadOnly = true;
        //}
        //else
        //{
        //    txtInvPaidAmtSum.Text = "";
        //    txtSurAmt.Text = "";
        //}
        cmbInvPayType.Enabled = false;
    }
    protected void cmbCurrencyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BaseBO baseBO = new BaseBO();
        //Resultset rs = new Resultset();
        //CurExRate curExRate = new CurExRate();
        //baseBO.WhereClause = "ToCurTypeID=" + Convert.ToInt32(cmbCurrencyType.SelectedValue);
        //rs = baseBO.Query(curExRate);
        //if (rs.Count == 1)
        //{
        //    curExRate = rs.Dequeue() as CurExRate;
        //    txtInvPayExRate.Text = curExRate.ExRate.ToString();
        //    txtSurAmt.Text =Convert.ToString(Convert.ToDecimal(txtInvPaidAmtSum.Text) * Convert.ToDecimal(txtInvPayExRate.Text));
        //}
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string message = (string)GetGlobalResourceObject("BaseInfo", "Prompt_thisPaidGreaterPaid");
        string message1 = (string)GetGlobalResourceObject("BaseInfo", "Prompt_thisPaidlowerGreaterPaid");
        string message2 = (string)GetGlobalResourceObject("BaseInfo", "Prompt_PayInAmt");
        string MSG = (string)GetGlobalResourceObject("BaseInfo", "Prompt_thisPaidMoreList");
        decimal thisPaid = 0; //本次累计付款额
        decimal payInAmt = 0; //代收款金额

        if (Convert.ToInt32(cmbInvPayType.SelectedValue) == InvoicePay.INVPAYTYPE_REPLACE_FUND)
        {
            payInAmt = PayInPO.GetPayInAmtSum(Convert.ToInt32(ViewState["ShopID"]));
            if (Convert.ToDecimal(txtInvPaidAmtSum.Text) > payInAmt)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message2 + "')", true);
                return;
            }
        }

        foreach (GridViewRow gvr in GrdVewInvoiceDetail.Rows)
        {
            if (((TextBox)gvr.FindControl("txtPayment")).Text == "" || ((TextBox)gvr.FindControl("txtPayment")).Text == null)
            {
            }
            else if (Convert.ToDecimal(((TextBox)gvr.FindControl("txtPayment")).Text) > Convert.ToDecimal(((Label)gvr.FindControl("Label3")).Text.ToString()))  //本次付款额 > 余额
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + MSG + "')", true);
                return;
            }
            else
            {
                thisPaid += Convert.ToDecimal(((TextBox)gvr.FindControl("txtPayment")).Text);  //本次累计付款额


            }
        }
        //判断本次累计付款额是否大于付款总额
        if (thisPaid > Convert.ToDecimal(txtInvPaidAmtSum.Text))
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message + "')", true);
        }
        else if (thisPaid < Convert.ToDecimal(txtInvPaidAmtSum.Text)) //判断本次累计付款额是否小于付款总额
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + message1 + thisPaid +"')", true);
        }
        else
        {
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            Surplus surpluss = new Surplus();
            BaseBO baseBO = new BaseBO();
            BaseTrans baseTeans = new BaseTrans();
            Resultset rs = new Resultset();
            SurBal surBal = new SurBal();
            InvoiceHeader invoiceHeader = new InvoiceHeader();
            InvoiceDetail invoiceDetail = new InvoiceDetail();

            SurBalDel surBalDel = new SurBalDel();
            InvoicePay invoicePay = new InvoicePay();
            PayOut payOut = new PayOut();
            PayIn payIn = new PayIn();
            DepBal depBal = new DepBal();
            DepBalDel depBalDel = new DepBalDel();

            ArrayList aryList = new ArrayList();

            decimal surBalAmtSum = 0;//Convert.ToDecimal(ViewState["invPaidAmt"]);
            decimal invPaidAmt = 0;
            baseTeans.BeginTrans();

            //生成结算付款单ID
            invoicePay.InvPayID = BaseApp.GetInvPayID();
            //结算币种
            invoicePay.InvPayCurID = Convert.ToInt32(GrdVewInvoiceHeader.SelectedRow.Cells[7].Text);
            //结算汇率
            decimal invPayExRate = Convert.ToDecimal(GrdVewInvoiceHeader.SelectedRow.Cells[6].Text.ToString());
            invoicePay.InvPayExRate = invPayExRate;

            try
            {
                //计算付款金额
                foreach (GridViewRow gvrDetail in GrdVewInvoiceDetail.Rows)
                {
                    if (((TextBox)gvrDetail.FindControl("txtPayment")).Text == "" || ((TextBox)gvrDetail.FindControl("txtPayment")).Text == null || ((TextBox)gvrDetail.FindControl("txtPayment")).Text == "0" || ((TextBox)gvrDetail.FindControl("txtPayment")).Text == "0.00")
                    {
                        //break;
                    }
                    else
                    {
                        //更新结算明细中费用已结金额


                        invoiceDetail.InvPaidAmt = Convert.ToDecimal(((TextBox)gvrDetail.FindControl("txtPayment")).Text) + Convert.ToDecimal(((Label)gvrDetail.FindControl("Label2")).Text.ToString());// Convert.ToDecimal(gvrDetail.Cells[3].Text);
                        invoiceDetail.InvPaidAmtL = invoiceDetail.InvPaidAmt * invPayExRate;
                        if (invoiceDetail.InvPaidAmt == Convert.ToDecimal(((Label)gvrDetail.FindControl("Label1")).Text.ToString()))  //已结算


                        {
                            invoiceDetail.InvDetStatus = InvoiceDetail.INVOICEDETAIL_FULL_BACKING_OUT;
                        }
                        else if (invoiceDetail.InvPaidAmt > 0 && invoiceDetail.InvPaidAmt < Convert.ToDecimal(((Label)gvrDetail.FindControl("Label1")).Text.ToString()))  //部分结算
                        {
                            invoiceDetail.InvDetStatus = InvoiceDetail.INVOICEDETAIL_PART_BACKING_OUT;
                        }
                        else  //未结算


                        {
                            invoiceDetail.InvDetStatus = InvoiceDetail.INVOICEDETAIL_AVAILABILITY;
                        }
                        baseTeans.WhereClause = "InvDetailID=" + Convert.ToInt32(gvrDetail.Cells[0].Text);
                        if (baseTeans.Update(invoiceDetail) < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                            baseTeans.Rollback();
                            return;
                        }
                        surBalAmtSum = Convert.ToDecimal(((TextBox)gvrDetail.FindControl("txtPayment")).Text) + surBalAmtSum;

                        //结算付款单明细


                        InvoicePayDetail invoicePayDetail = new InvoicePayDetail();
                        invoicePayDetail.InvPayDetID = BaseApp.GetInvPayDetID();
                        invoicePayDetail.ChargeTypeID = Convert.ToInt32(gvrDetail.Cells[7].Text);
                        invoicePayDetail.InvPayID = invoicePay.InvPayID;
                        invoicePayDetail.InvID = Convert.ToInt32(ViewState["InvID"]);
                        invoicePayDetail.InvDetailID = Convert.ToInt32(gvrDetail.Cells[0].Text);
                        invoicePayDetail.InvActPayAmt = Convert.ToDecimal(((Label)gvrDetail.FindControl("Label1")).Text);
                        invoicePayDetail.InvActPayAmtL = Convert.ToDecimal(((Label)gvrDetail.FindControl("Label1")).Text) / Convert.ToDecimal(txtInvPayExRate.Text);
                        invoicePayDetail.InvPaidAmt = System.Math.Abs(Convert.ToDecimal(((TextBox)gvrDetail.FindControl("txtPayment")).Text));
                        invoicePayDetail.InvPaidAmtL = System.Math.Abs(Convert.ToDecimal(((TextBox)gvrDetail.FindControl("txtPayment")).Text)) / Convert.ToDecimal(txtInvPayExRate.Text);
                        invoicePayDetail.InvPayDetStatus = InvoicePayDetail.INVOICEPAYDETAIL_AVAILABILITY;
                        invoicePayDetail.PayOutAmtSum = 0;
                        aryList.Add(invoicePayDetail); 
                    }
                }

                //更新结算主表中的已结金额
                invoiceHeader.InvPaidAmt = surBalAmtSum + Convert.ToDecimal(ViewState["invPaidAmt"]);
                invoiceHeader.InvPaidAmtL = invoiceHeader.InvPaidAmt / invPayExRate;
                if (invoiceHeader.InvPaidAmt == Convert.ToDecimal(ViewState["invActPayAmt"]))  //已结算


                {
                    invoiceHeader.InvStatus = InvoiceHeader.INVOICEHEADER_FULL_BACKING_OUT;
                }
                else if (invoiceHeader.InvPaidAmt > 0 && invoiceHeader.InvPaidAmt < Convert.ToDecimal(ViewState["invActPayAmt"]))  //部分结算
                {
                    invoiceHeader.InvStatus = InvoiceHeader.INVOICEHEADER_PART_BACKING_OUT;
                }
                else  //未结算


                {
                    invoiceHeader.InvStatus = InvoiceHeader.INVOICEHEADER_AVAILABILITY;
                }
                baseTeans.WhereClause = "";
                baseTeans.WhereClause = "InvID = " + Convert.ToInt32(ViewState["DetailInvID"]);
                if (baseTeans.Update(invoiceHeader) < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                    baseTeans.Rollback();
                    return;
                }

                #region
                /*余款诋付
            if (Convert.ToInt32(cmbInvPayType.SelectedValue) == InvoicePay.INVPAYTYPE_RESIDUAL_MORTAGAGE)
            {
                //invPaidAmt = Convert.ToDecimal(txtInvPaidAmt.Text);

                //*结算付款单


                invoicePay.CustID = Convert.ToInt32(ViewState["CustID"]);
                invoicePay.ContractID = Convert.ToInt32(ViewState["ContractID"]);
                invoicePay.CreateUserID = sessionUser.UserID;
                invoicePay.CreateTime = DateTime.Now;
                invoicePay.ModifyUserID = sessionUser.UserID;
                invoicePay.ModifyTime = DateTime.Now;
                invoicePay.OprDeptID = sessionUser.DeptID;
                invoicePay.OprRoleID = sessionUser.RoleID;
                invoicePay.RefID = 101;
                invoicePay.InvPayDate = DateTime.Now;
                //invoicePay.InvPayTime = DateTime.Now;
                invoicePay.InvPayType = Convert.ToInt32(cmbInvPayType.SelectedValue);
                invoicePay.InvPayCurID = Convert.ToInt32(cmbCurrencyType.SelectedValue);
                invoicePay.InvPayExRate = Convert.ToDecimal(txtInvPayExRate.Text);
                invoicePay.InvPaidAmt = surBalAmtSum;
                invoicePay.InvPaidAmtL = invPayExRate * Convert.ToDecimal(surBalAmtSum);
                invoicePay.InvPaidAmtSum = surBalAmtSum;
                invoicePay.InvPaidAmtSumL = invPayExRate * Convert.ToDecimal(surBalAmtSum);
                invoicePay.InvSurAmt = surBalAmtSum;
                invoicePay.InvSurAmtL = invPayExRate * Convert.ToDecimal(surBalAmtSum);
                invoicePay.SurProcType = InvoicePay.INVPAYTYPE_SURPROCTYPE_BACKING_OUT_CUST;
                invoicePay.InvPayStatus = InvoicePay.SURBAL_UP_TO_SNUFF;
                invoicePay.Note = txtnote.Text.Trim();

                if (baseTeans.Insert(invoicePay) < 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                    baseTeans.Rollback();
                    return;
                }

                baseBO.WhereClause = "CustID=" + Convert.ToInt32(ViewState["CustID"]) + " and PayOutAmtSum < SurAmt";
                baseBO.OrderBy = "SurDate ASC";
                rs = baseBO.Query(surpluss);
                if (rs.Count > 0)
                {
                    //*余款处理
                    surBal.CreateTime = DateTime.Now;
                    surBal.CreateUserID = sessionUser.UserID;
                    surBal.CustID = Convert.ToInt32(ViewState["CustID"]);
                    surBal.ModifyTime = DateTime.Now;
                    surBal.ModifyUserID = sessionUser.UserID;
                    surBal.OprDeptID = sessionUser.DeptID;
                    surBal.OprRoleID = sessionUser.RoleID;
                    surBal.SurBalAmt = surBalAmtSum;
                    surBal.SurBalAmtL = surBalAmtSum * invPayExRate;
                    surBal.SurBalDate = DateTime.Now;
                    surBal.SurBalID = BaseApp.GetSurBalID();
                    surBal.SurBalInvID = Convert.ToInt32(ViewState["DetailInvID"]);
                    surBal.SurBalStatus = SurBal.SURBAL_UP_TO_SNUFF;
                    surBal.SurBalType = SurBal.SURBAL_MORTAGAGE;

                    invoiceHeader.InvPaidAmt = Convert.ToDecimal(ViewState["HeaderInvPaidAmt"]) + Convert.ToDecimal(surBalAmtSum);
                    baseTeans.WhereClause = "InvID = " + Convert.ToInt32(ViewState["InvID"]);

                    if (baseTeans.Update(invoiceHeader) < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                        baseTeans.Rollback();
                        return;
                    }

                    if (baseTeans.Insert(surBal) < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                        baseTeans.Rollback();
                        return;
                    }

                    foreach (Surplus surplus in rs)
                    {
                        surBalAmtSum = Convert.ToDecimal(surBalAmtSum) - (surplus.SurAmt - surplus.PayOutAmtSum);

                        //*更新余款信息中余款累计金额


                        surplus.PayOutAmtSum = Convert.ToDecimal(surplus.SurAmt) - System.Math.Abs(surBalAmtSum);
                        baseTeans.WhereClause = "SurID = " + surplus.SurID;
                        if (baseTeans.Update(surplus) < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                            baseTeans.Rollback();
                            return;
                        }

                        //*插入余款处理明细
                        surBalDel.SurBalDelID = BaseApp.GetSurBalDelID();
                        surBalDel.SurID = surplus.SurID;
                        surBalDel.SurBalID = surBal.SurBalID;
                        surBalDel.SurCurID = Convert.ToInt32(cmbCurrencyType.SelectedValue);
                        surBalDel.SurExRate = Convert.ToDecimal(txtInvPayExRate.Text);
                        surBalDel.SurBalAmt = Convert.ToDecimal(surplus.SurAmt) - System.Math.Abs(surBalAmtSum);
                        surBalDel.SurBalAmtL = Convert.ToDecimal(surplus.SurAmt) - System.Math.Abs(surBalAmtSum) * Convert.ToDecimal(txtInvPayExRate.Text);
                        if (baseTeans.Insert(surBalDel) < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                            baseTeans.Rollback();
                            return;
                        }

                       //*结算付款单明细



                        invoicePayDetail.InvPayDetID = BaseApp.GetInvPayDetID();
                        invoicePayDetail.ChargeTypeID = invoicePay.InvPayCurID;
                        invoicePayDetail.InvPayID = invoicePay.InvPayID;
                        invoicePayDetail.InvID = Convert.ToInt32(ViewState["InvID"]);
                        invoicePayDetail.InvActPayAmt = Convert.ToDecimal(txtInvPaidAmt.Text);
                        invoicePayDetail.InvActPayAmtL = Convert.ToDecimal(txtInvPaidAmt.Text) * Convert.ToDecimal(txtInvPayExRate.Text);
                        invoicePayDetail.InvPaidAmt = System.Math.Abs(surBalAmtSum);
                        invoicePayDetail.InvPaidAmtL = System.Math.Abs(surBalAmtSum) * Convert.ToDecimal(txtInvPayExRate.Text);
                        invoicePayDetail.InvPayDetStatus = InvoicePayDetail.INVOICEPAYDETAIL_AVAILABILITY;
                        invoicePayDetail.PayOutAmtSum = 0;

                        if (baseTeans.Insert(invoicePayDetail) < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                            baseTeans.Rollback();
                            return;
                        }

                        if (surBalAmtSum < 0 || invPaidAmt == 0)
                        {
                            break;
                        }
                    }
                }
            }*/

                /*代收款诋扣


                else if (Convert.ToInt32(cmbInvPayType.SelectedValue) == InvoicePay.INVPAYTYPE_REPLACE_FUND)
                {

                    //*结算付款单


                    invoicePay.CustID = Convert.ToInt32(ViewState["CustID"]);
                    invoicePay.ContractID = Convert.ToInt32(ViewState["ContractID"]);
                    invoicePay.CreateUserID = sessionUser.UserID;
                    invoicePay.CreateTime = DateTime.Now;
                    invoicePay.ModifyUserID = sessionUser.UserID;
                    invoicePay.ModifyTime = DateTime.Now;
                    invoicePay.OprDeptID = sessionUser.DeptID;
                    invoicePay.OprRoleID = sessionUser.RoleID;
                    invoicePay.RefID = 101;
                    invoicePay.InvPayDate = DateTime.Now;
                    //invoicePay.InvPayTime = DateTime.Now;
                    invoicePay.InvPayType = Convert.ToInt32(cmbInvPayType.SelectedValue);
                    invoicePay.InvPayCurID = Convert.ToInt32(cmbCurrencyType.SelectedValue);
                    invoicePay.InvPayExRate = Convert.ToDecimal(txtInvPayExRate.Text);
                    invoicePay.InvPaidAmt = surBalAmtSum;
                    invoicePay.InvPaidAmtL = Convert.ToDecimal(txtInvPayExRate.Text) * Convert.ToDecimal(surBalAmtSum);
                    invoicePay.InvPaidAmtSum = surBalAmtSum;
                    invoicePay.InvPaidAmtSumL = Convert.ToDecimal(txtInvPayExRate.Text) * Convert.ToDecimal(surBalAmtSum);
                    invoicePay.InvSurAmt = surBalAmtSum;
                    invoicePay.InvSurAmtL = Convert.ToDecimal(txtInvPayExRate.Text) * Convert.ToDecimal(surBalAmtSum);
                    invoicePay.SurProcType = InvoicePay.INVPAYTYPE_SURPROCTYPE_BACKING_OUT_CUST;
                    invoicePay.InvPayStatus = InvoicePay.SURBAL_UP_TO_SNUFF;
                    invoicePay.Note = txtnote.Text.Trim();

                    if (baseTeans.Insert(invoicePay) < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                        baseTeans.Rollback();
                        return;
                    }

                    baseBO.WhereClause = "a.ContractID=b.ContractID and b.ShopID=c.ShopID and a.ContractID=" + Convert.ToInt32(ViewState["ContractID"]) + " and PayOutAmtSum < PayInAmt";
                    baseBO.OrderBy = "PayInDate ASC";
                    rs = baseBO.Query(payIn);
                    if (rs.Count > 0)
                    {

                        //*更新结算单


                        invoiceHeader.InvPaidAmt = Convert.ToDecimal(ViewState["HeaderInvPaidAmt"]) + Convert.ToDecimal(surBalAmtSum);
                        baseTeans.WhereClause = "InvID = " + Convert.ToInt32(ViewState["InvID"]);

                        if (baseTeans.Update(invoiceHeader) < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                            baseTeans.Rollback();
                            return;
                        }

                        foreach (PayIn payIns in rs)
                        {
                            surBalAmtSum = Convert.ToDecimal(surBalAmtSum) - (payIns.PayInAmt - payIns.PayOutAmtSum);

                            //*更新代收款信息累计返还金额


                            payIn.PayOutAmtSum = Convert.ToDecimal(payIns.PayInAmt) - System.Math.Abs(surBalAmtSum);
                            payIn.ModifyTime = DateTime.Now;
                            payIn.ModifyUserID = sessionUser.UserID;

                            if (payIn.PayOutAmtSum < payIns.PayInAmt)
                            {
                                payIn.PayInStatus = PayIn.PAYIN_PART_BALANCE_IN_HAND;
                            }
                            if (payIn.PayOutAmtSum == payIns.PayInAmt)
                            {
                                payIn.PayInStatus = PayIn.PAYIN_BALANCE_IN_HAND;
                            }

                            baseTeans.WhereClause = "PayInID = " + payIns.PayInID;
                            if (baseTeans.Update(payIn) < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                                baseTeans.Rollback();
                                return;
                            }

                            //*插入代收款返还信息


                            payOut.PayOutID = BaseApp.GetPayOutID();
                            payOut.PayInID = payIns.PayInID;
                            payOut.CreateUserID = sessionUser.UserID;
                            payOut.CreateTime = DateTime.Now;
                            payOut.ModifyUserID = sessionUser.UserID;
                            payOut.ModifyTime = DateTime.Now;
                            payOut.OprDeptID = sessionUser.DeptID;
                            payOut.OprRoleID = sessionUser.RoleID;
                            payOut.PayOutAmt = Convert.ToDecimal(payIns.PayInAmt) - System.Math.Abs(surBalAmtSum);
                            payOut.PayOutDate = DateTime.Now;
                            payOut.InvPayID = payIns.PayInID;
                            payOut.PayOutType = PayOut.PAYOUT_MORTAGAGE;
                            payOut.PayOutStatus = PayOut.PAYOUT_UP_TO_SNUFF;

                            if (baseTeans.Insert(payOut) < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                                baseTeans.Rollback();
                                return;
                            }

                            //*结算付款单明细


                            invoicePayDetail.InvPayDetID = BaseApp.GetInvPayDetID();
                            invoicePayDetail.ChargeTypeID = invoicePay.InvPayCurID;
                            invoicePayDetail.InvPayID = invoicePay.InvPayID;
                            invoicePayDetail.InvID = Convert.ToInt32(ViewState["InvID"]);
                            invoicePayDetail.InvActPayAmt = Convert.ToDecimal(txtInvPaidAmt.Text);
                            invoicePayDetail.InvActPayAmtL = Convert.ToDecimal(txtInvPaidAmt.Text) * Convert.ToDecimal(txtInvPayExRate.Text);
                            invoicePayDetail.InvPaidAmt = System.Math.Abs(surBalAmtSum);
                            invoicePayDetail.InvPaidAmtL = System.Math.Abs(surBalAmtSum) * Convert.ToDecimal(txtInvPayExRate.Text);
                            invoicePayDetail.InvPayDetStatus = InvoicePayDetail.INVOICEPAYDETAIL_AVAILABILITY;
                            invoicePayDetail.PayOutAmtSum = 0;

                            if (baseTeans.Insert(invoicePayDetail) < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                                baseTeans.Rollback();
                                return;
                            }

                            if (surBalAmtSum < 0 || invPaidAmt == 0)
                            {
                                break;
                            }
                        }
                    }
                }*/

                /*押金诋付
                else if (Convert.ToInt32(cmbInvPayType.SelectedValue) == InvoicePay.INVPAYTYPE_DEPOSIT_MORTAGAGE)
                {
                    baseBO.WhereClause = "CustID=" + Convert.ToInt32(ViewState["CustID"]) + " and InvPayDetStatus =" +
                                        InvoicePayDetail.INVOICEPAYDETAIL_AVAILABILITY + " or InvPayDetStatus=" + InvoicePayDetail.INVOICEPAYDETAIL_PART_BACKING_OUT +
                                        "and PayOutAmtSum < b.InvPaidAmt";
                    baseBO.OrderBy = "PayOutAmtSum Desc";
                    rs = baseBO.Query(invoicePayDetail);
                    if (rs.Count > 0)
                    {
                        //*押金返还处理

                        depBal.DepBalID = BaseApp.GetDepBalID();
                        depBal.CustID = Convert.ToInt32(ViewState["CustID"]);
                        depBal.CreateTime = DateTime.Now;
                        depBal.CreateUserID = sessionUser.UserID;
                        depBal.ModifyTime = DateTime.Now;
                        depBal.ModifyUserID = sessionUser.UserID;
                        depBal.OprDeptID = sessionUser.DeptID;
                        depBal.OprRoleID = sessionUser.RoleID;
                        depBal.DepBalCurID = Convert.ToInt32(cmbCurrencyType.SelectedValue);
                        depBal.DepBalExRate = Convert.ToDecimal(txtInvPayExRate.Text);
                        depBal.DepBalAmt = surBalAmtSum;
                        depBal.DepBalAmtL = surBalAmtSum * Convert.ToDecimal(txtInvPayExRate.Text);
                        depBal.DepBalType = DepBal.DEPBAL_MORTAGAGE;
                        depBal.Note = txtnote.Text.Trim();

                        invoiceHeader.InvPaidAmt = Convert.ToDecimal(ViewState["HeaderInvPaidAmt"]) + Convert.ToDecimal(surBalAmtSum);
                        baseTeans.WhereClause = "InvID = " + Convert.ToInt32(ViewState["InvID"]);

                        if (baseTeans.Update(invoiceHeader) < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                            baseTeans.Rollback();
                            return;
                        }

                        if (baseTeans.Insert(depBal) < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                            baseTeans.Rollback();
                            return;
                        }

                        foreach (InvoicePayDetail invoicePayDetails in rs)
                        {
                            surBalAmtSum = Convert.ToDecimal(surBalAmtSum) - (invoicePayDetails.InvPaidAmt - invoicePayDetails.PayOutAmtSum);

                            //*更新结算单付款明细信息中付款累计金额

                            invoicePayDetails.PayOutAmtSum = Convert.ToDecimal(invoicePayDetails.InvPaidAmt) - System.Math.Abs(surBalAmtSum);
                            if (surBalAmtSum > 0)
                            {
                                invoicePayDetails.InvPayDetStatus = InvoicePayDetail.INVOICEPAYDETAIL_FULL_BACKING_OUT;
                            }
                            else
                            {
                                invoicePayDetails.InvPayDetStatus = InvoicePayDetail.INVOICEPAYDETAIL_PART_BACKING_OUT;
                            }
                            baseTeans.WhereClause = "InvPayDetID = " + invoicePayDetails.InvPayDetID;
                            if (baseTeans.Update(invoicePayDetails) < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                                baseTeans.Rollback();
                                return;
                            }

                            //*插入押金返还明细
                            depBalDel.DepBalDetID = BaseApp.GetDepBalDetID();
                            depBalDel.InvPayDetID = invoicePayDetails.InvPayDetID;
                            depBalDel.DepBalID = depBal.DepBalID;
                            depBalDel.DepBalCurID = Convert.ToInt32(cmbCurrencyType.SelectedValue);
                            depBalDel.DepBalExRate = Convert.ToDecimal(txtInvPayExRate.Text);
                            depBalDel.DepBalAmt = Convert.ToDecimal(invoicePayDetails.InvPaidAmt) - System.Math.Abs(surBalAmtSum);
                            depBalDel.DepBalAmtL = Convert.ToDecimal(invoicePayDetails.InvPaidAmt) - System.Math.Abs(surBalAmtSum) * Convert.ToDecimal(txtInvPayExRate.Text);

                            if (baseTeans.Insert(depBalDel) < 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                                baseTeans.Rollback();
                                return;
                            }

                            if (surBalAmtSum < 0 || invPaidAmt == 0)
                            {
                                break;
                            }
                        }
                    }
                }*/
                /*其他方式*/
                //else
                //{
                /*if ((Convert.ToDecimal(txtSurAmt.Text) - Convert.ToDecimal(surBalAmtSum)) > 0)
                    {
                        surpluss.SurID = BaseApp.GetSurID();
                        surpluss.CustID = Convert.ToInt32(ViewState["CustID"]);
                        surpluss.BillID = Convert.ToInt32(ViewState["InvID"]);
                        surpluss.BillType = Convert.ToInt32(ViewState["InvType"]);
                        surpluss.SurCurTypeID = Convert.ToInt32(cmbCurrencyType.SelectedValue);
                        surpluss.SurExRate = Convert.ToDecimal(txtInvPayExRate.Text);
                        surpluss.SurAmt = (Convert.ToDecimal(txtSurAmt.Text) - Convert.ToDecimal(surBalAmtSum));
                        surpluss.SurAmtL = (Convert.ToDecimal(txtSurAmt.Text) - Convert.ToDecimal(surBalAmtSum)) * Convert.ToDecimal(txtInvPayExRate.Text);
                        surpluss.SurDate = Convert.ToDateTime(DateTime.Now);
                        surpluss.InvPayType = Convert.ToInt32(cmbInvPayType.SelectedValue);
                        surpluss.PayOutAmtSum = 0;

                        if (baseTeans.Insert(surpluss) < 1)
                        {
                            baseTeans.Rollback();
                            return;
                        }
                    }*/
                #endregion

                //结算付款单


                invoicePay.CustID = Convert.ToInt32(ViewState["CustID"]);
                invoicePay.ContractID = Convert.ToInt32(ViewState["ContractID"]);
                invoicePay.CreateUserID = sessionUser.UserID;
                invoicePay.CreateTime = DateTime.Now;
                invoicePay.ModifyUserID = sessionUser.UserID;
                invoicePay.ModifyTime = DateTime.Now;
                invoicePay.OprDeptID = sessionUser.DeptID;
                invoicePay.OprRoleID = sessionUser.RoleID;
                invoicePay.RefID = 101;
                invoicePay.InvPayDate = Convert.ToDateTime(txtInvPayDate.Text);
                //invoicePay.InvPayTime = DateTime.Now;
                invoicePay.InvPayType = Convert.ToInt32(cmbInvPayType.SelectedValue);
                invoicePay.InvPayCurID = Convert.ToInt32(cmbCurrencyType.SelectedValue);
                invoicePay.InvPayExRate = Convert.ToDecimal(txtInvPayExRate.Text);
                invoicePay.InvPaidAmt = surBalAmtSum;
                invoicePay.InvPaidAmtL = Convert.ToDecimal(surBalAmtSum) / Convert.ToDecimal(txtInvPayExRate.Text);
                invoicePay.InvPaidAmtSum = surBalAmtSum;
                invoicePay.InvPaidAmtSumL = Convert.ToDecimal(surBalAmtSum) / Convert.ToDecimal(txtInvPayExRate.Text);
                invoicePay.InvSurAmt = surBalAmtSum;
                invoicePay.InvSurAmtL = Convert.ToDecimal(surBalAmtSum) / Convert.ToDecimal(txtInvPayExRate.Text);
                invoicePay.SurProcType = InvoicePay.INVPAYTYPE_SURPROCTYPE_BACKING_OUT_CUST;
                invoicePay.InvPayStatus = InvoicePay.SURBAL_UP_TO_SNUFF;
                invoicePay.Note = txtnote.Text.Trim();

                if (Convert.ToInt32(cmbInvPayType.SelectedValue) == InvoicePay.INVPAYTYPE_REPLACE_FUND)
                {
                    int x = PayInPO.InvPayOutAndPayIn(Convert.ToInt32(ViewState["ShopID"]), Convert.ToDecimal(txtInvPaidAmtSum.Text), baseTeans, sessionUser, invoicePay.InvPayID);
                    //*更新代收款信息累计返还金额

                    //DataSet payInDS = PayInPO.GetPayInByShopID(Convert.ToInt32(ViewState["ShopID"]));
                    //int payInCount = payInDS.Tables[0].Rows.Count;
                    //decimal tempPaidAmt = Convert.ToDecimal(txtInvPaidAmtSum.Text);
                    //for (int x = 0; x < payInCount; x++)
                    //{
                    //    decimal tempPayInAmt = Convert.ToDecimal(payInDS.Tables[0].Rows[x]["PayInAmt"]);
                    //    if (tempPaidAmt > 0)
                    //    {
                    //        if (tempPaidAmt < tempPayInAmt)
                    //        {
                    //            payIn.PayOutAmtSum = tempPaidAmt;
                    //            payOut.PayOutAmt = tempPaidAmt;
                    //            tempPaidAmt = tempPaidAmt - tempPaidAmt;
                    //        }
                    //        else
                    //        {
                    //            payOut.PayOutAmt = tempPayInAmt;
                    //            payIn.PayOutAmtSum = tempPayInAmt;
                    //            tempPaidAmt = tempPaidAmt - tempPayInAmt;
                    //        }
                            
                    //        payIn.ModifyTime = DateTime.Now;
                    //        payIn.ModifyUserID = sessionUser.UserID;

                    //        if (payIn.PayOutAmtSum < tempPayInAmt)
                    //        {
                    //            payIn.PayInStatus = PayIn.PAYIN_PART_BALANCE_IN_HAND;
                    //        }
                    //        if (payIn.PayOutAmtSum == tempPayInAmt)
                    //        {
                    //            payIn.PayInStatus = PayIn.PAYIN_BALANCE_IN_HAND;
                    //        }

                    //        baseTeans.WhereClause = "PayInID = " + Convert.ToInt32(payInDS.Tables[0].Rows[x]["PayInID"]);
                    //        if (baseTeans.Update(payIn) < 1)
                    //        {
                    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notUpdateStr + "'", true);
                    //            baseTeans.Rollback();
                    //            return;
                    //        }

                    //        //*插入代收款返还信息
                    //        payOut.PayOutID = BaseApp.GetPayOutID();
                    //        payOut.PayInID = Convert.ToInt32(payInDS.Tables[0].Rows[x]["PayInID"]);
                    //        payOut.CreateUserID = sessionUser.UserID;
                    //        payOut.CreateTime = DateTime.Now;
                    //        payOut.ModifyUserID = sessionUser.UserID;
                    //        payOut.ModifyTime = DateTime.Now;
                    //        payOut.OprDeptID = sessionUser.DeptID;
                    //        payOut.OprRoleID = sessionUser.RoleID;
                    //        payOut.PayOutDate = DateTime.Now;
                    //        payOut.InvPayID = invoicePay.InvPayID;
                    //        payOut.PayOutType = PayOut.PAYOUT_MORTAGAGE;
                    //        payOut.PayOutStatus = PayOut.PAYOUT_UP_TO_SNUFF;

                    //        if (baseTeans.Insert(payOut) < 1)
                    //        {
                    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                    //            baseTeans.Rollback();
                    //            return;
                    //        }
                    //    }
                    //}
                }


                if (surBalAmtSum > 0)
                {
                    if (baseTeans.Insert(invoicePay) < 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                        baseTeans.Rollback();
                        return;
                    }

                    for (int i = 0; i < aryList.Count; i++)
                    {
                        if (baseTeans.Insert((BasePO)aryList[i]) < 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + notInsertStr + "'", true);
                            baseTeans.Rollback();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                baseTeans.Rollback();
                throw ex;
            }
            SetTextEnable(false);
            baseTeans.Commit();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + AddStr + "'", true);
            clear();
        }
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        clear();
        SetTextEnable(false);
    }

    protected void clear()
    {
        txtCustName.Text = "";
        txtContractID.Text = "";
        txtInvPaidAmtSum.Text = "";
        txtInvPaidAmtSum.CssClass = "ipt160px";
        cmbInvPayType.Enabled = true;
        BindInvPayType();
        txtInvPayExRate.Text = "";
        txtSurAmt.Text = "";
        txtInvPaidAmt.Text = "";
        HidnAmt.Value = "";
        txtInvPayDate.Text = "";
        ViewState["ContractID"] = null;
        ViewState["InvID"] = null;
        ViewState["DetailInvID"] = null;
        ViewState["CustID"] = null;
        ViewState["HeaderInvPaidAmt"] = null;
        GrdVewInvoiceDetail.DataSource = null;
        GrdVewInvoiceDetail.DataBind();
        //BindInvoiceDetailNull();
        BindInvoiceDetail();
        BindInvoiceHeader();
    }
    //protected void btnBackType_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
    //    BindInvoiceHeader();
    //}
    //protected void btnNextType_Click(object sender, EventArgs e)
    //{
    //    lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
    //    BindInvoiceHeader();
    //}
    //protected void btnLast_Click(object sender, EventArgs e)
    //{
    //    lblDetailCurrent.Text = Convert.ToString(int.Parse(lblDetailCurrent.Text) - 1);
    //    BindInvoiceDetail();
    //}
    //protected void BtnNext_Click(object sender, EventArgs e)
    //{
    //    lblDetailCurrent.Text = Convert.ToString(int.Parse(lblDetailCurrent.Text) + 1);
    //    BindInvoiceDetail();
    //}

    private void SetTextEnable(bool s)
    {
        cmbInvPayType.Enabled = s;
        txtInvPaidAmtSum.Enabled = s;
        cmbCurrencyType.Enabled = s;
        txtInvPayDate.Enabled = s;
        txtnote.Enabled = s;
        btnSave.Enabled = s;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Invoice/InvoiceHeader/InvoiceHeader.aspx");
    }
    protected void GrdVewInvoiceHeader_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindInvoiceHeader();
        foreach (GridViewRow grv in GrdVewInvoiceHeader.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    protected void GrdVewInvoiceDetail_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        BindInvoiceDetail();
    }

}
