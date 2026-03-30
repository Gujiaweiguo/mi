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
public partial class BaseInfo_User_MainTree : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    private void Page_UnLoad(object sender, System.EventArgs e)
    {
        
    }

    private void Page_PreRenderComplete(object sender, System.EventArgs e)
    {
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
        treeNode.Target = "rightPartFrame";
    }
    protected void MyAccordion_DataBinding(object sender, EventArgs e)
    {

    }
    protected void MyAccordion_ItemCommand(object sender, CommandEventArgs e)
    {
       
    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);

    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.Load += new System.EventHandler(this.Page_Load);

    }
    #endregion
}
