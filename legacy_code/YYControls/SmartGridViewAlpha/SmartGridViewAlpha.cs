/*----------
 * webabcd.cnblogs.com
 * 比较乱，懒得重构了
----------*/
using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Collections.Specialized;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// 继承自GridView
    /// </summary>
    [
    ToolboxData(@"<{0}:SmartGridViewAlpha runat='server'></{0}:SmartGridViewAlpha>"),
    System.Drawing.ToolboxBitmap(typeof(SmartGridViewAlpha), "icon.bmp"),
    ToolboxItem(false),
    ParseChildren(true),
    PersistChildren(false)
    ]
    public class SmartGridViewAlpha : GridView, IPostBackDataHandler
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmartGridViewAlpha()
        {
            this.PreRender += new EventHandler(SmartGridViewAlpha_PreRender);
        }

        /// <summary>
        /// SmartGridViewAlpha_PreRender
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void SmartGridViewAlpha_PreRender(object sender, EventArgs e)
        {
            #region 改变通过CheckBox选中的行的样式 注册客户端脚本
            if ((!String.IsNullOrEmpty(ChangeRowCSSByCheckBox.CheckBoxID)
                && !String.IsNullOrEmpty(ChangeRowCSSByCheckBox.CssClassRowSelected))
                || !String.IsNullOrEmpty(CssClassMouseOver))
            {
                // 注册实现改变行样式的客户端脚本
                if (!Page.ClientScript.IsClientScriptBlockRegistered("jsChangeRowClassName"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(
                        this.GetType(),
                        "jsChangeRowClassName", JavaScriptConstant.jsChangeRowClassName
                        );
                }
                // 注册调用双击CheckBox函数的客户端脚本
                if (!Page.ClientScript.IsStartupScriptRegistered("jsInvokeDoubleClickCheckBox"))
                {
                    Page.ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "jsInvokeDoubleClickCheckBox", @"<script type=""text/javascript"">yy_DoubleClickCheckBox();</script>"
                        );
                }
            }
            #endregion

            #region 数据行响应鼠标的单击和双击事件，加载客户端代码
            if (!String.IsNullOrEmpty(RowClickButtonID) || !String.IsNullOrEmpty(RowDoubleClickButtonID))
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("jsClickAndDoubleClick"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(
                        this.GetType(),
                        "jsClickAndDoubleClick", JavaScriptConstant.jsClickAndDoubleClick
                        );
                }
            }
            #endregion

            #region 每行复选框的全选与取消全选 PreRender部分
            // CheckboxAlls里有对象则注册一些完成实现全选功能的客户端脚本
            if (CheckboxAlls.Count > 0)
            {
                // 注册实现 每行复选框的全选与取消全选 功能的JavaScript
                if (!Page.ClientScript.IsClientScriptBlockRegistered("JsCheckAll"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(
                        this.GetType(),
                        "JsCheckAll", JavaScriptConstant.jsCheckAll.Replace
                            ("[$AllName$]", this.HiddenCheckboxAllID).Replace
                            ("[$ItemName$]", this.HiddenCheckboxItemID).Replace
                            ("[$GroupSeparator$]", this.GroupSeparator.ToString()).Replace
                            ("[$ItemSeparator$]", this.ItemSeparator.ToString())
                        );
                }

                // 给_checkItemIDString赋值
                _checkItemIDString = "";
                foreach (KeyValuePair<int, string> kvp in _checkItemIDDictionary)
                {
                    _checkItemIDString += this.GroupSeparator + kvp.Value;
                }
                if (_checkItemIDString.StartsWith(this.GroupSeparator.ToString()))
                {
                    _checkItemIDString = _checkItemIDString.Remove(0, 1);
                }

                // 注册实现 每行复选框的全选与取消全选 功能的两个隐藏字段
                // 有的时候回发后没有重新绑定GridView，就会造成_checkAllIDString和_checkItemIDString为空
                // 所以把这两个值存到ViewSate中
                if (!String.IsNullOrEmpty(_checkAllIDString) && !String.IsNullOrEmpty(_checkItemIDString))
                {
                    ViewState[this.HiddenCheckboxAllID] = _checkAllIDString;
                    ViewState[this.HiddenCheckboxItemID] = _checkItemIDString;
                }
                if (ViewState[this.HiddenCheckboxAllID] != null && ViewState[this.HiddenCheckboxItemID] != null)
                {
                    Page.ClientScript.RegisterHiddenField(this.HiddenCheckboxAllID, ViewState[this.HiddenCheckboxAllID].ToString());
                    Page.ClientScript.RegisterHiddenField(this.HiddenCheckboxItemID, ViewState[this.HiddenCheckboxItemID].ToString());
                }
            }
            #endregion

            #region 给数据行增加右键菜单
            if (ContextMenus.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ContextMenu cm in ContextMenus)
                {
                    // item = new contextItem("", "", "", "", "", "");
                    // 1-菜单项的文本
                    // 2-图标链接
                    // 3-所调用的命令按钮的ID
                    // 4-链接地址
                    // 5-链接的target
                    // 6-右键菜单的项的类别
                    // 7-自定义属性key
                    // 8-自定义属性value

                    // 命令按钮
                    if (cm.ItemType == ContextMenu.ItemTypeCollection.Command)
                    {
                        sb.Append("item = new contextItem(\"" + cm.Text +
                            "\", \"" + ResolveUrl(cm.Icon) + "\", \"" +
                            cm.CommandButtonId + "\", \"\", \"\", \"Command\", \"\", \"\");");
                    }
                    // 链接
                    else if (cm.ItemType == ContextMenu.ItemTypeCollection.Link)
                    {
                        sb.Append("item = new contextItem(\"" + cm.Text +
                            "\", \"" + ResolveUrl(cm.Icon) + "\", \"\", \"" +
                            cm.NavigateUrl + "\", \"" +
                            cm.Target + "\", \"Link\", \"\", \"\");");
                    }
                    // 自定义属性
                    else if (cm.ItemType == ContextMenu.ItemTypeCollection.Custom)
                    {
                        sb.Append("item = new contextItem(\"" + cm.Text +
                             "\", \"" + ResolveUrl(cm.Icon) +
                             "\", \"\", \"\", \"\", \"Custom\", \"" + cm.Key + "\", \"" + cm.Value + "\");");
                    }
                    // 分隔线
                    else if (cm.ItemType == ContextMenu.ItemTypeCollection.Separator)
                    {
                        sb.Append("item = new contextItem(\"\", \"\", \"\", \"\", \"\", \"Separator\", \"\", \"\");");
                    }

                    sb.Append("myMenu.addItem(item);");
                }

                // 注册客户端代码
                if (!Page.ClientScript.IsClientScriptBlockRegistered("jsContextMenu"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(
                        this.GetType(),
                        "jsContextMenu", JavaScriptConstant.jsContextMenu.Replace("[$MakeMenu$]", sb.ToString())
                        );
                }
            }
            #endregion

            #region 固定行、列后使页面保存滚动条的位置信息
            if (FixRowCol.EnableScrollState)
            {
                // 滚动条x位置
                Page.ClientScript.RegisterHiddenField("yy_SmartGridViewAlpha_x", _yy_SmartGridViewAlpha_x.ToString());
                // 滚动条y位置
                Page.ClientScript.RegisterHiddenField("yy_SmartGridViewAlpha_y", _yy_SmartGridViewAlpha_y.ToString());

                // 设置GridView的滚动条的位置
                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "jsSetScroll", "<script type=\"text/javascript\">document.getElementById('yy_ScrollDiv').scrollLeft=" + _yy_SmartGridViewAlpha_x + ";document.getElementById('yy_ScrollDiv').scrollTop=" + _yy_SmartGridViewAlpha_y + ";</script>"
                    );

                // 将控件注册为要求在页回发至服务器时进行回发处理的控件
                if (Page != null) Page.RegisterRequiresPostBack(this);
            }
            #endregion
        }

        /// <summary>
        /// OnRowDataBound
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            // 如果规定了不触发RowDataBound则不触发此事件
            if (!_raiseRowDataBound) return;

            // 确保tr有id属性，并且值正确
            e.Row.Attributes.Add("id", e.Row.ClientID);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                #region 鼠标经过行时变化行的样式
                // _cssClassMouseOver不是空则执行
                if (!string.IsNullOrEmpty(this._cssClassMouseOver))
                {
                    // 在<tr>上onmouseover时<tr>的样式（css类名）
                    e.Row.Attributes.Add("onmouseover", "yy_ChangeRowClassName(this.id, '" + this._cssClassMouseOver + "', false)");

                    // 样式的名称（css类名）
                    string cssClassMouseOut = "";

                    // 根据RowState的不同，onmouseout时<tr>的不同样式（css类名）
                    switch (e.Row.RowState)
                    {
                        case DataControlRowState.Alternate:
                            cssClassMouseOut = base.AlternatingRowStyle.CssClass;
                            break;
                        case DataControlRowState.Edit:
                            cssClassMouseOut = base.EditRowStyle.CssClass;
                            break;
                        case DataControlRowState.Normal:
                            cssClassMouseOut = base.RowStyle.CssClass;
                            break;
                        case DataControlRowState.Selected:
                            cssClassMouseOut = base.SelectedRowStyle.CssClass;
                            break;
                        default:
                            cssClassMouseOut = "";
                            break;
                    }

                    // 增加<tr>的dhtml事件onmouseout
                    e.Row.Attributes.Add("onmouseout", "yy_ChangeRowClassName(this.id, '" + cssClassMouseOut + "', false)");
                }
                #endregion

                #region 给命令按钮增加客户端确认提示框
                if (this._confirmButtons != null)
                {
                    // GridViewRow的每个TableCell
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        // TableCell里的每个Control
                        foreach (Control c in tc.Controls)
                        {
                            // 如果控件继承自接口IButtonControl
                            if (c.GetType().GetInterface("IButtonControl") != null && c.GetType().GetInterface("IButtonControl").Equals(typeof(IButtonControl)))
                            {
                                // 从用户定义的ConfirmButtons集合中分解出ConfirmButton
                                foreach (ConfirmButton cb in _confirmButtons)
                                {
                                    // 如果发现的按钮的CommandName在ConfirmButtons有定义的话
                                    if (((IButtonControl)c).CommandName == cb.CommandName)
                                    {
                                        // 增加确认框属性
                                        ((IAttributeAccessor)c).SetAttribute("onclick", "return confirm('" + cb.ConfirmMessage + "')");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                // GridViewRow的每个TableCell
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    #region 每行复选框的全选与取消全选 DataRow部分
                    // TableCell里的每个Control
                    for (int j = 0; j < e.Row.Cells[i].Controls.Count; j++)
                    {
                        if (e.Row.Cells[i].Controls[j] is CheckBox)
                        {
                            CheckBox chk = (CheckBox)e.Row.Cells[i].Controls[j];

                            // 判断该CheckBox是否属于全选CheckBox 
                            bool isCheckboxAll = false;
                            foreach (CheckboxAll ca in CheckboxAlls)
                            {
                                if (chk.NamingContainer.ClientID + "_" + ca.CheckboxItemID == chk.ClientID)
                                {
                                    isCheckboxAll = true;
                                    break;
                                }
                            }

                            // 给该CheckBox增加客户端代码
                            if (isCheckboxAll)
                            {
                                // 给Control增加一个客户端onclick
                                chk.Attributes.Add("onclick", "yy_ClickCheckItem()");
                                // 给_checkItemIDDictionary赋值
                                if (_checkItemIDDictionary.Count == 0 || !_checkItemIDDictionary.ContainsKey(i))
                                {
                                    _checkItemIDDictionary.Add(i, chk.ClientID);
                                }
                                else
                                {
                                    string s;
                                    _checkItemIDDictionary.TryGetValue(i, out s);
                                    _checkItemIDDictionary.Remove(i);
                                    _checkItemIDDictionary.Add(i, s + this.ItemSeparator + chk.ClientID);
                                }

                                break;
                            }
                        }
                    }
                    #endregion
                }

                #region 数据行响应鼠标的单击和双击事件
                if (!String.IsNullOrEmpty(RowClickButtonID) || !String.IsNullOrEmpty(RowDoubleClickButtonID))
                {
                    // GridViewRow的每个TableCell
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        // TableCell里的每个Control
                        foreach (Control c in tc.Controls)
                        {
                            // 如果控件继承自接口IButtonControl
                            if (c.GetType().GetInterface("IButtonControl") != null && c.GetType().GetInterface("IButtonControl").Equals(typeof(IButtonControl)))
                            {
                                if (!String.IsNullOrEmpty(RowClickButtonID))
                                {
                                    // 该按钮的ID等于单击行所对应的按钮ID
                                    if (c.ID == RowClickButtonID)
                                    {
                                        // 增加行的单击事件，调用客户端脚本，根据所对应按钮的ID执行所对应按钮的click事件
                                        e.Row.Attributes.Add("onclick", "javascript:yy_RowClick('" + c.ClientID + "')");
                                    }
                                }

                                if (!String.IsNullOrEmpty(RowDoubleClickButtonID))
                                {
                                    // 该按钮的ID等于双击行所对应的按钮ID
                                    if (c.ID == RowDoubleClickButtonID)
                                    {
                                        // 增加行的双击事件，调用客户端脚本，根据所对应按钮的ID执行所对应按钮的click事件
                                        e.Row.Attributes.Add("ondblclick", "javascript:yy_RowDoubleClick('" + c.ClientID + "')");
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 改变通过CheckBox选中的行的样式
                if (!String.IsNullOrEmpty(ChangeRowCSSByCheckBox.CheckBoxID) && !String.IsNullOrEmpty(ChangeRowCSSByCheckBox.CssClassRowSelected))
                {
                    foreach (TableCell tc in e.Row.Cells)
                    {
                        // 如果发现了指定的CheckBox
                        if (tc.FindControl(ChangeRowCSSByCheckBox.CheckBoxID) != null)
                        {
                            CheckBox chk = tc.FindControl(ChangeRowCSSByCheckBox.CheckBoxID) as CheckBox;
                            string cssClassUnchecked = "";

                            // 根据RowState的不同，取消行的选中后<tr>的不同样式（css类名）
                            switch (e.Row.RowState)
                            {
                                case DataControlRowState.Alternate:
                                    cssClassUnchecked = base.AlternatingRowStyle.CssClass;
                                    break;
                                case DataControlRowState.Edit:
                                    cssClassUnchecked = base.EditRowStyle.CssClass;
                                    break;
                                case DataControlRowState.Normal:
                                    cssClassUnchecked = base.RowStyle.CssClass;
                                    break;
                                case DataControlRowState.Selected:
                                    cssClassUnchecked = base.SelectedRowStyle.CssClass;
                                    break;
                                default:
                                    cssClassUnchecked = "";
                                    break;
                            }

                            // 给行增加一个yy_selected属性，用于客户端判断行是否是选中状态
                            e.Row.Attributes.Add("yy_selected", "false");

                            // 添加CheckBox的click事件的客户端调用代码
                            string strOnclickScript = "";
                            if (!String.IsNullOrEmpty(chk.Attributes["onclick"]))
                            {
                                strOnclickScript += chk.Attributes["onclick"];
                            }
                            strOnclickScript += ";if (this.checked) "
                                + "{yy_ChangeRowClassName('" + e.Row.ClientID + "', '" + ChangeRowCSSByCheckBox.CssClassRowSelected + "', true);"
                                + "yy_SetRowSelectedAttribute('" + e.Row.ClientID + "', 'true')} "
                                + "else {yy_ChangeRowClassName('" + e.Row.ClientID + "', '" + cssClassUnchecked + "', true);"
                                + "yy_SetRowSelectedAttribute('" + e.Row.ClientID + "', 'false')}";
                            chk.Attributes.Add("onclick", strOnclickScript);

                            break;
                        }
                    }
                }
                #endregion

                #region 给数据行增加右键菜单 DataRow部分
                if (ContextMenus.Count > 0)
                {
                    // 给数据行增加客户端代码
                    e.Row.Attributes.Add("oncontextmenu", "showMenu('" + e.Row.ClientID + "');return false;");
                }
                #endregion
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                // GridViewRow的每个TableCell
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    #region 每行复选框的全选与取消全选 Header部分
                    // TableCell里的每个Control
                    for (int j = 0; j < e.Row.Cells[i].Controls.Count; j++)
                    {
                        if (e.Row.Cells[i].Controls[j] is CheckBox)
                        {
                            CheckBox chk = (CheckBox)e.Row.Cells[i].Controls[j];

                            // 判断该CheckBox是否属于全选CheckBox 
                            bool isCheckboxAll = false;
                            foreach (CheckboxAll ca in CheckboxAlls)
                            {
                                if (chk.NamingContainer.ClientID + "_" + ca.CheckboxAllID == chk.ClientID)
                                {
                                    isCheckboxAll = true;
                                    break;
                                }
                            }

                            // 给该CheckBox增加客户端代码
                            if (isCheckboxAll)
                            {
                                // 给Control增加一个客户端onclick
                                chk.Attributes.Add("onclick", "yy_ClickCheckAll(this)");
                                // 给_checkAllIDString赋值
                                if (String.IsNullOrEmpty(this._checkAllIDString))
                                {
                                    this._checkAllIDString += chk.ClientID;
                                }
                                else
                                {
                                    this._checkAllIDString += this.GroupSeparator + chk.ClientID;
                                }
                                break;
                            }
                        }
                    }
                    #endregion

                    #region 排序时在标题处标明是升序还是降序
                    // TableCell里有一个Control并且是LinkButton
                    if (e.Row.Cells[i].Controls.Count == 1 && e.Row.Cells[i].Controls[0] is LinkButton)
                    {
                        // LinkButton的命令参数等于排序字段
                        if (((LinkButton)e.Row.Cells[i].Controls[0]).CommandArgument == this.SortExpression)
                        {
                            Image img = null;
                            Label lbl = null;

                            // 升序
                            if (this.SortDirection == SortDirection.Ascending)
                            {
                                // 升序图片
                                if (!String.IsNullOrEmpty(_sortTip.SortAscImage))
                                {
                                    img = new Image();
                                    img.ImageUrl = base.ResolveUrl(_sortTip.SortAscImage);
                                }
                                // 升序文字
                                if (!String.IsNullOrEmpty(_sortTip.SortAscText))
                                {
                                    lbl = new Label();
                                    lbl.Text = _sortTip.SortAscText;
                                }
                            }
                            // 降序
                            else if (this.SortDirection == SortDirection.Descending)
                            {
                                // 降序图片
                                if (!String.IsNullOrEmpty(_sortTip.SortDescImage))
                                {
                                    img = new Image();
                                    img.ImageUrl = base.ResolveUrl(_sortTip.SortDescImage);
                                }
                                // 降序文字
                                if (!String.IsNullOrEmpty(_sortTip.SortDescText))
                                {
                                    lbl = new Label();
                                    lbl.Text = _sortTip.SortDescText;
                                }
                            }

                            // TableCell里加上图片
                            if (img != null)
                            {
                                e.Row.Cells[i].Controls.Add(img);
                            }
                            // TableCell里加上文字
                            if (lbl != null)
                            {
                                e.Row.Cells[i].Controls.Add(lbl);
                            }
                        }
                    }
                    #endregion
                }
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                #region 如果需要固定Pager的话
                if (FixRowCol.IsFixPager)
                {
                    if (this.PagerSettings.Position == PagerPosition.Top || (this.PagerSettings.Position == PagerPosition.TopAndBottom && _isTopPager))
                    {
                        // TopPager固定行和列
                        e.Row.Cells[0].Attributes.Add("style", "z-index:999; position: relative; top: expression(this.offsetParent.scrollTop); left: expression(this.offsetParent.scrollLeft);");
                        // 现在是TopPager，之后就是BottomPager了，所以设置_isTopPager为false
                        _isTopPager = false;
                    }
                    else if (this.PagerSettings.Position == PagerPosition.TopAndBottom && !_isTopPager)
                    {
                        // BottomPager只固定列
                        e.Row.Cells[0].Attributes.Add("style", "z-index:999; position: relative; left: expression(this.offsetParent.scrollLeft);");
                        // 现在是BottomPager，之后就是TopPager了，所以设置_isTopPager为true
                        _isTopPager = true;
                    }
                }
                #endregion
            }

            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                #region 固定行、列 DataRow和Header部分
                // 给每一个指定固定的列的单元格加上css属性
                if (!String.IsNullOrEmpty(FixRowCol.FixColumnIndices))
                {
                    // 列索引
                    foreach (string s in FixRowCol.FixColumnIndices.Split(','))
                    {
                        int i;
                        if (!Int32.TryParse(s, out i))
                            throw new ArgumentException("FixColumnIndices", "含有非整形的字符");
                        if (i > e.Row.Cells.Count)
                            throw new ArgumentOutOfRangeException("FixColumnIndices", "溢出");

                        e.Row.Cells[i].Attributes.Add("style", "position: relative; left: expression(this.offsetParent.scrollLeft);");
                    }
                }

                bool isFixRow = false; // 当前行是否固定
                if (FixRowCol.IsFixHeader && e.Row.RowType == DataControlRowType.Header)
                {
                    isFixRow = true;
                }

                if (!String.IsNullOrEmpty(FixRowCol.FixRowIndices) && e.Row.RowType == DataControlRowType.DataRow)
                {
                    // 行索引
                    foreach (string s in FixRowCol.FixRowIndices.Split(','))
                    {
                        int i;
                        if (!Int32.TryParse(s, out i))
                            throw new ArgumentException("FixRowIndices", "含有非整形的字符");
                        if (i > e.Row.Cells.Count)
                            throw new ArgumentOutOfRangeException("FixRowIndices", "溢出");

                        if (i == e.Row.RowIndex)
                        {
                            isFixRow = true;
                            break;
                        }
                    }
                }

                // 固定该行
                if (isFixRow)
                {
                    // 该行的每一个单元格
                    for (int j = 0; j < e.Row.Cells.Count; j++)
                    {
                        // 该单元格不属于固定列
                        if (String.IsNullOrEmpty(e.Row.Cells[j].Attributes["style"]) || e.Row.Cells[j].Attributes["style"].IndexOf("position: relative;") == -1)
                        {
                            e.Row.Cells[j].Attributes.Add("style", " position: relative; top: expression(this.offsetParent.scrollTop);");
                        }
                        // 该单元格属于固定列
                        else
                        {
                            e.Row.Cells[j].Attributes.Add("style", e.Row.Cells[j].Attributes["style"] + "top: expression(this.offsetParent.scrollTop); z-index: 666;");
                        }
                    }
                }
                #endregion
            }

            base.OnRowDataBound(e);
        }

        /// <summary>
        /// Render
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            #region 为了固定行、列要 用div包围GridView <div>部分
            // 给GridView一个容器 <div>
            if (!FixRowCol.TableWidth.IsEmpty || !FixRowCol.TableHeight.IsEmpty)
            {
                if (FixRowCol.TableWidth.IsEmpty) FixRowCol.TableWidth = new Unit(100, UnitType.Percentage);
                if (FixRowCol.TableHeight.IsEmpty) FixRowCol.TableHeight = new Unit(100, UnitType.Percentage);

                writer.Write("<div id='yy_ScrollDiv' style=\"overflow: auto; width: "
                    + FixRowCol.TableWidth.ToString() + "; height: "
                    + FixRowCol.TableHeight.ToString() + "; position: relative;\" ");

                // 如果保持滚动条的状态的话，用隐藏字段记录滚动条的位置
                if (FixRowCol.EnableScrollState)
                {
                    writer.Write("onscroll=\"document.getElementById('yy_SmartGridViewAlpha_x').value = this.scrollLeft; document.getElementById('yy_SmartGridViewAlpha_y').value = this.scrollTop;\">");
                }
                else
                {
                    writer.Write(">");
                }
            }
            #endregion

            base.Render(writer);

            #region 为了固定行、列要 用div包围GridView </div>部分
            // </div> 结束
            if (!FixRowCol.TableWidth.IsEmpty || !FixRowCol.TableHeight.IsEmpty)
            {
                writer.Write("</div>");
            }
            #endregion
        }

        /// <summary>
        /// OnRowCommand
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowCommand(GridViewCommandEventArgs e)
        {
            #region 导出为Excel
            if (e.CommandName.ToLower() == "exporttoexcel")
            {
                System.Web.HttpContext.Current.Response.ClearContent();
                // e.CommandArgument用“;”隔开两部分，左边的部分为导出Excel的文件名称
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + e.CommandArgument.ToString().Split(';')[0] + ".xls");
                System.Web.HttpContext.Current.Response.ContentType = "application/excel";

                System.IO.StringWriter sw = new System.IO.StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                // e.CommandArgument用“;”隔开两部分，右边的部分为需要隐藏的列的索引（列索引用“,”分开）
                if (e.CommandArgument.ToString().Split(';').Length > 1)
                {
                    foreach (string s in e.CommandArgument.ToString().Split(';')[1].Split(','))
                    {
                        int i;

                        if (!Int32.TryParse(s, out i))
                        {
                            throw new ArgumentException("需要隐藏的列的索引不是整数");
                        }

                        if (i > this.Columns.Count)
                        {
                            throw new ArgumentOutOfRangeException("需要隐藏的列的索引超出范围");
                        }

                        this.Columns[i].Visible = false;
                    }
                }

                // 隐藏“导出Excel”按钮
                ((Control)e.CommandSource).Visible = false;

                // 如果HeaderRow里的控件是button的话，则把它替换成文本
                foreach (TableCell tc in this.HeaderRow.Cells)
                {
                    // TableCell里的每个Control
                    foreach (Control c in tc.Controls)
                    {
                        // 如果控件继承自接口IButtonControl
                        if (c.GetType().GetInterface("IButtonControl") != null && c.GetType().GetInterface("IButtonControl").Equals(typeof(IButtonControl)))
                        {
                            // 如果该控件不是“导出Excel”按钮则把button转换成文本
                            if (!c.Equals(e.CommandSource))
                            {
                                tc.Controls.Clear();
                                tc.Text = ((IButtonControl)c).Text;
                            }
                        }
                    }
                }

                // 将服务器控件的内容输出到所提供的 System.Web.UI.HtmlTextWriter 对象中
                this.RenderControl(htw);

                System.Web.HttpContext.Current.Response.Write(sw.ToString());
                System.Web.HttpContext.Current.Response.End();
            }
            #endregion

            base.OnRowCommand(e);
        }

        /// <summary>
        /// OnRowCreated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            #region 自定义分页样式
            if (e.Row.RowType == DataControlRowType.Pager && PagingStyle == Paging.PagingStyleCollection.Default)
            {
                LinkButton First = new LinkButton();
                LinkButton Prev = new LinkButton();
                LinkButton Next = new LinkButton();
                LinkButton Last = new LinkButton();

                TableCell tc = new TableCell();

                e.Row.Controls.Clear();

                #region 显示总记录数 每页记录数 当前页数/总页数
                tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                if (_recordCount.HasValue)
                {
                    tc.Controls.Add(new LiteralControl(_recordCount.ToString()));
                    tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                    tc.Controls.Add(new LiteralControl(PageSize.ToString()));
                    tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                }
                tc.Controls.Add(new LiteralControl((PageIndex + 1).ToString()));
                tc.Controls.Add(new LiteralControl("/"));
                tc.Controls.Add(new LiteralControl(PageCount.ToString()));
                tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;"));
                #endregion

                #region “首页 上一页 下一页 尾页”按钮
                if (!String.IsNullOrEmpty(PagerSettings.FirstPageImageUrl))
                {
                    First.Text = "<img src='" + ResolveUrl(PagerSettings.FirstPageImageUrl) + "' border='0'/>";
                }
                else
                {
                    First.Text = PagerSettings.FirstPageText;
                }
                First.CommandName = "Page";
                First.CommandArgument = "First";
                First.Font.Underline = false;

                if (!String.IsNullOrEmpty(PagerSettings.PreviousPageImageUrl))
                {
                    Prev.Text = "<img src='" + ResolveUrl(PagerSettings.PreviousPageImageUrl) + "' border='0'/>";
                }
                else
                {
                    Prev.Text = PagerSettings.PreviousPageText;
                }
                Prev.CommandName = "Page";
                Prev.CommandArgument = "Prev";
                Prev.Font.Underline = false;


                if (!String.IsNullOrEmpty(PagerSettings.NextPageImageUrl))
                {
                    Next.Text = "<img src='" + ResolveUrl(PagerSettings.NextPageImageUrl) + "' border='0'/>";
                }
                else
                {
                    Next.Text = PagerSettings.NextPageText;
                }
                Next.CommandName = "Page";
                Next.CommandArgument = "Next";
                Next.Font.Underline = false;

                if (!String.IsNullOrEmpty(PagerSettings.LastPageImageUrl))
                {
                    Last.Text = "<img src='" + ResolveUrl(PagerSettings.LastPageImageUrl) + "' border='0'/>";
                }
                else
                {
                    Last.Text = PagerSettings.LastPageText;
                }
                Last.CommandName = "Page";
                Last.CommandArgument = "Last";
                Last.Font.Underline = false;
                #endregion

                #region 添加首页，上一页按钮
                if (this.PageIndex <= 0)
                {
                    First.Enabled = Prev.Enabled = false;
                }
                else
                {
                    First.Enabled = Prev.Enabled = true;
                }

                tc.Controls.Add(First);
                tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                tc.Controls.Add(Prev);
                tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                #endregion

                #region 显示数字分页按钮
                // 当前页左边显示的数字分页按钮的数量
                int rightCount = (int)(PagerSettings.PageButtonCount / 2);
                // 当前页右边显示的数字分页按钮的数量
                int leftCount = PagerSettings.PageButtonCount % 2 == 0 ? rightCount - 1 : rightCount;
                for (int i = 0; i < PageCount; i++)
                {
                    if (PageCount > PagerSettings.PageButtonCount)
                    {
                        if (i < PageIndex - leftCount && PageCount - 1 - i > PagerSettings.PageButtonCount - 1)
                        {
                            continue;
                        }
                        else if (i > PageIndex + rightCount && i > PagerSettings.PageButtonCount - 1)
                        {
                            continue;
                        }
                    }

                    if (i == PageIndex)
                    {
                        tc.Controls.Add(new LiteralControl("<span style='color:red;font-weight:bold'>" + (i + 1).ToString() + "</span>"));
                    }
                    else
                    {
                        LinkButton lb = new LinkButton();
                        lb.Text = (i + 1).ToString();
                        lb.CommandName = "Page";
                        lb.CommandArgument = (i + 1).ToString();

                        tc.Controls.Add(lb);
                    }

                    tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                }
                #endregion

                #region 添加下一页，尾页按钮
                if (this.PageIndex >= PageCount - 1)
                {
                    Next.Enabled = Last.Enabled = false;
                }
                else
                {
                    Next.Enabled = Last.Enabled = true;
                }
                tc.Controls.Add(Next);
                tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                tc.Controls.Add(Last);
                tc.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
                #endregion

                tc.ColumnSpan = this.Columns.Count;
                e.Row.Controls.Add(tc);
            }
            #endregion

            base.OnRowCreated(e);
        }

        /// <summary>
        /// OnLoad
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            // 查找ObjectDataSource
            ObjectDataSource ods = Parent.FindControl(this.DataSourceID) as ObjectDataSource;
            if (ods != null)
            {
                ods.Selected += new ObjectDataSourceStatusEventHandler(ods_Selected);
            }

            base.OnLoad(e);
        }

        private int? _recordCount = null;
        /// <summary>
        /// 计算总记录数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ods_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.ReturnValue is IListSource)
            {
                _recordCount = ((IListSource)e.ReturnValue).GetList().Count;
            }

            // 需要触发RowDataBound
            _raiseRowDataBound = true;
        }

        #region 公共属性
        private string _cssClassMouseOver;
        /// <summary>
        /// 鼠标经过的样式 CSS 类名
        /// </summary>
        [Browsable(true)]
        [Description("鼠标经过的样式 CSS 类名")]
        [DefaultValue("")]
        [Category("扩展")]
        public virtual string CssClassMouseOver
        {
            get { return _cssClassMouseOver; }
            set { _cssClassMouseOver = value; }
        }

        private ConfirmButtons _confirmButtons;
        /// <summary>
        /// 确认按钮集合
        /// </summary>
        [
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Description("确认按钮集合，确认按钮的CommandName和提示信息"),
        Category("扩展")
        ]
        public virtual ConfirmButtons ConfirmButtons
        {
            get
            {
                if (_confirmButtons == null)
                {
                    _confirmButtons = new ConfirmButtons();
                }
                return _confirmButtons;
            }
        }

        private SortTip _sortTip;
        /// <summary>
        /// 排序提示信息
        /// </summary>
        [
        Description("排序提示信息"),
        Category("扩展"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual SortTip SortTip
        {
            get
            {
                if (_sortTip == null)
                {
                    _sortTip = new SortTip();
                }
                return _sortTip;
            }
        }

        private CheckboxAlls _checkboxAlls;
        /// <summary>
        /// 复选框组集合 一个组由一个 全选复选框 和多个 项复选框组成
        /// </summary>
        [
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Description("复选框组集合 一个组由一个 全选复选框 和多个 项复选框组成"),
        Category("扩展")
        ]
        public virtual CheckboxAlls CheckboxAlls
        {
            get
            {
                if (_checkboxAlls == null)
                {
                    _checkboxAlls = new CheckboxAlls();
                }
                return _checkboxAlls;
            }
        }

        private FixRowCol _fixRowCol;
        /// <summary>
        /// 固定表头、指定行或指定列
        /// </summary>
        [
        Description("固定表头、指定行或指定列"),
        Category("扩展"),
        DefaultValue(""),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual FixRowCol FixRowCol
        {
            get
            {
                if (_fixRowCol == null)
                {
                    _fixRowCol = new FixRowCol();
                }
                return _fixRowCol;
            }
        }

        private string _rowClickButtonID;
        /// <summary>
        /// 单击行事件所对应的按钮的ID
        /// </summary>
        [Description("单击行事件所对应的按钮的ID"), DefaultValue(""), Category("扩展")]
        public virtual string RowClickButtonID
        {
            get { return _rowClickButtonID; }
            set { _rowClickButtonID = value; }
        }

        private string _rowDoubleClickButtonID;
        /// <summary>
        /// 双击行事件所对应的按钮的ID
        /// </summary>
        [Description("双击行事件所对应的按钮的ID"), DefaultValue(""), Category("扩展")]
        public virtual string RowDoubleClickButtonID
        {
            get { return _rowDoubleClickButtonID; }
            set { _rowDoubleClickButtonID = value; }
        }

        private ChangeRowCSSByCheckBox _changeRowCSSByCheckBox;
        /// <summary>
        /// 通过行的CheckBox的选中与否来修改行的样式
        /// </summary>
        [
        Description("通过行的CheckBox的选中与否来修改行的样式"),
        Category("扩展"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty)
        ]
        public virtual ChangeRowCSSByCheckBox ChangeRowCSSByCheckBox
        {
            get
            {
                if (_changeRowCSSByCheckBox == null)
                {
                    _changeRowCSSByCheckBox = new ChangeRowCSSByCheckBox();
                }
                return _changeRowCSSByCheckBox;
            }
        }

        private ContextMenus _contextMenus;
        /// <summary>
        /// 行的右键菜单集合
        /// </summary>
        [
        PersistenceMode(PersistenceMode.InnerProperty),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Description("行的右键菜单"),
        Category("扩展")
        ]
        public virtual ContextMenus ContextMenus
        {
            get
            {
                if (_contextMenus == null)
                {
                    _contextMenus = new ContextMenus();
                }
                return _contextMenus;
            }
        }

        private Paging.PagingStyleCollection _pagingStyle;
        /// <summary>
        /// 自定义分页样式
        /// </summary>
        [Description("自定义分页样式"), DefaultValue(""), Category("扩展")]
        public Paging.PagingStyleCollection PagingStyle
        {
            get { return _pagingStyle; }
            set { _pagingStyle = value; }
        }
        #endregion

        #region 内部属性

        /// <summary>
        /// 隐藏字段的ID，用于存每组的全选复选框ID
        /// </summary>
        protected string HiddenCheckboxAllID
        {
            get { return "yy_hdn_checkboxAll"; }
        }
        /// <summary>
        /// 隐藏字段的ID，用于存每组的项复选框ID
        /// </summary>
        protected string HiddenCheckboxItemID
        {
            get { return "yy_hdn_checkboxItem"; }
        }

        /// <summary>
        /// 组分隔符，一个 全选复选框 和其对应的n个 项复选框 为一个组
        /// </summary>
        protected char GroupSeparator
        {
            get { return ','; }
        }
        /// <summary>
        /// 项分隔符，项复选框 每个项之间的分隔符
        /// </summary>
        protected char ItemSeparator
        {
            get { return '|'; }
        }

        #endregion

        #region 只可以访问的公共属性
        /// <summary>
        /// 固定行、列的时候X滚动条的位置
        /// </summary>
        public int ScrollX
        {
            get { return this._yy_SmartGridViewAlpha_x; }
        }

        /// <summary>
        /// 固定行、列的时候Y滚动条的位置
        /// </summary>
        public int ScrollY
        {
            get { return this._yy_SmartGridViewAlpha_y; }
        }
        #endregion

        #region 私有变量
        /// <summary>
        /// 用于存每组的全选复选框ID
        /// </summary>
        private string _checkAllIDString;
        /// <summary>
        /// 用于存每的项复选框ID
        /// </summary>
        private string _checkItemIDString;
        /// <summary>
        /// 每行有一个组的所有项复选框
        /// </summary>
        private Dictionary<int, string> _checkItemIDDictionary = new Dictionary<int, string>();
        /// <summary>
        /// 当前行是顶端分页行吗？
        /// </summary>
        private bool _isTopPager = true;
        /// <summary>
        /// 如果固定行、列的话 滚动条的x位置
        /// </summary>
        private int _yy_SmartGridViewAlpha_x;
        /// <summary>
        /// 如果固定行、列的话 滚动条的y位置
        /// </summary>
        private int _yy_SmartGridViewAlpha_y;
        /// <summary>
        /// 是否触发RowDataBound事件
        /// </summary>
        private bool _raiseRowDataBound = false;
        #endregion

        #region 如果固定行、列的话，用于获取滚动条的位置，要继承IPostBackDataHandler接口
        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {

        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            // 获取两个保存了 固定行、列后 的GridView滚动条的位置信息
            _yy_SmartGridViewAlpha_x = String.IsNullOrEmpty(postCollection["yy_SmartGridViewAlpha_x"]) ? 0 : Convert.ToInt32(postCollection["yy_SmartGridViewAlpha_x"]);
            _yy_SmartGridViewAlpha_y = String.IsNullOrEmpty(postCollection["yy_SmartGridViewAlpha_y"]) ? 0 : Convert.ToInt32(postCollection["yy_SmartGridViewAlpha_y"]);

            return false;
        }
        #endregion
    }
}
