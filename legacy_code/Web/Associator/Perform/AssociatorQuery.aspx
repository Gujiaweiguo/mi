<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssociatorQuery.aspx.cs" Inherits="Associator_Perform_AssociatorQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_lblAssociatorQuery")%></title>
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
	
</head>
<body style="margin:0px">
    <form id="form1" runat="server">
    <div>
        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="height: 24px; text-align: left; width: 573px;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAssociatorQuery %>"></asp:Label></td>
            <td style="height: 24px; width: 663px;" class="tdTopRightBackColor" align="left"></td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 50%;padding-left:20px; padding-right:20px">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:99%;">
                        <legend style="text-align: left">
                        <asp:Label ID="Label33" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblQueryCondition %>"></asp:Label>
                        </legend>
                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                            <tr>
                                <td style="width: 77px" class="baseLable">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorCode %>"></asp:Label></td>
                                <td style="width: 146px">
                                    <asp:TextBox ID="TextBox1" runat="server" Width="130px"></asp:TextBox></td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 77px" class="baseLable">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"></asp:Label></td>
                                <td style="width: 146px">
                                    <asp:TextBox ID="TextBox2" runat="server" Width="130px"></asp:TextBox></td>
                                <td rowspan="3">
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:99%;">
                                    <legend style="text-align: left">
                                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_CardIDInput %>"></asp:Label>
                                    </legend>
                                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RadioButton1" runat="server" Text="<%$ Resources:BaseInfo,Associator_radCardMac %>"/></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="RadioButton2" runat="server" Text="<%$ Resources:BaseInfo,Associator_radKeyboard %>"/></td>
                                        </tr>
                                    </table>
                                  </fieldset>  
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 77px" class="baseLable">
                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorIdentity %>"></asp:Label></td>
                                <td style="width: 146px">
                                    <asp:TextBox ID="TextBox3" runat="server" Width="130px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 77px" class="baseLable">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardID %>"></asp:Label></td>
                                <td style="width: 146px">
                                    <asp:TextBox ID="TextBox4" runat="server" Width="130px"></asp:TextBox></td>
                            </tr>
                        </table>
                     </fieldset>   
                </td>
                <td>
                 <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:99%;">
                        <legend style="text-align: left">
                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblContact %>"></asp:Label>
                        </legend>
                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                            <tr>
                                <td style="width: 149px" colspan="2">
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingAddr %>"></asp:Label></td>
                                <td style="width: 69px">
                                    </td>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,Rpt_MobileTel %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 149px" colspan="2">
                                    <asp:TextBox ID="TextBox5" runat="server" Width="196px"></asp:TextBox></td>
                                <td style="width: 69px">
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:BaseInfo,User_lblOfficeTel %>" Width="91px"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="TextBox8" runat="server" Width="130px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 149px" colspan="2">
                                    <asp:TextBox ID="TextBox6" runat="server" Width="196px"></asp:TextBox></td>
                                <td style="width: 69px">
                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorMobileTel %>" Width="87px"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="TextBox9" runat="server" Width="130px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 149px" colspan="2">
                                    <asp:TextBox ID="TextBox7" runat="server" Width="196px"></asp:TextBox></td>
                                <td style="width: 69px">
                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorMobileTel %>" Width="93px"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="TextBox10" runat="server" Width="130px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 20%">
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblEMail %>" Width="88px"></asp:Label></td>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBox11" runat="server" Width="196px"></asp:TextBox></td>
                            </tr>
                        </table>
                     </fieldset> 
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; width: 50%">
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; width: 50%">
                     <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:99%;">
                        <legend style="text-align: left">
                        <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblCardInfo %>"></asp:Label>
                        </legend>
                         <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                             <tr>
                                 <td style="width: 119px" align="center">
                                     <asp:Label ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblNo %>"></asp:Label></td>
                                 <td style="width: 66px" align="center">
                                     <asp:Label ID="Label15" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabLevel %>"></asp:Label></td>
                                 <td style="width: 66px" align="center">
                                     <asp:Label ID="Label16" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblType %>"></asp:Label></td>
                                 <td style="width: 98px" align="center">
                                     <asp:Label ID="Label17" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblDoCardDate %>"></asp:Label></td>
                                 <td align="center">
                                     <asp:Label ID="Label18" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTermDate %>"></asp:Label></td>
                             </tr>
                             <tr>
                                 <td style="width: 119px">
                                     <asp:TextBox ID="TextBox12" runat="server" Width="108px"></asp:TextBox></td>
                                 <td style="width: 66px">
                                     <asp:TextBox ID="TextBox13" runat="server" Width="57px"></asp:TextBox></td>
                                 <td style="width: 66px">
                                     <asp:TextBox ID="TextBox14" runat="server" Width="57px"></asp:TextBox></td>
                                 <td style="width: 98px">
                                     &nbsp;<asp:TextBox ID="TextBox15" runat="server" Width="83px"></asp:TextBox></td>
                                 <td>
                                     <asp:TextBox ID="TextBox16" runat="server" Width="81px"></asp:TextBox></td>
                             </tr>
                             <tr>
                                 <td style="width: 119px">
                                     <asp:Label ID="Label19" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblHistoryInfo %>"></asp:Label></td>
                                 <td style="width: 66px">
                                 </td>
                                 <td style="width: 66px">
                                 </td>
                                 <td style="width: 98px">
                                 </td>
                                 <td>
                                 </td>
                             </tr>
                             <tr>
                                 <td colspan="5">
                                     <asp:GridView ID="GridView1" runat="server" Width="435px">
                                     </asp:GridView>
                                 </td>
                             </tr>
                             <tr>
                                 <td style="width: 119px">
                                     <asp:Label ID="Label20" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblHistoryInfo %>"></asp:Label></td>
                                 <td style="width: 66px">
                                 </td>
                                 <td style="width: 66px">
                                 </td>
                                 <td style="width: 98px">
                                 </td>
                                 <td>
                                 </td>
                             </tr>
                             <tr>
                                 <td colspan="5">
                                     <asp:GridView ID="GridView2" runat="server" Width="435px">
                                     </asp:GridView>
                                 </td>
                             </tr>
                         </table>
                      </fieldset> 
                </td>
                <td style="vertical-align:top">
                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:99%;">
                        <legend style="text-align: left">
                        <asp:Label ID="Label21" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblIntegralInfo %>"></asp:Label>
                        </legend>
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                        <tr>
                            <td style="width: 97px">
                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAccumulateIntegral %>"></asp:Label></td>
                            <td colspan="2">
                                <asp:TextBox ID="TextBox17" runat="server" Width="196px"></asp:TextBox></td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 97px">
                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIntegralToZero %>" Width="128px"></asp:Label></td>
                            <td style="width: 131px">
                                <asp:TextBox ID="TextBox18" runat="server" Width="130px"></asp:TextBox></td>
                            <td style="width: 75px">
                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblDateToZero %>" Width="82px"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TextBox20" runat="server" Width="130px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 97px">
                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblLargessExchargIntegral %>" Width="134px"></asp:Label></td>
                            <td style="width: 131px">
                                <asp:TextBox ID="TextBox19" runat="server" Width="130px"></asp:TextBox></td>
                            <td style="width: 75px">
                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblLargessExchargIntegral %>" Width="87px"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TextBox21" runat="server" Width="130px"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td colspan="4">
                            <hr>
                            </td>
                        </tr>
                         <tr>
                            <td style="width: 97px">
                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblLargessExchargIntegral %>" Width="119px"></asp:Label></td>
                            <td style="width: 131px">
                                <asp:TextBox ID="TextBox22" runat="server" Width="130px"></asp:TextBox></td>
                            <td style="width: 75px">
                            </td>
                            <td>
                            </td>
                        </tr>
                         <tr>
                            <td style="width: 97px">
                                <asp:Label ID="Label28" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>" Width="125px"></asp:Label></td>
                             <td colspan="3">
                                 <asp:TextBox ID="TextBox23" runat="server" Width="281px"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td style="width: 97px">
                                <asp:Label ID="Label29" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>" Width="122px"></asp:Label></td>
                            <td style="width: 131px">
                                <asp:TextBox ID="TextBox24" runat="server" Width="130px"></asp:TextBox></td>
                            <td style="width: 75px">
                            </td>
                            <td>
                            </td>
                        </tr>
                         <tr>
                            <td style="width: 97px">
                                <asp:Label ID="Label30" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIntegralChange %>" Width="123px"></asp:Label></td>
                            <td style="width: 131px">
                                <asp:TextBox ID="TextBox25" runat="server" Width="130px"></asp:TextBox></td>
                            <td style="width: 75px">
                                <asp:Label ID="Label32" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblModifyDate %>" Width="87px"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TextBox26" runat="server" Width="130px"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td style="width: 97px">
                            </td>
                            <td style="width: 131px">
                            </td>
                            <td style="width: 75px">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                  </fieldset>  
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; width: 50%">
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; height: 27px;" colspan="2">
                    <asp:ListBox ID="ListBox1" runat="server" Height="27px" Width="778px"></asp:ListBox></td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; width: 50%">
                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:99%;">
                        <legend style="text-align: left">
                        <asp:Label ID="Label34" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblParticular %>"></asp:Label>
                        </legend>
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                        <tr>
                            <td style="width:50%" align="center">
                                <asp:Button ID="Button2" runat="server" Text="<%$ Resources:BaseInfo,Associator_btnExpenditureSurvey %>" /></td>
                            <td align="center">
                                <asp:Button ID="Button1" runat="server" Text="<%$ Resources:BaseInfo,Associator_btnChargeRecord %>" /></td>
                        </tr>
                    </table>
                 </fieldset>       
                </td>
                <td>
                 <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:99%;">
                        <legend style="text-align: left">
                        <asp:Label ID="Label35" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,btn_lblPrint %>"></asp:Label>
                        </legend>
                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                        <tr>
                            <td style="width:16%">
                                <asp:Button ID="Button3" runat="server" Text="<%$ Resources:BaseInfo,Associator_btnExpenditureSurvey %>" /></td>
                            <td style="width: 78px">
                                <asp:Button ID="Button4" runat="server" Text="<%$ Resources:BaseInfo,Associator_btnChargeRecordRPT %>" /></td>
                                <td style="width: 103px">
                                    <asp:TextBox ID="TextBox27" runat="server" Width="89px"></asp:TextBox></td>
                                 <td style="width: 103px"></td>
                                  <td>
                                      <asp:TextBox ID="TextBox28" runat="server" Width="89px"></asp:TextBox></td>
                        </tr>
                    </table>
                   </fieldset>     
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; width: 50%">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 573px">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
