using System;
using System.Collections.Generic;
using System.Text;

using Base.Sys;

namespace Base.Util
{
    /**
     * 该类是数据库配置参数的实体类，存放数据库连接所需的参数，对应一个数据库的配置
     */
    public class DBParam : BaseObject
    {
        //数据库实例名（id）
        private String id = null;
        //数据库类型
        private String dbType = null;
        //数据库类型描述
        private String typeDescription = null;
        //数据库提供者字符
        private String provider = null;
        //数据源
        private String dataSource = null;
        //用户名
        private String userID = null;
        //密码
        private String password = null;
        //是否缺省数据库
        private bool isDefault = false;
        //数据库描述
        private String description = null;

        public String ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public String DBType
        {
            get { return this.dbType; }
            set { this.dbType = value; }
        }

        public String TypeDescription
        {
            get { return this.typeDescription; }
            set { this.typeDescription = value; }
        }

        public String Provider
        {
            get { return this.provider; }
            set { this.provider = value; }
        }

        public String DataSource
        {
            get { return this.dataSource; }
            set { this.dataSource = value; }
        }

        public String UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }

        public String Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        public bool IsDefault
        {
            get { return this.isDefault; }
            set { this.isDefault = value; }
        }

        public String Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
    }
}
