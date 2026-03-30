<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoicesQuery.aspx.cs" Inherits="Lease_InvoicesTrace_InvoicesTrace" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                 <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px; width: 100%; text-align: center;">
                    <tr>
                        <td class="tdTopBackColor" style="width: 5px">
                            <img alt="" class="imageLeftBack" />
                        </td>
                        <td class="tdTopBackColor">
                            <%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustTitle") %>
                        </td>
                    </tr>
                </table>
                <table style="width:100%" class="tdBackColor" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:20px">
                        <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%">
                            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></td>
                        <td style="width:20%">
                            <asp:DropDownList ID="DropDownList1" runat="server">
                            </asp:DropDownList></td>
                        <td style="width:10%">
                            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label></td>
                        <td style="width:20%">
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                        <td style="width:10%">  
                            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label></td>
                        <td style="width:20%">
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                        <td style="width:10%">
                            <asp:Button ID="Button1" runat="server" Text="Button" /></td>
                    </tr>
                    <tr style="height:20px">
                        <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" align="center" style="padding-left:15px; padding-right:15px">
                             <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 100%; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                              </table>
                        </td>
                    </tr>
                     <tr style="height:10px">
                       <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                        <td style="width:20%">
                        </td>
                        <td style="width:10%">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" align="center" style="padding-left:15px; padding-right:15px">
                            <asp:GridView ID="GridView1" runat="server" Width="100%">
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                        </td>
                        <td style="width: 20%">
                        </td>
                        <td style="width: 10%">
                        </td>
                        <td style="width: 20%">
                        </td>
                        <td style="width: 10%">
                        </td>
                        <td style="width: 20%">
                        </td>
                        <td style="width: 10%">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>