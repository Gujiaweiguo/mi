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
using Lease.PotBargain;
using Invoice;
using WorkFlow.WrkFlw;
using WorkFlow;
using Base;
using BaseInfo.User;
using WorkFlow.Uiltil;
using System.Drawing;
using BaseInfo.authUser;
public partial class Lease_ChargeAccount_WaterElectCharge : BasePage
{
    public string baseinfo;
    //private static Hashtable ht;
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
        if (!this.IsPostBack)
        {
            Hashtable ht = new Hashtable();
            baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Account_lblWaterElectCharge");
            this.BindShopHw();//绑定硬件类型
            this.BindChargeType();//绑定费用类型
            this.ShowTree(this.ddlHdwCode.SelectedValue);//显示左侧列表树
            this.txtStartDate.Text = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "-01";//开始日期
            this.txtEndDate.Text = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "-" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString();//结束日期
            this.txtPrice.Text = "1.0";
            IniChargeDetailDT();

            ViewState["count"] = 0;
            ViewState["flag"] = "0";
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

                if (Request.QueryString["WrkFlwID"] != null)
                {
                    HidenWrkID.Value = Request.QueryString["WrkFlwID"].ToString();
                }
                else
                {
                    HidenWrkID.Value = Request.Cookies["Info"].Values["wrkFlwID"].ToString();
                }
                if (Request.QueryString["VoucherID"] != null)
                {
                    HidenVouchID.Value = Request.QueryString["VoucherID"].ToString();
                }
                else
                {
                    HidenVouchID.Value = Request.Cookies["Info"].Values["conID"].ToString();
                }
                ViewState["flag"] = "2";

                WrkFlwEntity objWrkFlwEntity = WrkFlwApp.GetWrkFlwEntity(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), Convert.ToInt32(Request.QueryString["Sequence"].ToString()));
                //string ss = objWrkFlwEntity.VoucherMemo;
                //this.listBoxRemark.Text = ss;
                QueryChargeDetail(chgID);
                btnAdd.Enabled = false;
            }
            else
            {
                btnBlankOut.Enabled = false;
                btnMessage.Enabled = false;
                this.btnPutIn.Enabled = false;//提交
                page(0);
            }
            btnMessage.Attributes.Add("onclick", "ShowMessage()");
        }
    }
    /// <summary>
    /// 绑定水电费信息
    /// </summary>
    /// <param name="chgid"></param>
    private void QueryChargeDetail(int chgid)
    {
        BindShopHw();
        page(chgid);
    }
    /// <summary>
    /// 页面绑定
    /// </summary>
    /// <param name="chgId"></param>
    protected void page(int chgId)
    {
        BaseBO baseBO = new BaseBO();
        DataTable dt = new DataTable();
        PagedDataSource pds = new PagedDataSource();
        //baseBO.WhereClause = "ChgID = " + chgId;
        //ChargeDetail objChargeDetail = new ChargeDetail();convert(char(10),startdate,120)
        baseBO.WhereClause = "rangecode = " + chgId;
        ChargeDetail objChargeDetail = new ChargeDetail();
        objChargeDetail.SetQuerySql("select ChargeDetail.ChgDetID,ChargeDetail.HdwID,ChargeDetail.ChgID,ChargeDetail.ChargeTypeID,ChargeDetail.HdwID,ChargeDetail.ChgName,Convert(char(10),StartDate,120) as StartDate,Convert(char(10),EndDate,120) as EndDate,ChargeDetail.RefID,ChargeDetail.ChgAmt,ChargeDetail.LastQty,ChargeDetail.CurQty,ChargeDetail.CostQty,ChargeDetail.Times,ChargeDetail.FreeQty,ChargeDetail.Price,'' as HdwCode,'' as ChargeTypeName,ChargeDetail.ChgPeriod,charge.ShopID,(select shopname from conshop where conshop.shopid=charge.shopid) as ShopName,'' as ShopHdwHdwID,ChargeDetail.ErrorSign from  ChargeDetail left join charge on charge.ChgID=ChargeDetail.ChgID");
       
        DataSet ds = baseBO.QueryDataSet(objChargeDetail);
        dt = ds.Tables[0];
        if (chgId != 0) ChargeDetailDT = dt;
        int count = dt.Rows.Count;

        if (count > 0)
        {
            ViewState["flag"] = "2";
            for (int i = 0; i < count; i++)
            {
                baseBO.WhereClause = "";
                baseBO.WhereClause = "ChargeTypeID = " + Convert.ToInt32(dt.Rows[i]["ChargeTypeID"]);
                DataSet tempDs = baseBO.QueryDataSet(new ChargeType());
                dt.Rows[i]["ChargeTypeName"] = tempDs.Tables[0].Rows[0]["ChargeTypeName"].ToString();

                baseBO.WhereClause = "";
                baseBO.WhereClause = "HdwID = " + Convert.ToInt32(dt.Rows[i]["HdwID"]);
                DataSet tempHDs = baseBO.QueryDataSet(new ShopHdw());
                dt.Rows[i]["HdwCode"] = tempHDs.Tables[0].Rows[0]["HdwName"].ToString();

                dt.Rows[i]["ShopHdwHdwID"] = dt.Rows[i]["HdwID"].ToString();
            }
            this.ddlChargeType.SelectedValue = dt.Rows[0]["ChargeTypeID"].ToString();
        }
        for (int i = count; i < 15; i++)
        {
            dt.Rows.Add(dt.NewRow());
        }
        gvCharge.DataSource = dt;
        gvCharge.DataBind();
        ViewState["count"] = count;
    }
    /// <summary>
    /// 添加表中的列
    /// </summary>
    protected void IniChargeDetailDT()
    {
        ChargeDetailDT = new DataTable();
        ChargeDetailDT.Columns.Add("ChgDetID");
        ChargeDetailDT.Columns.Add("ChargeTypeID");
        ChargeDetailDT.Columns.Add("ChargeTypeName");
        ChargeDetailDT.Columns.Add("StartDate");
        ChargeDetailDT.Columns.Add("EndDate");
        ChargeDetailDT.Columns.Add("HdwID");
        ChargeDetailDT.Columns.Add("HdwCode");
        ChargeDetailDT.Columns.Add("LastQty");
        ChargeDetailDT.Columns.Add("CurQty");
        ChargeDetailDT.Columns.Add("CostQty");
        ChargeDetailDT.Columns.Add("FreeQty");
        ChargeDetailDT.Columns.Add("Times");
        ChargeDetailDT.Columns.Add("Price");
        ChargeDetailDT.Columns.Add("ChgAmt");
        ChargeDetailDT.Columns.Add("ShopID");
        ChargeDetailDT.Columns.Add("ShopName");
        ChargeDetailDT.Columns.Add("ShopHdwHdwID");
        ChargeDetailDT.Columns.Add("ErrorSign");
        SaveChargeDetailDT = ChargeDetailDT.Clone();
    }
    /// <summary>
    /// 绑定硬件代码
    /// </summary>
    private void BindShopHw()
    {
        this.ddlHdwCode.Items.Clear();
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "HdwTypeStatus=1";
        Resultset rs = objBaseBo.Query(new HdwType());
        foreach (HdwType objHdwType in rs)
        {
            this.ddlHdwCode.Items.Add(new ListItem(objHdwType.HdwTypeName,objHdwType.HdwTypeID.ToString()));
        }
    }
    /// <summary>
    /// 绑定费用类别
    /// </summary>
    private void BindChargeType()
    {
        this.ddlChargeType.Items.Clear();
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "ChargeClass = " + ChargeType.CHARGECLASS_WATERORDLECT;
        Resultset chageRs = baseBo.Query(new ChargeType());
        foreach (ChargeType chargeType in chageRs)
        {
            this.ddlChargeType.Items.Add(new ListItem(chargeType.ChargeTypeName, chargeType.ChargeTypeID.ToString()));
        }
    }
    private void ShowTree(string strHdwTypeID)
    {
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
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
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
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            baseBO.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
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

                        baseBO.WhereClause = "floors.BuildingID=" + building.BuildingID;

                        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
                        {
                            baseBO.WhereClause += " AND EXISTS ( " + AuthBase.AUTH_SQL_SHOP + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_BUILD + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_FLOOR + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_CONTRACT + sessionUser.UserID +
                                                 ") AND EXISTS ( " + AuthBase.AUTH_SQL_STORE + sessionUser.UserID + ")";
                        }
                        rsf = baseBO.Query(new floorsAuth());
                        foreach (floorsAuth floors in rsf)
                        {
                            jsdept +=store.DeptID.ToString()+ building.BuildingID.ToString() + floors.FloorID.ToString() + "|" +store.DeptID.ToString()+ building.BuildingID + "|" + floors.FloorName + "^";
                            baseBO.WhereClause = "a.StoreId=" + store.DeptID + "and a.FloorID=" + floors.FloorID + " and a.BuildingID=" + building.BuildingID + " and ( ShopStatus = " + ConShop.CONSHOP_TYPE_PAUSE + " or ShopStatus = " + ConShop.CONSHOP_TYPE_INGEAR + ") and a.ShopTypeID = b.ShopTypeID and a.shopid in (select shopid from shophdw where hdwtypeid = '" + strHdwTypeID + "')  order by Shopcode";
                            rsl = baseBO.Query(new ConShop());
                            foreach (ConShop conShop in rsl)
                            {
                                jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + conShop.ShopId.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + "|" + conShop.ShopName + "^";
                                baseBO.WhereClause = "ShopHdw.ShopID='" + conShop.ShopId.ToString() + "' and hdwtypeid='" + strHdwTypeID + "'";
                                Resultset rsShopHdw = baseBO.Query(new ShopHdw());
                                foreach (ShopHdw objShopHdw in rsShopHdw)
                                {
                                    jsdept += store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + conShop.ShopId.ToString() + objShopHdw.HdwID.ToString() + "|" + store.DeptID.ToString() + building.BuildingID.ToString() + floors.FloorID.ToString() + conShop.ShopId.ToString() + "|" + objShopHdw.HdwName.ToString() + "^";
                                }
                            }
                        }
                    }
                }
            }
        }
        depttxt.Value = jsdept;
    }

    protected void treeClick_Click(object sender, EventArgs e)
    {
        ViewState["ID"] = deptid.Value;
        this.btnAdd.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 保存事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveChargeDetailDT.Rows.Clear();
        FindChecked();
        string strArr = ","+ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',')+",";
        string strShopIDS = ","+ViewState["ShoCheckeds"].ToString().TrimStart(',').TrimEnd(',')+",";
        if (ViewState["checkeds"].ToString().TrimStart(',').TrimEnd(',') == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            return;
        }
        if ((this.ddlHdwCode.Items.Count <= 0))
        {
            this.page(0);
            return;
        }
        string strDateISNull = "没有数据";
        string strDateISError = "数据错误";
        bool bDataISError = false;
        for(int i=0;i<ChargeDetailDT.Rows.Count;i++)
        {
            if (ChargeDetailDT.Rows[i]["ShopHdwHdwID"].ToString() != "")
            {
                if (strArr.IndexOf("," + ChargeDetailDT.Rows[i]["ShopHdwHdwID"] + ",") >= 0)
                {
                    if (ChargeDetailDT.Rows[i]["CurQty"].ToString() == "" || ChargeDetailDT.Rows[i]["CurQty"].ToString() == "0")
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" +strDateISNull+ "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                }
            }
        }
        BaseTrans baseTrans = new BaseTrans();
        ChargeDetail chgDetl = new ChargeDetail();
        BaseBO objBaseBo = new BaseBO();
        baseTrans.BeginTrans();//new
        try
        {
            if (ViewState["flag"].ToString() == "0")//新增
            {
                if (ViewState["ID"].ToString().Length == 15)
                {
                    Charge charge = new Charge();
                    charge.ChgID = BaseApp.GetChgID();
                    charge.ShopID = Int32.Parse(ViewState["ID"].ToString().Substring(9, 3));
                    string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", charge.ShopID.ToString());
                    charge.ChgStatus = Charge.CHGSTATUS_TYPE_TEMP;
                    charge.InvCode = "0";
                    charge.RangeCode = BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "Charge", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "Charge", "1")) + 1;
                    //baseTrans.BeginTrans();
                    ViewState["chgID"] = charge.RangeCode;
                    if (strShopIDS.IndexOf("," + charge.ShopID + ",") >= 0)//
                    {
                        if (baseTrans.Insert(charge) != -1)
                        {
                            if (strArr.IndexOf("," + ChargeDetailDT.Rows[0]["ShopHdwHdwID"] + ",") >= 0)//判断是否选择了数据
                            {
                                chgDetl.ChgID = charge.ChgID;
                                try { chgDetl.HdwID = Convert.ToInt32(ChargeDetailDT.Rows[0]["HdwID"]); }
                                catch { chgDetl.HdwID = 0; }
                                chgDetl.ChargeTypeID = Convert.ToInt32(ChargeDetailDT.Rows[0]["ChargeTypeID"]);
                                chgDetl.ChgName = ChargeDetailDT.Rows[0]["ChargeTypeName"].ToString();
                                chgDetl.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[0]["StartDate"]);
                                chgDetl.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[0]["EndDate"]);
                                TextBox txtLastQty = (TextBox)this.gvCharge.Rows[0].FindControl("txtLastQty");
                                if (txtLastQty.Text.Trim() != "")
                                    chgDetl.LastQty = Convert.ToInt32(txtLastQty.Text.Trim());
                                else
                                    chgDetl.LastQty = 0;
                                TextBox txtCurQty = (TextBox)this.gvCharge.Rows[0].FindControl("txtCurQty");
                                if (txtCurQty.Text.Trim() != "")
                                    chgDetl.CurQty = Convert.ToInt32(txtCurQty.Text.Trim());
                                else
                                    chgDetl.CurQty = 0;

                                if (ChargeDetailDT.Rows[0]["Times"].ToString().Trim() != "")
                                    chgDetl.Times = Convert.ToInt32(ChargeDetailDT.Rows[0]["Times"]);
                                else
                                    chgDetl.Times = 1;
                                TextBox txtFreeQty = (TextBox)this.gvCharge.Rows[0].FindControl("txtFreeQty");
                                if (txtFreeQty.Text.Trim() != "")
                                    chgDetl.FreeQty = Convert.ToInt32(txtFreeQty.Text.Trim());
                                else
                                    chgDetl.FreeQty = 0;
                                TextBox txtPrice = (TextBox)this.gvCharge.Rows[0].FindControl("txtPrice");
                                if (txtPrice.Text.Trim() != "")
                                    chgDetl.Price = Convert.ToDecimal(txtPrice.Text.Trim());
                                else
                                    chgDetl.Price = 0;
                                chgDetl.CostQty = chgDetl.CurQty - chgDetl.LastQty;//消耗量
                                try { chgDetl.ChgAmt = (chgDetl.CurQty - chgDetl.FreeQty - chgDetl.LastQty) * chgDetl.Times * chgDetl.Price; }
                                catch { chgDetl.ChgAmt = 0; }
                                if (ChargeDetailDT.Rows[0]["ChgDetID"].ToString().Trim() != "")
                                    chgDetl.ChgDetID = Convert.ToInt32(ChargeDetailDT.Rows[0]["ChgDetID"]);
                                else
                                    chgDetl.ChgDetID = BaseApp.GetChgDetID();
                                if (chgDetl.ChgAmt >= 0)//费用金额大于等于零时
                                {
                                    if (baseTrans.Insert(chgDetl) != -1)
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
                        //int voucherID = charge.ChgID;
                        int voucherID = charge.RangeCode;
                        String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim();
                        ViewState["voucherHints"] = voucherHints;
                        String voucherMemo = "";

                        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

                        WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);
                        //baseTrans.Commit();
                        /*保存草稿提交的节点ID*/
                        HttpCookie cookies = new HttpCookie("Info");

                        cookies.Expires = System.DateTime.Now.AddDays(1);
                        cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                        cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                        cookies.Values.Add("conID", ViewState["chgID"].ToString());
                        cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                        Response.AppendCookie(cookies);
                        //btnBlankOut.Enabled = true;
                        this.btnPutIn.Enabled = true;//提交
                        //this.btnMessage.Enabled = true;
                    }
                }
                if (ViewState["ID"].ToString().Length == 12)//保存商铺
                {
                    Charge charge = new Charge();
                    charge.ChgID = BaseApp.GetChgID();
                    charge.ShopID = Int32.Parse(ViewState["ID"].ToString().Substring(9, 3));
                    string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", charge.ShopID.ToString());
                    charge.ChgStatus = Charge.CHGSTATUS_TYPE_TEMP;
                    charge.InvCode = "0";
                    charge.RangeCode = BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "Charge", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "Charge", "1")) + 1;
                    baseTrans.BeginTrans();
                    ViewState["chgID"] = charge.RangeCode;
                    if (baseTrans.Insert(charge) != -1)
                    {
                        if (ViewState["count"] != null && ViewState["count"].ToString() != "0")
                        {
                            int ss = Convert.ToInt32(ViewState["count"]);
                            for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                            {
                                if (strArr.IndexOf("," + ChargeDetailDT.Rows[l]["ShopHdwHdwID"] + ",") >= 0)//判断是否选择了数据
                                {
                                    chgDetl.ChgID = charge.ChgID;
                                    try { chgDetl.HdwID = Convert.ToInt32(ChargeDetailDT.Rows[l]["HdwID"]); }
                                    catch { chgDetl.HdwID = 0; }
                                    chgDetl.ChargeTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChargeTypeID"]);
                                    chgDetl.ChgName = ChargeDetailDT.Rows[l]["ChargeTypeName"].ToString();
                                    chgDetl.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["StartDate"]);
                                    chgDetl.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["EndDate"]);
                                    TextBox txtLastQty = (TextBox)this.gvCharge.Rows[l].FindControl("txtLastQty");
                                    if (txtLastQty.Text.Trim() != "")
                                        chgDetl.LastQty = Convert.ToInt32(txtLastQty.Text.Trim());
                                    else
                                        chgDetl.LastQty = 0;
                                    TextBox txtCurQty = (TextBox)this.gvCharge.Rows[l].FindControl("txtCurQty");
                                    if (txtCurQty.Text.Trim() != "")
                                        chgDetl.CurQty = Convert.ToInt32(txtCurQty.Text.Trim());
                                    else
                                        chgDetl.CurQty = 0;

                                    if (ChargeDetailDT.Rows[l]["Times"].ToString().Trim() != "")
                                        chgDetl.Times = Convert.ToInt32(ChargeDetailDT.Rows[l]["Times"]);
                                    else
                                        chgDetl.Times = 1;
                                    TextBox txtFreeQty = (TextBox)this.gvCharge.Rows[l].FindControl("txtFreeQty");
                                    if (txtFreeQty.Text.Trim() != "")
                                        chgDetl.FreeQty = Convert.ToInt32(txtFreeQty.Text.Trim());
                                    else
                                        chgDetl.FreeQty = 0;
                                    TextBox txtPrice = (TextBox)this.gvCharge.Rows[l].FindControl("txtPrice");
                                    if (txtPrice.Text.Trim() != "")
                                        chgDetl.Price = Convert.ToDecimal(txtPrice.Text.Trim());
                                    else
                                        chgDetl.Price = 0;
                                    chgDetl.CostQty = chgDetl.CurQty - chgDetl.LastQty;//消耗量
                                    try { chgDetl.ChgAmt = (chgDetl.CurQty - chgDetl.FreeQty - chgDetl.LastQty) * chgDetl.Times * chgDetl.Price; }
                                    catch { chgDetl.ChgAmt = 0; }
                                    if (ChargeDetailDT.Rows[l]["ChgDetID"].ToString().Trim() != "")
                                        chgDetl.ChgDetID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChgDetID"]);
                                    else
                                        chgDetl.ChgDetID = BaseApp.GetChgDetID();
                                    if ((chgDetl.ChgAmt>=0))//费用金额不为零
                                    {
                                        if (baseTrans.Insert(chgDetl) != -1)
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
                    if (bDataISError == true)
                    {
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                        return;
                    }
                    SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                    //int voucherID = charge.ChgID;
                    int voucherID = charge.RangeCode;
                    String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim();
                    ViewState["voucherHints"] = voucherHints;
                    String voucherMemo = "";

                    VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);

                    WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);
                    //baseTrans.Commit();
                    /*保存草稿提交的节点ID*/
                    HttpCookie cookies = new HttpCookie("Info");

                    cookies.Expires = System.DateTime.Now.AddDays(1);
                    cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                    cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                    cookies.Values.Add("conID", ViewState["chgID"].ToString());
                    cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                    Response.AppendCookie(cookies);
                    //btnBlankOut.Enabled = true;
                    this.btnPutIn.Enabled = true;//提交
                    //this.btnMessage.Enabled = true;
                }
                if (ViewState["ID"].ToString().Length == 9)//选择楼层
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                    //string strSql = @"Select ShopId,AreaId,BuildingID,BrandID,UnitTypeID,ContractID,FloorID,LocationID,a.CreateUserID,a.CreateTime,a.ModifyUserID,a.ModifyTime,a.OprRoleID,a.OprDeptID,ShopCode,ShopName,RefID,RentArea,ShopStatus,a.ShopTypeID,ShopStartDate,ShopEndDate,ContactorName,Tel,StoreID,ShopTypeName From ConShop a,ShopType b WHERE FloorID='" + strFloorID + "'";
                    //strSql += " and BuildingID='" + strBuildingID + "' and ( ShopStatus = 0 or ShopStatus = 1) and a.ShopTypeID = b.ShopTypeID order by Shopcode";
                    string strSql = @"Select conshop.ShopId,conshop.AreaId,conshop.BuildingID,conshop.BrandID,conshop.UnitTypeID,conshop.ContractID,conshop.FloorID,conshop.LocationID,conshop.ShopCode,conshop.ShopName,conshop.RefID,conshop.RentArea,conshop.ShopStatus,ConShop.ShopTypeID,conshop.ShopStartDate,conshop.ShopEndDate,conshop.ContactorName,conshop.Tel,conshop.StoreID,ShopType.ShopTypeName From ConShop left join ShopType  on ConShop.ShopTypeID = ShopType.ShopTypeID left join ShopHdw  on ShopHdw.shopid=ConShop.shopid ";
                    strSql += " WHERE ConShop.FloorID='" + strFloorID + "' and ConShop.BuildingID='" + strBuildingID + "' and ( ConShop.ShopStatus = 0 or  ConShop.ShopStatus = 1) and ShopHdw.HdwCond=" + ShopHdw.HDWCOND_GOOD + " and ShopHdw.HdwTypeID = '" + this.ddlHdwCode.SelectedValue + "' group by conshop.ShopId,conshop.AreaId,conshop.BuildingID,conshop.BrandID,conshop.UnitTypeID,conshop.ContractID,conshop.FloorID,conshop.LocationID,conshop.ShopCode,conshop.ShopName,conshop.RefID,conshop.RentArea,conshop.ShopStatus,ConShop.ShopTypeID,conshop.ShopStartDate,conshop.ShopEndDate,conshop.ContactorName,conshop.Tel,conshop.StoreID,ShopType.ShopTypeName order by Shopcode";

                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        int RangeCode = BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "Charge", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "Charge", "1")) + 1;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            objBaseBo.WhereClause = "HdwCond = " + ShopHdw.HDWCOND_GOOD + "and ShopHdw.ShopID ='" + ds.Tables[0].Rows[i]["ShopId"].ToString() + "' and HdwTypeID='" + this.ddlHdwCode.SelectedValue + "'";
                            Resultset rs = objBaseBo.Query(new ShopHdw());
                            if (rs.Count > 0)
                            {
                                Charge charge = new Charge();
                                charge.ChgID = BaseApp.GetChgID();
                                charge.ShopID = Int32.Parse(ds.Tables[0].Rows[i]["ShopID"].ToString());
                                string strShopName = ds.Tables[0].Rows[i]["ShopName"].ToString();
                                charge.ChgStatus = Charge.CHGSTATUS_TYPE_TEMP;
                                charge.InvCode = "0";
                                charge.RangeCode = RangeCode;//批量号
                                //baseTrans.BeginTrans();
                                ViewState["chgID"] = charge.RangeCode;
                                if (strShopIDS.IndexOf("," + charge.ShopID + ",") >= 0)
                                {
                                    if (baseTrans.Insert(charge) != -1)
                                    {
                                        for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                                        {
                                            if (strArr.IndexOf("," + ChargeDetailDT.Rows[l]["ShopHdwHdwID"].ToString() + ",") >= 0)//判断是否选择
                                            {
                                                if (ChargeDetailDT.Rows[l]["ShopID"].ToString() == ds.Tables[0].Rows[i]["ShopId"].ToString())
                                                {
                                                    chgDetl.ChgID = charge.ChgID;
                                                    try { chgDetl.HdwID = Convert.ToInt32(ChargeDetailDT.Rows[l]["HdwID"]); }
                                                    catch { chgDetl.HdwID = 0; }
                                                    chgDetl.ChargeTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChargeTypeID"]);
                                                    chgDetl.ChgName = ChargeDetailDT.Rows[l]["ChargeTypeName"].ToString();
                                                    chgDetl.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["StartDate"]);
                                                    chgDetl.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["EndDate"]);

                                                    chgDetl.LastQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["LastQty"]);

                                                    chgDetl.CurQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["CurQty"]);

                                                    if (ChargeDetailDT.Rows[l]["Times"].ToString().Trim() != "")
                                                        chgDetl.Times = Convert.ToInt32(ChargeDetailDT.Rows[l]["Times"]);
                                                    else
                                                        chgDetl.Times = 1;

                                                    chgDetl.FreeQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["FreeQty"]);

                                                    chgDetl.Price = Convert.ToDecimal(ChargeDetailDT.Rows[l]["Price"]);

                                                    chgDetl.CostQty = chgDetl.CurQty - chgDetl.LastQty;//消耗量
                                                    try { chgDetl.ChgAmt = (chgDetl.CurQty - chgDetl.FreeQty - chgDetl.LastQty) * chgDetl.Times * chgDetl.Price; }
                                                    catch { chgDetl.ChgAmt = 0; }
                                                    if (ChargeDetailDT.Rows[l]["ChgDetID"].ToString().Trim() != "")
                                                        chgDetl.ChgDetID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChgDetID"]);
                                                    else
                                                        chgDetl.ChgDetID = BaseApp.GetChgDetID();
                                                    if (chgDetl.ChgAmt >=0)
                                                    {
                                                        if (baseTrans.Insert(chgDetl) != -1)
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
                                    //baseTrans.Commit();
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
                        ///*保存草稿提交的节点ID*/
                        //BaseTrans objBaseTrans = new BaseTrans();
                        //objBaseTrans.BeginTrans();
                        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                        int voucherID = RangeCode;

                        String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim();
                        ViewState["voucherHints"] = voucherHints;
                        String voucherMemo = "";
                        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                        WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);
                        //objBaseTrans.Commit();
                        HttpCookie cookies = new HttpCookie("Info");
                        cookies.Expires = System.DateTime.Now.AddDays(1);
                        cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                        cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                        cookies.Values.Add("conID", RangeCode.ToString());
                        cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                        Response.AppendCookie(cookies);
                        this.btnPutIn.Enabled = true;//提交
                    }
                }
                if (ViewState["ID"].ToString().Length == 6)//选择大楼
                {
                    string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                    //string strSql = @"Select ShopId,AreaId,BuildingID,BrandID,UnitTypeID,ContractID,FloorID,LocationID,a.CreateUserID,a.CreateTime,a.ModifyUserID,a.ModifyTime,a.OprRoleID,a.OprDeptID,ShopCode,ShopName,RefID,RentArea,ShopStatus,a.ShopTypeID,ShopStartDate,ShopEndDate,ContactorName,Tel,StoreID,ShopTypeName From ConShop a,ShopType b WHERE ";
                    //strSql += " BuildingID='" + strBuildingID + "' and ( ShopStatus = 0 or ShopStatus = 1) and a.ShopTypeID = b.ShopTypeID order by Shopcode";
                    string strSql = @"Select conshop.ShopId,conshop.AreaId,conshop.BuildingID,conshop.BrandID,conshop.UnitTypeID,conshop.ContractID,conshop.FloorID,conshop.LocationID,conshop.ShopCode,conshop.ShopName,conshop.RefID,conshop.RentArea,conshop.ShopStatus,ConShop.ShopTypeID,conshop.ShopStartDate,conshop.ShopEndDate,conshop.ContactorName,conshop.Tel,conshop.StoreID,ShopType.ShopTypeName From ConShop left join ShopType  on ConShop.ShopTypeID = ShopType.ShopTypeID left join ShopHdw  on ShopHdw.shopid=ConShop.shopid ";
                    strSql += " WHERE conshop.BuildingID='" + strBuildingID + "' and ( conshop.ShopStatus = 0 or  conshop.ShopStatus = 1) and ShopHdw.HdwCond=" + ShopHdw.HDWCOND_GOOD + " and ShopHdw.HdwTypeID = '" + this.ddlHdwCode.SelectedValue + "' group by conshop.ShopId,conshop.AreaId,conshop.BuildingID,conshop.BrandID,conshop.UnitTypeID,conshop.ContractID,conshop.FloorID,conshop.LocationID,conshop.ShopCode,conshop.ShopName,conshop.RefID,conshop.RentArea,conshop.ShopStatus,ConShop.ShopTypeID,conshop.ShopStartDate,conshop.ShopEndDate,conshop.ContactorName,conshop.Tel,conshop.StoreID,ShopType.ShopTypeName order by Shopcode";

                    DataSet ds = objBaseBo.QueryDataSet(strSql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        int RangeCode = BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "Charge", "1") == "" ? 101 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(RangeCode)", "1", "Charge", "1")) + 1;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            objBaseBo.WhereClause = "HdwCond = " + ShopHdw.HDWCOND_GOOD + "and ShopHdw.ShopID ='" + ds.Tables[0].Rows[i]["ShopId"].ToString() + "' and HdwTypeID='" + this.ddlHdwCode.SelectedValue + "'";
                            Resultset rs = objBaseBo.Query(new ShopHdw());
                            if (rs.Count > 0)
                            {
                                Charge charge = new Charge();
                                charge.ChgID = BaseApp.GetChgID();
                                charge.ShopID = Int32.Parse(ds.Tables[0].Rows[i]["ShopID"].ToString());
                                string strShopName = ds.Tables[0].Rows[i]["ShopName"].ToString();
                                charge.ChgStatus = Charge.CHGSTATUS_TYPE_TEMP;
                                charge.InvCode = "0";
                                charge.RangeCode = RangeCode;//批量号
                                //baseTrans.BeginTrans();
                                ViewState["chgID"] = charge.RangeCode;
                                if (baseTrans.Insert(charge) != -1)
                                {
                                    for (int l = 0; l < ChargeDetailDT.Rows.Count; l++)
                                    {
                                        if (strArr.IndexOf("," + ChargeDetailDT.Rows[l]["ShopHdwHdwID"].ToString() + ",") >= 0)//判断是否选择
                                        {
                                            if (ChargeDetailDT.Rows[l]["ShopID"].ToString() == ds.Tables[0].Rows[i]["ShopId"].ToString())
                                            {
                                                chgDetl.ChgID = charge.ChgID;
                                                try { chgDetl.HdwID = Convert.ToInt32(ChargeDetailDT.Rows[l]["HdwID"]); }
                                                catch { chgDetl.HdwID = 0; }
                                                chgDetl.ChargeTypeID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChargeTypeID"]);
                                                chgDetl.ChgName = ChargeDetailDT.Rows[l]["ChargeTypeName"].ToString();
                                                chgDetl.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["StartDate"]);
                                                chgDetl.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["EndDate"]);

                                                chgDetl.LastQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["LastQty"]);

                                                chgDetl.CurQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["CurQty"]);

                                                if (ChargeDetailDT.Rows[l]["Times"].ToString().Trim() != "")
                                                    chgDetl.Times = Convert.ToInt32(ChargeDetailDT.Rows[l]["Times"]);
                                                else
                                                    chgDetl.Times = 1;

                                                chgDetl.FreeQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["FreeQty"]);

                                                chgDetl.Price = Convert.ToDecimal(ChargeDetailDT.Rows[l]["Price"]);

                                                chgDetl.CostQty = chgDetl.CurQty - chgDetl.LastQty;//消耗量
                                                try { chgDetl.ChgAmt = (chgDetl.CurQty - chgDetl.FreeQty - chgDetl.LastQty) * chgDetl.Times * chgDetl.Price; }
                                                catch { chgDetl.ChgAmt = 0; }
                                                if (ChargeDetailDT.Rows[l]["ChgDetID"].ToString().Trim() != "")
                                                    chgDetl.ChgDetID = Convert.ToInt32(ChargeDetailDT.Rows[l]["ChgDetID"]);
                                                else
                                                    chgDetl.ChgDetID = BaseApp.GetChgDetID();

                                                if (chgDetl.ChgAmt >=0)
                                                {
                                                    if (baseTrans.Insert(chgDetl) != -1)
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
                                //baseTrans.Commit();
                            }
                        }
                        if(bDataISError==true)
                        {
                            baseTrans.Rollback();
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + strDateISError + "'", true);
                            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                            return;
                        }
                        /*保存草稿提交的节点ID*/
                        
                        //BaseTrans objBaseTrans = new BaseTrans();
                        //objBaseTrans.BeginTrans();
                        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
                        int voucherID = RangeCode;

                        String voucherHints = DateTime.Now.ToString() + this.ddlChargeType.SelectedItem.Text.Trim();
                        ViewState["voucherHints"] = voucherHints;
                        String voucherMemo = "";
                        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
                        WrkFlwApp.CommitVoucherDraft(Convert.ToInt32(Request.QueryString["WrkFlwID"]), Convert.ToInt32(Request.QueryString["NodeID"]), WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, vInfo, baseTrans);
                        //objBaseTrans.Commit();
                        HttpCookie cookies = new HttpCookie("Info");
                        cookies.Expires = System.DateTime.Now.AddDays(1);
                        cookies.Values.Add("wrkFlwID", Request.QueryString["WrkFlwID"].ToString());
                        cookies.Values.Add("nodeID", Request.QueryString["NodeID"].ToString());
                        cookies.Values.Add("conID", RangeCode.ToString());
                        cookies.Values.Add("ReturnSequence", WrkFlwApp.returnSequence.ToString());
                        Response.AppendCookie(cookies);
                        this.btnPutIn.Enabled = true;//提交
                    }
                }
            }
            else//更新
            {
                for (int l = 0; l<ChargeDetailDT.Rows.Count; l++)
                {
                    string txtShopHdwHdwID = ChargeDetailDT.Rows[l]["ShopHdwHdwID"].ToString();
                    if (strArr.IndexOf("," + txtShopHdwHdwID + ",") >= 0)//判断是否选择了数据
                    {
                        try { chgDetl.HdwID = Convert.ToInt32(ChargeDetailDT.Rows[l]["HdwID"]); }
                        catch { chgDetl.HdwID = 0; }
                        chgDetl.ChargeTypeID = Int32.Parse(this.ddlChargeType.SelectedValue);
                        chgDetl.ChgName = ChargeDetailDT.Rows[l]["ChargeTypeName"].ToString();
                        chgDetl.StartDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["StartDate"]);
                        chgDetl.EndDate = Convert.ToDateTime(ChargeDetailDT.Rows[l]["EndDate"]);
                        chgDetl.CurQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["CurQty"]);
                        chgDetl.LastQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["LastQty"]);
                        if (ChargeDetailDT.Rows[l]["Times"].ToString().Trim() != "")
                            chgDetl.Times = Convert.ToInt32(ChargeDetailDT.Rows[l]["Times"]);
                        else
                            chgDetl.Times = 1;
                        chgDetl.FreeQty = Convert.ToInt32(ChargeDetailDT.Rows[l]["FreeQty"]);

                        chgDetl.Price = Convert.ToDecimal(ChargeDetailDT.Rows[l]["Price"]);

                        chgDetl.CostQty = chgDetl.CurQty - chgDetl.LastQty;//消耗量
                        try { chgDetl.ChgAmt = (chgDetl.CurQty - chgDetl.FreeQty - chgDetl.LastQty) * chgDetl.Times * chgDetl.Price; }
                        catch { chgDetl.ChgAmt = 0; }
                        if (chgDetl.ChgAmt >=0)
                        {
                            string txtChgDetID = ChargeDetailDT.Rows[l]["ChgDetID"].ToString();
                            //BaseTrans objBaseTrans = new BaseTrans();
                            //objBaseTrans.BeginTrans();
                            //objBaseTrans.WhereClause = "ChgDetID=" + txtChgDetID;
                            baseTrans.WhereClause = "ChgDetID=" + txtChgDetID;//new
                            if (baseTrans.Update(chgDetl) != -1)
                            {
                                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidAdd") + "'", true);
                            }
                            //objBaseTrans.Commit();
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
        baseTrans.Commit();
        btnAdd.Enabled = false;
        this.btnPutIn.Enabled = true;
        this.page(Int32.Parse(ViewState["chgID"].ToString()));
        SumChgAmt();//计算合计金额
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 计算合计金额
    /// </summary>
    private void SumChgAmt()
    {
        decimal ChgAmt = 0;
        for (int i = 0; i < ChargeDetailDT.Rows.Count; i++)
        {
            if (ChargeDetailDT.Rows[i]["ShopHdwHdwID"].ToString() != "")
            {
                decimal dChgAmt = 0;
                try { dChgAmt = decimal.Parse(ChargeDetailDT.Rows[i]["ChgAmt"].ToString()); }
                catch { }
                ChgAmt += dChgAmt;
            }
        }
        this.lblChg.Visible = true;
        this.lblChgAmt.Visible = true;
        this.lblChgAmt.Text = ChgAmt.ToString();
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
            TextBox txtShopHdwHdwID = (TextBox)this.gvCharge.Rows[i].FindControl("txtShopHdwHdwID");
            TextBox txtCurQty = (TextBox)this.gvCharge.Rows[i].FindControl("txtCurQty");
            string strtemp = txtShopHdwHdwID.Text.Trim();
            if (strtemp != "")
            {
                if (strHaveSelects.IndexOf("," + strtemp + ",") >= 0)
                {
                    ((System.Web.UI.WebControls.CheckBox)this.gvCharge.Rows[i].Cells[0].FindControl("Checkbox")).Checked = true;
                    //txtCurQty.Text = ht[txtShopHdwHdwID.Text.Trim()].ToString();
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
        if (ViewState["ShoCheckeds"] != null)
            strShopChecks = "," + ViewState["ShoCheckeds"].ToString() + ",";
        for (int i = 0; i < this.gvCharge.Rows.Count; i++)
        {
            TextBox txtShopHdwHdwID = (TextBox)this.gvCharge.Rows[i].FindControl("txtShopHdwHdwID");
            TextBox txtCurQty = (TextBox)this.gvCharge.Rows[i].FindControl("txtCurQty");
            TextBox txtFreeQty = (TextBox)this.gvCharge.Rows[i].FindControl("txtFreeQty");
            TextBox txtPrice = (TextBox)this.gvCharge.Rows[i].FindControl("txtPrice");
            TextBox txtStartDate = (TextBox)this.gvCharge.Rows[i].FindControl("txtStartDate");
            TextBox txtEndDate = (TextBox)this.gvCharge.Rows[i].FindControl("txtEndDate");
            TextBox txtShopID = (TextBox)this.gvCharge.Rows[i].FindControl("txtShopID");
            TextBox txtLastQty = (TextBox)this.gvCharge.Rows[i].FindControl("txtLastQty");
            TextBox txtTimes = (TextBox)this.gvCharge.Rows[i].FindControl("txtTimes");

            TextBox txtCostQty = (TextBox)this.gvCharge.Rows[i].FindControl("txtCostQty");//使用量
            TextBox txtChgAmt = (TextBox)this.gvCharge.Rows[i].FindControl("txtChgAmt");//费用金额

            string strtemp = txtShopHdwHdwID.Text.Trim();
            string strShopID = txtShopID.Text.Trim();
            if (((System.Web.UI.WebControls.CheckBox)this.gvCharge.Rows[i].Cells[0].FindControl("Checkbox")).Checked)
            {
                if (checkeds.IndexOf("," + strtemp + ",") < 0)
                {
                    checkeds += strtemp + ",";
                    strShopChecks += strShopID + ",";
                    for (int j = 0; j < ChargeDetailDT.Rows.Count; j++)
                    {
                        if (ChargeDetailDT.Rows[j]["ShopHdwHdwID"].ToString() == strtemp && ChargeDetailDT.Rows[j]["ShopHdwHdwID"].ToString() != "")
                        {
                            if (txtCurQty.Text.Trim() != "")
                                ChargeDetailDT.Rows[j]["CurQty"] = txtCurQty.Text.Trim();
                            ChargeDetailDT.Rows[j]["FreeQty"] = txtFreeQty.Text.Trim();
                            ChargeDetailDT.Rows[j]["Price"] = txtPrice.Text.Trim();
                            ChargeDetailDT.Rows[j]["StartDate"] = txtStartDate.Text.Trim();
                            ChargeDetailDT.Rows[j]["EndDate"] = txtEndDate.Text.Trim();
                            ChargeDetailDT.Rows[j]["LastQty"] = txtLastQty.Text.Trim();
                            ChargeDetailDT.Rows[j]["Times"] = txtTimes.Text.Trim();
                            if (txtCostQty.Text.Trim() != "")
                                ChargeDetailDT.Rows[j]["CostQty"] = txtCostQty.Text.Trim();//
                            if (txtChgAmt.Text.Trim() != "")
                                ChargeDetailDT.Rows[j]["ChgAmt"] = txtChgAmt.Text.Trim();
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < ChargeDetailDT.Rows.Count; j++)
                    {
                        if (ChargeDetailDT.Rows[j]["ShopHdwHdwID"].ToString() == strtemp && ChargeDetailDT.Rows[j]["ShopHdwHdwID"].ToString() != "")
                        {
                            if (txtCurQty.Text.Trim() != "")
                                ChargeDetailDT.Rows[j]["CurQty"] = txtCurQty.Text.Trim();
                            ChargeDetailDT.Rows[j]["FreeQty"] = txtFreeQty.Text.Trim();
                            ChargeDetailDT.Rows[j]["Price"] = txtPrice.Text.Trim();
                            ChargeDetailDT.Rows[j]["StartDate"] = txtStartDate.Text.Trim();
                            ChargeDetailDT.Rows[j]["EndDate"] = txtEndDate.Text.Trim();
                            ChargeDetailDT.Rows[j]["LastQty"] = txtLastQty.Text.Trim();
                            ChargeDetailDT.Rows[j]["Times"] = txtTimes.Text.Trim();
                            if (txtCostQty.Text.Trim() != "")
                                ChargeDetailDT.Rows[j]["CostQty"] = txtCostQty.Text.Trim();//
                            if (txtChgAmt.Text.Trim()!="")
                                ChargeDetailDT.Rows[j]["ChgAmt"] = txtChgAmt.Text.Trim();
                        }
                    }
                }
            }
            else
            {
                //如果没选中则在串中去掉
                checkeds = checkeds.Replace("," + strtemp + ",", ",");
            }
        }
        checkeds = checkeds.TrimEnd(',').TrimStart(',');
        ViewState["checkeds"] = checkeds;
        ViewState["ShoCheckeds"] = strShopChecks.TrimEnd(',').TrimStart(',');
    }
    /// <summary>
    /// 提交审批
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        int voucherID = Convert.ToInt32(ViewState["chgID"]);
        String voucherMemo = "";
        int wrkFlwID = Convert.ToInt32(Request.Cookies["Info"].Values["wrkFlwID"]);
        int nodeID = Convert.ToInt32(Request.Cookies["Info"].Values["nodeID"]);
        int sequence = Convert.ToInt32(Request.Cookies["Info"].Values["sequence"]);
        String voucherHints = BaseInfo.BaseCommon.GetTextValueByID("voucherHints", "sequence", "WrkFlwEntity", sequence.ToString());
        if (voucherHints=="") voucherHints = ViewState["voucherHints"].ToString();
        VoucherInfo vInfo = new VoucherInfo(voucherID, voucherHints, voucherMemo, objSessionUser.DeptID, objSessionUser.UserID);
        
        if (Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]) != 0)
        {
            WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["ReturnSequence"]), vInfo);
        }
        else if (Convert.ToInt32(Request.QueryString["Sequence"].ToString()) != 0)
        {
            WrkFlwApp.ConfirmVoucher(wrkFlwID, nodeID, Convert.ToInt32(Request.Cookies["Info"].Values["Sequence"]), vInfo);
        }
        else
        {
            WrkFlwApp.CommitVoucher(wrkFlwID, nodeID, vInfo);
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "tree", "ReturnDefault()", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
        page(0);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_ConfirmOK") + "'", true);
    }
    /// <summary>
    /// 意见查看
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnMessage_Click(object sender, EventArgs e)
    {
        page(Convert.ToInt32(ViewState["chgID"]));
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 作废
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBalnkOut_Click(object sender, EventArgs e)
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

            if (baseTrans.ExecuteUpdate("UPDATE Charge SET chgstatus = 4 WHERE RangeCode=" + voucherID) == -1)
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
            page(0);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "UpdateTreePage", "UpdateTreePage()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        catch (Exception ex)
        {
            baseTrans.Rollback();
            throw ex;
        }
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
        if (ViewState["ID"] != null && ViewState["ID"].ToString() != "")
        {
            #region 选择单个硬件
            if (ViewState["ID"].ToString().Length == 15)//选择单个硬件
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strShopID = ViewState["ID"].ToString().Substring(9, 3);
                string strHdwID = ViewState["ID"].ToString().Substring(12, 3);
                string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", strShopID);
                string strShopCode = BaseInfo.BaseCommon.GetTextValueByID("ShopCode", "ShopID", "ConShop", strShopID);
                objBaseBo.WhereClause = "ShopHdw.HdwCond = " + ShopHdw.HDWCOND_GOOD + "and ShopHdw.ShopID ='" + strShopID + "' and ShopHdw.HdwTypeID='" + this.ddlHdwCode.SelectedValue + "' and ShopHdw.HdwID='" + strHdwID + "'";
                Resultset rs = objBaseBo.Query(new ShopHdw());
                if (rs.Count > 0)
                {
                    ChargeDetailDT.Rows.Clear();
                    foreach (ShopHdw objShopHdw in rs)
                    {
                        DataRow dr = ChargeDetailDT.NewRow();
                        dr["HdwCode"] = objShopHdw.HdwName;//硬件代码
                        string strSql = "select Top 1 A.CurQty from ChargeDetail A,Charge B where A.ChgID = B.ChgID and A.HdwID = " + objShopHdw.HdwID.ToString() + " and B.ShopID = " + strShopID + " and B.ChgStatus != " + Charge.CHGSTATUS_TYPE_END + " order by A.ChgdetID desc";
                        DataSet ds = objBaseBo.QueryDataSet(strSql);
                        if (ds.Tables[0].Rows.Count == 1)
                            dr["LastQty"] = ds.Tables[0].Rows[0]["CurQty"].ToString();
                        else
                            dr["LastQty"] = "0";
                        dr["HdwID"] = objShopHdw.HdwID.ToString();//硬件代码
                        dr["FreeQty"] = "0";//免费使用量
                        dr["StartDate"] = this.txtStartDate.Text.Trim();//开始时间
                        dr["EndDate"] = this.txtEndDate.Text.Trim();//结束时间
                        dr["Price"] = this.txtPrice.Text.Trim();//单价
                        dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue.ToString();
                        dr["ChargeTypeName"] = this.ddlChargeType.SelectedItem.Text.Trim();//费用类型
                        dr["ShopID"] = strShopID.ToString();//商铺编号
                        dr["ShopName"] = strShopCode + "   " + strShopName.ToString();//商铺名称
                        dr["Times"] = "1";//倍率
                        dr["ShopHdwHdwID"] = objShopHdw.HdwID.ToString();//ShopHdw表的排序号

                        ChargeDetailDT.Rows.Add(dr);
                        ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                    }
                    for (int i = 1; i < 15; i++)
                    {
                        ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                    }
                    gvCharge.DataSource = ChargeDetailDT;
                    gvCharge.DataBind();
                }
                else
                {
                    this.page(0);
                }
            }
            #endregion
            #region 选择商铺
            if (ViewState["ID"].ToString().Length == 12)//选择商铺
            {
                string strBuildingID = ViewState["ID"].ToString().Substring(3, 3);
                string strFloorID = ViewState["ID"].ToString().Substring(6, 3);
                string strShopID = ViewState["ID"].ToString().Substring(9, 3);
                string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", strShopID);
                string strShopCode = BaseInfo.BaseCommon.GetTextValueByID("ShopCode", "ShopID", "ConShop", strShopID);
                objBaseBo.WhereClause = "ShopHdw.HdwCond = " + ShopHdw.HDWCOND_GOOD + "and ShopHdw.ShopID ='" + strShopID + "' and ShopHdw.HdwTypeID='" + this.ddlHdwCode.SelectedValue + "'";
                Resultset rs = objBaseBo.Query(new ShopHdw());
                if (rs.Count > 0)
                {
                    ViewState["count"] = "0";
                    ChargeDetailDT.Rows.Clear();
                    //int j = 0;
                    foreach (ShopHdw objShopHdw in rs)
                    {
                        DataRow dr = ChargeDetailDT.NewRow();
                        dr["HdwCode"] = objShopHdw.HdwName;//硬件代码
                        string strSql = "select Top 1 A.CurQty from ChargeDetail A,Charge B where A.ChgID = B.ChgID and A.HdwID = " + objShopHdw.HdwID.ToString() + " and B.ShopID = " + strShopID + " and B.ChgStatus != " + Charge.CHGSTATUS_TYPE_END + " order by A.ChgdetID desc";
                        DataSet ds = objBaseBo.QueryDataSet(strSql);
                        if (ds.Tables[0].Rows.Count == 1)
                            dr["LastQty"] = ds.Tables[0].Rows[0]["CurQty"].ToString();
                        else
                            dr["LastQty"] = "0";
                        dr["HdwID"] = objShopHdw.HdwID.ToString();//硬件代码
                        dr["FreeQty"] = "0";//免费使用量
                        dr["StartDate"] = this.txtStartDate.Text.Trim();//开始时间
                        dr["EndDate"] = this.txtEndDate.Text.Trim();//结束时间
                        dr["Price"] = this.txtPrice.Text.Trim();//单价
                        dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue.ToString();
                        dr["ChargeTypeName"] = this.ddlChargeType.SelectedItem.Text.Trim();//费用类型
                        dr["ShopID"] = strShopID.ToString();//商铺编号
                        dr["ShopName"] = strShopCode + "   " + strShopName.ToString();//商铺名称
                        dr["Times"] = "1";//倍率
                        dr["ShopHdwHdwID"] = objShopHdw.HdwID.ToString();//ShopHdw表的排序号
                        
                        ChargeDetailDT.Rows.Add(dr);
                        ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                    }
                    for (int i = (Convert.ToInt32(ViewState["count"]) % 15); i < 15; i++)
                    {
                        ChargeDetailDT.Rows.Add(ChargeDetailDT.NewRow());
                    }
                    gvCharge.DataSource = ChargeDetailDT;
                    gvCharge.DataBind();
                }
                else
                {
                    this.page(0);
                }
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
                        objBaseBo.WhereClause = "HdwCond = " + ShopHdw.HDWCOND_GOOD + "and ShopHdw.ShopID ='" + ds.Tables[0].Rows[i]["ShopId"].ToString() + "' and HdwTypeID='" + this.ddlHdwCode.SelectedValue + "'";
                        string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                        string strShopCode = BaseInfo.BaseCommon.GetTextValueByID("ShopCode", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                        Resultset rs = objBaseBo.Query(new ShopHdw());
                        if (rs.Count > 0)
                        {
                            foreach (ShopHdw objShopHdw in rs)
                            {
                                DataRow dr = ChargeDetailDT.NewRow();
                                dr["HdwCode"] = objShopHdw.HdwName;//硬件代码
                                string strSql2 = "select Top 1 A.CurQty from ChargeDetail A,Charge B where A.ChgID = B.ChgID and A.HdwID = " + objShopHdw.HdwID.ToString() + " and B.ShopID = " + ds.Tables[0].Rows[i]["ShopId"].ToString() + " and B.ChgStatus != " + Charge.CHGSTATUS_TYPE_END + " order by A.ChgdetID desc";
                                DataSet ds2 = objBaseBo.QueryDataSet(strSql2);
                                if (ds2.Tables[0].Rows.Count == 1)
                                    dr["LastQty"] = ds2.Tables[0].Rows[0]["CurQty"].ToString();
                                else
                                    dr["LastQty"] = "0";
                                dr["HdwID"] = objShopHdw.HdwID.ToString();//硬件代码
                                dr["FreeQty"] = "0";//免费使用量
                                dr["StartDate"] = this.txtStartDate.Text.Trim();//开始时间
                                dr["EndDate"] = this.txtEndDate.Text.Trim();//结束时间
                                dr["Price"] = this.txtPrice.Text.Trim();//单价
                                dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue.ToString();
                                dr["ChargeTypeName"] = this.ddlChargeType.SelectedItem.Text.Trim();//费用类型
                                dr["ShopID"] = ds.Tables[0].Rows[i]["ShopId"].ToString();//商铺编号
                                dr["ShopName"] = strShopCode + "   " + strShopName.ToString();//商铺名称
                                dr["Times"] = "1";//倍率
                                dr["ShopHdwHdwID"] = objShopHdw.HdwID.ToString();//ShopHdw表的排序号
                                ChargeDetailDT.Rows.Add(dr);
                                ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                            }
                        }
                    }
                    for(int i=(Convert.ToInt32(ViewState["count"])%15);i<15;i++)
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
                        objBaseBo.WhereClause = "HdwCond = " + ShopHdw.HDWCOND_GOOD + "and shophdw.ShopID ='" + ds.Tables[0].Rows[i]["ShopId"].ToString() + "' and HdwTypeID='" + this.ddlHdwCode.SelectedValue + "'";
                        string strShopName = BaseInfo.BaseCommon.GetTextValueByID("ShopName", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                        string strShopCode = BaseInfo.BaseCommon.GetTextValueByID("ShopCode", "ShopID", "ConShop", ds.Tables[0].Rows[i]["ShopId"].ToString());
                        Resultset rs = objBaseBo.Query(new ShopHdw());
                        if (rs.Count > 0)
                        {
                            foreach (ShopHdw objShopHdw in rs)
                            {
                                DataRow dr = ChargeDetailDT.NewRow();
                                dr["HdwCode"] = objShopHdw.HdwName;//硬件代码
                                string strSql2 = "select Top 1 A.CurQty from ChargeDetail A,Charge B where A.ChgID = B.ChgID and A.HdwID = " + objShopHdw.HdwID.ToString() + " and B.ShopID = " + ds.Tables[0].Rows[i]["ShopId"].ToString() + " and B.ChgStatus != " + Charge.CHGSTATUS_TYPE_END + "  order by A.ChgdetID desc";
                                DataSet ds2 = objBaseBo.QueryDataSet(strSql2);
                                if (ds2.Tables[0].Rows.Count == 1)
                                    dr["LastQty"] = ds2.Tables[0].Rows[0]["CurQty"].ToString();
                                else
                                    dr["LastQty"] = "0";
                                dr["HdwID"] = objShopHdw.HdwID.ToString();//硬件代码
                                dr["FreeQty"] = "0";//免费使用量
                                dr["StartDate"] = this.txtStartDate.Text.Trim();//开始时间
                                dr["EndDate"] = this.txtEndDate.Text.Trim();//结束时间
                                dr["Price"] = this.txtPrice.Text.Trim();//单价
                                dr["ChargeTypeID"] = this.ddlChargeType.SelectedValue.ToString();
                                dr["ChargeTypeName"] = this.ddlChargeType.SelectedItem.Text.Trim();//费用类型
                                dr["ShopID"] = ds.Tables[0].Rows[i]["ShopId"].ToString();//商铺编号
                                dr["ShopName"] = strShopCode + "   " + strShopName.ToString();//商铺名称
                                dr["Times"] = "1";//倍率
                                dr["ShopHdwHdwID"] = objShopHdw.HdwID.ToString();//ShopHdw表的排序号
                                ChargeDetailDT.Rows.Add(dr);
                                ViewState["count"] = Convert.ToInt32(ViewState["count"]) + 1;
                            }
                        }
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
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + " "+ "'", true);
    }
    protected void gvCharge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex >= 0)
        {
            //TextBox txtHdwCode = (TextBox)e.Row.Cells[3].FindControl("txtHdwCode");//硬件名称
            Label txtHdwCode = (Label)e.Row.Cells[3].FindControl("txtHdwCode");//硬件名称
            TextBox txtLastQty = (TextBox)e.Row.Cells[4].FindControl("txtLastQty");//上次读数
            TextBox txtCurQty = (TextBox)e.Row.Cells[5].FindControl("txtCurQty");//本次读数
            TextBox txtFreeQty = (TextBox)e.Row.Cells[7].FindControl("txtFreeQty");//免费使用量
            TextBox txtPrice = (TextBox)e.Row.Cells[9].FindControl("txtPrice");//单价
            TextBox txtChgAmt = (TextBox)e.Row.Cells[12].FindControl("txtChgAmt");//费用金额
            TextBox txtTimes = (TextBox)e.Row.Cells[6].FindControl("txtTimes");//倍率
            TextBox txtCostQty = (TextBox)e.Row.Cells[8].FindControl("txtCostQty");//使用量
            TextBox txtStartDate = (TextBox)e.Row.Cells[8].FindControl("txtStartDate");
            TextBox txtEndDate = (TextBox)e.Row.Cells[8].FindControl("txtEndDate");
            txtCostQty.Enabled = false;
            txtChgAmt.Enabled = false;
            txtCostQty.ForeColor = Color.Black;
            txtLastQty.Attributes.Add("onblur", "SumTotalRmb('" + txtLastQty.ClientID + "','" + txtCurQty.ClientID + "','" + txtFreeQty.ClientID + "','" + txtPrice.ClientID + "','" + txtChgAmt.ClientID + "','" + txtTimes.ClientID + "','" + txtCostQty.ClientID + "')");
            txtCurQty.Attributes.Add("onblur", "SumTotalRmb('" + txtLastQty.ClientID + "','" + txtCurQty.ClientID + "','" + txtFreeQty.ClientID + "','" + txtPrice.ClientID + "','" + txtChgAmt.ClientID + "','" + txtTimes.ClientID + "','" + txtCostQty.ClientID + "')");
            txtFreeQty.Attributes.Add("onblur", "SumTotalRmb('" + txtLastQty.ClientID + "','" + txtCurQty.ClientID + "','" + txtFreeQty.ClientID + "','" + txtPrice.ClientID + "','" + txtChgAmt.ClientID + "','" + txtTimes.ClientID + "','" + txtCostQty.ClientID + "')");
            txtPrice.Attributes.Add("onblur", "SumTotalRmb('" + txtLastQty.ClientID + "','" + txtCurQty.ClientID + "','" + txtFreeQty.ClientID + "','" + txtPrice.ClientID + "','" + txtChgAmt.ClientID + "','" + txtTimes.ClientID + "','" + txtCostQty.ClientID + "')");
            txtTimes.Attributes.Add("onblur", "SumTotalRmb('" + txtLastQty.ClientID + "','" + txtCurQty.ClientID + "','" + txtFreeQty.ClientID + "','" + txtPrice.ClientID + "','" + txtChgAmt.ClientID + "','" + txtTimes.ClientID + "','" + txtCostQty.ClientID + "')");
            TextBox txtChgDetID = (TextBox)e.Row.Cells[1].FindControl("txtChgDetID");
            if (txtChgDetID.Text.Trim() != "")
            {
                ((System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Checkbox")).Checked = true;
                ((System.Web.UI.WebControls.CheckBox)e.Row.Cells[0].FindControl("Checkbox")).Enabled = false;
            }
            if (e.Row.Cells[19].Text == "1")
            {
                foreach (TableCell oCell in e.Row.Cells)
                {
                    oCell.Attributes.Add("Class", "Error");
                    txtLastQty.ForeColor = Color.Red;
                    txtCurQty.ForeColor = Color.Red;
                    txtTimes.ForeColor = Color.Red;
                    txtFreeQty.ForeColor = Color.Red;
                    txtCostQty.ForeColor = Color.Red;
                    txtPrice.ForeColor = Color.Red;
                    txtStartDate.ForeColor = Color.Red;
                    txtEndDate.ForeColor = Color.Red;
                    txtChgAmt.ForeColor = Color.Red;
                }
            }
        }
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
        //if (ViewState["chgID"] == null || ViewState["chgID"].ToString()=="")
        //{
            gvCharge.DataSource = ChargeDetailDT;
            gvCharge.DataBind();
        //}
        //else
        //    this.page(Int32.Parse(ViewState["chgID"].ToString()));
        SetDataGridSelectRecords(ViewState["checkeds"].ToString());//设置选择项
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void ddlHdwCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ShowTree(this.ddlHdwCode.SelectedValue);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
        this.page(0);
    }
}
