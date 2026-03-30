using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;

using Base;
using Base.Biz;
using Base.DB;

using BaseInfo.Role;

using Lease.SMSPara;

using BaseInfo.User;
using WorkFlow.WrkFlw;
using WorkFlow.Uiltil;
using WorkFlow.Func;
using WorkFlow.WorkFlowMail;

namespace WorkFlow
{
    /**
     * 该类实现了工作流调用接口的方法
     */
    public class WrkFlwApp
    {

        public static int returnSequence = 0;




        /**
         * 根据指定的用户角色，获取该角色对应的工作流定义信息
         */
        public static WrkFlwDef[] GetWrkFlwDefs(int roleID,int bizGrpID)
        {
            //从数据库中查询指定角色的工作流定义节点
            BaseBO bo = new BaseBO();
            WrkFlwDef po = new WrkFlwDef();
            po.SetRoleID(roleID, bizGrpID);
            bo.OrderBy = "a.BizGrpID, b.WrkFlwID";
            Resultset rs = bo.Query(po);
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwDef[] wrkFlwDefs = new WrkFlwDef[rs.Count];
                int i = 0;
                foreach (WrkFlwDef wrkFlwDef in rs)
                {
                    wrkFlwDefs[i++] = wrkFlwDef;
                }
                return wrkFlwDefs;
            }
            return null;
        }
        
