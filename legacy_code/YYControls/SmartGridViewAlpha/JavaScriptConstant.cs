using System;
using System.Collections.Generic;
using System.Text;

namespace YYControls.SmartGridViewAlpha
{
    /// <summary>
    /// javascript
    /// </summary>
    public class JavaScriptConstant
    {
        #region 复选框全选的脚本
        internal const string jsCheckAll = @"<script type=""text/javascript"">
        //<![CDATA[
        // 是否调用“yy_ClickCheckItem”函数
        var isInvokeClickCheckItem = true;
        // 隐藏字段的ID，用于存每组的全选复选框ID
        var hdnAllName = '[$AllName$]';
        // 隐藏字段的ID，用于存每的项复选框ID
        var hdnItemName = '[$ItemName$]';
        // 组分隔符，一个 全选复选框 和其对应的n个 项复选框 为一个组
        var groupSeparator = '[$GroupSeparator$]';
        // 项分隔符，项复选框 每个项之间的分隔符
        var itemSeparator = '[$ItemSeparator$]';

        var strAlls = yy_GetObject(hdnAllName).value;
        var strItems = yy_GetObject(hdnItemName).value;

        // 全选复选框数组
        var aryAlls = strAlls.split(groupSeparator);
        // 项复选框数组，每行有一个组的所有项复选框
        var aryItems = strItems.split(groupSeparator);

        // 全选复选框被单击，参数为 该全选复选框这个对象
        function yy_ClickCheckAll(objAll)
        {
            // 每个全选复选框
	        for (var i=0; i<aryAlls.length; i++)
	        {
                // 该全选复选框是所单击的全选复选框
		        if (aryAlls[i] == objAll.id)
		        {
                    // 该全选复选框同组下的项复选框数组
			        aryItem = aryItems[i].split(itemSeparator);
			        for (var j=0; j<aryItem.length; j++)
			        {
                        // 项复选框是disabled，则不进行任何操作
                        if (yy_GetObject(aryItem[j]).parentElement.disabled)
                        {
                            yy_GetObject(aryItem[j]).checked = false;
                        }
                        // 单击的全选复选框是选中状态
				        else if (objAll.checked)
				        {
					        yy_GetObject(aryItem[j]).checked = false;

                            isInvokeClickCheckItem = false;
                            // 调用该CheckBox的click事件
                            // 注意如果CheckBox的checked是false，那么调用其click事件后，checked将变成true
                            yy_GetObject(aryItem[j]).click();
				        }
				        else
				        {
					        yy_GetObject(aryItem[j]).checked = true;

                            isInvokeClickCheckItem = false;
                            // 调用该CheckBox的click事件
                            // 注意如果CheckBox的checked是true，那么调用其click事件后，checked将变成false
                            yy_GetObject(aryItem[j]).click();
				        }

			        }
        		
			        break;
		        }
	        }
        }

        // 项复选框被单击
        function yy_ClickCheckItem()
        {
            if (isInvokeClickCheckItem == false) 
            {
                isInvokeClickCheckItem = true;
                return;
            }

            // 每组项复选框
	        for (var i=0; i<aryItems.length; i++)
	        {
                // 该组的所有项复选框数组
		        aryItem = aryItems[i].split(itemSeparator);
                // 标记，是否同组的项复选框都被选中
		        var bln = true;
		        for (var j=0; j<aryItem.length; j++)
		        {
                    // 如果该项复选框没被选中，并且不是disabled，则bln设为false
			        if (!yy_GetObject(aryItem[j]).checked && !yy_GetObject(aryItem[j]).parentElement.disabled)
			        {
				        bln = false;
				        break;
			        }
		        }
        		
                // bln为true，则设置同组的全选复选框为选中
		        if (bln)
		        {
			        yy_GetObject(aryAlls[i]).checked = true;
		        }
                // 否则
		        else
		        {
			        yy_GetObject(aryAlls[i]).checked = false;
		        }
	        }
        }

        function yy_GetObject(param)
        {
            if (document.getElementById(param))
	            return document.getElementById(param);
        }

        //]]>
        </script>";
        #endregion

