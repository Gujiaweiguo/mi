using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
using Base.Util;
using Base.Biz;

using BaseInfo;

namespace BaseInfo.Dept
{
    public class Dept : CommonInfoPO, ITreeNode
    {

        public static int DEPT_TYPE_CHILD_COMPANY = 1;
        public static int DEPT_TYPE_CHILD_SONLD = 2;
        public static int DEPT_TYPE_REGION_HQ = 3;
        public static int DEPT_TYPE_REGION = 4;
        public static int DEPT_TYPE_CITY = 5;
        public static int DEPT_TYPE_MALL = 6;
        public static int DEPT_TYPE_DEPT = 7;

        public static int[] GetDeptType()
        {
            int[] getDeptType = new int[4];
            getDeptType[0] = DEPT_TYPE_DEPT;
            getDeptType[1] = DEPT_TYPE_MALL;
            getDeptType[2] = DEPT_TYPE_CHILD_SONLD;
            getDeptType[3] = DEPT_TYPE_CHILD_COMPANY;
            return getDeptType;
        }

        public static String GetDeptTypeDesc(int getDeptType)
        {
            if (getDeptType == DEPT_TYPE_DEPT)
            {
                return "DEPT_TYPE_DEPT";  //部门
            }
            else if (getDeptType == DEPT_TYPE_MALL)
            {
                return "DEPT_TYPE_MALL";
            }
            else if (getDeptType == DEPT_TYPE_CHILD_SONLD)
            {
                return "DEPT_TYPE_CHILD_SONLD";
            }
            else if (getDeptType == DEPT_TYPE_CHILD_COMPANY)
            {
                return "DEPT_TYPE_CHILD_COMPANY";
            }
            else
                return "未知";

        }


        public static int INDEPBALANCE_STATUS_INVALID = 0;
        public static int INDEPBALANCE_STATUS_VALID = 1;

        public static int[] GetIndepBalanceStatus()
        {
            int[] indepBalanceStaus = new int[2];
            indepBalanceStaus[0] = INDEPBALANCE_STATUS_VALID;
            indepBalanceStaus[1] = INDEPBALANCE_STATUS_INVALID;
            return indepBalanceStaus;
        }

        public static String GetIndepBalanceStatusDesc(int indepBalanceStaus)
        {
            if (indepBalanceStaus == INDEPBALANCE_STATUS_INVALID)
            {
                return "INDEPBALANCE_STATUS_INVALID";  //否
            }
            if (indepBalanceStaus == INDEPBALANCE_STATUS_VALID)
            {
                return "INDEPBALANCE_STATUS_VALID";   //是
            }
            return "未知";
        }

        public String IndepBalanceStatusDesc
        {
            get { return GetIndepBalanceStatusDesc(indepBalance); }
        }


        public static int DEPTSTATUS_INVALID = 0;
        public static int DEPTSTATUS_VALID = 1;

        public static int[] GetDeptStatus()
        {
            int[] deptStatus = new int[2];
            deptStatus[0] = DEPTSTATUS_VALID;
            deptStatus[1] = DEPTSTATUS_INVALID;
            return deptStatus;
        }

        public static String GetDeptStatusDesc(int deptStatus)
        {
            if (deptStatus == DEPTSTATUS_INVALID)
            {
                return "DEPTSTATUS_INVALID";  //停用
            }
            if (deptStatus == DEPTSTATUS_VALID)
            {
                return "DEPTSTATUS_VALID";   //启用
            }
            return "NO";
        }

        public String DeptStatusDesc
        {
            get { return GetDeptStatusDesc(deptStatus); }
        }




        private List<ITreeNode> children = new List<ITreeNode>();
        private ITreeNode parent = null;

        private int deptId = 0;                    //部门内码
        private int createUserId = 0;              //创建用户代码
        private DateTime createTime = DateTime.Now;//创建时间
        private int modifyUserID = 0;              //最后修改用户代码
        private DateTime modifyTime = DateTime.Now;//最后修改时间
        private int oprRoleID = 0;                 //操作用户角色代码
        private int oprDeptID = 0;                 //操作用户机构代码
        private string deptCode = "";              //部门编码
        private string deptName = "";              //部门名称
        private int deptLevel = 0;                 //部门级别--自动生成
        private int pDeptId = 0;                   //父部门内码--自动生成
        private int deptStatus = 0;                //部门状况
        private int deptType = 0;                  //部门城市
        private string regAddr = "";               //注册类型
        private string city = "";                  //所在地址
        private string officeAddr = "";            //办公地址
        private string postAddr = "";              //邮寄地址
        private string postCode = "";              //邮政编码
        private string tel = "";                   //联系电话
        private string officeTel = "";             //办公电话
        private string fax = "";                    //传真
        private int indepBalance = 0;          //是否独立计算
        //private string deptDesc = "";              //部门描述

        private int orderID = 0;//排序号

        public override String GetTableName()
        {
            return "Dept";
        }
        public override String GetColumnNames()
        {
            return "CreateUserId,CreateTime,DeptID,DeptCode,DeptName,DeptLevel,PDeptID,DeptType,City,RegAddr,OfficeAddr,PostAddr,PostCode,"
            + "Tel,OfficeTel,Fax,DeptStatus,IndepBalance,OrderID";
        }
        public override String GetUpdateColumnNames()
        {
            return "DeptCode,DeptName,DeptType,City,RegAddr,OfficeAddr,PostAddr,PostCode,Tel,OfficeTel,Fax,ModifyUserID,ModifyTime,OprRoleID,OprDeptID,IndepBalance,DeptStatus,OrderID";
        }


