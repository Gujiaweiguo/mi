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
using System.IO;
using BaseInfo.User;
using Base.Page;
using BaseInfo.Dept;
using BaseInfo.Role;
using Base.Sys;
using WorkFlow.WorkFlowMail;
using System.Collections.Generic;
using WorkFlow.WrkFlw;
using Base.Sys;
using System.Text;
/// <summary>
/// add by lcp at 2009-3-8
/// </summary>
public partial class WorkFlow_WrkFlwMailConfig : BasePage
{
    public string baseInfo;
    private static Hashtable ids = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        Resultset rs = new Resultset();
        BaseBO baseBO = new BaseBO();
        this.ShowTree();
        
        this.SetControlPro();
        if (!this.IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "WrkFlwEmail_SetUp");
            this.SetControlLock();
            //int[] mailStatus = WrkFlwMailSetInfo.GetWorkFlowMailStatus();

            int[] deptStatus = Dept.GetDeptStatus();
            for (int i = 0; i < deptStatus.Length; i++)
            {
                this.ddlEmailState.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", BaseInfo.Role.Role.GetRoleStatusDesc(deptStatus[i])), deptStatus[i].ToString()));
            }

            List<WrkFlwNode> wrkFlwNodes = (List<WrkFlwNode>)Session["WrkFlwNodes"];
            rs = baseBO.Query(new QueryBizGrp());
            foreach (QueryBizGrp bizGrps in rs)
            {
                this.ddlBizGrpID.Items.Add(new ListItem(bizGrps.BizGrpName, bizGrps.BizGrpID.ToString()));
            }
        }
    }
    /// <summary>
    /// 控件添加属性
    /// </summary>
    private void SetControlPro()
    {
        //btnSave.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSaveing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnSave.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnSave.gif) no-repeat left top';this.style.fontWeight='normal';");
        btnSave.Attributes.Add("onclick", "return EmailValidator(form1)");
        //btnAdd.Attributes.Add("OnMouseOver", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnEditing.gif) no-repeat left top';this.style.fontWeight='bold';");
        //btnAdd.Attributes.Add("OnMouseOut", "this.style.background='url(../App_Themes/CSS/BtnImage/BtnEdit.gif) no-repeat left top';this.style.fontWeight='normal';");
    }
    /// <summary>lcp,2009-3-5
        /// 读取部门信息
        /// </summary>
    private void ShowTree()
    {
        string jsdept = "";

        BaseBO baseBO = new BaseBO();
        baseBO.OrderBy = "wrkFlwMailID";
        Resultset rs = baseBO.Query(new WorkFlow.WorkFlowMail.WrkFlwMail());

        if (rs.Count > 0)
        {
            jsdept = "100" + "|" + "10" + "|" + "邮件模板" + "^";
            foreach (WorkFlow.WorkFlowMail.WrkFlwMail info in rs)
            {
                jsdept += info.WrkFlwMailID + "|" + info.PBizGrpID + "|" + info.MailSubject + "^";
            }
            depttxt.Value = jsdept;

        }
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        this.SetControlUNLock();

        ViewState["wrkFlwMailID"] = deptid.Value;
            Session["wrkFlwMailID"] = deptid.Value;
        if (ViewState["wrkFlwMailID"] != null || ViewState["wrkFlwMailID"].ToString() != "")
        {
            this.ddlBizGrpID.Enabled = true;
            this.ddlEmailState.Enabled = true;
            this.txtTitle.ReadOnly = false;
            this.txtContent.ReadOnly = false;
            BaseBO objBaseBo = new BaseBO();
            //WrkFlwMailSetInfo objWrkMailSetInfo = new WrkFlwMailSetInfo();
            WrkFlwMail objWrkFlwMail = new WrkFlwMail();
            objBaseBo.WhereClause = "WrkFlwMailID = " + ViewState["wrkFlwMailID"].ToString();
            Resultset rs = objBaseBo.Query(new WrkFlwMail());
            if (rs.Count != 0)
            {
                objWrkFlwMail = rs.Dequeue() as WrkFlwMail;
                this.txtContent.Text = objWrkFlwMail.MailText;
                this.txtTitle.Text = objWrkFlwMail.MailSubject;
                this.ddlBizGrpID.SelectedValue = Convert.ToString(objWrkFlwMail.BizGrpID);
                this.ddlEmailState.SelectedValue = Convert.ToString(objWrkFlwMail.MailStatus);
            }
        }

        //Session["wrkFlwMailID"] = deptid.Value;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    /// <summary>
    /// 设置控件不可用
    /// </summary>
    private void SetControlLock()
    {
        this.ddlBizGrpID.Enabled = false;
        this.ddlEmailState.Enabled = false;
        this.txtTitle.ReadOnly = true;
        this.txtTitle.Text = "";
        this.txtTitle.CssClass = "EnabledColor";
        this.txtContent.Enabled = false;
        this.txtContent.Text = "";
        this.txtContent.CssClass = "EnabledColor";
    }
    /// <summary>
    /// 设置控件可用
    /// </summary>
    private void SetControlUNLock()
    {
        this.ddlBizGrpID.Enabled = true;
        this.ddlEmailState.Enabled = true;
        this.txtTitle.ReadOnly = false;
        this.txtContent.Enabled = true;
    }
    private void SetControlValueISNull()
    {
        this.txtTitle.Text = "";
        this.txtContent.Text = "";
    }
    /// <summary>
    /// 添加参数设置
    /// </summary>
    /// <returns></returns>
    private void SaveAdd()
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseTrans objTran = new BaseTrans();
        //WrkFlwMailSetInfo objWrkMailSetInfo = new WrkFlwMailSetInfo();
        //objWrkMailSetInfo.BizGrpID = Session["BizGrpID"].ToString();
        WrkFlwMail objWrkFlMail = new WrkFlwMail();
        objWrkFlMail.WrkFlwMailID = this.GetSkuMasterSkuID("WrkFlwMail", "wrkflwmailid");
        objWrkFlMail.CreateUserId = objSessionUser.UserID;
        objWrkFlMail.CreateTime = DateTime.Now;
        objWrkFlMail.OprRoleID = objSessionUser.OprRoleID;
        objWrkFlMail.OprDeptID = objSessionUser.OprDeptID;
        objWrkFlMail.MailSubject = this.txtTitle.Text.ToString();
        objWrkFlMail.MailStatus = Int32.Parse(this.ddlEmailState.SelectedValue);
        objWrkFlMail.BizGrpID = Int32.Parse(this.ddlBizGrpID.SelectedValue);
        objWrkFlMail.MailText = this.txtContent.Text.ToString();
        if (this.txtTitle.Text.Trim() != "")
        {
            try
            {
                objTran.BeginTrans();
                if (objTran.Insert(objWrkFlMail) != -1)
                {
                    objTran.Commit();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
                }
                else
                {
                    objTran.Rollback();
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + ex + "')", true);
                objTran.Rollback();
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidMessage.Value + "'", true);
            return;
        }
        ViewState["wrkFlwMailID"] = "";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidAdd.Value + "'", true);
    }
    /// <summary>
    /// 更新参数设置
    /// </summary>
    /// <returns></returns>
    private void SaveUpdate()
    {
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        BaseTrans objTrans = new BaseTrans();
        try
        {
            BaseBO objBaseBo = new BaseBO();
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE WrkFlwMail SET ModifyUserID='"+objSessionUser.ModifyUserID+"',");
            sb.Append("ModifyTime='"+DateTime.Now.ToString()+"',");
            sb.Append("OprRoleID='"+objSessionUser.OprRoleID+"',OprDeptID='"+objSessionUser.OprDeptID+"',");
            sb.Append("MailSubject='" + this.txtTitle.Text.Trim() + "',MailText='" + this.txtContent.Text + "',MailStatus='" + this.ddlEmailState.SelectedValue + "',BizGrpID='" + this.ddlBizGrpID.SelectedValue + "' where wrkFlwMailID = '" + Session["wrkFlwMailID"].ToString() + "'");
            if (objBaseBo.ExecuteUpdate(sb.ToString()) != -1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
                return;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "", "alert('" + ex + "')", true);
            objTrans.Rollback();
        }
        ViewState["wrkFlwMailID"] = "";
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" +this.hidUpdate.Value + "'", true);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Session["wrkFlwMailID"] == null || Session["wrkFlwMailID"].ToString() == "")
            this.SaveAdd();
        else
            this.SaveUpdate();
        Session["wrkFlwMailID"] = "";
        this.SetControlLock();
        this.SetControlValueISNull();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);

        Response.Redirect("WrkFlwMailConfig.aspx");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        this.SetControlUNLock();
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "showtree", "treearray()", true);
    }
    public  int GetSkuMasterSkuID(String table, String field)
    {
        lock (ids)
        {

            int id = 0;
            //如果hashtable中已经有该id，则取出来，加1
            String key = (table + "_" + field).ToLower();
            if (ids[key] != null)
            {
                id = (int)ids[key] + 1;
                ids[key] = id;
                return id;
            }
            //如果hashtable里没有该id，则从数据表中取最大值，加1
            String sql = "select max(Convert(int," + field + ")) from " + table;
            BaseBO bo = new BaseBO();
            DataSet ds = bo.QueryDataSet(sql);
            //如果数据表中有数据，则取出后加1
            //if (ds.Tables[0].Rows.Count!=1)
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                id = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
            }
            //如果数据表没有数据（一般在首次使用后出现这种情况），则取缺省值10001，10001以内用作备用值
            else
            {
                id = 100001;
            }

            ids[key] = id;
            return id;
        }
    }
}
