using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace BaseInfo.Role
{
    public class AuthShop:BasePO
    {

        private int authShopID = 0;
        private int buildingID = 0;
        private int floorID = 0;
        private int userID = 0;
        private int createUserID = 0; //创建用户代码
        private DateTime createTime = DateTime.Now; //创建时间
        private int modifyUserID = 0; //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;  //最后修改时间
        private int oprRoleID = 0; //操作用户的角色代码
        private int oprDeptID = 0;  //操作用户的机构代码
        private int storeID = 0;

        public override String GetTableName()
        {
            return "AuthShop";
        }

        public override String GetColumnNames()
        {
            return "AuthShopID,BuildingID,FloorID,UserID,StoreID";
        }

        public override string GetInsertColumnNames()
        {
            return "AuthShopID,BuildingID,FloorID,UserID,CreateUserID,CreateTime,ModifyUserID," +
                    "ModifyTime,OprRoleID,OprDeptID,StoreID";
        }

        public override string GetUpdateColumnNames()
        {
            return "";
        }



        public int AuthShopID
        {
            get { return authShopID; }
            set { authShopID = value; }
        }

        public int BuildingID
        {
            get { return buildingID; }
            set { buildingID = value; }
        }

        public int FloorID
        {
            get { return floorID; }
            set { floorID = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public int CreateUserID
        {
            get { return createUserID; }
            set { createUserID = value; }
        }
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }
        public int ModifyUserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }
        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }
        public int OprRoleID
        {
            get { return oprRoleID; }
            set { oprRoleID = value; }
        }
        public int OprDeptID
        {
            get { return oprDeptID; }
            set { oprDeptID = value; }
        }
        public int StoreID
        {
            set { storeID = value; }
            get { return storeID; }
        }
    }
}
