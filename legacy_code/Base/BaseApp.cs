using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.Data;

using Base.Util;
using Base.Biz;

namespace Base
{
    /**
     * 该类实现了应用程序中基本的函数
     */
    public class BaseApp
    {
        /**
         * 存放ID的哈希表
         */
        private static Hashtable ids = new Hashtable();

        /**
         * 获得部门内码，在增加部门时使用
         */
        public static int GetDeptID()
        {
            return GetID("Dept", "DeptID");
        }

                /**
        *广告位ID
        */
        public static int GetConAdBoardID()
        {
            return GetID("ConAdBoard", "ConAdBoardID");
        }

        /**
        *场地ID
        */
        public static int GetConAreaID()
        {
            return GetID("ConArea", "ConAreaID");
        }
        public static int GetBuilderAuditingStatus()
        {
            return GetID("BuilderAuditing", "Status");
        }
        /**
         * 获得用户内码，在增加用户时使用
         */
        public static int GetUserID()
        {
            return GetID("[Users]", "UserID");
        }
        /**
         * 获得角色内码，在增加角色时使用
         */
        public static int GetRoleID()
        {
            return GetID("Role", "RoleID");
        }
        /**
         * 获得部门权限内码，在增加部门，设定部门权限时使用
         */
        public static int GetDeptAuthID()
        {
            return GetID("DeptAuth", "DeptAuthID");
        }
        
        /**
         * 获得工作流内码，在定义工作流时使用
         */
        public static int GetWrkFlwID()
        {
            return GetID("WrkFlw", "WrkFlwID");
        }

        /**
         * 获得用户定义内码
         */
        public static int GetCustTypeID()
        {
            return GetID("CustType", "CustTypeID");
        }

        /**
         * 获得潜在客户证照内码
         */
        public static int GetCustLicenseID()
        {
            return GetID("PotCustLicense", "CustLicenseID");
        }

        /**
         * 获得潜在客户通讯录内码
         */
        public static int GetCustContactID()
        {
            return GetID("PotCustContact", "CustContactID");
        }

        /**
         * 获得客户通讯录内码
         */
        public static int GetCustContactIDD()
        {
            return GetID("CustContact", "CustContactID");
        }

        /**
         * 获得潜在客户信息内码
         */
        public static int GetCustID()
        {
            return GetID("PotCustomer", "CustID");
        }

        /**
         * 获得潜在客户信息编码
         */
        public static int GetCustCode()
        {
            return GetID("PotCustomer", "CustCode");
        }

        /**
         * 获得客户证照内码
         */
        public static int GetCustLicenseIDD()
        {
            return GetID("CustLicense", "CustLicenseID");
        }

        /**
         * 获得意向商铺代码内码
         */
        public static int GetPotShopID()
        {
            return GetID("PotShop", "PotShopID");
        }

        /**
         * 获得谈判记录内码
         */
        public static int GetPalaverID()
        {
            return GetID("CustPalaver", "PalaverID");
        }

        /**
         * 获得大楼内码
         */
        public static int GetBuildingID()
        {
            return GetID("Building", "BuildingID");
        }

        /**
         * 获得楼层内码
         */
        public static int GetFloorsID()
        {
            return GetID("Floors", "FloorID");
        }

        /**
         * 获得方位内码
         */
        public static int GetLocationID()
        {
            return GetID("Location", "LocationID");
        }

        /**
         * 获得经营区域内码
         */
        public static int GetAreaID()
        {
            return GetID("Area", "AreaID");
        }

        /**
         * 获得单元内码
         */
        public static int GetUnitID()
        {
            return GetID("Unit", "UnitID");
        }

        /**
         * 获得用户合同内码
         */
        public static int GetContractID()
        {
            return GetID("Contract", "ContractID");
        }

        /**
         * 获得结算公式内码
         */
        public static int GetFormulaID()
        {
            return GetID("ConFormulaH", "FormulaID");
        }

        /**
         * 获得商铺信息内码
         */
        public static int GetShopID()
        {
            return GetID("ConShop", "ShopID");
        }

