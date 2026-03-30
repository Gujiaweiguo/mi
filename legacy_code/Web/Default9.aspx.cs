using System;
using System.Web.UI;
using Base.XML;
using Base.Biz;
using BaseInfo.User;
using Base.DB;
using Base.Page;
using Base.Sys;
public partial class Default9 : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUser sessionUsers = (SessionUser)Session["UserAccessInfo"];
        int userid = sessionUsers.UserID;
        string user = sessionUsers.UserID.ToString();
        //显示用户名称
        BaseBO userBo = new BaseBO();
        userBo.WhereClause = "UserId = " + sessionUsers.UserID;
        Resultset rsUser = userBo.Query(new Users());
        if (rsUser.Count == 1)
        {
            Users userInfo = rsUser.Dequeue() as Users;
            this.lblUserName.Text = userInfo.UserName;
        }
        else 
        {
            this.lblUserName.Text = (String)GetGlobalResourceObject("BaseInfo", "Menu_NoUserName");
        }


        //this.Menu1.Items.Clear();
        //InitMenu();
        String strMenu = "";
        String strMenuCreated = "";
        String strMenuName = "";
        TShowMenu tShowMenu = new TShowMenu();
        
        strMenu = "var menuMgr = new NlsMenuManager(" + "'mgr1'" + ");" + "\r\n";
        strMenu = strMenu + "menuMgr.defaultEffect = " + "'fade'" + ";" + "\r\n";
        strMenu = strMenu + "function initMenu() { " + "\r\n";
        SessionUser sessionUser = (SessionUser)Session["UserAccessInfo"];
        int prId = 0;
        BaseBO baseBO = new BaseBO();
        tShowMenu.RoleID = sessionUser.RoleID;
        baseBO.OrderBy = " MenuLevel,PmenuID,MenuOrder";
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

        ////生成大区xml
        LocalXML localXML = new LocalXML();
        localXML.GetXml(user);
        ////生成城市xml
        CityXML cityXML = new CityXML();
        cityXML.GetXml(user);
        ////生成MALLxml
        ShoppingMallXML mallXML = new ShoppingMallXML();
        mallXML.GetXml(user);
        ////生成大楼xml
        BuildingXML buildingXML = new BuildingXML();
        buildingXML.GetXml(user);
        ////生成楼层xml
        FloorXML floorXML = new FloorXML();
        floorXML.GetXml(user, "");
        //生成商铺xml
        ShopXML shopXML = new ShopXML();
        shopXML.GetXml(user, "", 0);
        shopXML.GetXml(user, "", 1);
        shopXML.GetXml(user, "", 2);
        //生成快捷按钮xml
        ColorLumpXML colorXML = new ColorLumpXML();
        colorXML.GetXml(user, 0);
        colorXML.GetXml(user, 1);
        colorXML.GetXml(user, 2);
        //生成xml权限转移
        MoveXMLFiles movexml = new MoveXMLFiles();
        movexml.MoveFiles(userid);
    }



}
