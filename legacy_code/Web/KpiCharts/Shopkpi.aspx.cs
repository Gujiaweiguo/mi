using System;
using Base.DB;
using Base;
using Base.Biz;

using System.Data;

public partial class KpiCharts_Shopkpi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["ShopID"] != null && Request.QueryString["ShopID"] != "")
            {
                int shopid=Convert.ToInt32(Request.QueryString["ShopID"].ToString());
                GetContractInfo(shopid);
            }
        }
    }
    /// <summary>
    /// 获得合同信息
    /// </summary>
    /// <param name="shopid">商铺id</param>
    private void GetContractInfo(int shopid)
    {
        BaseBO baseBo = new BaseBO();
        string strsql = "select conshop.shopname,conshopbrand.brandname,contract.contractcode,contracttypename,customer.custname," +
                      "contract.constartdate,contract.conenddate from conshop " +
                      "inner join contract on (contract.contractid=conshop.contractid) " +
                      "inner join customer on (contract.custid=customer.custid) " +
                      "inner join contracttype on (contract.contracttypeid=contracttype.contracttypeid) " +
                      "left join conshopbrand on (conshop.brandid=conshopbrand.brandid) " +
                      "where conshop.shopid=" + shopid.ToString();
        DataSet ds = baseBo.QueryDataSet(strsql);
        if (ds.Tables[0].Rows.Count == 1)
        {
            this.contractcode.Text  = ds.Tables[0].Rows[0]["contractcode"].ToString();
            this.custname.Text = ds.Tables[0].Rows[0]["custname"].ToString();
            this.shopname.Text = ds.Tables[0].Rows[0]["shopname"].ToString();
            this.contrcttype.Text = ds.Tables[0].Rows[0]["contracttypename"].ToString();
            this.brandname.Text = ds.Tables[0].Rows[0]["brandname"].ToString();
            this.startdate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["constartdate"].ToString()).ToString("yyyy-MM-dd") ;
            this.enddate.Text =Convert.ToDateTime( ds.Tables[0].Rows[0]["conenddate"].ToString()).ToString("yyyy-MM-dd");

            this.lblShopName1.Text ="商铺名称:" + ds.Tables[0].Rows[0]["shopname"].ToString();
            this.lblShopName2.Text = "商铺名称:" + ds.Tables[0].Rows[0]["shopname"].ToString();
        }
    }
    /// <summary>
    /// 获得商铺销售数据
    /// </summary>
    /// <param name="shopid">商铺id</param>
    private void GetSalesInfo(int shopid)
    { }
    /// <summary>
    /// 获得商铺费用数据
    /// </summary>
    /// <param name="shopid">商铺id</param>
    private void GetInvInfo(int shopid)
    { }
}
