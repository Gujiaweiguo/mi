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
using Base.Page;


namespace Lease.ChangeLease
{
    public class ConfirmChangeLease : IConfirmVoucher
    {
        /**
         * 确认单据后对数据生效的处理
         */
        public void ValidVoucher(int voucherID, int operType, BaseTrans baseTrans)
        {
            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                ConFormulaMod conFormulaMod = new ConFormulaMod();
                Lease.Contract.Contract contract = new Lease.Contract.Contract();
                ConFormulaH conFormulaH = new ConFormulaH();
                ConFormulaM conFormulaM = new ConFormulaM();
                ConFormulaP conFormulaP = new ConFormulaP();
                Resultset rsMMod = new Resultset();
                Resultset rsPMod = new Resultset();
                BaseBO baseBO = new BaseBO();
                Resultset rs = new Resultset();
                int formulaHID = 0;

                baseBO.WhereClause = "ConFormulaModID=" + voucherID;
                rs = baseBO.Query(conFormulaMod);

                if (rs.Count == 1)
                {

                    conFormulaMod = rs.Dequeue() as ConFormulaMod;

                    baseBO.WhereClause = "ContractID=" + conFormulaMod.ContractID;
                    rs = baseBO.Query(contract);
                    if (rs.Count == 1)
                    {
                        contract = rs.Dequeue() as Lease.Contract.Contract;
                        conFormulaMod.ModUser = contract.CommOper;
                        conFormulaMod.RefID = contract.RefID;
                        conFormulaMod.IsValid = ConFormulaMod.CONFORMULAMOD_BECOME_EFFECTIVE;
                        conFormulaMod.ModDate = DateTime.Now;

                        baseTrans.WhereClause = "ConFormulaModID=" + voucherID;
                        if (baseTrans.Update(conFormulaMod) == -1)
                        {
                            /*添加失败*/
                            baseTrans.Rollback();
                            return;
                        }

                        /*查找结算公式ID*/
                        baseBO.WhereClause = "ContractID=" + conFormulaMod.ContractID;
                        rs = baseBO.Query(conFormulaH);
                        if (rs.Count > 0)
                        {
                            conFormulaH = rs.Dequeue() as ConFormulaH;
                            formulaHID = conFormulaH.FormulaID;
                        }

                        /*删除结算公式抽成*/
                        baseTrans.WhereClause = "FormulaID=" + formulaHID;
                        if (baseTrans.Delete(conFormulaP) == -1)
                        {
                            /*删除失败*/
                            baseTrans.Rollback();
                            return;
                        }

                        /*删除结算公式保底*/
                        baseTrans.WhereClause = "FormulaID=" + formulaHID;
                        if (baseTrans.Delete(conFormulaM) == -1)
                        {
                            /*删除失败*/
                            baseTrans.Rollback();
                            return;
                        }


                        /*删除结算公式*/
                        baseTrans.WhereClause = "ContractID=" + conFormulaMod.ContractID;
                        if (baseTrans.Delete(conFormulaH) == -1)
                        {
                            /*删除失败*/
                            baseTrans.Rollback();
                            return;
                        }

                        baseTrans.Commit();

                        baseTrans.BeginTrans();

                        /*添加变更结算公式*/
                        baseBO.WhereClause = "ConFormulaModID=" + voucherID;
                        rs = baseBO.Query(new ConFormulaHMod());
                        if (rs.Count > 0)
                        {
                            foreach (ConFormulaHMod conFormulaHMod in rs)
                            {
                                conFormulaH.FormulaID = BaseApp.GetFormulaID();
                                conFormulaH.ChargeTypeID = conFormulaHMod.ChargeTypeID;
                                conFormulaH.ContractID = conFormulaMod.ContractID;
                                conFormulaH.FStartDate = conFormulaHMod.FStartDate;
                                conFormulaH.FEndDate = conFormulaHMod.FEndDate;
                                conFormulaH.FormulaType = conFormulaHMod.FormulaType;
                                conFormulaH.TotalArea = conFormulaHMod.TotalArea;
                                conFormulaH.UnitPrice = conFormulaHMod.UnitPrice;
                                conFormulaH.BaseAmt = conFormulaHMod.BaseAmt;
                                conFormulaH.FixedRental = conFormulaHMod.FixedRental;
                                conFormulaH.RateType = conFormulaHMod.RateType;
                                conFormulaH.PcentOpt = conFormulaHMod.PcentOpt;
                                conFormulaH.MinSumOpt = conFormulaHMod.MinSumOpt;
                                if (baseTrans.Insert(conFormulaH) == -1)
                                {
                                    /*添加失败*/
                                    baseTrans.Rollback();
                                    return;
                                }

                                /*添加结算公式保底*/
                                baseBO.WhereClause = "FormulaID=" + conFormulaHMod.FormulaID;
                                rsMMod = baseBO.Query(new ConFormulaMMod());
                                if (rsMMod.Count > 0)
                                {
                                    foreach (ConFormulaMMod conFormulaMMod in rsMMod)
                                    {
                                        conFormulaM.FormulaID = conFormulaH.FormulaID;
                                        conFormulaM.ConFormulaMID = BaseApp.GetConFormulaMID();
                                        conFormulaM.MinSum = conFormulaMMod.MinSum;
                                        conFormulaM.SalesTo = conFormulaMMod.SalesTo;

                                        if (baseTrans.Insert(conFormulaM) == -1)
                                        {
                                            /*添加失败*/
                                            baseTrans.Rollback();
                                            return;
                                        }
                                    }
                                }

                                /*添加结算公式抽成*/
                                baseBO.WhereClause = "FormulaID=" + conFormulaHMod.FormulaID;
                                rsPMod = baseBO.Query(new ConFormulaPMod());
                                if (rsMMod.Count > 0)
                                {
                                    foreach (ConFormulaPMod conFormulaPMod in rsPMod)
                                    {
                                        conFormulaP.FormulaID = conFormulaH.FormulaID;
                                        conFormulaP.ConFormulaPID = BaseApp.GetConFormulaPID();
                                        conFormulaP.Pcent = conFormulaPMod.Pcent;
                                        conFormulaP.SalesTo = conFormulaPMod.SalesTo;

                                        if (baseTrans.Insert(conFormulaP) == -1)
                                        {
                                            /*添加失败*/
                                            baseTrans.Rollback();
                                            return;
                                        }
                                    }
                                }
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
