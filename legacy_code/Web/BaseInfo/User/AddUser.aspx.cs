using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using BaseInfo.User;
using Base.Page;
using Base.Biz;
using Base.DB;
using BaseInfo.Dept;
using BaseInfo.Role;
using Base.Sys;

public partial class BaseInfo_User_AddUser : BasePage
{
    public string baseInfo;
    int numCount = 0;
     protected void Page_Load(object sender, EventArgs e)
     {

         //btnBack.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnbacking.gif) no-repeat left top';this.style.fontWeight='bold';");
         //btnBack.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnback.gif) no-repeat left top';this.style.fontWeight='normal';");
         //btnNext.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnexting.gif) no-repeat left top';this.style.fontWeight='bold';");
         //btnNext.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/Btnnext.gif) no-repeat left top';this.style.fontWeight='normal';");
         //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
         //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
         btnSave.Attributes.Add("onclick", "return allTextBoxValidator(form1)");
         Session["DeptID"] = deptid.Value;
         selectdeptid.Value = Convert.ToString(Session["DeptID"]);

         string jsdept = "";

         BaseBO baseBo = new BaseBO();
         //baseBo.OrderBy = "deptid";
         //baseBo.WhereClause = "deptstatus = 1";
         //Resultset rs = baseBo.Query(new Dept());
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

         if (!IsPostBack)
        {
            BaseBO baseBO = new BaseBO();
            int[] status = Users.GetUserStatus();
            foreach (int sta in status)
            {
                this.cmbUserState.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", Users.GetUserStautsDesc(sta)), sta.ToString()));
            }
            baseBO.WhereClause = "RoleStatus=" + Role.IS_ROLESTATUS_YES;
             rs = baseBO.Query(new Role());
            foreach (Role roles in rs)
            {
                lstRoleName.Items.Add(new ListItem(roles.RoleName, roles.RoleID.ToString()));
            }
            try
            {
                page();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
        }
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
        
    //取消
    protected void  btnCancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>location='history.go(-1)'</script>");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseBO baseBO = new BaseBO();
        UserRole userAuthInfo = new UserRole();
        Users userInfo = new Users();
        BaseTrans trans = new BaseTrans();
        PassWord pwd = new PassWord();

        if (this.lstRoleName.SelectedValue == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '请选择角色名称'", true);
            return;
        }
        string deptid = Session["DeptID"].ToString();
        if (deptid == "")
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "deptidnull();", true);
            return;
        }
        baseBO.WhereClause = "UserCode='" + txtUserCode.Text + "'";
        Resultset rs = baseBO.Query(userInfo);
        if (rs.Count >= 1)
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "usercodebeing();", true);
            return;
        }
       

        userInfo.UserID = Base.BaseApp.GetUserID();                                        //用户ID
        userInfo.CreateUserID = objSessionUser.UserID;                          //创建用户Id
        userInfo.CreateTime = DateTime.Now;
        userInfo.UserCode = txtUserCode.Text.Trim();                                //用户编码
        userInfo.UserName = this.txtUserName.Text.Trim();                             //用户名称

        pwd.EncryptDecrypStr = this.txtPassword.Text.Trim();
        pwd.DesEncrypt();
        userInfo.Password = pwd.MyDesStr;                            //密码

        userInfo.Mobile1 = this.txtMobile1.Text.Trim();                               //联系电话1
        userInfo.OfficeTel = this.txtOfficeTel.Text.Trim();                           //办公电话
        userInfo.UserStatus = Convert.ToInt32(this.cmbUserState.Visible);    //用户状态                                        //备注
        userInfo.EMail = txtEmail.Text.Trim();
        userInfo.WorkNo = txtWorkNo.Text.Trim();
        //部门
        DataSet deptDataset = baseBO.QueryDataSet(userInfo);

        userAuthInfo.UserID = userInfo.UserID;           //插入UserAuth表中的 UserID

        if (this.lstRoleName.SelectedItem != null)
        {
            try
            {
                trans.BeginTrans();

                if (trans.Insert(userInfo) != -1)
                {
                    foreach (ListItem lst in this.lstRoleName.Items)
                    {
                        if (lst.Selected)
                        {
                            userAuthInfo.RoleID = Convert.ToInt32(lst.Value);
                            userAuthInfo.DeptID = Convert.ToInt32(deptid);
                            userAuthInfo.UserID = userInfo.UserID;
                            trans.Insert(userAuthInfo);
                        }
                    }
                    trans.Commit();
                    page();
                    textclear();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
                    
                }
                else
                {
                    trans.Rollback();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                    return;
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('"+ ex +"')", true);
                trans.Rollback();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidSelect.Value + "'", true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
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
        lblCurrent.Text = "1";
        page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        
    }
    protected void ibtnPrev_Click(object sender, ImageClickEventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) - 1);
        page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void ibtnNext_Click(object sender, ImageClickEventArgs e)
    {
        lblCurrent.Text = Convert.ToString(int.Parse(lblCurrent.Text) + 1);
        page();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    protected void page()
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        PagedDataSource pds = new PagedDataSource();
        int spareRow = 0;
        if(Session["DeptID"].ToString()=="")
        {
            baseBO.WhereClause = "a.userid=b.userid and b.deptid=" + 0;
        }
        else
        {
            baseBO.WhereClause = "a.userid=b.userid and b.deptid=" + Session["DeptID"];
        }

        baseBO.GroupBy = "a.userid,usercode,username,WorkNo,officetel,UserStatus";

        BaseInfo.BaseCommon.BindGridView(baseBO, new UserInfo(), this.GrdUser);
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
        //        this.GrdUser.DataSource = pds;
        //        this.GrdUser.DataBind();
        //        spareRow = GrdUser.Rows.Count;
        //        for (int i = 0; i < pds.PageSize - spareRow; i++)
        //        {
        //            dt.Rows.Add(dt.NewRow());
        //        }
        //        pds.DataSource = dt.DefaultView;
        //        GrdUser.DataSource = pds;
        //        GrdUser.DataBind();
        //}
        #endregion
    }

    private void textclear()
    {

        txtUserCode.Text = "";
        txtUserName.Text = "";
        txtPassword.Text = "";
        txtMobile1.Text = "";
        txtOfficeTel.Text = "";
        txtWorkNo.Text = "";
        txtEmail.Text = "";
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
        page();
        foreach (GridViewRow grv in this.GrdUser.Rows)
        {
            grv.BackColor = Color.White;
        }
    }
}


