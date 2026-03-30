using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.AdContract
{
    public class AdBoardManage : BasePO
    {
        private int adboardid = 0;
        private string adboardcode = "";
        private string adboardname = "";
        private int adboardtypeid = 0;
        private int adboardstatus = 0;
        private string note = "";
        private int storeid = 0;
        private int buildingid = 0;
        private decimal floorarea = 0;
        private decimal usearea = 0;
        

        public static int ADSTATUS_NO = 0;  //无效
        public static int ADSTATUS_YES = 1; //有效

        public static int[] GetAdStatus()
        {
            int[] AdStatus = new int[2];
            AdStatus[0] = ADSTATUS_YES;
            AdStatus[1] = ADSTATUS_NO;

            return AdStatus;
        }

        public static String GetAdStatusDesc(int AdStatus)
        {
            if (AdStatus == ADSTATUS_YES)
            {
                return "CUST_TYPE_STATUS_VALID";
            }
            if (AdStatus == ADSTATUS_NO)
            {
                return "CUST_TYPE_STATUS_INVALID";
            }
            return "Public_Sealed";
        }

        public override String GetTableName()
        {
            return "AdBoardManage";
        }
        public override String GetColumnNames()
        {
            return "AdBoardID,AdBoardCode,AdBoardName,AdBoardTypeID,AdBoardStatus,Note,StoreID,BuildingID,FloorArea,UseArea";
        }
        public override String GetUpdateColumnNames()
        {
            return "AdBoardCode,AdBoardName,AdBoardTypeID,AdBoardStatus,Note,FloorArea,UseArea";
        }
        public int AdBoardID
        {
            get { return adboardid; }
            set { adboardid = value; }
        }
        public string AdBoardCode
        {
            get { return adboardcode; }
            set { adboardcode = value; }
        }
        public string AdBoardName
        {
            get { return adboardname; }
            set { adboardname = value; }
        }
        public int AdBoardTypeID
        {
            get { return adboardtypeid; }
            set { adboardtypeid = value; }
        }
        public int AdBoardStatus
        {
            get { return adboardstatus; }
            set { adboardstatus = value; }
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
        public decimal FloorArea
        {
            get { return floorarea; }
            set { floorarea = value; }
        }
        public decimal UseArea
        {
            get { return usearea; }
            set { usearea = value; }
        }
    }
}