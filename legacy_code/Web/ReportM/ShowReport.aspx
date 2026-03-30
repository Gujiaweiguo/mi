<script language="javascript" type="text/javascript">
function Showsrc()
{
    //parent.location.href=document.getElementById('urla').value;
    document.getElementById('mid').src = document.getElementById('rpturl').value;
}
</script>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowReport.aspx.cs" Inherits="ReportM_ShowReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body onload="Showsrc()" style="margin: 0 ,0,0,0; ">
    <form id="form1" runat="server">
    <div>
    <iframe  id="mid" frameborder="0" width="100%" height="97%"  scrolling ="auto" style="margin: 0 ,0,0,0; "></iframe>
        <asp:HiddenField ID="rpturl" runat="server" />
        &nbsp;</div>
    </form>
</body>
</html>
