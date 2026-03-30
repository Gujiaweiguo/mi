using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Lease.CustLicense
{
    public class CustOprInfo : BasePO
    {
        private int custoprinfoid = 0;
        private int custid = 0;
        private int custtypeid = 0;
        private string operateareas = "";
        private int shopnumber = 0;
        private decimal areasalesrate = 0;
        private decimal basediscount = 0;
        private string promotearea = "";
        private string promotecost = "";
        private int planshopnumber = 0;
        private string planarea = "";
        private DateTime plandate = DateTime.Now;
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;
        public override String GetTableName()
        {
            return "CustOprInfo";
        }
        public override String GetColumnNames()
        {
            return "CustOprInfoId,CustID,CustTypeID,OperateAreas,ShopNumber,AreaSalesRate,BaseDiscount,PromoteArea,PromoteCost,planShopNumber,planArea,planDate,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "CustTypeID,OperateAreas,ShopNumber,AreaSalesRate,BaseDiscount,PromoteArea,PromoteCost,planShopNumber,planArea,planDate,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public int CustOprInfoId
        {
            get { return custoprinfoid; }
            set { custoprinfoid = value; }
        }
        public int CustID
        {
            get { return custid; }
            set { custid = value; }
        }
        public int CustTypeID
        {
            get { return custtypeid; }
            set { custtypeid = value; }
        }
        public string OperateAreas
        {
            get { return operateareas; }
            set { operateareas = value; }
        }
        public int ShopNumber
        {
            get { return shopnumber; }
            set { shopnumber = value; }
        }
        public decimal AreaSalesRate
        {
            get { return areasalesrate; }
            set { areasalesrate = value; }
        }
        public decimal BaseDiscount
        {
            get { return basediscount; }
            set { basediscount = value; }
        }
        public string PromoteArea
        {
            get { return promotearea; }
            set { promotearea = value; }
        }
        public string PromoteCost
        {
            get { return promotecost; }
            set { promotecost = value; }
        }
        public int planShopNumber
        {
            get { return planshopnumber; }
            set { planshopnumber = value; }
        }
        public string planArea
        {
            get { return planarea; }
            set { planarea = value; }
        }
        public DateTime planDate
        {
            get { return plandate; }
            set { plandate = value; }
        }
        public int CreateUserId
        {
            get { return createuserid; }
            set { createuserid = value; }
        }
        public DateTime CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }
        public int ModifyUserId
        {
            get { return modifyuserid; }
            set { modifyuserid = value; }
        }
        public DateTime ModifyTime
        {
            get { return modifytime; }
            set { modifytime = value; }
        }
        public int OprRoleID
        {
            get { return oprroleid; }
            set { oprroleid = value; }
        }
        public int OprDeptID
        {
            get { return oprdeptid; }
            set { oprdeptid = value; }
        }
    }
}