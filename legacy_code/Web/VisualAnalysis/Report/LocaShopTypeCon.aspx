<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LocaShopTypeCon.aspx.cs" Inherits="VisualAnalysis_Report_LocaShopTypeCon" %>

    
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title></title>
<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript" language="JavaScript" src="../../FusionCharts/JS/FusionCharts.js"></script>
<style type="text/css" media="all">@import "../../grid/doc_no_left.css";</style>
<link href="../../grid/highlight/style.css" rel="stylesheet" type="text/css" />
<LINK href="../../grid/highlight/voteresult.css" type="text/css" rel="Stylesheet">
<link rel="stylesheet" type="text/css" media="all" href="../../grid/calendar/calendar-blue.css"  />
<link rel="stylesheet" type="text/css" href="../../grid/gt_grid.css" />
<link rel="stylesheet" type="text/css" href="../../grid/skin/vista/skinstyle.css" />

<link href="../../grid/highlight/style.css" rel="stylesheet" type="text/css" />
<script src="../../grid/highlight/jssc3.js" type="text/javascript"></script>
<script type="text/javascript" src="../../grid/calendar/calendar.js"></script>
<script type="text/javascript" src="../../grid/calendar/calendar-cn-utf8.js"></script>
<script type="text/javascript" src="../../grid/calendar/calendar-setup.js"></script>
<script type="text/javascript" src="../../grid/data/test_data.js"></script>
<script type="text/javascript" src="../../grid/data/xyhz_data.js"></script>
<script type="text/javascript" src="../../grid/data/czxzb_data.js"></script>
<script type="text/javascript" src="../../grid/gt_msg_en.js"></script>
<script type="text/javascript" src="../../grid/gt_const.js"></script>
<script type="text/javascript" src="../../grid/gt_grid_all.js"></script>

<script language="javascript" type="text/javascript">
    function Load() {
        var addr = "<%=strPath %>";
        addTabTool("返回," + addr);
        loadTitle();
    }
    function changeXML(t, d) {
        document.getElementById('xmlFile2').innerText = String(t);
        document.frames(d).document.location.reload();
    }
    function liOnclick(n, c) {
        for (var i = 0; i < c; i++) {
            var liA = document.getElementById(String('l' + String(i)));
            liA.className = 'LI';
        }
        var liId = document.getElementById(String('l' + String(n)));
        if (liId.className == 'LI') {
            liId.className = 'LiOnclc';
        } else if (liId.className == 'LiOnclc') {
            liId.className = 'LI';
        }
        if (n == "1") {
            changeXML('../../FusionCharts/xml/Combi3D2cz.xml', 'iFr1')
        }
        if(n=='2') {
            changeXML('../../FusionCharts/xml/Combi3D2sp.xml', 'iFr1')
        }
        if (n == '0') {
            changeXML('../../FusionCharts/xml/Combi3D2zld.xml', 'iFr1')
        }
    }
//项目空置面积
var grid_demo_id2 = "myGrid2";
var dsOption = {

    fields: [
		{ name: 'country' },
		{ name: 'employee' }
	],

    recordType: 'array',
    data: __TEST_DATA__
}
function my_renderer(value, record, columnObj, grid, colNo, rowNo) {
    var no = record[columnObj.fieldIndex];
    var color = "000000";
    return "<span style=\"color:" + color + ";\">" + no + "</span>";
}

function my_Numrenderer(value, record, columnObj, grid, colNo, rowNo) {
    var no = record[columnObj.fieldIndex];
    if (no <= 0) {
        var color = "ff0000";
        return "<span style=\"color:" + color + ";\">" + no + "</span>";
    } else if (no > 0) {
        var color = "000000";
        return "<span style=\"color:" + color + ";\">" + no + "</span>";
    }
}

function my_hdRenderer(header, colObj, grid) {
    var color = "666666";
    return "<span style=\"color:" + color + ";\" font:24px;>" + String(header) + "</span>";
}

function my_Imgrenderer(value, record, columnObj, grid, colNo, rowNo) {
    var no = record[columnObj.fieldIndex];
    return "<img style=\"padding-top:4px;\" src=\"" + no.toLowerCase() + "\">";
}

var colsOption = [
     { id: 'employee', header: "项目名称", width: 120, align: "left", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'country', header: "面积", width: 100, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer }
];
var gridOption2 = {
    id: grid_demo_id2,
    width: "223",  //"100%", // 700,
    height: "160",  //"100%", // 330
    pageSize: 5,   //数据行数
    container: 'gridbox2',
    replaceContainer: true,
    showGridMenu: false,
    allowCustomSkin: true,
    allowFreeze: true,
    allowHide: true,
    allowGroup: true,
    toolbarContent: 'nav',
    toolbarPosition: 'no',
    dataset: dsOption,    //数据
    columns: colsOption,
    skin: 'mac'
};
var mygrid2 = new Sigma.Grid(gridOption2);
Sigma.Util.onLoad(Sigma.Grid.render(mygrid2));

