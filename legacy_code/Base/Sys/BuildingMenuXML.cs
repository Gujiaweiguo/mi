using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Sys
{
    public class BuildingMenuXML : XmlPO
    {
        public override string GetXmlElementNames()
        {
            return "MenuDesc,MenuUrl";
        }

        public override string GetXmlName()
        {
            return "BuildingMenu";
        }

        public override string GetXmlNodeNames()
        {
            return "Menu";
        }

        public override string GetXmlNodeNextNames()
        {
            return "";
        }
        public override string GetDataSetSql()
        {
            return "";
        }


    }
}
