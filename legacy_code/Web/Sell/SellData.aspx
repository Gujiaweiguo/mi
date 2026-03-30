<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SellData.aspx.cs" Inherits="Sell_SellData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=baseInfo %></title>
        <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	function Load()
    {
        addTabTool("<%=strFresh %>,Sell/SellData.aspx");
        loadTitle();
    }
    </script>
</head>
<body onload='Load()' style="margin:0px">
    <form id="form1" runat="server">
    <div id="showLeaseBargain">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 401px; width: 100%;">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 356px; height: 22px; text-align: left; vertical-align: top;">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Sell_SellData %>" Width="295px"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px; vertical-align: top; text-align: right;" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 129px;">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 635px; height: 81px; vertical-align: top; top: 20px; text-align: center;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%; position: relative; top: 10px;">
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #738495">
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #ffffff">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                    
                                                        </td>
                                                </tr>
                                                <tr>
                                                <td style="height: 5px; text-align: center; vertical-align: top;" colspan="2">
                                                    <asp:Label ID="Label2" runat="server"></asp:Label>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: middle; width: 10%; height: 24px; text-align: right" valign="top">
                                                        &nbsp;&nbsp;
                                                    </td>
                                                    <td style="vertical-align: top; height: 24px" width="50%">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 10%; height: 24px; text-align: right; vertical-align: middle;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,BankCard_TransmitSelect %>" CssClass="labelStyle"></asp:Label>
                                                        &nbsp;
                                                        </td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; height: 24px;" width="50%">
                                                        &nbsp;<asp:FileUpload ID="FileUpload1" runat="server" Width="330px" />
                                                        <asp:Button ID="btnTransmit" runat="server" OnClick="Button1_Click" Text="<%$ Resources:BaseInfo,BankCard_btnTransmit %>" Width="70px" /></td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: middle; width: 10%; height: 24px; text-align: right" valign="top">
                                                    </td>
                                                    <td style="vertical-align: top; height: 24px" width="50%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: middle; width: 10%; height: 30px; text-align: right" valign="top">
                                                        &nbsp;&nbsp;
                                                    </td>
                                                    <td style="vertical-align: top; height: 30px" width="50%">
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                </table>
                            </div>
    </form>
</body>
</html>
