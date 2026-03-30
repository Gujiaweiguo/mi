using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Lease.ConShop
{
    public class TpUsr:BasePO
    {
        private string tPUsrId = "";
        private string tPUsrNm = "";
        private string szpin = "";
        private string tPUsrStatus = "";
        private string gpId = "";
        private string sex = "";
        private DateTime dob = DateTime.Now;
        private string eduLevelNm = "";
        private string married = "";
        private string health = "";
        private string phone = "";
        private string addr1 = "";
        private string addr2 = "";
        private string addr3 = "";
        private string iDNo = "";
        private DateTime dateStart = DateTime.Now;
        private string jobTitleNm = "";
        private string newEmp = "";
        private string remarks = "";
        private string shopPhone = "";
        private string shopMgr = "";
        private string reason = "";
        private string deposit = "";
        private string unitId = "";
        private string custid = "";
        private string buildingid = "";
        private Nullable<DateTime> deleteTime;
        private DateTime entryAt = DateTime.Now;
        private string entryBy = "";
        private int tpUsrStatus = 0;

        /*有效*/
        public static int TPUSR_STATUS_YES = 1;
        /*无效*/
        public static int TPUSR_STATUS_NO = 0;

        public static int[] GetTpUsrStatus()
        {
            int[] tpUsrStatus = new int[2];
            tpUsrStatus[0] = TPUSR_STATUS_YES;
            tpUsrStatus[1] = TPUSR_STATUS_NO;
            return tpUsrStatus;
        }

        public static String GetTpUsrStatusDesc(int tpUsrStatus)
        {
            if (tpUsrStatus == TPUSR_STATUS_NO)
            {
                return "BizGrp_NO";
            }
            if (tpUsrStatus == TPUSR_STATUS_YES)
            {
                return "BizGrp_YES";
            }
            return "NULL";
        }

        public String TpUsrStatusDesc
        {
            get { return GetTpUsrStatusDesc(this.TpUsrStatus); }
        }


        public override string GetTableName()
        {
            return "TpUsr";
        }

        public override string GetColumnNames()
        {
            return "TPUsrId,TPUsrNm,szPin,TPUsrStatus,GpId,Sex,Dob,EduLevelNm,Married,Health,Phone,Addr1,Addr2,Addr3,IDNo,DateStart,JobTitleNm,NewEmp,Remarks,ShopPhone,ShopMgr,Reason,Deposit,UnitId,DeleteTime,BuildingID,CustID";
        }

        public override string GetInsertColumnNames()
        {
            //return "TPUsrId,TPUsrNm,szPin,TPUsrStatus,GpId,Sex,Dob,Phone,IDNo,DateStart,JobTitleNm,UnitId";
            return "TPUsrId,TPUsrNm,szPin,TPUsrStatus,GpId,Sex,Dob,EduLevelNm,Married,Health,Phone,Addr1,Addr2,Addr3,IDNo,DateStart,JobTitleNm,NewEmp,Remarks,ShopPhone,ShopMgr,Reason,Deposit,UnitId,EntryAt,EntryBy,BuildingID,CustID";
        }

        public override string GetUpdateColumnNames()
        {
            return "TPUsrNm,TPUsrStatus,szPin,GpId,Sex,Dob,EduLevelNm,Married,Health,Phone,Addr1,Addr2,Addr3,IDNo,DateStart,JobTitleNm,NewEmp,Remarks,ShopPhone,ShopMgr,Reason,Deposit,UnitId,DeleteTime,EntryAt,EntryBy,BuildingID,CustID";
        }

        public string TPUsrId
        {
            get { return tPUsrId; }
            set { tPUsrId = value; }
        }

        public string TPUsrNm
        {
            get { return tPUsrNm; }
            set { tPUsrNm = value; }
        }

        public string szPin
        {
            get { return szpin; }
            set { szpin = value; }
        }

        public string TPUsrStatus
        {
            get { return tPUsrStatus; }
            set { tPUsrStatus = value; }
        }

        public string GpId
        {
            get { return gpId; }
            set { gpId = value; }
        }

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public DateTime Dob
        {
            get { return dob; }
            set { dob = value; }
        }

        public string EduLevelNm
        {
            get { return eduLevelNm; }
            set { eduLevelNm = value; }
        }

        public string Married
        {
            get { return married; }
            set { married = value; }
        }

        public string Health
        {
            get { return health; }
            set { health = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Addr1
        {
            get { return addr1; }
            set { addr1 = value; }
        }

        public string Addr2
        {
            get { return addr2; }
            set { addr2 = value; }
        }

        public string Addr3
        {
            get { return addr3; }
            set { addr3 = value; }
        }

        public string IDNo
        {
            get { return iDNo; }
            set { iDNo = value; }
        }

        public DateTime DateStart
        {
            get { return dateStart; }
            set { dateStart = value; }
        }

        public string JobTitleNm
        {
            get { return jobTitleNm; }
            set { jobTitleNm = value; }
        }

        public string NewEmp
        {
            get { return newEmp; }
            set { newEmp = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        public string ShopPhone
        {
            get { return shopPhone; }
            set { shopPhone = value; }
        }

        public string ShopMgr
        {
            get { return shopMgr; }
            set { shopMgr = value; }
        }

        public string Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        public string Deposit
        {
            get { return deposit; }
            set { deposit = value; }
        }

        public string UnitId
        {
            get { return unitId; }
            set { unitId = value; }
        }

        public Nullable<DateTime> DeleteTime
        {
            get { return deleteTime; }
            set { deleteTime = value; }
        }

        public DateTime EntryAt
        {
            get { return entryAt; }
            set { entryAt = value; }
        }

        public string EntryBy
        {
            get { return entryBy; }
            set { entryBy = value; }
        }

        public int TpUsrStatus
        {
            get { return tpUsrStatus; }
            set { tpUsrStatus = value; }
        }
        public string BuildingID
        {
            get { return this.buildingid; }
            set { this.buildingid = value; }
        }
        public string CustID
        {
            get { return this.custid; }
            set { this.custid = value; }
        }
    }
}
