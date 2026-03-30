using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using Invoice.InvoiceH;
using Base.DB;
using Base.Biz;
using Lease;
using Base;
using Lease.Contract;
using Lease.Union;

namespace Invoice
{
    /// <summary>
    /// 滞纳金PO
    /// </summary>
    public class InvoiceInterestPO
    {
        public static int INTERESTAMT_NO = 0;  //滞纳金未生成
        public static int INTERESTAMT_YES = 1; //滞纳金生成成功
        public static decimal interestRate; //滞纳金利率
        //滞纳金计算开始日期
        public static DateTime intStartDate = DateTime.Now;
        //获取滞纳金费用类别ID
        public static int chargeTypeID = 0;
        public static ArrayList aryList;



        /// <summary>
        /// 计算滞纳金
        /// </summary>
        /// <param name="invID">结算单ID</param>
        /// <param name="invStartDate">付款开始日期</param>
        /// <param name="startDate">滞纳金计算开始日期</param>
        /// <param name="endDate">滞纳金计算结束日期</param>
        /// <param name="contractID">合同ID</param>
        /// <param name="htb">操作员信息</param>
        /// <param name="bizMode">合同类型</param>
        /// <returns></returns>
        public static int InvInterestCount(int invID,DateTime invStartDate,DateTime startDate, DateTime endDate, int contractID,Hashtable htb,int bizMode)
        {
            aryList = new ArrayList();
            int intDay = 0;  //滞纳金启征天数
            decimal latePayInt = 0; //滞纳金利率
            //结算单生成日期
            DateTime invDate = Convert.ToDateTime(GetInvoiceHeader(invID).ToShortDateString());
            //滞纳金计算开始日期
            intStartDate = startDate;
            //获取滞纳金费用类别ID
            chargeTypeID = GetChargeTypeID();
            if (bizMode == Contract.BIZ_MODE_LEASE)  //租赁
            {
                //获取租赁合同相关条款信息
                Resultset conLeaseRS = GetConLease(contractID);
                if (conLeaseRS.Count > 0)
                {
                    ConLease conLease = conLeaseRS.Dequeue() as ConLease;
                    intDay = conLease.IntDay;
                    latePayInt = conLease.LatePayInt;
                    //intStartDate = invDate.AddDays(intDay - 1);
                }
            }
            else if (bizMode == Contract.BIZ_MODE_UNIT)  //联营
            {
                //获取联营合同相关条款信息
                Resultset conLeaseRS = GetConUnion(contractID);
                if (conLeaseRS.Count > 0)
                {
                    ConUnion conUnion = conLeaseRS.Dequeue() as ConUnion;
                    intDay = conUnion.IntDay;
                    latePayInt = conUnion.LatePayInt;
                    //intStartDate = invDate.AddDays(intDay - 1);
                }
            }

            //获取收款结算单明细
            Resultset invDetailRS = GetInvoiceDetail(invID);
            if (invDetailRS.Count > 0)
            {
                foreach (InvoiceDetail invDetail in invDetailRS)
                {
                    decimal tempInvPaidAmt = 0;
                    decimal invPaidAmt = 0;
                    decimal notInvMoney = 0;
                    int flag = 0;
                    DateTime lastDate = DateTime.Now;

                    DateTime intEndDate = GetIntEndDate(invDetail.InvDetailID);
                    if (intEndDate != Convert.ToDateTime("1900-01-01"))
                    {
                        intStartDate = intEndDate.AddDays(1);
                    }
                    if (intStartDate > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        return INTERESTAMT_NO;
                    }

                    //滞纳金利率
                    interestRate = GetInterestRate(contractID, Convert.ToInt32(invDetail.ChargeTypeID));
                    //该费用未设置滞纳金利率则取租赁合同相关条款滞纳金利率
                    if (interestRate == -1)
                    {
                        interestRate = latePayInt;
                    }

                    //根据结算单明细ID获取付款明细
                    DataSet invPayDetailDS = GetInvoicePayDetail(invDetail.InvDetailID, invStartDate, endDate);
                    int count = invPayDetailDS.Tables[0].Rows.Count;
                    if (count > 0)   //有付款情况
                    {
                        decimal sumInvPaidAmt = 0;
                        decimal sumInvActPayAmt = 0;
                        for (int i = 0; i < count; i++)
                        {
                            //获取付款日期
                            DateTime invPayDate = Convert.ToDateTime(invPayDetailDS.Tables[0].Rows[i]["InvPayDate"]);//GetInvPayDate(Convert.ToInt32(invPayDetailDS.Tables[0].Rows[i]["InvPayID"]));
                            lastDate = invPayDate;
                            sumInvPaidAmt += Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvPaidAmt"]);
                            sumInvActPayAmt = Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[0]["InvActPayAmt"]);

                            //付款日期 > 滞纳金启征日期
                            if (invPayDate > intStartDate)
                            {
                                invPaidAmt += Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvPaidAmt"]);
                                if (flag > 0)
                                {
                                    tempInvPaidAmt += Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i - 1]["InvPaidAmt"]);
                                    //未结算金额
                                    notInvMoney = Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvActPayAmt"]) - tempInvPaidAmt;
                                }
                                //计算滞纳金
                                if ((Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvActPayAmt"]) - Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvPaidAmt"])) == 0)  //一次性已付清(超过滞纳金启征日期)
                                {
                                    flag = 0;
                                    TimeSpan ts = invPayDate - intStartDate;
                                    int interestDay = Convert.ToInt32(ts.Days) + 1;
                                    decimal interestAmt = Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvPaidAmt"]) * (Convert.ToDecimal(interestDay)) * interestRate;

                                    AddAryList(intStartDate,invPayDate, interestAmt, interestDay, invDetail.InvDetailID, invDetail.InvID, Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvPaidAmt"]), htb, contractID, Convert.ToInt32(invPayDetailDS.Tables[0].Rows[i]["ChargeTypeID"]));
                                }
                                else if ((Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvActPayAmt"]) - sumInvPaidAmt == 0)) //多次已付清(超过滞纳金启征日期)
                                {
                                    flag = 0;
                                    TimeSpan ts = invPayDate - intStartDate;
                                    int interestDay = Convert.ToInt32(ts.Days) + 1;
                                    decimal interestAmt = Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvPaidAmt"]) * (Convert.ToDecimal(interestDay)) * interestRate;
                                    AddAryList(intStartDate, invPayDate, interestAmt, interestDay, invDetail.InvDetailID, invDetail.InvID, Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvPaidAmt"]), htb, contractID, Convert.ToInt32(invPayDetailDS.Tables[0].Rows[i]["ChargeTypeID"]));
                                }
                                else //未付清(超过滞纳金启征日期)
                                {
                                    if (flag == 0)
                                    {
                                        TimeSpan ts = invPayDate - intStartDate;
                                        int interestDay = Convert.ToInt32(ts.Days);
                                        decimal interestAmt = Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvActPayAmt"]) * (Convert.ToDecimal(interestDay)) * interestRate;

                                        AddAryList(intStartDate,invPayDate.AddDays(-1), interestAmt, interestDay, invDetail.InvDetailID, invDetail.InvID, Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvActPayAmt"]), htb, contractID, Convert.ToInt32(invPayDetailDS.Tables[0].Rows[i]["ChargeTypeID"]));
                                        flag++;
                                    }
                                    else
                                    {
                                        TimeSpan ts = invPayDate - Convert.ToDateTime(invPayDetailDS.Tables[0].Rows[i - 1]["InvPayDate"]);
                                        int interestDay = Convert.ToInt32(ts.Days) + 1;
                                        decimal interestAmt = notInvMoney * (Convert.ToDecimal(interestDay)) * interestRate;

                                        AddAryList(Convert.ToDateTime(invPayDetailDS.Tables[0].Rows[i - 1]["InvPayDate"]),invPayDate, interestAmt, interestDay, invDetail.InvDetailID, invDetail.InvID, notInvMoney, htb, contractID, Convert.ToInt32(invPayDetailDS.Tables[0].Rows[i]["ChargeTypeID"]));
                                        flag++;
                                    }
                                }
                            }
                            else if(count == 1)//付款日期 <= 滞纳金启征日期 并未付清
                            {
                                //从滞纳金启征日期到滞纳金结束日期计算滞纳金
                                decimal latePayAmt = Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvActPayAmt"]) - Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[i]["InvPaidAmt"]);
                                if (latePayAmt > 0)
                                {
                                    flag = 0;
                                    TimeSpan ts = endDate - intStartDate;
                                    int interestDay = Convert.ToInt32(ts.Days) + 1;
                                    decimal interestAmt = latePayAmt * (Convert.ToDecimal(interestDay)) * interestRate;

                                    AddAryList(intStartDate,endDate, interestAmt, interestDay, invDetail.InvDetailID, invDetail.InvID, latePayAmt, htb, contractID, Convert.ToInt32(invPayDetailDS.Tables[0].Rows[i]["ChargeTypeID"]));
                                }
                            }
                        }
                        if (sumInvPaidAmt != sumInvActPayAmt && flag > 0)  //付款后但未付清，从付款后到滞纳金计算结束日期开始计算滞纳金
                        {
                            TimeSpan ts = endDate - lastDate;
                            int interestDay = Convert.ToInt32(ts.Days) + 1;
                            if (interestDay > 0)
                            {
                                decimal interestAmt = (Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[0]["InvActPayAmt"]) - invPaidAmt) * Convert.ToDecimal(interestDay) * interestRate;

                                AddAryList(lastDate,endDate, interestAmt, interestDay, invDetail.InvDetailID, invDetail.InvID, Convert.ToDecimal(invPayDetailDS.Tables[0].Rows[0]["InvActPayAmt"]) - invPaidAmt, htb, contractID, Convert.ToInt32(invDetail.ChargeTypeID.ToString()));
                            }
                        }
                    }
                    else  //无付款情况
                    {
                        //根据结算单明细ID获取付款明细
                        DataSet invPayDS = GetInvoicePayDetail(invDetail.InvDetailID);
                        int invPayCount = invPayDS.Tables[0].Rows.Count;
                        //if (invPayCount == 0)
                        //{
                            TimeSpan ts = endDate - intStartDate;
                            int interestDay = Convert.ToInt32(ts.Days) + 1;

                            decimal interestAmt = invDetail.InvActPayAmt * (Convert.ToDecimal(interestDay)) * interestRate;

                            AddAryList(intStartDate,endDate, interestAmt, interestDay, invDetail.InvDetailID, invDetail.InvID, invDetail.InvActPayAmt, htb, contractID, Convert.ToInt32(invDetail.ChargeTypeID.ToString()));
                        //}
                    }
                }
            }
            BaseTrans trans = new BaseTrans();
            trans.BeginTrans();
            try
            {
                int aryCount = aryList.Count;
                for (int j = 0; j < aryCount; j++)
                {
                    InvoiceInterest invInterest = (InvoiceInterest)aryList[j];
                    invInterest.InterestID = BaseApp.GetInterestID();
                    trans.Insert((BasePO)(aryList[j]));
                }
                if (aryCount == 0)
                {
                    return INTERESTAMT_NO;
                }

            }
            catch(Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            trans.Commit();
            return INTERESTAMT_YES;
        }

