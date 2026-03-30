<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreRentRate.aspx.cs" Inherits="VisualAnalysis_Report_StoreRentRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <meta http-equiv="Pragma" content ="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
<title></title>
<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script language="JavaScript" src="../../FusionCharts/JS/FusionCharts.js"></script>
<style type="text/css" media="all">@import "../../grid/doc_no_left.css";</style>
<link href="../../grid/highlight/style.css" rel="stylesheet" type="text/css" />
<LINK href="../../grid/highlight/voteresult.css" type="text/css" rel="Stylesheet">
<link rel="stylesheet" type="text/css" href="../../grid/gt_grid.css" />
<link rel="stylesheet" type="text/css" href="../../grid/skin/mac/skinstyle.css" />
<link href="../../grid/highlight/style.css" rel="stylesheet" type="text/css" />
<script src="../../grid/highlight/jssc3.js" type="text/javascript"></script>
<script type="text/javascript" src="../../grid/gt_msg_en.js"></script>
<script type="text/javascript" src="../../grid/gt_grid_all.js"></script>
<script src="../../Grid/gridFunc.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    var grid_demo_id2 = "myGrid2";
    var dsOption = {

        fields: [
		{ name: 'unitcode' },
		{ name: 'shopname' },
		{ name: 'floorarea', type: 'float' },
		{ name: 'usearea', type: 'float' },
		{ name: 'rentarea', type: 'float' }
	],
        uniqueField: 0,
        recordType: 'object'
    }

    var colsOption = [
     { id: 'unitcode', header: "单元编码", width: 70, align: "left", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'shopname', header: "商铺名称", width: 70, align: "left", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'floorarea', header: "建筑面积", width: 70, align: "left", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'usearea', header: "可租面积", width: 70, align: "left", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'rentarea', header: "签约面积", width: 70, align: "left", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer }
];
    var gridOption2 = {
        id: grid_demo_id2,
        width: "400",  //"100%", // 700,
        height: "400",  //"100%", // 330,
        container: 'gridbox2',
        replaceContainer: true,
        dataset: dsOption,
        columns: colsOption,
        pageSizeList: [5, 10, 15, 20],
        pageSize: 20,
        toolbarContent: 'nav goto | pagesize ',
        skin: 'mac',
        exportURL: 'GetData.aspx',
        exportFileName: 'ddddd',
        remotePaging: false,
        loadURL: '../../GetData.aspx'
    };
    var mygrid2 = new Sigma.Grid(gridOption2);
    Sigma.Util.onLoad(Sigma.Grid.render(mygrid2));
</script>
<script language="javascript" type="text/javascript">
    function onLoad() {
        addBack();
    }
</script>
<style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
-->
</style>
</head>
<body onload="onLoad()">
<table  style="display:none"><tr><td id="xmlFile" runat="server"></td></tr></table> 
<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" class="spaceBg">
<tr><td colspan="2" height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;font-size:small;">
<img src="../../images/Bullet.gif" width="9" height="9" /> <%=deptName%></td></tr>
<tr height="100%">
<td width="400" height="400" align="left" valign="top">
<div id="titl" style="height:40; background-color:#F9F9F9; border:0px;padding-left:10px;font-size:small;" >
 <img src="../../images/Bullet.gif" width="9" height="9" /> 本月单元出租情况列表</div>
<div id="gridbox2" align="left"></div>
</td>
<td width="50%" height="100%" align="left" valign="top">
<div id="Div1" style="height:60; vertical-align:middle; background-color:#F9F9F9; border:0px;padding-left:10px;font-size:small;" >
 <img src="../../images/Bullet.gif" width="9" height="9" /> 本月单元出租情况列表</div>
<div id="chartdiv5" align="left">FusionCharts. </div>
<script type="text/javascript">
    var chart5 = new FusionCharts("../../FusionCharts/SWF/FCF_StackedColumn3D.swf", "ChartId", "600", "350", "0", "0");
    chart5.setDataURL(xmlFile.innerHTML);
    chart5.render("chartdiv5");
</script>		
</td>
</tr></table></body></html>