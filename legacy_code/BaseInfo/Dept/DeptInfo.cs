using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using Base.DB;

namespace BaseInfo.Dept
{
    public class DeptInfo:BasePO
    {
        private DateTime createTime = DateTime.Now;//创建时间
        private DateTime modifyTime = DateTime.Now;//最后修改时间
        private string deptCode = "";              //部门编码
        private string deptName = "";              //部门名称
        private int deptLevel = 0;                 //部门级别--自动生成
        private int deptType = 0;                  //部门类型
        private string city = "";                  //所在城市
        private string regAddr = "";               //注册地址
        private string officeAddr = "";            //办公地址
        private string postAddr = "";              //邮寄地址
        private string postCode = "";              //邮政编码
        private string tel = "";                   //联系电话
        private string officeTel = "";             //办公电话
        private string fax = "";                    //传真
        private int indepBalance = 0;          //是否独立计算
        private string concessionAuth = "";
        private string contractAuth = "";
        private string tradeAuth = "";
        private string feeAuth = "";
        private string otherAuth = "";
        private int deptStatus = 0;
        private int orderID = 0;//排序号
 
        public override String GetTableName()
        {
            return "";
        }
        public override String GetColumnNames()
        {
            return "DeptCode,DeptName,DeptType,City,RegAddr,OfficeAddr,PostAddr,PostCode,Tel,OfficeTel,Fax,DeptLevel,DeptStatus,OrderID,IndepBalance";
        }
        public override String GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            //return "select DeptCode,DeptName,DeptType,City,RegAddr,OfficeAddr,PostAddr,PostCode,Tel,OfficeTel,Fax,DeptLevel,ConcessionAuth,ContractAuth,TradeAuth,OtherAuth,IndepBalance,DeptStatus from Dept a, DeptAuth b";
            return "select DeptCode,DeptName,DeptType,City,RegAddr,OfficeAddr,PostAddr,PostCode,Tel,OfficeTel,Fax,DeptLevel,DeptStatus,OrderID,IndepBalance from Dept";
        }

        /**
         * 部门信息
         */

        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }
        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }

        public int DeptType
        {
            get { return deptType; }
            set { deptType = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string RegAddr
        {
            get { return regAddr; }
            set { regAddr = value; }
        }
        public string OfficeAddr
        {
            get { return officeAddr; }
            set { officeAddr = value; }
        }
        public string PostAddr
        {
            get { return postAddr; }
            set { postAddr = value; }
        }
        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }
        public string OfficeTel
        {
            get { return officeTel; }
            set { officeTel = value; }
        }
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public int DeptLevel
        {
            get { return deptLevel; }
            set {deptLevel = value;}
        }

        public string ConcessionAuth
        {
            get { return concessionAuth; }
            set { concessionAuth = value; }
        }

        public string ContractAuth
        {
            get { return contractAuth; }
            set { contractAuth = value; }
        }

        public string TradeAuth
        {
            get { return tradeAuth; }
            set { tradeAuth = value; }
        }

        public string FeeAuth
        {
            get { return feeAuth; }
            set { feeAuth = value; }
        }

        public string OtherAuth
        {
            get { return otherAuth; }
            set { otherAuth = value; }
        }


        public int IndepBalance
        {
            get { return indepBalance; }
            set { indepBalance = value; }
        }
        public int DeptStatus
        {
            get { return deptStatus; }
            set { deptStatus = value; }
        }
        public int OrderID
        {
            set { orderID = value; }
            get { return orderID; }
        }
    }
}
