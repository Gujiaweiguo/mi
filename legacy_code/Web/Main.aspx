<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Main" StylesheetTheme="Basic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="App_Themes/Munu/transparent/buttonface-h.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Munu/transparent/buttonface-v.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="App_Themes/Munu/lib/nlsmenu.js"></script>
        <script type="text/javascript" src="App_Themes/Munu/lib/nlsmenueffect.js"></script>
        <script type="text/javascript" src="App_Themes/Munu/lib/nlsmenuext_dyn.js"></script>
        <script type="text/javascript" src="App_Themes/Munu/lib/nlsmenuext_htm.js"></script>
    
    
    <link runat="server" rel="stylesheet" href="~/CSS/Import.css" type="text/css" id="AdaptersInvariantImportCSS" />
     <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="App_Themes/JS/BaseClass.js"></script>--%>
<title></title>
<%--
<script type="text/JavaScript">
<!--
function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_showHideLayers() { //v6.0
  var i,p,v,obj,args=MM_showHideLayers.arguments;
  for (i=0; i<(args.length-2); i+=3) if ((obj=MM_findObj(args[i]))!=null) { v=args[i+2];
    if (obj.style) { obj=obj.style; v=(v=='show')?'visible':(v=='hide')?'hidden':v; }
    obj.visibility=v; }
}

//-->
</script>--%>

<%-- <link href="App_Themes/Munu/transparent/transparent-h.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Munu/transparent/transparent-v.css" rel="stylesheet" type="text/css" />--%>
        <!-- NlsMenu script includes -->


<%--<script type="text/javascript">
        
  function changeEffect(o) {
    menuMgr.defaultEffect=o.value;
    menuMgr.hideMenus();
    var oMn;
    for (var it in menuMgr.menus) {
      oMn=menuMgr.menus[it];
      if (o.value=="") {
        oMn.effect=null;
        oMn.rt.ready=true;
      } else {
        oMn.useEffect(menuMgr.defaultEffect);
      }
      var mn=NlsMenu.$GE(it);
      var elm=mn.childNodes[0];
      elm.style.top="0px";
      elm.style.left="0px";      
    }
  }
  
</script>--%>





</head>

<body scroll="no" leftmargin="0" topmargin="0" marginheight="0" marginwidth="0" style=" margin-left:12px; text-align:center;">

<form id="form1" runat="server">
<table width="980px" border="0" cellspacing="0" cellpadding="0">

  <tr>
    <td align="left" valign="top" style="height: 84px" colspan="">
<table width="980" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td style="height: 47px"><img src="App_Themes/Main/Images/logo.gif" width="980" height="47" /></td>
  </tr>
  <tr>
    <td><img src="App_Themes/Main/Images/image.GIF" width="980" height="46" /></td>
    </tr>
</table>
    </td>
  </tr>
  <tr style="height: 28px;width:970px;text-align:left; " >
  
                         <td colspan="0"  style="height: 25px;width:800px; background-image:url(App_Themes/Basic/bg.GIF);text-align:left;" >
                                        <table style="margin-left:8px; width:184px; height:5px; text-align:left;" border="0"; cellpadding="1"; cellspacing="1";>
                                           <tr>
                                             <td style="height: 25px; text-align:left; width: 270px; vertical-align:top;">
                                             <script type="text/javascript">
                                              initMenu();
                                                menuMgr.renderMenubar();
                                             </script>
                                               
                                             </td>
                                            </tr>
                                        </table>
                        </td>
                        </tr>
  <tr>
    <td align="left" valign="top" style="height:470px;"  >
        <table width="100%" cellspacing="0" cellpadding="0" >
            <tr>
              <td id="leftPart" width="200" align="left" valign="top" bgcolor="#FFFFFF"  >
              
               <table style=" width:200px; height:2px; background-color:White;" border="0"; cellpadding="1"; cellspacing="1";>
                   <tr>
                        <td>
                        
                        </td>

                    </tr>
                </table>
               <table style="width:200px; height:25px;" border="0"; cellpadding="0"; cellspacing="0";>
                    <tr style="width: 184px;">
                        <td style="width: 5px; background-image: url(App_Themes/Main/Images/Left.jpg); border:0px; height: 25px;">
                
                </td>
                <td style="background-color: #B4BEC8; width: 195px;border:0px; height: 25px;">
                    <asp:Label ID="Label1" runat="server" Font-Size="12pt" Text="<%$ Resources:BaseInfo,Munu_WorkbencNavigator %>" Width="187px"></asp:Label></td>
            </tr>
        </table> 
        <iframe name="leftPartFrame" frameborder="0" src="MainTree.aspx" width="100%"    scrolling="No" style="visibility: inherit; width: 198; z-index: 2">
<%--BaseInfo/User/MainTree.aspx--%>
        </iframe>
        </td>
              <td id="midPart" align="center" valign="middle" bgcolor="#F0F0F0" style="cursor:move; width: 7px; height: 152px;" onmousedown="MouseDown(this)"  onmouseup="MouseUp()"><img id="myTabArrow" src="App_Themes/Main/Images/btnTabLeft.gif" onclick="shutAndOpenLeftTab()" style="cursor:hand;" /></td>
              <td id="rightPart" valign="top">
                  <table border="0" cellpadding="0" cellspacing="0" style="width: 771px; height: 24px">
                      <tr height="2">
                          <td colspan="6" style="height: 2px">
                          </td>
                      </tr>
                      <tr>
                          <td style="border-right: 0px; border-top: 0px; background-image: url(App_Themes/Main/Images/Left.jpg);
                              vertical-align: top; border-left: 0px; width: 5px; border-bottom: 0px; height: 28px;">
                          </td>
                          <td style="border-right: 0px; border-top: 0px; vertical-align: top; border-left: 0px;
                              width: 195px; border-bottom: 0px; background-color: #b4bec8; height: 28px;" valign="bottom">
                          </td>
                          <td style="border-right: 0px; border-top: 0px; background-image: url(App_Themes/Main/Images/right.jpg);
                              vertical-align: top; border-left: 0px; width: 7px; border-bottom: 0px; height: 28px;">
                          </td>
                          <td style="border-right: 0px; border-top: 0px; vertical-align: top; border-left: 0px;
                              width: 47px; border-bottom: 0px; height: 28px; text-align: center">
                          </td>
                          <td style="vertical-align: top; text-align: right; height: 28px;">
                              <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/function/Help24.GIF" />&nbsp;
                          </td>
                          <td style="vertical-align: top; width: 412px; height: 28px">
                              <asp:TextBox ID="txtWroMessage" runat="server" Width="408px" ReadOnly="True"></asp:TextBox>
                          </td>
                      </tr>
                  </table>
              <iframe name="rightPartFrame" frameborder="0" src="" width="100%" scrolling="No"></iframe></td>
            </tr>
     <%--       --%>
        </table>
    </td>
  </tr>
</table>
<%--<script type="text/javascript" src="App_Themes/JS/BaseClass.js"></script>
<script type="text/javascript">
function resetFrameHight() {
	getWindowHeight = document.body.clientHeight;
	//alert(getWindowHeight);
	sendToFrameHeight = getWindowHeight - 107 - 20;
	//alert(sendToFrameHeight);
	document.getElementById("leftPartFrame").height = sendToFrameHeight;
	document.getElementById("rightPartFrame").height = sendToFrameHeight;
}
resetFrameHight();
</script>--%>
    
    
    
  </form>

</body>
</html>
