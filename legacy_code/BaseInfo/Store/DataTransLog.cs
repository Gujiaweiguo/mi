using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace BaseInfo.Store
{
    public class DataTransLog : BasePO
    {
        private int logid = 0;
        private int storeid = 0;
        private DateTime bizdate = DateTime.Now;
        private int datatypeid = 0;
        private int datacount = 0;
        private int transtypeid = 0;
        private DateTime transstartdate = DateTime.Now;
        private DateTime transenddate = DateTime.Now;
        private string transflag = "";
        private string returnmsg = "";
        public override String GetTableName()
        {
            return "DataTransLog";
        }
        public override String GetColumnNames()
        {
            return "LogID,StoreID,BizDate,DataTypeID,DataCount,TransTypeID,TransStartDate,TransEndDate,TransFlag,ReturnMsg";
        }
        public override String GetUpdateColumnNames()
        {
            return "StoreID,BizDate,DataTypeID,DataCount,TransTypeID,TransStartDate,TransEndDate,TransFlag,ReturnMsg";
        }
        public override string GetInsertColumnNames()
        {
            return "StoreID,BizDate,DataTypeID,DataCount,TransTypeID,TransStartDate,TransEndDate,TransFlag,ReturnMsg";
        }
        public int LogID
        {
            get { return logid; }
            set { logid = value; }
        }
        public int StoreID
        {
            get { return storeid; }
            set { storeid = value; }
        }
        public DateTime BizDate
        {
            get { return bizdate; }
            set { bizdate = value; }
        }
        public int DataTypeID
        {
            get { return datatypeid; }
            set { datatypeid = value; }
        }
        public int DataCount
        {
            get { return datacount; }
            set { datacount = value; }
        }
        public int TransTypeID
        {
            get { return transtypeid; }
            set { transtypeid = value; }
        }
        public DateTime TransStartDate
        {
            get { return transstartdate; }
            set { transstartdate = value; }
        }
        public DateTime TransEndDate
        {
            get { return transenddate; }
            set { transenddate = value; }
        }
        public string TransFlag
        {
            get { return transflag; }
            set { transflag = value; }
        }
        public string ReturnMsg
        {
            get { return returnmsg; }
            set { returnmsg = value; }
        }
    }
}