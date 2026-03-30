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

using WorkFlow.Uiltil;
using WorkFlow.WrkFlw;
using BaseInfo.User;
public partial class WorkFlow_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitTree();
        }
    }
    private void InitTree()
        {
            //WrkFlwTree wrkFlwTree = new WrkFlwTree();
            //SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
            //wrkFlwTree.BuildTree(objSessionUser.RoleID);
            
            //foreach (WrkFlwTreeNode rootNode in wrkFlwTree.RootNodes)
            //{
            //    TreeNode treeRoot = new TreeNode();

            //    InitTreeNode(treeRoot, rootNode);

            //    TreeView1.Nodes.Add(treeRoot);
            //}
        }

    //private void InitTreeNode(TreeNode treeNode, WrkFlwTreeNode wrkFlwNode)
    //{

    //    foreach (WrkFlwTreeNode wfNode in wrkFlwNode.ChildNodes)
    //    {
    //        TreeNode tn = new TreeNode();
    //        treeNode.ChildNodes.Add(tn);
    //        InitTreeNode(tn, wfNode);
    //    }
    //    treeNode.Value = wrkFlwNode.NodeValue;
    //    treeNode.Text = wrkFlwNode.NodeText;
    //    treeNode.NavigateUrl =  wrkFlwNode.NodeValue;
    //    treeNode.ImageUrl = wrkFlwNode.ImageURL;
    //}
}

