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


using Base.DB;
using Base.Biz;
using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using BaseInfo.User;
using Base.Page;
public partial class Default10 :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindMenu();
    }

    #region "绑定主菜单"
    /// <summary>
    /// 绑定主菜单



    /// </summary>
    private void BindMenu()
    {
        String whereSql;
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];

        whereSql = "BizGrpStatus=" + BizGrp.BIZ_GRP_STATUS_VALID;
        whereSql = whereSql + " AND BizgrpID in ( select a.BizgrpID from BizGrp a, WrkFlw b, WrkFlwNode c, Func d ";
        whereSql = whereSql + "where a.BizGrpID=b.BizGrpID  and b.WrkFlwID=c.WrkFlwID and c.FuncID=d.FuncID ";
        whereSql = whereSql + "and a.BizGrpStatus=" + BizGrp.BIZ_GRP_STATUS_VALID;
        whereSql = whereSql + "and b.WrkFlwStatus=" + WrkFlw.WRK_FLW__STATUS_VALID;
        whereSql = whereSql + "and c.RoleID=" + objSessionUser.RoleID + " ) ";



        BaseBO baseBO = new BaseBO();

        baseBO.OrderBy = "BizGrpID";
        baseBO.WhereClause = whereSql;
        
        LeftMenu.DataSource = baseBO.QueryDataSet(new BizGrp());
        LeftMenu.DataBind();
    }
    #endregion


    #region "绑定子菜单"
    /// <summary>
    /// 绑定子菜单事件



    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LeftMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView aRowView = (DataRowView)e.Item.DataItem;
        string bizGrpID = aRowView["BizGrpID"].ToString();
        

        WrkFlwTree wrkFlwTree = new WrkFlwTree();
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        wrkFlwTree.BuildTree(objSessionUser.DeptID,objSessionUser.RoleID, Convert.ToInt32(bizGrpID));

        foreach (WrkFlwTreeNode rootNode in wrkFlwTree.RootNodes)
        {
            TreeNode treeRoot = new TreeNode();
            InitTreeNode(treeRoot, rootNode);
            //TreeView1.Nodes.Add(treeRoot);
            TreeView treeView = (TreeView)e.Item.FindControl("TreeView1");
            treeView.Nodes.Add(treeRoot);
        }
        
    }


    #endregion


    private void InitTree()
    {

        
    }

    private void InitTreeNode(TreeNode treeNode, WrkFlwTreeNode wrkFlwNode)
    {
        foreach (WrkFlwTreeNode wfNode in wrkFlwNode.ChildNodes)
        {
            TreeNode tn = new TreeNode();
            treeNode.ChildNodes.Add(tn);
            InitTreeNode(tn, wfNode);
        }
        if (treeNode.Depth == 1)
        {
            treeNode.NavigateUrl = wrkFlwNode.NodeValue;
        }
        else
        {
            treeNode.NavigateUrl = "";
        }
        treeNode.Value = wrkFlwNode.NodeValue;
        treeNode.Text = wrkFlwNode.NodeText;
        treeNode.ImageUrl = wrkFlwNode.ImageURL;
        /*点击父节点为打开子节点事件*/
        treeNode.SelectAction = TreeNodeSelectAction.Expand;
    }
}
