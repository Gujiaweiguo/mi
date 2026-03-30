<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShowImage.aspx.cs" Inherits="BaseInfo_User_ShowImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Height="173px"
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Style="z-index: 100;
            left: 0px; position: absolute; top: 0px" Width="868px" CssClass="gridview">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="用户内码" />
                <asp:BoundField DataField="UserName" HeaderText="用户名称" />
                <asp:BoundField DataField="Identity" HeaderText="身份认证" />
                <asp:BoundField DataField="WorkNo" HeaderText="工牌号" />
                <asp:BoundField DataField="Mobile1" HeaderText="移动电话1" />
                <asp:BoundField DataField="Mobile2" HeaderText="移动电话2" />
                <asp:BoundField DataField="EMail" HeaderText="电子邮箱" />
                <asp:BoundField DataField="UserStatus" HeaderText="用户状态" />
                <asp:BoundField DataField="ValidDate" HeaderText="有效期" />
                <asp:BoundField DataField="Note" HeaderText="备注" />
            </Columns>
        </asp:GridView>
    
    </div>
    </form>
</body>
</html>
