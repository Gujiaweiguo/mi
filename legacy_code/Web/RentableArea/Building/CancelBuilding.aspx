<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CancelBuilding.aspx.cs" Inherits="LeaseArea_Building_CancelBuilding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <table align="center">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text="可出租面积管理->取消大楼" Width="206px"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 74px">
                    </td>
                    <td style="width: 136px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 74px">
                        <asp:Label ID="lblBuildingCode" runat="server" Text="大楼编号"></asp:Label>
                    </td>
                    <td style="width: 136px">
                        <asp:TextBox ID="txtBuildingCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 74px">
                    </td>
                    <td style="width: 136px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 74px">
                    </td>
                    <td style="width: 136px">
                        <asp:Button ID="btnOk" runat="server" Text="确定" OnClick="btnOk_Click" />
                        &nbsp; &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="取消" /></td>
                </tr>
            </table>
        </div>
    
    </div>
    </form>
</body>
</html>
