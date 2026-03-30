using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator.Perform
{
    /// <summary>
    /// 会员信息
    /// </summary>
    public class Member : BasePO
    {
        private int membID = 0;    //会员ID
        private string membCode = "";            //会员编码
        private string membName = "";       //会员姓名
        private int certType = 0;      //证件类型
        private string certNum = "";    //证件号
        private DateTime joinDate = DateTime.Now;   //入会日期
        private string addr = "";   //地址
        private string postCode = "";        //邮编
        private string phone = "";   //固定电话
        private string mobile = "";     //移动电话
        private DateTime birthday = DateTime.Now;  //出生日期
        private int sex = 0;    //性别
        private string nationality = "";            //国籍
        private string race = "";       //民族
        private int marStatus = 0;      //婚姻状况
        private DateTime weddingDay = DateTime.Now;    //结婚纪念日
        private int eduLevel = 0;   //教育水平
        private int distance = 0;   //距离范围
        private int vehicle = 0;        //交通工具
        private int salary = 0;   //月收入
        private int occupation = 0;     //职业
        private int position = 0;  //职位
        private int membStatus = 0;  //会员状态
        private string note = "";  //备注

        public int MembID
        {
            get { return membID; }
            set { membID = value; }
        }

        public string MembCode
        {
            get { return membCode; }
            set { membCode = value; }
        }

        public string MembName
        {
            get { return membName; }
            set { membName = value; }
        }

        public int CertType
        {
            get { return certType; }
            set { certType = value; }
        }

        public string CertNum
        {
            get { return certNum; }
            set { certNum = value; }
        }

        public DateTime JoinDate
        {
            get { return joinDate; }
            set { joinDate = value; }
        }

        public string Addr
        {
            get { return addr; }
            set { addr = value; }
        }

        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }

        public int Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        public string Race
        {
            get { return race; }
            set { race = value; }
        }

        public int MarStatus
        {
            get { return marStatus; }
            set { marStatus = value; }
        }

        public DateTime WeddingDay
        {
            get { return weddingDay; }
            set { weddingDay = value; }
        }

        public int EduLevel
        {
            get { return eduLevel; }
            set { eduLevel = value; }
        }

        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public int Vehicle
        {
            get { return vehicle; }
            set { vehicle = value; }
        }

        public int Salary
        {
            get { return salary; }
            set { salary = value; }
        }

        public int Occupation
        {
            get { return occupation; }
            set { occupation = value; }
        }

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public int MembStatus
        {
            get { return membStatus; }
            set { membStatus = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public override string GetTableName()
        {
            return "Member";
        }

        public override string GetColumnNames()
        {
            return "MembID,MembCode,MembName,CertType,CertNum,JoinDate,Addr,PostCode,Phone,Mobile,Birthday,Sex,Nationality,Race,MarStatus,WeddingDay,EduLevel,Distance,Vehicle,Salary,Occupation,Position,MembStatus,Note";
        }

        public override string GetInsertColumnNames()
        {
            return "MembID,MembCode,MembName,CertType,CertNum,JoinDate,Addr,PostCode,Phone,Mobile,Birthday,Sex,Nationality,Race,MarStatus,WeddingDay,EduLevel,Distance,Vehicle,Salary,Occupation,Position,MembStatus,Note";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }
    }
}
