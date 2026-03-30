using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.PotCustLicense
{
    public class PotCustLicenseInfo:BasePO
    {
        private int custLicenseID=0;
        private string custLicenseCode = "";
        private int custID=0;
        private string custLicenseName="";
        private int custLicenseType=0;
        private DateTime custLicenseStartDate=DateTime.Now;
        private DateTime custLicenseEndDate = DateTime.Now;
        private string note="";
        private string uploadLicense = "";

        public static int CUST_DEAL_IN_LICENCE = 1;//营业执照
        public static int CUST_SANITATION_APPROBATE = 2; //卫生许可证

        public static int CUST_OTHER = 3;  //其他


        public static int[] GetPotCustLicenseType()
        {
            int[] getPotCustLicenseType = new int[3];

            getPotCustLicenseType[0] = CUST_DEAL_IN_LICENCE;
            getPotCustLicenseType[1] = CUST_SANITATION_APPROBATE;
            getPotCustLicenseType[2] = CUST_OTHER;

            return getPotCustLicenseType;
        }

        public static String GetPotCustLicenseTypeDesc(int getPotCustLicenseType)
        {
            if (getPotCustLicenseType == CUST_DEAL_IN_LICENCE)
            {
                return "CUST_DEAL_IN_LICENCE";
            }
            if (getPotCustLicenseType == CUST_SANITATION_APPROBATE)
            {
                return "CUST_SANITATION_APPROBATE";
            }
            if (getPotCustLicenseType == CUST_OTHER)
            {
                return "CUST_OTHER";
            }
            return "NO";
        }
        public override string GetTableName()
        {
            return "PotCustLicense";
        }

        public override string GetColumnNames()
        {
            return "CustLicenseID,CustLicenseCode,CustLicenseName,CustLicenseType,CustLicenseStartDate,CustLicenseEndDate,CustID,UploadLicense";
        }

        public override string GetUpdateColumnNames()
        {
            return "CustLicenseCode,CustLicenseName,CustLicenseType,CustLicenseStartDate,CustLicenseEndDate"; ;
        }

        public int CustLicenseID
        {
            get { return custLicenseID; }
            set { custLicenseID = value; }
        }

        public string CustLicenseCode
        {
            get { return custLicenseCode; }
            set { custLicenseCode = value; }
        }
        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }
        public string CustLicenseName
        {
            get { return custLicenseName; }
            set { custLicenseName = value; }
        }
        public int CustLicenseType
        {
            get { return custLicenseType; }
            set { custLicenseType = value; }
        }

        public DateTime CustLicenseStartDate
        {
            get { return custLicenseStartDate; }
            set { custLicenseStartDate = value; }
        }
        public DateTime CustLicenseEndDate
        {
            get { return custLicenseEndDate; }
            set { custLicenseEndDate = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
        public string UploadLicense
        {
            get { return uploadLicense; }
            set { uploadLicense = value; }
        }
    }
}
