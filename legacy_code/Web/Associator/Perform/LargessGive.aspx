<!--

/// 修改人：hesijian
/// 修改时间:2009年5月11日

-->

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LargessGive.aspx.cs" Inherits="Associator_Perform_LargessGive" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Associator_chkExtend")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        <!--
        table.tbIntegral tr{ height:28px; }
        
        table.tbCard tr{ height:10px; }
        
        table.tbIntegral tr.headLine{ height:1px; }
        table.tbIntegral tr.bodyLine{ height:1px; }
        
        table.tbIntegral td.baseLable{ padding-right:5px;text-align:right;}
        table.tbIntegral td.baseInput{ align:left;padding-right:20px }
         .style4
         {
             width: 25%;
             height: 20px;
         }
         .style5
         {
             width: 11%;
             height: 20px;
         }
         .style6
         {
             width: 40%;
             height: 20px;
         }
        --> 
    </style>  
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	         addTabTool("<%=chkExtend %>,Associator/Perform/LargessGive.aspx");
	        loadTitle();
	    }
    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server" >
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
                    <asp:Label ID="Label0" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblLargessCharge %>"></asp:Label>
                </td>
                <td class="tdTopBackColor" style="width: 1%">
                    <img alt="" class="imageRightBack" />
                </td>
            </tr>
        </table>
        <table style="width:100%" border="0" cellpadding="0" cellspacing="0" class="tdBackColor">
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
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_ServiceDesk %>"
                        Width="45px"></asp:Label></td>
                <td style="width:25%">
                    <asp:DropDownList ID="dropServiceDesk" runat="server" Width="160px" AutoPostBack="True" OnSelectedIndexChanged="dropServiceDesk_SelectedIndexChanged">
                    </asp:DropDownList></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    &nbsp;<asp:Label ID="LabName" runat="server" Text="用户名"></asp:Label></td>
                <td style="width:40%">
                    &nbsp;
                    <asp:TextBox ID="txtUser" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="159px" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr style="height:10px">
                <td colspan="4" style="padding-right: 20px; padding-left: 30px; text-align: right">
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
            <tr style="height:30px">
                <td style="text-align:right; padding-right:5px; width:25%;">
                    <asp:Label ID="Label1" runat="server" Text="活动内容"></asp:Label></td>
                <td colspan="3">
                    <asp:DropDownList ID="dropActID" runat="server" Width="414px" AutoPostBack="True" OnSelectedIndexChanged="dropActID_SelectedIndexChanged">
                    </asp:DropDownList>&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label3" runat="server" Text="开始日期"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtStartDate" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="159px" Enabled="False"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label4" runat="server" Text="结束日期"></asp:Label></td>
                <td style="width:40%">
                    <asp:TextBox ID="txtEndDate" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="160px" Enabled="False"></asp:TextBox></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label5" runat="server" Text="赠品内容"></asp:Label></td>
                <td colspan="3">
                    <asp:TextBox ID="txtGiftDesc" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="411px" Enabled="False"></asp:TextBox></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label6" runat="server" Text="方法方式"></asp:Label></td>
                <td style="width:25%">
                    <asp:RadioButton ID="radGiftOption" runat="server" Enabled="False" GroupName="A"
                        Text="<%$ Resources:BaseInfo,Associator_rdoEveryTime %>" Width="120px" /></td>
                <td style="text-align:left; padding-right:5px;" colspan="2">
                    <asp:RadioButton ID="radGiftOptionOneDay" runat="server" Enabled="False" GroupName="A"
                        Text="<%$ Resources:BaseInfo,Associator_rdoEveryDay %>" Width="131px" /></td>
            </tr>
             <tr style="height:10px">
                <td colspan="4" style="padding-right: 20px; padding-left: 30px; text-align: right">
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
                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardID %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtCardID" runat="server" CssClass="ipt160px" Width="161px"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                        Text="<%$ Resources:BaseInfo,User_lblQuery %>" Width="65px" /></td>
                <td style="width:40%">
                    </td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtMembName" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="161px" Enabled="False"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label7" runat="server" Text="证件号码"></asp:Label></td>
                <td style="width:40%">
                    <asp:TextBox ID="txtCertNum" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="161px" Enabled="False"></asp:TextBox></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorMobileTel %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtTel" runat="server" BackColor="#F5F5F4" CssClass="ipt160px" Width="161px" Enabled="False"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblNumber %>"></asp:Label></td>
                <td style="width:40%">
                    <asp:TextBox ID="txtNumber" runat="server" CssClass="ipt160px"
                        Width="161px"></asp:TextBox></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label15" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIssueDate %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtActDate" runat="server" BackColor="#F5F5F4" CssClass="ipt160px" Width="161px" Enabled="False"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    </td>
                <td style="width:40%">
                    &nbsp;</td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; " class="style4">
                    </td>
                <td class="style4">
                    </td>
                <td style="text-align:right; padding-right:5px; " class="style5">
                    </td>
                <td class="style6">
                    </td>
            </tr>
             <tr style="height:40px">
                 <td style="text-align:right; padding-right:5px; width:25%">
                 </td>
                 <td style="width:25%">
                 </td>
                 <td style="text-align:right; padding-right:5px; width:11%">
                 </td>
                 <td style="width:40%">
                     <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                         OnClick="btnSave_Click" Text="确定" />
                     <asp:Button ID="btnQuit" runat="server" CssClass="buttonCancel" 
                         OnClick="btnQuit_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                 </td>
             </tr>
             <tr>
                 <td class="style4" style="text-align:right; padding-right:5px; ">
                 </td>
                 <td class="style4">
                 </td>
                 <td class="style5" style="text-align:right; padding-right:5px; ">
                 </td>
                 <td class="style6">
                 </td>
             </tr>
        </table>
    </div>
    </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
