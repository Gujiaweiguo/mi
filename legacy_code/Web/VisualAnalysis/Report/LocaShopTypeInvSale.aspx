<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LocaShopTypeInvSale.aspx.cs" Inherits="VisualAnalysis_Report_LocaShopTypeInvSale" %>

    
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title></title>
<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript" language="JavaScript" src="../../FusionCharts/JS/FusionCharts.js"></script>
<style type="text/css" media="all">@import "../../grid/doc_no_left.css";</style>
<link href="../../grid/highlight/style.css" rel="stylesheet" type="text/css" />
<LINK href="../../grid/highlight/voteresult.css" type="text/css" rel="Stylesheet">
<link rel="stylesheet" type="text/css" href="../../grid/gt_grid.css" />


<script language="javascript" type="text/javascript">
    function Load() {
        var addr = "<%=strPath %>";
        addTabTool("返回," + addr);
        loadTitle();
    }
    function changeXML(t, d) {
        document.getElementById('xmlFile2').innerText = String(t);
        document.frames(d).document.location.reload();
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
        if (n == "1") {
            changeXML('../../FusionCharts/xml/Combi3D2InvSaleCZL.xml', 'iFr1')
        }
        if(n=='2') {
            changeXML('../../FusionCharts/xml/Combi3D2InvSaleSp.xml', 'iFr1')
        }
        if (n == '0') {
            changeXML('../../FusionCharts/xml/Combi3D2InvSaleZL.xml', 'iFr1')
        }
    }
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
<body style="margin-top:0px;" onload="liOnclick(0,3);Load();">
<table width="100%" height="440" border="0" cellpadding="0" cellspacing="0" class="spaceBg" style="margin:0px;">
  <tr>       
         <td width="100%" height="25" align="left" bgcolor="#eeeeee" class="smallTitleTxt" style="border:0px;border-bottom:#cccccc 1px solid;padding-left:10px;">
            <img src="../../images/Bullet.gif" width="9" height="9" /> <%=deptName %>商铺类型租金销售比分析 </td>
  </tr>
  <tr height="80%"> 
		 <td width="100%" valign="top" colspan="1">
		                <table width="100%" height="355" border="0" cellpadding="0" cellspacing="0" style="border:#dddddd 1px solid;margin-top:6px;">
		                    <tr height="25" valign="top">
		                        <td width="100%" colspan="2" height="25" align="left" valign="middle" bgcolor="#F9F9F9" class="smallTitleTxt" style="border:0px;border-bottom:#dddddd 1px solid;padding-left:10px;"><img src="../../images/Bullet.gif" width="9" height="9" /> 各项目商铺类型租金销售比分析  --  11月份
		                        </td>
		                    </tr>
		                    <tr height="400px" valign="top" align="center"  >
		                        <td width="100%" bgcolor="#dddddd" align="center" >
		                            <DIV class="box-area" id="from_global_host" style="DISPLAY:block;">
                                    <menu style="display:block;" class="menu-area">
                                    <li id="l0" class="LI" onmouseup="liOnclick(0,3)" style="cursor:hand;">主力店
                                    </li>
                                    <li id="l1" class="LI" onmouseup="liOnclick(1,3)" style="cursor:hand;">次主力店
                                    </li>
                                     <li id="L2" class="LI" onmouseup="liOnclick(2,3)" style="cursor:hand;">散铺
                                    </li>
                                    </menu>
									<div class="infor_tb">
											 <iframe id="iFr1" src="../../FusionCharts/chartMSCol3D.aspx" width="100%" height="322" scrolling="no" frameborder="0">
		                                    </iframe>
									</div>
                                    </DIV>
	                          </td> 
		                    </tr>
           </table>
		 </td>
  </tr>

</table>
<table style="display:none";><tr><td id="xmlFile2">../../FusionCharts/xml/Combi3D2InvSaleZL.xml</td></tr></table>
</body>
</html>
