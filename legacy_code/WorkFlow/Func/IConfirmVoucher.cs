using System;
using System.Collections.Generic;
using System.Text;

using Base.Biz;

namespace WorkFlow.Func
{
    /**
     * 该接口定义了设置数据生效或（和）数据可打印的方法，如果工作流节点需要修改数据的生效状态或打印状态，则需要提供实现该接口的类。
     * 要求实现相应的方法，方法的使用见其描述
     */
    public interface IConfirmVoucher
    {
        /**
         * 确认单据后对数据生效的处理
         * voucherID：确认处理的单据号
         * trans：事务对象，所有数据库操作都需要使用该事务
         */
        void ValidVoucher(int voucherID, int operType, BaseTrans trans);

        /**
         * 确认单据后对数据打印的处理
         * voucherID：确认处理的单据号
         * trans：事务对象，所有数据库操作都需要使用该事务
         */
        void PrintVoucher(int voucherID, int operType, BaseTrans trans);
    }
}
