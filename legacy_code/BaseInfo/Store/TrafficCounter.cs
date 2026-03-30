using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace BaseInfo.Store
{
    public class TrafficCounter : BasePO
    {
        private int storeid = 0;
        private int doorid = 0;
        private int counterid = 0;
        private string name = "";
        private int status = 0;
        private string note = "";
        public override String GetTableName()
        {
            return "trafficcounter";
        }
        public override String GetColumnNames()
        {
            return "StoreID,DoorID,CounterID,Name,Status,Note";
        }
        public override String GetUpdateColumnNames()
        {
            return "StoreID,DoorID,CounterID,Name,Status,Note";
        }
        public override string GetInsertColumnNames()
        {
            return "StoreID,DoorID,CounterID,Name,Status,Note";
        }
        public int StoreID
        {
            get { return storeid; }
            set { storeid = value; }
        }
        public int DoorID
        {
            get { return doorid; }
            set { doorid = value; }
        }
        public int CounterID
        {
            get { return counterid; }
            set { counterid = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Status
        {
            get { return status; }
            set { status = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}