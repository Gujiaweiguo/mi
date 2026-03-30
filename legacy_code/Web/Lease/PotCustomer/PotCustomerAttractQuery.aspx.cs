using System;
using System.Data;
using System.Web.UI.WebControls;
using Base.Biz;
using Base.Page;
using Lease.PotCustLicense;
public partial class Lease_PotCustomer_PotCustomerAttractQuery : BasePage
{
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ViewState["strWhere"] = " and custid =-1";
            this.BindData(ViewState["strWhere"].ToString());
        }
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="strWhere"></param>
    private void BindData(string strWhere)
    {
        BaseBO objBaseBo = new BaseBO();
        PotCustomer objPotCustomer = new PotCustomer();
        
        string strSql = @"select potcustomer.custid,potcustomer.custcode,potcustomer.custname,potcustomer.custshortname,
(select custtypename from custtype where custtype.custtypeid=potcustomer.custtypeid) as custtypename,
(select count(isnull(shopsort,1)) from potshop where potshop.custid=potcustomer.custid) as shopcount
 from potcustomer where 1=1 "+strWhere;
        objPotCustomer.SetQuerySql(strSql);
        DataTable dt = objBaseBo.QueryDataSet(objPotCustomer).Tables[0];
        this.GrdCust.DataSource = dt;
        this.GrdCust.DataBind();
        int spareRow = GrdCust.Rows.Count;
        for (int i = 0; i < GrdCust.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        GrdCust.DataSource = dt;
        GrdCust.DataBind();
    }
    protected void GrdCust_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindData(ViewState["strWhere"].ToString());
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string strWhere = "";
        if (this.rbtCustCode.Checked == true)
        {
            if (this.txtCode.Text.Trim() != "")
            {
                strWhere += " and custcode like '%" + this.txtCode.Text.Trim() + "%'";
            }
        }
        if (this.rbtCustName.Checked == true)
        {
            if (this.txtName.Text.Trim() != "")
            {
                strWhere += " and CustName like '%" + this.txtName.Text.Trim() + "%'";
            }
        }
        if (this.rbtCustShortName.Checked == true)
        {
            if (this.txtShortName.Text.Trim() != "")
            {
                strWhere += " and CustShortName like '%" + this.txtShortName.Text.Trim() + "%'";
            }
        }
        ViewState["strWhere"]=strWhere;
        this.BindData(ViewState["strWhere"].ToString());
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[6].Text = "";
            }
            else if(e.Row.RowIndex>=0)
            {
                e.Row.Cells[6].Text = "<a href=PotCustomerAttractDetail.aspx?CustID="+e.Row.Cells[0].Text+">选择</a>";
            }
        }
    }
}
