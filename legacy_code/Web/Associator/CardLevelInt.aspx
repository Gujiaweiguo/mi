<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardLevelInt.aspx.cs" Inherits="Associator_CardLevelInt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_lblLeveLenactment")%></title>
    <link href="../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
		function Load()
	    {
	        addTabTool(document.getElementById("Associator_lblLeveLenactment").value + ",<%=url %>");
	        loadTitle();
	    }
	</script>
</head>
<body style="margin-top:0; margin-left:0" onload="Load();">
    <form id="form1" runat="server" style="width:100%">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
            <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%;">
            <tr>
                <td class="tdTopBackColor" style="width: 1%">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
            <%= (String)GetGlobalResourceObject("BaseInfo", "Associator_lblLeveLenactment")%></td>
            <td class="tdTopBackColor" style="width: 1%">
                    <img alt="" class="imageRightBack" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%" class="tdBackColor">
            <tr style="height:10px">
                <td>
                </td>
            </tr>
            <tr>
                <td style="padding-left:20px; padding-right:20px">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="labFastnessHire" runat="server" CssClass="labelStyle" Text="卡级别信息"></asp:Label>
                                    </legend>
                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr>
                                <td style="width:15%; text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblCardLevel" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardLevel %>"></asp:Literal></td>
                                <td style="width:25%">
                                    <asp:TextBox ID="txtCardClassId" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px"></asp:TextBox></td>
                                <td style="width:15%;text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblCardLevelName" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardLevelName %>"></asp:Literal></td>
                                <td style="width:45%">
                                    <asp:TextBox ID="txtCardClassNm" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                 <td style="width:15%; text-align:right; padding-right:5px">
                                     <asp:Literal ID="lblIntegral" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_Integral %>"></asp:Literal></td>
                                <td style="width:25%">
                                    <asp:TextBox ID="txtBonusPer" runat="server" CssClass="ipt160px" MaxLength="32" Width="156px"></asp:TextBox></td>
                                <td style="width:15%;text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblNewCardCharge" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_NewCardCharge %>"></asp:Literal></td>
                                <td style="width:45%">
                                    <asp:TextBox ID="txtNewCharge" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                 <td style="width:15%; text-align:right; padding-right:5px">
                                     <asp:Literal ID="lblLoseCard" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_LoseCard %>"></asp:Literal></td>
                                <td style="width:25%">
                                    <asp:TextBox ID="txtLostCharge" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px"></asp:TextBox></td>
                                <td style="width:15%;text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblBadCard" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_BadCard %>"></asp:Literal></td>
                                <td style="width:45%">
                                    <asp:TextBox ID="txtDemageCharge" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px"></asp:TextBox></td>
                            </tr>
                        </table>
                                </fieldset>
                </td>
            </tr>
            <tr style="height:10px">
                <td>
                </td>
            </tr>
            <tr>
                <td style="padding-left:20px; padding-right:20px">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="卡级别使用规则"></asp:Label>
                                    </legend>
                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr style="height:20px">
                                <td colspan="3" style="padding-left:20px">
                                    <asp:RadioButton ID="rdoConsumption" runat="server" Checked="True" GroupName="rdo"
                                        Text="<%$ Resources:BaseInfo,Associator_rdoConsumption %>" /><asp:RadioButton ID="rdoMoney"
                                            runat="server" GroupName="rdo" OnCheckedChanged="rdoMoney_CheckedChanged" Text="<%$ Resources:BaseInfo,Associator_rdoMoney %>" /></td>
                            </tr>
                        </table>
                                </fieldset>
                </td>
            </tr>
            <tr style="height:10px">
                <td>
                </td>
            </tr>
            <tr>
                <td style="padding-left:20px; padding-right:20px">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="期满规则"></asp:Label>
                                    </legend>
                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr style="height:20px">
                                <td colspan="3" style="padding-left:20px">
                                    <asp:CheckBox ID="chkInvalidationYear" runat="server" AutoPostBack="True" OnCheckedChanged="chkInvalidationYear_CheckedChanged"
                                        Text="<%$ Resources:BaseInfo,Associator_chkInvalidationYear %>" Width="176px" />
                                    <asp:DropDownList ID="cmbExpireYear" runat="server" CssClass="ipt160px" Enabled="False"
                                        Width="104px">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                                </fieldset>
                </td>
            </tr>
            <tr style="height:10px">
                <td>
                </td>
            </tr>
            <tr>
                <td style="padding-left:20px; padding-right:20px">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="失效规则"></asp:Label>
                                    </legend>
                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr style="height:25px">
                                <td colspan="4" style="height: 15px;padding-left: 10px">
                                     <asp:CheckBox ID="chkCardInvalidation" runat="server" AutoPostBack="True" OnCheckedChanged="chkCardInvalidation_CheckedChanged"
                                         Text="<%$ Resources:BaseInfo,Associator_chkCardInvalidation %>" /></td>
                            </tr>
                            <tr>
                                <td style="width:35%; text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblHolding" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblHolding %>"></asp:Literal></td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtInvVal" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px" Enabled="False"></asp:TextBox></td>
                                <td style="width:25%;text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblStat" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblStat %>"></asp:Literal></td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtInvMth" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px" Enabled="False"></asp:TextBox></td>
                            </tr>
                            <tr>
                                 <td style="width:35%; text-align:right; padding-right:5px">
                                     <asp:Literal ID="lblUnder" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblUnder %>"></asp:Literal></td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtInvWarnVal" runat="server" CssClass="ipt160px" MaxLength="32" Width="156px" Enabled="False"></asp:TextBox></td>
                                <td style="width:25%;text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblAdmonition" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAdmonition %>"></asp:Literal></td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtInvWarnMth" runat="server" CssClass="ipt160px" MaxLength="32" Width="156px" Enabled="False"></asp:TextBox></td>
                            </tr>
                        </table>
                                </fieldset>
                </td>
            </tr>
            <tr style="height:10px">
                <td>
                </td>
            </tr>
            <tr>
                <td style="padding-left:20px; padding-right:20px">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="降级规则"></asp:Label>
                                    </legend>
                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr style="height:25px">
                                <td colspan="4" style="padding-left: 10px">
                                     <asp:CheckBox ID="chkCardDegrade" runat="server" AutoPostBack="True" OnCheckedChanged="chkCardDegrade_CheckedChanged"
                                         Text="<%$ Resources:BaseInfo,Associator_chkCardDegrade %>" /></td>
                            </tr>
                            <tr>
                                <td style="width:30%; text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblBaseLine" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblBaseLine %>"></asp:Literal></td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtDnVal" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px" Enabled="False"></asp:TextBox></td>
                                <td style="width:20%;text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblDemotionLevel" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_lblDemotionLevel %>"></asp:Literal></td>
                                <td style="width:30%">
                                    <asp:DropDownList ID="cmdDnId" runat="server" CssClass="ipt160px" Enabled="False"
                                        Width="160px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                 <td style="width:30%; text-align:right; padding-right:5px">
                                     <asp:Literal ID="lblStatMonth" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblStatMonth %>"></asp:Literal></td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtDnMth" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px" Enabled="False"></asp:TextBox></td>
                                <td style="width:20%;text-align:right; padding-right:5px">
                                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAdmonition %>"></asp:Literal></td>
                                <td style="width:30%">
                                    <asp:TextBox ID="txtDnWarnMth" runat="server" CssClass="ipt160px" MaxLength="32" Width="156px" Enabled="False"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="padding-right: 5px; width: 30%; text-align: right">
                                    <asp:Literal ID="lblAdmonitionMes" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_lblAdmonitionMes %>"></asp:Literal></td>
                                <td style="width: 25%">
                                    <asp:TextBox ID="txtDnWarnVal" runat="server" CssClass="ipt160px" MaxLength="32" Width="156px" Enabled="False"></asp:TextBox></td>
                                <td style="padding-right: 5px; width: 10%; text-align: right">
                                </td>
                                <td style="width: 40%">
                                </td>
                            </tr>
                        </table>
                                </fieldset>
                </td>
            </tr>
            <tr style="height:10px">
                <td>
                </td>
            </tr>
            <tr>
                 <td style="padding-left:20px; padding-right:20px">
                    <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 100%;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="升级规则"></asp:Label>
                                    </legend>
                        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                            <tr style="height:25px">
                                <td colspan="4" style="padding-left: 10px; text-align: left">
                                     <asp:CheckBox ID="chkCardUpgrade" runat="server" AutoPostBack="True" OnCheckedChanged="chkCardUpgrade_CheckedChanged"
                                         Text="<%$ Resources:BaseInfo,Associator_chkCardUpgrade %>" /></td>
                            </tr>
                            <tr>
                                <td style="width:30%; text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblUpgradeM" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblUpgradeM %>"></asp:Literal></td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtUpVal" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px" Enabled="False"></asp:TextBox></td>
                                <td style="width:20%;text-align:right; padding-right:5px">
                                    <asp:Literal ID="lblUpgradeLevel" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_lblUpgradeLevel %>"></asp:Literal></td>
                                <td style="width:30%">
                                    <asp:DropDownList ID="cmdUpId" runat="server" CssClass="ipt160px" Enabled="False"
                                        Width="160px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                 <td style="width:30%; text-align:right; padding-right:5px">
                                     <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblStatMonth %>"></asp:Literal></td>
                                <td style="width:20%">
                                    <asp:TextBox ID="txtUpMth" runat="server" CssClass="ipt160px" MaxLength="32"
                                        Width="156px" Enabled="False"></asp:TextBox></td>
                                <td style="width:10%;text-align:right; padding-right:5px">
                                    </td>
                                <td style="width:40%">
                                    </td>
                            </tr>
                        </table>
                                </fieldset>
                </td>
            </tr>
            <tr >
                <td style="text-align:right; padding-right:40px">
                <asp:Button id="btnSave" onclick="btnSave_Click" runat="server" 
                        Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" CssClass="buttonSave"></asp:Button>
                    &nbsp;<asp:Button ID="btnQuit" runat="server" CssClass="buttonCancel" OnClick="btnQuit_Click"
                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                </td>
            </tr>
            <tr style="height:10px">
                <td>
                </td>
            </tr>
        </table>
    </div>
        <asp:HiddenField ID="Associator_lblLeveLenactment" runat="server" Value="<%$ Resources:BaseInfo,Associator_lblLeveLenactment %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
