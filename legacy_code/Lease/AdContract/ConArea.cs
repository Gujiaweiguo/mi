using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.AdContract
{
    public class ConArea : BasePO
    {
        private int conareaid = 0;
        private int contractid = 0;
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;
        private string conareacode = "";
        private string conareaname = "";
        private int conareatypeid = 0;
        private string conareadesc = "";
        private int conareastatus = 0;
        private DateTime conareastartdate = DateTime.Now;
        private DateTime conareaenddate = DateTime.Now;
        private decimal rentarea = 0;

        public static int BLANKOUT_STATUS_INVALID = 0;     //无效

        public static int BLANKOUT_STATUS_LEASEOUT = 1;    // 有效

        public static int BLANKOUT_STATUS_PAUSE = 2;    // 待审批


        public override String GetTableName()
        {
            return "ConArea";
        }
        public override String GetColumnNames()
        {
            return "ConAreaID,ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,ConAreaCode,ConAreaName,ConAreaTypeID,ConAreaDesc,ConAreaStatus,ConAreaStartDate,ConAreaEndDate,RentArea";
        }
        public override String GetUpdateColumnNames()
        {
            return "ContractID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,ConAreaCode,ConAreaName,ConAreaTypeID,ConAreaDesc,ConAreaStatus,ConAreaStartDate,ConAreaEndDate,RentArea";
        }
        public int ConAreaID
        {
            get { return conareaid; }
            set { conareaid = value; }
        }
        public int ContractID
        {
            get { return contractid; }
            set { contractid = value; }
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
        public string ConAreaCode
        {
            get { return conareacode; }
            set { conareacode = value; }
        }
        public string ConAreaName
        {
            get { return conareaname; }
            set { conareaname = value; }
        }
        public int ConAreaTypeID
        {
            get { return conareatypeid; }
            set { conareatypeid = value; }
        }
        public string ConAreaDesc
        {
            get { return conareadesc; }
            set { conareadesc = value; }
        }
        public int ConAreaStatus
        {
            get { return conareastatus; }
            set { conareastatus = value; }
        }
        public DateTime ConAreaStartDate
        {
            get { return conareastartdate; }
            set { conareastartdate = value; }
        }
        public DateTime ConAreaEndDate
        {
            get { return conareaenddate; }
            set { conareaenddate = value; }
        }
        public decimal RentArea
        {
            get { return rentarea; }
            set { rentarea = value; }
        }
    }
}