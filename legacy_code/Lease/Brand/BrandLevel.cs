using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;

namespace Lease.Brand
{
    public class BrandLevel : BasePO
    {
      int brandLevelID = 0;
      string brandLevelCode = "";
      string brandLevelName = "";
      string note = "";
      private int createUserID = 0;  //创建用户代码
      private DateTime createTime = DateTime.Now;  //创建时间
      private int modifyUserID = 0;  //最后修改用户代码
      private DateTime modifyTime = DateTime.Now;  //最后修改时间
      private int oprRoleID = 0;  //操作用户的角色代码
      private int oprDeptID = 0;  //操作用户的机构代码


      public int BrandLevelID
      {
          get { return brandLevelID; }
          set { brandLevelID = value; }
      }

      public string BrandLevelCode
      {
          get { return brandLevelCode; }
          set { brandLevelCode = value; }
      }

      public string  BrandLevelName
      {
          get { return brandLevelName; }
          set { brandLevelName = value; }
      }

      public string  Note
      {
          get { return note; }
          set { note = value; }
      }

      public int CreateUserID
      {
          get { return createUserID; }
          set { createUserID = value; }
      }

      public DateTime CreateTime
      {
          get { return createTime; }
          set { createTime = value; }
      }

      public int ModifyUserID
      {
          get { return modifyUserID; }
          set { modifyUserID = value; }
      }

      public DateTime ModifyTime
      {
          get { return modifyTime; }
          set { modifyTime = value; }
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

      public override string GetTableName()
      {
          return "BrandLevel";
      }

      public override string GetColumnNames()
      {
          return "BrandLevelID,BrandLevelCode,BrandLevelName,Note,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
      }

      public override string GetQuerySql()
      {
          return "Select BrandLevelID,BrandLevelCode,BrandLevelName,Note,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID from BrandLevel";
      }

      public override string GetInsertColumnNames()
      {
          return "BrandLevelID,BrandLevelCode,BrandLevelName,Note,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
      }

      public override string GetUpdateColumnNames()
      {
          return "BrandLevelID,BrandLevelCode,BrandLevelName,Note,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
      }





    }
}
