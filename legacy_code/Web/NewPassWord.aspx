<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewPassWord.aspx.cs" Inherits="NewPassWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
    <link href="App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    
    <script type="text/javascript"  src="App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="JavaScript/Common.js"></script>
	<script type="text/javascript" src="App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
    <script type="text/javascript" src="App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="App_Themes/nlstree/nlsctxmenu.js"></script>
	<script language="javascript" type="text/javascript" src="JavaScript/TabTools.js"></script>
<script type="text/javascript">
	function Load()
    {
        addTabTool("<%=baseInfo %>,NewPassword.aspx");
        loadTitle();
    }
    </script>
</head>
<body style="margin:0px" onload ="Load()">
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 4px">
                    <tr>
                        <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 356px;
                            height: 22px; text-align: left">
                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_NewPassword %>"
                                Width="295px"></asp:Label></td>
                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                        </td>
                        <td class="tdTopRightBackColor" style="vertical-align: top; width: 115px; height: 22px;
                            text-align: right" valign="top">
                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                    </tr>
                </table>
                <table class="tdBackColor" style="width: 100%;height: 400px">
                    <tr>
                        <td align="left" colspan="7" style="width: 60%; height: 100%">
                            <asp:Panel ID="Panel2" runat="server" Height="90%" Width="90%">
                                <table style="width: 60%">
                                    <tr>

                                        <td align="center" style="width: 30%; height: 14px">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Login_UserID %>" CssClass="labelStyle"></asp:Label></td>
                                        <td align="left" colspan="4" style="height: 14px">
                                            <asp:TextBox ID="txtUsrID" runat="server" CssClass="textstyle" Width="30%" Enabled="False"></asp:TextBox>
                                            <asp:TextBox ID="txtUsrName" runat="server" CssClass="textstyle" Width="56%" Enabled="False"></asp:TextBox></td>
                                    </tr>
                                    <tr>

                                        <td align="center" style="width: 30%; height: 13px">
                                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources:BaseInfo,User_OldPwd %>" CssClass="labelStyle"></asp:Label></td>
                                        <td align="left" colspan="4" style="height: 13px">
                                            <asp:TextBox ID="txtoldUsrPwd" runat="server"  Width="50%" CssClass="textstyle" TextMode="Password"></asp:TextBox>
                                            </td>
                                    </tr>
                                    <tr>

                                        <td align="center" style="width: 30%; height: 13px">
                                            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_NewPwd %>"></asp:Label></td>
                                        <td align="left" colspan="4" style="height: 13px">
                                            <asp:TextBox ID="txtNewPwd" runat="server" CssClass="textstyle" Width="50%" TextMode="Password"></asp:TextBox></td>
                                    </tr>
                                    <tr>

                                        <td align="center" style="width: 30%; height: 13px">
                                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_NewPwdT %>"></asp:Label></td>
                                        <td align="left" colspan="4" style="height: 13px">
                                            <asp:TextBox ID="txtNewPwdT" runat="server" CssClass="textstyle" Width="50%" TextMode="Password"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="5" style="height: 5px">
                                            <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnAdd_Click"
                                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                            <asp:Button ID="btnCancel" runat="server"
                                                            CssClass="buttonCancel" OnClick="btnQuit_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
</form> 
</body>
</html>
