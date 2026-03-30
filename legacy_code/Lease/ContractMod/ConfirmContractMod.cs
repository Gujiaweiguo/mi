using System;
using System.Collections.Generic;
using System.Text;

using WorkFlow;
using WorkFlow.Func;
using WorkFlow.WrkFlw;
using Base.Biz;
using Base.DB;
using Base;
using RentableArea;

namespace Lease.ContractMod
{
    public class ConfirmContractMod : IConfirmVoucher
    {
        /**
        * 确认单据后对数据生效的处理
        */
        public void ValidVoucher(int voucherID, int operType, BaseTrans trans)
        {
            if (operType == FuncApp.OPER_TYPE_CONFIRM)
            {
                ContractUpdate contractUpdate = new ContractUpdate();
                ConShop.ConShop conShop = new ConShop.ConShop();
                ConShop.ConShopUnit conShopUnit = new ConShop.ConShopUnit();
                ConLease conLease = new ConLease();

                ContractMod contractMod = new ContractMod();
                ConLeaseMod conLeaseMod = new ConLeaseMod();
                ConShopMod conShopMod = new ConShopMod();
                ConShopUnitMod conShopUnitMod = new ConShopUnitMod();
                UnitsStutas unitsStutas = new UnitsStutas();

                Resultset rs = new Resultset();
                Resultset rsConLease = new Resultset();
                Resultset rsConShop = new Resultset();
                Resultset rsUnitStatus = new Resultset();
                Resultset rsUnit = new Resultset();
                BaseBO baseBO = new BaseBO();

                int shopID=0;

                /*合同信息*/
                baseBO.WhereClause = "ConModID = " + voucherID;
                rs = baseBO.Query(contractMod);

                if (rs.Count == 1)
                {
                    contractMod = rs.Dequeue() as ContractMod;

                    contractUpdate.AdditionalItem = contractMod.AdditionalItem;
                    contractUpdate.ChargeStartDate = contractMod.ChargeStartDate;
                    contractUpdate.ConEndDate = contractMod.ConEndDate;
                    contractUpdate.ConStartDate = contractMod.ConStartDate;
                    contractUpdate.ContractTypeID = contractMod.ContractTypeID;
                    contractUpdate.EConURL = contractMod.EConURL;
                    contractUpdate.NorentDays = contractMod.NorentDays;
                    contractUpdate.Note = contractMod.Note;
                    contractUpdate.Notice = contractMod.Notice;
                    contractUpdate.Penalty = contractMod.Penalty;
                    contractUpdate.PenaltyItem = contractMod.PenaltyItem;
                    contractUpdate.RefID = contractMod.RefID;
                    contractUpdate.RootTradeID = contractMod.RootTradeID;
                    contractUpdate.TradeID = contractMod.TradeID;

                    trans.WhereClause = "ContractID = " + contractMod.ContractID;
                    if (trans.Update(contractUpdate) == -1)
                    {
                        trans.Rollback();
                        return;
                    }
                }

                /*相关条款*/
                baseBO.WhereClause = "ConModID = " + voucherID;
                rsConLease = baseBO.Query(conLeaseMod);
                if (rsConLease.Count == 1)
                {
                    conLeaseMod = rsConLease.Dequeue() as ConLeaseMod;

                    conLease.BalanceMonth = conLeaseMod.BalanceMonth;
                    conLease.BillCycle = conLeaseMod.BillCycle;
                    conLease.CurTypeID = conLeaseMod.CurTypeID;
                    conLease.IfPrepay = conLeaseMod.IfPrepay;
                    conLease.IntDay = conLeaseMod.IntDay;
                    conLease.LatePayInt = conLeaseMod.LatePayInt;
                    conLease.MonthSettleDays = conLeaseMod.MonthSettleDays;
                    conLease.PayTypeID = conLeaseMod.PayTypeID;
                    conLease.SettleMode = conLeaseMod.SettleMode;
                    conLease.TaxRate = conLeaseMod.TaxRate;
                    conLease.TaxType = conLeaseMod.TaxType;
                    conLease.RentInc = "";

                    trans.WhereClause = "ContractID = " + contractMod.ContractID;
                    if (trans.Update(conLease) == -1)
                    {
                        trans.Rollback();
                        return;
                    }
                }

                /*商铺信息*/
                baseBO.WhereClause = "a.ShopTypeID = b.ShopTypeID And ConModID = " + voucherID;
                rsConShop = baseBO.Query(conShopMod);

                if (rsConShop.Count > 0)
                {
                    foreach (ConShopMod conShopMods in rsConShop)
                    {
                        conShop.AreaId = conShopMods.AreaId;
                        conShop.BrandID = conShopMods.BrandID;
                        conShop.BuildingID = conShopMods.BuildingID;
                        conShop.ContactorName = conShopMods.ContactorName;
                        conShop.FloorID = conShopMods.FloorID;
                        conShop.LocationID = conShopMods.LocationID;
                        conShop.RefID = conShopMods.RefID;
                        conShop.RentArea = conShopMods.RentArea;
                        conShop.ShopCode = conShopMods.ShopCode;
                        conShop.ShopEndDate = conShopMods.ShopEndDate;
                        conShop.ShopName = conShopMods.ShopName;
                        conShop.ShopStartDate = conShopMods.ShopStartDate;
                        conShop.ShopStatus = conShopMods.ShopStatus;
                        conShop.ShopTypeID = conShopMods.ShopTypeID;
                        conShop.Tel = conShopMods.Tel;               
                        conShop.CreateTime = DateTime.Now;
                        conShop.ModifyTime = DateTime.Now;
                        conShop.ContractID = conShopMods.ContractID;

                        if (conShopMods.ShopId == 0)
                        {
                            conShop.ShopId = BaseApp.GetShopID();
                            shopID = conShop.ShopId;
                            if (trans.Insert(conShop) == -1)
                            {
                                trans.Rollback();
                                return;
                            }
                        }
                        else
                        {
                            shopID = conShopMods.ShopId;
                            BaseTrans baseTrans = new BaseTrans();

                            baseTrans.BeginTrans();

                            baseTrans.WhereClause = "ShopID=" + conShopMods.ShopId;
                            if (baseTrans.Update(conShop) == -1)
                            {
                                baseTrans.Rollback();
                                return;
                            }

                            baseTrans.Commit();
                            
                            /*查询更改商铺，把原单元标志设置成未出租状态*/
                            baseBO.WhereClause = "ShopID=" + conShopMods.ShopId;
                            rsUnitStatus = baseBO.Query(conShopUnit);

                            if (rsUnitStatus.Count > 0)
                            {
                                foreach (ConShop.ConShopUnit conShopUnits in rsUnitStatus)
                                {
                                    unitsStutas.UnitStatus = Units.BLANKOUT_STATUS_INVALID;
                                    trans.WhereClause = "UnitID = " + conShopUnits.UnitID;
                                    if (trans.Update(unitsStutas) == -1)
                                    {
                                        trans.Rollback();
                                        return;
                                    }
                                }
                            }


                            /*删除原有的商铺单元*/
                            baseBO.WhereClause = "ShopID=" + conShopMods.ShopId;
                            if (baseBO.Delete(conShopUnit) == -1)
                            {
                                return;
                            }
                        }

                        baseBO.WhereClause = "ShopModID = " + conShopMods.ShopModID;
                        rsUnit = baseBO.Query(conShopUnitMod);

                        if (rsUnit.Count > 0)
                        {
                            foreach (ConShopUnitMod conUnitMod in rsUnit)
                            {
                                conShopUnit.UnitID = conUnitMod.UnitID;
                                conShopUnit.ShopID = shopID;
                                conShopUnit.RentStatus = ConShop.ConShopUnit.RENTSTATUS_TYPE_YES;

                                if (trans.Insert(conShopUnit) == -1)
                                {
                                    trans.Rollback();
                                    return;
                                }

                                /*查询修改商铺，把原单元标志设置成已出租状态*/
                                unitsStutas.UnitStatus = Units.BLANKOUT_STATUS_LEASEOUT;
                                trans.WhereClause = "UnitID = " + conShopUnit.UnitID;
                                if (trans.Update(unitsStutas) == -1)
                                {
                                    trans.Rollback();
                                    return;
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
