using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Base.XML
{
   public  class FloorXMLInfo : BasePO
    {

        private string buildingCode = null ;
        private string floorCode = null;
        private string floorDesc = null;
        private string remark = "";
        private string  whereSQL = null;

       public FloorXMLInfo(string wheresql )
        {
            whereSQL = wheresql;
        }



        public override String GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "";
            //return "BuildingCode,FloorCode,FloorDesc,Remark";
        }

        public override String GetUpdateColumnNames()
        {
            return "";
            //return "BuildingCode,FloorCode,FloorDesc,Remark";
        }
        public override String GetInsertColumnNames()
        {
            return "";
            //return "BuildingCode,FloorCode,FloorDesc,Remark";

        }
       public override string GetQuerySql()
       {
           //return "select BuildingCode,FloorCode,FloorDesc,Remark from " +
           //       "(select Building.BuildingCode,floors.FloorID as FloorCode ,floors.FloorName as FloorDesc," +
           //       "'' as Remark from building inner join floors on (building.buildingid=floors.buildingid) ) as a";

         return   "select BuildingCode,FloorCode,FloorDesc,Remark from " +
           "(select distinct Building.Buildingid as BuildingCode,floors.FloorID as FloorCode ,floors.FloorName as FloorDesc,'' as Remark " +
           "from building left join floors on (building.buildingid=floors.buildingid) left join " +
           "authuser on (floors.floorid=authuser.floorid ) "+whereSQL+") as a";
          
       }

       public string BuildingCode
       {
           get { return this.buildingCode; }
           set { this.buildingCode = value; }
       }
       public string FloorCode
       {
           get { return this.floorCode; }
           set { this.floorCode = value; }
       }
       public string FloorDesc
       {
           get { return this.floorDesc; }
           set { this.floorDesc = value; }
       }
       public string Remark
       {
           get { return remark; }
           set { remark = value; }
       }
       public string  wheresql
       {
           get { return this.whereSQL; }
           set { this.whereSQL = value; }
       }


    }
}
