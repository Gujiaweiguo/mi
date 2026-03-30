<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreSalesTrends.aspx.cs" Inherits="VisualAnalysis_Report_StoreSalesTrends" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta http-equiv="Pragma" content ="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
    <script language="JavaScript" src="../../FusionCharts/JS/FusionCharts.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
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
<body onload="onLoad();">
<table>
<tr style="display:none">
<td id="xmlFile" runat="server"></td>
<td id="xmlFile2" runat="server"></td>
<td id="xmlFile3" runat="server"></td>
</tr>
</table> 

<table width="100%" height="300" border="0" cellpadding="0" cellspacing="0" class="spaceBg">
  <tr>                
            <td colspan=2 height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;font-size:small;">
                       <img src="../../images/Bullet.gif" width="9" height="9" /> <%=deptName%></td>
  </tr>
  <tr height="60%">
          <td width="100%" height="100%" align="center" valign="top" colspan="2">
					<div id="chartdiv5" align="center">FusionCharts. </div>
					<script type="text/javascript">
					    var chart5 = new FusionCharts("../../FusionCharts/SWF/MSColumn3DLineDY.swf", "ChartId", "100%", "100%", "0", "0");
					    chart5.setDataURL(xmlFile.innerHTML);
					    chart5.setTransparent(true);
					    chart5.render("chartdiv5");
					</script>		
		 </td>
  </tr>
  <tr height="40%">
          <td width="50%" height="100%" align="center" valign="top">
					<div id="Div1" align="center">FusionCharts. </div>
					<script type="text/javascript">
					    var chart5 = new FusionCharts("../../FusionCharts/SWF/FCF_Line.swf", "ChartId", "500", "250", "0", "0");
					    chart5.setDataURL(xmlFile2.innerHTML);
					    chart5.setTransparent(true);
					    chart5.render("Div1");
					</script>		
		 </td>
          <td width="50%" height="100%" align="center" valign="top">
					<div id="Div2" align="center">FusionCharts. </div>
					<script type="text/javascript">
					    var chart5 = new FusionCharts("../../FusionCharts/SWF/FCF_Line.swf", "ChartId", "500", "250", "0", "0");
					    chart5.setDataURL(xmlFile3.innerHTML);
					    chart5.setTransparent(true);
					    chart5.render("Div2");
					</script>		
		 </td>
  </tr>
</table>
</body>
</html>