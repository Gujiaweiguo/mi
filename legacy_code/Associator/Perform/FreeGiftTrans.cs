using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

/// <summary>
/// 修改人：hesijian
/// 修改日期：2009年5月15日
/// </summary>

namespace Associator.Perform
{
    /// <summary>
    /// 免费发放记录
    /// </summary>
    public class FreeGiftTrans:BasePO
    {
        /// <summary>
        /// 赠品发放ID
        /// </summary>
        private int giftTransID = 0;

        /// <summary>
        /// 赠品ID
        /// </summary>
        private int giftID = 0;

        /// <summary>
        /// 活动ID
        /// </summary>
        private int actID = 0;

        /// <summary>
        /// 会员ID
        /// </summary>
        private int membID = 0;

        /// <summary>
        /// 发放日期
        /// </summary>
        private DateTime actDate = DateTime.Now;

        /// <summary>
        /// 发放数量
        /// </summary>
        private int giftQty = 0;

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //private DateTime createTime = DateTime.Now;

        ///// <summary>
        ///// 修改用户ID
        ///// </summary>
        //private int modifyUserID = 0;

        ///// <summary>
        ///// 修改时间
        ///// </summary>
        //private DateTime modifyTime = DateTime.Now;

        ///// <summary>
        ///// 角色ID
        ///// </summary>
        //private int oprRoleID = 0;

        ///// <summary>
        ///// 角色部门
        ///// </summary>
        //private int oprDeptID = 0;

        ///// <summary>
        ///// 服务台ID
        ///// </summary>
        //private int counterID = 0;

        ///// <summary>
        ///// 创建角色ID
        ///// </summary>
        //private int createUserID = 0;


        /// <summary>
        /// 赠品发放ID
        /// </summary>
        public int GiftTransID
        {
            get { return giftTransID; }
            set { giftTransID = value; }
        }

        /// <summary>
        /// 赠品ID
        /// </summary>
        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }

        /// <summary>
        /// 活动ID
        /// </summary>
        public int ActID
        {
            get { return actID; }
            set { actID = value; }
        }

        /// <summary>
        /// 会员ID
        /// </summary>
        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }


        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ActDate
        {
            get { return actDate; }
            set { actDate = value; }
        }

        /// <summary>
        /// 发放数量
        /// </summary>
        public int GiftQty
        {
            get { return giftQty; }
            set { giftQty = value; }
        }

        ////增加部分
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime
        //{
        //    get { return createTime; }
        //    set { createTime = value; }
        //}


        ///// <summary>
        ///// 修改用户ID
        ///// </summary>
        //public int ModifyUserID
        //{
        //    get { return modifyUserID; }
        //    set { modifyUserID = value; }
        //}


        ///// <summary>
        ///// 修改时间
        ///// </summary>
        //public DateTime ModifyTime
        //{
        //    get { return modifyTime; }
        //    set { modifyTime = value; }
        //}


        ///// <summary>
        ///// 角色ID
        ///// </summary>
        //public int OprRoleID
        //{
        //    get { return oprRoleID; }
        //    set { oprRoleID = value; }
        //}


        ///// <summary>
        ///// 角色部门
        ///// </summary>
        //public int OprDeptID
        //{
        //    get { return oprDeptID; }
        //    set { oprDeptID = value; }
        //}


        ///// <summary>
        ///// 服务台ID
        ///// </summary>
        //public int CounterID
        //{
        //    get { return counterID; }
        //    set { counterID = value; }
        //}

        ///// <summary>
        ///// 创建角色ID
        ///// </summary>
        //public int CreateUserID
        //{
        //    get { return createUserID; }
        //    set { createUserID = value; }
        //}

        public override string GetTableName()
        {
            return "FreeGiftTrans";
        }

        public override string GetColumnNames()
        {
            return "GiftTransID,GiftID,ActID,MembID,ActDate,GiftQty";
        }

        public override string GetInsertColumnNames()
        {
            return "GiftTransID,GiftID,ActID,MembID,ActDate,GiftQty";
        }

        public override string GetUpdateColumnNames()
        {
            return "GiftID,ActID,MembID,ActDate,GiftQty";
        }
    }
}