        /**
         * 获得广告位内码
         */
        public static int GetAdBoardID()
        {
            return GetID("AdBoard", "AdBoardID");
        }


        /**
         * 获得业务组内码
         */
        public static int GetBizGrpID()
        {
            return GetID("BizGrp", "BizGrpID");
        }

        /**
         * 获得广告合同内码
         */
        public static int GetAdContractID()
        {
            return GetID("AdContract", "AdContractID");
        }

        /**
         * 获得场地类型内码
         */
        public static int GetAreaTypeID()
        {
            return GetID("AreaType", "AreaTypeID");
        }
        /**
         * 获得工作流运行实体序列号，在插入工作流运行实体时使用
         */
        public static int GetWrkFlwSequence()
        {
            return GetID("WrkFlwEntity", "Sequence");
        }

        /**
         * 获得工作流节点代码
         */
        public static int GetWrkFlwNodeID()
        {
            return GetID("WrkFlwNode", "NodeID");
        }

        /**
         * 经营类别代码
         */
        public static int GetTradeID()
        {
            return GetID("TradeRelation", "TradeID");
        }

        /**
         * 费用类型代码
         */
        public static int GetChargeTypeID()
        {
            return GetID("ChargeType", "ChargeTypeID");
        }

        /**
         * 品牌类型代码
         */
        public static int GetBrandID()
        {
            return GetID("ConShopBrand", "BrandID");
        }

        /**
         * 结算单代码
         */
        public static int GetInvID()
        {
            return GetID("InvoiceHeader", "InvID");
        }

        /**
         * 结算单明细代码
         */
        public static int GetInvDetailID()
        {
            return GetID("InvoiceDetail", "InvDetailID");
        }


        /**
        * 结算公式抽成代码
        */
        public static int GetConFormulaPID()
        {
            return GetID("ConFormulaP", "ConFormulaPID");
        }

        /**
        * 结算公式保底代码
        */
        public static int GetConFormulaModID()
        {
            return GetID("ConFormulaMod", "ConFormulaModID");
        }

        /**
        * 合同租金公式变更单代码
        */
        public static int GetConFormulaMID()
        {
            return GetID("ConFormulaM", "ConFormulaMID");
        }

        /**
        * 合同租金公式变更单租金公式代码
        */
        public static int GetFormulaIDMod()
        {
            return GetID("ConFormulaHMod", "FormulaID");
        }

        /**
        * 合同续约变更单代码
        */
        public static int GetConOverTimeID()
        {
            return GetID("ConOvertimeBill", "ConOverTimeID");
        }


        /**
        * 合同终约单代码
        */
        public static int GetConTermD()
        {
            return GetID("ConTerminateBill", "ConTermD");
        }


        /**
       * 费用单代码
       */
        public static int GetChgID()
        {
            return GetID("Charge", "ChgID");
        }

        /**
       * 费用明细代码
       */
        public static int GetChgDetID()
        {
            return GetID("ChargeDetail", "ChgDetID");
        }

        /// <summary>
        /// 代收款信息代码
        /// </summary>
        /// <returns></returns>
        public static int GetPayInID()
        {
            return GetID("PayIn","PayInID");
        }

        /**
        * 余款处理代码
        */
        public static int GetSurBalID()
        {
            return GetID("SurBal", "SurBalID");
        }

        /**
        * 余款处理明细代码
        */
        public static int GetSurBalDelID()
        {
            return GetID("SurBalDel", "SurBalDelID");
        }


        /**
        * 结算付款单代码
        */
        public static int GetInvPayID()
        {
            return GetID("InvoicePay", "InvPayID");
        }

        /**
        * 结算付款单明细代码
        */
        public static int GetInvPayDetID()
        {
            return GetID("InvoicePayDetail", "InvPayDetID");
        }

        /**
        * 代收款返还信息代码
        */
        public static int GetPayOutID()
        {
            return GetID("PayOut", "PayOutID");
        }

