<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportShow.aspx.cs" Inherits="ReportM_ReportShow" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692FBEA5521E1304"
     Namespace="CrystalDecisions.Web"  TagPrefix="CRs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CRs:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
            AutoDataBind="true" Height="50px" ReuseParameterValuesOnRefresh="True" 
            Width="350px" />
        
    </div>
    </form>
</body>
</html>
