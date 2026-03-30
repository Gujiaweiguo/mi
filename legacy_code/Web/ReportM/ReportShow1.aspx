<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportShow1.aspx.cs" Inherits="ReportM_Customer_Default" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=11.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
        <%--<object id="CrystalPrintControl" classid="CLSID:BAEE131D-290A-4541-A50A-8936F159563A"
            codebase="http://10.1.141.30/PrintControl.cab#Version=10,2,0,1016"
            viewastext>
        </object>--%>
    </div>
    </form>
</body>
</html>