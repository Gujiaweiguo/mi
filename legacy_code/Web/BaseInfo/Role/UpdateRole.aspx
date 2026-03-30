<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateRole.aspx.cs" Inherits="BaseInfo_Role_Default2"  %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title> 

    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle">
            <tr height="5">
                <td colspan="8">
                </td>
            </tr>
            <tr>
                <td style="width: 60px; height: 401px; text-align: center" valign="bottom">
                    <img height="401" class="compartLink" />
                </td>
                <td style="width: 310px; height: 401px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 288px" width="280">
                        <tr>
                            <td class="tdTopBackColor" colspan="2" valign="top">
                                <img alt="" class="imageLeftBack" />
                                <asp:Label ID="Label1" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Dept_Title %>"></asp:Label></td>
                            <td class="tdTopRightBackColor" colspan="2" valign="top">
                                &nbsp;<img class="imageRightBack" /></td>
                        </tr>
                        <tr height="1">
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center"
                                valign="top">
                                <span style="font-size: 13pt">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 233px; height: 278px">
                        <tr>
                            <td colspan="3" style="height: 45px">
                                &nbsp; &nbsp;
                                                <hr style="width: 206px; height:2px; color:#b4bec8;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 41px; width: 60px; text-align: right;">
                                &nbsp;<asp:Label ID="lblRoleCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblRoleCode %>"
                            Width="64px" Font-Size="11pt"></asp:Label></td>
                            <td style="height: 41px">
                                &nbsp;</td>
                            <td style="height: 41px; width: 154px;">
                        <asp:TextBox ID="txtRoleCode" runat="server" CssClass="textBoxStyle"></asp:TextBox>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="height: 42px; width: 60px; text-align: right;">
                                &nbsp;<asp:Label ID="lblRoleName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblRoleName %>"
                            Width="64px" Font-Size="11pt"></asp:Label></td>
                            <td style="height: 42px">
                                &nbsp;</td>
                            <td style="height: 42px; width: 154px;">
                        <asp:TextBox ID="txtRoleName" runat="server" CssClass="textBoxStyle"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 60px; height: 42px; text-align: right">
                                <asp:Label ID="labBizGrp" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AddRole_labBizGrp %>"
                                    Width="75px"></asp:Label></td>
                            <td style="height: 42px">
                            </td>
                            <td style="width: 154px; height: 42px">
                                <asp:DropDownList ID="cmbBizGrp" runat="server" CssClass="cmb150px">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td style="height: 41px; width: 60px; text-align: right;">
                                &nbsp;<asp:Label ID="lblRoleStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblRoleStatus %>"
                            Width="64px" Font-Size="11pt"></asp:Label></td>
                            <td style="height: 41px">
                                &nbsp;</td>
                            <td style="height: 41px; width: 154px;">
                        <asp:DropDownList ID="cmbRoleStatus" runat="server" CssClass="cmb150px">
                        </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td style="height: 37px; width: 60px; text-align: right;">
                                &nbsp;<asp:Label ID="lblLeader" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Role_lblLeader %>"
                            Width="64px" Font-Size="11pt"></asp:Label></td>
                            <td style="height: 37px">
                                &nbsp;</td>
                            <td style="height: 37px; width: 154px;">
                        <asp:DropDownList ID="cmbLeader" runat="server" CssClass="cmb150px">
                        </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td colspan="3" rowspan="2">
                                <br />
                               
                                    <hr style="width: 206px;height:2px; color:#b4bec8;" />
                          </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td colspan="3" rowspan="2" style="height: 17px">
                                &nbsp; &nbsp; &nbsp;
                            
                                <asp:ImageButton ID="ImageButton4" runat="server"
                                        ImageUrl="~/images/Add/BtnSave.gif" OnClick="ImageButton4_Click" Height="32px" Width="76px" /></td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                                    &nbsp; &nbsp;</span></td>
                        </tr>
                    </table>
                </td>
                <td style="font-size: 13pt; width: 5px; height: 401px">
                </td>
                <td colspan="3" style="font-size: 13pt; width: 315px; height: 401px">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 288px" width="280">
                        <tr>
                            <td class="tdTopBackColor" colspan="2" valign="top">
                                <img alt="" class="imageLeftBack" />
                                <asp:Label ID="labRoleUpdateTitle" runat="server" Text="<%$ Resources:BaseInfo,AddRole_labRoleUpdateTitle %>"></asp:Label><a style="font-size: 18px"><span
                                    style="font-size: 12pt"></span></a></td>
                            <td class="tdTopRightBackColor" colspan="2"
                                valign="top">
                                &nbsp;<img class="imageRightBack" /></td>
                        </tr>
                        <tr height="1">
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center"
                                valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 280px; height: 280px">
                                        <tr>
                                            <td colspan="4" style="text-align: center; height: 217px;" rowspan="9" valign="top">
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Height="77px"
                                            OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                            Width="254px" PageSize="11">
                                            <Columns>
                                                <asp:BoundField DataField="RoleID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                    <FooterStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="RoleCode" HeaderText="编码" />
                                                <asp:BoundField DataField="RoleName" HeaderText="角色名称" />
                                                <asp:BoundField DataField="RoleStatusDesc" HeaderText="状态" />
                                                <asp:BoundField DataField="LeaderDesc" HeaderText="领导" />
                                                <asp:CommandField ShowSelectButton="True" />
                                            </Columns>
                                                    <HeaderStyle BackColor="PaleGoldenrod" />
                                        </asp:GridView>          
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td style="width: 72px; height: 33px; text-align: right">
                                            </td>
                                            <td colspan="3" style="height: 33px; text-align: center" valign="middle">
                                                <hr style="width: 198px; height:3px;" />
                                                &nbsp;&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 72px; height: 30px; text-align: right">
                                            </td>
                                            <td style="height: 30px; text-align: center" colspan="3">
                                                &nbsp;
                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/back32left.gif" />&nbsp;
                                                <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/next32.gif" /></td>
                                        </tr>
                                    </table>
                                &nbsp; &nbsp; &nbsp;&nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td style="width: 60px; height: 401px; text-align: center" valign="top">
                    <img height="401" class="compartLink" /></td>
            </tr>
        </table>
    
    </form>
</body>
</html>






