<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default10.aspx.cs" Inherits="Default10" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WorkFlow</title>
    <link href="App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ttl { CURSOR: pointer; COLOR: #ffffff; PADDING-TOP: 4px }
        A:active{COLOR: #000000;TEXT-DECORATION: none}
        A:hover{COLOR: #000000;TEXT-DECORATION: none}
        A:link{COLOR: #000000;TEXT-DECORATION: none}
        A:visited{COLOR: #000000;TEXT-DECORATION: none}
        TD {
        FONT-SIZE: 12px; FONT-FAMILY: "Verdana", "Arial", "细明体", "sans-serif"
        }
	</style>

    <script type="text/javascript">
        function showHide(obj,Imgobj)
        {
	        var oStyle = obj.style;
	        
	        if(oStyle.display == "none")
	        {
	            oStyle.display = "block";
	            Imgobj.src="imagesDemo/Menu/box_topleftbot.gif";
	        }
	        else
	        {
	            oStyle.display = "none"
	            Imgobj.src="imagesDemo/Menu/box_topleft.gif";
	        }
        }
    </script>

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body leftmargin="0" topmargin="0" >
    <form id="form2" runat="server">
    <table style="border-right: #909090 2px solid; border-left: #909090 2px solid; border-top-color: #909090;
            border-bottom: #909090 2px solid; height: 98%; width:100%; background-color:#F0F0F0;">
    <tr>
    <td style="width:100%; vertical-align: top; text-align: center;">
    
        <table border="0" cellpadding="0" cellspacing="0" style=" width: 90%;height: 8px">
            <tr style="width: 90%; height: 8px">
                <td style="background-color: #DFBF8D;">
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="1" cellspacing="1" style=" width: 90%; height: 5px">
            <tr>
                <td style="width: 90%;">
                </td>
            </tr>
        </table>
    <asp:Repeater ID="LeftMenu" runat="server" OnItemDataBound="LeftMenu_ItemDataBound">
        <ItemTemplate>
        <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
        <td>
            <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                <tr>
                    <td width="23">
                        <img  id="IMG_<%# Eval("BizGrpID")%>" height="25" src="imagesDemo/Menu/box_topleft.gif" width="23"></td>
                    <td class="ttl" onclick="JavaScript:showHide(M_<%# Eval("BizGrpID")%>,IMG_<%# Eval("BizGrpID")%>);" width="100%"
                        background="imagesDemo/Menu/box_topbg.gif">
                        <%# Eval("BizGrpName")%>
                    </td>
                  <td width="7">
                        <img height="25" src="imagesDemo/Menu/box_topright.gif" width="7"></td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td>
            <table id="M_<%# Eval("BizGrpID")%>" style="display: none" cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                <tr>
                    <td  height="0px" width='100%' style="border-right: #909090 1px solid; border-left: #909090 1px solid; border-top-color: #909090;
            border-bottom: #909090 1px solid; background-color:White;">
                        <table width='100%'>
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:TreeView ID="TreeView1" runat="server" MaxDataBindDepth="1" NodeIndent="15"
                                            Target="rightPartFrame" ImageSet="Inbox" ExpandDepth="0" >
                                            <ParentNodeStyle Font-Bold="False" />
                                            <HoverNodeStyle BackColor="#FFFF80" Font-Underline="False" />
                                            <SelectedNodeStyle Font-Bold="True" Font-Underline="False" />
                                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                                NodeSpacing="0px" VerticalPadding="0px" />
                                        </asp:TreeView>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td>
            
            <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                <tr>
                    <td colspan="3" >
                        <img height='10' src='imagesDemo/Menu/box_bottom.gif' width='100%'></td>
                </tr>
            </table>
        </td>
        </tr>
        </table>
        </ItemTemplate>
    </asp:Repeater>
        </td>
    </tr>
    </table>
    </form>
</body>
</html>
