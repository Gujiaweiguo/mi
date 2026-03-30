using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Base.XML
{
    public class ShopXMLInfo2 : BasePO
    {
        private string shopID = null;
        private string shopingMallID = null;
        private string buildingID = null;
        private string floorID = null;
        private string locationID = null;
        private string areaID = null;
        private string unitID = null;
        private string shopCode = null;
        private string shopDesc = null;
        private string floorArea = null;
        private string rentArea = null;
        private string rentStatus = null;
        private string brand = null;
        private string customer = null;
        private string Map = null;
        private string Plan = null;
        private string X = null;
        private string Y = null;
        private string Depth = null;
        private string Rb = null;
        private string Gb = null;
        private string Bb = null;
        private string noX = null;
        private string noY = null;
        private string nameX = null;
        private string nameY = null;
        private string remark = null;


        public override string GetTableName()
        {
            return "";
        }
        public override string GetColumnNames()
        {
            return "";
        }
        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public override string GetQuerySql()
        {
            //return "select " +
            //        "ShopingMallID,BuildingID,FloorID,LocationID,AreaID,UnitID,ShopCode,ShopDesc,FloorArea,RentArea,RentStatus,Brand,Customer,map,plans,x,y,depth,rb,gb,bb,NoX,NoY,NameX,NameY,Remark from"+
            //        "(select ShopingMallID,BuildingID,FloorID,LocationID,AreaID,UnitID,ShopCode,ShopDesc,unitstatus,status,FloorArea,RentArea,"+
            //        "RentStatus,Brand,Customer,map,plans,x,y,depth,NoX,NoY,NameX,NameY,Remark "+
            //        ",case  when unitstatus=1 and status=1 then 255  else 0  end  as 'rb' "+
            //        ",case  when unitstatus=1 and status=1 then 0  else 0  end  as 'gb' "+
            //        ",case  when unitstatus=1 and status=1 then 0  else 0  end  as 'bb' "+
            //        " from (select UnitID,unitcode as ShopCode ,Building.BuildingCode as BuildingID,FloorID ,LocationID,AreaID,FloorArea,'¡¡' as ShopDesc,unit.unitstatus from "+
            //        " unit,building where unit.buildingid=building.buildingid) as b left join (select shopxml.ShopingMallID, shopxml.shopcode as "+
            //        " unitcode,shopxml.RentStatus , shopxml.Brand ,shopxml.Customer,shopxml.map,shopxml.plans "+
            //        " ,shopxml.x,shopxml.y,shopxml.depth,shopxml.rb,shopxml.gb,shopxml.bb,shopxml.NoX,shopxml.NoY,shopxml.NameX,shopxml.NameY,shopxml.Remark  "+
            //        " from shopxml)as a  on (a.unitcode=b.shopcode) left join "+
            //        " (select RentArea,shopcode as shopcodec ,case  when (Select CONVERT(varchar(100), GETDATE(), 23)) > shopenddate  then  '0'   else '1' end as status  from conshop )as c on (b.shopcode=c.shopcodec) ) as d";

                 return "select " +
                        "StoreID,BuildingID,FloorID,LocationID,AreaID,UnitID,ShopCode,ShopDesc,FloorArea,RentArea,RentStatus,Brand,Customer,map,plans,x,y,depth,rb,gb,bb,NoX,NoY,NameX,NameY,Remark " +
                        "from (select shopxml.StoreID,shopxml.BuildingID,shopxml.FloorID,shopxml.LocationID,shopxml.AreaID,shopxml.UnitID, " +
                        "shopxml.ShopCode,shopxml.ShopDesc,shopxml.FloorArea,shopxml.RentArea,shopxml.RentStatus,shopxml.Brand, " +
                        "shopxml.Customer,shopxml.map,shopxml.plans,shopxml.x,shopxml.y,shopxml.depth,shopxml.NoX,shopxml.NoY, " +
                        "shopxml.NameX,shopxml.NameY,shopxml.Remark " +
                        ",case  when unitstatus=1 then 255  else 0  end  as 'rb' " +
                        ",case  when unitstatus=1 then 0  else 0  end  as 'gb' " +
                        ",case  when unitstatus=1 then 0  else 0  end  as 'bb' " +
                        "from shopxml inner join conshop " +
                        "on (shopxml.shopid=conshop.shopid) " +
                        "inner join conshopunit on (conshop.shopid=conshopunit.shopid)" +
                        "inner join unit on (unit.unitid = conshopunit.unitid) " +
                        "inner join contract on (conshop.contractid=contract.contractid) " +
                        "inner join traderelation on " +
                        "(contract.tradeid=traderelation.tradeid)) as a";


        }

        public string ShopID
        {
            get { return this.shopID; }
            set { this.shopID = value; }
        }
        public string StoreID
        {
            get { return this.shopingMallID; }
            set { this.shopingMallID = value; }
        }
        public string BuildingID
        {
            get { return this.buildingID; }
            set { this.buildingID = value; }
        }
        public string FloorID
        {
            get { return floorID; }
            set { floorID = value; }
        }
        public string LocationID
        {
            get { return locationID; }
            set { locationID = value; }
        }
        public string AreaID
        {
            get { return this.areaID; }
            set { this.areaID = value; }
        }
        public string UnitID
        {
            get { return this.unitID; }
            set { this.unitID = value; }
        }
        public string ShopCode
        {
            get { return shopCode; }
            set { shopCode = value; }
        }
        public string ShopDesc
        {
            get { return shopDesc; }
            set { shopDesc = value; }
        }
        public string FloorArea
        {
            get { return this.floorArea; }
            set { this.floorArea = value; }
        }
        public string RentArea
        {
            get { return this.rentArea; }
            set { this.rentArea = value; }
        }
        public string RentStatus
        {
            get { return rentStatus; }
            set { rentStatus = value; }
        }
        public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }
        public string Customer
        {
            get { return this.customer; }
            set { this.customer = value; }
        }
        public string map
        {
            get { return this.Map; }
            set { this.Map = value; }
        }
        public string plan
        {
            get { return Plan; }
            set { Plan = value; }
        }
        public string x
        {
            get { return X; }
            set { X = value; }
        }
        public string y
        {
            get { return this.Y; }
            set { this.Y = value; }
        }
        public string depth
        {
            get { return this.Depth; }
            set { this.Depth = value; }
        }
        public string rb
        {
            get { return Rb; }
            set { Rb = value; }
        }
        public string gb
        {
            get { return Gb; }
            set { Gb = value; }
        }
        public string bb
        {
            get { return this.Bb; }
            set { this.Bb = value; }
        }
        public string NoX
        {
            get { return this.noX; }
            set { this.noX = value; }
        }
        public string NoY
        {
            get { return noY; }
            set { noY = value; }
        }
        public string NameX
        {
            get { return nameX; }
            set { nameX = value; }
        }
        public string NameY
        {
            get { return this.nameY; }
            set { this.nameY = value; }
        }
        public string Remark
        {
            get { return this.remark; }
            set { this.remark = value; }
        }




    }
}
