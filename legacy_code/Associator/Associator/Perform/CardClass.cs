using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 会员卡级别
    /// </summary>
    public class CardClass:BasePO
    {
        private int cardClassID = 0;    //级别ID
        private string cardClassNm = "";            //级别名称
        private decimal bonusPer = 0;           //积分值
        private decimal scoreFactor = 0;       //积分因数
        private decimal newCharge = 0;      //新卡费
        private decimal lostCharge = 0;    //丢卡更新费
        private decimal demageCharge = 0;   //坏卡更新费
        private string  invalidate = "";   //是否可失效
        private string downgrade = "";        //是否可降级
        private string upgrade = "";   //是否可升级
        private string basedOn = "";     //消费积分
        private string expire = "";        //消费金额
        private int expireYear = 0;        //次年限内失效
        private decimal invVal = 0;        //会员卡持有效所需达到的积分或金额
        private int invMth = 0;        //统计以上积分或金额的月份数
        private decimal invWarnVal = 0;        //低于此积分或金额将打印警告信
        private int invWarnMth = 0;        //发出警告信的月份数
        private decimal dnVal = 0;        //降级底线积分或金额
        private int dnMth = 0;        //统计以上金额和积分的月份数
        private string dnId = "";        //降级至该卡级别
        private decimal dnWarnVal = 0;        //低于此积分或金额将打印警告信
        private int dnWarnMth = 0;        //发出警告信的月份数
        private decimal upVal = 0;        //升级所要达到的积分和金额
        private int upMth = 0;        //统计以上金额和积分的月份数
        private string upId = "";        //升级至该卡级别
        private int createUserID;
	    private DateTime createTime = DateTime.Now;
	    private int modifyUserID;
	    private DateTime modifyTime;
	    private int oprRoleID;
	    private int oprDeptID;

        public static string STATUS_YES = "Y";
        public static string STATUS_NO = "N";

        public static string STATUS_BONUS_B = "B";
        public static string STATUS_BONUS_T = "T";

        public int CardClassID
        {
            get { return cardClassID; }
            set { cardClassID = value; }
        }

        public string CardClassNm
        {
            get { return cardClassNm; }
            set { cardClassNm = value; }
        }

        public decimal BonusPer
        {
            get { return bonusPer; }
            set { bonusPer = value; }
        }


        public decimal ScoreFactor
        {
            get { return scoreFactor; }
            set { scoreFactor = value; }
        }

        public decimal NewCharge
        {
            get { return newCharge; }
            set { newCharge = value; }
        }

        public decimal LostCharge
        {
            get { return lostCharge; }
            set { lostCharge = value; }
        }

        public decimal DemageCharge
        {
            get { return demageCharge; }
            set { demageCharge = value; }
        }

        public string Invalidate
        {
            get { return invalidate; }
            set { invalidate = value; }
        }

        public string DownGrade
        {
            get { return downgrade; }
            set { downgrade = value; }
        }

        public string Upgrade
        {
            get { return upgrade; }
            set { upgrade = value; }
        }

        public string BasedOn
        {
            get { return basedOn; }
            set { basedOn = value; }
        }

        public string Expire
        {
            get { return expire; }
            set { expire = value; }
        }

        public int ExpireYear
        {
            get { return expireYear; }
            set { expireYear = value; }
        }

        public decimal InvVal
        {
            get { return invVal; }
            set { invVal = value; }
        }

        public int InvMth
        {
            get { return invMth; }
            set { invMth = value; }
        }

        public decimal InvWarnVal
        {
            get { return invWarnVal; }
            set { invWarnVal = value; }
        }

        public int InvWarnMth
        {
            get { return invWarnMth; }
            set { invWarnMth = value; }
        }

        public decimal DnVal
        {
            get { return dnVal; }
            set { dnVal = value; }
        }

        public int DnMth
        {
            get { return dnMth; }
            set { dnMth = value; }
        }

        public string DnId
        {
            get { return dnId; }
            set { dnId = value; }
        }

        public decimal DnWarnVal
        {
            get { return dnWarnVal; }
            set { dnWarnVal = value; }
        }

        public int DnWarnMth
        {
            get { return dnWarnMth; }
            set { dnWarnMth = value; }
        }

        public decimal UpVal
        {
            get { return upVal; }
            set { upVal = value; }
        }

        public int UpMth
        {
            get { return upMth; }
            set { upMth = value; }
        }

        public string UpId
        {
            get { return upId; }
            set { upId = value; }
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

        public override string GetTableName()
        {
            return "CardClass";
        }

        public override string GetColumnNames()
        {
            return "CardClassID,CardClassNm,BonusPer,NewCharge,LostCharge,DemageCharge,Invalidate,DownGrade,Upgrade,BasedOn,Expire,ExpireYear,InvVal,InvMth,InvWarnVal,InvWarnMth,DnVal,DnMth,DnId,DnWarnVal,DnWarnMth,UpVal,UpMth,UpId,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetInsertColumnNames()
        {
            return "CardClassID,CardClassNm,BonusPer,NewCharge,LostCharge,DemageCharge,Invalidate,DownGrade,Upgrade,BasedOn,Expire,ExpireYear,InvVal,InvMth,InvWarnVal,InvWarnMth,DnVal,DnMth,DnId,DnWarnVal,DnWarnMth,UpVal,UpMth,UpId,CreateUserID,CreateTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "CardClassNm,BonusPer,NewCharge,LostCharge,DemageCharge,Invalidate,DownGrade,Upgrade,BasedOn,Expire,ExpireYear,InvVal,InvMth,InvWarnVal,InvWarnMth,DnVal,DnMth,DnId,DnWarnVal,DnWarnMth,UpVal,UpMth,UpId,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }
    }
}
