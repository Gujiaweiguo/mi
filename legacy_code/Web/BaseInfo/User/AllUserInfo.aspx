<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllUserInfo.aspx.cs"  MasterPageFile="~/BaseInfo/User/MasterPage.master"Inherits="BaseInfo_User_AllUserInfo" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" CssClass="gridview" >
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="用户ID" />
            <asp:BoundField DataField="UserName" HeaderText="用户名" />
            <asp:BoundField DataField="WorkNo" HeaderText="工作编号" />
            <asp:BoundField DataField="ModifyTime" HeaderText="最后修改日期" />
            <asp:BoundField DataField="Mobile1" HeaderText="联系电话1" />
            <asp:BoundField DataField="Mobile1" HeaderText="联系电话2" />
            <asp:BoundField DataField="OfficeTel" HeaderText="办公电话" />
            <asp:BoundField DataField="EMail" HeaderText="电子邮箱" />
            <asp:BoundField DataField="ValidDate" HeaderText="有效期" />
            <asp:CommandField HeaderText="修改" SelectText="修改" ShowSelectButton="True" />
        </Columns>
        <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
        <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
        <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
        <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
        <AlternatingRowStyle BackColor="#F7F7F7" />
    </asp:GridView>
</asp:Content>


