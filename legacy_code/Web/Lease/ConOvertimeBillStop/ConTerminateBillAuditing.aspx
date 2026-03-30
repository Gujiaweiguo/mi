<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConTerminateBillAuditing.aspx.cs" Inherits="Lease_ConOvertimeBillStop_ConTerminateBillAuditing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
        <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="showmain" border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle"
            style="height: 450px">
            <tr height="15">
                <td colspan="8" style="height: 5px">
                </td>
            </tr>
            <tr>
                <td style="width: 75px; height: 401px; text-align: center" valign="top">
                    <img height="401" src="../../images/shuxian.jpg" />
                </td>
                <td colspan="5" style="vertical-align: top; width: 571px; height: 401px" valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="showLeaseBargain">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 715px; height: 420px">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 167px; height: 22px; text-align: left">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,ConTerminateBill_ConTerminateBill %>"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 635px; height: 15px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" width="50%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" width="100%">
                                                            <tr style="height: 5px">
                                                                <td style="width: 118px">
                                                                </td>
                                                                <td style="width: 164px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>" Width="50px"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: right;">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td>
                                                                </td>
                                                                <td style="width: 164px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <td class="baseLable" style="height: 28px; text-align: right;">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"></asp:Label>
                                                                    &nbsp;</td>
                                                                <td style="height: 28px">
                                                                    <asp:DropDownList ID="cmbTradeID" runat="server" Enabled="False" Width="165px">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 28px; text-align: right;">
                                                                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="cmbContractStatus" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px; text-align: right;">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtContractCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px; text-align: right;">
                                                                    <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtRefID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtConStartDate" runat="server" CssClass="Enabledipt160px" onclick="calendarExt(GetNorentDays)"
                                                                        ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtConEndDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>"
                                                                        Width="99px"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="DDownListPenalty" runat="server" Enabled="False" Width="165px">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right; height: 22px;">
                                                                    <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 22px">
                                                                    <asp:DropDownList ID="DDownListTerm" runat="server" Enabled="False" Width="165px">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top" width="50%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" width="100%">
                                                            <tr style="height: 5px">
                                                                <td style="width: 136px">
                                                                </td>
                                                                <td class="baseInput">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="164">
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
                                                                <td class="baseLable" style="height: 30px; text-align: right">
                                                                    <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblEndDate %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtNewConStartDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblNorentDays %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td class="baseInput" style="height: 30px">
                                                                    <asp:TextBox ID="txtNewConEndDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label32" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labEConURL %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td class="baseInput" style="height: 30px">
                                                                    <asp:TextBox ID="txtBargain" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 8px">
                                                                <td>
                                                                </td>
                                                                <td style="width: 164px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label54" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labAdditionalItem %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="listBoxAddItem" runat="server" CssClass="ipt160px" Height="72px"
                                                                        TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td>
                                                                </td>
                                                                <td style="width: 164px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <td class="baseLable" style="text-align: right">
                                                                    <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td rowspan="2" valign="top">
                                                                    <asp:TextBox ID="listBoxRemark" runat="server" CssClass="ipt160px" Height="58px"
                                                                        TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 28px">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 15px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td>
                                                                </td>
                                                                <td rowspan="1" valign="top">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 28px">
                                                                </td>
                                                                <td rowspan="1" style="height: 28px" valign="top">
                                                                    <asp:Button ID="butConsent" runat="server" CssClass="buttonSave" OnClick="butConsent_Click"
                                                                        Text="<%$ Resources:BaseInfo,CustPalaver_butConsent %>" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="Expressionshow">
                               </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 93px; height: 401px; text-align: center" valign="top">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
