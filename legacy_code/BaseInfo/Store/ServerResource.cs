using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class ServerResource : BasePO
    {
        private int resourceid = 0;
        private string resourcecode = "";
        private string resourcename = "";
        private int deptid = 0;
        private string ip = "";
        private int port = 0;
        private string loginnm = "";
        private string pwd = "";
        private string dbname = "";
        private string status = "";
        private string node = "";
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;

        //状态
        public static int STATUS_UNUSED = 0;  //未启用
        public static int STATUS_AVAILABILITY = 1; //有效
        public static int STATUS_CANCEL= 2; //作废

        public static int[] GetStatus()
        {
            int[] getStatus = new int[3];
            getStatus[0] = STATUS_UNUSED;
            getStatus[1] = STATUS_AVAILABILITY;
            getStatus[2] = STATUS_CANCEL;

            return getStatus;
        }

        public static String GetStatusDec(int getStatus)
        {
            if (getStatus == STATUS_UNUSED)
            {
                return "Store_UnUsed";
            }
            if (getStatus == STATUS_AVAILABILITY)
            {
                return "Store_Availability";
            }
            if (getStatus == STATUS_CANCEL)
            {
                return "ConLease_butDel";
            }
            return "Public_Sealed";
        }

        public override String GetTableName()
        {
            return "ServerResource";
        }
        public override String GetColumnNames()
        {
            return "ResourceID,ResourceCode,ResourceName,DeptID,IP,PORT,LoginNm,Pwd,DBName,STATUS,Node,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "ResourceCode,ResourceName,DeptID,IP,PORT,LoginNm,Pwd,DBName,STATUS,Node,ModifyUserID,ModifyTime";
        }
        public override string GetInsertColumnNames()
        {
            return "ResourceID,ResourceCode,ResourceName,DeptID,IP,PORT,LoginNm,Pwd,DBName,STATUS,Node,CreateUserID,CreateTime";
        }
        public int ResourceID
        {
            get { return resourceid; }
            set { resourceid = value; }
        }
        public string ResourceCode
        {
            get { return resourcecode; }
            set { resourcecode = value; }
        }
        public string ResourceName
        {
            get { return resourcename; }
            set { resourcename = value; }
        }
        public int DeptID
        {
            get { return deptid; }
            set { deptid = value; }
        }
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }
        public int PORT
        {
            get { return port; }
            set { port = value; }
        }
        public string LoginNm
        {
            get { return loginnm; }
            set { loginnm = value; }
        }
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        public string DBName
        {
            get { return dbname; }
            set { dbname = value; }
        }
        public string STATUS
        {
            get { return status; }
            set { status = value; }
        }
        public string Node
        {
            get { return node; }
            set { node = value; }
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