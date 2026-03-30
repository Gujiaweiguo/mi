using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    public class SelectAnPMaster:BasePO
    {
        private int anpID = 0;
        private string anPNm = "";
        private int themeID = 0;
        private string themeNm = "";
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
            return "AnpID,AnPNm,ThemeNm,StartDate,EndDate,TargetSales,TargetPeopletime";
        }

        public override string GetInsertColumnNames()
        {
            return "";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public override string GetQuerySql()
        {
            return "Select AnpID,AnPNm,ThemeNm,StartDate,EndDate,TargetSales,TargetPeopletime From AnPMaster a,Theme b";
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

        public string ThemeNm
        {
            get { return themeNm; }
            set { themeNm = value; }
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