        /**
         * 根据指定的用户角色、工作流内码、节点内码，获取未处理的工作流运行实体节点信息（包括功能URL）
         */
        public static WrkFlwEntityInfo[] GetWrkFlwEntityInfos(int roleID, int wrkFlwID,int nodeID)
        {
            //从数据库中查询指定角色的工作流运行实体节点
            BaseBO bo = new BaseBO();
            bo.AppendWhere = false;
            bo.WhereClause = "b.RoleID=" + roleID + " and a.WrkFlwID=" + wrkFlwID + " and a.NodeID=" + nodeID
                + " and a.NodeStatus in (" + WrkFlwEntity.NODE_STATUS_NORMAL_PENDING + "," + WrkFlwEntity.NODE_STATUS_REJECT_PENDING + "," + WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT + ")";
            bo.OrderBy = "StartTime";
            Resultset rs = bo.Query(new WrkFlwEntityInfo());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwEntityInfo[] entities = new WrkFlwEntityInfo[rs.Count];
                int i = 0;
                foreach (WrkFlwEntityInfo entity in rs)
                {
                    entities[i++] = entity;
                }
                return entities;
            }
            return null;
        }

        /**
         * 根据指定的用户部门、角色、工作流内码、节点内码，获取未处理的工作流运行实体节点信息（包括功能URL）
         */
        public static WrkFlwEntityInfo[] GetWrkFlwEntityInfos(int deptID,int roleID, int wrkFlwID, int nodeID)
        {
            //从数据库中取得当前用户部门的子部门
            BaseBO bo = new BaseBO();
            //string strDept = BaseInfo.Dept.Dept.GetPDeptID(deptID);
            string strDept = deptID.ToString();
            //从数据库中查询指定角色的工作流运行实体节点
            bo = new BaseBO();
            bo.AppendWhere = false;
            bo.WhereClause = "b.RoleID=" + roleID + " and a.WrkFlwID=" + wrkFlwID + " and a.NodeID=" + nodeID
                + " and a.NodeStatus in (" + WrkFlwEntity.NODE_STATUS_NORMAL_PENDING + "," + WrkFlwEntity.NODE_STATUS_REJECT_PENDING + "," + WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT + ")" +
                " and a.deptid in (" + strDept + ")";
            bo.OrderBy = "StartTime";
            Resultset rs = bo.Query(new WrkFlwEntityInfo());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwEntityInfo[] entities = new WrkFlwEntityInfo[rs.Count];
                int i = 0;
                foreach (WrkFlwEntityInfo entity in rs)
                {
                    entities[i++] = entity;
                }
                return entities;
            }
            return null;
        }

        /**
         * 根据指定的用户角色，获取未处理的，提交上级审批的工作流运行实体节点信息（包括功能URL）
         */
        public static WrkFlwMgrEntityInfo[] GetWrkFlwMgrEntityInfos(int roleID)
        {
            //从数据库中查询指定角色的工作流运行实体节点
            BaseBO bo = new BaseBO();
            bo.AppendWhere = false;
            bo.WhereClause = "a.RoleID=" + roleID + " and a.NodeStatus=" + WrkFlwEntity.NODE_STATUS_MGR_PENDING;
            bo.OrderBy = "StartTime";
            Resultset rs = bo.Query(new WrkFlwMgrEntityInfo());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwMgrEntityInfo[] entities = new WrkFlwMgrEntityInfo[rs.Count];
                int i = 0;
                foreach (WrkFlwMgrEntityInfo entity in rs)
                {
                    entities[i++] = entity;
                }
                return entities;
            }
            return null;
        }

        /**
         * 根据指定的工作流内码、节点内码，获取工作节点定义信息
         */
        public static WrkFlwNode GetWrkFlwNode(int wrkFlwID, int nodeID)
        {
            BaseBO bo = new BaseBO();
            bo.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID;
            Resultset rs = bo.Query(new WrkFlwNode());
            if (rs.Count > 0)
            {
                return rs.Dequeue() as WrkFlwNode;
            }
            throw new WrkFlwException("no workflow node defination for workflowid:" + wrkFlwID + "-nodeid:" + nodeID);
        }
        /**
         * 根据指定的工作流内码，获取该工作流首节点定义信息
         */
        public static WrkFlwNode GetFirstWrkFlwNode(int wrkFlwID)
        {
            BaseBO bo = new BaseBO();
            bo.WhereClause = "WrkFlwID=" + wrkFlwID + " and WrkStep=1";
            Resultset rs = bo.Query(new WrkFlwNode());
            if (rs.Count > 0)
            {
                return rs.Dequeue() as WrkFlwNode;
            }
            throw new WrkFlwException("no workflow node defination for workflowid:" + wrkFlwID);
        }
        /**
         * 根据指定的工作流内码，获取该工作流末节点定义信息
         */
        public static WrkFlwNode GetLastWrkFlwNode(int wrkFlwID)
        {
            BaseBO bo = new BaseBO();
            bo.WhereClause = "WrkFlwID=" + wrkFlwID;
            bo.OrderBy = "WrkStep DESC";
            Resultset rs = bo.Query(new WrkFlwNode());
            if (rs.Count > 0)
            {
                return rs.Dequeue() as WrkFlwNode;
            }
            throw new WrkFlwException("no workflow node defination for workflowid:" + wrkFlwID);
        }

        /**
         * 根据指定的工作流内码、节点内码、序列号，获取工作流节点运行实体信息
         */
        public static WrkFlwEntity GetWrkFlwEntity(int wrkFlwID, int nodeID, int sequence)
        {
            BaseBO bo = new BaseBO();
            bo.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            Resultset rs = bo.Query(new WrkFlwEntity());
            if (rs.Count > 0)
            {
                return rs.Dequeue() as WrkFlwEntity;
            }
            throw new WrkFlwException("no workflow entity for workflowid:" + wrkFlwID + "-nodeid:" + nodeID + "-sequence:" + sequence);
        }

        /**
         * 根据指定的工作流内码、节点内码，获取后续的工作节点定义信息
         */
        public static WrkFlwNode[] GetNextWrkFlwNodes(int wrkFlwID, int nodeID)
        {
            //获得当前节点
            WrkFlwNode wrkFlwNode = GetWrkFlwNode(wrkFlwID, nodeID);
            //获得后续节点
            BaseBO bo = new BaseBO();
            bo.WhereClause = "WrkFlwID=" + wrkFlwID + " and WrkStep=" + (wrkFlwNode.WrkStep + 1);
            Resultset rs = bo.Query(new WrkFlwNode());
            if (rs.Count > 0)
            {
                WrkFlwNode[] nodes = new WrkFlwNode[rs.Count];
                int i = 0;
                foreach (WrkFlwNode node in rs)
                {
                    nodes[i++] = node;
                }
                return nodes;
            }
            return null;
        }

        /**
         * 根据指定的工作流内码、单据代码，获取全部的工作流运行实体节点信息
         */
        public static WrkFlwEntity[] GetWrkFlwEntities(int wrkFlwID, int voucherID)
        {
            //从数据库中查询指定工作流内码、单据代码的工作流运行实体
            BaseBO bo = new BaseBO();
            bo.WhereClause = "WrkFlwID=" + wrkFlwID + " and VoucherID=" + voucherID;
            bo.OrderBy = "Sequence";
            Resultset rs = bo.Query(new WrkFlwEntity());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwEntity[] entities = new WrkFlwEntity[rs.Count];
                int i = 0;
                foreach (WrkFlwEntity entity in rs)
                {
                    entities[i++] = entity;
                }
                return entities;
            }
            return null;
        }

        /**
         * 根据指定的工作流内码、序列号、单据代码，获取指定工作流运行实体（sequence）之前的全部的工作流运行实体节点信息,驳回单据
         */
        public static WrkFlwEntityDisprove[] GetWrkFlwEntitiesDisprove(int wrkFlwID, int sequence, int voucherID)
        {
            //从数据库中查询指定工作流内码、单据代码的工作流运行实体
            BaseBO bo = new BaseBO();
            bo.WhereClause = "a.WrkFlwID=" + wrkFlwID + " and VoucherID=" + voucherID + " and Sequence<" + sequence;
            bo.OrderBy = "Sequence";
            Resultset rs = bo.Query(new WrkFlwEntityDisprove());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwEntityDisprove[] entities = new WrkFlwEntityDisprove[rs.Count];
                int i = 0;
                foreach (WrkFlwEntityDisprove entity in rs)
                {
                    entities[i++] = entity;
                }
                return entities;
            }
            return null;
        }

        /**
         * 根据指定的工作流内码、序列号、单据代码，获取指定工作流运行实体（sequence）之前的全部的工作流运行实体节点信息
         */
        public static WrkFlwEntity[] GetWrkFlwEntities(int wrkFlwID, int sequence, int voucherID)
        {
            //从数据库中查询指定工作流内码、单据代码的工作流运行实体
            BaseBO bo = new BaseBO();
            bo.WhereClause = "WrkFlwID=" + wrkFlwID + " and VoucherID=" + voucherID + " and Sequence<" + sequence;
            bo.OrderBy = "Sequence";
            Resultset rs = bo.Query(new WrkFlwEntity());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwEntity[] entities = new WrkFlwEntity[rs.Count];
                int i = 0;
                foreach (WrkFlwEntity entity in rs)
                {
                    entities[i++] = entity;
                }
                return entities;
            }
            return null;
        }

        /**
         * 提交单据，只有制单节点才会调用该方法，外部不需要使用同一个事务保存数据
         */
        public static void CommitVoucher(int wrkFlwID, int nodeID, VoucherInfo voucherInfo)
        {
            CommitVoucher(wrkFlwID, nodeID, WrkFlwEntity.NODE_STATUS_NORMAL_COMPLETED, voucherInfo);
        }

        /**
         * 提交单据，只有制单节点才会调用该方法，外部不需要使用同一个事务保存数据，需要指定节点的状态
         */
        public static void CommitVoucher(int wrkFlwID, int nodeID, int nodeStatus, VoucherInfo voucherInfo)
        {
            BaseTrans trans = new BaseTrans();
            trans.BeginTrans();
            CommitVoucher(wrkFlwID, nodeID, nodeStatus, voucherInfo, trans);
            trans.Commit();
        }
        /**
       * 提交单据，只有制单节点才会调用该方法，外部需要使用同一个事务保存数据
       */
        public static void CommitVoucher(int wrkFlwID, int nodeID, VoucherInfo voucherInfo, BaseTrans trans)
        {
            CommitVoucher(wrkFlwID, nodeID, WrkFlwEntity.NODE_STATUS_NORMAL_COMPLETED, voucherInfo, trans);
        }
        /**
         * 提交单据，只有制单节点才会调用该方法，外部需要使用同一个事务保存数据，需要指定节点的状态
         */
        public static void CommitVoucher(int wrkFlwID, int nodeID, int nodeStatus, VoucherInfo voucherInfo, BaseTrans trans)
        {
            //获取后续节点（由于首节点必须有后续节点，所以首先判断，也为了在后续处理中，不用中断数据库事务）
            WrkFlwNode[] nodes = GetNextWrkFlwNodes(wrkFlwID, nodeID);
            //没有后续节点
            if (nodes == null)
            {
                throw new WrkFlwException("not next workflow node for first node.wrkflwid:" + wrkFlwID + "-nodeid:" + nodeID);
            }
            //插入首节点的运行实体
            int sequence = BaseApp.GetWrkFlwSequence();
            WrkFlwEntity thisEntity = new WrkFlwEntity(wrkFlwID, nodeID, sequence);
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.StartTime = DateTime.Now;
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = nodeStatus;
            trans.Insert(thisEntity);
            //只有首节点是已完成的节点，才插入后续节点
            if (nodeStatus == WrkFlwEntity.NODE_STATUS_NORMAL_COMPLETED)
            {
                //插入紧邻的后续节点
                for (int i = 0; i < nodes.Length; i++)
                {
                    WrkFlwEntity nextEntity = new WrkFlwEntity(wrkFlwID, nodes[i].NodeID, BaseApp.GetWrkFlwSequence());
                    nextEntity.SetVoucherInfo(voucherInfo);
                    nextEntity.VoucherMemo = "";
                    nextEntity.StartTime = DateTime.Now;
                    nextEntity.CompletedTime = DateTime.MaxValue; ;
                    nextEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_NORMAL_PENDING;
                    nextEntity.PreSequence = sequence;
                    trans.Insert(nextEntity);
                }
            }
        }


        /**
         * 提交草稿单据，只有制单节点才会调用该方法，外部需要使用同一个事务保存数据，需要指定节点的状态
         */
        public static void CommitVoucherDraft(int wrkFlwID, int nodeID, VoucherInfo voucherInfo)
        {
            BaseTrans trans = new BaseTrans();
            trans.BeginTrans();
            CommitVoucherDraft(wrkFlwID, nodeID, WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT, voucherInfo, trans);
            trans.Commit();
        }
        /**
         * 提交草稿单据，只有制单节点才会调用该方法，外部需要使用同一个事务保存数据，需要指定节点的状态
         */
        public static void CommitVoucherDraft(int wrkFlwID, int nodeID, int nodeStatus, VoucherInfo voucherInfo, BaseTrans trans)
        {
            //插入首节点的运行实体
            int sequence = BaseApp.GetWrkFlwSequence();
            WrkFlwEntity thisEntity = new WrkFlwEntity(wrkFlwID, nodeID, sequence);
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.StartTime = DateTime.Now;
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = nodeStatus;
            trans.Insert(thisEntity);
            returnSequence = sequence;
        }

        /**
         * 确认（审核）单据，只有非制单(审批)节点才会调用该方法（领导审批使用相同的处理）
         */
        public static void ConfirmVoucher(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo)
        {
            BaseTrans trans = new BaseTrans();
            trans.BeginTrans();

            ConfirmVoucher(wrkFlwID, nodeID, sequence, voucherInfo, trans);

            trans.Commit();
        }

        /**
         * 确认（审核）单据，只有非制单(审批)节点才会调用该方法（领导审批使用相同的处理），外部需要使用同一个事务保存数据
         */
        public static void ConfirmVoucher(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo, BaseTrans trans)
        {
            //1、获得当前并行节点的数量（如果是1，则该节点确认后，即可继续流转，如果大于1，则该节点确认后，
            //   则不可继续流转，在这里提前获得，是为了后续操作能在一个事务中完成）
            //获得当前节点定义信息
            WrkFlwNode wrkFlwNode = GetWrkFlwNode(wrkFlwID, nodeID);
            //是否并行的未完成的运行实体数量（驳回完成属于未完成）
            String sql = "select count(*) from WrkFlwEntity a, WrkFlwNode b where a.WrkFlwID=b.WrkFlwID and a.NodeID=b.NodeID and b.WrkFlwID=" + wrkFlwID + " and b.WrkStep=" + wrkFlwNode.WrkStep + " and a.VoucherID=" + voucherInfo.VoucherID
                + " and a.NodeStatus in (" + WrkFlwEntity.NODE_STATUS_MGR_PENDING + "," + WrkFlwEntity.NODE_STATUS_NORMAL_PENDING + "," + WrkFlwEntity.NODE_STATUS_REJECT_PENDING + "," + WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT + ")";
            BaseBO bo = new BaseBO();
            DataSet ds = bo.QueryDataSet(sql);
            int count = int.Parse(ds.Tables[0].Rows[0][0].ToString());

            //2、更新当前节点信息（状态为完成状态）
            //获取运行实体的完成状态
            WrkFlwEntity tmpEntity = GetWrkFlwEntity(wrkFlwID, nodeID, sequence);
            int status = 0;
            //获取紧邻的后续节点
            WrkFlwNode[] nextNodes = GetNextWrkFlwNodes(wrkFlwID, nodeID);
            if (nextNodes == null)
            {
                status = WrkFlwEntity.NODE_STATUS_WRKFLW_NORMAL_COMPLETED;
            }
            else
            {
                status = GetCompletedStatus(tmpEntity.NodeStatus);
            }
            //更新节点信息，状态为完成（包括正常流转完成、上级审批完成）
            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = status;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //有未完成的并行的节点(由于count是提前选出的，所以要判断是否大于1)
            if (count > 1)
            {
                return;
            }
            //3、调用相应的业务处理接口（数据是否生效、是否可打印）
            try
            {
                ConfirmVoucherProcess(wrkFlwID, nodeID, voucherInfo, FuncApp.OPER_TYPE_CONFIRM, trans);
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
            //4、处理后续节点
            //没有后续节点，即为末节点
            if (nextNodes == null)
            {
                try
                {
                    //工作流转接
                    WrkFlwBO wrkFlwBO = new WrkFlwBO();
                    WrkFlw.WrkFlw wrkFlw = wrkFlwBO.GetWrkFlw(wrkFlwID);
                    if (wrkFlw.IfTransit == WrkFlw.WrkFlw.IF_TRANSIT_YES)
                    {
                        ConfirmToTransitWKF(wrkFlwID, trans);
                    }
                    return;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
            //插入紧邻的后续节点
            for (int i = 0; i < nextNodes.Length; i++)
            {
                WrkFlwEntity nextEntity = new WrkFlwEntity(wrkFlwID, nextNodes[i].NodeID, BaseApp.GetWrkFlwSequence());
                nextEntity.SetVoucherInfo(voucherInfo);
                nextEntity.VoucherMemo = "";
                nextEntity.StartTime = DateTime.Now;
                nextEntity.CompletedTime = DateTime.MaxValue;
                nextEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_NORMAL_PENDING;
                nextEntity.PreSequence = sequence;
                trans.Insert(nextEntity);
            }
            WrkFlwMailEntity(wrkFlwID, nodeID,trans);
        }

        /**
         * 将指定节点送领导审批
         */
        public static void SmtToMgr(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo)
        {
            //1、查找领导角色，判断是否有领导角色
            int roleID = new RoleBO().GetMgrRoleID(voucherInfo.DeptID);
            //没有相应的领导角色
            if (roleID == -1)
            {
                throw new WrkFlwException("No manager role int dept:" + voucherInfo.DeptID);
            }

            //2、更新本节点的状态为已完成
            BaseTrans trans = new BaseTrans();
            trans.BeginTrans();
            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_NORMAL_COMPLETED;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //3、提交领导审批，插入运行实体，状态为上级审批待处理
            WrkFlwEntity nextEntity = new WrkFlwEntity(wrkFlwID, nodeID, BaseApp.GetWrkFlwSequence());
            nextEntity.SetVoucherInfo(voucherInfo);
            nextEntity.StartTime = DateTime.Now;
            nextEntity.CompletedTime = DateTime.MaxValue;
            nextEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_MGR_PENDING;
            nextEntity.RoleID = roleID;
            nextEntity.PreSequence = sequence;
            trans.Insert(nextEntity);

            trans.Commit();
        }

        /**
         * 一般驳回处理（领导驳回见MgrRejectVoucher）
         */
        public static void RejectVoucher(int wrkFlwID, int nodeID, int sequence, int toWrkFlwID, int toNodeID, VoucherInfo voucherInfo)
        {
            //更新被驳回节点的状态为驳回已处理
            BaseTrans trans = new BaseTrans();
            trans.BeginTrans();
            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_COMPLETED;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //调用相应的业务处理接口（数据是否生效、是否可打印）
            try
            {
                ConfirmVoucherProcess(wrkFlwID, nodeID, voucherInfo, FuncApp.OPER_TYPE_REJECT, trans);
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }

            //插入新的驳回至节点，状态为驳回待处理
            WrkFlwEntity rejEntity = new WrkFlwEntity(toWrkFlwID, toNodeID, BaseApp.GetWrkFlwSequence());
            rejEntity.SetVoucherInfo(voucherInfo);
            rejEntity.StartTime = DateTime.Now;
            rejEntity.CompletedTime = DateTime.MaxValue;
            rejEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_PENDING;
            trans.Insert(rejEntity);
            trans.Commit();
        }

        /**
         * 驳回处理（驳回到首节点）
         */
        public static void RejectVoucherTwoNode(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo)
        {
            //更新被驳回节点的状态为驳回已处理
            BaseTrans trans = new BaseTrans();
            WrkFlwNode wrkFlwNode = new WrkFlwNode();
            Resultset rs = new Resultset();
            BaseBO baseBO = new BaseBO();

            trans.BeginTrans();
            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_WRKFLW_OVERRULE;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //调用相应的业务处理接口（数据是否生效、是否可打印）
            try
            {
                ConfirmVoucherProcess(wrkFlwID, nodeID, voucherInfo, FuncApp.OPER_TYPE_REJECT, trans);
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }

            //插入新的驳回至节点，状态为驳回待处理
            baseBO.WhereClause ="WrkFlwID = " + wrkFlwID + " and WrkStep=1";
            rs = baseBO.Query(wrkFlwNode);
            if (rs.Count == 1)
            {
                wrkFlwNode = rs.Dequeue() as WrkFlwNode;
                WrkFlwEntity rejEntity = new WrkFlwEntity(wrkFlwNode.WrkFlwID, wrkFlwNode.NodeID, BaseApp.GetWrkFlwSequence());
                rejEntity.SetVoucherInfo(voucherInfo);
                rejEntity.StartTime = DateTime.Now;
                rejEntity.CompletedTime = DateTime.MaxValue;
                rejEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_PENDING;
                trans.Insert(rejEntity);
            }
            trans.Commit();
        }
        /**
         * 驳回处理（驳回到首节点，同一个事务操作）
         */
        public static void RejectVoucherTwoNode(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo, BaseTrans trans)
        {
            //更新被驳回节点的状态为驳回已处理
            WrkFlwNode wrkFlwNode = new WrkFlwNode();
            Resultset rs = new Resultset();
            BaseBO baseBO = new BaseBO();

            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_COMPLETED;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //调用相应的业务处理接口（数据是否生效、是否可打印）
            try
            {
                ConfirmVoucherProcess(wrkFlwID, nodeID, voucherInfo, FuncApp.OPER_TYPE_REJECT, trans);
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }

            //插入新的驳回至节点，状态为驳回待处理
            baseBO.WhereClause = "WrkFlwID = " + wrkFlwID + " and WrkStep=1";
            rs = baseBO.Query(wrkFlwNode);
            if (rs.Count == 1)
            {
                wrkFlwNode = rs.Dequeue() as WrkFlwNode;
                WrkFlwEntity rejEntity = new WrkFlwEntity(wrkFlwNode.WrkFlwID, wrkFlwNode.NodeID, BaseApp.GetWrkFlwSequence());
                rejEntity.SetVoucherInfo(voucherInfo);
                rejEntity.StartTime = DateTime.Now;
                rejEntity.CompletedTime = DateTime.MaxValue;
                rejEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_PENDING;
                trans.Insert(rejEntity);
            }
        }

        /**************************************************
                2008/10/05,冯武修改改
        **************************************************/

        /**
        * 驳回处理（驳回到首节点，同一个事务操作）
        */
        public static void RejectVoucherTwoNode1(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo, BaseTrans trans)
        {
            //更新被驳回节点的状态为驳回已处理
            WrkFlwNode wrkFlwNode = new WrkFlwNode();
            Resultset rs = new Resultset();
            BaseBO baseBO = new BaseBO();

            int firstNodeID = FindFirstNodeID(wrkFlwID, sequence);

            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_COMPLETED;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //调用相应的业务处理接口（数据是否生效、是否可打印）
            try
            {
                ConfirmVoucherProcess(wrkFlwID, nodeID, voucherInfo, FuncApp.OPER_TYPE_REJECT, trans);
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }

            if (firstNodeID == 0)
            {
                return;
            }
            else
            {
                //插入新的驳回至节点，状态为驳回待处理
                baseBO.WhereClause = "WrkFlwID = " + wrkFlwID + " and NodeID = " + firstNodeID + " and WrkStep=1";
                rs = baseBO.Query(wrkFlwNode);
                if (rs.Count == 1)
                {
                    wrkFlwNode = rs.Dequeue() as WrkFlwNode;
                    WrkFlwEntity rejEntity = new WrkFlwEntity(wrkFlwNode.WrkFlwID, wrkFlwNode.NodeID, BaseApp.GetWrkFlwSequence());
                    rejEntity.SetVoucherInfo(voucherInfo);
                    rejEntity.StartTime = DateTime.Now;
                    rejEntity.CompletedTime = DateTime.MaxValue;
                    rejEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_PENDING;
                    trans.Insert(rejEntity);
                }
            }
        }
        /**************************************************
                2008/10/05,冯武修改改
        **************************************************/
        /// <summary>
        /// 根据工作流ID和节点步骤找工作流节点ID
        /// </summary>
        /// <param name="wrkFlwID">工作流ID</param>
        /// <param name="sequence">节点步骤</param>
        /// <returns></returns>
        private static int FindFirstNodeID(int wrkFlwID,int sequence)
        {
            WrkFlwEntity wrkFlwEntity = new WrkFlwEntity();
            Resultset rs = new Resultset();
            BaseBO basebO = new BaseBO();

            basebO.WhereClause = "WrkFlwID = " + wrkFlwID + " and Sequence = " + sequence;
            rs = basebO.Query(wrkFlwEntity);

            wrkFlwEntity = rs.Dequeue() as WrkFlwEntity;
            if (wrkFlwEntity.PreSequence == 0)
            {
                return wrkFlwEntity.NodeID;
            }
            else
            {
                return FindFirstNodeID(wrkFlwEntity.WrkFlwID, wrkFlwEntity.PreSequence);
            }
        }

        /**
         * 领导驳回处理（驳回提交点）
         */
        public static void MgrRejectVoucher(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo)
        {
            //更新本节点的信息（状态为领导驳回处理完成）
            BaseTrans trans = new BaseTrans();
            trans.BeginTrans();
            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_MGR_REJECT_COMPLETED;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //调用相应的业务处理接口（数据是否生效、是否可打印）
            try
            {
                ConfirmVoucherProcess(wrkFlwID, nodeID, voucherInfo, FuncApp.OPER_TYPE_REJECT, trans);
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }

            //插入被驳回运行实体
            WrkFlwEntity rejEntity = new WrkFlwEntity(wrkFlwID, nodeID, BaseApp.GetWrkFlwSequence());
            rejEntity.SetVoucherInfo(voucherInfo);
            rejEntity.StartTime = DateTime.Now;
            rejEntity.CompletedTime = DateTime.MaxValue;
            rejEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_PENDING;
            trans.Insert(rejEntity);
            trans.Commit();
        }

        /**
         * 工作流作废处理（当该工作流不使用时调用该方法作废此工作流，改变节点标志为作废）
         */
        public static void BlankOutVoucherNode(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo)
        {
            //更新被作废节点的状态为作废状态
            BaseTrans trans = new BaseTrans();
            WrkFlwNode wrkFlwNode = new WrkFlwNode();
            Resultset rs = new Resultset();
            BaseBO baseBO = new BaseBO();

            trans.BeginTrans();
            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_WRKFLW_BLANK_OUT;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //调用相应的业务处理接口（数据是否生效、是否可打印）
            try
            {
                ConfirmVoucherProcess(wrkFlwID, nodeID, voucherInfo, FuncApp.OPER_TYPE_REJECT, trans);
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
            trans.Commit();
        }

        /**
         * 工作流作废处理同一个事物处理（当该工作流不使用时调用该方法作废此工作流，改变节点标志为作废）
         */
        public static void BlankOutVoucherNode(int wrkFlwID, int nodeID, int sequence, VoucherInfo voucherInfo, BaseTrans trans)
        {
            //更新被作废节点的状态为作废状态
            WrkFlwNode wrkFlwNode = new WrkFlwNode();
            Resultset rs = new Resultset();
            BaseBO baseBO = new BaseBO();

            WrkFlwEntity thisEntity = new WrkFlwEntity();
            thisEntity.SetVoucherInfo(voucherInfo);
            thisEntity.CompletedTime = DateTime.Now;
            thisEntity.NodeStatus = WrkFlwEntity.NODE_STATUS_WRKFLW_BLANK_OUT;
            trans.WhereClause = "WrkFlwID=" + wrkFlwID + " and NodeID=" + nodeID + " and Sequence=" + sequence;
            trans.Update(thisEntity);

            //调用相应的业务处理接口（数据是否生效、是否可打印）
            try
            {
                ConfirmVoucherProcess(wrkFlwID, nodeID, voucherInfo, FuncApp.OPER_TYPE_REJECT, trans);
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
        }

        /**
         * 确认引发的工作流转接处理
         */
        public static void ConfirmToTransitWKF(int wrkFlwID, BaseTrans trans)
        {
            //获得业务处理接口
            ITransitWrkFlw transitWrkFlw = FuncApp.GetTransitWrkFlwInterface(wrkFlwID);
            //处理业务单据
            VoucherInfo voucherInfo = transitWrkFlw.ConfirmToTransit(trans);
            //!!!!!!!!!!!!!暂时定为在接口中实现所有操作
            ////获得后续转接工作流的首节点
            //WrkFlwNode[] nodes = GetTransitHeadNodes(wrkFlwID);
            ////没有转接工作流
            //if (nodes == null)
            //{
            //    return;
            //}
            //BaseTrans trans = new BaseTrans();
            //trans.BeginTrans();
            ////插入转接工作流首节点运行实体
            //foreach (WrkFlwNode node in nodes)
            //{
            //    WrkFlwEntity entity = new WrkFlwEntity(node.WrkFlwID, node.NodeID, BaseApp.GetWrkFlwSequence());
            //    entity.SetVoucherInfo(voucherInfo);
            //    entity.StartTime = DateTime.Now;
            //    entity.CompletedTime = DateTime.MaxValue;
            //    entity.NodeStatus = WrkFlwEntity.NODE_STATUS_NORMAL_PENDING;
            //    trans.Insert(entity);
            //}
            //trans.Commit();    

        }
        /**
         * 
         */
        public static void RejectToTransitWKF(int wrkFlwID, VoucherInfo voucherInfo, BaseTrans trans)
        {
            //获得业务处理接口
            ITransitWrkFlw transitWrkFlw = FuncApp.GetTransitWrkFlwInterface(wrkFlwID);
            //处理业务单据
            transitWrkFlw.RejectToTransit(voucherInfo.VoucherID, trans);
            //!!!!!!!!!!!!!!!!!1暂时定为在接口中完成所有操作
            //获得后续转接工作流的首节点
            //WrkFlwNode[] nodes = GetTransitTailNodes(wrkFlwID);
            ////没有转接工作流
            //if (nodes == null)
            //{
            //    return;
            //}
            //BaseTrans trans = new BaseTrans();
            //trans.BeginTrans();
            ////插入转接工作流首节点运行实体
            //foreach (WrkFlwNode node in nodes)
            //{
            //    WrkFlwEntity entity = new WrkFlwEntity(node.WrkFlwID, node.NodeID, BaseApp.GetWrkFlwSequence());
            //    entity.SetVoucherInfo(voucherInfo);
            //    entity.StartTime = DateTime.Now;
            //    entity.CompletedTime = DateTime.MaxValue;
            //    entity.NodeStatus = WrkFlwEntity.NODE_STATUS_REJECT_PENDING;
            //    trans.Insert(entity);
            //}
            //trans.Commit();
        }

        /**
         * 处理超时的工作流运行实体
         */
        public void ProcessTimeoutEntities()
        {
            //1、获取所有未处理的工作流
            WrkFlwEntityBO bo = new WrkFlwEntityBO();
            WrkFlwEntity[] entities = bo.GetUnsettledEntities();
            //没有未处理的工作流运行实体
            if (entities == null)
            {
                return;
            }
            //2、根据工作流定义，处理未处理的运行实体
            foreach (WrkFlwEntity entity in entities)
            {
                ProcessTimeoutEntity(entity);
            }
        }

        /**
         * 获取指定状态对应的完成状态
         */
        private static int GetCompletedStatus(int status)
        {
            if (status == WrkFlwEntity.NODE_STATUS_MGR_PENDING)
            {
                return WrkFlwEntity.NODE_STATUS_MGR_COMPLETED;
            }
            else if (status == WrkFlwEntity.NODE_STATUS_NORMAL_PENDING)
            {
                return WrkFlwEntity.NODE_STATUS_NORMAL_COMPLETED;
            }
            else if (status == WrkFlwEntity.NODE_STATUS_REJECT_PENDING)
            {
                return WrkFlwEntity.NODE_STATUS_REJECT_COMPLETED;
            }
            else if (status == WrkFlwEntity.NODE_STATUS_WRKFLW_DRAFT)
            {
                return WrkFlwEntity.NODE_STATUS_NORMAL_COMPLETED;
            }
            else
            {
                throw new WrkFlwException("Work flow entity status is wrong when ConfirmVoucher.status:" + status);
            }
        }

        /**
         * 调用业务单据处理接口
         */
        private static void ConfirmVoucherProcess(int wrkFlwID, int nodeID, VoucherInfo voucherInfo, int operType, BaseTrans trans)
        {
            WrkFlwNode wrkFlwNode = GetWrkFlwNode(wrkFlwID, nodeID);
            //调用相应的业务处理接口（数据是否生效、是否可打印）
            if (wrkFlwNode.ValidAfterConfirm == WrkFlwNode.VALID_AFTER_CONFIRM_YES
                || wrkFlwNode.PrintAfterConfirm == WrkFlwNode.PRINT_AFTER_CONFIRM_YES)
            {
                IConfirmVoucher confirmVoucher = FuncApp.GetConfirmVoucherInterface(wrkFlwNode.WrkFlwID, wrkFlwNode.NodeID);
                if (confirmVoucher == null)
                {
                    throw new WrkFlwException("No process class for work node.wrkflwid:" + wrkFlwNode.WrkFlwID + "-nodeid:" + wrkFlwNode.NodeID);
                }
                if (wrkFlwNode.ValidAfterConfirm == WrkFlwNode.VALID_AFTER_CONFIRM_YES)
                {
                    confirmVoucher.ValidVoucher(voucherInfo.VoucherID, operType, trans);
                }
                if (wrkFlwNode.PrintAfterConfirm == WrkFlwNode.PRINT_AFTER_CONFIRM_YES)
                {
                    confirmVoucher.PrintVoucher(voucherInfo.VoucherID, operType, trans);
                }
            }
        }

        /**
         * 根据工作流的定义，判断工作流运行实体是否超时
         * 不需要判断首节点和末节点，对于工作流转接的首节点，可能不能自动处理，因为单据信息可能需要手工补充完整，
         * 则在定义时，设置该节点为“超时不做任何处理”；对于需要转接的末节点采取相应的策略。
         */
        private void ProcessTimeoutEntity(WrkFlwEntity entity)
        {
            //1、根据工作流定义，判断是否需要超时处理
            WrkFlwNode node = GetWrkFlwNode(entity.WrkFlwID, entity.NodeID);
            if (node.TimeoutHandler == WrkFlwNode.TIMEOUT_HANDLER_NOTHING)
            {
                return;
            }
            //2、根据工作流定义，判断是否超时，并做相应的处理
            TimeSpan delayTime = DateTime.Now - entity.StartTime;
            if (delayTime.Minutes > node.LongestDelay)
            {
                //根据工作流定义，对超时实体进行处理
                //驳回到工作流起点
                if (node.TimeoutHandler == WrkFlwNode.TIMEOUT_HANDLER_REJECT)
                {
                    WrkFlwNode firstNode = GetFirstWrkFlwNode(entity.WrkFlwID);
                    VoucherInfo voucherInfo = new VoucherInfo(entity.VoucherID, entity.VoucherHints, "自动驳回首节点", entity.DeptID, -1);
                    RejectVoucher(entity.WrkFlwID, entity.NodeID, entity.Sequence, firstNode.WrkFlwID, firstNode.NodeID, voucherInfo);
                    return;
                }
                //提交上级审批
                if (node.TimeoutHandler == WrkFlwNode.TIMEOUT_HANDLER_MGR)
                {
                    VoucherInfo voucherInfo = new VoucherInfo(entity.VoucherID, entity.VoucherHints, "自动提交上级审批", entity.DeptID, -1);
                    SmtToMgr(entity.WrkFlwID, entity.NodeID, entity.Sequence, voucherInfo);
                    return;
                }
                //自动通过
                if (node.TimeoutHandler == WrkFlwNode.TIMEOUT_HANDLER_AUTO)
                {
                    VoucherInfo voucherInfo = new VoucherInfo(entity.VoucherID, entity.VoucherHints, "自动通过", entity.DeptID, -1);
                    ConfirmVoucher(entity.WrkFlwID, entity.NodeID, entity.Sequence, voucherInfo);
                    return;
                }
            }
        }

        /**
         * 获得指定工作流的转接工作流首节点
         * 由于改成手动，暂时未用
         */
        //private static WrkFlwNode[] GetTransitHeadNodes(int wrkFlwID)
        //{
        //    BaseBO bo = new BaseBO();
        //    bo.WhereClause = "WrkFlwID in (select NextWrkFlwID from WrkFlwTransfer where WrkFlwID=" + wrkFlwID + " and TransferStatus=" + WrkFlwTransfer.TRANSFER_STATUS_VALID + ")";
        //    Resultset rs = bo.Query(new WrkFlw.WrkFlw());
        //    if (rs.Count > 0)
        //    {
        //        WrkFlwNode[] nodes = new WrkFlwNode[rs.Count];
        //        int i = 0;
        //        foreach (WrkFlw.WrkFlw wrkFlw in rs)
        //        {
        //            nodes[i++] = GetFirstWrkFlwNode(wrkFlw.WrkFlwID);
        //        }
        //        return nodes;
        //    }
        //    return null;
        //}

        /**
         * 获得指定工作流的转接工作流末节点
         * 由于改成手动，暂时未用
         */
        //private static WrkFlwNode[] GetTransitTailNodes(int wrkFlwID)
        //{
        //    BaseBO bo = new BaseBO();
        //    bo.WhereClause = "WrkFlwID in (select WrkFlwID from WrkFlwTransfer where NextWrkFlwID=" + wrkFlwID + " and TransferStatus=" + WrkFlwTransit.TRANSIT_STATUS_VALID + ")";
        //    Resultset rs = bo.Query(new WrkFlw.WrkFlw());
        //    if (rs.Count > 0)
        //    {
        //        WrkFlwNode[] nodes = new WrkFlwNode[rs.Count];
        //        int i = 0;
        //        foreach (WrkFlw.WrkFlw wrkFlw in rs)
        //        {
        //            nodes[i++] = GetLastWrkFlwNode(wrkFlw.WrkFlwID);
        //        }
        //        return nodes;
        //    }
        //    return null;
        //}

        public static void WrkFlwMailEntity(int wrkFlwID, int nodeID,BaseTrans trans)
        {
            int wrkFlwMailID = 0;
            SMSPara sMSPara = new SMSPara();
            BaseBO baseBO = new BaseBO();
            Resultset rs = new Resultset();

            WrkFlwMail wrkFlwMail = new WrkFlwMail();
            WrkFlwNode wrkFlwNode = new WrkFlwNode();

            WrkFlwMailApp wrkFlwMailApp = new WrkFlwMailApp();


            string mailSubject = "";
            string mailText = "";
            string mailSMTP = "";
            string mailSMTPUser = "";
            string mailSMTPPassword = "";

            rs = baseBO.Query(new SMSPara());
            if (rs.Count == 1)
            {
                sMSPara = rs.Dequeue() as SMSPara;

                mailSMTP = sMSPara.MailSMTP;
                mailSMTPUser = sMSPara.MailSMTPUser;
                mailSMTPPassword = sMSPara.MailSMTPPassword;
            }

            WrkFlwMailList wrkFlwMailList = new WrkFlwMailList();

            baseBO.WhereClause = "WrkFlwID = " + wrkFlwID + " and NodeID=" + nodeID;
            rs = baseBO.Query(wrkFlwNode);
            if (rs.Count == 1)
            {
                wrkFlwNode = rs.Dequeue() as WrkFlwNode;
                wrkFlwMailID = wrkFlwNode.WrkFlwMailID;
            }

            baseBO.WhereClause = "WrkFlwMailID=" + wrkFlwMailID;

            rs = baseBO.Query(wrkFlwMail);
            if (rs.Count == 1)
            {
                wrkFlwMail = rs.Dequeue() as WrkFlwMail;
                mailText = wrkFlwMail.MailText;
                mailSubject = wrkFlwMail.MailSubject;
            }

            SessionUser sessionUser = (SessionUser)System.Web.HttpContext.Current.Session["UserAccessInfo"];
            
            WrkFlwNode[] nextNodes = GetNextWrkFlwNodes(wrkFlwID, nodeID);
            if (nextNodes == null)
            {
                wrkFlwMailID = 0;
            }
            else
            {
                foreach (WrkFlwNode wrknode in nextNodes)
                {
                    DataSet ds = new DataSet();
                    baseBO.WhereClause = "";
                    ds = baseBO.QueryDataSet("Select UserName,Email From UserRole Left Join Users On UserRole.UserID = Users.UserID Where RoleID= " + wrknode.RoleID);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        wrkFlwMailList.Text = mailText;
                        wrkFlwMailList.Subject = mailSubject;
                        wrkFlwMailList.Addressee = ds.Tables[0].Rows[i]["Email"].ToString();
                        wrkFlwMailList.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        wrkFlwMailList.WrkFlwMailListID = BaseApp.GetWrkFlwMailListID();
                        wrkFlwMailList.AttachFile1 = "";
                        wrkFlwMailList.AttachFile2 = "";
                        wrkFlwMailList.AttachFile3 = "";
                        wrkFlwMailList.CreateUserID = sessionUser.UserID;
                        wrkFlwMailList.MailSMTP = mailSMTP;
                        wrkFlwMailList.MailSMTPPassword = mailSMTPPassword;
                        wrkFlwMailList.MailSMTPUser = mailSMTPUser;
                        wrkFlwMailList.OprDeptID = sessionUser.DeptID;
                        wrkFlwMailList.OprRoleID = sessionUser.RoleID;
                        wrkFlwMailList.SendStatus = WrkFlwMailList.WorkFlowMailList_NO;

                        wrkFlwMailApp.SendOneMail(wrkFlwMailList, trans);
                    }

                }
            }


        }

        //====================================非核心方法===========================================
        /**
         * 判断指定工作流节点运行实体是否可以提交领导审批
         */
        public static bool CanSmtToMgr(int wrkFlwID, int nodeID, int sequence)
        {
            //节点定义是否可以提交领导
            WrkFlwNode wrkFlwNode = GetWrkFlwNode(wrkFlwID, nodeID);
            if (wrkFlwNode.SmtToMgr == WrkFlwNode.SMT_TO_MGR_NO)
            {
                return false;
            }
            //运行实体是否是正常流转待处理状态
            WrkFlwEntity entity = GetWrkFlwEntity(wrkFlwID, nodeID, sequence);
            if (entity.NodeStatus == WrkFlwEntity.NODE_STATUS_NORMAL_PENDING
                || entity.NodeStatus == WrkFlwEntity.NODE_STATUS_REJECT_PENDING)
            {
                return true;
            }

            return false;
        }


    }
}
