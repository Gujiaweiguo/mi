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
using Base.Page;
using Lease.PotBargain;

namespace Invoice
{
    /// <summary>
    /// 费用计算
    /// </summary>

    public class ChargeAccount
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
        public static ArrayList blankAryList;  //存空白记录;
        public static int curTypeID;  //结算币种
        public static Decimal exRate;  //汇率
        public static decimal tempMoney;  //用于非租金
        public static int tempFlag;
        public static int accountPFlag;   //计算抽成标志
        public static DateTime stopDate;   //合同结束日期
        public static ArrayList invCodeAryList; //水电费结算单号
        public static ArrayList otherInvCodeAryList;  //其他费用结算单号
        public static ArrayList interestIDAryList;    //滞纳金单号
        public static int invType;    //合同类型
                
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
        /// <param name="balanceMonth">计帐月</param>
        /// <param name="isFirst">是否首期; 0:否 ; 1 :是</param>
        /// <param name="chargeType">费用类型</param>
        /// <param name="Htb">操作员信息</param>
        /// <param name="bancthID">批次号</param>
        /// <param name="invCode">结算单代码</param>
        /// <param name="bfoChgAryList">未生成前期费用的费用类型ID</param>
        public static int AccountCharge(int contractID, DateTime balanceMonth, int isFirst,string chargeType,Hashtable Htb,string bancthID,out int invCode,out ArrayList bfoChgAryList)
        {
            //DateTime invHPMoth;
            DateTime blceMoth = balanceMonth;
            tempFlag = 0;
            tempMoney = 0;
            blankFlag = 0;
            TotalAllMoney = 0;
            accountPFlag = 0;
            invType = 0;
            invId = 0;　　//结算单ID
            aryList = new ArrayList();
            blankAryList = new ArrayList();
            bfoChgAryList = new ArrayList();
            invCodeAryList = new ArrayList();
            otherInvCodeAryList = new ArrayList();
            interestIDAryList = new ArrayList();

            //取结算币种
            Resultset curyRs = GetConLease(contractID);
            ConLease conLse = curyRs.Dequeue() as ConLease;
            curTypeID = conLse.CurTypeID;
            //取结算币种对应的汇率
            exRate = GetCurExRate(curTypeID);
            if (exRate == 0)
            {
                invCode = invId;
                return PROMT_EXRATE_NO;
            }

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
                invType = contract.BizMode;
                custID = contract.CustID;
                //判断合同是否有效
                if (contract.ContractStatus == Contract.CONTRACTSTATUS_TYPE_INGEAR || contract.ContractStatus == Contract.CONTRACTSTATUS_TYPE_END)
                {
                    //合同有效
                    chgeType = chargeType.Substring(0, chargeType.Length - 1);
                    
                        //取费用类型;
                    string[] ctx = Regex.Split(chgeType, ",");
                        int cTCountx = ctx.Length - 1;
                        //合同终止日期
                        if (contract.StopDate == Convert.ToDateTime("0001-01-01"))
                        {
                            stopDate = Convert.ToDateTime("1900-01-01");
                        }
                        else
                        {
                            stopDate = contract.StopDate;
                        }


                        //取合同相关条款
                        int billCyle = 0;   //结算周期
                        int settleMode = 0; //结算处理方式
                        Resultset conLeaseRs = GetConLease(contractID);
                        if (conLeaseRs.Count == 1)
                        {
                            ConLease conLease = conLeaseRs.Dequeue() as ConLease;
                            billCyle = conLease.BillCycle;   //结算周期
                            settleMode = conLease.SettleMode;  //结算处理方式
                            ifPrepay = conLease.IfPrepay;     //是否预收保底
                            monthSettleDays = conLease.MonthSettleDays;  //月结天数
                        }

                        //计算其它费用和记表费用
                        for (int m = 0; m <= cTCountx; m++)
                        {
                            bool isChargeType = CheckChargeType(Convert.ToInt32(ctx[m]));
                            if (isChargeType == true)
                            {
                                //for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                //{
                                    DateTime baceMon = balanceMonth.AddMonths(0);
                                    //计算非租金费用
                                    tempFlag = CountNoLeaseCharge(contractID, Convert.ToInt32(ctx[m]), baceMon);
                                //}
                            }
                        }
                        //判断是否首期
                        if (isFirst == InvoiceHeader.ISFIRST_YES)
                        {
                            //费用类型;
                            for (int x = 0; x <= cTCountx; x++)
                            {

                                balanceMonth = blceMoth;
                                //检查首期费用是否已生成
                                Resultset invHeaRs = CheckIsFist(contract.ContractID, InvoiceHeader.ISFIRST_YES);  //是否已生成首期费用
                                if (invHeaRs.Count > 0)
                                {
                                    invCode = 0;
                                    return PROMT_FIRST_CHARGE_YES;
                                }
                                else
                                {
                                    isFist = InvoiceHeader.ISFIRST_YES;   //赋值为首期费用
                                    bool yearEnd = IsYearEndCount(Convert.ToInt32(ctx[x]));
                                    if (yearEnd == false)
                                    {
                                        //计算一次性费用
                                        int oneResult = CountOneCharge(contractID, balanceMonth, Convert.ToInt32(ctx[x]), contract.ChargeStartDate, contract.ConEndDate, isFist, billCyle);

                                        if (oneResult == 0)  //不是一次性费用
                                        {
                                            //是首期,判断是否从1号开始
                                            if (contract.ChargeStartDate.Day == 1)
                                            {
                                                accountStartDateTime = contract.ChargeStartDate;
                                                accountEndDateTime = accountStartDateTime.AddMonths(billCyle).AddDays(-1);
                                                for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                                {
                                                    blankFlag = 0;
                                                    //应结时间段
                                                    if (week == 0)
                                                    {
                                                        accountStartDateTime = contract.ChargeStartDate;
                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                    }
                                                    else
                                                    {
                                                        accountStartDateTime = contract.ChargeStartDate.AddMonths(week);
                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                        balanceMonth = balanceMonth.AddMonths(1);
                                                    }
                                                    //结算时段是否在合同范围内
                                                    int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, balanceMonth, Convert.ToInt32(ctx[x]));
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
                                                }
                                            }
                                            else
                                            {
                                                if (settleMode == ConLease.SETTLEMODE_TYPE_FIRST) //首月对齐
                                                {
                                                    for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                                    {
                                                        blankFlag = 0;
                                                        //应结时间段
                                                        if (week == 0)
                                                        {
                                                            accountStartDateTime = contract.ChargeStartDate;
                                                            if (accountStartDateTime.Day == 31 && (accountStartDateTime.AddMonths(1).Day == 30 || accountStartDateTime.AddMonths(1).Day == 29 || accountStartDateTime.AddMonths(1).Day == 28))
                                                            {
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.AddMonths(1).Day);
                                                            }
                                                            else
                                                            {
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (contract.ChargeStartDate.Day == 31)
                                                            {
                                                                accountStartDateTime = contract.ChargeStartDate.AddMonths(week).AddDays(-contract.ChargeStartDate.AddMonths(week).Day + 1);
                                                            }
                                                            else
                                                            {
                                                                accountStartDateTime = contract.ChargeStartDate.AddMonths(week).AddDays(-contract.ChargeStartDate.Day + 1);
                                                            }
                                                            accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                            balanceMonth = balanceMonth.AddMonths(1);
                                                        }
                                                        //结算时段是否在合同范围内
                                                        int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, balanceMonth, Convert.ToInt32(ctx[x]));
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
                                                    }
                                                }
                                                if (settleMode == ConLease.SETTLEMODE_TYPE_LAST) //次月对齐
                                                {
                                                    for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                                    {
                                                        blankFlag = 0;
                                                        //应结时间段
                                                        if (week == 0)
                                                        {
                                                            accountStartDateTime = contract.ChargeStartDate;
                                                            if (accountStartDateTime.Day == 31 && (accountStartDateTime.AddMonths(1).Day == 30 || accountStartDateTime.AddMonths(1).Day == 29 || accountStartDateTime.AddMonths(1).Day == 28))
                                                            {
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1);
                                                            }
                                                            else
                                                            {
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                            }
                                                        }
                                                        else if (week == 1)
                                                        {
                                                            accountStartDateTime = contract.ChargeStartDate.AddMonths(1);
                                                            if (contract.ChargeStartDate.Day == 31 && (contract.ChargeStartDate.AddMonths(week).Day == 30 || contract.ChargeStartDate.AddMonths(week).Day == 29 || contract.ChargeStartDate.AddMonths(week).Day == 28))
                                                            {
                                                                accountStartDateTime = contract.ChargeStartDate.AddMonths(1).AddDays(1);
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                            }
                                                            else
                                                            {
                                                                if (contract.ChargeStartDate.Day == 31 && contract.ChargeStartDate.AddMonths(week).Day == 31)
                                                                {
                                                                    accountEndDateTime = accountStartDateTime.AddMonths(1);
                                                                }
                                                                else
                                                                {
                                                                    accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                                }
                                                            }
                                                            balanceMonth = balanceMonth.AddMonths(1);
                                                        }
                                                        else
                                                        {
                                                            if (contract.ChargeStartDate.Day == 31)
                                                            {
                                                                accountStartDateTime = contract.ChargeStartDate.AddMonths(week + 1).AddDays(-contract.ChargeStartDate.AddMonths(week + 1).Day + 1);
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                            }
                                                            else
                                                            {
                                                                accountStartDateTime = contract.ChargeStartDate.AddMonths(week).AddDays(-contract.ChargeStartDate.AddMonths(week).Day + 1);
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                            }
                                                            balanceMonth = balanceMonth.AddMonths(1);
                                                        }
                                                        //结算时段是否在合同范围内
                                                        int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, balanceMonth, Convert.ToInt32(ctx[x]));
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
                                                    }
                                                    balanceMonth = blceMoth;
                                                }
                                                if (settleMode == ConLease.SETTLEMODE_TYPE_NO) //不做调整
                                                {
                                                    for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                                    {
                                                        blankFlag = 0;
                                                        //应结时间段
                                                        if (week == 0)
                                                        {
                                                            accountStartDateTime = contract.ChargeStartDate;
                                                            accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                            if (accountStartDateTime.Day == 31 && (accountStartDateTime.AddMonths(1).Day == 30 || accountStartDateTime.AddMonths(1).Day == 29 || accountStartDateTime.AddMonths(1).Day == 28))
                                                            {
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (contract.ChargeStartDate.Day == 31)
                                                            {
                                                                accountStartDateTime = contract.ChargeStartDate.AddMonths(week + 1).AddDays(-contract.ChargeStartDate.AddMonths(week + 1).Day + 1);
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                            }
                                                            else
                                                            {
                                                                accountStartDateTime = contract.ChargeStartDate.AddMonths(week);
                                                                accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                            }
                                                            balanceMonth = balanceMonth.AddMonths(1);
                                                        }
                                                        //结算时段是否在合同范围内
                                                        int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, balanceMonth, Convert.ToInt32(ctx[x]));
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
                                                    }
                                                    balanceMonth = blceMoth;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int p = 0; p <= 1; p++)
                            {
                                //DateTime periodDate;
                                string chargeTypeValue = "";
                                if (p == 0)
                                {
                                    chargeTypeValue = ChargeAccount.GetRentID(chargeType);
                                    if (chargeTypeValue == "")
                                    {
                                        chargeTypeValue = chargeType;
                                        balanceMonth = blceMoth;
                                        p = 1;
                                    }
                                    else
                                    {
                                        balanceMonth = blceMoth.AddMonths(-1);
                                    }
                                }
                                else
                                {
                                    chargeTypeValue = chargeType;
                                    balanceMonth = blceMoth;
                                }
                                chgeType = chargeTypeValue.Substring(0, chargeTypeValue.Length - 1);
                                //取费用类型;
                                string[] ct = Regex.Split(chgeType, ",");
                                int cTCount = ct.Length - 1;
                                isFist = InvoiceHeader.ISFIRST_NO;   //赋值为非首期费用
                                BaseBO basebo = new BaseBO();
                                ////判断该月非抽成费用是否已生成
                                //string temp_sql = "select Count(*) from InvoiceDetail A,InvoiceHeader B where A.InvID = B.InvID and B.ContractID = " + contractID +
                                //                    " and A.Period = '" + balanceMonth + "' and A.RentType != " + InvoiceDetail.RENTTYPE_FIXED_P +
                                //                    " and A.RentType != " + InvoiceDetail.RENTTYPE_SINGLE_P +
                                //                    " and A.RentType != " + InvoiceDetail.RENTTYPE_MUNCH_P;
                                //判断该月租金费用是否已生成
                                string temp_sql = "select Count(InvDetailID) from InvoiceDetail A,InvoiceHeader B where A.InvID = B.InvID and B.ContractID = " + contractID +
                                                    " and A.Period = '" + balanceMonth +
                                                    "' and A.ChargeTypeID in (select ChargeTypeID from ChargeType where ChargeClass = " + ChargeType.CHARGECLASS_LEASE + ")";
                                DataSet tempRS = basebo.QueryDataSet(temp_sql);
                                if (Convert.ToInt32(tempRS.Tables[0].Rows[0][0]) > 0)
                                {
                                    //是否存在正常抽成记录
                                    bool formulaP = IsExistConFormulaP(contractID, balanceMonth);
                                    if (formulaP == true)
                                    {
                                        invCode = 0;
                                        //return PROMT_MONTH_CHARGE_YES;
                                    }
                                    else
                                    {
                                        for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                        {
                                            DateTime tempBalanceMonth = balanceMonth.AddMonths(week);
                                            DataSet blankDS = GetBlankRecord(contractID, tempBalanceMonth);  //获取空白抽成记录
                                            int qCount = blankDS.Tables[0].Rows.Count;
                                            if (qCount > 0)   //存在空白记录
                                            {
                                                for (int q = 0; q < qCount; q++)
                                                {
                                                    accountStartDateTime = Convert.ToDateTime(blankDS.Tables[0].Rows[q]["InvStartDate"]);
                                                    accountEndDateTime = Convert.ToDateTime(blankDS.Tables[0].Rows[q]["InvEndDate"]);
                                                    if (DateTime.Now > Convert.ToDateTime(accountEndDateTime.ToShortDateString() + " 23:59:59"))
                                                    {
                                                        for (int u = 0; u <= cTCount; u++)
                                                        {
                                                            bool leaseFlag = IsLease(Convert.ToInt32(ct[u]));
                                                            if (leaseFlag == true)
                                                            {
                                                                accountPFlag = 1;
                                                                int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, tempBalanceMonth, Convert.ToInt32(ct[u]));
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
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else if (aryList.Count <= 0)
                                            {
                                                if (tempFlag == 0)
                                                {
                                                    invCode = 0;
                                                    //return PROMT_MONTH_CHARGE_YES;
                                                }
                                            }
                                        }
                                    }
                                }
                                //else
                                //{
                                //检查首期费用是否已生成
                                Resultset invHeaRs = CheckIsFist(contract.ContractID, InvoiceHeader.ISFIRST_YES);  //是否已生成首期费用
                                if (invHeaRs.Count > 0)  //首期费用已生成
                                {
                                    DateTime xMonth = balanceMonth;
                                    //费用类型;
                                    for (int m = 0; m <= cTCount; m++)
                                    {
                                        //判断是否为滞纳金
                                        bool isInterest = IsInterest(Convert.ToInt32(ct[m]));
                                        if (isInterest == true)
                                        {
                                            tempFlag = SetInterestPrint(contractID);
                                            if (m < cTCount)
                                            {
                                                m = m + 1;
                                            }
                                        }
                                        //判断是否为年终结算
                                        bool yearEnd = IsYearEndCount(Convert.ToInt32(ct[m]));
                                        if (yearEnd == true)
                                        {
                                            YearEndCount(contractID, contract.ChargeStartDate, contract.ConEndDate, contract.StopDate, balanceMonth);

                                        }
                                        else
                                        {

                                            //检查每个费用在该月是否已生成
                                            string temp_sql1 = "select Count(InvDetailID) from InvoiceDetail A,InvoiceHeader B where A.InvID = B.InvID and B.ContractID = " + contractID +
                                                    " and A.Period = '" + xMonth +
                                                    "' and A.ChargeTypeID = " + Convert.ToInt32(ct[m]);
                                            DataSet xDS = basebo.QueryDataSet(temp_sql1);
                                            if (Convert.ToInt32(xDS.Tables[0].Rows[0][0]) <= 0)
                                            {
                                                //计算一次性费用
                                                int oneResult = CountOneCharge(contractID, balanceMonth, Convert.ToInt32(ct[m]), contract.ChargeStartDate, contract.ConEndDate, isFist, billCyle);

                                                if (oneResult == 0)  //不是一次性费用
                                                {
                                                    accountPFlag = 0;
                                                    balanceMonth = blceMoth;
                                                    int isBeginCharge = 1;
                                                    string bfSql = "select Count(ChargeTypeID) from ChargeType where ChargeClass in (" + ChargeType.CHARGECLASS_LEASE + "," + ChargeType.CHARGECLASS_FANST + ") and ChargeTypeID = " + Convert.ToInt32(ct[m]);
                                                    DataSet bfDS = basebo.QueryDataSet(bfSql);
                                                    if (Convert.ToInt32(bfDS.Tables[0].Rows[0][0]) > 0)
                                                    {
                                                        isBeginCharge = CheckBeginCharge(contractID, balanceMonth.AddMonths(-1), Convert.ToInt32(ct[m]));   //检查前期费用是否已生成  1:已生成; 0:未生成
                                                    }
                                                    if (isBeginCharge == 1)  //前期费用已生成
                                                    {
                                                        //取费用开始计算日期(上次结算结束日期加一;每种费用类型计算的结束日期)
                                                        DateTime lastAcntDate = GetLastAccountDate(contractID, balanceMonth, Convert.ToInt32(ct[m]));
                                                        if (lastAcntDate != Convert.ToDateTime("9999-12-31"))
                                                        {
                                                            if (settleMode == ConLease.SETTLEMODE_TYPE_FIRST) //首月对齐
                                                            {
                                                                for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                                                {
                                                                    blankFlag = 0;
                                                                    //应结时间段
                                                                    if (week == 0)
                                                                    {
                                                                        accountStartDateTime = lastAcntDate;
                                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                                    }
                                                                    else
                                                                    {
                                                                        accountStartDateTime = lastAcntDate.AddMonths(week);
                                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                                        balanceMonth = balanceMonth.AddMonths(1);
                                                                    }
                                                                    //结算时段是否在合同范围内
                                                                    int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, balanceMonth, Convert.ToInt32(ct[m]));
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
                                                                }
                                                                balanceMonth = blceMoth;
                                                            }
                                                            if (settleMode == ConLease.SETTLEMODE_TYPE_LAST) //次月对齐
                                                            {
                                                                blankFlag = 0;
                                                                for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                                                {
                                                                    //应结时间段
                                                                    if (week == 0)
                                                                    {
                                                                        accountStartDateTime = lastAcntDate;
                                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                                    }
                                                                    else if (week == 1)
                                                                    {
                                                                        accountStartDateTime = lastAcntDate.AddMonths(1);
                                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                                        balanceMonth = balanceMonth.AddMonths(1);
                                                                    }
                                                                    else
                                                                    {
                                                                        accountStartDateTime = lastAcntDate.AddMonths(week);
                                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-accountStartDateTime.Day);
                                                                        balanceMonth = balanceMonth.AddMonths(1);
                                                                    }
                                                                    //结算时段是否在合同范围内
                                                                    int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, balanceMonth, Convert.ToInt32(ct[m]));
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
                                                                }
                                                                balanceMonth = blceMoth;
                                                            }
                                                            if (settleMode == ConLease.SETTLEMODE_TYPE_NO) //不做调整
                                                            {
                                                                for (week = 0; week < billCyle; week++)  //判断并循环完成所有周期
                                                                {
                                                                    blankFlag = 0;
                                                                    //应结时间段
                                                                    if (week == 0)
                                                                    {
                                                                        accountStartDateTime = lastAcntDate;
                                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                                    }
                                                                    else
                                                                    {
                                                                        accountStartDateTime = lastAcntDate.AddMonths(week);
                                                                        accountEndDateTime = accountStartDateTime.AddMonths(1).AddDays(-1);
                                                                        balanceMonth = balanceMonth.AddMonths(1);
                                                                    }
                                                                    //结算时段是否在合同范围内
                                                                    int result = IsDateTimeInContract(contractID, accountStartDateTime, accountEndDateTime, contract.ConStartDate, contract.ConEndDate, balanceMonth, Convert.ToInt32(ct[m]));
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
                                                                }
                                                                balanceMonth = blceMoth;
                                                            }
                                                        }
                                                    }
                                                    else //前期费用未生成
                                                    {
                                                        bool isChargeType = CheckChargeType(Convert.ToInt32(ct[m]));
                                                        if (isChargeType == false)
                                                        {
                                                            bfoChgAryList.Add(Convert.ToInt32(ct[m]));
                                                        }
                                                        //invCode = invId;
                                                        //return PROMT_BEFORE_CHARGE_NO;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else //首期费用未生成
                                {
                                    invCode = invId;
                                    return PROMT_FIRST_CHARGE_NO;
                                }
                            }
                            
                        }
                    //}
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
            invCode = InsertData(contractID, blceMoth, bancthID);
            return PROMT_SUCCED;
        }

        /// <summary>
        /// 检查是否已生成首期费用
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="isFirst">是否首期</param>
        /// <returns></returns>
        private static Resultset CheckIsFist(int contractID,int isFirst)
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
        /// <param name="invPeriod">结算单记账月</param>
        /// <param name="chgTypeID">费用类别ID</param>
        /// <returns>bulidStatus: 1:已生成; 0:未生成</returns>
        private static int CheckBeginCharge(int contractID,DateTime invPeriod,int chgTypeID)
        {
            int bulidStatus;
            BaseBO baseBo = new BaseBO();
            string sql = "select Count(*) as count from InvoiceDetail A,InvoiceHeader B where A.InvID = B.InvID and A.Period = '" +
                          invPeriod + "' and B.ContractID = " + contractID + " and A.ChargeTypeID = " + chgTypeID;
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
        private static int IsDateTimeInContract(int contractID, DateTime accountStartTime, DateTime accountEndTime, DateTime contractStartTime, DateTime contractEndTime, DateTime balanceMonth, int chargeTypeID)
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
                        GetFormulaHByDate(contractID, accountStartTime, accountEndTime, balanceMonth, chargeTypeID);
                        return 0;
                    }
                    else if (stopDate < accountEndTime)  //终止日期 < 结算结束日期
                    {
                        //终止日期在结算开始日期与结算结束日期之间，则结算日期取终止日期
                        accountEndTime = stopDate;
                        GetFormulaHByDate(contractID, accountStartTime, accountEndTime, balanceMonth, chargeTypeID);
                        return 0;
                    }
                }
                else
                {
                    //根据结算时间段获取结算公式
                    GetFormulaHByDate(contractID, accountStartTime, accountEndTime, balanceMonth, chargeTypeID);
                    return 0;
                }
                return 0;
            }
            else if ((accountStartTime >= contractStartTime) && (accountEndTime >= contractEndTime))  //结算开始日期 > 费用开始日期 and 结算结束日期 > 合同结束日期
            {
                accountEndTime = contractEndTime;
                if (accountStartTime <= accountEndTime)
                {
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
                            GetFormulaHByDate(contractID, accountStartTime, accountEndTime, balanceMonth, chargeTypeID);
                            return 0;
                        }
                    }
                    else
                    {
                        GetFormulaHByDate(contractID, accountStartTime, accountEndTime, balanceMonth, chargeTypeID);
                        return 0;
                    }
                    return 0;
                }
                else
                {
                    return PROMT_CONTRACT_STOP; //合同终止
                }
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
                        GetFormulaHByDate(contractID, accountStartTime, accountEndTime, balanceMonth, chargeTypeID);
                        return 0;
                    }
                }
                else
                {
                    GetFormulaHByDate(contractID, accountStartTime, accountEndTime, balanceMonth, chargeTypeID);
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
        private static void GetFormulaHByDate(int contractID, DateTime stratDate, DateTime endDate, DateTime balanceMonth,int chargeTypeID)
        {
            decimal TotalMoney = 0;     //总费用金额
            
            BaseBO baseBo = new BaseBO();

            DateTime computeStartDate = DateTime.Now;
            DateTime computeEndDate = DateTime.Now;
            
            //取费用类型;
            string[] ss = Regex.Split(chgeType, ",");
            int chgeTypeCount = ss.Length;
            //取计算公式
            Resultset formulHRs = GetFormulaH(contractID,chargeTypeID,stratDate, endDate);
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

                //判断是否需要计算此费用类型
                if (conFormulaH.ChargeTypeID == chargeTypeID)
                {
                    //if (conFormulaH.FormulaType == ConFormulaH.FORMULATYPE_TYPE_THREE)  //如果为一次性收取，则取公式日期
                    //{
                    //    computeStartDate = conFormulaH.FStartDate;
                    //    computeEndDate = conFormulaH.FEndDate;
                    //}
                    //else
                    //{
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
                    //}

                    
                    //DataSet conFormulaMds;
                    //DataSet conFormulaPds;
                    float settleDays = GetAccountDays(computeStartDate, computeEndDate, conFormulaH.RateType);
                    
                    if (conFormulaH.FormulaType == ConFormulaH.FORMULATYPE_TYPE_TWO)   //公式类别为抽成保底
                    {
                        /*既有抽成,也有保底;即比较销售额,如果销售大于保底,即取抽成设定,否则取保底设定*/
                        //获取销售额
                        //decimal paidAmt = GetPaidAmt(contractID, stratDate, endDate);
                        decimal paidAmt = GetPaidAmtByCon(contractID, computeStartDate, computeEndDate);
                        int formulID = conFormulaH.FormulaID;
                        /*************************取抽成*************************/

                        string pcentOpt = conFormulaH.PcentOpt;    //抽成方式
                        if (isFist != InvoiceHeader.ISFIRST_YES)
                        {
                            salesToP = GetPMomey(paidAmt, settleDays, formulID, pcentOpt);

                            //按商品抽成
                            DataSet tempDS = GetPaidAmtByMaster(contractID, computeStartDate, computeEndDate);
                            for (int x = 0; x < tempDS.Tables[0].Rows.Count; x++)
                            {
                                salesToP += GetPMomeyByMasterP(Convert.ToDecimal(tempDS.Tables[0].Rows[x][0]), settleDays, Convert.ToDecimal(tempDS.Tables[0].Rows[x][1]));
                            }

                        }

                        /***********************预收保底***************************/
                        string minSumOpt = conFormulaH.MinSumOpt;        //保底方式
                        if (ifPrepay == ConLease.IFPREPAY_TYPE_YES)  //预收保底
                        {
                            //bool ifExistMin = IsMinSum(balanceMonth);    //是否已生成保底
                            DataSet blankDS = GetBlankRecord(contractID,balanceMonth);  //是否存在空白记录
                            if (blankDS.Tables[0].Rows.Count > 0)    //存在空白记录
                            {
                                decimal countMinMoney = GetMinMoney(paidAmt, salesToP, settleDays, formulID, minSumOpt);  //取公式保底金额(结算金额)
                                countMinMoney = Math.Round(countMinMoney, 2);
                                minMoney = GetExistMinMoney(computeStartDate,computeEndDate,contractID);   //取已生成保底金额 
                                if (countMinMoney > minMoney)  //已收保底与结算金额
                                {
                                    minMoney = countMinMoney - minMoney; //结算金额，则保底 = 结算金额 - 保底金额
                                    rentType = GetRentType(pcentOpt);
                                }
                                else
                                {
                                    outFlag = 1;
                                }
                            }
                            else   //未生成保底
                            {
                                salesTo = GetMinMoney(paidAmt, 0, settleDays, formulID, minSumOpt);  //取公式保底金额
                                //if (blankFlag == 0)
                                //{
                                    //生成空白抽成记录
                                    InvoiceDetail invDetail = new InvoiceDetail();
                                    invDetail.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                                    invDetail.ChargeTypeID = conFormulaH.ChargeTypeID; //费用类别ID
                                    invDetail.InvID = invId;  //结算单ID
                                    invDetail.Period = stratDate.AddDays(-stratDate.Day + 1);//balanceMonth.AddDays(-balanceMonth.Day + 1);     //费用记账月
                                    invDetail.InvStartDate = computeStartDate;  //费用开始日期
                                    invDetail.InvEndDate = computeEndDate;      //费用结束日期
                                    invDetail.InvCurTypeID = curTypeID;  //结算币种
                                    invDetail.InvExRate = exRate;  //结算汇率
                                    invDetail.InvPayAmt = 0;  //费用应结金额
                                    invDetail.InvPayAmtL = 0;  //费用应结本币金额 = 费用应结金额 * 结算汇率
                                    invDetail.InvActPayAmt = 0; //费用实际应结金额
                                    invDetail.InvActPayAmtL = 0;  //费用实际应结本币金额
                                    invDetail.RentType = InvoiceDetail.RENTTYPE_BLANK_RECORD_P;   //租金类别
                                    blankAryList.Add(invDetail);
                                    //blankFlag++;
                                //}
                            }
                        }
                        else if (ifPrepay == ConLease.IFPREPAY_TYPE_NO)  //非预收保底
                        {
                            if (isFist == InvoiceHeader.ISFIRST_YES)  //是首期
                            {
                                minMoney = 0;
                            }
                            else
                            {
                                minMoney = GetMinMoney(paidAmt, salesToP, settleDays, formulID, minSumOpt);  //取公式保底金额
                                if (salesToP > minMoney)   //抽成租金 > 保底金额
                                {
                                    minMoney = salesToP;  //保底金额 = 抽成租金
                                }
                                rentType = GetRentType(pcentOpt);
                            }
                        }
                    }
                    decimal baseAmt = 0m;  //基本租金
                    decimal TotalFixedRental = 0m; //租金额

                    /**********************公式类别为固定***********************/

                    if (accountPFlag == 0)
                    {
                        if (conFormulaH.FormulaType == ConFormulaH.FORMULATYPE_TYPE_ONE)   //公式类别为固定
                        {
                            TotalFixedRental = conFormulaH.FixedRental * Convert.ToDecimal(settleDays); //总租金额
                            if (GetChargeClass(conFormulaH.ChargeTypeID) != ChargeType.CHARGECLASS_LEASE)
                                rentType = InvoiceDetail.RENTTYPE_NO_RENT;

                        }

                        /**********************公式类别为一次性收取***********************/

                        //if (conFormulaH.FormulaType == ConFormulaH.FORMULATYPE_TYPE_THREE)  //公式类别为一次性收取
                        //{
                        //    if (stratDate <= computeStartDate && endDate >= computeStartDate)
                        //    {
                        //        baseAmt = conFormulaH.BaseAmt; //基本租金
                        //        if (GetChargeClass(conFormulaH.ChargeTypeID) != ChargeType.CHARGECLASS_LEASE)
                        //            rentType = InvoiceDetail.RENTTYPE_NO_RENT;
                        //        else
                        //            rentType = InvoiceDetail.RENTTYPE_ONCE;   //租金类别：一次性收取
                        //    }
                        //}
                    }

                    TotalMoney = TotalFixedRental + baseAmt + salesTo + minMoney;   //总费用 = 总租金额 + 基本租金 + 根据抽成保底得出的金额 + 预收保底

                    if (conFormulaH.FormulaType == ConFormulaH.FORMULATYPE_TYPE_THREE && baseAmt != 0 && outFlag == 0)
                    {
                        InvoiceDetail invoiceDet = new InvoiceDetail();
                        invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                        invoiceDet.ChargeTypeID = conFormulaH.ChargeTypeID; //费用类别ID
                        invoiceDet.InvID = invId;  //结算单ID
                        invoiceDet.Period = computeStartDate.AddDays(-computeStartDate.Day + 1);//balanceMonth.AddDays(-balanceMonth.Day + 1);     //费用记账月
                        invoiceDet.InvStartDate = computeStartDate;  //费用开始日期
                        invoiceDet.InvEndDate = computeEndDate;      //费用结束日期
                        invoiceDet.InvCurTypeID = curTypeID;  //结算币种
                        invoiceDet.InvExRate = exRate;  //结算汇率
                        invoiceDet.InvPayAmt = TotalMoney;  //费用应结金额
                        invoiceDet.InvPayAmtL = TotalMoney / exRate;  //费用应结本币金额 = 费用应结金额 / 结算汇率
                        invoiceDet.InvActPayAmt = TotalMoney; //费用实际应结金额
                        invoiceDet.InvActPayAmtL = TotalMoney / exRate;  //费用实际应结本币金额
                        invoiceDet.RentType = rentType;   //租金类别

                        TotalAllMoney += TotalMoney;
                        TotalMoney = 0;
                        aryList.Add(invoiceDet);
                    }
                    else if(conFormulaH.FormulaType != ConFormulaH.FORMULATYPE_TYPE_THREE && outFlag == 0)
                    {
                        InvoiceDetail invoiceDet = new InvoiceDetail();
                        invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                        invoiceDet.ChargeTypeID = conFormulaH.ChargeTypeID; //费用类别ID
                        invoiceDet.InvID = invId;  //结算单ID
                        invoiceDet.Period = computeStartDate.AddDays(-computeStartDate.Day + 1);//balanceMonth.AddDays(-balanceMonth.Day + 1);     //费用记账月
                        invoiceDet.InvStartDate = computeStartDate;  //费用开始日期
                        invoiceDet.InvEndDate = computeEndDate;      //费用结束日期
                        invoiceDet.InvCurTypeID = curTypeID;  //结算币种
                        invoiceDet.InvExRate = exRate;  //结算汇率
                        invoiceDet.InvPayAmt = TotalMoney;  //费用应结金额
                        invoiceDet.InvPayAmtL = TotalMoney / exRate;  //费用应结本币金额 = 费用应结金额 * 结算汇率
                        invoiceDet.InvActPayAmt = TotalMoney; //费用实际应结金额
                        invoiceDet.InvActPayAmtL = TotalMoney / exRate;  //费用实际应结本币金额
                        invoiceDet.RentType = rentType;   //租金类别

                        TotalAllMoney += TotalMoney;
                        TotalMoney = 0;
                        aryList.Add(invoiceDet);
                    }
                }
            }          
        }

        private static int InsertData(int contractID, DateTime balanceMonth, string bcthID)
        {
            //取结算币种
            Resultset curyRs = GetConLease(contractID);
            ConLease conLse = curyRs.Dequeue() as ConLease;
            int curTypeID = conLse.CurTypeID;
            //取结算币种对应的汇率
            Decimal exRate = GetCurExRate(curTypeID);

            int count = aryList.Count;  //结算单明细行数
            int blackCount = blankAryList.Count;  //抽成记录行数

            InvoiceHeader invoiceHer = new InvoiceHeader();
            if (count != 0 || blackCount != 0)
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
                invoiceHer.InvType = invType;//InvoiceHeader.INVTYPE_LEASE;         //结算类型
                invoiceHer.IsFirst = isFist;  //是否首期
                invoiceHer.CurTypeID = curTypeID;  //币种代码
                invoiceHer.InvCurTypeID = curTypeID; //结算币种
                invoiceHer.InvExRate = exRate;   //结算汇率
                //invoiceHer.InvPeriod = balanceMonth.AddDays(-balanceMonth.Day + 1); //结算单记账月
                invoiceHer.InvPeriod = balanceMonth;
                invoiceHer.InvPayAmt = TotalAllMoney;  //结算金额
                invoiceHer.InvPayAmtL = TotalAllMoney / exRate; //结算本币金额 = 结算金额 * 结算汇率
                invoiceHer.InvActPayAmt = TotalAllMoney;  //费用实际应结金额
                invoiceHer.InvActPayAmtL = TotalAllMoney / exRate;  //费用实际应结本币金额
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
                        for (int j = 0; j < count; j++)   //循环保存结算明细
                        {
                            //aryList.Add(invId);
                            InvoiceDetail invoiceDet = (InvoiceDetail)aryList[j];
                            invoiceDet.InvID = invId;
                            baseTrans.Insert((BasePO)aryList[j]);
                        }

                        for (int k = 0; k < blackCount; k++)   //循环保存抽成记录
                        {
                            InvoiceDetail invoiceDet = (InvoiceDetail)blankAryList[k];
                            invoiceDet.InvID = invId;
                            baseTrans.Insert((BasePO)blankAryList[k]);
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
                        int interestCount = interestIDAryList.Count;  //修改滞纳金表中的结算单号
                        for (int n = 0; n < interestCount; n++)
                        {
                            string interestSql = "update InvoiceInterest set InvCode = '" + invId + "' where InterestID = " + Convert.ToInt32(interestIDAryList[n]);
                            baseTrans.ExecuteUpdate(interestSql);
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
        /// <param name="balanceMonth">费用记帐月</param>
        /// <returns></returns>
        private static int CountNoLeaseCharge(int contractID, int chgTypeID, DateTime balanceMonth)
        {
            bool isChargeType = CheckChargeType(chgTypeID);
            if (isChargeType == true)
            {
                DataSet shopDS = GetShopID(contractID);
                int shopCount = shopDS.Tables[0].Rows.Count;
                for (int iShop = 0; iShop < shopCount; iShop++)
                {
                    int cnShopID = Convert.ToInt32(shopDS.Tables[0].Rows[iShop]["ShopID"]);
                    int chgeID;
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
        /// 获取年终结算时间段
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="conStartDate">合同开始时间</param>
        /// <param name="conEndDate">合同结束时间</param>
        /// <param name="stopDate">终止日期</param>
        /// <param name="balanceMonth">费用记帐月</param>
        private static void YearEndCount(int contractID, DateTime conStartDate, DateTime conEndDate, DateTime stopDate, DateTime balanceMonth)
        {
            DataSet ds = GetYearEndCountFormula(contractID);
            int count = ds.Tables[0].Rows.Count;

            //获取已生成的年终结算记录数
            string str_sql = "select Count(distinct(A.InvID)) from InvoiceHeader A " +
                             " inner join InvoiceDetail B on A.InvID = B.InvID " +
                             " inner join ChargeType C on B.ChargeTypeID = C.ChargeTypeID " +
                             " where C.ChargeClass = " + ChargeType.CHARGECLASS_YEAREND +
                             " and A.ContractID = " + contractID;

            BaseBO baseBO = new BaseBO();
            DataSet rs = baseBO.QueryDataSet(str_sql);
            int yearEndCount = Convert.ToInt32(rs.Tables[0].Rows[0][0]);

            if (count > yearEndCount)
            {
                //for (int i = 0; i < count; i++)
                //{
                DateTime accountStartTime = Convert.ToDateTime(ds.Tables[0].Rows[yearEndCount]["FStartDate"]);
                DateTime accountEndTime = Convert.ToDateTime(ds.Tables[0].Rows[yearEndCount]["FEndDate"]);

                   
                    //accountEndTime = Convert.ToDateTime(ds.Tables[0].Rows[yearEndCount]["FEndDate"]);

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
                                    GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, balanceMonth, yearEndCount);
                                }
                                else if (stopDate < accountEndTime)
                                {
                                    accountEndTime = stopDate;
                                    GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, balanceMonth, yearEndCount);
                                }
                            }
                            else
                            {
                                GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, balanceMonth, yearEndCount);
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
                                    GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, balanceMonth, yearEndCount);
                                }
                            }
                            else
                            {
                                GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, balanceMonth, yearEndCount);
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
                                    GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, balanceMonth, yearEndCount);
                                }
                            }
                            else
                            {
                                GetYearEndCount(contractID, accountStartTime, accountEndTime, ds, balanceMonth, yearEndCount);
                            }
                        }
                        else  //时间段完全不在合同范围内
                        {
                            return;
                        }
                    }
                //}
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
        private static void GetYearEndCount(int contractID, DateTime stratDate, DateTime endDate, DataSet ds, DateTime balanceMonth, int yearEndCount)
        {
            //公式类别为抽成保底
            if (ds.Tables[0].Rows[yearEndCount]["FormulaType"].ToString() == ConFormulaH.FORMULATYPE_TYPE_TWO)
            {
                //年终结算金额
                decimal yearEndMoney = 0;

                //获取销售额
                decimal paidAmt = GetPaidAmt(contractID, stratDate, endDate);
                int formulID = Convert.ToInt32(ds.Tables[0].Rows[yearEndCount]["FormulaID"]);
                float settleDays = 1;
                /*************************取抽成*************************/

                //抽成方式
                string pcentOpt = ds.Tables[0].Rows[yearEndCount]["PcentOpt"].ToString();  

                //获取抽成租金
                decimal salesToP = GetPMomey(paidAmt, settleDays, formulID, pcentOpt);

                //保底方式
                string minSumOpt = ds.Tables[0].Rows[yearEndCount]["MinSumOpt"].ToString();       

                //取公式保底金额(结算金额)
                decimal countMinMoney = GetMinMoney(paidAmt, 0, settleDays, formulID, minSumOpt); 

                //抽成大于保底额
                if (salesToP > countMinMoney)
                {
                    yearEndMoney = salesToP - countMinMoney;
                    InvoiceDetail invDetail = new InvoiceDetail();
                    invDetail.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                    invDetail.ChargeTypeID = Convert.ToInt32(ds.Tables[0].Rows[yearEndCount]["ChargeTypeID"]); //费用类别ID
                    invDetail.InvID = invId;  //结算单ID
                    invDetail.Period = stratDate.AddDays(-stratDate.Day +1);//balanceMonth.AddDays(-balanceMonth.Day + 1);     //费用记账月
                    invDetail.InvStartDate = stratDate;  //费用开始日期
                    invDetail.InvEndDate = endDate;      //费用结束日期
                    invDetail.InvCurTypeID = curTypeID;  //结算币种
                    invDetail.InvExRate = exRate;  //结算汇率
                    invDetail.InvPayAmt = yearEndMoney;  //费用应结金额
                    invDetail.InvPayAmtL = yearEndMoney / exRate;  //费用应结本币金额 = 费用应结金额 * 结算汇率
                    invDetail.InvActPayAmt = yearEndMoney; //费用实际应结金额
                    invDetail.InvActPayAmtL = yearEndMoney / exRate;  //费用实际应结本币金额
                    invDetail.RentType = InvoiceDetail.RENTTYPE_NO_RENT;   //租金类别

                    TotalAllMoney += Convert.ToDecimal(yearEndMoney);
                    aryList.Add(invDetail);
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
        /// 获取抽成租金
        /// </summary>
        /// <param name="saleMoney">销售额</param>
        /// <param name="settleDays">比重天数</param>
        /// <param name="formulaId">抽成公式ID</param>
        /// <param name="pcentOpt">抽成方式</param>
        /// <returns></returns>
        private static decimal GetPMomey(decimal saleMoney,float settleDays,int formulaId,string pcentOpt)
        {
            decimal saleToP = 0m;
            DataSet conFormulaPds = GetConFormulaP(formulaId);
            int conFormulaPCount = conFormulaPds.Tables[0].Rows.Count;
            if (conFormulaPCount > 0)
            {
                if (pcentOpt == ConFormulaH.PCENTOPT_TYPE_FAST)  //抽成为固定
                {
                    saleToP = saleMoney * Convert.ToDecimal(conFormulaPds.Tables[0].Rows[0]["Pcent"]);
                    rentType = InvoiceDetail.RENTTYPE_FIXED_P;   //租金类别：固定抽成租金
                }
                else if (pcentOpt == ConFormulaH.PCENTOPT_TYPE_S)  //抽成为单级
                {
                    for (int u = 0; u < conFormulaPCount; u++)
                    {
                        if (saleMoney < Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["SalesTo"]) * Convert.ToDecimal(settleDays))
                        {
                            saleToP = saleMoney * Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["Pcent"]);
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
                            saleToP += (salesTo - beginSales) * Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["Pcent"]);
                            beginSales = salesTo;

                            //comRate = comRate - (Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["SalesTo"]) - comRate);
                        }
                        else if (u == 0)
                        {
                            saleToP += saleMoney * Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["Pcent"]);
                            break;
                        }
                        else
                        {
                            saleToP += (saleMoney - (Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u - 1]["SalesTo"]) * Convert.ToDecimal(settleDays))) * Convert.ToDecimal(conFormulaPds.Tables[0].Rows[u]["Pcent"]);
                            break;
                        }
                    }
                    rentType = InvoiceDetail.RENTTYPE_MUNCH_P;       //租金类别：多级抽成租金
                }
            }
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
                        rentType = InvoiceDetail.RENTTYPE_FIXED_M;          //租金类别：固定保底租金
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
                        rentType = InvoiceDetail.RENTTYPE_LEVEL_M;      //租金类别：级别保底租金
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
        /// 计算一次性费用
        /// </summary>
        /// <param name="conID">合同ID</param>
        /// <param name="balanceMonth">记帐月</param>
        /// <param name="chargeType">费用类别ID</param>
        /// <param name="conStartDate">合同开始时间</param>
        /// <param name="conEndDate">合同结束时间</param>
        /// <param name="firstFlag">是否首期标志</param>
        /// <param name="billCyle">结算周期</param>
        private static int CountOneCharge(int conID, DateTime balanceMonth, int chargeType, DateTime conStartDate, DateTime conEndDate, int firstFlag,int billCyle)
        {
            int result = 0;
            string str_sql = "";
            if (firstFlag == InvoiceHeader.ISFIRST_YES)
            {
                str_sql = "select FormulaID,ChargeTypeID,ContractID,FStartDate,FEndDate,FormulaType,RateType,PcentOpt,MinSumOpt,TotalArea," +
                                 "FixedRental,UnitPrice,BaseAmt" +
                                 " from ConFormulaH " +
                                 " where ContractID = " + conID +
                                 " and FStartDate >= '" + conStartDate +
                                 "' and FStartDate <= '" + balanceMonth.AddMonths(billCyle).AddDays(-balanceMonth.Day) +
                                 "' and ChargeTypeID = " + chargeType +
                                 " and FormulaType = '" + ConFormulaH.FORMULATYPE_TYPE_THREE + "'";
            }
            else
            {

                str_sql = "select FormulaID,ChargeTypeID,ContractID,FStartDate,FEndDate,FormulaType,RateType,PcentOpt,MinSumOpt,TotalArea," +
                                 "FixedRental,UnitPrice,BaseAmt" +
                                 " from ConFormulaH " +
                                 " where ContractID = " + conID +
                                 " and FStartDate >= '" + balanceMonth +
                                 "' and FStartDate <= '" + balanceMonth.AddMonths(billCyle).AddDays(-balanceMonth.Day) +
                                 "' and ChargeTypeID = " + chargeType +
                                 " and FormulaType = '" + ConFormulaH.FORMULATYPE_TYPE_THREE + "'";
            }
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            int count = ds.Tables[0].Rows.Count;
            for(int i = 0; i < count; i++)
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

        /*private static void GetOneChargeDate(int conID, DateTime balanceMonth, int chargeType, DateTime conStartDate, DateTime conEndDate, int billCyle, int firstFlag, int settleMode)
        {
            DateTime oneChargeStartDate;
            DateTime oneChargeEndDate;
            int week;
            if (firstFlag == InvoiceHeader.ISFIRST_YES)  //首期
            {
                if (settleMode == ConLease.SETTLEMODE_TYPE_FIRST)   //首月对齐
                {
                    oneChargeStartDate = conStartDate;
                    oneChargeEndDate = oneChargeStartDate.AddMonths(billCyle).AddDays(-oneChargeStartDate.Day);
                }
                else if (settleMode == ConLease.SETTLEMODE_TYPE_LAST) //次月对齐
                {
                    if (billCyle == 1)
                    {
                        oneChargeStartDate = conStartDate;
                        oneChargeEndDate = oneChargeStartDate.AddMonths(1).AddDays(-1);
                    }
                    else
                    {
                        oneChargeStartDate = conStartDate;
                        oneChargeEndDate = conStartDate.AddMonths(billCyle).AddDays(-conStartDate.Day);
                    }
                }
                else if (settleMode == ConLease.SETTLEMODE_TYPE_NO) //不做调整
                {
                    oneChargeStartDate = conStartDate;
                    oneChargeEndDate = oneChargeStartDate.AddMonths(billCyle).AddDays(-1);
                }
            }
            else  //非首期
            {
                if (settleMode == ConLease.SETTLEMODE_TYPE_FIRST) //首月对齐
                {
                    oneChargeStartDate = balanceMonth;
                    oneChargeEndDate = oneChargeStartDate.AddMonths(billCyle).AddDays(-oneChargeStartDate.Day);
                }
                else if (settleMode == ConLease.SETTLEMODE_TYPE_LAST) //次月对齐
                {
                    if (billCyle == 1)
                    {
                        oneChargeStartDate = balanceMonth;
                        oneChargeEndDate = oneChargeStartDate.AddMonths(billCyle).AddDays(-1);
                    }
                    else
                    {
                        oneChargeStartDate = balanceMonth;
                        oneChargeEndDate = oneChargeStartDate.AddMonths(billCyle).AddDays(-conStartDate.Day);
                    }
                }
                else if (settleMode == ConLease.SETTLEMODE_TYPE_NO) //不做调整
                {
                    
                }
            }
        }*/


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
        private static DataSet GetChgDetail(int chgID,int chgTypeID)
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
        private static Resultset GetFormulaH(int contractID,int chargeTypeID, DateTime startTime, DateTime endTime)
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
        public static decimal GetCurExRate(int curTypeId)
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
                            conID + " and A.BizDate between '" + beginDate + "' and '" + endDate + "' and mediamno != 800 ";
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(sql);
            decimal paidAmt = (ds.Tables[0].Rows[0]["PaidAmt"] == DBNull.Value ? 0m : Convert.ToDecimal(ds.Tables[0].Rows[0]["PaidAmt"]));
            return paidAmt;
        }

        /// <summary>
        /// 获取按合同抽成率的销售额
        /// </summary>
        /// <param name="conID">合同号</param>
        /// <param name="beginDate">费用计算开始时间</param>
        /// <param name="endDate">费用计算结束时间</param>
        /// <returns></returns>
        private static decimal GetPaidAmtByCon(int conID, DateTime beginDate, DateTime endDate)
        {
            string sql = "select sum(PaidAmt) as PaidAmt from TransSkuMedia A,ConShop B where A.ShopID = B.ShopID and B.ContractID = " +
                            conID + " and A.BizDate between '" + beginDate + "' and '" + endDate + "' and mediamno != 800 AND isCust = 'Y'";
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
        private static DataSet GetBlankRecord(int contractID,DateTime balanceMonth)
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
        private static decimal GetExistMinMoney(DateTime sDate,DateTime eDate, int contractID)
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
        private static bool IsExistConFormulaP(int contractID,DateTime balanceMonth)
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
        /// <param name="balanceMonth">费用计帐月</param>
        /// <param name="chgeTypeID">费用类型</param>
        /// <returns></returns>
        private static DateTime GetLastAccountDate(int conID, DateTime balanceMonth, int chgeTypeID)
        {
            DateTime lastAccountDate = Convert.ToDateTime("9999-12-31");
            string sql = "select max(A.InvEndDate) as invEndDate from invoicedetail A , invoiceheader B " +
                            " where A.invid = B.invid and B.Contractid = " +conID+ " and A.Period = '" + balanceMonth.AddMonths(-1) +
                            "' and A.ChargeTypeID = "+ chgeTypeID;
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
            string str_sql = "select ChargeTypeID,ChargeTypeCode,ChargeTypeName,ChargeClass,IsChargeCross,Note,AccountNumber from ChargeType "+
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
        private static float GetAccountDays(DateTime accountStartDate,DateTime accountEndDate,string rateType)
        {
            float settleDays = 0;
            int days = GetMonthDay(accountStartDate); //获取该月的天数

            TimeSpan ts = accountEndDate - accountStartDate;
            double conFormulaDays = ts.TotalDays + 1;

            if (rateType == ConFormulaH.RATETYPE_TYPE_MONTH)  //月租金
            {
                if (accountStartDate.AddDays(days - 1) == accountEndDate)  //整月
                {
                    settleDays = days/days;
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
        /// 判断费用类别是否为滞纳金
        /// </summary>
        /// <param name="chargeTypeID">费用类别ID</param>
        /// <returns></returns>
        private static bool IsInterest(int chargeTypeID)
        {
            bool isInterest;
            BaseBO baseBo = new BaseBO();
            baseBo.WhereClause = "ChargeTypeID = " + chargeTypeID + " and ChargeClass = " + ChargeType.CHARGECLASS_INTEREST;
            Resultset rs = baseBo.Query(new ChargeType());
            if (rs.Count > 0)
                isInterest = true;
            else
                isInterest = false;
            return isInterest;
        }

        /// <summary>
        /// 使已生成的滞纳金生效并打印
        /// </summary>
        /// <param name="chargeTypeID">费用类别ID</param>
        /// <returns></returns>
        private static int SetInterestPrint(int contractID)
        {
            rentType = InvoiceDetail.RENTTYPE_NO_RENT;   //租金类别：非租金
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ContractID = " + contractID + " and InvCode = ''";
            Resultset rs = baseBO.Query(new InvoiceInterest());
            if (rs.Count > 0)
            {
                foreach (InvoiceInterest invInterest in rs)
                {
                    interestIDAryList.Add(invInterest.InterestID);
                    InvoiceDetail invoiceDet = new InvoiceDetail();
                    invoiceDet.InvDetailID = BaseApp.GetInvDetailID();  //结算单明细ID
                    invoiceDet.ChargeTypeID = Convert.ToInt32(invInterest.ChargeTypeID); //费用类别ID
                    invoiceDet.InvID = invId;  //结算单ID
                    invoiceDet.Period = Convert.ToDateTime(invInterest.IntStartDate).AddDays(-invInterest.IntStartDate.Day + 1);//balanceMonth.AddDays(-balanceMonth.Day + 1);     //费用记账月
                    invoiceDet.InvStartDate = Convert.ToDateTime(invInterest.IntStartDate);  //费用开始日期
                    invoiceDet.InvEndDate = Convert.ToDateTime(invInterest.IntEndDate);      //费用结束日期
                    invoiceDet.InvCurTypeID = curTypeID;  //结算币种
                    invoiceDet.InvExRate = exRate;  //结算汇率
                    invoiceDet.InvPayAmt = Convert.ToDecimal(invInterest.InterestAmt);  //费用应结金额
                    invoiceDet.InvPayAmtL = Convert.ToDecimal(invInterest.InterestAmt) / exRate;  //费用应结本币金额 = 费用应结金额 / 结算汇率
                    invoiceDet.InvActPayAmt = Convert.ToDecimal(invInterest.InterestAmt); //费用实际应结金额
                    invoiceDet.InvActPayAmtL = Convert.ToDecimal(invInterest.InterestAmt) / exRate;  //费用实际应结本币金额
                    invoiceDet.RentType = rentType;   //租金类别
                    invoiceDet.Note = invInterest.Note;

                    TotalAllMoney += Convert.ToDecimal(invInterest.InterestAmt);
                    tempMoney += Convert.ToDecimal(invInterest.InterestAmt);
                    aryList.Add(invoiceDet);
                    if (tempMoney > 0)
                        tempFlag = 1;
                }
            }
            return tempFlag;
        }

        /// <summary>
        /// 获取抽成的结算类型(RentType)
        /// </summary>
        /// <param name="pcentOpt">抽成方式</param>
        /// <returns></returns>
        private static int GetRentType(string pcentOpt)
        {
            int rentType = 0;
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
            return rentType;
        }

        /// <summary>
        /// 获取客户名
        /// </summary>
        /// <param name="conID">合同ID</param>
        /// <returns></returns>
        private static string GetCustName(int conID)
        {
            string str_sql = "select CustName from Customer A , Contract B "+
                             " where A.CustID = B.CustID and B.ContractID = " + conID;
            BaseBO baseBo = new BaseBO();
            DataSet ds = baseBo.QueryDataSet(str_sql);
            return ds.Tables[0].Rows[0]["CustName"].ToString();
        }

        /// <summary>
        /// 获取费用大类为租金的ChargeTypeID
        /// </summary>
        /// <param name="chargeType">选中的费用列别</param>
        /// <returns></returns>
        public static string GetRentID(string chargeType)
        {
            string str_sql = "SELECT ChargeTypeID FROM ChargeType WHERE ChargeClass = " + ChargeType.CHARGECLASS_LEASE;
            str_sql = str_sql + " and ChargeTypeID in ( " + chargeType +"0 ) ";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);

            string cTypeID = "";
            int count = ds.Tables[0].Rows.Count;
            for (int i = 0; i < count; i++)
            {
                cTypeID += ds.Tables[0].Rows[i]["ChargeTypeID"] + ",";
            }
            return cTypeID;
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
