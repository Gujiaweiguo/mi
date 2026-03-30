<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default9.aspx.cs" Inherits="Default9" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link href="App_Themes/Munu/transparent/buttonface-h.css" rel="stylesheet" type="text/css" />
        <link href="App_Themes/Munu/transparent/buttonface-v.css" rel="stylesheet" type="text/css" />
        
         <%--<link href="Css/css.css" type="text/css" rel="stylesheet"/>--%>
        <link href="App_Themes/Tabbar/css/TabTool.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="App_Themes/Munu/lib/nlsmenu.js"></script>
        <script type="text/javascript" src="App_Themes/Munu/lib/nlsmenueffect.js"></script>
        <script type="text/javascript" src="App_Themes/Munu/lib/nlsmenuext_dyn.js"></script>
        <script type="text/javascript" src="App_Themes/Munu/lib/nlsmenuext_htm.js"></script>
    <link href="App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <title><asp:Literal runat="server" text="<%$ Resources:BaseInfo,Main_Operation_Interface%>"></asp:Literal> </title>
</head>
<body onload="switchSysBar()" scroll="no" onbeforeunload="return CloseEvent();" leftmargin="0" topmargin="0" marginheight="0" marginwidth="0" style="  text-align:center;">
    <form id="form1" runat="server" z-index="1">
    <table border="0" cellspacing="0" cellpadding="0" width="98%" height="100%">
        <tr>
            <td colspan="3" style="border-bottom: 0px solid #000000; width:100%; height: 5px; ">
                <table height="49" width = "100%" border="0" cellspacing="0" cellpadding="0" >
                    <tr>
                        <td style="height:47px;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src=App_Themes/Main/Images/help-top.jpg', sizingMethod='scale'); padding-left:47px;">
                            <img src="App_Themes/Main/Images/HelpTitle.gif" width="232px" height="34px" />
                        </td>
                    </tr>
                    <tr style="text-align:left; " >  
                        <td colspan="0"  style=" text-align:left; width:100%; "  >
                            <table style=" width:100%; text-align:left;" border="0"; cellpadding="0"; cellspacing="0";>
                                <tr>
                                     <td style="width: 100%; text-align:left;background-image:url('App_Themes/Main/Images/bannerbg.jpg'); vertical-align:top;">
                                         <script type="text/javascript">
                                              initMenu();
                                              menuMgr.renderMenubar();
                                         </script>
                                                                                   
                                     </td>

                               </tr>
                            </table>
                        </td>
                    </tr>
               </table>
            </td>
        </tr>
        
        
        <tr>
            <td id="frmTitle" name="frmTitle" nowrap="nowrap" valign="middle" align="center" style="border-right: 0px solid #000000; width: 18%;">
               <table style=" width:100%; height:2px; background-color:White;" border="0"; cellpadding="1"; cellspacing="1";>
                  <tr>
                     <td></td>
                  </tr>
               </table>
               <table style="width:100%; height:25px;" border="0"; cellpadding="0"; cellspacing="0";>
                  <tr style="width: 100%;">
                    <td style="width: 5px; background-image: url(App_Themes/Main/Images/Left.jpg); border:0px; height: 25px;"></td>
                    <td style="background-color: #D6C08F; width: 193px;border:0px; height: 25px; text-align: center;">
                      <a style="font:12pt;"><asp:Literal ID="TitleTree" runat="server"  Text="<%$ Resources:BaseInfo,Munu_WorkbencNavigator %>"/></a></td>
                  </tr>
               </table>
               <table style="width:100%; height:94%" border="0"; cellpadding="0"; cellspacing="0";>
                 <tr>
                   <td>
                     <iframe id="LeftTree" name="BoardTitle" style="height: 100%; visibility: inherit; width: 100%; z-index: 2"
                       scrolling="auto" frameborder="0" src="Default10.aspx"></iframe>
                   </td>
                 </tr>
               </table>
            </td>
            <td style="width: 10pt" bgcolor="#7898A8" width="10" title="CloseOpenLeft" class="navPoint">
                <table border="0" cellpadding="0" cellspacing="0" width="7" height="100%" align="right">
                    <tr>
                        <td valign="middle" align="right" style="width: 13px; background-color:#F0F0F0;">
                            <img border="0" src="imagesDemo/Menu/close.gif" id="menuimg" alt="HiddenLeft" onmouseover="javascript: menuonmouseover();"
                                onmouseout="javascript: menuonmouseout();" onclick="javascript:switchSysBar()"
                                style="cursor: hand" width="7" height="76" /></td>
                    </tr>
                </table>
            </td>
            <td style="width: 100%; vertical-align: top;"> 
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;border-right:#F0F0F0 2px solid; border-bottom:#909090 1px solid;" >
                      <tr height="2">
                          <td colspan="4" style="height: 2px">
                          </td>
                      </tr>
                      <tr style=" height: 28px;">
                          <td style="text-align: left; background-color:#D6C08F; height: 28px;">&nbsp;&nbsp;&nbsp;</td>
                          <td style="text-align: left; width: 70%;background-color:#D6C08F; height: 28px;">
                                 <asp:Label ID="TitleShow" runat="server" ></asp:Label>
                           </td>
                          <td style="vertical-align: top; text-align: right; width: 5%;background-color:#D6C08F; height: 28px;">
                              <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/function/Help24.GIF" />&nbsp;</td>
                          <td style="vertical-align: top; width: 25%; height: 28px;background-color:#D6C08F">
                              <asp:TextBox ID="txtWroMessage" runat="server"  width="280px" ReadOnly ="true" ></asp:TextBox>&nbsp;</td>
                      </tr>
                        <tr >
                            <td colspan="4">
                                <table style="width: 100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 5%;"></td>
                                        <td id="TabTools" colspan="2" style="vertical-align: bottom; text-align: right"></td>
                                        <td style="width: 5%;"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style=" height: 100%;">
                          <td colspan="4" style="margin-top: -10px; vertical-align: top; width: 100%;">
                            <table style="width:100%;  padding-top: 0px; height: 100%; margin-top:0" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                 <td style="width:5%; height: 100%; text-align: center;">
                                     <img height="401" src="images/shuxian.jpg" /></td>
                                  <td style="width:90%; vertical-align: top; height: 100%;margin-top:0">
                                    <iframe style=" visibility: inherit; width: 100%; height: 100%; z-index:1;" frameborder="0" id="mainFrame" name="rightPartFrame" onclick="return mainFrame_onclick()"  src="Disktop.aspx">
                                    </iframe>
                                  </td>
                                  <td style="width:5%; height: 100%; text-align: center;">
                                     <img height="401" src="images/shuxian.jpg" /></td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                  </table>

            </td>
        </tr>

