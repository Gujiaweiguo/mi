//----------------------------
// http://webabcd.cnblogs.com/
//----------------------------

/*Helper 开始*/
String.prototype.yy_stv_startsWith = function(s)
{   
/// <summary>StartsWith()</summary>

    var reg = new RegExp("^" + s);   
    return reg.test(this);
}

function yy_stv_addEvent(obj, evtType, fn) 
{
/// <summary>绑定事件</summary>

    // FF
    if (obj.addEventListener)
    {
        obj.addEventListener(evtType, fn, true);
        return true;
    }
    // IE
    else if (obj.attachEvent)
    {
        var r = obj.attachEvent("on" + evtType, fn);
        return r;
    }
    else
    {
        return false;
    }    
}
/*Helper 结束*/


/*联动复选框 开始*/
var yy_stv_ccTreeView_pre = new Array(); // cs中动态向其灌数据（TreeView内控件ID的前缀数组）

function yy_stv_ccClickCheckbox(e) 
{
/// <summary>单击复选框时</summary>

    var evt = e || window.event; // FF || IE
    var obj = evt.target || evt.srcElement  // FF || IE
    
    yy_stv_checkChildCheckbox(obj);
    yy_stv_checkCertainCheckbox(obj);
}


function yy_stv_checkParentCheckbox(table, checked)
{
/// <summary>设置全选复选框的状态</summary>

    var nodes = table.parentNode.parentNode.childNodes;
        
    for (var i=1; i<nodes.length; i++)
    {
        if (nodes[i] == table.parentNode)
        {
            if (typeof(nodes[i-1]) == 'undefined' || typeof(nodes[i-1].rows) == 'undefined') return;

            for (var j=0; j < nodes[i-1].rows[0].cells.length; j++)
            {
                var chk = nodes[i-1].rows[0].cells[j].childNodes[0];
                if (chk.tagName == "INPUT" && chk.type == "checkbox") 
                {
                    chk.checked = checked;
                    yy_stv_checkCertainCheckbox(nodes[i-1]);
                    return;
                }
            }
        }
    }
}

function yy_stv_checkChildCheckbox(obj)
{   
/// <summary>单击子复选框时</summary>

    var checked;
    
    if (obj.tagName == "INPUT" && obj.type == "checkbox") 
    {
        checked = obj.checked;
        do
        {
            obj = obj.parentNode;
        } 
        while (obj.tagName != "TABLE")
    }
    
    var nodes = obj.parentNode.childNodes;
        
    for (var i=0; i<nodes.length - 1; i++)
    {
        if (nodes[i] == obj && nodes[i + 1].tagName == "DIV")
        {
            var elements = nodes[i+1].getElementsByTagName("INPUT");
            
            for (j=0; j< elements.length; j++) 
            {       
                if (elements[j].type == 'checkbox') 
                {
                    elements[j].checked = checked;
                }
            }    
        }
    }
}

function yy_stv_checkCertainCheckbox(obj)
{    
/// <summary>单击某一复选框时</summary>

    var checkedNum = 0;
    var uncheckedNum = 0;
    
    if (obj.tagName == "INPUT" && obj.type == "checkbox") 
    {
        do
        {
            obj = obj.parentNode;
        } 
        while (obj.tagName != "TABLE")
    }
                
    var tables = obj.parentNode.getElementsByTagName("TABLE");
     
    if (typeof(tables) == 'undefined') return;
       
    for (var i=0; i < tables.length; i++)
    {        
        for (var j=0; j < tables[i].rows[0].cells.length; j++)
        {
            var chk = tables[i].rows[0].cells[j].childNodes[0];
            if (chk.tagName == "INPUT" && chk.type == "checkbox") 
            {
                if (chk.checked)
                    checkedNum ++;
                else
                    uncheckedNum ++;
            }
        }
    }
    
    if (uncheckedNum == 0)
    {
        yy_stv_checkParentCheckbox(obj, true);
    }
    else
    {
        yy_stv_checkParentCheckbox(obj, false);
    }
}


function yy_stv_attachCheckboxClickListener()
{
/// <summary>监听所有联动复选框的单击事件</summary>

    var elements =  document.getElementsByTagName("INPUT");
    
    for (i=0; i< elements.length; i++) 
    {       
        if (elements[i].type == 'checkbox') 
        {
            for (j=0; j<yy_stv_ccTreeView_pre.length; j++)
            {
                if (elements[i].id.yy_stv_startsWith(yy_stv_ccTreeView_pre[j]))
                {
                    yy_stv_addEvent(elements[i], 'click', yy_stv_ccClickCheckbox); 
                    break;
                }
            }
        }
    }    
}

if (document.all)
{
    window.attachEvent('onload', yy_stv_attachCheckboxClickListener);
}
else
{
    window.addEventListener('load', yy_stv_attachCheckboxClickListener, false);
}
/*联动复选框 结束*/