        /**
        * 押金返还代码
        */
        public static int GetDepBalID()
        {
            return GetID("DepBal", "DepBalID");
        }

        /**
        * 押金返还明细信息代码
        */
        public static int GetDepBalDetID()
        {
            return GetID("DepBalDel", "DepBalDetID");
        }

        /**
        * 调整单代码
        */
        public static int GetInvAdjID()
        {
            return GetID("InvAdj", "InvAdjID");
        }

        /**
        * 调整单明细代码
        */
        public static int GetInvAdjDetID()
        {
            return GetID("InvAdjDet", "InvAdjDetID");
        }

        /**
        * 余款表代码
        */
        public static int GetSurID()
        {
            return GetID("Surplus", "SurID");
        }

        /**
        * 优惠结算单明细代码
        */
        public static int GetDiscDetID()
        {
            return GetID("InvDiscDet", "DiscDetID");
        }

        /**
        * 优惠结算单代码
        */
        public static int GetDiscID()
        {
            return GetID("InvDisc", "DiscID");
        }

        /**
        * 取消结算单代码
        */
        public static int GetInvCelID()
        {
            return GetID("InvCancel", "InvCelID");
        }

        /**
        * 银行卡交易明细代码
        */
        public static int GetBankTransID()
        {
            return GetID("BankTransDet", "BankTransID");
        }

        /**
        * 交易金额流水代码
        */
        public static int GetTransSkuMedID()
        {
            return GetID("TransSkuMedia", "TransSkuMedID");
        }

        /**
       * 交易金额流水代码
       */
        public static int GetTransSkuID()
        {
            return GetID("TransSku", "TransSkuID");
        }

        /**
      * TransHeader(TransID)
      */
        public static int GetTransID()
        {
            return GetID("TransHeader", "Transid");
        }

        /**
          * 商铺类型代码
          */
        public static int GetShopTypeID()
        {
            return GetID("ShopType", "ShopTypeID");
        }

        /**
          * 合同续约租金类别代码
          */
        public static int GetFormulaIDHOvtm()
        {
            return GetID("ConFormulaHOvtm", "FormulaID");
        }

        /**
          * 合同续约租金抽成公式代码
          */
        public static int GetFormulaIDPOvtm()
        {
            return GetID("ConFormulaPOvtm", "FormulaID");
        }

        /**
          * 合同续约保底公式代码
          */
        public static int GetFormulaIDMOvtm()
        {
            return GetID("ConFormulaMOvtm", "FormulaID");
        }

        /**
          * 结算公式保底公式变更代码
          */
        public static int GetConFormulaMModID()
        {
            return GetID("ConFormulaMMod", "ConFormulaMModID");
        }

        /**
          * 结算公式抽成公式变更代码
          */
        public static int GetConFormulaPModID()
        {
            return GetID("ConFormulaPMod", "ConFormulaPModID");
        }

        /**
          * 其他费用ID
          */
        public static int GetOtherChargeHID()
        {
            return GetID("OtherChargeH", "OtherChargeHID");
        }

        /**
          * 其他费用明细ID
          */
        public static int GetOtherChargeDID()
        {
            return GetID("OtherChargeD", "OtherChargeDID");
        }

        /**
         * 费用计算日志ID
         */
        public static int GetChargeCountID()
        {
            return GetID("ChargeCountLog", "ChargeCountID");
        }

        /**
        * 结算主表批次号
        */
        public static int GetInvoiceHeaderBancthID()
        {
            return GetID("InvoiceHeader", "BancthID");
        }

        /**
        * 赠品ID
        */
        public static int GetGiftID()
        {
            return GetID("Gift", "GiftID");
        }


        /**
        * 赠品发放活动ID
        */
        public static int GetActID()
        {
            return GetID("GiftActivity", "ActID");
        }

        /**
        * 赠品兑换ID
        */
        public static int GetRedeemID()
        {
            return GetID("RedeemH", "RedeemID");
        }

        /**
       * 币种ID
       */
        public static int GetCurTypeID()
        {
            return GetID("CurrencyType", "CurTypeID");
        }

