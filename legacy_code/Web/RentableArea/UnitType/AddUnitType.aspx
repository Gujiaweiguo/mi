<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddUnitType.aspx.cs" Inherits="LeaseArea_AddUnitType" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td rowspan="2" style="width: 40px" valign="middle">
                            <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
                        <td class="workAreaMainTitle" style="height: 20px" valign="middle">
                            <asp:Label ID="lblUnitType" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitType %>"></asp:Label></td>
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
                                            单元类别</div>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td style="height: 185px" valign="middle">
                                        <table align="center" style="width: 353px; height: 63px">
                                            <tr>
                                                <td style="width: 361px; height: 21px">
                                                    <asp:Label ID="lblUnitTypeCode" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitTypeCode %>" Width="102px" CssClass="label"></asp:Label></td>
                                                <td style="width: 492px; height: 21px" align="left">
                                                    <asp:TextBox ID="txtUnitTypeCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 361px">
                                                    <asp:Label ID="lblUnitTypeName" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitTypeName %>" CssClass="label"></asp:Label></td>
                                                <td style="width: 492px" align="left">
                                                    <asp:TextBox ID="txtUnitTypeName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 361px">
                                                    <asp:Label ID="lblUnitTypeStatus" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitTypeStatus %>" CssClass="label"></asp:Label></td>
                                                <td style="width: 492px" align="left">
                                                    <asp:DropDownList ID="cmbUnitTypeStatus" runat="server" Width="153px" CssClass="cmb160px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 361px; height: 22px">
                                                    <asp:Label ID="lblNote" runat="server" Text="<%$ Resources:BaseInfo,User_lblNote %>" CssClass="label"></asp:Label></td>
                                                <td style="width: 492px; height: 22px" align="left">
                                                    <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" CssClass="mipt"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 361px">
                                                </td>
                                                <td style="width: 492px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 361px">
                                                </td>
                                                <td style="width: 492px">
                                                    <asp:Button ID="btnOk" runat="server" CssClass="btnOn50px" OnClick="btnOk_Click"
                                                        Text="确定" />
                                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="btnOn50px" Text="取消" OnClick="btnCancel_Click" /></td>
                                            </tr>
                                        </table>
                                        &nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
</asp:Content>

