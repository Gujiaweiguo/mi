扩展GridView控件正式版


详细介绍
http://www.cnblogs.com/webabcd/archive/2007/02/04/639830.html


控件使用
1、鼠标经过行的时候改变该行的样式，鼠标离开行的时候恢复该行的样式
使用方法（设置属性）：  
MouseOverCssClass - 鼠标经过行时行的 CSS 类名

2、对多个字段进行复合排序；升序、降序的排序状态提示
使用方法（设置SmartSorting复合属性）： 
AllowSortTip - 是否启用排序提示 
AllowMultiSorting - 是否启用复合排序 
SortAscImageUrl - 升序提示图片的URL（不设置则使用默认图片） 
SortDescImageUrl - 降序提示图片的URL（不设置则使用默认图片） 
SortAscText - 升序提示文本 
SortDescText - 降序提示文本

3、根据按钮的CommandName设置其客户端属性
使用方法（设置ClientButtons集合属性）： 
BoundCommandName - 需要绑定的CommandName 
AttributeKey - 属性的名称 
AttributeValue - 属性的值（两个占位符：{0} - CommandArgument；{1} - Text） 
Position - 属性的值的位置

4、联动复选框（复选框的全选和取消全选）。选中指定的父复选框，则设置指定的所有子复选框为选中状态；取消选中指定的父复选框，则设置指定的所有子复选框为取消选中状态 
使用方法（设置CascadeCheckboxes集合属性）： 
ParentCheckboxID - 模板列中 父复选框ID 
ChildCheckboxID - 模板列中 子复选框ID 
YYControls.Helper.SmartGridView中的静态方法 
List GetCheckedDataKey(GridView gv, int columnIndex) 
List GetCheckedDataKey(GridView gv, string checkboxId)

5、固定指定行、指定列，根据RowType固定行，根据RowState固定行 
使用方法（设置FixRowColumn复合属性）： 
FixRowType - 需要固定的行的RowType（用逗号“,”分隔） 
FixRowState - 需要固定的行的RowState（用逗号“,”分隔） 
FixRows - 需要固定的行的索引（用逗号“,”分隔） 
FixColumns - 需要固定的列的索引（用逗号“,”分隔） 
TableWidth - 表格的宽度 
TableHeight - 表格的高度

6、响应行的单击事件和双击事件，并在服务端处理 
使用方法（设置属性）： 
BoundRowClickCommandName - 行的单击事件需要绑定的CommandName 
BoundRowDoubleClickCommandName - 行的双击事件需要绑定的CommandName

7、行的指定复选框选中的时候改变该行的样式，行的指定复选框取消选中的时候恢复该行的样式 
使用方法（设置CheckedRowCssClass复合属性）： 
CheckBoxID - 模板列中 数据行的复选框ID 
CssClass - 选中的行的 CSS 类名

8、导出数据源的数据为Excel、Word或Text（应保证数据源的类型为DataTable或DataSet） 
使用方法： 
为SmartGridView添加的方法
Export(string fileName)
Export(string fileName, ExportFormat exportFormat)
Export(string fileName, ExportFormat exportFormat, Encoding encoding)
YYControls.Helper.Common中的静态方法
Export(DataTable dt, ExportFormat exportFormat, string fileName, Encoding encoding)
Export(DataTable dt, int[] columnIndexList, ExportFormat exportFormat, string fileName, Encoding encoding)
Export(DataTable dt, int[] columnIndexList, string[] headers, ExportFormat exportFormat, string fileName, Encoding encoding)
Export(DataSet ds, string[] headers, string[] fields, ExportFormat exportFormat, string fileName, Encoding encoding)

9、给数据行增加右键菜单，响应服务端事件或超级链接 
使用方法（设置ContextMenus集合属性）： 
Text - 菜单的文本内容 
BoundCommandName - 需要绑定的CommandName 
NavigateUrl - 链接的URL 
Target - 链接的目标窗口或框架 
SmartGridView的属性ContextMenuCssClass - 右键菜单的级联样式表 CSS 类名（右键菜单的结构div ul li a）

10、自定义分页样式。显示总记录数、每页记录数、当前页数、总页数、首页、上一页、下一页、末页和分页按钮 
使用方法（设置CustomPagerSettings复合属性）： 
PagingMode - 自定义分页的显示模式 
TextFormat - 自定义分页的文本显示样式（四个占位符：{0}-每页显示记录数；{1}-总记录数；{2}-当前页数；{3}-总页数）


OK