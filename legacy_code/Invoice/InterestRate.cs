using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    public class InterestRate : BasePO
    {
        #region Model
        private int interestRateID;
        private int contractID;
        private int chargeTypeID;
        private decimal intRate;
        private int createUserID = 0;  //创建用户代码
        private DateTime createTime = DateTime.Now;  //创建时间
        private int modifyUserID = 0;  //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;  //最后修改时间
        private int oprRoleID = 0;  //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码

        /// <summary>
        /// 滞纳金利率ID
        /// </summary>
        public int InterestRateID
        {
            set { interestRateID = value; }
            get { return interestRateID; }
        }
        /// <summary>
        /// 合同ID
        /// </summary>
        public int ContractID
        {
            set { contractID = value; }
            get { return contractID; }
        }
        /// <summary>
        /// 费用类别ID
        /// </summary>
        public int ChargeTypeID
        {
            set { chargeTypeID = value; }
            get { return chargeTypeID; }
        }
        /// <summary>
        /// 滞纳金利率
        /// </summary>
        public decimal IntRate
        {
            set { intRate = value; }
            get { return intRate; }
        }
        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }

        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }

        public int OprRoleID
        {
            get { return oprRoleID; }
            set { oprRoleID = value; }
        }

        public int OprDeptID
        {
            get { return oprDeptID; }
            set { oprDeptID = value; }
        }
        #endregion

        public override string GetTableName()
        {
            return "InterestRate";
        }

        public override string GetColumnNames()
        {
            return "InterestRateID,ContractID,ChargeTypeID,IntRate,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "InterestRateID,ContractID,ChargeTypeID,IntRate,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "ContractID,ChargeTypeID,IntRate,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

    }
}