        /// <summary>
        /// 将数据添加到数组
        /// </summary>
        /// <param name="startDate">滞钠金开始日期</param>
        /// <param name="endDate">滞钠金结束日期</param>
        /// <param name="iAmt">滞钠金</param>
        /// <param name="iDay">滞钠天数</param>
        /// <param name="lInvDetailID">结算单明细ID</param>
        /// <param name="lInvID">结算单ID</param>
        /// <param name="lPayAmt">滞钠本金</param>
        /// <param name="htb">操作员信息</param>
        /// <param name="contractID">合同号</param>
        /// <param name="chgTypeID">费用类型ID</param>
        public static void AddAryList(DateTime startDate,DateTime endDate, Decimal iAmt, int iDay, int lInvDetailID, int lInvID, decimal lPayAmt, Hashtable htb, int contractID,int chgTypeID)
        {
            if (iAmt > 0)
            {
                InvoiceInterest invInterest = new InvoiceInterest();
                invInterest.CreateTime = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                invInterest.CreateUserID = Convert.ToInt32(htb["CreateUserID"]);  //操作员
                invInterest.ExtendDay = 0;
                invInterest.IntEndDate = endDate;
                invInterest.InterestAmt = iAmt;
                invInterest.InterestDay = iDay;
                invInterest.InterestRate = interestRate;
                invInterest.IntStartDate = startDate;
                invInterest.ContractID = contractID;
                invInterest.InvCode = "";
                invInterest.LateInvDetailID = lInvDetailID;
                invInterest.LateInvID = lInvID;
                invInterest.LatePayAmt = lPayAmt;
                invInterest.ModifyTime = DateTime.Now;
                invInterest.ModifyUserID = Convert.ToInt32(htb["CreateUserID"]);  //操作员
                invInterest.Note = "";
                invInterest.OprDeptID = Convert.ToInt32(htb["OprDeptID"]);  //操作员部门
                invInterest.OprRoleID = Convert.ToInt32(htb["OprRoleID"]);  //操作员角色
                invInterest.ChargeTypeID = chargeTypeID;
                invInterest.Note = GetChargeTypeName(chgTypeID);
                aryList.Add(invInterest);
            }
        }

