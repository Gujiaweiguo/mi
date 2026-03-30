<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PalaverModi.aspx.cs" Inherits="Lease_PotCustomer_PalaverModi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Title_Palaver")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
  <%--  <script type="text/javascript"  src="../../JavaScript/setday.js"></script>--%>
    <script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
     <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
		function Load()
		{
		    loadTitle();
		    document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
		}
		</script>
</head>
<body style="margin-top:0; margin-left:0" onload="Load();">
    <form id="form1" runat="server">
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                   <%= (String)GetGlobalResourceObject("BaseInfo", "Title_Palaver")%>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 410px">
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 50%; height: 320px; text-align: right"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 311px" width="255">
                        <tr>
                            <td class="tdBackColor" style="width: 143px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td style="width: 160px; height: 10px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 163px; text-align: center">
                                    <tr>
                                        <td style="width: 324px; position: relative; top: 6px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 324px; position: relative; top: 6px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px; text-align: center"
                                valign="bottom">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 143px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td style="width: 160px; height: 10px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 143px; height: 22px; text-align: right">
                                <asp:Label ID="lblPalaverTime" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPalaverTime %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPalaverTime" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 143px; height: 22px; text-align: right">
                                <asp:Label ID="lblPalaverUser" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPalaverUser %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPalaverUser" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 143px; height: 22px; text-align: right">
                                <asp:Label ID="lblPalaverAim" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPalaverAim %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPalaverAim" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 143px; height: 22px; text-align: right">
                                <asp:Label ID="lblPalaverNode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPalaverNode %>"></asp:Label>
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 160px; text-align: center">
                                    <tr>
                                        <td style="width: 324px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 324px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 100px; text-align: center">
                                <asp:TextBox ID="txtPalaverNode" runat="server" CssClass="OpenColor" Height="141px"
                                    MaxLength="512" TextMode="MultiLine" Width="233px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 22px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 173px; text-align: center">
                                    <tr>
                                        <td style="left: 15px; width: 324px; position: relative; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="left: 15px; width: 324px; position: relative; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 22px; text-align: left">
                                &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px"
                                    OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>"
                                    Width="70px" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Height="31px" OnClick="btnCancel_Click"
                                    Text="<%$ Resources:BaseInfo,User_btnCancel %>" Width="70px" />
                                </td>
                        </tr>
                    </table>
                </td>
                <td class="tdBackColor" style="width: 280px; height: 330px" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 311px" width="280">
                        <tr>
                            <td class="tdBackColor" style="width: 280px; height: 22px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 271px; text-align: center">
                                    <tr>
                                        <td style="width: 324px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 324px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="vertical-align: top; width: 280px; height: 254px;
                                text-align: center" valign="top">
                                <asp:GridView ID="GrdCustPalaverInfo" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Font-Size="12pt"
                                    Font-Strikeout="False" Height="285px" OnRowDataBound="GrdCustPalaverInfo_RowDataBound"
                                    OnSelectedIndexChanged="GrdCustPalaverInfo_SelectedIndexChanged" PageSize="12"
                                    Width="277px">
                                    <Columns>
                                        <asp:BoundField DataField="PalaverID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PalaverTime" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverTime %>">
                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PalaverAim" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverAim %>">
                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                        </asp:BoundField>
                                        <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_lblUserQuery %>" SelectText="<%$ Resources:BaseInfo,User_lblUserQuery %>"
                                            ShowSelectButton="True">
                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                        </asp:CommandField>
                                    </Columns>
                                    <FooterStyle BackColor="Red" ForeColor="#000066" />
                                    <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 280px; height: 22px; text-align: center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 270px; text-align: center">
                                    <tr>
                                        <td style="width: 324px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 324px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 280px; height: 22px; text-align: right">
                                <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                    Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNext" runat="server"
                                        CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" /><asp:Label
                                            ID="lblCurrent" runat="server" ForeColor="Red">1</asp:Label><asp:Label ID="lblTotalNum"
                                                runat="server"></asp:Label>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
