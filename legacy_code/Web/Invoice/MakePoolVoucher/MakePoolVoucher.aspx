<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MakePoolVoucher.aspx.cs" Inherits="Invoice_MakePoolVoucher_MakePoolVoucher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "MakePoolVoucher_lblRentalInt")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
    function Load()
    {
       addTabTool("<%=strFresh %>,Invoice/MakePoolVoucher/MakePoolVoucher.aspx");
        loadTitle();
    }
     //输入验证
        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtBeginDate.value))
            {
                parent.document.all.txtWroMessage.value =('<%= publicMes_DateError %>');
                return false;
            }
            
             if(isEmpty(document.all.txtEndDate.value))
            {
                parent.document.all.txtWroMessage.value =('<%= publicMes_DateError %>');
                return false;
            }
            
            
            if(document.getElementById("txtBeginDate").value > document.getElementById("txtEndDate").value)
            {
                parent.document.all.txtWroMessage.value =('<%= beginEndDate %>');
                return false;
            }
        }  
    </script>
</head>
<body topmargin=0 leftmargin=0 onload="Load()">
    <form id="form1" runat="server">
                            <div id="showLeaseBargain">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 401px; width: 100%;">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 356px; height: 22px; text-align: left; vertical-align: top;">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,MakePoolVoucher_lblRentalInt %>" Width="295px"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px; vertical-align: top; text-align: right;" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 129px;">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 635px; height: 81px; vertical-align: top; top: 20px; text-align: center;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%; position: relative; top: 10px;">
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #738495">
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #ffffff">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                    
                                                        </td>
                                                </tr>
                                                <tr>
                                                <td style="height: 5px; text-align: center; vertical-align: top;" colspan="2">
                                                <asp:Label ID="lblMsg" runat="server"></asp:Label>&nbsp;
                                                                </td>
                                                </tr>
                                                 <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 10%; height: 24px; text-align: right; vertical-align: middle;">
                                                        <asp:Label ID="lblSubsName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MakePoolVoucher_lblSubs %>"
                                                            Width="86px"></asp:Label></td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; height: 24px;" width="50%">
                                                        <asp:DropDownList ID="cmbSubs" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="cmbSubs_SelectedIndexChanged">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 10%; height: 24px; text-align: right; vertical-align: middle;">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MakePoolVoucher_WarrantName %>"
                                                            Width="86px"></asp:Label></td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; height: 24px;" width="50%">
                                                        <asp:DropDownList ID="cmbAccountParaID" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 10%; height: 24px; text-align: right; vertical-align: middle;">
                                                        <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_InvDate %>"
                                                            Width="86px"></asp:Label></td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; height: 24px;" width="50%">
                                                        <asp:TextBox ID="txtBeginDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox>
                                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                </tr>
                                                                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 10%; height: 24px; text-align: right; vertical-align: middle;">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MakePoolVoucher_FYear %>"
                                                            Width="86px"></asp:Label></td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; height: 24px;" width="50%"><asp:DropDownList ID="cmbFYear" runat="server" Width="80px">
                                                    </asp:DropDownList>
                                                        <asp:DropDownList ID="cmbFPeriod" runat="server" Width="81px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                                                                                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 10%; height: 24px; text-align: right; vertical-align: middle;">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MakePoolVoucher_ComeAndGoCo %>"
                                                            Width="86px"></asp:Label></td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; height: 24px;" width="50%">
                                                        <asp:TextBox ID="txtCustomer" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                </tr>
                                                                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 10%; height: 24px; text-align: right; vertical-align: middle;">
                                                        </td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; height: 24px;" width="50%">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                            Text="<%$ Resources:PublicRes,Pub_Select %>" />  

                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                            Text="<%$ Resources:BaseInfo,Role_lblSubAuthExport %>" 
                                            onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                            onmouseup="BtnUp(this.id);"/>  

                                                        </td>
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                </table>
                            </div>
    </form>
</body>
</html>
