
/*创建树 <树节点的字符串(depttxt)，选择的节点ID(deptid)，记录选择过的节点ID(selectdeptid)，点击节点触发的事件控件名称(treeClick)>*/
var tabbar ;
function treearray(TreeStr,SelectID,SelectedID,ClickName)
{
    var t = new NlsTree('MyTree');
    var treestr =document.getElementById("depttxt").value;
     
    var treearr = new Array();
    var n=0;
    var id;
    var pid;
    var name;
    var imgurl;
    var num = treestr.split("^");
    for(var i=0;i<num.length-1;i++)
    {
        if(num[i]!="")
        {
           var treenode = num[i].split("|");
            for(var j=0;j<treenode.length;j++)
            {
                id=treenode[0];
                pid=treenode[1];
                name=treenode[2];
                imgurl=treenode[3];
            }
            
            t.add(id, pid, name, "", imgurl, true);
            
        }
    }
            t.opt.sort='no';
            t.opt.enbScroll=true;
            t.opt.height="260px";
            t.opt.width="235px";
            t.opt.trg="mainFrame";
            t.opt.oneExp=true;

            t.treeOnClick = ev_click;
            t.render("treeview");
            t.collapseAll();

            if(document.form1.selectdeptid.value!='')
            {
                t.expandNode(document.form1.selectdeptid.value);
                t.selectNodeById(document.form1.selectdeptid.value);
            }
            
}

function ev_click(e, id)
{
    document.form1.deptid.value=id;
    document.form1.selectdeptid.value=id;
    document.form1.treeClick.click(); 
     
} 

