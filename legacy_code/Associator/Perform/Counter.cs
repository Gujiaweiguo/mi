using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 服务台
    /// </summary>
    public class Counter : BasePO
    {
        private int counterID = 0; //服务台ID
        private string counterDesc = ""; //服务台描叙
        private string counterLoc = ""; //服务台位置

        public int CounterID
        {
            get { return counterID; }
            set { counterID = value; }
        }

        public string CounterDesc
        {
            get { return counterDesc; }
            set { counterDesc = value; }
        }

        public string CounterLoc
        {
            get { return counterLoc; }
            set { counterLoc = value; }
        }

        public override string GetTableName()
        {
            return "Counter";
        }

        public override string GetColumnNames()
        {
            return "CounterID,CounterDesc,CounterLoc";
        }

        public override string GetInsertColumnNames()
        {
            return "CounterID,CounterDesc,CounterLoc";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
    }
}
