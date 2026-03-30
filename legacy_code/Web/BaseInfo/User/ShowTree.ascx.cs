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
using BaseInfo.User;

public partial class BaseInfo_User_ShowTree : System.Web.UI.UserControl
{
    private void Page_Load(object sender, System.EventArgs e)
    {
        // 在此处放置用户代码以初始化页面

        if (!IsPostBack)
        {
            
            TreeView1.Nodes.Clear();
            InitTree();
        }
    }

    private void InitTree()
    {

        BaseTree tree = new BaseTree(new ShowMenu());
        tree.CreateTree();
        foreach (ITreeNode tn in tree.RootNodes)
        {
            TreeNode treeRoot = new TreeNode();
            InitTreeNode(treeRoot, tn);
            TreeView1.Nodes.Add(treeRoot);
        }
    }

    private void InitTreeNode(TreeNode treeNode, ITreeNode deptNode)
    {
        foreach (ITreeNode dn in deptNode.GetChildren())
        {
            TreeNode tn = new TreeNode();
            treeNode.ChildNodes.Add(tn);
            InitTreeNode(tn, dn);
        }
        treeNode.NavigateUrl = deptNode.GetValue();
        treeNode.Text = deptNode.GetText();
    }

}
