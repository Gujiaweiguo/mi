using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.Helper
{
    /// <summary>
    /// SmartGridView的Helper
    /// </summary>
    // 5_1_a_s_p_x.c_o_m

    public class SmartGridView
    {
        /// <summary>
        /// 获取GridView中通过复选框选中的行的DataKey集合
        /// </summary>
        /// <param name="gv">GridView</param>
        /// <param name="columnIndex">CheckBox在GridView中的列索引</param>
        /// <returns></returns>
        public static List<DataKey> GetCheckedDataKey(GridView gv, int columnIndex)
        {
            if (gv.DataKeyNames.Length == 0)
            {
                throw new ArgumentNullException("DataKeys", "未设置GridView的DataKeyNames");
            }

            List<DataKey> list = new List<DataKey>();

            int i = 0;
            foreach (GridViewRow gvr in gv.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    foreach (Control c in gvr.Cells[columnIndex].Controls)
                    {
                        if (c is CheckBox && ((CheckBox)c).Checked)
                        {
                            list.Add(gv.DataKeys[i]);
                            break;
                        }
                    }

                    i++;
                }
            }

            return list;
        }

        /// <summary>
        /// 获取GridView中通过复选框选中的行的DataKey集合
        /// </summary>
        /// <param name="gv">GridView</param>
        /// <param name="checkboxId">CheckBox的ID</param>
        /// <returns></returns>
        public static List<DataKey> GetCheckedDataKey(GridView gv, string checkboxId)
        {
            return GetCheckedDataKey(gv, GetColumnIndex(gv, checkboxId));
        }

        /// <summary>
        /// 获取列索引
        /// </summary>
        /// <param name="gv">GridView</param>
        /// <param name="controlId">控件ID</param>
        /// <returns></returns>
        public static int GetColumnIndex(GridView gv, string controlId)
        {
            foreach (GridViewRow gvr in gv.Rows)
            {
                for (int i = 0; i < gvr.Cells.Count; i++)
                {
                    foreach (Control c in gvr.Cells[i].Controls)
                    {
                        if (c.ID == controlId)
                        {
                            return i;
                        }
                    }
                }
            }

            return -1;
        }
    }
}
