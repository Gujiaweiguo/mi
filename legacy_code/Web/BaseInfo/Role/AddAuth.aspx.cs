using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using Base.Biz;
using Base.DB;
using BaseInfo.User;
using Base.Page;
using BaseInfo.Dept;
using BaseInfo.Role;
using Base;

public partial class BaseInfo_Role_AddAuth : BasePage
{
    public string baseInfo;
    int numCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PalStore.Attributes.Add("OnDoubleClick", "SelectStore();");
        Session["DeptID"] = deptid.Value;
        selectdeptid.Value = Convert.ToString(Session["DeptID"]);

        string jsdept = "";

        BaseBO baseBO = new BaseBO();
        //baseBO.OrderBy = "deptid";
        //Resultset rs = baseBO.Query(new Dept());
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
        Resultset rs = baseBO.Query(objDept);
        if (rs.Count > 0)
        {

            foreach (Dept dept in rs)
            {
                jsdept += dept.DeptID + "|" + dept.PDeptID + "|" + dept.DeptName + "^";
            }
            depttxt.Value = jsdept;

        }
        #region
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnCancel.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCanceling.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnCancel.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnCancel.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
        //btnSave.Attributes.Add("onclick", "return allTextBoxValidator(form1)");
        #endregion
        if (!IsPostBack)
        {
            baseBO.WhereClause = "";
            baseBO.OrderBy = "";
            
            UserInfo userInfo = new UserInfo();
            baseBO.WhereClause = " a.UserID = b.UserID  and b.Deptid=12345678";
            try
            {
                page();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
            ViewState["userID"] = "";
            
        }
    }
    /// <summary>
    /// 绑定商业项目
    /// </summary>
    private void BindCheckBoxStoreList()
    {
        BaseBO objBaseBo = new BaseBO();
        Dept objDept = new Dept();
        string strSql = "select DeptID,DeptName from Dept  where depttype=6 and deptstatus=1";
        this.cblist.DataTextField = "DeptName";
        this.cblist.DataValueField = "DeptID";
        this.cblist.DataSource = objBaseBo.QueryDataSet(strSql);
        this.cblist.DataBind();
        if (this.lblStoreID.Text.Trim() != "")
        {
            string strStoreID = ","+this.lblStoreID.Text.Trim() + ",";
            for (int i = 0; i < this.cblist.Items.Count; i++)
            {
                if (strStoreID.IndexOf("," + this.cblist.Items[i].Value + ",") >= 0)
                    this.cblist.Items[i].Selected = true;
            }
        }
    }
    /// <summary>
    /// 绑定大楼
    /// </summary>
    private void BindCheckBoxBuildingList()
    {
        BaseBO objBaseBo = new BaseBO();
        Dept objDept = new Dept();
        string strSql = "select buildingid,buildingname from building  where storeid in ("+this.lblStoreID.Text.Trim()+")";
        this.ckbBuilding.DataTextField = "buildingname";
        this.ckbBuilding.DataValueField = "buildingid";
        this.ckbBuilding.DataSource = objBaseBo.QueryDataSet(strSql);
        this.ckbBuilding.DataBind();
        if (this.lblBuildingID.Text.Trim() != "")
        {
            string strBuildingID = "," + this.lblBuildingID.Text.Trim() + ",";
            for (int i = 0; i < this.ckbBuilding.Items.Count; i++)
            {
                if (strBuildingID.IndexOf("," + this.ckbBuilding.Items[i].Value + ",") >= 0)
                    this.ckbBuilding.Items[i].Selected = true;
            }
        }
    }
    /// <summary>
    /// 绑定楼层
    /// </summary>
    private void BindCheckBoxFloorsList()
    {
        this.ckbFloor.Items.Clear();
        BaseBO objBaseBo = new BaseBO();
        Dept objDept = new Dept();
        string strSql = "select floorid,floorname,(select buildingname from building where buildingid = floors.buildingid) as buildingname from floors  where floorstatus =1 and buildingid in (" + this.lblBuildingID.Text.Trim() + ")";
        DataSet ds = objBaseBo.QueryDataSet(strSql);
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            this.ckbFloor.Items.Add(new ListItem(ds.Tables[0].Rows[i]["buildingname"].ToString()+ds.Tables[0].Rows[i]["floorname"].ToString(), ds.Tables[0].Rows[i]["floorid"].ToString()));
        }
        if (this.lblFloor.Text.Trim() != "")
        {
            string strFloor = "," + this.lblFloor.Text.Trim() + ",";
            for (int i = 0; i < this.ckbFloor.Items.Count; i++)
            {
                if (strFloor.IndexOf("," + this.ckbFloor.Items[i].Value + ",") >= 0)
                    this.ckbFloor.Items[i].Selected = true;
            }
        }
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        UserInfo userInfo = new UserInfo();

