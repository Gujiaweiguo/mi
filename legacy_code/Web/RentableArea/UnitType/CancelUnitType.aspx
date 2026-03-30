<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CancelUnitType.aspx.cs" Inherits="LeaseArea_UnitType_CancelUnitType" %>

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
                    <div>
                        <table align="center" style="position: relative">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text="可出租面积管理->取消单元类型" Width="206px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 93px">
                                </td>
                                <td style="width: 187px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 93px">
                                    <asp:Label ID="lblUnitTypeCode" runat="server" Text="单元类型编号" Width="97px"></asp:Label>
                                </td>
                                <td style="width: 187px">
                                    <asp:TextBox ID="txtUnitTypeCode" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 93px">
                                </td>
                                <td style="width: 187px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 93px">
                                </td>
                                <td style="width: 187px">
                                    <asp:Button ID="btnOk" runat="server" Text="确定" OnClick="btnOk_Click" />
                                    &nbsp; &nbsp;
                                    <asp:Button ID="btnCancel" runat="server" Text="取消" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
