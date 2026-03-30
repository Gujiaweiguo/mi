<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaseItemModifyAutiding.aspx.cs" Inherits="Lease_LeaseItemModify_LeaseItemModifyAutiding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "LeaseModify_Title")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        
        <!--
            talbe.tblLease tr{ height:30px;}
            table.tblLease tr.bodyLine{ height:1px;}
        -->
        
        
    </style>  
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
     <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script type="text/javascript">
	    function Load()
	    {
	        loadTitle();
	    } 
    </script>
</head>
<body onload="Load()" style="margin:0px" >
    <form id="form1" runat="server">
    <div id="LeaseItem">

     <table border="0" cellpadding="0" cellspacing="0" style="height: 25px" width="100%">
            <tr>
                <td class="tdTopBackColor" style="width: 445px; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" />
                    <asp:Label ID="Label49" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblLeaseItem %>" Width="367px"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 628px; height: 25px; text-align:right;" valign="top">
                    <img class="imageRightBack" /></td>
            </tr>
            </table>
            
            <table class="tblLease" style="width:100%; height:359px" border="0" cellpadding="0" cellspacing="0">
                <tr class="headLine">
                    <td class="tdBackColor" style="background-color:White;" colspan="4">
                    </td>
                </tr>
                <tr>
                    <td class="tdBackColor" style="width: 118px; text-align: right" colspan="4">
                    </td>
                </tr>
             <tr class="bodyLine">
                 <td class="tdBackColor" style="width: 163px; height: 19px; text-align: right">
                     </td>
                 <td class="tdBackColor" style="width: 182px; height: 19px" valign="bottom">
                 <table style="width:165px;" border="0" cellpadding="0" cellspacing="0">
                     <tr class="bodyLine">
                         <td
                              style="height: 1px; background-color: #738495">
                         </td>
                     </tr>
                     <tr class="bodyLine">
                         <td style="height: 1px; background-color: #ffffff"></td>
                     </tr>
                 </table>
                 </td>
                 <td class="tdBackColor" style="width: 163px; height:20px">
                 </td>
                 <td class="tdBackColor" style="width: 165px" valign="bottom" align="left">
                     <table style="width:165px;" border="0" cellpadding="0" cellspacing="0">
                         <tr class="bodyLine">
                             <td 
                                  style="height: 1px; background-color: #738495">
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
                    <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label36" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labBillCycle %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor">
                     <asp:DropDownList ID="DDownListBillCycle" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                    <td class="tdBackColor" align="right" style="padding-right:5px">
                     <asp:Label ID="Label47" runat="server" Text="<%$ Resources:BaseInfo,ConLease_lblTaxFrank %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" >
                        <asp:TextBox ID="txtTaxRate" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                </tr >
                <tr >
                    <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label41" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labCurrencyType %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" style="width: 182px; height: 23px">
                     <asp:DropDownList ID="DDownListCurrencyType" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                    <td class="tdBackColor" align="right" style="padding-right:5px">
                     <asp:Label ID="Label48" runat="server" Text="<%$ Resources:BaseInfo,ConLease_InvoiceType %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" style="width: 262px">
                     <asp:DropDownList ID="DDownListTaxType" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                </tr>
                <tr >
                    <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label37" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labSettleMode %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" style="width: 182px; height: 23px">
                     <asp:DropDownList ID="DDownListSettleMode" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                    <td class="tdBackColor" style="width: 107px; text-align: right">
                    </td>
                    <td class="tdBackColor" style="width: 262px">
                    </td>
                </tr>
                <tr >
                    <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label38" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labMonthSettleDays %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" style="width: 182px; height: 23px">
                        <asp:TextBox ID="txtMonthSettleDays" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                    <td class="tdBackColor" style="width: 107px; text-align: right">
                    </td>
                    <td class="tdBackColor" style="width: 262px">
                    </td>
                </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label39" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labYesNoMin %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 23px;">
                     <asp:DropDownList ID="DDownListIfPrepay" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList>
                 </td>
                 <td class="tdBackColor" style="width: 107px; text-align: right">
                 </td>
                 <td class="tdBackColor" style="width: 262px">
                 </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label40" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labFirstAccountMonth %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 24px;">
                     <asp:TextBox ID="txtBalanceMonth" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                 <td class="tdBackColor" style="width: 107px; text-align: right; height: 24px;">
                     </td>
                 <td class="tdBackColor" rowspan="4" valign="top" style="width: 262px">
                     </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="width: 163px; height: 15px; text-align: right">
                     </td>
                 <td class="tdBackColor" style="width: 182px; height: 19px" valign="bottom">
                 <table style="width:165px;" border="0" cellpadding="0" cellspacing="0">
                     <tr class="bodyLine">
                         <td
                              style="height: 1px; background-color: #738495">
                         </td>
                     </tr>
                     <tr class="bodyLine">
                         <td style="height: 1px; background-color: #ffffff"></td>
                     </tr>
                 </table>
                 </td>
                 <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                 </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label44" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPayTypeID %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 19px">
                     <asp:DropDownList ID="DDownListPayTypeId" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                 <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                 </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label45" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labLatePayInt %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 19px">
                     <asp:TextBox ID="txtLatePayInt" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                 <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                 </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px; height: 26px;" align="right">
                     <asp:Label ID="Label46" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labIntDay %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 26px">
                     <asp:TextBox ID="txtIntDay" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                 <td class="tdBackColor" style="width: 107px; height: 26px; text-align: right">
                 </td>
                 <td class="tdBackColor" style="height: 26px; width: 262px;">
                 </td>
             </tr>
                <tr >
                    <td class="tdBackColor" colspan="2" style="height: 19px">
                        </td>
                    <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                    </td>
                    <td class="tdBackColor" style="height: 19px; width: 262px;">
                    </td>
                </tr>
            </table>
         

        </div>
    </form>
</body>
</html>
