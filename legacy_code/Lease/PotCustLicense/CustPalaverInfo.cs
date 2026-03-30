using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.PotCustLicense
{
    
    public class CustPalaverInfo:BasePO
    {
        private int custID = 0;
        private int palaverID = 0;
        private DateTime palaverTime = DateTime.Now.Date ;
      //  private string palaverUser = "";
        private string palaverAim = "";
       // private string palavernode = "";
        private string palaverName = "";
        private string palaverContent = "";
        private int processTypeId = 0;
        private string contactorName = "";
        private int palaverRound = 0;
        private string palaverPlace = "";
        private string palaverResult = "";
        private string unSolved = "";
        private string node = "";
        private int createUserId = 0;//创建用户内码
        private DateTime createTime=DateTime.Now;//创建时间
        private int modifyUserId = 0;//修改用户内码
        private DateTime modifyTime=DateTime.Now;//修改时间
        private int oprRoleID = 0;//创建修改用户角色内码
        private int oprDeptID = 0;//创建修改用户部门内码
        private int sequence = 0;//WrkFlwEntity 中的排序号,用来判断属于哪个商户
        private int palaverSort = 0;
        private int palaverStatus = 0;

        public override String GetTableName()
        {
            return "CustPalaver";
        }

        public override String GetColumnNames()
        {
            return "PalaverID,CustID,PalaverTime,PalaverName,PalaverAim,PalaverContent,ProcessTypeId,ContactorName,PalaverRound,PalaverPlace,PalaverResult,UnSolved,Node,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public override String GetInsertColumnNames()
        {
            return "PalaverID,CustID,PalaverTime,PalaverName,PalaverAim,PalaverContent,ProcessTypeId,ContactorName,PalaverRound,PalaverPlace,PalaverResult,UnSolved,Node,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID,Sequence,PalaverSort,PalaverStatus";
        }

        public override String GetUpdateColumnNames()
        {
            return "PalaverTime,PalaverName,PalaverAim,PalaverContent,ProcessTypeId,ContactorName,PalaverRound,PalaverPlace,PalaverResult,UnSolved,Node,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }


        public int PalaverID
        {
            get { return palaverID; }
            set { palaverID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        public DateTime PalaverTime
        {
            get { return palaverTime; }
            set { palaverTime = value; }
        }

        //public string PalaverUser
        //{
        //    get { return palaverUser; }
        //    set { palaverUser = value; }
        //}

        public string PalaverAim
        {
            get { return palaverAim; }
            set { palaverAim = value; }
        }

        //public string PalaverNode
        //{
        //    get { return palavernode; }
        //    set { palavernode = value; }
        //}
        public string PalaverName
        {
            get { return palaverName; }
            set { palaverName = value; }
        }
        public string PalaverContent
        {
            get { return palaverContent; }
            set { palaverContent = value; }
        }
        public int ProcessTypeId
        {
            set { processTypeId = value; }
            get { return processTypeId; }
        }
        public string ContactorName
        {
            set { contactorName = value; }
            get { return contactorName; }
        }
        public int PalaverRound
        {
            set { palaverRound = value; }
            get { return palaverRound; }
        }
        public string PalaverPlace
        {
            set { palaverPlace = value; }
            get { return palaverPlace; }
        }
        public string PalaverResult
        {
            set { palaverResult = value; }
            get { return palaverResult; }
        }
        public string UnSolved
        {
            set { unSolved = value; }
            get { return unSolved; }
        }
        public string Node
        {
            set { node = value; }
            get { return node; }
        }
        public int CreateUserId
        {
            set { createUserId = value; }
            get { return createUserId; }
        }
        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
        public int ModifyUserId
        {
            set { modifyUserId = value; }
            get { return modifyUserId; }
        }
        public DateTime ModifyTime
        {
            set { modifyTime = value; }
            get { return modifyTime; }
        }
        public int OprRoleID
        {
            set { oprRoleID = value; }
            get { return oprRoleID; }
        }
        public int OprDeptID
        {
            set { oprDeptID = value; }
            get { return oprDeptID; }
        }
        public int Sequence
        {
            set { sequence = value; }
            get { return sequence; }
        }
        public int PalaverSort
        {
            set { palaverSort = value; }
            get { return palaverSort; }
        }
        public int PalaverStatus
        {
            set { palaverStatus = value; }
            get { return palaverStatus; }
        }
    }
}
