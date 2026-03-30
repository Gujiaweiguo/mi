using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

using Base;
using Base.DB;
using Base.Biz;
using Lease;
using Lease.Contract;
using Lease.Formula;
using Lease.ConShop;
using BaseInfo.User;
using Lease.Union;
using Base.Page;
using Lease.PotBargain;

namespace Invoice
{
    /// <summary>
    /// 联营费用计算
    /// </summary>
    public class UnionChargeCount
    {
        public static float monthSettleDays; //月结算天数
        public static int ifPrepay; //是否预收保底
        public static int isFist;  //是否首期
        public static int custID;  //客户ID
        public static Hashtable Hstb; //操作员信息
        public static int flag;  //用于基本租金计算做标志
        public static string chgeType;  //费用类型
        public static int rentType;     //租金类别
        public static int invId;               //结算单ID
        public static decimal TotalAllMoney;     //总费用金额(结算主表中的结算金额)
        public static ArrayList aryList;
        public static int blankFlag; //空白记录标志
        public static string blankRecord_Sql;  //空白记录sql;
        public static int curTypeID;  //结算币种
        public static Decimal exRate;  //汇率
        public static decimal tempMoney;  //用于非租金
        public static int tempFlag;
        public static int accountPFlag;   //计算抽成标志
        public static DateTime stopDate;   //合同结束日期
        public static ArrayList invCodeAryList; //水电费结算单号
        public static ArrayList otherInvCodeAryList;  //其他费用结算单号
        public static ArrayList PMAryList;    //抽成
        public static decimal inTaxRate;  //进项税率;
        public static decimal outTaxRate;  //销项税率;
        public static DateTime contractStartDate;  //合同开始日期
        public static DateTime chargeStartDate;    //费用开始日期
        public static int noPFlag;  //免租期不计算抽成标志

        //提示信息
        public static int PROMT_SUCCED = 0;  //成功
        public static int PROMT_CONTRACT_NO = -1;  //合同无效
        public static int PROMT_FIRST_CHARGE_NO = -2;  //首期费用未生成
        public static int PROMT_BEFORE_CHARGE_NO = -3;  //前期费用未生成
        public static int PROMT_CONTRACT_DATE_NO = -4;  //结算时间段不在合同范围内
        public static int PROMT_CONTRACT_INFO_NO = -5;  //未合同信息
        public static int PROMT_FIRST_CHARGE_YES = -6;  //首期费用已生成
        public static int PROMT_MONTH_CHARGE_YES = -7;  //该月费用已生成
        public static int PROMT_EXRATE_NO = -8;  //汇率不存在
        public static int PROMT_CONTRACT_STOP = -9;  //合同已终止

