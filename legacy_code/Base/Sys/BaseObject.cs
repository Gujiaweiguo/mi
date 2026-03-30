using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Base.Util;
using Base.DB;

namespace Base.Sys
{
    /**
     * 该类实现了一些基础性功能 
     */
    public class BaseObject
    {
        /**
         * 获取自身对象的属性值
         */
        public String GetSelf()
        {
            return BaseObject.GetObject(this);
        }

        public void LogSelf()
        {
            Logger.Log(this.GetSelf());
        }

        public void TraceSelf()
        {
            Logger.Trace(this.GetSelf());
        }

        public Object GetProperty(String name)
        {
            Type type = this.GetType();
            PropertyInfo info = type.GetProperty(name);
            if (info != null)
            {
                return info.GetValue(this, null);
            }
            throw new DBException("no property:" + name + " defined in class:" + type);
        }

        /**
         * 获得制定对象的属性值
         */
        public static String GetObject(Object obj)
        {
            StringBuilder builder = new StringBuilder();
            Type type = obj.GetType();
            PropertyInfo[] infos = type.GetProperties();
            int count = 0;
            foreach (PropertyInfo info in infos)
            {
                builder.Append(info.Name).Append(":").Append(info.GetValue(obj, null));
                if (count < infos.Length - 1)
                {
                    builder.Append("-");
                }
                count++;
            }
            return builder.ToString();
        }

    }
}
