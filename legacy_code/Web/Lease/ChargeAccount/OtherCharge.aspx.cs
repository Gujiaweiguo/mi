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
using Base;
using Lease;
using Lease.ConShop;
using Invoice;
using Lease.PotBargain;
using BaseInfo.User;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using Base.Page;
using BaseInfo.Dept;
using RentableArea;
using BaseInfo.authUser;

public partial class Lease_ChargeAccount_OtherCharge : BasePage
{
    public string baseinfo;
    /// <summary>
    /// 用于绑定的表
    /// </summary>
    protected DataTable ChargeDetailDT
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
    protected DataTable SaveChargeDetailDT
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
        baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Account_lblOtherCharge");
        if (!this.IsPostBack)
        {
            this.ShowTree();
            this.BindChargeType();//绑定费用类型
            this.txtStartDate.Text = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "-01";//开始日期
            this.txtEndDate.Text = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "-" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();//结束日期
            ViewState["count"] = 0;
            ViewState["flag"] = "0";
            IniChargeDetailDT();
            if (Request.QueryString["VoucherID"] != null)
            {
                int chgID = Convert.ToInt32(Request["VoucherID"]);
                ViewState["chgID"] = chgID;

                HttpCookie cookies = new HttpCookie("Info");
                cookies.Expires = System.DateTime.Now.AddDays(1);
                cookies.Values.Add("conID", Request["VoucherID"].ToString());
                cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                cookies.Values.Add("sequence", Request.QueryString["Sequence"].ToString());
                cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());

                Response.AppendCookie(cookies);

                this.page(chgID);

                ViewState["flag"] = "2";
                btnAdd.Enabled = false;
                //btnEdit.Enabled = false;

                WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"].ToString()));
                string ss = objWrkFlwEntity.VoucherMemo;
                //this.listBoxRemark.Text = ss;
                btnAdd.Enabled = false;
            }
            else
            {
                page(0);
                this.btnPutIn.Enabled = false;//提交
                btnBlankOut.Enabled = false;
            }
        }
    }
    protected void IniChargeDetailDT()
    {
        ChargeDetailDT = new DataTable();
        //ChargeDetailDT.Columns.Add("ChgPeriod");
        ChargeDetailDT.Columns.Add("OtherChargeDID");
        ChargeDetailDT.Columns.Add("ChargeTypeID");
        ChargeDetailDT.Columns.Add("ChgName");
        ChargeDetailDT.Columns.Add("StartDate");
        ChargeDetailDT.Columns.Add("EndDate");
        ChargeDetailDT.Columns.Add("ChgAmt");
        ChargeDetailDT.Columns.Add("Note");
        ChargeDetailDT.Columns.Add("RefID");
        ChargeDetailDT.Columns.Add("ShopID");
        ChargeDetailDT.Columns.Add("ShopName");
        SaveChargeDetailDT = ChargeDetailDT.Clone();
    }
    /// <summary>
    /// 绑定费用类型
    /// </summary>
    private void BindChargeType()
    {
        ddlChargeType.Items.Clear();
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "ChargeClass = " + ChargeType.CHARGECLASS_MAINTAIN + " or ChargeClass = " + ChargeType.CHARGECLASS_OTHER;
        Resultset chageRs = objBaseBo.Query(new ChargeType());
        foreach (ChargeType chargeType in chageRs)
        {
            ddlChargeType.Items.Add(new ListItem(chargeType.ChargeTypeName, chargeType.ChargeTypeID.ToString()));
        }
    }
    protected void page(int chgId)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        baseBO.WhereClause = "RangeCode = " + chgId;
        //DataSet ds = baseBO.QueryDataSet(new OtherChargeD());
        OtherChargeD objOtherChargeD = new OtherChargeD();
        objOtherChargeD.SetQuerySql("select OtherChargeD.OtherChargeDID,OtherChargeD.OtherChargeHID as OtherChargeHID,OtherChargeD.ChargeTypeID,OtherChargeD.ChgName,OtherChargeD.ChgPeriod,Convert(char(10),StartDate,120) as StartDate,Convert(char(10),EndDate,120) as EndDate,OtherChargeD.RefID,OtherChargeD.ChgAmt,OtherChargeD.Note,OtherChargeH.shopid,(select shopname from conshop where conshop.shopid=OtherChargeH.shopid) as shopname from OtherChargeD left join OtherChargeH on OtherChargeD.otherchargehid=OtherChargeH.otherchargehid");
        DataSet ds = baseBO.QueryDataSet(objOtherChargeD);
        dt = ds.Tables[0];
        if (dt.Rows.Count > 0) ViewState["flag"] = "2";
        if (chgId != 0) ChargeDetailDT = dt;
        int count = dt.Rows.Count;

        for (int i = count; i < 15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvCharge.DataSource = dt;
        gvCharge.DataBind();
        ViewState["count"] = count;
    }
    /// <summary>
    /// 显示树形商铺
    /// </summary>
    /// <param name="strHdwTypeID"></param>
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO baseBO = new BaseBO();
        //BaseBO baseBOBuilding = new BaseBO();
        //BaseBO baseareaBO = new BaseBO();
        Resultset rs = new Resultset();
        //Resultset rsd = new Resultset();
        //Resultset rsf = new Resultset();
        //Resultset rsl = new Resultset();
        //Resultset rsu = new Resultset();
        Dept dept = new Dept();
        //Dept deptGrp = new Dept();

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
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            baseBO.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        rs = baseBO.Query(dept);
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
                            baseBO.WhereClause = "a.StoreId=" + store.DeptID + "and a.FloorID=" + floors.FloorID + " and a.BuildingID=" + building.BuildingID + " and ( ShopStatus = " + ConShop.CONSHOP_TYPE_PAUSE + " or ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + ") and a.ShopTypeID = b.ShopTypeID  order by Shopcode";
                            rs = baseBO.Query(new ConShop());
                            foreach (ConShop conShop in rs)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + conShop.ShopId.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" +conShop.ShopCode+"    "+conShop.ShopName + "^";
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }
    protected void gvCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            TextBox txtOtherChargeDID = (TextBox)e.Row.Cells[1].FindControl("txtOtherChargeDID");
            if (txtOtherChargeDID.Text.Trim() != "")
            {
                ((System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Checkbox")).Checked = true;
                ((System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Checkbox")).Enabled = false;
            }
            TextBox txtChgAmt = (TextBox)e.Row.Cells[1].FindControl("txtChgAmt");
            BaseInfo.BaseCommon.SetNumberTextBoxAttribute(new TextBox[] { txtChgAmt });
        }
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        this.btnAdd.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ChargeDetailDT.Rows.Clear();
        SaveChargeDetailDT.Rows.Clear();
        ViewState["flag"] = "0";
        ViewState["checkeds"] = "";
        BaseBO objBaseBo = new BaseBO();
        this.btnPutIn.Enabled = false;
        btnBlankOut.Enabled = false;
        if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
        {
            #region 选择商铺
            if (ViewState["ID"].ToString().Length == 12)//选择商铺
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strShopID = ViewState["ID"].ToString().Substring(9, 3);
                string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", strShopID);
                string strShopCode = BaseInfo.BaseCommon.GetTextValueByID("ShopCode", "ShopID", "ConShop", strShopID);
                DataRow dr = ChargeDetailDT.NewRow();
                dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue;
                dr["ChgName"] = this.ddlChargeType.SelectedItem;
                dr["StartDate"] = this.txtStartDate.Text.Trim();
                dr["EndDate"] = this.txtEndDate.Text.Trim();
                dr["ShopID"] = strShopID;
                dr["ShopName"] = strShopCode + "  " + strShopName;
                ChargeDetailDT.Rows.Add(dr);
                for (int i = 1; i < 15; i++)
                {
                    ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                }
                gvCharge.DataSource = ChargeDetailDT;
                gvCharge.DataBind();
            }
            #endregion
            #region 选择楼层
            if (ViewState["ID"].ToString().Length == 9)//选择楼层
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strSql = @"Select a.ShopId,a.AreaId,a.BuildingID,a.BrandID,a.UnitTypeID,a.ContractID,a.FloorID,a.LocationID,a.CreateUserID,a.CreateTime,a.ModifyUserID,a.ModifyTime,a.OprRoleID,a.OprDeptID,a.ShopCode,a.ShopName,a.RefID,a.RentArea,a.ShopStatus,a.ShopTypeID,a.ShopStartDate,a.ShopEndDate,a.ContactorName,a.Tel,a.StoreID,b.ShopTypeName From ConShop a,ShopType b WHERE a.FloorID='" + strFloorID + "'";
                strSql += " and a.BuildingID='" + strBuildingID + "' and ( a.ShopStatus = 0 or a.ShopStatus = 1) and a.ShopTypeID = b.ShopTypeID order by a.Shopcode";
                DataSet ds = objBaseBo.QueryDataSet(strSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["count"] = "0";
                    ChargeDetailDT.Rows.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                        string strShopCode = BaseInfo.BaseCommon.GetTextValueByID("ShopCode", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                        DataRow dr = ChargeDetailDT.NewRow();
                        dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue;
                        dr["ChgName"] = this.ddlChargeType.SelectedItem;
                        dr["StartDate"] = this.txtStartDate.Text.Trim();
                        dr["EndDate"] = this.txtEndDate.Text.Trim();
                        dr["ShopID"] = ds.Tables[0].Rows[i]["ShopId"].ToString();
                        dr["ShopName"] = strShopCode + "  " + strShopName;
                        ChargeDetailDT.Rows.Add(dr);
                        ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                    }
                    for (int i = (Convert.ToInt32(ViewState["count"]) % 15); i < 15; i++)
                    {
                        ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                    }
                }
                gvCharge.DataSource = ChargeDetailDT;
                gvCharge.DataBind();
            }
            #endregion
            #region 选择大楼
            if (ViewState["ID"].ToString().Length == 6)//选择大楼
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strSql = @"Select a.ShopId,a.AreaId,a.BuildingID,a.BrandID,a.UnitTypeID,a.ContractID,a.FloorID,a.LocationID,a.CreateUserID,a.CreateTime,a.ModifyUserID,a.ModifyTime,a.OprRoleID,a.OprDeptID,a.ShopCode,a.ShopName,a.RefID,a.RentArea,a.ShopStatus,a.ShopTypeID,a.ShopStartDate,a.ShopEndDate,a.ContactorName,a.Tel,a.StoreID,b.ShopTypeName From ConShop a,ShopType b WHERE ";
                strSql += " a.BuildingID='" + strBuildingID + "' and ( a.ShopStatus = 0 or a.ShopStatus = 1) and a.ShopTypeID = b.ShopTypeID order by a.Shopcode";
                DataSet ds = objBaseBo.QueryDataSet(strSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ChargeDetailDT.Rows.Clear();
                    ViewState["count"] = "0";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                        string strShopCode = BaseInfo.BaseCommon.GetTextValueByID("ShopCode", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                        DataRow dr = ChargeDetailDT.NewRow();
                        dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue;
                        dr["ChgName"] = this.ddlChargeType.SelectedItem;
                        dr["StartDate"] = this.txtStartDate.Text.Trim();
                        dr["EndDate"] = this.txtEndDate.Text.Trim();
                        dr["ShopID"] = ds.Tables[0].Rows[i]["ShopId"].ToString();
                        dr["ShopName"] = strShopCode + "  " + strShopName;
                        ChargeDetailDT.Rows.Add(dr);
                        ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                    }
                    for (int i = (Convert.ToInt32(ViewState["count"]) % 15); i < 15; i++)
                    {
                        ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                    }
                }
                gvCharge.DataSource = ChargeDetailDT;
                gvCharge.DataBind();
            }
            #endregion
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveChargeDetailDT.Rows.Clear();
        FindChecked();
        string strArr = "," + ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') + ",";
        if (ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            return;
        }
        string strDateISNull = "没有数据";
        string strDateISError = "数据错误";
        bool bDataISError = false;
        for (int i = 0; i < ChargeDetailDT.Rows.Count; i++)
        {
            if (ChargeDetailDT.Rows[i]["ShopID"].ToString() != "")
            {
                if (strArr.IndexOf("," + ChargeDetailDT.Rows[i]["ShopID"] + ",") >= 0)
                {
                    if (ChargeDetailDT.Rows[i]["ChgAmt"].ToString() == "" || ChargeDetailDT.Rows[i]["ChgAmt"].ToString() == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISNull + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                }
            }
        }
        BaseTrans baseTrans = new BaseTrans();
        OtherChargeD otherChgD = new OtherChargeD();
        BaseBO objBaseBo = new BaseBO();
        try
        {
            if (ViewState["flag"].ToString() == "0")//新增
            {
                #region 保存商铺
                if (ViewState["ID"].ToString().Length == 12)//保存商铺
                {
                    OtherChargeH charge = new OtherChargeH();
                    charge.OtherChargeHID = BaseApp.GetOtherChargeHID();
                    charge.ShopID = Int32.Parse(ViewState["ID"].ToString().Substring(9, 3));
                    string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", charge.ShopID.ToString());
                    charge.ChgStatus = Charge.CHGSTATUS_TYPE_TEMP;
                    charge.InvCode = "0";
                    charge.RangeCode = BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "OtherChargeH", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "OtherChargeH", "1")) + 1;
                    baseTrans.BeginTrans();
                    ViewState["chgID"] = charge.RangeCode;
                    if (strArr.IndexOf("," + ChargeDetailDT.Rows[0]["ShopID"] + ",") >= 0)//判断是否选择了数据
                    {
                        if (baseTrans.Insert(charge) != -1)
                        {
                            otherChgD.OtherChargeDID = BaseApp.GetOtherChargeDID();
                            otherChgD.OtherChargeHID = charge.OtherChargeHID;
                            otherChgD.ChargeTypeID = Convert.ToInt32(ChargeDetailDT.Rows[0]["ChargeTypeID"]);
                            otherChgD.ChgName = ChargeDetailDT.Rows[0]["ChgName"].ToString();
                            otherChgD.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[0]["StartDate"]);
                            otherChgD.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[0]["EndDate"]);
                            otherChgD.ChgAmt = Convert.ToDecimal(ChargeDetailDT.Rows[0]["ChgAmt"]);
                            otherChgD.RefID = ChargeDetailDT.Rows[0]["RefID"].ToString();
                            if (otherChgD.ChgAmt > 0)//费用金额不为零时保存
                            {
                                if (baseTrans.Insert(otherChgD) != -1)
                                {
                                    SaveChargeDetailDT.ImportRow(ChargeDetailDT.Rows[0]);
                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                                }
                            }
                            else
                            {
                                bDataISError = true;
                            }
                        }
                    }
                    if (bDataISError == true)
                    {
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                    SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                    int voucherID = charge.RangeCode;
                    String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim();
                    ViewState["voucherHints"] = voucherHints;
                    String voucherMemo = "";
                    VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                    WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);
                    baseTrans.Commit();
                    /*保存草稿提交的节点ID*/
                    HttpCookie cookies = new HttpCookie("Info");
                    cookies.Expires = System.DateTime.Now.AddDays(1);
                    cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                    cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                    cookies.Values.Add("conID", ViewState["chgID"].ToString());
                    cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                    Response.AppendCookie(cookies);
                    this.btnPutIn.Enabled = true;
                    btnBlankOut.Enabled = true;
                }
                #endregion
                #region 保存楼层
                if (ViewState["ID"].ToString().Length == 9)//选择楼层
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    string strFloorID = ViewState["ID"].ToString().Substring(6, 3);

                    string strSql = @"Select conshop.ShopId,conshop.AreaId,conshop.BuildingID,conshop.BrandID,conshop.UnitTypeID,conshop.ContractID,conshop.FloorID,conshop.LocationID,conshop.ShopCode,conshop.ShopName,conshop.RefID,conshop.RentArea,conshop.ShopStatus,ConShop.ShopTypeID,conshop.ShopStartDate,conshop.ShopEndDate,conshop.ContactorName,conshop.Tel,conshop.StoreID,ShopType.ShopTypeName From ConShop left join ShopType  on ConShop.ShopTypeID = ShopType.ShopTypeID left join ShopHdw  on ShopHdw.shopid=ConShop.shopid ";
                    strSql += " WHERE conshop.FloorID='" + strFloorID + "' and conshop.BuildingID='" + strBuildingID + "' and ( conshop.ShopStatus = 0 or  conshop.ShopStatus = 1)  group by conshop.ShopId,conshop.AreaId,conshop.BuildingID,conshop.BrandID,conshop.UnitTypeID,conshop.ContractID,conshop.FloorID,conshop.LocationID,conshop.ShopCode,conshop.ShopName,conshop.RefID,conshop.RentArea,conshop.ShopStatus,ConShop.ShopTypeID,conshop.ShopStartDate,conshop.ShopEndDate,conshop.ContactorName,conshop.Tel,conshop.StoreID,ShopType.ShopTypeName order by Shopcode";

                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        int RangeCode = BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "OtherChargeH", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "OtherChargeH", "1")) + 1;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            OtherChargeH charge = new OtherChargeH();
                            charge.OtherChargeHID = BaseApp.GetOtherChargeHID();
                            charge.ShopID = Int32.Parse(ds.Tables[0].Rows[i]["ShopID"].ToString());
                            string strShopName = ds.Tables[0].Rows[i]["ShopName"].ToString();
                            charge.ChgStatus = Charge.CHGSTATUS_TYPE_TEMP;
                            charge.InvCode = "0";
                            charge.RangeCode = RangeCode;//批量号
                            baseTrans.BeginTrans();
                            ViewState["chgID"] = charge.RangeCode;
                            if (strArr.IndexOf("," + charge.ShopID.ToString() + ",") >= 0)//判断是否选择
                            {
                                if (baseTrans.Insert(charge) != -1)
                                {
                                    for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                                    {
                                        //if (strArr.IndexOf("," + ChargeDetailDT.Rows[l]["ShopID"].ToString() + ",") >= 0)//判断是否选择
                                        //{
                                        if (ChargeDetailDT.Rows[l]["ShopID"].ToString() == ds.Tables[0].Rows[i]["ShopId"].ToString())
                                        {
                                            otherChgD.OtherChargeDID = BaseApp.GetOtherChargeDID();
                                            otherChgD.OtherChargeHID = charge.OtherChargeHID;
                                            otherChgD.ChargeTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChargeTypeID"]);
                                            otherChgD.ChgName = ChargeDetailDT.Rows[l]["ChgName"].ToString();
                                            otherChgD.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["StartDate"]);
                                            otherChgD.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["EndDate"]);
                                            otherChgD.ChgAmt = Convert.ToDecimal(ChargeDetailDT.Rows[l]["ChgAmt"]);
                                            otherChgD.RefID = ChargeDetailDT.Rows[l]["RefID"].ToString();
                                            if (otherChgD.ChgAmt > 0)
                                            {
                                                if (baseTrans.Insert(otherChgD) != -1)
                                                {
                                                    SaveChargeDetailDT.ImportRow(ChargeDetailDT.Rows[0]);
                                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                                                }
                                            }
                                            else
                                            {
                                                bDataISError = true;
                                            }
                                        }
                                    }
                                }
                                baseTrans.Commit();
                            }
                        }
                        if (bDataISError == true)
                        {
                            baseTrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        ///*保存草稿提交的节点ID*/
                        BaseTrans objBaseTrans = new BaseTrans();
                        objBaseTrans.BeginTrans();
                        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                        int voucherID = RangeCode;
                        String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim();
                        ViewState["voucherHints"] = voucherHints;
                        String voucherMemo = "";
                        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                        WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, objBaseTrans);
                        objBaseTrans.Commit();
                        HttpCookie cookies = new HttpCookie("Info");
                        cookies.Expires = System.DateTime.Now.AddDays(1);
                        cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                        cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                        cookies.Values.Add("conID", RangeCode.ToString());
                        cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                        Response.AppendCookie(cookies);
                        this.btnPutIn.Enabled = true;//提交
                        btnBlankOut.Enabled = true;
                    }
                }
                #endregion
                #region 保存大楼
                if (ViewState["ID"].ToString().Length == 6)//选择大楼
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);

                    string strSql = @"Select conshop.ShopId,conshop.AreaId,conshop.BuildingID,conshop.BrandID,conshop.UnitTypeID,conshop.ContractID,conshop.FloorID,conshop.LocationID,conshop.ShopCode,conshop.ShopName,conshop.RefID,conshop.RentArea,conshop.ShopStatus,ConShop.ShopTypeID,conshop.ShopStartDate,conshop.ShopEndDate,conshop.ContactorName,conshop.Tel,conshop.StoreID,ShopType.ShopTypeName From ConShop left join ShopType  on ConShop.ShopTypeID = ShopType.ShopTypeID left join ShopHdw  on ShopHdw.shopid=ConShop.shopid ";
                    strSql += " WHERE conshop.BuildingID='" + strBuildingID + "' and ( conshop.ShopStatus = 0 or  conshop.ShopStatus = 1) group by conshop.ShopId,conshop.AreaId,conshop.BuildingID,conshop.BrandID,conshop.UnitTypeID,conshop.ContractID,conshop.FloorID,conshop.LocationID,conshop.ShopCode,conshop.ShopName,conshop.RefID,conshop.RentArea,conshop.ShopStatus,ConShop.ShopTypeID,conshop.ShopStartDate,conshop.ShopEndDate,conshop.ContactorName,conshop.Tel,conshop.StoreID,ShopType.ShopTypeName order by Shopcode";

                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        int RangeCode = BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "OtherChargeH", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "OtherChargeH", "1")) + 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            OtherChargeH charge = new OtherChargeH();
                            charge.OtherChargeHID = BaseApp.GetOtherChargeHID();
                            charge.ShopID = Int32.Parse(ds.Tables[0].Rows[i]["ShopID"].ToString());
                            string strShopName = ds.Tables[0].Rows[i]["ShopName"].ToString();
                            charge.ChgStatus = Charge.CHGSTATUS_TYPE_TEMP;
                            charge.InvCode = "0";
                            charge.RangeCode = RangeCode;//批量号
                            baseTrans.BeginTrans();
                            ViewState["chgID"] = charge.RangeCode;
                            //if (strArr.IndexOf("," + charge.ShopID.ToString() + ",") >= 0)//判断是否选择
                            //{
                            if (baseTrans.Insert(charge) != -1)
                            {
                                for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                                {
                                    if (strArr.IndexOf("," + ChargeDetailDT.Rows[l]["ShopID"].ToString() + ",") >= 0)//判断是否选择
                                    {
                                        if (ChargeDetailDT.Rows[l]["ShopID"].ToString() == ds.Tables[0].Rows[i]["ShopId"].ToString())
                                        {
                                            otherChgD.OtherChargeDID = BaseApp.GetOtherChargeDID();
                                            otherChgD.OtherChargeHID = charge.OtherChargeHID;
                                            otherChgD.ChargeTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChargeTypeID"]);
                                            otherChgD.ChgName = ChargeDetailDT.Rows[l]["ChgName"].ToString();
                                            otherChgD.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["StartDate"]);
                                            otherChgD.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["EndDate"]);
                                            otherChgD.ChgAmt = Convert.ToDecimal(ChargeDetailDT.Rows[l]["ChgAmt"]);
                                            otherChgD.RefID = ChargeDetailDT.Rows[l]["RefID"].ToString();
                                            if (otherChgD.ChgAmt > 0)
                                            {
                                                if (baseTrans.Insert(otherChgD) != -1)
                                                {
                                                    SaveChargeDetailDT.ImportRow(ChargeDetailDT.Rows[0]);
                                                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                                                }
                                            }
                                            else
                                            {
                                                bDataISError = true;
                                            }
                                        }
                                    }
                                }
                            }
                            baseTrans.Commit();
                            //}
                        }
                        if (bDataISError == true)
                        {
                            baseTrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        /*保存草稿提交的节点ID*/
                        BaseTrans objBaseTrans = new BaseTrans();
                        objBaseTrans.BeginTrans();
                        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                        int voucherID = RangeCode;

                        String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim();
                        ViewState["voucherHints"] = voucherHints;
                        String voucherMemo = "";
                        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                        WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, objBaseTrans);
                        objBaseTrans.Commit();
                        HttpCookie cookies = new HttpCookie("Info");
                        cookies.Expires = System.DateTime.Now.AddDays(1);
                        cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                        cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                        cookies.Values.Add("conID", RangeCode.ToString());
                        cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                        Response.AppendCookie(cookies);
                        this.btnPutIn.Enabled = true;//提交
                        btnBlankOut.Enabled = true;
                    }
                }
                #endregion
            }
            else//更新
            {
                for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                {
                    string txtShopID = ChargeDetailDT.Rows[l]["ShopID"].ToString();
                    if (strArr.IndexOf("," + txtShopID + ",") >= 0)//判断是否选择了数据
                    {
                        otherChgD.OtherChargeHID = Convert.ToInt32(ChargeDetailDT.Rows[l]["OtherChargeHID"]);
                        otherChgD.ChargeTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChargeTypeID"]);
                        otherChgD.ChgName = ChargeDetailDT.Rows[l]["ChgName"].ToString();
                        otherChgD.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["StartDate"]);
                        otherChgD.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["EndDate"]);
                        otherChgD.ChgAmt = Convert.ToDecimal(ChargeDetailDT.Rows[l]["ChgAmt"]);
                        otherChgD.RefID = ChargeDetailDT.Rows[l]["RefID"].ToString();
                        if (otherChgD.ChgAmt > 0)
                        {
                            string txtOtherChargeDID = ChargeDetailDT.Rows[l]["OtherChargeDID"].ToString();
                            BaseTrans objBaseTrans = new BaseTrans();
                            objBaseTrans.BeginTrans();
                            objBaseTrans.WhereClause = "OtherChargeDID=" + txtOtherChargeDID;
                            if (objBaseTrans.Update(otherChgD) != -1)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                            }
                            objBaseTrans.Commit();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            Response.Write(ex);
        }

        btnAdd.Enabled = false;
        this.btnPutIn.Enabled = true;
        this.btnBlankOut.Enabled = true;
        this.page(Int32.Parse(ViewState["chgID"].ToString()));

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 记录每页选中的情况
    /// </summary>
    /// <param name="strHaveSelects"></param>
    public void SetDataGridSelectRecords(string strHaveSelects)
    {
        strHaveSelects = "," + strHaveSelects.TrimEnd(',').TrimStart(',') + ",";
        for (int i = 0; i < this.gvCharge.Rows.Count; i++)
        {
            TextBox txtShopID = (TextBox)this.gvCharge.Rows[i].FindControl("txtShopID");
            string strtemp = txtShopID.Text.Trim();
            if (strtemp != "")
            {
                if (strHaveSelects.IndexOf("," + strtemp + ",") >= 0)
                {
                    ((System.Web.UI.WebControls.CheckBox)this.gvCharge.Rows[i].Cells[0].FindControl("Checkbox")).Checked = true;
                }
            }
        }
    }
    /// <summary>
    /// 记录表中选中记录的情况
    /// </summary>
    /// <returns></returns>
    private void FindChecked()
    {
        string checkeds = "";
        string strShopChecks = "";
        if (ViewState["checkeds"] != null)
            checkeds = "," + ViewState["checkeds"].ToString() + ",";
        for (int i = 0; i < this.gvCharge.Rows.Count; i++)
        {
            TextBox txtRefID = (TextBox)this.gvCharge.Rows[i].FindControl("txtRefID");
            TextBox txtChgAmt = (TextBox)this.gvCharge.Rows[i].FindControl("txtChgAmt");
            TextBox txtStartDate = (TextBox)this.gvCharge.Rows[i].FindControl("txtStartDate");
            TextBox txtEndDate = (TextBox)this.gvCharge.Rows[i].FindControl("txtEndDate");
            TextBox txtShopID = (TextBox)this.gvCharge.Rows[i].FindControl("txtShopID");

            string strShopID = txtShopID.Text.Trim();
            if (((System.Web.UI.WebControls.CheckBox)this.gvCharge.Rows[i].Cells[0].FindControl("Checkbox")).Checked)
            {
                if (checkeds.IndexOf("," + strShopID + ",") < 0)
                {
                    checkeds += strShopID + ",";
                    
                    for (int j = 0; j < ChargeDetailDT.Rows.Count; j++)
                    {
                        if (ChargeDetailDT.Rows[j]["ShopID"].ToString() == strShopID)
                        {
                            ChargeDetailDT.Rows[j]["RefID"] = txtRefID.Text.Trim();
                            if (txtChgAmt.Text.Trim()!="")
                                ChargeDetailDT.Rows[j]["ChgAmt"] = txtChgAmt.Text.Trim();
                            ChargeDetailDT.Rows[j]["StartDate"] = txtStartDate.Text.Trim();
                            ChargeDetailDT.Rows[j]["EndDate"] = txtEndDate.Text.Trim();
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < ChargeDetailDT.Rows.Count; j++)
                    {
                        if (ChargeDetailDT.Rows[j]["ShopID"].ToString() == strShopID)
                        {
                            ChargeDetailDT.Rows[j]["RefID"] = txtRefID.Text.Trim();
                            if (txtChgAmt.Text.Trim()!="")
                                ChargeDetailDT.Rows[j]["ChgAmt"] = txtChgAmt.Text.Trim();
                            ChargeDetailDT.Rows[j]["StartDate"] = txtStartDate.Text.Trim();
                            ChargeDetailDT.Rows[j]["EndDate"] = txtEndDate.Text.Trim();
                        }
                    }
                }
            }
            else
            {
                //如果没选中则在串中去掉
                checkeds = checkeds.Replace("," + strShopID + ",", ",");
            }
        }
        checkeds = checkeds.TrimEnd(',').TrimStart(',');
        ViewState["checkeds"] = checkeds;
    }
    protected void gvCharge_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        gvCharge.DataSource = ChargeDetailDT;
        gvCharge.DataBind();
        SetDataGridSelectRecords(ViewState["checkeds"].ToString());//设置选择项
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 提交审批
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPutIn_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(ViewState["chgID"]);
        int sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
        String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
        if (voucherHints == "") voucherHints = ViewState["voucherHints"].ToString();
        String voucherMemo = "";

        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
        int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
        try
        {
            if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
            }
            else if (Convert.ToInt32(Request.QueryString["Sequence"].ToString()) != 0)
            {
                WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]), vInfo);
            }
            else
            {
                WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo);
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        page(0);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_ConfirmOK") + "'", true);
        this.btnPutIn.Enabled = false;
        this.btnSave.Enabled = false;
        btnBlankOut.Enabled = false;
    }

    protected void btnBlankOut_Click(object sender, EventArgs e)
    {
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseTrans baseTrans = new BaseTrans();
        baseTrans.BeginTrans();
        try
        {
            int deptID = sessionUser.DeptID;
            int userID = sessionUser.UserID;

            int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
            int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
            int sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
            int voucherID = Convert.ToInt32(Request.Cookies["Info"].Values["conID"]);
            String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
            String voucherMemo = "";

            if (baseTrans.ExecuteUpdate("UPDATE OtherChargeH SET chgstatus = 4 WHERE OtherChargeHID=" + voucherID) == -1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                baseTrans.Rollback();
                return;
            }

            VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, deptID, userID);
            WrkFlwApp.BlankOutVoucherNode(wrkFlwID, nodeID, sequence, vInfo, baseTrans);

            baseTrans.Commit();

            //删除cookies
            HttpCookie cookies = new HttpCookie("Info");
            cookies.Expires = System.DateTime.Now.AddDays(1);
            cookies.Values.Add("conID", "");
            Response.AppendCookie(cookies);
            this.btnPutIn.Enabled = false;
            this.btnBlankOut.Enabled = false;
            page(0);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            throw ex;
        }
    }
}