        /// <summary>
        /// 根据合同号获取租赁合同相关条款
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <returns></returns>
        public static Resultset GetConLease(int contractID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ContractID = " + contractID;
            Resultset rs = baseBO.Query(new ConLease());
            return rs;
        }

        /// <summary>
        /// 根据合同号获取联营合同相关条款
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <returns></returns>
        public static Resultset GetConUnion(int contractID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ContractID = " + contractID;
            Resultset rs = baseBO.Query(new ConUnion());
            return rs;
        }

        /// <summary>
        /// 根据结算单号获取收款结算单明细
        /// </summary>
        /// <param name="InvID">结算单号</param>
        /// <returns></returns>
        public static Resultset GetInvoiceDetail(int InvID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "InvID = " + InvID + " and RentType != " + InvoiceDetail.RENTTYPE_BLANK_RECORD_P + " and NOT EXISTS (select ChargeTypeID from chargetype where chargeclass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST + " and chargetype.chargeTypeID = InvoiceDetail.chargeTypeID)";
            Resultset rs = baseBO.Query(new InvoiceDetail());
            return rs;
        }

        /// <summary>
        /// 根据合同号和费用类型ID获取滞纳金利率
        /// </summary>
        /// <param name="contractID">合同ID</param>
        /// <param name="chargeTypeID">费用类型ID</param>
        /// <returns></returns>
        public static Decimal GetInterestRate(int contractID, int chargeTypeID)
        {
            decimal interestRate = -1;
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ContractID = " + contractID + " and ChargeTypeID = " + chargeTypeID;
            Resultset rs = baseBO.Query(new InterestRate());
            if (rs.Count > 0)
            {
                InterestRate intRate = rs.Dequeue() as InterestRate;
                interestRate = intRate.IntRate;
            }
            return interestRate;
        }

