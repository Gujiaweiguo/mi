using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Invoice
{
    public class BudgetDetail : BasePO
    {
        private int budgetid = 0;
        private int batchid = 0;
        private int unitid = 0;
        private decimal floorarea = 0;
        private decimal usearea = 0;
        private int shoptypeid = 0;
        private int tradeid = 0;
        private int unittypeid = 0;
        private int budgetyear = 0;
        private DateTime startdate = DateTime.Now;
        private DateTime enddate = DateTime.Now;
        private int chargetypeid = 0;
        private string renttype = "";
        private decimal rentamt = 0;
        private int budgetstatus = 0;
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;

        //预算状态
        public static int BDGSTATUS_TYPE_TEMP = 0;  //草搞状态
        public static int BDGSTATUS_TYPE_INGEAR = 1; //提交审批
        public static int BDGSTATUS_TYPE_ATTREM = 2; //审批通过
        public static int BDGSTATUS_TYPE_CREATE = 3; //生成预算
        public static int BDGSTATUS_TYPE_REJECT = 4; //审批驳回
        public static int BDGSTATUS_TYPE_END = 5;  //作废




        public override String GetTableName()
        {
            return "BudgetDetail";
        }
        public override String GetColumnNames()
        {
            return "BudgetID,BatchID,UnitID,FloorArea,UseArea,ShopTypeID,TradeID,UnitTypeID,BudgetYear,StartDate,EndDate,ChargeTypeID,RentType,RentAmt,BudgetStatus,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetInsertColumnNames()
        {
            return "BudgetID,BatchID,UnitID,FloorArea,UseArea,ShopTypeID,TradeID,UnitTypeID,BudgetYear,StartDate,EndDate,ChargeTypeID,RentType,RentAmt,BudgetStatus,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }

        public override String GetUpdateColumnNames()
        {
            return "BudgetID,BatchID,UnitID,FloorArea,UseArea,ShopTypeID,TradeID,UnitTypeID,BudgetYear,StartDate,EndDate,ChargeTypeID,RentType,RentAmt,BudgetStatus,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public int BudgetID
        {
            get { return budgetid; }
            set { budgetid = value; }
        }
        public int BatchID
        {
            get { return batchid; }
            set { batchid = value; }
        }
        public int UnitID
        {
            get { return unitid; }
            set { unitid = value; }
        }
        public decimal FloorArea
        {
            get { return floorarea; }
            set { floorarea = value; }
        }
        public decimal UseArea
        {
            get { return usearea; }
            set { usearea = value; }
        }
        public int ShopTypeID
        {
            get { return shoptypeid; }
            set { shoptypeid = value; }
        }
        public int TradeID
        {
            get { return tradeid; }
            set { tradeid = value; }
        }
        public int UnitTypeID
        {
            get { return unittypeid; }
            set { unittypeid = value; }
        }
        public int BudgetYear
        {
            get { return budgetyear; }
            set { budgetyear = value; }
        }
        public DateTime StartDate
        {
            get { return startdate; }
            set { startdate = value; }
        }
        public DateTime EndDate
        {
            get { return enddate; }
            set { enddate = value; }
        }
        public int ChargeTypeID
        {
            get { return chargetypeid; }
            set { chargetypeid = value; }
        }
        public string RentType
        {
            get { return renttype; }
            set { renttype = value; }
        }
        public decimal RentAmt
        {
            get { return rentamt; }
            set { rentamt = value; }
        }
        public int BudgetStatus
        {
            get { return budgetstatus; }
            set { budgetstatus = value; }
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