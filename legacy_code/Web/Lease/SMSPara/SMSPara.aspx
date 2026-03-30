<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SMSPara.aspx.cs" Inherits="Lease_SMSPara_SMSPara" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_SMSPara")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:600px;height:401px;}
            

            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
        <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,Lease/SMSPara/SMSPara.aspx");
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
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_SMSPara %>"></asp:Label>
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
                                                    <td style="width: 70px; height: 10px;">
                                                    </td>
                                                    <td style="width: 90px; height: 10px; text-align: left;">
                                                    </td>
                                                    <td style="width: 190px; height: 10px;">
                                                        
                                                    </td>
                                                    <td style="width: 34px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 100px; height: 22px;" class="lable">
                                                        &nbsp;<asp:Label ID="Label13" runat="server" CssClass="labelStyle" Height="17px"
                                                            Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>" Width="73px"></asp:Label>
                                                        </td>
                                                    <td class="lable" style="width: 90px; height: 10px; text-align: left;">
                                                        <asp:CheckBox ID="chkContract" runat="server" Text="<%$ Resources:BaseInfo,SkuInfo_GoUpNum %>" CssClass="labelStyle" AutoPostBack="True" OnCheckedChanged="chkContract_CheckedChanged" /></td>
                                                    <td style="width: 218px; height: 22px;">
                                                        <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" Height="17px" MaxLength="32" ></asp:TextBox></td>
                                                    <td style="width: 44px; height: 22px;">
                                                        </td>
                                                    <td style="height: 22px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 100px; height: 17px;">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Height="17px" Text="<%$ Resources:BaseInfo,LeaseAreaType_CustCode %>"
                                                            Width="73px"></asp:Label>
                                                        </td>
                                                    <td class="lable" style="width: 90px; height: 10px; text-align: left;">
                                                        <asp:CheckBox ID="chkCustCode" runat="server" Text="<%$ Resources:BaseInfo,SkuInfo_GoUpNum %>" CssClass="labelStyle" AutoPostBack="True" OnCheckedChanged="chkCustCode_CheckedChanged" /></td>
                                                    <td style="width: 190px; height: 17px;">
                                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" Height="17px" MaxLength="16" ></asp:TextBox></td>
                                                    <td style="width: 44px; height: 17px;">
                                                    </td>
                                                    <td style="height: 17px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 100px; height: 16px;" class="lable">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Height="17px" Text="<%$ Resources:BaseInfo,Rpt_SkuId %>"
                                                            Width="73px"></asp:Label>
                                                        </td>
                                                    <td class="lable" style="width: 90px; height: 10px; text-align: left;">
                                                        <asp:CheckBox ID="chkSkuCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,SkuInfo_GoUpNum %>" AutoPostBack="True" OnCheckedChanged="chkSkuCode_CheckedChanged" /></td>
                                                    <td style="width: 190px; height: 16px;">
                                                        <asp:TextBox ID="txtSkuCode" runat="server" CssClass="ipt160px" Height="17px" MaxLength="16"></asp:TextBox></td>
                                                    <td style="width: 44px; height: 16px;">
                                                        </td>
                                                    <td style="height: 16px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 100px; height: 15px;" class="lable">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Height="17px" Text="<%$ Resources:BaseInfo,Rpt_UserID %>"
                                                            Width="73px"></asp:Label>
                                                        </td>
                                                    <td class="lable" style="width: 90px; height: 10px; text-align: left;">
                                                        <asp:CheckBox ID="chkUserCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,SkuInfo_GoUpNum %>" AutoPostBack="True" OnCheckedChanged="chkUserCode_CheckedChanged"/></td>
                                                    <td style="width: 190px; height: 15px;">
                                                        <asp:TextBox ID="txtUserCode" runat="server" CssClass="ipt160px" Height="17px" MaxLength="8"></asp:TextBox></td>
                                                    <td style="width: 44px; height: 15px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 15px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 100px; height: 7px;">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Height="17px" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"
                                                            Width="73px"></asp:Label></td>
                                                    <td class="lable" style="width: 90px; height: 10px; text-align: left;">
                                                        <asp:CheckBox ID="chkShopCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,SkuInfo_GoUpNum %>" AutoPostBack="True" OnCheckedChanged="chkUserCode_CheckedChanged"/></td>
                                                    <td style="width: 190px; height: 7px;">
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                    </td>
                                                    <td style="height: 7px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="height: 12px; text-align: center;" colspan="5">
                                                    
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 96%">
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
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 100px; height: 10px;">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Height="17px" Text="<%$ Resources:BaseInfo,SMSpara_lblSubAuthExport %>"
                                                            Width="73px"></asp:Label></td>
                                                    <td align="right" class="lable" colspan="3" style="height: 10px; text-align: left">
                                                        <asp:TextBox ID="txtOut" runat="server" CssClass="ipt160px" Height="17px" Width="91%" MaxLength="128"></asp:TextBox></td>
                                                    <td style="height: 10px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 100px; height: 16px;" class="lable">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Height="17px" Text="<%$ Resources:BaseInfo,SMSpara_lblSubAuthInput %>"
                                                            Width="73px"></asp:Label></td>
                                                    <td colspan="3" style="height: 16px">
                                                        <asp:TextBox ID="txtIn" runat="server" CssClass="ipt160px" Height="17px" Width="90%" MaxLength="128"></asp:TextBox></td>
                                                    <td style="height: 16px">
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd"  OnClick="btnIn_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                            Text="<%$ Resources:BaseInfo,SMSPara_InPuth %>"  /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="height: 10px; text-align: center;" class="lable" colspan="5">
                                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 96%">
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
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 100px; height: 30px; text-align: right;">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,SMSPara_MailSMTP %>" CssClass="labelStyle" Height="17px"></asp:Label></td>
                                                    <td class="lable" style="text-align: left;" colspan="3">
                                                        <asp:TextBox ID="txtMailSMTP" runat="server" CssClass="ipt160px"  Height="17px" Width="91%" MaxLength="128"></asp:TextBox></td>
                                                    <td style="height: 4px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 100px; height: 30px; text-align: right;">
                                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:BaseInfo,SMSPara_MailSMTPUser %>" CssClass="labelStyle" Height="17px"></asp:Label></td>
                                                    <td style="text-align: left;" colspan="3">
                                                        <asp:TextBox ID="txtMailSMTPUser" runat="server" CssClass="ipt160px" Height="17px" Width="90%" MaxLength="128"></asp:TextBox></td>
                                                    <td style="text-align: left; vertical-align: bottom;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 100px; height: 30px; text-align: right;">
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,SMSPara_MailSMTPPassword %>" CssClass="labelStyle" Height="17px"></asp:Label></td>
                                                    <td style="text-align: left;" colspan="3">
                                                        <asp:TextBox ID="txtMailSMTPPassword" runat="server" CssClass="ipt160px" Height="17px" Width="90%" TextMode="Password" MaxLength="128"></asp:TextBox></td>
                                                    <td style="width: 170px; height: 30px">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"  OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                            Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 100px; height: 18px; text-align: right">
                                                    </td>
                                                    <td colspan="3" style="width: 100px; height: 18px; text-align: right">
                                                    </td>
                                                    <td style="width: 100px; height: 18px; text-align: right">
                                                    </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
