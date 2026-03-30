using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    public class LCust:BasePO
    {
        private int membId = 0;
        private string membCode = "";
        private string memberName = "";
        private string lOtherId = "";
        private string salutation = "";
        private DateTime dateJoint = DateTime.Now;
        private string addr1 = "";
        private string addr2 = "";
        private string addr3 = "";
        private string offPhone = "";
        private string homePhone = "";
        private string mobilPhone = "";
        private string email = "";
        private DateTime dob = DateTime.Now;
        private string natNm = "";
        private string raceNm = "";
        private string incomeId = "";
        private string bizNm = "";
        private string jobTitleNm = "";
        private string sexNm = "";
        private string mStatusNm = "";
        private string mAnnDate = "";
        private string eduLevelNm = "";
        private string distanceId = "";
        private string recreationNm1 = "";
        private string recreationNm2 = "";
        private string recreationNm3 = "";
        private string preferMerNm1 = "";
        private string preferMerNm2 = "";
        private string preferMerNm3 = "";
        private string preferGiftNm1 = "";
        private string preferGiftNm2 = "";
        private string preferGiftNm3 = "";
        private byte[] memberPic;
        private string myField1Id = "";
        private string myField2Id = "";
        private string remarks = "";
        private DateTime entryAt = DateTime.Now;
        private string entryBy = "";
        private string postalCode1 = "";
        private string postalCode2 = "";
        private string postalCode3 = "";
        private string comefromNM = "";
        private string custPassword = "";
        private int createUserID;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID;
        private int oprDeptID;
        private int ageID;


        public override string GetTableName()
        {
            return "Member";
        }
        
        public override string GetColumnNames()
        {
            return "MembId,LOtherId,Salutation,MembCode,MemberName,DateJoint,Addr1,Addr2,Addr3,OffPhone,HomePhone,MobilPhone,Email,Dob,NatNm,RaceNm,IncomeId,BizNm,JobTitleNm," +
                    "SexNm,MStatusNm,MAnnDate,EduLevelNm,DistanceId,RecreationNm1,RecreationNm2,RecreationNm3,PreferMerNm1,PreferMerNm2,PreferMerNm3," +
                    "PreferGiftNm1,PreferGiftNm2,PreferGiftNm3,MyField1Id,MyField2Id,Remarks,EntryAt,EntryBy,PostalCode1,PostalCode2,PostalCode3,ComefromNM," +
                    "CustPassword,MemberPic,AgeID";
        }

        public override string GetInsertColumnNames()
        {
            return "MembId,MembCode,LOtherId,Salutation,MemberName,DateJoint,Addr1,Addr2,Addr3,OffPhone,HomePhone,MobilPhone,Email,Dob,NatNm,RaceNm,IncomeId,BizNm,JobTitleNm," +
                    "SexNm,MStatusNm,MAnnDate,EduLevelNm,DistanceId,RecreationNm1,RecreationNm2,RecreationNm3,PreferMerNm1,PreferMerNm2,PreferMerNm3," +
                    "PreferGiftNm1,PreferGiftNm2,PreferGiftNm3,MyField1Id,MyField2Id,Remarks,EntryAt,EntryBy,PostalCode1,PostalCode2,PostalCode3,ComefromNM," +
                    "CustPassword,CreateUserID,CreateTime,OprRoleID,OprDeptID,AgeID";
        }

        public override string GetUpdateColumnNames()
        {
            return "LOtherId,Salutation,MemberName,DateJoint,Addr1,Addr2,Addr3,OffPhone,HomePhone,MobilPhone,Email,Dob,NatNm,RaceNm,IncomeId,BizNm,JobTitleNm," +
                    "SexNm,MStatusNm,MAnnDate,EduLevelNm,DistanceId,RecreationNm1,RecreationNm2,RecreationNm3,PreferMerNm1,PreferMerNm2,PreferMerNm3," +
                    "PreferGiftNm1,PreferGiftNm2,PreferGiftNm3,MyField1Id,MyField2Id,Remarks,EntryAt,EntryBy,PostalCode1,PostalCode2,PostalCode3,ComefromNM," +
                    "CustPassword,ModifyUserID,ModifyTime,AgeID";
        }


        public int MembId
        {
            get { return membId; }
            set { membId = value; }
        }

        public string MembCode
        {
            get { return membCode; }
            set { membCode = value; }
        }

        public string LOtherId
        {
            get { return lOtherId; }
            set { lOtherId = value; }
        }

        public string Salutation
        {
            get { return salutation; }
            set { salutation = value; }
        }

        public string MemberName
        {
            get { return memberName; }
            set { memberName = value; }
        }

        public DateTime DateJoint
        {
            get { return dateJoint; }
            set { dateJoint = value; }
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

        public string OffPhone
        {
            get { return offPhone; }
            set { offPhone = value; }
        }

        public string HomePhone
        {
            get { return homePhone; }
            set { homePhone = value; }
        }

        public string MobilPhone
        {
            get { return mobilPhone; }
            set { mobilPhone = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public DateTime Dob
        {
            get { return dob; }
            set { dob = value; }
        }

        public string NatNm
        {
            get { return natNm; }
            set { natNm = value; }
        }

        public string RaceNm
        {
            get { return raceNm; }
            set { raceNm = value; }
        }

        public string IncomeId
        {
            get { return incomeId; }
            set { incomeId = value; }
        }

        public string BizNm
        {
            get { return bizNm; }
            set { bizNm = value; }
        }

        public string JobTitleNm
        {
            get { return jobTitleNm; }
            set { jobTitleNm = value; }
        }

        public string SexNm
        {
            get { return sexNm; }
            set { sexNm = value; }
        }

        public string MStatusNm
        {
            get { return mStatusNm; }
            set { mStatusNm = value; }
        }

        public string MAnnDate
        {
            get { return mAnnDate; }
            set { mAnnDate = value; }
        }

        public string EduLevelNm
        {
            get { return eduLevelNm; }
            set { eduLevelNm = value; }
        }

        public string DistanceId
        {
            get { return distanceId; }
            set { distanceId = value; }
        }

        public string RecreationNm1
        {
            get { return recreationNm1; }
            set { recreationNm1 = value; }
        }

        public string RecreationNm2
        {
            get { return recreationNm2; }
            set { recreationNm2 = value; }
        }

        public string RecreationNm3
        {
            get { return recreationNm3; }
            set { recreationNm3 = value; }
        }

        public string PreferMerNm1
        {
            get { return preferMerNm1; }
            set { preferMerNm1 = value; }
        }

        public string PreferMerNm2
        {
            get { return preferMerNm2; }
            set { preferMerNm2 = value; }
        }

        public string PreferMerNm3
        {
            get { return preferMerNm3; }
            set { preferMerNm3 = value; }
        }

        public string PreferGiftNm1
        {
            get { return preferGiftNm1; }
            set { preferGiftNm1 = value; }
        }

        public string PreferGiftNm2
        {
            get { return preferGiftNm2; }
            set { preferGiftNm2 = value; }
        }

        public string PreferGiftNm3
        {
            get { return preferGiftNm3; }
            set { preferGiftNm3 = value; }
        }

        public string MyField1Id
        {
            get { return myField1Id; }
            set { myField1Id = value; }
        }

        public string MyField2Id
        {
            get { return myField2Id; }
            set { myField2Id = value; }
        }

        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
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

        public string PostalCode1
        {
            get { return postalCode1; }
            set { postalCode1 = value; }
        }

        public string PostalCode2
        {
            get { return postalCode2; }
            set { postalCode2 = value; }
        }

        public string PostalCode3
        {
            get { return postalCode3; }
            set { postalCode3 = value; }
        }

        public string ComefromNM
        {
            get { return comefromNM; }
            set { comefromNM = value; }
        }

        public string CustPassword
        {
            get { return custPassword; }
            set { custPassword = value; }
        }

        public byte[] MemberPic
        {
            get { return memberPic; }
            set { memberPic = value; }
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

        public int AgeID
        {
            get { return ageID; }
            set { ageID = value; }
        }
    }
}
