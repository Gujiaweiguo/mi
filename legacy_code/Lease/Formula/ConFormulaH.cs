using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.Formula
{
    public class ConFormulaH:BasePO
    {
        private int formulaID;
        private int chargeTypeID;
        private int contractID;
        private int createUserID;
        private DateTime createTime;
        private int modifyUserID;
        private DateTime modifyTime;
        private int oprRoleID;
        private int oprDeptID;
        private DateTime fStartDate;
        private DateTime fEndDate;
        private string formulaType;
        private decimal totalArea = 0;
        private decimal unitPrice = 0;
        private decimal baseAmt = 0;
        private decimal fixedRental = 0;
        private string rateType = "M";
        private string pcentOpt;
        private string minSumOpt = "F";
        private string chargeTypeName;
        private string formulaTypeName;

        public int FormulaID
        {
            get { return formulaID; }
            set { formulaID = value; }
        }

        public int ChargeTypeID
        {
            get { return chargeTypeID; }
            set { chargeTypeID = value; }
        }

        public string ChargeTypeName
        {
            get { return chargeTypeName; }
            set { chargeTypeName = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
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
            get{return modifyUserID;}
            set{modifyUserID = value;}
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

        public DateTime FStartDate
        {
            get { return fStartDate; }
            set { fStartDate = value; }
        }

        public DateTime FEndDate
        {
            get { return fEndDate; }
            set { fEndDate = value; }
        }

        public string FormulaType
        {
            get { return formulaType; }
            set { formulaType = value; }
        }

        public string FormulaTypeName
        {
            get { return formulaTypeName; }
            set { formulaTypeName = value; }
        }

        public decimal TotalArea
        {
            get { return totalArea; }
            set { totalArea = value; }
        }

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }

        public decimal BaseAmt
        {
            get { return baseAmt; }
            set { baseAmt = value; }
        }

        public decimal FixedRental
        {
            get { return fixedRental; }
            set { fixedRental = value; }
        }

        public string RateType
        {
            get { return rateType; }
            set { rateType = value; }
        }

        public string PcentOpt
        {
            get { return pcentOpt; }
            set { pcentOpt = value; }
        }

        public string MinSumOpt
        {
            get { return minSumOpt; }
            set { minSumOpt = value; }
        }

        public override string GetTableName()
        {
            return "ConFormulaH";
        }

        public override string GetColumnNames()
        {
            return "FormulaID,ChargeTypeID,ContractID,FStartDate,FEndDate,FormulaType,RateType,PcentOpt,MinSumOpt,TotalArea," +
                   "FixedRental,UnitPrice,BaseAmt";
        }

        public override string GetQuerySql()
        {
            return "select FormulaID,ChargeTypeID,ContractID,FStartDate,FEndDate,FormulaType,RateType,PcentOpt,MinSumOpt,TotalArea," +
                   "FixedRental,UnitPrice,BaseAmt,'' as ChargeTypeName,'' as FormulaTypeName from ConFormulaH";
        }

        public override string GetInsertColumnNames()
        {
            //return "FormulaID,ChargeTypeID,ContractID,CreateUserID,CreateTime,ModifyUserID,OprRoleID,OprDeptID,FStartDate,FEndDate,FormulaType," +
            //       "TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt";
            return "FormulaID,ChargeTypeID,ContractID,FStartDate,FEndDate,FormulaType,RateType,PcentOpt,MinSumOpt,TotalArea," +
                   "FixedRental,UnitPrice,BaseAmt";
        }

        public override string GetUpdateColumnNames()
        {
            //return "FormulaID,ChargeTypeID,ContractID,CreateUserID,CreateTime,ModifyUserID,OprRoleID,OprDeptID,FStartDate,FEndDate,FormulaType," +
            //       "TotalArea,UnitPrice,BaseAmt,FixedRental,RateType,PcentOpt,MinSumOpt";

            return "ChargeTypeID,ContractID,FStartDate,FEndDate,FormulaType,RateType,PcentOpt,MinSumOpt,TotalArea," +
                "FixedRental,UnitPrice,BaseAmt";
        }

        //公式类别　FormulaType
        public static string FORMULATYPE_TYPE_ONE = "F";
        public static string FORMULATYPE_TYPE_TWO = "V";
        public static string FORMULATYPE_TYPE_THREE = "O";

        public static string[] GetFormulaTypeStatus()
        {
            string[] FormulaType = new string[3];
            FormulaType[0] = FORMULATYPE_TYPE_ONE;
            FormulaType[1] = FORMULATYPE_TYPE_TWO;
            FormulaType[2] = FORMULATYPE_TYPE_THREE;
            return FormulaType;
        }

        public static string GetFormulaTypeStatusDesc(string FormulaType)
        {
            if (FormulaType.Equals(FORMULATYPE_TYPE_ONE))
            {
                return "FORMULATYPE_TYPE_ONE"; // "固定";
            }
            if (FormulaType.Equals(FORMULATYPE_TYPE_TWO))
            {
                return "FORMULATYPE_TYPE_TWO";// "抽成保底";
            }
            if (FormulaType.Equals(FORMULATYPE_TYPE_THREE))
            {
                return "FORMULATYPE_TYPE_THREE"; //"一次性收取";
            }
            return "NO";
        }

        //固定租金
        public static string RATETYPE_TYPE_MONTH = "M";
        public static string RATETYPE_TYPE_DAY = "D";

        //抽成
        public static string PCENTOPT_TYPE_FAST = "F"; //固定
        public static string PCENTOPT_TYPE_S = "S"; //级别式
        public static string PCENTOPT_TYPE_M = "M"; //多级式

        //保底
        public static string MINSUMOPT_TYPE_FAST = "F"; //固定
        public static string MINSUMOPT_TYPE_T = "T"; //级别
    }
}