        #region 数据行响应鼠标的单击和双击事件
        internal const string jsClickAndDoubleClick = @"<script type=""text/javascript"">
        //<![CDATA[
        // 判断是否响应双击的变量。false-响应单击；true响应双击。默认响应双击。
        var isDoubleClick = true;
        function yy_RowClick(id)
        {
            isDoubleClick = false; 
            setTimeout(""yy_RowClickTimeout('""+id+""')"", 300);
        }
        function yy_RowClickTimeout(id)
        {
            if (isDoubleClick == false)
            {
                // 执行ID所指按钮的click事件
                document.getElementById(id).click();
            }
            isDoubleClick = true;
        }
        function yy_RowDoubleClick(id)
        {
            isDoubleClick = true; 
            if (isDoubleClick == true)
            {
                // 执行ID所指按钮的click事件
                document.getElementById(id).click();
            }
            isDoubleClick = true;
        }
        //]]>
        </script>";
        #endregion

        #region 修改行的className
        internal const string jsChangeRowClassName = @"<script type=""text/javascript"">
        //<![CDATA[
        function yy_ChangeRowClassName(id, cssClass, isForce)
        {
            objRow = document.getElementById(id);
            // 如果row的yy_selected属性是'false'或者没有yy_selected属性或者要求强制设置
            // 那么修改该行的className
            if (!objRow.attributes['yy_selected'] || objRow.attributes['yy_selected'].value == 'false' || isForce == true)
            {
                document.getElementById(id).className = cssClass;
            }
        }
        // 设置行的yy_selected属性
        function yy_SetRowSelectedAttribute(id, bln)
        {
            document.getElementById(id).attributes['yy_selected'].value = bln;
        }
        // 以id结尾的CheckBox执行两次click事件
        function yy_DoubleClickCheckBox(id)
        {
            var allInput = document.all.tags('INPUT');
  　　　　　for (var i=0; i < allInput.length; i++) 
  　　　　　{
  　　　　　    if (allInput[i].type == 'checkbox' && allInput[i].id.endWith('checkitem'))
   　　　　     {
                    // 触发click事件而不执行yy_ClickCheckItem()函数
                    isInvokeClickCheckItem = false;
    　　　　　　    allInput[i].click();
                    isInvokeClickCheckItem = false;
                    allInput[i].click();
   　　　　　　 }     
  　　　　　}

        }
        String.prototype.endWith = function(oString){   
            var reg = new RegExp(oString + ""$"");   
            return reg.test(this);
        }  
        //]]>
        </script>";
        #endregion

        #region 给数据行增加右键菜单
        internal const string jsContextMenu = @"<script type=""text/javascript"">
        //<![CDATA[
        // 数据行的ClientId
        var _rowClientId = '';

        // 以下实现右键菜单，网上找的，不知道原创是谁
        function contextMenu()
        {
	        this.items = new Array();
	        this.addItem = function (item)
	        {
		        this.items[this.items.length] = item;
	        }

	        this.show = function (oDoc)
	        {
		        var strShow = '';
		        var i;

                // 加上word-break: keep-all; 防止菜单项换行
		        strShow = ""<div id='rightmenu' style='word-break: keep-all;BACKGROUND-COLOR: #ffffff; BORDER: #000000 1px solid; LEFT: 0px; POSITION: absolute; TOP: 0px; VISIBILITY: hidden; Z-INDEX: 10'>"";
		        strShow += ""<table border='0' height='"";
		        strShow += this.items.length * 20;
		        strShow += ""' cellpadding='0' cellspacing='0'>"";
		        strShow += ""<tr height='3'><td bgcolor='#d0d0ce' width='2'></td><td>"";
		        strShow += ""<table border='0' width='100%' height='100%' cellpadding=0 cellspacing=0 bgcolor='#ffffff'>"";
		        strShow += ""<tr><td bgcolor='#d0d0ce' width='23'></td><td><img src=' ' height='1' border='0'></td></tr></table>"";
		        strShow += ""</td><td width='2'></td></tr>"";
		        strShow += ""<tr><td bgcolor='#d0d0ce'></td><td>"";
		        strShow += ""<table border='0' width='100%' height='100%' cellpadding=3 cellspacing=0 bgcolor='#ffffff'>"";
        		
		        oDoc.write(strShow);

		        for(i=0; i<this.items.length; i++)
		        {
			        this.items[i].show(oDoc);
		        }
        		
		        strShow = ""</table></td><td></td></tr>"";
		        strShow += ""<tr height='3'><td bgcolor='#d0d0ce'></td><td>"";
		        strShow += ""<table border='0' width='100%' height='100%' cellpadding=0 cellspacing=0 bgcolor='#ffffff'>"";
		        strShow += ""<tr><td bgcolor='#d0d0ce' width='23'></td><td><img src=' ' height='1' border='0'></td></tr></table>"";
		        strShow += ""</td><td></td></tr>"";
		        strShow += ""</table></div>\n"";
        		
		        oDoc.write(strShow);
	        }
        }

