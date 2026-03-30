using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 滞纳金明细
    /// </summary>
    public class InvoiceInterest:BasePO
    {
        #region Model
        private int interestid;
        private int createuserid;
        private DateTime createtime;
        private int modifyuserid;
        private DateTime modifytime;
        private int oprroleid;
        private int oprdeptid;
        private int lateinvid;
        private int lateinvdetailid;
        private string invcode;
        private DateTime intstartdate;
        private DateTime intenddate;
        private int interestday;
        private int extendday;
        private decimal latepayamt;
        private decimal interestrate;
        private decimal interestamt;
        private string note;
        private int chargeTypeID;
        private int contractID;
        private string chargeTypeName;
        
        /// <summary>
        /// 滞纳金明细ID
        /// </summary>
        public int InterestID
        {
            set { interestid = value; }
            get { return interestid; }
        }
        /// <summary>
        /// 创建用户代码
        /// </summary>
        public int CreateUserID
        {
            set { createuserid = value; }
            get { return createuserid; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            set { createtime = value; }
            get { return createtime; }
        }
        /// <summary>
        /// 最后修改用户代码
        /// </summary>
        public int ModifyUserID
        {
            set { modifyuserid = value; }
            get { return modifyuserid; }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            set { modifytime = value; }
            get { return modifytime; }
        }
        /// <summary>
        /// 操作用户的角色代码
        /// </summary>
        public int OprRoleID
        {
            set { oprroleid = value; }
            get { return oprroleid; }
        }
        /// <summary>
        /// 操作用户的机构代码
        /// </summary>
        public int OprDeptID
        {
            set { oprdeptid = value; }
            get { return oprdeptid; }
        }
        /// <summary>
        /// 欠款结算单ID
        /// </summary>
        public int LateInvID
        {
            set { lateinvid = value; }
            get { return lateinvid; }
        }
        /// <summary>
        /// 欠款结算明细单ID
        /// </summary>
        public int LateInvDetailID
        {
            set { lateinvdetailid = value; }
            get { return lateinvdetailid; }
        }
        /// <summary>
        /// 结算单编码
        /// </summary>
        public string InvCode
        {
            set { invcode = value; }
            get { return invcode; }
        }
        /// <summary>
        /// 滞纳开始日期
        /// </summary>
        public DateTime IntStartDate
        {
            set { intstartdate = value; }
            get { return intstartdate; }
        }
        /// <summary>
        /// 滞纳结束日期
        /// </summary>
        public DateTime IntEndDate
        {
            set { intenddate = value; }
            get { return intenddate; }
        }
        /// <summary>
        /// 滞纳天数
        /// </summary>
        public int InterestDay
        {
            set { interestday = value; }
            get { return interestday; }
        }
        /// <summary>
        /// 宽限期
        /// </summary>
        public int ExtendDay
        {
            set { extendday = value; }
            get { return extendday; }
        }
        /// <summary>
        /// 滞纳本金
        /// </summary>
        public decimal LatePayAmt
        {
            set { latepayamt = value; }
            get { return latepayamt; }
        }
        /// <summary>
        /// 滞纳金利率
        /// </summary>
        public decimal InterestRate
        {
            set { interestrate = value; }
            get { return interestrate; }
        }
        /// <summary>
        /// 滞纳金
        /// </summary>
        public decimal InterestAmt
        {
            set { interestamt = value; }
            get { return interestamt; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            set { note = value; }
            get { return note; }
        }
        /// <summary>
        /// 费用类型ID
        /// </summary>
        public int ChargeTypeID
        {
            set { chargeTypeID = value; }
            get { return chargeTypeID; }
        }
        /// <summary>
        /// 费用类型名称
        /// </summary>
        public string ChargeTypeName
        {
            set { chargeTypeName = value; }
            get { return chargeTypeName; }
        }
        /// <summary>
        /// 合同ID
        /// </summary>
        public int ContractID
        {
            set { contractID = value; }
            get { return contractID; }
        }
        #endregion Model

        public override string GetTableName()
        {
            return "InvoiceInterest";
        }

        public override string GetColumnNames()
        {
            return "InterestID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,LateInvID,LateInvDetailID,InvCode,IntStartDate,IntEndDate,InterestDay,ExtendDay,LatePayAmt,InterestRate,InterestAmt,Note,ChargeTypeID,ContractID,ChargeTypeName";
        }

        public override string GetInsertColumnNames()
        {
            return "InterestID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,LateInvID,LateInvDetailID,InvCode,IntStartDate,IntEndDate,InterestDay,ExtendDay,LatePayAmt,InterestRate,InterestAmt,Note,ChargeTypeID,ContractID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "select InterestID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,LateInvID,LateInvDetailID,InvCode,IntStartDate,IntEndDate,InterestDay,ExtendDay,LatePayAmt,InterestRate,InterestAmt,Note,ChargeTypeID,ContractID,'' as ChargeTypeName from InvoiceInterest";
        }

    }
}
