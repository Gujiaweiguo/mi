using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.AdContract
{
    public class ConAdBoard : BasePO
    {
        private int _conadboardid = 0;
        private int _contractid = 0;
        private int _createuserid = 0;
        private DateTime _createtime = DateTime.Now;
        private int _modifyuserid = 0;
        private DateTime _modifytime = DateTime.Now;
        private int _oprroleid = 0;
        private int _oprdeptid = 0;
        private int _adboardid = 0;
        private string _conadboarddesc = "";
        private int _conadboardstatus = 0;
        private DateTime _conadboardstartdate = DateTime.Now;
        private DateTime _conadboardenddate = DateTime.Now;
        private decimal _rentarea = 0;
        private int _airtime = 0;
        private string _freq = "";
        private int _freqdays = 0;
        private string _freqmon = FreqWeek_No;
        private string _freqtue = FreqWeek_No;
        private string _freqwed = FreqWeek_No;
        private string _freqthu = FreqWeek_No;
        private string _freqfri = FreqWeek_No;
        private string _freqsat = FreqWeek_No;
        private string _freqsun = FreqWeek_No;
        private int _betweento = 1;
        private int _betweenfr = 1;
        private int _storeid = 0;
        private int _buildingid = 0;

        public static string Freq_Day = "D";  //间隔类型
        public static string Freq_Month = "M";
        public static string Freq_Week = "W";

        public static string FreqWeek_Yes="Y";
        public static string FreqWeek_No="N";

        public static int BLANKOUT_STATUS_INVALID = 0;     //无效

        public static int BLANKOUT_STATUS_LEASEOUT = 1;    // 有效

        public static int BLANKOUT_STATUS_PAUSE = 2;    // 待审批
        //是否作废
        public static int[] GetBlankOutStatus()
        {
            int[] blankOutStaus = new int[2];
            blankOutStaus[0] = BLANKOUT_STATUS_INVALID;
            blankOutStaus[1] = BLANKOUT_STATUS_LEASEOUT;
            return blankOutStaus;
        }

        public static String GetBlankOutStatusDesc(int blankOutStaus)
        {
            if (blankOutStaus == BLANKOUT_STATUS_INVALID)
            {
                return "BizGrp_NO";
            }
            if (blankOutStaus == BLANKOUT_STATUS_LEASEOUT)
            {
                return "BizGrp_YES";
            }
            return "未知";
        }

        public override String GetTableName()
        {
            return "ConAdBoard";
        }

        public override String GetColumnNames()
        {
            return "ConAdBoardID,ContractID,AdBoardID,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID," +
                    "ConAdBoardDesc,ConAdBoardStatus,ConAdBoardStartDate,ConAdBoardEndDate,RentArea,Airtime,Freq,FreqDays,FreqMon,FreqTue,FreqWed,FreqThu,FreqFri,FreqSat,FreqSun,BetweenFr,BetweenTo,StoreID,BuildingID";
        }

        public override String GetInsertColumnNames()
        {
            return "ConAdBoardID,ContractID,AdBoardID,CreateUserID,CreateTime,OprRoleID,OprDeptID," +
                    "ConAdBoardDesc,ConAdBoardStatus,ConAdBoardStartDate,ConAdBoardEndDate,RentArea,Airtime,Freq,FreqDays,FreqMon,FreqTue,FreqWed,FreqThu,FreqFri,FreqSat,FreqSun,BetweenFr,BetweenTo,StoreID,BuildingID";
        }

        public override String GetUpdateColumnNames()
        {
            return "ContractID,AdBoardID,ModifyUserID,ModifyTime,OprRoleID,OprDeptID," +
                    "ConAdBoardDesc,ConAdBoardStatus,ConAdBoardStartDate,ConAdBoardEndDate,RentArea,Airtime,Freq,FreqDays,FreqMon,FreqTue,FreqWed,FreqThu,FreqFri,FreqSat,FreqSun,BetweenFr,BetweenTo,StoreID,BuildingID";
        }

        /// <summary>
        /// 广告合同主键
        /// </summary>
        public int ConAdBoardID
        {
            set { _conadboardid = value; }
            get { return _conadboardid; }
        }
        /// <summary>
        /// 合同ID
        /// </summary>
        public int ContractID
        {
            set { _contractid = value; }
            get { return _contractid; }
        }
        /// <summary>
        /// 广告位ID
        /// </summary>
        public int AdBoardID
        {
            set { _adboardid = value; }
            get { return this._adboardid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreateUserID
        {
            set { _createuserid = value; }
            get { return _createuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ModifyUserID
        {
            set { _modifyuserid = value; }
            get { return _modifyuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifyTime
        {
            set { _modifytime = value; }
            get { return _modifytime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OprRoleID
        {
            set { _oprroleid = value; }
            get { return _oprroleid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int OprDeptID
        {
            set { _oprdeptid = value; }
            get { return _oprdeptid; }
        }
   
        /// <summary>
        /// 
        /// </summary>
        public string ConAdBoardDesc
        {
            set { _conadboarddesc = value; }
            get { return _conadboarddesc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ConAdBoardStatus
        {
            set { _conadboardstatus = value; }
            get { return _conadboardstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ConAdBoardStartDate
        {
            set { _conadboardstartdate = value; }
            get { return _conadboardstartdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ConAdBoardEndDate
        {
            set { _conadboardenddate = value; }
            get { return _conadboardenddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal RentArea
        {
            set { _rentarea = value; }
            get { return _rentarea; }
        }
        /// <summary>
        /// 播放时长(秒)
        /// </summary>
        public int Airtime
        {
            set { _airtime = value; }
            get { return this._airtime; }            
        }
        /// <summary>
        /// 间隔类型（M,D,W）
        /// </summary>
        public string Freq
        {
            set { _freq = value; }
            get { return this._freq; }
        }
        /// <summary>
        /// 间隔天数
        /// </summary>
        public int FreqDays
        {
            set { _freqdays  = value; }
            get { return this._freqdays; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FreqMon
        {
            set { _freqmon=value;}
            get { return _freqmon;}
        }
        public string FreqTue
        {
            set {_freqtue=value;}
            get { return _freqtue;}
        }
        public string FreqWed
        {
            set { _freqwed=value;}
            get { return _freqwed; }
        }
        public string FreqThu
        {
            set { _freqthu = value; }
            get { return _freqthu; }
        }
        public string FreqFri
        {
            set { _freqfri = value; }
            get { return _freqfri; }
        }
        public string FreqSat
        {
            set { _freqsat = value; }
            get { return _freqsat; }
        }
        public string FreqSun
        {
            set { _freqsun = value; }
            get { return _freqsun; }
        }
        public int BetweenFr
        {
            set { _betweenfr = value; }
            get { return _betweenfr; }
        }
        public int BetweenTo
        {
            set { _betweento = value; }
            get { return _betweento; }
        }
        public int StoreID
        {
            set { _storeid = value; }
            get { return _storeid; }
        }
        public int BuildingID
        {
            set { _buildingid = value; }
            get { return _buildingid; }
        }
    }
}