        string deptId = "";

        deptId = deptid.Value;
        ViewState["DeptID"] = deptId;

        Session["DeptID"] = deptid.Value;
        page();
        foreach (GridViewRow grv in this.GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
        this.PalStore.Visible = false;
        this.PalBuilding.Visible = false;
        this.PalFloor.Visible = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        AuthShop authShop = new AuthShop();
        BaseBO baseBO = new BaseBO();
        BaseTrans baseTrans = new BaseTrans();
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];

        if (ViewState["userID"] != null && ViewState["userID"].ToString() != "")
        {
            if (rdoFloor.Checked)
            {
                if (baseBO.ExecuteUpdate("Delete AuthContract Where AuthContractID = '" + ViewState["userID"].ToString() + "'") < 0)
                {
                    return;
                }
                baseBO.WhereClause = "UserID=" + ViewState["userID"].ToString();
                baseBO.Delete(authShop);
                baseTrans.BeginTrans();
                
                if (this.lblFloor.Text.Trim() != "")
                {
                    string[] arrFloor = this.lblFloor.Text.Trim().Split(',');
                    for (int i = 0; i < arrFloor.Length; i++)
                    {
                        if (arrFloor[i].Trim() != "")
                        {
                            DataSet ds = baseBO.QueryDataSet("select buildingid,storeid from floors where floorid = " + arrFloor[i].Trim() + "");
                            authShop.AuthShopID = BaseApp.GetAuthShopID();
                            authShop.BuildingID = Convert.ToInt32(ds.Tables[0].Rows[0]["buildingid"].ToString());
                            authShop.FloorID = Convert.ToInt32(arrFloor[i].Trim());
                            authShop.UserID = Convert.ToInt32(ViewState["userID"]);
                            authShop.CreateUserID = sessionUser.UserID;
                            authShop.OprDeptID = sessionUser.DeptID;
                            authShop.OprRoleID = sessionUser.RoleID;
                            try { authShop.StoreID = Int32.Parse(ds.Tables[0].Rows[0]["storeid"].ToString()); }
                            catch { authShop.StoreID = 0; }
                            if (baseTrans.Insert(authShop) == -1)
                            {
                                return;
                            }
                        }
                    }
                }
                else if (this.lblBuildingID.Text.Trim() != "" && this.lblFloor.Text.Trim() == "")//选择项目、大楼
                {
                    string[] arrBuild = this.lblBuildingID.Text.Trim().Split(',');
                    for (int i = 0; i < arrBuild.Length; i++)
                    {
                        if (arrBuild[i].Trim() != "")
                        {
                            DataSet ds = baseBO.QueryDataSet("select buildingid,storeid,floorid from floors where buildingid = " + arrBuild[i].Trim() + "");
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                authShop.AuthShopID = BaseApp.GetAuthShopID();
                                authShop.BuildingID = Convert.ToInt32(arrBuild[i].Trim());
                                authShop.FloorID = Convert.ToInt32(ds.Tables[0].Rows[j]["floorid"].ToString());
                                authShop.UserID = Convert.ToInt32(ViewState["userID"]);
                                authShop.CreateUserID = sessionUser.UserID;
                                authShop.OprDeptID = sessionUser.DeptID;
                                authShop.OprRoleID = sessionUser.RoleID;
                                try { authShop.StoreID = Int32.Parse(ds.Tables[0].Rows[j]["storeid"].ToString()); }
                                catch { authShop.StoreID = 0; }
                                if (baseTrans.Insert(authShop) == -1)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                else if (this.lblStoreID.Text.Trim()!="" && this.lblBuildingID.Text.Trim() == "" && this.lblFloor.Text.Trim() == "")//只选择项目
                {
                    string[] arrStore = this.lblStoreID.Text.Trim().Split(',');
                    for (int i = 0; i < arrStore.Length; i++)
                    {
                        if (arrStore[i].Trim() != "")
                        {
                            DataSet ds = baseBO.QueryDataSet("select buildingid,storeid,floorid from floors where storeid = " + arrStore[i].Trim() + "");
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                authShop.AuthShopID = BaseApp.GetAuthShopID();
                                authShop.BuildingID = Convert.ToInt32(ds.Tables[0].Rows[j]["buildingid"].ToString());
                                authShop.FloorID = Convert.ToInt32(ds.Tables[0].Rows[j]["floorid"].ToString());
                                authShop.UserID = Convert.ToInt32(ViewState["userID"]);
                                authShop.CreateUserID = sessionUser.UserID;
                                authShop.OprDeptID = sessionUser.DeptID;
                                authShop.OprRoleID = sessionUser.RoleID;
                                try { authShop.StoreID = Int32.Parse(arrStore[i].ToString()); }
                                catch { authShop.StoreID = 0; }
                                if (baseTrans.Insert(authShop) == -1)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                baseTrans.Commit();
                //lstFloorName.Items.Clear();
                //Session["DeptID"] = "";
                //ddlBuildingName.SelectedIndex = 0;
                this.lblStoreID.Text = "";
                this.lblBuildingID.Text = "";
                this.lblFloor.Text = "";
            }
            else
            {
                int status = 1;
                AuthContract authContract = new AuthContract();
                if (rdoOnlyOne.Checked)
                {
                    status = AuthContract.AuthContractFlag_OnlyOne;
                }
                else if (rdoOnlyDept.Checked)
                {
                    status = AuthContract.AuthContractFlag_OnlyDept;
                }
                else
                {
                    status = AuthContract.AuthContractFlag_OnlyAll;
                }
                authContract.AuthContractID = BaseApp.GetAuthContractID();
                authContract.UserID = Convert.ToInt32(ViewState["userID"].ToString());
                authContract.AuthContractFlag = status;
                authContract.CreateUserID = sessionUser.UserID;
                authContract.OprDeptID = sessionUser.DeptID;
                authContract.OprRoleID = sessionUser.RoleID;
                if (baseBO.ExecuteUpdate("Delete AuthShop Where UserID = '" + ViewState["userID"].ToString() + "'") < 0)
                {
                    return;
                }
                baseTrans.BeginTrans();
                if (baseTrans.Insert(authContract) == -1)
                {
                    return;
                }
                baseTrans.Commit();
                //Session["DeptID"] = "";
            }
        }
        page();
        ViewState["userID"] = "";
        this.btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
        this.PalStore.Visible = false;
        this.PalBuilding.Visible = false;
        this.PalFloor.Visible = false;
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GrdUser.Rows)
        {
            if (gvr.Cells[1].Text == "&nbsp;")
            {
                gvr.Cells[5].Text = "";
            }
        }
        //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        //this.ddlBuildingName.SelectedIndex = 0;
        //this.ddlBuildingName_SelectedIndexChanged(sender, e);
        //this.rdoFloor.Checked = true;
        //this.rdoContract.Checked = false;
        Response.Redirect("~/BaseInfo/Role/AddAuth.aspx");
    }
    protected void GrdUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["userID"] = GrdUser.SelectedRow.Cells[0].Text;
        
        page();
        this.btnSave.Enabled = true;
        this.PalFloor.Visible = false;//隐藏楼层
        this.PalBuilding.Visible = false;//隐藏大楼
        this.PalStore.Visible = false;//隐藏项目
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }

    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        //Resultset rs = new Resultset();
        //PagedDataSource pds = new PagedDataSource();
        //int spareRow = 0;
        if (Session["DeptID"].ToString() == "")
        {
            baseBO.WhereClause = "a.userid=b.userid and b.deptid=" + 0;
        }
        else
        {
            baseBO.WhereClause = "a.userid=b.userid and b.deptid=" + Session["DeptID"];
        }

        baseBO.GroupBy = "a.userid,usercode,username,WorkNo,officetel,UserStatus";

        //DataTable dt = baseBO.QueryDataSet(new UserInfo()).Tables[0];

        UserInfo objUserInfo = new UserInfo();
        BaseInfo.BaseCommon.BindGridView(baseBO, objUserInfo, this.GrdUser);
        #region 
        //pds.DataSource = dt.DefaultView;

        //if (pds.Count < 1)
        //{
        //    for (int i = 0; i < GrdUser.PageSize; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdUser.DataSource = pds;
        //    GrdUser.DataBind();
        //}
        //else
        //{
        //    pds.AllowPaging = true;
        //    pds.PageSize = 12;
        //    lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
        //    pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
        //    if (pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = true;
        //    }

        //    if (pds.IsLastPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = false;
        //    }

        //    if (pds.IsFirstPage && pds.IsLastPage)
        //    {
        //        btnBack.Enabled = false;
        //        btnNext.Enabled = false;
        //    }
        //    if (!pds.IsLastPage && !pds.IsFirstPage)
        //    {
        //        btnBack.Enabled = true;
        //        btnNext.Enabled = true;
        //    }
        //    this.GrdUser.DataSource = pds;
        //    this.GrdUser.DataBind();
        //    spareRow = GrdUser.Rows.Count;
        //    for (int i = 0; i < pds.PageSize - spareRow; i++)
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //    }
        //    pds.DataSource = dt.DefaultView;
        //    GrdUser.DataSource = pds;
        //    GrdUser.DataBind();
        //}
        #endregion
    }
    public string SubStr(string sString, int nLeng)
    {
        if (sString.Length <= nLeng)
        {
            return sString;
        }
        string sNewStr = sString.Substring(0, nLeng);
        sNewStr = sNewStr + "...";
        return sNewStr;
    }
    protected void GrdUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string gIntro = "";
        if (e.Row.Cells.Count > 1)
        {
            if (e.Row.Cells[0].Text != "&nbsp;")
            {
                gIntro = e.Row.Cells[2].Text.ToString();
                e.Row.Cells[2].Text = SubStr(gIntro, 4);
                gIntro = e.Row.Cells[3].Text.ToString();
                e.Row.Cells[3].Text = SubStr(gIntro, 3);
            }
            else
            {
                e.Row.Cells[5].Text = "";
            }

            if (e.Row.Cells[4].Text.Length == 1)
            {
                if (e.Row.Cells[4].Text.Equals(Users.USER_STATUS_VALID.ToString()))
                {
                    e.Row.Cells[4].Text = (String)GetGlobalResourceObject("Parameter", "User_StatusEnabled");
                }
                else if (e.Row.Cells[4].Text.Equals(Users.USER_STATUS_LEAVE.ToString()))
                {
                    e.Row.Cells[4].Text = (String)GetGlobalResourceObject("Parameter", "User_StatusResigned");
                }
                else if (e.Row.Cells[4].Text.Equals(Users.USER_STATUS_FREEZE.ToString()))
                {
                    e.Row.Cells[4].Text = (String)GetGlobalResourceObject("Parameter", "User_StatusDisabled");
                }
            }
        }
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        
    }
    protected void rdoFloor_CheckedChanged(object sender, EventArgs e)
    {
        rdoAll.Enabled = false;
        rdoOnlyDept.Enabled = false;
        rdoOnlyOne.Enabled = false;
    }
    protected void GrdUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
        this.page();
        foreach (GridViewRow grv in this.GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
        this.btnSave.Enabled = false;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void cblist_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.cblist.Items.Count > 0)
        {
            this.lblStoreID.Text=","+this.lblStoreID.Text.TrimEnd(',').TrimStart(',')+",";
            foreach (ListItem item in this.cblist.Items)
            {
                string strStoreID = item.Value;
                if (item.Selected)
                {
                    if(this.lblStoreID.Text.IndexOf(","+strStoreID+",")<0)
                        this.lblStoreID.Text+=strStoreID+",";
                }
                else
                {
                    if (this.lblStoreID.Text.IndexOf("," + strStoreID + ",") >= 0)
                        this.lblStoreID.Text = this.lblStoreID.Text.Replace("," + strStoreID + ",", ",");
                }
            }
            this.lblStoreID.Text = this.lblStoreID.Text.TrimEnd(',').TrimStart(',');
        }
        this.page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnStore_Click(object sender, EventArgs e)
    {
        //if (this.PalStore.Visible == true)
        //    this.PalStore.Visible = false;
        //else
        //{
            this.BindCheckBoxStoreList();//绑定商业项目
            this.PalStore.Visible = true;
            this.PalBuilding.Visible = false;
            this.PalFloor.Visible = false;
        //}
        this.page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnBuilding_Click(object sender, EventArgs e)
    {
        if (this.lblStoreID.Text.Trim() != "")
        {
        //    if (this.PalBuilding.Visible == true)
        //        this.PalBuilding.Visible = false;
        //    else
        //    {
                this.BindCheckBoxBuildingList();//绑定大楼
 
                this.PalBuilding.Visible = true;
                this.PalStore.Visible = false;
                this.PalFloor.Visible = false;
            //}
        }
        this.page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void btnFloor_Click(object sender, EventArgs e)
    {
        if (this.lblBuildingID.Text.Trim() != "")
        {
            //if (this.PalFloor.Visible == true)
            //    this.PalFloor.Visible = false;
            //else
            //{
                this.BindCheckBoxFloorsList();//绑定楼层
                this.PalFloor.Visible = true;
                this.PalBuilding.Visible = false;
                this.PalStore.Visible = false;
            //}
        }
        this.page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void ckbBuilding_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ckbBuilding.Items.Count > 0)
        {
            this.lblBuildingID.Text = "," + this.lblBuildingID.Text.TrimEnd(',').TrimStart(',') + ",";
            foreach (ListItem item in this.ckbBuilding.Items)
            {
                string strBuildID = item.Value;
                if (item.Selected)
                {
                    if (this.lblBuildingID.Text.IndexOf("," + strBuildID + ",") < 0)
                        this.lblBuildingID.Text += strBuildID + ",";
                }
                else
                {
                    if (this.lblBuildingID.Text.IndexOf("," + strBuildID + ",") >= 0)
                        this.lblBuildingID.Text = this.lblBuildingID.Text.Replace("," + strBuildID + ",", ",");
                }
            }
            this.lblBuildingID.Text = this.lblBuildingID.Text.TrimEnd(',').TrimStart(',');
        }
        this.page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void ckbFloor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ckbFloor.Items.Count > 0)
        {
            this.lblFloor.Text = "," + this.lblFloor.Text.TrimEnd(',').TrimStart(',') + ",";
            foreach (ListItem item in this.ckbFloor.Items)
            {
                string strFloorID = item.Value;
                if (item.Selected)
                {
                    if (this.lblFloor.Text.IndexOf("," + strFloorID + ",") < 0)
                        this.lblFloor.Text += strFloorID + ",";
                }
                else
                {
                    if (this.lblFloor.Text.IndexOf("," + strFloorID + ",") >= 0)
                        this.lblFloor.Text = this.lblFloor.Text.Replace("," + strFloorID + ",", ",");
                }
            }
            this.lblFloor.Text = this.lblFloor.Text.TrimEnd(',').TrimStart(',');
        }
        this.page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
}
