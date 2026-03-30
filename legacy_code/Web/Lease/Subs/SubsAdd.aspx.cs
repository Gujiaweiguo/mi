using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Lease.Subs;
using BaseInfo.Dept;
using Base.Biz;
using BaseInfo.User;
using Base.DB;
using Base.Page;
public partial class Lease_Subs_SubsAdd : BasePage
{
    /**
     * 存放在ViewState里,用来标志当前的操作
     */

    public string strBaseInfo;
    public string strFresh;
    private void Page_Load(object sender, System.EventArgs e)
    {
        Page.Response.Buffer = false;
        Page.Response.Cache.SetNoStore();
        
        this.SetControlPro();//为按钮添加属性
        this.ShowTree();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "a1", "styletabbar_atv()", true);
        if (!IsPostBack)
        {
            ViewState["ID"] = deptid.Value;
            selectdeptid.Value = Convert.ToString(ViewState["ID"]);
            if (Request["browse"] != null)//从浏览菜单进入
            {
                string strBrowse = Request["browse"].ToString();
                if (strBrowse.ToLower() == "yes")
                {
                    this.btnSave.Visible = false;
                    this.btnCancel.Visible = false;
                    this.LockControl();
                    strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Subs_SubCompanyBrowse");
                }
            }
            else
                strBaseInfo = (String)GetGlobalResourceObject("BaseInfo", "Subs_SubCompanyVindicate");
            this.BindFinStatus();
            this.BindStatus();
            strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
        this.btnSave.Enabled = false;
    }
    private void LockControl()
    {
        this.txtCode.Enabled = false;
        this.txtCompanyShortName.Enabled = false;
        this.txtCompanyName.Enabled = false;
        this.txtBank.Enabled = false;
        this.txtBankAccount.Enabled = false;
        this.ddlFinType.Enabled = false;
        this.ddlStatus.Enabled = false;
    }
    /// <summary>
    /// 为按钮添加属性
    /// </summary>
    private void SetControlPro()
    {
        #region
        //btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        #endregion
    }
    /// <summary>
    /// 绑定是否启用下拉框
    /// </summary>
    private void BindStatus()
    {
        int[] deptStatus = Dept.GetDeptStatus();
        for (int i = 0; i < deptStatus.Length; i++)
        {
            this.ddlStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Dept.GetDeptStatusDesc(deptStatus[i])), deptStatus[i].ToString()));
        }
    }
    /// <summary>
    /// 绑定财务类型
    /// </summary>
    private void BindFinStatus()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "FinancialTypeStatus=1";
        BaseInfo.BaseCommon.BindDropDownList(objBaseBo, new FinancialType(), "FinancialTypeID", "FinancialTypeName", this.ddlFinType);
    }
    /// <summary>
    /// 绑定子公司树
    /// </summary>
    private void ShowTree()
    {
        string str = "";
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select deptid,pdeptid,deptname from dept where deptid=100");
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            str += ds.Tables[0].Rows[0]["deptid"].ToString() + "|" + ds.Tables[0].Rows[0]["pdeptid"].ToString() + "|" + ds.Tables[0].Rows[0]["deptname"].ToString() + "|" + "" + "^";
        }
        objBaseBo.WhereClause = "Depttype=2 and deptstatus=1";
        objBaseBo.OrderBy = "deptid";
        Resultset rs = objBaseBo.Query(new Dept());
        if (rs.Count > 0)
        {
            foreach (Dept objDept in rs)
            {
                //str += objDept.DeptID + "|" + objDept.PDeptID + "|" + objDept.DeptName + "|" + "" + "^";
                if (objDept.DeptStatus == Dept.DEPTSTATUS_INVALID)
                {
                    str += objDept.DeptID + "|" + objDept.PDeptID + "|" + objDept.DeptName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                }
                else
                {
                    str += objDept.DeptID + "|" + objDept.PDeptID + "|" + objDept.DeptName + "|" + "" + "^";
                }
            }
        }
        depttxt.Value = str;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.ClearTextBox();
        ViewState["DeptID"] = null;
        //Response.Redirect("~/Lease/Subs/SubsAdd.aspx");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 获得一条记录
    /// </summary>
    /// <param name="strID"></param>
    private void GetSubValueByID(string strID)
    {
        BaseBO objBaseBo = new BaseBO();
        Dept objDept = new Dept();
        Subs objSubs = new Subs();

        objBaseBo.WhereClause = "deptid=" + strID;
        Resultset rsd = objBaseBo.Query(new Dept());
        objDept = rsd.Dequeue() as Dept;
        
        objBaseBo.WhereClause = "SubsId=" + strID;
        Resultset rs = objBaseBo.Query(new Subs());
        
        if (rs.Count != 0)
        {
            this.txtCode.Text = objDept.DeptCode; //objSubs.SubsCode;
            this.txtCompanyName.Text = objSubs.SubsName;
            this.txtCompanyShortName.Text = objSubs.SubsShortName;
            this.txtBank.Text = objSubs.BankName;
            this.txtBankAccount.Text = objSubs.BankAcct;
            this.ddlFinType.SelectedValue = objSubs.FinancialTypeID.ToString(); 
            
            this.ddlStatus.SelectedValue = objSubs.SubsStatus.ToString(); 
        }
        else
        {

            if (rsd.Count != 0)
            {
                this.txtCode.Text = objDept.DeptCode;
                this.txtCompanyName.Text = objDept.DeptName;
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["DeptID"] != null && ViewState["DeptID"].ToString() != "")
        {
            if (this.JudgeISExist(ViewState["DeptID"].ToString()) == true)//存在记录
                this.SaveUpdate();//更新
            else
                this.SaveAdd();//新增保存
        }
        ViewState["DeptID"] = "";
        this.ClearTextBox();
        this.ShowTree();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 判断表中是否存在记录
    /// </summary>
    /// <param name="strID"></param>
    /// <returns></returns>
    private bool JudgeISExist(string strID)
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "subsid=" + strID;
        DataSet ds = objBaseBo.QueryDataSet(new Subs());
        if (ds.Tables[0].Rows.Count > 0)
            return true;//存在记录
        else
            return false;//不存在记录
    }
    /// <summary>
    /// 更新数据
    /// </summary>
    private void SaveUpdate()
    {
        BaseBO objBaseBo = new BaseBO();
        Subs objSubs = new Subs();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objSubs.SubsCode = this.txtCode.Text.Trim();
        objSubs.SubsName = this.txtCompanyName.Text.Trim();
        objSubs.SubsShortName = this.txtCompanyShortName.Text.Trim();
        try { objSubs.FinancialTypeID = Int32.Parse(this.ddlFinType.SelectedValue); }
        catch { objSubs.FinancialTypeID = 0; }
        try { objSubs.SubsStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { objSubs.SubsStatus = 0; }
        objSubs.BankName = this.txtBank.Text.Trim();//银行
        objSubs.BankAcct = this.txtBankAccount.Text.Trim();//银行账号

        objSubs.CreateUserId = objSessionUser.CreateUserID;
        objSubs.CreateTime = DateTime.Now.Date;
        objSubs.ModifyUserId = objSessionUser.ModifyUserID;
        objSubs.ModifyTime = DateTime.Now.Date;
        objSubs.OprRoleID = objSessionUser.OprRoleID;
        objSubs.OprDeptID = objSessionUser.OprDeptID;
        objBaseBo.WhereClause = "subsId=" + ViewState["DeptID"].ToString();
        if (objBaseBo.Update(objSubs) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.btnSave.Enabled = true;
        if (deptid.Value != "100")//阳光新业
        {
            this.ClearTextBox();
            ViewState["DeptID"] = deptid.Value;
            this.GetStoreValueByID(ViewState["DeptID"].ToString());
        }
        else
        {
            this.ClearTextBox();
            this.btnSave.Enabled = false;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 获得一条记录
    /// </summary>
    /// <param name="strID"></param>
    private void GetStoreValueByID(string strID)
    {
        BaseBO objBaseBo = new BaseBO();
        Dept objDept = new Dept();
        Subs objSubs = new Subs();

        objBaseBo.WhereClause = "deptid=" + strID;
        Resultset rsd = objBaseBo.Query(new Dept());
        objDept = rsd.Dequeue() as Dept;

        objBaseBo.WhereClause = "subsId=" + strID;
        Resultset rs = objBaseBo.Query(new Subs());
        if (rs.Count != 0)
        {
            objSubs = rs.Dequeue() as Subs;
            this.txtCode.Text = objDept.DeptCode;//objSubs.SubsCode;
            this.txtCompanyName.Text = objSubs.SubsName;
            this.txtCompanyShortName.Text = objSubs.SubsShortName;
            this.txtBank.Text = objSubs.BankName;
            this.txtBankAccount.Text = objSubs.BankAcct;
            try { this.ddlFinType.SelectedValue = objSubs.FinancialTypeID.ToString(); }
            catch {  }
            this.ddlStatus.SelectedValue = objSubs.SubsStatus.ToString();
        }
        else
        {
            if (rsd.Count != 0)
            {
                objDept = rsd.Dequeue() as Dept;
                this.txtCode.Text = objDept.DeptCode;
                //this.txtCompanyName.Text = objDept.DeptName;
                this.txtCompanyShortName.Text = objDept.DeptName;
            }
        }
    }
    /// <summary>
    /// 清空控件中的值
    /// </sueckmmary>
    private void ClearTextBox()
    {
        this.txtCode.Text = "";
        this.txtCompanyName.Text = "";
        this.txtCompanyShortName.Text = "";
        this.txtBank.Text = "";
        this.txtBankAccount.Text = "";
        ViewState["DeptID"] = "";
        this.ddlStatus.SelectedValue = "1";
    }
    /// <summary>
    /// 新增
    /// </summary>
    private void SaveAdd()
    {
        BaseBO objBaseBo = new BaseBO();
        Subs objSubs = new Subs();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        objSubs.SubsID = Int32.Parse(ViewState["DeptID"].ToString());
        objSubs.SubsCode = this.txtCode.Text.Trim();
        objSubs.SubsName = this.txtCompanyName.Text.Trim();
        objSubs.SubsShortName = this.txtCompanyShortName.Text.Trim();
        try { objSubs.FinancialTypeID = Int32.Parse(this.ddlFinType.SelectedValue); }
        catch { objSubs.FinancialTypeID = 0; }
        try { objSubs.SubsStatus = Int32.Parse(this.ddlStatus.SelectedValue); }
        catch { objSubs.SubsStatus = 0; }
        objSubs.BankName = this.txtBank.Text.Trim();//银行
        objSubs.BankAcct = this.txtBankAccount.Text.Trim();//银行账号

        objSubs.CreateUserId = objSessionUser.CreateUserID;
        objSubs.CreateTime = DateTime.Now.Date;
        objSubs.ModifyUserId = objSessionUser.ModifyUserID;
        objSubs.ModifyTime = DateTime.Now.Date;
        objSubs.OprRoleID = objSessionUser.OprRoleID;
        objSubs.OprDeptID = objSessionUser.OprDeptID;

        if (objBaseBo.Insert(objSubs) != -1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
    }
}
