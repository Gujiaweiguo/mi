<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnitInfo.aspx.cs" Inherits="RentableArea_Building_UnitInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
    </script>
    <style type="text/css">
.labelStyle
{
	azimuth: right;
	font-size: 9pt;
	text-align:right;
	color:Black;
}
 .Enabledipt150px {
	width:150px;
	height:22px;
	border-right:#FFFFFF 1px solid;
	border-top:#A0A0A0 1px solid;
	border-left:#A0A0A0 1px solid;
	border-bottom:#FFFFFF 1px solid;
	background-color:#F5F5F4;
	padding-left:1px;
	padding-top:1px;
	font-size:10.5pt;
 }

.cmb150px{
	font-size:10.5pt;
	height:20px;
	width:150px;
	}
.buttonEdit
{
    /*   图片路径*/
    /*去掉边框*/
        border-style: none;
            border-color: inherit;
            border-width: medium;
            background: url('../../App_Themes/CSS/BtnImage/btn_Edit.gif') no-repeat left top;
            text-align:right;
            color:#363D4F;
            font-size:12px;
            height:34px;
            width:62px;		
}
.buttonSave 
{
    /*   图片路径*/
    /*去掉边框*/
        border-style: none;
            border-color: inherit;
            border-width: medium;
            background: url('../../App_Themes/CSS/BtnImage/btn_Save.gif') no-repeat left top;
            text-align:right;
            color:#363D4F;
            font-size:12px;
            height:34px;
            width:62px;	
}

