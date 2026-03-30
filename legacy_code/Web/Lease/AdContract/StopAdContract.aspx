<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StopAdContract.aspx.cs" Inherits="Lease_AdContract_StopAdContract" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
            <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <table border="0" cellpadding="0" cellspacing="0" style="border-right: #b4bec8 thin solid;
                        border-top: #b4bec8 thin solid; border-left: #b4bec8 thin solid; width: 755px;
                        border-bottom: #b4bec8 thick solid; height: 401px">
                        <tr height="5">
                            <td colspan="8" style="height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 59px; height: 401px; text-align: center">
                                <img height="401" src="../../images/shuxian.jpg" />
                            </td>
                            <td colspan="5" style="font-size: 13pt; width: 548px; height: 401px">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div id="PotCustomer">
                                            <table id="TABLE1" border="0" cellpadding="0" cellspacing="0" style="width: 587px;
                                                height: 460px">
                                                <tr>
                                                    <td class="tdTopBackColor" style="width: 7px; height: 25px" valign="top">
                                                        <img alt="" class="imageLeftBack" /></td>
                                                    <td class="tdTopRightBackColor" colspan="2" style="width: 538px; height: 25px; text-align: right"
                                                        valign="top">
                                                        <img alt="" class="imageRightBack" /></td>
                                                </tr>
                                                <%--                         <tr>
                             <td class="tdTopBackColor"  style="width: 7px; height: 25px; text-align:left;" valign="top" >
                                 <img alt="" class="imageLeftBack" /></td>
                             <td class="tdTopBackColor" colspan="2" style="width: 518px; height: 25px; text-align: left;
                                 vertical-align: bottom;">
                                 <table border="0" cellpadding="0" cellspacing="0">
                                     <tr style="width: 24px">
                                         <td style="width: 155px; height: 18px">
                                             fdsdsfsfsfsdfsdfsfsfdsdfs</td>
                                     </tr>
                                     <tr style="width: 1px">
                                         <td style="width: 155px">
                                         </td>
                                     </tr>
                                 </table>
                             </td>
                             <td class="tdTopRightBackColor"  style="width: 7px; height: 25px; text-align: right;" valign="top"><img alt="" class="imageRightBack" /></td>
                         </tr>--%>
                                                <tr>
                                                    <td class="tdBackColor" colspan="2" style="width: 538px; height: 330px; text-align: left"
                                                        valign="top">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="height: 330px" width="255">
                                                            <tr>
                                                                <td colspan="4" style="height: 1px; background-color: white">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="height: 10px">
                                                                </td>
                                                            </tr>
                                                            <tr style="width: 280px">
                                                                <td colspan="4" style="text-align: right">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 260px">
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
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblCustCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"
                                                                        Width="92px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                    &nbsp;</td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"
                                                                        Width="93px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    &nbsp;<asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"
                                                                        Width="97px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                                                </td>
                                                            </tr>
                                                            <tr style="width: 280px">
                                                                <td colspan="4" style="text-align: right">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 260px">
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
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblAdType" runat="server" CssClass="labelStyle" Height="16px" Text="<%$ Resources:BaseInfo,AdContract_lblAdType %>"
                                                                        Width="91px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:RadioButton ID="rbtnFastness" runat="server" GroupName="AdContract" Text="<%$ Resources:BaseInfo,AdContract_rbtnFastness %>"
                                                                        Width="57px" Checked="True" />
                                                                    <asp:RadioButton ID="rbtnCommonly" runat="server" GroupName="AdContract" Text="<%$ Resources:BaseInfo,AdContract_rbtnCommonly %>"
                                                                        Width="61px" /></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblFastnessAd" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblFastnessAd %>"
                                                                        Width="95px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:DropDownList ID="cmbFastnessAd" runat="server" Width="141px">
                                                                    </asp:DropDownList></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    &nbsp;<asp:Label ID="lblAdDesc" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblAdDesc %>"
                                                                        Width="97px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtAdDesc" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                                                </td>
                                                            </tr>
                                                            <tr style="width: 280px">
                                                                <td colspan="4" style="text-align: right">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 260px">
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
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px">
                                                                    <asp:Label ID="lblAdContractCode" runat="server" CssClass="labelStyle" Height="18px"
                                                                        Text="<%$ Resources:BaseInfo,AdContract_lblAdContractCode %>" Width="99px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtAdContractCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblStartDate %>"
                                                                        Width="95px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblEndDate %>"
                                                                        Width="96px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 20px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblTotalMoney" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblTotalMoney %>"
                                                                        Width="90px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 6px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtTotalMoney" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 30px; height: 22px">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="tdBackColor" style="width: 45%; height: 330px" valign="top">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="height: 100px" width="280">
                                                            <tr>
                                                                <td colspan="4" style="width: 280px; height: 1px; background-color: white">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="width: 280px; height: 10px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" style="text-align: left">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 260px">
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
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblPrepayment" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblPrepayment %>"
                                                                        Width="85px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtPrepayment" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 30px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblPayingCycle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblPayingCycle %>"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:DropDownList ID="cmbPayingCycle" runat="server" Width="153px">
                                                                    </asp:DropDownList></td>
                                                                <td class="tdBackColor" style="width: 30px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblPaymentPerCycle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblPaymentPerCycle %>"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtPaymentPerCycle" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 30px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblLastPaymentDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblLastPaymentDate %>"
                                                                        Width="85px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtLastPaymentDate" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 30px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblAdMaker" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblAdMaker %>"
                                                                        Width="86px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="TextBox2" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 30px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 1px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 5px; height: 1px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 30px; height: 1px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" style="text-align: left">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 260px">
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
                                                            <tr>
                                                                <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="lblNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"
                                                                        Width="49px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" Height="45px" TextMode="MultiLine"
                                                                        Width="146px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 30px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 5px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 30px; height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" style="text-align: left">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 260px">
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
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 5px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 30px; height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"
                                                                        Width="49px"></asp:Label></td>
                                                                <td class="tdBackColor" style="width: 5px; height: 22px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 22px">
                                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="ipt160px" Height="43px" TextMode="MultiLine"
                                                                        Width="146px"></asp:TextBox></td>
                                                                <td class="tdBackColor" style="width: 30px; height: 22px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdBackColor" style="width: 85px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 5px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 160px; height: 5px">
                                                                </td>
                                                                <td class="tdBackColor" style="width: 30px; height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" style="text-align: left">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 260px">
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
                                                            <tr>
                                                                <td colspan="4" style="height: 44px">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="PotCustLicense">
                                            <%--                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
               <ContentTemplate>

                <asp:Panel ID="Panel1" runat="server" CssClass="popupControl">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                   <center>
                        <asp:Calendar ID="Calendar1" runat="server" Width="160px" DayNameFormat="Shortest"
                            BackColor="White" BorderColor="#999999" CellPadding="1" Font-Names="Verdana"
                            Font-Size="8pt" ForeColor="Black" OnSelectionChanged="Calendar1_SelectionChanged">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                        </asp:Calendar>
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
                <asp:Panel ID="Panel2" runat="server" CssClass="popupControl">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                   <center>
                        <asp:Calendar ID="Calendar2" runat="server" Width="160px" DayNameFormat="Shortest"
                            BackColor="White" BorderColor="#999999" CellPadding="1" Font-Names="Verdana"
                            Font-Size="8pt" ForeColor="Black" OnSelectionChanged="Calendar2_SelectionChanged">
                                <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <SelectorStyle BackColor="#CCCCCC" />
                                <WeekendDayStyle BackColor="#FFFFCC" />
                                <OtherMonthDayStyle ForeColor="#808080" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="#999999" Font-Size="7pt" BorderColor="Black" Font-Bold="True" />
                        </asp:Calendar>
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
       <cc1:PopupControlExtender ID="PopupControlExtender2" runat="server"
            TargetControlID="txtLicenseEndDate"
            PopupControlID="Panel2"
            Position="Bottom" />
        <cc1:PopupControlExtender ID="PopupControlExtender1" runat="server"
            TargetControlID="txtLicenseBeginDate"
            PopupControlID="Panel1"
            Position="Bottom" />
        
        
             </ContentTemplate>
        </asp:UpdatePanel>--%>
                                            &nbsp;</div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 60px; height: 401px" valign="top">
                                <img height="401" src="../../images/shuxian.jpg" /></td>
                        </tr>
                    </table>
                    &nbsp;
                </div>
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
