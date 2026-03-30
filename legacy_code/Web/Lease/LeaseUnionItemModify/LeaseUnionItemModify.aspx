<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaseUnionItemModify.aspx.cs" Inherits="Lease_LeaseUnionItemModify_LeaseUnionItemModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_LeaseUnionItemModify")%></title>
    
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
	    
	    //验证数字类型
        function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110)
            {
		        window.event.returnValue =true;
	        }else
	        {
		        window.event.returnValue =false;
	        }
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
                <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 709px">
                    <tr style="height: 29px">
                        <td class="tdTopRightBackColor" style="width: 5px; height: 29px" valign="top">
                            <img align="left" class="imageLeftBack" src="" /></td>
                        <td class="tdTopRightBackColor" style="width: 635px; height: 29px; text-align: left">
                            <asp:Label ID="Label29" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblUnionItem %>"
                                Width="304px"></asp:Label></td>
                        <td class="tdTopRightBackColor" style="height: 29px; text-align: right" valign="top">
                            <img class="imageRightBack" src="" /></td>
                    </tr>
                    <tr class="headLine">
                        <td colspan="3" style="height: 1px; background-color: white">
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" class="tblUnionLease" style="width: 709px;
                    height: 345px">
                    <tr class="colLine">
                        <td class="tdBackColor" style="height: 20px">
                        </td>
                        <td class="tdBackColor" style="width: 516px" valign="bottom">
                            <table border="0" cellpadding="0" cellspacing="0" width="165">
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
                        <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                            <asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labBillCycle %>"
                                Width="60px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 516px">
                            <asp:DropDownList ID="DDownListBillCycle" runat="server" Width="165px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                            <asp:Label ID="Label19" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labCurrencyType %>"
                                Width="70px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 516px">
                            <asp:DropDownList ID="DDownListCurrencyType" runat="server" Width="165px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                            <asp:Label ID="Label17" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labAccountCycle %>"
                                Width="72px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 516px">
                            <asp:DropDownList ID="DDownListAccountCycle" runat="server" Width="165px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr class="colLine">
                        <td class="tdBackColor" style="height: 20px">
                        </td>
                        <td class="tdBackColor" style="width: 516px; height: 20px;" valign="bottom">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
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
                        <td align="right" class="tdBackColor" style="padding-right: 5px; height: 28px;" width="20%">
                            <asp:Label ID="Label22" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConUnionRentInc %>"
                                Width="80px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 516px; height: 28px;">
                            <asp:TextBox ID="txtRentInc" runat="server" CssClass="ipt160px" Style="ime-mode: disabled"></asp:TextBox></td>
                    </tr>
                    <tr class="colLine">
                        <td class="tdBackColor" style="width: 1px; height: 20px;">
                        </td>
                        <td class="tdBackColor" style="width: 516px; height: 20px;" valign="bottom">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
                                <tr class="bodyLine">
                                    <td class="tdBackColor" style="width: 160px; height: 1px; background-color: #738495">
                                    </td>
                                </tr>
                                <tr class="bodyLine">
                                    <td class="tdBackColor" style="width: 160px; height: 1px; background-color: #ffffff">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                            <asp:Label ID="Label26" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_lblTaxFrank %>"
                                Width="70px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 516px">
                            <asp:TextBox ID="txtTaxRate" runat="server" CssClass="ipt160px" Style="ime-mode: disabled"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                            <asp:Label ID="Label27" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_InvoiceType %>"
                                Width="70px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 516px">
                            <asp:DropDownList ID="DDownListTaxType" runat="server" Width="165px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr class="colLine">
                        <td class="tdBackColor" style="width: 1px; height: 20px;">
                        </td>
                        <td class="tdBackColor" style="width: 516px; height: 20px;" valign="bottom">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
                                <tr class="bodyLine">
                                    <td class="tdBackColor" style="width: 160px; height: 1px; background-color: #738495">
                                    </td>
                                </tr>
                                <tr class="bodyLine">
                                    <td class="tdBackColor" style="width: 160px; height: 1px; background-color: #ffffff">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseConUnion_InTaxRate %>"
                                Width="70px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 516px">
                            <asp:TextBox ID="txtInTaxRate" runat="server" CssClass="ipt160px" Style="ime-mode: disabled"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; height: 16px;" width="20%">
                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseConUnion_OutTaxRate %>"
                                Width="70px"></asp:Label></td>
                        <td class="tdBackColor" style="width: 516px; height: 16px;">
                            <asp:TextBox ID="txtOutTaxRate" runat="server" CssClass="ipt160px" Style="ime-mode: disabled"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px; height: 25px" width="20%">
                        </td>
                        <td class="tdBackColor" style="width: 516px; height: 45px">
                            <asp:Button ID="btnTempSave" runat="server" CssClass="buttonCancel" Height="31px"
                                OnClick="btnTempSave_Click" Text="<%$ Resources:BaseInfo,Lease_NewLineBtnTemp %>"
                                Width="80px" /></td>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                        </td>
                        <td class="tdBackColor" style="width: 516px">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>;
    </form>
</body>
</html>
