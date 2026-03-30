<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardLevelInt1.aspx.cs" Inherits="Associator_CardLevelInt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
	        addTabTool(document.getElementById("Associator_lblLeveLenactment").value + ",Associator/CardLevelInt.aspx");
	        loadTitle();
	    }
	</script>
</head>
<body style="margin-top:0; margin-left:0" onload="Load();">
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
            <%= (String)GetGlobalResourceObject("BaseInfo", "Associator_lblLeveLenactment")%></td>
            </tr>
        </table>
    <table width="770" height="466" border="0" cellpadding="0" cellspacing="0" style="width: 100%">
  <tr>
    <td align="left" valign="top" style="text-align: center" class="tdBackColor"><table width="730" height="400" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td height="30" colspan="2" align="center" valign="middle">
            </td>
        </tr>
      <tr>
        <td width="322" height="168" align="center" valign="top"><table width="322" height="168" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td height="24" colspan="2" align="center" valign="middle">
                <asp:Literal ID="lblLevelMes" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblLevelMes %>"></asp:Literal></td>
            </tr>
          <tr>
            <td width="120" align="right" valign="middle" style="height: 24px">
                <asp:Literal ID="lblCardLevel" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardLevel %>"></asp:Literal></td>
            <td align="left" style="height: 24px">&nbsp;<asp:TextBox ID="txtCardClassId" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="24" align="right" valign="middle">
                <asp:Literal ID="lblCardLevelName" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardLevelName %>"></asp:Literal></td>
            <td height="24" align="left">&nbsp;<asp:TextBox ID="txtCardClassNm" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="24" align="right" valign="middle">
                <asp:Literal ID="lblIntegral" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_Integral %>"></asp:Literal></td>
            <td height="24" align="left">&nbsp;<asp:TextBox ID="txtBonusPer" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="24" align="right" valign="middle">
                <asp:Literal ID="lblNewCardCharge" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_NewCardCharge %>"></asp:Literal></td>
            <td height="24" align="left">&nbsp;<asp:TextBox ID="txtNewCharge" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="24" align="right" valign="middle">
                <asp:Literal ID="lblLoseCard" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_LoseCard %>"></asp:Literal></td>
            <td height="24" align="left">&nbsp;<asp:TextBox ID="txtLostCharge" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="24" align="right" valign="middle">
                <asp:Literal ID="lblBadCard" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_BadCard %>"></asp:Literal></td>
            <td height="24" align="left">&nbsp;<asp:TextBox ID="txtDemageCharge" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
        </table></td>
        <td width="408" rowspan="3" valign="top"><table width="408" height="370" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td height="125" align="left" valign="top"><table width="400" height="125" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td height="25" align="center" valign="middle">
                    <asp:Literal ID="lblInvalidationRule" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblInvalidationRule %>"></asp:Literal></td>
              </tr>
              <tr>
                <td height="100" align="center" valign="top"><table width="400" height="100" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="230" height="25" align="right" valign="middle">
                        <asp:Literal ID="lblHolding" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblHolding %>"></asp:Literal></td>
                    <td width="170" height="25" align="left" valign="middle">&nbsp;<asp:TextBox ID="txtInvVal" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td height="25" align="right" valign="middle">
                        <asp:Literal ID="lblStat" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblStat %>"></asp:Literal></td>
                    <td height="25" align="left" valign="middle">&nbsp;<asp:TextBox ID="txtInvMth" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td height="25" align="right" valign="middle">
                        <asp:Literal ID="lblUnder" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblUnder %>"></asp:Literal></td>
                    <td height="25" align="left" valign="middle">&nbsp;<asp:TextBox ID="txtInvWarnVal" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td height="25" align="right" valign="middle">
                        <asp:Literal ID="lblAdmonition" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAdmonition %>"></asp:Literal></td>
                    <td height="25" align="left" valign="middle">&nbsp;<asp:TextBox ID="txtInvWarnMth" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                </table></td>
              </tr>
            </table></td>
          </tr>
          <tr>
            <td height="145" align="right" valign="top"><table width="400" height="145" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td height="25" align="center" valign="middle">
                    <asp:Literal ID="lblDemotion" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblDemotion %>"></asp:Literal></td>
              </tr>
              <tr>
                <td height="120" align="center" valign="top"><table width="400" height="120" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="185" align="right" valign="middle" style="height: 28px">
                        <asp:Literal ID="lblBaseLine" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblBaseLine %>"></asp:Literal></td>
                    <td width="215" align="left" valign="middle" style="height: 28px">&nbsp;<asp:TextBox ID="txtDnVal" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td height="24" align="right" valign="middle">
                        <asp:Literal ID="lblStatMonth" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblStatMonth %>"></asp:Literal></td>
                    <td align="left" valign="middle">&nbsp;<asp:TextBox ID="txtDnMth" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td align="right" valign="middle" style="height: 24px">
                        <asp:Literal ID="lblDemotionLevel" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_lblDemotionLevel %>"></asp:Literal></td>
                    <td align="left" valign="middle" style="height: 24px">&nbsp;<asp:DropDownList ID="cmdDnId" runat="server" CssClass="ipt160px" Width="160px" Enabled="False">
                        </asp:DropDownList></td>
                  </tr>
                  <tr>
                    <td align="right" valign="middle" style="height: 24px">
                        <asp:Literal ID="lblAdmonitionMes" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_lblAdmonitionMes %>"></asp:Literal></td>
                    <td align="left" valign="middle" style="height: 24px">&nbsp;<asp:TextBox ID="txtDnWarnVal" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td height="24" align="right" valign="middle">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAdmonition %>"></asp:Literal>&nbsp;</td>
                    <td align="left" valign="middle">&nbsp;<asp:TextBox ID="txtDnWarnMth" runat="server" CssClass="ipt160px" MaxLength="32"
                            ReadOnly="True" Width="156px"></asp:TextBox></td>
                  </tr>
                </table></td>
              </tr>
            </table></td>
          </tr>
          <tr>
            <td height="100" align="right" valign="top"><table width="400" height="84" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td align="center" valign="middle" style="height: 21px">
                    <asp:Literal ID="lblUpgrade" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblUpgrade %>"></asp:Literal></td>
              </tr>
              <tr>
                <td height="63" align="center" valign="top"><table width="400" height="63" border="0" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="170" align="right" valign="middle" style="height: 28px">
                        <asp:Literal ID="lblUpgradeM" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblUpgradeM %>"></asp:Literal></td>
                    <td width="230" align="left" valign="middle" style="height: 28px">&nbsp;<asp:TextBox ID="txtUpVal" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td align="right" valign="middle">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblStatMonth %>"></asp:Literal></td>
                    <td align="left" valign="middle">&nbsp;<asp:TextBox ID="txtUpMth" runat="server" CssClass="ipt160px" MaxLength="32"
                            Width="156px" ReadOnly="True"></asp:TextBox></td>
                  </tr>
                  <tr>
                    <td align="right" valign="middle" style="height: 15px">
                        <asp:Literal ID="lblUpgradeLevel" runat="server" EnableViewState="False" Text="<%$ Resources:BaseInfo,Associator_lblUpgradeLevel %>"></asp:Literal></td>
                    <td align="left" valign="middle" style="height: 15px">&nbsp;<asp:DropDownList ID="cmdUpId" runat="server" CssClass="ipt160px" Width="160px" Enabled="False">
                        </asp:DropDownList></td>
                  </tr>
                </table></td>
              </tr>
              
            </table></td>
          </tr>
        </table>
            <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Width="70px" /></td>
      </tr>
      <tr>
        <td width="322" height="126"><table width="322" height="126" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td height="21" align="center" valign="middle">
                <asp:Literal ID="lblCardRule" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardRule %>"></asp:Literal></td>
          </tr>
          <tr>
            <td height="105"><table width="322" height="105" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td height="21" align="right" style="width: 11px">&nbsp;</td>
                <td><asp:CheckBox ID="chkCardInvalidation" runat="server" Text="<%$ Resources:BaseInfo,Associator_chkCardInvalidation %>" AutoPostBack="True" OnCheckedChanged="chkCardInvalidation_CheckedChanged" /></td>
              </tr>
              <tr>
                <td height="21" align="right" style="width: 11px">&nbsp;</td>
                <td><asp:CheckBox ID="chkCardDegrade" runat="server" Text="<%$ Resources:BaseInfo,Associator_chkCardDegrade %>" AutoPostBack="True" OnCheckedChanged="chkCardDegrade_CheckedChanged" /></td>
              </tr>
              <tr>
                <td align="right" style="width: 11px; height: 21px;">&nbsp;</td>
                <td style="height: 21px"><asp:CheckBox ID="chkCardUpgrade" runat="server" Text="<%$ Resources:BaseInfo,Associator_chkCardUpgrade %>" AutoPostBack="True" OnCheckedChanged="chkCardUpgrade_CheckedChanged" /></td>
              </tr>
              <tr>
                <td align="right" style="height: 21px; width: 11px;">&nbsp;</td>
                <td style="height: 21px"><asp:RadioButton ID="rdoConsumption" runat="server" Text="<%$ Resources:BaseInfo,Associator_rdoConsumption %>" GroupName="rdo" Checked="True" /></td>
              </tr>
              <tr>
                <td align="right" style="height: 21px; width: 11px;">&nbsp;</td>
                <td style="height: 21px"><asp:RadioButton ID="rdoMoney" runat="server" Text="<%$ Resources:BaseInfo,Associator_rdoMoney %>" GroupName="rdo" OnCheckedChanged="rdoMoney_CheckedChanged" /></td>
              </tr>
            </table></td>
          </tr>
          
        </table></td>
        </tr>
      <tr>
        <td width="322" height="76" align="center" valign="top"><table width="322" height="76" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td align="center" valign="middle" style="height: 21px">
                <asp:Literal ID="lblRule" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblRule %>"></asp:Literal></td>
          </tr>
          <tr>
            <td height="55" align="left" valign="top"><table width="322" height="30" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td width="40" align="right" valign="middle" style="height: 36px">&nbsp;</td>
                <td width="172" align="center" valign="middle" style="height: 36px">
                    <asp:CheckBox ID="chkInvalidationYear" runat="server" Text="<%$ Resources:BaseInfo,Associator_chkInvalidationYear %>" Width="176px" AutoPostBack="True" OnCheckedChanged="chkInvalidationYear_CheckedChanged" /></td>
                <td width="110" align="left" valign="middle" style="height: 36px">&nbsp;<asp:DropDownList ID="cmbExpireYear" runat="server" CssClass="ipt160px" Width="104px" Enabled="False">
                    </asp:DropDownList></td>
              </tr>
            </table></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
</table>
    </div>
        <asp:HiddenField ID="Associator_lblLeveLenactment" runat="server" Value="<%$ Resources:BaseInfo,Associator_lblLeveLenactment %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
