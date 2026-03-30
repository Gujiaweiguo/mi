<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketChargeGift1.aspx.cs" Inherits="Associator_Perform_TicketChargeGift" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_TicketChargeGift") %></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        <!--
        table.tbIntegral tr{ height:28px; }
        
        table.tbCard tr{ height:10px; }
        
        table.tbIntegral tr.headLine{ height:1px; }
        table.tbIntegral tr.bodyLine{ height:1px; }
        
        table.tbIntegral td.baseLable{ padding-right:5px;text-align:right;}
        table.tbIntegral td.baseInput{ align:left;padding-right:20px }
        --> 
    </style>  
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	         addTabTool("<%=ticketChargeGift %>,Associator/Perform/Integral.aspx");
	        loadTitle();
	    }

    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
    <div>
         <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="height: 24px; text-align: left; width: 540px;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Associator_TicketChargeGift %>"></asp:Label></td>
            <td style="height: 24px; width: 663px;" class="tdTopRightBackColor" align="left"></td>
            <td style=" height: 24px;" class="tdTopRightBackColor" valign="top">
                <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
            </tr>
            <tr class="tdBackColor" style="height:20px">
                <td>
                </td>
                <td style="width: 663px">
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td colspan="3" style="padding-left:20px; padding-right:20px;">
                     <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:56%;">
                        <legend style="text-align: left">
                            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblSelServiceDesk %>"></asp:Label>
                        </legend>
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 48px">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_UserID %>" Width="67px"></asp:Label></td>
                            <td style="width: 89px">
                                <asp:TextBox ID="TextBox1" runat="server" Width="78px"></asp:TextBox></td>
                            <td style="width: 44px">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_ServiceDesk %>" Width="62px"></asp:Label></td>
                            <td style="width: 74px">
                                <asp:DropDownList ID="DropDownList1" runat="server" Width="94px">
                                </asp:DropDownList></td>
                            <td>
                                <asp:TextBox ID="TextBox2" runat="server" Width="236px"></asp:TextBox></td>
                        </tr>
                    </table>
                </fieldset>
                </td>
            </tr>
            <tr class="tdBackColor" style="height:20px">
                <td>
                </td>
                <td style="width: 663px">
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor" style="padding-left:20px; padding-right:20px;">
                <td colspan="2">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:100%;">
                        
                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 71px">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Associator_Largess %>"></asp:Label></td>
                                <td style="width: 534px">
                                    <asp:DropDownList ID="DropDownList2" runat="server" Width="103px">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="TextBox3" runat="server" Width="236px"></asp:TextBox></td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 71px">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTicketM %>" Width="92px"></asp:Label></td>
                                <td style="width: 534px">
                                    <asp:TextBox ID="TextBox4" runat="server" Width="78px"></asp:TextBox></td>
                                <td>
                                </td>
                            </tr>
                        </table>
                     </fieldset>   
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor" style="height:20px">
                <td>
                </td>
                <td style="width: 663px">
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td colspan="2" style="padding-left:20px; padding-right:20px;">
                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:73%;" id="FIELDSET2" language="javascript" onclick="return FIELDSET1_onclick()">
                                        
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 70px">
                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardID %>"></asp:Label></td>
                            <td style="width: 189px">
                                <asp:TextBox ID="TextBox8" runat="server" Width="161px"></asp:TextBox></td>
                            <td style="width: 69px">
                                <asp:Button ID="Button1" runat="server" Text="检索" /></td>
                            <td rowspan="3">
                            <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:73%;" id="FIELDSET3" language="javascript" onclick="return FIELDSET1_onclick()">
                                        <legend style="text-align: left">
                                            <asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_CardIDInput %>"></asp:Label>
                                        </legend>
                                <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton3" runat="server" Text="<%$ Resources:BaseInfo,Associator_radCardMac %>" Width="118px"/></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton4" runat="server" Text="<%$ Resources:BaseInfo,Associator_radKeyboard %>"/></td>
                                    </tr>
                                </table>
                             </fieldset>           
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"></asp:Label></td>
                            <td style="width: 189px">
                                <asp:TextBox ID="TextBox9" runat="server" Width="161px"></asp:TextBox></td>
                            <td style="width: 69px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px;">
                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,User_lblIdentity %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="TextBox10" runat="server" Width="161px"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px">
                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Associator_TicketM %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="TextBox11" runat="server" Width="161px"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                            <td style="height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px">
                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,Rpt_TransId %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="TextBox12" runat="server" Width="161px"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                            <td style="height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px">
                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblExchangeNumber %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="TextBox13" runat="server" Width="161px"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                            <td style="height: 16px"></td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblExchangeDate %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="TextBox5" runat="server" Width="161px"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                            <td style="height: 16px">
                                <asp:Button ID="Button2" runat="server" Text="兑换确认" /></td>
                        </tr>
                    </table>
                 </fieldset>                       
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor" style="height:20px">
                <td>
                </td>
                <td style="width: 663px">
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td>
                </td>
                <td style="width: 663px">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
