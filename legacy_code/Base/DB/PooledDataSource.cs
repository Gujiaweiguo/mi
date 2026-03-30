using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Base.Sys;
using Base.Util;

namespace Base.DB
{
    /**
     * 本类实现了数据库连接池的管理和维护
     * 通过继承ObjectPool实现了数据库连接的池化管理
     * 通过实现IDataSource接口，对外提供了数据库连接管理的接口
     */
    class PooledDataSource : ObjectPool, IDataSource
    {
        /**
         * 数据库配置参数
         */
        private DBParam dbParam = null;

        /**
         * 对应的数据库适配器
         */
        private DbDataAdapter dataAdapter = null;

        public PooledDataSource()
        {
            this.dbParam = ParamManager.GetDefaultDBParam();
            this.dataAdapter = DataSourceFactory.CreateDataAdapter(this.dbParam);
        }

        public PooledDataSource(DBParam dbParam)
        {
            this.dbParam = dbParam;
            this.dataAdapter = DataSourceFactory.CreateDataAdapter(this.dbParam);
        }

        public DbDataAdapter GetDataAdapter()
        {
            return this.dataAdapter;
        }

        protected override Object Create()
        {
            return DataSourceFactory.CreateConnection(this.dbParam);
        }

        protected override void Expire(Object obj)
        {
            ((DbConnection)obj).Close();
        }

        protected override bool Validate(Object obj)
        {
            return ((DbConnection)obj).State == ConnectionState.Open;
        }

        public DbConnection GetConnection()
        {
            return ((DbConnection) base.CheckOut());
        }

        public void FreeConnection(DbConnection conn)
        {
            base.CheckIn(conn);
        }

    }
}
