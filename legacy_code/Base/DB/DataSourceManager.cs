using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.Common;

using Base.Util;

namespace Base.DB
{
    /**
     * 数据源管理器，负责管理所有数据源
     */
    class DataSourceManager
    {   /**
         * 数据源列表
         */
        private Hashtable dataSources = null;
        /**
         * 数据源管理器实例
         */
        private static DataSourceManager instance = null;

        private DataSourceManager()
        {
            Init();
        }

        public static DataSourceManager GetInstance()
        {
            if (instance == null)
            {
                instance = new DataSourceManager();
            }
            return instance;
        }
        /**
         * 获得缺省数据源
         */
        public IDataSource GetDataSource()
        {
            return GetDataSource(ParamManager.GetDefaultDBParam().ID);
        }
        /**
         * 获得指定数据源
         */
        public IDataSource GetDataSource(String db)
        {
            return (IDataSource)dataSources[db];
        }

        /**
         * 获得缺省数据源的连接
         */
        public DbConnection GetConnection()
        {
            return GetConnection(ParamManager.GetDefaultDBParam().ID);
        }

        /**
         * 获得指定数据源的连接
         */
        public DbConnection GetConnection(String db)
        {
            return GetDataSource(db).GetConnection();
        }

        /**
         * 释放缺省数据源的连接
         */
        public void FreeConnection(DbConnection conn)
        {
            FreeConnection(ParamManager.GetDefaultDBParam().ID, conn);
        }

        /**
         * 释放指定数据源的连接
         */
        public void FreeConnection(String db, DbConnection conn)
        {
            GetDataSource(db).FreeConnection(conn);
        }
        
        /**
         * 获得缺省数据源的适配器
         */
        public DbDataAdapter GetDataAdapter()
        {
            return GetDataAdapter(ParamManager.GetDefaultDBParam().ID);
        }

        /**
         * 获得指定数据源的适配器
         */
        public DbDataAdapter GetDataAdapter(String db)
        {
            return GetDataSource(db).GetDataAdapter();
        }
        /**
         * 初始化数据源管理器，根据配置参数，创建全部数据源
         */
        private void Init()
        {
            if (dataSources == null)
            {
                dataSources = DataSourceFactory.CrateAllDataSources();
            }
        }
    }
}
