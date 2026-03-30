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

public partial class BaseInfo_User_ShowMenu : System.Web.UI.UserControl
{
    private void Page_Load(object sender, System.EventArgs e)
    {
        // 在此处放置用户代码以初始化页面


        if (!IsPostBack)
        {
            this.Menu1.Items.Clear();
            InitMenu();
        }
    }


    private void InitMenu()
    {

        BaseTree tree = new BaseTree(new ShowMenu());
        tree.CreateTree();
        foreach (ITreeNode tn in tree.RootNodes)
        {
            MenuItem menuRoot = new MenuItem();
            InitTreeNode(menuRoot, tn);
            this.Menu1.Items.Add(menuRoot);
        }
    }

    private void InitTreeNode(MenuItem treeNode, ITreeNode deptNode)
    {

        foreach (ITreeNode dn in deptNode.GetChildren())
        {
            MenuItem tn = new MenuItem();
            treeNode.ChildItems.Add(tn);
            InitTreeNode(tn, dn);
        }
        treeNode.NavigateUrl = deptNode.GetValue();
        treeNode.Text = deptNode.GetText();
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {

    }
}
