using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Lease.CustLicense;
using Base.Page;
using BaseInfo.User;
using Base.DB;
using Lease.PotCust;
using Lease.ConShop;
using Shop.ShopType;
using BaseInfo.authUser;


public partial class Lease_Customer_CustomerQ : BasePage
{
    Resultset rs = null;
    Resultset rs2 = null;
    private int numCount = 0;
    private int numCountCust = 0;

    public string Tool_UserList;
    public string PotCustomer_Basic;
    public string PotCustomer_ClientCard;
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        if (!this.IsPostBack)
        {
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustName"), "0"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustType"), "1"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotShop_lblMainBrand"), "2"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType"), "3"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Contact"), "4"));
            cmbCustTypeq.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ADDArchives"), "5"));

            TextBox2.Visible = true;
            dropCondit.Visible = false;

            page("Customer.CustID= -1");
            ClearGridSelected();
            Tool_UserList = (String)GetGlobalResourceObject("BaseInfo", "Tool_UserList");
            PotCustomer_Basic = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic");
            PotCustomer_ClientCard = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ClientCard");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Customer_labBusinessManQuery");
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Publicmessage", "Load();", true);
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        //查询
        //lblTotalNum.Text = "1";
        //lblCurrent.Text = "1";
        QueryData();
        ClearGridSelected();

    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        QueryData();
        ClearGridSelected();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        QueryData();
        ClearGridSelected();
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[7].Text = "";
            }
        }
    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect("CustomerInfo.aspx?custID=" + GrdCust.SelectedRow.Cells[0].Text);
    }

    public void BindGridView()
    {
        this.GrdCust.DataSource = new BaseBO().Query(new CustomerQuery());
        this.GrdCust.DataBind();
    }

    //protected void page(string wherestr)
    //{
    //    BaseBO baseBO = new BaseBO();
    //    PagedDataSource pds = new PagedDataSource();
    //    int spareRow = 0;


    //    baseBO.OrderBy = "Customer.CustCode";
    //    baseBO.WhereClause = wherestr;

    //    DataTable dt = baseBO.QueryDataSet(new CustomerInfo()).Tables[0];
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
        DataTable dt = baseBO.QueryDataSet(new CustomerInfo()).Tables[0];
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

    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        sNewStr = sNewStr + "...";
        return sNewStr;
    }

    private void QueryData()
    {
        string whereStr = "";
        string queryValue = cmbCustTypeq.SelectedValue;
        switch (queryValue)
        {
            case "0":   //商户名称
                whereStr = " Customer.custName LIKE '%" + TextBox2.Text + "%' ";
                break;
            case "1":   //商户类型
                whereStr = " Customer.CustTypeID = " + dropCondit.SelectedValue;
                break;
            case "2":   //主营品牌
                whereStr = " EXISTS (SELECT 1 FROM ConShop INNER JOIN Contract ON ConShop.ContractID = Contract.ContractID WHERE Contract.custID = Customer.custID " +
                            "AND ConShop.BrandID = '" + dropCondit.SelectedValue + "')";
                break;
            case "3":   //商铺类型
                whereStr = "EXISTS (SELECT 1 FROM ConShop INNER JOIN Contract ON ConShop.ContractID = Contract.ContractID WHERE Contract.custID = Customer.custID " +
                            "AND ConShop.ShopTypeID =" + dropCondit.SelectedValue + ")";
                break;
            case "4":   //联系人

                whereStr = "EXISTS (SELECT 1 FROM CustContact WHERE CustContact.custID = Customer.custID AND CustContact.ContactMan LIKE '%" + TextBox2.Text + "%')";

                break;
            case "5":  //建档人

                whereStr = "Users.UserName LIKE '%" + TextBox2.Text + "%'";
                break;
            default:
                break;
        }
        ViewState["WhereStr"] = whereStr;
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            whereStr += " AND Customer.custid in ( " + AuthBase.AUTH_SQL_CUST + sessionUser.UserID + ")";
        }
        page(whereStr);
    }
    protected void cmbCustTypeq_SelectedIndexChanged(object sender, EventArgs e)
    {
        string queryValue = cmbCustTypeq.SelectedValue;
        switch (queryValue)
        {
            case "0":   //商户名称
                TextBox2.Visible = true;
                TextBox2.Text = "";
                dropCondit.Visible = false;
                break;
            case "1":   //商户类型
                TextBox2.Visible = false;
                dropCondit.Visible = true;
                BindCustType();
                break;
            case "2":   //主营品牌
                TextBox2.Visible = false;
                dropCondit.Visible = true;
                BindBrand();
                break;
            case "3":   //商铺类型
                TextBox2.Visible = false;
                dropCondit.Visible = true;
                BindShopType();
                break;
            case "4":   //联系人


                TextBox2.Visible = true;
                TextBox2.Text = "";
                dropCondit.Visible = false;
                break;
            case "5":  //建档人


                TextBox2.Visible = true;
                TextBox2.Text = "";
                dropCondit.Visible = false;
                break;
            default:
                break;
        }
        ClearGridSelected();
        //QueryData();
    }

    //绑定商户类型
    private void BindCustType()
    {
        dropCondit.Items.Clear();
        BaseBO baseBo = new BaseBO();
        Resultset custTypeRS = baseBo.Query(new CustType());
        dropCondit.Items.Clear();
        foreach (CustType csType in custTypeRS)
        {
            dropCondit.Items.Add(new ListItem(csType.CustTypeName.ToString(),csType.CustTypeID.ToString()));
        }
    }

    //绑定主营品牌
    private void BindBrand()
    {
        dropCondit.Items.Clear();
        BaseBO baseBo = new BaseBO();
        Resultset brandRS = baseBo.Query(new ConShopBrand());
        dropCondit.Items.Clear();
        foreach (ConShopBrand csBrand in brandRS)
        {
            dropCondit.Items.Add(new ListItem(csBrand.BrandName.ToString(),csBrand.BrandId.ToString()));
        }
    }

    //绑定商铺类型
    private void BindShopType()
    {
        BaseBO baseBO = new BaseBO();
        rs = baseBO.Query(new ShopType());
        dropCondit.Items.Clear();
        foreach (ShopType shopType in rs)
        {
            dropCondit.Items.Add(new ListItem(shopType.ShopTypeName, shopType.ShopTypeID.ToString()));
        }
    }

    private void ClearGridSelected()
    {
        foreach(GridViewRow gvr in GrdCust.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[7].Text = "";
            }
        }
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
