using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Base.Biz;
using Base.DB;
using RentableArea;
using BaseInfo.Dept;
using Base;
using Base.Page;
using BaseInfo.User;
using BaseInfo.authUser;

public partial class AreaVindicate : BasePage
{   
      /**
     * 存放在session里,用来标志当前的操作
     */
    private static String OPR_ADD = "Add";
    private static String OPR_EDIT = "Edit";
    public string baseInfo;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        showtreenode();
        #region
        //btnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnEdit.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnEdit.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/btnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
        #endregion
        btnSave.Attributes.Add("onclick", "return allTextBoxValidator(form1)");
        if (!IsPostBack)
        {
            int[] status = Area.GetAreaStatus();
            for (int i = 0; i < status.Length; i++)
            {
                this.cmbStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter",Area.GetAreaStatusDesc(status[i])), status[i].ToString()));
            }
        }
        this.btnEdit.Enabled = false;
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "AreaVindicate_labAreaVindicate");
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }
    /// <summary>
    /// 检测编码是否唯一 add by lcp at 2009-4-27
    /// </summary>
    /// <returns></returns>
    private bool CheckCode()
    {
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "AreaCode='" + this.txtAreaCode.Text.Trim() + "'";
        DataSet ds = objBaseBo.QueryDataSet(new Area());
        if (ds != null && ds.Tables[0].Rows.Count > 0)
            return true;
        else
            return false;
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Area areaInfo = new Area();
        BaseBO baseBO = new BaseBO();
        String oprFlag = Convert.ToString(Session["Flag"]);
        int result = 0;
        if (oprFlag == null || Convert.ToString(oprFlag) == ""){
            return;
        }
        if (oprFlag.Equals(OPR_ADD))
        {
            if (this.CheckCode())
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + "经营区域编码重复。" + "'", true);
                return;
            }
            areaInfo.AreaID = BaseApp.GetAreaID();
            areaInfo.AreaCode = this.txtAreaCode.Text.Trim();
            areaInfo.AreaName = this.txtAreaName.Text.Trim();
            areaInfo.Note = this.txtNote.Text.Trim();
            areaInfo.AreaStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            try { areaInfo.StoreID = Int32.Parse(ViewState["StoreID"].ToString()); }
            catch { areaInfo.StoreID = 0; }
            if(baseBO.Insert(areaInfo)<1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                return;
            }
            textlock();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        }
        else
        {
            areaInfo.AreaCode = this.txtAreaCode.Text.Trim();
            areaInfo.AreaName = this.txtAreaName.Text.Trim();
            areaInfo.Note = this.txtNote.Text.Trim();
            areaInfo.AreaStatus = Convert.ToInt32(cmbStatus.SelectedValue);
            try { areaInfo.StoreID = Int32.Parse(ViewState["StoreID"].ToString()); }
            catch { areaInfo.StoreID = 0; }
            baseBO.WhereClause = "AreaID=" + Convert.ToInt32(Session["AreaID"].ToString());
            if (baseBO.Update(areaInfo) < 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
                return;
            }
            textlock();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
        }
        btnEdit.Enabled = false;
        titleArea.Text = "";
        showtreenode();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        textlock();
        this.btnEdit.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        String oprFlag = Convert.ToString(Session["AreaID"]);
        if (oprFlag == null || Convert.ToString(oprFlag) == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAreaNotSelect.Value + "'", true);
            return;
        }

        txtAreaCode.ReadOnly = false;
        txtAreaCode.CssClass = "ipt160px";
        txtAreaName.ReadOnly = false;
        txtAreaName.CssClass = "ipt160px";
        txtNote.ReadOnly = false;
        txtNote.CssClass = "OpenColor";
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        cmbStatus.Enabled = true;
        Session["Flag"] = OPR_EDIT;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        textopen();
        titleArea.Text = hidAddArea.Value;
        Session["Flag"] = "Add";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    protected void treeClick_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        string areaid = "";
        Area area = new Area();
        areaid = deptid.Value;
        if (baseBO.QueryDataSet("select deptid from dept where deptid ='" + areaid.Substring(0, 3) + "' and depttype=6").Tables[0].Rows.Count>0)
        {
            ViewState["StoreID"] = deptid.Value.ToString().Substring(0, 3);
            if (areaid.Length > 3)
            {
                Session["AreaID"] = deptid.Value.Substring(3);

                baseBO.WhereClause = "AreaID=" + areaid.Substring(3);
                Resultset rs = baseBO.Query(area);
                if (rs.Count == 1)
                {
                    area = rs.Dequeue() as Area;
                    txtAreaCode.Text = area.AreaCode;
                    txtAreaName.Text = area.AreaName;
                    txtNote.Text = area.Note;
                    titleArea.Text = area.AreaName;
                    cmbStatus.SelectedValue = area.AreaStatus.ToString();
                }
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnSave.Enabled = false;
            }
            else
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = false;
                btnSave.Enabled = false;
            }
        txtAreaCode.ReadOnly = true;
        txtAreaCode.CssClass = "Enabledipt160px";
        txtAreaName.ReadOnly = true;
        txtAreaName.CssClass = "Enabledipt160px";
        txtNote.ReadOnly = true;
        txtNote.CssClass = "EnabledColor";
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }

    private void textlock()
    {
        txtAreaCode.Text = "";
        txtAreaCode.ReadOnly = true;
        txtAreaCode.CssClass = "Enabledipt160px";
        txtAreaName.Text = "";
        txtAreaName.ReadOnly = true;
        txtAreaName.CssClass = "Enabledipt160px";
        txtNote.Text = "";
        txtNote.ReadOnly = true;
        txtNote.CssClass = "EnabledColor";
        cmbStatus.SelectedValue = Area.AREA_STATUS_VALID.ToString();
        cmbStatus.Enabled = false;
        btnAdd.Enabled = true;
        btnEdit.Enabled = true;
        btnSave.Enabled = false;
    }

    private void textopen()
    {
        txtAreaCode.Text = "";
        txtAreaCode.ReadOnly =false;
        txtAreaCode.CssClass = "ipt160px";
        txtAreaName.Text = "";
        txtAreaName.ReadOnly = false;
        txtAreaName.CssClass = "ipt160px";
        txtNote.Text = "";
        txtNote.CssClass = "OpenColor";
        txtNote.ReadOnly = false;
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        cmbStatus.SelectedValue = Area.AREA_STATUS_VALID.ToString();
        cmbStatus.Enabled = true;
    }

    private void showtreenode()
    {
        #region
        //string jsdept = "";
        //BaseBO baseBO = new BaseBO();
        //BaseBO baseareaBO = new BaseBO();
        //Resultset rs = new Resultset();

        //Dept dept = new Dept();

        //baseBO.WhereClause = "DeptType=" + Dept.DEPT_TYPE_CHILD_COMPANY;

        //rs = baseBO.Query(dept);

        //if (rs.Count == 1)
        //{
        //    dept = rs.Dequeue() as Dept;
        //    jsdept = dept.DeptID + "|" + "0" + "|" + dept.DeptName + "|" + "^";
        //}
        //else
        //{
        //    return;
        //}

        //rs = baseareaBO.Query(new Area());
        //if (rs.Count > 0)
        //{
        //    foreach (Area area in rs)
        //    {
        //        if (area.AreaStatus == Area.AREA_STATUS_INVALID)
        //        {
        //            jsdept += area.AreaID + "|" + dept.DeptID + "|" + area.AreaName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
        //        }
        //        else
        //        {
        //            jsdept += area.AreaID + "|" + dept.DeptID + "|" + area.AreaName + "|" + "" + "^";
        //        }
        //    }
        //    depttxt.Value = jsdept;
        //}
        #endregion
        string jsdept = "";
        BaseBO baseBo = new BaseBO();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        string strSql = @"SELECT 
		CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,isnull(OrderID,0) as OrderID
FROM 
		Dept where DeptType in (1,3,4,5) 
union
SELECT 
		CreateUserId,CreateTime,DeptID,DeptCode,
		DeptName,DeptLevel,PDeptID,DeptType,City,
		RegAddr,OfficeAddr,PostAddr,PostCode,Tel,
		OfficeTel,Fax,DeptStatus,IndepBalance,isnull(OrderID,0) as OrderID
FROM 
		Dept where DeptType=6";
        if (AuthBase.GetAuthUser(sessionUser.UserID) > 0)
        {
            strSql += " and EXISTS (SELECT storeID FROM authUser WHERE  dept.deptID = authUser.storeID AND userID =" + sessionUser.UserID + ")";
        }
        strSql += " ORDER BY Pdeptid";
        
        Dept objDept = new Dept();
        objDept.SetQuerySql(strSql);
        Resultset rs = baseBo.Query(objDept);
        if (rs.Count > 0)
        {
            foreach (Dept dept in rs)
            {
                if(dept.DeptType!=6)
                {
                    if (baseBo.QueryDataSet("select deptid  from dept where pdeptid='" + dept.DeptID + "'").Tables[0].Rows.Count > 0)
                    {
                        jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "" + "^";
                    }
                }
                else
                {
                    jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "|" + "" + "^";
                    baseBo.WhereClause = "StoreID='" + dept.DeptID.ToString() + "'";
                    Resultset rsArea = baseBo.Query(new Area());
                    if (rsArea.Count > 0)
                    {
                        foreach (Area area in rsArea)
                        {
                            if (area.AreaStatus == Area.AREA_STATUS_INVALID)
                            {
                                jsdept += dept.DeptID.ToString() + area.AreaID.ToString() + "|" + dept.DeptID + "|" + area.AreaName + "|" + "../../App_Themes/nlstree/img/node3.gif" + "^";
                            }
                            else
                            {
                                jsdept += dept.DeptID.ToString() + area.AreaID.ToString() + "|" + dept.DeptID + "|" + area.AreaName + "|" + "" + "^";
                            }
                        }
                    }
                }
            }
            depttxt.Value = jsdept;
        }
    }
}
