using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlow.Uiltil
{
    /**
     * 单据操作中，与工作流相关的信息
     */
    public class VoucherInfo
    {
        private int voucherID = 0;
        private string voucherHints = null;
        private string voucherMemo = null;
        private int deptID = 0;
        private int userID = 0;

        public VoucherInfo(int voucherID, string voucherHints, string voucherMemo, int deptID, int userID)
        {
            this.voucherID = voucherID;
            this.voucherHints = voucherHints;
            this.voucherMemo = voucherMemo;
            this.deptID = deptID;
            this.userID = userID;
        }

        public int VoucherID
        {
            get { return this.voucherID; }
            set { this.voucherID = value; }
        }
        public string VoucherHints
        {
            get { return this.voucherHints; }
            set { this.voucherHints = value; }
        }
        public string VoucherMemo
        {
            get { return this.voucherMemo; }
            set { this.voucherMemo = value; }
        }
        public int DeptID
        {
            get { return this.deptID; }
            set { this.deptID = value; }
        }
        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }
    }
}
