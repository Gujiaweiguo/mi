using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Lease.ConShop
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
        private decimal totalArea;
        private decimal unitPrice;
        private decimal baseAmt;
        private decimal fixedRental;
        private string rateType;
        private string pcentOpt;
        private string minSumOpt;

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
        
    }
}
