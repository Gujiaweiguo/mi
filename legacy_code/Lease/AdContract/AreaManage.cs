using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.AdContract
{
    public class AreaManage : BasePO
    {
        private int areaid = 0;
        private string areacode = "";
        private string areaname = "";
        private int areatypeid = 0;
        private int areastatus = 0;
        private string note = "";
        private int storeid = 0;
        private int buildingid = 0;

        public override String GetTableName()
        {
            return "AreaManage";
        }
        public override String GetColumnNames()
        {
            return "AreaID,AreaCode,AreaName,AreaTypeID,AreaStatus,Note,StoreID,BuildingID";
        }
        public override String GetUpdateColumnNames()
        {
            return "AreaCode,AreaName,AreaTypeID,AreaStatus,Note";
        }
        public int AreaID
        {
            get { return areaid; }
            set { areaid = value; }
        }
        public string AreaCode
        {
            get { return areacode; }
            set { areacode = value; }
        }
        public string AreaName
        {
            get { return areaname; }
            set { areaname = value; }
        }
        public int AreaTypeID
        {
            get { return areatypeid; }
            set { areatypeid = value; }
        }
        public int AreaStatus
        {
            get { return areastatus; }
            set { areastatus = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
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
    }
}