//即将到期
var grid_demo_id3 = "myGrid3";
var dsOption3 = {

    fields: [
		{ name: 'country' },
		{ name: 'employee' },
		{ name: 'dhl' }
	],

    recordType: 'array',
    data: __XYHZ_DATA__
}
var colsOption3 = [
     { id: 'employee', header: "项目名称", width: 120, align: "left", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'country', header: "面积", width: 100, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer }
];
var gridOption3 = {
    id: grid_demo_id3,
    width: "223",  //"100%", // 700,
    height: "125",  //"100%", // 330
    pageSize: 6,
    container: 'gridbox3',
    replaceContainer: true,
    showGridMenu: true,
    allowCustomSkin: true,
    allowFreeze: true,
    allowHide: true,
    allowGroup: true,
    toolbarContent: 'nav',
    toolbarPosition: 'no',
    dataset: dsOption3,
    columns: colsOption3,
    skin: 'mac'
};
var mygrid3 = new Sigma.Grid(gridOption3);
Sigma.Util.onLoad(Sigma.Grid.render(mygrid3));

</script>
<style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	padding:0px;
}
-->
</style> 


</head>
<body style="margin-top:0px;" onload="liOnclick(0,3);Load();">
<table width="100%" height="100" border="0" cellpadding="0" cellspacing="0" class="spaceBg" style="margin:0px;">
  <tr>       
         <td colspan=2 width="60%" height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> <%=deptName %>商铺类型出租率分析 </td>

  </tr>
  <tr height="80%">
          <td width="20%" align="center" valign="top" colspan="1">
							<table width="96%" height="195" border="0" cellpadding="0" cellspacing="0" style="border:#dddddd 1px solid;margin-top:6px;">
                              <tr>
                                <td width="100%" height="25" align="left" valign="middle" bgcolor="#F9F9F9" class="smallTitleTxt" style="border:0px;border-bottom:#dddddd 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> 当前项目空置面积(单位:平米)</td>
                                
                              </tr>
                              <tr>
                                <td height="170" colspan="2" align="center" valign="top">
				                    <div id="gridbox2"></div></td>
                              </tr>
                            </table>
                            <table width="96%" height="154" border="0" cellpadding="0" cellspacing="0" style="border:#ddd 1px solid;margin-top:6px;">
                            <tr>
                              <td width="100%" height="25" align="left" bgcolor="#F9F9F9" class="smallTitleTxt" style="border:0px;border-bottom:#ddd 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> 合同即将到期面积(单位:平米)</td>
                            </tr>
                            <tr>
                              <td height="129" align="center" valign="top" >
				                <div id="gridbox3" style="height:100px;width:100px;" ></div>
  			                  </td>
                            </tr>
                          </table>
		
		 </td>
		 <td width="80%" valign="top" colspan="1">
		                <table width="100%" height="355" border="0" cellpadding="0" cellspacing="0" style="border:#dddddd 1px solid;margin-top:6px;">
		                    <tr height="25" valign="top">
		                        <td width="100%" colspan="2" height="25" align="left" valign="middle" bgcolor="#F9F9F9" class="smallTitleTxt" style="border:0px;border-bottom:#dddddd 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> 商铺类型出租率分析
		                        </td>
		                    </tr>
		                    <tr height="330px" valign="middle" align="center"  >
		                        <td width="100%" bgcolor="#dddddd" align="center" >
		                            <DIV class="box-area" id="from_global_host" style="DISPLAY:block;">
                                    <menu style="display:block;" class="menu-area">
                                    <li id="l0" class="LI" onmouseup="liOnclick(0,3)" style="cursor:hand;">主力店
                                    </li>
                                    <li id="l1" class="LI" onmouseup="liOnclick(1,3)" style="cursor:hand;">次主力店
                                    </li>
                                     <li id="L2" class="LI" onmouseup="liOnclick(2,3)" style="cursor:hand;">散铺
                                    </li>
                                    </menu>
									<div class="infor_tb">
											 <iframe id="iFr1" src="../../FusionCharts/chartMSCol3D.aspx" width="100%" height="322" scrolling="no" frameborder="0">
		                                    </iframe>
									</div>
                                    </DIV>
	                          </td> 
		                    </tr>
           </table>
		 </td>
  </tr>

</table>
<table style="display:none";><tr><td id="xmlFile2">../../FusionCharts/xml/Combi3D2zld.xml</td></tr></table>
</body>
</html>
