using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Invoice
{
    public class Budget : BasePO
    {
        private int batchid = 0;
        private int unitid = 0;
        private int storeid = 0;
        private int buildingid = 0;
        private int floorid = 0;
        private int areaid = 0;
        private int locationid = 0;
        private int mainlocationid = 0;
        private int tradeid = 0;
        private int shoptypeid = 0;
        private int unittypeid = 0;
        private decimal floorarea = 0;
        private decimal usearea = 0;
        private int budgetyear = 0;
        private DateTime startdate = DateTime.Now;
        private DateTime enddate = DateTime.Now;
        private int chargetypeid = 0;
        private decimal monthamt = 0;
        private decimal yearamt = 0;
        private decimal unitprice = 0;
        private int budgetstatus = 0;
        private int createuserid = 0;
        private DateTime createtime = DateTime.Now;
        private int modifyuserid = 0;
        private DateTime modifytime = DateTime.Now;
        private int oprroleid = 0;
        private int oprdeptid = 0;

        //预算状态
        public static int BudgetSTATUS_NO = 0;  //无效
        public static int BudgetSTATUS_YES = 1; //有效


        public override String GetTableName()
        {
            return "Budget";
        }
        public override String GetColumnNames()
        {
            return "BatchID,UnitID,StoreID,BuildingID,FloorID,AreaID,LocationID,MainLocationID,TradeID,ShopTypeID,UnitTypeID,FloorArea,UseArea,BudgetYear,StartDate,EndDate,ChargeTypeID,MonthAmt,YearAmt,UnitPrice,BudgetStatus,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override String GetUpdateColumnNames()
        {
            return "BatchID,UnitID,StoreID,BuildingID,FloorID,AreaID,LocationID,MainLocationID,TradeID,ShopTypeID,UnitTypeID,FloorArea,UseArea,BudgetYear,StartDate,EndDate,ChargeTypeID,MonthAmt,YearAmt,UnitPrice,BudgetStatus,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
        }
        public override string GetInsertColumnNames()
        {
            return "BatchID,UnitID,StoreID,BuildingID,FloorID,AreaID,LocationID,MainLocationID,TradeID,ShopTypeID,UnitTypeID,FloorArea,UseArea,BudgetYear,StartDate,EndDate,ChargeTypeID,MonthAmt,YearAmt,UnitPrice,BudgetStatus,CreateUserId,CreateTime,ModifyUserId,ModifyTime,OprRoleID,OprDeptID";
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
        public int StoreID
        {
            get { return storeid; }
            set { storeid = value; }
        }
        public int BuildingID
        {
            get { return buildingid; }
            set { buildingid = value; }
        }
        public int FloorID
        {
            get { return floorid; }
            set { floorid = value; }
        }
        public int AreaID
        {
            get { return areaid; }
            set { areaid = value; }
        }
        public int LocationID
        {
            get { return locationid; }
            set { locationid = value; }
        }
        public int MainLocationID
        {
            get { return mainlocationid; }
            set { mainlocationid = value; }
        }
        public int TradeID
        {
            get { return tradeid; }
            set { tradeid = value; }
        }
        public int ShopTypeID
        {
            get { return shoptypeid; }
            set { shoptypeid = value; }
        }
        public int UnitTypeID
        {
            get { return unittypeid; }
            set { unittypeid = value; }
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
        public decimal MonthAmt
        {
            get { return monthamt; }
            set { monthamt = value; }
        }
        public decimal YearAmt
        {
            get { return yearamt; }
            set { yearamt = value; }
        }
        public decimal UnitPrice
        {
            get { return unitprice; }
            set { unitprice = value; }
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
