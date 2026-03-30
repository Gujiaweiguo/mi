using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Perform
{
    public class BonusF : BasePO
    {
        private int bonusFID;
        private string bonusFDesc = "";
        private int dIndex;
        private decimal adjFactor;
        private string adjShop;
        private string adjCard;
        private string proTime;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private string frequency;
        private string mon;
        private string tue;
        private string wed;
        private string thu;
        private string fri;
        private string sat;
        private string sun;
        private DateTime startTime = DateTime.Now;
        private DateTime endTime = DateTime.Now;
        private decimal proFactor = 1;
        private DateTime lastRun = DateTime.Now;
        private DateTime entryAt;
        private string entryBy;
        private DateTime deleteTime = DateTime.Now;



        public override string GetTableName()
        {
            return "BonusF";
        }

        public override string GetColumnNames()
        {
            return "BonusFID,BonusFDesc,DIndex,AdjFactor,AdjShop,AdjCard,ProTime,StartDate,EndDate,Frequency,Mon,Tue,Wed,Thu,Fri,Sat,Sun,StartTime,EndTime,ProFactor,LastRun,EntryAt,EntryBy,DeleteTime";
        }

        public override string GetInsertColumnNames()
        {
            return "BonusFID,BonusFDesc,DIndex,AdjFactor,AdjShop,AdjCard,ProTime,StartDate,EndDate,Frequency,Mon,Tue,Wed,Thu,Fri,Sat,Sun,StartTime,EndTime,ProFactor,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "BonusFDesc,DIndex,AdjFactor,AdjShop,AdjCard,ProTime,StartDate,EndDate,Frequency,Mon,Tue,Wed,Thu,Fri,Sat,Sun,StartTime,EndTime,ProFactor,EntryAt,EntryBy";
        }

        public int BonusFID
        {
            get { return bonusFID; }
            set { bonusFID = value; }
        }

        public string BonusFDesc
        {
            get { return bonusFDesc; }
            set { bonusFDesc = value; }
        }

        public int DIndex
        {
            get { return dIndex; }
            set { dIndex = value; }
        }

        public decimal AdjFactor
        {
            get { return adjFactor; }
            set { adjFactor = value; }
        }

        public string AdjShop
        {
            get { return adjShop; }
            set { adjShop = value; }
        }

        public string AdjCard
        {
            get { return adjCard; }
            set { adjCard = value; }
        }

        public string ProTime
        {
            get { return proTime; }
            set { proTime = value; }
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

        public string Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        public string Mon
        {
            get { return mon; }
            set { mon = value; }
        }

        public string Tue
        {
            get { return tue; }
            set { tue = value; }
        }

        public string Wed
        {
            get { return wed; }
            set { wed = value; }
        }

        public string Thu
        {
            get { return thu; }
            set { thu = value; }
        }

        public string Fri
        {
            get { return fri; }
            set { fri = value; }
        }

        public string Sat
        {
            get { return sat; }
            set { sat = value; }
        }

        public string Sun
        {
            get { return sun; }
            set { sun = value; }
        }

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        public decimal ProFactor
        {
            get { return proFactor; }
            set { proFactor = value; }
        }

        public DateTime LastRun
        {
            get { return lastRun; }
            set { lastRun = value; }
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

        public DateTime DeleteTime
        {
            get { return deleteTime; }
            set { deleteTime = value; }
        }
    }
}
