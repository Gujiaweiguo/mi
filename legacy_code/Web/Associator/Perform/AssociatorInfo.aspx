<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssociatorInfo.aspx.cs" Inherits="Associator_Perform_AssociatorInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_AssociatorMes")%></title>
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
                <td style="height: 24px; text-align: left; width: 275px;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_AssociatorMes %>"></asp:Label></td>
            <td style="height: 24px; width: 663px;" class="tdTopRightBackColor" align="left"></td>
            </tr>
             <tr class="tdBackColor" style="height:20px">
                <td colspan="2"></td>
            </tr>
            <tr class="tdBackColor">
                <td style="width: 275px; padding-left:20px; padding-right:20px">
                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:99%;">
                        <legend style="text-align: left">
                        <asp:Label ID="Label33" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Menu_AssociatorMes %>"></asp:Label>
                        </legend>
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                        <tr>
                            <td class="baseLable" style="width: 85px">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorCode %>" Width="42px"></asp:Label></td>
                            <td colspan="2">
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="width: 85px">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"></asp:Label></td>
                            <td colspan="2">
                                <asp:DropDownList ID="DropDownList1" runat="server">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                                <asp:TextBox ID="TextBox2" runat="server" Width="139px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="width: 85px">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorIdentity %>"></asp:Label></td>
                            <td colspan="2">
                                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="width: 85px">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblInitiateDate %>" Width="66px"></asp:Label></td>
                            <td colspan="2">
                                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                &nbsp;&nbsp;
                                <asp:Button ID="Button1" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorPwd %>" /></td>
                        </tr>
                    </table>
                 </fieldset>                   
                </td>
                <td style="vertical-align:top">
                   <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:73%;">
                        <legend style="text-align: left">
                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblContact %>"></asp:Label>
                        </legend>
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 159px">
                                <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingAddr %>"></asp:Label></td>
                            <td style="width: 97px">
                                <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblPostCode %>"></asp:Label></td>
                            <td style="width: 75px">
                            </td>
                             <td>
                                 <asp:Label ID="Label9" runat="server" Text="<%$ Resources:BaseInfo,Dept_lblTel %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 159px">
                                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></td>
                            <td style="width: 97px">
                                <asp:TextBox ID="TextBox8" runat="server" Width="86px"></asp:TextBox></td>
                            <td style="width: 75px;text-align:right; padding-right:3px">
                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblOfficeTel %>"></asp:Label></td>
                             <td>
                                 <asp:TextBox ID="TextBox11" runat="server" Width="134px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 159px">
                                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox></td>
                            <td style="width: 97px">
                                <asp:TextBox ID="TextBox9" runat="server" Width="85px"></asp:TextBox></td>
                            <td style="width: 75px;text-align:right; padding-right:3px">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorTel %>"></asp:Label></td>
                             <td>
                                 <asp:TextBox ID="TextBox12" runat="server" Width="134px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 159px">
                                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox></td>
                            <td style="width: 97px">
                                <asp:TextBox ID="TextBox10" runat="server" Width="85px"></asp:TextBox></td>
                            <td style="width: 75px;text-align:right; padding-right:3px">
                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorMobileTel %>"></asp:Label></td>
                             <td>
                                 <asp:TextBox ID="TextBox13" runat="server" Width="134px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 159px; padding-right:5px; text-align:right">
                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblEMail %>"></asp:Label></td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBox14" runat="server" Width="308px"></asp:TextBox></td>
                        </tr>
                    </table>
                </fieldset> 
                </td>  
             </tr>  
             <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; width: 275px">
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px;" colspan="2">
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                             <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid;">
                                    <legend style="text-align: left">
                                    <asp:Label ID="Label19" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblIntegralType %>"></asp:Label>
                                    </legend>
                               <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                   <tr>
                                       <td>
                                           <asp:RadioButton ID="RadioButton3" runat="server" Text="<%$ Resources:BaseInfo,Associator_OnceAvailability %>"/></td>
                                       <td>
                                           <asp:RadioButton ID="RadioButton4" runat="server" Text="<%$ Resources:BaseInfo,Associator_DayAvailability %>"/></td>
                                       <td>
                                           <asp:RadioButton ID="RadioButton5" runat="server" Text="<%$ Resources:BaseInfo,Associator_LongTime %>"/></td>
                                   </tr>
                               </table>
                </fieldset> 
                            </td>
                            <td style="width:10%; padding-right:3px" align="right" >
                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorOrigin %>"></asp:Label></td>
            <td>
                <asp:TextBox ID="TextBox20" runat="server"></asp:TextBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; width: 275px">
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px;" colspan="2">
                     <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid;">
                                    <legend style="text-align: left">
                                    <asp:Label ID="Label21" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblDetails %>"></asp:Label>
                                    </legend>
                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                            <tr>
                                <td class="baseLable" style="width: 85px">
                                    <asp:Label ID="Label22" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorBirthday %>"></asp:Label></td>
                                <td style="width: 160px">
                                    <asp:TextBox ID="TextBox21" runat="server" CssClass="ipt160px" Width="125px"></asp:TextBox></td>
                                <td style="width: 97px" class="baseLable">
                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorDistance %>"></asp:Label></td>
                                <td style="width: 220px"><asp:DropDownList ID="DropDownList9" runat="server" Width="51px">
                                </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="TextBox15" runat="server" CssClass="ipt160px" Width="64px"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="TextBox16" runat="server" CssClass="ipt160px" Width="64px"></asp:TextBox></td>
                                <td rowspan="6" style="vertical-align:top">
                                </td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 85px">
                                    <asp:Label ID="Label23" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorGender %>"></asp:Label></td>
                                <td style="width: 160px">
                                    <asp:DropDownList ID="DropDownList2" runat="server" Width="132px">
                                    </asp:DropDownList></td>
                                <td style="width: 97px" class="baseLable">
                                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorEarning %>"></asp:Label></td>
                                <td style="width: 220px"><asp:DropDownList ID="DropDownList10" runat="server" Width="51px">
                                </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="TextBox17" runat="server" CssClass="ipt160px" Width="64px"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="TextBox18" runat="server" CssClass="ipt160px" Width="64px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 85px">
                                    <asp:Label ID="Label24" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorFolk %>"></asp:Label></td>
                                <td style="width: 160px"><asp:DropDownList ID="DropDownList3" runat="server" Width="132px">
                                </asp:DropDownList></td>
                                <td style="width: 97px" class="baseLable">
                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorOccupation %>"></asp:Label></td>
                                <td style="width: 220px"><asp:DropDownList ID="DropDownList11" runat="server" Width="132px">
                                </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 85px">
                                    <asp:Label ID="Label25" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorNationality %>"></asp:Label></td>
                                <td style="width: 160px"><asp:DropDownList ID="DropDownList4" runat="server" Width="132px">
                                </asp:DropDownList></td>
                                <td style="width: 97px" class="baseLable">
                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorDuty %>"></asp:Label></td>
                                <td style="width: 220px"><asp:DropDownList ID="DropDownList12" runat="server" Width="132px">
                                </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 85px">
                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorMarriage %>"></asp:Label></td>
                                <td style="width: 160px"><asp:DropDownList ID="DropDownList5" runat="server" Width="132px">
                                </asp:DropDownList></td>
                                <td style="width: 97px" class="baseLable">
                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorVehicle %>"></asp:Label></td>
                                <td style="width: 220px"><asp:DropDownList ID="DropDownList13" runat="server" Width="132px">
                                </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 85px">
                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorCommemorate %>"></asp:Label></td>
                                <td style="width: 160px"><asp:DropDownList ID="DropDownList6" runat="server" Width="37px">
                                </asp:DropDownList>
                                    <asp:Label ID="Label13" runat="server" Text="/" Width="1px"></asp:Label>
                                    <asp:DropDownList ID="DropDownList7" runat="server" Width="37px">
                                    </asp:DropDownList></td>
                                <td style="width: 97px" class="baseLable">
                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorCustom %>"></asp:Label></td>
                                <td style="width: 220px"><asp:DropDownList ID="DropDownList14" runat="server" Width="132px">
                                </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 85px">
                                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorBringuUp %>"></asp:Label></td>
                                <td style="width: 160px"><asp:DropDownList ID="DropDownList8" runat="server" Width="132px">
                                </asp:DropDownList></td>
                                <td style="width: 97px" class="baseLable">
                                </td>
                                <td style="width: 220px">
                                </td>
                                <td>
                                    <asp:Button ID="Button2" runat="server" Text="导入图片" /></td>
                            </tr>
                            <tr>
                                <td style="width: 85px">
                                </td>
                                <td style="width: 160px" align="center">
                                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorInterest %>"></asp:Label></td>
                                <td style="width: 97px">
                                </td>
                                <td style="width: 220px">
                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorBeFondOf %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorLargess %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 85px">
                                </td>
                                <td style="width: 160px">
                                    <asp:DropDownList ID="DropDownList15" runat="server" Width="136px">
                                    </asp:DropDownList></td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DropDownList16" runat="server" Width="283px">
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="DropDownList17" runat="server" Width="183px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 85px">
                                </td>
                                <td style="width: 160px">
                                    <asp:DropDownList ID="DropDownList18" runat="server" Width="136px">
                                    </asp:DropDownList></td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DropDownList19" runat="server" Width="283px">
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="DropDownList20" runat="server" Width="183px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="width: 85px">
                                </td>
                                <td style="width: 160px">
                                    <asp:DropDownList ID="DropDownList21" runat="server" Width="136px">
                                    </asp:DropDownList></td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DropDownList22" runat="server" Width="283px">
                                    </asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="DropDownList23" runat="server" Width="183px">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                    </fieldset>  
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px;" colspan="2">
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                        <tr>
                            <td class="baseLable" style="width: 89px">
                                <asp:Label ID="Label35" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorRemark %>"></asp:Label></td>
                            <td>
                                <asp:ListBox ID="ListBox1" runat="server" Height="26px" Width="662px"></asp:ListBox></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-right: 20px; padding-left: 20px; width: 275px">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
