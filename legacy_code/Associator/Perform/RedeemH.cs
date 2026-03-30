using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

///<summary>
/// 修改人：hesijian
/// 修改时间：2009年5月15日
///</summary>

namespace Associator.Perform
{
    /// <summary>
    /// 积分兑换记录主表
    /// </summary>
    public class RedeemH : BasePO
    {
        private int redeemID = 0;  //赠品兑换ID
        private int membID = 0;      //会员ID
        private DateTime redeemDate = DateTime.Now;  //兑换日期
        private decimal bonusPrev = 0;  //兑换前积分
        private decimal redeemAmt = 0;  //赠品兑换总积分
        private decimal bonusCurr = 0;  //兑换后积分
        private int giftID = 0;
        private int giftQty = 0;
        private DateTime createTime = DateTime.Now;  //创建时间
        private int modifyUserID = 0; //修改用户ID
        private DateTime modifyTime = DateTime.Now; //修改时间
        private int oprRoleID = 0; //角色ID
        private int oprDeptID = 0; //角色部门
        private int counterID = 0; //服务台ID
        private int createUserID = 0;//创建角色ID

        public int RedeemID
        {
            get { return redeemID; }
            set { redeemID = value; }
        }

        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }

        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }

        public DateTime RedeemDate
        {
            get { return redeemDate; }
            set { redeemDate = value; }
        }

        public decimal BonusPrev
        {
            get { return bonusPrev; }
            set { bonusPrev = value; }
        }

        public decimal RedeemAmt
        {
            get { return redeemAmt; }
            set { redeemAmt = value; }
        }

        public decimal BonusCurr
        {
            get { return bonusCurr; }
            set { bonusCurr = value; }
        }

        public int GiftQty
        {
            get { return giftQty; }
            set { giftQty = value; }
        }

        //增加部分
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }


        /// <summary>
        /// 修改用户ID
        /// </summary>
        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }


        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }


        /// <summary>
        /// 角色ID
        /// </summary>
        public int OprRoleID
        {
            get { return oprRoleID; }
            set { oprRoleID = value; }
        }


        /// <summary>
        /// 角色部门
        /// </summary>
        public int OprDeptID
        {
            get { return oprDeptID; }
            set { oprDeptID = value; }
        }


        /// <summary>
        /// 服务台ID
        /// </summary>
        public int CounterID
        {
            get { return counterID; }
            set { counterID = value; }
        }

        /// <summary>
        /// 创建角色ID
        /// </summary>
        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }

        public override string GetTableName()
        {
            return "RedeemH";
        }

        public override string GetColumnNames()
        {
            return "RedeemID,MembID,GiftID,RedeemDate,BonusPrev,RedeemAmt,BonusCurr,GiftQty,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,CounterID";
        }

        public override string GetInsertColumnNames()
        {
            return "RedeemID,MembID,GiftID,RedeemDate,BonusPrev,RedeemAmt,BonusCurr,GiftQty,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,CounterID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
    }
}
