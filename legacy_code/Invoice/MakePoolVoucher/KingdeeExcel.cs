using System;
using System.Collections.Generic;
using System.Text;

namespace Invoice.MakePoolVoucher
{
    public class KingdeeExcel
    {
        private string fDate = "";
        private string fYear = "";
        private string fPeriod = "";
        private string fGroupID = "";
        private int fNumber = 0;
        private string fAccountNum = "";
        private string fAccountName = "";
        private string fCurrencyNum = "";
        private string fCurrencyName = "";
        private decimal fAmountFor = 0;
        private decimal fDebit = 0;
        private decimal fCredit = 0;
        private string fPreparerID = "'NONE";
        private string fCheckerID = "'NONE";
        private string fApproveID = "'NONE";
        private string fCashierID = "'NONE";
        private string fHandler = "";
        private string fSettleTypeID = "'*";
        private string fSettleNo = "";
        private string fExplanation = "";
        private string fQuantity = "'0";
        private string fMeasureUnitID = "'*";
        private string fUnitPrice = "'0";
        private string fReference = "";
        private string fTransDate = DateTime.Now.ToString("yyyy/MM/dd");
        private string fTransNo = "";
        private string fAttachments = "0";
        private string fSerialNum = "1";
        private string fObjectName = "";
        private string fParameter = "";
        private string fExchangeRate = "1";
        private int fEntryID = 0;
        private string fItem = "";
        private decimal fPosted = 0;
        private string fInternalInd = "";
        private string fCashFlow = "";


        /// <summary>
        /// 凭证日期
        /// </summary>
        public string FDate
        {
            get { return fDate; }
            set { fDate = value; }
        }

        /// <summary>
        /// 会计年度
        /// </summary>
        public string FYear
        {
            get { return fYear; }
            set { fYear = value; }
        }

        /// <summary>
        /// 会计期间
        /// </summary>
        public string FPeriod
        {
            get { return fPeriod; }
            set { fPeriod = value; }
        }

        /// <summary>
        /// 科目组-
        /// </summary>
        public string FGroupID
        {
            get { return fGroupID; }
            set { fGroupID = value; }
        }

        /// <summary>
        /// 凭证数量
        /// </summary>
        public int FNumber
        {
            get { return fNumber; }
            set { fNumber = value; }
        }

        /// <summary>
        /// 科目号
        /// </summary>
        public string FAccountNum
        {
            get { return fAccountNum; }
            set { fAccountNum = value; }
        }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string FAccountName
        {
            get { return fAccountName; }
            set { fAccountName = value; }
        }

        /// <summary>
        /// 币别代码
        /// </summary>
        public string FCurrencyNum
        {
            get { return fCurrencyNum; }
            set { fCurrencyNum = value; }
        }

        /// <summary>
        /// 币别名称
        /// </summary>
        public string FCurrencyName
        {
            get { return fCurrencyName; }
            set { fCurrencyName = value; }
        }

        /// <summary>
        /// 原币金额
        /// </summary>
        public decimal FAmountFor
        {
            get { return fAmountFor; }
            set { fAmountFor = value; }
        }

        /// <summary>
        /// 借方
        /// </summary>
        public decimal FDebit
        {
            get { return fDebit; }
            set { fDebit = value; }
        }

        /// <summary>
        /// 贷方
        /// </summary>
        public decimal FCredit
        {
            get { return fCredit; }
            set { fCredit = value; }
        }

        /// <summary>
        /// 制单
        /// </summary>
        public string FPreparerID
        {
            get { return fPreparerID; }
            set { fPreparerID = value; }
        }

        /// <summary>
        /// 审核
        /// </summary>
        public string FCheckerID
        {
            get { return fCheckerID; }
            set { fCheckerID = value; }
        }

        /// <summary>
        /// 核准
        /// </summary>
        public string FApproveID
        {
            get { return fApproveID; }
            set { fApproveID = value; }
        }

        /// <summary>
        /// 出纳
        /// </summary>
        public string FCashierID
        {
            get { return fCashierID; }
            set { fCashierID = value; }
        }

        /// <summary>
        /// 经办
        /// </summary>
        public string FHandler
        {
            get { return fHandler; }
            set { fHandler = value; }
        }

        /// <summary>
        /// 结算方式
        /// </summary>
        public string FSettleTypeID
        {
            get { return fSettleTypeID; }
            set { fSettleTypeID = value; }
        }

        /// <summary>
        /// 结算号
        /// </summary>
        public string FSettleNo
        {
            get { return fSettleNo; }
            set { fSettleNo = value; }
        }

        /// <summary>
        /// 凭证摘要
        /// </summary>
        public string FExplanation
        {
            get { return fExplanation; }
            set { fExplanation = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public string FQuantity
        {
            get { return fQuantity; }
            set { fQuantity = value; }
        }

        /// <summary>
        /// 数量单位
        /// </summary>
        public string FMeasureUnitID
        {
            get { return fMeasureUnitID; }
            set { fMeasureUnitID = value; }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public string FUnitPrice
        {
            get { return fUnitPrice; }
            set { fUnitPrice = value; }
        }

        /// <summary>
        /// 参考信息
        /// </summary>
        public string FReference
        {
            get { return fReference; }
            set { fReference = value; }
        }

        /// <summary>
        /// 业务日期
        /// </summary>
        public string FTransDate
        {
            get { return fTransDate; }
            set { fTransDate = value; }
        }

        /// <summary>
        /// 往来编号
        /// </summary>
        public string FTransNo
        {
            get { return fTransNo; }
            set { fTransNo = value; }
        }

        /// <summary>
        /// 附件数
        /// </summary>
        public string FAttachments
        {
            get { return fAttachments; }
            set { fAttachments = value; }
        }

        /// <summary>
        /// 序号
        /// </summary>
        public string FSerialNum
        {
            get { return fSerialNum; }
            set { fSerialNum = value; }
        }

        /// <summary>
        /// 系统模块
        /// </summary>
        public string FObjectName
        {
            get { return fObjectName; }
            set { fObjectName = value; }
        }

        /// <summary>
        /// 业务描述
        /// </summary>
        public string FParameter
        {
            get { return fParameter; }
            set { fParameter = value; }
        }

        /// <summary>
        /// 汇率
        /// </summary>
        public string FExchangeRate
        {
            get { return fExchangeRate; }
            set { fExchangeRate = value; }
        }

        /// <summary>
        /// 分录序号
        /// </summary>
        public int FEntryID
        {
            get { return fEntryID; }
            set { fEntryID = value; }
        }

        /// <summary>
        /// 合算项目
        /// </summary>
        public string FItem
        {
            get { return fItem; }
            set { fItem = value; }
        }

        /// <summary>
        /// 过账
        /// </summary>
        public decimal FPosted
        {
            get { return fPosted; }
            set { fPosted = value; }
        }

        /// <summary>
        /// 机制凭证
        /// </summary>
        public string FInternalInd
        {
            get { return fInternalInd; }
            set { fInternalInd = value; }
        }

        /// <summary>
        /// 现金流量
        /// </summary>
        public string FCashFlow
        {
            get { return fCashFlow; }
            set { fCashFlow = value; }
        }

    }
}
