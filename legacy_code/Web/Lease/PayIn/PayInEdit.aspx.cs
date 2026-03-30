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
using Lease.PayIn;
using BaseInfo.User;
using Base.Page;
using Lease.ConShop;

public partial class Lease_PayIn_PayInEdit : BasePage
{
    public string baseInfo;  //基本信息
    protected void Page_Load(object sender, EventArgs e)
    {
        txtShopCode.Attributes.Add("onclick", "ShowTree()");
        if (!this.IsPostBack)
        {
            BindDropPayInDataSource();
            BindDropPayInStatus();
            SetControls(true);

            btnEdit.Enabled = false;
            btnSave.Enabled = false;

            //输入控制
            txtPayInAmt.Attributes.Add("onkeydown", "textleave()");

            ViewState["currentCount"] = 1;
            page();
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Lease_lblPayInEdit");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "PayInID = " + Convert.ToInt32(ViewState["payinID"]);
        PayIn payin = new PayIn();
        //数据源是系统自动生成
        if (dropPayInDataSource.SelectedValue == PayIn.PAYINDATASOURCE_SYS.ToString())
        {
            payin.PaidAmt = Convert.ToDecimal(txtPayInAmt.Text);
            payin.PayInDate = Convert.ToDateTime(txtPayInDate.Text);
            payin.PayInEndDate = Convert.ToDateTime(txtPayInEndDate.Text);
            payin.PayInStartDate = Convert.ToDateTime(txtPayInStartDate.Text);
            payin.PayInStatus = Convert.ToInt32(dropPayInStatus.SelectedValue);
            payin.ModifyTime = DateTime.Now;
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            payin.ModifyUserID = objSessionUser.UserID;

            dropPayInStatus.Enabled = false;
            dropPayInStatus.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        }
        //数据源是手工录入
        if (dropPayInDataSource.SelectedValue == PayIn.PAYINDATASOURCE_HAND.ToString())
        {
            payin.PaidAmt = Convert.ToDecimal(txtPayInAmt.Text);
            payin.PayInDate = Convert.ToDateTime(txtPayInDate.Text);
            payin.PayInEndDate = Convert.ToDateTime(txtPayInEndDate.Text);
            payin.PayInStartDate = Convert.ToDateTime(txtPayInStartDate.Text);
            payin.PayInStatus = Convert.ToInt32(dropPayInStatus.SelectedValue);
            payin.ModifyTime = DateTime.Now;
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            payin.ModifyUserID = objSessionUser.UserID;

            txtPayInAmt.Enabled = true;
            txtPayInDate.Enabled = true;
            txtPayInEndDate.Enabled = true;
            txtPayInStartDate.Enabled = true;

            txtPayInAmt.BackColor = System.Drawing.Color.FromName("#F5F5F4");
            txtPayInDate.BackColor = System.Drawing.Color.FromName("#F5F5F4");
            txtPayInEndDate.BackColor = System.Drawing.Color.FromName("#F5F5F4");
            txtPayInStartDate.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        }
        if (baseBo.Update(payin) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdateLost") + "'", true);
        }

        btnEdit.Enabled = true;
        btnSave.Enabled = false;
        SetControlCol();
        page();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        SetControlCol();
        //数据源是系统自动生成
        if (dropPayInDataSource.SelectedValue == PayIn.PAYINDATASOURCE_SYS.ToString())
        {
            dropPayInStatus.Enabled = true;
            dropPayInStatus.BackColor = System.Drawing.Color.White;
        }
        //数据源是手工录入
        if (dropPayInDataSource.SelectedValue == PayIn.PAYINDATASOURCE_HAND.ToString())
        {
            txtPayInAmt.Enabled = true;
            txtPayInDate.Enabled = true;
            txtPayInEndDate.Enabled = true;
            txtPayInStartDate.Enabled = true;

            txtPayInAmt.BackColor = System.Drawing.Color.White;
            txtPayInDate.BackColor = System.Drawing.Color.White;
            txtPayInEndDate.BackColor = System.Drawing.Color.White;
            txtPayInStartDate.BackColor = System.Drawing.Color.White;
        }
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        page();
    }
    protected void btnCel_Click(object sender, EventArgs e)
    {
        SetControlsEmpty();
        txtPayInCode.Text = "";
        //txtShopCode.Text = "";
        //allvalue.Value = "";
        SetControlCol();
        btnEdit.Enabled = false;
        btnSave.Enabled = false;
        page();
    }
    //protected void btnBack_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) - 1);
    //    SetControlCol();
    //    page();
    //}
    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    ViewState["currentCount"] = Convert.ToInt32(Convert.ToInt32(ViewState["currentCount"]) + 1);
    //    SetControlCol();
    //    page();
    //}
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        ViewState["shopID"] = null;
        ViewState["payInCode"] = null;
        if ((txtPayInCode.Text != "") || (txtShopCode.Text != ""))
        {
            SetControlsEmpty();
            BaseBO baseBo = new BaseBO();
            string sql = "select A.CustName,B.ContractID,B.ContractCode,C.ShopID,C.ShopCode,D.PayInCode,D.PayInID,D.PayInStartDate,D.PayInEndDate,D.PayInDate,D.PayInDataSource,D.PayInAmt,D.PaidAmt from Customer A,Contract B,ConShop C,PayIn D" +
                            " where A.CustID = B.CustID and B.ContractID = C.ContractID and C.ShopID = D.ShopID and D.PayInStatus in (" + PayIn.PAYINSTATRS_NOINV + "," + PayIn.PAYINSTATRS_HALFINV + ")";
            if (txtPayInCode.Text.Trim() != "")
            {
                sql = sql + " and D.PayInCode = '" + txtPayInCode.Text + "'";
            }
            //if (txtShopCode.Text.Trim() != "")
            //{
            //    sql = sql + " and C.ShopCode = '" + txtShopCode.Text + "'";
            //}
            if (allvalue.Value != "")
            {
                sql = sql + " and C.ShopID = '" + allvalue.Value + "'";
            }
            DataSet ds = baseBo.QueryDataSet(sql);
            if (ds.Tables[0].Rows.Count >= 1)
            {
                if (txtPayInCode.Text != "")
                {
                    ViewState["payInCode"] = ds.Tables[0].Rows[0]["PayInCode"].ToString();
                }
                if (txtShopCode.Text != "")
                {
                    ViewState["shopID"] = ds.Tables[0].Rows[0]["ShopID"].ToString();
                }
                ViewState["payinID"] = ds.Tables[0].Rows[0]["PayInID"].ToString();
                txtPayInCodeF.Text = ds.Tables[0].Rows[0]["PayInCode"].ToString();
                try { txtPayInStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PayInStartDate"].ToString()).ToShortDateString(); }
                catch { }
                try { txtPayInEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PayInEndDate"].ToString()).ToShortDateString(); }
                catch { }
                try { txtPayInDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["PayInDate"].ToString()).ToShortDateString(); }
                catch { }
                dropPayInDataSource.SelectedValue = ds.Tables[0].Rows[0]["PayInDataSource"].ToString();
                txtPayInAmt.Text = ds.Tables[0].Rows[0]["PaidAmt"].ToString();
                txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
                txtContractCode.Text = ds.Tables[0].Rows[0]["ContractCode"].ToString();
                txtShopCodeF.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "msg_NotFindData") + "'", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") + "'", true);
        }
        SetControlCol();
        page();
    }


    /// <summary>
    /// 绑定数据来源
    /// </summary>
    private void BindDropPayInDataSource()
    {
        dropPayInDataSource.Items.Clear();
        int[] payInDataSource = PayIn.GetPayInDataSource();
        int s = payInDataSource.Length;
        for (int i = 0; i < s; i++)
        {
            dropPayInDataSource.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter",PayIn.GetPayInDataSourceDesc(payInDataSource[i])), payInDataSource[i].ToString()));
        }
    }

    /// <summary>
    /// 绑定代收款状态



    /// </summary>
    private void BindDropPayInStatus()
    {
        dropPayInStatus.Items.Clear();
        int[] payInStatus = PayIn.GetPayInStatus();
        int s = payInStatus.Length;
        for (int i = 0; i < s; i++)
        {
            dropPayInStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("parameter",PayIn.GetPayInStatusDesc(payInStatus[i])), payInStatus[i].ToString()));
        }
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        if ((ViewState["shopID"] != "") && (ViewState["shopID"] != null))
        {
            baseBO.WhereClause = " ShopID = '" + ViewState["shopID"] + "' and ";
        }
        baseBO.WhereClause += " 1 = 1 ";
        if ((ViewState["payInCode"] != "") && (ViewState["payInCode"] != null))
        {
            baseBO.WhereClause += " and PayInCode = '" + ViewState["payInCode"] + "'";
        }
        baseBO.WhereClause += " and PayInStatus in (" + PayIn.PAYINSTATRS_NOINV + "," + PayIn.PAYINSTATRS_HALFINV + ")";
        if (((ViewState["shopID"] == "") || (ViewState["shopID"] == null)) && ((ViewState["payInCode"] == "") || (ViewState["payInCode"] == null)))
        {
            baseBO.WhereClause = " 1 = 0";
        }

