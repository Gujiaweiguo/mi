<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chartPie3D.aspx.cs" Inherits="FusionCharts_chartPie3D" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title></title>
<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
<script language="JavaScript" src="JS/FusionCharts.js"></script>
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
<body>
<div id="chartDiv1" align="center">FusionCharts.</div>
	<script type="text/javascript">
		var chart4 = new FusionCharts("SWF/Pie3D.swf", "ChartId", "100%", "100%", "0", "0");
		chart4.setDataURL(parent.xmlFile.innerText);
		chart4.setTransparent(true);
		chart4.render("chartDiv1");
	</script>
</body>
</html>