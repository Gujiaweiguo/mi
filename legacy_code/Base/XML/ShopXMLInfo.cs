using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Base.XML
{
    public class ShopXMLInfo : BasePO
    {

        private string whereSQL = null;
        private int ShopType = 0;
        string sql = "";

        //  shopType=0 业态分布
        //  shopType=1 合同状态
        //  shopType=2 销售分析
        public ShopXMLInfo(string wheresql,int shopType)
        {
            whereSQL = wheresql;
            ShopType = shopType;
            if (ShopType == 0)
            {
//                sql = @"select 
//                        localarea.localID as locaID,localcity.deptid as cityID,store.storeid as mallID,building.buildingID as buildID,floors.floorID,conshop.shopID,unit.UnitID,
//                        conshop.ShopCode,conshop.shopname as ShopDesc,unit.FloorArea,unit.RentArea,unit.unitstatus as RentStatus,conshop.shopname as Brand,
//                        Customer.custname as Customer,unit.map,unit.map plans,unit.x,unit.y,1 as depth,TradeRelation.rb,TradeRelation.gb,TradeRelation.bb,0 as nox,0 as NoY,0 as NameX,0 as NameY,null as Remark
//                         from localarea inner join localcity on localarea.localid=localcity.localid 
//                        inner join dept on localcity.deptid=localcity.deptid
//                        inner join store on dept.deptid=store.storeid
//                        inner join building on store.storeid=building.storeid
//                        inner join floors on building.buildingid=floors.buildingid
//                        inner join unit on floors.floorid=unit.floorid
//                        inner join conshopunit on unit.unitid=conshopunit.unitid
//                        inner join conshop on conshopunit.shopid=conshop.shopid
//                        inner join contract on conshop.contractid=contract.contractid
//                        inner join customer on contract.custid=customer.custid
//                        inner join TradeRelation on TradeRelation.TradeID=contract.TradeID " + whereSQL;
                sql= @"select 
                        localarea.localID as locaID,localcity.deptid as cityID,store.storeid as mallID,building.buildingID as buildID,floors.floorID,conshop.shopID,unit.UnitID,
                        conshop.ShopCode,conshop.shopname as ShopDesc,unit.FloorArea,unit.RentArea,unit.unitstatus as RentStatus,conshop.shopname as Brand,
                        Customer.custname as Customer,isnull(unit.map,'../../VAGraphic/img/units/' + cast(unit.unitid as varchar(5)) + '.png') map,unit.map plans,unit.x,unit.y,1 as depth,isnull(TradeRelation.rb,0) rb,isnull(TradeRelation.gb,0) gb,isnull(TradeRelation.bb,0) bb,0 as nox,0 as NoY,0 as NameX,0 as NameY,null as Remark
                        from unit
                        inner join store on unit.StoreID =store.storeid
                        inner join building on unit.BuildingID =building.BuildingID 
                        inner join floors on unit.floorid=floors.floorid
                        inner join dept on store.storeid=Dept.DeptID 
                        left join (select deptid,pdeptid, deptname from Dept where DeptType=5 and DeptStatus=1) localcity on (localcity.DeptID=dept.PDeptID)
                        left join (select deptid localID,pdeptid deptname from Dept where DeptType=4 and DeptStatus=1) localarea on (localarea.localID=localcity.PDeptID)
                        left join conshopunit on unit.unitid=conshopunit.unitid
                        left join conshop on conshopunit.shopid=conshop.shopid
                        left join contract on conshop.contractid=contract.contractid
                        left join customer on contract.custid=customer.custid
                        left join TradeRelation on TradeRelation.TradeID=contract.TradeID " + whereSQL;
            }
            if (ShopType == 1)
            {
                sql = @"select 
                    localarea.localID as locaID,localcity.deptid as cityID,store.storeid as mallID,building.buildingID as buildID,floors.floorID,conshop.shopID,unit.UnitID,
                    conshop.ShopCode,conshop.shopname as ShopDesc,unit.FloorArea,unit.RentArea,unit.unitstatus as RentStatus,conshop.shopname as Brand,
                    Customer.custname as Customer,isnull(unit.map,'../../VAGraphic/img/units/' + cast(unit.unitid as varchar(5)) + '.png') map,unit.map plans,unit.x,unit.y,1 as depth,
                    case when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>0 and 30>datediff(day,getdate(),isnull(contract.stopdate,conenddate)) then 255 
                    when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>30 then 0 when datediff(day,getdate(),isnull(contract.stopdate,conenddate))<=0 then 255 
                    end as rb,
				                        case when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>0 and 30>datediff(day,getdate(),isnull(contract.stopdate,conenddate)) then 255 
                    when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>30 then 255 when datediff(day,getdate(),isnull(contract.stopdate,conenddate))<=0 then 0 
                    end as gb,
				                        case when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>0 and 30>datediff(day,getdate(),isnull(contract.stopdate,conenddate)) then 0
                    when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>30 then 0 when datediff(day,getdate(),isnull(contract.stopdate,conenddate))<=0 then 0 
                    end as bb,
                    0 NoX,0 NoY,0 NameX,0 NameY,null Remark
                   from unit
                        inner join store on unit.StoreID =store.storeid
                        inner join building on unit.BuildingID =building.BuildingID 
                        inner join floors on unit.floorid=floors.floorid
                        inner join dept on store.storeid=Dept.DeptID 
                        left join (select deptid,pdeptid, deptname from Dept where DeptType=5 and DeptStatus=1) localcity on (localcity.DeptID=dept.PDeptID)
                        left join (select deptid localID,pdeptid deptname from Dept where DeptType=4 and DeptStatus=1) localarea on (localarea.localID=localcity.PDeptID)
                        left join conshopunit on unit.unitid=conshopunit.unitid
                        left join conshop on conshopunit.shopid=conshop.shopid
                        left join contract on conshop.contractid=contract.contractid
                        left join customer on contract.custid=customer.custid
                        left join TradeRelation on TradeRelation.TradeID=contract.TradeID  " + whereSQL;
            }
            if (ShopType == 2)
            {
                sql = @"select 
                    localarea.localID as locaID,localcity.deptid as cityID,store.storeid as mallID,building.buildingID as buildID,floors.floorID,conshop.shopID,unit.UnitID,
                    conshop.ShopCode,conshop.shopname as ShopDesc,unit.FloorArea,unit.RentArea,unit.unitstatus as RentStatus,conshop.shopname as Brand,
                    Customer.custname as Customer,isnull(unit.map,'../../VAGraphic/img/units/' + cast(unit.unitid as varchar(5)) + '.png') map,unit.map plans,unit.x,unit.y,1 as depth,
                    case when transshopmth.paidamt<10000 then 255 when transshopmth.paidamt>=10000 and transshopmth.paidamt<30000 then 0 
						 when transshopmth.paidamt>=30000 and transshopmth.paidamt<50000 then 255 when transshopmth.paidamt>=50000 and transshopmth.paidamt<100000 then 26  else 0 end as rb,
					case when transshopmth.paidamt<10000 then 0 when transshopmth.paidamt>=10000 and transshopmth.paidamt<30000 then 255 
						 when transshopmth.paidamt>=30000 and transshopmth.paidamt<50000 then 255 when transshopmth.paidamt>=50000 and transshopmth.paidamt<100000 then 0  else 0 end as gb,
					case when transshopmth.paidamt<10000 then 0 when transshopmth.paidamt>=10000 and transshopmth.paidamt<30000 then 0 
						 when transshopmth.paidamt>=30000 and transshopmth.paidamt<50000 then 0 when transshopmth.paidamt>=50000 and transshopmth.paidamt<100000 then 184  else 0 end as bb,
                    0 NoX,0 NoY,0 NameX,0 NameY,null Remark
                     from unit
                        inner join store on unit.StoreID =store.storeid
                        inner join building on unit.BuildingID =building.BuildingID 
                        inner join floors on unit.floorid=floors.floorid
                        inner join dept on store.storeid=Dept.DeptID 
                        left join (select deptid,pdeptid, deptname from Dept where DeptType=5 and DeptStatus=1) localcity on (localcity.DeptID=dept.PDeptID)
                        left join (select deptid localID,pdeptid, deptname from Dept where DeptType=4 and DeptStatus=1) localarea on (localarea.localID=localcity.PDeptID)
                        left join conshopunit on unit.unitid=conshopunit.unitid
                        left join conshop on conshopunit.shopid=conshop.shopid
                        left join contract on conshop.contractid=contract.contractid
                        left join customer on contract.custid=customer.custid
                        left join TradeRelation on TradeRelation.TradeID=contract.TradeID 
                        left join (select ShopID,paidamt from TransShopMth where Month='" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddMonths(-1).Month.ToString() + "-01') as TransShopMth on (TransShopMth.ShopID=ConShop.ShopID) " + whereSQL ;

            }



        }

        public override String GetTableName()
        {
            return "";
        }
        public override String GetColumnNames()
        {
            return "";
        }
        public override String GetUpdateColumnNames()
        {
            return "";
        }
        public override string GetQuerySql()
        {
            return sql;
        }
      

    }
}