using System;
using System.Collections.Generic;
using System.Text;

using WorkFlow.Uiltil;
using Base.Biz;

namespace WorkFlow.Func
{
    /**
     * 该接口定义了工作流转接的方法，包括：1、上一工作流完成，转接后续工作流；2、后一工作流被驳回，转接前置工作流。
     * 相应的描述见方法前的注释
     */
    public interface ITransitWrkFlw
    {
        /**
         * 确认引发的工作流转接处理，返回下一个工作流首节点单据的信息
         * trans：事务对象，所有数据库操作都需要使用该事务
         */
        VoucherInfo ConfirmToTransit(BaseTrans trans);

        /**
         * 驳回引发的工作流转接处理
         * voucherID：当前工作流驳回的单据号
         * trans：事务对象，所有数据库操作都需要使用该事务
         */
        void RejectToTransit(int voucherID, BaseTrans trans);
    }
}
