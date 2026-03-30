using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


using BaseInfo.Dept;
using Base.Biz;
using BaseInfo.User;
using Base.DB;
using Base;
using Base.Page;
public partial class DeptTree : BasePage
{
    /**
     * 存放在ViewState里,用来标志当前的操作
     */
    private static String OPR_ADD = "Add";
    private static String OPR_EDIT = "Edit";
    public string baseInfo;

    private void Page_Load(object sender, System.EventArgs e)
    {
        Page.Response.Buffer = false;
        Page.Response.Cache.SetNoStore();
        btnAdd.Focus();
        ViewState["DeptID"] = deptid.Value;
        selectdeptid.Value = Convert.ToString(ViewState["DeptID"]);
        showtree();
        this.btnAdd.Enabled = false;
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "a1", "styletabbar_atv()", true);
        if (!IsPostBack)
        {
            #region
            //btnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
            //btnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
            //btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
            //btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
            //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
            //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
            //btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
            //btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");


            //绑定部门
            //int[] status = Dept.GetDeptType();
            //for (int i = 0; i < status.Length; i++)
            //{
            //    ddlstDeptType.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Dept.GetDeptTypeDesc(status[i])), status[i].ToString()));
            //}
            #endregion
            //绑定部门，只绑定下级部门 edit by lcp at 2009-
            BaseInfo.BaseCommon.BindDropDownList("select DeptType,DeptTypeName from DeptType", "DeptType", "DeptTypeName", this.ddlstDeptType);
            //
            //

            int[] statusIndepBalance = Dept.GetIndepBalanceStatus();
            for (int i = 0; i < statusIndepBalance.Length; i++)
            {
                cmbIndepBalance.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Dept.GetIndepBalanceStatusDesc(statusIndepBalance[i])), statusIndepBalance[i].ToString()));
            }

            int[] deptStatus = Dept.GetDeptStatus();
            for (int i = 0; i < deptStatus.Length; i++)
            {
                cmbDeptStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Dept.GetDeptStatusDesc(deptStatus[i])), deptStatus[i].ToString()));
            }

            //rbtnAllShop.Attributes.Add("onclick", "allShopChange()");
            //rbtnShop.Attributes.Add("onclick", "ShopChange()");

            //rbtnAllTreaty.Attributes.Add("onclick", "AllTreaty()");
            //rbtnTreaty.Attributes.Add("onclick", "Treaty()");

            //rbtnAllVocation.Attributes.Add("onclick", "AllVocation()");
            //rbtnVocation.Attributes.Add("onclick", "Vocation()");

            //rbtnNoRrestrict.Attributes.Add("onclick", "NoRrestrict()");
            //rbtnRrestrict.Attributes.Add("onclick", "Rrestrict()");

            btnSave.Attributes.Add("onclick", "return allTextBoxValidator(form1)");
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
    }
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        String oprFlag = Convert.ToString(ViewState["DeptID"]);
        if (oprFlag != null && Convert.ToString(oprFlag) != "")
        {
            //绑定部门，只绑定下级部门 edit by lcp 
            BaseBO objBaseBo = new BaseBO();
            DataSet ds = objBaseBo.QueryDataSet("select DeptType from dept where deptid='" + oprFlag + "'");
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {

                BaseInfo.BaseCommon.BindDropDownList("select DeptType,DeptTypeName from DeptType where DeptType >= '" + Int32.Parse(ds.Tables[0].Rows[0]["DeptType"].ToString()) + "'", "DeptType", "DeptTypeName", this.ddlstDeptType);
            }
            //
            DataSet dsDept = objBaseBo.QueryDataSet("select depttype from dept where deptid='" + oprFlag + "'");
            try { this.ddlstDeptType.SelectedValue = dsDept.Tables[0].Rows[0]["depttype"].ToString(); }
            catch { }
            ViewState["Flag"] = OPR_EDIT;
            textupdate();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "parent.document.all.txtWroMessage.value = '" + hidSelect.Value + "'", true);
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        textlock();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        BaseBO baseBO = new BaseBO();
        Dept objDept = new Dept();
        DeptAuth objDeptAuth = new DeptAuth();
        BaseTrans trans = new BaseTrans();
        String oprFlag = Convert.ToString(ViewState["Flag"]);
        #region 
        //if (rbtnShop.Checked)
        //{
        //    if (txtConcessionAuth.Text == null || txtConcessionAuth.Text == "")
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidCondition.Value + "'", true);
        //        txtConcessionAuth.Focus();
        //        return;
        //    }
        //}

        //if (rbtnTreaty.Checked)
        //{
        //    if (txtContractAuth.Text == null || txtContractAuth.Text=="")
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidCondition.Value + "'", true);
        //        txtContractAuth.Focus();
        //        return;
        //    }
        //}

        //if (rbtnVocation.Checked)
        //{
        //    if (txtTradeAuth.Text == null || txtTradeAuth.Text == "")
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidCondition.Value + "'", true);
        //        txtTradeAuth.Focus();
        //        return;
        //    }
        //}

        //if (rbtnRrestrict.Checked)
        //{
        //    if (txtOtherAuth.Text == null || txtOtherAuth.Text == "")
        //    {
        //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidCondition.Value + "'", true);
        //        txtOtherAuth.Focus();
        //        return;
        //    }
        //}
        #endregion
        if (oprFlag == null || Convert.ToString(oprFlag) == ""){

            return;
        }
        if(oprFlag.Equals(OPR_ADD))
        {
            baseBO.WhereClause = "DeptCode='" + txtDeptCode.Text.Trim() + "'";
            Resultset rs = baseBO.Query(objDept);
            if (rs.Count >= 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidUserBeing.Value + "'", true);
                return;
            }
            trans.BeginTrans();
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            /**
             * --初始化添加数据--
             */
            objDept.CreateUserId = objSessionUser.UserID;
            objDept.CreateTime = DateTime.Now;

            objDept.DeptID = BaseApp.GetDeptID();
            objDept.DeptCode = txtDeptCode.Text.Trim();
            objDept.DeptName = txtDeptName.Text.Trim();
            objDept.PDeptID = Convert.ToInt32(ViewState["DeptID"].ToString());
            objDept.DeptLevel = Convert.ToInt32(txtDeptLevel.Text);
            objDept.DeptType = Convert.ToInt32(ddlstDeptType.SelectedValue);
            objDept.IndepBalance = Convert.ToInt32(cmbIndepBalance.SelectedValue);           //测试用值
            objDept.DeptStatus = Convert.ToInt32(cmbDeptStatus.SelectedValue.ToString());
            try { objDept.OrderID = Int32.Parse(this.txtOrderID.Text.Trim()); }
            catch { objDept.OrderID = 0; }


            objDeptAuth.DeptAuthID = BaseApp.GetDeptAuthID();
            objDeptAuth.DeptID = objDept.DeptID;
            //objDeptAuth.ConcessionAuth = txtConcessionAuth.Text.Trim().ToString();
            //objDeptAuth.ContractAuth = txtContractAuth.Text.Trim().ToString();
            //objDeptAuth.TradeAuth = txtTradeAuth.Text.Trim().ToString();
            //objDeptAuth.OtherAuth = txtOtherAuth.Text.Trim().ToString();
            //objDeptAuth.DeptAuthName = "Oasis";
           
            if (trans.Insert(objDept) < 1)
            {
                trans.Rollback();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidInsert.Value + "'", true);
                return;
            }

            //if (trans.Insert(objDeptAuth) < 1)
            //{
            //    trans.Rollback();
            //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidInsert.Value + "'", true);
            //    return;
            //}

            trans.Commit();
            this.btnAdd.Enabled = false;
           
            ViewState["Flag"] = "";
            textlock();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "parent.document.all.txtWroMessage.value =  '" + hidAdd.Value + "'", true);
        }
        else if (oprFlag.Equals(OPR_EDIT))
        {
            /**
             * --初始化添加数据--
             */
            int deptid = Convert.ToInt32(ViewState["DeptID"]);
            trans.BeginTrans();
            SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
            objDept.DeptCode = txtDeptCode.Text.Trim();
            objDept.DeptName = txtDeptName.Text.Trim();
            objDept.DeptType = Convert.ToInt32(ddlstDeptType.SelectedValue);
            objDept.DeptStatus = Convert.ToInt32(cmbDeptStatus.SelectedValue.ToString());
            objDept.ModifyTime = DateTime.Now;
            objDept.ModifyUserID = sessionUser.UserID;
            objDept.OprDeptID = sessionUser.DeptID;
            objDept.OprRoleID = sessionUser.RoleID;
            objDept.IndepBalance = Convert.ToInt32(cmbIndepBalance.SelectedValue);
            try { objDept.OrderID = Int32.Parse(this.txtOrderID.Text.Trim()); }
            catch { objDept.OrderID = 0; }
            trans.WhereClause = "deptid ='" + deptid + "'";

            if (trans.Update(objDept) < 1)
            {
                trans.Rollback();
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidInsert.Value + "'", true);
                return;
            }
            if (this.cmbDeptStatus.SelectedValue == "0")
            {
                string strSql = "update Dept set deptstatus=0 where PDeptID='" + ViewState["DeptID"].ToString() + "'";
                trans.ExecuteUpdate(strSql);
            }
            //trans.WhereClause = "deptid ='" + deptid + "'";
            //objDeptAuth.ConcessionAuth = txtConcessionAuth.Text.Trim();
            //objDeptAuth.ContractAuth = txtContractAuth.Text.Trim();
            //objDeptAuth.TradeAuth = txtTradeAuth.Text.Trim();
            //objDeptAuth.OtherAuth = txtOtherAuth.Text.Trim();
            //if (deptid.ToString() != "100")
            //{
            //    if (trans.Update(objDeptAuth) < 1)
            //    {
            //        trans.Rollback();
            //        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + hidInsert.Value + "'", true);
            //        return;
            //    }
            //}
            trans.Commit();
            this.btnAdd.Enabled = false;
            ViewState["Flag"] = "";
            textlock();
        }
        showtree();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
   
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        String oprFlag = Convert.ToString(ViewState["DeptID"]);
        if (oprFlag != null && Convert.ToString(oprFlag) != "")
        {
            string sqlStr = "select DeptType,DeptTypeName from DeptType where DeptType > (select depttype from dept where deptid='" + oprFlag + "')";
            BaseBO basebo = new BaseBO();
            DataSet ds = basebo.QueryDataSet(sqlStr);
            ddlstDeptType.DataSource = ds.Tables[0].DefaultView;
            ddlstDeptType.DataValueField = "DeptType";
            ddlstDeptType.DataTextField = "DeptTypeName";
            ddlstDeptType.DataBind();

            textopen();

            txtDeptLevel.Text = Convert.ToString(Convert.ToInt32(txtDeptLevel.Text) + 1);
            ViewState["Flag"] = "Add";
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '请先选择部门。'", true);
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void deptid_ValueChanged(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        BaseBO baseBo = new BaseBO();
        Dept dept = new Dept();
        DeptBO deptBO = new DeptBO();
        DeptInfo objDeptInfo = new DeptInfo();
        string deptId = "";
        TreeNode Node = new TreeNode();

        deptId = deptid.Value;
       
       ViewState["DeptID"] = deptId;
       this.btnAdd.Enabled = true;

       //绑定部门，只绑定下级部门 edit by lcp 
       BaseBO objBaseBo = new BaseBO();
       DataSet ds = objBaseBo.QueryDataSet("select DeptType from dept where deptid='" + deptId + "'");
       if (ds != null && ds.Tables[0].Rows.Count == 1)
       {

           BaseInfo.BaseCommon.BindDropDownList("select DeptType,DeptTypeName from DeptType where DeptType >= '" + Int32.Parse(ds.Tables[0].Rows[0]["DeptType"].ToString()) + "'", "DeptType", "DeptTypeName", this.ddlstDeptType);
       }
       //


       deptBO.WhereClause = "DeptID =" + deptId;
       Resultset rs = deptBO.Query(objDeptInfo);
        if (rs.Count == 1)
        {
            objDeptInfo = rs.Dequeue() as DeptInfo;
            //if (objDeptInfo.DeptType != Dept.DEPT_TYPE_MALL)
            //{
            //DataSet dst = objBaseBo.QueryDataSet("select depttype from dept where deptid='" + deptId + "'");
            //int a = Convert.ToInt32(dst.Tables[0].Rows[0][0]);
            ddlstDeptType.SelectedValue =
                    objDeptInfo.DeptType.ToString(); //显示部门            
            //}
            txtDeptCode.Text = objDeptInfo.DeptCode;
            txtDeptName.Text = objDeptInfo.DeptName;
            treetext.Text = objDeptInfo.DeptName;
            cmbDeptStatus.SelectedValue = objDeptInfo.DeptStatus.ToString();
            txtDeptLevel.Text = Convert.ToString(objDeptInfo.DeptLevel);
            this.txtOrderID.Text = objDeptInfo.OrderID.ToString().Trim();
            #region
            //if (objDeptInfo.ConcessionAuth.ToString() == "" || objDeptInfo.ConcessionAuth.ToString()==null)
            //{
            //    rbtnAllShop.Checked = true;
            //    rbtnShop.Checked = false;
            //    txtConcessionAuth.Text = "";
            //}
            //else
            //{
            //    rbtnAllShop.Checked = false;
            //    rbtnShop.Checked = true;
            //    txtConcessionAuth.Text = objDeptInfo.ConcessionAuth;
            //}
            //if (objDeptInfo.ContractAuth.ToString() == "" || objDeptInfo.ContractAuth.ToString() == null)
            //{
            //    rbtnAllTreaty.Checked = true;
            //    rbtnTreaty.Checked = false;
            //    txtContractAuth.Text = "";
            //}
            //else
            //{
            //    rbtnAllTreaty.Checked = false;
            //    rbtnTreaty.Checked = true;
            //    txtContractAuth.Text = objDeptInfo.ContractAuth;
            //}
            //if (objDeptInfo.TradeAuth.ToString() == "" || objDeptInfo.TradeAuth.ToString() == null)
            //{
            //    rbtnAllVocation.Checked = true;
            //    rbtnVocation.Checked = false;
            //    txtTradeAuth.Text = "";
            //}
            //else
            //{
            //    rbtnAllVocation.Checked = false;
            //    rbtnVocation.Checked = true;
            //    txtTradeAuth.Text = objDeptInfo.TradeAuth;
            //}
            //if (objDeptInfo.OtherAuth.ToString() == "" || objDeptInfo.OtherAuth.ToString() == null)
            //{
            //    rbtnNoRrestrict.Checked = true;
            //    rbtnRrestrict.Checked = false;
            //    txtOtherAuth.Text = "";
            //}
            //else
            //{
            //    rbtnNoRrestrict.Checked = false;
            //    rbtnRrestrict.Checked = true;
            //    txtOtherAuth.Text = objDeptInfo.OtherAuth;
            //}
            #endregion
            cmbIndepBalance.SelectedValue = objDeptInfo.IndepBalance.ToString();
        }
        else if (Convert.ToInt32(deptId) == 100)
        {
            baseBo.WhereClause = "DeptID=" + deptId;
            baseBo.OrderBy = "DeptID";
            rs = baseBo.Query(dept);
            if (rs.Count == 1)
            {
                dept = rs.Dequeue() as Dept;
                txtDeptCode.Text = dept.DeptCode;
                txtDeptName.Text = dept.DeptName;
                treetext.Text = dept.DeptName;
                cmbDeptStatus.SelectedValue = dept.DeptStatus.ToString();
                txtDeptLevel.Text = Convert.ToString(dept.DeptLevel);
            }
        }
        btnAdd.Enabled = true;
        DataSet dsType=objBaseBo.QueryDataSet("select depttype from dept where deptid='" + deptId + "'");
        if(Convert.ToInt32(dsType.Tables[0].Rows[0][0])==7)
        {
            btnAdd.Enabled = false;
        }
        txtDeptCode.ReadOnly = true;
        txtDeptName.ReadOnly = true;
        //txtConcessionAuth.ReadOnly = true;
        //txtContractAuth.ReadOnly = true;
        //txtTradeAuth.ReadOnly = true;
        //txtOtherAuth.ReadOnly = true;
        ddlstDeptType.Enabled = false;
        cmbIndepBalance.Enabled = false;
        cmbDeptStatus.Enabled = false;
        btnSave.Enabled = false;
        //btnAdd.Enabled = true;
        btnEdit.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "mess", "parent.document.all.txtWroMessage.value = ''", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    protected void rbtnAllShop_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void textlock()
    {
        txtDeptCode.ReadOnly = true;
        txtDeptCode.CssClass = "Enabledipt160px";
        txtDeptName.ReadOnly = true;
        txtDeptName.CssClass = "Enabledipt160px";
        //txtConcessionAuth.ReadOnly = true;
        //txtConcessionAuth.Text = "";
        //txtConcessionAuth.CssClass = "EnabledColor";
        //txtContractAuth.ReadOnly = true;
        //txtContractAuth.Text = "";
        //txtContractAuth.CssClass = "EnabledColor";
        //txtTradeAuth.ReadOnly = true;
        //txtTradeAuth.Text = "";
        //txtTradeAuth.CssClass = "EnabledColor";
        //txtOtherAuth.ReadOnly = true;
        //txtOtherAuth.Text = "";
        //txtOtherAuth.CssClass = "EnabledColor";
        txtDeptCode.Text = "";
        txtDeptName.Text = "";
        txtOrderID.ReadOnly = true;
        txtOrderID.CssClass = "Enabledipt160px";
        this.txtOrderID.Text = "";
        ddlstDeptType.Enabled = false;
        cmbIndepBalance.Enabled = false;
        cmbDeptStatus.Enabled = false;
        btnSave.Enabled = false;
        btnAdd.Enabled = true;
        btnEdit.Enabled = false;
        //rbtnAllShop.Enabled = false;
        //rbtnAllTreaty.Enabled = false;
        //rbtnAllVocation.Enabled = false;
        //rbtnNoRrestrict.Enabled = false;
        //rbtnRrestrict.Enabled = false;
        //rbtnShop.Enabled = false;
        //rbtnTreaty.Enabled = false;
        //rbtnVocation.Enabled = false;
        treetext.Text = "";
        //rbtnAllShop.Checked = false;
        //rbtnShop.Checked = true;
        //rbtnAllTreaty.Checked = false;
        //rbtnTreaty.Checked = true;
        //rbtnAllVocation.Checked = false;
        //rbtnVocation.Checked = true;
        //rbtnNoRrestrict.Checked = false;
        //rbtnRrestrict.Checked = true;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = ''", true);
    }

    private void textopen()
    {
        txtDeptCode.ReadOnly = false;
        txtDeptCode.CssClass = "ipt160px";
        txtDeptName.ReadOnly = false;
        txtDeptName.CssClass = "ipt160px";
        this.txtOrderID.ReadOnly = false;
        txtOrderID.CssClass = "ipt160px";
        //txtConcessionAuth.ReadOnly = false;
        //txtConcessionAuth.Text = "";
        //txtConcessionAuth.CssClass = "OpenColor";

        //txtContractAuth.ReadOnly = false;
        //txtContractAuth.Text = "";
        //txtContractAuth.CssClass = "OpenColor";

        //txtTradeAuth.ReadOnly = false;
        //txtTradeAuth.Text = "";
        //txtTradeAuth.CssClass = "OpenColor";

        //txtOtherAuth.ReadOnly = false;
        //txtOtherAuth.Text = "";
        //txtOtherAuth.CssClass = "OpenColor";

        txtDeptCode.Text = "";
        txtDeptName.Text = "";

        ddlstDeptType.Enabled = true;
        cmbIndepBalance.Enabled = true;
        cmbDeptStatus.Enabled = true;
        btnSave.Enabled = true;
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;

        //rbtnAllShop.Enabled = true;
        //rbtnAllTreaty.Enabled = true;
        //rbtnAllVocation.Enabled = true;
        //rbtnNoRrestrict.Enabled = true;
        //rbtnRrestrict.Enabled = true;
        //rbtnShop.Enabled = true;
        //rbtnTreaty.Enabled = true;
        //rbtnVocation.Enabled = true;
        //rbtnAllShop.Checked = true;
        //rbtnShop.Checked = false;
        //rbtnAllTreaty.Checked = true;
        //rbtnTreaty.Checked = false;
        //rbtnAllVocation.Checked = true;
        //rbtnVocation.Checked = false;
        //rbtnNoRrestrict.Checked = true;
        //rbtnRrestrict.Checked = false;
    }
    private void textupdate()
    {
        txtDeptCode.ReadOnly = false;
        txtDeptCode.CssClass = "ipt160px";
        txtDeptName.ReadOnly = false;
        txtDeptName.CssClass = "ipt160px";
        txtOrderID.ReadOnly = false;
        txtOrderID.CssClass = "ipt160px";
        //txtConcessionAuth.ReadOnly = false;
        //txtConcessionAuth.CssClass = "OpenColor";
        //txtContractAuth.ReadOnly = false;
        //txtContractAuth.CssClass = "OpenColor";
        //txtTradeAuth.ReadOnly = false;
        //txtTradeAuth.CssClass = "OpenColor";
        //txtOtherAuth.ReadOnly = false;
        //txtOtherAuth.CssClass = "OpenColor";
        ddlstDeptType.Enabled = true;
        cmbIndepBalance.Enabled = true;
        cmbDeptStatus.Enabled = true;
        //rbtnAllShop.Enabled = true;
        //rbtnAllTreaty.Enabled = true;
        //rbtnAllVocation.Enabled = true;
        //rbtnNoRrestrict.Enabled = true;
        //rbtnRrestrict.Enabled = true;
        //rbtnShop.Enabled = true;
        //rbtnTreaty.Enabled = true;
        //rbtnVocation.Enabled = true;
        btnSave.Enabled = true;
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
    }

    private void showtree()
    {
        string jsdept = "";

        BaseBO baseBo = new BaseBO();
        string strSql = @"SELECT 
		CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,OrderID
FROM 
		Dept
 Group  By PDeptID,CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,OrderID
 ORDER BY Pdeptid,isnull(orderid,0) ";
        Dept objDept = new Dept();
        objDept.SetQuerySql(strSql);
        Resultset rs = baseBo.Query(objDept);
        if (rs.Count > 0)
        {

            foreach (Dept dept in rs)
            {
                if (dept.DeptStatus == Dept.DEPTSTATUS_INVALID)
                {
                    jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                }
                else
                {
                    jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "" + "^";
                }
            }
            depttxt.Value = jsdept;
        }
    }
    protected void ddlstDeptType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}
