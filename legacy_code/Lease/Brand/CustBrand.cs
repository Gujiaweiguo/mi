using System;
using System.Collections.Generic;
using System.Text;
using Base.Biz;
using Base.DB;

namespace Lease.Brand
{
    /// <summary>
    /// 商户经营品牌  add by lcp at 2009-3-19
    /// </summary>
    public class CustBrand:BasePO
    {
        private int custBrandId = 0;//主键
        private int custId = 0;//商户内码
        private int tradeId = 0;//经营业态内码
        private int brandId = 0;//品牌内码
        private int operateTypeId = 0;//品牌经营方式内码
        private string consumerSex = "";//受众（消费者）性别
        private string consumerAge = "";//受众（消费者）年龄段
        private decimal avgAmt = 0;//客均价（客单价）
        private string note = "";//备注
        private int createUserId = 0;//创建用户内码
        private DateTime createTime=DateTime.Now;//创建时间
        private int modifyUserId = 0;//修改用户内码
        private DateTime modifyTime=DateTime.Now;//修改时间
        private int oprRoleID = 0;//创建修改用户角色内码
        private int oprDeptID = 0;//创建修改用户部门内码

        private int clientLevelId = 0;//客层id
        private string priceRange = "";//价格区间


        public override string GetTableName()
        {
            return "CustBrand";
        }
        public override string GetColumnNames()
        {
            return "CustBrandId,CustId,TradeId,BrandId,OperateTypeId,ConsumerSex,ConsumerAge,AvgAmt,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID,ClientLevelId,PriceRange";
        }
        public override string GetUpdateColumnNames()
        {
            return "CustId,TradeId,BrandId,OperateTypeId,ConsumerSex,ConsumerAge,AvgAmt,Note,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID,ClientLevelId,PriceRange";
        }
        public override string GetQuerySql()
        {
            return "select CustBrandId,(select tradename from traderelation where tradeid=CustBrand.tradeid) as TradeName,(select brandname from ConShopBrand where brandid=CustBrand.brandid) as BrandName from CustBrand";
        }

        public int CustBrandId
        {
            set { custBrandId = value; }
            get { return custBrandId; }
        }
        public int CustId
        {
            set { custId = value; }
            get { return custId; }
        }
        public int TradeId
        {
            set { tradeId = value; }
            get { return tradeId; }
        }
        public int BrandId
        {
            set { brandId = value; }
            get { return brandId; }
        }
        public int OperateTypeId
        {
            set { operateTypeId = value; }
            get { return operateTypeId; }
        }
        public string ConsumerSex
        {
            set { consumerSex = value; }
            get { return consumerSex; }
        }
        public string ConsumerAge
        {
            set { consumerAge = value; }
            get { return consumerAge; }
        }
        public decimal AvgAmt
        {
            set{ avgAmt=value;}
            get{return avgAmt;}
        }
        public string Note
        {
            set { note = value; }
            get { return note; }
        }
        public int CreateUserId
        {
            set { createUserId = value; }
            get { return createUserId; }
        }
        public DateTime CreateTime
        {
            set { createTime = value; }
            get { return createTime; }
        }
        public int ModifyUserId
        {
            set { modifyUserId = value; }
            get { return modifyUserId; }
        }
        public DateTime ModifyTime
        {
            set { modifyTime = value; }
            get { return modifyTime; }
        }
        public int OprRoleID
        {
            set { oprRoleID = value; }
            get { return oprRoleID; }
        }
        public int OprDeptID
        {
            set { oprDeptID = value; }
            get { return oprDeptID; }
        }
        public int ClientLevelId
        {
            set { clientLevelId = value; }
            get { return clientLevelId; }
        }
        public string PriceRange
        {
            set { priceRange = value; }
            get { return priceRange; }
        }
    }
}
