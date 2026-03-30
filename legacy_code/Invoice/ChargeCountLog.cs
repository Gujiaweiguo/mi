using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Invoice
{
    /// <summary>
    /// 费用生成日志
    /// </summary>
    public class ChargeCountLog:BasePO
    {
        private int chargeCountID = 0;  //费用计算日志ID
        private int contractID = 0;     //合同ID
        private int custID = 0;         //客户ID
        private int chargeTypeID = 0;   //费用类别ID
        private int productStatus = 0;  //生成标志
        private decimal invPayAmt = 0;  //生成结算金额
        private DateTime productDate = DateTime.Now;  //生成时间
        private DateTime chargePeriod = DateTime.Now;  //费用记帐月
        private string productNote = "";    //生成结果
        private string bancthID = "";       //批次号

        public static int PRODUCTSTATUS_YES = 0;  //生成成功
        public static int PRODUCTSTATUS_NO = 1;  //生成失败

        public int ChargeCountID
        {
            get { return chargeCountID; }
            set { chargeCountID = value; }
        }

        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        public int CustID
        {
            get { return custID; }
            set { custID = value; }
        }

        public int ChargeTypeID
        {
            get { return chargeTypeID; }
            set { chargeTypeID = value; }
        }

        public int ProductStatus
        {
            get { return productStatus; }
            set { productStatus = value; }
        }

        public decimal InvPayAmt
        {
            get { return invPayAmt; }
            set { invPayAmt = value; }
        }

        public DateTime ProductDate
        {
            get { return productDate; }
            set { productDate = value; }
        }

        public DateTime ChargePeriod
        {
            get { return chargePeriod; }
            set { chargePeriod = value; }
        }

        public string ProductNote
        {
            get { return productNote; }
            set { productNote = value; }
        }

        public string BancthID
        {
            get { return bancthID; }
            set { bancthID = value; }
        }

        public override string GetTableName()
        {
            return "ChargeCountLog";
        }

        public override string GetColumnNames()
        {
            return "ChargeCountID,ContractID,CustID,ChargeTypeID,ProductStatus,InvPayAmt,ProductDate,ChargePeriod,ProductNote,BancthID";
        }

        public override string GetInsertColumnNames()
        {
            return "ChargeCountID,ContractID,CustID,ChargeTypeID,ProductStatus,InvPayAmt,ProductDate,ChargePeriod,ProductNote,BancthID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }

    }
}