        DataSet ds = baseBO.QueryDataSet(new PayIn());
        dt = ds.Tables[0];
        int count = ds.Tables[0].Rows.Count;
        int ss = 0;

        for (int j = 0; j < count; j++)
        {
            dt.Rows[j]["PayInStatusName"] = (String)GetGlobalResourceObject("Parameter", PayIn.GetPayInStatusDesc(Convert.ToInt32(dt.Rows[j]["PayInStatus"])));
        }

        pds.DataSource = dt.DefaultView;
        if (pds.Count < 1)
        {
            for (int i = 0; i < 10; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
        }
        else
        {
            //gdvwPayIn.EmptyDataText = "";
            //pds.AllowPaging = true;
            //pds.PageSize = 10;
            //pds.CurrentPageIndex = Convert.ToInt32(ViewState["currentCount"]) - 1;
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
            gdvwPayIn.DataSource = pds;
            //gdvwPayIn.DataBind();

            //ss = gdvwPayIn.Rows.Count;
            //for (int i = 0; i < pds.PageSize - ss; i++)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //pds.DataSource = dt.DefaultView;
            gdvwPayIn.DataBind();
            ss = gdvwPayIn.Rows.Count;
            for (int i = 0; i < gdvwPayIn.PageSize - ss; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }


        }
        gdvwPayIn.DataSource = pds;
        gdvwPayIn.DataBind();
        for (int j = 0; j < gdvwPayIn.PageSize - ss; j++)
            gdvwPayIn.Rows[(gdvwPayIn.PageSize - 1) - j].Cells[4].Text = "";
    }

