using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
using Base.Util;
using Base.Biz;

using Base;

namespace BaseInfo.Dept
{
    public class MiInfoVindicate :CommonInfoPO
    {
        private int deptId = 0;                    //部门内码
        private string deptName = "";            //办公地址
        private string officeAddr = "";            //办公地址
        private string officeAddr2 = "";            //办公地址
        private string officeAddr3 = "";            //办公地址
        private string postAddr = "";              //邮寄地址
        private string postAddr2 = "";              //邮寄地址
        private string postAddr3 = "";              //邮寄地址
        private string postCode = "";              //邮政编码
        private string eMail = null;               //电子邮箱
        private string web = null;                 //网站

        private string officeTel = "";             //办公电话
        private string tel = "";                   //招商电话
        private string propertytel = "";           //物业电话

        private string legalRep = "";               //法定代表人

        private string legalRepTitle = "";               //LegalRepTitle
        private decimal regCap = 0;               //注册资金
        private string regAddr = "";               //注册地址

        private string regCode = "";                 //工商注册号

        private string taxCode = "";                 //税务登记号

        private string bankName = "";                //开户银行

        private string bankAcct = "";                //银行开户帐号


        public int DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }
        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }

        public string OfficeAddr
        {
            get { return officeAddr; }
            set { officeAddr = value; }
        }

        public string OfficeAddr2
        {
            get { return officeAddr2; }
            set { officeAddr2 = value; }
        }

        public string OfficeAddr3
        {
            get { return officeAddr3; }
            set { officeAddr3 = value; }
        }
        public string PostAddr
        {
            get { return postAddr; }
            set { postAddr = value; }
        }

        public string PostAddr2
        {
            get { return postAddr2; }
            set { postAddr2 = value; }
        }

        public string PostAddr3
        {
            get { return postAddr3; }
            set { postAddr3 = value; }
        }



        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }
        public string EMail
        {
            get { return eMail; }
            set { eMail = value; }
        }
        public string Web
        {
            get { return web; }
            set { web = value; }
        }

        public string OfficeTel
        {
            get { return officeTel; }
            set { officeTel = value; }
        }
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }
        public string Propertytel
        {
            get { return propertytel; }
            set { propertytel = value; }
        }


        public string LegalRep
        {
            get { return legalRep; }
            set { legalRep = value; }
        }
        public string LegalRepTitle
        {
            get { return legalRepTitle; }
            set { legalRepTitle = value; }
        }
        public decimal RegCap
        {
            get { return regCap; }
            set { regCap = value; }
        }
        public string RegAddr
        {
            get { return regAddr; }
            set { regAddr = value; }
        }


        public string RegCode
        {
            get { return regCode; }
            set { regCode = value; }
        }
        public string TaxCode
        {
            get { return taxCode; }
            set { taxCode = value; }
        }
        public string BankName
        {
            get { return bankName; }
            set { bankName = value; }
        }
        public string BankAcct
        {
            get { return bankAcct; }
            set { bankAcct = value; }
        }

        public override string GetTableName()
        {
            return "Dept";
        }

        public override string GetColumnNames()
        {
            return "DeptId,DeptName,OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3,PostCode,OfficeTel,Tel,Propertytel,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct";
        }

        public override string GetUpdateColumnNames()
        {
            return "DeptName,OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3,PostCode,OfficeTel,Tel,Propertytel,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct";
        }
        public override string GetInsertColumnNames()
        {
            return "DeptId,DeptName,OfficeAddr,OfficeAddr2,OfficeAddr3,PostAddr,PostAddr2,PostAddr3,PostCode,OfficeTel,Tel,Propertytel,LegalRep,LegalRepTitle,RegCap,RegAddr,RegCode,TaxCode,BankName,BankAcct," + GetColumnNames();
        }
    }
}
