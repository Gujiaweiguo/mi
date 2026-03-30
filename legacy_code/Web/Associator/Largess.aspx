<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Largess.aspx.cs" Inherits="Associator_Largess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_chkExtend")%></title>
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
	        addTabTool(document.getElementById("Associator_chkExtend").value + ",Associator/Largess.aspx");
	        loadTitle();
	    }
	    
	    //输入验证
        function InputValidator(sForm)
        {   
            if(document.getElementById("txtStartDate").value > document.getElementById("txtEndDate").value)
            {
                parent.document.all.txtWroMessage.value =('<%= beginEndDate %>');
                return false;
            }
        } 
	</script>
	
    <style type="text/css">
        .style1
        {
            height: 10px;
        }
        .style2
        {
            height: 8px;
        }
        .style3
        {
            height: 20px;
        }
        .style4
        {
            height: 14px;
        }
        .style5
        {
            height: 15px;
        }
        .style6
        {
            height: 10px;
            width: 1px;
        }
        .style7
        {
            width: 1px;
        }
        .style8
        {
            height: 8px;
            width: 1px;
        }
        .style9
        {
            height: 20px;
            width: 1px;
        }
    </style>
	
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
                    <%= (String)GetGlobalResourceObject("BaseInfo", "Associator_chkExtend")%>
                </td>
            </tr>
        </table>
    <table width="100%" height="300" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td height="250" align="center" valign="top" class="tdBackColor"><table width="550" height="250" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td height="125" align="left" valign="bottom"><table width="550" height="120" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="150" height="30" align="right" valign="middle">
                </td>
            <td width="400" align="left" valign="middle">&nbsp;</td>
          </tr>
          <tr>
            <td height="30" align="right" valign="middle">
                <asp:Literal ID="lblBewrite" runat="server" Text="活动名称" EnableViewState="False"></asp:Literal></td>
            <td align="left" valign="middle">&nbsp;<asp:TextBox ID="txtActDesc" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
          <tr>
            <td align="right" valign="middle" class="style5">
                </td>
            <td align="left" valign="middle" class="style5"></td>
          </tr>
            <tr>
                <td align="right" style="height: 30px" valign="middle">
                    <asp:Literal ID="lblTime" runat="server" EnableViewState="False" 
                        Text="<%$ Resources:BaseInfo,Associator_lblTime %>"></asp:Literal>
                </td>
                <td align="left" style="height: 30px" valign="middle">
                    &nbsp;<asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" 
                        MaxLength="32" onclick="calendar()" Width="156px"></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" Text=" ￣ "></asp:Label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" MaxLength="32" 
                        onclick="calendar()" Width="156px"></asp:TextBox>
                </td>
            </tr>
          <tr>
            <td align="right" valign="middle" class="style4">
                </td>
            <td align="left" valign="middle" class="style4"></td>
          </tr>
            <tr>
                <td align="right" style="height: 28px" valign="middle">
                    <asp:Literal ID="lblSymbol" runat="server" EnableViewState="False" 
                        Text="<%$ Resources:BaseInfo,Associator_lblSymbol %>"></asp:Literal>
                </td>
                <td align="left" style="height: 28px" valign="middle">
                    &nbsp;<asp:DropDownList ID="cmbGiftID" runat="server" Width="160px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="height: 28px" valign="middle">
                    &nbsp;</td>
                <td align="left" style="height: 28px" valign="middle">
                    &nbsp;</td>
            </tr>
        </table></td>
      </tr>
      <tr>
        <td height="125" align="center" valign="middle"><table width="260" height="75" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td height="25" align="center" valign="middle">
                <asp:Literal ID="lblSelect" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblSelect %>"></asp:Literal></td>
          </tr>
            <tr>
                <td align="left" height="50">
                    <table border="0" cellpadding="0" cellspacing="0" height="50" width="260">
                        <tr>
                            <td align="right" class="style6" valign="middle">
                            </td>
                            <td align="left" class="style1" valign="middle" width="210">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style7" height="25" valign="middle">
                                &nbsp;</td>
                            <td align="left" valign="middle" width="210">
                                <asp:RadioButton ID="rdoEveryTime" runat="server" Checked="True" 
                                    GroupName="every" Text="<%$ Resources:BaseInfo,Associator_rdoEveryTime %>" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style8" valign="middle">
                            </td>
                            <td align="left" class="style2" valign="middle">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style7" height="25" valign="middle">
                                &nbsp;</td>
                            <td align="left" valign="middle">
                                <asp:RadioButton ID="rdoEveryDay" runat="server" GroupName="every" 
                                    Text="<%$ Resources:BaseInfo,Associator_rdoEveryDay %>" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="style9" valign="middle">
                            </td>
                            <td align="left" class="style3" valign="middle">
                            </td>
                        </tr>
                    </table>
                    &nbsp;
                    <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                        OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />
                </td>
            </tr>
            <tr>
                <td align="left" height="50">
                    &nbsp;</td>
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
