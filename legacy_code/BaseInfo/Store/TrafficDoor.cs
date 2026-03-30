using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace BaseInfo.Store
{
    public class TrafficDoor : BasePO
    {
        private int storeid = 0;
        private int buildingid = 0;
        private int doorid = 0;
        private string doorname = "";
        private int doorstatus = 0;
        private string note = "";
        public override String GetTableName()
        {
            return "TrafficDoor";
        }
        public override String GetColumnNames()
        {
            return "StoreID,BuildingID,DoorID,DoorName,DoorStatus,Note";
        }
        public override String GetUpdateColumnNames()
        {
            return "StoreID,BuildingID,DoorName,DoorStatus,Note";
        }
        public override string GetInsertColumnNames()
        {
            return "StoreID,BuildingID,DoorName,DoorStatus,Note";
        }
        public int StoreID
        {
            get { return storeid; }
            set { storeid = value; }
        }
        public int BuildingID
        {
            get { return buildingid; }
            set { buildingid = value; }
        }
        public int DoorID
        {
            get { return doorid; }
            set { doorid = value; }
        }
        public string DoorName
        {
            get { return doorname; }
            set { doorname = value; }
        }
        public int DoorStatus
        {
            get { return doorstatus; }
            set { doorstatus = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}