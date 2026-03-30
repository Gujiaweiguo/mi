<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LocaRentRate.aspx.cs" Inherits="VisualAnalysis_Report_LocaRentRate" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title></title>
<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript" language="JavaScript" src="../../FusionCharts/JS/FusionCharts.js"></script>
    <link href="../../Grid/highlight/voteresult.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
    function Load() {
        var addr = "<%=strPath %>";
        addTabTool("返回," + addr);
        loadTitle();
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
    }
    function updateChart(employeeId){			
		var strURL = "../../FusionCharts/GetChartXmlData.aspx?chartobjtype=MSColumn3DLineDY";
		strURL = escape(strURL);
		
		var chartObj = getChartFromId("chartDiv2");			
		chartObj.setDataURL(strURL);
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
<table width="100%" height="400" border="0" cellpadding="0" cellspacing="0" class="spaceBg">
  <tr>                
            <td colspan=2 height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;font-size:small;">
                       <img src="../../images/Bullet.gif" width="9" height="9" /> <%=deptName%>项目出租率分析  --  <%=strmonth%></td>
  </tr>
  <tr width="100%">
          <td width="100%" height="70%" align="center" valign="top" colspan="2">
            <%=GetChartHtml()%>		
		 </td>
  </tr>
<tr height="330px" valign="middle" align="center"  >
    <td width="100%" bgcolor="#dddddd" align="center" >
		    <DIV class="box-area" id="from_global_host" style="DISPLAY:block;">
                <menu style="display:block;" class="menu-area">
                    <li id="l0" class="LI" onmouseup="liOnclick(0,3)" style="cursor:hand;">主力店</li>
                    <li id="l1" class="LI" onmouseup="liOnclick(1,3)" style="cursor:hand;">次主力店</li>
                    <li id="L2" class="LI" onmouseup="liOnclick(2,3)" style="cursor:hand;">散铺</li>
                </menu>
		        <div class="infor_tb">
			        <div id="chartDiv2" align="center">FusionCharts.</div>
	                <script type="text/javascript">
	                    var chart4 = new FusionCharts("../../FusionCharts/SWF/MSColumn3DLineDY.swf?ChartNoDataText=请选择一个项目查询", "ChartId", "100%", "100%", "0", "0");
	                    chart4.setDataURL("<chart></chart>");
		                chart4.setTransparent(true);
		                chart4.render("chartDiv2");
	                </script>
		        </div>
            </DIV>
	 </td> 
</tr>
</table>
</body>
</html>