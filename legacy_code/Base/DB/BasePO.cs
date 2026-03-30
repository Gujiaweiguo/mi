using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Reflection;

using Base.Sys;
using Base.DB;
using Base.Util;

namespace Base.DB
{
    /**
     * 
     */
    public abstract class BasePO : BaseObject
    {
        /**
         * 字段之间的分隔符，在需要获得字段数组时使用
         */
        public static char[] FIELD_SPLITTER ={ ',' };

        /**
         * 用来存放外部设置的sql
         */
        private String querySql = null;

        /**
         * 将数据库中当前记录存放到该PO中，返回当前PO
         */
        public BasePO FetchResult(DbDataReader reader)
        {
            Type type = this.GetType();
            String[] fields = this.GetColumnNames().Split(FIELD_SPLITTER);
            //字段顺序计数
            int ordinal = 0;
            foreach (String field in fields)
            {
                String fld = Utils.TrimField(field);
                PropertyInfo info = type.GetProperty(fld);
                if (info == null)
                {
                    throw new DBException("No property:" + field + " defined in class:" + type);
                }
                //由于数据库中的null值，不能付给c#的变量，这里需要判断
                if (reader.IsDBNull(ordinal))
                {
                    info.SetValue(this, null, null);
                }
                else
                {
                    info.SetValue(this, reader.GetValue(ordinal), null);
                }
                ordinal++;
            }
            return this;
        }
        /**
         * 由外部设置完整的SQL
         */
        public void SetQuerySql(String querySql)
        {
            this.querySql = querySql;
        }

        /**
         * 获取查询sql语句，不包括where等子句
         */
        public virtual String GetQuerySql()
        {
            if (this.querySql != null)
            {
                return this.querySql;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ").Append(this.GetColumnNames()).Append(" FROM ").Append(this.GetTableName());
            return builder.ToString();
        }

        /**
         * 获取插入sql语句
         */
        public String GetInsertSql()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO ").Append(this.GetTableName()).Append("(").Append(this.GetInsertColumnNames()).Append(")").Append(" VALUES (");

            String[] fields = this.GetInsertColumnNames().Split(FIELD_SPLITTER);
            int count = 0;
            foreach (String field in fields)
            {
                builder.Append("@").Append(Utils.TrimField(field));
                if (count < fields.Length - 1)
                {
                    builder.Append(",");
                }
                count++;
            }
            builder.Append(")");
            return builder.ToString();
        }

        /**
         * 获取更新sql语句，不包括where等子句
         */
        public String GetUpdateSql()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE ").Append(this.GetTableName()).Append(" SET ");
            String[] fields = this.GetUpdateColumnNames().Split(FIELD_SPLITTER);
            int count = 0;
            foreach (String field in fields)
            {
                builder.Append(field).Append("=@").Append(Utils.TrimField(field));
                if (count < fields.Length - 1)
                {
                    builder.Append(",");
                }
                count++;
            }
            return builder.ToString();
        }

        /**
         * 获取删除sql语句，不包括where等子句
         */
        public String GetDeleteSql()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("DELETE FROM ").Append(this.GetTableName());
            return builder.ToString();
        }

        /**
         * 返回数据库表名。
         */
        public abstract String GetTableName();

        /**
         * 返回字段组合（查询、插入的字段）
         */
        public abstract String GetColumnNames();

        public virtual String GetInsertColumnNames()
        {
            return GetColumnNames();
        }

        /**
         * 返回需要更新的字段组合
         */
        public abstract String GetUpdateColumnNames();

        /**
         * 获取所有字段对应的数值,使用分号分隔,没有使用引号
         */
        public String GetColumnValues()
        {
            String[] columns = GetColumnNames().Split(FIELD_SPLITTER);
            return GetValues(columns);
        }

        /**
         * 获取更新字段对应的数值,使用分号分隔,没有使用引号
         */
        public String GetUpdateColumnValues()
        {
            String[] columns = GetUpdateColumnNames().Split(FIELD_SPLITTER);
            return GetValues(columns);
        }

        /**
         * 获得指定列的数值
         */
        private String GetValues(String[] columns)
        {
            StringBuilder builder = new StringBuilder();
            int count = 0;
            foreach (String column in columns)
            {
                builder.Append(GetProperty(Utils.TrimField(column)));
                if (count < columns.Length)
                {
                    builder.Append(":");
                }
                count++;
            }
            return builder.ToString();
        }
    }
}
