<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CancelDaily.aspx.cs" Inherits="Sell_CancelDaily" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
        <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript">
	function Load()
    {
        addTabTool("<%=baseInfo %>,Sell/CancelDaily.aspx");
        loadTitle();
    }
    </script>
</head>
<body onload='Load()' style="margin:0px">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <div id="showLeaseBargain">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 401px; width: 100%;">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 70%; height: 22px; text-align: left; vertical-align: top;">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Sale_lblCancelDaily %>" Width="149px"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 20%; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width:10%; height: 22px; vertical-align: top; text-align: right;" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 100%; height: 339px" valign="top">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 12%; height: 30px;">
                                                    </td>
                                                    <td style="width: 16%; height: 30px;">
                                                    </td>
                                                    <td style="width: 8%; height: 30px;">
                                                    </td>
                                                    <td style="width: 17%; height: 30px;">
                                                    </td>
                                                    <td style="width: 25%; height: 30px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 12%; padding-right:10px" align="right">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblDate %>"></asp:Label></td>
                                                    <td style="width: 16%">
                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 8%; padding-right:10px" align="right">
                                                        <asp:Button ID="BtnQry" runat="server" Text="<%$ Resources:BaseInfo,User_lblQuery %>" CssClass="buttonQuery" OnClick="BtnQry_Click"/></td>
                                                    <td style="width: 17%">
                                                        </td>
                                                    <td style="width: 25%">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 12%; height: 30px;">
                                                    </td>
                                                    <td style="width: 16%; height: 30px;">
                                                    </td>
                                                    <td style="width: 8%; height: 30px;">
                                                    </td>
                                                    <td style="width: 17%; height: 30px;">
                                                    </td>
                                                    <td style="width: 25%; height: 30px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 12%">
                                                    </td>
                                                    <td style="width: 16%">
                                                    </td>
                                                    <td style="width: 8%">
                                                    </td>
                                                    <td style="width: 17%">
                                                    </td>
                                                    <td style="width: 25%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" style="padding-left:20px; padding-right:20px">
                                                        <asp:GridView ID="GVDaily" runat="server" Width="100%" AutoGenerateColumns="False" BackColor="White" OnRowCommand="GVDaily_RowCommand">
                                                            <Columns>
                                                                <asp:BoundField DataField="BatchID">
                                                                    <ItemStyle CssClass="hidden" />
                                                                    <HeaderStyle CssClass="hidden" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ShopName" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="ShopCode" HeaderText="<%$ Resources:BaseInfo,Lease_lblShopCode %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="minTransID" HeaderText="<%$ Resources:BaseInfo,Sale_lblMinTransID %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="maxTransID" HeaderText="<%$ Resources:BaseInfo,Sale_lblMaxTransID %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="transNumber" HeaderText="<%$ Resources:BaseInfo,Sale_lblTransNumber %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="sumPaidAmt" HeaderText="<%$ Resources:BaseInfo,Associator_lblBargaining %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:ButtonField CommandName="CancelDaily" HeaderText="<%$ Resources:BaseInfo,User_btnCancel %>" Text="<%$ Resources:BaseInfo,User_btnCancel %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                </asp:ButtonField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 12%">
                                                    </td>
                                                    <td style="width: 16%">
                                                        <asp:Label ID="lblTotalNum" runat="server" Height="1px" Visible="False" Width="32px"></asp:Label><asp:Label
                                                            ID="lblCurrent" runat="server" ForeColor="Red" Height="1px" Visible="False" Width="1px">1</asp:Label></td>
                                                    <td style="width: 8%">
                                                    </td>
                                                    <td style="width: 17%">
                                                    </td>
                                                    <td style="width: 25%">
                                                        <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" Height="31px"
                                                            OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>" Width="71px" /><asp:Button
                                                                ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" Height="30px"
                                                                OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Width="73px" /></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 12%">
                                                    </td>
                                                    <td style="width: 16%">
                                                    </td>
                                                    <td style="width: 8%">
                                                    </td>
                                                    <td style="width: 17%">
                                                    </td>
                                                    <td style="width: 25%">
                                                    </td>
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                </table>
                            </div>
         </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
