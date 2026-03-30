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

using RentableArea;
using Base.Biz;
using Base.DB;
using Base;
using Base.Page;
using BaseInfo.User;
using System.Drawing;

public partial class Lease_TradeRelation_SkuClass : BasePage
{
    public string baseInfo;
    public string selectTradeLevel;
    private static int OPR_ADD = 1;
    private static int OPR_EDIT = 2;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            baseInfo = (String)GetGlobalResourceObject("BaseInfo", "RentableArea_SkuClassVindicate");
            ShowTree();
            selectTradeLevel = (String)GetGlobalResourceObject("BaseInfo", "Prompt_LeaseTradeID");

            int[] status = TradeRelation.GetTradeRelationStatus();
            for (int i = 0; i < status.Length; i++)
            {
                cmbTradePStatus.Items.Add(new ListItem((String)GetGlobalResourceObject("Parameter", TradeRelation.GetTradeRelationStatusDesc(status[i])), status[i].ToString()));
            }
        }
        btnSave.Attributes.Add("onclick", "return CheckData();");
        btnEdit.Attributes.Add("onclick", "return CheckData();");

    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        btnAdd.Enabled = false;

        txtTradePCode.ReadOnly = false;
        txtTradePName.ReadOnly = false;
        cmbTradePStatus.Enabled = true;
        ClearText();
        ViewState["Flag"] = OPR_ADD;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    protected void treeClick_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        Resultset rs = new Resultset();
        SkuClass objSkuClass = new SkuClass();
        ViewState["ClassID"] = deptid.Value;

        baseBO.WhereClause = "ClassID=" + deptid.Value;
        rs = baseBO.Query(objSkuClass);
        if (rs.Count == 1)
        {
            objSkuClass = rs.Dequeue() as SkuClass;
            txtTradePCode.Text = objSkuClass.ClassCode;
            txtTradePName.Text = objSkuClass.ClassName;
            txtTradePCode.ReadOnly = true;
            txtTradePName.ReadOnly = true;
            cmbTradePStatus.SelectedValue = objSkuClass.ClassStatus.ToString();
            cmbTradePStatus.Enabled = false;
        }
        btnEdit.Enabled = true;
        btnAdd.Enabled = true;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "parent.document.all.txtWroMessage.value = ''", true);
        Session["editLog"] = txtTradePCode.Text;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        btnEdit.Enabled = false;
        btnSave.Enabled = true;
        btnAdd.Enabled = false;
        txtTradePCode.ReadOnly = false;
        txtTradePName.ReadOnly = false;
        cmbTradePStatus.Enabled = true;
        ViewState["Flag"] = OPR_EDIT;
        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtTradePCode.Text != "" && txtTradePName.Text != "")
        {
            SkuClass objSkuClass = new SkuClass();
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet("select ClassCode from SkuClass where ClassCode='" + txtTradePCode.Text.Trim() + "'");
            if (Convert.ToInt32(ViewState["Flag"]) == OPR_ADD)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "RentableArea_SkuClassCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtTradePCode.select()", true);
                }
                else
                {

                    if (ViewState["ClassID"].ToString().Length == 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_NotNode") + "'", true);
                    }

                    if (Convert.ToInt32(ViewState["ClassID"]) == 100)
                    {
                        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                        objSkuClass.CreateUserID = sessionUser.UserID;
                        objSkuClass.ClassID = BaseApp.GetID("SkuClass", "ClassID");
                        objSkuClass.ClassCode = txtTradePCode.Text.Trim();
                        objSkuClass.ClassName = txtTradePName.Text.Trim();
                        objSkuClass.PClassID = 100;
                        objSkuClass.ClassLevel = TradeRelation.TRADELEVEL_STATUS_ONE;
                        objSkuClass.ClassStatus = Convert.ToInt32(cmbTradePStatus.SelectedValue);
                    }
                    else
                    {
                        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                        objSkuClass.CreateUserID = sessionUser.UserID;
                        objSkuClass.ClassID = BaseApp.GetID("SkuClass", "ClassID");
                        objSkuClass.ClassCode = txtTradePCode.Text.Trim();
                        objSkuClass.ClassName = txtTradePName.Text.Trim();
                        objSkuClass.PClassID = Convert.ToInt32(ViewState["ClassID"]);
                        objSkuClass.ClassLevel = TradeRelation.TRADELEVEL_STATUS_TWO;
                        objSkuClass.ClassStatus = Convert.ToInt32(cmbTradePStatus.SelectedValue);
                    }
                    if (baseBO.Insert(objSkuClass) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    }
                    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_Success") + "'", true);
                    ShowTree();
                    ClearText();
                    this.btnSave.Enabled = false;
                }
            }
            else if (Convert.ToInt32(ViewState["Flag"]) == OPR_EDIT)
            {
                if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows[0][0].ToString() == Session["editLog"].ToString())
                {
                    if (ViewState["ClassID"].ToString().Length == 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "PublicMes_NotNode") + "'", true);
                    }
                    if (Convert.ToInt32(ViewState["ClassID"]) == 100)
                    {
                        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                        objSkuClass.ModifyUserID = sessionUser.UserID;
                        objSkuClass.ModifyTime = DateTime.Now;
                        objSkuClass.ClassCode = txtTradePCode.Text.Trim();
                        objSkuClass.ClassName = txtTradePName.Text.Trim();
                        objSkuClass.ClassStatus = Convert.ToInt32(cmbTradePStatus.SelectedValue);
                    }
                    else
                    {
                        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
                        objSkuClass.ModifyUserID = sessionUser.UserID;
                        objSkuClass.ModifyTime = DateTime.Now;
                        objSkuClass.ClassCode = txtTradePCode.Text.Trim();
                        objSkuClass.ClassName = txtTradePName.Text.Trim();
                        objSkuClass.ClassStatus = Convert.ToInt32(cmbTradePStatus.SelectedValue);
                    }
                    baseBO.WhereClause = "ClassID = " + Convert.ToInt32(ViewState["ClassID"]);
                    if (baseBO.Update(objSkuClass) == -1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_WorkLose") + "'", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidUpdate") + "'", true);
                    }
                    //}
                    ShowTree();
                    ClearText();
                    this.btnSave.Enabled = false;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '" + (String)GetGlobalResourceObject("BaseInfo", "RentableArea_SkuClassCode") + (String)GetGlobalResourceObject("BaseInfo", "Dict_Exist") + "'", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "document.all.txtTradePCode.select()", true);
                }
            }
            ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "treeview", "treearray()", true);
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "select", "select()", true);
        }
    }
    private void ShowTree()
    {
        BaseBO baseBO = new BaseBO();
        string jsdept = "";
        //baseBO.WhereClause = "TradeStatus = " + TradeRelation.TRADERELATION_STATUS_VALID;
        baseBO.OrderBy = "ClassLevel,PClassID";
        Resultset rs = baseBO.Query(new SkuClass());
        jsdept = "100" + "|" + "0" + "|" + (String)GetGlobalResourceObject("BaseInfo", "RentableArea_SkuClass") + "^";
        if (rs.Count > 0)
        {
            foreach (SkuClass objSkuClass in rs)
            {
                if (objSkuClass.ClassLevel == 1)
                {
                    jsdept += objSkuClass.ClassID + "|" + "100" + "|" + objSkuClass.ClassName + "^";
                }
                else
                {
                    jsdept +=  objSkuClass.ClassID.ToString() + "|" + objSkuClass.PClassID + "|" + objSkuClass.ClassName + "^";
                }
            }
        }
        depttxt.Value = jsdept;
    }

    private void ClearText()
    {
        txtTradePCode.Text = "";
        txtTradePName.Text = "";
        cmbTradePStatus.SelectedIndex = 0;
    }
    
    protected void btnCel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("~/Lease/TradeRelation/SkuClass.aspx");
    }
}
