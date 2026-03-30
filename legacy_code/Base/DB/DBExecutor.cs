using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

using Base.Util;

namespace Base.DB
{
    /**
     * 数据操作的执行类
     */
    internal class DBExecutor
    {
        /**
         * 数据库连接
         */
        private DbConnection conn = null;

        /**
         * 数据库事务
         */
        private DbTransaction trans = null;

        /**
         * 执行查询操作，以PO形式，返回结果集
         */
        internal Resultset Query(BasePO basePO, String sql)
        {
            Resultset rs = new Resultset();
            this.conn = DataSourceManager.GetInstance().GetConnection();
            DbDataReader reader = null;
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.CommandText = sql;
                comm.CommandTimeout = 240;
                reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    BasePO po = (BasePO)Activator.CreateInstance(basePO.GetType());
                    po.FetchResult(reader);
                    rs.Add(po);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
            }
            return rs;
        }

        /**
         * 执行插入数据库操作
         */
        internal int Insert(BasePO basePO, String sql)
        {
            int count = 0;
            this.conn = DataSourceManager.GetInstance().GetConnection();
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.CommandText = sql;
                String[] columns = basePO.GetInsertColumnNames().Split(BasePO.FIELD_SPLITTER);
                foreach (String column in columns)
                {
                    DbParameter param = comm.CreateParameter();
                    param.ParameterName = Utils.TrimField(column);
                    param.Value = basePO.GetProperty(Utils.TrimField(column));
                    comm.Parameters.Add(param);
                }
                count = comm.ExecuteNonQuery();
            }
            finally
            {
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
            }
            return count;
        }

        /**
         * 执行更新数据库操作
         */
        internal int Update(BasePO basePO, String sql)
        {
            int count = 0;
            this.conn = DataSourceManager.GetInstance().GetConnection();
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.CommandText = sql;
                String[] columns = basePO.GetUpdateColumnNames().Split(BasePO.FIELD_SPLITTER);
                foreach (String column in columns)
                {
                    DbParameter param = comm.CreateParameter();
                    param.ParameterName = Utils.TrimField(column);
                    param.Value = basePO.GetProperty(Utils.TrimField(column));
                    comm.Parameters.Add(param);
                }
                count = comm.ExecuteNonQuery();
            }
            finally
            {
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
            }
            return count;
        }

        /**
         * 执行删除数据库操作
         */
        internal int Delete(BasePO basePO, String sql)
        {
            int count = 0;
            this.conn = DataSourceManager.GetInstance().GetConnection();
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.CommandText = sql;
                count = comm.ExecuteNonQuery();
            }
            finally
            {
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
            }
            return count;
        }
        /**
         * 以dataset形式获取结果集
         */
        internal DataSet QueryDataSet(String sql)
        {
            this.conn = DataSourceManager.GetInstance().GetConnection();
            DataSet dataSet = new DataSet();
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.CommandText = sql;
                comm.CommandTimeout = 240;
                DbDataAdapter dataAdapter = DataSourceManager.GetInstance().GetDataAdapter();
                dataAdapter.SelectCommand = comm;
                dataAdapter.Fill(dataSet);
            }
            finally
            {
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
            }
            return dataSet;
        }
        
        /**
         * 执行更新、插入、删除等数据库操作
         */
        internal int ExecuteUpdate(String sql)
        {
            int count = 0;
            this.conn = DataSourceManager.GetInstance().GetConnection();
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.CommandText = sql;
                count = comm.ExecuteNonQuery();
            }
            finally
            {
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
            }
            return count;
        }

        /**
         * 开始事务
         */
        internal void BeginTrans()
        {
            this.conn = DataSourceManager.GetInstance().GetConnection();
            try
            {
                this.trans = this.conn.BeginTransaction();
            }
            catch (Exception e)
            {
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
                throw e;
            }
        }

        /**
         * 提交事务
         */
        internal void Commit()
        {
            try
            {
                if (this.trans != null)
                {
                    this.trans.Commit();
                }
            }
            catch (Exception e)
            {
                this.trans.Rollback();
                throw e;
            }
            finally
            {
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
                this.trans = null;
            }
        }
        /**
         * 回滚事务
         */
        internal void Rollback()
        {
            try
            {
                if (this.trans != null)
                {
                    this.trans.Rollback();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                DataSourceManager.GetInstance().FreeConnection(this.conn);
                this.conn = null;
                this.trans = null;
            }
        }

        /**
         * 执行插入数据库操作
         */
        internal int InsertWithTrans(BasePO basePO, String sql)
        {
            int count = 0;
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.Transaction = this.trans;
                comm.CommandText = sql;
                String[] columns = basePO.GetInsertColumnNames().Split(BasePO.FIELD_SPLITTER);
                foreach (String column in columns)
                {
                    DbParameter param = comm.CreateParameter();
                    param.ParameterName = Utils.TrimField(column);
                    param.Value = basePO.GetProperty(Utils.TrimField(column));
                    comm.Parameters.Add(param);
                }
                count = comm.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Rollback();
                throw e;
            }
            return count;
        }

        /**
         * 执行更新数据库操作
         */
        internal int UpdateWithTrans(BasePO basePO, String sql)
        {
            int count = 0;
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.Transaction = this.trans;
                comm.CommandText = sql;
                String[] columns = basePO.GetUpdateColumnNames().Split(BasePO.FIELD_SPLITTER);
                foreach (String column in columns)
                {
                    DbParameter param = comm.CreateParameter();
                    param.ParameterName = Utils.TrimField(column);
                    param.Value = basePO.GetProperty(Utils.TrimField(column));
                    comm.Parameters.Add(param);
                }
                count = comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Rollback();
                throw e;
            }
            return count;
        }

        /**
         * 执行删除数据库操作
         */
        internal int DeleteWithTrans(BasePO basePO, String sql)
        {
            int count = 0;
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.Transaction = this.trans;
                comm.CommandText = sql;
                count = comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Rollback();
                throw e;
            }
            return count;
        }
        /**
         * 执行更新、插入、删除等数据库操作
         */
        internal int ExecuteUpdateWithTrans(String sql)
        {
            int count = 0;
            try
            {
                DbCommand comm = conn.CreateCommand();
                comm.Transaction = this.trans;
                comm.CommandText = sql;
                count = comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Rollback();
                throw e;
            }
            return count;
        }
    }
}
