<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CancelUnit.aspx.cs" Inherits="LeaseArea_Unit_CancelUnit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <div>
                    <table align="center" style="position: relative">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text="可出租面积管理->取消单元" Width="206px"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 74px">
                            </td>
                            <td style="width: 136px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 74px">
                                <asp:Label ID="lblFloorCode" runat="server" Text="单元编号"></asp:Label>
                            </td>
                            <td style="width: 136px">
                                <asp:TextBox ID="txtUnitCode" runat="server"></asp:TextBox></td>
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
        </div>
    
    </div>
    </form>
</body>
</html>
