<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnRegCard.aspx.cs" Inherits="Associator_UnRegCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_NotEnrolAssociator")%></title>
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
	        addTabTool(document.getElementById("hidNotEnrolAssociator").value + ",Associator/UnRegCard.aspx");
	        loadTitle();
	    }
	</script>
        <style type="text/css">
            .style1
            {
                height: 19px;
            }
        </style>
</head>
<body style="margin-top:0; margin-left:0" onload="Load();">
    <form id="form1" runat="server">
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "Associator_NotEnrolAssociator")%>
                </td>
            </tr>
        </table>
    <table width="100%" height="370" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="400" valign="top" style="text-align: right; background-color:#e1e0b2;"><table width="400" height="260" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td width="150" align="right" valign="middle" >
            &nbsp;</td>
        <td width="250" align="left" valign="middle" >&nbsp;</td>
      </tr>
      <tr>
        <td width="150" align="right" valign="middle" >
            <asp:Literal ID="lblAssociatorCard" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAssociatorCard %>"></asp:Literal></td>
        <td width="250" align="left" valign="middle" >&nbsp;<asp:TextBox ID="TextBox1" runat="server" CssClass="ipt160px" MaxLength="32"
                Width="156px"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right" valign="middle" style="height: 22px">
            <asp:Literal ID="lblCardLevel" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardLevel %>"></asp:Literal></td>
        <td align="left" valign="middle" style="height: 22px">&nbsp;<asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ipt160px" Width="142px">
            </asp:DropDownList></td>
      </tr>
      <tr>
        <td align="right" valign="middle">
            <asp:Literal ID="lblBargaining" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblBargaining %>"></asp:Literal></td>
        <td align="left" valign="middle">&nbsp;<asp:TextBox ID="TextBox3" runat="server" CssClass="ipt160px" MaxLength="32"
                Width="156px"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right" valign="middle">
            <asp:Literal ID="lblDate" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblDate %>"></asp:Literal></td>
        <td align="left" valign="middle">&nbsp;<asp:TextBox ID="TextBox4" runat="server" CssClass="ipt160px" MaxLength="32"
                Width="156px"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right" valign="middle">
            <asp:Literal ID="lblShop" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblShop %>"></asp:Literal></td>
        <td align="left" valign="middle">&nbsp;<asp:TextBox ID="TextBox5" runat="server" CssClass="ipt160px" MaxLength="32"
                Width="156px"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right" valign="middle">
            <asp:Literal ID="lblMesPutIn" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblMesPutIn %>"></asp:Literal></td>
        <td align="left" valign="middle">&nbsp;<asp:TextBox ID="TextBox6" runat="server" CssClass="ipt160px" MaxLength="32"
                Width="156px"></asp:TextBox></td>
      </tr>
    </table></td>
    <td width="249" align="left" valign="top" class="tdBackColor"></td>
  </tr>
</table>
        <asp:HiddenField ID="hidNotEnrolAssociator" runat="server" Value="<%$ Resources:BaseInfo,Associator_NotEnrolAssociator %>" />
    </div>
    </form>
</body>
</html>

