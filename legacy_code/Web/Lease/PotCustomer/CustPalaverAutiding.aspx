<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustPalaverAutiding.aspx.cs" Inherits="Lease_PotCustomer_CustPalaverAutiding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Title_CustLicense")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
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
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 405px; text-align: center;">
            <tr>
                <td colspan="2" style="vertical-align: top; width: 100%; height: 22px">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 22px">
                        <tr>
                            <td class="tdTopRightBackColor" style="width: 8px; height: 22px; text-align: left"
                                valign="top">
                                <img alt="" class="imageLeftBack" style="height: 22px; text-align: left" />
                            </td>
                            <td class="tdTopRightBackColor" style="width: 545px; height: 22px; text-align: left">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblVoucherTitle %>"
                                    Width="152px"></asp:Label></td>
                            <td class="tdTopRightBackColor" style="width: 20px; height: 22px; text-align: right"
                                valign="top">
                                <img class="imageRightBack" style="width: 7px; height: 22px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 1px; background-color: white">
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 485px; height: 5px">
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="2" style="vertical-align: middle; height: 11px;
                    text-align: left">
                    <table border="0" cellpadding="0" cellspacing="0" style="left: 104px; width: 522px;
                        top: 0px; text-align: center">
                        <tr>
                            <td style="width: 240px; height: 1px; background-color: #738495">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 240px; height: 1px; background-color: #ffffff">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" class="tdBackColor" colspan="2" style="left: 5px; vertical-align: middle;
                    width: 485px; height: 150px; text-align: center" valign="middle">
                    <table>
                        <tr>
                            <td style="vertical-align: top; text-align: center">
                                <asp:GridView ID="GrdCustPalaverInfo" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="153px"
                                    OnRowDataBound="GrdCustPalaverInfo_RowDataBound" OnSelectedIndexChanged="GrdCustPalaverInfo_SelectedIndexChanged"
                                    PageSize="6" Width="526px">
                                    <Columns>
                                        <asp:BoundField DataField="PalaverID" FooterText="PalaverID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PalaverTime" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverTime %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PalaverName" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverUser %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PalaverAim" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverAim %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:CommandField SelectText="<%$ Resources:BaseInfo,User_lblUserQuery %>" ShowSelectButton="True">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
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
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="2" style="vertical-align: bottom; width: 485px;
                    height: 35px; text-align: right">
                    <asp:Label ID="lblCurrent" runat="server" ForeColor="Red">1</asp:Label><asp:Label
                        ID="lblTotalNum" runat="server"></asp:Label>&nbsp;<asp:Button ID="btnBack" runat="server"
                            CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button
                                ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click"
                                Text="<%$ Resources:BaseInfo,Button_next %>" />
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="tdBackColor" style="width: 315px; height: 12px">
                    <asp:Label ID="lblPalaverNode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPalaverNode %>"
                        Width="89px"></asp:Label></td>
                <td class="tdBackColor" style="vertical-align: bottom; width: 2688px; height: 12px;
                    text-align: left" valign="bottom">
                    <table border="0" cellpadding="0" cellspacing="0" style="left: 104px; width: 436px;
                        top: 0px; text-align: center">
                        <tr>
                            <td style="width: 240px; height: 1px; background-color: #738495">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 240px; height: 1px; background-color: #ffffff">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="2" style="vertical-align: middle; width: 485px;
                    height: 150px; text-align: center">
                    <table style="width: 531px">
                        <tr>
                            <td colspan="2" style="left: 5px; position: relative; height: 131px">
                                <asp:TextBox ID="txtPalaverContent" runat="server" CssClass="EnabledColor" Height="104px"
                                    ReadOnly="True" TextMode="MultiLine" Width="508px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
