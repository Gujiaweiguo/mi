using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
using Base.Util;

namespace Base.Biz
{
    public class BaseTree
    {
        /**
         * 树的节点，用来获取具体的节点信息
         */
        private ITreeNode treeNode = null;
        
        /**
         * 树根结点的列表，支持多个根结点
         */
        private List<ITreeNode> rootNodes = new List<ITreeNode>();

        public BaseTree(ITreeNode treeNode)
        {
            this.treeNode = treeNode;
        }

        /**
         * 创建树
         */
        public void CreateTree()
        {
            if (rootNodes != null)
            {
                rootNodes.Clear();
            }
            BaseBO bo = new BaseBO();
            bo.WhereClause = treeNode.GetRootWhere();
            Resultset rs = bo.Query(treeNode as BasePO);
            foreach (ITreeNode rootNode in rs)
            {
                rootNodes.Add(rootNode);
                RecursiveCreate(rootNode);
            }
        }

        public List<ITreeNode> RootNodes
        {
            get { return this.rootNodes; }
        }

        public void PrintNodes()
        {
            foreach (ITreeNode rootNode in this.RootNodes)
            {
                PrintNode(rootNode,1);
            }
        }

        /**
         * 递归创建子节点
         */
        private void RecursiveCreate(ITreeNode parentNode)
        {
            BaseBO bo = new BaseBO();
            bo.WhereClause = parentNode.GetChildrenWhere();
            Resultset rs = bo.Query(parentNode as BasePO);
            if (rs.Count == 0)
            {
                return;
            }
            foreach (ITreeNode childNode in rs)
            {
                parentNode.AddChild(childNode);
                childNode.SetParent(parentNode);
                RecursiveCreate(childNode);
            }
        }


        private void PrintNode(ITreeNode node,int level)
        {
            String s = "";
            for (int i = 0; i < level; i++)
            {
                s += "--";
            }

            s += "[" + node.GetValue() + ":" + node.GetText() +"-"+(node.GetParent()==null?"":node.GetParent().GetValue()+":"+node.GetParent().GetText())+"]";
            Logger.Log(s);
            level++;
            foreach (ITreeNode tr in node.GetChildren())
            {
                PrintNode(tr, level);
            }
        }
    }
}
