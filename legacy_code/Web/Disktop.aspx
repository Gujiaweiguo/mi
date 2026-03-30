<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Disktop.aspx.cs" Inherits="Disktop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=(string)GetGlobalResourceObject("BaseInfo", "Menu_VAtongzhou")%></title>
    <link href="App_Themes/CSS/Style.css" rel="stylesheet" type="text/css" />
   <script language="javascript" type="text/javascript" src="JavaScript/TabTools.js"></script>    
    <script language="javascript" type="text/javascript" src="../FusionCharts/JS/FusionCharts.js"></script>
    <script type="text/javascript">
	    function onLoad()
	    {
	    }	    
	    //xml路径
        function getUserID(ttt)
        {
            var path ="VisualAnalysis/VAMenu/149/";
	        return path;
	    }
        //楼层图
        function getP(ttt)
        {
            var path = "VisualAnalysis/VAGraphic/";
	        return path;
        }
        function getShow(ttt)
        {
	        var path = document.getElementById("hidFloorID").value;
	        return path;
        }
        function sendAreaID(t) {
            var a = String(t);    
            var arya=a.split('^');  
            document.frames["chartIframe"].location.href="KpiCharts/gpkpiFrame.htm";           
            if (arya[0]=="Loca")
            {
                document.getElementById("hidLocaID").value = arya[1];
                document.getElementById("xmlFiles").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/loca" + arya[1] + "Kpi1.xml";
                document.getElementById("xmlFiles2").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/loca" + arya[1] + "Kpi2.xml";
                document.getElementById("xmlFiles3").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/loca" + arya[1] + "Kpi3.xml";
            }
            if(arya[0]=="City")
            {
                document.getElementById("hidCityID").value = arya[1];
                document.getElementById("xmlFiles").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/city" + arya[1] + "Kpi1.xml";
                document.getElementById("xmlFiles2").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/city" + arya[1] + "Kpi2.xml";
                document.getElementById("xmlFiles3").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/city" + arya[1] + "Kpi3.xml";
            }
            if (arya[0]=="Group")
            {
                  document.getElementById("xmlFiles").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/GpKpi1.xml";
                  document.getElementById("xmlFiles2").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/GpKpi2.xml";
                  document.getElementById("xmlFiles3").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/GpKpi3.xml";
            }  
            if (arya[0]=="Mall") {
                document.getElementById("hidMall").value = arya[1];            
                document.getElementById("xmlFiles").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/mall" + arya[1] + "Kpi1.xml";
                document.getElementById("xmlFiles2").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/mall" + arya[1] + "Kpi2.xml";
                document.getElementById("xmlFiles3").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/mall" + arya[1] + "Kpi3.xml";
            }   
            if (arya[0]=="Build") {
                document.getElementById("hidBuild").value = arya[1];           
                document.getElementById("xmlFiles").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/build" + arya[1] + "Kpi1.xml";
                document.getElementById("xmlFiles2").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/build" + arya[1] + "Kpi2.xml";
                document.getElementById("xmlFiles3").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/build" + arya[1] + "Kpi3.xml";
            }
            if (arya[0]=="Floor") {
                document.getElementById("hidFloorID").value = arya[1];  
                document.getElementById("xmlFiles").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/Floor" + arya[1] + "Kpi1.xml";
                document.getElementById("xmlFiles2").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/Floor" + arya[1] + "Kpi2.xml";
                document.getElementById("xmlFiles3").innerText="../VisualAnalysis/VAMenu/"+document.getElementById("Hiduser").value+"/Floor" + arya[1] + "Kpi3.xml";
            }
            if (arya[0]=="Shop")
            {
                document.frames["chartIframe"].location.href="KpiCharts/shopkpi.aspx?ShopID=" + arya[1];               
             }
            window.parent.frames["chartIframe"].location.reload();
        }　        
    </script>
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
</head>
<body onload="onLoad();" style="margin:0;">
    <form id="form1"  runat="server">       
    <table style="margin:0;">
    <tr>
        <td valign="top" >
            <div >
                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" name="movie" height="510" id="movie1" style="width: 860px">
                  <param id="movie" name="movie" value="wincor.swf"  runat="server"/>
                  <param name="quality" value="high" />
                  <param name="wmode" value="opaque" />
                  <embed id="movie2" src="wincor.swf" width="860" height="510" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" name="movie"></embed>
                </object>
            </div>
        </td>
        <%--<td valign="top" width="207.5" >
            <iframe id="chartIframe" src="KpiCharts/gpkpiFrame.htm" width="207.5" height="510" scrolling="no" frameborder="0" valign="top" ></iframe> 
        </td>--%>
    </tr>
    </table>
<asp:HiddenField ID="hidFloorID" runat="server" />
<asp:HiddenField ID="hidLocaID" runat="server" />
<asp:HiddenField ID="hidCityID" runat="server" />
<asp:HiddenField ID="hidMall" runat="server" />
<asp:HiddenField ID="hidBuild" runat="server" />
<asp:HiddenField ID="Hiduser" runat="server" />
<asp:TextBox  ID="ReturnValue" runat ="server" Visible="False" />
<table style="display:none"><tr>
<td id="strValue" runat="server"></td> 
<td id="xmlFiles" runat="server">../FusionCharts/XML/GppKpi1.xml</td>
<td id="xmlFiles2" runat="server">../FusionCharts/XML/GppKpi2.xml</td>
<td id="xmlFiles3" runat="server">../FusionCharts/XML/GppKpi3.xml</td>
</tr></table>
</form>
</body>
</html>
