using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Base.XML
{
           public  class AreaColorXMLInfo : BasePO
    {
               private int RentArea = 0;

               public AreaColorXMLInfo(int rentArea)
        {
            RentArea = rentArea;
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
    "select " +
    "ShopingMallID,BuildingID,FloorID,LocationID,AreaID,UnitID,ShopCode,ShopDesc,FloorArea,RentArea,RentStatus,Brand,Customer,map,plans,x,y,depth,rb,gb,bb,NoX,NoY,NameX,NameY,Remark " +
    "from(select " +
    " ShopingMallID,BuildingID,FloorID,LocationID,AreaID,UnitID,ShopCode,ShopDesc,unitstatus,status,FloorArea,RentArea,RentStatus,Brand,Customer,map,plans,x,y,depth,NoX,NoY,NameX,NameY,Remark " +
    " ,case  when  status=1 then  255 else 153  end  as 'rb' ," +
    " case  when  status=1 then 153  else 204  end  as 'gb' " +
    " ,case  when   status=1 then 0  else 0  end  as 'bb'  " +
    " from (select UnitID,unitcode as ShopCode ,Building.BuildingCode as " +
    " BuildingID,FloorID ,LocationID,AreaID,FloorArea,'¡¡' as ShopDesc,unit.unitstatus from  unit,building where " +
    " unit.buildingid=building.buildingid) as b left join (select shopxml.ShopingMallID, shopxml.shopcode as  unitcode,shopxml.RentStatus , " +
    " shopxml.Brand ,shopxml.Customer,shopxml.map,shopxml.plans  " +
    " ,shopxml.x,shopxml.y,shopxml.depth,shopxml.rb,shopxml.gb,shopxml.bb,shopxml.NoX,shopxml.NoY,shopxml.NameX,shopxml.NameY,shopxml.Remark   " +
    " from shopxml)as a  on (a.unitcode=b.shopcode) left join  (select RentArea,shopcode as shopcodec ,case  when  RentArea> " + RentArea + "  then  '0'   else '1' end as status  from conshop )as c on (b.shopcode=c.shopcodec) ) as d";


        }
    }
}