        /**
         * 部门信息
         */
        public int CreateUserId
        {
            get { return createUserId; }
            set { createUserId = value; }
        }

        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        public int ModifyuserID
        {
            get { return modifyUserID; }
            set { modifyUserID = value; }
        }

        public DateTime ModifyTime
        {
            get { return modifyTime; }
            set { modifyTime = value; }
        }

        public int DeptID
        {
            get { return deptId; }
            set { deptId = value; }
        }
        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }
        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }

        public int DeptLevel
        {
            get { return deptLevel; }
            set { deptLevel = value; }
        }

        public int DeptType
        {
            get { return deptType; }
            set { deptType = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string RegAddr
        {
            get { return regAddr; }
            set { regAddr = value; }
        }
        public string OfficeAddr
        {
            get { return officeAddr; }
            set { officeAddr = value; }
        }
        public string PostAddr
        {
            get { return postAddr; }
            set { postAddr = value; }
        }
        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }
        public string OfficeTel
        {
            get { return officeTel; }
            set { officeTel = value; }
        }
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        public int PDeptID
        {
            get { return pDeptId; }
            set { pDeptId = value; }
        }

        public int DeptStatus
        {
            get { return deptStatus; }
            set { deptStatus = value; }
        }

        public int IndepBalance
        {
            get { return indepBalance; }
            set { indepBalance = value; }
        }

        public int OprRoleID
        {
            get { return oprRoleID; }
            set { oprRoleID = value; }
        }

        public int OprDeptID
        {
            get { return oprDeptID; }
            set { oprDeptID = value; }
        }
        public int OrderID
        {
            set { orderID = value; }
            get { return orderID; }
        }

        public String DeptTypeStatusDesc
        {
            get { return GetDeptTypeDesc(this.DeptType); }
        }

        /**
        * 获取节点的值
        **/
        public String GetValue()
        {
            return this.DeptLevel.ToString() + "," + this.DeptID.ToString();
        }

        /**
         * 获取节点的显示文本
         */
        public String GetText()
        {
            return this.DeptName;
        }
        /**
         * 获得节点的提示信息
         */
        public String GetTip()
        {
            return "";
        }

        /**
         * 添加子节点
         */
        public void AddChild(ITreeNode childNode)
        {
            this.children.Add(childNode);
        }

        /**
         * 获取子节点的集合
         */
        public List<ITreeNode> GetChildren()
        {
            return this.children;
        }

        /**
         * 设置父节点
         */
        public void SetParent(ITreeNode parent)
        {
            this.parent = parent;
        }

        /**
         * 获取父节点
         */
        public ITreeNode GetParent()
        {
            return this.parent;
        }

        /**
         * 获取子节点时使用的where条件，格式如："PDeptID="+DeptID
         */
        public String GetChildrenWhere()
        {
            return "PDeptID=" + this.DeptID + "and DeptStatus = 1";
        }

        /**
         * 获取根结点时需要的where条件，格式如："DeptLevle=1"
         */
        public String GetRootWhere()
        {
            return "DeptLevel = 1";
        }
        /// <summary>
        /// 获取父节点集合
        /// </summary>
        /// <returns></returns>
        public static String GetPDeptID(int deptid)
        {
            BaseBO baseBo = new BaseBO();
            string strRet = "";
            string strsql = "select a.deptid,b.deptid,c.deptid,d.deptid,e.deptid,f.deptid from dept a " +
                          "left join dept b on (a.pdeptid=b.deptid) left join dept c on (c.deptid=b.pdeptid) " +
                          "left join dept d on (d.deptid=c.pdeptid) left join dept e on (e.deptid=d.pdeptid) " +
                          "left join dept f on (f.deptid=e.pdeptid) where a.deptid=" + deptid;
            System.Data.DataSet  ds = new System.Data.DataSet();
            ds = baseBo.QueryDataSet(strsql);
            int intCol = ds.Tables[0].Columns.Count;
            for (int i = 0; i < intCol; i++)
            {
                if (strRet.Trim() == "")
                {
                    strRet = ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    if (ds.Tables[0].Rows[0][i].ToString() != "")
                    {
                        strRet += "," + ds.Tables[0].Rows[0][i].ToString();
                    }
                }
            }
            return strRet;              
        }
        /// <summary>
        /// 返还子部门集合
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public static string GetChildDeptID(int deptid)
        {
            BaseBO baseBo = new BaseBO();
            string strRet = "";
            string strsql = "select a.deptid,b.deptid,c.deptid,d.deptid,e.deptid,f.deptid from dept a " +
                          "left join dept b on (a.deptid=b.pdeptid) left join dept c on (c.pdeptid=b.deptid) " +
                          "left join dept d on (d.pdeptid=c.deptid) left join dept e on (e.pdeptid=d.deptid) " +
                          "left join dept f on (f.pdeptid=e.deptid) where a.deptid=" + deptid;
            System.Data.DataSet ds = new System.Data.DataSet();
            ds = baseBo.QueryDataSet(strsql);
            
            return strRet;
        }


    }
}
