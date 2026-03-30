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
using BaseInfo.Store;
using Base.Page;
using BaseInfo.Dept;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class Store_Store : BasePage
{
    public string PotCustomer_Basic;
    public string Store_CardInfo;
    public string Store_ItemBackdrop;
    public string Store_FloorThing;
    public string isbro;//是编辑还是浏览
    public string baseinfo;
    protected void Page_Load(object sender, EventArgs e)
    {
        //控件添加属性
        this.SetControlPro();
        //显示树形列表
        this.ShowTree();
        if (!this.IsPostBack)
        {
            if (Request["look"] != null)
            {
                isbro = Request["look"].ToString();
                if (isbro.ToLower() == "yes")
                {
                    this.btnCancel.Visible = false;
                    this.btnSave.Visible = false;
                    this.LockControl();
                    baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Store_BusinessItemBasicInfobrowse");
                }
                else
                    baseinfo = (String)GetGlobalResourceObject("BaseInfo", "Store_BusinessItemBasicInfoMaintenance");
            }
            this.BindStoreType();
            this.BindStoreManageType();
            this.BindStatus();//绑定是否启用
            PotCustomer_Basic = (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic");
            Store_CardInfo = (String)GetGlobalResourceObject("BaseInfo", "Store_CardInfo");
            Store_ItemBackdrop = (String)GetGlobalResourceObject("BaseInfo", "Store_ItemBackdrop");
            Store_FloorThing = (String)GetGlobalResourceObject("BaseInfo", "Store_FloorThing");
            this.btnSave.Enabled = false;
        }
    }
    /// <summary>
    /// 绑定是否启用
    /// </summary>
    private void BindStatus()
    {
        int[] deptStatus = Dept.GetDeptStatus();
        for (int i = 0; i < deptStatus.Length; i++)
        {
            this.ddlStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Dept.GetDeptStatusDesc(deptStatus[i])), deptStatus[i].ToString()));
        }
    }
    private void LockControl()
    {
        this.txtStoreCode.Enabled = false;
        this.txtOfficeAddr.Enabled = false;
        this.txtOfficeAddr2.Enabled = false;
        this.txtOfficeAddr2.Enabled = false;
        this.txtStoreName.Enabled = false;
        this.txtStoreShortName.Enabled = false;
        this.txtStoreType.Enabled = false;
        this.txtStoreAddr.Enabled = false;
        this.txtStoreAmbit.Enabled = false;
        this.txtGroundParking.Enabled = false;
        this.txtUnderParking.Enabled = false;
        this.txtPropertyTel.Enabled = false;
        this.txtOfficeTel.Enabled = false;
        this.txtOfficeTel2.Enabled = false;
        this.txtOfficeZip.Enabled = false;
        this.ddlStoreManageType.Enabled = false;
        this.ddlStatus.Enabled = false;
    }
    /// <summary>
    /// 绑定项目定位类型
    /// </summary>
    private void BindStoreType()
    {
        BaseBO objBaseBo = new BaseBO();
        StoreType objStoreType = new StoreType();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo,objStoreType,"StoreTypeCode", "StoreTypeName", txtStoreType);
    }
    /// <summary>
    /// 绑定项目管理方式
    /// </summary>
    private void BindStoreManageType()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "status=1";
        StoreManageType objType = new StoreManageType();
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, objType, "TypeID", "TypeName",this.ddlStoreManageType);
    }
    /// <summary>
    /// 为控件添加属性
    /// </summary>
    private void SetControlPro()
    {
        #region
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        #endregion
        this.btnSave.Attributes.Add("onclick", "return CheckIsNull()");
        this.txtStoreName.Attributes.Add("onblur", "TextIsNotNull(txtStoreName,ImgCustName)");
        this.txtStoreShortName.Attributes.Add("onblur", "TextIsNotNull(txtStoreShortName,ImgCustShortName)");
    }
    /// <summary>
    /// 显示树形列表
    /// </summary>
    private void ShowTree()
    {
        string jsdept = "";
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "depttype = 6";
        objBaseBo.OrderBy = "OrderID";
        BaseBO baseBo = new BaseBO();
        DataSet ds = new DataSet();
        ds = baseBo.QueryDataSet("select DeptName from Dept where DeptType=1");
        if (ds.Tables[0].Rows.Count > 0)
        {
            jsdept = "100" + "|" + "10" + "|" + ds.Tables[0].Rows[0][0] + "^";
        }
        else
        {
            jsdept = "100" + "|" + "10" + "|" + "商业项目" + "^";        
        }
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            objBaseBo.WhereClause += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        Resultset rs = objBaseBo.Query(new Dept());
        if (rs.Count > 0)
        {
            foreach (Dept objDept in rs)
            {
                jsdept += objDept.DeptID + "|" + "100" + "|" + objDept.DeptName + "^";
            }
        }
        depttxt.Value = jsdept;
    }
    /// <summary>
    /// 树形列表单击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeClick_Click(object sender, EventArgs e)
    {
        if (deptid.Value != "100")//阳光新业
        {
            this.ClearTextBox();
            ViewState["DeptID"] = deptid.Value;
            this.GetStoreValueByID(ViewState["DeptID"].ToString());
        }
        else
            this.ClearTextBox();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "line", "Load()", true);
        this.btnSave.Enabled = true;
    }
    /// <summary>
    /// 获得一条记录
    /// </summary>
    /// <param name="strID"></param>
    private void GetStoreValueByID(string strID)
    {
        BaseBO objBaseBo = new BaseBO();
        Store objStore = new Store();
        objBaseBo.WhereClause = "StoreId=" + strID;
        Resultset rs = objBaseBo.Query(new Store());
        if (rs.Count != 0)
        {
            objStore = rs.Dequeue() as Store;
            this.txtStoreShortName.Text = objStore.StoreShortName;
            try { this.txtStoreType.SelectedValue = objStore.StoreType.ToString(); }//类型
            catch { }
            try { this.ddlStoreManageType.SelectedValue = objStore.StoreManageTypeID.ToString(); }//项目管理类型
            catch { }
            this.txtStoreAddr.Text = objStore.StoreAddr;
            this.txtStoreAmbit.Text = objStore.StoreAmbit;
            this.txtGroundParking.Text = objStore.GroundParking.ToString();
            this.txtUnderParking.Text = objStore.UnderParking.ToString();
            this.txtOfficeAddr.Text = objStore.OfficeAddr;
            this.txtOfficeAddr2.Text = objStore.OfficeAddr2;
            this.txtOfficeAddr3.Text = objStore.OfficeAddr3;
            this.txtOfficeZip.Text = objStore.OfficeZip;
            this.txtOfficeTel.Text = objStore.OfficeTel;
            this.txtPropertyTel.Text = objStore.PropertyTel;
            this.txtOfficeTel2.Text = objStore.OfficeTel2;
            this.txtOrder.Text = objStore.OrderID.ToString();
            try { this.ddlStatus.SelectedValue = objStore.StoreStatus.ToString(); }//项目管理类型
            catch { }
        }
        Dept objDept = new Dept();
        objBaseBo.WhereClause = "deptid=" + strID;
        Resultset rsd = objBaseBo.Query(new Dept());
        if (rsd.Count != 0)
        {
            objDept = rsd.Dequeue() as Dept;
            this.txtStoreCode.Text = objDept.DeptCode;
            this.txtStoreName.Text = objDept.DeptName;
            //this.txtStoreShortName.Text = objDept.DeptName;
        }
    }
    /// <summary>
    /// 保存事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["DeptID"] != null && ViewState["DeptID"].ToString() != "")
        {
            if (this.JudgeISExist(ViewState["DeptID"].ToString()) == true)
                this.SaveUpdate();
            else
                this.SaveAdd();
        }
        ViewState["DeptID"] = "";
        this.ClearTextBox();
        this.ShowTree();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "line", "Load()", true);
        this.btnSave.Enabled = false;
    }
    /// <summary>
    /// 判断表中是否存在记录
    /// </summary>
    /// <param name="strID"></param>
    /// <returns></returns>
    private bool JudgeISExist(string strID)
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "storeid="+strID;
        DataSet ds = objBaseBo.QueryDataSet(new Store());
        if (ds.Tables[0].Rows.Count > 0)
            return true;//存在记录
        else
            return false;//不存在记录
    }
    
    /// <summary>
    /// 清空控件中的值
    /// </sueckmmary>
    private void ClearTextBox()
    {
        this.txtStoreCode.Text = "";
        this.txtStoreName.Text = "";
        this.txtStoreShortName.Text = "";
        this.txtStoreAddr.Text = "";
        this.txtStoreAmbit.Text = "";
        this.txtGroundParking.Text = "";
        this.txtUnderParking.Text = "";
        this.txtOfficeAddr.Text = "";
        this.txtOfficeAddr2.Text = "";
        this.txtOfficeAddr3.Text = "";
        this.txtOfficeZip.Text = "";
        this.txtOfficeTel.Text = "";
        this.txtPropertyTel.Text = "";
        this.txtOfficeTel2.Text = "";
    }
    /// <summary>
    /// 新增
    /// </summary>
    private void SaveAdd()
    {
        BaseBO objBaseBo = new BaseBO();
        Store objStore = new Store();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objStore.StoreId = Int32.Parse(ViewState["DeptID"].ToString());
        objStore.StoreCode = this.txtStoreCode.Text.Trim();
        objStore.StoreName = this.txtStoreName.Text.Trim();
        objStore.StoreShortName = this.txtStoreShortName.Text.Trim();
        try { objStore.StoreType = Int32.Parse(this.txtStoreType.SelectedValue.ToString()); }//商业项目定位类型
        catch { }
        try { objStore.StoreManageTypeID = Int32.Parse(this.ddlStoreManageType.SelectedValue.ToString()); }
        catch { }
        objStore.StoreAddr = this.txtStoreAddr.Text.Trim();
        objStore.StoreAmbit = this.txtStoreAmbit.Text.Trim();
        if (this.txtGroundParking.Text.Trim() != "")
            objStore.GroundParking = Int32.Parse(this.txtGroundParking.Text.Trim());
        else
            objStore.GroundParking = 0;
        if (this.txtUnderParking.Text.Trim() != "")
            objStore.UnderParking = Int32.Parse(this.txtUnderParking.Text.Trim());
        else
            objStore.UnderParking = 0;
        objStore.OfficeAddr = this.txtOfficeAddr.Text.Trim();

        objStore.OfficeAddr2 = this.txtOfficeAddr2.Text.Trim();
        objStore.OfficeAddr3 = this.txtOfficeAddr3.Text.Trim();

        objStore.OfficeZip = this.txtOfficeZip.Text.Trim();
        objStore.OfficeTel = this.txtOfficeTel.Text.Trim();
        objStore.PropertyTel = this.txtPropertyTel.Text.Trim();
        objStore.OfficeTel2 = this.txtOfficeTel2.Text.Trim();

        objStore.CreateUserId = objSessionUser.CreateUserID;
        objStore.CreateTime = DateTime.Now.Date;
        objStore.ModifyUserId = objSessionUser.ModifyUserID;
        objStore.ModifyTime = DateTime.Now.Date;
        objStore.OprRoleID = objSessionUser.OprRoleID;
        objStore.OprDeptID = objSessionUser.OprDeptID;

        try { objStore.OrderID = Int32.Parse(this.txtOrder.Text.Trim()); }
        catch{objStore.OrderID=0;}
        try { objStore.StoreStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { }
        if (objBaseBo.Insert(objStore) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "line", "Load()", true);           
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    private void SaveUpdate()
    {
        BaseBO objBaseBo = new BaseBO();
        Store objStore = new Store();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        //objStore.StoreId = Base.BaseApp.GetCustumerID("Store", "StoreId");
        objStore.StoreCode = this.txtStoreCode.Text.Trim();
        objStore.StoreName = this.txtStoreName.Text.Trim();
        objStore.StoreShortName = this.txtStoreShortName.Text.Trim();
        try { objStore.StoreType = Int32.Parse(this.txtStoreType.SelectedValue.ToString()); }//商业项目定位类型
        catch { }
        try { objStore.StoreManageTypeID = Int32.Parse(this.ddlStoreManageType.SelectedValue.ToString()); }
        catch { }
        objStore.StoreAddr = this.txtStoreAddr.Text.Trim();
        objStore.StoreAmbit = this.txtStoreAmbit.Text.Trim();
        if (this.txtGroundParking.Text.Trim() != "")
            objStore.GroundParking = Int32.Parse(this.txtGroundParking.Text.Trim());
        else
            objStore.GroundParking = 0;
        if (this.txtUnderParking.Text.Trim() != "")
            objStore.UnderParking = Int32.Parse(this.txtUnderParking.Text.Trim());
        else
            objStore.UnderParking = 0;
        objStore.OfficeAddr = this.txtOfficeAddr.Text.Trim();
        objStore.OfficeAddr2 = this.txtOfficeAddr2.Text.Trim();
        objStore.OfficeAddr3 = this.txtOfficeAddr3.Text.Trim();
        objStore.OfficeZip = this.txtOfficeZip.Text.Trim();
        objStore.OfficeTel = this.txtOfficeTel.Text.Trim();
        objStore.PropertyTel = this.txtPropertyTel.Text.Trim();
        objStore.OfficeTel2 = this.txtOfficeTel2.Text.Trim();
        objStore.CreateUserId = objSessionUser.CreateUserID;
        objStore.CreateTime = DateTime.Now.Date;
        objStore.ModifyUserId = objSessionUser.ModifyUserID;
        objStore.ModifyTime = DateTime.Now.Date;
        objStore.OprRoleID = objSessionUser.OprRoleID;
        objStore.OprDeptID = objSessionUser.OprDeptID;

        try { objStore.OrderID = Int32.Parse(this.txtOrder.Text.Trim()); }
        catch { objStore.OrderID = 0; }
        try { objStore.StoreStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { }

        objBaseBo.WhereClause = "StoreId=" + ViewState["DeptID"].ToString();
        if (objBaseBo.Update(objStore) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "line", "Load()", true);                       
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/BaseInfo/Store/StoreInfoRef.aspx?look=no");
    }
}
