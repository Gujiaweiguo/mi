<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StorePosStatus.aspx.cs" Inherits="VisualAnalysis_Report_StorePosStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<meta http-equiv="Pragma" content ="no-cache" />
<meta http-equiv="Cache-Control" content="no-cache" />
<meta http-equiv="Expires" content="0" />
<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript" language="JavaScript" src="../../FusionCharts/JS/FusionCharts.js"></script>
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
    function onLoad() {
        addBack();
    }
</script>
<script language="javascript" type="text/javascript">
    var grid_demo_id2 = "myGrid2";
    var dsOption = {
        fields: [
		{ name: 'no', type: 'float' },
		{ name: 'POSid' },
		{ name: 'shopcode' },
		{ name: 'shopname' },
		{ name: 'ip' },
		{ name: 'tpusrid' },
		{ name: 'poslasttime' },
		{ name: 'posstatus' }
	],
        uniqueField: 0,
        recordType: 'object'
    }

    var colsOption = [
     { id: 'no', header: "序号", width: 40, align: "center", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'POSid', header: "POS编号", width: 70, align: "center", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'shopcode', header: "商铺编码", width: 120, align: "center", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'shopname', header: "商铺名称", width: 120, align: "center", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'ip', header: "POS机IP", width: 120, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'tpusrid', header: "收银员编号", width: 70, align: "center", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'poslasttime', header: "更新时间", width: 150, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer },
     { id: 'posstatus', header: "POS状态", width: 70, align: "right", hdRenderer: my_hdRenderer, headAlign: "center", renderer: my_renderer }
];
    var gridOption2 = {
        id: grid_demo_id2,
        width: "760",  //"100%", // 700,
        height: "420",  //"100%", // 330,
        container: 'gridbox2',
        replaceContainer: true,
        dataset: dsOption,
        columns: colsOption,
        pageSizeList: [5, 10, 15, 20],
        pageSize: 20,
        toolbarContent: 'nav goto | pagesize | print | state', 
        skin: 'mac',
        exportURL: '../../GetData.aspx',
        exportFileName: 'ddddd',
        remotePaging: false,
        loadURL: '../../GetData.aspx'
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
<body onload="onLoad()">
<table width="100%" height="100" border="0" cellpadding="0" cellspacing="0" class="spaceBg" style="margin:0px;">
  <tr>       
         <td colspan=2 width="60%" height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> <%=strStoreName %> </td>

  </tr>
  <tr height="100%">
          <td width="100%" align="left" valign="top" colspan="1">
				<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" style="border:#dddddd 1px solid;margin-top:6px;">
                  
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
