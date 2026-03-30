<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CancelArea.aspx.cs" Inherits="LeaseArea_Area_CancelArea" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table align="center" style="position: relative">
        <tr>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text="可出租面积管理->取消区域" Width="206px"></asp:Label></td>
            <td colspan="1" style="width: 109px">
            </td>
        </tr>
        <tr>
            <td style="width: 74px">
            </td>
            <td style="width: 136px">
            </td>
            <td style="width: 109px">
            </td>
        </tr>
        <tr>
            <td style="width: 74px">
                <asp:Label ID="lblAreaCode" runat="server" CssClass="label" Text="区域编号"></asp:Label>
            </td>
            <td style="width: 136px">
                <asp:TextBox ID="txtAreaCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
            <td style="width: 109px">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAreaCode"
                    ErrorMessage="请填写编号" Style="position: relative" Width="96px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 74px">
            </td>
            <td style="width: 136px">
            </td>
            <td style="width: 109px">
            </td>
        </tr>
        <tr>
            <td style="width: 74px">
            </td>
            <td style="width: 136px">
                <asp:Button ID="btnOk" runat="server" CssClass="btnOn50px" OnClick="btnOk_Click"
                    Text="确定" />
                &nbsp; &nbsp;
                <asp:Button ID="btnCancel" runat="server" CssClass="btnOn50px" Text="取消" /></td>
            <td style="width: 109px">
            </td>
        </tr>
    </table>
</asp:Content>