        /// <summary>
        /// 费用计算
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="balanceMonth">计帐月</param>
        /// <param name="isFirst">是否首期; 0:否 ; 1 :是</param>
        /// <param name="chargeType">费用类型</param>
        /// <param name="Htb">操作员信息</param>
        /// <param name="bancthID">批次号</param>
        /// <param name="invCode">结算单代码</param>
        /// <param name="bfoChgAryList">未生成前期费用的费用类型ID</param>
        public static int AccountCharge(int contractID, DateTime startTime,DateTime endTime, int isFirst, string chargeType, Hashtable Htb, string bancthID, out int invCode, out ArrayList bfoChgAryList)
        {
            tempFlag = 0;
            tempMoney = 0;
            blankFlag = 0;
            TotalAllMoney = 0;
            accountPFlag = 0;
            noPFlag = 0;
            invId = 0;　　//结算单ID
            aryList = new ArrayList();
            bfoChgAryList = new ArrayList();
            invCodeAryList = new ArrayList();
            otherInvCodeAryList = new ArrayList();
            PMAryList = new ArrayList();
            chgeType = chargeType.Substring(0, chargeType.Length - 1);

            //取结算币种
            Resultset curyRs = GetConUnion(contractID);
            ConUnion conLse = curyRs.Dequeue() as ConUnion;
            curTypeID = conLse.CurTypeID;
            //取结算币种对应的汇率
            exRate = GetCurExRate(curTypeID);
            if (exRate == 0)
            {
                invCode = invId;
                return PROMT_EXRATE_NO;
            }

            //取费用类型;
            string[] ct = Regex.Split(chgeType, ",");
            int cTCount = ct.Length - 1;

            DateTime accountStartDateTime; //结算时间段中的开始时间
            DateTime accountEndDateTime;   //结算时间段中的结束时间
            int week;  //周期
            flag = 0;  //用于基本租金计算做标志
            Hstb = Htb;
            //string invcode = "";

            Resultset rs = GetContract(contractID); //获取合同信息
            if (rs.Count == 1)
            {
                Contract contract = rs.Dequeue() as Contract;
                custID = contract.CustID;
                //判断合同是否有效
                if (contract.ContractStatus == Contract.CONTRACTSTATUS_TYPE_INGEAR || contract.ContractStatus == Contract.CONTRACTSTATUS_TYPE_END)
                {
                    //合同有效
                    contractStartDate = contract.ConStartDate;
                    chargeStartDate = contract.ChargeStartDate;

                    //合同终止日期
                    if (contract.StopDate == Convert.ToDateTime("0001-01-01"))
                    {
                        stopDate = Convert.ToDateTime("1900-01-01");
                    }
                    else
                    {
                        stopDate = contract.StopDate;
                    }


                    //取联营合同相关条款
                    int billCyle = 0;   //结算周期
                    //int settleMode = 0; //结算处理方式
                    Resultset conUnionRs = GetConUnion(contractID);
                    if (conUnionRs.Count == 1)
                    {
                        ConUnion conUnion = conUnionRs.Dequeue() as ConUnion;
                        billCyle = conUnion.BillCycle;   //结算周期
                        inTaxRate = conUnion.InTaxRate;  //进项税率
                        outTaxRate = conUnion.OutTaxRate;  //销项税率;
                        monthSettleDays = conUnion.MonthSettleDays; //月结天数设定
                    }

                    
                    if (isFirst == InvoiceHeader.ISFIRST_YES)
                    {
                        isFist = InvoiceHeader.ISFIRST_YES;   //赋值为首期费用
                        //费用类型;
                        for (int x = 0; x <= cTCount; x++)
                        {
                            bool yearEnd = IsYearEndCount(Convert.ToInt32(ct[x]));
                            if (yearEnd == false)
                            {
                                //判断是否为一次性费用
                                int oneCount = IsOneCharge(contractID, Convert.ToInt32(ct[x]),startTime,endTime);
                                if (oneCount > 0)
                                {
                                    int isResult = CheckIsOneChargeYes(contractID, contract.ChargeStartDate, startTime, endTime, Convert.ToInt32(ct[x]));
                                    if (isResult == 1)
                                    {
                                        //计算一次性费用
                                        int oneResult = CountOneCharge(contractID, Convert.ToInt32(ct[x]), contract.ChargeStartDate, contract.ConEndDate, billCyle);
                                    }
                                }
                                //if (oneResult == 0)  //不是一次性费用
                                //{
                                //    for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                //    {
                                //        blankFlag = 0;
                                //        //应结时间段
                                //        if (week == 0)
                                //        {
                                //            accountStartDateTime = contract.ChargeStartDate;
                                //            accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                //        }
                                //        else
                                //        {
                                //            accountStartDateTime = contract.ChargeStartDate.AddMonths(week);
                                //            accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                //        }
                                //        //结算时段是否在合同范围内
                                //        int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, Convert.ToInt32(ct[x]));
                                //        if (result == PROMT_CONTRACT_DATE_NO)
                                //        {
                                //            invCode = invId;
                                //            //return PROMT_CONTRACT_DATE_NO;
                                //        }
                                //        else if (result == PROMT_CONTRACT_STOP)
                                //        {
                                //            invCode = invId;
                                //            //return PROMT_CONTRACT_STOP;
                                //        }
                                //    }
                                //}
                            }
                        }
                    }
                    else
                    {
                        isFist = InvoiceHeader.ISFIRST_NO;   //赋值为非首期费用
                        BaseBO basebo = new BaseBO();
                        //费用类型;
                        for (int m = 0; m <= cTCount; m++)
                        {
                            /****************计算其他费用和计表费用*****************/
                            bool isChargeType = CheckChargeType(Convert.ToInt32(ct[m]));
                            if (isChargeType == true)
                            {
                                //计算非租金费用
                                tempFlag = CountNoLeaseCharge(contractID, Convert.ToInt32(ct[m]));
                            }
                            /*******************计算银行卡手续费*************************/
                            //判断费用类别是否为银行内卡
                            bool isInCardRate = CheckIsInnerCardRate(Convert.ToInt32(ct[m]));
                            if (isInCardRate == true)
                            {
                                //计算银行内卡手续费
                                AccountInCardRate(contractID, startTime, endTime, Convert.ToInt32(ct[m]));
                            }
                            else
                            {
                                //判断费用类别是否为银行外卡
                                bool isOutCardRate = CheckIsOutCardRate(Convert.ToInt32(ct[m]));
                                if (isOutCardRate == true)
                                {
                                    //计算银行内卡手续费
                                    AccountOutCardRate(contractID, startTime, endTime, Convert.ToInt32(ct[m]));
                                }
                                else
                                {
                                    //判断是否为年终结算
                                    bool yearEnd = IsYearEndCount(Convert.ToInt32(ct[m]));
                                    if (yearEnd == true)
                                    {
                                        YearEndCount(contractID, contract.ChargeStartDate, contract.ConEndDate, contract.StopDate);

                                    }
                                    else
                                    {
                                        DateTime lastAcntDate = DateTime.Now;
                                        int PMFlag = 0; //联营抽成保底标志
                                        //检查费用类型是否为联营抽成保底
                                        string bfPMSql = "select Count(ChargeTypeID) from ChargeType where ChargeClass in (" + ChargeType.CHARGECLASS_UNION + ") and ChargeTypeID = " + Convert.ToInt32(ct[m]);
                                        DataSet bfPMDS = basebo.QueryDataSet(bfPMSql);
                                        if (Convert.ToInt32(bfPMDS.Tables[0].Rows[0][0]) > 0)
                                        {
                                            PMFlag = 1;
                                            //取费用开始计算日期(上次结算日期加一;每种费用类型计算的结束日期)
                                            lastAcntDate = GetLastAccountDate(contractID, Convert.ToInt32(ct[m]), "InvoiceJVDetail");
                                        }
                                        else
                                        {
                                            //取费用开始计算日期(上次结算日期加一;每种费用类型计算的结束日期)
                                            lastAcntDate = GetLastAccountDate(contractID, Convert.ToInt32(ct[m]), "InvoiceDetail");
                                        }

                                        if (lastAcntDate != Convert.ToDateTime("9999-12-31"))
                                        {
                                            if (startTime <= lastAcntDate)
                                            {
                                                //判断是否为一次性费用
                                                //int oneCount = IsOneCharge(contractID, Convert.ToInt32(ct[m]));
                                                int oneCount = IsOneCharge(contractID, Convert.ToInt32(ct[m]), startTime, endTime);
                                                if (oneCount > 0)
                                                {
                                                    if (contract.ChargeStartDate <= startTime)
                                                    {
                                                        int isResult = CheckIsOneChargeYes(contractID, contract.ChargeStartDate, startTime, endTime, Convert.ToInt32(ct[m]));
                                                        if (isResult == 1)
                                                        {
                                                            //计算一次性费用
                                                            int oneResult = CountOneCharge(contractID, Convert.ToInt32(ct[m]), startTime, endTime, billCyle);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    noPFlag = 0;
                                                    int isBeginCharge = 1;
                                                    string bfSql = "select Count(ChargeTypeID) from ChargeType where ChargeClass in (" + ChargeType.CHARGECLASS_LEASE + "," + ChargeType.CHARGECLASS_FANST + "," + ChargeType.CHARGECLASS_UNION + ") and ChargeTypeID = " + Convert.ToInt32(ct[m]);
                                                    DataSet bfDS = basebo.QueryDataSet(bfSql);
                                                    if (Convert.ToInt32(bfDS.Tables[0].Rows[0][0]) > 0)
                                                    {
                                                        if (PMFlag == 1)
                                                        {
                                                            isBeginCharge = CheckBeginCharge(contractID, startTime, Convert.ToInt32(ct[m]), "InvoiceJVDetail");   //检查前期费用是否已生成  1:已生成; 0:未生成
                                                        }
                                                        else
                                                        {
                                                            isBeginCharge = CheckBeginCharge(contractID, startTime, Convert.ToInt32(ct[m]), "InvoiceDetail");   //检查前期费用是否已生成  1:已生成; 0:未生成
                                                        }
                                                    }
                                                    if (isBeginCharge == 1)  //前期费用已生成
                                                    {
                                                        if (lastAcntDate != Convert.ToDateTime("9999-12-31"))
                                                        {
                                                            //for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                                            //{
                                                            ////应结时间段
                                                            //if (week == 0)
                                                            //{
                                                            //    accountStartDateTime = lastAcntDate;
                                                            //    accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                            //}
                                                            //else
                                                            //{
                                                            //    accountStartDateTime = lastAcntDate.AddMonths(week);
                                                            //    accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                            //}
                                                            accountStartDateTime = lastAcntDate;
                                                            accountEndDateTime = endTime;
                                                            //结算时段是否在合同范围内
                                                            int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, Convert.ToInt32(ct[m]));
                                                            if (result == PROMT_CONTRACT_DATE_NO)
                                                            {
                                                                invCode = invId;
                                                                //return PROMT_CONTRACT_DATE_NO;
                                                            }
                                                            else if (result == PROMT_CONTRACT_STOP)
                                                            {
                                                                invCode = invId;
                                                                //return PROMT_CONTRACT_STOP;
                                                            }
                                                            //}
                                                        }
                                                    }
                                                    else //前期费用未生成
                                                    {
                                                        bool isChgType = CheckChargeType(Convert.ToInt32(ct[m]));
                                                        if (isChgType == false)
                                                        {
                                                            bfoChgAryList.Add(Convert.ToInt32(ct[m]));
                                                        }
                                                        //invCode = invId;
                                                        //return PROMT_BEFORE_CHARGE_NO;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            noPFlag = 1;
                                            //判断是否为一次性费用
                                            int oneCount = IsOneCharge(contractID, Convert.ToInt32(ct[m]), startTime, endTime);
                                            if (oneCount > 0)
                                            {
                                                int isResult = CheckIsOneChargeYes(contractID, contract.ChargeStartDate, startTime, endTime, Convert.ToInt32(ct[m]));
                                                if (isResult == 1)
                                                {
                                                    //计算一次性费用
                                                    int oneResult = CountOneCharge(contractID, Convert.ToInt32(ct[m]), startTime, endTime, billCyle);
                                                }
                                            }
                                            //for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                            //{
                                            //应结时间段
                                            //if (week == 0)
                                            //{
                                            //    accountStartDateTime = contract.ChargeStartDate;
                                            //    accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                            //}
                                            //else
                                            //{
                                            //    accountStartDateTime = contract.ChargeStartDate.AddMonths(week);
                                            //    accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                            //}
                                            accountStartDateTime = contract.ChargeStartDate;
                                            accountEndDateTime = endTime;
                                            //结算时段是否在合同范围内
                                            int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, Convert.ToInt32(ct[m]));
                                            if (result == PROMT_CONTRACT_DATE_NO)
                                            {
                                                invCode = invId;
                                                //return PROMT_CONTRACT_DATE_NO;
                                            }
                                            else if (result == PROMT_CONTRACT_STOP)
                                            {
                                                invCode = invId;
                                                //return PROMT_CONTRACT_STOP;
                                            }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    invCode = invId;
                    return PROMT_CONTRACT_NO;
                }
            }
            else
            {
                invCode = invId;
                return PROMT_CONTRACT_INFO_NO;
            }
            invCode = InsertData(contractID, startTime, bancthID);
            return PROMT_SUCCED;
        }

        /// <summary>
        /// 检查是否已生成首期费用
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="isFirst">是否首期</param>
        /// <returns></returns>
        private static Resultset CheckIsFist(int contractID, int isFirst)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ContractID = " + contractID + " and IsFirst = " + isFirst + " and InvStatus != " + InvoiceHeader.INVSTATUS_CANCEL;
            Resultset rs = baseBo.Query(new InvoiceHeader());
            return rs;
        }

        /// <summary>
        /// 检查前期费用是否已生成
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="startDate">费用计算开始日期</param>
        /// <param name="chgTypeID">费用类别ID</param>
        /// <param name="table">表名</param>
        /// <returns>bulidStatus: 1:已生成; 0:未生成</returns>
        private static int CheckBeginCharge(int contractID, DateTime startDate, int chgTypeID,string table)
        {
            int bulidStatus;
            BaseBO baseBo = new BaseBO();
            string sql = "select Count(*) as count from " + table + " A,InvoiceHeader B where A.InvID = B.InvID and A.InvEndDate = '" +
                          startDate.AddDays(-1) + "' and B.ContractID = " + contractID + " and A.ChargeTypeID = " + chgTypeID;
            DataSet ds = baseBo.QueryDataSet(sql);
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["count"]) > 0)
                bulidStatus = 1;
            else
                bulidStatus = 0;
            return bulidStatus;
        }

        /// <summary>
        /// 结算时段是否在合同范围内
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="accountStartTime">结算开始时间</param>
        /// <param name="accountEndTime">结算结束时间</param>
        /// <param name="contractStartTime">合同开始时间</param>
        /// <param name="contractEndTime">合同结束时间</param>
        /// <param name="balanceMonth">计帐月</param>
        /// <param name="chargeTypeID">费用类别ID</param>
        /// <returns></returns>
        private static int IsDateTimeInContract(int contractID, DateTime accountStartTime, DateTime accountEndTime, DateTime contractStartTime, DateTime contractEndTime,int chargeTypeID)
        {
            //判断时间段是否在合同范围内
            if ((accountStartTime >= contractStartTime) && (accountEndTime <= contractEndTime)) //时间段完全在合同范围内
            {
                if (stopDate > Convert.ToDateTime("1900-01-01"))
                {
                    //终止日期小于结算结束日期
                    if (stopDate < accountStartTime)
                    {
                        return PROMT_CONTRACT_STOP;  //合同终止
                    }
                    else if (stopDate > accountEndTime)   //终止日期 > 结算结束日期
                    {
                        GetFormulaHByDate(contractID, accountStartTime, accountEndTime,chargeTypeID);
                        return 0;
                    }
                    else if (stopDate < accountEndTime)  //终止日期 < 结算结束日期
                    {
                        //终止日期在结算开始日期与结算结束日期之间，则结算日期取终止日期
                        accountEndTime = stopDate;
                        GetFormulaHByDate(contractID, accountStartTime, accountEndTime, chargeTypeID);
                        return 0;
                    }
                }
                else
                {
                    //根据结算时间段获取结算公式
                    GetFormulaHByDate(contractID, accountStartTime, accountEndTime, chargeTypeID);
                    return 0;
                }
                return 0;
            }
            else if ((accountStartTime >= contractStartTime) && (accountEndTime >= contractEndTime))  //结算开始日期 > 费用开始日期 and 结算结束日期 > 合同结束日期
            {
                accountEndTime = contractEndTime;
                if (stopDate > Convert.ToDateTime("1900-01-01"))
                {
                    if (stopDate < accountStartTime)  //终止日期 < 结算开始日期
                    {
                        return PROMT_CONTRACT_STOP; //合同终止
                    }
                    else if (stopDate < accountEndTime)  //终止日期 < 结算结束日期
                    {
                        accountEndTime = stopDate;  //结算结束日期 = 终止日期
                        //根据结算时间段获取结算公式
                        GetFormulaHByDate(contractID, accountStartTime, accountEndTime, chargeTypeID);
                        return 0;
                    }
                }
                else
                {
                    GetFormulaHByDate(contractID, accountStartTime, accountEndTime, chargeTypeID);
                    return 0;
                }
                return 0;
            }
            else if ((accountStartTime < contractStartTime) && (accountEndTime > contractEndTime)) //结算开始日期 < 费用开始日期 and 结算结束日期 > 合同结束日期
            {
                accountStartTime = contractStartTime;
                accountEndTime = contractEndTime;
                if (stopDate.Date > Convert.ToDateTime("1900-01-01"))
                {
                    if (stopDate > accountEndTime)  //终止日期 > 结算结束日期
                    {
                        accountEndTime = stopDate;  //结算结束日期 = 终止日期
                        GetFormulaHByDate(contractID, accountStartTime, accountEndTime, chargeTypeID);
                        return 0;
                    }
                }
                else
                {
                    GetFormulaHByDate(contractID, accountStartTime, accountEndTime, chargeTypeID);
                    return 0;
                }
                return 0;
            }
            else  //时间段完全不在合同范围内
            {
                return PROMT_CONTRACT_DATE_NO;
            }
        }

        /// <summary>
        /// 根据结算时间段获取结算公式
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="stratDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="balanceMonth">计帐月</param>
        /// <param name="chargeTypeID">费用类别ID</param>
        private static void GetFormulaHByDate(int contractID, DateTime stratDate, DateTime endDate, int chargeTypeID)
        {
            decimal TotalMoney = 0;     //总费用金额

            BaseBO baseBo = new BaseBO();

            DateTime computeStartDate = DateTime.Now;
            DateTime computeEndDate = DateTime.Now;

            //取计算公式
            Resultset formulHRs = GetFormulaH(contractID, chargeTypeID, stratDate, endDate);
            int myCount = formulHRs.Count;

            //多条计算公式循环
            foreach (ConFormulaH conFormulaH in formulHRs)
            {
                int outFlag = 0;
                //预收保底额
                decimal minMoney = 0m;
                //抽成额
                decimal salesToP = 0;
                //根据抽成保底得出的金额
                decimal salesTo = 0;

                //for (int m = 0; m < chgeTypeCount; m++)
                //{
                //判断是否需要计算此费用类型
                if (conFormulaH.ChargeTypeID == chargeTypeID)
                {
                    if (conFormulaH.FStartDate <= stratDate)
                    {
                        computeStartDate = stratDate;
                    }
                    if (conFormulaH.FStartDate > stratDate)
                    {
                        computeStartDate = conFormulaH.FStartDate;
                    }
                    if (conFormulaH.FEndDate <= endDate)
                    {
                        computeEndDate = conFormulaH.FEndDate;
                    }
                    if (conFormulaH.FEndDate > endDate)
                    {
                        computeEndDate = endDate;
                    }

                    float settleDays = GetAccountDays(computeStartDate, computeEndDate, conFormulaH.RateType);

                    if (isFist != InvoiceHeader.ISFIRST_YES)
                    {
                        if (conFormulaH.FormulaType == ConFormulaH.FORMULATYPE_TYPE_TWO)   //公式类别为抽成保底
                        {
                            /*既有抽成,也有保底;即比较销售额,如果销售大于保底,即取抽成设定,否则取保底设定*/
                            //获取销售额
                            //decimal paidAmt = GetPaidAmt(contractID, stratDate, endDate);
                            decimal notPPaidAmt = 0;  //免租期间销售额
                            if (noPFlag == 1)
                            {
                                if (contractStartDate < chargeStartDate)
                                {
                                    noPFlag = 0;
                                    //免租期间销售额不参与抽成
                                    notPPaidAmt = GetPaidAmt(contractID, contractStartDate, chargeStartDate.AddDays(-1));
                                }
                            }
                            decimal paidAmt = GetPaidAmtByCon(contractID, computeStartDate, computeEndDate);
                            paidAmt = paidAmt / exRate;
                            int formulID = conFormulaH.FormulaID;
                            /*************************取抽成*************************/

                            string pcentOpt = conFormulaH.PcentOpt;    //抽成方式
                            decimal pcent = 0;
                            if (isFist != InvoiceHeader.ISFIRST_YES)
                            {
                                salesToP = GetPMomey(paidAmt, settleDays, formulID, pcentOpt, out pcent);

                                //按商品抽成
                                DataSet tempDS = GetPaidAmtByMaster(contractID, computeStartDate, computeEndDate);
                                for (int x = 0; x < tempDS.Tables[0].Rows.Count; x++)
                                {
                                    decimal tempSales = Convert.ToDecimal(tempDS.Tables[0].Rows[x][0]);
                                    decimal tempSalesP = GetPMomeyByMasterP(tempSales, settleDays, Convert.ToDecimal(tempDS.Tables[0].Rows[x][1]));
                                    salesToP += tempSalesP;

                                    InvoiceJVDetail invoiceJVDet = new InvoiceJVDetail();
                                    invoiceJVDet.invJVDetailID = BaseApp.GetinvJVDetailID();
                                    invoiceJVDet.InvID = invId;
                                    invoiceJVDet.RentType = rentType;
                                    invoiceJVDet.Period = stratDate.AddDays(-stratDate.Day + 1);
                                    invoiceJVDet.InvStartDate = computeStartDate;
                                    invoiceJVDet.InvEndDate = computeEndDate;
                                    invoiceJVDet.InvCurTypeID = curTypeID;
                                    invoiceJVDet.InvExRate = exRate;
                                    invoiceJVDet.invSalesAmt = (tempSales + notPPaidAmt) * exRate;
                                    invoiceJVDet.invSalesAmtL = tempSales + notPPaidAmt;
                                    invoiceJVDet.invPcent = Convert.ToDecimal(tempDS.Tables[0].Rows[x][1]);
                                    invoiceJVDet.InTaxRate = inTaxRate;
                                    invoiceJVDet.OutTaxRate = outTaxRate;
                                    invoiceJVDet.InvPayAmt = 0;
                                    invoiceJVDet.InvPayAmtL = 0;
                                    invoiceJVDet.invJVCostAmt = invoiceJVDet.invSalesAmt - invoiceJVDet.InvPayAmt;
                                    invoiceJVDet.invJVCostAmtL = invoiceJVDet.invSalesAmtL - invoiceJVDet.InvPayAmtL;
                                    invoiceJVDet.InvAdjAmt = 0;
                                    invoiceJVDet.InvAdjAmtL = 0;
                                    invoiceJVDet.InvActPayAmt = invoiceJVDet.InvPayAmt;
                                    invoiceJVDet.InvActPayAmtL = invoiceJVDet.InvPayAmtL;
                                    invoiceJVDet.InvPaidAmt = 0;
                                    invoiceJVDet.InvPaidAmtL = 0;
                                    invoiceJVDet.InvDetStatus = InvoiceJVDetail.INVDETSTATUS_AVAILABILITY;
                                    invoiceJVDet.Note = "";
                                    invoiceJVDet.ChargeTypeID = chargeTypeID;

                                    TotalAllMoney += invoiceJVDet.InvActPayAmt;

                                    PMAryList.Add(invoiceJVDet);
                                }

                                string minSumOpt = conFormulaH.MinSumOpt;        //保底方式
                                if (isFist == InvoiceHeader.ISFIRST_YES)  //是首期
                                {
                                    minMoney = 0;
                                }
                                else
                                {
                                    minMoney = GetMinMoney(paidAmt, salesToP, settleDays, formulID, minSumOpt);  //取公式保底金额
                                    if (salesToP >= minMoney)   //抽成租金 >= 保底金额
                                    {
                                        minMoney = salesToP;  //保底金额 = 抽成租金
                                        if (pcentOpt == ConFormulaH.PCENTOPT_TYPE_FAST)
                                        {
                                            rentType = InvoiceDetail.RENTTYPE_FIXED_P;   //租金类别：固定抽成租金
                                        }
                                        if (pcentOpt == ConFormulaH.PCENTOPT_TYPE_S)
                                        {
                                            rentType = InvoiceDetail.RENTTYPE_SINGLE_P;    //租金类别：单级抽成租金
                                        }
                                        if (pcentOpt == ConFormulaH.PCENTOPT_TYPE_M)
                                        {
                                            rentType = InvoiceDetail.RENTTYPE_MUNCH_P;       //租金类别：多级抽成租金
                                        }

                                        InvoiceJVDetail invoiceJVDet = new InvoiceJVDetail();
                                        invoiceJVDet.invJVDetailID = BaseApp.GetinvJVDetailID();
                                        invoiceJVDet.InvID = invId;
                                        invoiceJVDet.RentType = rentType;
                                        invoiceJVDet.Period = stratDate.AddDays(-stratDate.Day + 1);
                                        invoiceJVDet.InvStartDate = computeStartDate;
                                        invoiceJVDet.InvEndDate = computeEndDate;
                                        invoiceJVDet.InvCurTypeID = curTypeID;
                                        invoiceJVDet.InvExRate = exRate;
                                        invoiceJVDet.invSalesAmt = (paidAmt + notPPaidAmt) * exRate;
                                        invoiceJVDet.invSalesAmtL = paidAmt + notPPaidAmt;
                                        invoiceJVDet.invPcent = pcent;
                                        invoiceJVDet.InTaxRate = inTaxRate;
                                        invoiceJVDet.OutTaxRate = outTaxRate;
                                        invoiceJVDet.InvPayAmt = minMoney;
                                        invoiceJVDet.InvPayAmtL = minMoney / exRate;
                                        invoiceJVDet.invJVCostAmt = invoiceJVDet.invSalesAmt - invoiceJVDet.InvPayAmt;
                                        invoiceJVDet.invJVCostAmtL = invoiceJVDet.invSalesAmtL - invoiceJVDet.InvPayAmtL;
                                        invoiceJVDet.InvAdjAmt = 0;
                                        invoiceJVDet.InvAdjAmtL = 0;
                                        invoiceJVDet.InvActPayAmt = invoiceJVDet.InvPayAmt;
                                        invoiceJVDet.InvActPayAmtL = invoiceJVDet.InvPayAmtL;
                                        invoiceJVDet.InvPaidAmt = 0;
                                        invoiceJVDet.InvPaidAmtL = 0;
                                        invoiceJVDet.InvDetStatus = InvoiceJVDetail.INVDETSTATUS_AVAILABILITY;
                                        invoiceJVDet.Note = "";
                                        invoiceJVDet.ChargeTypeID = chargeTypeID;

                                        TotalAllMoney += invoiceJVDet.InvActPayAmt;

                                        PMAryList.Add(invoiceJVDet);
                                    }
                                }
                                if (salesToP < minMoney)
                                {
                                    InvoiceJVDetail invoiceJVDet = new InvoiceJVDetail();
                                    invoiceJVDet.invJVDetailID = BaseApp.GetinvJVDetailID();
                                    invoiceJVDet.InvID = invId;
                                    invoiceJVDet.RentType = rentType;
                                    invoiceJVDet.Period = stratDate.AddDays(-stratDate.Day + 1);
                                    invoiceJVDet.InvStartDate = computeStartDate;
                                    invoiceJVDet.InvEndDate = computeEndDate;
                                    invoiceJVDet.InvCurTypeID = curTypeID;
                                    invoiceJVDet.InvExRate = exRate;
                                    invoiceJVDet.invSalesAmt = (paidAmt + notPPaidAmt) * exRate;
                                    invoiceJVDet.invSalesAmtL = paidAmt + notPPaidAmt;
                                    invoiceJVDet.invPcent = pcent;
                                    invoiceJVDet.InTaxRate = inTaxRate;
                                    invoiceJVDet.OutTaxRate = outTaxRate;
                                    invoiceJVDet.InvPayAmt = minMoney;
                                    invoiceJVDet.InvPayAmtL = minMoney / exRate;
                                    invoiceJVDet.invJVCostAmt = invoiceJVDet.invSalesAmt - invoiceJVDet.InvPayAmt;
                                    invoiceJVDet.invJVCostAmtL = invoiceJVDet.invSalesAmtL - invoiceJVDet.InvPayAmtL;
                                    invoiceJVDet.InvAdjAmt = 0;
                                    invoiceJVDet.InvAdjAmtL = 0;
                                    invoiceJVDet.InvActPayAmt = invoiceJVDet.InvPayAmt;
                                    invoiceJVDet.InvActPayAmtL = invoiceJVDet.InvPayAmtL;
                                    invoiceJVDet.InvPaidAmt = 0;
                                    invoiceJVDet.InvPaidAmtL = 0;
                                    invoiceJVDet.InvDetStatus = InvoiceJVDetail.INVDETSTATUS_AVAILABILITY;
                                    invoiceJVDet.Note = "";
                                    invoiceJVDet.ChargeTypeID = chargeTypeID;

                                    TotalAllMoney += invoiceJVDet.InvActPayAmt;

                                    PMAryList.Add(invoiceJVDet);
                                }
                            }
                        }
                    }
                    decimal baseAmt = 0m;  //基本租金
                    decimal TotalFixedRental = 0m; //租金额

                    /**********************公式类别为固定***********************/
                    if (conFormulaH.FormulaType == ConFormulaH.FORMULATYPE_TYPE_ONE)   //公式类别为固定
                    {
                        TotalFixedRental = conFormulaH.FixedRental * Convert.ToDecimal(settleDays); //总租金额
                        if (GetChargeClass(conFormulaH.ChargeTypeID) != ChargeType.CHARGECLASS_LEASE)
                            rentType = InvoiceDetail.RENTTYPE_NO_RENT;

                        InvoiceDetail invoiceDet = new InvoiceDetail();
                        invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                        invoiceDet.ChargeTypeID = conFormulaH.ChargeTypeID; //费用类别ID
                        invoiceDet.InvID = invId;  //结算单ID
                        invoiceDet.Period = stratDate.AddDays(-stratDate.Day + 1);     //费用记账月
                        invoiceDet.InvStartDate = computeStartDate;  //费用开始日期
                        invoiceDet.InvEndDate = computeEndDate;      //费用结束日期
                        invoiceDet.InvCurTypeID = curTypeID;  //结算币种
                        invoiceDet.InvExRate = exRate;  //结算汇率
                        invoiceDet.InvPayAmt = TotalFixedRental;  //费用应结金额
                        invoiceDet.InvPayAmtL = TotalFixedRental / exRate;  //费用应结本币金额 = 费用应结金额 / 结算汇率
                        invoiceDet.InvActPayAmt = TotalFixedRental; //费用实际应结金额
                        invoiceDet.InvActPayAmtL = TotalFixedRental / exRate;  //费用实际应结本币金额
                        invoiceDet.RentType = rentType;   //租金类别

                        TotalAllMoney += TotalFixedRental;
                        aryList.Add(invoiceDet);

                    }
                }
                //}
            }
        }

        /// <summary>
        /// 计算一次性费用
        /// </summary>
        /// <param name="conID">合同ID</param>
        /// <param name="chargeType">费用类别ID</param>
        /// <param name="conStartDate">合同开始时间</param>
        /// <param name="conEndDate">合同结束时间</param>
        /// <param name="billCyle">结算周期</param>
        private static int CountOneCharge(int conID, int chargeType, DateTime conStartDate, DateTime conEndDate,int billCyle)
        {
            int result = 0;
            string str_sql = "";

            str_sql = "select FormulaID,ChargeTypeID,ContractID,FStartDate,FEndDate,FormulaType,RateType,PcentOpt,MinSumOpt,TotalArea," +
                             "FixedRental,UnitPrice,BaseAmt" +
                             " from ConFormulaH " +
                             " where ContractID = " + conID +
                             " and FStartDate >= '" + conStartDate +
                             "' and FStartDate <= '" + conStartDate.AddMonths(billCyle).AddDays(-conStartDate.Day) +
                             "' and ChargeTypeID = " + chargeType +
                             " and FormulaType = '" + ConFormulaH.FORMULATYPE_TYPE_THREE + "'";

            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                if (Convert.ToDateTime(ds.Tables[0].Rows[i]["FStartDate"]) >= conStartDate && Convert.ToDateTime(ds.Tables[0].Rows[i]["FEndDate"]) >= conStartDate)
                {
                    decimal baseAmt = Convert.ToDecimal(ds.Tables[0].Rows[i]["BaseAmt"]);

                    InvoiceDetail invDetail = new InvoiceDetail();
                    invDetail.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                    invDetail.ChargeTypeID = Convert.ToInt32(ds.Tables[0].Rows[i]["ChargeTypeID"]); //费用类别ID
                    invDetail.InvID = invId;  //结算单ID
                    invDetail.Period = Convert.ToDateTime(ds.Tables[0].Rows[i]["FStartDate"]).AddDays(-Convert.ToDateTime(ds.Tables[0].Rows[i]["FStartDate"]).Day + 1);//balanceMonth.AddDays(-balanceMonth.Day + 1);     //费用记账月
                    invDetail.InvStartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FStartDate"]);  //费用开始日期
                    invDetail.InvEndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["FEndDate"]);      //费用结束日期
                    invDetail.InvCurTypeID = curTypeID;  //结算币种
                    invDetail.InvExRate = exRate;  //结算汇率
                    invDetail.InvPayAmt = baseAmt;  //费用应结金额
                    invDetail.InvPayAmtL = baseAmt / exRate;  //费用应结本币金额 = 费用应结金额 * 结算汇率
                    invDetail.InvActPayAmt = baseAmt; //费用实际应结金额
                    invDetail.InvActPayAmtL = baseAmt / exRate;  //费用实际应结本币金额
                    invDetail.RentType = InvoiceDetail.RENTTYPE_ONCE;   //租金类别

                    TotalAllMoney += Convert.ToDecimal(baseAmt);
                    aryList.Add(invDetail);
                }
                result = 1;
            }
            return result;
        }

        private static int InsertData(int contractID,DateTime startDate, string bcthID)
        {
            //取结算币种
            Resultset curyRs = GetConUnion(contractID);
            ConUnion conLse = curyRs.Dequeue() as ConUnion;
            int curTypeID = conLse.CurTypeID;
            //取结算币种对应的汇率
            Decimal exRate = GetCurExRate(curTypeID);

            InvoiceHeader invoiceHer = new InvoiceHeader();
            if (TotalAllMoney != 0)
            {
                //结算主表
                invoiceHer.CustID = custID;     //客户ID
                invoiceHer.CreateUserID = Convert.ToInt32(Hstb["CreateUserID"]);  //操作员
                invoiceHer.OprDeptID = Convert.ToInt32(Hstb["OprDeptID"]);  //操作员部门
                invoiceHer.OprRoleID = Convert.ToInt32(Hstb["OprRoleID"]);  //操作员角色
                invoiceHer.CustName = GetCustName(contractID);   //客户名
                //invoiceHer.InvCode = invId.ToString(); //结算单代码
                invoiceHer.ContractID = contractID;  //合同号
                invoiceHer.PrintFlag = 0;    //打印标志
                //invoiceHer.InvID = invId;        //结算单ID
                invoiceHer.IsFirst = isFist;  //是否首期
                invoiceHer.CurTypeID = curTypeID;  //币种代码
                invoiceHer.InvCurTypeID = curTypeID; //结算币种
                invoiceHer.InvExRate = exRate;   //结算汇率
                invoiceHer.InvPeriod = startDate.AddDays(-startDate.Day + 1); //结算单记账月
                if (isFist == InvoiceHeader.ISFIRST_YES)
                {
                    invoiceHer.InvType = InvoiceHeader.INVTYPE_LEASE;
                }
                else
                {
                    invoiceHer.InvType = InvoiceHeader.INVTYPE_UNION;
                }
                //invoiceHer.InvPayAmt = TotalAllMoney;  //结算金额
                //invoiceHer.InvPayAmtL = TotalAllMoney / exRate; //结算本币金额 = 结算金额 * 结算汇率
                //invoiceHer.InvActPayAmt = TotalAllMoney;  //费用实际应结金额
                //invoiceHer.InvPayAmtL = TotalAllMoney / exRate;  //费用实际应结本币金额
                invoiceHer.BancthID = bcthID;   //批次号


                BaseTrans baseTrans = new BaseTrans();
                baseTrans.BeginTrans();
                try
                {
                    invId = BaseApp.GetInvID();　　//结算单ID
                    invoiceHer.InvCode = invId.ToString();
                    invoiceHer.InvID = invId;
                    if (baseTrans.Insert(invoiceHer) == 1)  //保存结算主表
                    {
                        int count = aryList.Count;
                        for (int j = 0; j < count; j++)   //循环保存结算明细
                        {
                            //aryList.Add(invId);
                            InvoiceDetail invoiceDet = (InvoiceDetail)aryList[j];
                            invoiceDet.InvID = invId;
                            baseTrans.Insert((BasePO)aryList[j]);
                        }
                        int pmCount = PMAryList.Count;
                        for (int i = 0; i < pmCount; i++)
                        {
                            InvoiceJVDetail invJVDet = (InvoiceJVDetail)PMAryList[i];
                            invJVDet.InvID = invId;
                            baseTrans.Insert((BasePO)PMAryList[i]);
                        }
                        int chargeCount = invCodeAryList.Count;
                        for (int l = 0; l < chargeCount; l++)  //修改水电费结算单号
                        {
                            string str_sql = "update Charge set InvCode = '" + invId + "' where ChgID = " + Convert.ToInt32(invCodeAryList[l]);
                            if (baseTrans.ExecuteUpdate(str_sql) == 1)
                            {
                            }
                        }
                        int otherChargeCount = otherInvCodeAryList.Count;
                        for (int m = 0; m < otherChargeCount; m++)  //修改其他费用结算单号
                        {
                            string sql = "update OtherChargeH set InvCode = '" + invId + "' where OtherChargeHID = " + Convert.ToInt32(otherInvCodeAryList[m]);
                            if (baseTrans.ExecuteUpdate(sql) == 1)
                            {
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    baseTrans.Rollback();
                    throw ex;
                }
                baseTrans.Commit();
                return invId;
            }
            else
            {
                return invId = 0;
            }
        }

        /// <summary>
        /// 计算其它费用和水电费
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="chgTypeID">费用类型ID</param>
        /// <returns></returns>
        private static int CountNoLeaseCharge(int contractID, int chgTypeID)
        {
            bool isChargeType = CheckChargeType(chgTypeID);
            if (isChargeType == true)
            {
                DataSet shopDS = GetShopID(contractID);
                int shopCount = shopDS.Tables[0].Rows.Count;
                for (int iShop = 0; iShop < shopCount; iShop++)
                {
                    int cnShopID = Convert.ToInt32(shopDS.Tables[0].Rows[iShop]["ShopID"]);
                    int chgeCount = 0;
                    DataSet chgeDS = new DataSet();
                    DataSet chgDs = new DataSet();
                    if (GetChargeClass(chgTypeID) == ChargeType.CHARGECLASS_WATERORDLECT)  //水电费
                    {
                        chgDs = GetChgID(cnShopID);
                        int count = chgDs.Tables[0].Rows.Count;
                        for (int i = 0; i < count; i++)
                        {
                            chgeDS = GetChgDetail(Convert.ToInt32(chgDs.Tables[0].Rows[i]["ChgID"]), chgTypeID);
                            chgeCount = chgeDS.Tables[0].Rows.Count;
                            if (chgeCount > 0)
                            {
                                invCodeAryList.Add(chgDs.Tables[0].Rows[i]["ChgID"]);
                                tempFlag = AddAryList(chgeDS);
                            }
                        }
                    }
                    else if (GetChargeClass(chgTypeID) == ChargeType.CHARGECLASS_OTHER)  //其它费用
                    {
                        chgDs = GetOtherChgID(cnShopID);
                        int oCount = chgDs.Tables[0].Rows.Count;
                        for (int l = 0; l < oCount; l++)
                        {
                            chgeDS = GetOtherChgD(Convert.ToInt32(chgDs.Tables[0].Rows[l]["OtherChargeHID"]), chgTypeID);
                            chgeCount = chgeDS.Tables[0].Rows.Count;
                            if (chgeCount > 0)
                            {
                                otherInvCodeAryList.Add(chgDs.Tables[0].Rows[l]["OtherChargeHID"]);
                                tempFlag = AddAryList(chgeDS);
                            }
                        }
                    }

                }
            }
            return tempFlag;
        }

        /// <summary>
        /// 将水电费或其他费用添加到数组
        /// </summary>
        /// <param name="chgeDS">费用信息</param>
        /// <returns></returns>
        private static int AddAryList(DataSet chgeDS)
        {
            rentType = InvoiceDetail.RENTTYPE_NO_RENT;   //租金类别：非租金
            int chgeCount = chgeDS.Tables[0].Rows.Count;
            for (int k = 0; k < chgeCount; k++)
            {
                InvoiceDetail invoiceDet = new InvoiceDetail();
                invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                invoiceDet.ChargeTypeID = Convert.ToInt32(chgeDS.Tables[0].Rows[k]["ChargeTypeID"]); //费用类别ID
                invoiceDet.InvID = invId;  //结算单ID
                invoiceDet.Period = Convert.ToDateTime(chgeDS.Tables[0].Rows[k]["StartDate"]).AddDays(-Convert.ToDateTime(chgeDS.Tables[0].Rows[k]["StartDate"]).Day + 1);//balanceMonth.AddDays(-balanceMonth.Day + 1);     //费用记账月
                invoiceDet.InvStartDate = Convert.ToDateTime(chgeDS.Tables[0].Rows[k]["StartDate"]);  //费用开始日期
                invoiceDet.InvEndDate = Convert.ToDateTime(chgeDS.Tables[0].Rows[k]["EndDate"]);      //费用结束日期
                invoiceDet.InvCurTypeID = curTypeID;  //结算币种
                invoiceDet.InvExRate = exRate;  //结算汇率
                invoiceDet.InvPayAmt = Convert.ToDecimal(chgeDS.Tables[0].Rows[k]["ChgAmt"]);  //费用应结金额
                invoiceDet.InvPayAmtL = Convert.ToDecimal(chgeDS.Tables[0].Rows[k]["ChgAmt"]) / exRate;  //费用应结本币金额 = 费用应结金额 / 结算汇率
                invoiceDet.InvActPayAmt = Convert.ToDecimal(chgeDS.Tables[0].Rows[k]["ChgAmt"]); //费用实际应结金额
                invoiceDet.InvActPayAmtL = Convert.ToDecimal(chgeDS.Tables[0].Rows[k]["ChgAmt"]) / exRate;  //费用实际应结本币金额
                invoiceDet.RentType = rentType;   //租金类别

                TotalAllMoney += Convert.ToDecimal(chgeDS.Tables[0].Rows[k]["ChgAmt"]);
                tempMoney += Convert.ToDecimal(chgeDS.Tables[0].Rows[k]["ChgAmt"]);
                aryList.Add(invoiceDet);
                if (tempMoney > 0)
                    tempFlag = 1;
            }
            return tempFlag;
        }


        /// <summary>
        /// 获取抽成租金
        /// </summary>
        /// <param name="saleMoney">销售额</param>
        /// <param name="settleDays">比重天数</param>
        /// <param name="formulaId">抽成公式ID</param>
        /// <param name="pcentOpt">抽成方式</param>
        /// <param name="pcent">抽成率</param>
        /// <returns></returns>
        private static decimal GetPMomey(decimal saleMoney, float settleDays, int formulaId, string pcentOpt,out decimal pcent)
        {
            decimal saleToP = 0m;
            decimal xPcent = 0;
            DataSet conFormulaPds = GetConFormulaP(formulaId);
            int conFormulaPCount = conFormulaPds.Tables[0].Rows.Count;
            if (conFormulaPCount > 0)
            {
                if (pcentOpt == ConFormulaH.PCENTOPT_TYPE_FAST)  //抽成为固定
                {
                    xPcent = Convert.ToDecimal(conFormulaPds.Tables[0].Rows[0]["Pcent"]);  //抽成率
                    saleToP = saleMoney * xPcent;
                    rentType = InvoiceDetail.RENTTYPE_FIXED_P;   //租金类别：固定抽成租金
                }
                else if (pcentOpt == ConFormulaH.PCENTOPT_TYPE_S)  //抽成为单级
                {
                    for (int u = 0; u < conFormulaPCount; u++)
                    {
                        if (saleMoney < Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["SalesTo"]) * Convert.ToDecimal(settleDays))
                        {
                            xPcent = Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["Pcent"]);
                            saleToP = saleMoney * xPcent;
                            break;
                        }
                    }
                    rentType = InvoiceDetail.RENTTYPE_SINGLE_P;    //租金类别：单级抽成租金
                }
                else if (pcentOpt == ConFormulaH.PCENTOPT_TYPE_M)  //抽成为多级式
                {
                    decimal beginSales = 0m;
                    for (int u = 0; u < conFormulaPCount; u++)
                    {
                        decimal salesTo = Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["SalesTo"]) * Convert.ToDecimal(settleDays);
                        if (saleMoney > salesTo)
                        {
                            xPcent = Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["Pcent"]);
                            saleToP += (salesTo - beginSales) * xPcent;
                            beginSales = salesTo;

                            //comRate = comRate - (Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["SalesTo"]) - comRate);
                        }
                        else if (u == 0)
                        {
                            xPcent = Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["Pcent"]);
                            saleToP += saleMoney * xPcent;
                            break;
                        }
                        else
                        {
                            xPcent = Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["Pcent"]);
                            saleToP += (saleMoney - (Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u - 1]["SalesTo"]) * Convert.ToDecimal(settleDays))) * xPcent;
                            break;
                        }
                    }
                    rentType = InvoiceDetail.RENTTYPE_MUNCH_P;       //租金类别：多级抽成租金
                }
            }
            pcent = xPcent;
            return saleToP;
        }

