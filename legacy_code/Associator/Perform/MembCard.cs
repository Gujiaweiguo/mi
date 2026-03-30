using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Perform
{
    public class MembCard : BasePO
    {
        private string membCardId = "";
        private int membId = 0;
        private int cardClassId = 0;
        private DateTime dateIssued = DateTime.Now;
        private string cardTypeId = "";
        private string cardStatusId = "";
        private string cardOwner = "";
        private string prLCustId = "";
        private DateTime expDate = DateTime.Now;
        private string newMembCardID = "";
        private string changeReason = "";
        private int createUserID;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID;
        private DateTime modifyTime;
        private int oprRoleID;
        private int oprDeptID;

        public static char OPTN_ORMA_LCUST = 'N';
        public static char OPT_EMPLOYEE = 'E';
        public static char OPT_OTHERS = 'O';

        //public static int MEMBCARDSTATUS_NO = 0;     //未启用
        //public static int MEMBCARDSTATUS_VAILD = 1;  //有效
        //public static int MEMBCARDSTATUS_VAIN = 2;   //无效


        //public static int[] GetMembCardStatus()
        //{
        //    int[] membCardStatus = new int[3];
        //    membCardStatus[0] = MEMBCARDSTATUS_NO;
        //    membCardStatus[1] = MEMBCARDSTATUS_VAILD;
        //    membCardStatus[2] = MEMBCARDSTATUS_VAIN;
        //    return membCardStatus;
        //}

        //public static string GetMembCardStatusDesc(int membCardStatus)
        //{
        //    if (membCardStatus == MEMBCARDSTATUS_NO)
        //    {
        //        return "Memb_MembCardNoUse"; //未启用
        //    }
        //    if (membCardStatus == MEMBCARDSTATUS_VAILD)
        //    {
        //        return "Memb_MembCardYes";  //有效
        //    }
        //    if (membCardStatus == MEMBCARDSTATUS_VAIN)
        //    {
        //        return "Memb_MembCardNoUse"; //无效
        //    }
        //    return "Public_Sealed";
        //}


        public override string GetTableName()
        {
            return "MembCard";
        }

        public override string GetColumnNames()
        {
            return "MembCardId,MembId,CardClassId,DateIssued,CardTypeId,CardStatusId,CardOwner,PrLCustId,ExpDate,NewMembCardID,ChangeReason,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "MembCardId,MembId,CardClassId,DateIssued,CardTypeId,CardStatusId,CardOwner,PrLCustId,ExpDate,NewMembCardID,ChangeReason,CreateUserID,CreateTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "MembCardId,DateIssued,CardTypeId,CardStatusId,CardOwner,PrLCustId,ExpDate,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }


        public string MembCardId
        {
            get { return membCardId; }
            set { membCardId = value; }
        }

        public int MembId
        {
            get { return membId; }
            set { membId = value; }
        }

        public int CardClassId
        {
            get { return cardClassId; }
            set { cardClassId = value; }
        }

        public DateTime DateIssued
        {
            get { return dateIssued; }
            set { dateIssued = value; }
        }

        public string CardTypeId
        {
            get { return cardTypeId; }
            set { cardTypeId = value; }
        }

        public string CardStatusId
        {
            get { return cardStatusId; }
            set { cardStatusId = value; }
        }

        public string CardOwner
        {
            get { return cardOwner; }
            set { cardOwner = value; }
        }

        public string PrLCustId
        {
            get { return prLCustId; }
            set { prLCustId = value; }
        }

        public DateTime ExpDate
        {
            get { return expDate; }
            set { expDate = value; }
        }

        public string NewMembCardID
        {
            get { return newMembCardID; }
            set { newMembCardID = value; }
        }

        public string ChangeReason
        {
            get { return changeReason; }
            set { changeReason = value; }
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
