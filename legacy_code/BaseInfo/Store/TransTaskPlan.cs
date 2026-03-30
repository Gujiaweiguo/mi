using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class TransTaskPlan : BasePO
    {
        private int taskid = 0;
        private int planid = 0;
        private string runtype = "";
        private DateTime startdate = DateTime.Now;
        private DateTime enddate = DateTime.Now;
        private int enddateflag = 1;
        private DateTime starttime = DateTime.Now;
        private DateTime endtime = DateTime.Now;
        private string node = "";

        //任务状态
        public static int RUNTYPE_ONCE = 0;  //无效
        public static int RUNTYPE_TIMES = 1; //有效

        public static int[] GetRunType()
        {
            int[] runType = new int[2];
            runType[0] = RUNTYPE_ONCE;
            runType[1] = RUNTYPE_TIMES;

            return runType;
        }

        public static String GetRunTypeDesc(int runType)
        {
            if (runType == RUNTYPE_ONCE)
            {
                return "Store_RunOnce";
            }
            if (runType == RUNTYPE_TIMES)
            {
                return "Store_RunTimes";
            }
            return "Public_Sealed";
        }

        public override String GetTableName()
        {
            return "TransTaskPlan";
        }
        public override String GetColumnNames()
        {
            return "TaskID,PlanID,RunType,StartDate,EndDate,EndDateFlag,StartTime,EndTime,Node";
        }
        public override String GetUpdateColumnNames()
        {
            return "RunType,StartDate,EndDate,EndDateFlag,StartTime,EndTime,Node";
        }
        public override String GetInsertColumnNames()
        {
            return "TaskID,PlanID,RunType,StartDate,EndDate,EndDateFlag,StartTime,EndTime,Node";
        }
        public int TaskID
        {
            get { return taskid; }
            set { taskid = value; }
        }
        public int PlanID
        {
            get { return planid; }
            set { planid = value; }
        }
        public string RunType
        {
            get { return runtype; }
            set { runtype = value; }
        }
        public DateTime StartDate
        {
            get { return startdate; }
            set { startdate = value; }
        }
        public DateTime EndDate
        {
            get { return enddate; }
            set { enddate = value; }
        }
        public int EndDateFlag
        {
            get { return enddateflag; }
            set { enddateflag = value; }
        }
        public DateTime StartTime
        {
            get { return starttime; }
            set { starttime = value; }
        }
        public DateTime EndTime
        {
            get { return endtime; }
            set { endtime = value; }
        }
        public string Node
        {
            get { return node; }
            set { node = value; }
        }
    }
}