        /**
         * 汇率ID
         */
        public static int GetExRateID()
        {
            return GetID("CurExRate", "ExRateID");
        }

        /**
         * 费用计算日志批次号
         */
        public static int GetBancthID()
        {
            return GetID("ChargeCountLog", "BancthID");
        }

        /**
         * 租赁合同终约ID
         */
        public static int GetConTerID()
        {
            return GetID("ConTerminateBill", "ConTerID");
        }

        /**
         * 硬件信息ID
         */
        public static int GetHdwID()
        {
            return GetID("ShopHdw", "HdwID");
        }


        /**
         * 租赁合同相关信息修改ID
         */
        public static int GetConModID()
        {
            return GetID("ContractMod", "ConModID");
        }

        /**
         * 租赁合同相关信息修改商铺ID
         */
        public static int GetConModShopID()
        {
            return GetID("ConShopMod", "ShopModID");
        }

        /**
         * 硬件类型ID
         */
        public static int GetHdwTypeID()
        {
            return GetID("HdwType", "HdwTypeID");
        }

        /**
         * 平面广告ID
         */
        public static int GetPrintsID()
        {
            return GetID("Prints", "PrintsID");
        }


        /**
         * 电视广告ID
         */
        public static int GetTVID()
        {
            return GetID("TV", "TVID");
        }

        /**
         * 广播广告ID
         */
        public static int GetRadioID()
        {
            return GetID("Radio", "RadioID");
        }

        /**
         * 网络广告ID
         */
        public static int GetInternetID()
        {
            return GetID("Internet", "InternetID");
        }

        /**
         * 网络广告ID
         */
        public static int GetThemeID()
        {
            return GetID("Theme", "ThemeID");
        }

        /**
         * 网络广告ID
         */
        public static int GetAnpID()
        {
            return GetID("AnPMaster", "AnpID");
        }

        /**
         * 平面媒体信息ID
         */
        public static int GetMPrintsID()
        {
            return GetID("MPrints", "MPrintsID");
        }

        /**
         * 电视台信息ID
         */
        public static int GetMTVID()
        {
            return GetID("MTV", "MTVID");
        }

        /**
         * 广播电台信息ID
         */
        public static int GetMRadioID()
        {
            return GetID("MRadio", "MRadioID");
        }

        /**
         * 网络信息ID
         */
        public static int GetMInternetID()
        {
            return GetID("MInternet", "MInternetID");
        }

        /**
         * 美陈信息ID
         */
        public static int GetMDisplayID()
        {
            return GetID("MDisplay", "MDisplayID");
        }

        /**
         * 其他信息ID
         */
        public static int GetMActivityID()
        {
            return GetID("MActivity", "MActivityID");
        }

        /**
         * 返款结算单明细ID
         */
        public static int GetinvJVDetailID()
        {
            return GetID("InvoiceJVDetail", "invJVDetailID");
        }

        /**
        * 滞纳金明细ID
        */
        public static int GetInterestID()
        {
            return GetID("invoiceInterest", "InterestID");
        }

        /**
        * 滞纳金利率ID
        */
        public static int GetInterestRateID()
        {
            return GetID("InterestRate", "InterestRateID");
        }

        /**
       * 商品编码
       */
        public static int GetSkuID()
        {
            return GetSkuMasterSkuID("SkuMaster", "SkuID");
        }

        /**
       * 租赁结算单表头设置编码
       */
        public static int GetInvoiceParaID()
        {
            return GetID("InvoicePara", "InvoiceParaID");
        }

        /**
       * 联营结算单表头设置编码
       */
        public static int GetInvoiceJVParaID()
        {
            return GetID("InvoiceJVPara", "InvoiceJVParaID");
        }

       ///**
       //* 会员编码
       //*/
       // public static int GetMemberId()
       // {
       //     return GetID("Member", "MembId");
       // }

        /**
      * 赠品发放ID
      */
        public static int GetGiftTransID()
        {
            return GetID("FreeGiftTrans", "GiftTransID");
        }

