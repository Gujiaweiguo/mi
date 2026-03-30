<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OverruleInvDiscQuery.aspx.cs" Inherits="Invoice_InvDisc_OverruleInvDiscQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "InvDiscDet_InvDiscDe")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	function hidden()
	{
		document.getElementById("lblTotalNum").style.display="none";
        document.getElementById("lblCurrent").style.display="none";
        loadTitle();
        
    }
	</script>
	
</head>
<body topmargin=0 leftmargin=0 onload="hidden()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 457px">
            <tr>
                <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 820px;
                    height: 22px; text-align: left">
                    <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,InvDiscDet_InvDiscDe %>"
                        Width="281px"></asp:Label></td>
                <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                </td>
                <td class="tdTopRightBackColor" style="width: 7px; height: 22px" valign="top">
                    <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="3" style="position: static; height: 339px; text-align: center"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 45px">
                        <tr class="headLine">
                            <td colspan="4" style="height: 1px; background-color: white" width="100%">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 635px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 5px; text-align: center; width: 100%;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <!--  *********left
                    -->
                            <td colspan="2" style="vertical-align: top; width: 100%; height: 405px">
                                <table id="" border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 100%;
                                    height: 1px">
                                    <tr style="height: 5px">
                                        <td style="width: 154px; height: 5px">
                                        </td>
                                        <td class="baseInput" style="width: 199px; height: 5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="baseInput" colspan="2" rowspan="3" style="vertical-align: top; height: 110px;
                                            text-align: center">
                                            <asp:GridView ID="GrdVewInvAdj" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderStyle="Inset" BorderWidth="1px" Height="300px" Width="550px">
                                                <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                <Columns>
                                                    <asp:BoundField DataField="DiscDetID">
                                                        <ItemStyle CssClass="hidden" />
                                                        <HeaderStyle CssClass="hidden" />
                                                        <FooterStyle CssClass="hidden" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InvCode" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InvDate" HeaderText="<%$ Resources:BaseInfo,Rpt_InvDate %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InvPayAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayAmt %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InvPaidAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_InvPaidAmt %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DiscRate" HeaderText="<%$ Resources:BaseInfo,InvDiscDet_DiscDetRatio %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InvDiscAmt" HeaderText="<%$ Resources:BaseInfo,InvDiscDet_DiscDetMoney %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td style="width: 1px; height: 119px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" rowspan="1" style="vertical-align: middle; height: 60px; text-align: center"
                                            valign="top">
                                            <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" OnClick="btnBack_Click"
                                                Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNext" runat="server"
                                                    CssClass="buttonNext" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" /><asp:Label
                                                        ID="lblTotalNum" runat="server" CssClass="hidden" Height="9px" Width="62px"></asp:Label><asp:Label
                                                            ID="lblCurrent" runat="server" CssClass="hidden" ForeColor="Red" Height="9px"
                                                            Width="1px">1</asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                            <!--  *********right
                    -->
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
       
        <asp:HiddenField ID="InvDiscDet_InvDiscDe" runat="server" Value="<%$ Resources:BaseInfo,InvDiscDet_InvDiscDe %>"/>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
