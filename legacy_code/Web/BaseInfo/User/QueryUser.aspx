<%@ Page Language="C#" AutoEventWireup="true" Title="查询用户" CodeFile="QueryUser.aspx.cs" Inherits="MI_Net.QueryUser"  MasterPageFile="~/BaseInfo/User/MasterPage.master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td align="left" class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblDept" runat="server" Text="用户查询"></asp:Label></td>
        </tr>
        <tr>
            <td align="left" class="workAreaMainTitleMemo" style="height: 18px" valign="middle">
            </td>
        </tr>
    </table>
    <hr style="width: 100%; size: 1" />
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" style="width: 50%; height: 238px" valign="top">
                <table bgcolor="#fff4ae" border="0" cellpadding="10" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 10px">
                            <div class="boxTitle">
                                基本信息
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 185px">
                            <table border="0" style="background: #ffffff" width="100%">
                            </table>
                            <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                <tr>
                                    <td align="left" style="width: 24px; height: 20px">
                    <asp:Label id="lblUserName" runat="server" Text='<%$ Resources:BaseInfo,User_lblUserName %>' CssClass="label" Width="60px"></asp:Label></td>
                                    <td align="left" style="width: 419px; color: #000000; height: 20px">
                    <asp:TextBox id="txtUserName" runat="server" CssClass="ipt150px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 24px; height: 14px">
                    <asp:Label id="lblWorkNo" runat="server" Text='<%$ Resources:BaseInfo,User_lblWorkNo %>' CssClass="label" Width="61px"></asp:Label></td>
                                    <td align="left" style="width: 419px; color: #000000; height: 14px">
                    <asp:TextBox id="txtWorkNo" runat="server" CssClass="ipt150px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 24px; height: 17px">
                    <asp:Label id="lblValidDate" runat="server" Text='<%$ Resources:BaseInfo,User_lblValidDate %>' CssClass="label" Width="60px"></asp:Label></td>
                                    <td align="left" style="width: 419px; color: #000000; height: 17px">
                    <asp:TextBox id="txtValidDate" runat="server" CssClass="ipt150px"></asp:TextBox></td>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="left" colspan="2" rowspan="2">
                <asp:GridView ID="GridView1" runat="server" Width="964px" AutoGenerateColumns="False" Height="111px" HorizontalAlign="Center" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" CssClass="gridview"  >
                                <Columns>
                    <asp:BoundField DataField="UserID" HeaderText="用户ID" />
                    <asp:BoundField DataField="UserName" HeaderText="用户名" />
                </Columns>
                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                <AlternatingRowStyle BackColor="#F7F7F7" />
                    <PagerSettings Mode="NextPreviousFirstLast" />
                </asp:GridView>
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                </tr>
                            </table>
                    <asp:Button id="btnOk" runat="server" onclick="btnOk_Click" Text='<%$ Resources:BaseInfo,User_btnOk %>' CssClass="btnOn50px" /><asp:Button id="btnCancel" runat="server" Text='<%$ Resources:BaseInfo,User_btnCancel %>' CssClass="btnOn50px" OnClick="btnCancel_Click" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

