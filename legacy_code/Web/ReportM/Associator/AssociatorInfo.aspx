<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssociatorInfo.aspx.cs" Inherits="ReportM_Associator_AssociatorInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_AssociatorMes")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        var str= document.getElementById("Menu_AssociatorMes").value + ",Associator/AddAssociator.aspx";
	        addTabTool(str);
	        loadTitle();
	    }
    </script>
	
</head>
<body style="margin-top:0; margin-left:0" onload="Load()">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%;">
            <tr>
                <td class="tdTopBackColor" style="width: 1%">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor" style="width:98px">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "Menu_AssociatorMes")%>
                </td>
                <td class="tdTopBackColor" style="width: 1%">
                    <img alt="" class="imageRightBack" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%" class="tdBackColor">
            <tr style="height:10px">
                <td style="text-align:right;" colspan="4">
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:20px; padding-left:30px;" colspan="4">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="height: 1px; background-color: #738495">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 1px; background-color: #ffffff">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height:10px">
                <td colspan="4" style="padding-right: 20px; padding-left: 30px; text-align: right">
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    </td>
                <td style="width:25%">
                    </td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    </td>
                <td style="width:40%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right; height: 28px;">
                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorBirthday %>"></asp:Label></td>
                <td style="width: 25%; height: 28px;">
                    <asp:TextBox ID="txtDob" runat="server" CssClass="ipt160px" MaxLength="16" onclick="calendar()"
                        Width="168px"></asp:TextBox></td>
                <td style="padding-right: 5px; width: 11%; text-align: right; height: 28px;">
                    <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorDistance %>"></asp:Label></td>
                <td style="width: 40%; height: 28px;">
                    <asp:DropDownList ID="cmbDistanceId" runat="server" CssClass="ipt160px" Width="171px">
                    </asp:DropDownList></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorGender %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:DropDownList ID="cmbSexNm" runat="server" CssClass="ipt160px" Width="171px">
                    </asp:DropDownList></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorEarning %>"></asp:Label></td>
                <td style="width:40%">
                    <asp:DropDownList ID="cmbIncomeId" runat="server" CssClass="ipt160px" Width="170px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:5px; width:25%; height: 16px;">
                    <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorFolk %>"></asp:Label></td>
                <td style="width:25%; height: 16px;">
                    <asp:DropDownList ID="cmbRaceNm" runat="server" CssClass="ipt160px" Width="171px">
                    </asp:DropDownList></td>
                <td style="text-align:right; padding-right:5px; width:11%; height: 16px;">
                    </td>
                <td style="width:40%; height: 16px;">
                    </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:5px; width:25%; height: 28px;">
                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorNationality %>"></asp:Label></td>
                <td style="width:25%; height: 28px;">
                    <asp:DropDownList ID="cmbNatNm" runat="server" CssClass="ipt160px" Width="170px">
                    </asp:DropDownList></td>
                <td style="text-align:right; padding-right:5px; width:11%; height: 28px;">
                    <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorDuty %>"></asp:Label></td>
                <td style="width:40%; height: 28px;">
                    <asp:DropDownList ID="cmbJobTitleNm" runat="server" CssClass="ipt160px" Width="169px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right; height: 28px;">
                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorMarriage %>"></asp:Label></td>
                <td style="width: 25%; height: 28px;">
                    <asp:DropDownList ID="cmbMStatusNm" runat="server" CssClass="ipt160px" Width="170px">
                    </asp:DropDownList></td>
                <td style="padding-right: 5px; width: 11%; text-align: right; height: 28px;">
                    <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorVehicle %>"></asp:Label></td>
                <td style="width: 40%; height: 28px;">
                    <asp:DropDownList ID="DropDownList12" runat="server" CssClass="ipt160px" Width="169px">
                    </asp:DropDownList></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorCommemorate %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:DropDownList ID="cmbMAnnDateD" runat="server" CssClass="ipt160px" Width="84px">
                    </asp:DropDownList><asp:DropDownList ID="cmbMAnnDateM" runat="server" CssClass="ipt160px" Width="83px">
                    </asp:DropDownList></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorBringuUp %>"></asp:Label></td>
                <td style="width:40%">
                    <asp:DropDownList ID="cmbEduLevelNm" runat="server" CssClass="ipt160px" Width="168px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                </td>
                <td style="width:25%">
                    &nbsp; &nbsp;&nbsp;
                </td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    &nbsp;</td>
                <td style="width:40%">
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" style="padding-right:20px; padding-left:30px">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="height: 1px; background-color: #738495">
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 1px; background-color: #ffffff">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height:10px">
                <td colspan="4" style="padding-right: 20px; padding-left: 30px">
                </td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:5px; width:25%; height: 21px;">
                    <asp:Label ID="Label10" runat="server" Text="当前积分"></asp:Label></td>
                <td style="width:25%; height: 21px;">
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="ipt160px" MaxLength="16" onclick="calendar()"
                        Width="51px"></asp:TextBox>
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="ipt160px" MaxLength="16" onclick="calendar()"
                        Width="51px"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%; height: 21px;">
                    <asp:Label ID="Label15" runat="server" Text="卡类型"></asp:Label></td>
                <td style="width:40%; height: 21px;"><asp:DropDownList ID="DropDownList1" runat="server" CssClass="ipt160px" Width="125px">
                </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="Label13" runat="server" Text="总积分"></asp:Label></td>
                <td style="width: 25%">
                    <asp:TextBox ID="TextBox3" runat="server" CssClass="ipt160px" MaxLength="16" onclick="calendar()"
                        Width="51px"></asp:TextBox><asp:TextBox ID="TextBox4" runat="server" CssClass="ipt160px"
                            MaxLength="16" onclick="calendar()" Width="51px"></asp:TextBox></td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                    <asp:Label ID="Label16" runat="server" Text="卡类别"></asp:Label></td>
                <td style="width: 40%"><asp:DropDownList ID="DropDownList2" runat="server" CssClass="ipt160px" Width="125px">
                </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; height: 16px; text-align: right">
                    <asp:Label ID="Label14" runat="server" Text="积分日期"></asp:Label></td>
                <td style="width: 25%; height: 16px">
                    <asp:TextBox ID="TextBox5" runat="server" CssClass="ipt160px" MaxLength="16" onclick="calendar()"
                        Width="79px"></asp:TextBox><asp:TextBox ID="TextBox6" runat="server" CssClass="ipt160px"
                            MaxLength="16" onclick="calendar()" Width="79px"></asp:TextBox></td>
                <td style="padding-right: 5px; width: 11%; height: 16px; text-align: right">
                    <asp:Label ID="Label17" runat="server" Text="期满日期"></asp:Label></td>
                <td style="width: 40%; height: 16px">
                    <asp:TextBox ID="TextBox7" runat="server" CssClass="ipt160px" MaxLength="16" onclick="calendar()"
                        Width="79px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right; height: 15px;">
                    <asp:Label ID="Label18" runat="server" Text="入会日期"></asp:Label></td>
                <td style="width: 25%; height: 15px;">
                    <asp:TextBox ID="TextBox8" runat="server" CssClass="ipt160px" MaxLength="16" onclick="calendar()"
                        Width="79px"></asp:TextBox><asp:TextBox ID="TextBox9" runat="server" CssClass="ipt160px"
                            MaxLength="16" onclick="calendar()" Width="79px"></asp:TextBox></td>
                <td style="padding-right: 5px; width: 11%; text-align: right; height: 15px;">
                    </td>
                <td style="width: 40%; height: 15px;">
                    </td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    </td>
                <td style="width: 25%">
                    </td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                    </td>
                <td style="width: 40%">
                    </td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    </td>
                <td style="width: 25%">
                    </td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                    </td>
                <td style="width: 40%">
                    </td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; height: 21px; text-align: right">
                    </td>
                <td style="width: 25%; height: 21px">
                    &nbsp;</td>
                <td style="padding-right: 5px; width: 11%; height: 21px; text-align: right">
                    </td>
                <td style="width: 40%; height: 21px">
                    </td>
            </tr>
            <tr style="height:10px">
                <td colspan="4" style="padding-right: 20px; padding-left: 30px">
                </td>
            </tr>
            <tr >
                <td style="padding-right:20px; padding-left:30px;" colspan="4"><table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td style="height: 1px; background-color: #738495">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 1px; background-color: #ffffff">
                        </td>
                    </tr>
                </table>
                </td>
            </tr>
            <tr style="height:10px">
                <td colspan="4" style="padding-right: 20px; padding-left: 30px">
                </td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    </td>
                <td style="width: 25%">
                    </td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                </td>
                <td rowspan="3" style="width: 40%">
                    </td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    </td>
                <td style="width: 25%">
                    </td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                </td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    </td>
                <td style="width: 25%">
                    </td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                </td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; height: 5px; text-align: right">
                    </td>
                <td style="height: 5px" colspan="2">
                    </td>
                <td style="width: 40%; height: 5px">
                    </td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                </td>
                <td style="width: 25%">
                </td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                </td>
                <td style="width: 40%">
                    <asp:Button ID="btnOK" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click"
                        Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                    <asp:Button ID="BtnCel" runat="server"
                            CssClass="buttonCancel" OnClick="BtnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %> " /></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                </td>
                <td style="width: 25%">
                </td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                </td>
                <td style="width: 40%">
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
            </asp:UpdatePanel>
        <asp:HiddenField ID="Menu_AssociatorMes" runat="server" Value="<%$ Resources:BaseInfo,Menu_AssociatorMes %>" />
        <asp:HiddenField ID="Menu_AssociatorMesT" runat="server" Value="<%$ Resources:BaseInfo,Associator_Particular %>" />
    </form>
</body>
</html>

