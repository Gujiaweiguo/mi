using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Base.XML
{
    public class ShopXMLInfo3 : BasePO
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
        private string WhereStart = null;
        private string WhereEnd = null;
        private string AvgStart = null;
        private string AvgEnd = null;
        private string DataSource = null;

        public ShopXMLInfo3(string wherestart, string whereend, string avgstart, string avgend, string datasource)
        { 
             WhereStart = wherestart;
             WhereEnd = whereend;
             AvgStart =avgstart;
             AvgEnd = avgend;
             DataSource = datasource;
        }

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
            return
            "select "+
           "StoreID,BuildingID,FloorID,LocationID,AreaID,UnitID,ShopCode,ShopDesc,FloorArea,RentArea,RentStatus,Brand,Customer,map,plans,x,y,depth,rb,gb,bb,NoX,NoY,NameX,NameY,Remark "+
            "from(select "+
           " StoreID,BuildingID,FloorID,LocationID,AreaID,UnitID,ShopCode,ShopDesc,unitstatus,FloorArea,RentArea,RentStatus,Brand,Customer,map,plans,x,y,depth,NoX,NoY,NameX,NameY,Remark ,SalesAmt,AvgAreaSales"+
            ",case  when areasales=1 then 80  else 255  end  as 'rb' ,"+
            "case  when areasales=1  then 124  else 153  end  as 'gb' "+
           " ,case  when areasales =1 then 255  else 0  end  as 'bb' "+
           "  from (select UnitID,unitcode as ShopCode ,Building.BuildingCode as "+
            "BuildingID,FloorID ,LocationID,AreaID,FloorArea,'¡¡' as ShopDesc,unit.unitstatus from  unit,building where "+
           " unit.buildingid=building.buildingid) as b left join (select shopxml.storeid, shopxml.shopcode as  unitcode,shopxml.RentStatus , "+
            "shopxml.Brand ,shopxml.Customer,shopxml.map,shopxml.plans  "+
            ",shopxml.x,shopxml.y,shopxml.depth,shopxml.rb,shopxml.gb,shopxml.bb,shopxml.NoX,shopxml.NoY,shopxml.NameX,shopxml.NameY,shopxml.Remark   "+
           " from shopxml)as a  on (a.unitcode=b.shopcode) left join  (select RentArea,shopcode as shopcodec  "+
            "from conshop )as c on (b.shopcode=c.shopcodec)left join "+
            "(SELECT SUM(salesAmt) AS SalesAmt,SUM(area) AS area,shopcodes as shopcodes,CASE SUM(area) WHEN 0 THEN 0 ELSE SUM(salesAmt)/SUM(area) END AS AvgAreaSales,"+
            "case when  SUM(salesAmt)/SUM(area) between "+AvgStart+" and "+AvgEnd+" then 1 else 0 end as areasales"+
           " FROM (SELECT transSku.shopID,MAX(conshop.RentArea) AS area,conshop.shopcode as shopcodes,SUM(transSku.paidAmt) AS salesAmt FROM transSku INNER JOIN "+
            "conShop ON (conShop.shopID = transSku.shopID)  WHERE bizDate between "+
            "'" + WhereStart + "' AND '" + WhereEnd + "' "+DataSource+"  GROUP BY transSku.shopID,conshop.shopcode ) AS f  group by shopcodes  ) as g on (b.shopcode=g.shopcodes) ) as d ";
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

        public string wherestart
        {
            get { return WhereStart; }
            set { WhereStart = value; }
        }
        public string whereend
        {
            get { return WhereEnd; }
            set { WhereEnd = value; }
        }
        public string avgstart
        {
            get { return this.AvgStart; }
            set { this.AvgStart = value; }
        }
        public string avgend
        {
            get { return this.AvgEnd; }
            set { this.AvgEnd = value; }
        }
        public string datasource
        {
            get { return this.DataSource; }
            set { this.DataSource = value; }
        }

    }
}
