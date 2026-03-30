<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyCard.aspx.cs" Inherits="Associator_Perform_ModifyCard" ResponseEncoding="gb2312" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
      <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_lblUpdateCardInfo")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        <!--
        table.tbIntegral tr{ height:20px; }
        
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
	<script type="text/javascript"  src="../../JavaScript/Calendar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool("<%=lblUpdateCardInfo %>,<%=url %>");
	        loadTitle();
	    }
	    
	    function checkISNull()
	    {
	         if(isEmpty(document.all.txtCardID.value))
            {
                parent.document.all.txtWroMessage.value =('<%= enterCardID %>');
                document.all.txtCardID.focus();
                return false;
            }
	    }
    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <div>
         <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                 <td style="height: 24px; text-align: left; width: 854px;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblUpdateCardInfo %>"></asp:Label></td>
            <td style="height: 24px; width: 663px;" class="tdTopRightBackColor" align="left"></td>
            <td style=" height: 24px;" class="tdTopRightBackColor" valign="top">
                <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
            </tr>
            <tr class="tdBackColor" style="height:20px">
                <td style="width: 854px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
             <tr class="tdBackColor">
                 <td colspan="2" style="padding-left:10px; padding-right:10px">
                     <table border="0" cellpadding="0" cellspacing="0" style="width: 117%">
                         <tr>
                             <td style="height: 1px; background-color: #738495">
                             </td>
                         </tr>
                         <tr>
                             <td style="height: 1px; background-color: #ffffff">
                             </td>
                         </tr>
                     </table>
                 </td>
                 <td>
                 </td>
             </tr>
            <tr class="tdBackColor">
                <td style="width: 854px; height: 21px;">
                </td>
                <td style="height: 21px">
                </td>
                <td style="height: 21px">
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="padding-left:30px; padding-right:30px" colspan="2">
                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbIntegral">
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                    <asp:Label ID="Label1" runat="server" Text="会员卡号"></asp:Label></td>
                                <td style="width: 156px">
                                    <asp:TextBox ID="txtCardID" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                        Width="136px"></asp:TextBox></td>
                                <td style="width: 59px">
                                </td>
                                <td rowspan="1">
                                </td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorCode %>"></asp:Label></td>
                                <td style="width: 156px">
                                    <asp:TextBox ID="txtMembCode" runat="server" Width="136px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                                <td style="width: 59px">
                                    </td>
                                <td rowspan="8">
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:89%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_rabCardStuta %>"></asp:Label>
                                    </legend>
                                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tbCard">
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="optNew" runat="server" Text="<%$ Resources:BaseInfo,Associator_rabNewCard %>" Checked="True" GroupName="NCard" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="optRenew" runat="server" Text="<%$ Resources:BaseInfo,Associator_rabUpdateCard %>" GroupName="NCard"/></td>
                                        </tr>
                                        <tr>
                                            <td>
                                            <hr>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="optLost" runat="server" Text="<%$ Resources:BaseInfo,Associator_rabLose %>" GroupName="NCard"/></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="optDemage" runat="server" Text="<%$ Resources:BaseInfo,Associator_rabDamage %>" GroupName="NCard"/></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="optInvalidate" runat="server" Text="<%$ Resources:BaseInfo,Associator_rabInvalid %>" GroupName="NCard"/></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="optDowngrade" runat="server" Text="<%$ Resources:BaseInfo,Associator_rabDegrade %>" GroupName="NCard"/></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px">
                                                <asp:RadioButton ID="optUpgrade" runat="server" Text="<%$ Resources:BaseInfo,Associator_rabRise %>" GroupName="NCard"/></td>
                                        </tr>
                                        <tr>
                                            <td>
                                             <hr>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="optReturn" runat="server" Text="<%$ Resources:BaseInfo,Associator_rabBackCard %>" GroupName="NCard"/></td>
                                        </tr>
                                    </table>
                                    </fieldset>   
                                </td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"></asp:Label></td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtMembName" runat="server" Width="136px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIssueDate %>"></asp:Label></td>
                                <td colspan="2">
                                <asp:TextBox ID="txtDate" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Width="137px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblOtherPassPort %>"></asp:Label></td>
                                <td style="width: 156px">
                                    <asp:TextBox ID="txtPassPort" runat="server" Width="135px" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                                <td style="width: 59px">
                                </td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                    <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblCarDnature %>"></asp:Label></td>
                                <td colspan="2">
                                    <asp:RadioButton ID="radCustomer" runat="server" Checked="True" GroupName="B" Text="<%$ Resources:BaseInfo,Associator_lblCustomerCard %>"
                                        Width="58px" /><asp:RadioButton ID="radEmployee" runat="server" GroupName="B" Text="<%$ Resources:BaseInfo,Associator_lblEmployeeCard %>"
                                            Width="63px" /><asp:RadioButton ID="radOther" runat="server" GroupName="B" Text="<%$ Resources:BaseInfo,Dept_Other %>"
                                                Width="56px" /></td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardType %>"></asp:Label></td>
                                <td style="width: 156px">
                                    <asp:DropDownList ID="dropCardType" runat="server" Width="139px">
                                    </asp:DropDownList></td>
                                <td style="width: 59px" class="baseLable">
                                    </td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardLevel %>"></asp:Label></td>
                                <td style="width: 156px">
                                    <asp:DropDownList ID="dropCardLevel" runat="server" Width="137px">
                                    </asp:DropDownList></td>
                                <td class="baseLable" style="width: 59px">
                                </td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTermDate %>"></asp:Label></td>
                                <td style="width: 156px">
                                    <asp:TextBox ID="txtExpiredDate" runat="server" Width="135px" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                <td style="width: 59px" class="baseLable">
                                </td>
                            </tr>
                            <tr>
                                <td class="baseLable" style="width: 75px">
                                </td>
                                <td align="right" style="width: 156px">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="buttonSave" OnClick="btnSubmit_Click"
                                        Text="<%$ Resources:BaseInfo,User_btnOk %>" /><asp:Button ID="btnCancel" runat="server" CssClass="buttonClear"
                                            OnClick="btnCancel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
                                <td class="baseLable" style="width: 59px">
                                </td>
                                <td rowspan="1">
                                </td>
                            </tr>
                        </table>
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
                    &nbsp;</td>
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
    </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
