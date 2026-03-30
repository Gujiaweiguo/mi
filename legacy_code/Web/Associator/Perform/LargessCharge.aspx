<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LargessCharge.aspx.cs" Inherits="Associator_Perform_LargessCharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>积分换赠品</title>
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
         .style1
         {
             width: 25%;
             height: 26px;
         }
         .style2
         {
             width: 11%;
             height: 26px;
         }
         .style3
         {
             width: 40%;
             height: 26px;
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
	         addTabTool("<%=lbllargessExcharg %>,Associator/Perform/LargessCharge.aspx");
	        loadTitle();
	    }
	    
	    function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtNum.value))
            {
                parent.document.all.txtWroMessage.value =('数量不能为空！');
                return false;
            }
        }
	    
	    function AccountBonus()
	    {
	        if((document.getElementById("txtNum").value != "undefined")&&(document.getElementById("txtNum").value != ""))
	        {
	            var mustBonus = document.getElementById("txtNum").value * document.getElementById("txtMustBonus").value;
	            document.getElementById("txtTotalBonus").value = mustBonus;
	            document.getElementById("txtOverTotal").value = document.getElementById("txtBonus").value - mustBonus;
	        }
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
                    积分换赠品

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
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardID %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtCardID" runat="server" CssClass="ipt160px" Width="161px"></asp:TextBox></td>
                <td style="text-align:left; padding-right:5px; width:11%">
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
                    <asp:Label ID="Label13" runat="server" Text="总积分"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtBonus" runat="server" BackColor="#F5F5F4" CssClass="ipt160px" Width="161px" Enabled="False"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label15" runat="server" Text="到期日"></asp:Label></td>
                <td style="width:40%">
                    <asp:TextBox ID="txtActDate" runat="server" BackColor="#F5F5F4" CssClass="ipt160px" Width="161px" Enabled="False"></asp:TextBox></td>
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
                <td style="text-align:right; padding-right:5px; width:25%">
                    </td>
                <td style="width:25%">
                    </td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    </td>
                <td style="width:40%">
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
                <td style="text-align:right; padding-right:5px; width:25%">
                    </td>
                <td style="width:25%">
                    </td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    </td>
                <td style="width:40%">
                    </td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label8" runat="server" Text="赠品名称"></asp:Label></td>
                <td style="width:25%">
                    <asp:DropDownList ID="dropActID" runat="server" Width="167px" AutoPostBack="True" OnSelectedIndexChanged="dropActID_SelectedIndexChanged">
                    </asp:DropDownList></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label1" runat="server" Text="所需积分"></asp:Label></td>
                <td style="width:40%">
                    <asp:TextBox ID="txtMustBonus" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Enabled="False" Width="161px"></asp:TextBox></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label4" runat="server" Text="数量"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtNum" runat="server" CssClass="ipt160px" Width="161px"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label5" runat="server" Text="兑换日期"></asp:Label></td>
                <td style="width:40%">
                    <asp:TextBox ID="txtDate" runat="server" CssClass="ipt160px" Width="161px"></asp:TextBox></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label6" runat="server" Text="兑换总积分"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtTotalBonus" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Enabled="False" Width="161px"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label3" runat="server" Text="节余积分"></asp:Label>
                    </td>
                <td style="width:40%">
                    <asp:TextBox ID="txtOverTotal" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Enabled="False" Width="161px"></asp:TextBox></td>
            </tr>
             <tr>
                <td style="text-align:right; padding-right:5px; width:25%">
                    &nbsp;</td>
                <td style="width:25%">
                    &nbsp;</td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    &nbsp;</td>
                <td style="width:40%">
                    &nbsp;</td>
            </tr>
             <tr>
                 <td style="text-align:right; padding-right:5px; width:25%">
                 </td>
                 <td style="width:25%">
                 </td>
                 <td style="text-align:right; padding-right:5px; width:11%">
                 </td>
                 <td style="width:40%">
                     <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                         OnClick="btnSave_Click" Text="确定" />
                     &nbsp;<asp:Button ID="btnQuit" runat="server" CssClass="buttonCancel" 
                         OnClick="btnQuit_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                 </td>
             </tr>
             <tr>
                 <td class="style1" style="text-align:right; padding-right:5px; ">
                 </td>
                 <td class="style1">
                 </td>
                 <td class="style2" style="text-align:right; padding-right:5px; ">
                 </td>
                 <td class="style3">
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
                    </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
