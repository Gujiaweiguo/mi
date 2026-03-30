<!--
/// 
/// 编写人:何思键
/// 编写时间:2009年4月8日
/// 
-->
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptVisualAnalysis.aspx.cs" Inherits="Report_VisualAnalysis" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>    
	    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript">
  function Load()
	    {
	        var addr = document.getElementById("txtHidden").value;
	        addTabTool("<%=baseInfo %>,"+addr);
	        loadTitle();
	    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body style="margin:0px;"  onload="Load()">
    <form id="form1" runat="server"><asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
    <div>
       <asp:HiddenField ID="txtHidden" runat="server" Value="" />
      <iframe style="height: 100%; visibility: inherit; width: 100%;"
                       scrolling="auto" frameborder="0" src="../../ReportM/ReportShow.aspx" ></iframe>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form> 
</body>
</html>
