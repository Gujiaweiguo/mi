using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

using Base;
using Base.Biz;
using Base.DB;

using BaseInfo.Role;

using WorkFlow.WrkFlw;
using WorkFlow.Uiltil;
using WorkFlow.Func;
using WorkFlow.WrkFlwRpt;
namespace WorkFlow.WrkFlwRpt
{
    public class WrkFlwRptBO
    {
        /**
        * 根据指定的用户角色查询已处理完成工作流运行实体节点信息（包括功能URL）
        */
        public static WrkFlwRptPO[] GetWrkFlwEntityInfoNormal_Completed(int roleID, int NodeStatus)
        {
            //从数据库中查询指定角色的工作流运行实体节点
            BaseBO bo = new BaseBO();
            bo.AppendWhere = false;
            bo.WhereClause = "b.RoleID=" + roleID + " and a.NodeStatus = " + NodeStatus;
            bo.OrderBy = "StartTime";
            Resultset rs = bo.Query(new WrkFlwRptPO());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                
                WrkFlwRptPO[] entities = new WrkFlwRptPO[rs.Count];
                int i = 0;
                foreach (WrkFlwRptPO entity in rs)
                {
                   TimeSpan day = (entity.CompletedTime - entity.StartTime);
                    entity.Stop = (int)day.Days;
                    entities[i++] = entity;
                }
                return entities;
            }
            return null;
        }


        /**
        * 
        */
        public static WrkFlwRptPO[] GetWrkFlwEntityInfoOvertime(int roleID)
        {
            //从数据库中查询指定角色的工作流运行实体节点
            BaseBO bo = new BaseBO();
            bo.AppendWhere = false;
            bo.WhereClause = "b.RoleID=" + roleID + " and a.NodeStatus <> " + WrkFlwEntity.NODE_STATUS_REJECT_PENDING + " and a.NodeStatus <>" + WrkFlwEntity.NODE_STATUS_NORMAL_COMPLETED;
            bo.OrderBy = "StartTime";
            Resultset rs = bo.Query(new WrkFlwRptPO());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwRptPO[] entities = new WrkFlwRptPO[rs.Count];
                int i = 0;
                foreach (WrkFlwRptPO entity in rs)
                {
                    TimeSpan day = (DateTime.Now - entity.StartTime);
                    if ( (int)day.Days > entity.LongestDelay)
                    {
                        entity.Stop = Convert.ToInt32((DateTime.Now - entity.StartTime));
                        entities[i++] = entity;
                    }
                }
                return entities;
            }
            return null;
        }

        /**
        * 
        */
        public static WrkFlwRptPO[] GetWrkFlwEntityInfoDisposaling(int roleID)
        {
            //从数据库中查询指定角色的工作流运行实体节点
            BaseBO bo = new BaseBO();
            bo.AppendWhere = false;
            bo.WhereClause = "b.RoleID=" + roleID + " and a.NodeStatus <> " + WrkFlwEntity.NODE_STATUS_REJECT_PENDING + " and a.NodeStatus <>" + WrkFlwEntity.NODE_STATUS_NORMAL_COMPLETED + "and a.NodeStatus<>" + WrkFlwEntity.NODE_STATUS_WRKFLW_NORMAL_COMPLETED;
            bo.OrderBy = "StartTime";
            Resultset rs = bo.Query(new WrkFlwRptPO());
            //将查询结果存放在数组中
            if (rs.Count > 0)
            {
                WrkFlwRptPO[] entities = new WrkFlwRptPO[rs.Count];
                int i = 0;
                foreach (WrkFlwRptPO entity in rs)
                {
                    TimeSpan day = (DateTime.Now - entity.StartTime);
                    if ((int)day.Days <= entity.LongestDelay)
                    {
                        day = (DateTime.Now - entity.StartTime);
                        entity.Stop = (int)day.Days;
                        entities[i++] = entity;
                    }
                }
                return entities;
            }
            return null;
        }
    }
}
