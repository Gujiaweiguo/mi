<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptLCust.aspx.cs" Inherits="ReportM_Associator_RptLCust" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
            <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/Associator/RptLCust.aspx");
	        loadTitle();
	    }
	</script>
</head>
<body style="margin-top:0; margin-left:0" onload='Load()'>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="tdTopRightBackColor" style="width: 5px">
                            <img class="imageLeftBack" />
                        </td>
                        <td class="tdTopRightBackColor" style="text-align: left">
                            <asp:Label ID="Label1" runat="server" Text="会员信息查询"></asp:Label>
                        </td>
                        <td class="tdTopRightBackColor" style="width: 5px">
                            <img class="imageRightBack" />
                        </td>
                    </tr>
                    <tr style="height: 1px">
                        <td colspan="3" style="height: 1px; background-color: white">
                        </td>
                    </tr>
                </table>
                <table class="tdBackColor" style="width: 100%">
                    <tr style="height: 10px">
                        <td style="width: 89px">
                        </td>
                        <td style="width: 218px">
                        </td>
                        <td style="width: 44px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 22px">
                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="会员编码"></asp:Label></td>
                        <td style="width: 218px; height: 22px">
                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                        <td style="width: 44px; height: 22px">
                        </td>
                        <td style="height: 22px">
                            </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 17px">
                            <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="入会日期"></asp:Label></td>
                        <td style="width: 218px; height: 17px">
                            <asp:TextBox ID="txtStartBizTime" runat="server" CssClass="ipt160px" Height="18px"
                                onclick="calendar()" Width="79px"></asp:TextBox>
                            <asp:TextBox ID="txtEndBizTime" runat="server" CssClass="ipt160px" Height="18px"
                                onclick="calendar()" Width="78px"></asp:TextBox></td>
                        <td style="width: 44px; height: 17px">
                        </td>
                        <td style="height: 17px">
                            </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="font-size: 9pt; width: 89px; height: 17px">
                            <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="卡性"></asp:Label></td>
                        <td style="font-size: 9pt; width: 218px; height: 17px">
                            <asp:DropDownList ID="DropCardType" runat="server" Width="167px">
                            </asp:DropDownList></td>
                        <td style="width: 44px; height: 17px">
                        </td>
                        <td style="height: 17px">
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 15px">
                            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="姓名"></asp:Label></td>
                        <td style="width: 218px; height: 15px">
                            <asp:TextBox ID="txtName" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                        <td align="right" class="lable" style="width: 44px; height: 15px">
                        </td>
                        <td style="height: 15px">
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 7px">
                            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="移动电话"></asp:Label></td>
                        <td style="width: 218px; height: 7px">
                            <asp:TextBox ID="txtTel" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                        <td align="right" class="lable" style="width: 44px; height: 7px">
                        </td>
                        <td style="height: 7px">
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 12px">
                            <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="证件"></asp:Label></td>
                        <td style="width: 218px; height: 12px">
                            <asp:TextBox ID="txtID" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                        <td align="right" class="lable" style="width: 44px; height: 12px">
                        </td>
                        <td style="height: 12px">
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 10px">
                        </td>
                        <td style="width: 218px; height: 10px">
                        </td>
                        <td align="right" class="lable" style="width: 44px; height: 10px">
                        </td>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 16px">
                        </td>
                        <td style="width: 218px; height: 16px">
                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click"
                                Text="<%$ Resources:BaseInfo,User_lblQuery %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Height="35px" Width="62px"/>
                            <asp:Button ID="BtnCancel" runat="server"
                                    CssClass="buttonCancel" OnClick="BtnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Height="35px" Width="62px"/></td>
                        <td style="width: 44px; height: 16px">
                        </td>
                        <td style="height: 16px">
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 10px">
                        </td>
                        <td style="width: 218px; height: 10px">
                        </td>
                        <td style="width: 44px; height: 10px">
                        </td>
                        <td style="height: 10px">
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 4px">
                        </td>
                        <td style="width: 218px; height: 4px">
                        </td>
                        <td style="width: 44px; height: 4px">
                        </td>
                        <td style="height: 4px">
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
