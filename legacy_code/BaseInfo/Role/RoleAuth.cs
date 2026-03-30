using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Role
{
    public class RoleAuth:BasePO
    {
        private int roleid = 0;
        private int funcid = 0;
        private int subAuthLevel = 13;
        private int isPrint = 0;
        private int isExport = 0;

        /// <summary>
        /// 查看
        /// </summary>
        public static int SUBAUTHLEVEL_EXAMINE = 2;
        /// <summary>
        /// 增加
        /// </summary>
        public static int SUBAUTHLEVEL_ADD = 3;
        /// <summary>
        /// 修改
        /// </summary>
        public static int SUBAUTHLEVEL_MODIFY = 5;
        /// <summary>
        /// 删除/作废
        /// </summary>
        public static int SUBAUTHLEVEL_DEL = 8;
        /// <summary>
        /// 默认恢复
        /// </summary>
        public static int SUBAUTHLEVEL_DEFAULT = 13;
        /// <summary>
        /// 不可打印
        /// </summary>
        public static int ISPRINT_NO = 0;
        /// <summary>
        /// 可打印
        /// </summary>
        public static int ISPRINT_YES = 1;
        /// <summary>
        /// 不可导出
        /// </summary>
        public static int ISEXPORT_NO = 0;
        /// <summary>
        /// 可导出
        /// </summary>
        public static int ISEXPORT_YES = 1;



        public override string GetTableName()
        {
            return "RoleAuth";
        }

        public override String GetColumnNames()
        {
            return "RoleID,FuncID,SubAuthLevel,IsPrint,IsExport";
        }
        public override string GetUpdateColumnNames()
        {
            return "SubAuthLevel,IsPrint,IsExport";
        }

        public int RoleID
        {
            set { roleid = value; }
            get { return roleid; }
        }

        public int FuncID
        {
            set { funcid = value; }
            get { return funcid; }
        }

        public int SubAuthLevel
        {
            set { subAuthLevel = value; }
            get { return subAuthLevel; }
        }

        public int IsPrint
        {
            set { isPrint = value; }
            get { return isPrint; }
        }

        public int IsExport
        {
            set { isExport = value; }
            get { return isExport; }
        }
    }
}
