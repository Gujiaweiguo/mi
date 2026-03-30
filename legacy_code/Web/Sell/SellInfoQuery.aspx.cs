using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;

using Base.Biz;
using Base.Page;
using BaseInfo.User;
using Shop.ShopTreeOperate;
using Base.DB;
using BaseInfo.authUser;
using RentableArea;
using BaseInfo.Dept;

public partial class Sell_SellInfoQuery : BasePage
{
    public string strBaseinfo = "";
    public string fresh = "";
    private string SqlStr;
    protected void Page_Load(object sender, EventArgs e)
    {        
        for (int i = 0; i < CBList.Items.Count; i++)
        {
            if (CBList.Items[i].Selected == true)
            {
                ShopView.Columns[i+1].Visible = true;
            }
            else
            {
                ShopView.Columns[i+1].Visible = false;
            }
        }
        if (RbtDay.Checked == true)
        {
            SqlStr = "select store.storename,contract.contractcode,customer.custshortname,a.shopcode,a.shopname,a.shoptypename,a.tradename,a.brandname,convert(varchar(10),a.bizdate,120) bizdate,a.paidamt,'' as skudesc,'' as datasource,trafficdata.innum " +
                    "from transshopday a " +
                    "left join conshop on conshop.shopid=a.shopid " +
                    "left join contract on contract.contractid=conshop.contractid " +
                    "left join customer on contract.custid=customer.custid " +
                    "left join store on store.storeid=a.storeid " +
                    "left join (select storeid,bizdate,sum(innum) as innum from trafficdata group by bizdate,storeid) trafficdata on (trafficdata.storeid=a.storeid and trafficdata.bizdate=a.bizdate)" +
                    "where a.bizdate ";
            ShopView.Columns[12].Visible = false;
            ShopView.Columns[11].Visible = false;
            ShopView.Columns[13].Visible = false;
        }
        else if (RbtMtn.Checked == true)
        {
            SqlStr = "select store.storename,contract.contractcode,customer.custshortname,a.shopcode,a.shopname,a.shoptypename,a.tradename,a.brandname,convert(varchar(7),a.month,120) bizdate,a.paidamt,'' as skudesc,'' as datasource,trafficdata.innum " +
                    "from transshopmth a " +
                    "left join conshop on conshop.shopid=a.shopid " +
                    "left join contract on contract.contractid=conshop.contractid " +
                    "left join customer on contract.custid=customer.custid " +
                    "left join store on store.storeid=a.storeid "+
                    "left join (select storeid,bizdate,sum(innum) as innum from trafficdata group by bizdate,storeid) trafficdata on (trafficdata.storeid=a.storeid and convert(varchar(7),trafficdata.bizdate,120)=convert(varchar(7),a.month,120))" +
                    "where a.month ";
            ShopView.Columns[12].Visible = false;
            ShopView.Columns[11].Visible = false;
            ShopView.Columns[13].Visible = false;
        }
        else if (RbtDetail.Checked == true)
        {
            SqlStr = "select store.storename,contract.contractcode,customer.custshortname,conshop.shopcode,a.shopname,shoptype.shoptypename,a.trade2name tradename,a.brandname,convert(varchar(10),a.bizdate,120) bizdate,a.paidamt,a.skudesc,(case a.datasource when '1' then 'POS销售' when '2' then '导入销售' when '3' then '手工录入' end) datasource,trafficdata.innum " +
                        "from transsku a " +
                        "left join conshop on conshop.shopid=a.shopid " +
                        "left join shoptype on conshop.shoptypeid=shoptype.shoptypeid " +
                        "left join contract on contract.contractid=conshop.contractid " +
                        "left join customer on contract.custid=customer.custid " +
                        "left join store on store.storeid=a.storeid "+
                        "left join (select storeid,bizdate,sum(innum) as innum from trafficdata group by bizdate,storeid) trafficdata on (trafficdata.storeid=a.storeid and trafficdata.bizdate=a.bizdate)" +
                        "where a.bizdate ";
            ShopView.Columns[13].Visible = false;
        }
        else if (RbtTotal.Checked == true)
        {
            SqlStr = "select store.storename,contract.contractcode,customer.custshortname,conshop.shopcode,a.shopname,shoptype.shoptypename,a.trade2name tradename," +
                        "a.brandname,'' bizdate,sum(a.paidamt) paidamt,'' as skudesc,'' as datasource,trafficdata.innum " +
                        "from transsku a " +
                        "left join conshop on conshop.shopid=a.shopid " +
                        "left join contract on contract.contractid=conshop.contractid " +
                        "left join customer on contract.custid=customer.custid " +
                        "left join store on store.storeid=a.storeid " +
                        "left join shoptype on shoptype.shoptypeid=conshop.shoptypeid "+
                        "left join (select storeid,bizdate,sum(innum) as innum from trafficdata group by bizdate,storeid) trafficdata on (trafficdata.storeid=a.storeid and trafficdata.bizdate between '" + TextBox1.Text + "' and '" + TextBox2.Text + "')" +
                        "where a.bizdate ";
        }
        ShopView.Columns[0].Visible = true;
        ShopView.Columns[1].Visible = true;
        ShopView.Columns[5].Visible = true;
        ShopView.Columns[10].Visible = true;
        ShopView.Columns[9].Visible = true;
        BtnSave.Enabled = false;
        if (!this.IsPostBack)
        {            
            strBaseinfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_SellInfoQuery");
            fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.TextBox1.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.TextBox2.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            Session["whereSqlStr"] = " between '" + TextBox1.Text + "' and '" + TextBox2.Text + "' and 1=2";
            this.BindDate(SqlStr);
            ShowShopTree();
            for (int i = 0; i < CBList.Items.Count; i++)
            {
                CBList.Items[i].Selected = true;
            }
        }
    }
    public void ShowShopTree()
    {
        //string strSql = "select StoreId,FloorID,BuildingID,shopid,shopcode,shopname,convert(char(10),getdate(),120) as Date,0 as Cash,0 as BankCard from conshop  where  EXISTS (SELECT tenantid FROM skumaster WHERE  skumaster.tenantid=conshop.shopid)";
        //ShopTreeOperate objShopTreeOperate = new ShopTreeOperate(false, sessionUser.UserID, strSql);

        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseBO objBase = new BaseBO();
        Resultset rs = new Resultset();
        Dept objDept = new Dept();
        objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        string jsdept = "";
        rs = objBase.Query(objDept);
        if (rs.Count == 1)
        {
            objDept = rs.Dequeue() as Dept;
            jsdept = objDept.DeptID + "|" + "0" + "|" + objDept.DeptName + "^";
        }
        else
        {
            jsdept = "";
        }
        objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;//商业项目
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBase.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = objBase.Query(objDept);
        if (rs.Count > 0)
        {
            foreach (Dept store in rs)
            {
                jsdept += store.DeptID + "|" + objDept.DeptID + "|" + store.DeptName + "|" + 0 + "^";
                objBase.WhereClause = "StoreId=" + store.DeptID;
                rs = objBase.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        //jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                        objBase.WhereClause = "floors.BuildingID=" + building.BuildingID;

                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            objBase.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        }
                        rs = objBase.Query(new floorsAuth());
                        foreach (floorsAuth floors in rs)
                        {
                            jsdept += store.DeptID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + "|" + floors.FloorName + "|" + 0 + "^";
                            //if (this.bShowLocation)//显示方位
                            //{
                            //objBase.WhereClause = "FloorID=" + floors.FloorID;
                            //rs = objBase.Query(new Location());
                            //foreach (Location location in rs)
                            //{
                            //    jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "|" + 0 + "^";
                            //    DataSet ds = objBase.QueryDataSet("select storeid,buildingid,floorid,shopid,shopcode,shopname from conshop where conshop.ShopStartDate<='" + DateTime.Now.ToString() + "' and conshop.ShopEndDate >= '" + DateTime.Now.ToString() + "' and conshop.StoreId=" + store.DeptID + " and conshop.FloorID=" + floors.FloorID + " and conshop.BuildingID=" + building.BuildingID + " and conshop.locationid=" + location.LocationID);
                            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            //    {
                            //        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + ds.Tables[0].Rows[i]["ShopID"].ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + ds.Tables[0].Rows[i]["shopcode"].ToString() + " " + ds.Tables[0].Rows[i]["ShopName"].ToString() + "|" + 0 + "^";
                            //    }
                            //}
                            //}
                            //else
                            //{
                            //objBase.WhereClause = "";
                            DataSet ds = objBase.QueryDataSet("select storeid,buildingid,floorid,shopid,shopcode,shopname from conshop where conshop.ShopStartDate<='" + DateTime.Now.ToString() + "' and conshop.ShopEndDate >= '" + DateTime.Now.ToString() + "' and conshop.StoreId=" + store.DeptID + " and conshop.FloorID=" + floors.FloorID + " and conshop.BuildingID=" + building.BuildingID);
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                jsdept += store.DeptID.ToString() + floors.FloorID.ToString() + ds.Tables[0].Rows[i]["ShopID"].ToString() + "|" + store.DeptID.ToString() + floors.FloorID.ToString() + "|" + ds.Tables[0].Rows[i]["shopcode"].ToString() + " " + ds.Tables[0].Rows[i]["ShopName"].ToString() + "^";
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
        BindItem();
    }
    /// <summary>
    /// 绑定列表数据
    /// </summary>
    private void BindDate(string SqlString)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        PagedDataSource pds = new PagedDataSource();
        ds = baseBo.QueryDataSet(SqlString + Session["whereSqlStr"].ToString() + " order by contractcode,bizdate");
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        int spareRow = 0;
        int count = dt.Rows.Count;
        pds.DataSource = dt.DefaultView;
        if (pds.Count < 1)
        {
            for (int i = 0; i < ShopView.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            ShopView.DataSource = pds;
            ShopView.DataBind();
        }
        else
        {
            this.ShopView.DataSource = pds;
            this.ShopView.DataBind();
            spareRow = ShopView.Rows.Count;
            for (int i = 0; i < ShopView.PageSize - spareRow; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            ShopView.DataSource = pds;
            ShopView.DataBind();
        }
    }

    protected void ShopView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.RowIndex >= 0 && e.Row.Cells[1].Text.Trim() != "&nbsp;")
            {
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
            }
        }
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        char[] treenodeid = new char[] { ',' };
        string treestr = deptid.Value;
        string strUnitCode = "";
        string strStoreID = "";
        string strFloorID="";
        string strShopID="";
        string strBuiFloorLoca = "";
        int p = 0;
        if (treestr == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '树节点选择为空'", true);
        }
        else
        {
            foreach (string substr in treestr.Split(treenodeid))
            {
                if (substr.Length == 3)
                {
                    p = 2;
                    strStoreID = strStoreID + substr + ",";
                }
                else if (p != 2 && substr.Length == 6)
                {
                    p = 1;
                    strFloorID = strFloorID + substr.Substring(3) + ",";
                }
                else if (p == 0 && substr.Length == 9)
                {
                    strShopID = strShopID + substr.Substring(6) + ",";
                }
            }


            if (DateTime.Parse(TextBox1.Text).CompareTo(DateTime.Parse(TextBox2.Text)) > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '结束日期应大于开始日期'", true);
            }
            else
            {
                #region 选择商铺
                if (p == 0)//选择商铺
                {
                    if (RbtMtn.Checked == true)
                    {
                        Session["whereSqlStr"] = "between '" + DateTime.Parse(TextBox1.Text).ToString("yyyy-MM-01") + "' and '" + DateTime.Parse(TextBox2.Text).ToString("yyyy-MM-01") + "' and a.shopid in (" + strShopID.TrimEnd(',') + ") ";
                    }
                    else if (RbtTotal.Checked == true)
                    {
                        Session["whereSqlStr"] = "between '" + TextBox1.Text + "' and '" + TextBox2.Text + "' and a.shopid in (" + strShopID.TrimEnd(',') + ")  group by  store.storename,contract.contractcode,customer.custshortname,conshop.shopcode,a.shopname,shoptype.shoptypename,a.trade2name,a.brandname,trafficdata.innum";
                        ShopView.Columns[9].Visible = false;
                        ShopView.Columns[11].Visible = false;
                        ShopView.Columns[12].Visible = false;
                        ShopView.Columns[13].Visible = false;
                    }
                    else
                    {
                        Session["whereSqlStr"] = "between '" + TextBox1.Text + "' and '" + TextBox2.Text + "' and a.shopid in (" + strShopID.TrimEnd(',') + ") ";
                    }
                    BindDate(SqlStr);
                }
                #endregion
                #region 选择楼层
                if (p == 1)//选择楼层
                {
                    if (RbtMtn.Checked == true)
                    {
                        Session["whereSqlStr"] = "between '" + DateTime.Parse(TextBox1.Text).ToString("yyyy-MM-01") + "' and '" + DateTime.Parse(TextBox2.Text).ToString("yyyy-MM-01") + "' and  a.floorid in (" + strFloorID.TrimEnd(',') + ") ";
                    }
                    else if (RbtTotal.Checked == true)
                    {
                        SqlStr = "select store.storename,'' contractcode,'' custshortname,'' shopcode,floors.floorname shopname,'' shoptypename,'' tradename," +
                                "'' brandname,'' bizdate,sum(paidamt) paidamt,'' as skudesc,'' as datasource,trafficdata.innum " +
                                "from transsku a " +
                                "left join floors on a.floorid=floors.floorid " +
                                "left join store on store.storeid=a.storeid "+
                                "left join (select storeid,bizdate,sum(innum) as innum from trafficdata group by bizdate,storeid) trafficdata on (trafficdata.storeid=a.storeid and trafficdata.bizdate between '" + TextBox1.Text + "' and '" + TextBox2.Text + "')" +
                                "where a.bizdate ";
                        Session["whereSqlStr"] = "between '" + TextBox1.Text + "' and '" + TextBox2.Text + "' and a.floorid in (" + strFloorID.TrimEnd(',') + ")  group by store.storename,floors.floorname,trafficdata.innum ";
                        ShopView.Columns[2].Visible = false;
                        ShopView.Columns[3].Visible = false;
                        ShopView.Columns[4].Visible = false;
                        ShopView.Columns[6].Visible = false;
                        ShopView.Columns[7].Visible = false;
                        ShopView.Columns[8].Visible = false;
                        ShopView.Columns[9].Visible = false;
                        ShopView.Columns[11].Visible = false;
                        ShopView.Columns[12].Visible = false;
                        ShopView.Columns[13].Visible = false;
                    }
                    else
                    {
                        Session["whereSqlStr"] = "between '" + TextBox1.Text + "' and '" + TextBox2.Text + "' and a.floorid in (" + strFloorID.TrimEnd(',') + ") ";
                    }
                    BindDate(SqlStr);
                }
                #endregion
                #region 选择项目
                if (p == 2)//选择项目
                {
                    if (RbtMtn.Checked == true)
                    {
                        Session["whereSqlStr"] = "between '" + DateTime.Parse(TextBox1.Text).ToString("yyyy-MM-01") + "' and '" + DateTime.Parse(TextBox2.Text).ToString("yyyy-MM-01") + "' and  a.Storeid in (" + strStoreID.TrimEnd(',') + ") ";
                    }
                    else if (RbtTotal.Checked == true)
                    {
                        SqlStr = "select store.storename,'' contractcode,'' custshortname,'' shopcode,'' shopname,'' shoptypename,'' tradename," +
                                "'' brandname,'' bizdate,sum(paidamt) paidamt,'' as skudesc,'' as datasource,trafficdata.innum " +
                                "from transsku a " +
                                "left join store on a.storeid=store.storeid " +
                                "left join (select storeid,bizdate,sum(innum) as innum from trafficdata group by bizdate,storeid) trafficdata on (trafficdata.storeid=a.storeid and trafficdata.bizdate between '" + TextBox1.Text + "' and '" + TextBox2.Text + "')" +
                                "where a.bizdate ";
                        Session["whereSqlStr"] = "between '" + TextBox1.Text + "' and '" + TextBox2.Text + "' and a.storeid in (" + strStoreID.TrimEnd(',') + ")  group by store.storename,trafficdata.innum ";
                        ShopView.Columns[2].Visible = false;
                        ShopView.Columns[3].Visible = false;
                        ShopView.Columns[4].Visible = false;
                        ShopView.Columns[5].Visible = false;
                        ShopView.Columns[6].Visible = false;
                        ShopView.Columns[7].Visible = false;
                        ShopView.Columns[8].Visible = false;
                        ShopView.Columns[9].Visible = false;
                        ShopView.Columns[11].Visible = false;
                        ShopView.Columns[12].Visible = false;
                    }
                    else
                    {
                        Session["whereSqlStr"] = "between '" + TextBox1.Text + "' and '" + TextBox2.Text + "' and a.Storeid in (" + strStoreID.TrimEnd(',') + ") ";
                    }
                    BindDate(SqlStr);
                }
                #endregion
            }
            //BtnSave.Enabled = true;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "hiditem", "HidItem()", true);
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        if (ViewState["ID"].ToString().Length >= 9)
        {
            this.btnAdd.Enabled = true;
        }
        else
        {
            this.btnAdd.Enabled = false;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "hiditem", "HidItem()", true);
    }

    private void BindItem()
    {
        CBList.Items.Add("1 " + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_BusinessItem"));
        CBList.Items.Add("2 " + (String)GetGlobalResourceObject("BaseInfo", "AdBoard_lblContractID"));
        CBList.Items.Add("3 " + (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustName"));
        CBList.Items.Add("4 " + (String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labShopCode"));
        CBList.Items.Add("5 " + (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPotShopName"));
        CBList.Items.Add("6 " + (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopType"));
        CBList.Items.Add("7 " + (String)GetGlobalResourceObject("BaseInfo", "RentableArea_lblTradeRelation"));
        CBList.Items.Add("8 " + (String)GetGlobalResourceObject("BaseInfo", "Rpt_Brand"));
        CBList.Items.Add("9 " + (String)GetGlobalResourceObject("BaseInfo", "Rpt_Date"));
        CBList.Items.Add("10 " + (String)GetGlobalResourceObject("BaseInfo", "ConLease_labSellCount"));
        CBList.Items.Add("11 " + (String)GetGlobalResourceObject("BaseInfo", "Rpt_SkuDesc"));
        CBList.Items.Add("12 " + (String)GetGlobalResourceObject("BaseInfo", "Lease_lblPayInDataSource"));
        CBList.Items.Add("13 " + (String)GetGlobalResourceObject("BaseInfo", "RptSalesTraffic_Traffic"));
    }
    protected void CBList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string items = "";
        for (int i = 0; i < CBList.Items.Count; i++)
        {
            if (CBList.Items[i].Selected == true)
            {
                items += CBList.Items[i].Text.Substring(0, CBList.Items[i].Text.IndexOf (' ')) + ",";
            }
        }
        TextBox3.Text = items;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void RbtMtn_CheckedChanged(object sender, EventArgs e)
    {
        Label32.Text = "销售开始月";
        Label33.Text = "销售结束月";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "hiditem", "HidItem()", true);
    }
    protected void RbtDetail_CheckedChanged(object sender, EventArgs e)
    {
        Label32.Text = (String)GetGlobalResourceObject("BaseInfo", "Sell_SaleBeginDate");
        Label33.Text = (String)GetGlobalResourceObject("BaseInfo", "Sell_SaleEndDate");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "hiditem", "HidItem()", true);
    }
    protected void RbtDay_CheckedChanged(object sender, EventArgs e)
    {
        Label32.Text = (String)GetGlobalResourceObject("BaseInfo", "Sell_SaleBeginDate");
        Label33.Text = (String)GetGlobalResourceObject("BaseInfo", "Sell_SaleEndDate");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "hiditem", "HidItem()", true);
    }
    protected void RbtTotal_CheckedChanged(object sender, EventArgs e)
    {
        Label32.Text = (String)GetGlobalResourceObject("BaseInfo", "Sell_SaleBeginDate");
        Label33.Text = (String)GetGlobalResourceObject("BaseInfo", "Sell_SaleEndDate");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "hiditem", "HidItem()", true);
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet objDs = new DataSet();
        string CPath = "C:/Sell/";
        if (!Directory.Exists(CPath))
            Directory.CreateDirectory(CPath);
        if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
        {

            #region 选择商铺
            if (ViewState["ID"].ToString().Length == 9)//选择商铺
            {
                string strShopID = ViewState["ID"].ToString().Substring(6);
                objDs = baseBo.QueryDataSet("select shopname from conshop where shopid='" + strShopID + "'");
                if (RbtMtn.Checked == true)
                {
                    CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + TextBox1.Text.ToString().Substring(0, 7) + "销售月报.xls";
                }
                else if (RbtDay.Checked == true)
                {
                    CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + TextBox1.Text.ToString() + "-" + TextBox2.Text.ToString() + "销售日报.xls";
                }
                else if (RbtDetail.Checked == true)
                {
                    CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + TextBox1.Text.ToString() + "-" + TextBox2.Text.ToString() + "销售明细.xls";
                }
            }
            #endregion
            #region 选择楼层
            if (ViewState["ID"].ToString().Length == 6)//选择楼层
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(3);
                objDs = baseBo.QueryDataSet("select building.buildingname,floors.floorname from building,floors where building.buildingid='" + strBuildingID + "' and floors.floorid='" + strFloorID + "'");
                if (RbtMtn.Checked == true)
                {
                    CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + objDs.Tables[0].Rows[0][1].ToString().Trim() + TextBox1.Text.ToString().Substring(0, 7) + "销售月报.xls";
                }
                else if (RbtDay.Checked == true)
                {
                    CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + objDs.Tables[0].Rows[0][1].ToString().Trim() + TextBox1.Text.ToString() + "-" + TextBox2.Text.ToString() + "销售日报.xls";
                }
                else if (RbtDetail.Checked == true)
                {
                    CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + objDs.Tables[0].Rows[0][1].ToString().Trim() + TextBox1.Text.ToString() + "-" + TextBox2.Text.ToString() + "销售明细.xls";
                }
            }
            #endregion
            try
            {
                if (File.Exists(CPath))
                    File.Delete(CPath);
                Microsoft.Office.Interop.Excel.XlFileFormat version = Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel7;//Excel 2003版本
                //创建Application对象
                Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
                xApp.Visible = false;
                //WorkBook对象
                Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks.Add(true);
                //指定要操作的Sheet
                Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
                objDs = baseBo.QueryDataSet(SqlStr + Session["whereSqlStr"].ToString() + " order by a.shopname,bizdate");
                int j = 1;
                for (int i = 0; i < CBList.Items.Count; i++)
                {
                    if (i == 5 || i == 10 || i == 11)
                    {
                        xSheet.Cells[1, j] = ShopView.Columns[i].HeaderText.ToString();
                        for (int k = 2; k < objDs.Tables[0].Rows.Count + 2; k++)
                        {
                            xSheet.Cells[k, j] = objDs.Tables[0].Rows[k - 2][i].ToString();
                        }
                        j++;
                    }
                    else if (CBList.Items[i].Selected == true)
                    {
                        xSheet.Cells[1, j] = ShopView.Columns[i].HeaderText.ToString();
                        for (int k = 2; k < objDs.Tables[0].Rows.Count + 2; k++)
                        {
                            xSheet.Cells[k, j] = objDs.Tables[0].Rows[k - 2][i].ToString();
                        }
                        j++;
                    }
                }
                xSheet.SaveAs(CPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xSheet = null;
                xBook = null;
                xApp.Workbooks.Close();
                xApp.Quit();
                xApp = null;
                System.GC.Collect();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '导出成功。'", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '导出错误，请重新操作。'", true);
            }
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "hiditem", "HidItem()", true);
    }
}
