using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;

using Base.Biz;
using Base.DB;
using Base.Page;
using BaseInfo.User;
using BaseInfo.Dept;
using RentableArea;
using BaseInfo.authUser;

public partial class Lease_QueryLeaseDetail : BasePage
{
    public string strBaseinfo = "";
    public string fresh = "";
    private int p = 0;
    private string SqlConType = "-1,";
    private string SqlStr = @"select store.storename,floors.floorname,conshop.shopcode,traderelation.tradename,customer.custcode,customer.custshortname,
                            conshopbrand.brandname,contract.norentdays,'' norentcondition,convert(varchar(10),conshop.shopstartdate,120) shopstartdate,convert(varchar(10),conshop.shopenddate,120) shopenddate,
                            aa.单位租金,aa.月租金,aa.年租金,aa.推广费,aa.物业费,aa.履约保证金,aa.POS机押金,aa.POS租赁费,aa.营业员押金,aa.电费押金,aa.人员培训费,
                            aa.钥匙押金,aa.胸卡押金,aa.信用卡承担,aa.Vip卡承担,aa.年节费,'' 水费,'' 电费,'' 燃气费
                            from conshop
                            left join store on conshop.storeid=store.storeid
                            left join floors on floors.floorid=conshop.floorid
                            left join contract on conshop.contractid=contract.contractid
                            left join traderelation on contract.tradeid=traderelation.tradeid
                            left join customer on contract.custid=customer.custid
                            left join conshopbrand on conshop.brandid=conshopbrand.brandid
                            left join (
                            select a.contractid,cast(sum(case when chargetypeid in (101,102) and formulatype='V' then a.minsum/TotalArea when chargetypeid in (101,102) and formulatype<>'V' then a.fixedrental/TotalArea end) as decimal(10,2)) 单位租金,
                                   sum(case when chargetypeid in (101,102) and formulatype='V' then a.minsum when chargetypeid in (101,102) and formulatype<>'V' then a.fixedrental end) 月租金,
                                   sum(case when chargetypeid in (101,102) and formulatype='V' then a.minsum*12 when chargetypeid in (101,102) and formulatype<>'V' then a.fixedrental*12 end) 年租金,
                                   sum(case when chargetypeid=105 and formulatype='V' then a.minsum when chargetypeid=105 and formulatype='O' then a.BaseAmt when chargetypeid=105 and formulatype='F' then a.fixedrental end) 推广费,
                                   sum(case when chargetypeid=109 and formulatype='V' then a.minsum when chargetypeid=109 and formulatype='O' then a.BaseAmt when chargetypeid=109 and formulatype='F' then a.fixedrental end) 物业费,
                                   sum(case when chargetypeid=112 and formulatype='V' then a.minsum when chargetypeid=112 and formulatype='O' then a.BaseAmt when chargetypeid=112 and formulatype='F' then a.fixedrental end) 履约保证金,
                                   sum(case when chargetypeid=117 and formulatype='V' then a.minsum when chargetypeid=117 and formulatype='O' then a.BaseAmt when chargetypeid=117 and formulatype='F' then a.fixedrental end) POS机押金,
                                   sum(case when chargetypeid=106 and formulatype='V' then a.minsum when chargetypeid=106 and formulatype='O' then a.BaseAmt when chargetypeid=106 and formulatype='F' then a.fixedrental end) POS租赁费,
                                   sum(case when chargetypeid=118 and formulatype='V' then a.minsum when chargetypeid=118 and formulatype='O' then a.BaseAmt when chargetypeid=118 and formulatype='F' then a.fixedrental end) 营业员押金,
                                   sum(case when chargetypeid=114 and formulatype='V' then a.minsum when chargetypeid=114 and formulatype='O' then a.BaseAmt when chargetypeid=114 and formulatype='F' then a.fixedrental end) 电费押金,
                                   '' 人员培训费,'' 钥匙押金,'' 胸卡押金,'' 信用卡承担,'' Vip卡承担,'' 年节费
                            from 
                            (select contract.contractid,store.storeshortname, rtrim(floors.floorname) + shoptype.shoptypename as shoptype, Contract.ContractCode,Customer.CustCode,Customer.CustName, 
                            Case Contract.BizMode when 1 then '租赁' when '2' then '联营' End as BizMode , 
                            Case Contract.ContractStatus when '0' then '初始状态' when '1' then '草稿' when '2' then '正常' when '3' then '到期' when '4' then '终止' when '5' then '暂停' End as ContractStatus, 
                            ConFormulaH.FStartDate,ConFormulaH.FEndDate,conformulah.formulatype, Case ConFormulaH.FormulaType when 'F' then '固定租金' when 'V' then '抽成与保底' when 'O' then '一次性收取' end as FormulaTypeName,
                            chargetype.chargetypeid,chargetype.chargetypename,
                            ConFormulaH.TotalArea,ConFormulaH.UnitPrice,BaseAmt,FixedRental,ConFormulaP.Pcent,ConFormulaP.SalesTo as PSalesTo,ConFormulaM.MinSum,ConFormulaM.SalesTo as MSalesTo 
                            from Contract 
                            Left join ConFormulaH on (Contract.ContractId=ConFormulaH.ContractId) 
                            left join ConFormulaP on (ConFormulaP.FormulaID=ConFormulaH.FormulaID) 
                            left join ConFormulaM on (ConFormulaM.FormulaID=ConFormulaH.FormulaID) 
                            inner join conshop on (conshop.contractid=contract.contractid)
                            inner join store on (conshop.storeid=store.storeid)
                            inner join shoptype on (conshop.shoptypeid=shoptype.shoptypeid) 
                            inner join floors on (floors.floorid=conshop.floorid) 
                            left join Customer On (Customer.CustId=Contract.CustId)
                            left join chargetype on conformulah.chargetypeid=chargetype.chargetypeid ";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Chbzc.Checked==true)
        {
            SqlConType += "2,";
        }
        if (Chbcg.Checked==true)
        {
            SqlConType += "1,";
        }
        if (Chbdq.Checked==true)
        {
            SqlConType += "3,";
        }
        if (Chbzz.Checked==true)
        {
            SqlConType += "4,";
        }
        SqlConType = SqlConType.TrimEnd(',');
        ShowShopTree();
        if (!this.IsPostBack)
        {
            txtChargeDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            strBaseinfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_QueryLeaseDetail");
            fresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            Session["whereSqlStr"] = " where '" + txtChargeDate.Text.ToString().Trim() + "' between ConFormulaH.FStartDate and ConFormulaH.FEndDate ) a group by contractid ) aa on (contract.contractid=aa.contractid) where 1=2";
            BindDate(SqlStr);
        }
    }

    private void ShowShopTree()
    {
        BaseBO objBase = new BaseBO();
        Resultset rs = new Resultset();
        Dept objDept = new Dept();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
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
            return;
        }
        objBase.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;//商业项目
        objBase.OrderBy = " OrderID ";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBase.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = objBase.Query(objDept);
        objBase.OrderBy = "";
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
                            string whereSql = " where conshop.FloorID=" + floors.FloorID + " and contractid in (select contractid from contract where contractstatus in (" + SqlConType + ")) ";
                            DataSet ds = objBase.QueryDataSet("select conshop.shopid,conshop.shopcode,conshop.shopname from conshop " + whereSql);
                            if (ds.Tables[0].Rows.Count > 0)
                            { 
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    jsdept += store.DeptID.ToString() + floors.FloorID.ToString() + ds.Tables[0].Rows[i][0].ToString() + "|" + store.DeptID.ToString() + floors.FloorID.ToString() + "|" + ds.Tables[0].Rows[i][1].ToString() + ds.Tables[0].Rows[i][2].ToString() + "|" + 0 + "^";
                                }
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
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
        p = 0;
        if (treestr == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '商铺选择为空'", true);
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

            if (CheckBox3.Checked == true)
            {
                for (int i = 11; i <= 29; i++)
                {
                    ShopView.Columns[i].Visible = true;
                }
            }
            else
            {
                for (int i = 11; i <= 29; i++)
                {
                    ShopView.Columns[i].Visible = false;
                }
            }

            #region 选择商铺
            if (p == 0)//选择商铺
            {
                Session["whereSqlStr"] = "where '" + txtChargeDate.Text.ToString().Trim() + "' between ConFormulaH.FStartDate and ConFormulaH.FEndDate ) a group by contractid ) aa on (contract.contractid=aa.contractid) where  conshop.shopid in (" + strShopID.TrimEnd(',') + ") and conshop.contractid in (select contractid from contract where contractstatus in (" + SqlConType + "))";
                BindDate(SqlStr);
            }
            #endregion
            #region 选择楼层
            if (p == 1)//选择楼层
            {
                Session["whereSqlStr"] = "where '" + txtChargeDate.Text.ToString().Trim() + "' between ConFormulaH.FStartDate and ConFormulaH.FEndDate ) a group by contractid ) aa on (contract.contractid=aa.contractid) where  conshop.floorid in (" + strFloorID.TrimEnd(',') + ") and conshop.contractid in (select contractid from contract where contractstatus in (" + SqlConType + "))";
                BindDate(SqlStr);
            }
            #endregion
            #region 选择项目
            if (p == 2)//选择项目
            {
                Session["whereSqlStr"] = "where '" + txtChargeDate.Text.ToString().Trim() + "' between ConFormulaH.FStartDate and ConFormulaH.FEndDate ) a group by contractid ) aa on (contract.contractid=aa.contractid) where  conshop.storeid in (" + strStoreID.TrimEnd(',') + ") and conshop.contractid in (select contractid from contract where contractstatus in (" + SqlConType + "))";
                BindDate(SqlStr);
            }
            #endregion

        }
        
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  ''", true);
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        //ViewState["ID"] = deptid.Value;
        //if (ViewState["ID"].ToString().Length > 9)
        //{
        //    this.btnAdd.Enabled = true;
        //}
        //else
        //{
        //    this.btnAdd.Enabled = false;
        //}
        ////ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void ShopView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 绑定列表数据
    /// </summary>
    private void BindDate(string SqlString)
    {
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        PagedDataSource pds = new PagedDataSource();
        ds = baseBo.QueryDataSet(SqlString + Session["whereSqlStr"].ToString());
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        DataSet objDs = new DataSet();
        string CPath = "C:/合同明细查询" + DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
        //if (!Directory.Exists(CPath))
        //    Directory.CreateDirectory(CPath);

            //#region 选择商铺
            //if (ViewState["ID"].ToString().Length == 9)//选择商铺
            //{
            //    string strShopID = ViewState["ID"].ToString().Substring(6);
            //    objDs = baseBo.QueryDataSet("select shopname from conshop where shopid='" + strShopID + "'");
            //    if (RbtMtn.Checked == true)
            //    {
            //        CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + TextBox4.Text.ToString().Substring(0, 7) + "销售月报.xls";
            //    }
            //    else if (RbtDay.Checked == true)
            //    {
            //        CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + TextBox1.Text.ToString() + "-" + TextBox2.Text.ToString() + "销售日报.xls";
            //    }
            //    else if (RbtDetail.Checked == true)
            //    {
            //        CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + TextBox1.Text.ToString() + "-" + TextBox2.Text.ToString() + "销售明细.xls";
            //    }
            //}
            //#endregion
            //#region 选择楼层
            //if (ViewState["ID"].ToString().Length == 6)//选择楼层
            //{
            //    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
            //    string strFloorID = ViewState["ID"].ToString().Substring(3);
            //    objDs = baseBo.QueryDataSet("select building.buildingname,floors.floorname from building,floors where building.buildingid='" + strBuildingID + "' and floors.floorid='" + strFloorID + "'");
            //    if (RbtMtn.Checked == true)
            //    {
            //        CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + objDs.Tables[0].Rows[0][1].ToString().Trim() + TextBox4.Text.ToString().Substring(0, 7) + "销售月报.xls";
            //    }
            //    else if (RbtDay.Checked == true)
            //    {
            //        CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + objDs.Tables[0].Rows[0][1].ToString().Trim() + TextBox1.Text.ToString() + "-" + TextBox2.Text.ToString() + "销售日报.xls";
            //    }
            //    else if (RbtDetail.Checked == true)
            //    {
            //        CPath += objDs.Tables[0].Rows[0][0].ToString().Trim() + objDs.Tables[0].Rows[0][1].ToString().Trim() + TextBox1.Text.ToString() + "-" + TextBox2.Text.ToString() + "销售明细.xls";
            //    }
            //}
            //#endregion
            
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



                //objDs = baseBo.QueryDataSet(SqlStr + Session["whereSqlStr"].ToString() + " order by a.shopname,bizdate");
                //int j = 1;
                //for (int i = 0; i < CBList.Items.Count; i++)
                //{
                //    if (i == 3 || i == 7 || i == 8)
                //    {
                //        xSheet.Cells[1, j] = ShopView.Columns[i].HeaderText.ToString();
                //        for (int k = 2; k < objDs.Tables[0].Rows.Count + 2; k++)
                //        {
                //            xSheet.Cells[k, j] = objDs.Tables[0].Rows[k - 2][i].ToString();
                //        }
                //        j++;
                //    }
                //    else if (CBList.Items[i].Selected == true)
                //    {
                //        xSheet.Cells[1, j] = ShopView.Columns[i].HeaderText.ToString();
                //        for (int k = 2; k < objDs.Tables[0].Rows.Count + 2; k++)
                //        {
                //            xSheet.Cells[k, j] = objDs.Tables[0].Rows[k - 2][i].ToString();
                //        }
                //        j++;
                //    }
                //}


                for (int i = 0; i < ShopView.Columns.Count; i++)
                {
                    xSheet.Cells[1, i+1] = ShopView.Columns[i].HeaderText.ToString();
                    for (int j = 0; j < ShopView.Rows.Count; j++)
                    {
                        if (ShopView.Rows[j].Cells[0].Text.ToString() != "&nbsp;")
                        {
                            if (ShopView.Rows[j].Cells[i].Text.ToString().Trim() != "&nbsp;")
                            {
                                xSheet.Cells[j + 2, i + 1] = ShopView.Rows[j].Cells[i].Text.ToString().Trim();
                            }
                            else
                            {
                                xSheet.Cells[j + 2, i + 1] = "";
                            }
                        }
                    }
                }


                    xSheet.SaveAs(CPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                xSheet = null;
                xBook = null;
                xApp.Workbooks.Close();
                xApp.Quit();
                xApp = null;
                System.GC.Collect();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '成功导出到C盘根目录。'", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '导出错误。'", true);
            }
    }
}
