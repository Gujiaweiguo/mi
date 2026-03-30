using System;
using System.Web;
using System.Web.UI;
using Base.Biz;
using Base.DB;
using Base.Page;
using Base.Sys;
using BaseInfo;
using BaseInfo.Dept;
using BaseInfo.User;


public partial class Login1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtUserCode.Focus();
        if (!IsPostBack)
        {
            //String[] langs = SessionUser.GetLanguages();
            //foreach (String lang in langs)
            //{
            //    this.cboLanguage.Items.Add(new ListItem(SessionUser.GetLanguageDesc(lang), lang));
            //}
            ViewState["Language"] = "CHN";
           // lblLoginTit.Text = "系统登录";

            this.txtPassword.Attributes.Add("onkeypress", "keyclick()");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        BaseBO bo = new BaseBO();
        bo.WhereClause = "UserCode='" + this.txtUserCode.Text.Trim() + "'";
        Resultset rs = bo.Query(new SessionUser());
        PassWord pwd = new PassWord();

        if (rs.Count == 0)   //没有查询的数据没有符合要求的
        {
            string user = "";
            if (ViewState["Language"].ToString() == "English")
            {
                user = "The user inexistence!";
            }
            if (ViewState["Language"].ToString() == "CHN")
            {
                user = "用户不存在!";
            }
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + user + "')", true);
            return;
        }

        SessionUser sessionUser = rs.Dequeue() as SessionUser;

        pwd.EncryptDecrypStr = txtPassword.Text.Trim();
        pwd.DesEncrypt();

        if (!sessionUser.Password.Equals(pwd.MyDesStr))
        {
            string password = "";
            if (ViewState["Language"].ToString() == "English")
            {
                password = "Password Error!";
            }
            if (ViewState["Language"].ToString() == "CHN")
            {
                password = "密码错误!";
            }
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + password + "')", true);
            return;
        }

        if (Convert.ToInt32(sessionUser.UserStatus) != Users.USER_STATUS_VALID)
        {
            string userStatus = "";
            if (ViewState["Language"].ToString() == "English")
            {
                userStatus = "User trial suspended!";
            }
            if (ViewState["Language"].ToString() == "CHN")
            {
                userStatus = "用户暂停试用!";
            }
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + userStatus + "')", true);
            return;
        }

        bo.WhereClause = "UserID=" + sessionUser.UserID;
        Resultset userRoles = bo.Query(new UserRole());
        if (userRoles.Count == 0)   //没有查询的数据没有符合要求的
        {
            string role = "";
            if (ViewState["Language"].ToString() == "English")
            {
                role = "The user no role!";
            }
            if (ViewState["Language"].ToString() == "CHN")
            {
                role = "用户没有定义角色!";
            }
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + role + "')", true);
            return;
        }

        UserRole userRole = userRoles.Dequeue() as UserRole;
        //登陆日志
        string strIp = Page.Request.UserHostAddress.ToString();
        UserPageLog.UserLoginLog(sessionUser.UserID, strIp);      
        
        SessionUserLog sessionUserLog = new SessionUserLog();
        /*保存页面日志Session*/
        sessionUserLog.DeptID = userRole.DeptID;
        sessionUserLog.RoleID = userRole.RoleID;
        sessionUserLog.UserID = sessionUser.UserID;         
        sessionUser.DeptID = userRole.DeptID;
        sessionUser.RoleID = userRole.RoleID;
        sessionUser.LastAccessTime = DateTime.Now;
        //判断用户部门是否停用 add by lcp
        BaseBO objBaseBo = new BaseBO();
        objBaseBo.WhereClause = "deptid=" + userRole.DeptID;
        Resultset rsDept = objBaseBo.Query(new Dept());
        if (rsDept.Count == 1)
        {
            Dept objDept = rsDept.Dequeue() as Dept;
            if (objDept.DeptStatus == 0)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('该用户的部门已停用,不能登陆')", true);
                return;
            }
        }
        //
        if (ViewState["Language"].ToString() == "English")
        {
            sessionUser.Language = SessionUser.LANGUAGE_EN_US;
            Session["Currentculture"] = SessionUser.LANGUAGE_EN_US;
        }
        if (ViewState["Language"].ToString() == "CHN")
        {
            sessionUser.Language = SessionUser.LANGUAGE_ZH_CN;
            Session["Currentculture"] = SessionUser.LANGUAGE_ZH_CN;
        }


        /*获取Mall名称信息*/
        bo.WhereClause = "DeptID = 100";
        rs = bo.Query(new MiInfoVindicate());
        if (rs.Count == 1)
        {
            MiInfoVindicate miInfo = rs.Dequeue() as MiInfoVindicate;
            Session["MallTitle"] = miInfo.DeptName;
        }

        Session["UserAccessInfo"] = sessionUser;   //将所有对保存到会话中
        
        Session["UserRoles"] = userRoles;

        Session["SessionUserLog"] = sessionUserLog; //保存页面打开日志

        clearCookies();
        //Page.RegisterStartupScript("StartUp", "<script language='javascript'>login();</script>");
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "win", "login()", true);
    }

    protected void clearCookies()
    {
        /*删除Cookies商户ID*/
        HttpCookie cookiesCustumer = new HttpCookie("Custumer");

        cookiesCustumer.Expires = System.DateTime.Now.AddHours(1);
        cookiesCustumer.Values.Add("CustumerID", "");
        Response.AppendCookie(cookiesCustumer);

        /*删除Cookies合同ID*/
        HttpCookie cookiesContract = new HttpCookie("Contract");

        cookiesContract.Expires = System.DateTime.Now.AddHours(1);
        cookiesContract.Values.Add("ContractID", "");
        Response.AppendCookie(cookiesContract);

        /*删除Cookies驳回和草稿状态*/
        HttpCookie cookiesDisprove = new HttpCookie("Disprove");

        cookiesDisprove.Expires = System.DateTime.Now.AddHours(1);
        cookiesDisprove.Values.Add("DisproveID", "0");
        Response.AppendCookie(cookiesDisprove);


        /*删除Cookies工作流ID和节点ID*/
        HttpCookie cookiesWorkFlow = new HttpCookie("WorkFlow");

        cookiesWorkFlow.Expires = System.DateTime.Now.AddHours(1);
        cookiesWorkFlow.Values.Add("WorkFlowID", "");
        cookiesWorkFlow.Values.Add("NodeID", "");
        cookiesWorkFlow.Values.Add("Sequence", "");
        cookiesWorkFlow.Values.Add("VoucherID", "");
        Response.AppendCookie(cookiesWorkFlow);


        /*清除合同Cookies 合同ID,工作流ID,节点ID,单据ID*/
        HttpCookie cookies = new HttpCookie("Info");
        cookies.Expires = System.DateTime.Now.AddDays(1);
        cookies.Values.Add("conID", "");
        cookies.Values.Add("wrkFlwID", "");
        cookies.Values.Add("sequence", "");
        cookies.Values.Add("nodeID", "");
        cookies.Values.Add("Disprove", "");
        cookies.Values.Add("ConOverTimeID", "");
        cookies.Values.Add("ReturnSequence", "");
        cookies.Values.Add("ConFormulaModID", "");
        cookies.Values.Add("CustShortName", "");
        Response.AppendCookie(cookies);

        /*把换页状态存入Cookies - 0 为不换页 1 为换页*/
        HttpCookie cookiesBarter = new HttpCookie("Barter");

        cookiesBarter.Expires = System.DateTime.Now.AddHours(1);
        cookiesBarter.Values.Add("BarterID", "0");
        Response.AppendCookie(cookiesBarter);


        /*把节点编号存入Cookies*/
        HttpCookie cookiesSequence = new HttpCookie("Sequence");

        cookiesSequence.Expires = System.DateTime.Now.AddHours(1);
        cookiesSequence.Values.Add("SequenceID", "");
        Response.AppendCookie(cookiesSequence);

        /*把推广活动编号存入Cookies*/
        HttpCookie cookiesAnPMaster = new HttpCookie("AnPMaster");

        cookiesAnPMaster.Expires = System.DateTime.Now.AddHours(1);
        cookiesAnPMaster.Values.Add("AnPID", "");
        Response.AppendCookie(cookiesAnPMaster);


    }

    protected void BtnCHN_Click(object sender, EventArgs e)
    {
        ViewState["Language"] = "CHN";
        //lblUserName.Text = "用户代码";
        //lblPassword.Text = "登录口令";
        //lblLoginTit.Text = "系统登录";
        //Button1.Text = "确认";
    }
    protected void BtnEnglish_Click(object sender, EventArgs e)
    {
        ViewState["Language"] = "English";
        //lblUserName.Text = "User code";
        //lblPassword.Text = "Password";
        //lblLoginTit.Text = "Login in";
        //Button1.Text = "OK";
    }
}
