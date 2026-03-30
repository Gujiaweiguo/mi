using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    /*网络*/
    public class MInternet:BasePO
    {
        private int mInternetID = 0;
        private int internetID = 0;
        private int anPID = 0;
        private string internetNm = "";
        private int contentsID = 0;
        private decimal estCosts = 0;
        private decimal costs = 0;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private int freq = 0;
        private int freqDays = 0;
        private int freqMon = 0;
        private int freqTue = 0;
        private int freqWed = 0;
        private int freqThu = 0;
        private int freqFri = 0;
        private int freqSat = 0;
        private int freqSun = 0;
        private int betweenFr = 0;
        private int betweenTo = 0;
        private string printSizeNm = "";
        private string contentsNm = "";
        private int createUserID = 0;
        private DateTime createTime = DateTime.Now;
        private int modifyUserID = 0;
        private DateTime modifyTime = DateTime.Now;
        private int oprRoleID = 0;
        private int oprDeptID = 0;

        /*日*/
        public static int MPRINTS_FREQ_DAY = 1;

        /*星期*/
        public static int MPRINTS_FREQ_WEEK = 2;

        /*月*/
        public static int MPRINTS_FREQ_MONTH = 3;

        /*选择每周日期天数*/
        public static int MPRINTS_WEEK_STATUS_YES = 1;

        /*取消每周日期天数*/
        public static int MPRINTS_WEEK_STATUS_NO = 0;


        public override string GetTableName()
        {
            return "MInternet";
        }

        public override string GetColumnNames()
        {
            return "MInternetID,InternetID,AnPID,InternetNm,ContentsID,EstCosts,Costs,StartDate,EndDate,Freq,FreqDays,FreqMon,FreqTue,FreqWed,FreqThu,FreqFri,FreqSat,FreqSun," +
                    "BetweenFr,BetweenTo,ContentsNm";
        }

        public override string GetInsertColumnNames()
        {
            return "MInternetID,InternetID,AnPID,InternetNm,ContentsID,EstCosts,Costs,StartDate,EndDate,Freq,FreqDays,FreqMon,FreqTue,FreqWed,FreqThu,FreqFri,FreqSat,FreqSun," +
                    "BetweenFr,BetweenTo,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
        }

        public override string GetUpdateColumnNames()
        {
            return "InternetID,InternetNm,ContentsID,EstCosts,Costs,StartDate,EndDate,Freq,FreqDays,FreqMon,FreqTue,FreqWed,FreqThu,FreqFri,FreqSat,FreqSun," +
                    "BetweenFr,BetweenTo,ModifyUserID,ModifyTime";
        }

        public override string GetQuerySql()
        {
            return "Select MInternetID,InternetID,AnPID,InternetNm,a.ContentsID,EstCosts,Costs,StartDate,EndDate,Freq,FreqDays,FreqMon,FreqTue,FreqWed,FreqThu,FreqFri," +
                    "FreqSat,FreqSun,BetweenFr,BetweenTo,ContentsNm From MInternet a Left Join " +
                    "Contents b On a.ContentsID=b.ContentsID";
        }

        public int MInternetID
        {
            get { return mInternetID; }
            set { mInternetID = value; }
        }

        public int InternetID
        {
            get { return internetID; }
            set { internetID = value; }
        }

        public int AnPID
        {
            get { return anPID; }
            set { anPID = value; }
        }

        public string InternetNm
        {
            get { return internetNm; }
            set { internetNm = value; }
        }

        public int ContentsID
        {
            get { return contentsID; }
            set { contentsID = value; }
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

        public int Freq
        {
            get { return freq; }
            set { freq = value; }
        }

        public int FreqDays
        {
            get { return freqDays; }
            set { freqDays = value; }
        }

        public int FreqMon
        {
            get { return freqMon; }
            set { freqMon = value; }
        }

        public int FreqTue
        {
            get { return freqTue; }
            set { freqTue = value; }
        }

        public int FreqWed
        {
            get { return freqWed; }
            set { freqWed = value; }
        }

        public int FreqThu
        {
            get { return freqThu; }
            set { freqThu = value; }
        }

        public int FreqFri
        {
            get { return freqFri; }
            set { freqFri = value; }
        }

        public int FreqSat
        {
            get { return freqSat; }
            set { freqSat = value; }
        }

        public int FreqSun
        {
            get { return freqSun; }
            set { freqSun = value; }
        }

        public int BetweenFr
        {
            get { return betweenFr; }
            set { betweenFr = value; }
        }

        public int BetweenTo
        {
            get { return betweenTo; }
            set { betweenTo = value; }
        }

        public string PrintSizeNm
        {
            get { return printSizeNm; }
            set { printSizeNm = value; }
        }

        public string ContentsNm
        {
            get { return contentsNm; }
            set { contentsNm = value; }
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
