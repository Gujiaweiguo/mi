using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Page;
using Lease.PotCustLicense;
using Lease.Contract;
using Base.Biz;
using Base.DB;
using RentableArea;
using Base;
using Shop.ShopType;
using Base.Util;

public partial class Lease_PotCustomer_PotShopNew : BasePage
{
    private static int SELECTED = -1;
    private static int DISPROVE_UP = 1;
    private static int DISPROVE_IN = 2;
    public string strError;
    public string strErrorTime;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if (Request["look"] != null)
            {
                if (Request["look"] == "yes")
                {
                    this.btnSave.Visible = false;
                    this.btnCancel.Visible = false;
                    this.ddlBusinessItem.Enabled = false;
                    this.ddlShopType.Enabled = false;
                    this.cmbBizMode.Enabled = false;
                }
            }
            else
            {
                this.txtUnits.Attributes.Add("onclick", "ShowUnitTree()");
            }
            strError = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage");//信息不能为空
            strErrorTime = (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime");//时间比较
            this.btnSave.Attributes.Add("onclick", "return InputValidator()");
            txtRentalPrice.Attributes.Add("onblur", "TextIsNotNull(txtRentalPrice,ImgRentalPrice)");
            txtRentArea.Attributes.Add("onblur", "TextIsNotNull(txtRentArea,imgRentArea)");
            txtRentInc.Attributes.Add("onblur", "TextIsNotNull(txtRentInc,imgRentInc)");
            txtPcent.Attributes.Add("onblur", "TextIsNotNull(txtPcent,imgPcent)");
            txtMainBrand.Attributes.Add("onblur", "TextIsNotNull(txtMainBrand,imgMainBrand)");
            txtShopStartDate.Attributes.Add("onblur", "TextIsNotNull(txtShopStartDate,imgShopStartDate)");
            txtShopEndDate.Attributes.Add("onblur", "TextIsNotNull(txtShopEndDate,imgShopEndDate)");
            txtUnits.Attributes.Add("onblur", "TextIsNotNull(txtUnits,imgUnits)");
            txtPotShopName.Attributes.Add("onblur", "TextIsNotNull(txtPotShopName,imgPotShopName)");
            //btnSave.Attributes.Add("onclick", "return LicenseBoxValidator(txtShopStartDate,txtShopEndDate,'" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidDateTime")  + "')");
            txtRentalPrice.Attributes.Add("onkeydown", "textleave()");
            txtRentArea.Attributes.Add("onkeydown", "textleave()");
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            PotShop potShop = new PotShop();


            /*商铺类型*/
            rs = baseBO.Query(new ShopType());
            ddlShopType.Text = "";
            foreach (ShopType shopType in rs)
            {
                ddlShopType.Items.Add(new ListItem(shopType.ShopTypeName, shopType.ShopTypeID.ToString()));
            }
            //绑定商业项目
            BaseInfo.BaseCommon.BindDropDownList("Select deptid,deptname from dept where depttype=6 order by orderid", "deptid", "deptname", this.ddlBusinessItem);

            int[] bizModes = Contract.GetBizModes();
            for (int i = 0; i < bizModes.Length; i++)
            {
                cmbBizMode.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Contract.GetBizModeDesc(bizModes[i])), bizModes[i].ToString()));
            }

            rs = baseBO.Query(new Area());
            cmbArea.Text = "";
            cmbArea.Items.Add(new ListItem((String)GetGlobalResourceObject("BaseInfo", "PotShop_Selected"), SELECTED.ToString()));

            foreach (Area area in rs)
            {
                cmbArea.Items.Add(new ListItem(area.AreaName, area.AreaID.ToString()));
            }

            try
            {
                if (Request.Cookies["Custumer"].Values["CustumerID"] != "")
                {
                    ViewState["CustID"] = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
                    baseBO.WhereClause = "a.CustID=" + ViewState["CustID"];
                    rs = baseBO.Query(new PotCustomerInfo());
                    if (rs.Count == 1)
                    {
                        PotCustomerInfo potCustomerInfo = rs.Dequeue() as PotCustomerInfo;
                        txtCreateUserID.Text = potCustomerInfo.CustCode.ToString();
                        txtCustName.Text = potCustomerInfo.CustName;
                        txtCustShortName.Text = potCustomerInfo.CustShortName;
                        //txtContactorName.Text = potCustomerInfo.ContactorName;
                        //txtOfficeTel.Text = potCustomerInfo.OfficeTel;
                        //txtMobileTel.Text = potCustomerInfo.MobileTel;
                        txtCommOper.Text = potCustomerInfo.UserName;
                        cmbBizMode.SelectedValue = potShop.BizMode.ToString();
                        ViewState["CustomerStatus"] = potCustomerInfo.CustomerStatus;
                    }
                    baseBO.WhereClause = " custid =" + ViewState["CustID"].ToString() + " and shopstatus=1 ";
                    rs = baseBO.Query(potShop);
                    if (rs.Count == 1)
                    {
                        potShop = rs.Dequeue() as PotShop;
                        txtPotShopName.Text = potShop.PotShopName;
                        txtMainBrand.Text = potShop.MainBrand;
                        txtShopStartDate.Text = potShop.ShopStartDate.ToString("yyyy-MM-dd");
                        txtShopEndDate.Text = potShop.ShopEndDate.ToString("yyyy-MM-dd");
                        cmbArea.SelectedValue = potShop.AreaID.ToString();
                        txtRentalPrice.Text = potShop.RentalPrice.ToString();
                        txtRentArea.Text = potShop.RentArea.ToString();
                        txtNode.Text = potShop.Note;
                        ddlShopType.SelectedValue = potShop.ShopTypeID.ToString();
                        cmbBizMode.SelectedValue = potShop.BizMode.ToString();
                        this.txtMainBrand.Text = potShop.MainBrand;
                        this.txtRentInc.Text = potShop.RentInc;
                        this.txtPcent.Text = potShop.Pcent;
                        this.txtWaterReg.Text = potShop.WaterReg;
                        this.txtHighReg.Text = potShop.HighReg;
                        this.txtLoadReg.Text = potShop.LoadReg;
                        this.txtPowerReg.Text = potShop.PowerReg;
                        this.ddlBusinessItem.SelectedValue = potShop.StoreID.ToString();
                        ViewState["Disprove"] = DISPROVE_UP;

                        //显示意向单元
                        DataSet ds = baseBO.QueryDataSet("select unitid,unitcode,buildingid,floorid,locationid from PotShopUnit where potshopid='"+potShop.PotShopID+"'");
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                this.txtUnitID.Text += ds.Tables[0].Rows[i]["unitid"].ToString()+",";
                                this.txtUnits.Text += ds.Tables[0].Rows[i]["unitcode"].ToString()+",";
                                this.txtStore.Text += ds.Tables[0].Rows[i]["buildingid"].ToString() + ";" + ds.Tables[0].Rows[i]["floorid"].ToString() + ";" + ds.Tables[0].Rows[i]["locationid"].ToString() + ",";
                            }
                        }
                        //
                    }
                    else
                    {
                        try { this.ddlShopType.SelectedValue = "103"; }
                        catch { this.ddlShopType.SelectedValue = "0"; }
                        ViewState["Disprove"] = DISPROVE_IN;
                    }
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
                Logger.Log("读取商铺信息错误:", ex);
                btnSave.Enabled = false;
            }
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ViewState["Disprove"]) == DISPROVE_IN)
        {
            AddPotShop();
        }
        else if (Convert.ToInt32(ViewState["Disprove"]) == DISPROVE_UP)
        {
            UpdatePotShop();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
        }
    }

    private void textClear()
    {
        txtPotShopName.Text = "";
        txtMainBrand.Text = "";
        txtShopStartDate.Text = "";
        txtShopEndDate.Text = "";
        cmbArea.SelectedIndex = 0;
        txtRentalPrice.Text = "";
        txtRentArea.Text = "";
        txtNode.Text = "";
    }
    private void UpdatePotShop()
    {
        BaseBO baseBO = new BaseBO();
        BaseTrans baseTrans = new BaseTrans();
        PotShop potShop = new PotShop();
        PotCustomerStatus potCustomerStatus = new PotCustomerStatus();
        potShop.PotShopID = BaseApp.GetPotShopID();
        potShop.CustID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);
        potShop.PotShopName = txtPotShopName.Text.Trim();
        potShop.MainBrand = txtMainBrand.Text.Trim();
        try { potShop.ShopStartDate = Convert.ToDateTime(txtShopStartDate.Text); }
        catch { }
        try { potShop.ShopEndDate = Convert.ToDateTime(txtShopEndDate.Text); }
        catch { }

        try { potShop.AreaID = Convert.ToInt32(cmbArea.SelectedValue); }
        catch { potShop.AreaID = 0; }
        try { potShop.RentalPrice = Convert.ToDecimal(txtRentalPrice.Text); }
        catch { potShop.RentalPrice = 0; }
        try { potShop.RentArea = Convert.ToDecimal(txtRentArea.Text); }
        catch { potShop.RentArea = 0; }

        try { potShop.BizMode = Convert.ToInt32(cmbBizMode.SelectedValue); }
        catch { potShop.BizMode = 0; }
        try { potShop.StoreID = Convert.ToInt32(this.ddlBusinessItem.SelectedValue.ToString()); }
        catch { potShop.StoreID = 0; }
        potShop.RentInc = this.txtRentInc.Text.Trim();
        potShop.Pcent = this.txtPcent.Text.Trim();
        potShop.Note = txtNode.Text.Trim();
        potShop.HighReg = this.txtHighReg.Text.Trim();//层高要求
        potShop.LoadReg = this.txtLoadReg.Text.Trim();//荷载要求
        potShop.WaterReg = this.txtWaterReg.Text.Trim();//上下水
        potShop.PowerReg = this.txtPowerReg.Text.Trim();//电量要求
        potShop.UnitId = this.txtUnitID.Text.TrimEnd(',');
        try { potShop.ShopTypeID = Convert.ToInt32(ddlShopType.SelectedValue); }
        catch { potShop.ShopTypeID = 0; }

        //保存单元
        try
        {
            string strPotShopId = "";
            DataSet ds = baseBO.QueryDataSet("select potshopid from PotShop where custid='" + Request.Cookies["Custumer"].Values["CustumerID"] .ToString()+ "'");
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                strPotShopId = ds.Tables[0].Rows[0]["potshopid"].ToString();
            }
            baseBO.ExecuteUpdate("delete from PotShopUnit where PotShopID='" + strPotShopId + "'");//删除原有的单元
            char[] treenodeid = new char[] { ',' };
            string treestr = this.txtUnitID.Text.TrimEnd(',');
            string strCode = this.txtUnits.Text.TrimEnd(',');
            string strBuildFloorLoca = this.txtStore.Text.TrimEnd(',');//得到楼、楼层、方位ID

            string[] strUnitID = treestr.Split(treenodeid);
            string[] strUnitCode = strCode.Split(treenodeid);
            string[] strBFL = strBuildFloorLoca.Split(treenodeid);//得到楼、楼层、方位ID

            for (int i = 0; i < strUnitID.Length; i++)
            {
                if (strUnitID[i].ToString() != "")
                {
                    string[] BuFlLo = strBFL[i].Split(';');
                    string buildID = BuFlLo[0].ToString();
                    string FloorID = BuFlLo[1].ToString();
                    string LocationID = BuFlLo[2].ToString();
                    baseBO.ExecuteUpdate("insert into PotShopUnit(PotShopID,storeID,buildingID,floorID,LocationID,UnitId,unitCode) values ('" + strPotShopId + "','" + this.ddlBusinessItem.SelectedValue.ToString() + "','" + buildID + "','" + FloorID + "','" + LocationID + "','" + strUnitID[i].ToString() + "','" + strUnitCode[i].ToString() + "')");
                }
            }
        }
        catch { }
        //
        baseTrans.BeginTrans();
        baseTrans.WhereClause = "custid=" + ViewState["CustID"];
        if (baseTrans.Update(potShop) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
            baseTrans.Rollback();
            return;
        }
        baseTrans.Commit();
    }

    private void AddPotShop()
    {
        BaseBO baseBO = new BaseBO();
        PotShop potShop = new PotShop();
        PotCustomerStatus potCustomerStatus = new PotCustomerStatus();
        try
        {
            potShop.PotShopID = BaseApp.GetPotShopID();
            potShop.CustID = Convert.ToInt32(Request.Cookies["Custumer"].Values["CustumerID"]);

            potShop.PotShopName = txtPotShopName.Text.Trim();
            potShop.MainBrand = txtMainBrand.Text.Trim();
            try { potShop.ShopStartDate = Convert.ToDateTime(txtShopStartDate.Text); }
            catch { }
            try { potShop.ShopEndDate = Convert.ToDateTime(txtShopEndDate.Text); }
            catch { }
            potShop.UnitId = this.txtUnitID.Text.TrimEnd(',');
            try { potShop.AreaID = Convert.ToInt32(cmbArea.SelectedValue); }
            catch { potShop.AreaID = 0; }
            try { potShop.RentalPrice = Convert.ToDecimal(txtRentalPrice.Text); }
            catch { potShop.RentalPrice = 0; }
            try { potShop.RentArea = Convert.ToDecimal(txtRentArea.Text); }
            catch { potShop.RentArea = 0; }

            try { potShop.BizMode = Convert.ToInt32(cmbBizMode.SelectedValue); }
            catch { potShop.BizMode = 0; }
            try { potShop.StoreID = Convert.ToInt32(this.ddlBusinessItem.SelectedValue.ToString()); }
            catch { potShop.StoreID = 0; }
            potShop.RentInc = this.txtRentInc.Text.Trim();
            potShop.Pcent = this.txtPcent.Text.Trim();
            potShop.Note = txtNode.Text.Trim();//备注信息
            potShop.HighReg = this.txtHighReg.Text.Trim();//层高要求
            potShop.LoadReg = this.txtLoadReg.Text.Trim();//荷载要求
            potShop.WaterReg = this.txtWaterReg.Text.Trim();//上下水
            potShop.PowerReg = this.txtPowerReg.Text.Trim();//电量要求
            try { potShop.ShopTypeID = Convert.ToInt32(ddlShopType.SelectedValue); }
            catch { potShop.ShopTypeID = 0; }
            potShop.UnitId = this.txtUnitID.Text.TrimEnd(',');
            potShop.Sequence = Convert.ToInt32(Request.Cookies["Sequence"].Values["SequenceID"]);
            potShop.ShopSort = BaseInfo.BaseCommon.GetTextValueByID("max(ShopSort)", "custid", "potshop", potShop.CustID.ToString()) == "" ? 1 : Int32.Parse(BaseInfo.BaseCommon.GetTextValueByID("max(ShopSort)", "custid", "potshop", potShop.CustID.ToString())) + 1;

            potShop.ShopStatus = 1;
           
            //保存单元
            try
            {
                char[] treenodeid = new char[] { ',' };
                string treestr = this.txtUnitID.Text.TrimEnd(',');
                string strCode = this.txtUnits.Text.TrimEnd(',');
                string strBuildFloorLoca = this.txtStore.Text.TrimEnd(',');//得到楼、楼层、方位ID

                string[] strUnitID = treestr.Split(treenodeid);
                string[] strUnitCode = strCode.Split(treenodeid);
                string[] strBFL = strBuildFloorLoca.Split(treenodeid);//得到楼、楼层、方位ID

                for (int i = 0; i < strUnitID.Length; i++)
                {
                    if (strUnitID[i].ToString() != "")
                    {
                        string[] BuFlLo = strBFL[i].Split(';');
                        string buildID = BuFlLo[0].ToString();
                        string FloorID = BuFlLo[1].ToString();
                        string LocationID = BuFlLo[2].ToString();
                        baseBO.ExecuteUpdate("insert into PotShopUnit(PotShopID,storeID,buildingID,floorID,LocationID,UnitId,unitCode) values ('" + potShop.PotShopID + "','" + this.ddlBusinessItem.SelectedValue.ToString() + "','" + buildID + "','" + FloorID + "','" + LocationID + "','" + strUnitID[i].ToString() + "','" + strUnitCode[i].ToString() + "')");
                    }
                }
            }
            catch { }
            //

            if (baseBO.Insert(potShop) != -1)
            {
                ViewState["Disprove"] = DISPROVE_UP;
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "ShowInfo_DelFail") + "'", true);
                return;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_ErrorLog") + "'", true);
            Logger.Log("意向商铺信息错误:", ex);
        }
    }
    protected void btnQuit_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/PotCustomer/PotShopNew.aspx");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDellBrand_Click(object sender, EventArgs e)
    {
        //BaseBO objBaseBo = new BaseBO();
        //objBaseBo.WhereClause = "TradeID = " + Convert.ToInt32(hidTradeID.Text);
        //Resultset rs = objBaseBo.Query(new TradeRelation());
        //if (rs.Count == 1)
        //{
        //    TradeRelation tradeRelation = rs.Dequeue() as TradeRelation;
        //    this.txtTrade.Text = tradeRelation.TradeName;
        //}
        //objBaseBo.WhereClause = "";
    }
}
