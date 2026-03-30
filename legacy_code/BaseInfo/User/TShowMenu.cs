using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
using Base.Biz;
using Base.Util;

namespace BaseInfo.User
{
    public class TShowMenu:BasePO
    {
        private int menuID = 0;
        private string menuName = "";
        private int menuLevel = 0;
        private int pMenuID = 0;
        private string menuURL = "";
        private int menuOrder = 0;
        private int isleaf = 0;
        private int roleID = 0;
        private string baseInfo = "";


        #region  菜单信息

        public int MenuID
        {
            get { return menuID; }
            set { menuID = value; }
        }
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }
        public int MenuLevel
        {
            get { return menuLevel; }
            set { menuLevel = value; }
        }
        public int PMenuID
        {
            get { return pMenuID; }
            set { pMenuID = value; }
        }
        public string MenuURL
        {
            get { return menuURL; }
            set { menuURL = value; }
        }
        public int MenuOrder
        {
            get { return menuOrder; }
            set { menuOrder = value; }
        }
        public int IsLeaf
        {
            get { return isleaf; }
            set { isleaf = value; }
        }

        public int RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }

        public string BaseInfo
        {
            get { return baseInfo; }
            set { baseInfo = value; }
        }

        #endregion

        //得到表
        public override String GetTableName()
        {
            return "TMenu";
        }



        //得到要查询的列名
        public override String GetColumnNames()
        {
            return "MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo";
        }
        public override String GetUpdateColumnNames()
        {
            return "MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo";
        }

        public override string GetQuerySql()
        {
           // return "SELECT MenuID,MenuName,MenuLevel,PMenuID,replace(menuURL,'65537',ISNULL((SELECT MIN(funcID) FROM func WHERE func.menuid = menuInfo.menuid and funcType =0  ),0)) AS MenuURL,MenuOrder,IsLeaf,BaseInfo " +
                   //" FROM ( " +
                   //"        SELECT MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo " +
                   //"          FROM tmenu " +
                   //"         WHERE tmenu.menuID in ( " +
                   //"        SELECT DISTINCT tmenu.PmenuID " +
                   //"          FROM tmenu INNER JOIN " +
                   //"               (SELECT DISTINCT ( CASE m1.menuLevel WHEN '3' THEN m1.PmenuID ELSE m1.menuID END ) AS PmenuID " +
                   //"                  FROM tmenu AS m1 " +
                   //"                 WHERE EXISTS (SELECT 1  " +
                   //"                                 FROM func INNER JOIN  " +
                   //"                                      roleAuth ON (func.funcid = roleAuth.funcid) " +
                   //"                                WHERE roleid = " + roleID + "  " +
                   //"                                  AND func.menuID = m1.menuID " +
                   //"                               ) " +
                   //"                   AND isleaf=1 " +
                   //"                ) AS m2 ON tmenu.menuID = m2.PmenuID " +
                   //"          AND tmenu.menulevel = 2 " +
                   //"        ) " +
                   //"        UNION ALL " +
                   //"        SELECT MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo " +
                   //"          FROM tmenu INNER JOIN " +
                   //"               (SELECT DISTINCT ( CASE m1.menuLevel WHEN '3' THEN m1.PmenuID ELSE m1.menuID END ) AS parentMenuID " +
                   //"                  FROM tmenu AS m1 " +
                   //"                 WHERE EXISTS (SELECT 1  " +
                   //"                                 FROM func INNER JOIN  " +
                   //"                                      roleAuth ON (func.funcid = roleAuth.funcid) " +
                   //"                                WHERE roleid = " + roleID + "  " +
                   //"                                  AND func.menuID = m1.menuID " +
                   //"                               ) " +
                   //"                   AND isleaf=1 " +
                   //"                ) AS m2 ON tmenu.menuID = m2.parentMenuID " +
                   //"          AND tmenu.menulevel = 2 " +
                   //"        UNION ALL " +
                   //"        SELECT  MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo " +
                   //"          FROM tmenu  " +
                   //"         WHERE EXISTS (SELECT 1  " +
                   //"                         FROM func INNER JOIN  " +
                   //"                              roleAuth ON (func.funcid = roleAuth.funcid) " +
                   //"                        WHERE roleid = " + roleID + "  " +
                   //"                          AND func.menuID = tmenu.menuID " +
                   //"                       ) " +
                   //"           AND isleaf=1 " +
                   //"           AND menuLevel = 3 " +
                   //"        UNION ALL " +  //增加3级子菜单
                   //"        SELECT  MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo " +
                   //"          FROM tmenu  " +
                   //"           where tmenu.menuID in ( select DISTINCT tmenu.PmenuID FROM tmenu INNER JOIN " +
                   //"                              (SELECT DISTINCT ( CASE m1.menuLevel WHEN '5' THEN m1.PmenuID ELSE m1.menuID END ) AS PmenuID " +
                   //"                                       FROM tmenu AS m1 WHERE EXISTS (SELECT 1 " +
                   //"                                            FROM func INNER JOIN  " +
                   //"                                            roleAuth ON (func.funcid = roleAuth.funcid) " +
                   //"                                            WHERE roleid = " + roleID + "  " +
                   //"                                            AND func.menuID = m1.menuID " +
                   //") AND isleaf=1" +
                   //"    ) AS m2 ON tmenu.menuID = m2.PmenuID AND tmenu.menulevel = 4" +
                   //") " +
                   //"        UNION ALL " +   //增加4级命令菜单
                   //"        SELECT  MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo " +
                   //"          FROM tmenu  " +
                   //"         WHERE EXISTS (SELECT 1  " +
                   //"                         FROM func INNER JOIN  " +
                   //"                              roleAuth ON (func.funcid = roleAuth.funcid) " +
                   //"                        WHERE roleid = " + roleID + "  " +
                   //"                          AND func.menuID = tmenu.menuID " +
                   //"                       ) " +
                   //"           AND isleaf=1 " +
                   //"           AND menuLevel = 4 " +  
                   //"        ) AS menuInfo " +
                   //" ";
            return "SELECT MenuID,MenuName,MenuLevel,PMenuID,replace(menuURL,'65537',ISNULL((SELECT MIN(funcID) FROM func WHERE func.menuid = menuInfo.menuid and funcType =0  ),0)) AS MenuURL,MenuOrder,IsLeaf,BaseInfo" +
                    " FROM ( " +
                    "       SELECT MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo" +
                    "       FROM tmenu WHERE tmenu.menuID in (" +
                    "           SELECT DISTINCT tmenu.PmenuID FROM tmenu INNER JOIN" +
                    "               (SELECT DISTINCT ( CASE m1.menuLevel WHEN '3' THEN m1.PmenuID ELSE m1.menuID END ) AS PmenuID " +
                    "                FROM tmenu AS m1 WHERE EXISTS " +
                    "                   (SELECT 1  FROM func INNER JOIN roleAuth ON (func.funcid = roleAuth.funcid) WHERE func.menuID = m1.menuID AND roleid =" + roleID + "  )" +
                 //   "               AND isleaf=1" +
                    "           ) AS m2 ON tmenu.menuID = m2.PmenuID AND tmenu.menulevel = 2" +
                    "      ) and tmenu.menulevel=1" +   //一级菜单
                    " UNION ALL" +
                    "       SELECT MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo FROM tmenu WHERE tmenu.menuID in (" +
                    "           SELECT DISTINCT tmenu.PmenuID FROM tmenu INNER JOIN " +
                    "               (SELECT DISTINCT ( CASE m1.menuLevel WHEN '4' THEN m1.PmenuID ELSE m1.menuID END ) AS PmenuID FROM tmenu AS m1 WHERE EXISTS " +
                    "                   (SELECT 1  FROM func INNER JOIN roleAuth ON (func.funcid = roleAuth.funcid) WHERE func.menuID = m1.menuID AND roleid =" + roleID + " )" +
                    "                AND isleaf=1 " +
                    "           ) AS m2 ON tmenu.menuID = m2.PmenuID " + //AND tmenu.menulevel = 3" +
                    "       ) and tmenu.menulevel=2 " +  //二级菜单
                    " UNION ALL" +
                    "       SELECT MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo FROM tmenu INNER JOIN" +
                    "           (SELECT DISTINCT  m1.menuID AS parentMenuID FROM tmenu AS m1 WHERE EXISTS" +
                    "               (SELECT 1  FROM func INNER JOIN roleAuth ON (func.funcid = roleAuth.funcid) WHERE func.menuID = m1.menuID AND roleid =" + roleID + "  )" +
                    "           AND isleaf=1 and m1.menulevel=2" +
                    "       ) AS m2 ON tmenu.menuID = m2.parentMenuID AND tmenu.menulevel = 2" + //二级命令
                    " UNION ALL" +
                    "       SELECT MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo FROM tmenu WHERE tmenu.menuID in" +
                    "           (SELECT DISTINCT tmenu.PmenuID FROM tmenu INNER JOIN" +
                    "               (SELECT DISTINCT ( CASE m1.menuLevel WHEN '5' THEN m1.PmenuID ELSE m1.menuID END ) AS PmenuID FROM tmenu AS m1 WHERE EXISTS" +
                    "                   (SELECT 1  FROM func INNER JOIN roleAuth ON (func.funcid = roleAuth.funcid) WHERE func.menuID = m1.menuID AND roleid =" + roleID + "  )" +
                    "                AND isleaf=1" +
                    "           ) AS m2 ON tmenu.menuID = m2.PmenuID AND tmenu.menulevel = 4" +
                    "       ) and tmenu.menulevel=3" +  //三级菜单
                    " UNION ALL" +
                    "       SELECT MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo FROM tmenu INNER JOIN" +
                    "           (SELECT DISTINCT  m1.menuID AS parentMenuID FROM tmenu AS m1 WHERE EXISTS" +
                    "                 (SELECT 1  FROM func INNER JOIN roleAuth ON (func.funcid = roleAuth.funcid) WHERE func.menuID = m1.menuID AND roleid =" + roleID + "  )" +
                    "           AND isleaf=1 and m1.menulevel=3" +
                    "       ) AS m2 ON tmenu.menuID = m2.parentMenuID AND tmenu.menulevel = 3" +  //三级命令
                    " UNION ALL" +
                    "       SELECT  MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf,BaseInfo FROM tmenu WHERE EXISTS" +
                    "           (SELECT 1  FROM func INNER JOIN roleAuth ON (func.funcid = roleAuth.funcid) WHERE func.menuID = tmenu.menuID AND roleid =" + roleID + "  )" +
                    "       AND isleaf=1 AND menuLevel = 4 " +  //四级命令
                    ") AS menuInfo ";
            
        }



        private List<ITreeNode> parents = new List<ITreeNode>();
        private ITreeNode parent = null;


        /**
         * 获取节点的值
         */
        public String GetValue()
        {
            return this.MenuURL;
        }

        /**
         * 获取节点的显示文本
         */
        public String GetText()
        {
            return this.MenuName;
        }

        /**
         * 获得节点的提示信息
         */
        public String GetTip()
        {
            return "";
        }

        /**
         * 获取子节点的集合
         */
        public List<ITreeNode> GetParents()
        {
            return this.parents;
        }


        /**
         * 添加子节点
         */
        public void AddChild(ITreeNode childNode)
        {
            this.parents.Add(childNode);
        }

        /**
         * 获取父节点
         */
        public ITreeNode GetParent()
        {
            return this.parent;
        }

        /**
         * 获取根节点时使用的where条件，格式如："pMenuId="+pMenuId
         */
        public String GetChildrenWhere()
        {
            return "MenuID=" + this.PMenuID;
        }
        /**
          * 获取根结点时需要的where条件，格式如："DeptLevle=1"
          */
        public String GetLeafWhere()
        {
            return "";
        }
    }
}
