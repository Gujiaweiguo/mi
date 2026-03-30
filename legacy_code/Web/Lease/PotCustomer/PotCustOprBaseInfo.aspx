<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustOprBaseInfo.aspx.cs" Inherits="Lease_PotCustomer_PotCustOprBaseInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustManageSurvey")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script language="javascript">
//输入验证
        function InputValidator()
        {
             if(isInteger(document.all.txtShopNumber.value)==false)
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>')+"please input nummber.";
                document.all.txtShopNumber.focus();
                return false;
            }
            
             if(isDouble(document.all.txtAreaSalesRate.value)==false)
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>')+"please input nummber.";
                document.all.txtAreaSalesRate.focus();
                return false;
            }
             if(isDouble(document.all.txtBaseDiscount.value)==false)
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>')+"please input nummber.";
                document.all.txtBaseDiscount.focus();
                return false;
            }
            if(isInteger(document.all.txtplanShopNumber.value)==false)
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>')+"please input nummber.";
                document.all.txtplanShopNumber.focus();
                return false;
            }
        }
</script>
</head>
<body style="margin-top:0; margin-left:0" onload="loadTitle();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustManageSurvey")%>
                    </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 410px">
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 50%; height: 281px; text-align: right"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 255px; height: 231px">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 9px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 10px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 164px; text-align: center">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCreateUserID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_CustCode %>"
                                    Width="50px"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCreateUserID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"
                                    Width="63px"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"
                                    Width="58px"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 27px; text-align: right">
                                <asp:Label ID="lblBizMode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 27px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 27px">
                                <font color=red>
                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="cmb160px" Width="163px">
                                    </asp:DropDownList>&nbsp;</font></td>
                            <td class="tdBackColor" style="width: 5px; height: 27px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 28px; text-align: right">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_SetupShopArea %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 28px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 28px">
                                <font color=red>
                                    <asp:TextBox ID="txtOperateAreas" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></font></td>
                            <td class="tdBackColor" style="width: 5px; height: 28px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_ShopNumber %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <font color=red>
                                    <asp:TextBox ID="txtShopNumber" runat="server" CssClass="ipt160px" MaxLength="9"></asp:TextBox></font></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 160px; height: 22px;text-align: right">
                                <asp:Label ID="lblRentalPrice" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_AreaSalesRate %>"></asp:Label>&nbsp;&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtAreaSalesRate" runat="server" CssClass="ipt160px" MaxLength="10"></asp:TextBox><font color=red></font></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 2px">
                                <asp:Label ID="lblRentArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BaseDiscount %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 10px; text-align: center">
                                <asp:TextBox ID="txtBaseDiscount" runat="server" CssClass="ipt160px" MaxLength="4"></asp:TextBox><font color=red></font></td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PromoteArea %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPromoteArea" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PromoteCost %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPromoteCost" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox><font color=red></font></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        
                    </table>
                </td>
                <td class="tdBackColor" style="width: 280px; height: 281px" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 280px; height: 231px">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 9px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 10px; text-align: right">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 10px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 10px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 164px; text-align: center">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 19px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 125px; height: 22px; text-align: right">
                                <asp:Label ID="lblShopStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_planArea %>"
                                    Width="90px"></asp:Label>&nbsp;&nbsp; &nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 22px; text-align: center;">
                                <asp:TextBox ID="txtplanArea" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox><font color=red></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblShopEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_planShopNumber %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 22px; text-align: center;">
                                <font color=red>
                               <asp:TextBox ID="txtplanShopNumber" runat="server" CssClass="ipt160px" Height="22px" Width="160px"
                                    ></asp:TextBox></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_planDate %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 22px; text-align: left;">
                               <font color=red>
                                <asp:TextBox ID="txtPlanDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 10px; text-align: right">
                                &nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 10px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 10px;">
                                <font color=red></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style=" height: 3px; text-align: right;">
                                <table border="0" cellpadding="0" cellspacing="0" style=" width:90%;">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 11px; text-align: right">
                                &nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 11px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 11px">
                               <font color=red></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 11px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px; text-align: center;">
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    OnClick="btnAdd_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>"  />&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                     OnClick="btnQuit_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>"
                                     /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 12px; text-align: right">
                                &nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 12px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 12px">
                                <font color=red></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 12px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                &nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 22px">
                                <font color=red></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                         <tr>
                            <td class="tdBackColor" style="width: 96px; height: 8px; text-align: right">
                                &nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 8px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 8px">
                                <font color=red></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right" valign="top">
                                &nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 22px" valign="top">
                                <font color=red></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 22px">
                                </td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="4" style="width: 100%; height: 49px; text-align: center">
                    <table style="width: 508px">
                        <tr>
                            <td style="text-align: right; width: 505px; height: 47px;">
                    
                             
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="3" style="height: 3px; text-align: right">
                </td>
            </tr>
        </table>
    
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
                    <asp:TextBox ID="txtUnitID" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="txtStore" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
        <asp:TextBox ID="txtHidValue" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
    </form>
</body>
</html>

