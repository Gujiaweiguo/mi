<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WrkFlwMessage.aspx.cs" Inherits="Lease_AuditingLease_WrkFlwMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin:0px">
    <form id="form1" runat="server">
    <div>
    <asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 100%; height: 24px; text-align: left;" class="tdTopRightBackColor" align="left">
                            <img class="imageLeftBack" src="" style="width: 7px"  />
                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,WrkFlwEntity_lblMessage %>" Width="99px"></asp:Label></td>
                        <td style="width: 100%; height: 24px;" class="tdTopRightBackColor" align="left"></td>
                        <td style="width: 7px; height: 24px;" class="tdTopRightBackColor" valign="top">
                            <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="margin-left:20px; margin-right:20px">
                            <asp:GridView ID="GVMessage" runat="server" AutoGenerateColumns="False" PageSize="15" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:BaseInfo,WrkFlwEntity_lblConfirmUser %>">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                     </asp:BoundField>
                                    <asp:BoundField DataField="CompletedTime" HeaderText="<%$ Resources:BaseInfo,WrkFlwEntity_lblConfirmDate %>" >
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                     </asp:BoundField>
                                    <asp:BoundField DataField="VoucherMemo" HeaderText="<%$ Resources:BaseInfo,WrkFlwEntity_lblConfirmMessage %>" >
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NodeStatusName" HeaderText="<%$ Resources:BaseInfo,Rpt_Status %>" >
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr class="tdBackColor">
                        <td colspan="3">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>

