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
using Base.DB;
using Base.Page;
public partial class Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.Menu1.Items.Clear();
        //InitMenu();
        String strMenu = "";
        String strMenuCreated = "";
        String strMenuName="";
        TShowMenu tShowMenu = new TShowMenu();
        strMenu = "var menuMgr = new NlsMenuManager(" + "'mgr1'" + ");" + "\r\n";
        strMenu = strMenu + "menuMgr.defaultEffect = " + "'fade'" + ";" + "\r\n";
        strMenu = strMenu + "function initMenu() { " + "\r\n";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        int prId = 0;
        BaseBO baseBO = new BaseBO();
        tShowMenu.RoleID = 101;
        baseBO.OrderBy = "pmenuid,menuorder";
        Resultset rs = baseBO.Query(tShowMenu);
 
        foreach (TShowMenu menu in rs)
        {
            prId = menu.PMenuID;

            strMenuName = (String)GetGlobalResourceObject("BaseInfo", menu.BaseInfo);
            if (strMenuName == "" && strMenuName == null)
            {
                strMenuName = "";
            }
            if (strMenuCreated.IndexOf("||" + prId + "||") == -1)
            {
                strMenuCreated = strMenuCreated + "||" + prId + "||";
                if (prId == 0)
                {
                    strMenu = strMenu + "var mn" + prId + " = menuMgr.createMenubar(" + "'mn" + prId + "'" + ");" + "\r\n";
                    strMenu = strMenu + "mn" + prId + ".itemSpc=" + "'0px'" + ";" + "\r\n";
                    strMenu = strMenu + "mn" + prId + ".orient=" + "" + "'H'" + "" + ";" + "\r\n";
                }
                else
                {
                    strMenu = strMenu + "var mn" + prId + " = menuMgr.createMenu(" + "'mn" + prId + "'" + ");" + "\r\n";
                    strMenu = strMenu + "mn" + prId + ".target=" + "'rightPartFrame'" + ";" + "\r\n";
                    strMenu = strMenu + "mn" + prId + ".applyBorder(false, false, true, false)" + ";" + "\r\n";
                    strMenu = strMenu + "mn" + prId + ".itemSpc=" + "'0px'" + ";" + "\r\n";
                    strMenu = strMenu + "mn" + prId + ".orient=" + "" + "'V'" + "" + ";" + "\r\n";

                }


                strMenu = strMenu + "mn" + prId + ".absWidth=" + "" + "'120px'" + "" + ";" + "\r\n";


                strMenu = strMenu + "mn" + prId + ".showIcon=true;" + "\r\n";
            }

            strMenu = strMenu + "mn" + prId + ".addItem(" + "'" + menu.MenuID + "'" + ", " +
              "'" + strMenuName + "'" + ", " +
              "'" + menu.MenuURL + "'" + ", " +
              "'" + "');" + "\r\n";

            if (menu.IsLeaf == 0)
            {
                strMenu = strMenu + "mn" + prId + ".addSubmenu(" + "'" + menu.MenuID + "'" + ", " +
                "" + "'mn" + menu.MenuID + "'" + ");" + "\r\n";
            }
        }
        strMenu = strMenu + "} " + "\r\n";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MyScript", strMenu, true);



    }
    //private void InitMenu()
    //{

    //    BaseTree tree = new BaseTree(new ShowMenu());
    //    tree.CreateTree();
    //    foreach (ITreeNode tn in tree.RootNodes)
    //    {
    //        MenuItem menuRoot = new MenuItem();
    //        InitTreeNode(menuRoot, tn);
    //        this.Menu1.Items.Add(menuRoot);
    //    }
    //}

    //private void InitTreeNode(MenuItem treeNode, ITreeNode deptNode)
    //{

    //    foreach (ITreeNode dn in deptNode.GetChildren())
    //    {
    //        MenuItem tn = new MenuItem();
    //        treeNode.ChildItems.Add(tn);
    //        InitTreeNode(tn, dn);
    //    }
    //    treeNode.NavigateUrl = deptNode.GetValue();
    //    treeNode.Text = deptNode.GetText();
    //    treeNode.Target = "rightPartFrame";
    //}
}
