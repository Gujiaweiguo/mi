using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Base.Page;
using WorkFlow.WrkFlw;
using Base.Util;
using Base.Biz;
using BaseInfo.Role;

namespace WorkFlow.Uiltil
{
    public class WrkFlwTree
    {
        private const String ROOT_NODE_TEXT = "业务单据处理";
        private const String ROOT_NODE_VALUE = "业务单据处理";

        private const String IMAGE_LEVEL1 = "~/App_Themes/CSS/Images/Level1.gif";
        private const String IMAGE_LEVEL2 = "~/App_Themes/CSS/Images/Level2.gif";
        private const String IMAGE_LEVEL3 = "~/App_Themes/CSS/Images/Level3.gif";


        private Queue<WrkFlwTreeNode> rootNodes = new Queue<WrkFlwTreeNode>();
        /**
         * 创建指定用户角色的工作流树
         */

        public void BuildTree(int deptID,int roleID,int bizGrpID)
        {
            //创建业务单据处理树
            BuildBizTree(deptID,roleID, bizGrpID);
            //创建送上级审批单据处理树
            BuildMgrTree(roleID);
        }
        /**
         * 创建业务单据处理树
         */
        public void BuildBizTree(int deptID,int roleID,int bizGrpID)
        {
            //首层
            WrkFlwDef[] wrkFlwDefs = WrkFlwApp.GetWrkFlwDefs(roleID, bizGrpID);
            //生成已定义给该角色的工作流树
            if (wrkFlwDefs != null)
            {
                WrkFlwTreeNode rootNode = new WrkFlwTreeNode(ROOT_NODE_TEXT, ROOT_NODE_VALUE, IMAGE_LEVEL1);
                WrkFlwTreeNode lay2Node = null;
                WrkFlwTreeNode lay3Node = null;
                for (int i = 0; i < wrkFlwDefs.Length; i++)
                {
                    WrkFlwDef wrkFlwDef = wrkFlwDefs[i];
                    //第二层：业务组
                    //if (lay2Node == null || !lay2Node.NodeValue.Equals(wrkFlwDef.BizGrpName))
                    //{
                    //    lay2Node = new WrkFlwTreeNode(wrkFlwDef.BizGrpName, wrkFlwDef.BizGrpName, IMAGE_LEVEL2);
                    //    lay2Node.ParentNode = rootNode;
                    //    rootNode.ChildNodes.Add(lay2Node);
                    //}
                    //第三层：工作流
                    if (lay3Node == null || !lay3Node.NodeValue.Equals(wrkFlwDef.WrkFlwName))
                    {
                        lay3Node = new WrkFlwTreeNode(wrkFlwDef.WrkFlwName, wrkFlwDef.WrkFlwName, IMAGE_LEVEL3);
                        //lay3Node.ParentNode = lay2Node;
                        //lay2Node.ChildNodes.Add(lay3Node);
                    }
                    //第四层：业务处理
                    //如果是首节点，需要判断是不是制作单据
                    if (wrkFlwDef.WrkStep == WrkFlwNode.FIRST_WRK_STEP)
                    {
                        //如果制作单据，则显示相应的功能URL
                        if (wrkFlwDef.InitVoucher == WrkFlw.WrkFlw.INIT_VOUCHER_YES)
                        {
                            WrkFlwTreeNode wrkFlwTreeNode = new WrkFlwTreeNode(wrkFlwDef.NodeName, wrkFlwDef.FuncURL + "?WrkFlwID=" + wrkFlwDef.WrkFlwID + "&NodeID=" + wrkFlwDef.NodeID + "&Type=New", wrkFlwDef.ImageURL);
                            wrkFlwTreeNode.ParentNode = lay3Node;
                            lay3Node.ChildNodes.Add(wrkFlwTreeNode);
                        }
                    }
                    //查询运行实体节点(无论是否首节点，都要查询运行实体，因为首节点可能存在被驳回的单据)
                    //WrkFlwEntityInfo[] entities = WrkFlwApp.GetWrkFlwEntityInfos(roleID, wrkFlwDef.WrkFlwID, wrkFlwDef.NodeID);
                    WrkFlwEntityInfo[] entities = WrkFlwApp.GetWrkFlwEntityInfos(deptID, roleID, wrkFlwDef.WrkFlwID, wrkFlwDef.NodeID);
  
                    if (entities != null)
                    {
                        int count = 0;
                        foreach (WrkFlwEntityInfo entity in entities)
                        {
                            WrkFlwTreeNode wrkFlwTreeNode = new WrkFlwTreeNode(entity.VoucherHints, entity.FuncURL + "?WrkFlwID=" + entity.WrkFlwID + "&NodeID=" + entity.NodeID + "&Sequence=" + entity.Sequence + "&VoucherID=" + entity.VoucherID + "&Type=Old", entity.ImageURL);
                            wrkFlwTreeNode.ParentNode = lay3Node;
                            lay3Node.ChildNodes.Add(wrkFlwTreeNode);
                            count++;
                        }
                        lay3Node.IncreaseCount(count);
                    }
                    rootNodes.Enqueue(lay3Node);
                }
            }
        }
        /**
         * 创建送上级审批单据处理树
         */
        public void BuildMgrTree(int roleID)
        {
            //根据运行实体，生成该角色待审批的“提交上级审批”的单据
            //判断该角色是不是领导角色
            String sql = "select IsLeader from Role where RoleID=" + roleID;
            BaseBO bo = new BaseBO();
            DataSet ds = bo.QueryDataSet(sql);
            int isLeader = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            if (isLeader != Role.IS_LEADER_YES)
            {
                return;
            }
            //首层
            //获取该角色待审批的“提交上级审批”的单据
            WrkFlwMgrEntityInfo[] mgrEntities = WrkFlwApp.GetWrkFlwMgrEntityInfos(roleID);
            //生成单据树
            if (mgrEntities != null)
            {
                WrkFlwTreeNode rootNode = new WrkFlwTreeNode("报送审批单据", "报送审批单据", IMAGE_LEVEL1);
                WrkFlwTreeNode lay2Node = null;
                WrkFlwTreeNode lay3Node = null;
                for (int i = 0; i < mgrEntities.Length; i++)
                {
                    WrkFlwMgrEntityInfo mgrEntity = mgrEntities[i];
                    //第二层：业务组
                    if (lay2Node == null || !lay2Node.NodeValue.Equals(mgrEntity.BizGrpName))
                    {
                        lay2Node = new WrkFlwTreeNode(mgrEntity.BizGrpName, mgrEntity.BizGrpName, IMAGE_LEVEL2);
                        rootNode.ChildNodes.Add(lay2Node);
                    }
                    //第三层：工作流
                    if (lay3Node == null || !lay3Node.NodeValue.Equals(mgrEntity.WrkFlwName))
                    {
                        lay3Node = new WrkFlwTreeNode(mgrEntity.WrkFlwName, mgrEntity.WrkFlwName, IMAGE_LEVEL3);
                        lay2Node.ChildNodes.Add(lay3Node);
                    }
                    WrkFlwTreeNode wrkFlwTreeNode = new WrkFlwTreeNode(mgrEntity.VoucherHints, mgrEntity.FuncURL + "?WrkFlwID=" + mgrEntity.WrkFlwID + "&NodeID=" + mgrEntity.NodeID + "&Sequence=" + mgrEntity.Sequence + "&VoucherID=" + mgrEntity.VoucherID, mgrEntity.ImageURL);
                    lay3Node.ChildNodes.Add(wrkFlwTreeNode);
                    lay3Node.IncreaseCount(1);
                }

                RootNodes.Enqueue(rootNode);
            }

        }

        public Queue<WrkFlwTreeNode> RootNodes
        {
            get { return this.rootNodes; }
        }

        public void PrintTree()
        {
            foreach (WrkFlwTreeNode rootNode in rootNodes)
            {
                PrintNode(rootNode, 1);
            }
        }

        private void PrintNode(WrkFlwTreeNode node, int level)
        {
            String s = "";
            for (int i = 0; i < level; i++)
            {
                s += "--";
            }

            s += "[" + node.NodeText + ":" + node.NodeValue + ":" + node.ImageURL + "]";
            Logger.Log(s);
            level++;
            foreach (WrkFlwTreeNode tr in node.ChildNodes)
            {
                PrintNode(tr, level);
            }
        }
    }
}
