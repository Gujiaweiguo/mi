<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Lease_PotCustomer_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<table border="1" cellpadding="0" cellspacing="0" style="width: 544px; height: 298px">
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label1" runat="server" Text="商户号"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
                        ErrorMessage="*"></asp:RequiredFieldValidator></td>
                <td style="width: 79px; color: #000000">
                    &nbsp;<asp:Label ID="Label12" runat="server" Text="办公地址"></asp:Label></td>
                <td style="width: 166px">
                    &nbsp;<asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label2" runat="server" Text="商户名称"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
                <td style="width: 79px">
                    &nbsp;<asp:Label ID="Label13" runat="server" Text="邮寄地址"></asp:Label></td>
                <td style="width: 166px">
                    &nbsp;<asp:TextBox ID="TextBox13" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label3" runat="server" Text="商户简称"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
                <td style="width: 79px">
                    &nbsp;<asp:Label ID="Label14" runat="server" Text="邮政编码"></asp:Label></td>
                <td style="width: 166px">
                    &nbsp;<asp:TextBox ID="TextBox14" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 88px; height: 26px">
                    &nbsp;<asp:Label ID="Label4" runat="server" Text="商户类型"></asp:Label></td>
                <td style="width: 163px; height: 26px">
                    &nbsp;<asp:TextBox ID="TextBox4" runat="server"></asp:TextBox></td>
                <td style="height: 26px">
                    &nbsp;</td>
                <td style="width: 79px; height: 26px">
                    &nbsp;<asp:Label ID="Label15" runat="server" Text="企业主页"></asp:Label></td>
                <td style="width: 166px; height: 26px">
                    &nbsp;<asp:TextBox ID="TextBox15" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label5" runat="server" Text="法人姓名"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
                <td style="width: 79px">
                    &nbsp;<asp:Label ID="Label16" runat="server" Text="联系人姓名" Width="86px"></asp:Label></td>
                <td style="width: 166px">
                    &nbsp;<asp:TextBox ID="TextBox16" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label6" runat="server" Text="法人职务"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox6" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
                <td style="width: 79px">
                    &nbsp;<asp:Label ID="Label17" runat="server" Text="联系人职务" Width="83px"></asp:Label></td>
                <td style="width: 166px">
                    &nbsp;<asp:TextBox ID="TextBox17" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label7" runat="server" Text="注册资金"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox7" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
                <td style="width: 79px">
                    &nbsp;<asp:Label ID="Label18" runat="server" Text="办公电话"></asp:Label></td>
                <td style="width: 166px">
                    &nbsp;<asp:TextBox ID="TextBox18" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label8" runat="server" Text="注册地"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox8" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
                <td style="width: 79px">
                    &nbsp;<asp:Label ID="Label19" runat="server" Text="移动电话"></asp:Label></td>
                <td style="width: 166px">
                    &nbsp;<asp:TextBox ID="TextBox19" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 88px; height: 21px">
                    &nbsp;<asp:Label ID="Label10" runat="server" Text="工商注册号" Width="80px"></asp:Label></td>
                <td style="width: 163px; height: 21px">
                    &nbsp;<asp:TextBox ID="TextBox9" runat="server"></asp:TextBox></td>
                <td style="height: 21px">
                    &nbsp;</td>
                <td style="width: 79px; height: 21px">
                    &nbsp;<asp:Label ID="Label20" runat="server" Text="电子邮件"></asp:Label></td>
                <td style="width: 166px; height: 21px">
                    &nbsp;<asp:TextBox ID="TextBox20" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label9" runat="server" Text="税号"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox10" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
                <td colspan="2" rowspan="3" style="text-align: right">
                    &nbsp;
                    <hr style="color: skyblue" />
                    <asp:Button ID="Button1" runat="server" Text="保存" /></td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label11" runat="server" Text="开户银行"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox11" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 88px">
                    &nbsp;<asp:Label ID="Label23" runat="server" Text="银行帐号"></asp:Label></td>
                <td style="width: 163px">
                    &nbsp;<asp:TextBox ID="TextBox23" runat="server"></asp:TextBox></td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
