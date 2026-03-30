using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Sell
{
    /// <summary>
    /// 交易商品流水
    /// </summary>
    public class TransSku : BasePO
    {
        private int transSkuID = 0;  //交易ID
        private string transID = "";  //原交易号
        private int shopID = 0;         //商铺ID
        private string shopName = ""; //商铺名称
        private string posID = "";   //POS机ID
        private int userID = 0;         //收银员ID
        private DateTime bizDate = DateTime.Now;  //记帐日期
        private DateTime transTime = DateTime.Now; //交易时间
        private string receiptID = "";  //POS交易号
        private string skuID = "";  //商品ID
        private string skuDesc = "";  //商品描述
        private int trade2ID = 0;  //商品二级类别ID
        private string trade2Name = "";  //商品二级类别名称        
        private int salesMan = 0;  //销售人员
        private string pluCd = "";  //条码
        private int buildingID = 0;  //大楼ID
        private string buildingName = "";  //大楼名称
        private int floorID = 0;  //楼层内码
        private string floorName = "";  //楼层名称
        private int locationID = 0;  //方位内码
        private string locationName = ""; //方位名称
        private int areaID = 0;  //区域内码
        private string areaName = "";  //区域名称
        private int brandID = 0;  //商铺主品牌ID
        private string brandName = "";  //商铺主营牌名称
        private string taxCd = "";  //税率编码
        private decimal qty = 0;  //销售数量
        private decimal orgPrice = 0; //原价格
        private decimal newPrice = 0;  //销售价格
        private decimal itemDisc = 0;  //单品折扣
        private decimal allocDisc = 0;  //整单分配折扣
        private decimal payAmt = 0;  //应付金额
        private decimal tax = 0;  //税额
        private int promotionID = 0;  //惠策略ID
        private decimal prefAmt = 0;  //优惠金额
        private decimal paidAmt = 0;  //实付金额
        private decimal discRate = 0;  //商品扣点
        private decimal costAmt = 0;  //联营成本
        private decimal profitAmt = 0;  //毛利额
        private int transType = 1;  //交易类型
        private int dataSource = 1;  //数据来源
        private string batchID = "";  //批次号
        private int stroeID = 0; //商业项目id
        private int mainlocationID = 0; //大区


        public int TransSkuID
        {
            get { return transSkuID; }
            set { transSkuID = value; }
        }

        public string TransID
        {
            get { return transID; }
            set { transID = value; }
        }

        public int ShopID
        {
            get { return shopID; }
            set { shopID = value; }
        }

        public string ShopName
        {
            get { return shopName; }
            set { shopName = value; }
        }

        public string PosID
        {
            get { return posID; }
            set { posID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public DateTime BizDate
        {
            get { return bizDate; }
            set { bizDate = value; }
        }

        public DateTime TransTime
        {
            get { return transTime; }
            set { transTime = value; }
        }

        public string ReceiptID
        {
            get { return receiptID; }
            set { receiptID = value; }
        }

        public string SkuID
        {
            get { return skuID; }
            set { skuID = value; }
        }

        public string SkuDesc
        {
            get { return skuDesc; }
            set { skuDesc = value; }
        }

        public int Trade2ID
        {
            get { return trade2ID; }
            set { trade2ID = value; }
        }

        public string Trade2Name
        {
            get { return trade2Name; }
            set { trade2Name = value; }
        }

        public int SalesMan
        {
            get { return salesMan; }
            set { salesMan = value; }
        }

        public string PluCd
        {
            get { return pluCd; }
            set { pluCd = value; }
        }

        public int BuildingID
        {
            get { return buildingID; }
            set { buildingID = value; }
        }

        public string BuildingName
        {
            get { return buildingName; }
            set { buildingName = value; }
        }

        public int FloorID
        {
            get { return floorID; }
            set { floorID = value; }
        }

        public string FloorName
        {
            get { return floorName; }
            set { floorName = value; }
        }

        public int LocationID
        {
            get { return locationID; }
            set { locationID = value; }
        }

        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; }
        }

        public int AreaID
        {
            get { return areaID; }
            set { areaID = value; }
        }

        public string AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }

        public int BrandID
        {
            get { return brandID; }
            set { brandID = value; }
        }

        public string BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }

        public string TaxCd
        {
            get { return taxCd; }
            set { taxCd = value; }
        }

        public decimal Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        public decimal OrgPrice
        {
            get { return orgPrice; }
            set { orgPrice = value; }
        }

        public decimal NewPrice
        {
            get { return newPrice; }
            set { newPrice = value; }
        }

        public decimal ItemDisc
        {
            get { return itemDisc; }
            set { itemDisc = value; }
        }

        public decimal AllocDisc
        {
            get { return allocDisc; }
            set { allocDisc = value; }
        }

        public decimal PayAmt
        {
            get { return payAmt; }
            set { payAmt = value; }
        }

        public decimal Tax
        {
            get { return tax; }
            set { tax = value; }
        }

        public int PromotionID
        {
            get { return promotionID; }
            set { promotionID = value; }
        }

        public decimal PrefAmt
        {
            get { return prefAmt; }
            set { prefAmt = value; }
        }

        public decimal PaidAmt
        {
            get { return paidAmt; }
            set { paidAmt = value; }
        }

        public decimal DiscRate
        {
            get { return discRate; }
            set { discRate = value; }
        }

        public decimal CostAmt
        {
            get { return costAmt; }
            set { costAmt = value; }
        }

        public decimal ProfitAmt
        {
            get { return profitAmt; }
            set { profitAmt = value; }
        }

        public int TransType
        {
            get { return transType; }
            set { transType = value; }
        }

        public int DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public string BatchID
        {
            get { return batchID; }
            set { batchID = value; }
        }

        public int StoreID
        {
            get { return stroeID; }
            set { stroeID = value; }
        }

        public int MainLocationID
        {
            get { return mainlocationID; }
            set { mainlocationID = value; }
        }

        public static int TRANSTYPE_CONSUME = 1;  //消费交易
        public static int TRANSTYPE_BACK = 2;  //退货交易

        public static int DATASOURCE_POS = 1;  //POS系统
        public static int DATASOURCE_FILE = 2; //文件导入
        public static int DATASOURCE_WORK = 3; //手工录入

        public override string GetTableName()
        {
            return "TransSku";
        }

        public override string GetColumnNames()
        {
            return "TransSkuID,TransID,ShopID,ShopName,PosID,UserID,BizDate,TransTime,ReceiptID,SkuID,SkuDesc,Trade2ID,Trade2Name,SalesMan,PluCd,BuildingID,"+
                   "BuildingName,FloorID,FloorName,LocationID,LocationName,AreaID,AreaName,BrandID,BrandName,TaxCd,Qty,OrgPrice,NewPrice,ItemDisc,AllocDisc,"+
                   "PayAmt,Tax,PromotionID,PrefAmt,PaidAmt,DiscRate,CostAmt,ProfitAmt,TransType,DataSource,BatchID,StoreID,MainLocationID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
        public override string GetInsertColumnNames()
        {
            return "TransID,ShopID,ShopName,PosID,UserID,BizDate,TransTime,ReceiptID,SkuID,SkuDesc,Trade2ID,Trade2Name,SalesMan,PluCd,BuildingID," +
                   "BuildingName,FloorID,FloorName,LocationID,LocationName,AreaID,AreaName,BrandID,BrandName,TaxCd,Qty,OrgPrice,NewPrice,ItemDisc,AllocDisc," +
                   "PayAmt,Tax,PromotionID,PrefAmt,PaidAmt,DiscRate,CostAmt,ProfitAmt,TransType,DataSource,BatchID,StoreID,MainLocationID";
        }
    }
}
