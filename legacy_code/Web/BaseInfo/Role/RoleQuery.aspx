<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleQuery.aspx.cs" Inherits="BaseInfo_Role_Default" MasterPageFile="~/BaseInfo/User/MasterPage.master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left" rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td align="left" class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblDept" runat="server" Text="角色查询"></asp:Label></td>
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
                                基本信息维护
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 185px" valign="top">
                            <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                <tr>
                                    <td align="left" style="width: 24px; height: 20px">
    <asp:Label ID="lblRoleCode" runat="server" Text='<%$ Resources:BaseInfo,Role_lblRoleCode %>' CssClass="label" Width="53px"></asp:Label></td>
                                    <td align="left" style="width: 419px; color: #000000; height: 20px">
                                        <asp:TextBox ID="txtRoleCode" runat="server" CssClass="ipt150px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 24px; height: 14px">
    <asp:Label ID="lblRoleName" runat="server" Text='<%$ Resources:BaseInfo,Role_lblRoleName %>' CssClass="label" Width="63px"></asp:Label></td>
                                    <td align="left" style="width: 419px; color: #000000; height: 14px">
    <asp:TextBox ID="txtRoleName" runat="server" CssClass="ipt150px"></asp:TextBox></td>
                                </tr>
                            
                            </table>
    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="查询" CssClass="btnOn50px" />
        <asp:GridView ID="grdInfo" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" CssClass="gridview" Width="650px" >
            <Columns>
                <asp:BoundField DataField="RoleId" HeaderText="权限内码" />
                <asp:BoundField DataField="CreateUserID" HeaderText="创建用户代码" ReadOnly="True" />
                <asp:BoundField DataField="CreateTime" HeaderText="创建时间" ReadOnly="True" />
                <asp:BoundField DataField="ModifyUserID" HeaderText="最后修改用户代码" ReadOnly="True" />
                <asp:BoundField DataField="ModifyTime" HeaderText="最后修改时间" ReadOnly="True" />
                <asp:BoundField DataField="OprRoleID" HeaderText="操作用户的角色代码" />
                <asp:BoundField DataField="OprDeptID" HeaderText="操作用和的机构代码" />
                <asp:BoundField DataField="RoleCode" HeaderText="角色编码" />
                <asp:BoundField DataField="RoleName" HeaderText="角色名称" />
                <asp:BoundField DataField="rolestatus" HeaderText="角色状态" />
                <asp:BoundField DataField="IsLeader" HeaderText="是否领导" />
            </Columns>
            <FooterStyle BackColor="Tan" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
        </asp:GridView>
    </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    &nbsp;&nbsp;<br />
    &nbsp;<br />
    <br />
    <br />
    
</asp:Content>