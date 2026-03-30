using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Bill
{
    public class BillInfo:BasePO
    {
        private int billNum;
        private string shangHuName;
        private string shangHuMessage;
        private string tel;
        private string yingYeNum;
        private decimal hire;
        private string shangHuPlace;

        public override String GetTableName()
        {
            return "Bill";
        }

        public override String GetColumnNames()
        {
            return "BillNum,ShangHuName,ShangHuMessage,Tel,YingYeNum,Hire,ShangHuPlace";
        }

        public override String GetUpdateColumnNames()
        {
            return "BillNum,ShangHuName,ShangHuMessage,Tel,YingYeNum,Hire,ShangHuPlace,ValidAfterConfirm,PrintAfterConfirm";
        }

        public int BillNum
        {
            get { return this.billNum; }
            set { this.billNum = value; }
        }
        public string ShangHuName
        {
            get { return this.shangHuName; }
            set { this.shangHuName = value; }
        }
        public string ShangHuMessage
        {
            get { return this.shangHuMessage; }
            set { this.shangHuMessage = value; }
        }
        public string Tel
        {
            get { return this.tel; }
            set { this.tel = value; }
        }
        public string YingYeNum
        {
            get { return this.yingYeNum; }
            set { this.yingYeNum = value; }
        }
        public decimal Hire
        {
            get { return this.hire; }
            set { this.hire = value; }
        }
        public string ShangHuPlace
        {
            get { return this.shangHuPlace; }
            set { this.shangHuPlace = value; }
        }
    }
}