        /// <summary>
        /// 根据结算单号获取结算单付款日期
        /// </summary>
        /// <param name="invID"></param>
        /// <returns></returns>
        public static DateTime GetInvoiceHeader(int invID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "InvID = " + invID;
            Resultset rs = baseBO.Query(new InvoiceHeader());
            InvoiceHeader invHeader = rs.Dequeue() as InvoiceHeader;
            return Convert.ToDateTime(invHeader.InvDate);
        }

        /// <summary>
        /// 根据付款单号获取付款日期
        /// </summary>
        /// <param name="invPayID">付款单号ID</param>
        /// <returns></returns>
        public static DateTime GetInvPayDate(int invPayID)
        {
            DateTime invPayDate = Convert.ToDateTime("1900-01-01");
            string str_sql = "select A.InvPayDate from InvoicePay A where InvPayID = " + invPayID;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                invPayDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["InvPayDate"]);
            }
            return invPayDate;
        }

        /// <summary>
        /// 根据结算单明细ID获取付款明细信息
        /// </summary>
        /// <param name="invDetailID">结算单明细ID</param>
        /// <returns></returns>
        public static DataSet GetInvoicePayDetail(int invDetailID)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select InvPayDetID,ChargeTypeID,InvPayID,InvID,InvDetailID,InvActPayAmt,InvActPayAmtL,InvPaidAmt,InvPaidAmtL,InvPayDetStatus,PayOutAmtSum,Note from InvoicePayDetail where InvDetailID = " + invDetailID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据结算单明细ID获取付款明细信息
        /// </summary>
        /// <param name="invDetailID">结算单明细ID</param>
        /// <returns></returns>
        public static DataSet GetInvoicePayDetail(int invDetailID,DateTime startDate,DateTime endDate)
        {
            BaseBO baseBO = new BaseBO();
            //string str_sql = "select A.InvPayDate,B.InvPayDetID,B.ChargeTypeID,B.InvPayID,B.InvID,B.InvDetailID,B.InvActPayAmt,B.InvActPayAmtL,B.InvPaidAmt,B.InvPaidAmtL,B.InvPayDetStatus,B.PayOutAmtSum,B.Note from InvoicePay A,InvoicePayDetail B where A.InvPayID = B.InvPayID and A.InvPayDate >= '" +
            //                  startDate + "' and A.InvPayDate <= '" + endDate + "' and B.InvDetailID = " + invDetailID + " and B.InvActPayAmt != B.InvPaidAmt";
            string str_sql = "select A.InvPayDate,B.InvPayDetID,B.ChargeTypeID,B.InvPayID,B.InvID,B.InvDetailID,B.InvActPayAmt,B.InvActPayAmtL,B.InvPaidAmt,B.InvPaidAmtL,B.InvPayDetStatus,B.PayOutAmtSum,B.Note from InvoicePay A,InvoicePayDetail B where A.InvPayID = B.InvPayID and A.InvPayDate >= '" +
                              startDate + "' and A.InvPayDate <= '" + endDate + "' and B.InvDetailID = " + invDetailID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据结算单ID获取合同与客户信息
        /// </summary>
        /// <param name="invID">合同号</param>
        /// <returns></returns>
        public static DataSet GetContractInfo(string conCode)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "select A.ContractID,A.ContractCode,B.CustID,B.CustName,C.InvID from Contract A,Customer B,InvoiceHeader C " +
                             " where A.ContractID = C.ContractID and A.CustID = B.CustID and A.ContractCode = '" + conCode + "'";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据结算单明细ID获取改结算明细滞纳结束日期
        /// </summary>
        /// <param name="invDetailID">结算单明细ID</param>
        /// <returns></returns>
        public static DateTime GetIntEndDate(int invDetailID)
        {
            DateTime dt = Convert.ToDateTime("1900-01-01");
            BaseBO baseBO = new BaseBO();
            string str_sql = "select max(IntEndDate) as IntEndDate from invoiceInterest where LateInvDetailID = " + invDetailID;
            DataSet ds = baseBO.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows[0]["IntEndDate"].GetType().ToString() != "System.DBNull")
                dt = Convert.ToDateTime(ds.Tables[0].Rows[0]["IntEndDate"]);
            return dt;
        }

        /// <summary>
        /// 根据结算单明细ID获取结算付款单总付款额
        /// </summary>
        /// <param name="invDetailID">结算单明细ID</param>
        /// <returns></returns>
        public static decimal GetInvPaidAmt(int invDetailID,DateTime invPayDate)
        {
            decimal invPaidAmt = 0;
            BaseBO baseBO = new BaseBO();
            string str_sql = "select sum(A.InvPaidAmt) as InvPaidAmt from InvoicePayDetail A,InvoicePay B where A.InvPayID = B.InvPayID and A.InvDetailID = " + invDetailID + " and B.InvPayDate < '" + invPayDate + "'";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            if (ds.Tables[0].Rows[0]["InvPaidAmt"].GetType().ToString() != "System.DBNull")
                invPaidAmt = Convert.ToDecimal(ds.Tables[0].Rows[0]["InvPaidAmt"]);
            return invPaidAmt;
        }

        /// <summary>
        /// 获取滞纳金费用类别ID
        /// </summary>
        /// <returns></returns>
        private static int GetChargeTypeID()
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ChargeClass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST;
            Resultset rs = baseBO.Query(new ChargeType());
            ChargeType chargeType = rs.Dequeue() as ChargeType;
            return chargeType.ChargeTypeID;
        }

        /// <summary>
        /// 根据费用类别ID获取费用名称
        /// </summary>
        /// <param name="chargeTypeID"></param>
        /// <returns></returns>
        private static string GetChargeTypeName(int chargeTypeID)
        {
            BaseBO baseBO = new BaseBO();
            baseBO.WhereClause = "ChargeTypeID = " + chargeTypeID;
            DataSet tempDS = baseBO.QueryDataSet(new Lease.PotBargain.ChargeType());
            return tempDS.Tables[0].Rows[0]["ChargeTypeName"].ToString();
        }

        /// <summary>
        /// 重打滞纳金
        /// </summary>
        /// <param name="contractCode">合同号</param>
        /// <param name="sDate">开始时间</param>
        /// <param name="eDate">结束时间</param>
        /// <returns></returns>
        public static DataSet GetAgainInterestInfo(string contractCode,string sDate,string eDate)
        {
            string where1 = "";
            if (contractCode != "")
            {
                where1 = " AND C.ContractCode = '" + contractCode + "'";
            }
            string str_sql = "select distinct(B.InvID),A.CustCode,A.CustName,A.TaxCode,A.BankName,A.BankAcct,B.InvCode,B.InvDate,C.ContractCode" +
                             " from Customer A,InvoiceHeader B,Contract C,InvoiceInterest D" +
                             " where A.CustID = B.CustID and B.ContractID = C.ContractID and D.InvCode = B.InvCode" +
                             " AND D.CreateTime >= '" + sDate + " 00:00:00" +
                             "' AND D.CreateTime <= '" + eDate + " 23:59:59'" + where1 +
                             " order by InvCode Desc";
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        /// <summary>
        /// 根据条件获取滞纳金
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        public static DataSet GetInterest(string strWhere)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT InvoiceInterest.InterestID,InvoiceInterest.CreateUserID,InvoiceInterest.CreateTime,InvoiceInterest.ModifyUserID,InvoiceInterest.ModifyTime,InvoiceInterest.OprRoleID,InvoiceInterest.OprDeptID,InvoiceInterest.LateInvID," +
                             " InvoiceInterest.LateInvDetailID,InvoiceInterest.InvCode,InvoiceInterest.IntStartDate,InvoiceInterest.IntEndDate,InvoiceInterest.InterestDay,InvoiceInterest.ExtendDay,InvoiceInterest.LatePayAmt,InvoiceInterest.InterestRate," +
                             " InvoiceInterest.InterestAmt,InvoiceInterest.Note,InvoiceInterest.ChargeTypeID,InvoiceInterest.ContractID,'' as ChargeTypeName,Contract.ContractCode,ConShop.ShopName" +
                             " FROM InvoiceInterest,Contract,ConShop" +
                             " WHERE " + strWhere +
                             " AND InvoiceInterest.ContractID = Contract.ContractID " +
                             " AND Contract.ContractID = ConShop.ContractID";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        public static DataSet GetAccountInterestInvID(string whereStr)
        {
            string str_sql = "SELECT InvoiceHeader.InvID,InvoiceHeader.CurTypeID,InvoiceHeader.ContractID,InvoiceHeader.InvCode,InvoiceHeader.CustName,InvoiceHeader.InvDate,InvoiceHeader.InvPeriod,InvoiceHeader.InvStatus,InvoiceHeader.InvType,InvoiceHeader.IsFirst,InvoiceHeader.InvCurTypeID,InvoiceHeader.InvExRate,InvoiceHeader.InvPayAmt,InvoiceHeader.InvPayAmtL," +
                            " InvoiceHeader.InvAdjAmt,InvoiceHeader.InvAdjAmtL,InvoiceHeader.InvDiscAmt,InvoiceHeader.InvDiscAmtL,InvoiceHeader.InvChngAmt,InvoiceHeader.InvChngAmtL,InvoiceHeader.InvActPayAmt,InvoiceHeader.InvActPayAmtL,InvoiceHeader.InvPaidAmt,InvoiceHeader.InvPaidAmtL,InvoiceHeader.Note,InvoiceHeader.BancthID " +
                            " FROM InvoiceHeader,Contract" +
                            " WHERE InvoiceHeader.InvID " +
                            " NOT IN ( "+
                            " SELECT distinct(InvoiceDetail.InvID) FROM InvoiceDetail,InvoiceHeader,Contract " +
                            " WHERE InvoiceDetail.InvId = InvoiceHeader.InvId " +
                            " AND Contract.ContractID = InvoiceHeader.ContractID " +
                            " AND EXISTS (SELECT ChargeTypeID FROM ChargeType WHERE chargeclass = " + Lease.PotBargain.ChargeType.CHARGECLASS_INTEREST + " AND chargetype.chargeTypeID = InvoiceDetail.chargeTypeID )" + whereStr +
                            " ) and Contract.ContractID = InvoiceHeader.ContractID " + whereStr;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }

        public static DataSet GetInterestInfo(string whereStr)
        {
            string str_sql = "select InterestID,InvoiceInterest.CreateUserID,InvoiceInterest.CreateTime,InvoiceInterest.ModifyUserID,InvoiceInterest.ModifyTime,InvoiceInterest.OprRoleID,InvoiceInterest.OprDeptID,InvoiceInterest.LateInvID,LateInvDetailID,InvCode,IntStartDate,IntEndDate,InterestDay,ExtendDay,LatePayAmt,InterestRate,InterestAmt,InvoiceInterest.Note,ChargeTypeID,InvoiceInterest.ContractID,'' as ChargeTypeName" +
                            " from InvoiceInterest,Contract WHERE InvoiceInterest.ContractID = Contract.ContractID" + whereStr;
            BaseBO baseBO = new BaseBO();
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
                             
        }

    }
}
