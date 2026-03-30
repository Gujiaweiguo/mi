<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GiftStock.aspx.cs" Inherits="Associator_GiftStock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "TMunu_GiftStock")%></title>
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
    <script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
		function Load()
	    {
	        addTabTool("<%=tMunu_GiftStock%>,Associator/GiftStockQuery.aspx");
	        loadTitle();
	    }
	</script>
	
</head>
<body  style="margin-top:0; margin-left:0" onload="Load();">
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
                    <%= (String)GetGlobalResourceObject("BaseInfo", "TMunu_GiftStock")%></td>
            </tr>
        </table>
    <table width="100%" height="300" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td height="250" align="center" valign="top" class="tdBackColor"><table width="550" height="250" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td height="125" align="left" valign="bottom"><table width="550" height="120" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="150" height="30" align="right" valign="middle">
                <asp:Label ID="Label1" runat="server" Text="赠品"></asp:Label></td>
            <td width="400" align="left" valign="middle">&nbsp;<asp:DropDownList ID="cmbGiftID" runat="server" Width="160px">
            </asp:DropDownList></td>
          </tr>
          <tr>
            <td height="30" align="right" valign="middle">
                <asp:Label ID="Label2" runat="server" Text="服务台"></asp:Label></td>
            <td align="left" valign="middle">
                &nbsp;<asp:DropDownList ID="cmbStockID" runat="server" Width="160px">
            </asp:DropDownList></td>
          </tr>
          <tr>
            <td align="right" valign="middle" style="height: 30px">
                <asp:Label ID="Label3" runat="server" Text="数量"></asp:Label></td>
            <td align="left" valign="middle" style="height: 30px">&nbsp;<asp:TextBox ID="txtStockCnt" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px" ></asp:TextBox></td>
          </tr>
          <tr>
            <td align="right" valign="middle" style="height: 28px">
                <asp:Label ID="Label4" runat="server" Text="价格"></asp:Label></td>
            <td align="left" valign="middle" style="height: 28px">&nbsp;<asp:TextBox
                        ID="txtRefPrice" runat="server" CssClass="ipt160px" MaxLength="32" Width="156px"></asp:TextBox></td>
          </tr>
        </table></td>
      </tr>
      <tr>
        <td height="125" align="center" valign="middle"><table width="260" height="75" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td height="25" align="center" valign="middle">
                </td>
          </tr>
          <tr>
            <td height="50"><table width="260" height="50" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td width="50" height="25" align="right" valign="middle">&nbsp;</td>
                <td width="210" align="left" valign="middle">
                    </td>
              </tr>
              <tr>
                <td height="25" align="right" valign="middle">&nbsp;</td>
                <td align="left" valign="middle"></td>
              </tr>
            </table>
                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" /></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
</table>
<asp:HiddenField ID="Associator_chkExtend" runat="server" Value="<%$ Resources:BaseInfo,Associator_chkExtend %>" />
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
