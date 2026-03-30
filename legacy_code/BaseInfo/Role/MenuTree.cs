using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace BaseInfo.Role
{
    public class MenuTree:BasePO
    {
        private int menuID = 0;
        private int pMenuID = 0;
        private string menuName = "";

        public override string GetTableName()
        {
            return "Menu";
        }

        public override String GetColumnNames()
        {
            return "MenuID,PMenuID,MenuName";
        }
        public override string GetUpdateColumnNames()
        {
            return "";
        }

        public int MenuID
        {
            set { menuID = value; }
            get { return menuID; }
        }

        public int PMenuID
        {
            set { pMenuID = value; }
            get { return pMenuID; } 
        }

        public string MenuName
        {
            set { menuName = value; }
            get { return menuName; }
        }

    }
}
