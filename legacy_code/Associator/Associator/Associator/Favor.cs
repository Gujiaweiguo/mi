using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Associator
{
    public class Favor : BasePO
    {
        private int favorID = 0;
        private int membID = 0;
        private int fItemID = 0;

        public int FavorID
        {
            get { return favorID; }
            set { favorID = value; }
        }

        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }

        public int FItemID
        {
            get { return fItemID; }
            set { fItemID = value; }
        }

        public override string GetTableName()
        {
            return "Favor";
        }

        public override string GetColumnNames()
        {
            return "FavorID,MembID,FItemID";
        }

        public override string GetInsertColumnNames()
        {
            return "FavorID,MembID,FItemID";
        }

        public override string GetUpdateColumnNames()
        {
            return "MembID,FItemID";
        }
    }
}
