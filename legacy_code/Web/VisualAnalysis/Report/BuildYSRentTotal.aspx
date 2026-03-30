<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuildYSRentTotal.aspx.cs" Inherits="VisualAnalysis_Report_BuildYSRentTotal" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title></title>
<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<link rel="stylesheet" type="text/css" href="../../grid/gt_grid.css" />
<link rel="stylesheet" type="text/css" href="../../grid/skin/vista/skinstyle.css" />
<link rel="stylesheet" type="text/css" href="../../grid/skin/china/skinstyle.css" />
<link rel="stylesheet" type="text/css" href="../../grid/skin/mac/skinstyle.css" />
<script type="text/javascript" src="../../grid/gt_grid_all.js"></script>
<script type="text/javascript" src="../../grid/gt_msg_en.js"></script>
<script type="text/javascript" src="../../grid/gridFunc.js"></script>

<script language="javascript" type="text/javascript">
    function Load() {
        var addr = "<%=strPath %>";
        addTabTool("返回," + addr);
        loadTitle();
    }    
</script>





<style type="text/css">

body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	padding:0px;
}

</style> 


</head>
<body style="margin-top:0px;" onload="Load();GetGrid(); ">
<table width="100%" height="100" border="0" cellpadding="0" cellspacing="0" class="spaceBg" style="margin:0px;">
  <tr>       
         <td colspan=2 width="60%" height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> <%=deptName %>预算收入汇总分析 </td>

  </tr>
  <tr height="100%">
          <td width="100%" align="left" valign="top" colspan="1">
				<table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0" style="border:#dddddd 1px solid;margin-top:6px;">
                  <tr>
                    <td width="100%" height="25" align="left" valign="middle" bgcolor="#F9F9F9" class="smallTitleTxt" style="border:0px;border-bottom:#dddddd 1px solid;padding-left:10px;">
                        <img src="../../images/Bullet.gif" width="9" height="9" /> 预算收入汇总分析
                    </td>                                
                  </tr>
                  <tr>
                    <td width="100%" height="100%"  align="left" valign="top" >
	                    <div id="gridbox2">
	                    
	                    </div></td>
                  </tr>
                </table>            		
		 </td>

  </tr>

</table>
<%=strJs%>
</body>
</html>
