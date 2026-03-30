using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Sell
{
    public class MediaM : BasePO 
    {
        private int mediaMNo = 0;
        private string mediaMDesc = "";
        private int mediaNo = 0;
        private int cMediaNo = 0;
        private int cMediaMNo = 0;
        private int rMediaNo = 0;
        private int rMediaMNo = 0;
        private int changeAuto = 0;
        private string cardType = "";
        private Decimal comTenant = 0;
        private Decimal comMall = 0;
        private DateTime deleteTime = Convert.ToDateTime("1900-1-1");
        private DateTime entryAt = DateTime.Now;
        private string entryBy = "";
        private string mediaMEDesc = "";

        public int MediaMNo
        {
            get { return mediaMNo; }
            set { mediaMNo = value; }
        }
        public string MediaMDesc
        {
            get { return mediaMDesc; }
            set { mediaMDesc = value; }
        }
        public int MediaNo
        {
            get { return mediaNo; }
            set { mediaNo = value; }
        }
        public int CMediaNo
        {
            get { return cMediaNo; }
            set { cMediaNo = value; }
        }
        public int CMediaMNo
        {
            get { return cMediaMNo; }
            set { cMediaMNo = value; }
        }
        public int RMediaNo
        {
            get { return rMediaNo; }
            set { rMediaNo = value; }
        }
        public int RMediaMNo
        {
            get { return rMediaMNo; }
            set { rMediaMNo = value; }
        }
        public int ChangeAuto
        {
            get { return changeAuto; }
            set { changeAuto = value; }
        }
        public string CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }
        public Decimal ComTenant
        {
            get { return comTenant; }
            set { comTenant = value; }
        }
        public Decimal ComMall
        {
            get { return comMall; }
            set { comMall = value; }
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
        public string MediaMEDesc
        {
            get { return mediaMEDesc; }
            set { mediaMEDesc = value; }
        }


        public override string GetTableName()
        {
            return "MediaM";
        }

        public override string GetColumnNames()
        {
            return "MediaMNo,MediaMDesc,MediaNo,CMediaNo,CMediaMNo,RMediaNo,RMediaMNo,ChangeAuto,CardType,ComTenant,ComMall,DeleteTime,EntryAt,EntryBy,MediaMEDesc";
        }

        public override string GetUpdateColumnNames()
        {
            return "MediaMDesc,MediaNo,CMediaNo,CMediaMNo,RMediaNo,RMediaMNo,ChangeAuto,CardType,ComTenant,ComMall,DeleteTime,EntryAt,EntryBy,MediaMEDesc";
        }

        public override string GetInsertColumnNames()
        {
            return "MediaMNo,MediaMDesc,MediaNo,CMediaNo,CMediaMNo,RMediaNo,RMediaMNo,ChangeAuto,CardType,ComTenant,ComMall,DeleteTime,EntryAt,EntryBy,MediaMEDesc";
        }
    }
}
