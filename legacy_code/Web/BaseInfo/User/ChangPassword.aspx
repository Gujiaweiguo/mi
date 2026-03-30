<%@ Page Language="C#" MasterPageFile="~/BaseInfo/User/MasterPage.master" AutoEventWireup="true" CodeFile="ChangPassword.aspx.cs" Inherits="BaseInfo_User_ChangPassword" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td align="left" class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblDept" runat="server" Text="修改密码"></asp:Label></td>
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
                                基本信息</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 185px">
                            <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                <tr>
                                    <td align="left" style="width: 24px; height: 20px">
                <asp:Label ID="lblPassword" runat="server" Text="密码:" Width="38px"></asp:Label></td>
                                    <td align="left" style="width: 419px; color: #000000; height: 20px">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="ipt150px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="密码不能为空"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 24px; height: 14px">
                <asp:Label ID="lblRePassword" runat="server" Text="重复密码:" CssClass="label" Width="55px"></asp:Label></td>
                                    <td align="left" style="width: 419px; color: #000000; height: 14px">
                <asp:TextBox ID="txtRePassword" runat="server" TextMode="Password" CssClass="ipt150px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRePassword"
                    ErrorMessage="密码重复不能为空"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="确定" CssClass="btnOn50px" /><asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword"
                    ControlToValidate="txtRePassword" ErrorMessage="两次输入的密码不一致"></asp:CompareValidator></td>
                                </tr>
                                
                            </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

