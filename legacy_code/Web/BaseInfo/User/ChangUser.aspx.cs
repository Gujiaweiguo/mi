using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Base.Biz;
using Base.DB;
using BaseInfo.User;
using Base.Page;
using BaseInfo.Dept;
using BaseInfo.Role;
using Base.Sys;
using System.Drawing;

namespace MI_Net
{
    public partial class ChangUser : BasePage
    {
        public string baseInfo;
        int numCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["DeptID"] = deptid.Value;
            selectdeptid.Value = Convert.ToString(Session["DeptID"]);

            string jsdept = "";

            BaseBO baseBO = new BaseBO();
            //baseBO.OrderBy = "deptid";
            //baseBO.WhereClause = "deptstatus = 1";
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
            #endregion
            btnSave.Attributes.Add("onclick", "return allTextBoxValidator(form1)");

            if (!IsPostBack)
            {
                baseBO.OrderBy = "";
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
                this.btnSave.Enabled = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            Users userInfo = new Users();
            UserRole userAuthInfo = new UserRole();
            BaseTrans trans = new BaseTrans();
            PassWord pwd = new PassWord();
            string deptid = Session["DeptID"].ToString();
            if (deptid == "")
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "deptidnull();", true);
                foreach (GridViewRow grv in GrdUser.Rows)
                {
                    if (grv.Cells[1].Text == "&nbsp;")
                    {
                        grv.Cells[5].Text = "";
                    }
                }
                return;
            }
            //userInfo.ModifyUserID = objSessionUser.UserID;                 //创建用户Id
            //userInfo.ModifyTime = DateTime.Now;
            //userInfo.UserCode = txtUserCode.Text.Trim();                                //用户编码
            //userInfo.UserName = this.txtUserName.Text.Trim();                             //用户名称

            //pwd.EncryptDecrypStr = this.txtPassword.Text.Trim();
            //pwd.DesEncrypt();
            //userInfo.Password = pwd.MyDesStr;                             //密码 

            //userInfo.Mobile1 = this.txtMobile1.Text.Trim();                               //联系电话1
            //userInfo.OfficeTel = this.txtOfficeTel.Text.Trim();                           //办公电话
            //userInfo.UserStatus = Convert.ToInt32(cmbUserState.SelectedValue);    //用户状态 
            //userInfo.EMail = txtEmail.Text.Trim();
            //userInfo.WorkNo = txtWorkNo.Text.Trim();

            string strSql = "Update Users set ModifyUserID='" + objSessionUser.UserID + "',ModifyTime='" + DateTime.Now + "',UserCode='" + txtUserCode.Text.Trim() + "',UserName='" + this.txtUserName.Text.Trim() + "',Mobile1='" + this.txtMobile1.Text.Trim() + "',OfficeTel='" + this.txtOfficeTel.Text.Trim() + "',UserStatus='" + Convert.ToInt32(cmbUserState.SelectedValue) + "',EMail='" + txtEmail.Text.Trim() + "',WorkNo='" + txtWorkNo.Text.Trim() + "' where userId='" + ViewState["userID"].ToString() + "'";
            if (this.lstRoleName.SelectedItem != null)
            {
                try
                {
                    trans.BeginTrans();
                    //trans.WhereClause = "userId=" + ViewState["userID"].ToString();
                    //if (trans.Update(userInfo) != -1)
                    if (trans.ExecuteUpdate(strSql)!=-1)
                    {
                        trans.WhereClause = "UserID=" + ViewState["userID"].ToString();
                        trans.Delete(userAuthInfo);

                        foreach (ListItem lst in this.lstRoleName.Items)
                        {
                            if (lst.Selected)
                            {
                                userAuthInfo.RoleID = Convert.ToInt32(lst.Value);
                                userAuthInfo.UserID = Convert.ToInt32(ViewState["userID"]);
                                userAuthInfo.DeptID = Convert.ToInt32(deptid);
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
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdateLost.Value + "'", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + ex + "')", true);
                    trans.Rollback();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidSelect.Value + "'", true);
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidUpdate.Value + "'", true);
            this.btnSave.Enabled = false;
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
            //lblCurrent.Text = "1";
            page();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            foreach (GridViewRow grv in GrdUser.Rows)
            {
                grv.BackColor = Color.White;
            }
            textclear();
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
            lstRoleName.SelectedValue = null;
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BaseInfo/User/ChangUser.aspx");
        }

        protected void page()
        {
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            PagedDataSource pds = new PagedDataSource();
            int spareRow = 0;
            if (Session["DeptID"].ToString() == "")
            {
                baseBO.WhereClause = "a.userid=b.userid and b.deptid=" + 0;
            }
            else
            {
                baseBO.WhereClause = "a.userid=b.userid and b.deptid=" + Session["DeptID"];
            }
            baseBO.GroupBy = "a.userid,usercode,username,WorkNo,officetel,UserStatus";
            UserInfo objUserInfo = new UserInfo();
            BaseInfo.BaseCommon.BindGridView(baseBO, objUserInfo, this.GrdUser);
            #region
            //DataTable dt = baseBO.QueryDataSet(new UserInfo()).Tables[0];
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
            //    pds.CurrentPageIndex = int.Parse(lblCurrent.Text) - 1;
            //    lblTotalNum.Text = "/" + pds.PageCount.ToString() + " page";
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
                if (e.Row.Cells[1].Text != "&nbsp;")
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
        protected void GrdUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            Users userInfo = new Users();
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();
            ViewState["userID"] = GrdUser.SelectedRow.Cells[0].Text;
            baseBO.WhereClause = "userId=" + GrdUser.SelectedRow.Cells[0].Text;
            rs = baseBO.Query(new Users());
            if (rs.Count != 0)
            {
                userInfo = rs.Dequeue() as Users;
                txtUserCode.Text = userInfo.UserCode;     //用户编码
                txtUserName.Text = userInfo.UserName; ;   //用户名称
                txtMobile1.Text = userInfo.Mobile1;       //联系电话1
                txtOfficeTel.Text = userInfo.OfficeTel;   //办公电话
                cmbUserState.SelectedValue = userInfo.UserStatus.ToString(); //用户状态   
                txtWorkNo.Text = userInfo.WorkNo;
                txtEmail.Text = userInfo.EMail;
            }
            baseBO.WhereClause = "userId=" + GrdUser.SelectedRow.Cells[0].Text;
            rs = baseBO.Query(new UserRole());
            if (rs.Count != 0)
            {
                foreach (UserRole userrole in rs)
                {
                    lstRoleName.SelectedValue = userrole.RoleID.ToString();
                }
            }
            this.btnSave.Enabled = true;
            page();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
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
            foreach (GridViewRow grv in this.GrdUser.Rows)
            {
                grv.BackColor = Color.White;
            }
            this.page();
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
        }
    }

}

