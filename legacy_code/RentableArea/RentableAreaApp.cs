using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.Data;

using Base.DB;
using Base.Util;
using Base.Biz;

namespace RentableArea
{
    public class RentableAreaApp
    {
        /**
 * 通过从指定的数据表，采用最大值加1的方法，获取ID
 */
        private static Hashtable ids = new Hashtable();



        public static int GetAreaLevelID()
        {
            return GetID("AreaLevel", "GetAreaLevelID");
        }
        public static int GetID(String table, String field)
        {
            lock (ids)
            {

                int id = 0;
                //如果hashtable中已经有该id，则取出来，加1
                String key = (table + "_" + field).ToLower();
                if (ids[key] != null)
                {
                    id = (int)ids[key] + 1;
                    ids[key] = id;
                    return id;
                }
                //如果hashtable里没有该id，则从数据表中取最大值，加1
                String sql = "select max(" + field + ") from " + table;
                BaseBO bo = new BaseBO();
                DataSet ds = bo.QueryDataSet(sql);
                //如果数据表中有数据，则取出后加1
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    id = int.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1;
                }
                //如果数据表没有数据（一般在首次使用后出现这种情况），则取缺省值101，100以内用作备用值
                else
                {
                    id = 101;
                }

                ids[key] = id;
                return id;
            }
        }
    }
}
