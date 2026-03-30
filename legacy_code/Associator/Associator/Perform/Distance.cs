using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Associator.Perform
{
    /*æ‡¿Î–≈œ¢*/
    public class Distance:BasePO
    {
        private string distanceId = "";
        private int distanceFr = 0;
        private int distanceTo = 0;
        private string entryBy = "";
        private DateTime entryAt = DateTime.Now;

        public override string GetTableName()
        {
            return "Distance";
        }

        public override string GetColumnNames()
        {
            return "DistanceId,DistanceFr,DistanceTo,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "DistanceId,DistanceFr,DistanceTo,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "DistanceId,DistanceFr,DistanceTo,EntryAt,EntryBy";
        }

        public string DistanceId
        {
            get { return distanceId; }
            set { distanceId = value; }
        }

        public int DistanceFr
        {
            get { return distanceFr; }
            set { distanceFr = value; }
        }

        public int DistanceTo
        {
            get { return distanceTo; }
            set { distanceTo = value; }
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
    }
}
