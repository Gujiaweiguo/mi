using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Lease.PotCustLicense;
using Base.Page;
using Base.DB;
using Lease.PotCust;

public partial class Lease_PotCustomer_PotCustomerListUpdate : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;

    public string baseInfo;
    public string strFresh;
    public string Tool_UserList;
    public string PotCustomer_Basic;
    public string PotCustomer_ClientCard;
    public string PotCustomer_TitlePalaver;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //BaseBO baseBO = new BaseBO();
            //baseBO.WhereClause = "CustTypeStatus=" + CustType.CUST_TYPE_STATUS_VALID;
            //rs = baseBO.Query(new CustType());
            //foreach (CustType custtype in rs)
            //{
            //    cmbCustTypeq.Items.Add(new ListItem(custtype.CustTypeName, custtype.CustTypeID.ToString()));
            //}
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "LeaseAreaType_CustCode"), "0"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustName"), "1"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ADDArchives"), "2"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustType"), "3"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustShortName"), "4"));

            //绑定客户类型
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "CustTypeStatus=" + CustType.CUST_TYPE_STATUS_VALID;
            rs = baseBO.Query(new CustType());
            foreach (CustType custtype in rs)
            {
                dropCustType.Items.Add(new ListItem(custtype.CustTypeName, custtype.CustTypeID.ToString()));
            }


            page("a.CustID= 0");

            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_labCustomerUptate");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            Tool_UserList = (String)GetGlobalResourceObject("BaseInfo", "Tool_UserList");
            PotCustomer_Basic = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic");
            PotCustomer_ClientCard = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ClientCard");
            PotCustomer_TitlePalaver = (String)GetGlobalResourceObject("BaseInfo", "Title_Palaver");
            this.Form.DefaultButton = "btnQuery";
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "Load();", true);
        }
    }

    public void BindGridView()
    {
        this.GrdCust.DataSource = new BaseBO().Query(new PotCustomerQuery());
        this.GrdCust.DataBind();
    }

    //protected void page(string wherestr)
    //{
    //    BaseBO baseBO = new BaseBO();
    //    PagedDataSource pds = new PagedDataSource();
    //    int spareRow = 0;

    //    baseBO.WhereClause = wherestr;

    //    DataTable dt = baseBO.QueryDataSet(new PotCustomerInfo()).Tables[0];
    //    pds.DataSource = dt.DefaultView;

    //    if (pds.Count <= 1)
    //    {
    //        for (int i = 0; i < GrdCust.PageSize; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;
    //        GrdCust.DataSource = pds;
    //        GrdCust.DataBind();

    //        btnBack.Enabled = false;
    //        btnNext.Enabled = false;
    //    }
    //    else
    //    {
    //        GrdCust.EmptyDataText = "";
    //        pds.AllowPaging = true;
    //        pds.PageSize = 10;
    //        lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
    //        pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
    //        if (pds.IsFirstPage)
    //        {
    //            btnBack.Enabled = false;
    //            btnNext.Enabled = true;
    //        }

    //        if (pds.IsLastPage)
    //        {
    //            btnBack.Enabled = true;
    //            btnNext.Enabled = false;
    //        }

    //        if (pds.IsFirstPage && pds.IsLastPage)
    //        {
    //            btnBack.Enabled = false;
    //            btnNext.Enabled = false;
    //        }

    //        if (!pds.IsLastPage && !pds.IsFirstPage)
    //        {
    //            btnBack.Enabled = true;
    //            btnNext.Enabled = true;
    //        }

    //        this.GrdCust.DataSource = pds;
    //        this.GrdCust.DataBind();
    //        spareRow = GrdCust.Rows.Count;
    //        for (int i = 0; i < pds.PageSize - spareRow; i++)
    //        {
    //            dt.Rows.Add(dt.NewRow());
    //        }
    //        pds.DataSource = dt.DefaultView;
    //        GrdCust.DataSource = pds;
    //        GrdCust.DataBind();
    //    }

    //}
    protected void page(string strWhere)
    {
        int spareRow = 0;
        BaseBO baseBO = new BaseBO();
        PagedDataSource pds = new PagedDataSource();
        baseBO.WhereClause = strWhere;
        baseBO.OrderBy = "CustCode Asc";
        DataTable dt = baseBO.QueryDataSet(new PotCustomerInfo()).Tables[0];
        //for (int j = 0; j < dt.Rows.Count; j++)
        //{
        //    string customerStatus = (String)GetGlobalResourceObject("Parameter", PotCustomer.GetCustTypeStatusDesc(Convert.ToInt32(dt.Rows[j]["CustomerStatus"])));
        //    dt.Rows[j]["CustomerStatus"] = customerStatus;
        //}
        pds.DataSource = dt.DefaultView;
        GrdCust.DataSource = pds;
        GrdCust.DataBind();
        spareRow = GrdCust.Rows.Count;
        for (int i = 0; i < GrdCust.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdCust.DataSource = pds;
        GrdCust.DataBind();
    }

    private void QueryData()
    {
        string whereStr = "";
        string queryValue = cmbCustTypeq.SelectedValue;
        switch (queryValue)
        {
            case "0":   //客户号

                whereStr = "a.CustCode like '%" + TextBox2.Text + "%'";
                break;
            case "1":   //客户名称
                whereStr = "a.CustName like '%" + TextBox2.Text + "%'";
                break;
            case "2":  //建档人

                whereStr = "d.UserName like '%" + TextBox2.Text + "%'";
                break;
            case "3":  //客户类型
                whereStr = "a.CustTypeID like '%" + dropCustType.SelectedValue + "%'";
                break;
            case "4":  //客户简称

                whereStr = "a.CustShortName like '%" + TextBox2.Text + "%'";
                break;
            default:
                break;
        }
        ViewState["WhereStr"] = whereStr;
        page(whereStr);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        QueryData();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        QueryData();
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[6].Text = "";
            }
        }
    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
         Response.Redirect("PotCustomerBaseInfoUpdate.aspx?custID=" + GrdCust.SelectedRow.Cells[0].Text);
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //lblTotalNum.Text = "1";
        //lblCurrent.Text = "1";
        QueryData();
    }
    protected void cmbCustTypeq_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox2.Text = "";
        string queryValue = cmbCustTypeq.SelectedValue;
        switch (queryValue)
        {
            case "0":
            case "1":
            case "2":
            case "4":
                dropCustType.Visible = false;
                TextBox2.Visible = true;
                break;
            case "3":
                TextBox2.Visible = false;
                dropCustType.Visible = true;
                break;
            default:
                break;
        }
        //lblTotalNum.Text = "1";
        //lblCurrent.Text = "1";
        page("1=0");
    }
    protected void GrdCust_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
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
        page(ViewState["WhereStr"].ToString());
    }
}
