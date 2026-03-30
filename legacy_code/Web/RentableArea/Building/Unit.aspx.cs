using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base;
using Base.Biz;
using Base.DB;
using Base.Page;
using Base.Util;
using BaseInfo.Dept;
using BaseInfo.User;
using BaseInfo.authUser;
using RentableArea;
using System.IO;

public partial class RentableArea_Building_Unit : BasePage
{
    public string strBaseInfo = "";
    public string strFresh;
    private string strStoreID = "";
    private string UnitID = "";
    public string strLink = "";
    //private string strType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!this.IsPostBack)
        {
            ViewState["checkeds"] = "";
            ViewState["UnitID"] = "";
            ViewState["strWhere"] = "and 1=2";

            //this.strType = Request["Type"].ToString();
            ViewState["Type"] = Request["Type"].ToString();
            this.ShowTree();//加载树
            this.BindGrd(ViewState["strWhere"].ToString());
            if (ViewState["Type"].ToString().ToLower() == "add")
            {
                this.btnEdit.Visible = false;
                this.Button1.Visible = false;
            }
            if (ViewState["Type"].ToString().ToLower() == "edit")
            {
                this.btnAdd.Visible = false;
                this.btnExport.Visible = false;
                this.btnSave.Visible = false;
            }
            else if (ViewState["Type"].ToString().ToLower() == "browse")
            {
                this.btnAdd.Visible = false;
                this.btnEdit.Visible = false;
                this.btnExport.Visible = false;
                this.Button1.Visible = false;
                this.btnSave.Visible = false;
            }
            strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_labUnitTitle");
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            this.strLink = "RentableArea/Building/Unit.aspx?Type=" + ViewState["Type"].ToString() + "&funcid=65537";
        }
    }
    /// <summary>
    /// 加载树列表
    /// </summary>
    /// <param name="strType"></param>
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO objBaseBo = new BaseBO();
        Resultset rs = new Resultset();
        Dept objDept = new Dept();
        objBaseBo.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        rs = objBaseBo.Query(objDept);
        if (rs.Count == 1)
        {
            objDept = rs.Dequeue() as Dept;
            jsdept = objDept.DeptID + "|" + "0" + "|" + objDept.DeptName + "^";
        }
        else
            return;
        objBaseBo.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        objBaseBo.OrderBy = "OrderID";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = objBaseBo.Query(objDept);
        if (rs.Count > 0)
        {
            foreach (Dept store in rs)
            {
                jsdept += store.DeptID + "|" + objDept.DeptID + "|" + store.DeptName + "^";
                objBaseBo.WhereClause = "StoreId=" + store.DeptID;
                objBaseBo.OrderBy = "";
                rs = objBaseBo.Query(new Building());
                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                        objBaseBo.WhereClause = "floors.BuildingID=" + building.BuildingID;
                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            objBaseBo.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        }
                        rs = objBaseBo.Query(new floorsAuth());
                        foreach (floorsAuth floors in rs)
                        {
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + floors.BuildingID.ToString() + "|" + floors.FloorName.ToString() + "^";
                            objBaseBo.WhereClause = "FloorID=" + floors.FloorID;
                            rs = objBaseBo.Query(new Location());
                            foreach (Location location in rs)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + location.LocationID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + location.LocationName + "^";
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    /// <summary>
    /// 绑定列表数据
    /// </summary>
    /// <param name="strType">单元类型</param>
    private void BindGrd(string strWhere)
    {
        BaseBO objBaseBo = new BaseBO();
        Units objUnit = new Units();
        string strSql = @"select unit.UnitID,unit.BuildingID,building.buildingName,unit.AreaID,Area.AreaName,unit.FloorID,floors.floorname,                  unit.LocationID,Location.locationName,unit.UnitCode,unit.AreaLevelID,unit.FloorArea,unit.UseArea,unit.Note,unit.UnitStatus,                  unit.Trade2ID,tradeRelation.TradeName,unit.UnitTypeID,unittype.UnitTypeName,unit.StoreID,unit.ShopTypeID,                                    ShopType.ShopTypeName from unit
                inner join unittype on unittype.UnitTypeID=unit.UnitTypeID
                inner join building on building.buildingID=unit.buildingID
                inner join Floors on Floors.floorid=unit.floorid
                inner join Location on Location.locationID=Unit.locationID
                inner join Area on Area.AreaID=Unit.AreaId
                inner join TradeRelation on TradeRelation.tradeid=Unit.trade2id
                inner join ShopType on ShopType.ShopTypeID=Unit.ShopTypeID
                where 1=1 " + strWhere;
        BaseInfo.BaseCommon.BindGridView(strSql,this.GrdUnit);
    }
    /// <summary>
    /// 树列表点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeClick_Click(object sender, EventArgs e)
    {
        string strUnitID = deptid.Value;
        string strWhere = " and 1=2";
        if (strUnitID.Length == 12)
        {
            Session["dsExcel"] = null;
            ViewState["UnitID"] = strUnitID.ToString();//项目大楼楼层方位
            string strLocationID = strUnitID.Substring(strUnitID.Length - 3);// 方位
            string strFloorID = strUnitID.Substring(6, 3);//楼层
            string strBuildingID = strUnitID.Substring(3, 3);//大楼
            this.strStoreID = strUnitID.Substring(0,3);//项目
            
            if (rbtNoLeaseOut.Checked)//未出租
            {
                strWhere = " and Unit.LocationID=" + strLocationID + " and Unit.FloorID=" + strFloorID + " and Unit.BuildingID=" + strBuildingID + " and Unit.UnitStatus='" + Units.BLANKOUT_STATUS_INVALID + "'";
            }
            if (rbtLeaseOut.Checked)//已出租
            {
                strWhere = " and Unit.LocationID=" + strLocationID + " and Unit.FloorID=" + strFloorID + " and Unit.BuildingID=" + strBuildingID + " and Unit.UnitStatus='" + Units.BLANKOUT_STATUS_LEASEOUT + "'";
            }
            if (rbtBlankOut.Checked)//已作废
            {
                strWhere = " and Unit.LocationID=" + strLocationID + " and Unit.FloorID=" + strFloorID + " and Unit.BuildingID=" + strBuildingID + " and UnitStatus='" + Units.BLANKOUT_STATUS_VALID + "'";
            }
            this.btnAdd.Enabled = true;
            this.btnCancel.Enabled = true;
            this.btnEdit.Enabled = true;
            //this.btnSave.Enabled = true;
            //this.btnDel.Enabled = true;
            this.btnExport.Enabled = true;
            this.Button1.Disabled = false ;
            ViewState["strWhere"] = strWhere;
        }
        this.BindGrd(strWhere);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void GrdUnit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            if (e.Row.Cells[1].Text.Trim() == "&nbsp;")
                e.Row.Cells[0].Text = "";
            else if (ViewState["Type"].ToString().ToLower()!="add")
            {
                string strU = ViewState["UnitID"].ToString() + e.Row.Cells[1].Text.Trim();
                e.Row.Attributes["ondblclick"] = string.Format("javascript:window.showModalDialog('UnitInfo.aspx?UnitID=" + strU + "&Type=" + ViewState["Type"].ToString() + "','','dialogWidth=300px;dialogHeight=430px');");
            }
        }
    }
    /// <summary>
    /// 已出租
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbtLeaseOut_CheckedChanged(object sender, EventArgs e)
    {
        this.BindGrd("and 1=2");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 未出租
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbtNoLeaseOut_CheckedChanged(object sender, EventArgs e)
    {
        this.BindGrd("and 1=2");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 已作废
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rbtBlankOut_CheckedChanged(object sender, EventArgs e)
    {
        this.BindGrd("and 1=2");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //this.BindGrd("and 1=2");
        this.BindGrd(ViewState["strWhere"].ToString());
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        Session["dsExcel"] = null;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.BindGrd(ViewState["strWhere"].ToString());
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "Win", "window.showModalDialog('UnitInfo.aspx?UnitID=" + ViewState["UnitID"].ToString() + "&Type=Add','','dialogWidth=300px;dialogHeight=430px')", true);
    }
    /// <summary>
    /// 记录表中选中记录的情况
    /// </summary>
    /// <returns></returns>
    private void FindChecked()
    {
        string checkeds = "";
        if (ViewState["checkeds"] != null)
            checkeds = "," + ViewState["checkeds"].ToString() + ",";
        for (int i = 0; i < this.GrdUnit.Rows.Count; i++)
        {
            string strUnit = this.GrdUnit.Rows[i].Cells[1].Text.Trim();
            if (((System.Web.UI.WebControls.CheckBox)this.GrdUnit.Rows[i].Cells[0].FindControl("Checkbox")).Checked)
            {
                if (checkeds.IndexOf("," + strUnit + ",") < 0)
                    checkeds += strUnit + ",";
            }
            else
                checkeds = checkeds.Replace("," + strUnit + ",", ",");//如果没选中则在串中去掉
        }
        ViewState["checkeds"] = checkeds.TrimEnd(',').TrimStart(',');
    }
    /// <summary>
    /// 记录每页选中的情况
    /// </summary>
    /// <param name="strHaveSelects"></param>
    public void SetDataGridSelectRecords(string strHaveSelects)
    {
        strHaveSelects = "," + strHaveSelects.TrimEnd(',').TrimStart(',') + ",";
        for (int i = 0; i < this.GrdUnit.Rows.Count; i++)
        {
            string strUnit = this.GrdUnit.Rows[i].Cells[1].Text.Trim();
            if (strUnit != "")
            {
                if (strHaveSelects.IndexOf("," + strUnit + ",") >= 0)
                {
                    ((System.Web.UI.WebControls.CheckBox)this.GrdUnit.Rows[i].Cells[0].FindControl("Checkbox")).Checked = true;
                }
            }
        }
    }
    protected void GrdUnit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        FindChecked();//记录选择
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
        if (Session["dsExcel"] != null)
            this.BindImportData((DataSet)Session["dsExcel"]);
        else
        {
            this.BindGrd(ViewState["strWhere"].ToString());
            SetDataGridSelectRecords(ViewState["checkeds"].ToString());//设置选择项
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnD_Click(object sender, EventArgs e)
    {
        FindChecked();//记录选择
        if (ViewState["checkeds"].ToString() != "")
        {
            string strSql = @"select unit.unitid,unit.unitcode,contract.contractid from unit left join conshopunit
                            on unit.unitid=conshopunit.unitid
                            left join conshop on conshop.shopid=conshopunit.shopid
                            left join contract on contract.contractid=conshop.contractid where unit.unitid in (" + ViewState["checkeds"].ToString() + ")";
            BaseBO objBaseBo = new BaseBO();
            string strCode = "";
            DataSet ds = objBaseBo.QueryDataSet(strSql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["contractid"].ToString() != "")//该单元已经签了合同
                        strCode += ds.Tables[0].Rows[i]["unitcode"].ToString() + ",";
                }
            }
            if (strCode.TrimEnd(',').TrimStart(',') != "")
            {
                string strErr = strCode + " 单元已经签了合同，不能删除！";
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + strErr + "'", true);
                ViewState["checkeds"] = "";
                this.BindGrd(ViewState["strWhere"].ToString());
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
                return;
            }
            else
            {
                objBaseBo.ExecuteUpdate("delete from unit where unit.unitid in (" + ViewState["checkeds"].ToString() + ")");
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "删除成功。" + "'", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "没有选择要删除的单元。" + "'", true);
        }
        ViewState["checkeds"] = "";
        this.BindGrd(ViewState["strWhere"].ToString());
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        FindChecked();//记录选择
        if (ViewState["checkeds"].ToString().TrimEnd(',').TrimStart(',') == "" || ViewState["checkeds"].ToString().TrimEnd(',').TrimStart(',').Length > 3)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "请选择一个单元进行修改。" + "'", true);
            ViewState["checkeds"] = "";
            //this.BindGrd(ViewState["strWhere"].ToString());
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
            return;
        }
        string strU = ViewState["UnitID"].ToString() + ViewState["checkeds"].ToString();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "WinOpen", "window.showModalDialog('UnitInfo.aspx?UnitID=" + strU + "&Type=Edit','','dialogWidth=300px;dialogHeight=430px')", true);
    }
    /// <summary>
    /// 导入
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        //Session["dsExcel"] = null;ViewState["UnitID"]
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "Win", "window.open('UnitDataExport.aspx?LocationID=" + ViewState["UnitID"].ToString()+ "','','height=200,width=450,toolbar=no,menubar=no,scrollbars=no,resizable=no,location=no,status=no,left=300,top=300')", true);
        this.BindGrd(ViewState["strWhere"].ToString());
    }
    /// <summary>
    /// 导入数据，加载数据到列表中
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReFresh_Click(object sender, EventArgs e)
    {
        if(Session["dsExcel"]==null)
            return;
        DataSet ds = (DataSet)Session["dsExcel"];
        ds = this.ChangeDataSetColumnsName(ds);//更改数据集的列标题
        for (int i = ds.Tables[0].Rows.Count-1; i >0; i--)
        {
            if (ds.Tables[0].Rows[i][3].ToString().Trim() == "" || ds.Tables[0].Rows[i][8].ToString().Trim() == "" || ds.Tables[0].Rows[i][9].ToString().Trim()=="")
                ds.Tables[0].Rows.RemoveAt(i);
        }
        ds.Tables[0].AcceptChanges();
        
        this.BindImportData(ds);//绑定导入的数据
        this.btnSave.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
    }
    /// <summary>
    /// 绑定导入的数据
    /// </summary>
    private void BindImportData(DataSet ds)
    {
        //DataTable dt = new DataTable();
        //DataRow dr = new DataRow();
        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //{
        //    if (ds.Tables[0].Rows[i][3].ToString().Trim() == "")
        //        ds.Tables[0].Rows.RemoveAt(i);
        //}
        //ds.Tables[0].AcceptChanges();
        this.GrdUnit.DataSource = ds.Tables[0];
        this.GrdUnit.DataBind();
        int spareRow = this.GrdUnit.Rows.Count;
        for (int i = 0; i < this.GrdUnit.PageSize - spareRow; i++)
        {
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
        }
        
        ds.Tables[0].AcceptChanges();
        this.GrdUnit.DataSource = ds.Tables[0];
        this.GrdUnit.DataBind();
        foreach (GridViewRow gr in this.GrdUnit.Rows)
        {
            gr.Cells[0].Text = "";
        }
    }
    /// <summary>
    /// 更改数据集的列标题
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    private DataSet ChangeDataSetColumnsName(DataSet ds)
    {
        ds.Tables[0].Columns.Add("UnitID");
        ds.Tables[0].Columns[0].ColumnName = "BuildingName";
        ds.Tables[0].Columns[1].ColumnName = "FloorName";
        ds.Tables[0].Columns[2].ColumnName = "LocationName";
        ds.Tables[0].Columns[3].ColumnName = "UnitCode";
        ds.Tables[0].Columns[4].ColumnName = "UnitTypeName";//
        ds.Tables[0].Columns[5].ColumnName = "AreaName";
        ds.Tables[0].Columns[6].ColumnName = "TradeName";
        ds.Tables[0].Columns[7].ColumnName = "ShopTypeName";
        ds.Tables[0].Columns[8].ColumnName = "FloorArea";
        ds.Tables[0].Columns[9].ColumnName = "UseArea";
        ds.Tables[0].Columns.Add("StoreID");
        ds.AcceptChanges();
        return ds;
    }
    /// <summary>
    /// 将Excel中的数据导入到数据库中
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Session["dsExcel"] == null)
        {
            foreach (GridViewRow gr in this.GrdUnit.Rows)
            {
                if (gr.Cells[1].Text.Trim() == "&nbsp;" || gr.Cells[1].Text.Trim()=="")
                    gr.Cells[0].Text = "";
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
            return;
        }
        DataSet ds = (DataSet)Session["dsExcel"];
        ArrayList arrList = new ArrayList();
        bool bCheckAll = true;//判断所有数据都正确
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            int iRow = i + 1;
            string strBuildingName = ds.Tables[0].Rows[i][0].ToString();//大楼名称
            if (strBuildingName != "")
            {
                string strFloorName = ds.Tables[0].Rows[i][1].ToString();//楼层名称
                string strLocationName = ds.Tables[0].Rows[i][2].ToString();//方位名称
                string strUnitCode = ds.Tables[0].Rows[i][3].ToString();//单元编码
                string strUntiTypeName = ds.Tables[0].Rows[i][4].ToString();//单元类别
                string strAreaName = ds.Tables[0].Rows[i][5].ToString();//经营区域
                string strTradeName = ds.Tables[0].Rows[i][6].ToString();//业态
                string strShopTypeName = ds.Tables[0].Rows[i][7].ToString();//商铺类型
                //string strFloorArea = ds.Tables[0].Rows[i][8].ToString();//建筑面积
                //string strUseArea = ds.Tables[0].Rows[i][9].ToString();//出租面积

                Units objUnit = new Units();
                objUnit.UnitID = BaseApp.GetUnitID();
                objUnit.BuildingID = this.CheckBuilding(strBuildingName);//检查大楼
                objUnit.FloorID = this.CheckFloor(strFloorName, objUnit.BuildingID.ToString());//检查楼层
                objUnit.LocationID = this.CheckLocation(objUnit.FloorID.ToString(), strLocationName);//检查方位
                bool bCheckUnit = this.CheckUnitCode(strUnitCode);//检查单元编码
                if (bCheckUnit)
                    objUnit.UnitCode = strUnitCode;
                objUnit.UnitTypeID = this.CheckUnitType(strUntiTypeName);//检查单元类别
                objUnit.AreaID = this.CheckArea(strAreaName);//检查经营区域
                objUnit.Trade2ID = this.CheckTrade(strTradeName);//检查业态
                objUnit.ShopTypeID = this.CheckShopType(strShopTypeName);//检查商铺类型
                objUnit.StoreID = this.CheckStore(strBuildingName);//检查项目
                objUnit.Note = "";
                objUnit.UnitStatus = 0;
                try { objUnit.FloorArea = decimal.Parse(ds.Tables[0].Rows[i][8].ToString()); }//建筑面积
                catch { }
                try { objUnit.UseArea = decimal.Parse(ds.Tables[0].Rows[i][9].ToString()); }//出租面积
                catch { }

                if (objUnit.BuildingID == 0)
                    this.WriteLog(strBuildingName + " 大楼不存在", "错误行数：" + iRow);
                if (objUnit.FloorID == 0)
                    this.WriteLog(strFloorName + " 楼层不存在", "错误行数：" + iRow);
                if (objUnit.LocationID == 0)
                    this.WriteLog(strLocationName + " 方位不存在", "错误行数：" + iRow);
                if (objUnit.UnitTypeID == 0)
                    this.WriteLog(strUntiTypeName + " 单元类别不存在", "错误行数：" + iRow);
                if (objUnit.AreaID == 0)
                    this.WriteLog(strAreaName + " 经营区域不存在", "错误行数：" + iRow);
                if (objUnit.Trade2ID == 0)
                    this.WriteLog(strTradeName + " 业态不存在", "错误行数：" + iRow);
                if (objUnit.ShopTypeID == 0)
                    this.WriteLog(strShopTypeName + " 商铺类型不存在", "错误行数：" + iRow);
                if (bCheckUnit == false)
                    this.WriteLog(strUnitCode + " 单元编码已存在", "错误行数：" + iRow);

                if (objUnit.BuildingID != 0 && objUnit.FloorID != 0 && objUnit.LocationID != 0 && objUnit.UnitTypeID != 0 && objUnit.AreaID != 0 && objUnit.Trade2ID != 0 && objUnit.ShopTypeID != 0 && bCheckUnit == true)
                    arrList.Add(objUnit);
                else
                    bCheckAll = false;
            }
        }
        if (arrList != null && arrList.Count > 0)//保存数据
        {
            if (bCheckAll == true)//所有数据都正确
            {
                if (this.InsertObj(arrList) == false)
                {
                    this.WriteLog("error", "保存数据出错");//写日志
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "保存数据出错。" + "'", true);
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "数据中有错误，请查看错误日志。" + "'", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "数据加载出错。" + "'", true);

        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "保存数据成功。" + "'", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "treearray()", true);
        this.BindGrd(ViewState["strWhere"].ToString());
        Session["dsExcel"] = null;
    }
    /// <summary>
    /// 保存对象
    /// </summary>
    /// <param name="arrlist"></param>
    /// <returns></returns>
    private bool InsertObj(ArrayList arrList)
    {
        BaseTrans objTrans = new BaseTrans();
        try
        {
            objTrans.BeginTrans();//开始事务
            for (int i = 0; i < arrList.Count; i++)
            {
                if (arrList[i].ToString() != "")
                    objTrans.Insert((Units)arrList[i]);
            }
            objTrans.Commit();
            return true;
        }
        catch(Exception ex) 
        {
            objTrans.Rollback();
            return false;
        }
    }
    /// <summary>
    /// 检查商业项目
    /// </summary>
    /// <param name="strBuildingName"></param>
    /// <returns></returns>
    private int CheckStore(string strBuildingName)
    {
        int iStoreID = 0;
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select StoreID from Building where BuildingName='" + strBuildingName + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                iStoreID = Int32.Parse(ds.Tables[0].Rows[0]["StoreID"].ToString());
            }
            catch { }
        }
        return iStoreID;
    }
    /// <summary>
    /// 检查大楼是否存在
    /// </summary>
    /// <param name="strBuildingName"></param>
    /// <returns></returns>
    private int CheckBuilding(string strBuildingName)
    {
        int iBuildingID = 0;
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("Select BuildingID From Building Where BuildingName='" + strBuildingName + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                iBuildingID = Int32.Parse(ds.Tables[0].Rows[0]["BuildingID"].ToString());
            }
            catch
            { }
        }
        return iBuildingID;
    }
    /// <summary>
    /// 检查楼层
    /// </summary>
    /// <param name="strfloorCode">大楼编码</param>
    /// <param name="strBuildingID">大楼ID</param>
    /// <returns></returns>
    private int CheckFloor(string strfloorName, string strBuildingID)
    {
        int iFloorID = 0;
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("Select FloorID From Floors Where FloorName='" + strfloorName + "' And BuildingID='" + strBuildingID + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                iFloorID = Int32.Parse(ds.Tables[0].Rows[0]["FloorID"].ToString());
            }
            catch
            { }
        }
        return iFloorID;
    }
    /// <summary>
    /// 检查方位
    /// </summary>
    /// <param name="strFloorID"></param>
    /// <returns></returns>
    private int CheckLocation(string strFloorID,string strLocationName)
    {
        int iLocationID = 0;
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("Select LocationID From Location Where FloorID='" + strFloorID + "' and LocationName='" + strLocationName + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                iLocationID = Int32.Parse(ds.Tables[0].Rows[0]["LocationID"].ToString());
            }
            catch { }
        }
        return iLocationID;
    }
    /// <summary>
    /// 检查是否存在单元编码
    /// </summary>
    /// <param name="strUnitCode"></param>
    /// <returns></returns>
    private bool CheckUnitCode(string strUnitCode)
    {
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select UnitID from Unit where UnitCode='" + strUnitCode + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
            return false;
        else
            return true;
    }
    /// <summary>
    /// 检查单元类别
    /// </summary>
    /// <param name="strUnitTypeName"></param>
    /// <returns></returns>
    private int CheckUnitType(string strUnitTypeName)
    {
        int iUnitType = 0;
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select UnitTypeID from UnitType where UnitTypeName='" + strUnitTypeName + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                iUnitType = Int32.Parse(ds.Tables[0].Rows[0]["UnitTypeID"].ToString());
            }
            catch { }
        }
        return iUnitType;
    }
    /// <summary>
    /// 检查经营区域
    /// </summary>
    /// <param name="strAreaName"></param>
    /// <returns></returns>
    private int CheckArea(string strAreaName)
    {
        int iAreaID = 0;
        BaseBO objbaseBo = new BaseBO();
        DataSet ds = objbaseBo.QueryDataSet("select AreaID from Area where AreaName='" + strAreaName + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                iAreaID = Int32.Parse(ds.Tables[0].Rows[0]["AreaID"].ToString());
            }
            catch { }
        }
        return iAreaID;
    }
    /// <summary>
    /// 检查业态
    /// </summary>
    /// <param name="strTradeName"></param>
    /// <returns></returns>
    private int CheckTrade(string strTradeName)
    {
        int iTradeID = 0;
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select TradeID from TradeRelation where TradeName='" + strTradeName + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                iTradeID = Int32.Parse(ds.Tables[0].Rows[0]["TradeID"].ToString());
            }
            catch { }
        }
        return iTradeID;
    }
    /// <summary>
    /// 检查商铺类型
    /// </summary>
    /// <param name="strShopTypeName"></param>
    /// <returns></returns>
    private int CheckShopType(string strShopTypeName)
    {
        int iShopTypeID = 0;
        BaseBO objbaseBo = new BaseBO();
        DataSet ds = objbaseBo.QueryDataSet("select ShopTypeID from ShopType where ShopTypeName='" + strShopTypeName + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            try
            {
                iShopTypeID = Int32.Parse(ds.Tables[0].Rows[0]["ShopTypeID"].ToString());
            }
            catch { }
        }
        return iShopTypeID;
    }
    /// <summary>
    /// 写日志
    /// </summary>
    /// <param name="title"></param>
    /// <param name="strlog"></param>
    public void WriteLog(string strTitle, string strLog)
    {
        string strLogFilePath = ParamManager.DEPLOY_PATH + "/Log/";
        if (!Directory.Exists(strLogFilePath))//判断文件夹是否存在，若不存在，则创建
        {
            Directory.CreateDirectory(strLogFilePath);//创建文件夹 
        }
        FileStream fs = new FileStream(strLogFilePath + "Unit_Data_LOG.txt", FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter streamWriter = new StreamWriter(fs);
        streamWriter.BaseStream.Seek(0, SeekOrigin.End);
        streamWriter.WriteLine(" Unit_Data_Import:  " + strTitle + ":   " + strLog + ". " + DateTime.Now.ToString() + "\n");
        streamWriter.Flush();
        streamWriter.Close();
        fs.Close();
    }
}
