<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IssueNewCard.aspx.cs" Inherits="Associator_Perform_IssueNewCard" ResponseEncoding="gb2312" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>发行新会员卡</title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        <!--
        table.tbIntegral tr{ height:28px; }
        
        table.tbIntegral tr.headLine{ height:1px; }
        table.tbIntegral tr.bodyLine{ height:1px; }
        
        table.tbIntegral td.baseLable{ padding-right:5px;text-align:right;}
        table.tbIntegral td.baseInput{ align:left;padding-right:20px }
        --> 
    </style>  
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/Calendar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool("发行新会员卡,<%=url %>");
	        loadTitle();
	    }
	    
	      //输入验证
        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtCardID.value))
            {
                parent.document.all.txtWroMessage.value =('请刷会员卡！');
                return false;
            }
        }
	    
	    function ReadCard()
	    {
	        var str;

             str=prompt("请刷会员卡","");

             if(str!=null)
             {
                  str=str.substring(7,15);
                  document.getElementById("txtCardID").value=str;
             }
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
            <tr style="height:30px">
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorCode %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:TextBox ID="txtMembCode" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="179px" Enabled="False"></asp:TextBox></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Associator_AssociatorName %>"
                        Width="26px"></asp:Label></td>
                <td style="width:40%">
                    &nbsp;<asp:TextBox ID="txtMembName" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="179px"></asp:TextBox></td>
            </tr>
            <tr style="height:30px">
                <td style="padding-right: 5px; width: 25%; text-align: right">
                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblOtherPassPort %>"></asp:Label></td>
                <td style="width: 25%">
                    <asp:TextBox ID="txtPassPort" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                        Width="179px"></asp:TextBox></td>
                <td style="padding-right: 5px; width: 11%; text-align: right">
                    </td>
                <td style="width: 40%">
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
                <td style="text-align:right; padding-right:5px;" colspan="4">
                    </td>
            </tr>
            <tr style="height:30px">
                <td style="padding-right: 5px; width: 25%; text-align: right; height: 28px;">
                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_CardID %>"></asp:Label></td>
                <td style="width: 25%; height: 28px;">
                    <asp:TextBox ID="txtCardID" runat="server" CssClass="ipt160px" Width="121px" BackColor="#F5F5F4" onclik="ReadCard()"></asp:TextBox><input id="Button1" type="button" value="点击刷卡" onclick="return ReadCard()" style="width: 56px"/></td>
                <td style="padding-right: 5px; width: 11%; text-align: right; height: 28px;">
                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblIssueDate %>"></asp:Label></td>
                <td style="width: 40%; height: 28px;">
                    <asp:TextBox ID="txtDate" runat="server" CssClass="ipt160px" onclick="calendar()"
                        Width="181px"></asp:TextBox></td>
            </tr>
             <tr style="height:30px">
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblCarDnature %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:RadioButton ID="radCustomer" runat="server" Checked="True" GroupName="B" Text="<%$ Resources:BaseInfo,Associator_lblCustomerCard %>"
                        Width="58px" /><asp:RadioButton ID="radEmployee" runat="server" GroupName="B" Text="<%$ Resources:BaseInfo,Associator_lblEmployeeCard %>"
                            Width="63px" /><asp:RadioButton ID="radOther" runat="server" GroupName="B" Text="<%$ Resources:BaseInfo,Dept_Other %>"
                                Width="56px" /></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTermDate %>"></asp:Label></td>
                <td style="width:40%">
                    <asp:TextBox ID="txtTermDate" runat="server" CssClass="ipt160px" onclick="calendar()"
                        Width="181px"></asp:TextBox></td>
            </tr>
            <tr style="height:30px">
                <td style="text-align:right; padding-right:5px; width:25%">
                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardType %>"></asp:Label></td>
                <td style="width:25%">
                    <asp:DropDownList ID="dropCardType" runat="server" Width="180px">
                    </asp:DropDownList></td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblCardLevel %>"></asp:Label>&nbsp;</td>
                <td style="width:40%">
                    <asp:DropDownList ID="dropCardLevel" runat="server" Width="183px">
                    </asp:DropDownList></td>
            </tr>
            <tr style="height:30px">
                <td colspan="4" style="padding-right:20px; padding-left:30px">
                    </td>
            </tr>
            <tr style="height:30px">
                <td style="text-align:right; padding-right:5px; width:25%">
                    </td>
                <td style="width:25%">
                    </td>
                <td style="text-align:right; padding-right:5px; width:11%">
                    </td>
                <td style="width:40%">
                    <asp:Button ID="btnSubmit" runat="server" CssClass="buttonSave" OnClick="btnSubmit_Click"
                        Text="<%$ Resources:BaseInfo,User_btnOk %>" /><asp:Button ID="btnCancel" runat="server"
                            CssClass="buttonClear" OnClick="btnCancel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
            </tr>
            <tr style="height:100px">
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
    </form>
</body>
</html>
