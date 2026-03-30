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
using Base.Page;
using BaseInfo.Dept;
using RentableArea;
using Lease.ConShop;
using Base.XML;

public partial class VisualAnalysis_ShopAttrOperate : BasePage
{
    public string baseinfo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        baseinfo = (String)GetGlobalResourceObject("BaseInfo", "ShopXml_ShopAttrVindicate");
        if (!this.IsPostBack)
        {
            this.ShowTree();
        }
    }
    /// <summary>
    /// 显示树形列表
    /// </summary>
    private void ShowTree()
    {
        string strSql = @"select conshop.shopid,conshop.storeid ShopingMallID,conshop.buildingid,conshop.floorid,.conshop.locationid,conshop.areaid,conshop.shopid unitid,conshop.shopcode,conshop.shopname ShopDesc
,conshopunit.floorarea,conshop.rentarea,1 as RentStatus,conshopbrand.brandname brand ,customer.custshortname customer,1 depth
from conshop
inner join (select conshopunit.shopid,sum(unit.floorarea) as floorarea,sum (unit.usearea) as usearea from conshopunit inner join unit on (conshopunit.unitid=unit.unitid) group by conshopunit.shopid) as conshopunit on (conshopunit.shopid=conshop.shopid)
inner join conshopbrand on (conshop.brandid=conshopbrand.brandid)
inner join contract on (conshop.contractid=contract.contractid)
inner join customer on (contract.custid=customer.custid)
where contract.contractstatus=1";
        string jsdept = "";
        BaseBO baseBO = new BaseBO();
        BaseBO baseBOBuilding = new BaseBO();
        BaseBO baseareaBO = new BaseBO();
        Resultset rs = new Resultset();
        Resultset rsd = new Resultset();
        Resultset rsf = new Resultset();
        Resultset rsl = new Resultset();
        Resultset rsu = new Resultset();
        Dept dept = new Dept();
        Dept deptGrp = new Dept();
        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;   //根节点,取得集团
        rs = baseBO.Query(dept);
        if (rs.Count == 1)
        {
            deptGrp = rs.Dequeue() as Dept;
            jsdept = deptGrp.DeptID + "|" + "0" + "|" + deptGrp.DeptName + "^";
        }
        else
        {
            return;
        }
        baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_MALL;
        rsd = baseBO.Query(dept);
        if (rsd.Count > 0)
        {
            foreach (Dept store in rsd)
            {
                jsdept += store.DeptID + "|" + deptGrp.DeptID + "|" + store.DeptName + "^";
                baseBOBuilding.WhereClause = "StoreId=" + store.DeptID;
                rs = baseBOBuilding.Query(new Building());

                if (rs.Count > 0)
                {
                    foreach (Building building in rs)
                    {
                        jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + "|" + store.DeptID.ToString() + "|" + building.BuildingName.ToString() + "^";
                        baseBO.WhereClause = "BuildingID=" + building.BuildingID;
                        rsf = baseBO.Query(new Floors());
                        foreach (Floors floors in rsf)
                        {
                            jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + store.DeptID.ToString() + floors.BuildingID.ToString() + "|" + floors.FloorName.ToString() + "^";

                            baseBO.WhereClause = "FloorID=" + floors.FloorID + " and BuildingID=" + building.BuildingID;
                            ConShop objConshop = new ConShop();
                            objConshop.SetQuerySql(strSql);
                            rsu = baseBO.Query(objConshop);
                            foreach (ConShop objShop in rsu)
                            {
                                jsdept += objShop.ShopId + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + objShop.ShopName + "^";
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    /// <summary>
    /// 清空输入框
    /// </summary>
    private void ClearText()
    {
        this.txtNoX.Text = "";
        this.txtNoY.Text = "";
        this.txtNameX.Text = "";
        this.txtNameY.Text = "";
    }
    /// <summary>
    /// 树的点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ShopID"] = deptid.Value;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        this.btnSave.Enabled = true;
        this.ClearText();
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select NoX,NoY,NameX,NameY from ShopXML where shopid ='" + deptid.Value.ToString() + "'");
        if (ds!=null&&ds.Tables[0].Rows.Count==1)
        {
            this.txtNoX.Text = ds.Tables[0].Rows[0]["NoX"].ToString();
            this.txtNoY.Text = ds.Tables[0].Rows[0]["NoY"].ToString();
            this.txtNameX.Text = ds.Tables[0].Rows[0]["NameX"].ToString();
            this.txtNameY.Text = ds.Tables[0].Rows[0]["NameY"].ToString();
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["ShopID"] != null && ViewState["ShopID"].ToString() != "")
        {
            string strShopID = ViewState["ShopID"].ToString();//conshop.shopcode\shopname\rentarea\brand\customer
            ConShop objConShop = new ConShop();
            string strSql = @"select shopid,conshop.StoreID,ShopCode,ShopName,ConShop.RentArea,BrandID,(select BrandName from conshopbrand where conshopbrand.brandid=conshop.brandid) as BrandName,ContactorName,(select top 1 unitid from conshopunit where conshopunit.shopid=conshop.shopid) as UnitID,rb,gb,bb from ConShop
left join unit on unitid = (select top 1 unitid from conshopunit where conshopunit.shopid=conshop.shopid)
left join traderelation on traderelation.tradeid=unit.trade2id
 where ShopId='" + strShopID + "'";
            BaseBO objBaseBo = new BaseBO();
            ShopXMLInfo objShopXml = new ShopXMLInfo();
            DataSet ds = objBaseBo.QueryDataSet(strSql);
            if(ds!=null&&ds.Tables[0].Rows.Count==1)
            {
                objShopXml.StoreID  = ds.Tables[0].Rows[0]["StoreID"].ToString();
                objShopXml.ShopCode = ds.Tables[0].Rows[0]["ShopCode"].ToString();
                objShopXml.ShopDesc = ds.Tables[0].Rows[0]["ShopName"].ToString();
                objShopXml.RentArea = ds.Tables[0].Rows[0]["RentArea"].ToString();
                objShopXml.Brand = ds.Tables[0].Rows[0]["BrandName"].ToString();
                objShopXml.Customer = ds.Tables[0].Rows[0]["ContactorName"].ToString();
                objShopXml.NoX = this.txtNoX.Text.Trim();
                objShopXml.NoY = this.txtNoY.Text.Trim();
                objShopXml.NameX = this.txtNameX.Text.Trim();
                objShopXml.NameY = this.txtNameY.Text.Trim();
                objShopXml.RentStatus = "1";
                objShopXml.rb = ds.Tables[0].Rows[0]["rb"].ToString();
                objShopXml.gb = ds.Tables[0].Rows[0]["gb"].ToString();
                objShopXml.bb = ds.Tables[0].Rows[0]["bb"].ToString();
            }
            DataSet dsShopXml = objBaseBo.QueryDataSet("select NoX,NoY,NameX,NameY from ShopXML where id ='" + ViewState["ShopID"].ToString() + "'");
            if (dsShopXml != null && dsShopXml.Tables[0].Rows.Count == 1)//更新
            {
                objBaseBo.WhereClause = "id=" + ViewState["ShopID"].ToString();
                if (objBaseBo.Update(objShopXml) != -1)
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                    return;
                }
            }
            else//新增
            {
                objShopXml.ShopID  = ViewState["ShopID"].ToString();
                if (objBaseBo.Insert(objShopXml) != -1)
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
                else
                {
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                    return;
                }
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        }
        ViewState["ShopID"] = "";
        this.ClearText();
        this.btnSave.Enabled = false;
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.ClearText();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
}
