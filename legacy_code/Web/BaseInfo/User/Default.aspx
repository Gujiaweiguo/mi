<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="MI_Net._Default" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <br />
    <br />
    <br />
    <br />
    &nbsp;<br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Height="173px"
        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" CssClass="gridview" >
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="用户内码" />
            <asp:BoundField DataField="UserName" HeaderText="用户名称" />
            <asp:BoundField DataField="IdentityNo" HeaderText="身份认证" />
            <asp:BoundField DataField="WorkNo" HeaderText="工牌号" />
            <asp:BoundField DataField="Mobile1" HeaderText="移动电话1" />
            <asp:BoundField DataField="Mobile2" HeaderText="移动电话2" />
            <asp:BoundField DataField="EMail" HeaderText="电子邮箱" />
            <asp:BoundField DataField="UserStatus" HeaderText="用户状态" />
            <asp:BoundField DataField="ValidDate" HeaderText="有效期" />
            <asp:BoundField DataField="Note" HeaderText="备注" />
            <asp:CommandField NewText="修改" SelectText="修改" ShowSelectButton="True" />
        </Columns>
    </asp:GridView>
    <br />
    <br />
</asp:Content>



