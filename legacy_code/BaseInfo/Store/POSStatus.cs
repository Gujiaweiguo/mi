using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class POSStatus : BasePO
    {
        private int storeid = 0;//项目ID
        private int buildingid = 0;//大楼ID
        private int floorid = 0;//楼层ID
        private int locationid = 0;//方位ID
        private int posserverid = 0;//POS服务器ID
        private int shopid = 0;//商铺ID
        private string posid = "";//POSID
        private string ip = "";//IP地址
        private string tpusrid = "";//收银员编号
        private int posstatus = 0;//当前网络状态
        private DateTime poslasttime = DateTime.Now;//POS最后方位时间
        private DateTime updatetime = DateTime.Now;//上传时间

        public override String GetTableName()
        {
            return "POSStatus";
        }
        public override String GetColumnNames()
        {
            return "StoreID,BuildingID,FloorID,LocationID,PosServerID,ShopID,POSID,IP,TpUsrID,POSStatus,POSLastTime,UpdateTime";
        }
        public override String GetUpdateColumnNames()
        {
            return "StoreID,BuildingID,FloorID,LocationID,PosServerID,ShopID,POSID,IP,TpUsrID,POSStatus,POSLastTime,UpdateTime";
        }
        public override string GetInsertColumnNames()
        {
            return "StoreID,BuildingID,FloorID,LocationID,PosServerID,ShopID,POSID,IP,TpUsrID,POSStatus,POSLastTime,UpdateTime";
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
        public int FloorID
        {
            get { return floorid; }
            set { floorid = value; }
        }
        public int LocationID
        {
            get { return locationid; }
            set { locationid = value; }
        }
        public int POSServerID
        {
            get { return posserverid; }
            set { posserverid = value; }
        }
        public int ShopID
        {
            get { return shopid; }
            set { shopid = value; }
        }
        public string POSID
        {
            get { return posid; }
            set { posid = value; }
        }
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }
        public string TpUsrID
        {
            get { return tpusrid; }
            set { tpusrid = value; }
        }
        public int PosStatus
        {
            get { return posstatus; }
            set { posstatus = value; }
        }
        public DateTime POSLastTime
        {
            get { return poslasttime; }
            set { poslasttime = value; }
        }
        public DateTime UpdateTime
        {
            get { return updatetime; }
            set { updatetime = value; }
        }
    }
}