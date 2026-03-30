using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

using Base.DB;

namespace Base.Util
{
    /**
     * 配置参数管理器，完成配置参数的解析及管理
     */
    public class ParamManager
    {
        public static String DEPLOY_PATH = "E:/work/mi_net/Code";
        //public static String DEPLOY_PATH = "c:/work/mi_net/Code";
        private static String paramFile = DEPLOY_PATH + "/Web/Config.xml";
        private static Hashtable dbTypes = null;
        private static Hashtable dbParams = null;
        private static DateTime lastModified = System.DateTime.Now;

        /**
         * 根据数据库ID，获得相应的配置参数对象
         */
        public static DBParam GetDBParam(String id)
        {
            Init();

            return (DBParam)dbParams[id];
        }

        /**
         * 获得缺省的数据库配置参数对象
         */
        public static DBParam GetDefaultDBParam()
        {
            Init();

            ICollection keys = dbParams.Keys;
            foreach (Object obj in keys)
            {
                if (((DBParam)dbParams[obj]).IsDefault)
                {
                    return ((DBParam)dbParams[obj]);
                }
            }
            throw new DBException("db parameter error,no default db");
        }

        /**
         * 获得全部数据库的配置参数对象数组
         */
        public static DBParam[] GetAllDBParams()
        {
            Init();

            DBParam[] ps = new DBParam[dbParams.Count];
            ICollection keys = dbParams.Keys;
            int count = 0;
            foreach( Object obj in keys ) {
                ps[count] = (DBParam)dbParams[obj];
                count++;
            }
            return ps;
        }
        /**
         * 判断是否需要初始化配置参数（首次运行或配置文件发生修改，则重新初始化配置参数）
         */
        private static bool ShouldInit()
        {
            if (dbParams == null || lastModified != File.GetLastWriteTime(paramFile))
            {
                lastModified = File.GetLastAccessTime(paramFile);
                return true;
            }
            return false;
        }

        /**
         * 根据配置文件，初始化数据库参数
         */
        private static void Init()
        {
            if (!ShouldInit())
            {
                return;
            }

            if (dbParams == null)
            {
                dbParams = new Hashtable();
            }
            else
            {
                dbParams.Clear();
            }

            if (dbTypes == null)
            {
                dbTypes = new Hashtable();
            }
            else
            {
                dbTypes.Clear();
            }

            try
            {
                //lock (dbParams)
                //{
                //    int flag = 0;
                //    DBParam param = new DBParam();
                //    //XmlNode dbNode = dbRoot.ChildNodes[i];

                //    param.ID = "midb";
                //    //param.ID = a;
                //    param.DBType = "sqlserver";

                //    param.Provider = ".";
                //    param.TypeDescription = "microsotf sqlserver database";

                //    //if (dbNode.Attributes["default"] != null)
                //    //{
                //    param.IsDefault = true;
                //    //}
                //    param.DataSource = "MI_NET";
                //    param.UserID = "sa";
                //    param.Password = "sasa4321";
                //    param.Description = "jy database";

                //    //if (!dbParams.ContainsKey(param.ID))
                //    //dbParams.Add(param.ID, param);

                //    dbParams.Clear();
                //    foreach (DictionaryEntry de in dbParams)
                //    {
                //        if ((string)de.Key == param.ID)
                //        {
                //            flag = 1;
                //        }
                //    }
                //    if (flag == 0)
                //    {
                //        dbParams.Add(param.ID, param);
                //    }
                //}
                XmlDocument doc = new XmlDocument();
                doc.Load(paramFile);
                XmlElement element = doc.DocumentElement;

                //数据库类型根结点
                XmlNode typeRoot = element.FirstChild;
                if (typeRoot.HasChildNodes)
                {
                    for (int i = 0; i < typeRoot.ChildNodes.Count; i++)
                    {
                        DBType type = new DBType();
                        XmlNode typeNode = typeRoot.ChildNodes[i];
                        type.ID = typeNode.Attributes["id"].Value;
                        type.Provider = typeNode["provider"].InnerText;
                        type.Description = typeNode["description"].InnerText;
                        dbTypes.Add(type.ID, type);
                    }
                }

                //数据库根节点
                XmlNode dbRoot = typeRoot.NextSibling;

                if (dbRoot.HasChildNodes)
                {
                    for (int i = 0; i < dbRoot.ChildNodes.Count; i++)
                    {
                        DBParam param = new DBParam();
                        XmlNode dbNode = dbRoot.ChildNodes[i];

                        param.ID = dbNode.Attributes["id"].Value;
                        param.DBType = dbNode.Attributes["type-id"].Value;

                        param.Provider = ((DBType)dbTypes[param.DBType]).Provider;
                        param.TypeDescription = ((DBType)dbTypes[param.DBType]).Description;

                        if (dbNode.Attributes["default"] != null)
                        {
                            param.IsDefault = dbNode.Attributes["default"].Value.Equals("true");
                        }
                        param.DataSource = dbNode["datasource"].InnerText;
                        param.UserID = dbNode["userid"].InnerText;
                        param.Password = dbNode["password"].InnerText;
                        param.Description = dbNode["description"].InnerText;

                        dbParams.Add(param.ID, param);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Log("Exception when init parameter parser.", ex);
            }

        }

    };

    class DBType
    {
        private String id = null;
        private String provider = null;
        private String decription = null;

        public String ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public String Provider
        {
            get { return this.provider; }
            set { this.provider = value; }
        }

        public String Description
        {
            get { return this.decription; }
            set { this.decription = value; }
        }
    }
}
