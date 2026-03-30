using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;

namespace YYControls.SmartGridViewFunction
{
    /// <summary>
    /// 扩展功能：自定义分页样式
    /// </summary>
    public class CustomPagerSettingsFunction : ExtendFunction
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomPagerSettingsFunction()
            : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sgv">SmartGridView对象</param>
        public CustomPagerSettingsFunction(SmartGridView sgv)
            : base(sgv)
        {
    
        }

        /// <summary>
        /// 扩展功能的实现
        /// </summary>
        protected override void Execute()
        {
            this._sgv.RowCreated += new GridViewRowEventHandler(_sgv_RowCreated); 
        }

        /// <summary>
        /// SmartGridView的RowCreated事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _sgv_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                int recordCount = 0;

                if (this._sgv.DataSourceObject is DataTable)
                    recordCount = ((DataTable)this._sgv.DataSourceObject).Rows.Count;
                else if (this._sgv.DataSourceObject is DataSet)
                    recordCount = ((DataSet)this._sgv.DataSourceObject).Tables[0].Rows.Count;
                else if (this._sgv.DataSourceObject is ICollection)
                    recordCount = ((ICollection)this._sgv.DataSourceObject).Count;
                else
                    return;

                LinkButton First = new LinkButton();
                LinkButton Prev = new LinkButton();
                LinkButton Next = new LinkButton();
                LinkButton Last = new LinkButton();

                TableCell tc = new TableCell();

                e.Row.Controls.Clear();

                tc.Controls.Add(new LiteralControl("&nbsp"));

                #region 显示总记录数 每页记录数 当前页数/总页数
                string textFormat = String.Format(this._sgv.CustomPagerSettings.TextFormat,
                    this._sgv.PageSize.ToString(),
                    recordCount.ToString(),
                    (this._sgv.PageIndex + 1).ToString(),
                    this._sgv.PageCount.ToString());
                tc.Controls.Add(new LiteralControl(textFormat));
                #endregion

                #region 设置“首页 上一页 下一页 末页”按钮
                if (!String.IsNullOrEmpty(this._sgv.PagerSettings.FirstPageImageUrl))
                    First.Text = "<img src='" + this._sgv.ResolveUrl(this._sgv.PagerSettings.FirstPageImageUrl) + "' border='0'/>";
                else
                    First.Text = this._sgv.PagerSettings.FirstPageText;

                First.CommandName = "Page";
                First.CommandArgument = "First";

                if (!String.IsNullOrEmpty(this._sgv.PagerSettings.PreviousPageImageUrl))
                    Prev.Text = "<img src='" + this._sgv.ResolveUrl(this._sgv.PagerSettings.PreviousPageImageUrl) + "' border='0'/>";
                else
                    Prev.Text = this._sgv.PagerSettings.PreviousPageText;

                Prev.CommandName = "Page";
                Prev.CommandArgument = "Prev";


                if (!String.IsNullOrEmpty(this._sgv.PagerSettings.NextPageImageUrl))
                    Next.Text = "<img src='" + this._sgv.ResolveUrl(this._sgv.PagerSettings.NextPageImageUrl) + "' border='0'/>";
                else
                    Next.Text = this._sgv.PagerSettings.NextPageText;

                Next.CommandName = "Page";
                Next.CommandArgument = "Next";

                if (!String.IsNullOrEmpty(this._sgv.PagerSettings.LastPageImageUrl))
                    Last.Text = "<img src='" + this._sgv.ResolveUrl(this._sgv.PagerSettings.LastPageImageUrl) + "' border='0'/>";
                else
                    Last.Text = this._sgv.PagerSettings.LastPageText;

                Last.CommandName = "Page";
                Last.CommandArgument = "Last";
                #endregion

                #region 添加首页，上一页按钮
                if (this._sgv.PageIndex <= 0)
                    First.Enabled = Prev.Enabled = false;
                else
                    First.Enabled = Prev.Enabled = true;

                tc.Controls.Add(First);
                tc.Controls.Add(new LiteralControl("&nbsp;"));
                tc.Controls.Add(Prev);
                tc.Controls.Add(new LiteralControl("&nbsp;"));
                #endregion

                #region 显示数字分页按钮
                // 当前页左边显示的数字分页按钮的数量
                int rightCount = (int)(this._sgv.PagerSettings.PageButtonCount / 2);
                // 当前页右边显示的数字分页按钮的数量
                int leftCount = this._sgv.PagerSettings.PageButtonCount % 2 == 0 ? rightCount - 1 : rightCount;
                for (int i = 0; i < this._sgv.PageCount; i++)
                {
                    if (this._sgv.PageCount > this._sgv.PagerSettings.PageButtonCount)
                    {
                        if (i < this._sgv.PageIndex - leftCount && this._sgv.PageCount - 1 - i > this._sgv.PagerSettings.PageButtonCount - 1)
                        {
                            continue;
                        }
                        else if (i > this._sgv.PageIndex + rightCount && i > this._sgv.PagerSettings.PageButtonCount - 1)
                        {
                            continue;
                        }
                    }

                    if (i == this._sgv.PageIndex)
                    {
                        tc.Controls.Add(new LiteralControl("<span>" + (i + 1).ToString() + "</span>"));
                    }
                    else
                    {
                        LinkButton lb = new LinkButton();
                        lb.Text = (i + 1).ToString();
                        lb.CommandName = "Page";
                        lb.CommandArgument = (i + 1).ToString();

                        tc.Controls.Add(lb);
                    }

                    tc.Controls.Add(new LiteralControl("&nbsp;"));
                }
                #endregion

                #region 添加下一页，末页按钮
                if (this._sgv.PageIndex >= this._sgv.PageCount - 1)
                    Next.Enabled = Last.Enabled = false;
                else
                    Next.Enabled = Last.Enabled = true;

                tc.Controls.Add(Next);
                tc.Controls.Add(new LiteralControl("&nbsp"));
                tc.Controls.Add(Last);
                tc.Controls.Add(new LiteralControl("&nbsp;"));
                #endregion

                tc.Controls.Add(new LiteralControl("&nbsp"));

                tc.ColumnSpan = this._sgv.Columns.Count;
                e.Row.Controls.Add(tc);        
            }
        }
    }
}
