<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login1" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
   <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
<title>欢迎使用MI.NET系统</title>
<link href="CSS/Login1.css" rel="stylesheet" type="text/css" />
<style type="text/css">
<!--
body {
	background-image: url();
	background-color: #fdf3e2;
}
.divClass{
    text-decoration:underline;
    cursor:hand;
    font-size:15px;
    margin-right:5px;
    margin-top:5px;
    color:#0000CC;
    
}
-->
</style>
    <script type="text/javascript">
        function keyclick()
        {
            if(event.keyCode == 13)
            {
                event.keyCode = 9;
                document.getElementById("Button1").click();
            }
        }
        function login()
		{
			open('Default9.aspx','','left=0px,top=0px,width='+(window.screen.width-10)+',height='+(window.screen.height-50)+',toolbar=no, menubar=no, scrollbars=auto, resizable=yes,location=no, status=yes');
			window.opener=null;
			window.close();
			//location.href="Default9.aspx"; //该语句导致刷新两次
		}
        function ShowHelp()
        {
            //window.showModalDialog('LoginHelp.aspx','window','dialogWidth=600px;dialogHeight=400px'); 
            window.open("LoginHelp.aspx","","");
        }
    </script>
</head>
<body style="text-align: center; vertical-align:middle;">
    <form id="form1" runat="server">  
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                
       <table width="100%" height="630" border="0" align="center" cellpadding="0" cellspacing="0" background="images/images/bg.jpg">
  <tr>
    <td height="141" align="right" colspan="3" valign="top"><div class="divClass">
        <nobr><A onclick="ShowHelp();">登录帮助</A></nobr></div> 
        </td>
  </tr>
  <tr>
    <td height="314">&nbsp;</td>
    <td><table width="652" height="314" border="0" align="center" valign="middle" cellpadding="0" cellspacing="0" background="images/images/Login0.jpg">
      <tr>
        <td width="178" height="168">&nbsp;</td>
        <td width="296">&nbsp;</td>
        <td width="178">&nbsp;</td>
      </tr>
      <tr>
        <td height="65">&nbsp;</td>
        <td align="center" valign="middle"><table width="296" border="0" cellpadding="0" cellspacing="0" style="height: 18px">
            <tr>
              <td background="images/images/Login_03.gif" style="width: 62px; height: 28px">&nbsp;</td>
              <td valign="baseline" style="width: 163px; height: 27px" align="right"><asp:TextBox ID="txtUserCode" runat="server" CssClass="textstyle" width="154px" TabIndex="1" BorderStyle="Groove" Height="20px"></asp:TextBox></td>
              <td style="width: 13px; height: 27px"></td>
              <td width="66" rowspan="3" align="left" valign="middle">
              <asp:ImageButton ID="Button1" runat="server" ImageUrl ="~/images/images/Login_06.gif" OnClick="Button1_Click" TabIndex="3" Height="65px" Width="65px" /></td>
            </tr>
            <tr>
              <td style="width: 62px; height: 1px">&nbsp;</td>
              <td valign="top" style="width: 163px; height: 1px">&nbsp;</td>
              <td style="width: 13px; height: 1px"></td>
            </tr>
            <tr>
              <td background="images/images/Login_09.gif" style="width: 62px; height: 6px">&nbsp;</td>
              <td valign="top" style="width: 163px; height: 6px" align="right"><asp:TextBox ID="txtPassword" runat="server" CssClass="textstyle" TextMode="Password" TabIndex="2" Height="20px" Width="154px" BorderStyle="Groove"></asp:TextBox></td>
              <td style="width: 13px; height: 6px"></td>
            </tr>
        </table></td>
        <td>&nbsp;</td>
      </tr>
      <tr>
        <td style="height: 68px">&nbsp;</td>
        <td style="height: 68px">&nbsp;</td>
        <td style="height: 68px">&nbsp;</td>
      </tr>
    </table></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td height="175">&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
</table>
</ContentTemplate>
            </asp:UpdatePanel>

                      <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="False" />
     
                   
    </form>
</body>
</html>
