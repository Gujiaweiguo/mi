using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Invoice.InvoiceH
{
    public class CurrencyType:BasePO
    {
        private int curTypeID = 0;
        private string curTypeName = "";
        private int curTypeStatus = 0;
        private int isLocal = 0;
        private string note = "";

        public static int CURTYPESTATUS_INVALID = 0;
        public static int CURTYPESTATUS_VALID = 1;
        public override string GetTableName()
        {
            return "CurrencyType";
        }

        public override string GetColumnNames()
        {
            return "CurTypeID,CurTypeName,CurTypeStatus,IsLocal,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "CurTypeID,CurTypeName,CurTypeStatus,IsLocal,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "CurTypeName,CurTypeStatus,IsLocal,Note";
        }

        public int CurTypeID
        {
            get { return curTypeID; }
            set { curTypeID = value; }
        }

        public string CurTypeName
        {
            get { return curTypeName; }
            set { curTypeName = value; }
        }

        public int CurTypeStatus
        {
            get { return curTypeStatus; }
            set { curTypeStatus = value; }
        }

        public int IsLocal
        {
            get { return isLocal; }
            set { isLocal = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
