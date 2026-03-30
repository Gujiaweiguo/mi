<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankPayIn.aspx.cs" Inherits="Invoice_BankCard_BankPayIn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
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
        addTabTool("<%=baseInfo %>,Invoice/BankCard/BankPayIn.aspx");
        loadTitle();
    }
    function CheckData()
        {
            if(isEmpty(document.all.txtBingeDate.value))
            {
                parent.document.all.txtWroMessage.value='收款日期不能为空。';
                
                return false;
            }  
            if(isEmpty(document.all.txtEndDate.value))
            {
                parent.document.all.txtWroMessage.value='收款日期不能为空。';
                
                return false;
            } 
        }
    </script>
</head>
<body onload="Load()" topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                            <div id="showLeaseBargain">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 401px; width: 100%;">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 550px; height: 22px; text-align: left; vertical-align: top;">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_GenerateReceivables %>" Width="421px"></asp:Label></td>
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
                                                <td style="text-align: left; vertical-align: top; width: 508px;" colspan="3" rowspan="2">
                                                        <table>
                                                                                                               <tr>
                                                                                                               <td style="width: 26px"></td>
                                                        <td style="width: 3px; height: 30px;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,BankCard_Month %>" Width="85px" CssClass="labelStyle"></asp:Label></td>
                                                        <td style="width: 3px; height: 30px;">
                                                            <asp:DropDownList ID="cmbMonth" runat="server" Width="163px">
                                                            </asp:DropDownList></td>
                                                        <td style="width: 224px; height: 30px;">
                                                        </td>
                                                        <td style="width: 3px; height: 30px;">
                                                        </td>
                                                            <td style="width: 3px; height: 30px;">
                                                                </td>
                                                        </tr>
                                                                                                               <tr>
                                                                                                                   <td colspan="6" style="height: 1px">
                                                                                                                   </td>
                                                        </tr>
                                                        <tr>
                                                        <td style="width: 26px; height: 32px"></td>
                                                        <td style="width: 3px; height: 32px;">
                                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,BankCard_MonthBigen %>" Width="85px" CssClass="labelStyle"></asp:Label></td>
                                                        <td style="width: 3px; height: 32px;">
                                                            <asp:TextBox ID="txtBingeDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                        <td style="width: 224px; height: 32px;">
                                                        </td>
                                                        <td style="width: 3px; height: 32px;">
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,BankCard_MonthEnd %>" Width="87px" CssClass="labelStyle"></asp:Label></td>
                                                            <td style="width: 3px; height: 32px;">
                                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                        </tr>
                                                            <tr>
                                                                <td style="width: 26px; height: 32px">
                                                                </td>
                                                                <td style="width: 3px; height: 32px">
                                                                </td>
                                                                <td style="width: 3px; height: 32px">
                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                            Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" /></td>
                                                                <td style="width: 224px; height: 32px">
                                                                </td>
                                                                <td style="width: 3px; height: 32px">
                                                                </td>
                                                                <td style="width: 3px; height: 32px">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <!--  *********right
                    -->
                                                </tr>
                                            </table>
                                            </td>
                                    </tr>
                                </table>
                            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
