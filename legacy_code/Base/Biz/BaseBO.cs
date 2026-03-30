using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

using Base.DB;

namespace Base.Biz
{
    /**
     * 该类实现了对PO的操作（包括增加、查询、修改、删除），具体实现是通过调用DBExecutor的方法。
     */
    public class BaseBO
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
         * sql语句中的group by子句
         */
        private String groupBy = null;
        /**
         * sql语句中的order by子句
         */
        private String orderBy = null;
        /**
         * 是否需要追加 WHERER 关键字，否则，追加 AND
         */
        private bool appendWhere = true;

        /**
         * 数据库操作执行对象
         */
        private DBExecutor executor = new DBExecutor();

        /**
         * 根据指定PO对象和其他查询子句，查询数据，并将结果以po的形式，存放在结果集
         */
        public Resultset Query(BasePO basePO)
        {
            this.basePO = basePO;
            String sql = GetQuerySql();
            return executor.Query(basePO, sql);
        }

        /**
         * 根据指定的po，插入该po的数据保存到数据库
         */
        public int Insert(BasePO basePO)
        {
            this.basePO = basePO;
            String sql = GetInsertSql();
            return executor.Insert(basePO, sql);
        }
        /**
         * 根据指定的po及更新条件，将该po存放的数据更新到数据库
         */
        public int Update(BasePO basePO)
        {
            this.basePO = basePO;
            String sql = getUpdateSql();
            return executor.Update(basePO, sql);
        }

        /**
         * 根据指定的po及更新条件，将该po存放的数据从数据库中删除
         */
        public int Delete(BasePO basePO)
        {
            this.basePO = basePO;
            String sql = GetDeleteSql();
            return executor.Delete(basePO, sql);
        }
        /**
         * 以dataset形式获取结果集，sql语句通过basePO和BO的条件语句组合生成
         */
        public DataSet QueryDataSet(BasePO basePO)
        {
            this.basePO = basePO;
            String sql = GetQuerySql();
            return QueryDataSet(sql);
        }

        /**
         * 以dataset形式获取结果集，通过参数传入sql语句
         */
        public DataSet QueryDataSet(String sql)
        {
            return executor.QueryDataSet(sql);
        }

        /**
         * 执行制定的更新、插入、删除等sql语句
         */
        public int ExecuteUpdate(String sql)
        {
            return executor.ExecuteUpdate(sql);
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

        public String GroupBy
        {
            get { return this.groupBy; }
            set { this.groupBy = value; }
        }

        public String OrderBy
        {
            get { return this.orderBy; }
            set { this.orderBy = value; }
        }

        public bool AppendWhere
        {
            get { return this.appendWhere; }
            set { this.appendWhere = value; }
        }
        /**
         * 获取查询sql语句（增加查询条件等子句）
         */
        private String GetQuerySql()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.basePO.GetQuerySql());

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

            if (groupBy != null && groupBy.Length > 0)
            {
                builder.Append(" GROUP BY ");
                builder.Append(groupBy);
            }
            if (orderBy != null && orderBy.Length > 0)
            {
                builder.Append(" ORDER BY ");
                builder.Append(orderBy);
            }
            return builder.ToString();
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
                throw new DBException("No where clause defined in a update sql." + builder.ToString());
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
                throw new DBException("No where clause defined in a delete sql." + builder.ToString());
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
