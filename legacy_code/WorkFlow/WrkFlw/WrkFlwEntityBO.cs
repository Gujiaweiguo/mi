using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;
using Base.DB;

namespace WorkFlow.WrkFlw
{
    public class WrkFlwEntityBO : BaseBO
    {
        /**
         * 获取正常未处理的工作流实体，在工作流超时处理时使用
         * 不包括驳回未处理：驳回后，需要操作人员根据驳回原因进行处理
         * 提交上级未处理：滞留时间为无限，不引发超时处理
         */
        public WrkFlwEntity[] GetUnsettledEntities()
        {
            this.WhereClause = "NodeStatus=" + WrkFlwEntity.NODE_STATUS_NORMAL_PENDING;
            Resultset rs = Query(new WrkFlwEntity());
            if (rs.Count == 0)
            {
                return null;
            }
            int count = 0;
            WrkFlwEntity[] entities = new WrkFlwEntity[rs.Count];
            foreach (WrkFlwEntity entity in rs)
            {
                entities[count] = entity;
                count++;
            }
            return entities;
        }
    }
}
