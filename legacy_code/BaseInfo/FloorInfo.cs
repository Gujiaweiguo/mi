using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Store
{
    public class FloorInfo : BasePO
    {
        private int floorid = 0;
        private DateTime completedate = DateTime.Now;//竣工时间
        private string configurationtype = "";//结构类型
        private string floorthing = "";//楼层情况
        private decimal area = 0;//建筑面积
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;
        public override String GetTableName()
        {
            return "FloorInfo";
        }
        public override String GetColumnNames()
        {
            return "FloorId,CompleteDate,ConfigurationType,FloorThing,Area,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "CompleteDate,ConfigurationType,FloorThing,Area,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public int FloorId
        {
            get { return floorid; }
            set { floorid = value; }
        }
        public DateTime CompleteDate
        {
            get { return completedate; }
            set { completedate = value; }
        }
        public string ConfigurationType
        {
            get { return configurationtype; }
            set { configurationtype = value; }
        }
        public string FloorThing
        {
            get { return floorthing; }
            set { floorthing = value; }
        }
        public decimal Area
        {
            get { return area; }
            set { area = value; }
        }
        public int CreateUserId
        {
            get { return createuserid; }
            set { createuserid = value; }
        }
        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }
        public int ModifyUserId
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