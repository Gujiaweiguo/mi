using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Base.XML
{
   public  class ShoppingMallXMLInfo : BasePO
    {
        private string shopingMallArea = "";
        private int shopingMallCode = 0;                    //部门内码
        //private string deptCode = "";              //部门编码
        private string shopingMallDesc = "";              //部门名称
        private string address = "";                  //所在地址
        private string img = "";
        private string remark = "";

        
       
        public override String GetTableName()
        {
            return "Dept";
        }
        public override String GetColumnNames()
        {
            return "DeptID,DeptCode,DeptName,City";

        }
        public override String GetUpdateColumnNames()
        {
            return "DeptCode,DeptName,City";
        }

        public override string GetQuerySql()
        {

            return "select ShopingMallArea,ShopingMallCode,ShopingMallDesc,Address,Img,Remark from" +
                   "(select '' as ShopingMallArea,deptid as ShopingMallCode, deptname as ShopingMallDesc, City as Address ,''as Img , '' as Remark  from  dept " +
                   "where deptid=100) as a";
        }
        public string ShopingMallArea
        {
            get { return shopingMallArea; }
            set { shopingMallArea = value; }
        }
        public int ShopingMallCode
        {
            get { return shopingMallCode; }
            set { shopingMallCode = value; }
        }

        public string ShopingMallDesc
        {
            get { return shopingMallDesc; }
            set { shopingMallDesc = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
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
