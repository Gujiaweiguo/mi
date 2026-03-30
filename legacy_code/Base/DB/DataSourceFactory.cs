using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Collections;

using Base.Util;

namespace Base.DB
{
    class DataSourceFactory
    {
        /**
         * 根据指定数据库配置参数，创建数据源对象
         */
        public static IDataSource CreateDataSource(DBParam param)
        {
            return new PooledDataSource(param);
        }
        /**
         * 根据缺省的数据库配置参数，创建数据源对象
         */
        public static IDataSource CreateDefaultDataSource()
        {
            return CreateDataSource(ParamManager.GetDefaultDBParam());
        }

        /**
         * 根据数据库配置参数，创建全部数据源对象
         * 返回值为存放全部数据源的hashtable，key为数据源定义的ID，value为相应的数据源对象
         */
        public static Hashtable CrateAllDataSources(){
            Hashtable sources = new Hashtable();
            DBParam[] dbParams = ParamManager.GetAllDBParams();
            for (int i = 0; i < dbParams.Length; i++)
            {
                IDataSource source = CreateDataSource(dbParams[i]);
                sources.Add(dbParams[i].ID, source);
            }
            return sources;
        }

        /**
         * 根据数据库配置，创建数据库连接，并打开连接
         */
        public static DbConnection CreateConnection(DBParam param)
        {
            DbConnection conn = null;
            if (param.DBType.Equals("sqlserver"))
            {
                conn = new SqlConnection("server=" + param.Provider + ";database=" + param.DataSource + ";uid=" + param.UserID + ";pwd=" + param.Password);
            }
            else if (param.DBType.Equals("db2"))
            {
                conn = new OleDbConnection("Provider=" + param.Provider + ";Data Source=" + param.DataSource + ";User ID=" + param.UserID + ";Password=" + param.Password);
            }
            else if (param.DBType.Equals("oracle"))
            {
                conn = new OleDbConnection("Provider=" + param.Provider + ";Data Source=" + param.DataSource + ";User ID=" + param.UserID + ";Password=" + param.Password);
            }
            else if (param.DBType.Equals("access"))
            {
                conn = new OleDbConnection("Provider=" + param.Provider + ";Data Source=" + param.DataSource + ";User ID=" + param.UserID + ";Password=" + param.Password);
            }
            else if (param.DBType.Equals("odbc"))
            {
                conn = new OdbcConnection();
            }
            else
            {
                throw new DBException("db parameter error when CreateConnection.db type id:" + param.DBType);
            }
            conn.Open();
            return conn;
        }

        /**
         * 根据指定数据库配置参数，创建数据库适配器
         */
        public static DbDataAdapter CreateDataAdapter(DBParam param)
        {
            DbDataAdapter adapter = null;
            if (param.DBType.Equals("sqlserver"))
            {
                adapter = new SqlDataAdapter();
            }
            else if (param.DBType.Equals("db2"))
            {
                adapter = new OleDbDataAdapter();
            }
            else if (param.DBType.Equals("oracle"))
            {
                adapter = new OleDbDataAdapter();
            }
            else if (param.DBType.Equals("access"))
            {
                adapter = new OleDbDataAdapter();
            }
            else if (param.DBType.Equals("odbc"))
            {
                adapter = new OdbcDataAdapter();
            }
            else
            {
                throw new DBException("db parameter error when CreateDataAdapter.db type id:" + param.DBType);
            }

            return adapter;
        }
    }
}
