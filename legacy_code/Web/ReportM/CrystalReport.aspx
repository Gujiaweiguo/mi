<script language="javascript" type="text/javascript">
function ShowWaiting()
{
document.getElementById('doing').style.visibility = 'visible';
}

function CloseWaiting()
{
document.getElementById('doing').style.visibility = 'hidden';
}

function MyOnload()
{
document.getElementById('doing').style.visibility = 'hidden';
}

if (window.onload == null)
{
window.onload = MyOnload;
}

</script>

<%@ Page Language="C#" AutoEventWireup="false" CodeFile="CrystalReport.aspx.cs" Inherits="CrystalReport" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=11.5.3700.0, Culture=neutral, PublicKeyToken=692FBEA5521E1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</head>
    <div id='doing' style='Z-INDEX: 12000; LEFT: 0px; WIDTH: 100%; CURSOR: wait; POSITION: absolute; TOP: 0px; HEIGHT: 100%'>
    <table width='100%' id="Table1">
    <tr align='center' valign='middle'>
    <td >
    <table  id="Table2" class="loading">
    <tr align='center' valign='middle'>
    <td><img alt="absmiddle" src="images/1.gif" width="150" height="80" /><font color = "FF33AA"> <b >&nbsp;&nbsp;&nbsp;&nbsp;ÇëµÈ´ý...   </b></font></td>
    </tr>
    </table>
    </td>
    </tr>
    </table>
    </div>


<body onload="ShowWaiting()" onunload="CloseWaiting()" style="margin: 0 ,0,0,0; ">


    
    <form id="form1" method="post" runat="server">
    <div>
        <CR:CrystalReportViewer ID="crView" runat="server" AutoDataBind="true" PrintMode="ActiveX" />
        &nbsp;</div>
    </form>
</body>
</html>
