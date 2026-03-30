<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CityShopTypeSaleDetail.aspx.cs" Inherits="VisualAnalysis_Report_CityShopTypeSaleDetail" %>


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
<link rel="stylesheet" type="text/css" href="../../grid/skin/china/skinstyle.css" />
<link rel="stylesheet" type="text/css" href="../../grid/skin/mac/skinstyle.css" />
<link href="../../grid/highlight/style.css" rel="stylesheet" type="text/css" />
<script src="../../grid/highlight/jssc3.js" type="text/javascript"></script>
<script type="text/javascript" src="../../grid/calendar/calendar.js"></script>
<script type="text/javascript" src="../../grid/calendar/calendar-cn-utf8.js"></script>
<script type="text/javascript" src="../../grid/calendar/calendar-setup.js"></script>
<script type="text/javascript" src="../../grid/data/LocaSales.js"></script>


<script type="text/javascript" src="../../grid/gt_msg_en.js"></script>
<script type="text/javascript" src="../../grid/gt_const.js"></script>
<script type="text/javascript" src="../../grid/gt_grid_all.js"></script>

<script language="javascript" type="text/javascript">
    function Load() {
        var addr = "<%=strPath %>";
        addTabTool("返回," + addr);
        loadTitle();
    }

//区域账龄分析
var grid_demo_id2 = "myGrid2";
var dsOption = {

    fields: [
        { name: 'store' },
		{ name: 'mthSale' },
		{ name: 'ppSale' },
		{ name: 'ppRate' },
		{ name: 'lySale' },
		{ name: 'lyRate' }
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
     { id: 'store', header: "项目名称", width: 120, align: "left", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'mthSale', header: "当月销售(元)", width: 140, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'ppSale', header: "环比销售(元)", width: 100, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'ppRate', header: "环比差异(%)", width: 80, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'lySale', header: "同比销售(元)", width: 80, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'lyRate', header: "同比差异(%)", width: 80, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer }
];
var gridOption2 = {
	id : grid_demo_id2,
	width: "100%",  //"100%", // 700,
	height: "420",  //"100%", // 330,
	container : 'gridbox2', 
	replaceContainer : true, 
	dataset : dsOption ,
	columns : colsOption,
	pageSize : 10,
	toolbarContent : 'print', //打印
	skin: 'mac'
};


var mygrid2 = new Sigma.Grid(gridOption2);
Sigma.Util.onLoad(Sigma.Grid.render(mygrid2));


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
<body style="margin-top:0px;" onload="Load();">
<table width="100%" height="100" border="0" cellpadding="0" cellspacing="0" class="spaceBg" style="margin:0px;">
  <tr>       
         <td colspan=2 width="60%" height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> <%=deptName %>项目商铺类型销售分析 </td>

  </tr>
  <tr height="100%">
          <td width="100%" align="left" valign="top" colspan="1">
				<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" style="border:#dddddd 1px solid;margin-top:6px;">
                  <tr>
                    <td width="100%" height="25" align="left" valign="middle" bgcolor="#F9F9F9" class="smallTitleTxt" style="border:0px;border-bottom:#dddddd 1px solid;padding-left:10px;">
                        <img src="../../images/Bullet.gif" width="9" height="9" /> 各项目商铺类型销售分析  --  2009年11月
                    </td>                                
                  </tr>
                  <tr>
                    <td width="100%" height="100%"  align="left" valign="top" >
	                    <div id="gridbox2"></div></td>
                  </tr>
                </table>            		
		 </td>

  </tr>

</table>
</body>
</html>
