using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base;
using Base.Biz;
using Base.DB;
using Base.Page;
using Sell;
using BaseInfo.User;
using BaseInfo.Dept;
using RentableArea;
using BaseInfo.authUser;
using Shop.ShopTreeOperate;

public partial class Sell_ShopSellDifferenceReceipt : BasePage
{
    public string strBaseinfo = "";
    public string strFresh;
    /// <summary>
    /// 用于绑定的表
    /// </summary>
    protected DataTable ShopSellDT
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
    protected DataTable SaveShopSellDT
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
            strBaseinfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_ShopSellDifferenceReceipt");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.TextBox1.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.TextBox2.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.BindDate();
            this.IniShopSellDT();
            ShowShopTree();
            //ShowTree();
        }
    }
    public void ShowShopTree()
    {
        string strSql = "select StoreId,FloorID,BuildingID,shopid,shopcode,shopname,convert(char(10),getdate(),120) as Date,0 as Cash,0 as BankCard from conshop  where  EXISTS (SELECT tenantid FROM skumaster WHERE  skumaster.tenantid=conshop.shopid)";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        ShopTreeOperate objShopTreeOperate = new ShopTreeOperate(false, sessionUser.UserID, strSql);
        depttxt.Value = objShopTreeOperate.ShowShopTree();
    }
    #region 树形列表(没有用到)
    /// <summary>
    /// 显示树形列表
    /// </summary>
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        Dept dept = new Dept();

        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        rs = baseBO.Query(dept);
        if (rs.Count == 1)
        {
            dept = rs.Dequeue() as Dept;
            jsdept = dept.DeptID + "|" + "0" + "|" + dept.DeptName + "^";
        }
        else
        {
            return;
        }

        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        baseBO.OrderBy = " orderid ";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            baseBO.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = baseBO.Query(dept);
        baseBO.OrderBy = "";
        if (rs.Count > 0)
        {
            foreach (Dept store in rs)
            {
                jsdept += store.DeptID + "|" + dept.DeptID + "|" + store.DeptName + "^";
                baseBO.WhereClause = "StoreId=" + store.DeptID;
                rs = baseBO.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                        baseBO.WhereClause = "floors.BuildingID=" + building.BuildingID;

                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            baseBO.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        }
                        rs = baseBO.Query(new floorsAuth());
                        foreach (floorsAuth floors in rs)
                        {
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID + "|" + floors.FloorName + "^";
                            baseBO.WhereClause = "";
                            string strSql = "select StoreId,FloorID,BuildingID,shopid,shopcode,shopname,convert(char(10),getdate(),120) as Date,0 as Cash,0 as BankCard from conshop inner join skumaster on (skumaster.tenantid=conshop.shopid) where StoreId=" + store.DeptID + " and FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID + "";
                            DataSet ds = baseBO.QueryDataSet(strSql);
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + ds.Tables[0].Rows[i]["ShopID"].ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + ds.Tables[0].Rows[i]["ShopName"].ToString() + "^";
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    #endregion
    /// <summary>
    /// 绑定列表数据
    /// </summary>
    private void BindDate()
    {
        BaseBO baseBo = new BaseBO();

        PagedDataSource pds = new PagedDataSource();
        //int spareRow = 0;

        DataTable dt = new DataTable();
        //int count = dt.Rows.Count;
        dt.Columns.Add("ShopId");
        dt.Columns.Add("ShopName");
        dt.Columns.Add("Date");
        dt.Columns.Add("paidamt");
        dt.Columns.Add("Cash");
        dt.Columns.Add("trade");
        //for (int j = 0; j < count; j++)
        //{
        //    dt.Rows[j]["Date"] = TextBox1.Text;
        //}

        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < ShopView.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    ShopView.DataSource = pds;
        //    ShopView.DataBind();
        //}
        //else
        //{
            //this.ShopView.DataSource = pds;
            //this.ShopView.DataBind();
            //spareRow = ShopView.Rows.Count;
            for (int i = 0; i < ShopView.PageSize; i++)
            {
                dt.Rows.Add(dt.NewRow());
            }
            pds.DataSource = dt.DefaultView;
            ShopView.DataSource = pds;
            ShopView.DataBind();
        //}
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Sell/ShopSellDifferenceReceipt.aspx");
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <returns></returns>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        FindChecked();
        TransSku transSku = new TransSku();
        TransSkuMedia transSkuMedia = new TransSkuMedia();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        BaseBO baseBO = new BaseBO();
        DataTable tempTb = new DataTable();
        string skubancth = BaseApp.GetSkuMasterSkuID("TransSku", "BatchID").ToString();   //每个页面保存的时候是同一个批次号
        string skubancthTransID = skubancth;
        if (skubancthTransID.Length > 9)
        {
            skubancthTransID = skubancthTransID.Substring(skubancthTransID.Length - 9);
        }

        for (int i = 0; i < ShopSellDT.Rows.Count; i++)
        {
            string txtCash = ShopSellDT.Rows[i]["Cash"].ToString();//现金
            
            if (txtCash.Trim() != "0" && txtCash.Trim() != "")
            {
                if (ShopSellDT.Rows[i]["ShopID"].ToString().Trim() != "" && ShopSellDT.Rows[i]["ShopID"].ToString().Trim() != "&nbsp;")
                {
                    BaseTrans baseTrans = new BaseTrans();
                    baseTrans.BeginTrans();

                    string s = ShopSellDT.Rows[i]["ShopID"].ToString().Trim();
                    string txtDate = ShopSellDT.Rows[i]["Date"].ToString();//日期
                    //string txtBankCard = ShopSellDT.Rows[i]["BankCard"].ToString();//银行卡
                    string sql = "  SELECT " +
                                " ISNULL(ConShop.ShopID,0) as ShopID,ConShop.ShopName," +
                                " ISNULL(ConShop.BuildingId,0) as BuildingId, BuildingName," +
                                " ISNULL(ConShop.FloorId,0) as FloorId,FloorName," +
                                " ISNULL(ConShop.LocationId,0) as LocationId,LocationName," +
                                " ISNULL(ConShop.AreaId,0) as AreaId,AreaName," +
                                " ISNULL(ConShop.BrandId,0) as BrandId,BrandName," +
                                " ISNULL(TradeRelation.TradeId,0) as TradeId,TradeName," +
                                " ISNULL(ConShop.StoreID,0) as StoreID," +             //StoreID
                                " ISNULL(ConShop.MainLocationID,0) as MainLocationID," +
                                " ISNULL(skumaster.skuid,0) as skuid," +
                                " ISNULL(skumaster.skudesc,0) as skudesc" +
                                " FROM ConShop LEFT JOIN Building On (ConShop.BuildingId=Building.BuildingId)" +
                                " left join skumaster on (conshop.shopid=skumaster.tenantid)" +
                                " LEFT JOIN Floors On (ConShop.FloorId=Floors.FloorId)" +
                                " LEFT JOIN Location On (ConShop.LocationId=Location.LocationId)" +
                                " LEFT JOIN Area On (ConShop.AreaId=Area.AreaId)" +
                                " LEFT JOIN ConShopBrand On (ConShop.BrandId=ConShopBrand.BrandId)" +
                                " LEFT JOIN Contract On(ConShop.ContractId=Contract.ContractId) " +
                                " LEFT JOIN TradeRelation On(TradeRelation.TradeId=Contract.TradeId) " +
                                " WHERE ShopId=" + ShopSellDT.Rows[i]["ShopID"].ToString();

                    tempTb = baseBO.QueryDataSet(sql).Tables[0];
                    transSku.TransID = tempTb.Rows[0]["StoreID"].ToString().Trim() + '0' + ShopSellDT.Rows[i]["ShopID"].ToString().Trim() + skubancthTransID;//项目ID + “0”+ ShopID +批次号（后9位）,加0是为了防止和posid重复
                    transSku.ReceiptID = "0000";
                    transSku.ShopID = Int32.Parse(ShopSellDT.Rows[i]["ShopID"].ToString());
                    transSku.ShopName = tempTb.Rows[0]["ShopName"].ToString();
                    transSku.PosID = "999";//pos机
                    transSku.SkuID = tempTb.Rows[0]["skuid"].ToString();//商品编码
                    transSku.SkuDesc = tempTb.Rows[0]["skudesc"].ToString();//商品名称
                    transSku.Trade2ID = Convert.ToInt32(tempTb.Rows[0]["TradeId"]);
                    transSku.Trade2Name = tempTb.Rows[0]["TradeName"].ToString();
                    transSku.BuildingID = Convert.ToInt32(tempTb.Rows[0]["BuildingID"]);
                    transSku.BuildingName = tempTb.Rows[0]["BuildingName"].ToString();
                    transSku.FloorID = Convert.ToInt32(tempTb.Rows[0]["FloorID"]);
                    transSku.FloorName = tempTb.Rows[0]["FloorName"].ToString();
                    transSku.LocationID = Convert.ToInt32(tempTb.Rows[0]["LocationID"]);
                    transSku.LocationName = tempTb.Rows[0]["LocationName"].ToString();
                    transSku.AreaID = Convert.ToInt32(tempTb.Rows[0]["AreaID"]);
                    transSku.AreaName = tempTb.Rows[0]["AreaName"].ToString();
                    transSku.BrandID = Convert.ToInt32(tempTb.Rows[0]["BrandID"]);
                    transSku.BrandName = tempTb.Rows[0]["BrandName"].ToString();
                    transSku.PaidAmt = Convert.ToDecimal(double.Parse(txtCash) - double.Parse(ShopSellDT.Rows[i]["PaidAmt"].ToString()));//现金
                    transSku.PayAmt = Convert.ToDecimal(double.Parse(txtCash) - double.Parse(ShopSellDT.Rows[i]["PaidAmt"].ToString()));
                    transSku.NewPrice = Convert.ToDecimal(double.Parse(txtCash) - double.Parse(ShopSellDT.Rows[i]["PaidAmt"].ToString()));
                    transSku.BizDate = Convert.ToDateTime(txtDate.Trim());//日期
                    transSku.TransTime = Convert.ToDateTime(txtDate.Trim());//日期
                    transSku.BatchID = skubancth;//批次号
                    transSku.DataSource = TransSku.DATASOURCE_WORK;//数据来源
                    transSku.StoreID = Convert.ToInt32(tempTb.Rows[0]["StoreID"]);
                    transSku.MainLocationID = Convert.ToInt32(tempTb.Rows[0]["MainLocationID"]);
                    transSku.UserID = objSessionUser.UserID;
                    transSku.Qty = 1;

                    //交易金额流水表
                    transSkuMedia.TransID = tempTb.Rows[0]["StoreID"].ToString().Trim() + '0' + ShopSellDT.Rows[i]["ShopID"].ToString().Trim() + skubancthTransID;//项目ID + “0”+ ShopID +批次号（后9位）,加0是为了防止和posid重复
                    transSkuMedia.ShopID = Int32.Parse(ShopSellDT.Rows[i]["ShopID"].ToString());
                    transSkuMedia.ShopName = tempTb.Rows[0]["ShopName"].ToString();
                    transSkuMedia.PosID = "999";
                    transSkuMedia.BizDate = Convert.ToDateTime(txtDate.Trim());//日期
                    transSkuMedia.TransTime = Convert.ToDateTime(txtDate.Trim());//日期
                    transSkuMedia.ReceiptID = "0000";
                    transSkuMedia.MediaMNo = 100;//付款方式
                    transSkuMedia.MediaMDesc = "现金";
                    transSkuMedia.SkuID = tempTb.Rows[0]["skuid"].ToString();//商品编码
                    transSkuMedia.SkuDesc = tempTb.Rows[0]["skudesc"].ToString();//商品名称
                    transSkuMedia.PaidAmt = Convert.ToDecimal(double.Parse(txtCash) - double.Parse(ShopSellDT.Rows[i]["PaidAmt"].ToString()));//现金
                    transSkuMedia.DataSource = TransSku.DATASOURCE_WORK;//数据来源
                    transSkuMedia.BatchID = skubancth;//批次号
                    transSkuMedia.UserID = objSessionUser.UserID;
                    transSkuMedia.StoreID = Convert.ToInt32(tempTb.Rows[0]["StoreID"]);
                    transSkuMedia.MainLocationID = Convert.ToInt32(tempTb.Rows[0]["MainLocationID"]);
                    
                    try
                    {
                        baseTrans.Insert(transSku);
                        baseTrans.Insert(transSkuMedia);
                        //执行日报\月报生成
                        string strUpdateDaySql = "Exec SPMI_ComputerShopDaySales " + tempTb.Rows[0]["StoreID"].ToString() + ",'" + Convert.ToDateTime(txtDate.Trim()).ToString("yyyy-MM-dd") + "'";
                        baseTrans.ExecuteUpdate(strUpdateDaySql);
                        string strMthSql = "Exec SPMI_ComputerShopMonthSales " + tempTb.Rows[0]["StoreID"].ToString() + ",'" + Convert.ToDateTime(txtDate.Trim()).ToString("yyyy-MM-01") + "'";
                        baseTrans.ExecuteUpdate(strMthSql);

                        baseTrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    }
                }
                
            }
        }
        SaveShopSellDT.Rows.Clear();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        BindDate();
    }

    private string GetSixDate(string Date)
    {
        string[] parts = Date.Split('-');
        string SixStr = parts[0].Substring(2) + parts[1] + parts[2];
        return SixStr;
    }

    protected void ShopView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.Cells.Count > 1)
        if(e.Row.RowIndex>=0)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[3].Text = "";
                e.Row.Cells[4].Text = "";
                e.Row.Cells[2].Text = "";
                e.Row.Cells[5].Text = "";
                e.Row.Cells[6].Text = "";
            }
            else
            {
                TextBox txtCash = (TextBox)e.Row.Cells[5].FindControl("txtCash");
                //txtCash.Text = e.Row.Cells[4].Text;
                txtCash.Text = "0";
            }
        }
    }
    /// <summary>
    /// 记录表中选中记录的情况
    /// </summary>
    /// <returns></returns>
    private void FindChecked()
    {
        for (int i = 0; i < this.ShopView.Rows.Count; i++)
        {
            TextBox txtCash = (TextBox)this.ShopView.Rows[i].FindControl("txtCash");
            TextBox txtShopID = (TextBox)this.ShopView.Rows[i].FindControl("txtShopID");//ShopID

            string strShopID = txtShopID.Text.Trim();
            for (int j = 0; j < ShopSellDT.Rows.Count; j++)
            {
                if (ShopSellDT.Rows[j]["ShopID"].ToString() == strShopID && ShopSellDT.Rows[j]["Date"].ToString().Trim() == this.ShopView.Rows[i].Cells[3].Text.Trim())
                {
                    ShopSellDT.Rows[j]["Cash"] = txtCash.Text.Trim();
                    break;
                }
            }
            for (int j = 0; j < SaveShopSellDT.Rows.Count; j++)
            {
                if (SaveShopSellDT.Rows[j]["ShopID"].ToString() == strShopID && SaveShopSellDT.Rows[j]["Date"].ToString().Trim() == this.ShopView.Rows[i].Cells[3].Text.Trim())
                {
                    SaveShopSellDT.Rows[j]["Cash"] = txtCash.Text.Trim();
                    break;
                }
            }
        }
    }
    //protected void ShopView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    FindChecked();
    //    GridView theGrid = sender as GridView;
    //    int newPageIndex = 0;
    //    if (-2 == e.NewPageIndex)
    //    {
    //        TextBox txtNewPageIndex = null;
    //        GridViewRow pagerRow = theGrid.BottomPagerRow;
    //        if (null != pagerRow)
    //        {
    //            txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
    //        }
    //        if (null != txtNewPageIndex)
    //        {
    //            newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
    //        }
    //    }
    //    else
    //    { newPageIndex = e.NewPageIndex; }
    //    newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
    //    newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
    //    theGrid.PageIndex = newPageIndex;
    //    ShopView.DataSource = ShopSellDT;
    //    ShopView.DataBind();
    //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    //    foreach (GridViewRow grv in this.ShopView.Rows)
    //    {
    //        grv.BackColor = Color.White;
    //    }
    //}
    /// <summary>
    /// 为临时表添加列
    /// </summary>
    protected void IniShopSellDT()
    {
        ShopSellDT = new DataTable();
        ShopSellDT.Columns.Add("ShopID");
        ShopSellDT.Columns.Add("ShopName");
        ShopSellDT.Columns.Add("Date");
        ShopSellDT.Columns.Add("Cash");
        ShopSellDT.Columns.Add("paidamt");
        ShopSellDT.Columns.Add("trade");
        SaveShopSellDT = ShopSellDT.Clone();
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        AddBind();
    }

    private void AddBind()
    {
        if (DateTime.Parse(TextBox1.Text).CompareTo(DateTime.Parse(TextBox2.Text)) > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '结束日期应大于开始日期'", true);
        }
        else
        {
            //SaveShopSellDT.Rows.Clear();
            ShopSellDT.Rows.Clear();
            FindChecked();
            BaseBO objBaseBo = new BaseBO();
            //string strNum = "";
            if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
            {

                #region 选择商铺
                if (ViewState["ID"].ToString().Length == 12)//选择商铺
                {
                    ShopSellDT.Rows.Clear();
                    //string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    //string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                    string strShopID = ViewState["ID"].ToString().Substring(9, 3);
                    string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", strShopID);
                    string sqlString = "select transsku.shopid,conshop.shopcode,conshop.shopname,bizdate,sum(paidamt) as paidamt,transsku.trade2name from transsku left join conshop on conshop.shopid=transsku.shopid where bizdate between '" + TextBox1.Text.Trim() + "' and '" + TextBox2.Text.Trim() + "' and transsku.shopid='" + strShopID + "' group by transsku.shopid,conshop.shopcode,conshop.shopname,bizdate,transsku.trade2name ";
                    int day = DaysBetween(DateTime.Parse(TextBox1.Text), DateTime.Parse(TextBox2.Text));
                    DataSet ds = new DataSet();
                    ds = objBaseBo.QueryDataSet(sqlString);
                    DataSet ds1 = new DataSet();
                    ds1 = objBaseBo.QueryDataSet("select tradename from traderelation where tradeid=(select tradeid from contract where contractid=(select contractid from conshop where shopid='" + strShopID + "'))");
                    string trad = ds1.Tables[0].Rows[0]["tradename"].ToString();
                    for (int i = 0; i <= day; i++)
                    {
                        DataRow dr = SaveShopSellDT.NewRow();
                        dr["ShopID"] = strShopID;
                        dr["ShopName"] = strShopName;
                        dr["Date"] = DateTime.Parse(TextBox1.Text).AddDays(i).ToString("yyyy-MM-dd");
                        dr["Cash"] = "0";
                        dr["paidamt"] = "0";
                        dr["trade"] = trad;
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (dr["Date"].ToString() == DateTime.Parse(ds.Tables[0].Rows[j]["bizdate"].ToString()).ToString("yyyy-MM-dd"))
                                {
                                    dr["paidamt"] = ds.Tables[0].Rows[j]["paidamt"].ToString().Trim();
                                }
                            }
                        }

                        bool bContains = false;
                        foreach (DataRow drShop in SaveShopSellDT.Rows)
                        {
                            if (drShop["ShopID"].ToString() == strShopID.ToString() && DateTime.Parse(drShop["Date"].ToString()).ToString("yyyy-MM-dd") == DateTime.Parse(dr["Date"].ToString()).ToString("yyyy-MM-dd"))
                                bContains = true;
                        }
                        if (bContains == false)
                            SaveShopSellDT.Rows.Add(dr);//存储数据到临时表
                    }
                    SaveShopSellDT.DefaultView.Sort = "trade asc,ShopID asc,Date asc";
                    DataTable dtTemp = SaveShopSellDT.DefaultView.ToTable();
                    ShopSellDT = dtTemp.Copy();
                    ViewState["date"] = "asc";
                    ViewState["trade"] = "asc";
                    if (ShopSellDT.Rows.Count < 15)
                    {
                        for (int i = (ShopSellDT.Rows.Count % 15); i < 15; i++)
                        {
                            ShopSellDT.Rows.Add(ShopSellDT.NewRow());
                        }
                    }
                    ShopView.DataSource = ShopSellDT;
                    ShopView.DataBind();
                }
                #endregion
                #region 选择楼层
                if (ViewState["ID"].ToString().Length == 9)//选择楼层
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                    string strSql = "select StoreId,FloorID,BuildingID,shopid,shopcode,shopname,traderelation.tradename trade from conshop inner join skumaster on (skumaster.tenantid=conshop.shopid) left join contract on conshop.contractid=contract.contractid left join traderelation on contract.tradeid=traderelation.tradeid where FloorID='" + strFloorID + "' group by StoreId,FloorID,BuildingID,shopid,shopcode,shopname,traderelation.tradename";
                    string sqlString = "select transsku.shopid,conshop.shopcode,conshop.shopname,bizdate,sum(paidamt) as paidamt from transsku left join conshop on conshop.shopid=transsku.shopid where bizdate between '" + TextBox1.Text.Trim() + "' and '" + TextBox2.Text.Trim() + "' and transsku.floorid='" + strFloorID + "' group by transsku.shopid,conshop.shopcode,conshop.shopname,bizdate";
                    int day = DaysBetween(DateTime.Parse(TextBox1.Text), DateTime.Parse(TextBox2.Text));
                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataSet ds1 = new DataSet();
                        ds1 = objBaseBo.QueryDataSet(sqlString);
                        ViewState["count"] = "0";
                        ShopSellDT.Rows.Clear();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                            for (int k = 0; k <= day; k++)
                            {
                                DataRow dr = SaveShopSellDT.NewRow();
                                dr["ShopID"] = ds.Tables[0].Rows[i]["ShopId"].ToString();
                                dr["ShopName"] = strShopName;
                                dr["Date"] = DateTime.Parse(TextBox1.Text).AddDays(k).ToString("yyyy-MM-dd");
                                dr["Cash"] = "0";
                                dr["paidamt"] = "0";
                                dr["trade"] = ds.Tables[0].Rows[i]["trade"].ToString();
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                    {
                                        if (ds1.Tables[0].Rows[j]["shopid"].ToString().Trim() == dr["ShopID"].ToString().Trim() && dr["Date"].ToString() == DateTime.Parse(ds1.Tables[0].Rows[j]["bizdate"].ToString()).ToString("yyyy-MM-dd"))
                                        {
                                            dr["paidamt"] = ds1.Tables[0].Rows[j]["paidamt"].ToString().Trim();
                                            break;
                                        }
                                    }
                                }

                                bool bContains = false;
                                foreach (DataRow drShop in SaveShopSellDT.Rows)
                                {
                                    if (drShop["ShopID"].ToString() == ds.Tables[0].Rows[i]["ShopId"].ToString() && DateTime.Parse(drShop["Date"].ToString()).ToString("yyyy-MM-dd") == dr["Date"].ToString())
                                        bContains = true;
                                }
                                if (bContains == false)
                                    SaveShopSellDT.Rows.Add(dr);//存储数据到临时表
                                ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                            }
                        }
                        SaveShopSellDT.DefaultView.Sort = "trade asc,ShopID asc,Date asc";
                        DataTable dtTemp = SaveShopSellDT.DefaultView.ToTable();
                        ShopSellDT = dtTemp.Copy();
                        ViewState["date"] = "asc";
                        ViewState["trade"] = "asc";
                        if (ShopSellDT.Rows.Count < 15)
                        {
                            for (int i = (ShopSellDT.Rows.Count % 15); i < 15; i++)
                            {
                                ShopSellDT.Rows.Add(ShopSellDT.NewRow());
                            }
                        }
                        ShopView.DataSource = ShopSellDT;
                        ShopView.DataBind();
                    }
                    else
                    {
                        this.BindDate();
                    }
                }
                #endregion
                #region 选择大楼
                if (ViewState["ID"].ToString().Length == 6)//选择大楼
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    string strSql = "select StoreId,FloorID,BuildingID,shopid,shopcode,shopname,traderelation.tradename trade from conshop inner join skumaster on (skumaster.tenantid=conshop.shopid) left join contract on conshop.contractid=contract.contractid left join traderelation on contract.tradeid=traderelation.tradeid where BuildingID='" + strBuildingID + "' group by StoreId,FloorID,BuildingID,shopid,shopcode,shopname,traderelation.tradename ";
                    string sqlString = "select transsku.shopid,conshop.shopcode,conshop.shopname,bizdate,sum(paidamt) as paidamt from transsku left join conshop on conshop.shopid=transsku.shopid where bizdate between '" + TextBox1.Text.Trim() + "' and '" + TextBox2.Text.Trim() + "' and transsku.buildingid='" + strBuildingID + "' group by transsku.shopid,conshop.shopcode,conshop.shopname,bizdate";
                    int day = DaysBetween(DateTime.Parse(TextBox1.Text), DateTime.Parse(TextBox2.Text));
                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataSet ds1 = new DataSet();
                        ds1 = objBaseBo.QueryDataSet(sqlString);
                        ShopSellDT.Rows.Clear();
                        ViewState["count"] = "0";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                            for (int k = 0; k <= day; k++)
                            {
                                DataRow dr = SaveShopSellDT.NewRow();
                                dr["ShopID"] = ds.Tables[0].Rows[i]["ShopId"].ToString();
                                dr["ShopName"] = strShopName;
                                dr["Date"] = DateTime.Parse(TextBox1.Text).AddDays(k).ToString("yyyy-MM-dd");
                                dr["Cash"] = "0";
                                dr["paidamt"] = "0";
                                dr["trade"] = ds.Tables[0].Rows[i]["trade"].ToString();
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                                    {
                                        if (ds1.Tables[0].Rows[j]["shopid"].ToString().Trim() == dr["ShopID"].ToString().Trim() && dr["Date"].ToString() == DateTime.Parse(ds1.Tables[0].Rows[j]["bizdate"].ToString()).ToString("yyyy-MM-dd"))
                                        {
                                            dr["paidamt"] = ds1.Tables[0].Rows[j]["paidamt"].ToString().Trim();
                                            break;
                                        }
                                    }
                                }

                                bool bContains = false;
                                foreach (DataRow drShop in SaveShopSellDT.Rows)
                                {
                                    if (drShop["ShopID"].ToString() == ds.Tables[0].Rows[i]["ShopId"].ToString() && DateTime.Parse(drShop["Date"].ToString()).ToString("yyyy-MM-dd") == dr["Date"].ToString())
                                        bContains = true;
                                }
                                if (bContains == false)
                                    SaveShopSellDT.Rows.Add(dr);//存储数据到临时表

                                ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                            }
                        }
                        SaveShopSellDT.DefaultView.Sort = "trade asc,ShopID asc,Date asc";
                        DataTable dtTemp = SaveShopSellDT.DefaultView.ToTable();
                        ShopSellDT = dtTemp.Copy();
                        ViewState["date"] = "asc";
                        ViewState["trade"] = "asc";
                        if (ShopSellDT.Rows.Count < 15)
                        {
                            for (int i = (ShopSellDT.Rows.Count % 15); i < 15; i++)
                            {
                                ShopSellDT.Rows.Add(ShopSellDT.NewRow());
                            }
                        }
                        ShopView.DataSource = ShopSellDT;
                        ShopView.DataBind();
                    }
                    else
                    {
                        this.BindDate();
                    }
                }
                #endregion
            }
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        if (ViewState["ID"].ToString().Length >= 6)
        {
            this.btnAdd.Enabled = true;
            this.ButAdd.Enabled = true;
            this.btnSave.Enabled = true;
        }
        else
        {
            this.btnAdd.Enabled = false;
            this.ButAdd.Enabled = false;
            this.btnSave.Enabled = false;
        }
        //this.BindDate();
        this.SetGvClear();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    private void SetGvClear()
    {
        foreach (GridViewRow grv in this.ShopView.Rows)
        {
            if (grv.Cells.Count > 1)
            {
                if (grv.Cells[1].Text == "&nbsp;" || grv.Cells[1].Text == "")
                {
                    grv.Cells[2].Text = "";
                    grv.Cells[3].Text = "";
                    grv.Cells[4].Text = "";
                    grv.Cells[5].Text = "";
                    grv.Cells[6].Text = "";
                }
            }
        }
    }
    private int DaysBetween(DateTime date1, DateTime date2)
    {
        int days = 0;
        while (date1 != date2)
        {
            days++;
            date1 = date1.AddDays(1);
        }
        return days;
    }
    protected void ShopView_Sorting(object sender, GridViewSortEventArgs e)
    {
        FindChecked();
        //if (ShopSellDT.Rows.Count <= 15)
        //{
        //    for (int i = 0; i < ShopSellDT.Rows.Count; i++)
        //    {
        //        if (ShopSellDT.Rows[i]["ShopID"].ToString() == "")
        //        {
        //            ShopSellDT.Rows[i].Delete();
        //        }
        //    }
        //}
        if (e.SortExpression == "Date")
        {
            if (ViewState["date"] != null && ViewState["date"].ToString() != "")
            {
                if (ViewState["date"].ToString() == "asc")
                {
                    SaveShopSellDT.DefaultView.Sort = "Date DESC,trade " + ViewState["trade"].ToString().Trim() + ",ShopID ASC";
                    DataTable dtTemp = SaveShopSellDT.DefaultView.ToTable();
                    ShopSellDT.Clear();
                    ShopSellDT = dtTemp.Copy();
                    if (ShopSellDT.Rows.Count < 15)
                    {
                        for (int i = (ShopSellDT.Rows.Count % 15); i < 15; i++)
                        {
                            ShopSellDT.Rows.Add(ShopSellDT.NewRow());
                        }
                    }
                    ShopView.DataSource = ShopSellDT;
                    ShopView.DataBind();
                    ViewState["date"] = "desc";
                }
                else
                {
                    SaveShopSellDT.DefaultView.Sort = "Date ASC,trade " + ViewState["trade"].ToString().Trim() + ",ShopID ASC";
                    DataTable dtTemp = SaveShopSellDT.DefaultView.ToTable();
                    ShopSellDT.Clear();
                    ShopSellDT = dtTemp.Copy();
                    if (ShopSellDT.Rows.Count < 15)
                    {
                        for (int i = (ShopSellDT.Rows.Count % 15); i < 15; i++)
                        {
                            ShopSellDT.Rows.Add(ShopSellDT.NewRow());
                        }
                    }
                    ShopView.DataSource = ShopSellDT;
                    ShopView.DataBind();
                    ViewState["date"] = "asc";
                }
            }
        }
        else if (e.SortExpression == "trade")
        {
            if (ViewState["trade"] != null && ViewState["trade"].ToString() != "")
            {
                if (ViewState["trade"].ToString() == "asc")
                {
                    SaveShopSellDT.DefaultView.Sort = "trade DESC,ShopID ASC,Date " + ViewState["date"].ToString().Trim() + "";
                    DataTable dtTemp = SaveShopSellDT.DefaultView.ToTable();
                    ShopSellDT.Clear();
                    ShopSellDT = dtTemp.Copy();
                    if (ShopSellDT.Rows.Count < 15)
                    {
                        for (int i = (ShopSellDT.Rows.Count % 15); i < 15; i++)
                        {
                            ShopSellDT.Rows.Add(ShopSellDT.NewRow());
                        }
                    }
                    ShopView.DataSource = ShopSellDT;
                    ShopView.DataBind();
                    ViewState["trade"] = "desc";
                }
                else
                {
                    SaveShopSellDT.DefaultView.Sort = "trade ASC,ShopID ASC,Date " + ViewState["date"].ToString().Trim() + "";
                    DataTable dtTemp = SaveShopSellDT.DefaultView.ToTable();
                    ShopSellDT.Clear();
                    ShopSellDT = dtTemp.Copy();
                    if (ShopSellDT.Rows.Count < 15)
                    {
                        for (int i = (ShopSellDT.Rows.Count % 15); i < 15; i++)
                        {
                            ShopSellDT.Rows.Add(ShopSellDT.NewRow());
                        }
                    }
                    ShopView.DataSource = ShopSellDT;
                    ShopView.DataBind();
                    ViewState["trade"] = "asc";
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void ButAdd_Click(object sender, EventArgs e)
    {
        SaveShopSellDT.Rows.Clear();
        AddBind();
    }
}
