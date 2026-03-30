using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Base.XML
{
    public class BuildingMenuXMLInfo : BasePO
    {
        private string menuDesc = null;
        private string menuUrl = null;
        private int userID = 0;

        public BuildingMenuXMLInfo(int userid)
        {
            userID =userid;
        }


        public override String GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "";
        }
        public override String GetInsertColumnNames()
        {
            return "";
        }

        public override String GetUpdateColumnNames()
        {
            return "";
        }
        public override string GetQuerySql()
        {
            return "select MenuDesc,MenuUrl from (select func.funcid,func.BaseInfo as MenuDesc,replace(func.FuncUrl,'65537',func.funcid) AS MenuUrl from func,users,RoleAuth ,userrole " +
                    "where users.userid=userrole.userid and userrole.roleid=roleauth.roleid and "+
                    "roleauth.funcid=func.funcid "+
                    "and FuncType=3  and userrole.userid=" + userID + ") as menu order by menu.funcid";
        }

        public string MenuDesc
        {
            get { return this.menuDesc; }
            set { this.menuDesc = value; }
        }
        public string MenuUrl
        {
            get { return this.menuUrl; }
            set { this.menuUrl = value; }
        }
        public int UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

    }
}
