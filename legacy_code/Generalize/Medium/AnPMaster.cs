using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    /*推广活动信息*/
    public class AnPMaster:BasePO
    {
        private int anpID = 0;
        private string anPNm = "";
        private int themeID = 0;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private decimal targetSales = 0;
        private decimal targetPeopletime = 0;
        private string remark = "";

        public override string GetTableName()
        {
            return "AnPMaster";
        }

        public override string GetColumnNames()
        {
            return "AnpID,AnPNm,ThemeID,StartDate,EndDate,TargetSales,TargetPeopletime,Remark";
        }

        public override string GetInsertColumnNames()
        {
            return "AnpID,AnPNm,ThemeID,StartDate,EndDate,TargetSales,TargetPeopletime,Remark";
        }

        public override string GetUpdateColumnNames()
        {
            return "AnPNm,ThemeID,StartDate,EndDate,TargetSales,TargetPeopletime,Remark";
        }

        public int AnpID
        {
            get { return anpID; }
            set { anpID = value; }
        }

        public string AnPNm
        {
            get { return anPNm; }
            set { anPNm = value; }
        }

        public int ThemeID
        {
            get { return themeID; }
            set { themeID = value; }
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

        public decimal TargetSales
        {
            get { return targetSales; }
            set { targetSales = value; }
        }

        public decimal TargetPeopletime
        {
            get { return targetPeopletime; }
            set { targetPeopletime = value; }
        }

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

    }
}
