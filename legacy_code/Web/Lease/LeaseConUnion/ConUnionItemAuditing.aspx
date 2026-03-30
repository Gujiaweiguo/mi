<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConUnionItemAuditing.aspx.cs" Inherits="Lease_LeaseConUnion_ConUnionItemAuditing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseBalanceItem")%></title>
    
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js" language="javascript" charset="gb2312"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
    	function Load()
	    {
	        loadTitle();
	    }
    </script>
    
</head>
<body onload='Load()' style="margin:0px">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 100%;">
                    <tr style="height: 29px">
                        <td class="tdTopRightBackColor" style="width: 5px; height: 29px" valign="top">
                            <img align="left" class="imageLeftBack" src="" /></td>
                        <td class="tdTopRightBackColor" style="width: 635px; height: 29px; text-align: left">
                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblBasic %>"
                                Width="304px"></asp:Label></td>
                        <td class="tdTopRightBackColor" style="height: 29px; text-align: right" valign="top">
                            <img class="imageRightBack" src="" /></td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" class="tblLease" style="width: 100%;
                    height: 420px">
                    <tr class="headLine">
                        <td class="tdBackColor" colspan="4" style="background-color: white">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="4" style="width: 118px; text-align: right">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td class="tdBackColor" style="width: 163px; height: 19px; text-align: right">
                        </td>
                        <td class="tdBackColor" style="width: 182px; height: 19px" valign="bottom">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 165px">
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
                        <td class="tdBackColor" style="width: 163px; height: 20px">
                        </td>
                        <td align="left" class="tdBackColor" style="width: 165px" valign="bottom">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 165px">
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
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px">
                            <asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labBillCycle %>"
                                Width="60px"></asp:Label></td>
                        <td class="tdBackColor">
                            <asp:DropDownList ID="DDownListBillCycle" runat="server" Width="165px" Enabled="False">
                            </asp:DropDownList></td>
                        <td align="right" class="tdBackColor" style="padding-right: 5px">
                            <asp:Label ID="Label47" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_lblTaxFrank %>"></asp:Label></td>
                        <td class="tdBackColor">
                            <asp:TextBox ID="txtTaxRate" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px">
                            <asp:Label ID="Label19" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labCurrencyType %>"
                                Width="70px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 182px; height: 23px">
                            <asp:DropDownList ID="DDownListCurrencyType" runat="server" Width="165px" Enabled="False">
                            </asp:DropDownList></td>
                        <td align="right" class="tdBackColor" style="padding-right: 5px">
                            <asp:Label ID="Label48" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_InvoiceType %>"></asp:Label></td>
                        <td class="tdBackColor" style="width: 262px">
                            <asp:DropDownList ID="DDownListTaxType" runat="server" BackColor="#F5F5F4" Enabled="False"
                                Width="165px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px">
                            <asp:Label ID="Label17" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labAccountCycle %>"
                                Width="72px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 182px; height: 23px">
                            <asp:DropDownList ID="DDownListAccountCycle" runat="server" Width="165px" Enabled="False">
                            </asp:DropDownList></td>
                        <td class="tdBackColor" style="width: 107px; text-align: right">
                        </td>
                        <td class="tdBackColor" style="width: 262px">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px">
                            <asp:Label ID="Label38" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labMonthSettleDays %>"></asp:Label></td>
                        <td class="tdBackColor" style="width: 182px; height: 23px">
                            <asp:TextBox ID="txtMonthSettleDays" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
                        <td class="tdBackColor" style="width: 107px; text-align: right">
                        </td>
                        <td class="tdBackColor" style="width: 262px">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px">
                            <asp:Label ID="Label22" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConUnionRentInc %>"
                                Width="80px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 182px; height: 23px">
                            <asp:TextBox ID="txtRentInc" runat="server" CssClass="ipt160px" Style="ime-mode: disabled" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                        <td class="tdBackColor" style="width: 107px; text-align: right">
                        </td>
                        <td class="tdBackColor" style="width: 262px">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px">
                            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseConUnion_InTaxRate %>"
                                Width="70px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 182px; height: 24px">
                            <asp:TextBox ID="txtInTaxRate" runat="server" CssClass="ipt160px" Style="ime-mode: disabled" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                        <td class="tdBackColor" style="width: 107px; height: 24px; text-align: right">
                        </td>
                        <td class="tdBackColor" rowspan="4" style="width: 262px" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" style="width: 163px; height: 15px; text-align: right">
                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseConUnion_OutTaxRate %>"
                                Width="70px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 182px; height: 19px" valign="bottom">
                            <asp:TextBox ID="txtOutTaxRate" runat="server" CssClass="ipt160px" Style="ime-mode: disabled" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                        <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px">
                        </td>
                        <td class="tdBackColor" style="width: 182px; height: 19px">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 165px">
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
                        <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px">
                            <asp:Label ID="Label45" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labLatePayInt %>"></asp:Label></td>
                        <td class="tdBackColor" style="width: 182px; height: 19px">
                            <asp:TextBox ID="txtLatePayInt" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
                        <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; width: 163px; height: 26px">
                            <asp:Label ID="Label46" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labIntDay %>"></asp:Label></td>
                        <td class="tdBackColor" style="width: 182px; height: 26px">
                            <asp:TextBox ID="txtIntDay" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
                        <td class="tdBackColor" style="width: 107px; height: 26px; text-align: right">
                        </td>
                        <td class="tdBackColor" style="width: 262px; height: 26px">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="2" style="height: 19px">
                        </td>
                        <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                        </td>
                        <td class="tdBackColor" style="width: 262px; height: 19px">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;&nbsp;
    </form>
</body>
</html>

