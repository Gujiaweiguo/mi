using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
using Base.Biz;
using Base.Util;

namespace BaseInfo.User
{
    public class ShowMenu : BasePO, ITreeNode
    {
        private int menuID = 0;
        private string menuName = null;
        private int menuLevel = 0;
        private int pMenuID = 0;
        private string menuURL = null;
        private int menuOrder = 0;
        private int isleaf = 0;

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

        #endregion

        //得到表
        public override String GetTableName()
        {
            return "[Menu]";
        }



        //得到要查询的列名
        public override String GetColumnNames()
        {
            return "MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf";
        }
        public override String GetUpdateColumnNames()
        {
            return "MenuID,MenuName,MenuLevel,PMenuID,MenuURL,MenuOrder,IsLeaf";
        }


        private List<ITreeNode> children = new List<ITreeNode>();
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
        public List<ITreeNode> GetChildren()
        {
            return this.children;
        }

        /**
         * 设置父节点
         */
        public void SetParent(ITreeNode parent)
        {
            this.parent = parent;
        }

          /**
         * 添加子节点
         */
        public void AddChild(ITreeNode childNode)
        {
            this.children.Add(childNode);
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
            return "PMenuID=" + this.MenuID;
        }
        /**
          * 获取根结点时需要的where条件，格式如："DeptLevle=1"
          */
        public String GetRootWhere()
        {
            return "PMenuID=0";
        }

    }
}
