<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LargessGive1.aspx.cs" Inherits="Associator_Perform_LargessGive" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_chkExtend")%></title>
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
	         addTabTool("<%=chkExtend %>,Associator/Perform/LargessGive.aspx");
	        loadTitle();
	    }
    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server" >
    <div>
        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="height: 24px; text-align: left; width: 540px;" class="tdTopRightBackColor" align="left">
                    <img class="imageLeftBack" src="" style="width: 7px"  />
                    <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Associator_chkExtend %>"></asp:Label></td>
                <td style="height: 24px; width: 663px;" class="tdTopRightBackColor" align="left"></td>
                <td style=" height: 24px;" class="tdTopRightBackColor" valign="top">
                    <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
            </tr>
            <tr class="tdBackColor">
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
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_UserID %>" Width="53px"></asp:Label></td>
                            <td style="width: 89px">
                                <asp:TextBox ID="txtUser" runat="server" Width="78px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width: 44px">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_ServiceDesk %>" Width="45px"></asp:Label></td>
                            <td style="width: 74px">
                                <asp:DropDownList ID="dropServiceDesk" runat="server" Width="79px">
                                </asp:DropDownList></td>
                            <td>
                                <asp:TextBox ID="txtDeskName" runat="server" Width="236px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                        </tr>
                    </table>
                </fieldset>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="height: 20px">
                </td>
                <td style="width: 663px; height: 20px;">
                </td>
                <td style="height: 20px">
                </td>
            </tr>
            <tr class="tdBackColor" style="padding-left:20px; padding-right:20px;">
                <td colspan="2">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:100%;">
                        <legend style="text-align: left">
                            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblActivity %>"></asp:Label>
                        </legend>
                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 71px">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblActivityPostil %>" Width="66px"></asp:Label></td>
                                <td style="width: 534px">
                                    <asp:DropDownList ID="dropActID" runat="server" Width="103px">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtActDesc" runat="server" Width="236px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 71px">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTime %>" Width="65px"></asp:Label></td>
                                <td style="width: 534px">
                                    <asp:TextBox ID="txtStartDate" runat="server" Width="78px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox>－<asp:TextBox
                                        ID="txtEndDate" runat="server" Width="78px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 71px; height: 16px">
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblSymbol %>" Width="63px"></asp:Label></td>
                                <td style="width: 534px; height: 16px">
                                    <asp:TextBox ID="txtGiftID" runat="server" Width="78px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox>
                                    <asp:TextBox ID="txtGiftDesc" runat="server" Width="236px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                                <td style="height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 71px; height: 16px">
                                </td>
                                <td style="width: 534px; height: 16px">
                                 <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:73%;" id="FIELDSET1" language="javascript" onclick="return FIELDSET1_onclick()">
                                        <legend style="text-align: left">
                                            <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblSelect %>"></asp:Label>
                                        </legend>
                                     <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                         <tr>
                                             <td style="width: 162px">
                                                 <asp:RadioButton ID="radGiftOption" runat="server" Text="<%$ Resources:BaseInfo,Associator_rdoEveryTime %>" Width="142px" GroupName="A" /></td>
                                             <td>
                                                 <asp:RadioButton ID="radGiftOptionOneDay" runat="server" Text="<%$ Resources:BaseInfo,Associator_rdoEveryDay %>" Width="131px" GroupName="A"/></td>
                                         </tr>
                                     </table>
                                  </fieldset>      
                                </td>
                                <td style="height: 16px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 71px; height: 16px">
                                </td>
                                <td style="width: 534px; height: 16px">
                                </td>
                                <td style="height: 16px">
                                </td>
                            </tr>
                        </table>
                     </fieldset>   
                </td>
                <td>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="height: 20px">
                </td>
                <td style="width: 663px; height: 20px;">
                </td>
                <td style="height: 20px">
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
                                <asp:TextBox ID="txtCardID" runat="server" Width="161px" CssClass="ipt160px"></asp:TextBox></td>
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
                                            <asp:RadioButton ID="RadioButton3" runat="server" Text="<%$ Resources:BaseInfo,Associator_radCardMac %>" Width="117px"/></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadioButton4" runat="server" Text="<%$ Resources:BaseInfo,Associator_radKeyboard %>" Width="114px"/></td>
                                    </tr>
                                </table>
                             </fieldset>           
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"></asp:Label></td>
                            <td style="width: 189px">
                                <asp:TextBox ID="txtMembName" runat="server" Width="161px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width: 69px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px;">
                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,User_lblIdentity %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="txtCertNum" runat="server" Width="161px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px">
                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorMobileTel %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="txt" runat="server" Width="161px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                            <td style="height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px">
                                <asp:Label ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblNumber %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="txtMobile" runat="server" Width="161px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                            <td style="height: 16px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 70px; height: 16px">
                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIssueDate %>"></asp:Label></td>
                            <td style="width: 189px; height: 16px">
                                <asp:TextBox ID="txtNum" runat="server" Width="161px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width: 69px; height: 16px">
                            </td>
                            <td style="height: 16px"><asp:Button ID="Button2" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIssueOK %>" /></td>
                        </tr>
                    </table>
                 </fieldset>                       
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
