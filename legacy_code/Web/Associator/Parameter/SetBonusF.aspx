<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetBonusF.aspx.cs" Inherits="Associator_Parameter_SetBonusF" ResponseEncoding="gb2312"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
      <title>积分规则设定</title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        <!--
        table.tbIntegral tr{ height:20px; }
        
        table.tbCard tr{ height:10px; }
        
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
	<script src="../../JavaScript/setTime.js" type="text/javascript"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool("积分规则设定,Associator/Parameter/SetBonusF.aspx");
	        loadTitle();
	    }
    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <div>
        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                 <td style="height: 24px; text-align: left; width: 854px;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="积分规则设定"></asp:Label></td>
            <td style="height: 24px; width: 663px;" class="tdTopRightBackColor" align="left"></td>
            <td style=" height: 24px;" class="tdTopRightBackColor" valign="top">
                <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
            </tr>
            <tr class="tdBackColor" style="height:20px">
                <td style="width: 854px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
             <tr class="tdBackColor">
                 <td colspan="2" style="padding-left:10px; padding-right:10px">
                     <table border="0" cellpadding="0" cellspacing="0" style="width: 117%">
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
                 <td>
                 </td>
             </tr>
            <tr class="tdBackColor">
                <td style="width: 854px; height: 21px;">
                </td>
                <td style="height: 21px">
                </td>
                <td style="height: 21px">
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="height: 21px;" colspan="3">
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:20%; text-align:right;padding-right:5px">
                                <asp:Label ID="Label1" runat="server" Text="购买金额"></asp:Label>
                            </td>
                            <td style="width:30%">
                                <asp:TextBox ID="txtAmt" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                            <td style="width:8%">
                                <asp:Label ID="Label3" runat="server" Text="调整方式"></asp:Label></td>
                            <td style="width:40%">
                                <asp:CheckBoxList ID="CBoxListType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="CBoxListType_SelectedIndexChanged">
                                    <asp:ListItem Value="0">根据商铺积分</asp:ListItem>
                                    <asp:ListItem Value="1">根据卡积分</asp:ListItem>
                                    <asp:ListItem Value="2">根据促销时间</asp:ListItem>
                                </asp:CheckBoxList></td>
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:right; padding-right:5px">
                                &nbsp;</td>
                            <td colspan="3">
                                </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; height: 15px; text-align: right">
                            </td>
                            <td style="width: 30%; height: 15px">
                            </td>
                            <td style="width: 8%; height: 15px">
                            </td>
                            <td style="width: 40%; height: 15px">
                            </td>
                        </tr>
                        <tr class="tdBackColor">
                         <td colspan="4" style="padding-left:10px; padding-right:10px">
                             <table border="0" cellpadding="0" cellspacing="0" style="width: 117%">
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
                       <tr style="height:15px">
                            <td colspan="4" style="width: 20%; text-align: right">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right;padding-right:5px">
                                <asp:Label ID="Label2" runat="server" Text="开始日期"></asp:Label></td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                            <td style="width: 8%; text-align: right;padding-right:5px">
                                <asp:Label ID="Label4" runat="server" Text="结束日期"></asp:Label></td>
                            <td style="width: 40%">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="padding-right: 5px; width: 20%; text-align: right">
                            </td>
                            <td style="width: 30%">
                                <asp:RadioButtonList ID="RbtnListDOrW" runat="server" RepeatDirection="Horizontal" Enabled="False" AutoPostBack="True" OnSelectedIndexChanged="RbtnListDOrW_SelectedIndexChanged">
                                    <asp:ListItem Value="1">每日</asp:ListItem>
                                    <asp:ListItem Value="2">每星期</asp:ListItem>
                                </asp:RadioButtonList></td>
                            <td style="padding-right: 5px; width: 8%; text-align: right">
                            </td>
                            <td style="width: 40%">
                                <asp:CheckBoxList ID="CBoxListWeek" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" Enabled="False">
                                    <asp:ListItem Value="1">星期一</asp:ListItem>
                                    <asp:ListItem Value="2">星期二</asp:ListItem>
                                    <asp:ListItem Value="3">星期三</asp:ListItem>
                                    <asp:ListItem Value="4">星期四</asp:ListItem>
                                    <asp:ListItem Value="5">星期五</asp:ListItem>
                                    <asp:ListItem Value="6">星期六</asp:ListItem>
                                    <asp:ListItem Value="7">星期日</asp:ListItem>
                                </asp:CheckBoxList></td>
                        </tr>
                        <tr>
                            <td style="padding-right: 5px; width: 20%; text-align: right">
                                <asp:Label ID="Label5" runat="server" Text="开始时间"></asp:Label></td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtStartTime" runat="server" onclick="_SetTime(this)" CssClass="ipt160px" Enabled="False"></asp:TextBox>
                                </td>
                            <td style="padding-right: 5px; width: 8%; text-align: right">
                                <asp:Label ID="Label6" runat="server" Text="结束时间"></asp:Label></td>
                            <td style="width: 40%">
                            <asp:TextBox ID="txtEndTime" runat="server" onclick="_SetTime(this)" CssClass="ipt160px" Enabled="False"></asp:TextBox>
                                </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 5px; width: 20%; text-align: right">
                                &nbsp;</td>
                            <td style="width: 30%">
                                &nbsp;</td>
                            <td style="padding-right: 5px; width: 8%; text-align: right">
                                &nbsp;</td>
                            <td style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="padding-right: 5px; width: 20%; text-align: right">
                                <asp:Label ID="Label7" runat="server" Text="计算因素"></asp:Label>
                            </td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtProFactor" runat="server" CssClass="ipt160px" 
                                    Enabled="False"></asp:TextBox>
                            </td>
                            <td style="padding-right: 5px; width: 8%; text-align: right">
                            </td>
                            <td style="width: 40%">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right">
                                &nbsp;</td>
                            <td style="width: 30%">
                                &nbsp;</td>
                            <td style="width: 8%">
                                &nbsp;</td>
                            <td style="width: 40%">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right">
                            </td>
                            <td style="width: 30%">
                            </td>
                            <td style="width: 8%">
                            </td>
                            <td style="width: 40%">
                                <asp:Button ID="BtnSave" runat="server" CssClass="buttonSave" 
                                    OnClick="btnSubmit_Click" Text="<%$ Resources:BaseInfo,User_btnOk %>" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="height: 21px;" colspan="3">
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>