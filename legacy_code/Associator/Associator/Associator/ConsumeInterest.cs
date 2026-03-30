using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Associator
{
    public class ConsumeInterest : BasePO
    {
        private int consumeID = 0;
        private int membID = 0;
        private int iItemID = 0;

        public int ConsumeID
        {
            get { return consumeID; }
            set { consumeID = value; }
        }

        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }

        public int IItemID
        {
            get { return iItemID; }
            set { iItemID = value; }
        }

        public override string GetTableName()
        {
            return "ConsumeInterest";
        }

        public override string GetColumnNames()
        {
            return "ConsumeID,MembID,IItemID";
        }

        public override string GetInsertColumnNames()
        {
            return "ConsumeID,MembID,IItemID";
        }

        public override string GetUpdateColumnNames()
        {
            return "MembID,IItemID";
        }
    }
}
