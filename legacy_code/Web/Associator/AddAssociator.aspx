<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddAssociator.aspx.cs" Inherits="Associator_AddAssociator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= baseInfo %></title>
    <link href="../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript">
	    function Load()
	    {
//	        var str= document.getElementById("Menu_AssociatorMes").value + ",Associator/AddAssociator.aspx";
//	        addTabTool(str);
//	        loadTitle();
	        addTabTool("<%= baseInfo %>,<%= url %>");
	        loadTitle();
	    }
    </script>
	
</head>
<body style="margin-top:0; margin-left:0" onload="Load()">

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1200">
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
                    <asp:Label ID="labTitle" runat="server" Text="Label"></asp:Label>
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
            <tr style="height:10px">
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="lblAssociatorCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorCode %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtLCustId" runat="server" CssClass="ipt160px" MaxLength="16" Width="168px" BackColor="#F5F5F4" Enabled="False"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="lblAssociatorName" runat="server" CssClass="labelStyle" Text="称呼/姓名"></asp:Label></td>
                <td style="width:40%">
                    <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ipt160px" Width="55px">
                    </asp:DropDownList>
                    <asp:TextBox ID="txtLCustNm" runat="server" CssClass="ipt160px" MaxLength="64" Width="110px"></asp:TextBox>
                    <img src="../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
            </tr>
            <tr style="height:10px">
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="lblIdentity" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorIdentity %>"></asp:Label></td>
                <td style="width: 25%">
                    <asp:TextBox ID="txtLOtherId" runat="server" CssClass="ipt160px" MaxLength="32" Width="168px"></asp:TextBox></td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                    <asp:Label ID="lblEnrollment" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorEnrollment %>"></asp:Label></td>
                <td style="width: 40%">
                    <asp:TextBox ID="txtDateJoint" runat="server" CssClass="ipt160px" MaxLength="32"
                        onclick="calendar()" Width="167px"></asp:TextBox></td>
            </tr>
             <tr style="height:10px">
                <td style="text-align:right; padding-right:5px; width:20%">
                    <asp:Label ID="lblMobileTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorMobileTel %>"
                        Width="56px"></asp:Label></td>
                <td style="width:30%">
                    <asp:TextBox ID="txtMobilPhone" runat="server" CssClass="ipt160px" MaxLength="16"
                        Width="167px"></asp:TextBox>
                    <img src="../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="lblOfficeTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorOfficeTel %>"
                        Width="87px"></asp:Label>
                </td>
                <td style="width:40%">
                    <asp:TextBox ID="txtOffPhone" runat="server" CssClass="ipt160px" MaxLength="16" Width="167px"></asp:TextBox></td>
            </tr>
            <tr style="height:10px">
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="lblTel" runat="server" CssClass="labelStyle" Text="家庭电话"
                        Width="56px"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtHomePhone" runat="server" CssClass="ipt160px" MaxLength="16"
                        Width="167px"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="lblPostCode1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorPostCode %>"
                        Width="68px"></asp:Label></td>
                <td style="width:40%">
                    <asp:TextBox ID="txtPostalCode1" runat="server" CssClass="ipt160px" MaxLength="16"
                        Width="167px"></asp:TextBox></td>
            </tr>
            <tr style="height:10px">
                <td style="text-align:right; padding-right:5px; width:25%; height: 20px;">
                    <asp:Label ID="lblEmail" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorEmail %>"
                        Width="87px"></asp:Label></td>
                <td style="height: 28px;" colspan="3">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="ipt160px" Width="460px"></asp:TextBox></td>
            </tr>
            <tr style="height: 10px">
                <td style="padding-right: 5px; width: 20%; height: 28px; text-align: right">
                    <asp:Label ID="lblAddress1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorAddress %>"></asp:Label></td>
                <td colspan="3" style="height: 28px; width:80%">
                    <asp:TextBox ID="txtAddr1" runat="server" CssClass="ipt160px" Width="460px" MaxLength="128"></asp:TextBox>
                    <img src="../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
            </tr>
            <tr style="height: 10px">
                <td style="padding-right: 5px; width: 25%; height: 28px; text-align: right">
                </td>
                <td colspan="3" style="height: 28px">
                    <asp:TextBox ID="txtAddr2" runat="server" CssClass="ipt160px" MaxLength="128" Width="460px"></asp:TextBox></td>
            </tr>
            <tr style="height: 10px">
                <td style="padding-right: 5px; width: 25%; height: 28px; text-align: right">
                </td>
                <td colspan="3" style="height: 28px">
                    <asp:TextBox ID="txtAddr3" runat="server" CssClass="ipt160px" MaxLength="128" Width="460px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right; height: 28px;">
                    <asp:RadioButton ID="rdoDay" runat="server" GroupName="Associator" Visible="False"
                        Width="75px" /></td>
                <td style="width: 25%; height: 28px;">
                    &nbsp;
                    <asp:RadioButton ID="rdoOnce" runat="server" Checked="True" GroupName="Associator"
                        Width="71px" Visible="False" />
                    <asp:Label ID="lblOnceAvailability" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_OnceAvailability %>"
                        Width="51px" Visible="False"></asp:Label></td>
                <td style="padding-right: 5px; width: 11%; text-align: right; height: 28px;">
                    &nbsp;<asp:Label ID="lblLongTime" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_LongTime %>"
                        Visible="False" Width="56px"></asp:Label></td>
                <td style="width: 40%; height: 28px;">
                    <asp:TextBox ID="txtPostalCode2" runat="server" CssClass="ipt160px" MaxLength="16"
                        Width="19px" Visible="False"></asp:TextBox>
                    <asp:TextBox ID="txtPostalCode3" runat="server" CssClass="ipt160px" MaxLength="16"
                        Width="17px" Visible="False"></asp:TextBox>
                    <asp:RadioButton ID="rdoLongTime" runat="server" GroupName="Associator" Visible="False"
                        Width="99px" />
                    <asp:Label ID="lblDay" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_DayAvailability %>"
                        Visible="False" Width="56px"></asp:Label></td>
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
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorBirthday %>"></asp:Label>
                </td>
                <td style="width:25%">
                    <asp:DropDownList ID="cmbBM" runat="server" CssClass="ipt160px" Width="83px">
                </asp:DropDownList>
                    <asp:DropDownList ID="cmbBD" runat="server" CssClass="ipt160px" Width="84px">
                    </asp:DropDownList>
                    <img src="../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="lblOrigin" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorOrigin %>"
                        Width="89px"></asp:Label>
                    </td>
                <td style="width:40%">
                    <asp:DropDownList ID="drpOrigin" runat="server" CssClass="ipt160px" Width="168px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right; height: 21px;">
                    <asp:Label ID="Label18" runat="server" Text="年龄范围"></asp:Label></td>
                <td style="width: 25%; height: 21px;">
                    <asp:DropDownList ID="cmbAge" runat="server" Width="171px">
                    </asp:DropDownList>
                    <img src="../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                <td style="padding-right: 5px; width: 11%; text-align: right; height: 21px;">
                    <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorDistance %>"></asp:Label></td>
                <td style="width: 40%; height: 21px;">
                    <asp:DropDownList ID="cmbDistanceId" runat="server" CssClass="ipt160px" Width="168px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; height: 16px; text-align: right">
                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorGender %>"></asp:Label></td>
                <td style="width: 25%; height: 16px">
                    <asp:DropDownList ID="cmbSexNm" runat="server" CssClass="ipt160px" Width="171px">
                    </asp:DropDownList>
                    <img src="../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                <td style="padding-right: 5px; width: 11%; height: 16px; text-align: right">
                    <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorEarning %>"></asp:Label></td>
                <td style="width: 40%; height: 16px">
                    <asp:DropDownList ID="cmbIncomeId" runat="server" CssClass="ipt160px" Width="168px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorFolk %>"></asp:Label></td>
                <td style="width: 25%">
                    <asp:DropDownList ID="cmbRaceNm" runat="server" CssClass="ipt160px" Width="171px">
                    </asp:DropDownList></td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                    <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorOccupation %>"></asp:Label></td>
                <td style="width: 40%">
                    <asp:DropDownList ID="cmbBizNm" runat="server" CssClass="ipt160px" Width="168px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorNationality %>"></asp:Label></td>
                <td style="width: 25%">
                    <asp:DropDownList ID="cmbNatNm" runat="server" CssClass="ipt160px" Width="171px">
                    </asp:DropDownList></td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                    <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorDuty %>"></asp:Label></td>
                <td style="width: 40%">
                    <asp:DropDownList ID="cmbJobTitleNm" runat="server" CssClass="ipt160px" Width="168px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right; height: 21px;">
                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorMarriage %>"></asp:Label></td>
                <td style="width: 25%; height: 21px;">
                    <asp:DropDownList ID="cmbMStatusNm" runat="server" CssClass="ipt160px" Width="171px">
                    </asp:DropDownList></td>
                <td style="padding-right: 5px; width: 11%; text-align: right; height: 21px;">
                    <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorVehicle %>"></asp:Label></td>
                <td style="width: 40%; height: 21px;">
                    <asp:DropDownList ID="DropDownList12" runat="server" CssClass="ipt160px" Width="168px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; height: 21px; text-align: right">
                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorCommemorate %>"></asp:Label></td>
                <td style="width: 25%; height: 21px">
                    <asp:DropDownList ID="cmbMAnnDateM" runat="server" CssClass="ipt160px" Width="83px">
                    </asp:DropDownList>
                    <asp:DropDownList ID="cmbMAnnDateD" runat="server" CssClass="ipt160px" Width="84px">
                    </asp:DropDownList></td>
                <td style="padding-right: 5px; width: 11%; height: 21px; text-align: right">
                    <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorBringuUp %>"></asp:Label>&nbsp;</td>
                <td style="width: 40%; height: 21px">
                    <asp:DropDownList ID="cmbEduLevelNm" runat="server" CssClass="ipt160px" Width="168px">
                    </asp:DropDownList></td>
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
                    <asp:DropDownList ID="cmbRecreationNm1" runat="server" CssClass="ipt160px" Width="37px" Visible="False">
                    </asp:DropDownList>
                    <asp:DropDownList ID="cmbPreferMerNm1" runat="server" CssClass="ipt160px" Width="28px" Visible="False">
                    </asp:DropDownList>
                    <asp:DropDownList ID="cmbPreferGiftNm1" runat="server" CssClass="ipt160px" Width="32px" Visible="False">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:TextBox ID="txtDob" runat="server" CssClass="ipt160px" MaxLength="16" onclick="calendar()"
                        Width="59px" Height="9px" Visible="False"></asp:TextBox>&nbsp;
                    <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorCustom %>" Visible="False"></asp:Label>
                    <asp:DropDownList ID="DropDownList13" runat="server" CssClass="ipt160px" Width="79px" Visible="False">
                    </asp:DropDownList></td>
            </tr>
            <tr style="height:20px">
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="消费兴趣"></asp:Label></td>
                <td colspan="3">
                    <asp:CheckBoxList ID="CBoxInterest" runat="server" RepeatDirection="Horizontal" DataTextField="IItemName" DataValueField="IItemID">
                    </asp:CheckBoxList></td>
            </tr>
            <tr style="height:20px">
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="个人爱好"></asp:Label></td>
                <td colspan="3">
                    <asp:CheckBoxList ID="CBoxFavor" runat="server" RepeatDirection="Horizontal" DataTextField="FItemName" DataValueField="FItemID">
                    </asp:CheckBoxList></td>
            </tr>
            <tr style="height:20px">
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="活动讯息"></asp:Label></td>
                <td colspan="3">
                    <asp:CheckBoxList ID="CBoxActivity" runat="server" RepeatDirection="Horizontal" DataTextField="AItemName" DataValueField="AItemID">
                    </asp:CheckBoxList></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="Label17" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorRemark %>"></asp:Label></td>
                <td colspan="3">
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="ipt160px" Width="404px" MaxLength="20"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    &nbsp;</td>
                <td colspan="3">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                </td>
                <td style="width: 25%">
                </td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                </td>
                <td style="width: 40%">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                        Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />
                    &nbsp;<asp:Button ID="btnQuit" runat="server" CssClass="buttonCancel" OnClick="btnQuit_Click"
                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
            </tr>
            <tr>
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    &nbsp;</td>
                <td style="width: 25%">
                    &nbsp;</td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                    &nbsp;</td>
                <td style="width: 40%">
                    &nbsp;</td>
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