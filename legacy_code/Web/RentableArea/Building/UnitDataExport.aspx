<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnitDataExport.aspx.cs" Inherits="RentableArea_Building_UnitDataExport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>数据导入</title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script src="../../JavaScript/TreeShow.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script>
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
    function WinClose()
    {
        //window.parent.document.all("btnD").click();
        //alert(window.opener.location);
        //alert(window.parent.parent.document.form1.btnD);
        window.opener.document.all("btnReFresh").click();
        window.close();
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
            <div >
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 420px; height: 173px" class="tdBackColor">
                                    <tr>
                                        <td colspan="3" class="tdBackColor" style="height: 20px; text-align: center; vertical-align:middle;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 385px">
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #738495">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                    </td>
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; text-align: right; height: 25px;">
                                            <asp:Label ID="lblUnitCode" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,BankCard_TransmitSelect %>" 
                                                meta:resourcekey="lblUnitCodeResource1"></asp:Label></td>
                                        <td style="height: 25px">
                                            <input id="File1" type="file" runat="server" /></td>
                                        <td style="width: 300px; text-align: left; height: 25px;" align="center">
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnDown" runat="server" 
                                                CssClass="labelStyle" onclick="btnDown_Click">格式下载</asp:LinkButton>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="height: 30px; text-align: center">
                                                                                        &nbsp;<asp:Button ID="btnSave"
                                                    runat="server" CssClass="buttonSave" 
                                                Text="<%$ Resources:BaseInfo,BankCard_btnTransmit %>" 
                                                meta:resourcekey="btnSaveResource1" onmouseover="BtnOver(this.id);" 
                                                onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" onclick="btnSave_Click"/>
                                            &nbsp; 
                                             <input id="btnCancel" class="buttonCancel" onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" type="button" value="取消"  runat="server" onclick="javascript:window.close();"/>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="height: 10px; text-align: left">
                                            &nbsp;</td>
                                    </tr>
                                </table>
    
            </div>
    </form>
</body>
</html>
