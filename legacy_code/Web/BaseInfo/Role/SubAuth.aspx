<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubAuth.aspx.cs" Inherits="BaseInfo_Role_SubAuth" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Role_lblFunc")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	function BtnUp( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
}
function BtnOver( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
}
	</script>
</head>
<body  style="margin:0px">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width:48%" class="tdBackColor">
                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="Label2" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Role_lblFunc %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                             <table style="width:100%">
                                <tr>
                                    <td style="width:20%">
                                    </td>
                                    <td style="width:60%">
                                    </td>
                                    <td style="width:20%">
                                    </td>
                                </tr>
                                <tr style=" height:400px;">
                                    <td style="width:20%">
                                    </td>
                                    <td style="width:60%;background-color:White; vertical-align:top">
                                    <asp:TreeView Font-Size="9pt" ID="treeView" runat="server" OnSelectedNodeChanged="treeView_SelectedNodeChanged" ExpandDepth="0" ShowLines="True">
                            </asp:TreeView>
                                    </td>
                                    <td style="width:20%">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:20%">
                                    </td>
                                    <td style="width:60%">
                                    </td>
                                    <td style="width:20%">
                                    </td>
                                </tr>
                               
                            </table>
                        </td>
                        <td style="width:4%">
                        </td>
                        <td style="width:48%;" class="tdBackColor"  valign="top">
                            <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="Label1" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Role_lblSubAuth %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                             <table style="width:100%">
                                <tr>
                                    <td style="width:30%">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:30%">
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Width="235px"></asp:Label></td>
                                    
                                </tr>
                                <tr>
                                    <td style="width:30%; height:30px">
                                    </td>
                                    <td style="height:30px; text-align:left">
                                        <asp:CheckBox ID="chckBoxExport" runat="server" Text="<%$ Resources:BaseInfo,Role_lblSubAuthExport %>" />
                                        <asp:CheckBox ID="chckBoxPrint" runat="server" Text="<%$ Resources:BaseInfo,btn_lblPrint %>" /></td>
                                    
                                </tr>
                                <tr>
                                    <td style="width:30%; height:30px">
                                        <asp:Label ID="Label3" runat="server" Text="Label" Visible="False"></asp:Label></td>
                                    <td style="height:30px">
                                        <asp:DropDownList ID="DropListSubAuth" runat="server" Width="151px" Visible="False">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width:30% ; height:30px">
                                    </td>
                                    <td style="height:30px">
                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                            Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                    
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr >
                        <td style="width:48%" class="tdBackColor">
                        </td>
                        <td style="width:4%">
                        </td>
                        <td style="width:48%" class="tdBackColor">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
