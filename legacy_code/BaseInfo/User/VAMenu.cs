using System;
using System.Collections.Generic;
using System.Text;

using Base.XML;
using Base.Sys;
using BaseInfo.authUser;

namespace BaseInfo.User
{
    public class VAMenu
    {

        /// <summary>
        /// 根据项目ID生成VA菜单
        /// </summary>
        /// <param name="strStoreID">项目ID</param>
        //public static void GetVAMenu(int intStoreID,SessionUser sUser)
        //{
        //    SessionUser sessionUsers = sUser;
        //    int userid = sessionUsers.UserID;
        //    string user = sessionUsers.UserID.ToString();

        //    XmlExecutor XE1 = new XmlExecutor();
        //    //生成大楼层面菜单
        //    BuildingMenuXML BMX = new BuildingMenuXML();
        //    //生成大楼菜单内容
        //    BuildingMenuXMLInfo BMXI = new BuildingMenuXMLInfo(userid);
        //    XE1.BuildXml(BMX, BMXI, "", user);

        //    FloorMenuXML FMX = new FloorMenuXML();
        //    FloorMenuXMLInfo FMXI = new FloorMenuXMLInfo(userid);
        //    XE1.BuildXml(FMX, FMXI, "", user);

        //    ShopMenuXML SMX = new ShopMenuXML();
        //    ShopMenuXMLInfo SMXI = new ShopMenuXMLInfo(userid);
        //    XE1.BuildXml(SMX, SMXI, "", user);

        //    string wheresql = "";
        //    if (AuthBase.GetAuthUser(userid) > 0)
        //    {
        //        wheresql = "where floors.storeid=" + intStoreID + " and authuser.userid=" + userid;
        //    }
        //    else
        //    {
        //        wheresql = "where floors.storeid=" + intStoreID;
        //    }
        //    //生成floors按钮xml
        //    FloorXML FX = new FloorXML();
        //    FloorXMLInfo FXI = new FloorXMLInfo(wheresql);
        //    XE1.BuildXml(FX, FXI, "", user);

        //    ShopXML sx = new ShopXML();
        //    ShopXMLInfo sxi = new ShopXMLInfo();
        //    XE1.BuildXml(sx, sxi, "", user);

        //}
    }
}
