using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.ChangeLease
{
    public class ConFormulaMod:BasePO
    {
        private int conFormulaModID = 0;
        private int contractID = 0;
        private int isValid = 0;
        private DateTime modDate = DateTime.Now;
        private int modUser = 0;
        private string refID = "";
        private string note = "";
        private string modReason = "";

        public static int CONFORMULAMOD_BECOME_EFFECTIVE = 1;/*生效*/
        public static int CONFORMULAMOD_INVLIDATION = 0;/*未生效*/

        public override string GetTableName()
        {
            return "ConFormulaMod";
        }

        public override string GetColumnNames()
        {
            return "ConFormulaModID,ContractID,IsValid,ModDate,ModUser,RefID,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "ConFormulaModID,ContractID,IsValid,RefID,ModReason";
        }

        public override string GetUpdateColumnNames()
        {
            return "IsValid,ModDate,ModUser,RefID";
        }


        public int ConFormulaModID
        {
            get { return conFormulaModID; }
            set { conFormulaModID = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }

        public DateTime ModDate
        {
            get { return modDate; }
            set { modDate = value; }
        }

        public int ModUser
        {
            get { return modUser; }
            set { modUser = value; }
        }

        public string RefID
        {
            get { return refID; }
            set { refID = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public string ModReason
        {
            get { return modReason; }
            set { modReason = value; }
        }
    }
}
