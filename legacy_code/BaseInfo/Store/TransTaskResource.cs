using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class TransTaskRes : BasePO
    {
        private int taskid = 0;
        private int resourceid = 0;
        public override String GetTableName()
        {
            return "TransTaskRes";
        }
        public override String GetColumnNames()
        {
            return "TaskID,ResourceID";
        }
        public override String GetUpdateColumnNames()
        {
            return "TaskID,ResourceID";
        }
        public override string GetInsertColumnNames()
        {
            return "TaskID,ResourceID";
        }
        public int TaskID
        {
            get { return taskid; }
            set { taskid = value; }
        }
        public int ResourceID
        {
            get { return resourceid; }
            set { resourceid = value; }
        }
    }
}