<tr>
            <td colspan="3" style="height: 20px">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" height="20">
                    <tr>
                        <td class="down_text" style="background-color:#D6C08F">
                                    Powered By <a href="http://www.wincor-nixdorf.com" target="_blank"><font color="red">Wincor-Nixdorf</font></a> Retail & Banking Systems(Shanghai) Co.,Ltd.
                            </td>
                            <td align="right" width="420" bgcolor="#D6C08F">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td width="130">&nbsp;<font color="#000000"><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_CurUserName")%></font><asp:Label ID="lblUserName" ForeColor="#000000" runat="server"></asp:Label>&nbsp;</td>
                                    
                                    <%--<td width="75" style="cursor:pointer;border-left:1px solid #000000;" onclick="javascript: window.open('help/help.html')">&nbsp;<img src="imagesDemo/helpIcon.gif" style="margin-bottom: -3px" id="IMG2" onclick="return IMG3_onclick()">&nbsp;<font color="#000000"><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_Help")%></font></td>--%>
                                    <td width="75" style="cursor:pointer;border-left:1px solid #000000;" onclick="javascript: window.open('help/help.html')">&nbsp;<img src="imagesDemo/helpIcon.gif" style="margin-bottom: -3px" id="IMG2" onclick="return IMG3_onclick()">&nbsp;<font color="#000000"><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_Help")%></font></td>
                                    <td width="75" style="cursor:pointer;border-left:1px solid #000000;" onclick="javascript: window.mainFrame.location.href='newpassword.aspx'">&nbsp;<img src="imagesDemo/modify.gif" style="margin-bottom: -3px" id="IMG1" onclick="return IMG3_onclick()">&nbsp;<font color="#000000"><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_NewPassword")%></font></td>
                                    <td width="75" style="cursor:pointer;border-left:1px solid #000000;" onclick="javascript: window.mainFrame.location.href='Disktop1.aspx';window.parent.LeftTree.location.href='Default10.aspx';">&nbsp;<img src="imagesDemo/house.gif" style="margin-bottom: -3px" id="IMG3" onclick="return IMG3_onclick()">&nbsp;<font color="#000000"><%= (String)GetGlobalResourceObject("BaseInfo", "Login_Backhome")%></font></td>
                                    <td width="75" style="cursor:pointer;border-left:1px solid #000000;" onclick="javascript: window.top.location.href = 'Login.aspx'">&nbsp;<img src="imagesDemo/logout.gif" style="margin-bottom: -3px">&nbsp;<font color="#000000"><%= (String)GetGlobalResourceObject("BaseInfo", "Login_Loginout")%></font></td>
                                </tr>
                            </table>
                            
                            </td>
                    </tr>
                            </table>
                            
                            </td>
                    </tr>
                </table>


    </form>

</body>
</html>

<script language="JavaScript" type="text/javascript">

var DispClose = true;

function CloseEvent()
{
    if (DispClose)
    {
        return "";
    }
}
    rnd.today=new Date(); 

    rnd.seed=rnd.today.getTime(); 

    function rnd() { 

　　　　rnd.seed = (rnd.seed*9301+49297) % 233280; 

　　　　return rnd.seed/(233280.0); 

    }; 

    function rand(number) { 

　　　　return Math.ceil(rnd()*number); 

    }; 
    
    function AlertMessageBox(Messages)
    {
        DispClose = false; 
        window.location.href = location.href+"?"+rand(1000000);
        alert(Messages);
    }
    


function switchSysBar(){

 	if (document.all("frmTitle").style.display=="none") {
		document.all("frmTitle").style.display=""
		document.getElementById("menuimg").src="images/Menu/close.gif";
		document.getElementById("menuimg").src="HiddenLeft"
		}
	else {
		document.all("frmTitle").style.display="none"
		document.getElementById("menuimg").src="images/Menu/open.gif";
		document.getElementById("menuimg").src="OpenLeft"
	 }
}

 function menuonmouseover(){
 		if (document.all("frmTitle").style.display=="none") {
 		document.getElementById("menuimg").src="imagesDemo/Menu/open_on.gif";
 		}
 		else{
 		document.getElementById("menuimg").src="imagesDemo/Menu/close_on.gif";
 		}
 }
 function menuonmouseout(){
 		if (document.all("frmTitle").style.display=="none") {
 		document.getElementById("menuimg").src="imagesDemo/Menu/open.gif";
 		}
 		else{
 		document.getElementById("menuimg").src="imagesDemo/Menu/close.gif";
 		}
 }
     if(top!=self)
    {
        top.location.href = "default.aspx";
    }

</script>

