<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FloorsSales.aspx.cs" Inherits="VisualAnalysis_Report_FloorsSales" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">


<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title></title>
<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script language="JavaScript" src="../../FusionCharts/JS/FusionCharts.js"></script>
<script language="javascript" type="text/javascript">
function Load() {
	var addr = "Disktop.aspx";
	addTabTool("返回," + addr);
	loadTitle();
}
function changeXML( t,d )
{
    document.getElementById('xmlFile').innerText = String(t);
    document.getElementById('Td1').innerText = String(t);
    document.frames(d).document.location.reload();
}
function blur(t) {
    document.getElementById('if1').style.display = 'none';
    document.frames('if1').document.location.reload();
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
<body onload="Load()">
<table width="50" border="0" cellspacing="0" cellpadding="0" style="display:none;">
  <tr>
    <td id="xmlFile">xml/xsPie3d.xml</td>
    <td id="Td1">XML/Combi3D2.xml</td>
  </tr>
</table>

<table width="100%" height="100" border="0" cellpadding="0" cellspacing="0" class="spaceBg">
  <tr>       
         <td colspan="2" width="100%" height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> 新街口购物广场楼层销售分析 </td>

  </tr>
  <tr height="80%" style="display:block;">
          <td width="40%" height="322" align="center" valign="top">
					<div id="chartdiv5" align="center">FusionCharts. </div>
	                <script type="text/javascript">
	                    var chart4 = new FusionCharts("../../FusionCharts/SWF/MSColumn3DLineDY.swf", "ChartId", "100%", "100%", "0", "0");
	                    chart4.setDataURL("../../FusionCharts/XML/Combi3D2SaleF1.xml");
		                chart4.setTransparent(true);
		                chart4.render("chartdiv5");
	                </script>		
		 </td>
		 <td width="40%"  height="322" >
		 <iframe id="chartIframe" src="../../FusionCharts/chartPie3D.aspx" width="100%" height="322" scrolling="no" frameborder="0">
		 </iframe>
		 </td> 
  </tr>
<!--  <tr height="40%">
        <td height="295" align="center" valign="middle" colspan=2>
            <iframe id="Iframe2" src="t1.html" width="100%" height="295" scrolling="no" frameborder="0">
		    </iframe>
        </td>
  </tr>-->
</table>
</body>
</html>
