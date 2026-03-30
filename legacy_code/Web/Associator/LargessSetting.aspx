<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LargessSetting.aspx.cs" Inherits="Associator_LargessSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_Setup")%></title>
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
	        addTabTool(document.getElementById("Associator_Setup").value + ",Associator/LargessSetting.aspx");
	        loadTitle();
	                           document.getElementById("rdoYes").disabled  =false;
                   document.getElementById("rdoNo").disabled  ="true";
	    }
	    
	    function CheckedExByBonus()
	    {
	        if(document.getElementById("chkExByBonus").checked == true)   
              {   
                   document.getElementById("txtExByBonus").value=0;
                   document.getElementById("txtExByBonus").disabled=false;

              }   
              else   
              {   
                    document.getElementById("txtExByBonus").disabled=true;
              }   
	    }
	    
	    function CheckedExByReceipt()
	    {
	        if(document.getElementById("chkExByReceipt").checked == true)   
              {   

                   document.getElementById("txtExByReceipt").value=0;
                   document.getElementById("txtExByReceipt").disabled=false;
                   
              }   
              else   
              {   
                    document.getElementById("txtExByReceipt").disabled=true;
              }   
	    }
	    
	    	     //输入验证
        function InputValidator(sForm)
        {   
            if(document.getElementById("txtAnStartDate").value >= document.getElementById("txtEndDate").value)
            {
                parent.document.all.txtWroMessage.value =('<%= beginEndDate %>');
                return false;
            }
        } 
    </script>
    <style type="text/css">
        .style3
        {
            height: 29px;
        }
        .style4
        {
            width: 164px;
            height: 29px;
        }
        .style5
        {
            height: 15px;
        }
        .style7
        {
            width: 164px;
            height: 19px;
        }
        .style8
        {
            height: 19px;
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
                    <%= (String)GetGlobalResourceObject("BaseInfo", "Associator_Setup")%>
                </td>
            </tr>
        </table>
    <table width="100%" height="350" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td height="300" align="center" valign="top" class="tdBackColor"><table width="800" height="300" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td align="left" valign="bottom" style="height: 181px"><table width="800" height="150" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="150" height="30" align="right" valign="middle">
                <asp:Literal ID="lblLargessNum" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblLargessNum %>"></asp:Literal></td>
            <td align="left" valign="middle">&nbsp;<asp:TextBox ID="TextBox1" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="30" align="right" valign="middle">
                <asp:Literal ID="lblLargessBewrite" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblLargessBewrite %>"></asp:Literal></td>
            <td align="left" valign="middle">&nbsp;<asp:TextBox ID="txtGiftDesc" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="30" align="right" valign="middle">
                <asp:Literal ID="lblStartDate" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Literal></td>
            <td align="left" valign="middle">&nbsp;<asp:TextBox ID="txtAnStartDate" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px" onclick="calendar()"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="30" align="right" valign="middle">
                <asp:Literal ID="lblEndDate" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Literal></td>
            <td align="left" valign="middle">&nbsp;<asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" MaxLength="32"
                    Width="156px" onclick="calendar()"></asp:TextBox></td>
          </tr>
          <tr>
            <td height="30" align="right" valign="middle">&nbsp;</td>
            <td align="left" valign="middle">&nbsp;</td>
          </tr>
        </table></td>
      </tr>
      <tr>
        <td height="120" align="left" valign="top"><table width="800" height="120" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="250" align="center" valign="top" style="height: 131px"><table width="250" height="60" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td width="140" align="right" valign="middle" style="height: 25px">
                    <asp:CheckBox ID="chkExByBonus" runat="server" Text="<%$ Resources:BaseInfo,Associator_chkChangeInto %>" /></td>
                <td align="left" valign="middle" style="width: 164px; height: 25px;">
                    </td>
              </tr>
              <tr>
                <td align="right" valign="middle" class="style8">
                    </td>
                <td align="left" valign="middle" class="style7"></td>
              </tr>
                <tr>
                    <td align="right" class="style3" valign="middle">
                        <asp:Literal ID="lblNeedChangeInto" runat="server" 
                            Text="<%$ Resources:BaseInfo,Associator_lblNeedChangeInto %>"></asp:Literal>
                    </td>
                    <td align="left" class="style4" valign="middle">
                        &nbsp;<asp:TextBox ID="txtExByBonus" runat="server" CssClass="ipt160px" 
                            Enabled="False" MaxLength="32" Width="100px"></asp:TextBox>
                    </td>
                </tr>
            </table></td>
            <td width="280" align="center" valign="top" style="height: 131px"><table height="90" border="0" cellpadding="0" cellspacing="0" style="width: 298px">
              <tr>
                <td width="140" height="30" align="right" valign="middle"><asp:CheckBox ID="chkExByReceipt" runat="server" Text="<%$ Resources:BaseInfo,Associator_chkTicketChangeInto %>" /></td>
                <td align="left" valign="middle">&nbsp;</td>
              </tr>
              <tr>
                <td align="right" valign="middle" class="style5">
                    </td>
                <td align="left" valign="middle" class="style5"></td>
              </tr>
                <tr>
                    <td align="right" height="30" valign="middle">
                        <asp:Literal ID="lblChangeIntoM" runat="server" 
                            Text="<%$ Resources:BaseInfo,Associator_lblChangeIntoM %>"></asp:Literal>
                    </td>
                    <td align="left" valign="middle">
                        &nbsp;<asp:TextBox ID="txtExByReceipt" runat="server" CssClass="ipt160px" 
                            Enabled="False" MaxLength="32" Width="100px"></asp:TextBox>
                    </td>
                </tr>
              <tr>
                <td align="right" valign="middle" style="height: 40px">
                    <asp:Literal ID="lblChangeIntoOne" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblChangeIntoOne %>"></asp:Literal></td>
                <td align="left" valign="middle" style="height: 40px">
                    <asp:RadioButton ID="rdoYes" runat="server" Text="<%$ Resources:BaseInfo,Associator_rdoYes %>" Checked="True" GroupName="One" Enabled="False" /><asp:RadioButton ID="rdoNo" runat="server" Text="<%$ Resources:BaseInfo,Associator_rdoNo %>" GroupName="One" Enabled="False" /></td>
              </tr>
            </table>
                &nbsp; &nbsp;&nbsp;
                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"
                    OnClick="btnSave_Click" 
                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" /></td>
            <td width="270" align="center" valign="top" style="height: 131px"><table width="240" height="90" border="0" cellpadding="0" cellspacing="0">
              <tr>
                <td align="left" valign="middle"><asp:CheckBox ID="chkFreeGift" runat="server" Text="<%$ Resources:BaseInfo,Associator_chkExtend %>" /></td>
              </tr>
              <tr>
                <td align="center" valign="middle">&nbsp;</td>
              </tr>
              <tr>
                <td align="center" valign="middle">&nbsp;</td>
              </tr>
            </table></td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
</table>
        <asp:HiddenField ID="Associator_Setup" runat="server" Value="<%$ Resources:BaseInfo,Associator_Setup %>" />
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
