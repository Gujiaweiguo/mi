using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Associator
{
    public class Activity : BasePO
    {
        private int activityID = 0;
        private int membID = 0;
        private int aItemID = 0;

        public int ActivityID
        {
            get { return activityID; }
            set { activityID = value; }
        }

        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }

        public int AItemID
        {
            get { return aItemID; }
            set { aItemID = value; }
        }

        public override string GetTableName()
        {
            return "Activity";
        }

        public override string GetColumnNames()
        {
            return "ActivityID,MembID,AItemID";
        }

        public override string GetInsertColumnNames()
        {
            return "ActivityID,MembID,AItemID";
        }

        public override string GetUpdateColumnNames()
        {
            return "MembID,AItemID";
        }
    }
}
