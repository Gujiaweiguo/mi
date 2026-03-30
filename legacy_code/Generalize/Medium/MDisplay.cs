using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    /*ÃÀ³Â*/
    public class MDisplay:BasePO
    {
        private int mDisplayID = 0;
        private string displayNm = "";
        private int anPID = 0;
        private int buildingID = 0;
        private int floorID = 0;
        private int areaID = 0;
        private int locationID = 0;
        private string locationDesc = "";
        private string intention = "";
        private decimal estCosts = 0;
        private decimal costs = 0;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private string mcompany = "";
        private string displayDesc = "";
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;


        public override string GetTableName()
        {
            return "MDisplay";
        }

        public override string GetColumnNames()
        {
            return "MDisplayID,DisplayNm,AnPID,BuildingID,FloorID,AreaID,LocationID,LocationDesc,Intention,EstCosts,StartDate,EndDate,DisplayDesc,Mcompany";
        }

        public override string GetInsertColumnNames()
        {
            return "MDisplayID,DisplayNm,AnPID,BuildingID,FloorID,AreaID,LocationID,LocationDesc,Intention,EstCosts,StartDate,EndDate,DisplayDesc,Mcompany," +
                    "CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "MDisplayID,DisplayNm,BuildingID,FloorID,AreaID,LocationID,LocationDesc,Intention,EstCosts,StartDate,EndDate,DisplayDesc,Mcompany," +
                    "ModifyUserID,ModifyTime";
        }

        public int MDisplayID
        {
            get { return mDisplayID; }
            set { mDisplayID = value; }
        }

        public string DisplayNm
        {
            get { return displayNm; }
            set { displayNm = value; }
        }

        public int AnPID
        {
            get { return anPID; }
            set { anPID = value; }
        }

        public int BuildingID
        {
            get { return buildingID; }
            set { buildingID = value; }
        }

        public int FloorID
        {
            get { return floorID; }
            set { floorID = value; }
        }

        public int AreaID
        {
            get { return areaID; }
            set { areaID = value; }
        }

        public int LocationID
        {
            get { return locationID; }
            set { locationID = value; }
        }

        public string LocationDesc
        {
            get { return locationDesc; }
            set { locationDesc = value; }
        }

        public string Intention
        {
            get { return intention; }
            set { intention = value; }
        }

        public string DisplayDesc
        {
            get { return displayDesc; }
            set { displayDesc = value; }
        }

        public decimal EstCosts
        {
            get { return estCosts; }
            set { estCosts = value; }
        }

        public decimal Costs
        {
            get { return costs; }
            set { costs = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public string Mcompany
        {
            get { return mcompany; }
            set { mcompany = value; }
        }

        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }

        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }

        public int OprRoleID
        {
            get { return oprRoleID; }
            set { oprRoleID = value; }
        }

        public int OprDeptID
        {
            get { return oprDeptID; }
            set { oprDeptID = value; }
        }
    }
}
