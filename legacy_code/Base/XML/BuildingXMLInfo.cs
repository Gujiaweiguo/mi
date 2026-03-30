using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Base.XML
{
        public class BuildingXMLInfo : BasePO
    {
        private int mallCode = 0;
        private string buildingCode = null;
        private string buildingDesc = null;
        private string img = "";
        private string remark = "";

        public override String GetTableName()
        {
            return "Building";
        }

        public override String GetColumnNames()
        {
            return "BuildingID,BuildingCode,BuildingName,";
        }
        public override String GetInsertColumnNames()
        {
            return "BuildingID,BuildingCode,BuildingName,";
        }

        public override String GetUpdateColumnNames()
        {
            return "BuildingCode,BuildingName,";
        }
            public override string GetQuerySql()
            {
                return "select MallCode,BuildingCode,BuildingDesc,Img,Remark from" +
                       "(select  '' as MallCode,BuildingCode,BuildingName as BuildingDesc," +
                       "'' as Img,'' as Remark  from building )as a";
            }

            public int MallCode
            {
                get { return this.mallCode; }
                set { this.mallCode = value; }
            
            }
        public string BuildingCode
        {
            get { return this.buildingCode; }
            set { this.buildingCode = value; }
        }
            public string BuildingDesc
        {
            get { return this.buildingDesc; }
            set { this.buildingDesc = value; }
        }
        public string Img
        {
            get { return img; }
            set { img = value; }
        }
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

    }
}
