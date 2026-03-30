<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddFloor.aspx.cs" Inherits="LeaseArea_AddFloor" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblFloors" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblFloors %>"></asp:Label></td>
        </tr>
        <tr>
            <td class="workAreaMainTitleMemo" style="height: 18px" valign="middle">
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 50%; height: 238px" valign="middle">
                <table bgcolor="#fff4ae" border="0" cellpadding="10" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 10px">
                            <div class="boxTitle">
                                楼层定义</div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="height: 185px" valign="middle">
                            <table align="center" style="width: 353px; position: relative; height: 63px">
                                <tr>
                                    <td style="width: 290px; height: 21px">
                                        <asp:Label ID="lblFloorCode" runat="server" CssClass="label" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorCode %>"></asp:Label></td>
                                    <td align="left" style="width: 492px; height: 21px">
                                        <asp:TextBox ID="txtFloorCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px">
                                        <asp:Label ID="lblFloorName" runat="server" CssClass="label" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label></td>
                                    <td align="left" style="width: 492px">
                                        <asp:TextBox ID="txtFloorName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px">
                                        <asp:Label ID="lblFloorStatus" runat="server" CssClass="label" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorStatus %>"></asp:Label></td>
                                    <td align="left" style="width: 492px">
                                        <asp:DropDownList ID="cmbFloorStatus" runat="server" CssClass="cmb160px" Width="153px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px; height: 22px">
                                        <asp:Label ID="lblNote" runat="server" CssClass="label" Text="<%$ Resources:BaseInfo,User_lblNote %>"></asp:Label></td>
                                    <td align="left" style="width: 492px; height: 22px">
                                        <asp:TextBox ID="txtNote" runat="server" CssClass="mipt" TextMode="MultiLine"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px">
                                    </td>
                                    <td style="width: 492px">
                                        <asp:Button
                                            ID="btnOk" runat="server" CssClass="btnOn50px" OnClick="btnOk_Click" Text="确定" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btnOn50px" Text="取消" OnClick="btnCancel_Click" />
                                        &nbsp; &nbsp; &nbsp;
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
