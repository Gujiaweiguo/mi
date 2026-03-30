<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Nego.aspx.cs" Inherits="Nego" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>租赁商户立项</title>
         <link href="Css/css.css" type="text/css" rel="stylesheet"/>
    <link href="App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
     <script language="javascript" src="js/calendar.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
     <table style="HEIGHT: 480px" cellspacing="0" cellpadding="0" width="100%" border="0" >
  <tr>
  <td style="width:5%; height: 480px; text-align: center" valign="middle">
                    <img height="401" src="images/shuxian.jpg" /></td>
    <td valign="top" style="width: 90%;border-right: #909090 1px solid; border-left: #909090 1px solid; border-top-color:#909090 1px solid; height: 480px;">
        <table width="100%" border="0" cellpadding="2" cellspacing="1" style=" border-bottom:#909090 1px solid;" class="tdBackColor">
            <tr>
                <td style="width: 148px" align="right" >
                    谈判时间：</td>
                <td>
                    <input id="Button3" type="button" value="" onclick="setday(this)" runat="server" style="width: 153px"/></td>
            </tr>
            <tr>
                <td style="width: 148px" align="right">
                    谈判人员：</td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right" style="width: 148px">
                    谈判目标：</td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right" style="width: 148px">
                    谈判记录：</td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 148px">
                </td>
                <td>
                    <asp:ListBox ID="ListBox1" runat="server" Height="86px" Width="443px"></asp:ListBox></td>
            </tr>
            <tr>
                <td align="right" style="width: 148px">
                    谈判列表：</td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 148px; height: 151px;">
                </td>
                <td style="height: 151px; vertical-align: top;">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="446px" BorderStyle="None" BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px" CellPadding="3" EmptyDataText="Data Is Empty">
                        <Columns>
                            <asp:TemplateField HeaderText="谈判日期"></asp:TemplateField>
                            <asp:BoundField HeaderText="谈判目标" />
                            <asp:HyperLinkField HeaderText="查看" />
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="放　　弃" />
                    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="保　　存" />
                </td>
            </tr>
        </table>
        </td>
                <td style="width:5%; height: 480px; text-align: center" valign="middle">
                    <img height="401" src="images/shuxian.jpg" /></td>
        </tr></table>
    </form>
</body>
</html>
