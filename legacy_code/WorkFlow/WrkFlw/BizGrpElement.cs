using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlow.WrkFlw
{
    public class BizGrpElement
    {
        private int bizGrpID = 0;
        private string bizGrpCode = "";
        private string bizGrpName = "";
        private int bizGrpStatus = 0;
        private string note = "";
        private string bizGrpStatusName = "";


        public int BizGrpID
        {
            get { return this.bizGrpID; }
            set { this.bizGrpID = value; }
        }

        public string BizGrpCode
        {
            get { return this.bizGrpCode; }
            set { this.bizGrpCode = value; }
        }

        public string BizGrpName
        {
            get { return this.bizGrpName; }
            set { this.bizGrpName = value; }
        }

        public int BizGrpStatus
        {
            get { return this.bizGrpStatus; }
            set { this.bizGrpStatus = value; }
        }

        public string Note
        {
            get { return this.note; }
            set { this.note = value; }
        }

        public string BizGrpStatusName
        {
            get { return this.bizGrpStatusName; }
            set { this.bizGrpStatusName = value; }
        }
    }
}
