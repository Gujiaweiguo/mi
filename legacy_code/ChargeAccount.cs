using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Base;
using Base.DB;
using Base.Biz;
using Lease;
using Lease.Contract;
using Lease.Formula;

namespace Invoice
{
    /// <summary>
    /// 费用计算
    /// </summary>

    public class ChargeAccount
    {
        public static decimal TotalMoney;     //总费用金额
        public static float monthSettleDays; //月结算天数
        public static int ifPrepay; //是否预收保底
       
        /// <summary>
        /// 费用计算
        /// </summary>
        /// <param name="ContractID">合同号</param>
        /// <param name="isFirst">是否首期; 0:否 ; 1 :是</param>
        public static void AccountCharge(int contractID, int isFirst)
        {
            DateTime accountStartDateTime; //结算时间段中的开始时间
            DateTime accountEndDateTime;   //结算时间段中的结束时间
            //DateTime accountTime;          //结算时间段

            Resultset rs = GetContract(contractID); //获取合同信息
            if (rs.Count == 1)
            {
                Contract contract = rs.Dequeue() as Contract;
                //判断合同是否有效
                if (contract.ContractStatus == Contract.CONTRACTSTATUS_TYPE_INGEAR)
                {
                    //合同有效,判断是否首期
                    if (isFirst == InvoiceHeader.ISFIRST_YES)
                    {
                        //取合同相关条款
                        int billCyle = 0;   //结算周期
                        int settleMode = 0; //结算处理方式
                        int ifPrepay = 0;   //是否预收保底
                        Resultset conLeaseRs = GetConLease(contractID);
                        if (conLeaseRs.Count == 1)
                        {
                            ConLease conLease = conLeaseRs.Dequeue() as ConLease;
                            billCyle = conLease.BillCycle;
                            settleMode = conLease.SettleMode;
                            ifPrepay = conLease.IfPrepay;
                            monthSettleDays = conLease.MonthSettleDays;
                        }

                        //是首期,判断是否从1号开始
                        if (contract.ConStartDate.Day == 1)
                        {
                            accountStartDateTime = contract.ConStartDate;
                            accountEndDateTime = accountStartDateTime.AddMonths(billCyle).AddDays(-1);
                            //结算时段是否在合同范围内
                            IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate,contract.ConEndDate);
                        }
                        else
                        {
                            if (settleMode == ConLease.SETTLEMODE_TYPE_FIRST) //首月对齐
                            {
                                accountStartDateTime = contract.ConStartDate;
                                accountEndDateTime = accountStartDateTime.AddMonths(billCyle).AddDays(-accountStartDateTime.Day);
                                //结算时段是否在合同范围内
                                IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate);
                            }
                            if (settleMode == ConLease.SETTLEMODE_TYPE_LAST) //次月对齐
                            {
                                accountStartDateTime = contract.ConStartDate;
                                accountEndDateTime = accountStartDateTime.AddMonths(billCyle).AddDays(-1);
                                //结算时段是否在合同范围内
                                IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate);
                            }
                            if (settleMode == ConLease.SETTLEMODE_TYPE_NO) //不做调整
                            {
                                accountStartDateTime = contract.ConStartDate;
                                accountEndDateTime = accountStartDateTime.AddMonths(billCyle).AddDays(-1);
                                //结算时段是否在合同范围内
                                IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate);
                            }
                            
                        }
                    }
                    else
                    {
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private static Resultset GetContract(int contractID)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "";
            baseBo.WhereClause = "ContractID = " + contractID;
            Resultset rs = baseBo.Query(new Contract());
            return rs;
        }

