using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Sys
{
    public abstract class XmlPO
    {
        /**
        * 字段之间的分隔符，在需要获得字段数组时使用
        */
        public static char[] FIELD_SPLITTER ={ ',' };

        /**
         * 返回生成Xml的文件名称
         */
        public abstract String GetXmlName();

        /**
         * 返回要生成节点Xml的元素字段
         */
        public abstract String GetXmlElementNames();

        /**
         * 返回要生节点的Xml字段
         */
        public abstract String GetXmlNodeNames();

        /**
         * 返回要生二级节点的Xml字段
         */
        public abstract String GetXmlNodeNextNames();

        /**
         * 返回要生二级节点的Xml字段
         */
        public abstract String GetDataSetSql();


 
    }
}