        /// <summary>
        /// 根据商品抽成率获取抽成租金
        /// </summary>
        /// <param name="saleMoney">销售额</param>
        /// <param name="settleDays">比重天数</param>
        /// <param name="pcentOpt">抽成方式</param>
        /// <returns></returns>
        private static decimal GetPMomeyByMasterP(decimal saleMoney, float settleDays, decimal pcent)
        {
            decimal saleToP = 0m;
            saleToP = saleMoney * pcent;
            return saleToP;
        }

        /// <summary>
        /// 获取保底租金
        /// </summary>
        /// <param name="saleMoney">销售额</param>
        /// <param name="salesToP">抽成租金</param>
        /// <param name="settleDays">比重天数</param>
        /// <param name="formulaId">抽成公式ID</param>
        /// <param name="minSumOpt">保底方式</param>
        /// <returns></returns>
        private static decimal GetMinMoney(decimal saleMoney, decimal salesToP, float settleDays, int formulaId, string minSumOpt)
        {
            decimal salesToM = 0m;
            decimal salesTo = 0m;
            DataSet conFormulaMds = GetConFormulaM(formulaId);
            int conFormulaMCount = conFormulaMds.Tables[0].Rows.Count;

            if (conFormulaMCount > 0)
            {
                if (minSumOpt == ConFormulaH.MINSUMOPT_TYPE_FAST)   //保底为固定
                {
                    salesToM = Convert.ToDecimal(conFormulaMds.Tables[0].Rows[0]["MinSum"]) * Convert.ToDecimal(settleDays);
                    //如果销售抽成额大于固定保底额,则取销售抽成额,否则取固定保底额
                    if (salesToP >= salesToM)
                    {
                        salesTo = salesToP;
                    }
                    else
                    {
                        salesTo = salesToM;
                        rentType = InvoiceDetail.RENTTYPE_FIXED_M;          //租金类别：固定保底租金
                    }

                }
                else if (minSumOpt == ConFormulaH.MINSUMOPT_TYPE_T)   //保底为级别式
                {
                    for (int x = 0; x < conFormulaMCount; x++)
                    {
                        //判断销售额处于哪段保底额之间并取保底额(取保底设定)
                        if (saleMoney < Convert.ToDecimal(conFormulaMds.Tables[0].Rows[x]["SalesTo"]) * Convert.ToDecimal(settleDays))
                        {
                            salesToM = Convert.ToDecimal(conFormulaMds.Tables[0].Rows[x]["MinSum"]) * Convert.ToDecimal(settleDays);
                            break;
                        }
                    }
                    //如果销售抽成额大于保底额,则取销售抽成额,否则取保底额
                    if (salesToP >= salesToM)
                    {
                        salesTo = salesToP;
                    }
                    else
                    {
                        salesTo = salesToM;
                        rentType = InvoiceDetail.RENTTYPE_FIXED_P;      //租金类别：固定抽成租金
                    }

                }
            }
            else
            {
                rentType = InvoiceDetail.RENTTYPE_FIXED_M;          //租金类别：固定保底租金
                salesTo = salesToP;
            }
            return salesTo;
        }

