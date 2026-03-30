using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class TaskProcessClass : BasePO
    {
        private int classid = 0;//主键
        private string classname = "";//处理类名称
        private string classdesc = "";//任务描述
        private string classstatus = "";
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private string node = "";

        //是否有效
        public static int CLASSSTATUS_NO = 0;  //否
        public static int CLASSSTATUS_YES = 1; //是

        public static int[] GetClassStatus()
        {
            int[] classStatus = new int[2];
            classStatus[0] = CLASSSTATUS_NO;
            classStatus[1] = CLASSSTATUS_YES;

            return classStatus;
        }

        public static String GetClassStatusDesc(int classStatus)
        {
            if (classStatus == CLASSSTATUS_YES)
            {
                return "Store_Availability";
            }
            if (classStatus == CLASSSTATUS_NO)
            {
                return "Associator_rabInvalid";
            }
            return "Public_Sealed";
        }

        public override String GetTableName()
        {
            return "TaskProcessClass";
        }
        public override String GetColumnNames()
        {
            return "ClassID,ClassName,ClassDesc,ClassStatus,CreateUserID,CreateTime,ModifyUserID,ModifyTime,Node";
        }
        public override String GetUpdateColumnNames()
        {
            return "ClassName,ClassDesc,ClassStatus,ModifyUserID,ModifyTime,Node";
        }
        public override string GetInsertColumnNames()
        {
            return "ClassID,ClassName,ClassDesc,ClassStatus,CreateUserID,CreateTime,Node";
        }
        public int ClassID
        {
            get { return classid; }
            set { classid = value; }
        }
        public string ClassName
        {
            get { return classname; }
            set { classname = value; }
        }
        public string ClassDesc
        {
            get { return classdesc; }
            set { classdesc = value; }
        }
        public string ClassStatus
        {
            get { return classstatus; }
            set { classstatus = value; }
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
        public string Node
        {
            get { return node; }
            set { node = value; }
        }
    }
}