        function contextItem(text, icon, cmd, url, target, type, key, value)
        {
	        this.text = text ? text : '';
	        this.icon = icon ? icon : '';
	        this.cmd = cmd ? cmd : '';
            this.url = url ? url : '';
	        this.target = target ? target : '';
	        this.type = type ? type : 'Link';
            this.key = key ? key : '';
            this.value = value ? value : '';

	        this.show = function (oDoc)
	        {
		        var strShow = '';

		        if(this.type == 'Link' || this.type == 'Command' || this.type == 'Custom')
		        {
			        strShow += ""<tr "";
			        strShow += ""onmouseover=\""changeStyle(this, 'on');\"" "";
			        strShow += ""onmouseout=\""changeStyle(this, 'out');\"" "";

                    if (this.type == 'Command')
                    {
                        // 右键菜单是按钮类型，调用所对应的按钮的click事件
			            strShow += ""onclick=\""document.getElementById("";
			            strShow += ""_rowClientId + "";
                        strShow += ""'_"";
                        strShow += this.cmd;
                        strShow += ""').click()"";
                    }
                    else if (this.type == 'Custom')
                    {
                        // 右键菜单是用户自定义的
                        strShow += this.key;
                        strShow += ""="";
			            strShow += ""\"""";
                        strShow += this.value;
                    }
                    else
                    {
                        // 右键菜单是链接类型
                        if (this.target == 'Top') this.target = 'top';
                        if (this.target == 'Self') this.target = 'self';

                        if (this.target == 'top' || this.target == 'self')
                        {
                            strShow += ""onclick=\"""";
                            strShow += this.target;
			                strShow += "".location='"";
                            strShow += this.url;
                            strShow += ""'"";
                        }
                        else
                        {
                            strShow += ""onclick=\""window.open('"";
                            strShow += this.url;
                            strShow += ""')"";
                        }
                    }   
			        strShow += ""\"">"";
			        strShow += ""<td class='ltdexit' width='16'>"";

			        if (this.icon == '')
                    {
				        strShow += '&nbsp;';
                    }
			        else 
			        {
				        strShow += ""<img border='0' src='"";
				        strShow += this.icon;
				        strShow += ""' width='16' height='16' style='POSITION: relative'></img>"";
			        }

			        strShow += ""</td><td class='mtdexit'>"";
			        strShow += this.text;
			        strShow += ""</td><td class='rtdexit' width='5'>&nbsp;</td></tr>"";
		        }
                // 右键菜单是分隔线
		        else if (this.type == 'Separator')
		        {
			        strShow += ""<tr><td class='ltdexit'>&nbsp;</td>"";
			        strShow += ""<td class='mtdexit' colspan='2'><hr color='#000000' size='1'></td></tr>"";
		        }

		        oDoc.write(strShow);
	        }
        }

        function changeStyle(obj, cmd)
        { 
	        if(obj)
	        {
		        try 
		        {
			        var imgObj = obj.children(0).children(0);

			        if(cmd == 'on') 
			        {
				        obj.children(0).className = 'ltdfocus';
				        obj.children(1).className = 'mtdfocus';
				        obj.children(2).className = 'rtdfocus';
        				
				        if(imgObj)
				        {
					        if(imgObj.tagName.toUpperCase() == 'IMG')
					        {
						        imgObj.style.left = '-1px';
						        imgObj.style.top = '-1px';
					        }
				        }
			        }
			        else if(cmd == 'out') 
			        {
				        obj.children(0).className = 'ltdexit';
				        obj.children(1).className = 'mtdexit';
				        obj.children(2).className = 'rtdexit';

				        if(imgObj)
				        {
					        if(imgObj.tagName.toUpperCase() == 'IMG')
					        {
						        imgObj.style.left = '0px';
						        imgObj.style.top = '0px';
					        }
				        }
			        }
		        }
		        catch (e) {}
	        }
        }

        function showMenu(rowClientId)
        {
            _rowClientId = rowClientId;

	        var x, y, w, h, ox, oy;

	        x = event.clientX;
	        y = event.clientY;

	        var obj = document.getElementById('rightmenu');

	        if (obj == null)
		        return true;

	        ox = document.body.clientWidth;
	        oy = document.body.clientHeight;

	        if(x > ox || y > oy)
		        return false;

	        w = obj.offsetWidth;
	        h = obj.offsetHeight;

	        if((x + w) > ox)
		        x = x - w;

	        if((y + h) > oy)
		        y = y - h;

         
	        // obj.style.posLeft = x + document.body.scrollLeft;
	        // obj.style.posTop = y + document.body.scrollTop;
            // xhtml不支持上面的了
            // 就是说如果你的页头声明了页是xhtml的话就不能用上面那句了，vs2005创建的aspx会默认加上xhtml声明
            // 此时应该用如下的方法
            obj.style.posLeft = x + document.documentElement.scrollLeft;
            obj.style.posTop = y + document.documentElement.scrollTop;

	        obj.style.visibility = 'visible';

	        return false;
        }

        function hideMenu()
        {
	        if(event.button == 0)
	        {
		        var obj = document.getElementById('rightmenu');
		        if (obj == null)
			        return true;

		        obj.style.visibility = 'hidden';
		        obj.style.posLeft = 0;
		        obj.style.posTop = 0;
	        }
        }

        function writeStyle()
        {
	        var strStyle = '';

	        strStyle += ""<STYLE type='text/css'>"";
	        strStyle += ""TABLE {Font-FAMILY: 'Tahoma','Verdana','宋体'; FONT-SIZE: 9pt}"";
	        strStyle += "".mtdfocus {BACKGROUND-COLOR: #ccccff; BORDER-BOTTOM: #000000 1px solid; BORDER-TOP: #000000 1px solid; CURSOR: hand}"";
	        strStyle += "".mtdexit {BACKGROUND-COLOR: #ffffff; BORDER-BOTTOM: #ffffff 1px solid; BORDER-TOP: #ffffff 1px solid}"";
	        strStyle += "".ltdfocus {BACKGROUND-COLOR: #ccccff; BORDER-BOTTOM: #000000 1px solid; BORDER-TOP: #000000 1px solid; BORDER-LEFT: #000000 1px solid; CURSOR: hand}"";
	        strStyle += "".ltdexit {BACKGROUND-COLOR: #d0d0ce; BORDER-BOTTOM: #d0d0ce 1px solid; BORDER-TOP: #d0d0ce 1px solid; BORDER-LEFT: #d0d0ce 1px solid}"";
	        strStyle += "".rtdfocus {BACKGROUND-COLOR: #ccccff; BORDER-BOTTOM: #000000 1px solid; BORDER-TOP: #000000 1px solid; BORDER-RIGHT: #000000 1px solid; CURSOR: hand}"";
	        strStyle += "".rtdexit {BACKGROUND-COLOR: #ffffff; BORDER-BOTTOM: #ffffff 1px solid; BORDER-TOP: #ffffff 1px solid; BORDER-RIGHT: #ffffff 1px solid}"";
	        strStyle += ""</STYLE>"";

	        document.write(strStyle);
        }

        function makeMenu()
        {
	        var myMenu, item;

	        myMenu = new contextMenu();

            // 增加右键菜单项 开始
            // item = new contextItem("", "", "", "", "", "", "", "");
            // 1-菜单项的文本
            // 2-图标链接
            // 3-所调用的命令按钮的ID
            // 4-链接地址
            // 5-链接的target
            // 6-右键菜单的项的类别
            // 7-自定义属性key
            // 8-自定义属性value
            // myMenu.addItem(item);

            [$MakeMenu$]
            // 增加右键菜单项 结束

	        myMenu.show(this.document);

	        delete item;

	        delete myMenu;
        }

        function toggleMenu(isEnable)
        {
	        if(isEnable)
		        document.oncontextmenu = showMenu;
	        else
		        document.oncontextmenu = new function() {return true;};
        }

        writeStyle();

        makeMenu();

        document.onclick = hideMenu;
        //]]>
        </script>";
        #endregion
    }
}
