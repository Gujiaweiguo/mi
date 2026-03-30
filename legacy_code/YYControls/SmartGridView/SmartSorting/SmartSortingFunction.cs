using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能：复合排序和排序状态提示
    /// </summary>
    public class SmartSortingFunction : ExtendFunction
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmartSortingFunction()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public SmartSortingFunction(SmartGridView sgv)
            : base(sgv)
        {

        }

        /// <summary>
        /// 扩展功能的实现
        /// </summary>
        protected override void Execute()
        {
            // 开启SmartGridView的排序功能
            // this._sgv.AllowSorting = true;

            this._sgv.Sorting += new System.Web.UI.WebControls.GridViewSortEventHandler(_sgv_Sorting);
            this._sgv.RowDataBoundCell += new SmartGridView.RowDataBoundCellHandler(_sgv_RowDataBoundCell);
        }

        /// <summary>
        /// SmartGridView的RowDataBoundCell事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="gvtc"></param>
        void _sgv_RowDataBoundCell(object sender, GridViewTableCell gvtc)
        {
            if (!String.IsNullOrEmpty(this._sgv.SortExpression) 
                && this._sgv.SmartSorting.AllowSortTip
                && gvtc.RowType == DataControlRowType.Header)
            {
                // 显示排序状态提示
                DisplaySortTip(this._sgv.SortExpression, gvtc.TableCell);
            }
        }

        /// <summary>
        /// Sorting事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _sgv_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            // 如果允许复合排序的话，则设置复合排序表达式
            if (this._sgv.SmartSorting.AllowMultiSorting)
            {
                e.SortExpression = GetSortExpression(e);
            }
        }

        /// <summary>
        /// 获得复合排序表达式
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected string GetSortExpression(GridViewSortEventArgs e)
        {
            string[] sortColumns = null;
            string sortAttribute = this._sgv.SortExpression;

            if (!String.IsNullOrEmpty(sortAttribute))
            {
                sortColumns = sortAttribute.Split(',');
            }

            // 如果原排序表达式有当前排序字段
            if (sortAttribute.IndexOf(e.SortExpression) != -1)
            {
                // 更新排序表达式
                sortAttribute = ModifySortExpression(sortColumns, e.SortExpression);
            }
            else
            {
                // 为原排序表达式添加新的排序规则（升序）
                sortAttribute += String.Concat(",", e.SortExpression, " ASC");
            }

            return sortAttribute.Trim(',');
        }

        /// <summary>
        /// 更新排序表达式
        /// </summary>
        /// <param name="sortColumns">各个字段的排序表达式数组</param>
        /// <param name="sortExpression">当前需要排序的字段（该字段在sortColumns数组中）</param>
        /// <returns></returns>
        protected string ModifySortExpression(string[] sortColumns, string sortExpression)
        {
            // 当前需要排序的字段的升序表达式
            string ascSortExpression = String.Concat(sortExpression, " ASC");
            // 当前需要排序的字段的降序表达式
            string descSortExpression = String.Concat(sortExpression, " DESC");

            for (int i = 0; i < sortColumns.Length; i++)
            {
                // 各个字段的排序表达式数组中，有当前需要排序的字段的升序表达式
                if (ascSortExpression.Equals(sortColumns[i]))
                {
                    // 当前排序字段由升序变为降序
                    sortColumns[i] = descSortExpression;
                    break;
                }
                // 各个字段的排序表达式数组中，有当前需要排序的字段的降序表达式
                else if (descSortExpression.Equals(sortColumns[i]))
                {
                    // 从各个字段的排序表达式数组中，删除当前需要排序的字段
                    Array.Clear(sortColumns, i, 1);
                    break;
                }
            }

            return String.Join(",", sortColumns).Replace(",,", ",").Trim(',');
        }

        /// <summary>
        /// 显示排序状态提示
        /// </summary>
        /// <param name="sortExpression">排序表达式</param>
        /// <param name="tc">Header的TableCell</param>
        protected void DisplaySortTip(string sortExpression, TableCell tc)
        {
            string[] sortColumns = sortExpression.Split(',');

            if (tc.Controls.Count > 0 && tc.Controls[0] is LinkButton)
            {
                string columnName = ((LinkButton)tc.Controls[0]).CommandArgument;

                int sortOrderIndex = Array.FindIndex(sortColumns, delegate(string s) { return s.IndexOf(columnName) != -1; });

                // 如果排序表达式中有当前列
                if (sortOrderIndex != -1)
                {
                    // 当前列应该是升序还是降序（区分两种情况：复合排序和非复合排序）
                    SortDirection sortDirection = SortDirection.Ascending;
                    if (this._sgv.SmartSorting.AllowMultiSorting && sortColumns[sortOrderIndex].IndexOf("DESC") != -1)
                    {
                        sortDirection = SortDirection.Descending;
                    }
                    else if (!this._sgv.SmartSorting.AllowMultiSorting && this._sgv.SortDirection == SortDirection.Descending)
                    {
                        sortDirection = SortDirection.Descending;
                    }

                    // 排序状态提示（图片地址）
                    string sortImageUrl = null;
                    // 排序状态提示（文本）
                    string sortText = null;
                    if (sortDirection == SortDirection.Ascending)
                    {
                        sortText = this._sgv.SmartSorting.SortAscText;

                        sortImageUrl = this._sgv.SmartSorting.SortAscImageUrl;

                        if (String.IsNullOrEmpty(sortImageUrl))
                        {
                            sortImageUrl = 
                                this._sgv.Page.ClientScript.GetWebResourceUrl
                                (
                                    this.GetType(),
                                    "YYControls.SmartGridView.Resources.Asc.gif"
                                );
                        }
                    }
                    else
                    {
                        sortText = this._sgv.SmartSorting.SortDescText;

                        sortImageUrl = this._sgv.SmartSorting.SortDescImageUrl;

                        if (String.IsNullOrEmpty(sortImageUrl))
                        {
                            sortImageUrl = 
                                this._sgv.Page.ClientScript.GetWebResourceUrl
                                (
                                    this.GetType(),
                                    "YYControls.SmartGridView.Resources.Desc.gif"
                                );
                        }
                    }

                    // 添加排序状态提示图片
                    Image imgSortDirection = new Image();
                    imgSortDirection.ImageUrl = sortImageUrl;
                    tc.Controls.Add(imgSortDirection);

                    // 添加排序状态提示文本
                    tc.Controls.Add(new LiteralControl(sortText));

                    if (this._sgv.SmartSorting.AllowMultiSorting)
                    {
                        // 添加排序状态提示序号
                        tc.Controls.Add(new LiteralControl((sortOrderIndex + 1).ToString()));
                    }

                    #region 不用资源文件，又没有设置排序状态提示时的逻辑（已经注释掉了）
                    //// 添加排序状态提示图标
                    //Label lblSortDirection = new Label();
                    //// 被注释的代码使用的是webdings，但是FF不支持，所以放弃了
                    //// lblSortDirection.Font.Name = "webdings";
                    //// lblSortDirection.EnableTheming = false;
                    //if (sortDirection == SortDirection.Ascending)
                    //{
                    //    // lblSortDirection.Text = "5";
                    //    lblSortDirection.Text = "&#9650";
                    //}
                    //else
                    //{
                    //    // lblSortDirection.Text = "6";
                    //    lblSortDirection.Text = "&#9660";
                    //}
                    //tc.Controls.Add(lblSortDirection);

                    //if (this._sgv.SmartSorting.AllowMultiSorting)
                    //{
                    //    // 添加排序状态提示序号
                    //    tc.Controls.Add(new LiteralControl((sortOrderIndex + 1).ToString()));
                    //}
                    #endregion

                } // if (sortOrderIndex != -1)
            } // if (tc.Controls.Count > 0 && tc.Controls[0] is LinkButton)
        }
    }
}