    private void SetControls(bool s)
    {
        SetControlCol();
        txtCustName.Enabled = !s;
        txtPayInAmt.Enabled = !s;
        txtPayInCodeF.Enabled = !s;
        txtPayInDate.Enabled = !s;
        txtPayInEndDate.Enabled = !s;
        txtPayInStartDate.Enabled = !s;
        txtContractCode.Enabled = !s;
        dropPayInDataSource.Enabled = !s;
        dropPayInStatus.Enabled = !s;
        txtShopCodeF.Enabled = !s;
    }

    private void SetControlsEmpty()
    {
        //txtPayInCode.Text = "";
        //txtShopCode.Text = "";
        txtCustName.Text = "";
        txtPayInAmt.Text = "";
        txtPayInCodeF.Text = "";
        txtPayInDate.Text = "";
        txtPayInEndDate.Text = "";
        txtPayInStartDate.Text = "";
        txtContractCode.Text = "";
        txtShopCodeF.Text = "";
        BindDropPayInDataSource();
        BindDropPayInStatus();
    }

    private void SetControlCol()
    {
        txtCustName.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        txtPayInAmt.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        txtPayInCodeF.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        txtPayInDate.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        txtPayInEndDate.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        txtPayInStartDate.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        txtContractCode.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        dropPayInDataSource.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        dropPayInStatus.BackColor = System.Drawing.Color.FromName("#F5F5F4");
        txtShopCodeF.BackColor = System.Drawing.Color.FromName("#F5F5F4");
    }

    protected void gdvwPayIn_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["shopID"] = null;
        ViewState["payInCode"] = null;

        BaseBO baseBo = new BaseBO();
        int payInID = Convert.ToInt32(gdvwPayIn.SelectedRow.Cells[0].Text);
        int shopID = Convert.ToInt32(gdvwPayIn.SelectedRow.Cells[9].Text);

        txtPayInCodeF.Text = gdvwPayIn.SelectedRow.Cells[1].Text;
        txtPayInStartDate.Text = Convert.ToDateTime(gdvwPayIn.SelectedRow.Cells[5].Text).ToString("yyyy-MM-dd");
        txtPayInEndDate.Text = Convert.ToDateTime(gdvwPayIn.SelectedRow.Cells[6].Text).ToString("yyyy-MM-dd");
        txtPayInDate.Text = Convert.ToDateTime(gdvwPayIn.SelectedRow.Cells[7].Text).ToString("yyyy-MM-dd");
        dropPayInDataSource.SelectedValue = gdvwPayIn.SelectedRow.Cells[8].Text;
        txtPayInAmt.Text = gdvwPayIn.SelectedRow.Cells[2].Text;
       // dropPayInStatus.SelectedValue = gdvwPayIn.SelectedRow.Cells[10].Text;
        ViewState["payinID"] = payInID;

        //查询合同号、客户名称、商铺号 

        string sql = "select A.CustName,B.ContractCode,C.ShopCode from Customer A,Contract B,ConShop C,PayIn D" +
                            " where A.CustID = B.CustID and B.ContractID = C.ContractID and C.ShopID = D.ShopID and D.ShopID = " + shopID + 
                            " and PayInID = " + payInID;
        DataSet ds = baseBo.QueryDataSet(sql);
        txtCustName.Text = ds.Tables[0].Rows[0]["CustName"].ToString();
        txtContractCode.Text = ds.Tables[0].Rows[0]["ContractCode"].ToString();
        txtShopCodeF.Text = ds.Tables[0].Rows[0]["ShopCode"].ToString();
        btnEdit.Enabled = true;
        SetControlCol();
        if (txtPayInCode.Text != "")
        {
            ViewState["payInCode"] = gdvwPayIn.SelectedRow.Cells[1].Text;
        }
        if (txtShopCode.Text != "")
        {
            ViewState["shopID"] = gdvwPayIn.SelectedRow.Cells[9].Text;
        }
        page();
    }

    protected void gdvwPayIn_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        page();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        DataSet ds = ConShopPO.GetConShopByID(Convert.ToInt32(allvalue.Value));
        if (ds.Tables[0].Rows.Count == 1)
        {
            this.txtShopCode.Text = ds.Tables[0].Rows[0]["ShopName"].ToString();
        }
        page();
    }
}