        /// <summary>
        /// 获取合同信息
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <returns></returns>
        private static Resultset GetContract(int contractID)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "";
            baseBo.WhereClause = "ContractID = " + contractID;
            Resultset rs = baseBo.Query(new Contract());
            return rs;
        }

        /// <summary>
        /// 获取联营合同相关条款信息
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <returns></returns>
        private static Resultset GetConUnion(int contractID)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ContractID = " + contractID;
            Resultset rs = baseBo.Query(new ConUnion());
            return rs;
        }

        /// <summary>
        /// 获取合同对应的商铺ＩＤ
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <returns></returns>
        private static DataSet GetShopID(int contractID)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ContractID = " + contractID;
            string str_sql = "select ShopId,AreaId,BuildingID,BrandID,UnitTypeID,ContractID," +
                              "FloorID,LocationID,CreateUserID,CreateTime,ModifyUserID," +
                              "ModifyTime,OprRoleID,OprDeptID,ShopCode,ShopName,RefID," +
                              "RentArea,ShopStatus,ShopTypeID,ShopStartDate," +
                              "ShopEndDate,ContactorName,Tel from ConShop where ContractID = " + contractID;
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 获取费用单ID
        /// </summary>
        /// <param name="conshopID">商铺ＩＤ</param>
        /// <returns></returns>
        private static DataSet GetChgID(int conshopID)
        {
            BaseBO baseBo = new BaseBO();
            //baseBo.WhereClause = "ShopID = "+ conshopID + " and ChgPeriod = '"+ balanceMonth + "'";
            string str_sql = "select A.ChgID from Charge A where A.ShopID = " + conshopID +
                            " and A.InvCode = '" + 0 + "' and A.ChgStatus = " + Charge.CHGSTATUS_TYPE_ATTREM;
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 获取其它费用单ID
        /// </summary>
        /// <param name="conshopID">商铺ＩＤ</param>
        /// <returns></returns>
        private static DataSet GetOtherChgID(int conshopID)
        {
            BaseBO baseBo = new BaseBO();
            //baseBo.WhereClause = "ShopID = "+ conshopID + " and ChgPeriod = '"+ balanceMonth + "'";
            string str_sql = "select A.OtherChargeHID from OtherChargeH A where A.ShopID = " + conshopID +
                            " and A.InvCode = '" + 0 + "' and A.ChgStatus = " + Charge.CHGSTATUS_TYPE_ATTREM;
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 获取费用明细
        /// </summary>
        /// <param name="chgID">费用单ID</param>
        /// <returns></returns>
        private static Resultset GetChargeDetail(int chgID)
        {
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ChgDetID = " + chgID;
            Resultset rs = baseBo.Query(new ChargeDetail());
            return rs;
        }

        /// <summary>
        /// 获取费用明细
        /// </summary>
        /// <param name="chgID">费用单ID</param>
        /// <param name="chgTypeID">费用类别ID</param>
        /// <returns></returns>
        private static DataSet GetChgDetail(int chgID, int chgTypeID)
        {
            BaseBO baseBo = new BaseBO();
            string sql = "select ChgAmt,ChargeTypeID,StartDate,EndDate from ChargeDetail where ChgID = " + chgID + " and ChargeTypeID = " + chgTypeID;
            DataSet ds = baseBo.QueryDataSet(sql);
            return ds;
        }

        /// <summary>
        /// 获取其它费用明细
        /// </summary>
        /// <param name="chgID">费用单ID</param>
        /// <param name="chgTypeID">费用类别ID</param>
        /// <returns></returns>
        private static DataSet GetOtherChgD(int chgID, int chgTypeID)
        {
            BaseBO baseBo = new BaseBO();
            string sql = "select ChgAmt,ChargeTypeID,StartDate,EndDate from OtherChargeD where OtherChargeHID = " + chgID + " and ChargeTypeID = " + chgTypeID;
            DataSet ds = baseBo.QueryDataSet(sql);
            return ds;
        }

        /// <summary>
        /// 获取结算时间段计算公式
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <returns></returns>
        private static Resultset GetFormulaH(int contractID, int chargeTypeID, DateTime startTime, DateTime endTime)
        {
            BaseBO baseBo = new BaseBO();
            //baseBo.WhereClause = "ContractID = " + contractID + " and FStartDate >= '" + startTime + "' and FEndDate <= '" + endTime+"'";
            baseBo.WhereClause = "ContractID = " + contractID + " and ChargeTypeID = " + chargeTypeID + " and ( (FStartDate between '" + startTime + "' and '" + endTime + "')" +
                                   " or (FEndDate between '" + startTime + "' and '" + endTime + "')" +
                                   " or ( '" + startTime + "' between FStartDate and FEndDate )" +
                                   " or ( '" + endTime + "' between FStartDate and FEndDate ))";
            baseBo.OrderBy = "FStartDate";
            Resultset rs = baseBo.Query(new ConFormulaH());
            return rs;
        }

        /// <summary>
        /// 获取保底设定
        /// </summary>
        /// <param name="formulaHID">公式ID</param>
        /// <returns></returns>
        private static DataSet GetConFormulaM(int formulaHID)
        {
            BaseBO baseBo = new BaseBO();
            //string sql = "select a.Salesto,a.MinSum from ConFormulaM a right join(select min(salesto) as salesto from ConFormulaM where FormulaID = " + formulaHID +
            //                ") b on a.salesto = b.salesto and a.FormulaID = " + formulaHID;
            string sql = "select ConFormulaMID,FormulaID,SalesTo,MinSum from ConFormulaM where FormulaID = " + formulaHID + " order by SalesTo";
            DataSet ds = baseBo.QueryDataSet(sql);
            return ds;
        }

        /// <summary>
        /// 获取抽成设定
        /// </summary>
        /// <param name="formulaHID">公式ID</param>
        /// <returns></returns>
        private static DataSet GetConFormulaP(int formulaHID)
        {
            BaseBO baseBo = new BaseBO();
            //string sql = "select a.Salesto,a.MinSum from ConFormulaM a right join(select min(salesto) as salesto from ConFormulaM where FormulaID = " + formulaHID +
            //                ") b on a.salesto = b.salesto and a.FormulaID = " + formulaHID;
            string sql = "select ConFormulaPID,FormulaID,SalesTo,Pcent from ConFormulaP where FormulaID = " + formulaHID + " order by SalesTo";
            DataSet ds = baseBo.QueryDataSet(sql);
            return ds;
        }

        /// <summary>
        /// 获取币种汇率
        /// </summary>
        /// <param name="curTypeId">币种ID</param>
        /// <returns></returns>
        private static decimal GetCurExRate(int curTypeId)
        {
            decimal exRate = 0;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ToCurTypeID = " + curTypeId + " and ExRateDate = '" + DateTime.Now.ToShortDateString() + "'";
            Resultset rs = baseBo.Query(new CurExRate());
            if (rs.Count > 0)
            {
                CurExRate curEx = rs.Dequeue() as CurExRate;
                exRate = curEx.ExRate;
            }
            else
            {
                string str_sql = "select top 1 exRate from CurExRate where ToCurTypeID = " + curTypeId + " order by ExRateDate desc ";
                baseBo.WhereClause = "";
                DataSet ds = baseBo.QueryDataSet(str_sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    exRate = Convert.ToDecimal(ds.Tables[0].Rows[0]["exRate"]);
                }
            }
            return exRate;
        }

        /// <summary>
        /// 获取销售额
        /// </summary>
        /// <param name="conID">合同号</param>
        /// <param name="beginDate">费用计算开始时间</param>
        /// <param name="endDate">费用计算结束时间</param>
        /// <returns></returns>
        private static decimal GetPaidAmt(int conID, DateTime beginDate, DateTime endDate)
        {
            string sql = "select sum(PaidAmt) as PaidAmt from TransSkuMedia A,ConShop B where A.ShopID = B.ShopID and B.ContractID = " +
                            conID + " and A.BizDate between '" + beginDate + "' and '" + endDate + "'  and mediamno != 800 ";
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(sql);
            decimal paidAmt = (ds.Tables[0].Rows[0]["PaidAmt"] == DBNull.Value ? 0m : Convert.ToDecimal(ds.Tables[0].Rows[0]["PaidAmt"]));
            return paidAmt;
        }

        /// <summary>
        /// 获取销售额
        /// </summary>
        /// <param name="conID">合同号</param>
        /// <param name="beginDate">费用计算开始时间</param>
        /// <param name="endDate">费用计算结束时间</param>
        /// <returns></returns>
        private static decimal GetPaidAmtByCon(int conID, DateTime beginDate, DateTime endDate)
        {
            string sql = "select sum(PaidAmt) as PaidAmt from TransSkuMedia A,ConShop B where A.ShopID = B.ShopID and B.ContractID = " +
                            conID + " and A.BizDate between '" + beginDate + "' and '" + endDate + "'  and mediamno != 800 AND isCust = 'Y'";
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(sql);
            decimal paidAmt = (ds.Tables[0].Rows[0]["PaidAmt"] == DBNull.Value ? 0m : Convert.ToDecimal(ds.Tables[0].Rows[0]["PaidAmt"]));
            return paidAmt;
        }

        /// <summary>
        /// 获取按商品抽成率的销售额
        /// </summary>
        /// <param name="conID">合同号</param>
        /// <param name="beginDate">费用计算开始时间</param>
        /// <param name="endDate">费用计算结束时间</param>
        /// <returns></returns>
        private static DataSet GetPaidAmtByMaster(int conID, DateTime beginDate, DateTime endDate)
        {
            string sql = "select sum(PaidAmt) as PaidAmt,pcentRate from TransSkuMedia A,ConShop B where A.ShopID = B.ShopID and B.ContractID = " +
                            conID + " and A.BizDate between '" + beginDate + "' and '" + endDate + "' and mediamno != 800 AND isCust = 'N' GROUP BY pcentRate";
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(sql);
            //decimal paidAmt = (ds.Tables[0].Rows[0]["PaidAmt"] == DBNull.Value ? 0m : Convert.ToDecimal(ds.Tables[0].Rows[0]["PaidAmt"]));
            return ds;
        }

        /// <summary>
        /// 是否本月已生成保底
        /// </summary>
        /// <param name="balanceMonth">记帐月</param>
        /// <returns></returns>
        private static bool IsMinSum(DateTime balanceMonth)
        {
            bool isYesOrNoIfPrepay;
            string sql = "select count(InvDetailID) as colCount from InvoiceDetail where Period = '" + balanceMonth.AddMonths(-1) + "'";
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(sql);
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                isYesOrNoIfPrepay = true;
            else
                isYesOrNoIfPrepay = false;
            return isYesOrNoIfPrepay;
        }

        /// <summary>
        /// 取本月存在空白记录
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="balanceMonth">记帐月</param>
        /// <returns></returns>
        private static DataSet GetBlankRecord(int contractID, DateTime balanceMonth)
        {
            BaseBO baseBo = new BaseBO();
            string str_sql = "select A.InvDetailID,A.ChargeTypeID,A.InvID,A.Period,A.InvStartDate,A.InvEndDate,A.InvCurTypeID,A.InvExRate,A.InvPayAmt,A.InvPayAmtL,A.InvAdjAmt,A.InvAdjAmtL," +
                             "A.InvDiscAmt,A.InvDiscAmtL,A.InvChgAmt,A.InvChgAmtL,A.InvActPayAmt,A.InvActPayAmtL,A.InvPaidAmt,A.InvPaidAmtL,A.InvType,A.InvDetStatus,A.Note,A.RentType " +
                             " from InvoiceDetail A inner join InvoiceHeader B ON A.InvID = B.InvID" +
                             " where A.Period = '" + balanceMonth + "' and A.rentType = " + InvoiceDetail.RENTTYPE_BLANK_RECORD_P + " and B.ContractID = " + contractID;
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds;

        }

        /// <summary>
        /// 获取本月已收保底金额
        /// </summary>
        /// <param name="balanceMonth">记帐月</param>
        /// <param name="contractID">合同ID</param>
        /// <returns></returns>
        private static decimal GetExistMinMoney(DateTime sDate, DateTime eDate, int contractID)
        {
            decimal minMoney = 0m;
            string sql = "select A.InvPayAmtL from InvoiceDetail A,InvoiceHeader B where A.InvID = B.InvID and A.InvStartDate <= '" + sDate + "' and A.InvEndDate >= '" + eDate + "' and B.ContractID = " + contractID + " and A.rentType = '" + InvoiceDetail.RENTTYPE_FIXED_M + "' or A.rentType = '" + InvoiceDetail.RENTTYPE_LEVEL_M + "'";
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                minMoney = Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
            }
            return minMoney;
        }

        /// <summary>
        /// 是否存在正常抽成记录
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="balanceMonth">记帐月</param>
        /// <returns></returns>
        private static bool IsExistConFormulaP(int contractID, DateTime balanceMonth)
        {
            bool conFormulaP;
            BaseBO baseBo = new BaseBO();
            string str_sql = "select count(*) from InvoiceDetail A,InvoiceHeader B where A.InvID = B.InvID and B.ContractID = " + contractID +
                             " and A.Period = '" + balanceMonth + "' and A.RentType in (" + InvoiceDetail.RENTTYPE_FIXED_P + "," + InvoiceDetail.RENTTYPE_SINGLE_P + " ," + InvoiceDetail.RENTTYPE_MUNCH_P + ")";
            DataSet ds = baseBo.QueryDataSet(str_sql);
            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                conFormulaP = true;
            else
                conFormulaP = false;
            return conFormulaP;

        }

        /// <summary>
        /// 获取非首期时费用开始计算日期
        /// </summary>
        /// <param name="conID">合同号</param>
        /// <param name="chgeTypeID">费用类型</param>
        /// <param name="chgeTypeID">表名</param>
        /// <returns></returns>
        private static DateTime GetLastAccountDate(int conID, int chgeTypeID,string table)
        {
            DateTime lastAccountDate = Convert.ToDateTime("9999-12-31");
            string sql = "select max(A.InvEndDate) as invEndDate from " + table + " A , invoiceheader B " +
                            " where A.invid = B.invid and B.Contractid = " + conID +
                            " and A.ChargeTypeID = " + chgeTypeID;
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lastAccountDate = (ds.Tables[0].Rows[0]["invEndDate"] == DBNull.Value ? Convert.ToDateTime("9999-12-31") : Convert.ToDateTime(ds.Tables[0].Rows[0]["invEndDate"]).AddDays(1));
            }
            return lastAccountDate;
        }

        /// <summary>
        /// 检查费用类别是否属于计表费用或其它费用
        /// </summary>
        /// <param name="chgTypeID">费用类别ID</param>
        /// <returns></returns>
        private static bool CheckChargeType(int chgTypeID)
        {
            bool isChargeType = false;
            string str_sql = "select ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber from ChargeType " +
                             " where ChargeTypeID = " + chgTypeID + " and ChargeClass in( " + ChargeType.CHARGECLASS_WATERORDLECT + "," + ChargeType.CHARGECLASS_OTHER + ")";
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows.Count > 0)
                isChargeType = true;
            else
                isChargeType = false;
            return isChargeType;
        }

        /// <summary>
        /// 检查费用类别是否属于联营内卡手续费
        /// </summary>
        /// <param name="chgTypeID">费用类别ID</param>
        /// <returns></returns>
        private static bool CheckIsInnerCardRate(int chgTypeID)
        {
            bool isInnerCardRate = false;
            string str_sql = "select ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber from ChargeType " +
                             " where ChargeTypeID = " + chgTypeID + " and ChargeClass = " + ChargeType.CHARGECLASS_UNION_INNERCARDRATE;
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows.Count > 0)
                isInnerCardRate = true;
            else
                isInnerCardRate = false;
            return isInnerCardRate;
        }

        /// <summary>
        /// 检查费用类别是否属于联营外卡手续费
        /// </summary>
        /// <param name="chgTypeID">费用类别ID</param>
        /// <returns></returns>
        private static bool CheckIsOutCardRate(int chgTypeID)
        {
            bool isOutCardRate = false;
            string str_sql = "select ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber from ChargeType " +
                             " where ChargeTypeID = " + chgTypeID + " and ChargeClass = " + ChargeType.CHARGECLASS_UNION_OUTERCARDRATE;
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows.Count > 0)
                isOutCardRate = true;
            else
                isOutCardRate = false;
            return isOutCardRate;
        }

        /// <summary>
        /// 计算银行内卡刷卡手续费
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="typeID">费用类型</param>
        private static void AccountInCardRate(int contractID,DateTime startDate,DateTime endDate,int typeID)
        {
            decimal InSaleAmt = GetInCardSaleAmt(contractID, startDate, endDate);
            decimal inCardRate = InSaleAmt * 0.01m;

            InvoiceDetail invoiceDet = new InvoiceDetail();
            invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
            invoiceDet.ChargeTypeID = typeID; //费用类别ID
            invoiceDet.InvID = invId;  //结算单ID
            invoiceDet.Period = startDate.AddDays(-startDate.Day + 1);//费用记账月
            invoiceDet.InvStartDate = startDate;  //费用开始日期
            invoiceDet.InvEndDate = endDate;      //费用结束日期
            invoiceDet.InvCurTypeID = curTypeID;  //结算币种
            invoiceDet.InvExRate = exRate;  //结算汇率
            invoiceDet.InvPayAmt = inCardRate;  //费用应结金额
            invoiceDet.InvPayAmtL = inCardRate / exRate;  //费用应结本币金额 = 费用应结金额 / 结算汇率
            invoiceDet.InvActPayAmt = inCardRate; //费用实际应结金额
            invoiceDet.InvActPayAmtL = inCardRate / exRate;  //费用实际应结本币金额
            invoiceDet.RentType = InvoiceDetail.RENTTYPE_NO_RENT;   //租金类别：非租金

            TotalAllMoney += inCardRate;
            tempMoney += inCardRate;
            aryList.Add(invoiceDet);
        }

        /// <summary>
        /// 获取银行内卡刷卡金额
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        private static decimal GetInCardSaleAmt(int contractID,DateTime startDate,DateTime endDate)
        {
            string str_sql = "SELECT ISNULL(SUM(TransSkuMedia.PaidAmt),0) as BankAmt" +
                                " FROM TransSkuMedia,ConShop,Contract" +
                                " WHERE ConShop.ContractID = Contract.ContractID" +
                                " and ConShop.shopid = TransSkuMedia.ShopID" +
                                " AND Contract.BizMode = 2" +
                                " AND bizdate >= '" + startDate + "'" +
                                " AND bizdate <= '" + endDate + "'" +
                                " AND TransSkuMedia.MediaMNo in (401,501,601,701)" +
                                " AND Contract.ContractID = " + contractID;

            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            decimal bankAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["BankAmt"]);
            return bankAmt;
        }

        /// <summary>
        /// 计算银行外卡刷卡手续费
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="typeID">费用类型</param>
        private static void AccountOutCardRate(int contractID, DateTime startDate, DateTime endDate, int typeID)
        {
            decimal outSaleAmt = GetOutCardSaleAmt(contractID, startDate, endDate);
            decimal outCardRate = outSaleAmt * 0.03m;

            InvoiceDetail invoiceDet = new InvoiceDetail();
            invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
            invoiceDet.ChargeTypeID = typeID; //费用类别ID
            invoiceDet.InvID = invId;  //结算单ID
            invoiceDet.Period = startDate.AddDays(-startDate.Day + 1);//费用记账月
            invoiceDet.InvStartDate = startDate;  //费用开始日期
            invoiceDet.InvEndDate = endDate;      //费用结束日期
            invoiceDet.InvCurTypeID = curTypeID;  //结算币种
            invoiceDet.InvExRate = exRate;  //结算汇率
            invoiceDet.InvPayAmt = outCardRate;  //费用应结金额
            invoiceDet.InvPayAmtL = outCardRate / exRate;  //费用应结本币金额 = 费用应结金额 / 结算汇率
            invoiceDet.InvActPayAmt = outCardRate; //费用实际应结金额
            invoiceDet.InvActPayAmtL = outCardRate / exRate;  //费用实际应结本币金额
            invoiceDet.RentType = InvoiceDetail.RENTTYPE_NO_RENT;   //租金类别：非租金;

            TotalAllMoney += outCardRate;
            tempMoney += outCardRate;
            aryList.Add(invoiceDet);
        }

        /// <summary>
        /// 获取银行外卡刷卡金额
        /// </summary>
        /// <param name="contractID">合同号</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        private static decimal GetOutCardSaleAmt(int contractID, DateTime startDate, DateTime endDate)
        {
            string str_sql = "SELECT ISNULL(SUM(TransSkuMedia.PaidAmt),0) as BankAmt" +
                                " FROM TransSkuMedia,ConShop,Contract" +
                                " WHERE ConShop.ContractID = Contract.ContractID" +
                                " and ConShop.shopid = TransSkuMedia.ShopID" +
                                " AND Contract.BizMode = 2" +
                                " AND bizdate >= '" + startDate + "'" +
                                " AND bizdate <= '" + endDate + "'" +
                                " AND TransSkuMedia.MediaMNo in (402,502,602,702)" +
                                " AND Contract.ContractID = " + contractID;

            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            decimal bankAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["BankAmt"]);
            return bankAmt;
        }

        /// <summary>
        /// 根据费用ID获取费用分类
        /// </summary>
        /// <param name="chgTypeID">费用ID</param>
        /// <returns></returns>
        private static int GetChargeClass(int chgTypeID)
        {
            string str_sql = "select ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber from ChargeType " +
                             " where ChargeTypeID = " + chgTypeID;
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return Convert.ToInt32(ds.Tables[0].Rows[0]["ChargeClass"]);
        }

        /// <summary>
        /// 计算比重天数
        /// </summary>
        /// <param name="AccountStartDate">费用计算开始日期</param>
        /// <param name="AccountEndDate">费用计算结束日期</param>
        /// <param name="rateType">日租或月租</param>
        /// <returns></returns>
        private static float GetAccountDays(DateTime accountStartDate, DateTime accountEndDate, string rateType)
        {
            float settleDays = 0;
            int days = GetMonthDay(accountStartDate); //获取该月的天数
            //int days = 30;   //此处为益田客户暂时修改

            TimeSpan ts = accountEndDate - accountStartDate;
            double conFormulaDays = ts.TotalDays + 1;

            if (rateType == ConFormulaH.RATETYPE_TYPE_MONTH)  //月租金
            {
                if (accountStartDate.AddDays(days - 1) == accountEndDate)  //整月
                {
                    settleDays = days / days;
                }
                else  //非整月
                {
                    if (monthSettleDays == 0)  //每月结算天数为自然月
                    {
                        settleDays = days;
                    }
                    else
                    {
                        settleDays = monthSettleDays;
                    }
                    settleDays = Convert.ToSingle(conFormulaDays) / settleDays;  //计算比重天数
                }
                rentType = InvoiceDetail.RENTTYPE_FIXED_MONTH;  //租金类别：固定月租金
            }
            else if (rateType == ConFormulaH.RATETYPE_TYPE_DAY)
            {
                settleDays = Convert.ToSingle(conFormulaDays);
                rentType = InvoiceDetail.RENTTYPE_FIXED_DAY;  //租金类别：固定日租金
            }

            return settleDays;
        }

        /// <summary>
        /// 判断费用类别是否为租金
        /// </summary>
        /// <param name="chargeTypeId">费用类别ID</param>
        /// <returns></returns>
        private static bool IsLease(int chargeTypeId)
        {
            bool leaseFlag;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ChargeTypeID = " + chargeTypeId + " and ChargeClass = " + ChargeType.CHARGECLASS_LEASE;
            Resultset rs = baseBo.Query(new ChargeType());
            if (rs.Count > 0)
                leaseFlag = true;
            else
                leaseFlag = false;
            return leaseFlag;
        }

        /// <summary>
        /// 判断费用类别是否为年终结算
        /// </summary>
        /// <param name="chargeTypeID">费用类别ID</param>
        /// <returns></returns>
        private static bool IsYearEndCount(int chargeTypeID)
        {
            bool yearEndCount;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ChargeTypeID = " + chargeTypeID + " and ChargeClass = " + ChargeType.CHARGECLASS_YEAREND;
            Resultset rs = baseBo.Query(new ChargeType());
            if (rs.Count > 0)
                yearEndCount = true;
            else
                yearEndCount = false;
            return yearEndCount;
        }

        /// <summary>
        /// 获取年终结算时间段
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="conStartDate">合同开始时间</param>
        /// <param name="conEndDate">合同结束时间</param>
        /// <param name="stopDate">终止日期</param>
        /// <param name="balanceMonth">费用记帐月</param>
        private static void YearEndCount(int contractID, DateTime conStartDate, DateTime conEndDate, DateTime stopDate)
        {
            DataSet ds = GetYearEndCountFormula(contractID);
            //获取已生成的年终结算记录数
            string str_sql = "select Count(distinct(A.InvID)) from InvoiceHeader A " +
                             " inner join InvoiceDetail B on A.InvID = B.InvID " +
                             " inner join ChargeType C on B.ChargeTypeID = C.ChargeTypeID " +
                             " where C.ChargeClass = " + ChargeType.CHARGECLASS_YEAREND +
                             " and A.ContractID = " + contractID;

            BaseBO baseBO = new BaseBO();
            DataSet rs = baseBO.QueryDataSet(str_sql);
            int yearEndCount = Convert.ToInt32(rs.Tables[0].Rows[0][0]);
            int count = ds.Tables[0].Rows.Count;
            if (count > yearEndCount)
            {
                for (int i = 0; i < count; i++)
                {
                    DateTime accountStartTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["FStartDate"]);
                    DateTime accountEndTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["FEndDate"]);

                    accountEndTime = Convert.ToDateTime(ds.Tables[0].Rows[yearEndCount]["FEndDate"]);

                    //存在合同终止情况，总年终结算结束时间与合同终止时间比较，再取总年终结算结束时间
                    if (stopDate > Convert.ToDateTime("1900-01-01"))
                    {
                        if (stopDate <= conEndDate)   //终止日期 <= 合同结束日期
                        {
                            if (stopDate < accountEndTime) //终止日期 < 年终结算结束时间, 取终止日期
                            {
                                accountEndTime = stopDate;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }

                    //结算公式结束日期与系统时间比较，如果系统时间 >= 公式结束日期，则进行年终结算
                    if (DateTime.Now >= accountEndTime)
                    {

                        //判断公式开始、结束日期是否在合同时间段范围内

                        //完全在合同范围内
                        if ((accountStartTime >= conStartDate) && (accountEndTime <= conEndDate))
                        {
                            if (stopDate > Convert.ToDateTime("1900-01-01"))  //存在终止日期
                            {
                                if (stopDate > accountEndTime)  //终止日期 > 结算结束日期
                                {
                                    GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, yearEndCount);
                                }
                                else if (stopDate < accountEndTime)
                                {
                                    accountEndTime = stopDate;
                                    GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, yearEndCount);
                                }
                            }
                            else
                            {
                                GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, yearEndCount);
                            }
                        }
                        else if ((accountStartTime > conStartDate) && (accountEndTime > conEndDate))  //结算开始日期 > 费用开始日期 and 结算结束日期 > 合同结束日期
                        {
                            accountEndTime = conEndDate;
                            if (stopDate > Convert.ToDateTime("1900-01-01"))
                            {
                                if (stopDate < accountStartTime)  //终止日期 < 结算开始日期
                                {
                                    return; //合同终止
                                }
                                else if (stopDate < accountEndTime)  //终止日期 < 结算结束日期
                                {
                                    accountEndTime = stopDate;  //结算结束日期 = 终止日期
                                    GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, yearEndCount);
                                }
                            }
                            else
                            {
                                GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, yearEndCount);
                            }
                        }
                        else if ((accountStartTime < conStartDate) && (accountEndTime > conEndDate)) //结算开始日期 < 费用开始日期 and 结算结束日期 > 合同结束日期
                        {
                            accountStartTime = conStartDate;
                            accountEndTime = conEndDate;
                            if (stopDate.Date > Convert.ToDateTime("1900-01-01"))
                            {
                                if (stopDate > accountEndTime)  //终止日期 > 结算结束日期
                                {
                                    accountEndTime = stopDate;  //结算结束日期 = 终止日期
                                    GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, yearEndCount);
                                }
                            }
                            else
                            {
                                GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, yearEndCount);
                            }
                        }
                        else  //时间段完全不在合同范围内
                        {
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 计算年终结算金额
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="stratDate">结算开始时间</param>
        /// <param name="endDate">结算结束时间</param>
        /// <param name="ds">年终结算公式DS</param>
        /// <param name="ds">已经计算过年终结算公式的记录数</param>
        private static void GetYearEndCount(int contractID, DateTime stratDate, DateTime endDate, DataSet ds, int yearEndCount)
        {
            //公式类别为抽成保底
            if (ds.Tables[0].Rows[yearEndCount]["FormulaType"].ToString() == ConFormulaH.FORMULATYPE_TYPE_TWO)
            {
                //年终结算金额
                decimal yearEndMoney = 0;

                //获取销售额
                decimal paidAmt = GetPaidAmt(contractID, stratDate, endDate);
                paidAmt = paidAmt / exRate;
                int formulID = Convert.ToInt32(ds.Tables[0].Rows[yearEndCount]["FormulaID"]);
                float settleDays = 1;
                /*************************取抽成*************************/

                //抽成方式
                string pcentOpt = ds.Tables[0].Rows[yearEndCount]["PcentOpt"].ToString();
                //抽成率
                decimal pcent = 0;  
                //获取抽成租金
                decimal salesToP = GetPMomey(paidAmt, settleDays, formulID, pcentOpt,out pcent);

                //保底方式
                string minSumOpt = ds.Tables[0].Rows[yearEndCount]["MinSumOpt"].ToString();

                //取公式保底金额(结算金额)
                decimal countMinMoney = GetMinMoney(paidAmt, 0, settleDays, formulID, minSumOpt);

                //抽成大于保底额
                if (salesToP > countMinMoney)
                {
                    yearEndMoney = salesToP - countMinMoney;
                    InvoiceJVDetail invoiceJVDet = new InvoiceJVDetail();
                    invoiceJVDet.invJVDetailID = BaseApp.GetinvJVDetailID();
                    invoiceJVDet.InvID = invId;
                    invoiceJVDet.RentType = rentType;
                    invoiceJVDet.Period = stratDate.AddDays(-stratDate.Day + 1);
                    invoiceJVDet.InvStartDate = stratDate;
                    invoiceJVDet.InvEndDate = endDate;
                    invoiceJVDet.InvCurTypeID = curTypeID;
                    invoiceJVDet.InvExRate = exRate;
                    invoiceJVDet.invSalesAmt = paidAmt * exRate;
                    invoiceJVDet.invSalesAmtL = paidAmt;
                    invoiceJVDet.invPcent = pcent;
                    invoiceJVDet.InTaxRate = inTaxRate;
                    invoiceJVDet.OutTaxRate = outTaxRate;
                    invoiceJVDet.InvPayAmt = yearEndMoney;
                    invoiceJVDet.InvPayAmtL = yearEndMoney / exRate;
                    invoiceJVDet.invJVCostAmt = invoiceJVDet.invSalesAmt - invoiceJVDet.InvPayAmt;
                    invoiceJVDet.invJVCostAmtL = invoiceJVDet.invSalesAmtL - invoiceJVDet.InvPayAmtL;
                    invoiceJVDet.InvAdjAmt = 0;
                    invoiceJVDet.InvAdjAmtL = 0;
                    invoiceJVDet.InvActPayAmt = invoiceJVDet.invJVCostAmt;
                    invoiceJVDet.invJVCostAmtL = invoiceJVDet.invJVCostAmtL;
                    invoiceJVDet.InvPaidAmt = 0;
                    invoiceJVDet.InvPaidAmtL = 0;
                    invoiceJVDet.InvDetStatus = InvoiceJVDetail.INVDETSTATUS_AVAILABILITY;
                    invoiceJVDet.Note = "";
                    invoiceJVDet.ChargeTypeID = Convert.ToInt32(ds.Tables[0].Rows[yearEndCount]["ChargeTypeID"]); //费用类别ID
                    PMAryList.Add(invoiceJVDet);
                }
            }
        }

        /// <summary>
        /// 获取年终结算公式
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <returns></returns>
        private static DataSet GetYearEndCountFormula(int contractID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select A.FormulaID,A.ChargeTypeID,A.ContractID,A.FStartDate,A.FEndDate,A.FormulaType,A.RateType,A.PcentOpt,A.MinSumOpt,A.TotalArea," +
                             "A.FixedRental,A.UnitPrice,A.BaseAmt from ConFormulaH A " +
                             " inner join ChargeType B on A.ChargeTypeID = B.ChargeTypeID " +
                             " where A.ContractID = " + contractID + " and B.ChargeClass = " + ChargeType.CHARGECLASS_YEAREND + " order by A.FendDate asc";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 判断是否为一次性费用
        /// </summary>
        /// <param name="conID">合同号</param>
        /// <param name="chargeTypeID">费用类别ID</param>
        /// <returns></returns>
        private static int IsOneCharge(int conID, int chargeTypeID,DateTime startDate,DateTime endDate)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select count(ChargeTypeID) from ConFormulaH where ChargeTypeID = " + chargeTypeID +
                             " and ContractID = " + conID + " and FormulaType = '" + ConFormulaH.FORMULATYPE_TYPE_THREE + 
                            "' and FStartDate >= '" + startDate + "' and FEndDate <= '" + endDate + "'";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            int count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            return count;
        }

        /// <summary>
        /// 检查是否生成一次性费用
        /// </summary>
        /// <param name="conID">合同号</param>
        /// <param name="chargeDate">费用开始时间</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="chargeTypeID">费用类型ID</param>
        /// <returns></returns>
        private static int CheckIsOneChargeYes(int conID,DateTime chargeDate,DateTime startDate, DateTime endDate, int chargeTypeID)
        {
            BaseBO baseBO = new BaseBO();
            int result = 0;
            //string str_sql = "select FormulaID,ChargeTypeID,ContractID,FStartDate,FEndDate,FormulaType,RateType,PcentOpt,MinSumOpt,TotalArea," +
            //                 "FixedRental,UnitPrice,BaseAmt from ConFormulaH " +
            //                 " where ContractID = " + conID + 
            //                 " and ChargeTypeID = " + chargeTypeID +
            //                 " and FormulaType = '" + ConFormulaH.FORMULATYPE_TYPE_THREE + "'";
            //DataSet ds = baseBO.QueryDataSet(str_sql);
            //int count = ds.Tables[0].Rows.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    if (startDate < chargeDate)
            //    {
            //        startDate = chargeDate;
            //    }
            //    if (startDate >= Convert.ToDateTime(ds.Tables[0].Rows[i]["FStartDate"]) && startDate <= Convert.ToDateTime(ds.Tables[0].Rows[i]["FEndDate"]))
            //    {
            //        result = 0;
            //    }
            //    else
            //    {
            //        result = 1;
            //    }


                string sql = "select Count(InvDetailID) from InvoiceDetail A,InvoiceHeader B where A.InvID = B.InvID " + 
                             " and A.ChargeTypeID = " + chargeTypeID + " and B.ContractID = " + conID +
                             " and A.InvStartDate <= '" + startDate + 
                             "' and A.InvEndDate >= '" + startDate + "'";

                DataSet tempDS = baseBO.QueryDataSet(sql);
                if (Convert.ToInt32(tempDS.Tables[0].Rows[0][0]) > 0)
                {
                    result = 0;
                }
                else
                {
                    result = 1;
                }



            //}
            return result;
        }

        /// <summary>
        /// 获取客户名
        /// </summary>
        /// <param name="conID">合同ID</param>
        /// <returns></returns>
        private static string GetCustName(int conID)
        {
            string str_sql = "select CustName from Customer A , Contract B " +
                             " where A.CustID = B.CustID and B.ContractID = " + conID;
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds.Tables[0].Rows[0]["CustName"].ToString();
        }

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
