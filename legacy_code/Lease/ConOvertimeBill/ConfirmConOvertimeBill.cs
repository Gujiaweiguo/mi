using System;
using System.Collections.Generic;
using System.Text;

using WorkFlow.Func;
using Base.Biz;
using Base.DB;
using Base;
using Lease;
using Lease.PotCustLicense;
using Lease.ConShop;
using Lease.PotBargain;
using Lease.Formula;
using Lease.Customer;
using Lease.Contract;
using WorkFlow.WrkFlw;
using WorkFlow;
using WorkFlow.Uiltil;
using Lease.ChangeLease;
using Lease.ConOvertimeBill;


namespace Lease.ConOvertimeBill
{
    public class ConfirmConOvertimeBill : IConfirmVoucher
    {
        /**
         * 确认单据后对数据生效的处理
         */
        public void ValidVoucher(int voucherID, int operType, BaseTrans baseTrans)
        {
            ConOvertimeBill conOvertimeBill = new ConOvertimeBill();
            ConFormulaHOvtm conFormulaHOvtm = new ConFormulaHOvtm();
            ConFormulaH conFormulaH = new ConFormulaH();
            ConFormulaPOvtm conFormulaPOvtm = new ConFormulaPOvtm();
            ConFormulaP conFormulaP = new ConFormulaP();
            ConFormulaMOvtm conFormulaMOvtm = new ConFormulaMOvtm();
            ConFormulaM conFormulaM = new ConFormulaM();
            

            Resultset rs = new Resultset();
            Resultset rsOvtm_M_P = new Resultset();
            Resultset rsOverTimeBill = new Resultset();
            Resultset rsConShop = new Resultset();

            BaseBO baseBO = new BaseBO();

            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                baseBO.WhereClause = "ConOverTimeID = " + voucherID;
                rs = baseBO.Query(conFormulaHOvtm);
                if (rs.Count > 0)
                {

                    foreach (ConFormulaHOvtm conFormulaHOvtmE in rs)
                    {
                        conFormulaH.FormulaID = BaseApp.GetFormulaID();
                        conFormulaH.ChargeTypeID = conFormulaHOvtmE.ChargeTypeID;
                        conFormulaH.ContractID = conFormulaHOvtmE.ContractID;
                        conFormulaH.FStartDate = conFormulaHOvtmE.FStartDate;
                        conFormulaH.FEndDate = conFormulaHOvtmE.FEndDate;
                        conFormulaH.FormulaType = conFormulaHOvtmE.FormulaType;
                        conFormulaH.TotalArea = conFormulaHOvtmE.TotalArea;
                        conFormulaH.UnitPrice = conFormulaHOvtmE.UnitPrice;
                        conFormulaH.BaseAmt = conFormulaHOvtmE.BaseAmt;
                        conFormulaH.FixedRental = conFormulaHOvtmE.FixedRental;
                        conFormulaH.RateType = conFormulaHOvtmE.RateType;
                        conFormulaH.PcentOpt = conFormulaHOvtmE.PcentOpt;
                        conFormulaH.MinSumOpt = conFormulaHOvtmE.PcentOpt;
                        conFormulaH.CreateTime = DateTime.Now;
                        conFormulaH.ModifyTime = DateTime.Now;

                        if (baseTrans.Insert(conFormulaH) < 1)
                        {
                            baseTrans.Rollback();
                            return;
                        }

                        /*根据结算公式续约ID查找对应的结算公式插入正式的结算公式表中*/
                        baseBO.WhereClause = "ConFormulaPOvtmID=" + conFormulaHOvtmE.ConOverTimeID;

                        rsOvtm_M_P = baseBO.Query(conFormulaPOvtm);

                        if (rsOvtm_M_P.Count > 0)
                        {
                            foreach (ConFormulaPOvtm conFormulaPOvtmE in rsOvtm_M_P)
                            {
                                conFormulaP.ConFormulaPID = BaseApp.GetConFormulaPID();
                                conFormulaP.FormulaID = conFormulaH.FormulaID;
                                conFormulaP.SalesTo = conFormulaPOvtmE.SalesTo;
                                conFormulaP.Pcent = conFormulaPOvtmE.Pcent;

                                if (baseTrans.Insert(conFormulaP) < 1)
                                {
                                    baseTrans.Rollback();
                                    return;
                                }
                            }
                        }

                        baseBO.WhereClause = "ConFormulaMOvtmID=" + conFormulaHOvtm.ConOverTimeID;

                        rsOvtm_M_P = baseBO.Query(conFormulaMOvtm);

                        if (rsOvtm_M_P.Count > 0)
                        {
                            foreach (ConFormulaMOvtm ConFormulaMOvtmE in rsOvtm_M_P)
                            {
                                conFormulaM.ConFormulaMID = BaseApp.GetConFormulaMID();
                                conFormulaM.FormulaID = conFormulaH.FormulaID;
                                conFormulaM.SalesTo = ConFormulaMOvtmE.SalesTo;
                                conFormulaM.MinSum = ConFormulaMOvtmE.MinSum;

                                if (baseTrans.Insert(conFormulaM) < 1)
                                {
                                    baseTrans.Rollback();
                                    return;
                                }

                            }
                        }
                    }

                    baseBO.WhereClause = "a.ContractID=b.ContractID And b.CustID= c.CustID And ConOverTimeID = " + voucherID;
                    rsOverTimeBill = baseBO.Query(new ConOvertimeBill());

                    if (rsOverTimeBill.Count == 1)
                    {
                        conOvertimeBill = rsOverTimeBill.Dequeue() as ConOvertimeBill;

                        if (baseTrans.ExecuteUpdate("Update Contract Set ConEndDate = '" + conOvertimeBill.NewConEndDate + "' Where ContractID=" + conOvertimeBill.ContractID) < 1)
                        {
                            baseTrans.Rollback();
                            return;
                        }

                        baseBO.WhereClause = "a.ShopTypeID=b.ShopTypeID And ContractID = " + conOvertimeBill.ContractID;
                        rsConShop = baseBO.Query(new ConShop.ConShop());

                        foreach (ConShop.ConShop conShop in rsConShop)
                        {
                            if (baseTrans.ExecuteUpdate("Update ConShop Set ShopEndDate = '" + conOvertimeBill.NewConEndDate + "' Where ShopID = " + conShop.ShopId) == -1)
                            {
                                baseTrans.Rollback();
                                return;
                            }
                        }
                    }
                }
            }
        }

        /**
         * 确认单据后对数据打印的处理
         */
        public void PrintVoucher(int voucherID, int operType, BaseTrans trans)
        {
            //BillInfo objBillInfo = new BillInfo();
            //String sql = "Update Bill set PrintAfterConfirm = " + WrkFlwNode.PRINT_AFTER_CONFIRM_YES + " where BillNum=" + voucherID;
            //trans.ExecuteUpdate(sql);
        }
    }
}
