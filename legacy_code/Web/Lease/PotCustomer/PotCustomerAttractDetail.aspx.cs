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
using System.Data.SqlClient;
using System.Text;

using BaseInfo.Dept;
using Base.Biz;
using BaseInfo.User;
using Base.DB;
using Base;
using Base.Page;
using Lease.PotCustLicense;
using Lease.PotCust;
using System.Drawing;
using Lease.Contract;
using Shop.ShopType;

public partial class Lease_PotCustomer_PotCustomerAttractDetail : BasePage
{
    private string strCustID = "0";
    private void Page_Load(object sender, System.EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "a1", "styletabbar_atv()", true);
        if (!this.IsPostBack)
        {
            this.strCustID = Request["CustID"].ToString();
            ViewState["CustID"] = Request["CustID"].ToString();
            this.BindCustType();//绑定商户类型
            this.BindData(strCustID);//绑定列表
            this.BindCustData(this.strCustID);//绑定商户基本信息
            this.BindCustPalaver("0");//绑定商户谈判记录
            this.BindBizMode();//绑定经营方式
            this.BindShopType();//绑定商铺类型
            this.BindBusinessItem();//绑定商业项目
            this.BindProcessType();//绑定招商进程级别
        }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="strCustID"></param>
    private void BindData(string strcustid)
    {
        BaseBO objBaseBo = new BaseBO();
        string strSql = "select CustID,isnull(ShopSort,1) as ShopSort,(select DeptName from dept where dept.deptid=potshop.storeid) as StoreName from PotShop where custid=" + strcustid + "";
        DataTable dt = objBaseBo.QueryDataSet(strSql).Tables[0];
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
    /// <summary>
    /// 绑定商户类型
    /// </summary>
    private void BindCustType()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "CustTypeStatus=1";
        CustType objCustType = new CustType();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, objCustType, "CustTypeID", "CustTypeName", this.ddlCustType);
    }
    /// <summary>
    /// 绑定商户基本信息
    /// </summary>
    private void BindCustData(string strcustid)
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "custid = " + strcustid;
        Resultset rs = objBaseBo.Query(new PotCustomer());
        if (rs.Count == 1)
        {
            PotCustomer objPotCustomer = rs.Dequeue() as PotCustomer;
            this.txtCustCode.Text = objPotCustomer.CustCode;
            this.txtCustName.Text = objPotCustomer.CustName;
            this.txtCustShortName.Text = objPotCustomer.CustShortName;
            this.ddlCustType.SelectedValue = objPotCustomer.CustTypeID.ToString();
        }
    }
    /// <summary>
    /// 绑定商户谈判记录
    /// </summary>
    private void BindCustPalaver(string strcustid)
    {
        BaseBO objBaseBo = new BaseBO();
        string strSql = "select PalaverID,CustID,convert(char(10),PalaverTime,120) as PalaverTime,PalaverName,PalaverAim,PalaverContent,ProcessTypeId,ContactorName,PalaverRound,PalaverPlace,PalaverResult,UnSolved,Node from CustPalaver where CustID=" + strcustid + "";
        DataTable dt = objBaseBo.QueryDataSet(strSql).Tables[0];
        this.GrdCustPalaverInfo.DataSource = dt;
        this.GrdCustPalaverInfo.DataBind();
        int spareRow = GrdCustPalaverInfo.Rows.Count;
        for (int i = 0; i < GrdCustPalaverInfo.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        this.GrdCustPalaverInfo.DataSource = dt;
        this.GrdCustPalaverInfo.DataBind();
    }
    /// <summary>
    /// 绑定经营方式
    /// </summary>
    private void BindBizMode()
    {
        int[] bizModes = Contract.GetBizModes();
        for (int i = 0; i < bizModes.Length; i++)
        {
            this.ddlBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(bizModes[i])), bizModes[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定商铺类型
    /// </summary>
    private void BindShopType()
    {
        BaseBO objBaseBo = new BaseBO();
        Resultset rs = objBaseBo.Query(new ShopType());
        ddlShopType.Text = "";
        foreach (ShopType shopType in rs)
        {
            ddlShopType.Items.Add(new ListItem(shopType.ShopTypeName, shopType.ShopTypeID.ToString()));
        }
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    private void BindBusinessItem()
    {
        //绑定商业项目
        BaseInfo.BaseCommon.BindDropDownList("Select deptid,deptname from dept where depttype=6", "deptid", "deptname", this.ddlBusinessItem);
    }
    /// <summary>
    /// 绑定招商进程级别
    /// </summary>
    private void BindProcessType()
    {
        BaseInfo.BaseCommon.BindDropDownList("select ProcessTypeID,ProcessTypeName from ProcessType where status=1", "ProcessTypeID", "ProcessTypeName", this.ddlProcessTypeId);
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
        this.BindData(ViewState["CustID"].ToString());
        this.BindPalaver(Int32.Parse(ViewState["CustID"].ToString()),0);//绑定谈判记录p
    }
    protected void GrdCust_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[3].Text = "";
            }
        }
    }
    protected void GrdCustPalaverInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.BindData(ViewState["CustID"].ToString());
        this.BindPalaver(Int32.Parse(ViewState["CustID"].ToString()), Int32.Parse(ViewState["ShopSort"].ToString()));//绑定谈判记录p
        foreach (GridViewRow grv in GrdCustPalaverInfo.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
    protected void GrdCustPalaverInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")
            {
                e.Row.Cells[3].Text = "";
            }
        }
    }
    /// <summary>
    /// 绑定商铺意向信息
    /// </summary>
    private void BindShopIntent(int icustid,int iShopSort)
    {
        this.txtUnits.Text = "";
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "custid=" + icustid + " and isnull(shopsort,1)=" + iShopSort + "";
        Resultset rs = objBaseBo.Query(new PotShop());
        if (rs.Count == 1)
        {
            PotShop objPotShop = rs.Dequeue() as PotShop;
            this.ddlBizMode.SelectedValue = objPotShop.BizMode.ToString();
            this.ddlShopType.SelectedValue = objPotShop.ShopTypeID.ToString();
            this.ddlBusinessItem.SelectedValue = objPotShop.StoreID.ToString();
            this.txtRentalPrice.Text = objPotShop.RentalPrice.ToString();
            this.txtRentArea.Text = objPotShop.RentArea.ToString();
            this.txtRentInc.Text = objPotShop.RentInc;
            this.txtPcent.Text = objPotShop.Pcent;
            this.txtMainBrand.Text = objPotShop.MainBrand;
            this.txtShopStartDate.Text = objPotShop.ShopStartDate.ToShortDateString();
            this.txtShopEndDate.Text = objPotShop.ShopEndDate.ToShortDateString();
            this.txtPotShopName.Text = objPotShop.PotShopName;
            this.txtHighReg.Text = objPotShop.HighReg;
            this.txtLoadReg.Text = objPotShop.LoadReg;
            this.txtWaterReg.Text = objPotShop.WaterReg;
            this.txtPowerReg.Text = objPotShop.PowerReg;
            //显示意向单元
            objBaseBo.WhereClause = "";
            DataSet ds = objBaseBo.QueryDataSet("select unitid,unitcode,buildingid,floorid,locationid from PotShopUnit where potshopid='" + objPotShop.PotShopID + "'");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    this.txtUnits.Text += ds.Tables[0].Rows[i]["unitcode"].ToString() + ",";
                }
            }
        }
    }
    /// <summary>
    /// 绑定谈判记录
    /// </summary>
    /// <param name="icustid"></param>
    /// <param name="iShopSort"></param>
    private void BindPalaver(int icustid, int iShopSort)
    {
        BaseBO objBaseBo = new BaseBO();
        string strSql = "select PalaverID,CustID,convert(char(10),PalaverTime,120) as PalaverTime,PalaverName,PalaverAim,PalaverContent,ProcessTypeId,ContactorName,PalaverRound,PalaverPlace,PalaverResult,UnSolved,Node from CustPalaver where custid=" + icustid + " and palaversort=" + iShopSort + "";

        DataSet ds = objBaseBo.QueryDataSet(strSql);
        DataTable dt = ds.Tables[0];
        this.GrdCustPalaverInfo.DataSource = dt;
        this.GrdCustPalaverInfo.DataBind();
        int spareRow = GrdCustPalaverInfo.Rows.Count;
        for (int i = 0; i < GrdCustPalaverInfo.PageSize - spareRow; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        this.GrdCustPalaverInfo.DataSource = dt;
        this.GrdCustPalaverInfo.DataBind();
    }
    protected void GrdCust_SelectedIndexChanged(object sender, EventArgs e)
    {
        BaseBO objBaseBo = new BaseBO();
        int iCustID = Int32.Parse(this.GrdCust.SelectedRow.Cells[0].Text.Trim());
        int iShopSort = Int32.Parse(this.GrdCust.SelectedRow.Cells[1].Text.Trim());
        this.BindShopIntent(iCustID, iShopSort);//绑定商铺意向信息
        this.BindPalaver(iCustID, iShopSort);//绑定谈判记录
        this.BindData(ViewState["CustID"].ToString());
        ViewState["ShopSort"] = this.GrdCust.SelectedRow.Cells[1].Text.Trim();
    }
    protected void GrdCustPalaverInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iPalaverID = Int32.Parse(this.GrdCustPalaverInfo.SelectedRow.Cells[0].Text.Trim());
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "palaverid=" + iPalaverID;
        Resultset rs = objBaseBo.Query(new CustPalaverInfo());
        if (rs.Count == 1)
        {
            CustPalaverInfo objPalaver = rs.Dequeue() as CustPalaverInfo;
            this.ddlProcessTypeId.SelectedValue = objPalaver.ProcessTypeId.ToString();
            this.txtPalaverRound.Text = objPalaver.PalaverRound.ToString();
            this.txtPalaverPlace.Text = objPalaver.PalaverPlace;
            this.txtContactorName.Text = objPalaver.ContactorName;
            this.txtPalaverAim.Text = objPalaver.PalaverAim;
            this.txtPalaverContent.Text = objPalaver.PalaverContent;
            this.txtPalaverResult.Text = objPalaver.PalaverResult;
            this.txtUnSolved.Text = objPalaver.UnSolved;
            this.txtNode.Text = objPalaver.Node;
        }
        this.BindData(ViewState["CustID"].ToString());
        this.BindPalaver(Int32.Parse(ViewState["CustID"].ToString()), Int32.Parse(ViewState["ShopSort"].ToString()));//绑定谈判记录p
    }
}
