<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectAnPMaster.aspx.cs" Inherits="Generalize_Medium_SelectAnPMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
         <title><%= (String)GetGlobalResourceObject("BaseInfo", "Master_lblAnPRecordParticular")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
		<script type="text/javascript">
	    function Load()
	    {
	        addTabTool(document.getElementById("hidcon").value + ",Generalize/Medium/SelectAnPMaster.aspx");
	        loadTitle();
	        document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
	    }
    </script>
</head>
<body  style="margin-top:0; margin-left:0" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
    <table style="width: 100%" cellpadding="0" cellspacing="0">
<%--                    <tr>
                        <td style="width: 826px; height: 25px; vertical-align: middle; text-align: left;" class="tdTopBackColor" valign="top">
                            <img class="imageLeftBack" alt="" />
                            <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_labCustomerQuery %>" Width="420px"></asp:Label></td>
                        <td style="width: 538px; height: 25px; text-align: right" class="tdTopRightBackColor"
                            valign="top" colspan="2">
                            <img class="imageRightBack" alt="" /></td>
                    </tr>--%>
                    <tr >
                    <td>
                        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
                            width: 100%; text-align: center;" >
                            <tr>
                                <td class="tdTopBackColor" style="width: 5px">
                                    <img alt="" class="imageLeftBack" />
                                </td>
                                <td class="tdTopBackColor">
                                    <%= (String)GetGlobalResourceObject("BaseInfo", "Master_lblAnPRecordParticular")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 329px; text-align: center" class="tdBackColor" valign="top"
                            colspan="3">
                            <table style="width: 100%; height: 380px" cellspacing="0" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td style="width: 495px; height: 5px" class="tdBackColor" colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="8" style="height: 22px; width: 100%; text-align: center;">
                                        <table style="width: 100%">
                                        <tr>
                                        <td style="width: 15%; height: 22px">
                                            </td>
                                        <td style="width: 15%; height: 22px">
                                            </td>
                                        <td style="width: 5%; height: 22px">
                                            </td>
                                        <td style="width: 20%; height: 22px">
                                            <asp:DropDownList ID="cmbConSelect" runat="server" Width="115px" >
                                            </asp:DropDownList></td>
                                        <td style="width: 20%; height: 22px">
                                            <asp:TextBox ID="txtQueryMes" runat="server" CssClass="textstyle" Width="95px"></asp:TextBox>&nbsp;
                                            </td>
                                        <td style="width: 25%; height: 22px">
                                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                Text="<%$ Resources:BaseInfo,User_lblQuery %>" TabIndex="1" /></td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 12px; text-align: center" class="tdBackColor" colspan="8">
                                            <table style="left: 12px; width: 90%; position: relative" cellspacing="0" cellpadding="0"
                                                border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 260px; text-align: center; vertical-align: top;" class="tdBackColor" colspan="8">
                                            <table style="width: 549px; height: 260px;">
                                                <tbody>
                                                    <tr>
                                                        <td style="left: 7px; vertical-align: top; width: 100%; position: relative; text-align: center">
                                                            <asp:GridView ID="gvAnPMaster" runat="server" BorderWidth="1px" BorderStyle="Inset" CellPadding="3" BackColor="White" Width="531px" Height="258px"
                                                                AutoGenerateColumns="False" OnSelectedIndexChanged="gvAnPMaster_SelectedIndexChanged">
                                                                <Columns>
                                                                    <asp:BoundField DataField="AnpID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AnPNm" HeaderText="<%$ Resources:BaseInfo,Master_lblAnPNm %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ThemeNm" HeaderText="<%$ Resources:BaseInfo,Master_lblAnPTheme %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="StartDate" HeaderText="<%$ Resources:BaseInfo,Master_lblStart %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="EndDate" HeaderText="<%$ Resources:BaseInfo,Master_lblEnd %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TargetSales" HeaderText="<%$ Resources:BaseInfo,Master_lblSales %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TargetPeopletime" HeaderText="<%$ Resources:BaseInfo,Master_lblTargetPeopletime %>">
                                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" ShowSelectButton="True">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:CommandField>
                                                                </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 55px; height: 12px" class="tdBackColor">
                                        </td>
                                        <td style="width: 8px; height: 12px" class="tdBackColor">
                                        </td>
                                        <td style="width: 137px; height: 12px" class="tdBackColor">
                                        <asp:Label
                                                    ID="lblTotalNum" runat="server"></asp:Label><asp:Label ID="lblCurrent" runat="server" ForeColor="Red">1</asp:Label></td>
                                        <td style="height: 22px" class="tdBackColor">
                                        </td>
                                        <td style="vertical-align: top; width: 270px; height: 44px; left: 30px; text-align: right;" class="tdBackColor"
                                            colspan="4">
                                             <table style="width: 283px" cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495; left: 25px; position: relative;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff; left: 25px; position: relative;">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        <table>
                                        <tr>
                                        <td style="left: 40px; position: relative; height: 37px">
                                            <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                                Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNext" runat="server"
                                                    CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" />
                                        </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
            </table>
            
    </div>
        <asp:HiddenField ID="hidcon" runat="server" Value="<%$ Resources:BaseInfo,Master_lblAnPRecordParticular %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
