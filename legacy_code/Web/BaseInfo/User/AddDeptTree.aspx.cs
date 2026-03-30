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
using System.Data.SqlClient;
using System.Text;

using BaseInfo.Dept;
using Base.Biz;

public partial class BaseInfo_User_AddDeptTree : System.Web.UI.Page
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
        TreeNode treeRoot = new TreeNode();

        BaseTree tree = new BaseTree(new Dept());
        tree.CreateTree();
        foreach (ITreeNode tn in tree.RootNodes)
        {
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
        treeNode.Value = deptNode.GetValue();
        treeNode.Text = deptNode.GetText();
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string deptId = "";
        string deptName = "";
        string deptLevel = "";
        int intDeptLevel = 0;
        TreeNode Node = new TreeNode();
        if (TreeView1.SelectedNode.Parent != null)
        {
            if (TreeView1.SelectedNode.ChildNodes.Count == 0)
            {
                deptName = TreeView1.SelectedNode.Value;
                deptLevel = deptName.Substring(0, deptName.IndexOf(","));
                intDeptLevel = Convert.ToInt32(deptLevel);
                intDeptLevel = Convert.ToInt32(deptLevel) + 1;
            }
            else
            {
                deptName = TreeView1.SelectedNode.Value;
                intDeptLevel = Convert.ToInt32(deptName.Substring(0, deptName.IndexOf(",")));
            }
            deptId = TreeView1.SelectedNode.Value;
            //Response.Redirect("DeptAdd.aspx?id=" + deptId.Substring(deptId.IndexOf(",") + 1, deptId.Length - deptId.IndexOf(",") - 1) + "&deptLevel=" + intDeptLevel);
        }
    }
    protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {

    }

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    //Response.Write("<script> parent.document.frames['Add'].location.href='DeptQuery.aspx';</script>");
    //    //Response.Redirect("DeptQuery.aspx");

    //}

   
    protected void btnOK_Click1(object sender, EventArgs e)
    {
        if (TreeView1.CheckedNodes.Count > 0)
        {
            foreach (TreeNode node in TreeView1.CheckedNodes)
            {
                Label1.Text += node.Text + ",";
                Session["Dept"] = node.Text + ",";
            }
        }
       
        //Response.Write("<script language=javascript>alert(\"SetDepts('" + Label1.Text + "')\",\"JavaScrip\");</script>");
        Response.Write("<script language=javascript>test();</script>");
        Response.Write("<script language=javascript>window.opener.execScrip('SetDepts(" + Label1.Text + ")','JavaScrip');</script>");

        //Response.Write("<script language=javascript>window.close();</script>"); 
    }
}