using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.PotCustLicense
{
    public class PotCustContactInfo:BasePO
    {
        private int custContactID=0;
        private int custID=0;
        private string contactorName="";
        private string title="";
        private string officeTel="";
        private string mobileTel="";
        private string eMail="";
        private string officeAddr="";
        private string note="";

        public override string GetTableName()
        {
            return "PotCustContact";
        }

        public override string GetColumnNames()
        {
            return "CustContactID,ContactorName,Title,OfficeTel,MobileTel,EMail,CustID";
        }

        public override string GetUpdateColumnNames()
        {
            return "ContactorName,Title,OfficeTel,MobileTel,EMail";
        }

        public int CustContactID
        {
            get { return custContactID; }
            set { custContactID = value; }
        }
        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        public string ContactorName
        {
            get { return contactorName; }
            set { contactorName = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string OfficeTel
        {
            get { return officeTel; }
            set { officeTel = value; }
        }
        public string MobileTel
        {
            get { return mobileTel; }
            set { mobileTel = value; }
        }
        public string EMail
        {
            get { return eMail; }
            set { eMail = value; }
        }
        public string OfficeAddr
        {
            get { return officeAddr; }
            set { officeAddr = value; }
        }
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