        /// <summary>
        /// 获取合同相关条款信息
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <returns></returns>
        private static Resultset GetConLease(int contractID)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ContractID = " + contractID;
            Resultset rs = baseBo.Query(new ConLease());
            return rs;
        }

        /// <summary>
        /// 获取结算时间段计算公式
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <returns></returns>
        private static Resultset GetFormulaH(int contractID, DateTime startTime, DateTime endTime)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ContractID = " + contractID + " and FStartDate >= '" + startTime + "' and FEndDate <= '" + endTime+"'";
            baseBo.OrderBy = "FStartDate";
            Resultset rs = baseBo.Query(new ConFormulaH());
            return rs;
        }

        /// <summary>
        /// 获取保底设定
        /// </summary>
        /// <param name="formulaHID">公式ID</param>
        /// <returns></returns>
        private static Resultset GetConFormulaM(int formulaHID)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "FormulaID = " + formulaHID + " and SalesTo = min(SalesTo) ";
            Resultset rs = baseBo.Query(new ConFormulaM());
            return rs;
        }

        /// <summary>
        /// 结算时段是否在合同范围内
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="accountStartTime">结算开始时间</param>
        /// <param name="accountEndTime">结算结束时间</param>
        /// <param name="contractStartTime">合同开始时间</param>
        /// <param name="contractEndTime">合同结束时间</param>
        /// <returns></returns>
        private static void IsDateTimeInContract(int contractID, DateTime accountStartTime, DateTime accountEndTime, DateTime contractStartTime, DateTime contractEndTime)
        {
            //判断时间段是否在合同范围内
            if ((accountStartTime >= contractStartTime) && (accountEndTime <= contractEndTime)) //时间段完全在合同范围内
            {
                //根据结算时间段获取结算公式
                GetFormulaHByDate(contractID, accountStartTime, accountEndTime);

            }
            else if ((accountStartTime >= contractStartTime) || (accountEndTime <= contractEndTime))  //时间段部分在合同范围内
            {
                //结算日期部分在合同范围内
                accountEndTime = contractEndTime;  //结算日期大于合同日期,取合同日期
                //根据结算时间段获取结算公式
                GetFormulaHByDate(contractID, accountStartTime, accountEndTime);
            }
            else  //时间段完全不在合同范围内
            {
                return;
            }
        }

        /// <summary>
        /// 根据结算时间段获取结算公式
        /// </summary>
        /// <param name="contractID"></param>
        /// <param name="stratDate"></param>
        /// <param name="endDate"></param>
        private static void GetFormulaHByDate(int contractID, DateTime stratDate, DateTime endDate)
        {
            BaseBO baseBo = new BaseBO();
            //取结算币种
            Resultset curyRs = GetConLease(contractID);
            ConLease conLse = curyRs.Dequeue() as ConLease;
            int curTypeID = conLse.CurTypeID;

            //取计算公式
            Resultset formulHRs = GetFormulaH(contractID, stratDate, endDate);
            ConFormulaH conFormulaH = formulHRs.Dequeue() as ConFormulaH;
            ArrayList aryList = new ArrayList();
            ConFormulaM conFormulaM;
            int invId = BaseApp.GetInvID();
            
            decimal salesToM = 0;
            //判断是否预收保底
            if (ifPrepay == ConLease.IFPREPAY_TYPE_YES)
            {
                Resultset conFormulaMRs;
                if (conFormulaH.MinSumOpt == ConFormulaH.PCENTOPT_TYPE_FAST) //保底为固定
                {
                    conFormulaMRs = GetConFormulaM(conFormulaH.FormulaID);
                    conFormulaM = conFormulaMRs.Dequeue() as ConFormulaM;
                    salesToM = conFormulaM.SalesTo * conFormulaM.MinSum; //保底额 = 保底销售额*保底率
                }
                else if ((conFormulaH.MinSumOpt == ConFormulaH.PCENTOPT_TYPE_S) || (conFormulaH.MinSumOpt == ConFormulaH.PCENTOPT_TYPE_M)) //保低为级别
                {
                    conFormulaMRs = GetConFormulaM(conFormulaH.FormulaID);
                    conFormulaM = conFormulaMRs.Dequeue() as ConFormulaM;
                    salesToM = conFormulaM.SalesTo * conFormulaM.MinSum; //保底额 = 保底销售额*保底率
                }
                
            }
            //多条计算公式循环
            int formulaHCount = formulHRs.Count;
            for (int i = 0; i < formulaHCount; i++)
            {
                int days = GetMonthDay(conFormulaH.FStartDate); //获取该月的天数
                if (conFormulaH.FStartDate.AddDays(days - 1) == conFormulaH.FEndDate) //整月
                {
                    decimal TotalFixedRental = conFormulaH.FixedRental * days;  //总租金额=租金额*天数
                    decimal baseAmt = conFormulaH.BaseAmt; //基本租金
                    
                    TotalMoney = TotalFixedRental + baseAmt + salesToM;   //总费用 = 总租金额 + 基本租金 + 保底额
                }
                else
                {
                    float settleDays;
                    if (monthSettleDays == 0)  //每月结算天数为自然月
                    {
                        settleDays = days;
                    }
                    else
                    {
                        settleDays = monthSettleDays;
                    }

                    TimeSpan ts = conFormulaH.FStartDate - conFormulaH.FEndDate;   
                    double conFormulaDays = ts.TotalDays;
                    settleDays = Convert.ToSingle(conFormulaDays) / settleDays;  //计算比重天数

                    decimal TotalFixedRental = conFormulaH.FixedRental * Convert.ToDecimal(settleDays);  //总租金额=租金额*比重天数
                    decimal baseAmt = conFormulaH.BaseAmt; //基本租金
                    TotalMoney = TotalFixedRental + baseAmt + salesToM;   //总费用 = 总租金额 + 基本租金 + 保底额
                }
                
                InvoiceDetail invoiceDet = new InvoiceDetail();   //结算明细
                invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                invoiceDet.ChargeTypeID = conFormulaH.ChargeTypeID; //费用类别ID
                invoiceDet.InvID = invId;  //结算单ID
                //invoiceDet.Period =       //费用记账月
                invoiceDet.InvStartDate = conFormulaH.FStartDate;  //费用开始日期
                invoiceDet.InvEndDate = conFormulaH.FEndDate;      //费用结束日期
                invoiceDet.InvCurTypeID = curTypeID;  //结算币种
                //invoiceDet.InvExRate =   //结算汇率

                aryList.Add(invoiceDet);
            }
            

            InvoiceHeader invoiceHer = new InvoiceHeader();   //结算主表
            invoiceHer.InvID = invId;

            BaseTrans baseTrans = new BaseTrans();
            baseTrans.BeginTrans();
            try
            {
                if (baseTrans.Insert(invoiceHer) == 1)
                {
                }
                int count = aryList.Count;
                for (int j = 0; j < count; j++)
                {
                    if (baseTrans.Insert((BasePO)aryList[j]) == 1)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                baseTrans.Rollback();
            }
            baseTrans.Commit();
        }

        //private static decimal GetExRate(int curId)
        //{
        //    BaseBO basebo = new BaseBO();
        //    basebo.WhereClause = "CurTypeID = " + curId;
        //    Resultset rs = basebo.Query(new
        //}

        /// <summary>
        /// 获取一个月中的天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private static int GetMonthDay(DateTime date)
        {
            int days;
            int y = date.Year;
            int m = date.Month;
            switch (m)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    days = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    days = 30;
                    break;
                case 2:
                    {
                        if ((y % 400 == 0) || ((y % 4 == 0) && (y % 100 != 0)))
                        {
                            days = 29;
                        }
                        else
                        {
                            days = 28;
                        }
                    }
                    break;
                default: days = 0;
                    break;
            }
            return days;
        }


    }
}
