<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportCondition.aspx.cs" Inherits="ReportM_ReportCondition" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 329px; height: 313px">
            <tr>
                <td>
                    <asp:Label ID="lblCustomcode" runat="server" Text="客户编码"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtcustomcode" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblcustomname" runat="server" Text="客户名称"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txtcustomname" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    报表</td>
                <td>
                    <asp:TextBox ID="txtRptName" runat="server">/Mi/Base/Customer.rpt</asp:TextBox></td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查询" />
                    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
