using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Base.Biz
{
    /**
     * 该类实现了以事务方式对数据的处理
     */
    public class BaseTrans
    {

        /**
         * 需要操作的PO对象
         */
        private BasePO basePO = null;
        /**
         * SQL语句中的where条件
         */
        private String whereClause = null;
        /**
         * 是否需要追加 WHERER 关键字，否则，追加 AND
         */
        private bool appendWhere = true;

        /**
         * 数据库操作执行对象
         */
        private DBExecutor executor = new DBExecutor();

        /**
         * 根据指定的po，插入该po的数据保存到数据库
         */
        public int Insert(BasePO basePO)
        {
            this.basePO = basePO;
            String sql = GetInsertSql();
            return this.executor.InsertWithTrans(basePO, sql);
        }
        /**
         * 根据指定的po及更新条件，将该po存放的数据更新到数据库
         */
        public int Update(BasePO basePO)
        {
            this.basePO = basePO;
            String sql = getUpdateSql();
            return this.executor.UpdateWithTrans(basePO, sql);
        }

        /**
         * 根据指定的po及更新条件，将该po存放的数据从数据库中删除
         */
        public int Delete(BasePO basePO)
        {
            this.basePO = basePO;
            String sql = GetDeleteSql();
            return this.executor.DeleteWithTrans(basePO, sql);
        }

        /**
         * 执行制定的更新、插入、删除等sql语句
         */
        public int ExecuteUpdate(String sql)
        {
            return executor.ExecuteUpdateWithTrans(sql);
        }

        /**
         * 开始事务
         */
        public void BeginTrans()
        {
            this.executor.BeginTrans();
        }

        /**
         * 提交事务
         */
        public void Commit()
        {
            this.executor.Commit();
        }
        /**
         * 回滚事务
         */ 
        public void Rollback()
        {
            this.executor.Rollback();
        }

        public BasePO BasePO
        {
            get { return this.basePO; }
            set { this.basePO = value; }
        }

        public String WhereClause
        {
            get { return this.whereClause; }
            set { this.whereClause = value; }
        }

        public bool AppendWhere
        {
            get { return this.appendWhere; }
            set { this.appendWhere = value; }
        }
        
        /**
         * 获取更新sql语句（增加查询条件等子句）
         */
        private String GetInsertSql()
        {
            return this.basePO.GetInsertSql();
        }

        /**
         * 获取更新sql语句（增加查询条件等子句）
         */
        private String getUpdateSql()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.basePO.GetUpdateSql());
            if (this.whereClause == null || this.whereClause.Length == 0)
            {
                throw new DBException("no where clause defined in a update sql." + builder.ToString());
            }
            if (this.whereClause != null && this.whereClause.Length > 0)
            {
                if (this.appendWhere)
                {
                    builder.Append(" WHERE ");
                }
                else
                {
                    builder.Append(" AND ");
                }
                builder.Append(whereClause);
            }
            return builder.ToString();
        }

        /**
         * 获取删除sql语句（增加查询条件等子句）
         */
        private String GetDeleteSql()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.basePO.GetDeleteSql());
            if (this.whereClause == null || this.whereClause.Length == 0)
            {
                throw new DBException("no where clause defined in a delete sql." + builder.ToString());
            }
            if (this.whereClause != null && this.whereClause.Length > 0)
            {
                if (this.appendWhere)
                {
                    builder.Append(" WHERE ");
                }
                else
                {
                    builder.Append(" AND ");
                }
                builder.Append(whereClause);
            }
            return builder.ToString();
        }
    }
}
