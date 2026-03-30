<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddAssociatorT.aspx.cs" Inherits="Associator_AddAssociatorT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustTitle") %></title>
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
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        var str= document.getElementById("PotCustomer_Basic").value + ",Lease/PotCustomer/PotCustomerNew.aspx~" + 
	        document.getElementById("PotCustomer_ClientCard").value + ",Lease/PotCustomer/CustLicense.aspx~"+ document.getElementById("Hidden_Shop").value + 
	        ",Lease/PotCustomer/PotShopNew.aspx~" + document.getElementById("PotShop_lblPalaverNode").value +",Lease/PotCustomer/Palaver.aspx";
	        addTabTool(str);
	        loadTitle();
	    }
    </script>
	
</head>
<body style="margin-top:0; margin-left:0" onload="Load()">

    <form id="form1" runat="server">

    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustTitle") %>
                </td>
            </tr>
        </table>

        <table id="TABLE1" border="0" cellpadding="0" cellspacing="0" style="height: 405px; width:100%; text-align: center;">
            <tr>
                <td class="tdBackColor" colspan="3" style="height: 323px; text-align: center; width: 988px;" valign="top">
                    <table width="785" height="300" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                    <td style="height:15px"> 
                    </td>
                    </tr>
  <tr>
    <td style="height: 196px"><table width="785" height="160" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td width="300" align="right" valign="bottom" style="height: 211px; vertical-align: top;"><table height="150" border="0" cellpadding="0" cellspacing="0" style="width: 285px">
          <tr>
            <td width="110" align="right">
                <asp:Label ID="lblAssociatorCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorBirthday %>"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtTel" runat="server" CssClass="ipt160px" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;</td>
          </tr>
          <tr>
            <td align="right">
                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorGender %>"></asp:Label></td>
            <td style="width: 173px">
                <asp:TextBox ID="TextBox1" runat="server" CssClass="ipt160px" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;</td>
          </tr>
          <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorFolk %>"></asp:Label></td>
            <td style="width: 173px">
                <asp:TextBox ID="TextBox2" runat="server" CssClass="ipt160px" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;</td>
          </tr>
          <tr>
            <td align="right" style="height: 16px">
                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorNationality %>"></asp:Label></td>
            <td style="height: 16px; width: 173px;">
                <asp:TextBox ID="TextBox3" runat="server" CssClass="ipt160px" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;</td>
          </tr>
          <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorMarriage %>"></asp:Label></td>
            <td style="width: 173px">
                <asp:TextBox ID="TextBox4" runat="server" CssClass="ipt160px" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;</td>
          </tr>
          <tr>
            <td align="right">
                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorCommemorate %>"></asp:Label></td>
            <td style="width: 173px">
                <asp:TextBox ID="TextBox5" runat="server" CssClass="ipt160px" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;</td>
          </tr>
          <tr>
            <td align="right" style="height: 15px">
                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorBringuUp %>"></asp:Label></td>
            <td style="height: 15px; width: 173px;">
                <asp:TextBox ID="TextBox6" runat="server" CssClass="ipt160px" MaxLength="16" Width="160px"></asp:TextBox>&nbsp;</td>
          </tr>
        </table></td>
        <td align="right" valign="top" style="width: 286px; height: 211px; text-align: left"><table height="140" border="0" cellpadding="0" cellspacing="0" style="width: 289px">
          <tr>
            <td width="80" align="right">
                <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorDistance %>"></asp:Label></td>
            <td width="200">&nbsp;<asp:TextBox ID="TextBox7" runat="server" CssClass="ipt160px" MaxLength="16"
                    Width="160px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="right">
                <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorEarning %>"></asp:Label></td>
            <td>&nbsp;<asp:TextBox ID="TextBox8" runat="server" CssClass="ipt160px" MaxLength="16"
                    Width="160px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="right">
                <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorOccupation %>"></asp:Label></td>
            <td>&nbsp;<asp:TextBox ID="TextBox9" runat="server" CssClass="ipt160px" MaxLength="16"
                    Width="160px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="right">
                <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorDuty %>"></asp:Label>&nbsp;</td>
            <td>&nbsp;<asp:TextBox ID="TextBox10" runat="server" CssClass="ipt160px" MaxLength="16"
                    Width="160px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="right" style="height: 28px">
                <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorVehicle %>"></asp:Label></td>
            <td style="height: 28px">&nbsp;<asp:TextBox ID="TextBox11" runat="server" CssClass="ipt160px" MaxLength="16"
                    Width="160px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="right">
                <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorCustom %>"></asp:Label></td>
            <td>&nbsp;<asp:TextBox ID="TextBox12" runat="server" CssClass="ipt160px" MaxLength="16"
                    Width="160px"></asp:TextBox></td>
          </tr>
        </table></td>
        <td width="173" align="left" valign="middle" style="height: 211px"><table width="173" height="160" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td height="135" valign="bottom">
                <asp:Image ID="imgPeople" runat="server" Width="117px" /></td>
          </tr>
          <tr>
          <td style="height:10px">
          </td>
          </tr>
          <tr>
            <td style="padding-left:13px; text-align: center;" height="25" align="left" valign="middle">
                <asp:Button ID="Button1" runat="server" Text="Button" /></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
  <tr>
  <td style="height:20px">
  </td>
  </tr>
  <tr>
    <td height="100" align="center" valign="middle" style="text-align: center"><table height="100" border="0" cellpadding="0" cellspacing="0" style="width: 721px">
      <tr>
        <td width="250" align="right" valign="middle"><table width="230" height="100" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td align="center" valign="middle">
                <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorInterest %>"></asp:Label></td>
          </tr>
          <tr>
            <td align="center" valign="top">&nbsp;<asp:TextBox ID="TextBox13" runat="server" CssClass="ipt160px" MaxLength="16"
                    Width="160px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="center" valign="top">&nbsp;</td>
          </tr>
          <tr>
            <td align="center" valign="top">&nbsp;</td>
          </tr>
        </table></td>
        <td width="240" align="right" valign="middle"><table width="230" height="100" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td align="center" valign="middle" style="width: 230px">
                <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorBeFondOf %>"></asp:Label></td>
          </tr>
          <tr>
            <td align="center" valign="top" style="width: 230px">&nbsp;<asp:TextBox ID="TextBox14" runat="server" CssClass="ipt160px" MaxLength="16"
                    Width="160px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="center" valign="top" style="width: 230px">&nbsp;</td>
          </tr>
          <tr>
            <td align="center" valign="top" style="width: 230px">&nbsp;</td>
          </tr>
        </table></td>
        <td align="left" valign="middle"><table width="230" height="100" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td align="center" valign="middle">
                <asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorLargess %>"></asp:Label></td>
          </tr>
          <tr>
            <td align="center" valign="top">
                <asp:TextBox ID="TextBox15" runat="server" CssClass="ipt160px" MaxLength="16" Width="160px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="center" valign="top">&nbsp;</td>
          </tr>
          <tr>
            <td align="center" valign="top">&nbsp;</td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td height="40" align="center" valign="top"><table width="750" height="38" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td align="right" valign="middle" style="width: 34px">
            <asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorRemark %>"></asp:Label></td>
        <td align="left" valign="middle">&nbsp;<asp:TextBox ID="TextBox16" runat="server" CssClass="ipt160px" MaxLength="16"
                Width="662px"></asp:TextBox></td>
      </tr>
    </table></td>
  </tr>
</table>
                  </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
