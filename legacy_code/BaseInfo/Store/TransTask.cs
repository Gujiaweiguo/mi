using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class TransTask : BasePO
    {
        private int taskid = 0;
        private string taskname = "";
        private string tasktype = "";
        private int classid = 0;
        private int retry = 0;
        private string taskstatus = "";
        private int priority = 0;
        private string node = "";
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;

        //任务类别
        public static int TASKTYPE_UNDEFINE = 0;  //未定义
        public static int TASKTYPE_UPDATE = 1; //上传
        public static int TASKTYPE_DOWNLOAD = 2; //下载

        public static int[] GetTaskType()
        {
            int[] taskType = new int[3];
            taskType[0] = TASKTYPE_UNDEFINE;
            taskType[1] = TASKTYPE_UPDATE;
            taskType[2] = TASKTYPE_DOWNLOAD;

            return taskType;
        }

        public static String GetTaskTypeDec(int taskType)
        {
            if (taskType == TASKTYPE_UNDEFINE)
            {
                return "Store_UnDefine";
            }
            if (taskType == TASKTYPE_UPDATE)
            {
                return "Store_Update";
            }
            if (taskType == TASKTYPE_DOWNLOAD)
            {
                return "Store_DownLoad";
            }
            return "Public_Sealed";
        }

        //任务状态
        public static int TASKSTATUS_NO = 0;  //无效
        public static int TASKSTATUS_YES = 1; //有效

        public static int[] GetTaskStatus()
        {
            int[] taskStatus = new int[2];
            taskStatus[0] = TASKSTATUS_NO;
            taskStatus[1] = TASKSTATUS_YES;

            return taskStatus;
        }

        public static String GetTaskStatusDesc(int taskStatus)
        {
            if (taskStatus == TASKSTATUS_YES)
            {
                return "CUST_TYPE_STATUS_VALID";
            }
            if (taskStatus == TASKSTATUS_NO)
            {
                return "CUST_TYPE_STATUS_INVALID";
            }
            return "Public_Sealed";
        }

        //优先级
        public static int PRIORITY_HIGHEST = 0;  //最高级
        public static int PRIORITY_FIRST = 1; //第一级
        public static int PRIORITY_SECOND = 2; //第二级

        public static int[] GetPriority()
        {
            int[] priority = new int[3];
            priority[0] = PRIORITY_HIGHEST;
            priority[1] = PRIORITY_FIRST;
            priority[2] = PRIORITY_SECOND;

            return priority;
        }

        public static String GetPriorityDec(int priority)
        {
            if (priority == PRIORITY_HIGHEST)
            {
                return "Store_HighestLevel";
            }
            if (priority == PRIORITY_FIRST)
            {
                return "Store_FirstLevel";
            }
            if (priority == PRIORITY_SECOND)
            {
                return "Store_SecondLevel";
            }
            return "Public_Sealed";
        }

        public override String GetTableName()
        {
            return "TransTask";
        }
        public override String GetColumnNames()
        {
            return "TaskID,TaskName,TaskType,Retry,TaskStatus,Priority,Node,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,ClassID";
        }
        public override String GetUpdateColumnNames()
        {
            return "TaskName,TaskType,Retry,TaskStatus,Priority,Node,ModifyUserID,ModifyTime,ClassID";
        }
        public override String GetInsertColumnNames()
        {
            return "TaskID,TaskName,TaskType,Retry,TaskStatus,Priority,Node,CreateUserID,CreateTime,ClassID";
        }

        public int TaskID
        {
            get { return taskid; }
            set { taskid = value; }
        }
        public string TaskName
        {
            get { return taskname; }
            set { taskname = value; }
        }
        public string TaskType
        {
            get { return tasktype; }
            set { tasktype = value; }
        }
        public int ClassID
        {
            get { return classid; }
            set { classid = value; }
        }
        public int Retry
        {
            get { return retry; }
            set { retry = value; }
        }
        public string TaskStatus
        {
            get { return taskstatus; }
            set { taskstatus = value; }
        }
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
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