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

using BaseInfo;
using Base.Biz;
using Base;
using BaseInfo.Role;
using BaseInfo.User;
using Base.Page;
using Base.DB;
using WorkFlow.WrkFlw;

public partial class BaseInfo_Role_SubAuth : BasePage
{
    protected DataSet data;
    protected string query;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int roleid = Convert.ToInt32(Request.QueryString["RoleID"]);
            CreateDataSet(roleid);
            InitTree(treeView.Nodes,"0");
            treeView.ExpandDepth = 0;
        }
    }

    private DataSet CreateDataSet(int roleid)
    {
        query = "select '' as BizGrpID,BizGrpID as funcid,BizGrpName as funcname,'' as fckstatus from bizgrp" +
                " union all"+
                " select a.BizGrpID,b.FuncID,b.FuncName,(case when c.FuncID is null then '0' else '1' end)as fckStatus  from BizGrp a inner join  Func b on a.BizGrpID=b.BizGrpID INNER join " +
                "(select FuncID,RoleID from  RoleAuth where RoleID=" + roleid + ") c on b.FuncID=C.funcID Where FuncType IN ( " + FuncTree.FUNC_TYPE_FUNCTION + " , " + FuncTree.FUNC_TYPE_FUNCTION3 + " , " + FuncTree.FUNC_TYPE_FUNCTION2 + ")"+
                " group by a.BizGrpID,BizGrpName,b.FuncID,b.FuncName,c.FuncID";

        BaseBO baseBO = new BaseBO();
        data = baseBO.QueryDataSet(query);

        return data;
    }

    private void InitTree(TreeNodeCollection Nds, string parentID)
    {
        TreeNode tmpNd;
        DataRow[] rows = data.Tables[0].Select("BizGrpID='" + parentID + "'");

        foreach (DataRow row in rows)
        {
            tmpNd = new TreeNode();
            tmpNd.Value = row["FuncID"].ToString();
            tmpNd.Text = row["FuncName"].ToString();
            Nds.Add(tmpNd);
            InitTree(tmpNd.ChildNodes, tmpNd.Value);
        }
     
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        BaseBO baseBO = new BaseBO();
        RoleAuth roleAuth = new RoleAuth();
        if (chckBoxExport.Checked == true)
        {
           roleAuth.IsExport = RoleAuth.ISEXPORT_YES; 
        }
        else
        {
            roleAuth.IsExport = RoleAuth.ISEXPORT_NO;
        }
        if (chckBoxPrint.Checked == true)
        {
            roleAuth.IsPrint = RoleAuth.ISPRINT_YES;
        }
        else
        {
            roleAuth.IsPrint = RoleAuth.ISPRINT_NO;
        }
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        if (treeView.SelectedValue.ToString() != "0")
        {
            baseBO.WhereClause = "RoleID = " + Request.QueryString["RoleID"] + " AND FuncID = " + Convert.ToInt32(treeView.SelectedValue);
            int i = baseBO.Update(roleAuth);
            if (i == 1)
            {
                ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + (String)GetGlobalResourceObject("BaseInfo", "Prompt_subAuthYes") + "'", true);
            }
        }
        else
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, this.GetType(), "message", "parent.document.all.txtWroMessage.value = '" + hidInsert.Value + "'", true);
        }
    }
    protected void treeView_SelectedNodeChanged(object sender, EventArgs e)
    {
        chckBoxExport.Checked = false;
        chckBoxPrint.Checked = false;
        Label4.Text = treeView.SelectedNode.Text;
        BaseBO baseBO = new BaseBO();
        baseBO.WhereClause = "FuncID = " + Convert.ToInt32(treeView.SelectedValue) + " AND RoleID = " + Request.QueryString["RoleID"];
        Resultset rs = baseBO.Query(new RoleAuth());
        if (rs.Count == 1)
        {
            RoleAuth roleAuth = rs.Dequeue() as RoleAuth;
            if (roleAuth.IsPrint == 1)
            {
                chckBoxPrint.Checked = true;
            }
            else
            {
                chckBoxPrint.Checked = false;
            }
            if (roleAuth.IsExport == 1)
            {
                chckBoxExport.Checked = true;
            }
            else
            {
                chckBoxExport.Checked = false;
            }
        }
    }
}
