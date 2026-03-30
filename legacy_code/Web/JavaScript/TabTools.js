/*创建功能页面选项卡*/
function addTabTool(str) {
    if (str != "null") {
        var name;
        var url;
        var num = str.split("~");
        var addLi = '';
        for (var i = 0; i < num.length; i++) {
            if (num[i] != "") {
                var tools = num[i].split(",");
                name = tools[0];
                url = tools[1];
                if (i == 0) {
                    addLi = addLi + "<li id='current' onclick=current.id='';this.id='current' style='margin:0px;padding:0px;'><a style='font-size: 9pt' href='" + url + "' target='rightPartFrame'><span>" + name + "</span></a></li>";
                }
                else {
                    addLi = addLi + "<li onclick=current.id='';this.id='current' style='margin:0px;padding:0px;'><a style='font-size: 9pt' href='" + url + "' target='rightPartFrame'><span>" + name + "</span></a></li>";
                }
            }

        }
    }
    else {
        addLi = "<table border='0' cellpadding='0' cellspacing='0' ><tr><td style='height: 5px;'></td></tr></table>";
    }
    parent.document.all.TabTools.innerHTML = " <div id='header' style='text-align: right;'> <table border='0' cellpadding='0' cellspacing='0' ><tr><td>   <ul>  " + addLi + " </ul> </td></tr></table>    </div>";
}
function addBack()
{
        var addLi='';
        addLi = addLi + "<li id='current' onclick=current.id='';this.id='current' style='margin:0px;padding:0px;'><a style='font-size: 9pt' href='javascript:history.back(-1)' target='rightPartFrame'><span>后退</span></a></li>";
        parent.document.all.TabTools.innerHTML = " <div id='header' style='text-align: right;'> <table border='0' cellpadding='0' cellspacing='0' ><tr><td>   <ul>  " + addLi + " </ul> </td></tr></table>    </div>";
}

function loadTitle()
{
      parent.document.all.txtWroMessage.value ='';
      parent.document.all.TitleShow.innerText =document.title;
      return;
}

function UpdateTreePage()
{
    window.parent.LeftTree.location.reload();
}

function BtnUp( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
}
function BtnOver( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
}