<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptMemberAlterForBonus.aspx.cs" Inherits="ReportM_RptMember_RptMemberAlterForBonus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>    
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript">
    function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptMember/RptMemberAlterForBonus.aspx");
	        loadTitle();
	    }
    function aa()
    {
    
        parent.document.all.mainFrame.scrolling="NO";
    }
            function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110 || key == 189 || key == 109)
            {
		        window.event.returnValue =true;
	        }
	        else
	        {
		        window.event.returnValue =false;
	        }
	    }
    </script>
    
</head>
<body style="margin:0px" onload="Load();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="会员积分异常情况查询"></asp:Label>
                                                    </td>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                        <img class="imageRightBack"/>
                                                    </td>
                                                </tr>
                                                <tr style="height:1px">
                                                    <td colspan="3" style="background-color:White; height:1px">
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width:100%" class="tdBackColor">
                                                <tr style="height:10px">
                                                    <td style="width: 221px">
                                                    </td>
                                                    <td style="width: 10px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 221px;" class="lable">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblAssociatorCard %>"></asp:Label></td>
                                                    <td class="lable" style="width: 10px">
                                                    </td>
                                                    <td style="width: 218px;">
                                                        <asp:TextBox ID="txtmembCardID" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                    <td style="width: 44px;">
                                                        <asp:Label ID="Label13" runat="server" Text="排序顺序" Width="73px" Visible="False"></asp:Label></td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton4" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 221px">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserName %>"></asp:Label></td>
                                                    <td class="lable" style="width: 10px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtmemberName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                    <td style="width: 44px">
                                                        </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 221px" class="lable">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_SalesDate %>"></asp:Label></td>
                                                    <td class="lable" style="width: 10px">
                                                    </td>
                                                    <td style="width: 218px">
                                                         <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt75px" onclick="calendar()"></asp:TextBox>
                                                         <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt75px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 44px">
                                                        </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton6" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 221px" class="lable">
                                                       </td>
                                                    <td class="lable" style="width: 10px">
                                                    </td>
                                                    <td style="width: 218px">
                                                       </td>
                                                    <td style="width: 44px" class="lable" align="right">
                                                        </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton7" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 221px">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_BonusAmt %>"></asp:Label></td>
                                                    <td class="lable" style="width: 10px; text-align: left">
                                                        <asp:CheckBox ID="cbBonus" runat="server" TextAlign="Left" Width="11px" /></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtBonusAmt" runat="server" CssClass="ipt75px" style="text-align:right">10000</asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton8" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 221px">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_Bonusrate %>"></asp:Label></td>
                                                    <td class="lable" style="width: 10px; text-align: left">
                                                        <asp:CheckBox ID="cbDouble" runat="server" TextAlign="Left" Width="10px" /></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtBonusLead" runat="server" CssClass="ipt75px" style="text-align:right">5</asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton9" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 221px">
                                                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_Transrate %>"></asp:Label>
                                                        </td>
                                                    <td class="lable" style="width: 10px; text-align: left">
                                                        <asp:CheckBox ID="cbF" runat="server" Width="1px" /></td>
                                                    <td style="width: 218px">
                                                    <asp:TextBox ID="txtTradeFrequency" runat="server" CssClass="ipt75px" style="text-align:right">15</asp:TextBox>
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton10" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 221px" class="lable">
                                                        </td>
                                                    <td class="lable" style="width: 10px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton11" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 221px" class="lable">
                                                        </td>
                                                    <td class="lable" style="width: 10px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnOK" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click"/>
                                                        &nbsp;<asp:Button ID="BtnCel" runat="server" CssClass="buttonCancel" 
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %> "  OnClick="BtnCel_Click" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 221px">
                                                        </td>
                                                    <td class="lable" style="width: 10px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 221px; height: 28px;">
                                                    </td>
                                                    <td style="width: 10px; height: 28px">
                                                    </td>
                                                    <td style="width: 218px; height: 28px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 28px;">
                                                    </td>
                                                    <td style="height: 28px">
                                                    </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