.buttonCancel
{
    /*   图片路径*/
    /*去掉边框*/
        border-style: none;
            border-color: inherit;
            border-width: medium;
            background: url('../../App_Themes/CSS/BtnImage/btn_Cancel.gif') no-repeat left top;
            text-align:right;
            color:#363D4F;
            font-size:12px;
            height:34px;
            width:62px;		
} 
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <div class="tdBackColor">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 292px; height: 273px" class="tdBackColor">
                                    <tr>
                                        <td colspan="3" class="tdBackColor" style="height: 20px; text-align: center; vertical-align:middle;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 265px">
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
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblUnitCode %>" 
                                                meta:resourcekey="lblUnitCodeResource1"></asp:Label></td>
                                        <td style="height: 25px">
                                            &nbsp;</td>
                                        <td style="width: 195px; text-align: left; height: 25px;">
                                            <asp:TextBox ID="txtUnitCode" runat="server" CssClass="Enabledipt150px" 
                                                meta:resourcekey="txtUnitCodeResource1"></asp:TextBox>
                                            <img id="ImgUnitCode" src="../../App_Themes/Main/Images/must.gif" style="width: 16px;
                                                height: 16px" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 26px; text-align: right">
                                            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblUnitType %>" 
                                                meta:resourcekey="lblUnitCodeResource1"></asp:Label></td>
                                        <td style="height: 26px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 26px; text-align: left">
                                            <asp:DropDownList ID="ddlUnitType" runat="server" Width="155px" 
                                                CssClass="cmb140px" meta:resourcekey="cmbBuildingIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; text-align: right; height: 22px;">
                                            <asp:Label ID="lblSelBuildingID" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblSelBuildingID %>" 
                                                meta:resourcekey="lblSelBuildingIDResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; text-align: left; height: 25px;">
                                            <asp:DropDownList ID="ddlBuildingID" runat="server" 
                                                Width="155px" CssClass="cmb140px" AutoPostBack="True" Enabled="False" 
                                                meta:resourcekey="cmbBuildingIDResource1">
                                        </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 24px; text-align: right">
                                            <asp:Label ID="lblSelFloorID" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblSelFloorID %>" 
                                                meta:resourcekey="lblSelFloorIDResource1"></asp:Label></td>
                                        <td style="height: 24px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="ddlFloorID" runat="server" Width="155px" 
                                                CssClass="cmb140px" AutoPostBack="True" Enabled="False" 
                                                meta:resourcekey="cmbFloorIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 26px; text-align: right">
                                            <asp:Label ID="lblSelLocationID" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblSelLocationID %>" 
                                                meta:resourcekey="lblSelLocationIDResource1"></asp:Label></td>
                                        <td style="height: 26px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="ddlLocationID" runat="server" Width="155px" 
                                                CssClass="cmb140px" 
                                                Enabled="False" meta:resourcekey="cmbLocationIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 26px; text-align: right">
                                            <asp:Label ID="lblSelArea" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblSelArea %>" 
                                                meta:resourcekey="lblSelAreaResource1"></asp:Label></td>
                                        <td style="height: 26px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="ddlArea" runat="server" CssClass="cmb150px" 
                                                Width="155px" meta:resourcekey="cmbTradeRelationResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="lblTradeID" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>" 
                                                meta:resourcekey="lblTradeIDResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="ddlTradeID" runat="server" CssClass="cmb150px" 
                                                Width="155px" meta:resourcekey="cmbTradeIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" meta:resourcekey="lblTradeIDResource1"
                                                Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label></td>
                                        <td style="height: 22px">
                                        </td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:DropDownList ID="ddlShopType" runat="server" CssClass="cmb150px" 
                                                Width="155px" meta:resourcekey="cmbTradeIDResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="lblFloorArea" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblFloorArea %>" 
                                                meta:resourcekey="lblFloorAreaResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:TextBox ID="txtFloorArea" runat="server" CssClass="Enabledipt150px" 
                                                MaxLength="8" meta:resourcekey="txtFloorAreaResource1"></asp:TextBox>
                                            <img id="ImgFloorArea" src="../../App_Themes/Main/Images/must.gif" style="width: 16px;
                                                height: 16px" /></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="lblUseArea" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblUseArea %>" 
                                                meta:resourcekey="lblUseAreaResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:TextBox ID="txtUseArea" runat="server" CssClass="Enabledipt150px" 
                                                MaxLength="8" meta:resourcekey="txtUseAreaResource1"></asp:TextBox>
                                            <img id="ImgtxtUseArea" src="../../App_Themes/Main/Images/must.gif" style="width: 16px;
                                                height: 16px" /></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; height: 25px; text-align: right">
                                            <asp:Label ID="lblShopName" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblShopName %>" 
                                                meta:resourcekey="lblShopNameResource1"></asp:Label></td>
                                        <td style="height: 25px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:TextBox ID="txtShopName" runat="server" CssClass="Enabledipt150px" 
                                                meta:resourcekey="txtShopNameResource1" Enabled="False"></asp:TextBox></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; text-align: right; height: 25px;">
                                            <asp:Label ID="lblSelUnitStatus" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,RentableArea_lblSelUnitStatus %>" 
                                                meta:resourcekey="lblSelUnitStatusResource1"></asp:Label></td>
                                        <td style="height: 25px">
                                            &nbsp;</td>
                                        <td style="width: 195px; text-align: left; height: 25px;">
                                            <asp:DropDownList ID="ddlUnitStatus" runat="server" CssClass="cmb140px" 
                                                Width="155px" meta:resourcekey="cmbUnitStatusResource1">
                                            </asp:DropDownList></td>
                                    </tr>
                                                                        <tr>
                                        <td style="width: 131px; height: 22px; text-align: right">
                                            <asp:Label ID="lblNode" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,User_lblNote %>" 
                                                meta:resourcekey="lblNodeResource1"></asp:Label></td>
                                        <td style="height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 195px; height: 25px; text-align: left">
                                            <asp:TextBox ID="txtNode" runat="server" CssClass="Enabledipt150px" 
                                                MaxLength="128" meta:resourcekey="txtNodeResource1"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" 
                                            style="height: 10px; text-align: center; vertical-align: bottom;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 268px">
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
                                        <td colspan="3" style="height: 5px; text-align: right">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="height: 30px; text-align: center">
                                                                                        &nbsp;<asp:Button ID="btnSave"
                                                    runat="server" CssClass="buttonSave" 
                                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" 
                                                meta:resourcekey="btnSaveResource1" onmouseover="BtnOver(this.id);" 
                                                onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" onclick="btnSave_Click"/>
                                            &nbsp; 
                                             <input id="btnCancel" class="buttonCancel" onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" type="button" value="取消"  runat="server" onclick="javascript:window.close();"/>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="height: 10px; text-align: left">
                                            </td>
                                    </tr>
                                </table>
    
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
