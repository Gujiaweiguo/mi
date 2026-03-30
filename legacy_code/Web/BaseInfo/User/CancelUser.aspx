<%@ Page Language="C#" AutoEventWireup="true"  Title="作废用户" CodeFile="CancelUser.aspx.cs" Inherits="BaseInfo_User_CancelUser"  MasterPageFile="~/BaseInfo/User/MasterPage.master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td align="left" class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblDept" runat="server" Text="作废用户"></asp:Label></td>
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
                <asp:Label ID="lblUserCode" runat="server" Text='<%$ Resources:BaseInfo,User_lblUserCode %>' CssClass="label" Width="56px"></asp:Label></td>
                                    <td align="left" style="width: 419px; color: #000000; height: 20px">
            <asp:TextBox ID="txtUserCode" runat="server" CssClass="ipt150px" ></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserCode"
                    ErrorMessage="*"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2" rowspan="2">
                                        <asp:GridView id="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" CssClass="gridview">
<FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C"></FooterStyle>
<Columns>
<asp:BoundField DataField="UserCode" HeaderText="用户编码"></asp:BoundField>
<asp:BoundField DataField="UserName" HeaderText="用户名"></asp:BoundField>
<asp:BoundField DataField="WorkNo" HeaderText="工作编号"></asp:BoundField>
<asp:BoundField DataField="ModifyTime" HeaderText="最后修改日期"></asp:BoundField>
<asp:BoundField DataField="Mobile1" HeaderText="联系电话1"></asp:BoundField>
<asp:BoundField DataField="Mobile1" HeaderText="联系电话2"></asp:BoundField>
<asp:BoundField DataField="OfficeTel" HeaderText="办公电话"></asp:BoundField>
<asp:BoundField DataField="EMail" HeaderText="电子邮箱"></asp:BoundField>
<asp:BoundField DataField="ValidDate" HeaderText="有效期"></asp:BoundField>
<asp:CommandField SelectText="取消用户" ShowSelectButton="True" HeaderText="取消用户"></asp:CommandField>
</Columns>

<RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C"></RowStyle>

<SelectedRowStyle BackColor="#738A9C" ForeColor="#F7F7F7" Font-Bold="True"></SelectedRowStyle>

<PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right"></PagerStyle>

<HeaderStyle BackColor="#4A3C8C" ForeColor="#F7F7F7" Font-Bold="True"></HeaderStyle>

<AlternatingRowStyle BackColor="#F7F7F7"></AlternatingRowStyle>
</asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr style="color: #000000">
                                    <td align="left" colspan="2" rowspan="2">
                                        &nbsp;<asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查询" CssClass="btnOn50px" /></td>
                                </tr>
                                <tr style="color: #000000">
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>


