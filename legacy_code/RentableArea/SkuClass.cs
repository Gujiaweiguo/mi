using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace RentableArea
{
    public class SkuClass : BasePO
    {
        private int classid = 0;
        private int pclassid = 0;
        private string classcode = "";
        private string classname = "";
        private int classstatus = 0;
        private int classlevel = 0;
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;

        public override String GetTableName()
        {
            return "SkuClass";
        }
        public override String GetColumnNames()
        {
            return "ClassID,PClassID,ClassCode,ClassName,ClassStatus,ClassLevel,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "ClassCode,ClassName,ClassStatus,ModifyUserID,ModifyTime";
        }
        public int ClassID
        {
            get { return classid; }
            set { classid = value; }
        }
        public int PClassID
        {
            get { return pclassid; }
            set { pclassid = value; }
        }
        public string ClassCode
        {
            get { return classcode; }
            set { classcode = value; }
        }
        public string ClassName
        {
            get { return classname; }
            set { classname = value; }
        }
        public int ClassStatus
        {
            get { return classstatus; }
            set { classstatus = value; }
        }
        public int ClassLevel
        {
            get { return classlevel; }
            set { classlevel = value; }
        }
        public int CreateUserID
        {
            get { return createuserid; }
            set { createuserid = value; }
        }
        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }
        public int ModifyUserID
        {
            get { return modifyuserid; }
            set { modifyuserid = value; }
        }
        public DateTime ModifyTime
        {
            get { return modifytime; }
            set { modifytime = value; }
        }
        public int OprRoleID
        {
            get { return oprroleid; }
            set { oprroleid = value; }
        }
        public int OprDeptID
        {
            get { return oprdeptid; }
            set { oprdeptid = value; }
        }
    }
}