        /**
         * 获取会员号
         */
        public static int GetMembID()
        {
            return GetMemberID("Member", "MembID");
        }

        /**
       *小票兑换赠品ID
       */
        public static int GetExID()
        {
            return GetID("ExTrans", "ExID");
        }

        /**
       *用户数据权限楼层ID
       */
        public static int GetAuthShopID()
        {
            return GetID("AuthShop", "AuthShopID");
        }

        /**
       *用户数据权限
       */
        public static int GetAuthContractID()
        {
            return GetID("AuthContract", "AuthContractID");
        }

        /**
      *积分规则ID
      */
        public static int GetBonusFID()
        {
            return GetID("BonusF", "BonusFID");
        }

        /**
        *库存点ID
        */
        public static int GetGiftStockID()
        {
            return GetID("GiftStock", "StockID");
        }

        /**
       *ActivityID
       */
        public static int GetActivityID()
        {
            return GetID("Activity", "ActivityID");
        }

        /**
       *库存点ID
       */
        public static int GetFavorID()
        {
            return GetID("Favor", "FavorID");
        }

        /**
       *库存点ID
       */
        public static int GetConsumeID()
        {
            return GetID("ConsumeInterest", "ConsumeID");
        }



        /**
         * 从指定资源文件中，获取指定的资源字符串
         */
        public static String GetResourceString(String fileName,String key)
        {
            String dir = ParamManager.DEPLOY_PATH + "/Web/App_GlobalResources/";

            ResXResourceReader rsxr = new ResXResourceReader(dir + fileName + ".resx");
            try
            {
                foreach (DictionaryEntry d in rsxr)
                {
                    if (d.Key.Equals(key))
                    {
                        return d.Value.ToString();
                    }
                }
                return null;
            }
            finally
            {
                rsxr.Close();
            }

        }
        
        /**
         * 通过从指定的数据表，采用最大值加1的方法，获取ID
         */
        public static int GetID(String table, String field)
        {
            lock (ids)
            {

                int id = 0;
                //如果hashtable中已经有该id，则取出来，加1
                String key = (table + "_" + field).ToLower();
                if (ids[key] != null)
                {
                    id = (int)ids[key] + 1;
                    ids[key] = id;
                    return id;
                }
                //如果hashtable里没有该id，则从数据表中取最大值，加1
                String sql = "select max(" + field + ") from " + table;
                BaseBO bo = new BaseBO();
                DataSet ds = bo.QueryDataSet(sql);
                //如果数据表中有数据，则取出后加1
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    id = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
                }
                //如果数据表没有数据（一般在首次使用后出现这种情况），则取缺省值101，100以内用作备用值
                else
                {
                    id = 101;
                }

                ids[key] = id;
                return id;
            }
        }

