using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Sell
{
    public class Media : BasePO 
    {
        private int mediaNo = 0;
        private string mediaDesc = "";
        private int overpaid = 0;
        private int payType = 0;
        private DateTime deleteTime = Convert.ToDateTime("1900-1-1");
        private DateTime entryAt = DateTime.Now;
        private string entryBy = "";

        public int MediaNo
        {
            get { return mediaNo; }
            set { mediaNo = value; }
        }

        public string MediaDesc
        {
            get { return mediaDesc; }
            set { mediaDesc = value; }
        }

        public int Overpaid
        {
            get { return overpaid; }
            set { overpaid = value; }
        }

        public int PayType
        {
            get { return payType; }
            set { payType = value; }
        }

        public DateTime DeleteTime
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


        public override string GetTableName()
        {
            return "Media";
        }

        public override string GetColumnNames()
        {
            return "MediaNo,MediaDesc,Overpaid,PayType,DeleteTime,EntryAt,EntryBy";
        }

        public override string GetUpdateColumnNames()
        {
            return "MediaDesc,Overpaid,PayType,DeleteTime,EntryAt,EntryBy";
        }

        public override string GetInsertColumnNames()
        {
            return "MediaNo,MediaDesc,Overpaid,PayType,DeleteTime,EntryAt,EntryBy";
        }
    }
}
