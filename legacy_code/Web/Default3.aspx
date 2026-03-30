<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="Default3" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title></title>
<script language="javascript" type="text/javascript" src="JavaScript/TabTools.js"></script>
<script type="text/javascript" language="JavaScript" src="FusionCharts/JS/FusionCharts.js"></script>
<style type="text/css" media="all">@import "grid/doc_no_left.css";</style>
<link href="grid/highlight/style.css" rel="stylesheet" type="text/css" />
<LINK href="grid/highlight/voteresult.css" type="text/css" rel="Stylesheet">
<link rel="stylesheet" type="text/css" href="grid/gt_grid.css" />
<link rel="stylesheet" type="text/css" href="grid/skin/mac/skinstyle.css" />
<link href="grid/highlight/style.css" rel="stylesheet" type="text/css" />
<script src="grid/highlight/jssc3.js" type="text/javascript"></script>
<script type="text/javascript" src="grid/gt_msg_en.js"></script>
<script type="text/javascript" src="grid/gt_grid_all.js"></script>
<script src="Grid/gridFunc.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">

    var grid_demo_id2 = "myGrid2";
    var dsOption = {

        fields: [
		{ name: 'deptcode' },
		{ name: 'deptname' }
	],
        uniqueField: 0,
        recordType: 'object'
    }

    var colsOption = [
     { id: 'deptcode', header: "月份", width: 120, align: "center", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'deptname', header: "与预算差异", width: 70, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer }
];
    var gridOption2 = {
        id: grid_demo_id2,
        width: "100%",  //"100%", // 700,
        height: "420",  //"100%", // 330,
        container: 'gridbox2',
        replaceContainer: true,
        dataset: dsOption,
        columns: colsOption,
        pageSizeList: [5, 10, 15, 20],
        pageSize: 20,
        toolbarContent: 'nav goto | pagesize | print | state', 
        skin: 'mac',
        exportURL: 'GetData.aspx',
        exportFileName: 'ddddd',
        remotePaging: false,
        loadURL: 'GetData.aspx'
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
<body style="margin-top:0px;" >
<table width="100%" height="100" border="0" cellpadding="0" cellspacing="0" class="spaceBg" style="margin:0px;">
  <tr>       
         <td colspan=2 width="60%" height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;"><img src="images/Bullet.gif" width="9" height="9" />预算收入汇总分析 </td>

  </tr>
  <tr height="100%">
          <td width="100%" align="left" valign="top" colspan="1">
				<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" style="border:#dddddd 1px solid;margin-top:6px;">
                  <tr>
                    <td width="100%" height="25" align="left" valign="middle" bgcolor="#F9F9F9" class="smallTitleTxt" style="border:0px;border-bottom:#dddddd 1px solid;padding-left:10px;">
                        <img src="images/Bullet.gif" width="9" height="9" /> 预算收入汇总分析
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