        /**
         * 通过从指定的数据表，采用最大值加1的方法，获取ID ID默认为从10001开始
         */
        public static int GetCustumerID(String table, String field)
        {
            lock (ids)
            {

                int id = 0;
                //如果hashtable中已经有该id，则取出来，加1
                String key = (table + "_" + field).ToLower();
                if (ids[key] != null)
                {
                    id = (int)ids[key] + 1;
                    ids[key] = id;
                    return id;
                }
                //如果hashtable里没有该id，则从数据表中取最大值，加1
                String sql = "select max(" + field + ") from " + table;
                BaseBO bo = new BaseBO();
                DataSet ds = bo.QueryDataSet(sql);
                //如果数据表中有数据，则取出后加1
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    id = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
                }
                //如果数据表没有数据（一般在首次使用后出现这种情况），则取缺省值10001，10001以内用作备用值
                else
                {
                    id = 10001;
                }

                ids[key] = id;
                return id;
            }
        }

        /**
        * 通过从指定的数据表，采用最大值加1的方法，获取ID ID默认为从100001开始
        */
        public static int GetSkuMasterSkuID(String table, String field)
        {
            lock (ids)
            {

                int id = 0;
                //如果hashtable中已经有该id，则取出来，加1
                String key = (table + "_" + field).ToLower();
                if (ids[key] != null)
                {
                    id = (int)ids[key] + 1;
                    ids[key] = id;
                    return id;
                }
                //如果hashtable里没有该id，则从数据表中取最大值，加1
                String sql = "select max(Convert(int," + field + ")) from " + table;
                BaseBO bo = new BaseBO();
                DataSet ds = bo.QueryDataSet(sql);
                //如果数据表中有数据，则取出后加1
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    id = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
                }
                //如果数据表没有数据（一般在首次使用后出现这种情况），则取缺省值10001，10001以内用作备用值
                else
                {
                    id = 100001;
                }

                ids[key] = id;
                return id;
            }
        }

        /**
       * 通过从指定的数据表，采用最大值加1的方法，获取ID ID默认为从100001开始
       */
        public static int GetMemberID(String table, String field)
        {
            lock (ids)
            {

                int id = 0;
                //如果hashtable中已经有该id，则取出来，加1
                String key = (table + "_" + field).ToLower();
                if (ids[key] != null)
                {
                    id = (int)ids[key] + 1;
                    ids[key] = id;
                    return id;
                }
                //如果hashtable里没有该id，则从数据表中取最大值，加1
                String sql = "select max(" + field + ") from " + table;
                BaseBO bo = new BaseBO();
                DataSet ds = bo.QueryDataSet(sql);
                //如果数据表中有数据，则取出后加1
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    id = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
                }
                //如果数据表没有数据（一般在首次使用后出现这种情况），则取缺省值10001，10001以内用作备用值
                else
                {
                    id = 100000001;
                }

                ids[key] = id;
                return id;
            }
        }



        /**
      *系统参数维护ID
      */
        public static int GetSMSParaID()
        {
            return GetID("SMSPara", "SMSParaID");
        }

        /**
          *系统参数维护合同编码
          */
        public static int GetSMSParaNextContractCode()
        {
            return GetSMSPara("SMSPara", "NextContractCode");
        }

        /**
          *系统参数维护客户编码
          */
        public static int GetSMSParaNextCustCode()
        {
            return GetSMSPara("SMSPara", "NextCustCode");
        }

        /**
          *系统参数维护商品编码
          */
        public static int GetSMSParaNextSkuID()
        {
            return GetSMSPara("SMSPara", "NextSkuID");
        }

        /**
          *系统参数维护收银员编码
          */
        public static int GetSMSParaNextTPUserID()
        {
            return GetSMSPara("SMSPara", "NextTPUserID");
        }

        /**
         * 通过从指定的数据表，采用最大值加1的方法，获取ID
         */
        public static int GetSMSPara(String table, String field)
        {
            lock (ids)
            {
                BaseBO bo = new BaseBO();
                int id = 0;
                //如果hashtable中已经有该id，则取出来，加1
                String key = (table + "_" + field).ToLower();
                if (ids[key] != null)
                {
                    id = (int)ids[key] + 1;
                    ids[key] = id;

                    if (bo.ExecuteUpdate("Update " + table + " Set " + field + "=" + id) == -1)
                    {
                        return -1;
                    }

                    return id;

                }
                //如果hashtable里没有该id，则从数据表中取最大值，加1
                String sql = "select max(" + field + ") from " + table;

                DataSet ds = bo.QueryDataSet(sql);
                //如果数据表中有数据，则取出后加1
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    id = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;

                    if (bo.ExecuteUpdate("Update " + table + " Set " + field + "=" + id) == -1)
                    {
                        return -1;
                    }
                }
                //如果数据表没有数据（一般在首次使用后出现这种情况），则取缺省值101，100以内用作备用值
                else
                {
                    id = 1;
                }

                ids[key] = id;
                return id;
            }
        }

        /**
        *邮件发送列表编码
        */
        public static int GetWrkFlwMailListID()
        {
            return GetID("WrkFlwMailList", "WrkFlwMailListID");
        }


    }




}
