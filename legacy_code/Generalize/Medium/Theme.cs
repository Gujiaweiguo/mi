using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;
namespace Generalize.Medium
{
    /*»î¶¯Ãû³Æ*/
    public class Theme:BasePO
    {
        private int themeID = 0;
        private string themeNm = "";
        private string remark = "";

        public override string GetTableName()
        {
            return "Theme";
        }

        public override string GetColumnNames()
        {
            return "ThemeID,ThemeNm,Remark";
        }

        public override string GetInsertColumnNames()
        {
            return "ThemeID,ThemeNm,Remark";
        }

        public override string GetUpdateColumnNames()
        {
            return "ThemeNm,Remark";
        }

        public int ThemeID
        {
            get { return themeID; }
            set { themeID = value; }
        }

        public string ThemeNm
        {
            get { return themeNm; }
            set { themeNm = value; }
        }

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }
}
