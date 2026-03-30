<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddBuilding1.aspx.cs" Inherits="LeaseArea_AddBuilding"  %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td align="left" class="workAreaMainTitle" valign="middle">
                <asp:Label ID="lblBuilding" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblBuilding %>"></asp:Label></td>
        </tr>
        <tr>
            <td align="left" class="workAreaMainTitleMemo" style="height: 18px" valign="middle">
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" style="width: 50%; height: 238px" valign="top">
                <table bgcolor="#fff4ae" border="0" cellpadding="10" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 13px">
                            <div class="boxTitle">
                                大楼定义</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 185px">
                            <table align="center" style="width: 353px; height: 63px">
                                <tr>
                                    <td style="width: 290px; height: 21px">
                                        <asp:Label ID="lblBuildingCode" runat="server" Text="大楼编号" CssClass="label"></asp:Label></td>
                                    <td style="width: 492px; height: 21px">
                                        <asp:TextBox ID="txtBuildingCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px">
                                        <asp:Label ID="lblBuildingName" runat="server" Text="大楼名称" CssClass="label"></asp:Label></td>
                                    <td style="width: 492px">
                                        <asp:TextBox ID="txtBuildingName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px; height: 21px">
                                        <asp:Label ID="lblBuildingAddr" runat="server" Text="地址" CssClass="label"></asp:Label></td>
                                    <td style="width: 492px; height: 21px">
                                        <asp:TextBox ID="txtBuildingAddr" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px">
                                        <asp:Label ID="lblPostCode" runat="server" Text="邮编" CssClass="label"></asp:Label></td>
                                    <td style="width: 492px">
                                        <asp:TextBox ID="txtPostCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px">
                                        <asp:Label ID="lblBuildingStatus" runat="server" Text="大楼状态" CssClass="label"></asp:Label></td>
                                    <td style="width: 492px">
                                        <asp:DropDownList ID="cmbBuildingStatus" runat="server" Width="153px" CssClass="cmb160px">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px; height: 22px">
                                        <asp:Label ID="lblNote" runat="server" Text="备注" CssClass="label"></asp:Label></td>
                                    <td style="width: 492px; height: 22px">
                                        <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" CssClass="mipt"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 290px">
                                    </td>
                                    <td style="width: 492px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 290px">
                                    </td>
                                    <td style="width: 492px">
                                        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="确定" CssClass="btnOn50px" />
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="btnOn50px" OnClick="btnCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
