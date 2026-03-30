<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptAdvisContractInfoItem.aspx.cs" Inherits="RptBaseMenu_RptAdvisContractInfoItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Rpt_Title_lblAdvisContractInfoItem")%></title>
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
	        addTabTool("<%=baseInfo %>,ReportM/RptBase/RptAdvisContractInfoItem.aspx");
	        loadTitle();
	    }
	</script>
</head>
<body style="margin:0px" onload ="Load();">
    <form id="form1" runat="server">
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
                            <asp:Label ID="Label1" runat="server" Style="position: static" Text="<%$ Resources:BaseInfo, Rpt_Title_lblAdvisContractInfoItem %>"></asp:Label>
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
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_AdContractCode %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:TextBox ID="txtAdContractCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                        <td style="width: 44px">
                            <asp:Label ID="Label13" runat="server" Text="排序顺序" Visible="False" Width="73px"></asp:Label></td>
                        <td>
                            <asp:RadioButton ID="RadioButton4" runat="server" GroupName="b" Visible="False" /></td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton5" runat="server" GroupName="b" Visible="False" /></td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton6" runat="server" GroupName="b" Visible="False" /></td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_SignDate %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:TextBox ID="txtSignDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                        <td align="right" class="lable" style="width: 44px">
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton7" runat="server" GroupName="b" Visible="False" /></td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                        <td align="right" class="lable" style="width: 44px">
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton9" runat="server" GroupName="b" Visible="False" /></td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblEndDate %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                        <td align="right" class="lable" style="width: 44px">
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton10" runat="server" GroupName="b" Visible="False" /></td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo, AdContract_lblAdType %>" Visible="False"></asp:Label></td>
                        <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlAdType" runat="server" Width="165px" Visible="False">
                                                        </asp:DropDownList></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                            <asp:Button ID="BtnCancel" runat="server" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="BtnCancel_Click" /></td>
                        <td align="right" class="lable" style="width: 44px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"
                                Visible="False"></asp:Label></td>
                        <td style="width: 218px">
                                                        &nbsp;
                            <asp:DropDownList ID="ddlContractStatus" runat="server" Visible="False" Width="165px">
                            </asp:DropDownList></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                        </td>
                    </tr>
                     <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                                                        &nbsp;
                            <asp:DropDownList ID="DropDownList1" runat="server" Visible="False" Width="165px">
                            </asp:DropDownList></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                                                        &nbsp;
                            <asp:DropDownList ID="DropDownList2" runat="server" Visible="False" Width="165px">
                            </asp:DropDownList></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                        </td>
                    </tr>
                                        <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                                                        &nbsp;
                            <asp:DropDownList ID="DropDownList3" runat="server" Visible="False" Width="165px">
                            </asp:DropDownList></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
