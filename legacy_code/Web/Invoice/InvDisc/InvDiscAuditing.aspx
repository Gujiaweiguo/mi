<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvDiscAuditing.aspx.cs" Inherits="Invoice_InvDisc_InvDiscAuditing" %>

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
        var str= document.getElementById("InvDiscDet_InvDiscDe").value + ",Invoice/InvDisc/InvDiscAuditing.aspx";
        addTabTool(str);
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
                            <td colspan="2" style="height: 5px; text-align: center">
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
                            <td style="width: 33%; height: 405px" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 97%;
                                    height: 275px">
                                    <tr style="height: 5px">
                                        <td style="width: 98px; height: 5px">
                                        </td>
                                        <td style="width: 168px; height: 5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="baseLable" style="width: 98px; height: 30px; text-align: center">
                                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"
                                                Width="53px"></asp:Label></td>
                                        <td style="width: 168px; height: 30px">
                                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr style="height: 5px">
                                        <td style="width: 98px; height: 30px; text-align: center">
                                            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                        <td style="vertical-align: middle; width: 168px; height: 30px">
                                            <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="baseLable" style="width: 98px; height: 28px; text-align: center">
                                            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"
                                                Width="58px"></asp:Label></td>
                                        <td style="width: 168px; height: 28px">
                                            <asp:TextBox ID="txtContractID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="baseLable" style="width: 98px; height: 28px; text-align: center">
                                            <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"
                                                Width="58px"></asp:Label></td>
                                        <td style="width: 168px; height: 28px">
                                            <asp:TextBox ID="txtShopName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="baseLable" style="width: 98px; height: 30px; text-align: right">
                                            &nbsp;&nbsp;
                                        </td>
                                        <td style="width: 168px; height: 30px">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                        <td class="baseLable" style="width: 98px; text-align: center">
                                            <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvDiscDet_DiscDetRatio %>"
                                                Width="77px"></asp:Label></td>
                                        <td style="width: 168px; height: 30px">
                                            <asp:TextBox ID="txtDiscRate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="baseLable" style="width: 98px; height: 25px; text-align: center">
                                            <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvDiscDet_DiscDetCausation %>"></asp:Label></td>
                                        <td rowspan="2" style="width: 168px; height: 40px">
                                            <asp:TextBox ID="txtDiscReason" runat="server" CssClass="EnabledColor" Height="32px"
                                                ReadOnly="True" Width="160px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="baseLable" style="vertical-align: middle; width: 98px; height: 9px; text-align: right">
                                            &nbsp; &nbsp; &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="baseLable" style="width: 98px; height: 31px; text-align: center">
                                            <asp:Label ID="labPalaverNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,CustPalaver_labPalaverNote %>"
                                                Width="66px"></asp:Label></td>
                                        <td rowspan="2" style="width: 168px; height: 40px">
                                            <asp:TextBox ID="txtVoucherMemo" runat="server" CssClass="ipt160px" Height="32px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="baseLable" style="width: 98px; height: 9px; text-align: right">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 98px; height: 60px; text-align: right">
                                            </td>
                                        <td style="height: 60px">
                                            <asp:Button ID="butConsent" runat="server" CssClass="buttonSave" OnClick="butConsent_Click"
                                                Text="<%$ Resources:BaseInfo,CustPalaver_butConsent %>" />
                                            <asp:Button ID="butOverrule" runat="server" CssClass="buttonClear" OnClick="butOverrule_Click"
                                                Text="<%$ Resources:BaseInfo,CustPalaver_butOverrule %>" /></td>
                                    </tr>
                                </table>
                                            <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvDiscDet_DiscDetMoney %>" Visible="False"></asp:Label>
                                            <asp:TextBox ID="txtDiscAmt" runat="server" CssClass="Enabledipt160px" ReadOnly="True" Visible="False"></asp:TextBox></td>
                            <!--  *********right
                    -->
                            <td style="vertical-align: top; height: 405px" width="50%">
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
                                                BorderStyle="Inset" BorderWidth="1px" Height="300px"
                                                OnSelectedIndexChanged="GrdVewInvAdj_SelectedIndexChanged" Width="439px">
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
                                                    <asp:BoundField DataField="InvActPayAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayAmt %>">
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
                                                    <asp:BoundField DataField="DiscAmt" HeaderText="<%$ Resources:BaseInfo,InvDiscDet_DiscDetMoney %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    </asp:BoundField>
                                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    </asp:CommandField>
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

