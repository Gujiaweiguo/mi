<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CityRentRate.aspx.cs" Inherits="VisualAnalysis_Report_CityRentRate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title></title>
<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script language="JavaScript" src="../../FusionCharts/JS/FusionCharts.js"></script>
<script language="javascript" type="text/javascript">
    function Load() {
        var addr = "<%=strPath %>";
        addTabTool("返回," + addr);
        loadTitle();
        var a="<%=GetXml()%>";
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
<table>
<tr style="display:none">
<td id="xmlFile" runat="server">../../FusionCharts/XML/StoreCZ.xml</td>
</tr>
</table> 

<table width="100%" height="400" border="0" cellpadding="0" cellspacing="0" class="spaceBg">
  <tr>                
            <td colspan=2 height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;font-size:small;">
                       <img src="../../images/Bullet.gif" width="9" height="9" /> <%=deptName%>项目整体出租率分析  --  2009年11月份</td>
  </tr>
  <tr height="100%">
          <td width="100%" height="100%" align="center" valign="top">
					<div id="chartdiv5" align="center">FusionCharts. </div>
					<script type="text/javascript">
					    var chart5 = new FusionCharts("../../FusionCharts/SWF/MSColumn3DLineDY.swf", "ChartId", "100%", "100%", "0", "0");
					    chart5.setDataURL("../../FusionCharts/XML/CityCZL.xml);
					    chart5.setTransparent(true);
					    chart5.render("chartdiv5");
					</script>		
		 </td>
  </tr>

</table>
</body>
</html>