<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IssueCard.aspx.cs" Inherits="Associator_IssueCard_IssueCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_lblIssueNewCard")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        <!--
        table.tbIntegral tr{ height:28px; }
        
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
	        loadTitle();
	    }
    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
    <div>
        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                 <td style="height: 24px; text-align: left; width: 854px;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIssueNewCard %>"></asp:Label></td>
            <td style="height: 24px; width: 663px;" class="tdTopRightBackColor" align="left"></td>
            <td style=" height: 24px;" class="tdTopRightBackColor" valign="top">
                <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 854px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 854px">
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                        <tr>
                            <td style="width: 81px" class="baseLable">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardID %>"></asp:Label></td>
                            <td style="width: 251px">
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                            <td colspan="2" rowspan="2">
                            <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_CardIDInput %>"></asp:Label>
                                    </legend>
                                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton1" runat="server" Text="<%$ Resources:BaseInfo,Associator_radCardMac %>" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton2" runat="server" Text="<%$ Resources:BaseInfo,Associator_radKeyboard %>" /></td>
                                    </tr>
                                </table>
                        </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 81px" class="baseLable">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIssueDate %>"></asp:Label></td>
                            <td style="width: 251px">
                                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 81px" class="baseLable">
                            </td>
                            <td style="width: 251px">
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 854px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 854px;padding-left:30px; padding-right:30px">
                 <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblCardInfo %>"></asp:Label>
                                    </legend>
                     <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                         <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorCode %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:TextBox ID="TextBox3" runat="server" Width="136px"></asp:TextBox></td>
                             <td>
                                 <asp:Button ID="Button1" runat="server" Text="搜索" /></td>
                         </tr>
                         <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"></asp:Label></td>
                             <td colspan="2">
                                 <asp:TextBox ID="TextBox4" runat="server" Width="40px"></asp:TextBox>
                                 &nbsp;&nbsp;&nbsp;
                                 <asp:TextBox ID="TextBox7" runat="server" Width="136px"></asp:TextBox></td>
                         </tr>
                         <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblOtherPassPort %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:TextBox ID="TextBox5" runat="server" Width="135px"></asp:TextBox></td>
                             <td>
                             </td>
                         </tr>
                          <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardType %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:DropDownList ID="DropDownList1" runat="server" Width="143px">
                                 </asp:DropDownList></td>
                             <td rowspan="3">
                             <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblCarDnature %>"></asp:Label>
                                    </legend>
                                 <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                     <tr>
                                         <td style="padding-left:10px">
                                             <asp:RadioButton ID="RadioButton3" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCustomerCard %>" /></td>
                                     </tr>
                                     <tr>
                                         <td style="padding-left:10px">
                                             <asp:RadioButton ID="RadioButton4" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblEmployeeCard %>" /></td>
                                     </tr>
                                     <tr>
                                         <td style="padding-left:10px">
                                             <asp:RadioButton ID="RadioButton5" runat="server" Text="<%$ Resources:BaseInfo,Dept_Other %>" /></td>
                                     </tr>
                                 </table>
                             </fieldset>       
                             </td>
                         </tr>
                         <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardLevel %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:DropDownList ID="DropDownList2" runat="server" Width="142px">
                                 </asp:DropDownList></td>
                          </tr>
                          <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label9" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTermDate %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:TextBox ID="TextBox6" runat="server" Width="135px"></asp:TextBox></td>
                         </tr>
                     </table>
                  </fieldset>                  
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 854px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 854px;padding-left:30px; padding-right:30px">
                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblMainCardInfo %>"></asp:Label>
                                    </legend>
                     <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                         <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorCode %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:TextBox ID="TextBox8" runat="server" Width="136px"></asp:TextBox></td>
                             <td style="width: 100px">
                                 <asp:Button ID="Button2" runat="server" Text="检索" /></td>
                             <td>
                             </td>
                         </tr>
                         <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"></asp:Label></td>
                             <td colspan="2">
                                 <asp:TextBox ID="TextBox9" runat="server" Width="40px"></asp:TextBox>
                                 &nbsp;&nbsp;&nbsp;
                                 <asp:TextBox ID="TextBox10" runat="server" Width="136px"></asp:TextBox></td>
                             <td colspan="1">
                             </td>
                         </tr>
                         <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label15" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblOtherPassPort %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:TextBox ID="TextBox11" runat="server" Width="135px"></asp:TextBox></td>
                             <td style="width: 100px">
                             </td>
                             <td>
                             </td>
                         </tr>
                          <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label16" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardID %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:TextBox ID="TextBox13" runat="server" Width="135px"></asp:TextBox></td>
                             <td style="width: 100px" class="baseLable">
                                 <asp:Label ID="Label17" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIssueDate %>"></asp:Label></td>
                              <td>
                                  <asp:TextBox ID="TextBox15" runat="server" Width="135px"></asp:TextBox></td>
                         </tr>
                         <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label18" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardType %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:TextBox ID="TextBox14" runat="server" Width="135px"></asp:TextBox></td>
                                 <td style="width: 100px" class="baseLable">
                                     <asp:Label ID="Label20" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardType %>"></asp:Label></td>
                             <td>
                                 <asp:TextBox ID="TextBox16" runat="server" Width="135px"></asp:TextBox></td>
                          </tr>
                          <tr>
                             <td class="baseLable" style="width: 75px">
                                 <asp:Label ID="Label19" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTermDate %>"></asp:Label></td>
                             <td style="width: 156px">
                                 <asp:TextBox ID="TextBox12" runat="server" Width="135px"></asp:TextBox></td>
                                 <td style="width: 100px"></td>
                              <td>
                              </td>
                         </tr>
                     </table>
                  </fieldset>  
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 854px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 854px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

